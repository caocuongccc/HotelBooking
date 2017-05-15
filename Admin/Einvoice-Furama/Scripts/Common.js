$(document).ready(function () {
    commonJS.Inititalize();
    commonJS.RegistControlDateTime();
    commonJS.registerGenericCompareValidation();
    commonJS.registerCustomValidation();
});
$(document).ajaxComplete(function () {
    //commonJS.MaskSetup();
    commonJS.registerCustomValidation();
    commonJS.RegistControlDateTime();
    commonJS.registerGenericCompareValidation();
    commonJS.registerCustomValidation();
});

var commonJS = {
    AppPath: '',
    Inititalize: function () {
        $(document).ajaxStart(commonJS.OpenLoading);
        $(document).ajaxStop(commonJS.CloseLoading);
        $(document).ajaxComplete(function (event, request, settings) {
            commonJS.Redirect(request.responseText);
        });
        $(document).ajaxSuccess(function (event, request, settings) {
            commonJS.Redirect(request.responseText);
        });
    },
    Redirect:function(command){
        if (command === "Unauthorized") {
            document.write('');
            window.location.href = extensionJS.getVirtualPath("/Home/Index");
        } else if (command === "AccessDenied") {
            document.write('');
            window.location.href = extensionJS.getVirtualPath("/Error/AccessDenied");
        }
    },
    OpenLoading: function () {
        if ($.blockUI) {
            var showImage = '<p><img src="../Content/Images/ajax-loading-white.gif" id="imgLoading"><p>';
            $.blockUI({
                message: showImage,
                css: {
                    left: ($(window).width() - 100) / 2 + 'px',
                    width: '100px',
                    background: 'transparent',
                    border: '0px'
                },
                baseZ: 2000
            });
        }
    },
    CloseLoading: function () {
        if ($.unblockUI) {
            $.unblockUI()
        }
    },

    ValidateForm: function (idform) {
        var form = $(idform)
        $.validator.unobtrusive.parse(form);
        var checkValidate = $(form).valid();
        return checkValidate;
    },
    MaskSetup: function () {
        $.mask.definitions['A'] = "[aApP]";
        $.mask.definitions['M'] = "[mM]";
        $(".maskDate").mask("99/99/9999");
        $(".maskTime").mask("99:99 AM");
        $(".maskDateTime").mask("99/99/9999 99:99 AM");

        $(".maskTelephone").mask("(999) 999-9999");
        $(".maskTelephoneExt").mask("(999) 999-9999? x99999");
        $(".maskTax").mask("99-9999999");
        $(".maskSSN").mask("999-99-9999");
        $(".maskZipCode").mask("99999");
    },
    RegistControlDateTime: function () {
        $(".datetimepicker").each(function (t) {
            $(this).datepicker({
                dateFormat: "mm/dd/yy"
            })
        });
    },
    registerGenericCompareValidation: function () {
        if ($.validator) {
            $.validator.addMethod("genericcompare", function (value, element, params) {
                // debugger;
                if (element == undefined || params == undefined) return true;
                var propelename = params.split(",")[0];
                var operName = params.split(",")[1];
                var notToday = params.split(",")[2];
                if (params == undefined || params == null || params.length == 0 ||
                propelename == undefined || propelename == null || propelename.length == 0 ||
                operName == undefined || operName == null || operName.length == 0)
                    return true;
                var valueOther = $(propelename).val();
                if (valueOther == undefined || valueOther == null || valueOther == "") {
                    return true;
                }
                var val1 = (isNaN(value) ? Date.parse(value) : eval(value));
                var val2 = (isNaN(valueOther) ? Date.parse(valueOther) : eval(valueOther));
                var isToday = true;
                if (notToday != undefined) {
                    switch (notToday) {
                        case ">":
                            isToday = val1 > new Date();
                            break;
                        case "<":
                            isToday = val1 < new Date();
                            break;
                        case ">=":
                            isToday = val1 >= new Date();
                            break;
                        case "<=":
                            isToday = val1 <= new Date();
                            break;
                    }
                }

                if (operName == "GreaterThan")
                    return isToday && val1 > val2;
                if (operName == "LessThan")
                    return isToday && val1 < val2;
                if (operName == "GreaterThanOrEqual")
                    return isToday && val1 >= val2;
                if (operName == "LessThanOrEqual")
                    return isToday && val1 <= val2;
                return true;
            })
            $.validator.addMethod("requiredifany", function (value, element, params) {
                // debugger;
                if (value == undefined || element == undefined || params == undefined) return true;
                var propelename = params.split(",")[0];
                //if (params == undefined || params == null || params.length == 0 ||
                //value == undefined || value == null || value.length == 0 ||
                //propelename == undefined || propelename == null || propelename.length == 0 ||
                //operName == undefined || operName == null || operName.length == 0)
                //    return true;
                var valueOther = $(propelename).val();
                if (valueOther != undefined && valueOther != null && valueOther != '') {
                    return value != undefined && value != null && value != '';
                }
                return true;
            })
            ; $.validator.unobtrusive.adapters.add("genericcompare",
            ["comparetopropertyname", "operatorname"], function (options) {
                options.rules["genericcompare"] = "#" +
                options.params.comparetopropertyname + "," + options.params.operatorname;
                options.messages["genericcompare"] = options.message;
            });
        }
    },
    registerCustomValidation: function () {
        $('.custom-validation').each(function () {
            var rule = $(this).data('validation-rule');
            var para = $(this).data('validation-para');
            var message = $(this).data('validation-message');
            var validateObj = {};
            var messageObj = {};
            messageObj[rule] = message;
            validateObj[rule] = para;
            validateObj['messages'] = messageObj;
            if ($.data(this.form, 'validator') == undefined || $.data(this.form, 'validator') == null) {
                $.validator.unobtrusive.parse(this.form);
            }
            $(this).rules('add', validateObj);
        });
    },
    printRightPanel: function () {
        $('header,footer').addClass('hidden');
        $('.left-container').addClass('hidden');
        $('.hide-when-print').addClass('hidden');
        window.print();
        $('header,footer').removeClass('hidden');
        $('.left-container').removeClass('hidden');
        $('.hide-when-print').removeClass('hidden');
        return true;
    }
}
