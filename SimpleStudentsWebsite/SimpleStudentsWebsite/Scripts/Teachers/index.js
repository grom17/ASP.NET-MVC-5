$(function () {
    setTimeout(ReadTeachers, 0);
});

function ReadTeachers() {
    var div = $("#teachersDiv");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: $("#teachersDiv").data("action-url"),
        success: function (result) {
            div.html(result);
            $("#teachers").slimtable({
                colSettings:
                    [{
			            colNumber: 2, enableSort: false
                    }]
            });
            //$("#teachers").filterable();
        },
        function (req, status) {
            LoadingState(false, div);
        }
    });
}