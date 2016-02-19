function StudentDetails(Id) {
    $("#studentsDiv").addClass("hidden");
    var div = $("#studentDetailsDiv");
    div.removeClass("hidden");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: "/Students/StudentDetails",
        data: { Id: Id },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                $("#studentsDiv").addClass("hidden");
                div.html(res);
                LoadStudentGrades(Id);
                LoadTeachers();
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function LoadStudentGrades(Id) {
    var div = $("div#studentGrades");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        data: { Id: Id },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                div.html(res);
                ApplySelectableDataTable("studentGradesList");
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function ApplySelectableDataTable(tableName)
{
    // Setup - add a text input to each footer cell
    $('#' + tableName + ' tfoot th').each(function (i) {
        if (i > 0) {
            var title = $(this).text();
            $(this).html('<input type="search" class="form-control input-sm" placeholder="' + title + '" />');
        }
    });

    var selected = [];
    $('#' + tableName + ' tbody').on('click', 'tr', function () {
        var id = this.id;
        var index = $.inArray(id, selected);

        if (index === -1) {
            selected.push(id);
        } else {
            selected.splice(index, 1);
        }

        $(this).toggleClass('selected');
    });

    $('#' + tableName).DataTable({
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "Все"]],
        pageLength: 20,
        rowCallback: function( row, data ) {
            if ( $.inArray(data.TeacherId, selected) !== -1 ) {
                $(row).addClass('selected');
            }
        },
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
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });
}

function studentDetailFormSubmit() {
    addValidator("studentDetailForm");
    if ($("#studentDetailForm").valid()) {
        OnBeginUpdateStudent();
        var grades = [];
        $('#studentGradesList tr').each(function () {
            var td = $('td', this);
            grades.push({
                StudentId: $("#StudentId").val(),
                TeacherId: $('input[name="TeacherId"]', td).val(),
                TeacherFullName: $('input[name="TeacherFullName"]', td).val(),
                Subject: $('input[name="Subject"]', td).val(),
                Grade: $('input[name="Grade"]', td).val()
            });
        });
        grades.shift();
        var student = [];
        student.push({
            StudentId: $("#StudentId").val(),
            FirstName: $("#FirstName").val(),
            LastName: $("#LastName").val(),
            Login: $("#Login").val(),
        });

        $.ajax({
            url: "/Students/UpdateStudentDetails",
            type: "POST",
            data:
                {
                    StudentId: $("#StudentId").val(),
                    FirstName: $("#FirstName").val(),
                    LastName: $("#LastName").val(),
                    Login: $("#Login").val()
                }
            ,
            success: function (result) {
                AjaxCommonSuccessHandling(result, function () {
                    SetNeedRefresh();
                });
            },
        });

        $.ajax({
            url: "/Students/UpdateStudentGrades",
            type: "POST",
            data:
                JSON.stringify(grades)
            ,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                AjaxCommonSuccessHandling(result, function () {
                    SetNeedRefresh();
                });
            },
            complete: function (req, status) {
                LoadingState(false);
            }
        });
    }
}

function OnBeginUpdateStudent() {
    LoadingState(true);
    LoadingStateMessage($("#updateStudent"));
}
function OnSuccessUpdateStudent(result) {
    AjaxCommonSuccessHandling(result, function () {
        SetNeedRefresh();
    });
}
function OnCompleteUpdateStudent(result) {
    LoadingState(false);
}
