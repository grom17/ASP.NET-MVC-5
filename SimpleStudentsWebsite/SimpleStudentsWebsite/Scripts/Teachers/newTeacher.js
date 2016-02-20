function PrepareCreateTeacher() {
    $("#teachersDiv").addClass("hidden");
    var div = $("#newTeacherDiv");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                div.html(res);
                LoadTeacherStudents(0);
                addValidator("newTeacherForm");
                div.removeClass("hidden");
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false,div);
        }
    });
}

function newTeacherFormSubmit() {
    if ($("#newTeacherForm").valid()) {
        // TODO: Add check: if no one student was selected show error msg
        $("#newTeacherForm").trigger('submit');  
    }
}

function OnBeginCreateTeacher() {
    LoadingState(true, $("#newTeacherForm"));
    LoadingStateMessage($("#createTeacherBtn"), $("#newTeacherForm"));
}
function OnSuccessCreateTeacher(result) {
    AjaxCommonSuccessHandling(result, function () {
        UpdateTeacherStudents("newTeacherStudentsList", result.Id);
    });
}
function OnCompleteCreateTeacher() {
    LoadingState(false, $("#newTeacherForm"));
    updateDBInfo();
    SetNeedRefreshTeachers();
    //$('#newTeacherStudentsList').dataTable().fnDestroy();
    BackToTeachersList();
}