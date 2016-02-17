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
function ShowConfirm(qtext, posClick, negClick, textboxConfirmMessage) {
    var successClick = function () {
        posClick();
    };
    if (textboxConfirmMessage) {
        successClick = function () {
            var tb = $("#ConfirmDisable");
            var tbValue = ("" + tb.val());
            if (tbValue.toLowerCase() == textboxConfirmMessage.toLowerCase()) {
                NotificationDisableClose = false; 
                posClick();
            } else {
                tb.removeClass('valid').addClass('input-validation-error').next().removeClass('field-validation-valid').addClass('field-validation-error');
            }
        };
    }
    noty({
        layout: 'center',
        theme: 'defaultTheme',
        modal: true,
        text: qtext,
        animation: {
            open: { height: 'toggle' },
            close: { height: 'toggle' },
            easing: 'swing',
            speed: 0 // opening & closing animation speed
        },
        buttons: [
    { addClass: 'fa fa-yes', onClick: function ($noty) {
        successClick();
        if (!NotificationDisableClose) {
            $noty.close();
        }
    }
    },
    { addClass: 'fa fa-no', onClick: function ($noty) {
        NotificationDisableClose = false;
        negClick();
        $noty.close();
    }
    }
    ]
    });
    if (textboxConfirmMessage) {
        var textbox = $('<div class="confirm-controls"><input id="ConfirmDisable" name="ConfirmDisable" type="text" class="valid"/><span class="field-validation-valid">*</span></div>');
        NotificationDisableClose = true;
        $("#noty_center_layout_container").find('.noty_message').after(textbox);
    }
    $("#noty_center_layout_container").find('li').css({ width: '400px' });
}
var NotificationDisableClose = false;
$(document).ready(function () { $('body').click(function (e) { ClosePopupMessages(e) }) });
function ClosePopupMessages(e) {
    if (!NotificationDisableClose) {
        $.noty.clearQueue();
        $.noty.closeAll();
    }
}