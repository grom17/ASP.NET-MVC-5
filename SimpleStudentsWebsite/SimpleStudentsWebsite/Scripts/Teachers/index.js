$(function () {
    setTimeout(ReadTeachers, 0);
});

function ReadTeachers() {
    var div = $("#teachersDiv");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: $("#teachersDiv").data("action-url"),
        success: function (result) {
            ApplyTeachersListDataTable("teachers", $.parseJSON(result));
            div.removeClass('hidden');
        },
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}

function ApplyTeachersListDataTable(tableName, data) {
    if ($.fn.DataTable.isDataTable('#' + tableName)) {
        $('#' + tableName).dataTable().fnDestroy();
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
                    return '{0}<input type="hidden" name=TeacherId value="{0}" />'.format(data);
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

var NeedRefreshTeachers = false;
function SetNeedRefreshTeachers() {
    NeedRefreshTeachers = true;
}

function BackToTeachersList() {
    $("#teachersDiv").removeClass("hidden");
    $("#teacherDetailsDiv").addClass("hidden");
    $("#TeacherStudents").addClass("hidden");
    $("#newTeacherDiv").addClass("hidden");
    //$('#teacherStudentsList').dataTable().fnDestroy();
    if (NeedRefreshTeachers) {
        //$('#teachers').dataTable().fnDestroy();
        ReadTeachers();
        NeedRefreshTeachers = false;
    }
}

function LoadTeacherStudents(Id) {
    var div = Id == 0 ? $("div#newTeacherStudents") : $("div#teacherStudents");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        data: { Id: Id },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                ApplyTeacherDataTable(Id == 0 ? "newTeacherStudentsList" : "teacherStudentsList", $.parseJSON(res));
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function ApplyTeacherDataTable(tableName, data) {
    if ($.fn.DataTable.isDataTable('#' + tableName)) {
        $('#' + tableName).dataTable().fnDestroy();
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
        // data from server - list of teachers and grades for student
        data: data,
        columns: [
            {
                // first column as checkbox
                // if student belongs to the teacher it will be checked
                "data": "IsStudent", render: function (data, type, full) {
                    if (data == true) {
                        return '<input type="hidden" name=IsStudent value="true" />';
                    }
                    else {
                        return '<input type="hidden" name=IsStudent value="false" />';
                    }
                }
            },
            {
                // hidden column with Student Id value
                "data": "StudentId", render: function (data, type, full) {
                    return '{0}<input type="hidden" name=StudentId value="{0}" />'.format(data);
                }
            },
            {
                // First and Last Name of the student
                "data": "StudentFullName", render: function (data, type, full) {
                    return '{0}<input type="hidden" name=StudentFullName value="{0}" />'.format(data);
                }
            }
        ],
        columnDefs: [
            {
                // column with checkboxes
                orderable: false,
                targets: 0
            },
            {
                // column with Student Id
                visible: false,
                targets: [1]
            }
        ],
        // default order by student id
        order: [[1, 'asc']],
        // menu to select how much records will be shown on each page
        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "Все"]],
        // count of records to show on each page
        pageLength: 20,
        // select rows where student belongs to the teacher
        createdRow: function (row, data, index) {
            $('td', row).eq(0).addClass('select-checkbox');
            if (data.IsStudent == true) {
                $(row).addClass('selected')
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
        // Select/unselect row and update IsStudent value
        $(this).closest('tr').toggleClass('selected');
        table.cell(row, 0).data((!table.cell(row, 0).data()));
    });
}

function UpdateTeacherStudents(tableName, teacherId) {
    OnBeginUpdateTeacher();

    var table = $('#' + tableName).DataTable();
    var data = table.data();
    var rowsCount = table.rows().count();
    // array 'students' contains of Teacher Id, 
    // Id and flag is current student belongs to that teacher (IsStudent)
    // First and Last Name of the student
    var students = [];
    for (var i = 0; i < rowsCount; i++) {
        students.push({
            TeacherId: teacherId, // Teacher Id will be the same for all records
            StudentId: data[i].StudentId,
            IsStudent: data[i].IsStudent,
            StudentFullName: data[i].StudentFullName
        });
    }

    // Updating teacher students
    $.ajax({
        url: "/Teachers/UpdateTeacherStudents",
        type: "POST",
        data: JSON.stringify(students),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            AjaxCommonSuccessHandling(result, function () {
            });
        },
        complete: function (req, status) {
            OnCompleteUpdateTeacher();
        }
    });
}