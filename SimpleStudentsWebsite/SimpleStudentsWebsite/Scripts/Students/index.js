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

function BackToList()
{
    $("#studentsDiv").removeClass("hidden");
    $("#studentDetailsDiv").addClass("hidden");
    $("#StudentGrades").addClass("hidden");
    $("#newStudentDiv").addClass("hidden");
    if (NeedRefresh) {
        ReadStudents();
        NeedRefresh = false;
    }
}

function LoadTeachers() {
    var div = $("div#addTeacherSelect");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                div.html(res);
                $("select#teachersList").fSelect({
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

function AddTeacher() {
    div = $("#addTeacherSelect");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
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
                        '<tr><td><input type="hidden" value="' + Id + '" name="TeacherId">' + Id + '</td><td>' +
                        fullname + '</td><td>' + subject +
                        '</td><td><input type="text" value="" name="Grade"></td></tr>');
                });
            },
            error: AjaxCommonErrorHandling,
            complete: function (req, status) {
                LoadingState(false, div);
            }
        });
    });
}