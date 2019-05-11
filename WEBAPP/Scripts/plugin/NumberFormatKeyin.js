/**
 * @author jittagornp
 * create 16/11/2015
 * 
 * require jQuery - https://code.jquery.com/jquery-1.11.3.min.js
 * require numeral - https://cdnjs.cloudflare.com/ajax/libs/numeral.js/1.4.5/numeral.min.js
 */

(function (document, $, numeral, SELECTOR) {
    $(function () {

        var $doc = $(document);
        var $selector = $(SELECTOR);
        var FORMAT = '#,###,###,###,##0';

        function isFullNumberFormat(str) {

            if (isPointFormat(str) && isCommaFormat(str)) {
                return /^-?\d{1,3}(\,\d{3})+\.\d+$/g.test(str);
            }

            if (isPointFormat(str)) {
                return /^-?\d+\.\d+$/g.test(str);
            }

            if (isCommaFormat(str)) {
                return /^-?\d{1,3}(\,\d{3})+$/g.test(str);
            }

            return /^\-?\d+$/g.test(str);
        }

        function isNumberFormat(str) {

            if (isPointFormat(str)) {
                return /^-?\d+\.\d+$/g.test(str);
            }

            return /^\-?\d+$/g.test(str);
        }

        function isPointFormat(str) {
            return /\./g.test(str);
        }

        function isCommaFormat(str) {
            return /\,/g.test(str);
        }

        function isNotNumberFormat(str) {
            return str.length
                    && !/\.$/.test(str)
                    && !/\-$/.test(str)
                    && !isNumberFormat(str);
        }

        function isNumberKeyCode(code) {
            return code >= 48 && code <= 57;
        }

        function isNumpadKeyCode(code) {
            return code >= 96 && code <= 105;
        }

        function isCtrlA(event) {
            return event.keyCode === 65 && event.ctrlKey === true;
        }

        function isCtrlC(event) {
            return event.keyCode === 67 && event.ctrlKey === true;
        }

        function isCtrlV(event) {
            return event.keyCode === 86 && event.ctrlKey === true;
        }

        function isCtrlX(event) {
            return event.keyCode === 88 && event.ctrlKey === true;
        }

        function isRemove(event) {
            return event.keyCode === 46 || event.keyCode === 8;
        }

        function specialKeyCode(event) {
            return $.inArray(event.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
                    isCtrlA(event) ||
                    isCtrlC(event) ||
                    isCtrlV(event) ||
                    isCtrlX(event) ||
                    (event.keyCode >= 35 && event.keyCode <= 39);
        }

        function isNotNumberKeyCode(event) {
            // Ensure that it is a number and stop the keypress       
            return (event.shiftKey || !isNumberKeyCode(event.keyCode))
                    && !isNumpadKeyCode(event.keyCode);
        }

        function makeFloatingFormat(str) {
            return str.split(/\.(.*?)$/)[1]
                    .replace(/./g, function (e) {
                        return '#';
                    });
        }

        function format(str, digits) {
            var floatingFormat = "";
            if (digits > 0) {
                for (var i = 0; i < digits; i++) {
                    floatingFormat += "0";
                }
            }
            if (isPointFormat(str)) {
                var strs = str.split('.');
                var newStr = strs[0] + '.' + strs[1];
                if (digits > 0) {
                    return numeral(newStr).format(FORMAT + '.' + floatingFormat);
                }
                return numeral(newStr).format(FORMAT + '.' + makeFloatingFormat(newStr));
            }

            if (digits > 0) {
                return numeral(str).format(FORMAT + '.' + floatingFormat);
            }

            return numeral(str).format(FORMAT);
        }

        function toNumber(str) {
            return str.replace(/\,/g, '');
        }

        function doFormat() {
            var $input = $(this);
            if (enableFormat($input)) {
                var val = toNumber($input.val());
                if (isNumberFormat(val)) {
                    $input.val(format(val));
                }
            }
        }

        function pasteFormat() {
            var $input = $(this);
            var val = $input.val();
            if (val && !isFullNumberFormat(val)) {
                $input.val('');
            }
        }

        function fromFormat() {
            var $input = $(this);
            var val = toNumber($input.val());
            if (isNumberFormat(val)) {
                $input.val(val);
            }
        }

        function isNegativeORDot(event) {
            return event.keyCode === 189 || event.keyCode === 190;
        }

        function invalidNegativePosition(event, val) {
            return event.keyCode === 189 && /.+\-/g.test(val);
        }

        function doubleNegativeORDot(val) {
            return /(\-\-)+/g.test(val)
                    || /(\.\.)+/g.test(val)
                    || /(\-\.)+/g.test(val)
                    || /(\.\-)+/g.test(val);
        }

        function cutLast($input, val) {
            val && $input.val(val.substr(0, val.length - 1));
        }

        function enableFormat($el) {
            return $el.hasClass('number-format');
        }

        function callback(event, onSpc, onFormat) {

            if (specialKeyCode(event)) {
                if (isRemove(event)) {
                    onSpc && onSpc.call(this);
                }

                if (isCtrlV(event)) {
                    pasteFormat.call(this);
                }
                return;
            }

            var $input = $(this);
            var val = $input.val();
            var digits = $input.data("digits");

            if (isNegativeORDot(event)) {
                var posInt = $input.data("positiveint");
                if ((event.keyCode === 190 && digits === -1) || posInt) {
                    event.preventDefault();
                    return;
                }
                if ((invalidNegativePosition(event, val) || doubleNegativeORDot(val))) {
                    cutLast($input, val);
                    event.preventDefault();
                }
                return;
            }

            if (isNotNumberKeyCode(event)) {
                event.preventDefault();
                return;
            }

            val = toNumber(val);
            if (isNotNumberFormat(val)) {
                cutLast($input, val);
                event.preventDefault();
                return;
            }
            if (enableFormat($input)) {
                onFormat && onFormat.call(this, val);
            }
        }

        $doc.on(
                'keydown',
                SELECTOR,
                callback
            ).on('focusin',
                SELECTOR,
                function (event) {
                    $(this).val(toNumber($(this).val()));
                    $(this).select();
                })
            .on(
                'focusout',
                SELECTOR,
                function (event) {
                    var ctrl = $(this);
                    var val = toNumber(ctrl.val());
                    if (isNumberFormat(val)) {
                        if (enableFormat(ctrl)) {
                            var digits = ctrl.data("digits");
                            $(this).val(format(ctrl.val(), digits));
                        }
                    }
                    else {
                        ctrl.val('');
                    }
                }
            ).on(
                'submit',
                'form',
                function () {

                    $(this).find(SELECTOR).each(fromFormat);

                }
            );

        $selector.each(doFormat);
    });
})(document, jQuery, numeral, 'input[type="text"].input-number');