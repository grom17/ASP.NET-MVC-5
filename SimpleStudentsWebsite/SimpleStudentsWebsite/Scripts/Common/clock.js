function updateClock() {
    var now = new Date();

    var year = now.getFullYear();
    var month = formatTimeString(now.getMonth() + 1);
    var day = formatTimeString(now.getDate());
    var hours = formatTimeString(now.getHours());
    var minutes = formatTimeString(now.getMinutes());
    var seconds = now.getSeconds();

    $("#clock").text("Сегодня: " + day + "." + month + "." + year + " " + hours + (seconds % 2 == 0 ? ":" : ' ') + minutes);
}

function formatTimeString(num) {
    return (num < 10 ? "0" : "") + num;
}