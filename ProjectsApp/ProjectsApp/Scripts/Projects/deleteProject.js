function DeleteProject(Id) {
    // TODO: Add confirm message before delete project
    var btn = $("#deleteProject");
    var div = $("#projectDetailForm");
    var url = btn.data("action-url");
    LoadingState(true, div);
    LoadingStateMessage(btn, div);
    $.ajax({
        url: url,
        data: { ProjectId: Id},
        success: function (result) {
            OnSuccessDeleteProject(result);
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function OnSuccessDeleteProject(result) {
    AjaxCommonSuccessHandling(result, function () {
        SetNeedRefresh();
        BackToList();
    });
}