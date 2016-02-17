// show/hide loading panel for control
function LoadingState(value, container) {
    if (typeof container === 'undefined') {
        container = $('body');
    }
    if (value) {
        $("<div class='loading-window'><div class='loading-wrap'></div><div class='loading-element'><div class='loading-img'></div></div></div>").appendTo(container);
        CenterLoading(container);
    } else {
        container.find('.loading-window').remove();
    }
}
// show/hide loading panel for control
function LoadingStateMessage(ctrl, container) {
    if (typeof container === 'undefined') {
        container = $('body');
    }
    if (typeof ctrl === 'undefined') {
        ctrl = $('body');
    }
    var msg = $(ctrl).data('loading-msg');
    if (msg) {
        $("<span class='loading-message'>" + msg + "</span>").appendTo(container.find('.loading-element'));
        CenterLoading(container);
    }
    else
    {

    }
}
// function to apply margin to center the loading popup
function CenterLoading(container) {
    var le = container.find('.loading-element');
    var style = container.attr('style'); // to check width/height of hidden elements
    container.attr('style', "display: block");
    var leHeight = le.height();
    var leWidth = le.width();
    if (style) container.attr('style', style); else container.removeAttr('style')
    le.css({ 'margin-top': -(leHeight / 2), 'margin-left': -(leWidth / 2) });
}