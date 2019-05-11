$(function () {
    var confirmCloseMsg = $("#hdSystemLang").val() == "en-US" ? "Do your want to close this page?" : "คุณต้องการปิดหน้านี้หรือไม่?";

    //Date Picker By Jubpas
    $(".datepicker-input").JDatepicker();
    $(".datepicker").datepicker({
        locale: "en",
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true,
        orientation: 'bottom',
        enableOnReadonly: false
    }).on("hide", function (selected) {
        $(this).blur();
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });

    //Fileupload By Jubpas
    $(".input-fileupload").ace_file_input({
        no_file: 'No File ...',
        btn_choose: 'Choose',
        btn_change: 'Change',
        droppable: false,
        onchange: null,
        thumbnail: false //| true | large
        //whitelist:'gif|png|jpg|jpeg'
        //blacklist:'exe|php'
        //onchange:''
        //
    });

    $(".std-btn-confirm").vsmConfirm({
        title: null,
        columnClass: 'medium',
        buttons: {
            confirm: {
                text: $("#hdSysOK").val(),
                btnClass: "btn-primary",
                action: function () {
                    try {
                        this.buttons.confirm.disable();
                        var button = this.$target;
                        var url = button.data("url");
                        var btnType = button.data("stdbtntype");
                        var isValidate = button.data("isvalidate");
                        var requiredCer = button.data("requiredcer");
                        var overrideSubmit = button.data("overridesubmit");
                        var formName = button.data("formname");
                        var form = $("form[id=" + formName + "]");
                        if (isValidate == "True") {
                            var isValid = true;
                            isValid = form.valid();
                            if (!form.validFile()) {
                                isValid = false;
                            }
                            if (!form.validTable()) {
                                isValid = false;
                            }
                            if (isValid) {
                                if (btnType == "ButtonComfirmAjax") {
                                    if (requiredCer == "True" && $.hasFileInServer()) {
                                        $.vsmSelectCer({
                                            onSign: function (fP12, pwP12) {
                                                var selCer = this;
                                                if (overrideSubmit == "False") {
                                                    form.serializeFormDataWithCerServer(fP12, pwP12, function (result, formData) {
                                                        if (result) {
                                                            selCer.close();
                                                            waitingDialog.show();
                                                            formData.append("button", button.attr("name"));
                                                            $.ajax({
                                                                url: url,
                                                                type: 'post',
                                                                contentType: false,
                                                                processData: false,
                                                                data: formData,
                                                                success: OnAjaxSuccess,
                                                                error: OnAjaxError
                                                            });
                                                        }
                                                        else {
                                                            selCer.hideLoading(true);
                                                        }
                                                    });
                                                    return false;
                                                }
                                                else {
                                                    waitingDialog.show();
                                                    OnAjaxSubmit(button, form, fP12, pwP12);
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        waitingDialog.show();
                                        if (overrideSubmit == "False") {
                                            var formData = form.serializeFormData();
                                            formData.append("button", button.attr("name"));
                                            $.ajax({
                                                url: url,
                                                type: 'post',
                                                contentType: false,
                                                processData: false,
                                                data: formData,
                                                success: OnAjaxSuccess,
                                                error: OnAjaxError
                                            });
                                        }
                                        else {
                                            OnAjaxSubmit(button, form);
                                        }
                                    }
                                }
                                else if (btnType == "ButtonComfirm") {
                                    location.href = url;
                                }
                                else if (btnType == "ButtonComfirmSubmit") {
                                    form[0].action = url;
                                    form.submit();
                                }
                                else if (btnType == "ButtonComfirmNewWindows") {
                                    window.open(url, "_blank");
                                }
                            }
                            else {
                                if ($("#CaptchaImg").length > 0) {
                                    SetCaptcha();
                                }
                                var result = {
                                    Status: false,
                                    Style: 'danger',
                                    Message: $('#hdSysValidateError').val()
                                };
                                OnAjaxSuccess(result);
                            }
                        }
                        else {
                            if (btnType == "ButtonComfirmAjax") {
                                if (requiredCer == "True" && $.hasFileInServer()) {
                                    $.vsmSelectCer({
                                        onSign: function (fP12, pwP12) {
                                            var selCer = this;
                                            if (overrideSubmit == "False") {
                                                form.serializeFormDataWithCerServer(fP12, pwP12, function (result, formData) {
                                                    if (result) {
                                                        selCer.close();
                                                        waitingDialog.show();
                                                        formData.append("button", button.attr("name"));
                                                        $.ajax({
                                                            url: url,
                                                            type: 'post',
                                                            contentType: false,
                                                            processData: false,
                                                            data: formData,
                                                            success: OnAjaxSuccess,
                                                            error: OnAjaxError
                                                        });
                                                    }
                                                    else {
                                                        selCer.hideLoading(true);
                                                    }
                                                });
                                                return false;
                                            }
                                            else {
                                                waitingDialog.show();
                                                OnAjaxSubmit(button, form, fP12, pwP12);
                                            }
                                        }
                                    });
                                }
                                else {
                                    waitingDialog.show();
                                    if (overrideSubmit == "False") {
                                        var formData = form.serializeFormData();
                                        formData.append("button", button.attr("name"));
                                        $.ajax({
                                            url: url,
                                            type: 'post',
                                            contentType: false,
                                            processData: false,
                                            data: formData,
                                            success: OnAjaxSuccess,
                                            error: OnAjaxError
                                        });
                                    }
                                    else {
                                        OnAjaxSubmit(button, form);
                                    }
                                }
                            }
                            else if (btnType == "ButtonComfirm") {
                                location.href = url;
                            }
                            else if (btnType == "ButtonComfirmSubmit") {
                                form[0].action = url;
                                form.submit();
                            }
                            else if (btnType == "ButtonComfirmNewWindows") {
                                window.open(url, "_blank");
                            }
                        }
                    } catch (e) {
                    }
                }
            },
            cancel: {
                text: $("#hdSysCancel").val(),
                btnClass: "btn-primary"
            }
        }
    });

    $(".std-btn-clear").vsmConfirm({
        title: null,
        columnClass: 'medium',
        buttons: {
            confirm: {
                text: $("#hdSysOK").val(),
                btnClass: "btn-primary",
                action: function () {
                    location.href = this.$target.data('url');
                }
            },
            cancel: {
                text: $("#hdSysCancel").val(),
                btnClass: "btn-primary"
            }
        }
    });

    $(".std-btn-delete").vsmConfirm({
        title: null,
        columnClass: 'medium',
        buttons: {
            confirm: {
                text: $("#hdSysOK").val(),
                btnClass: "btn-primary",
                action: function () {
                    var button = this.$target;
                    waitingDialog.show();
                    OnBtnDelete(button);
                }
            },
            cancel: {
                text: $("#hdSysCancel").val(),
                btnClass: "btn-primary"
            }
        }
    });

    //Search Click
    $(".std-btn-search").on("click", function (btn) {
        var button = $(this);
        var formName = button.data("formname");
        var form = $("form[id=" + formName + "]");
        if (form != undefined && form != null) {
            var isValidate = button.data("isvalidate");
            if (isValidate == "True") {
                var isValid = true;
                isValid = form.valid();
                if (!form.validTable()) {
                    isValid = false;
                }
                if (isValid) {
                    var data = form.serializeObject();
                    data['button'] = this.name;
                    OnBtnSearch(data);
                }
            }
            else {
                var data = form.serializeObject();
                data['button'] = this.name;
                OnBtnSearch(data);
            }
        }
        return false;
    });

    $(".std-btn-print").on("click", function (btn) {
        var button = $(this);
        var formName = button.data("formname");
        var url = button.data("url");
        var isValidate = button.data("isvalidate");
        var form = $("form[id=" + formName + "]");
        if (form != undefined && form != null) {
            if (isValidate == "True") {
                var isValid = true;
                isValid = form.valid();
                if (isValid) {
                    var formData = form.serializeFormData();
                    formData.append("button", button.attr("name"));

                    $.ajax({
                        url: url,
                        type: 'post',
                        contentType: false,
                        processData: false,
                        data: formData,
                        success: function (url) {
                            if (url.Errors != undefined && url.Errors != null) {
                                OnAjaxSuccess(url);
                            }
                            else {
                                window.open(url, '_blank');
                            }
                        },
                        error: OnAjaxError
                    });
                }
            }
            else {
                var formData = form.serializeFormData();
                formData.append("button", button.attr("name"));

                $.ajax({
                    url: url,
                    type: 'post',
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (url) {
                        if (url.Errors != undefined && url.Errors != null) {
                            OnAjaxSuccess(url);
                        }
                        else {
                            window.open(url, '_blank');
                        }
                    },
                    error: OnAjaxError
                });
            }
        }
        return false;
    });
    $(".std-btn-submit").on("click", function (btn) {
        var button = $(this);
        var formName = button.data("formname");
        var url = button.data("url");
        var form = $("form[id=" + formName + "]");
        if (form != undefined && form != null) {
            form[0].action = url;
            var isValidate = button.data("isvalidate");
            var btn = $('<input type="hidden" name="button" value="' + button.attr("name") + '">')
            form.append(btn);
            if (isValidate == "True") {
                if (form.valid()) {
                    form.submit();
                }
            }
            else {
                form.submit();
            }
        }
        return false;
    });
    //For Error Validateion
    var isCss = $(".field-validation-error");
    if (!isCss.hasClass("text-danger")) {
        isCss.addClass("text-danger");
    }

    $("select[data-role=vsmmultiselect]").vsmMultiselect();

    $("input[data-val-range-max],input[data-val-length-max],input[data-val-maxlength-max]").each(function (i, e) {
        var input = $(e);
        var maxLength = input.data("val-maxlength-max");
        if (maxLength == undefined || maxLength == null || maxLength == "") {
            maxLength = input.data("val-length-max");
        }
        if (maxLength == undefined || maxLength == null || maxLength == "") {
            maxLength = input.data("val-range-max");
        }
        if (maxLength != undefined && maxLength != null) {
            input.attr("maxlength", maxLength);
        }
    });


    $('.std-modal-confirmclose').confirm({
        title: null,
        content: confirmCloseMsg,
        columnClass: 'medium',
        buttons: {
            confirm: {
                text: $("#hdSysOK").val(),
                btnClass: "btn-primary",
                action: function () {
                    var button = this.$target;
                    var modal = button.closest(".modal");
                    modal.modal("hide");
                }
            },
            cancel: {
                text: $("#hdSysCancel").val(),
                btnClass: "btn-primary"
            }
        }
    });
    $('.std-modal-confirmcancel').vsmConfirm({
        title: null,
        columnClass: 'medium',
        buttons: {
            confirm: {
                text: $("#hdSysOK").val(),
                btnClass: "btn-primary",
                action: function () {
                    var button = this.$target;
                    var modal = button.closest(".modal");
                    modal.modal("hide");
                }
            },
            cancel: {
                text: $("#hdSysCancel").val(),
                btnClass: "btn-primary"
            }
        }
    });

    //Remove Button Active
    //$(".btn").mouseup(function () {
    //    $(this).blur();
    //});

    $(".validcomparewith").each(function (i, c) {
        var ctlr = $(c);
        var ctlrCompWith = ctlr.data("valid-comparewith");
        var ctlrCompWithAll = $('[name=' + ctlrCompWith + ']');
        $.each(ctlrCompWithAll, function (i, item) {
            var ctlrCp = $(item);
            if (ctlr.val() != ctlrCp.val()) {
                ctlr.closest('.form-group').addClass('has-diff');
            }
            else if (ctlr.closest('.form-group').hasClass('has-diff')) {
                ctlr.closest('.form-group').removeClass('has-diff');
            }
        });

    }).on("change blur keyup", function () {
        var btn = $(this);
        var ctlrCompWith = btn.data("valid-comparewith");
        var ctlrCompWithAll = $('[name=' + ctlrCompWith + ']');
        $.each(ctlrCompWithAll, function (i, item) {
            var btnCp = $(item);
            if (btn.val() != btnCp.val()) {
                btn.closest('.form-group').addClass('has-diff');
            }
            else if (btn.closest('.form-group').hasClass('has-diff')) {
                btn.closest('.form-group').removeClass('has-diff');
            }
        });
    });
});

//end JQUERY
var OnAjaxSubmit = function (btn, form, fP12, pwP12) {
    waitingDialog.hide();
}

var OnCloseSuccess = function (result) {
    if (result.IsReturnValue) {
        $.each(result.ReturnValues, function (key, value) {
            try {
                if (key != null && key != undefined && key != "") {
                    var ctrl = $("#" + key);
                    if (ctrl != undefined) {
                        ctrl.val(value);
                    }
                }
            } catch (e) {

            }
        });
    }
    if (result.Status && (result.Mode == "SaveModify" || result.Mode == "SaveCreate")) {
        var standardButton = $(".buttontoobars").children();
        $.each(standardButton, function (i, item) {
            if ($.inArray(item.name, result.IgnoreBtn) == -1 && item.name != "Clear" && item.name != "Reset") {
                //item.remove();
                $(item).remove();//Fix For IE
            }
        });
        standardButton = $(".wizardbuttontoobars").children();
        $.each(standardButton, function (i, item) {
            if ($.inArray(item.name, result.IgnoreBtn) == -1 && item.name != "Clear" && item.name != "Reset") {
                //item.remove();
                $(item).remove();//Fix For IE
            }
        });
    }
}

var OnSuccess = function (result) {
    if ($.isPlainObject(result)) {
        if (result.Status && (result.Mode == "SaveModify" || result.Mode == "SaveCreate" || result.Mode == "Delete")) {
            if ((result.RedirectToUrl != null && result.RedirectToUrl != undefined && result.RedirectToUrl != "")) {
                if (result.ExistsChioce) {
                    var confirmMsg = '<br/>คุณต้องการที่จะปิดหน้านี้หรือไม่?';
                    if ($("#hdSystemLang").val() == "en-US") {
                        confirmMsg = "<br/>Are you sure to close this page?";
                    }
                    $.confirm({
                        title: null,
                        content: result.Message + confirmMsg,
                        columnClass: 'medium',
                        buttons: {
                            confirm: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    if (result.RedirectToUrl != null && result.RedirectToUrl != undefined && result.RedirectToUrl != "") {
                                        window.location.href = result.RedirectToUrl;
                                    }
                                    else {
                                        OnCloseSuccess(result);
                                    }
                                }
                            },
                            cancel: {
                                text: $("#hdSysCancel").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    if (result.CancelToUrl != null && result.CancelToUrl != undefined && result.CancelToUrl != "") {
                                        window.location.href = result.CancelToUrl;
                                    }
                                    else {
                                        OnCloseSuccess(result);
                                    }
                                }
                            }
                        }
                    });
                }
                else {
                    $.confirm({
                        title: null,
                        content: result.Message,
                        columnClass: 'medium',
                        buttons: {
                            confirm: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    if (result.RedirectToUrl != null && result.RedirectToUrl != undefined && result.RedirectToUrl != "") {
                                        window.location.href = result.RedirectToUrl;
                                    }
                                    else {
                                        OnCloseSuccess(result);
                                    }
                                }
                            }
                        }
                    });
                }
            }
            else {
                $.confirm({
                    title: null,
                    content: result.Message,
                    columnClass: 'medium',
                    buttons: {
                        confirm: {
                            text: $("#hdSysOK").val(),
                            btnClass: "btn-primary"
                        }
                    },
                    onClose: function () {
                        OnCloseSuccess(result);
                    }
                });
            }
        }
        else {
            var content = '<div class="alert alert-' + result.Style + ' alert-dismissable alert-' + result.Style + 'new"><button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button><h2>';
            if (result.Style == "danger") {
                content += '<i class="ace-icon fa fa-times-circle bigger-130"></i> ';
            }
            else if (result.Style == "warning") {
                content += '<i class="ace-icon fa fa-exclamation-triangle bigger-130"></i> ';
            }
            else if (result.Style == "info") {
                content += '<i class="ace-icon fa fa-check-circle bigger-130"></i> ';
            }
            content += result.Message + '</h2>';
            if (result.Errors != null && result.Errors != undefined) {
                content += '<hr class="message-inner-separator"><ul>';
                $.each(result.Errors, function (i, obj) {
                    content += '<li>';
                    if (obj.Key != null && obj.Key != "") {
                        content += '<strong>' + obj.Key + '</strong>';
                    }
                    if (obj.Message != null && obj.Message != "") {
                        content += ' ' + obj.Message;
                    }
                    content += '</li>';
                })
                content += '</ul>';
            }
            content += '</div>';

            $("#notification").html(content).fadeTo(2000, 500);

            OnAfterSuccess(result);
        }
    }
    else {
        $.confirm({
            title: null,
            closeIcon: true,
            content: result,
            columnClass: 'xl',
            buttons: {
                confirm: {
                    text: $("#hdSysOK").val(),
                    btnClass: "btn-primary"
                }
            }
        });
    }
}

