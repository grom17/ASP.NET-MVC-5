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
                $("#studentGradesList").slimtable({
                    colSettings:
                        [{
                            colNumber: 3, enableSort: false
                        }]
                });
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
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
