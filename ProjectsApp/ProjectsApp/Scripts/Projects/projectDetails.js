function ProjectDetails(ProjectId) {
    $("#projectsDiv").addClass("hidden");
    $("#newProjectDiv").addClass("hidden");
    var div = $("#projectDetailsDiv");
    div.removeClass("hidden");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: div.data('detail-url'),
        data: { ProjectId: ProjectId },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                div.html(res);
                $("#StartDate").parent().datetimepicker({
                    format: 'DD/MM/YYYY',
                    startView: "months",
                    minViewMode: "days",
                    pickTime: false
                });
                $("#EndDate").parent().datetimepicker({
                    format: 'DD/MM/YYYY',
                    startView: "months",
                    minViewMode: "days",
                    pickTime: false
                });
                LoadProjectExecutors(ProjectId);                
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}

function LoadProjectExecutors(ProjectId) {
    var div = $("div#projectExecutors");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        data: { ProjectId: ProjectId },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                ApplyProjectExecutorsDataTable("projectExecutorsList", $("#executorsTrHTML").val(), $.parseJSON(res));
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function AddProjectExecutor(ProjectId) {
    var PersonId = $("#NewExecutorId").val();
    var div = $("div#projectExecutors");
    var btn = $("#addProjectExecutor");
    LoadingState(true, div);
    LoadingStateMessage(btn, div);
    $.ajax({
        url: btn.data("action-url"),
        data: {
            ProjectId: ProjectId,
            PersonId: PersonId
        },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                if (res.NeedUpdate)
                    LoadProjectExecutors(ProjectId);
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function DeleteProjectExecutor(ProjectId) {
    var PersonId = $("#selectedExecutorId").val();
    var div = $("div#projectExecutors");
    var btn = $("#removeProjectExecutor");
    LoadingState(true, div);
    LoadingStateMessage(btn, div);
    $.ajax({
        url: btn.data("action-url"),
        data: {
            ProjectId: ProjectId,
            PersonId: PersonId
        },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                LoadProjectExecutors(ProjectId);
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function ApplyProjectExecutorsDataTable(tableName, trHTML, data) {
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
        lengthMenu: [[5, 10, 20, -1], [5, 10, 20, "Все"]],
        // count of records to show on each page
        pageLength: 5,
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

    // Show delete button on row click
    $('#' + tableName + ' tbody').on('click', 'td', function () {
        var row = $(this).closest('tr');
        var personId = table.cell(row, 0).data();
        var removeBtn = $("#removeProjectExecutor");
        if (row.hasClass('selected')) {
            row.removeClass('selected');
            removeBtn.addClass('hidden');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            row.addClass('selected');
            removeBtn.removeClass('hidden');
            $("#selectedExecutorId").val(personId);
            console.log($("#selectedExecutorId").val());
        }
    });
}

function projectDetailFormSubmit() {
    addValidator("projectDetailForm");
    if ($("#projectDetailForm").valid()) {
        $("#projectDetailForm").trigger('submit');
    }
}

function OnBeginUpdateProject() {
    LoadingState(true);
    LoadingStateMessage($("#updateEmployee"), $("#projectDetailsDiv"));
}
function OnSuccessUpdateProject(result) {
    AjaxCommonSuccessHandling(result, function () {
        SetNeedRefresh();
    });
}
function OnCompleteUpdateProject() {
    LoadingState(false);
    BackToList();
}
