function DeleteEmployee(Id) {
    // TODO: Add confirm message before delete employee
    var btn = $("#deleteEmployee");
    var div = $("#employeeDetailForm");
    var url = btn.data("action-url");
    LoadingState(true, div);
    LoadingStateMessage(btn, div);
    $.ajax({
        url: url,
        data: { PersonId: Id},
        success: function (result) {
            OnSuccessDeleteEmployee(result);
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function OnSuccessDeleteEmployee(result) {
    AjaxCommonSuccessHandling(result, function () {
        SetNeedRefresh();
        BackToList();
    });
}