$.ajaxSetup({ cache: false }); // prevent cache the ajax response for IE
$(document).ready(function () {
    setInterval('updateClock()', 1000);
    setTimeout('updateDBInfo()', 0);
});

window.onerror = function (msg, url, line, col, error) {
    try {
        LoadingState(false);
        var errorMessage = msg;
        if (url) {
            errorMessage += " at " + url;
            if (line) {
                errorMessage += " line: " + line;
                if (col) {
                    errorMessage += " col: " + col;
                }
            }
        }
        try {
            if (error && error.stack) {
                var errorName = error.name + ": " + error.message;
                errorMessage = error.stack;
                if (errorMessage.indexOf(errorName) == -1) { // firefox return stack without error name and message.
                    errorMessage = errorName + "\n" + errorMessage;
                }
            }
        } catch (e) { }
        LogErrorOnServer("JSError", errorMessage);
    } catch (e) { }
    return false;
};

// common handler ajax errors, just show notification
function AjaxCommonErrorHandling(req, status, error) {
    ShowError(error);
}
// common handler ajax success result, show error if any errors,
// show success message if have in response and run onsuccess delegate function
function AjaxCommonSuccessHandling(result, onsuccess) {
    if (result.errors) {
        ShowError(result.errors);
    } else {
        if (result.success) {
            ShowSuccess(result.success);
        }
        if (typeof result === "string" && result.indexOf("account-login") != -1) {
            window.location = "~/Account/Login";
        } else {
            var html = result;
            if (result.html) {
                html = result.html;
            }
            onsuccess(html);
        }
    }
}
function onError(e, status) {
    ShowError(e.errors);
}

// method to call backend action to log error
function LogErrorOnServer(operation, error) {
    $.ajax({
        url: $("body").data('log-error'),
        type: "POST",
        data: { Operation: operation, Error: error },
        success: function (result) {
        }
    })
}

// String.Format. First, checks if it isn't implemented yet.
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}

function addValidator(form) {
    form = "#" + form;
    $(form).removeData("validator");
    $(form).removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(form);
    var validator = $(form).data("validator");
    if (validator) {
        validator.settings.ignore = "";
    }
    DisableOnKeyUpValidation(form);
}

function DisableOnKeyUpValidation(form) {
    var validator = $(form).data("validator");
    if (validator) {
        validator.settings.onkeyup = false; // disable validation on keyup
    }
}