$(function() {
    setTimeout(ReadStudents, 0);
});

var NeedRefresh = false;
function SetNeedRefresh() {
    NeedRefresh = true;
}

function ReadStudents() {
    var div = $("#studentsDiv");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: $("#studentsDiv").data("action-url"),
        success: function(result) {
            AjaxCommonSuccessHandling(result, function() {
                ApplyStudentsListDataTable("students", $.parseJSON(result));
                div.removeClass('hidden');
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function(req, status) {
            LoadingState(false);
        }
    });
}

function BackToList() {
    $("#studentsDiv").removeClass("hidden");
    $("#studentDetailsDiv").addClass("hidden");
    $("#StudentGrades").addClass("hidden");
    $("#newStudentDiv").addClass("hidden");
    if (NeedRefresh) {
        ReadStudents();
        NeedRefresh = false;
    }
}

function LoadStudentGrades(Id) {
    var div = Id === 0 ? $("div#newStudentGrades") : $("div#studentGrades");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        data: { Id: Id },
        success: function(res) {
            AjaxCommonSuccessHandling(res, function() {
                ApplySelectableDataTable(Id === 0 ? "newStudentGradesList" : "studentGradesList", $.parseJSON(res));
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function(req, status) {
            LoadingState(false, div);
        }
    });
}

function ApplySelectableDataTable(tableName, data) {
    if ($.fn.DataTable.isDataTable('#' + tableName)) {
        $('#' + tableName).dataTable().fnDestroy();
    }
    // extension to allow order grades
    $.fn.dataTable.ext.order['dom-text-numeric'] = function(settings, col) {
        return this.api().column(col, { order: 'index' }).nodes().map(function(td, i) {
            return $('input', td).val() * 1;
        });
    };

    // add a text input to each footer cell
    $('#' + tableName + ' tfoot th').each(function (i) {
        if (i > 0) {
            var title = $(this).text();
            $(this).html('<input type="search" class="form-control input-sm" placeholder="' + title + '" />');
        }
    });

    // data table initialization
    $('#' + tableName).DataTable({
        // data from server - list of teachers and grades for student
        data: data,
        columns: [
            {
                // first column as checkbox
                // if student belongs to the teacher it will be checked
                "data": "IsTeacher", render: function (data, type, full) {
                    return '<input type="hidden" name=IsTeacher value="{0}" />'.format(data);
                }
            },
            {
                // hidden column with Teacher Id value
                "data": "TeacherId", render: function (data, type, full) {
                    return '<input type="hidden" name=TeacherId value="{0}" />'.format(data);
                }
            },
            {
                // First and Last Name of the teacher
                "data": "TeacherFullName", render: function (data, type, full) {
                    return '{0}<input type="hidden" name=TeacherFullName value="{0}" />'.format(data);
                }
            },
            {
                // teacher's subject
                "data": "Subject", render: function (data, type, full) {
                    return '{0}<input type="hidden" name=Subject value="{0}" />'.format(data);
                }
            },
            {
                // student's grade as textbox input, can be changed by click
                "data": "Grade", render: function (data, type, full) {
                    var value = data !== null ? data : "";
                    return '<input type="hidden" name=Grade value="{0}" />'.format(value) + 
                           '<input type="text" id=Grade disabled="disabled" class="form-control input-sm" name=Grade value="{0}" />'.format(value);
                },
                "orderDataType": "dom-text-numeric"
            }
        ],
        columnDefs: [
            {
                // column with checkboxes
                orderable: false,
                targets: 0
            },
            {
                // column with Teacher Id
                visible: false,
                targets: 1
            }
        ],
        // default order by grades
        order: [[4, 'desc']],
        // menu to select how much records will be shown on each page
        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "Все"]],
        // count of records to show on each page
        pageLength: 20,
        // select rows where student belongs to the teacher
        createdRow: function(row, data, index) {
            $('td', row).eq(0).addClass('select-checkbox');
            if (data.IsTeacher) {
                $(row).addClass('selected');
            }
        },
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

    $('#' + tableName + ' tbody').on('click', 'td', function () {
        var that = this;
        var row = $(this).closest('tr');
        // Open pop-up to type grade and save it
        if ($(that).attr("class") === "sorting_1") {
            var currentValue = table.cell(that).data() !== null ? table.cell(that).data() : "";
            ShowConfirm("Оценка:", function (grade) {
                table.cell(that).data(grade);
            }, function () { }, currentValue);
        }
            // Select/unselect row and update IsTeacher value
        else {
            $(this).closest('tr').toggleClass('selected');
            table.cell(row, 0).data(!table.cell(row, 0).data());
        }
    });
}

function UpdateStudentGrades(tableName, studentId) {
    OnBeginUpdateStudent();

    var table = $('#' + tableName).DataTable();
    var data = table.data();
    var rowsCount = table.rows().count();
    // array 'grades' contains of Student Id, 
    // Id and flag is current student belongs to that teacher (IsTeacher)
    // Subject and Grade
    var grades = [];
    for (var i = 0; i < rowsCount; i++) {
        grades.push({
            StudentId: studentId, // Student Id will be the same for all records
            TeacherId: data[i].TeacherId,
            IsTeacher: data[i].IsTeacher,
            TeacherFullName: data[i].TeacherFullName,
            Subject: data[i].Subject,
            Grade: data[i].Grade
        });
    }

    // Updating student grades
    $.ajax({
        url: "/Students/UpdateStudentGrades",
        type: "POST",
        data:
            JSON.stringify(grades)
        ,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            AjaxCommonSuccessHandling(result, function () {
            });
        },
        complete: function (req, status) {
            OnCompleteUpdateStudent();
        }
    });
}