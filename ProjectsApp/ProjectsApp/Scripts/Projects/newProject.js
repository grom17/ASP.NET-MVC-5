function PrepareCreateProject() {   
    var div = $("#newProjectDiv");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: div.data("action-url"),
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                $("#projectsDiv").addClass("hidden");
                div.html(res);
                addValidator("createProjectForm");
                $("#StartDate").parent().datetimepicker({
                    format: 'DD/MM/YYYY',
                    startView: "months",
                    minViewMode: "days",
                    pickTime: false,
                    defaultDate: Date.now,
                    locale: 'ru'
                });
                $("#EndDate").parent().datetimepicker({
                    format: 'DD/MM/YYYY',
                    startView: "months",
                    minViewMode: "days",
                    pickTime: false,
                    defaultDate: Date.now,
                    locale: 'ru'
                });
                div.removeClass("hidden");
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}

function createProjectFormSubmit() {
    if ($("#createProjectForm").valid()) {
        $("#createProjectForm").trigger('submit');
    }
}

function OnBeginCreateProject() {
    LoadingState(true, $("#createProjectForm"));
    LoadingStateMessage($("#createProjectBtn"), $("#createProjectForm"));
}
function OnSuccessCreateProject(result) {
    AjaxCommonSuccessHandling(result, function () {
        ProjectDetails(result.Id);
    });
}
function OnCompleteCreateProject() {
    LoadingState(false, $("#createProjectForm"));
    SetNeedRefresh();
}