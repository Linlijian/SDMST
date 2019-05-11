;
(function ($, $validator) {
    $validator = $validator || $.validator;
    if ($validator) {
        $.validator.methods.date = function (value, element) {
            var format = $(element).data("date-format");
            if (format == undefined || format == null || format == "") {
                return this.optional(element) || Globalize.parseDate(value, "dd/MM/yyyy", "en-US") || Globalize.parseDate(value, "dd/MM/yyyy HH:mm:ss", "en-US");
            }
            return this.optional(element) || Globalize.parseDate(value, format, "en-US");
        }

        $.validator.methods.number = function (value, element) {
            var nValue = value.replace(/\,/g, '');
            if (/\./g.test(nValue)) {
                return this.optional(element) || /^-?\d+\.\d+$/g.test(nValue);
            }
            return this.optional(element) || /^\-?\d+$/g.test(nValue);
        }

        $.validator.addMethod('activeandstatusmatch', function (value, element, params) {
            var isValid = true;
            var paramVal = $("[name=" + params.comparewith + "]").val();
            if (element.name == "STATUS") {
                if (paramVal == "Y" && (value == undefined || value == null || value == "N"))
                    isValid = false;
            }
            else {
                if (value == "Y" && (paramVal == undefined || paramVal == null || paramVal == "N"))
                    isValid = false;
            }
            return isValid;
        });

        // Registration - Will check if the phone number fits to phone prefix
        $.validator.unobtrusive.adapters.add('activeandstatusmatch', ['comparewith'], function (options) {
            options.rules['activeandstatusmatch'] = options.params;
            if (options.message != null) {
                options.messages['activeandstatusmatch'] = options.message;
            }
        });

        $.validator.addMethod('store', function (value, element, params) {
            var isValid = true;
            var elm = $(element);
            var paramIdxElm = elm.data('indexparameter');
            var valueElm = elm.val();
            var parameterValue = [];
            if (paramIdxElm != undefined && paramIdxElm != null) {
                parameterValue[paramIdxElm] = valueElm;
            }
            else {
                parameterValue.push(valueElm);
            }
            var paramNames = [];
            if (params.parameternames != undefined && params.parameternames != null && params.parameternames != "") {
                paramNames = params.parameternames.split(",");
                $.each(paramNames, function (i, item) {
                    var paramElm = $('[name=' + item + ']');
                    var paramIdx = paramElm.data('indexparameter');
                    var paramVal = paramElm.val();
                    if (paramIdx != undefined && paramIdx != null) {
                        parameterValue[paramIdx] = paramVal;
                    }
                    else {
                        parameterValue.push(paramVal);
                    }
                });

            }

            var nData = { keySource: params.keyid };
            nData["parameterValue"] = parameterValue.join();

            var rootPath = '';

            if (params.apppath != undefined) {
                rootPath = params.apppath;

                if (rootPath == '/') {
                    rootPath = '';
                }
            }

            $.ajax({
                url: rootPath + '/Ux/Autocomplete/IsValidStore',
                type: 'get',
                data: nData,
                async: false,
                success: function (result) {
                    isValid = result.Status;
                },
                error: function (request, status, error) {
                    if (request.status == 440) {//440 Login Timeout
                        var res = JSON.parse(request.responseText);
                        window.location.href = res.Url;
                    }
                    else {
                        alert(request.responseText);
                    }
                }
            });

            if (paramNames.length > 0) {
                $.each(paramNames, function (i, item) {
                    var paramElm = $('[name=' + item + ']');
                    if (!paramElm.is('[type=hidden]')) {
                        var formGroup = paramElm.closest('.form-group');
                        if (!isValid) {
                            if (formGroup != undefined && formGroup != null && formGroup.length > 0) {
                                if (formGroup.hasClass('has-success')) {
                                    formGroup.removeClass('has-success');
                                }
                                formGroup.addClass('has-error');
                            }
                            var validMsg = $('[data-valmsg-for="' + item + '"]');
                            if (validMsg != undefined && validMsg != null && validMsg.length > 0) {
                                if (validMsg.hasClass('field-validation-valid')) {
                                    validMsg.removeClass('field-validation-valid');
                                }
                                var errMsgConfig = paramElm.data('val-store');
                                var errMsg = '<span id="' + item + '-error" class="text-danger">' + errMsgConfig + '</span>';
                                validMsg.addClass('field-validation-error').html(errMsg);
                            }
                            paramElm.attr('aria-invalid', 'true');
                        }
                        else {
                            if (formGroup != undefined && formGroup != null && formGroup.length > 0) {
                                if (formGroup.hasClass('has-error')) {
                                    formGroup.removeClass('has-error');
                                }
                                formGroup.addClass('has-success');
                            }
                            var validMsg = $('[data-valmsg-for="' + item + '"]');
                            if (validMsg != undefined && validMsg != null && validMsg.length > 0) {
                                if (validMsg.hasClass('field-validation-error')) {
                                    validMsg.removeClass('field-validation-error');
                                }
                                validMsg.addClass('field-validation-valid').empty();
                            }
                            paramElm.attr('aria-invalid', 'false');
                        }
                    }
                });
            }
            return isValid;
        });

        // Registration - Will check if the phone number fits to phone prefix
        $.validator.unobtrusive.adapters.add('store', ['keyid', 'apppath', 'parameternames'], function (options) {
            options.rules['store'] = options.params;
            if (options.message != null) {
                options.messages['store'] = options.message;
            }
        });

        $.validator.addMethod('idcard', function (value, element, params) {
            var isValid = true;
            var paramVal = $("[name=" + params.comparewith + "]").val();

            var type = paramVal;

            //try
            //{//มาเป็น radio
            //    if ($("[name=" + params.comparewith + "]").length > 1) {
            //        if ($("[id=" + params.comparewith + "_P]").prop('checked')) {
            //            type = "P";//ประเภทบุคคลธรรมดา
            //        }
            //        else if ($("[id=" + params.comparewith + "_N]").prop('checked')) {
            //            type = "N";//ประเภทบุคคลธรรมดา
            //        }
            //        else if ($("[id=" + params.comparewith + "_J]").prop('checked')) {
            //            type = "J";//นิติบุคคล
            //        }
            //    }
            //}
            //catch (err) {
            //    type = paramVal;
            //}

            //เปลี่ยนเป็นเหลือแค่ Y,N 
            if (type != 'N' || type == undefined) {
                if (value.length != 13) {
                    isValid = false;
                }

                for (i = 0, sum = 0; i < 12; i++) {
                    sum += parseFloat(value.charAt(i)) * (13 - i);
                }
                if ((11 - sum % 11) % 10 != parseFloat(value.charAt(12))) {
                    isValid = false;
                }
            }

            return isValid;
        });
        $.validator.unobtrusive.adapters.add('idcard', ['comparewith'], function (options) {
            options.rules['idcard'] = options.params;
            if (options.message != null) {
                options.messages['idcard'] = options.message;
            }
        });

        //Check File Type Not exe or bat
        $.validator.addMethod('notfileexebat', function (value, element, params) {
            var isValid = true;
            var parameterValue = value;
            if (params.parameternames != undefined && params.parameternames != null && params.parameternames != "") {
                parameterValue += ",";
                var paramNames = params.parameternames.split(",");
                $.each(paramNames, function (i, item) {
                    var paramVal = $("[name=" + item + "]").val();
                    parameterValue += paramVal;
                    if (i != paramNames.length - 1) {
                        parameterValue += ",";
                    }
                });

            }

            if (parameterValue == 'exe' || parameterValue == 'bat') {
                isValid = false;
            }

            return isValid;
        });
        $.validator.unobtrusive.adapters.add('notfileexebat', ['keyid', 'parameternames'], function (options) {
            options.rules['notfileexebat'] = options.params;
            if (options.message != null) {
                options.messages['notfileexebat'] = options.message;
            }
        });

        //Check File Type Not exe or bat
        $.validator.addMethod('chackpassword', function (value, element, params) {
            var isValid = true;
            var parameterValue = value;
            if (params.parameternames != undefined && params.parameternames != null && params.parameternames != "") {
                parameterValue += ",";
                var paramNames = params.parameternames.split(",");
                $.each(paramNames, function (i, item) {
                    var paramVal = $("[name=" + item + "]").val();
                    parameterValue += paramVal;
                    if (i != paramNames.length - 1) {
                        parameterValue += ",";
                    }
                });

            }

            if (parameterValue.match(/[a-z]/) && parameterValue.match(/\d+/) && parameterValue.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/)) {
                isValid = true;
            }
            else {
                isValid = false;
            }

            return isValid;
        });
        $.validator.unobtrusive.adapters.add('chackpassword', ['keyid', 'parameternames'], function (options) {
            options.rules['chackpassword'] = options.params;
            if (options.message != null) {
                options.messages['chackpassword'] = options.message;
            }
        });

        $.validator.addMethod('equaltoafter', function (value, element, params) {
            var target = $(params);
            if (this.settings.onfocusout && target.not(".validate-equaltoafter-blur").length) {
                target.addClass("validate-equaltoafter-blur").on("blur.validate-equaltoafter", function () {
                    $(element).valid();
                });
            }
            return value === target.val();
        });
        $.validator.unobtrusive.adapters.add('equaltoafter', ['other'], function (options) {
            var prefix = options.element.name.substr(0, options.element.name.lastIndexOf(".") + 1);
            var other = options.params.other;
            var fullOtherName = other;
            if (fullOtherName.indexOf("*.") === 0) {
                fullOtherName = fullOtherName.replace("*.", prefix);
            }

            var otherName = fullOtherName.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
            var elementTo = $(options.form).find(":input").filter("[name='" + otherName + "']")[0];

            options.rules['equaltoafter'] = elementTo;
            if (options.message != null) {
                options.messages['equaltoafter'] = options.message;
            }
        });

        //check check box
        $.validator.addMethod('checkboxcheck', function (value, element, params) {
            var isValid = true;
            var paramVal = $("#" + params.comparewith).prop('checked');

            if ((value == undefined || value == null) && paramVal == false) {
                isValid = false;
            }

            return isValid;
        });
        $.validator.unobtrusive.adapters.add('checkboxcheck', ['comparewith'], function (options) {
            options.rules['checkboxcheck'] = options.params;
            if (options.message != null) {
                options.messages['checkboxcheck'] = options.message;
            }
        });

        //Not Empty With CheckBox Check
        $.validator.addMethod('notemptywithcheckbox', function (value, element, params) {
            var isValid = true;
            var paramVal = $("[name=" + params.comparewith + "]").prop('checked');

            var check = paramVal;

            if (check) {
                if (value == '' || value == undefined) {
                    isValid = false;
                }
            }

            return isValid;
        });
        $.validator.unobtrusive.adapters.add('notemptywithcheckbox', ['comparewith'], function (options) {
            options.rules['notemptywithcheckbox'] = options.params;
            if (options.message != null) {
                options.messages['idcard'] = options.message;
            }
        });

        $.validator.addMethod('greaterthanorequal', function (value, element, params) {
            function toNumber(str) {
                return str.replace(/\,/g, '');
            }
            var isValid = true;
            var parameterValue = Number(toNumber(value));
            var compareValue = 0;
            if (params.type === 'Ctrl') {
                compareValue = Number(toNumber($('#' + params.valuetocompare).val()));
            }
            else {
                compareValue = Number(params.valuetocompare);
            }
            isValid = (parameterValue >= compareValue);

            return isValid;
        });

        // Registration - Will check if the phone number fits to phone prefix
        $.validator.unobtrusive.adapters.add('greaterthanorequal', ['type', 'valuetocompare'], function (options) {
            options.rules['greaterthanorequal'] = options.params;
            if (options.message != null) {
                options.messages['greaterthanorequal'] = options.message;
            }
        });

        $.validator.addMethod('lessthanorequal', function (value, element, params) {
            function toNumber(str) {
                return str.replace(/\,/g, '');
            }
            var isValid = true;
            var parameterValue = Number(toNumber(value));
            var compareValue = 0;
            if (params.type === 'Ctrl') {
                compareValue = Number(toNumber($('#' + params.valuetocompare).val()));
            }
            else {
                compareValue = Number(params.valuetocompare);
            }
            isValid = (parameterValue <= compareValue);

            return isValid;
        });

        // Registration - Will check if the phone number fits to phone prefix
        $.validator.unobtrusive.adapters.add('lessthanorequal', ['type', 'valuetocompare'], function (options) {
            options.rules['lessthanorequal'] = options.params;
            if (options.message != null) {
                options.messages['lessthanorequal'] = options.message;
            }
        });

        //setDefaults
        $.validator.setDefaults({
            ignore: ".ignore,:hidden:not(.validate)"
        });
    }

})(jQuery, jQuery.validator);