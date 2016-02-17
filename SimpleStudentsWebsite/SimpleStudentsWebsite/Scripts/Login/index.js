function OnSuccessLogin(result) {
    AjaxCommonSuccessHandling(result, function () {
        if (result.url) {
            window.location.href = result.url;
        }
    });
}
function OnBeginLogin() {
    LoadingState(true);
    LoadingStateMessage($("#LoginBtn"));
}
function OnCompleteLogin() {
    LoadingState(false);
}