var OnAfterSuccess = function (result) {
}

var OnBtnSearch = function (result) {
    //overide Your Code
    if (typeof window["bindGrid" + result.button] != 'undefined' && $.isFunction(window["bindGrid" + result.button])) {
        window["bindGrid" + result.button](result);
    }
}

var OnBtnDelete = function (button) {
    //overide Your Code
    if (typeof window["on" + button[0].name] != 'undefined' && $.isFunction(window["on" + button[0].name])) {
        window["on" + button[0].name](button);
    }
    else {
        waitingDialog.hide();
    }
}

var OnAjaxSuccess = function (result) {
    waitingDialog.hide();
    setTimeout(function () {
        if (result.Mode == "Time Out") {
            var rootPath = $("#hdSysRootPath").val();
            if (rootPath == undefined || rootPath == null || rootPath == "/") {
                rootPath = "/";
            }
            window.location.href = rootPath + "Users/Account/SignOut";
        }
        else if (result.Mode == "Access denied") {
            if ($("#hdSysPageError").val() != "") {
                window.location.href = $("#hdSysPageError").val() + '?exception=' + toUrlString(result.Mode) + '&errorcode=401';
            }
            else {
                $.confirm({
                    title: null,
                    closeIcon: true,
                    content: result.Mode,
                    columnClass: 'm',
                    buttons: {
                        confirm: {
                            text: $("#hdSysOK").val(),
                            btnClass: "btn-primary"
                        }
                    }
                });
            }
        }
        else if (result.Mode == "Search" && result.Status) {
            OnBtnSearch(result);
        }
        else if (result.Mode == "Delete" && result.Status) {
            OnSuccess(result);
            result["page"] = 1;
            OnBtnSearch(result);
        }
        else {
            OnSuccess(result);
        }
    }, 500);
}

