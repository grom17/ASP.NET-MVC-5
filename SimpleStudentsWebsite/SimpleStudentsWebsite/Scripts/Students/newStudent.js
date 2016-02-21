function PrepareCreateStudent() {
    $("#studentsDiv").addClass("hidden");
    var div = $("#newStudentDiv");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: div.data("action-url"),
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                $("#studentsDiv").addClass("hidden");
                div.html(res);
                LoadStudentGrades(0);
                addValidator("newStudentForm");
                div.removeClass("hidden");
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}

function newStudentFormSubmit() {
    // TODO: Add check: if teacher selected but grade textbox is empty show error msg
    // TODO: Add check: if no one teacher was selected show error msg
    if ($("#newStudentForm").valid()) {  
        $("#newStudentForm").trigger('submit');
    }
}

function OnBeginCreateStudent() {
    LoadingState(true, $("#newStudentForm"));
    LoadingStateMessage($("#createStudentBtn"), $("#newStudentForm"));
}
function OnSuccessCreateStudent(result) {
    AjaxCommonSuccessHandling(result, function () {
        UpdateStudentGrades("newStudentGradesList", result.Id);
    });
}
function OnCompleteCreateStudent() {
    LoadingState(false, $("#newStudentForm"));
    updateDBInfo();
    SetNeedRefresh();
    BackToList();
}