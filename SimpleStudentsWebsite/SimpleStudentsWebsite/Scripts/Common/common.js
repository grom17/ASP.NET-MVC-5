$.ajaxSetup({ cache: false }); // prevent cache the ajax response for IE
$(document).ready(function () {
    setInterval('updateClock()', 1000);
    setTimeout('updateDBInfo()', 0);
});

window.onerror = function (msg, url, line, col, error) {
    try {
        LoadingState(false);
        var errorMessage = msg;
        if (url) {
            errorMessage += " at " + url;
            if (line) {
                errorMessage += " line: " + line;
                if (col) {
                    errorMessage += " col: " + col;
                }
            }
        }
        try {
            if (error && error.stack) {
                var errorName = error.name + ": " + error.message;
                errorMessage = error.stack;
                if (errorMessage.indexOf(errorName) == -1) { // firefox return stack without error name and message.
                    errorMessage = errorName + "\n" + errorMessage;
                }
            }
        } catch (e) { }
        LogErrorOnServer("JSError", errorMessage);
    } catch (e) { }
    return false;
};

// common handler ajax errors, just show notification
function AjaxCommonErrorHandling(req, status, error) {
    ShowError(error);
}
// common handler ajax success result, show error if any errors,
// show success message if have in response and run onsuccess delegate function
function AjaxCommonSuccessHandling(result, onsuccess) {
    if (result.errors) {
        ShowError(result.errors);
    } else {
        if (result.success) {
            ShowSuccess(result.success);
            if (onsuccess) {
                onsuccess();
            }
        }
        else if (typeof result === "string" && (result.indexOf("account-login") != -1 ||
                 result.indexOf("unauthorized") != -1)) {
            window.location = "Account/Login";
        }
        else {
            var html = result;
            if (result.html) {
                html = result.html;
            }
            onsuccess(html);
        }
    }
}
function onError(e, status) {
    ShowError(e.errors);
}

// method to call backend action to log error
function LogErrorOnServer(operation, error) {
    $.ajax({
        url: $("body").data('log-error'),
        type: "POST",
        data: { Operation: operation, Error: error },
        success: function (result) {
        }
    })
}

// String.Format. First, checks if it isn't implemented yet.
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}

function addValidator(form) {
    form = "#" + form;
    $(form).removeData("validator");
    $(form).removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(form);
    var validator = $(form).data("validator");
    if (validator) {
        validator.settings.ignore = "";
    }
    DisableOnKeyUpValidation(form);
}

function DisableOnKeyUpValidation(form) {
    var validator = $(form).data("validator");
    if (validator) {
        validator.settings.onkeyup = false; // disable validation on keyup
    }
}

function ApplyTeachersListDataTable(tableName, data) {
    if ($.fn.DataTable.isDataTable('#' + tableName)) {
        $('#' + tableName).dataTable().fnDestroy();
        $('#' + tableName).empty();
        // TODO: Get table structure from cshtml
        var trHTML = '<tr><th>Id</th><th>Преподаватель</th><th>Студенты</th></tr>';
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
        // data from server - list of teachers with students count
        data: data,
        columns: [
            {
                // hidden column with Teacher Id value
                "data": "Id", render: function (data, type, full) {
                    return '{0}<input type="hidden" name=Id value="{0}" />'.format(data);
                }
            },
            // First and Last Name of the teacher
            { "data": "Fullname" },
            { "data": "StudentsCount" }
        ],
        columnDefs: [
            {
                // column with Teacher Id
                visible: false,
                targets: 0
            }
        ],
        // default order by teacher fullname
        order: [[1, 'asc']],
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

    // Open teacher details on row click
    $('#' + tableName + ' tbody').on('click', 'td', function () {
        var row = $(this).closest('tr');
        var teacherId = table.cell(row, 0).data();
        TeacherDetails(teacherId);
    });
}

function ApplyStudentsListDataTable(tableName, data) {
    // TODO: move clear table function to common.js, send trHtml as parameter
    if ($.fn.DataTable.isDataTable('#' + tableName)) {
        $('#' + tableName).dataTable().fnDestroy();
        $('#' + tableName).empty();
        var trHTML = '<tr><th>Id</th><th>Студент</th><th>Средний балл</th></tr>';
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
        // data from server - list of students with average grade
        data: data,
        columns: [
            {
                // hidden column for Student Id
                "data": "Id", render: function (data, type, full) {
                    return '{0}<input type="hidden" name=Id value="{0}" />'.format(data);
                }
            },
            // First and Last Name of the student
            { "data": "Fullname" },
            // Average grade
            { "data": "Grades" }
        ],
        columnDefs: [
            {
                // column with Student Id
                visible: false,
                targets: 0
            }
        ],
        // default order by student fullname
        order: [[1, 'asc']],
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

    // Open student details on row click
    $('#' + tableName + ' tbody').on('click', 'td', function () {
        var row = $(this).closest('tr');
        var studentId = table.cell(row, 0).data();
        StudentDetails(studentId);
    });
}