var OnAjaxError = function (request, status, error) {
    waitingDialog.hide();
    setTimeout(function () {
        if (request.status == 440 || request.responseText.indexOf('The provided anti-forgery token') != -1) {//440 Login Timeout
            var rootPath = $("#hdSysRootPath").val();
            if (rootPath == undefined || rootPath == null || rootPath == "/") {
                rootPath = "/";
            }
            window.location.href = rootPath + "Users/Account/SignOut";
        }
        else if (request.status == 401) {//401 Access denied
            var res = JSON.parse(request.responseText);
            if ($("#hdSysPageError").val() != "") {
                window.location.href = $("#hdSysPageError").val() + '?exception=' + toUrlString(res.Mode) + '&errorcode=' + request.status;
            }
            else {
                $.confirm({
                    title: null,
                    closeIcon: true,
                    content: res.Mode,
                    columnClass: 'm',
                    buttons: {
                        confirm: {
                            text: $("#hdSysOK").val(),
                            btnClass: "btn-primary"
                        }
                    }
                });
            }
        }
        else {
            $.confirm({
                title: null,
                closeIcon: true,
                content: request.responseText,
                columnClass: 'xl',
                buttons: {
                    confirm: {
                        text: $("#hdSysOK").val(),
                        btnClass: "btn-primary"
                    }
                }
            });
        }
    }, 500);
}

