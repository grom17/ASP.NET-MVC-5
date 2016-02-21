function LoadReports(id) {
    var div;
    var mainDivs = [
        $("#bestStudentsDiv"),
        $("#teacherReportsDiv"),
        $("#teacherReportsDiv")
    ]
    var teacherDivs = [
        $("#teachersOfAllStdsDiv"),
        $("#teachersOfLowerCountStdsDiv")
    ]
    var table;
    div = mainDivs[id - 1];
    table = id == 1 ? "students" : "teachers";
    mainDivs.forEach(function (item)
    {
        item.addClass("hidden");
    });
    teacherDivs.forEach(function (item) {
        item.addClass("hidden");
    });
    LoadingState(true);
    LoadingStateMessage(div);
    $.ajax({
        url: div.data("action-url"),
        data: { id: id },
        success: function (result) {
            AjaxCommonSuccessHandling(result, function () {
                var data = $.parseJSON(result);
                if (id == 1) {
                    ApplyStudentsListDataTable("students", data);
                }
                else {
                    ApplyTeachersListDataTable("teachers", data);
                    teacherDivs[id - 2].removeClass("hidden");
                }
                div.removeClass("hidden");
            });
        },
        complete: function (req, status) {
            LoadingState(false);
        }
    });
}