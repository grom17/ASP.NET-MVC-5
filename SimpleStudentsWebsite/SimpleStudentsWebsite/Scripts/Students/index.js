$(function () {
    setTimeout(ReadStudents, 0);
});

var NeedRefresh = false;
function SetNeedRefresh()
{
    NeedRefresh = true;
}

function ReadStudents() {
    var div = $("#studentsDiv");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: $("#studentsDiv").data("action-url"),
        success: function (result) {
            AjaxCommonSuccessHandling(result, function () {
                div.html(result);
                $("#students").slimtable({
                    colSettings:
                        [{
			                colNumber: 2, enableSort: false
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

function LoadStudentGrades(Id)
{
    var div = $("#studentGrades");
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

function studentDetailFormSubmit()
{
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
            success: function (result) { OnSuccessUpdateStudent(result) },
            error: AjaxCommonErrorHandling,
            complete: function (req, status) {
                OnCompleteUpdateStudent();
            }
        });

        $.ajax({
            url: "/Students/UpdateStudentGrades",
            type: "POST",
            data: 
                JSON.stringify(grades)
            ,
            contentType: 'application/json; charset=utf-8',
            success: function (result) { OnSuccessUpdateStudent(result) },
            error: AjaxCommonErrorHandling,
            complete: function (req, status) {
                OnCompleteUpdateStudent();
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

function LoadTeachers()
{
    var div = $("#addTeacherSelect");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                div.html(res);
                $("#teachersList").fSelect({
                    placeholder: "Выберите преподавателей",
                    searchText: "Искать",
                    overflowText: '{n} выбрано',
                });
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function AddTeacher()
{
    var currentIds = [];
    $('#studentGradesList tr').each(function () {
        var td = $('td', this);
        currentIds.push($('input[name="TeacherId"]', td).val());
    });
    currentIds.shift();
    var teacherIds = $("#teachersList").val();
    var difference = [];

    jQuery.grep(teacherIds, function (el) {
        if (jQuery.inArray(el, currentIds) == -1) difference.push(el);
    });

    $(difference).each(function () {
        var Id = this;
        $.ajax({
            url: "/Students/GetTeacherById",
            data: { Id: Id },
            success: function (res) {
                AjaxCommonSuccessHandling(res, function () {
                    var fullname = res.Fullname;
                    var subject = res.Subject;
                    $('#studentGradesList tr:last').after(
                        '<tr><td><input type="hidden" value="'+ Id +'" name="TeacherId">' + Id + '</td><td>' +
                        fullname + '</td><td>' + subject +
                        '</td><td><input type="text" value="" name="Grade"></td></tr>');
                });
            },
            error: AjaxCommonErrorHandling
        });
    });
}

function BackToList()
{
    $("#studentsDiv").removeClass("hidden");
    $("#studentDetailsDiv").addClass("hidden");
    $("#StudentGrades").addClass("hidden");
    if (NeedRefresh) {
        ReadStudents();
        NeedRefresh = false;
    }
}