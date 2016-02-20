function StudentDetails(Id) {
    $("#studentsDiv").addClass("hidden");
    var div = $("#studentDetailsDiv");
    div.removeClass("hidden");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
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
            LoadingState(false, div);
        }
    });
}

function studentDetailFormSubmit() {
    addValidator("studentDetailForm");
    if ($("#studentDetailForm").valid()) {
        // If form valid set loading state
        OnBeginUpdateStudent();

        // Updating student details
        $.ajax({
            url: "/Students/UpdateStudentDetails",
            type: "POST",
            data:
                {
                    StudentId: $("#StudentId").val(),
                    FirstName: $("#FirstName").val(),
                    LastName: $("#LastName").val(),
                    Login: $("#Login").val()
                }
            ,
            success: function (result) {
                AjaxCommonSuccessHandling(result, function () {
                    // If update was OK students list will be refreshed
                    SetNeedRefresh();
                    UpdateStudentGrades("studentGradesList", $("#StudentId").val());
                });
            },
        });
    }
}

function OnBeginUpdateStudent() {
    LoadingState(true);
    LoadingStateMessage($("#updateStudent"), $("#studentForm"));
}
function OnSuccessUpdateStudent(result) {
    AjaxCommonSuccessHandling(result, function () {
    });
}
function OnCompleteUpdateStudent(result) {
    LoadingState(false, $("#studentForm"));
    updateDBInfo();
    SetNeedRefresh();
    BackToList();
}
