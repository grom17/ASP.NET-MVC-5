function StudentDelete(Id) {
    btn = $("#deleteStudent");
    div = $("#studentDetailForm");
    LoadingState(true, div);
    LoadingStateMessage(btn, div);
    $.ajax({
        url: btn.data("action-url"),
        data: { Id: Id},
        success: function (result) {
            OnSuccessDeleteStudent(result);
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function OnSuccessDeleteStudent(result) {
    AjaxCommonSuccessHandling(result, function () {
        updateDBInfo();
        SetNeedRefresh();
        BackToList();
    });
}