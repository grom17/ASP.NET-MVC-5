// make with timeout to allow hide it by 'click' event for whole body.
function ShowSuccess(message) {
    setTimeout(function () {
        ShowNotification(message, 'success');
    }, 0);
}
function ShowInfo(message) {
    setTimeout(function () {
        ShowNotification(message, 'information');
    }, 0);
}
function ShowWarn(message) {
    setTimeout(function () {
        ShowNotification(message, 'warning');
    }, 0);
}
function ShowError(message) {
    setTimeout(function () {
        ShowNotification(message, 'error');
    }, 0);
}
function ShowNotification(message, type) {
    if (message == null || message == '' || typeof message === "undefined") {
        return;
    }
    message = message.replace('\\n', '<br />');
    var n = noty({ text: message, layout: 'topRight', theme: 'defaultTheme', timeout: false, type: type, closeWith: ['click'] });
}
function NConfirm(qtext, postext, negtext, posClick, negClick) {
    MessageConfirm(qtext, posClick, negClick);
}
function MessageConfirm(qtext, posClick, negClick) {
    if ($("#noty_center_layout_container").length > 0) {
        return;
    }
    setTimeout(function () {
        ShowConfirm(qtext, posClick, negClick);
    }, 0);
}
function ShowConfirm(text, posClick, negClick, currentValue) {
    successClick = function () {
        var tb = $("#GradeTb");
        var tbValue = parseInt(tb.val());
        if (tbValue <= 5 && tbValue >= 1) {
            NotificationDisableClose = false;
            posClick(tbValue);
        } else {
            ShowError('Допустимое значение от 1 до 5');
        }
    };
    noty({
        layout: 'center',
        theme: 'defaultTheme',
        modal: true,
        text: text,
        animation: {
            open: { height: 'toggle' },
            close: { height: 'toggle' },
            easing: 'swing',
            speed: 0 // opening & closing animation speed
        },
        buttons: [{
            addClass: 'glyphicon glyphicon-ok', onClick: function ($noty) {
            successClick();
            if (!NotificationDisableClose) {
                $noty.close();
            }
        }
    },
    {
        addClass: 'glyphicon glyphicon-remove', onClick: function ($noty) {
        NotificationDisableClose = false;
        negClick();
        $noty.close();
    }
    }
    ]
    });
    var textboxString = '<input id="GradeTb" class="form-control input-sm' +
                        (' name="GradeTb" value="{0}" type="text"/>'.format(currentValue));
    var textbox = $(textboxString);
    NotificationDisableClose = true;
    $("#noty_center_layout_container").find('.noty_message').after(textbox);

    $("#noty_center_layout_container").find('li').css({ width: '200px' });
    $("#GradeTb").focus();
}
var NotificationDisableClose = false;
$(document).ready(function () { $('body').click(function (e) { ClosePopupMessages(e) }) });
function ClosePopupMessages(e) {
    if (!NotificationDisableClose) {
        $.noty.clearQueue();
        $.noty.closeAll();
    }
}