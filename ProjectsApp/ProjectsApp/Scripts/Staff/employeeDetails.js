function EmployeeDetails(PersonId) {
    $("#staffDiv").addClass("hidden");
    var div = $("#employeeDetailsDiv");
    div.removeClass("hidden");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: div.data('detail-url'),
        data: { PersonId: PersonId },
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

function employeeDetailFormSubmit() {
    addValidator("employeeDetailForm");
    if ($("#employeeDetailForm").valid()) {
        $("#employeeDetailForm").trigger('submit');
    }
}

function OnBeginUpdateEmployee() {
    LoadingState(true);
    LoadingStateMessage($("#updateEmployee"));
}
function OnSuccessUpdateEmployee(result) {
    AjaxCommonSuccessHandling(result, function () {
        SetNeedRefresh();
    });
}
function OnCompleteUpdateEmployee() {
    LoadingState(false);
    BackToList();
}
