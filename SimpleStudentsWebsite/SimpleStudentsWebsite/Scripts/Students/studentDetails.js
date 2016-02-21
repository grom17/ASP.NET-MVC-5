function StudentDetails(Id) {
    $("#studentsDiv").addClass("hidden");
    var div = $("#studentDetailsDiv");
    div.removeClass("hidden");
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: "/Students/StudentDetails",
        data: { Id: Id },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                $("#studentsDiv").addClass("hidden");
                div.html(res);
                LoadStudentGrades(Id);
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}

function studentDetailFormSubmit() {
    addValidator("studentDetailForm");
    if ($("#studentDetailForm").valid()) {
        $("#studentDetailForm").trigger('submit');
    }
}

function OnBeginUpdateStudent() {
    LoadingState(true);
    LoadingStateMessage($("#updateStudent"));
}
function OnSuccessUpdateStudent(result) {
    AjaxCommonSuccessHandling(result, function () {
        SetNeedRefresh();
        UpdateStudentGrades("studentGradesList", $("#StudentId").val());
    });
}
function OnCompleteUpdateStudent() {
    LoadingState(false);
    updateDBInfo();
    SetNeedRefresh();
    BackToList();
}
