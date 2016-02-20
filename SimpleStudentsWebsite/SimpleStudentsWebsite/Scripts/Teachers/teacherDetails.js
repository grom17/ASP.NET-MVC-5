function TeacherDetails(Id) {
    $("#teachersDiv").addClass("hidden");
    var div = $("#teacherDetailsDiv");
    div.removeClass("hidden");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        // TODO: add url to div
        url: "/Teachers/TeacherDetails",
        data: { Id: Id },
        success: function (res) {
            AjaxCommonSuccessHandling(res, function () {
                div.html(res);
                LoadTeacherStudents(Id);
            });
        },
        error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            LoadingState(false, div);
        }
    });
}

function teacherDetailFormSubmit() {
    addValidator("teacherDetailForm");
    if ($("#teacherDetailForm").valid()) {
        // If form valid set loading state
        OnBeginUpdateTeacher();

        // Updating teacher details
        $.ajax({
            // TODO: add url to div
            url: "/Teachers/UpdateTeacherDetails",
            type: "POST",
            data:
                {
                    TeacherId: $("#TeacherId").val(),
                    FirstName: $("#FirstName").val(),
                    LastName: $("#LastName").val(),
                    Login: $("#Login").val()
                }
            ,
            success: function (result) {
                AjaxCommonSuccessHandling(result, function () {
                    UpdateTeacherStudents("teacherStudentsList", $("#TeacherId").val());
                });
            },
            error: AjaxCommonErrorHandling,
            complete: function (req, status) {
                OnCompleteUpdateTeacher();
            }
        });
    }
}

function OnBeginUpdateTeacher() {
    LoadingState(true);
    LoadingStateMessage($("#updateTeacher"));
}

function OnCompleteUpdateTeacher() {
    LoadingState(false);
    updateDBInfo();
    SetNeedRefreshTeachers();
    BackToTeachersList();
}
