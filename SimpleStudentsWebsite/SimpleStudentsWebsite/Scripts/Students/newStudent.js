function PrepareCreateStudent() {
    $("#studentsDiv").addClass("hidden");
    var div = $("#newStudentDiv");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
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
            LoadingState(false,div);
        }
    });
}

function newStudentFormSubmit() {
    // TODO: Add check: if teacher selected but grade textbox is empty show error msg
    // TODO: Add check: if no one teacher was selected show error msg
    if ($("#newStudentForm").valid()) {
        OnBeginCreateStudent();
        $.ajax({
            url: "/Students/CreateStudent",
            type: "POST",
            data:{
                FirstName: $("#FirstName").val(),
                LastName: $("#LastName").val(),
                Login: $("#Login").val(),
                Password: $("#SecretKey").val()
            },
            success: function (result) { OnSuccessCreateStudent(result) },
            error: AjaxCommonErrorHandling
        });   
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