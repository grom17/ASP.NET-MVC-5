$(function () {
    setTimeout(ReadStudents, 0);
});

function ReadStudents() {
    var div = $("#studentsDiv");
    $.ajax({
        url: $("#studentsDiv").data("action-url"),
        success: function (result) {
            div.html(result);
            $("#students").slimtable({
                colSettings:
                    [{
			            colNumber: 2, enableSort: false
                    }]
            });
        }
    });
}