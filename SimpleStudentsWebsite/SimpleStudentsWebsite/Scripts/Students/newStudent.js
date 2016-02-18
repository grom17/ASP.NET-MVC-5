function PrepareCreateStudent() {
    $("#studentsDiv").addClass("hidden");
    var div = $("#newStudentDiv");
    div.removeClass("hidden");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                $("#studentsDiv").addClass("hidden");
                div.html(res);
                LoadStudentGrades(0);
                LoadTeachers();
                addValidator("newStudentForm");
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false,div);
        }
    });
}

function newStudentFormSubmit() {
    var gradesIsValid = newStudentGradesValidation();
    if (gradesIsValid && $("#newStudentForm").valid()) {
        OnBeginCreateStudent();
        $.ajax({
            url: "/Students/CreateStudent",
            type: "POST",
            data:
                {
                    FirstName: $("#FirstName").val(),
                    LastName: $("#LastName").val(),
                    Login: $("#Login").val(),
                    Password: $("#SecretKey").val()
                }
            ,
            success: function (result) { OnSuccessCreateStudent(result) },
            error: AjaxCommonErrorHandling
        });   
    }
}

function newStudentGradesValidation()
{
    var table = $("#newStudentForm").find("#studentGradesList tr");
    var rowCount = table.length;
    if (rowCount <= 1) {
        ShowError("Добавьте как минимум одного преподавателя");
        return;
    }
    var valid = true;
    var grades = [];
    table.each(function () {
        var td = $('td', this);
        grades.push({
            Grade: $('input[name="Grade"]', td).val()
        });
    });
    grades.shift();
    grades.forEach(function (item) {
        if (item == typeof(undefined) || item.Grade == "" || item.Grade == 0) {
            ShowError("Добавьте оценки");
            valid = false;
            return;
        }
    });
    return valid;
}

function addStudentGrades(Id, table)
{
    var grades = [];
    table.each(function () {
        var td = $('td', this);
        grades.push({
            StudentId: Id,
            TeacherId: $('input[name="TeacherId"]', td).val(),
            TeacherFullName: $('input[name="TeacherFullName"]', td).val(),
            Subject: $('input[name="Subject"]', td).val(),
            Grade: $('input[name="Grade"]', td).val()
        });
    });
    grades.shift();
    if (grades.length > 0) {
        $.ajax({
            url: "/Students/UpdateStudentGrades",
            type: "POST",
            data: JSON.stringify(grades),
            contentType: 'application/json; charset=utf-8',
            success: function (result) { OnSuccessUpdateStudent(result) },
            error: AjaxCommonErrorHandling,
            complete: function (req, status) {
                OnCompleteCreateStudent();
            }
        });
    }
}

function OnBeginCreateStudent() {
    LoadingState(true, $("#newStudentForm"));
    LoadingStateMessage($("#createStudentBtn"), $("#newStudentForm"));
}
function OnSuccessCreateStudent(result) {
    AjaxCommonSuccessHandling(result, function () {
        addStudentGrades(result.Id, $("#newStudentForm").find("#studentGradesList tr"));
    });
}
function OnCompleteCreateStudent(result) {
    LoadingState(false, $("#newStudentForm"));
    updateDBInfo();
    SetNeedRefresh();
    BackToList();
}