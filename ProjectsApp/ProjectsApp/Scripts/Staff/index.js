$(function () {
    setTimeout(ReadStaff, 0);
});

var NeedRefresh = false;
function SetNeedRefresh() {
    NeedRefresh = true;
}

function ReadStaff() {
    var div = $("#staffDiv");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: $("#staffDiv").data("action-url"),
        success: function (result) {
            AjaxCommonSuccessHandling(result, function () {
                ApplyStaffListDataTable("staff", $("#trHTML").val(), $.parseJSON(result));
                div.removeClass('hidden');
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}

function BackToList() {
    $("#staffDiv").removeClass("hidden");
    $("#employeeDetailsDiv").addClass("hidden");
    $("#newEmployeeDiv").addClass("hidden");
    if (NeedRefresh) {
        ReadStaff();
        NeedRefresh = false;
    }
}

function ApplyStaffListDataTable(tableName, trHTML, data) {
    // Need to destroy and recreate table if it's already exists
    if ($.fn.DataTable.isDataTable('#' + tableName)) {
        $('#' + tableName).dataTable().fnDestroy();
        $('#' + tableName).empty();
        var tableHTML = '<thead>' + trHTML + '</thead><tfoot>' + trHTML + '</tfoot>';
        $('#' + tableName).html(tableHTML);
    }

    // Setup - add a text input to each footer cell
    $('#' + tableName + ' tfoot th').each(function (i) {
        if (i > 0) {
            var title = $(this).text();
            $(this).html('<input type="search" class="form-control input-sm" placeholder="' + title + '" />');
        }
    });

    // data table initialization
    $('#' + tableName).DataTable({
        retrieve: true,
        // data from server - list of staff
        data: data,
        columns: [
            {
                // hidden column for Person Id
                "data": "PersonId", render: function (data, type, full) {
                    return '{0}<input type="hidden" name=PersonId value="{0}" />'.format(data);
                }
            },
            { "data": "FirstName" },
            { "data": "Patronymic" },
            { "data": "LastName" },
            { "data": "Email" }
        ],
        columnDefs: [
            {
                // column with Person Id
                visible: false,
                targets: 0
            }
        ],
        // default order by staff LastName
        order: [[3, 'asc']],
        // menu to select how much records will be shown on each page
        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "Все"]],
        // count of records to show on each page
        pageLength: 20,
        // some info on RU
        // TODO: Get all messages from  messages.xml file
        language: {
            "info": "Показано с _START_ по _END_ из _TOTAL_ записей",
            "paginate": {
                "previous": "<",
                "next": ">"
            },
            "lengthMenu": "Показывать _MENU_ записей",
            "search": "Искать:",
            "infoFiltered": " (всего _MAX_ записи)"
        },
        // move search for each field to the top of the table
        initComplete: function (settings, json) {
            var r = $('#' + tableName + ' tfoot tr');
            r.find('th').each(function (i) {
                if (i > 0) {
                    $(this).css('padding', 8);
                }
            });
            $('#' + tableName + ' thead').append(r);
            $('#search_0').css('text-align', 'center');
        }
    });

    // DataTable
    var table = $('#' + tableName).DataTable();

    // Apply the search
    table.columns().every(function () {
        var that = this;

        $('input', this.footer()).on('keyup change', function () {
            console.log(that, this.value);
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });

    // Open staff details on row click
    $('#' + tableName + ' tbody').on('click', 'td', function () {
        var row = $(this).closest('tr');
        var personId = table.cell(row, 0).data();
        EmployeeDetails(personId);
    });
}