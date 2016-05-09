$.ajaxSetup({ cache: false }); // prevent cache the ajax response for IE

// common handler ajax errors, just show notification
function AjaxCommonErrorHandling(req, status, error) {
    ShowError(error);
}
// common handler ajax success result, show error if any errors,
// show success message if have in response and run onsuccess delegate function
function AjaxCommonSuccessHandling(result, onsuccess) {
    if (result.errors) {
        ShowError(result.errors);
    }
    else {
        if (result.success) {
            ShowSuccess(result.success);
            if (onsuccess) {
                onsuccess();
            }
        }
        else {
            var html = result;
            if (result.html) {
                html = result.html;
            }
            onsuccess(html);
        }
    }
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
    $.validator.unobtrusive.parse($(form));
    var validator = $(form).data("validator");
    if (validator) {
        validator.settings.ignore = "";
    }
    DisableOnKeyUpValidation(form);
}

function DisableOnKeyUpValidation(form) {
    var validator = $(form).data("validator");
    if (validator) {
        validator.settings.onkeyup = false;
    }
}
