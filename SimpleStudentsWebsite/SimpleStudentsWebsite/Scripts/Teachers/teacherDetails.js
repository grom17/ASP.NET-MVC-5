function TeacherDetails(Id) {
    $("#teachersDiv").addClass("hidden");
    var div = $("#teacherDetailsDiv");
    div.removeClass("hidden");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        // TODO: add url to div
        url: "/Teachers/TeacherDetails",
        data: { Id: Id },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                div.html(res);
                LoadTeacherStudents(Id);
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function teacherDetailFormSubmit() {
    addValidator("teacherDetailForm");
    if ($("#teacherDetailForm").valid()) {
        $("#teacherDetailForm").trigger('submit');
    }
}

function OnBeginUpdateTeacher() {
    LoadingState(true);
    LoadingStateMessage($("#updateTeacher"));
}

function OnSuccessUpdateTeacher(result) {
    AjaxCommonSuccessHandling(result, function () {
        SetNeedRefreshTeachers();
        UpdateTeacherStudents("teacherStudentsList", $("#TeacherId").val());
    });
}

function OnCompleteUpdateTeacher() {
    LoadingState(false);
    updateDBInfo();
    BackToTeachersList();
}
