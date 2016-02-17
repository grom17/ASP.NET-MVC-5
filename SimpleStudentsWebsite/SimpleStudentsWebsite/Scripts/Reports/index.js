function LoadReports(id) {
    var div;
    var divs = [
        $("#bestStudentsDiv"),
        $("#teachersOfAllStdsDiv"),
        $("#teachersOfLowerCountStdsDiv")
    ]
    var table;
    div = divs[id - 1];
    table = id == 1 ? "students" : "teachers";
    divs.forEach(function (item)
    {
        item.addClass("hidden");
    });
    div.removeClass("hidden");
    LoadingState(true, div);
    LoadingStateMessage(div, div);
    $.ajax({
        url: div.data("action-url"),
        data: { id: id },
        success: function (result) {
            div.html(result);
            console.log($("table#" + table));
            $("table#" + table).slimtable({
                colSettings:
                [{
			        colNumber: 2, enableSort: false
                }]
            });
        },
        function (req, status) {
            LoadingState(false, div);
        }
    });
}