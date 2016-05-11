function ProjectDetails(ProjectId) {
    $("#projectsDiv").addClass("hidden");
    var div = $("#projectDetailsDiv");
    div.removeClass("hidden");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: div.data('detail-url'),
        data: { ProjectId: ProjectId },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                div.html(res);
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}

function projectDetailFormSubmit() {
    addValidator("projectDetailForm");
    if ($("#projectDetailForm").valid()) {
        $("#projectDetailForm").trigger('submit');
    }
}

function OnBeginUpdateProject() {
    LoadingState(true);
    LoadingStateMessage($("#updateEmployee"));
}
function OnSuccessUpdateProject(result) {
    AjaxCommonSuccessHandling(result, function () {
        SetNeedRefresh();
    });
}
function OnCompleteUpdateProject() {
    LoadingState(false);
    BackToList();
}