function confirmDelete(e) {
    var ajax = $(e).data("ajax");
    var callback = $(e).data("callback");
    //var form = this.form; //get current form

    $.confirm({
        title: null,
        closeIcon: true,
        buttons: {
            confirm: {
                text: $("#hdSysOK").val(),
                btnClass: "btn-primary",
                action: function () {
                    if (ajax == "True") {
                        OnAjaxDelete(callback);
                    }
                    else {
                        var form = $("form");
                        form[0].method = "post";
                        form[0].action = callback;
                        form.submit();
                    }
                }
            },
            cancel: {
                text: $("#hdSysCancel").val(),
                btnClass: "btn-primary"
            }
        }
    });

    return false;
}

function confirmDialog(e) {

    var callback = $(e).data("callback");
    var message = $(e).data("message");
    var ajax = $(e).data("ajax");

    //var form = this.form; //get current form
    $.confirm({
        post: true,
        text: message, //e.target.dataset.message,
        confirm: function (btn) {
            //alert("You just confirmed.");
            var form = $("form")[0];
            //form.method = "post";
            //form.action = callback;
            form.submit();
        },
        cancel: function (btn) {
            //alert("You cancelled.");
            return false;
        }
    });

    return false;
}

var jsonDateToFormat = function (data, format) {
    return $.jsonDateToFormat(data, format);
};

