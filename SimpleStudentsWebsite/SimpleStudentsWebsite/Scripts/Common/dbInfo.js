function updateDBInfo() {
    $.ajax({
        url: "/Home/GetDBInfo",
        success: function (result) {
            AjaxCommonSuccessHandling(result, function () {
                var studentsCount = result.studentsCount;
                var teachersCount = result.teachersCount;
                var dbInfoStudents = "Студентов в базе: {0}".format(studentsCount);
                var dbInfoTeachers = "Преподавателей в базе: {0}".format(teachersCount);
                $("#dbInfoStudents").text(dbInfoStudents);
                $("#dbInfoTeachers").text(dbInfoTeachers);
            });
        },
        error: AjaxCommonErrorHandling
    });
}