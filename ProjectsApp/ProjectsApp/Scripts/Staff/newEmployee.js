function PrepareCreateEmployee() {   
    var div = $("#newEmployeeDiv");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: div.data("action-url"),
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                $("#staffDiv").addClass("hidden");
                div.html(res);
                addValidator("newEmployeeForm");
                div.removeClass("hidden");
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}

function newEmployeeFormSubmit() {
    if ($("#newEmployeeForm").valid()) {
        $("#newEmployeeForm").trigger('submit');
    }
}

function OnBeginCreateEmployee() {
    LoadingState(true, $("#newEmployeeForm"));
    LoadingStateMessage($("#createEmployeeBtn"), $("#newEmployeeForm"));
}
function OnSuccessCreateEmployee(result) {
    AjaxCommonSuccessHandling(result, function () {
        EmployeeDetails(result.Id);
    });
}
function OnCompleteCreateEmployee() {
    LoadingState(false, $("#newEmployeeForm"));
    SetNeedRefresh();
    BackToList();
}