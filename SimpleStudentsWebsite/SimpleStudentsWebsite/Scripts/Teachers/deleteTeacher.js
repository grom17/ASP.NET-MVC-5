function TeacherDelete(Id) {
    // TODO: Add confirm message before delete teacher
    btn = $("#deleteTeacher");
    div = $("#teacherDetailForm");
    LoadingState(true, div);
    LoadingStateMessage(btn, div);
    $.ajax({
        url: btn.data("action-url"),
        data: { Id: Id},
        success: function (result) {
            OnSuccessDeleteTeacher(result);
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function OnSuccessDeleteTeacher(result) {
    AjaxCommonSuccessHandling(result, function () {
        updateDBInfo();
        SetNeedRefreshTeachers();
        BackToTeachersList();
    });
}