var jsonDateAndTimeToFormat = function (data) {
    return $.jsonDateAndTimeToFormat(data);
};

var gridRenderDateToFormat = function (data, type, full, meta) {
    return jsonDateToFormat(data);
};

var gridRenderDateAndTimeToFormat = function (data, type, full, meta) {
    return jsonDateAndTimeToFormat(data);
};

var gridRenderNumberToFormat = function (data, type, full, meta) {
    return toNumberFormat(data);
};

var gridRenderNumberToFormatDigit = function (data, type, full, meta, digit) {
    var str = toNumberFormat(data);
    return digitsFormat(str, digit);
};

var gridDateToISOString = function (data) {
    return $.gridDateToISOString(data);
};

var dateToISOString = function (data) {
    var nData = "";
    if (data != undefined && data != null && data != "") {
        var date = $.stringToDate(data);
        if (date != undefined || date != null) {
            nData = date.toISOString();
        }
    }
    return nData;
};

var OnAjaxWizardSuccess = function (result) {
    waitingDialog.hide();
    setTimeout(function () {
        if (result.Status && (result.Mode == "SaveModify" || result.Mode == "SaveCreate" || result.Mode == "Delete")) {
            $.confirm({
                title: null,
                content: result.Message,
                columnClass: 'medium',
                buttons: {
                    confirm: {
                        text: $("#hdSysOK").val(),
                        btnClass: "btn-primary",
                        action: function () {
                            if (result.RedirectToUrl != null && result.RedirectToUrl != undefined && result.RedirectToUrl != "")
                                window.location.href = result.RedirectToUrl;
                        }
                    }
                },
                onClose: function () {
                    OnCloseSuccess(result);
                }
            });
        }
        else {
            var content = '<div class="alert alert-' + result.Style + ' alert-dismissable"';
            if (result.Style == "danger") {
                content += ' style="border-width:1px;border-color:#b20b08;"><button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button><h2><i class="ace-icon fa fa-times-circle bigger-130"></i> ';
            }
            else if (result.Style == "warning") {
                content += ' style="border-width:1px;border-color:#8a6d3b;"><button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button><h2><i class="ace-icon fa fa-exclamation-triangle bigger-130"></i> ';
            }
            else if (result.Style == "warning") {
                content += ' style="border-width:1px;border-color:#3c763d;"><button type="button" class="close" data-dismiss="alert"><i class="ace-icon fa fa-times"></i></button><h2><i class="ace-icon fa fa-check-circle bigger-130"></i> ';
            }
            content += result.Message + '</h2>';
            if (result.Errors != null && result.Errors != undefined) {
                content += '<hr class="message-inner-separator"><ul>';
                $.each(result.Errors, function (i, obj) {
                    content += '<li>';
                    if (obj.Key != null && obj.Key != "") {
                        content += '<strong>' + obj.Key + '</strong>';
                    }
                    if (obj.Message != null && obj.Message != "") {
                        content += ' ' + obj.Message;
                    }
                    content += '</li>';
                })
                content += '</ul>';
            }
            content += '</div>';

            $("#notification").html(content).fadeTo(2000, 500);
        }
    }, 500);
}

var toUrlString = function (data) {
    return $.toUrlString(data);
};
var toNumber = function (data) {
    if (data == undefined || data == null || data == "") {
        return data;
    }
    return numeral(data).value();
};
var noExponents = function (value) {
    var data = String(value).split(/[eE]/);
    if (data.length == 1) return data[0];

    var z = '',
      sign = this < 0 ? '-' : '',
      str = data[0].replace('.', ''),
      mag = Number(data[1]) + 1;

    if (mag < 0) {
        z = sign + '0.';
        while (mag++) z += '0';
        return z + str.replace(/^\-/, '');
    }
    mag -= str.length;
    while (mag--) z += '0';
    return str + z;
}

var toNumberFormat = function (data, format, Fixformat) {

    function isPointFormat(str) {
        return /\./g.test(str);
    }

    function isNumberFormat(str) {

        if (isPointFormat(str)) {
            return /^-?\d+\.\d+$/g.test(str);
        }

        return /^\-?\d+$/g.test(str);
    }

    function makeFloatingFormat(str) {
        return str.split(/\.(.*?)$/)[1]
                .replace(/./g, function (e) {
                    return '#';
                });
    }

    function isExponentst(str) {
        return /^[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?$/g.test(str);
    }

    function toFormat(data) {
        var chars = nData.split('');
        var j = 1;
        for (var i = chars.length - 1; i > 0; i--) {
            if (j == 3) {
                j = 1;
                chars.splice(i, 0, ',');
            }
            else {
                j++;
            }

        }
        return chars.join('');
    }

    if (format == undefined || format == null || format == "") {
        format = "#,###,###,###,###,###,###,###,##0";
    }

    if (data != undefined && !isNaN(data)) {
        var nData = '' + data;
        if (isNumberFormat(nData)) {
            if (isPointFormat(nData) && (Fixformat == undefined || Fixformat == null || Fixformat == "")) {
                return numeral(nData).format(format + '.' + makeFloatingFormat(nData));
            }
            return numeral(nData).format(format);
        }
        else if (isExponentst(nData)) {
            nData = noExponents(nData);
            if (isPointFormat(nData)) {
                var datas = nData.split('.');
                return toFormat(datas[0]) + '.' + datas[1];
            }

            return toFormat(nData);
        }
    }

    return data;
}

var ExportNotIcon = function (idx, data, node) {
    if (node.className.indexOf("sorting_disabled") > -1) {
        return false;
    }
    else {
        return true;
    }
}

var ExportCellFormatText = function (data) {
    for (var i = 0; i < data.body.length; i++) {
        for (var j = 0; j < data.body[i].length; j++) {
            data.body[i][j] = data.body[i][j].toString().replace(">=", "\u2265").replace("<=", "\u2264").replace("<>", "\u2260");
        }
    }
}

function digitsFormat(str, digit) {

    function makeFloatingFormat(str) {
        return str.split(/\.(.*?)$/)[1]
                .replace(/./g, function (e) {
                    return '#';
                });
    }

    function isPointFormat(str) {
        return /\./g.test(str);
    }

    var digits = 2;
    if (digit != undefined) {
        digits = digit;
    }

    var FORMAT = '#,###,###,###,##0';
    var floatingFormat = "";
    if (digits > 0) {
        for (var i = 0; i < digits; i++) {
            floatingFormat += "0";
        }
    }
    if (digits > 0) {
        return numeral(str).format(FORMAT + '.' + floatingFormat);
    }
    if (isPointFormat(str)) {
        return numeral(str).format(FORMAT + '.' + makeFloatingFormat(str));
    }
    return numeral(str).format(FORMAT);
}

//Tooltip
$('[data-toggle="tooltip"]').tooltip({
    trigger: 'hover'
});

$("#btnCaptchaRefresh").click(function () {
    SetCaptcha();
});

function SetCaptcha() {
    LoadDefaultCaptcha();
    return true;
}

function LoadDefaultCaptcha() {
    var rootPath = $("#hdSysRootPath").val();
    if (rootPath == undefined || rootPath == null || rootPath == "/") {
        rootPath = "/";
    }

    $.ajax({
        url: rootPath + 'Users/Captcha/GetCaptcha',
        type: "POST",
        data: { "reset": true },
        success: function (result) {
            $("#divCaptcha").html('<img src="data:image/png;base64,' + result + '" class="img-responsive" />');
        },
        error: OnAjaxError
    });
}

var isNullOrEmpty = function (data) {
    return $.isNullOrEmpty(data);
}

//Ctrl In Grid
var getInputText = function (options) {
    var settings = {
        colName: 'txtGridEdit',
        value: '',
        readonly: false,
        type: 'text',
        digits: 0,
        required: false,
        valididcard: false,
        isidcard: false,
        maxLength: undefined
    };
    $.extend(settings, options);
    var tag = '<input id="' + settings.colName + '" name="' + settings.colName + '" type="text" class="form-control input-sm"';
    if (settings.readonly) {
        tag += ' readonly';
    }
    if (settings.maxLength != undefined) {
        tag += ' maxlength = "' + settings.maxLength + '"';
    }
    if (settings.required) {
        tag += ' placeholder="' + $("#hdSysInputPlaceholderRequired").val() + '"';
        tag += ' data-isrequired="true"';
    }
    if (settings.valididcard) {
        tag += ' data-valididcard="true"';
    }
    if (settings.isidcard) {
        tag += ' data-isidcard="true"';
    }
    tag += '>';
    var inp = $(tag);
    if (settings.type == 'number') {
        inp.addClass("input-number");
        if (!settings.isidcard) {
            inp.addClass("text-right");
        }
        if (!isNullOrEmpty(settings.value)) {
            inp.val(settings.value);
        }
    }
    else if (settings.type == 'number-format') {
        inp.addClass("input-number number-format text-right");
        var format = "#,###,###,###,###,###,###,###,##0";
        if (settings.digits == 2) {
            format += ".00";
            inp.data("digits", 2);
        }
        else if (settings.digits == 4) {
            format += ".0000";
            inp.data("digits", 4);
        }
        else if (settings.digits == 6) {
            format += ".000000";
            inp.data("digits", 6);
        }
        if (!isNullOrEmpty(settings.value)) {
            inp.val(toNumberFormat(settings.value, format));
        }
    }
    else {
        if (!isNullOrEmpty(settings.value)) {
            inp.val(settings.value);
        }
    }
    return inp;
}
var getInputSelect = function (options) {
    var settings = {
        colName: 'slGridEdit',
        value: '',
        readonly: false,
        required: false,
        options: [],
        optionValue: 'Value',
        optionText: 'Text'
    };
    $.extend(settings, options);
    var tag = '<select id="' + settings.colName + '" name="' + settings.colName + '" type="text" class="form-control input-sm"';
    if (settings.readonly) {
        tag += ' disabled';
    }
    if (settings.required) {
        tag += ' data-isrequired="true"';
    }
    tag += '></select>';
    var inp = $(tag);
    var opt = '<option value="">';
    if (settings.required) {
        opt += $("#hdSysSelectDefaultRequired").val();
    }
    opt += '</option>';
    inp.append(opt);
    if (settings.options != null && $.isArray(settings.options)) {
        $.each(settings.options, function (i, item) {
            opt = '<option value="' + item[settings.optionValue] + '"';
            if (item[settings.optionValue] == settings.value) {
                opt += ' selected';
            }
            opt += '>' + item[settings.optionText] + '</option>';
            inp.append(opt);
        });
    }
    return inp;
}
var getInputDate = function (options) {
    var settings = {
        colName: 'dtGridEdit',
        value: '',
        readonly: false,
        required: false,
        customTrigger: false
    };
    $.extend(settings, options);
    var inpg = $('<div class="input-group"></div>');
    var tag = '<input id="' + settings.colName + '" name="' + settings.colName + '" type="text" class="form-control input-sm datepicker-input"';
    if (settings.readonly) {
        tag += ' readonly';
    }
    if (!isNullOrEmpty(settings.value)) {
        tag += ' value="' + $.jsonDateToFormat(settings.value) + '"';
    }
    if (settings.required) {
        tag += ' placeholder="' + $("#hdSysInputPlaceholderRequired").val() + '"';
        tag += ' data-isrequired="true"';
    }
    tag += '>';
    var inp = $(tag);
    if (!settings.customTrigger && !settings.readonly) {
        inp.addClass('datepicker');
    }
    inpg.append(inp);
    var icon = $('<span class="input-group-addon"><i class="ace-icon fa fa-calendar"></i></span>');
    inpg.append(icon);
    return inpg;
}
var getInputFile = function (options) {
    var settings = {
        name: 'xxxx',
        colName: 'dtGridEdit',
        value: '',
        mode: '',
        allowType: '',
        allowTypeHelp: undefined,
        rowIndex: 0,
        readonly: false,
        multiple: false,
        required: false
    };
    $.extend(settings, options);
    var tag = '';
    if (!settings.readonly) {
        tag += '<input type="file" id="f' + settings.name + settings.colName + '" name="' + settings.colName + '" style="display:none;" data-vsmctrl="true" data-filetype="fileingrid"';
        if (!$.isNullOrEmpty(settings.allowType)) {
            tag += ' accept="' + settings.allowType + '"';
        }
        if (settings.multiple) {
            tag += ' multiple';
        }
        if (settings.required) {
            tag += ' data-isrequired="true"';
        }
        tag += ' data-mode="' + settings.mode + '"';
        if (settings.mode == 'Edit' || settings.mode == 'CellEdit') {
            tag += ' data-rowindex="' + settings.rowIndex + '"';
        }
        tag += '/>';
        tag += '<div class="gridToolbar">';
        tag += '<a href="#" id="btnAdd' + settings.name + settings.colName + '" class="datatables-btn" data-toggle="tooltip" data-mode="' + settings.mode + '"';
        tag += ' title="' + $("#hdSysAddFile").val() + '"';

        if (settings.mode == 'Edit' || settings.mode == 'CellEdit') {
            tag += ' data-rowindex="' + settings.rowIndex + '"';
        }
        tag += '>';
        tag += '<i class="ace-icon fa fa-plus-circle align-top bigger-125 green"></i>';
        tag += '</a>';
        tag += '<a href="#" id="btnDel' + settings.name + settings.colName + '" class="datatables-btn" data-toggle="tooltip" data-mode="' + settings.mode + '"';
        tag += ' title="' + $("#hdSysDeleteFile").val() + '"';
        if (settings.mode == 'Edit' || settings.mode == 'CellEdit') {
            tag += ' data-rowindex="' + settings.rowIndex + '"';
        }
        tag += '>';
        tag += '<i class="ace-icon fa fa-trash align-top bigger-125 red"></i>';
        tag += '</a>';
        tag += '<span class="help-block">';
        if (!$.isNullOrEmpty(settings.allowTypeHelp)) {
            tag += settings.allowTypeHelp;
        }
        tag += '</span>';
        tag += '</div>';
    }
    tag += '<table id="Grid' + settings.name + settings.colName + '" class="table table-striped table-bordered table-hover gridInGrid">';
    tag += '</table>';
    return tag;
}

var getInputCheckbox = function (options) {
    var settings = {
        colName: 'chkGridEdit',
        value: '',
        readonly: false,
        type: 'checkbox'
    };
    $.extend(settings, options);
    var tag = '<input id="' + settings.colName + '" name="' + settings.colName + '" type="checkbox" class="checkbox center-block"';
    if (settings.readonly) {
        tag += ' disabled';
    }
    if (!isNullOrEmpty(settings.value) && settings.value == 'Y') {
        tag += ' checked';
    }
    tag += '/>';
    var inp = $(tag);
    return inp;
}

var getInputAutocomplete = function (options) {
    var settings = {
        colName: 'atcGridEdit',
        value: '',
        readonly: false,
        required: false,
        keysource: '',
        bindfield: '',
        fieldtoinput: undefined,
        extraparams: undefined,
        paramsctrl: undefined,
        maxLength: undefined
    };
    $.extend(settings, options);
    var tag = '<input name="' + settings.colName + '" type="text" class="form-control input-sm autocomplete-input"';
    if (settings.readonly) {
        tag += ' readonly';
    }
    tag += '>';
    var inp = $(tag);
    if (settings.required) {
        inp.prop("placeholder", $("#hdSysInputPlaceholderRequired").val());
        tag += ' data-isrequired="true"';
    }
    if (settings.maxLength != undefined) {
        inp.prop('maxlength', settings.maxLength);
    }
    if (!isNullOrEmpty(settings.keysource) && !settings.readonly) {
        inp.data('keysource', settings.keysource);
    }
    if (!isNullOrEmpty(settings.bindfield) && !settings.readonly) {
        inp.data('bindfield', settings.bindfield);
    }
    if (settings.fieldtoinput != undefined && !settings.readonly) {
        inp.data('fieldtoinput', settings.fieldtoinput);
    }
    if (settings.extraparams != undefined && !settings.readonly) {
        inp.data('extraparams', settings.extraparams);
    }
    if (settings.paramsctrl != undefined && !settings.readonly) {
        inp.data('paramsctrl', settings.paramsctrl);
    }
    if (!isNullOrEmpty(settings.value)) {
        inp.data('setvalue', value);
        inp.val(settings.value);
    }
    var newHtml = inp;
    if (settings.readonly) {
        newHtml = $('<span class="block input-icon input-icon-right">');
        newHtml.append(inp);
        newHtml.append('<i class="ace-icon fa fa-search">');
    }
    return newHtml;
}

var getTextArea = function (options) {
    var settings = {
        colName: 'txaGridEdit',
        value: '',
        readonly: false,
        required: false,
        maxLength: undefined
    };
    $.extend(settings, options);
    var tag = '<textarea id="' + settings.colName + '" name="' + settings.colName + '" class="form-control"';
    if (settings.readonly) {
        tag += ' readonly';
    }
    if (settings.maxLength != undefined) {
        tag += ' maxlength = "' + settings.maxLength + '"';
    }
    if (settings.required) {
        tag += ' placeholder="' + $("#hdSysInputPlaceholderRequired").val() + '"';
        tag += ' data-isrequired="true"';
    }
    tag += '>';
    var inp = $(tag);

    if (!isNullOrEmpty(settings.value)) {
        inp.val(settings.value);
    }
    return inp;
}

var sysInterval;
var ifSlowShowLoading = function () {
    sysInterval = setInterval(function () {
        waitingDialog.show();
        clearInterval(sysInterval);
    }, 300);
};

function GenContentError(result) {
    var content = '<div class="alert alert-' + result.Style + ' alert-dismissable" style="border-width:1px;border-color:#b20b08;">';
    content += ' <button type="button" class="close" data-dismiss="alert">';
    content += ' <i class="ace-icon fa fa-times"></i>';
    content += ' </button>';
    content += ' <h2>';
    if (result.Style == "danger") {
        content += '<i class="ace-icon fa fa-times-circle bigger-130"></i> ';
    }
    else if (result.Style == "warning") {
        content += '<i class="ace-icon fa fa-exclamation-triangle bigger-130"></i> ';
    }
    else if (result.Style == "info") {
        content += '<i class="ace-icon fa fa-check-circle bigger-130"></i> ';
    }
    content += result.Message + '</h2>';
    content += ' <hr class="message-inner-separator"><ul>';
    $.each(result.Errors, function (i, obj) {
        content += '<li>';
        if (obj.Key != null && obj.Key != "") {
            content += '<strong>' + obj.Key + '</strong>';
        }
        if (obj.Message != null && obj.Message != "") {
            content += ' ' + obj.Message;
        }
        content += '</li>';
    })
    content += ' </ul>';

    return content;
};