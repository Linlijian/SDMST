(function ($) {

    //* require bootpag - http://raw.github.com/botmonster/jquery-bootpag/master/lib/jquery.bootpag.min.js
    $.fn.JAutocomplete = function (options) {

        var defaults = {
            color: "Red",
            url: null,
            colModel: null,
            ValueField: null,
            textField: null,
            datatype: "json",
            autoChoose: false,
            delayChoose: 300,
            delay: 500,
            rows: 10,
            minLength: 0,
            page: 1,
            pageSize: 10,
            pageIndex: 0,
            rowNumber: 0,
            defaultSet: true,
            valueFiled: "Value",
            setSelectValue: null,
            setValuetoField: null,
            fieldToField: null,
            //Set to CallBack
            extraParam: null,
            extraParamByInput: null,
            sidx: "",
            sort: "",
            recordSet: null,
            //Set to Serverside
            currentPage: 0,
            keySource: "",
            searchTerm: "",
            selectedOnly: false


        };
        var settings = $.extend({}, defaults, options);

        return this.each(function () {
            // Plugin code would go here...
            var elem = $(this);
            //elem.addClass("autocomplete-input");  //add ที่ MVC Helper
            elem.attr("autocomplete", "off");
            var newId = this.id + '_autocomplete';
            var newIdTable = newId + '_table';
            var parent = elem.parent();
            var newHtml = $('<span class="block input-icon input-icon-right">');
            newHtml.append(elem);
            newHtml.append('<i class="ace-icon fa fa-search iconautocomplete">');
            parent.prepend(newHtml);
            parent.addClass("autocomplete");
            parent.append('<div id="' + newId + '" class="ui-autocomplete-result"></div>');
            var contraniner = $("#" + newId);
            var pagerId = 'pager_' + newId;
            var readonly = elem.attr("readonly");
            if (!readonly) {
                $(this).keydown(function (event) {
                    var $field = $(this);
                    if (elem.attr("disabled")) {
                        return;
                    }
                    var keyCode = $.ui.keyCode;
                    var inp = String.fromCharCode(event.keyCode);
                    if (/[ก-ฮa-zA-Z0-9-_ ]/.test(inp)) {
                        //OK
                    } else {
                        if (validateKeyCode(keyCode) == false) {

                        }
                    }

                    //settings.searchTerm = $field.val();
                    // keypress is triggered before the input value is changed
                    clearTimeout(this.searching);
                    this.searching = setTimeout(function () {
                        onLoad();
                        // only search if the value has changed
                        //	if ( self.term != self.element.val()) {
                        //self.selectedItem = null;
                        //self.search(null, event);
                        settings.searchTerm = $field.val();
                        settings.setSelectValue = null;
                        settings.pageIndex = 0;
                        getData(newId, $field.val());

                        //	}
                    }, settings.delay);

                    //clearTimeout()
                });

                $(this).next().click(function (event) {
                    var $field = $(this).prev();
                    var disabled_ctrl = $(this).prev().attr("disabled");
                    if (disabled_ctrl == 'disabled') {
                    }
                    else {
                        onLoad();
                        settings.searchTerm = $field.val();
                        settings.setSelectValue = null;
                        settings.pageIndex = 0;
                        getData(newId, $field.val());
                    }
                });


                $('body').click(function (e) {
                    //if ($(contraniner).is(":visible")) {
                    //    setVisible(contraniner, false);
                    //}
                    var $container = $(contraniner);

                    if (!$container.is(e.target) // if the target of the click isn't the container...
                        && $container.has(e.target).length === 0) // ... nor a descendant of the container
                    {
                        if (settings.selectedOnly) {
                            var oldValue = elem.attr("data-valuebeforeselected");
                            if (oldValue != undefined && oldValue != null) {
                                elem.val(oldValue);
                            }
                        }
                        //container.hide();
                        setVisible(contraniner, false);
                    }
                });


                $("div.autocomplete").on("click", "#" + newIdTable + " tr.ui-row-autocomplete", function (e) {

                    var dataset = settings.recordSet;
                    var rowIndex = $(this).data("rowindex");
                    var selectValue = '';
                    if (dataset[rowIndex][settings.valueFiled] != undefined) {
                        selectValue = dataset[rowIndex][settings.valueFiled];
                    } else {
                        alert('Select value field not found nakrub');
                        return;
                    }
                    if (options.select != undefined) {
                        options.select(e, selectValue, rowIndex, dataset);
                        if (settings.defaultSet) {
                            elem.val(selectValue);
                            elem.attr("data-valuebeforeselected", selectValue);
                            onSelectToField(dataset, rowIndex);
                            $($($(this).parent().parent().parent().parent().parent().children()[0]).children()[0]).focusout();
                        }
                    } else {
                        if (settings.defaultSet) {
                            elem.val(selectValue);
                            elem.attr("data-valuebeforeselected", selectValue);
                            onSelectToField(dataset, rowIndex);
                            $($($(this).parent().parent().parent().parent().parent().children()[0]).children()[0]).focusout();
                        }
                    }

                    setVisible(contraniner, false);
                });
            }

            function onLoad() {
                //Setting Init()
                settings.keySource = elem.data("keysource");
                settings.valueFiled = elem.data("bindfield");
                settings.fieldToInput = elem.data("fieldtoinput");
                settings.extraParam = elem.data("extraparams");
                settings.extraParamByInput = elem.data("paramsctrl");
                settings.setSelectValue = elem.data("setvalue");
                settings.selectedOnly = elem.data("selectedonly");
            }

            function validateKeyCode(keycode) {

                var ret = false;
                switch (keycode) {
                    case keycode.DELETE:
                        ret = true;
                        break;
                    case keycode.BACKSPACE:
                        ret = true;
                        break;
                    default:
                        ret = false;
                        break;
                }

                return ret;
            }

            function onSelectToField(dataset, rowIndex) {
                if (settings.fieldToInput != undefined && settings.fieldToInput != null) {
                    $.each(settings.fieldToInput, function (field, input) {
                        var value = dataset[rowIndex][field];
                        var inputs = input.split(',');
                        $.each(inputs, function (i, elem) {
                            if (!$.isNullOrEmpty(elem)) {
                                $("[name=" + elem.trim() + "]").val(value);
                            }
                        });
                    });
                }
            }

            function setVisible(contraniner, visble) {
                if (visble) {
                    //contraniner.show();
                    contraniner.fadeIn("slow");


                } else {
                    //contraniner.hide();
                    contraniner.fadeOut("slow");
                }
            }

            function getData(id, searchTerm) {
                $.ajax({
                    url: settings.url,
                    dataType: settings.datatype,
                    beforeSend: function () {
                        parent.find('.ace-icon').removeClass('fa-search').addClass('fa-spinner fa-spin');
                    },
                    data: {
                        keySource: settings.keySource,
                        sort: settings.sort,
                        rows: settings.rows,
                        pageIndex: settings.pageIndex,
                        searchTerm: searchTerm,
                        extraParam: createParams()
                    },
                    success: function (data) {
                        parent.find('.ace-icon').removeClass('fa-spinner fa-spin').addClass('fa-search');
                        if (data.rows == undefined) {
                            //alert("Data Not Found.");
                            return;
                        }
                        settings.recordSet = data.rows;
                        var htmlresult = CreateUI(data.colModel, data.rows, newId);
                        contraniner.html(htmlresult);
                        setVisible(contraniner, true);
                        createPager(contraniner, data);
                    },
                    error: function (request, status, error) {
                        if (request.status == 440 || request.responseText.indexOf('The provided anti-forgery token') != -1) {//440 Login Timeout
                            var rootPath = $("#hdSysRootPath").val();
                            if (rootPath == undefined || rootPath == null || rootPath == "/") {
                                rootPath = "/";
                            }
                            window.location.href = rootPath + "Users/Account/SignOut";
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
                    }
                });
            }

            function createParams() {

                var objParam = {};

                if (settings.extraParam != null) {
                    objParam = settings.extraParam;
                }
                if (settings.extraParamByInput != null) {
                    var inputs = settings.extraParamByInput.split(',');
                    $.each(inputs, function (index, ctrkId) {
                        var value = $("#" + ctrkId).val();
                        objParam[ctrkId] = value;
                    });
                }

                return JSON.stringify(objParam);
            }

            function createPager(contraniner, data) {
                if (data == undefined && data == null) {
                    //alert("data is undefined");
                    return;
                }

                contraniner.append('<div class="row"> <div class="col-sm-6"><div class="auto-totalcout pull-left"> TotalCount: ' + data.totalcount + ' Records. </div></div>  <div class="col-sm-6"><div id="' + pagerId + '" class="pull-right auto-pager"></div></div></div>');
                var totalpager = Math.ceil(data.totalcount / settings.pageSize);
                var setPage = data.pageIndex + 1;

                $('#' + pagerId).bootpag({
                    total: totalpager, //todo set total page Math.ceil(x)
                    page: setPage, //todo set current page
                    maxVisible: 5,
                    leaps: true,
                    firstLastUse: false,
                    first: '←',
                    last: '→',
                    wrapClass: 'pagination',
                    activeClass: 'active',
                    disabledClass: 'disabled',
                    nextClass: 'next',
                    prevClass: 'prev',
                    lastClass: 'last',
                    firstClass: 'first'
                }).on("page", function (event, /* page number here */ num) {
                    //Add onPager Event Handeller
                    if (options.onPager != undefined) {
                        options.onPager(event, num);
                    }
                    //alert(num);
                    settings.pageIndex = num - 1;
                    getData(contraniner, settings.searchTerm);
                });
            }

            function CreateUI(colModel, dataRows, id) {
                var objcolModel = colModel;
                var objData = dataRows;

                var tbResult = '<div class="table-responsive"><table id="' + id + '_table" class="table table-striped table-bordered table-hover no-margin-bottom no-border-top table-autocomplete" data-tabletype="autocomplete">';

                var thead = '<thead><tr>';
                $.each(objcolModel, function (key, value) {
                    thead += '<th>' + value.HeaderName + '</th>';
                });
                thead += "</tr></thead>";

                var tbody = "<tbody>";
                var tr = '';
                var rowIndex = 0;
                $.each(objData, function (key, value) {
                    tr += '<tr class="ui-row-autocomplete" data-rowindex="' + rowIndex + '">';
                    var td = '';
                    $.each(objcolModel, function (index, data) {
                        if ($.isNullOrEmpty(data.COLUMNFORMAT) || data.COLUMNFORMAT == "N") {
                            td += '<td class="ui-col-autocomplete" data-colname="' + data.ColumnName + '">' + value[data.ColumnName] + '</td>';
                        }
                        else if (data.COLUMNFORMAT.indexOf("#0")) {
                            td += '<td class="ui-col-autocomplete" align="right" data-colname="' + data.ColumnName + '">' + toNumberFormat(value[data.ColumnName], data.COLUMNFORMAT, true) + '</td>';
                        }
                        else {
                            td += '<td class="ui-col-autocomplete" data-colname="' + data.ColumnName + '">' + value[data.ColumnName] + '</td>';
                        }
                    });
                    tr += td + '</tr>';
                    rowIndex++;
                });
                tbody += tr + '</tbody>';
                tbResult += thead + tbody + '</table></div>';

                return tbResult;
            }

        });
    };

    $.fn.JPager = function (options) {
        var defaults = {
            color: "Red"
        };
        var settings = $.extend({}, defaults, options);
        return this.each(function () {
            // Plugin code would go here...

            var obj = $(".pager-goto").detach();
            var elem = $(this);
            var x = obj.wrap('<p/>').parent().html();
            elem.append("<li>" + x + "</li>");

            $(".btn-pager-goto").on('click', function (e) {
                var $ctrl = $(this);
                var url = $ctrl.data("url");
                var page = $ctrl.parent().prev().val();
                if (url.toLowerCase().indexOf("?") >= 0) {
                    location = url + "&page=" + page;
                } else {
                    location = url + "?page=" + page;
                }

            });
            //$obj.append(elem.append("<li></li>"));

        });
    }

    $.fn.Setcolor = function (options) {
        var defaults = {
            color: "Red"
        };
        var settings = $.extend({}, defaults, options);

        return this.each(function () {
            // Plugin code would go here...
            var elem = $(this);
            elem.css("color", settings.color);
        });
    };

    $.fn.JDatepicker = function (options) {
        var defaults = {
            color: "Red"
        };
        var settings = $.extend({}, defaults, options);

        return this.each(function () {
            // Plugin code would go here...
            var elem = $(this);
            //var parent = $(this).parent();
            var newHtml = '<div class="input-group">' +
                elem.prop('outerHTML') +
                '<span class="input-group-addon"><i class="ace-icon fa fa-calendar"></i></span> </div>';
            //parent.append(newHtml);
            elem.after(newHtml);
            elem.detach();
        });
    };

    $.fn.serializeArray = function () {
        var rselectTextarea = /^(?:select|textarea)/i;
        var rinput = /^(?:color|date|datetime|datetime-local|email|hidden|month|number|password|range|search|tel|text|time|url|week)$/i;
        var rCRLF = /\r?\n/g;

        return this.map(function () {
            return this.elements ? jQuery.makeArray(this.elements) : this;
        }).filter(function () {
            return this.name && (this.checked || rselectTextarea.test(this.nodeName) || rinput.test(this.type) || this.type == "checkbox");
        }).map(function (i, elem) {
            var val = jQuery(this).val();
            if (this.type == 'checkbox') {
                if (this.checked) {
                    val = 'Y';
                }
                else {
                    val = 'N';
                }
            }
            else if (jQuery(this).hasClass("number-format")) {
                val = val.replace(/\,/g, '');
            }
            return val == null ? null : jQuery.isArray(val) ? jQuery.map(val, function (val, i) {
                return {
                    name: elem.name,
                    value: val
                };
            }) : {
                name: elem.name,
                value: val
            };
        }).get();
    };

    $.stringToDate = function (value, format) {
        var date = new Date();
        if (format != undefined && format != null && format != "") {
            date = Globalize.parseDate(value, format, "en-US");
        }
        else {
            date = Globalize.parseDate(value, "dd/MM/yyyy", "en-US");
            if (date == undefined || date == null) {
                date = Globalize.parseDate(value, "dd/MM/yyyy HH:mm:ss", "en-US");
            }
        }
        return date;
    };

    $.fn.serializeObject = function () {
        var result = {};
        var extend = function (i, element) {
            var node = result[element.name];

            // If node with same name exists already, need to convert it to an array as it
            // is a multi-value field (i.e., checkboxes)
            var item = $("[name='" + element.name + "']");
            if (item.hasClass("datepicker-input") && element.value != "") {
                var date = $.stringToDate(element.value);
                if (date != undefined && date != null) {
                    element.value = date.toISOString();
                }
            }
            if ('undefined' !== typeof node && node !== null) {
                if ($.isArray(node)) {
                    node.push(element.value);
                } else {
                    result[element.name] = [node, element.value];
                }
            } else {
                result[element.name] = element.value;
            }
        };

        $.each(this.serializeArray(), extend);
        var table = this.find("table[id][data-tabletype=multiselect],table[id][data-tabletype=detail],table[id][data-tabletype=editable]");
        if (table.length > 0) {
            $.each(table, function (i, item) {
                var tb = $(item);
                var tabletype = tb.data("tabletype");
                var selectedOnly = tb.data("selectedonly");
                var tbId = item.id;
                var tbName = tbId.replace("Grid", "");
                if (window[tbId] != undefined) {
                    var data = [];
                    if (selectedOnly) {
                        data = window[tbId].rows({ selected: true }).data().toArray();
                    }
                    else {
                        data = window[tbId].rows().data().toArray();
                    }
                    if (data.length > 0) {
                        result[tbName] = [];
                        var col = tb.data("col");
                        var colDate = tb.data("coldate");
                        var colDates = [];
                        var colFile = tb.data("colfile");
                        var colFiles = [];
                        var notCol = tb.data("notcol");
                        var notCols = [];
                        var fileColDates = ["FILE_DATE", "CRET_DATE"];
                        if (!$.isNullOrEmpty(colDate)) {
                            colDates = colDate.split(",");
                        }
                        if ($.inArray("CRET_DATE", colDates) == -1) {
                            colDates.push("CRET_DATE");
                        }
                        if ($.inArray("MNT_DATE", colDates) == -1) {
                            colDates.push("MNT_DATE");
                        }

                        if (!$.isNullOrEmpty(colFile)) {
                            colFiles = colFile.split(",");
                        }

                        if (!$.isNullOrEmpty(notCol)) {
                            notCols = notCol.split(",");
                        }

                        if (!$.isNullOrEmpty(col) && tabletype == "multiselect") {
                            var colPKs = col.split(",");
                            $.each(data, function (i, item) {
                                var newItem = {};
                                $.each(colPKs, function (j, colName) {
                                    colName = colName.trim();
                                    if (item[colName] != null && item[colName] != "") {
                                        if ($.inArray(colName, colDates) != -1) {
                                            newItem[colName] = $.gridDateToISOString(item[colName]);
                                        }
                                        else {
                                            newItem[colName] = item[colName];
                                        }
                                    }
                                });
                                result[tbName].push(newItem);
                            });
                        }
                        else {
                            $.each(data, function (i, item) {
                                var newItem = {};
                                $.each(item, function (key, value) {
                                    if ($.inArray(key, notCols) == -1) {
                                        if (!$.isNullOrEmpty(value)) {
                                            if ($.isArray(value) && $.inArray(key, colFiles) != -1) {
                                                var newValues = [];
                                                $.each(value, function (j, jitem) {
                                                    var newJitem = {};
                                                    $.each(jitem, function (jkey, jvalue) {
                                                        if (!$.isNullOrEmpty(jvalue)) {
                                                            if ($.inArray(jkey, fileColDates) != -1) {
                                                                newJitem[jkey] = $.gridDateToISOString(jvalue);
                                                            }
                                                            else {
                                                                newJitem[jkey] = jvalue;
                                                            }
                                                        }
                                                    });
                                                    newValues.push(newJitem);
                                                });
                                                newItem[key] = newJitem;
                                            }
                                            else {
                                                if ($.inArray(key, colDates) != -1) {
                                                    newItem[key] = $.gridDateToISOString(value);
                                                }
                                                else {
                                                    newItem[key] = value;
                                                }
                                            }
                                        }
                                    }
                                });
                                result[tbName].push(newItem);
                            });
                        }
                    }
                }
            });
        }
        return result;
    };

    $.fn.serializeObjectGrid = function (result) {
        if (result == undefined || result == null) {
            result = {};
        }

        var addNode = function (name, value) {
            var node = result[name];
            if ('undefined' !== typeof node && node !== null) {
                if ($.isArray(node)) {
                    node.push(value);
                } else {
                    result[name] = [node, value];
                }
            } else {
                result[name] = value;
            }
        };

        var extend = function (i, element) {

            // If node with same name exists already, need to convert it to an array as it
            // is a multi-value field (i.e., checkboxes)
            var item = $("[name='" + element.name + "']");
            if (item.hasClass("datepicker-input") && element.value != "") {
                var date = $.stringToDate(element.value);
                if (date != undefined && date != null) {
                    element.value = "/Date(" + date.getTime() + ")/";
                }
            }
            addNode(element.name, element.value);
            if (item.get(0).tagName.toLowerCase() == 'select') {
                addNode(element.name + "_TEXT", $("#" + element.name + " option:selected").text());
            }
        };

        $.each(this.serializeArray(), extend);
        return result;
    };

    $.fn.serializeRowToObjectGrid = function () {
        var result = {};

        var addNode = function (name, value) {
            var node = result[name];
            if ('undefined' !== typeof node && node !== null) {
                if ($.isArray(node)) {
                    node.push(value);
                } else {
                    result[name] = [node, value];
                }
            } else {
                result[name] = value;
            }
        };

        var extend = function (i, element) {

            // If node with same name exists already, need to convert it to an array as it
            // is a multi-value field (i.e., checkboxes)
            var item = $(element);
            var val = item.val();
            if (element.type == 'checkbox') {
                if (this.checked) {
                    val = 'Y';
                }
                else {
                    val = 'N';
                }
            }
            else if (element.type == 'file') {
                var gridName = element.id.substring(1, element.id.length);
                val = window['Grid' + gridName].rows().data().toArray();
            }
            else if (item.hasClass("number-format")) {
                val = val.replace(/\,/g, '');
            }
            else if (item.hasClass("datepicker-input")) {
                var date = $.stringToDate(val);
                if (date != undefined && date != null) {
                    val = "/Date(" + date.getTime() + ")/";
                }
            }
            addNode(element.name, val);
            if (element.tagName.toLowerCase() == 'select') {
                var selectedText = '';
                if (!$.isNullOrEmpty(val)) {
                    selectedText = $("option:selected", item).text();
                }
                addNode(element.name + "_TEXT", selectedText);
            }
        };
        var rselectTextarea = /^(?:select|textarea)/i;
        var rinput = /^(?:color|date|datetime|datetime-local|email|hidden|month|number|password|range|search|tel|text|time|url|week|file)$/i;
        var rCRLF = /\r?\n/g;
        var elems = $("input,select,textarea", this).not($("input,select,textarea", $('.dataTables_wrapper', this))).filter(function () {
            return this.name && !this.disabled && (this.checked || rselectTextarea.test(this.nodeName) || rinput.test(this.type) || this.type == "checkbox");
        });

        $.each(elems, extend);
        return result;
    };

    $.fn.serializeFormData = function () {
        var formData = new FormData();

        var extend = function (i, element) {
            //var node = result[element.name];
            var item = $("[name='" + element.name + "']");
            if (item.hasClass("datepicker-input") && element.value != "") {
                var date = $.stringToDate(element.value);
                if (date != undefined || date != null) {
                    formData.append(element.name, date.toISOString());
                }
            }
            else {
                formData.append(element.name, element.value);
            }
        };

        $.each(this.serializeArray(), extend);
        var enctype = this.attr("enctype");
        if (enctype != undefined && enctype != null) {
            var $form = this;

            var allFile = $form.find('[type=file]').not('table[data-tabletype=editable][type=file]');
            var fileCtrlName = [];
            $.each(allFile, function (i, elm) {
                var exist = false;
                $.each(fileCtrlName, function (i, element) {
                    if (element.name == elm.name) {
                        exist = true;
                        return false;
                    }
                });

                if (!exist && elm.name != "") {
                    var obj = {
                        name: elm.name,
                        vsmctrl: false,
                        multi: false,
                        grid: false
                    };
                    var vsmctrl = $(elm).data("vsmctrl");
                    var multiple = $(elm).data("multiple");
                    var grid = $(elm).data("grid");
                    if (multiple || grid) {
                        obj.multi = true;
                    }
                    if (grid) {
                        obj.grid = true;
                    }
                    if (vsmctrl) {
                        obj.vsmctrl = true;
                    }
                    fileCtrlName.push(obj);
                }
            });

            $.each(fileCtrlName, function (i, ctrlName) {
                if (ctrlName.grid) {
                    var ctrl = $form.find('[name=' + ctrlName.name + ']');
                    $.each(ctrl, function (idx, file) {
                        if ($(file).val() != "") {
                            if (ctrlName.grid) {
                                var gridIndex = $(file).data("index");
                                formData.append(ctrlName.name + '[' + idx + '].File', file.files[0]);
                                formData.append(ctrlName.name + '[' + idx + '].ROW_ID', gridIndex);
                            }
                        }
                    });
                }
                else if (ctrlName.vsmctrl) {
                    var data = window['Grid' + ctrlName.name].rows().data().toArray();
                    var dateElem = ["FILE_DATE", "CRET_DATE"];
                    var idx = 0
                    $.each(data, function (i, item) {
                        var fileInfo = {}
                        $.each(item, function (key, value) {
                            if (value != null && value != "") {
                                if ($.inArray(key, dateElem) != -1) {
                                    var date = $.gridDateToISOString(value);
                                    if (date != undefined && date != null && date != "") {
                                        formData.append(ctrlName.name + "[" + idx + "]." + key, date);
                                    }
                                }
                                else {
                                    formData.append(ctrlName.name + "[" + idx + "]." + key, value);
                                }
                            }

                        });
                        idx++;
                    });
                }
                else {
                    var ctrl = $form.find('[name=' + ctrlName.name + ']');
                    $.each(ctrl, function (idx, file) {
                        formData.append(ctrlName.name, file.files[0]);
                    });
                }
            });
        }

        var table = this.find("table[id][data-tabletype=multiselect],table[id][data-tabletype=detail],table[id][data-tabletype=editable]");
        if (table.length > 0) {
            $.each(table, function (i, item) {
                var tb = $(item);
                var tabletype = tb.data("tabletype");
                var selectedOnly = tb.data("selectedonly");
                var tbId = item.id;
                var tbName = tbId.replace("Grid", "");
                if (window[tbId] != undefined) {
                    var data = [];
                    if (selectedOnly) {
                        data = window[tbId].rows({ selected: true }).data().toArray();
                    }
                    else {
                        data = window[tbId].rows().data().toArray();
                    }
                    if (data.length > 0) {
                        var col = tb.data("col");
                        var colDate = tb.data("coldate");
                        var colDates = [];
                        var colFile = tb.data("colfile");
                        var colFiles = [];
                        var notCol = tb.data("notcol");
                        var notCols = [];
                        var fileColDates = ["FILE_DATE", "CRET_DATE"];
                        if (!$.isNullOrEmpty(colDate)) {
                            colDates = colDate.split(",");
                        }
                        if ($.inArray("CRET_DATE", colDates) == -1) {
                            colDates.push("CRET_DATE");
                        }
                        if ($.inArray("MNT_DATE", colDates) == -1) {
                            colDates.push("MNT_DATE");
                        }

                        if (!$.isNullOrEmpty(colFile)) {
                            colFiles = colFile.split(",");
                        }

                        if (!$.isNullOrEmpty(notCol)) {
                            notCols = notCol.split(",");
                        }

                        if (!$.isNullOrEmpty(col) && tabletype == "multiselect") {
                            var colPKs = col.split(",");
                            $.each(data, function (i, item) {
                                $.each(colPKs, function (j, colName) {
                                    if (item[colName] != null && item[colName] != "") {
                                        if ($.inArray(colName, colDates) != -1) {
                                            formData.append(tbName + "[" + i + "]." + colName, $.gridDateToISOString(item[colName]));
                                        }
                                        else {
                                            formData.append(tbName + "[" + i + "]." + colName, item[colName]);
                                        }
                                    }

                                });
                            });
                        }
                        else {
                            $.each(data, function (i, item) {
                                $.each(item, function (key, value) {
                                    if ($.inArray(key, notCols) == -1) {
                                        if (!$.isNullOrEmpty(value)) {
                                            if ($.isArray(value) && $.inArray(key, colFiles) != -1) {
                                                $.each(value, function (j, jitem) {
                                                    $.each(jitem, function (jkey, jvalue) {
                                                        if (!$.isNullOrEmpty(jvalue)) {
                                                            if ($.inArray(jkey, fileColDates) != -1) {
                                                                formData.append(tbName + "[" + i + "]." + key + "[" + j + "]." + jkey, $.gridDateToISOString(jvalue));
                                                            }
                                                            else {
                                                                formData.append(tbName + "[" + i + "]." + key + "[" + j + "]." + jkey, jvalue);
                                                            }
                                                        }
                                                    });
                                                });
                                            }
                                            else if ($.isArray(value)) {
                                                $.each(value, function (j, jvalue) {
                                                    if (!$.isNullOrEmpty(jvalue)) {
                                                        formData.append(tbName + "[" + i + "]." + key + "[]", jvalue);
                                                    }
                                                });
                                            }
                                            else {
                                                if ($.inArray(key, colDates) != -1) {
                                                    formData.append(tbName + "[" + i + "]." + key, $.gridDateToISOString(value));
                                                }
                                                else {
                                                    formData.append(tbName + "[" + i + "]." + key, value);
                                                }
                                            }
                                        }
                                    }
                                });
                            });
                        }
                    }
                }
            });
        }
        return formData;
    };

    $.fn.serializeFormDataWithOutTable = function () {
        var formData = new FormData();

        var extend = function (i, element) {
            //var node = result[element.name];
            var item = $("[name='" + element.name + "']");
            if (item.hasClass("datepicker-input") && element.value != "") {
                var date = $.stringToDate(element.value);
                if (date != undefined || date != null) {
                    formData.append(element.name, date.toISOString());
                }
            }
            else {
                formData.append(element.name, element.value);
            }
        };

        $.each(this.serializeArray(), extend);
        var enctype = this.attr("enctype");
        if (enctype != undefined && enctype != null) {
            var $form = this;

            var allFile = $form.find('[type=file]');
            var fileCtrlName = [];
            $.each(allFile, function (i, elm) {
                var exist = false;
                $.each(fileCtrlName, function (i, element) {
                    if (element.name == elm.name) {
                        exist = true;
                        return false;
                    }
                });

                if (!exist && elm.name != "") {
                    var obj = {
                        name: elm.name,
                        vsmctrl: false,
                        multi: false,
                        grid: false
                    };
                    var vsmctrl = $(elm).data("vsmctrl");
                    var multiple = $(elm).data("multiple");
                    var grid = $(elm).data("grid");
                    if (multiple || grid) {
                        obj.multi = true;
                    }
                    if (grid) {
                        obj.grid = true;
                    }
                    if (vsmctrl) {
                        obj.vsmctrl = true;
                    }
                    fileCtrlName.push(obj);
                }
            });

            $.each(fileCtrlName, function (i, ctrlName) {
                if (ctrlName.grid) {
                    var ctrl = $form.find('[name=' + ctrlName.name + ']');
                    $.each(ctrl, function (idx, file) {
                        if ($(file).val() != "") {
                            if (ctrlName.grid) {
                                var gridIndex = $(file).data("index");
                                formData.append(ctrlName.name + '[' + idx + '].File', file.files[0]);
                                formData.append(ctrlName.name + '[' + idx + '].ROW_ID', gridIndex);
                            }
                        }
                    });
                }
                else if (ctrlName.vsmctrl) {
                    var data = window['Grid' + ctrlName.name].rows().data().toArray();
                    var dateElem = ["FILE_DATE", "CRET_DATE"];
                    var idx = 0
                    $.each(data, function (i, item) {
                        var fileInfo = {}
                        $.each(item, function (key, value) {
                            if (value != null && value != "") {
                                if ($.inArray(key, dateElem) != -1) {
                                    formData.append(ctrlName.name + "[" + idx + "]." + key, $.gridDateToISOString(value));
                                }
                                else {
                                    formData.append(ctrlName.name + "[" + idx + "]." + key, value);
                                }
                            }

                        });
                        idx++;
                    });
                }
                else {
                    var ctrl = $form.find('[name=' + ctrlName.name + ']');
                    $.each(ctrl, function (idx, file) {
                        formData.append(ctrlName.name, file.files[0]);
                    });
                }
            });
        }

        return formData;
    };

    $.fn.serializeFormDataWithCer = function (fP12, pwP12, callback) {
        var formData = new FormData();

        var extend = function (i, element) {
            //var node = result[element.name];
            var item = $("[name='" + element.name + "']");
            if (item.hasClass("datepicker-input") && element.value != "") {
                var date = $.stringToDate(element.value);
                if (date != undefined || date != null) {
                    formData.append(element.name, date.toISOString());
                }
            }
            else {
                formData.append(element.name, element.value);
            }
        };

        $.each(this.serializeArray(), extend);

        var fileAll = 0;
        var fileCount = 0;

        var form = this;

        function dataBufferToStringCompleted(algorname, privateKey, dataName, dataFile, dataIndex, multi, grid, gridIndex, data) {

            var signature;
            if (algorname == 'SHA-1') {
                var md = forge.md.sha1.create();
                md.update(data);
                signature = privateKey.sign(md);
            }
            else if (algorname == 'SHA-256') {
                var md = forge.md.sha256.create();
                md.update(data);
                signature = privateKey.sign(md);
            }
            else if (algorname == 'SHA-384') {
                var md = forge.md.sha384.create();
                md.update(data);
                signature = privateKey.sign(md);
            }
            else if (algorname == 'SHA-512') {
                var md = forge.md.sha512.create();
                md.update(data);
                signature = privateKey.sign(md);
            }

            if (signature != undefined && signature != null && signature != "") {
                var eHex = forge.util.bytesToHex(signature);
                if (multi) {
                    formData.append(dataName + "[" + dataIndex + "].File", dataFile);
                    formData.append(dataName + "[" + dataIndex + "].SIGNATURE_SIGN", eHex);
                    if (grid) {
                        formData.append(dataName + "[" + dataIndex + "].ROW_ID", gridIndex);
                    }
                }
                else {
                    formData.append(dataName + ".File", dataFile);
                    formData.append(dataName + ".SIGNATURE_SIGN", eHex);
                }
            }
            fileCount++;
            if (fileAll == fileCount) {
                callback(true, formData);
            }
        }

        function dataBufferToString(dataBuffer, algorname, privateKey, dataName, datafile, dataIndex, multi, grid, gridIndex) {
            var binary = '';
            var bytes = new Uint8Array(dataBuffer);
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            dataBufferToStringCompleted(algorname, privateKey, dataName, datafile, dataIndex, multi, grid, gridIndex, binary);
        }

        function p12BufferToStringCompleted(p12) {
            try {
                var privateKey;
                var pkcs12Asn1 = forge.asn1.fromDer(p12);

                var pkcs12 = forge.pkcs12.pkcs12FromAsn1(pkcs12Asn1, false, pwP12);
                // load keys

                var bags = pkcs12.getBags({ bagType: forge.pki.oids.certBag });
                // bags are key'd by bagType and each bagType key's value
                // is an array of matches (in this case, certificate objects)
                var cert = bags[forge.pki.oids.certBag][0];


                //Find algorithm name from P12 file
                var algorname;
                if (cert.cert.md.algorithm.indexOf('sha1') !== -1) {
                    algorname = 'SHA-1'
                }

                else if (cert.cert.md.algorithm.indexOf('sha256') !== -1) {
                    algorname = 'SHA-256'
                }

                else if (cert.cert.md.algorithm.indexOf('sha384') !== -1) {
                    algorname = 'SHA-384'
                }

                else if (cert.cert.md.algorithm.indexOf('sha512') !== -1) {
                    algorname = 'SHA-512'
                }
                else {
                    $.alert({
                        title: null,
                        content: 'Algorithm not match !!!',
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                    return false;
                }

                //Check date not expire
                var CertnotAfter = cert.cert.validity.notAfter;
                if (CertnotAfter <= (new Date(Date.now()))) {
                    $.alert({
                        title: null,
                        content: 'ใบรับรองหมดอายุ',
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                    return false;
                }

                // get key bags
                var bagsKey = pkcs12.getBags({ bagType: forge.pki.oids.keyBag });
                // get key
                var bagKey = bagsKey[forge.pki.oids.keyBag][0];
                if (bagKey != undefined && bagKey.key != null) {
                    privateKey = bagKey.key;
                }
                else {
                    // get key bags
                    bagsKey = pkcs12.getBags({ bagType: forge.pki.oids.pkcs8ShroudedKeyBag });
                    // get key
                    bagKey = bagsKey[forge.pki.oids.pkcs8ShroudedKeyBag][0];
                    privateKey = bagKey.key;
                    // can now convert back to DER/PEM/etc for export
                }

                var allFile = form.find('[type=file]');
                var fileCtrlName = [];
                $.each(allFile, function (i, elm) {
                    var exist = false;
                    $.each(fileCtrlName, function (i, element) {
                        if (element.name == elm.name) {
                            exist = true;
                            return false;
                        }
                    });

                    if (!exist && elm.name != "") {
                        var obj = {
                            name: elm.name,
                            multi: false,
                            grid: false
                        };
                        var multiple = $(elm).data("multiple");
                        var grid = $(elm).data("grid");
                        if (multiple || grid) {
                            obj.multi = true;
                        }
                        if (grid) {
                            obj.grid = true;
                        }
                        fileCtrlName.push(obj);
                    }
                });

                $.each(fileCtrlName, function (i, ctrlName) {
                    var ctrl = form.find('[name=' + ctrlName.name + ']');
                    $.each(ctrl, function (idx, file) {
                        if ($(file).val() != "" && file.files.length > 0) {
                            fileAll++;
                            var gridIndex;
                            if (ctrlName.grid) {
                                gridIndex = $(file).data("index");
                            }
                            var readerData = new FileReader();
                            readerData.onload = function (e) {
                                dataBufferToString(e.target.result, algorname, privateKey, ctrlName.name, file.files[0], idx, ctrlName.multi, ctrlName.grid, gridIndex);
                            };
                            readerData.readAsArrayBuffer(file.files[0]);
                        }
                    });
                });
            } catch (e) {
                if (e.message.indexOf('Invalid password') != -1) {
                    $.alert({
                        title: null,
                        content: 'ใบรับรอง หรือ รหัสผ่านไม่ถูกต้อง',
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                } else {
                    $.alert({
                        title: null,
                        content: e.message,
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                }
            }
        }

        function p12BufferToString(p12Buffer) {
            var binary = '';
            var bytes = new Uint8Array(p12Buffer);
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            p12BufferToStringCompleted(binary);
        }

        var file = fP12[0].files[0];
        var reader = new FileReader();
        reader.onload = function (e) {
            p12BufferToString(e.target.result);
        }
        reader.readAsArrayBuffer(file);

        return false;
    };

    $.fn.serializeFormDataWithCerServer = function (fP12, pwP12, callback) {
        var formData = new FormData();

        var extend = function (i, element) {
            //var node = result[element.name];
            var item = $("[name='" + element.name + "']");
            if (item.hasClass("datepicker-input") && element.value != "") {
                var date = $.stringToDate(element.value);
                if (date != undefined || date != null) {
                    formData.append(element.name, date.toISOString());
                }
            }
            else {
                formData.append(element.name, element.value);
            }
        };

        $.each(this.serializeArray(), extend);

        var fileAll = 0;
        var fileCount = 0;

        var form = this;

        function p12BufferToStringCompleted(p12) {
            try {
                var privateKey;
                var pkcs12Asn1 = forge.asn1.fromDer(p12);

                var pkcs12 = forge.pkcs12.pkcs12FromAsn1(pkcs12Asn1, false, pwP12);
                // load keys

                var bags = pkcs12.getBags({ bagType: forge.pki.oids.certBag });
                // bags are key'd by bagType and each bagType key's value
                // is an array of matches (in this case, certificate objects)
                var certs = bags[forge.pki.oids.certBag];
                var notBefore = new Date(0);
                var cerExpired = false;
                var serialNumber = '';
                var cert;
                $.each(certs, function (i, item) {
                    if (item.cert.validity.notBefore.getTime() >= notBefore.getTime() && item.cert.validity.notAfter.getTime() > (new Date(Date.now())).getTime()) {
                        serialNumber = item.cert.serialNumber;
                        notBefore = item.cert.validity.notBefore;
                        cert = item;
                        cerExpired = false;
                    }
                    else if (item.cert.validity.notBefore.getTime() >= notBefore.getTime() && item.cert.validity.notAfter.getTime() <= (new Date(Date.now())).getTime()) {
                        cerExpired = true;
                    }
                });

                //Check date not expire
                if (cerExpired) {
                    $.alert({
                        title: null,
                        content: 'ใบรับรองหมดอายุ',
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                    return false;
                }

                //Find algorithm name from P12 file
                var algorname;
                if (cert.cert.md.algorithm.indexOf('sha1') !== -1) {
                    algorname = 'SHA-1'
                }
                else if (cert.cert.md.algorithm.indexOf('sha256') !== -1) {
                    algorname = 'SHA-256'
                }

                else if (cert.cert.md.algorithm.indexOf('sha384') !== -1) {
                    algorname = 'SHA-384'
                }

                else if (cert.cert.md.algorithm.indexOf('sha512') !== -1) {
                    algorname = 'SHA-512'
                }
                else {
                    $.alert({
                        title: null,
                        content: 'Algorithm not match !!!',
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                    return false;
                }

                // get key bags
                var bagsKey = pkcs12.getBags({ bagType: forge.pki.oids.keyBag });
                if (bagsKey != undefined && bagsKey != null && bagsKey[forge.pki.oids.keyBag].length > 0) {
                    // get key
                    var bagKey = bagsKey[forge.pki.oids.keyBag][0];
                    privateKey = bagKey.key;
                }

                if (bagsKey == undefined || bagsKey == null || bagsKey[forge.pki.oids.keyBag].length <= 0 || privateKey == null) {
                    // get key bags
                    bagsKey = pkcs12.getBags({ bagType: forge.pki.oids.pkcs8ShroudedKeyBag });
                    // get key
                    bagKey = bagsKey[forge.pki.oids.pkcs8ShroudedKeyBag][0];
                    privateKey = bagKey.key;
                }


                if (privateKey == null) {
                    $.alert({
                        title: null,
                        content: 'ใบรับรองนี้ระบบอาจจะไม่รองรับ<br>กรุณาติดต่อผู้ดูแลระบบ',
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                    return false;
                }
                var lsData = [];
                var rootPath = $("#hdSysRootPath").val();
                if (rootPath == undefined || rootPath == null || rootPath == "/") {
                    rootPath = "/";
                }
                $.ajax({
                    url: rootPath + 'Ux/File/GetFileData',
                    type: 'post',
                    async: false,
                    success: function (result) {
                        if (result.Status) {
                            lsData = result.data;
                        }
                    },
                    error: function (request, status, error) {
                        if (request.status == 440 || request.responseText.indexOf('The provided anti-forgery token') != -1) {//440 Login Timeout
                            window.location.href = rootPath + "Users/Account/SignOut";
                        }
                        else {
                            var errMsg = request.statusText;
                            if (request.responseText != null && request.responseText != "") {
                                errMsg = request.responseText;
                            }
                            $.alert({
                                title: null,
                                content: errMsg,
                                columnClass: 'medium',
                                buttons: {
                                    OK: {
                                        text: $("#hdSysOK").val(),
                                        btnClass: "btn-primary",
                                        action: function () {
                                            callback(false, formData);
                                        }
                                    }
                                }
                            });
                        }
                    }
                });

                $.each(lsData, function (i, obj) {
                    var data = "";
                    $.ajax({
                        url: rootPath + 'Ux/File/GetFileDataById',
                        type: 'post',
                        data: {
                            ID: obj.ID
                        },
                        async: false,
                        success: function (result) {
                            if (result.Status) {
                                data = result.data;
                            }
                        },
                        error: function (request, status, error) {
                            if (request.status == 440 || request.responseText.indexOf('The provided anti-forgery token') != -1) {//440 Login Timeout
                                window.location.href = rootPath + "Users/Account/SignOut";
                            }
                            else {
                                var errMsg = request.statusText;
                                if (request.responseText != null && request.responseText != "") {
                                    errMsg = request.responseText;
                                }
                                $.alert({
                                    title: null,
                                    content: errMsg,
                                    columnClass: 'medium',
                                    buttons: {
                                        OK: {
                                            text: $("#hdSysOK").val(),
                                            btnClass: "btn-primary",
                                            action: function () {
                                                callback(false, formData);
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    });
                    if (data != null) {
                        var signature;
                        if (algorname == 'SHA-1') {
                            var md = forge.md.sha1.create();
                            md.update(forge.util.decode64(data));
                            signature = privateKey.sign(md);
                        }
                        else if (algorname == 'SHA-256') {
                            var md = forge.md.sha256.create();
                            md.update(forge.util.decode64(data));
                            signature = privateKey.sign(md);
                        }
                        else if (algorname == 'SHA-384') {
                            var md = forge.md.sha384.create();
                            md.update(forge.util.decode64(data));
                            signature = privateKey.sign(md);
                        }
                        else if (algorname == 'SHA-512') {
                            var md = forge.md.sha512.create();
                            md.update(forge.util.decode64(data));
                            signature = privateKey.sign(md);
                        }

                        if (signature != undefined && signature != null && signature != "") {
                            var eHex = forge.util.bytesToHex(signature);
                            formData.append(obj.Name + "[" + i + "].SIGNATURE_SIGN", eHex);
                            formData.append(obj.Name + "[" + i + "].ROW_ID", obj.ROW_ID);
                            formData.append(obj.Name + "[" + i + "].CERTIFICATE_NUMBER", serialNumber);
                        }
                    }
                });
                callback(true, formData);
            } catch (e) {
                if (e.message.indexOf('Invalid password') != -1) {
                    $.alert({
                        title: null,
                        content: 'ใบรับรอง หรือ รหัสผ่านไม่ถูกต้อง',
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                } else {
                    $.alert({
                        title: null,
                        content: e.message,
                        columnClass: 'medium',
                        buttons: {
                            OK: {
                                text: $("#hdSysOK").val(),
                                btnClass: "btn-primary",
                                action: function () {
                                    callback(false, formData);
                                }
                            }
                        }
                    });
                }
            }
        }

        function p12BufferToString(p12Buffer) {
            var binary = '';
            var bytes = new Uint8Array(p12Buffer);
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            p12BufferToStringCompleted(binary);
        }

        var file = fP12[0].files[0];
        var reader = new FileReader();
        reader.onload = function (e) {
            p12BufferToString(e.target.result);
        }
        reader.readAsArrayBuffer(file);

        return false;
    };

    $.fn.clearForm = function () {
        var form = $(this);
        //reset unobtrusive validation summary, if it exists
        $(this).find("[data-valmsg-summary=true]")
            .removeClass("validation-summary-errors")
            .addClass("validation-summary-valid")
            .find("ul").empty();

        //reset unobtrusive field level, if it exists
        $(this).find("[data-valmsg-replace]")
            .removeClass("field-validation-error")
            .addClass("field-validation-valid")
            .empty()
            .closest(".form-group")
            .removeClass('has-error has-success');

        return this.each(function () {
            var type = this.type, tag = this.tagName.toLowerCase();
            if (tag == 'form')
                return $(':input', this).clearForm();

            if (type == 'text' || type == 'password' || tag == 'textarea' || tag == 'select' || type == 'hidden') {
                var item = $(this);
                if (item.hasClass("datepicker-input")) {
                    item.datepicker("clearDates");
                }
                if (item.data("valuebeforeselected") != undefined || item.attr("data-valuebeforeselected", "")) {
                    item.data("valuebeforeselected", "");
                    item.attr("data-valuebeforeselected", "");
                }
                this.value = '';
            }
            else if (type == 'checkbox' || type == 'radio') {
                this.value = '';
                this.checked = false;
            }
        });
    };

    $.jsonDateToFormat = function (data, format) {
        if (format == undefined) {
            format = "DD/MM/YYYY";
        }
        var dateFormat = "";
        if (data != null && data != undefined && data != "") {
            var pattern = /Date\(([^)]+)\)/;
            var results = pattern.exec(data);
            if ((results != null && results != undefined && results.length > 0) &&
                (results[1] != null && results[1] != undefined && results[1] != "")) {
                var dt = new Date(parseFloat(results[1]));
                var dtM = moment(dt);
                dateFormat = dtM.format(format);
            }
        }
        return dateFormat;
    };

    $.jsonDateToDate = function (data) {
        var date = null;
        if (data != null && data != undefined && data != "") {
            var pattern = /Date\(([^)]+)\)/;
            var results = pattern.exec(data);
            if ((results != null && results != undefined && results.length > 0) &&
                (results[1] != null && results[1] != undefined && results[1] != "")) {
                date = new Date(parseFloat(results[1]));
            }
        }
        return date;
    };

    $.jsonDateAndTimeToFormat = function (data) {
        var format = 'DD/MM/YYYY';
        var dateFormat = "";
        if (data != null && data != undefined && data != "") {
            var pattern = /Date\(([^)]+)\)/;
            var results = pattern.exec(data);
            if ((results != null && results != undefined && results.length > 0) &&
                (results[1] != null && results[1] != undefined && results[1] != "")) {
                var dt = new Date(parseFloat(results[1]));
                var dtM = moment(dt);

                if (dt.getHours() > 0 || dt.getMinutes() > 0 || dt.getSeconds() > 0) {
                    format = 'DD/MM/YYYY HH:mm:ss';
                }
                dateFormat = dtM.format(format);
            }
        }
        return dateFormat;
    };

    $.gridDateToISOString = function (data) {

        if (data != null && data != undefined && data != "") {
            var pattern = /Date\(([^)]+)\)/;
            var results = pattern.exec(data);
            if ((results != null && results != undefined && results.length > 0) &&
                (results[1] != null && results[1] != undefined && results[1] != "")) {
                var dt = new Date(parseFloat(results[1]));
                return dt.toISOString();
            }
            else {
                return "";
            }
        }
        return data;
    };

    $.arrayObjectToFormData = function (data, formData, toName, dateElements) {
        if (formData == null) {
            formData = new FormData();
        }
        if (dateElements != undefined && dateElements.length > 0) {
            $.each(data, function (i, item) {
                $.each(item, function (key, value) {
                    if (value != undefined && value != null && value != "") {
                        if ($.isArray(value)) {
                            $.each(value, function (j, jvalue) {
                                formData.append(toName + "[" + i + "]." + key + "[]", jvalue);
                            });
                        }
                        else {
                            if ($.inArray(key, dateElements) != -1) {
                                formData.append(toName + "[" + i + "]." + key, $.gridDateToISOString(value));
                            }
                            else {
                                formData.append(toName + "[" + i + "]." + key, value);
                            }
                        }
                    }
                });
            });
        }
        else {
            $.each(data, function (i, item) {
                $.each(item, function (key, value) {
                    if (value != undefined && value != null && value != "") {
                        formData.append(toName + "[" + i + "]." + key, value)
                    }
                });
            });
        }
        return formData;
    };

    $.arrayObjectToArrayObject = function (data, dateElements) {
        if (dateElements != undefined && dateElements.length > 0) {
            $.each(data, function (i, item) {
                $.each(dateElements, function (i, name) {
                    item[name] = $.gridDateToISOString(value);
                });
            });
            return data;
        }
        return data;
    };

    $.stringToGridDate = function (data) {
        if (data != undefined && data != null && data != "") {
            var date = $.stringToDate(data);
            if (date != undefined && date != null) {
                data = "/Date(" + date.getTime() + ")/";
            }
        }
        return data;
    };

    $.fn.vsmConfirm = function (options, option2) {
        if (typeof options === 'undefined') options = {
        };
        if (typeof options === 'string')
            options = {
                content: options,
                title: (option2) ? option2 : false
            };

        /*
         *  Alias of $.confirm to emulate native confirm()
         */
        $(this).each(function () {
            var $this = $(this);
            $this.on('click', function (e) {
                e.preventDefault();

                var textVal = $(this).text();
                if (textVal == undefined || textVal == null || textVal == '') {
                    textVal = this.title;
                    if (textVal == undefined || textVal == null || textVal == '') {
                        textVal = $(this).data('original-title');
                    }
                }
                options['title'] = null;
                options['content'] = 'คุณต้องการ' + textVal.trim() + 'หรือไม่?';
                if ($("#hdSystemLang").val() == "en-US") {
                    options['content'] = "Are you sure to continue?";
                }

                var jcOption = $.extend({}, options);
                if ($this.attr('data-title'))
                    jcOption['title'] = $this.attr('data-title');
                if ($this.attr('data-content'))
                    jcOption['content'] = $this.attr('data-content');
                if (typeof jcOption['buttons'] == 'undefined')
                    jcOption['buttons'] = {
                    };

                jcOption['$target'] = $this;
                if ($this.attr('href') && Object.keys(jcOption['buttons']).length == 0) {
                    var buttons = $.extend(true, {}, jconfirm.pluginDefaults.defaultButtons, (jconfirm.defaults || {}).defaultButtons || {});
                    var firstBtn = Object.keys(buttons)[0];
                    jcOption['buttons'] = buttons;
                    jcOption.buttons[firstBtn].action = function () {
                        location.href = $this.attr('href');
                    };
                }
                jcOption['closeIcon'] = false;
                $.confirm(jcOption);
            });
        });
        return $(this);
    };

    $.fn.hasFile = function () {
        var result = false;
        var file = this.find('[type=file]');
        $.each(file, function (i, item) {
            if ($(item).val() != "") {
                result = true;
                return false;
            }
        });
        return result;
    };

    $.fn.validFile = function () {
        var result = true;
        var enctype = this.attr("enctype");
        if (enctype != undefined && enctype != null) {
            var $form = this;
            var allFile = $form.find('[type=file]').not('table[data-tabletype=editable] [type=file]');
            var fileCtrlName = [];
            $.each(allFile, function (i, elm) {
                var exist = false;
                $.each(fileCtrlName, function (i, element) {
                    if (element.name == elm.name) {
                        exist = true;
                        return false;
                    }
                });

                if (!exist && elm.name != "") {
                    var obj = {
                        name: elm.name,
                        vsmctrl: false,
                        multi: false,
                        grid: false,
                        isrequired: false
                    };
                    obj.vsmctrl = $(elm).data("vsmctrl");
                    obj.multi = $(elm).data("multiple");
                    obj.grid = $(elm).data("grid");
                    obj.isrequired = $(elm).data("isrequired");
                    if (obj.grid) {
                        obj.multi = true;
                    }
                    fileCtrlName.push(obj);
                }
            });

            $.each(fileCtrlName, function (i, ctrlName) {
                if ($("#f" + ctrlName.name).closest("div").has("#validError" + ctrlName.name)) {
                    $("#validError" + ctrlName.name).remove();
                }
                if (ctrlName.vsmctrl && ctrlName.isrequired) {
                    var data = window['Grid' + ctrlName.name].rows().data();
                    if (data.length == 0 || data == undefined) {
                        result = false;
                        var text = "โปรดระบุ";
                        if ($("#hdSystemLang").val() == "en-US") {
                            text = "should not be empty";
                        }
                        $('<span id ="validError' + ctrlName.name + '" class="text-danger">' + text + '</span>').insertAfter("[name=" + ctrlName.name + "]");
                    }
                }
            });
        }
        return result;
    };

    $.fn.validTable = function () {
        var result = true;
        var $form = this;
        var table = this.find("table[id][data-tabletype=multiselect],table[id][data-tabletype=detail],table[id][data-tabletype=editable]");
        if (table.length > 0) {
            $.each(table, function (i, item) {
                if (item.id != null && item.id != "") {
                    var valid = true;
                    var ctrl = $(item);
                    var ctrlName = item.id.replace("Grid", "");
                    var tableType = ctrl.data("tabletype");
                    var isRequired = ctrl.data("isrequired");
                    var selectedOnly = ctrl.data("selectedonly");

                    if ($('#validError' + ctrlName).length > 0) {
                        $('#validError' + ctrlName).remove();
                    }
                    var existRowCtrl = ctrl.hasRowCtrl();
                    if (tableType == "editable" && valid && existRowCtrl) {
                        valid = false;
                        var text = "โปรดบันทึกข้อมูล";
                        if ($("#hdSystemLang").val() == "en-US") {
                            text = "Please Save data!";
                        }
                        if (ctrl.closest('.widget-box').length > 0) {
                            ctrl.closest('.widget-box').addClass('has-error');
                        }
                        var msg = $('<p id ="validError' + ctrlName + '" class="text-danger no-margin">' + text + '</p>');
                        msg.insertBefore(ctrl.closest(".gridIncludeToolbar"));
                    }

                    if (isRequired && valid) {
                        var data = [];
                        if (selectedOnly) {
                            data = window[item.id].rows({ selected: true }).data().toArray();
                        }
                        else {
                            data = window[item.id].rows().data().toArray();
                        }
                        if (data == undefined || data == null || data.length == 0) {
                            valid = false;
                            var text = "โปรดระบุ";
                            if ($("#hdSystemLang").val() == "en-US") {
                                text = "should not be empty";
                            }

                            var msg = $('<p id ="validError' + ctrlName + '" class="text-danger no-margin">' + text + '</p>');
                            if (ctrl.closest(".gridIncludeToolbar").length > 0) {
                                msg.insertBefore(ctrl.closest(".gridIncludeToolbar"));
                            }
                            else {
                                msg.insertBefore(ctrl.closest(".dataTables_wrapper"));
                            }
                            if (ctrl.closest('.widget-box').length > 0) {
                                ctrl.closest('.widget-box').addClass('has-error');
                            }
                        }
                    }

                    result = valid;
                    if (!valid) {
                        return false;
                    }
                }
            });
        }
        return result;
    };

    $.vsmSelectCer = function (options) {
        if (typeof options === 'undefined') options = {
        };
        options["title"] = 'Sign';
        options["content"] = '<form role="form" method="post" class="form-horizontal">' +
            '<div id="notSignFile" style="display: none;"></div>' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<div class="form-group required">' +
            '<label for="fP12" class="control-label col-md-4">Private Key (*.p12)</label>' +
            '<div class="col-md-7">' +
            '<input type="file" name="fP12" id="fP12" class="form-control" accept="application/x-pkcs12">' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<div class="form-group required">' +
            '<label for="pwP12" class="control-label col-md-4">Password</label>' +
            '<div class="col-md-7">' +
            '<input type="password" name="pwP12" id="pwP12" class="form-control" autocomplete="off">' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</form>';

        options["columnClass"] = 'medium';
        options["buttons"] = {
            Sign: {
                text: "Sign File",
                btnClass: "btn-primary",
                action: function () {
                    var formCer = this.$content.find('form');
                    if (formCer.valid()) {
                        var fP12 = this.$content.find('#fP12');
                        var pwP12 = this.$content.find('#pwP12').val();
                        if (typeof this.onSign === 'function') {
                            this.showLoading(true);
                            return this.onSign(fP12, pwP12);
                        }
                    }
                    else {
                        return false;
                    }
                }
            },
            cancel: {
                text: $("#hdSysCancel").val(),
                btnClass: "btn-primary"
            }
        };
        options["onContentReady"] = function () {
            // bind to events
            this.$content.find('#fP12').ace_file_input({
                no_file: 'No File ...',
                btn_choose: 'Choose',
                btn_change: 'Change',
                droppable: false,
                onchange: null,
                thumbnail: false,
                allowExt: ['p12'],
                allowMime: ['application/x-pkcs12']
            });

            var formCer = this.$content.find('form');
            formCer.validate({
                rules: {
                    fP12: {
                        required: true,
                        extension: "p12"
                    },
                    pwP12: {
                        required: true
                    }
                },
                highlight: function (element) {
                    $(element).closest('.form-group').addClass('has-error');
                },
                unhighlight: function (element) {
                    $(element).closest('.form-group').removeClass('has-error');
                },
                errorElement: 'span',
                errorClass: 'help-block',
                errorPlacement: function (error, element) {
                    if (element[0].name == "fP12") {
                        element.closest("div").append(error);
                    }
                    else {
                        if (element.parent('.input-group').length) {
                            error.insertAfter(element.parent());
                        } else {
                            error.insertAfter(element);
                        }
                    }
                }
            });

            formCer.on('keyup keypress', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });
        };

        options["onClose"] = function () {
            var fP12 = this.$content.find('#fP12');
            fP12.replaceWith($('<input type="file" name="fP12" id="fP12" class="form-control" accept="application/x-pkcs12">'));
            this.$content.find('#pwP12').val("");
        };

        return $.confirm(options);
    };

    $.hasFileInServer = function () {
        var isValid = false;
        var rootPath = $("#hdSysRootPath").val();
        if (rootPath == undefined || rootPath == null || rootPath == "/") {
            rootPath = "/";
        }
        $.ajax({
            url: rootPath + 'Ux/File/CheckHasFile',
            type: 'get',
            async: false,
            success: function (result) {
                isValid = result.Status;
            },
            error: function (request, status, error) {
                if (request.status == 440 || request.responseText.indexOf('The provided anti-forgery token') != -1) {//440 Login Timeout
                    var rootPath = $("#hdSysRootPath").val();
                    if (rootPath == undefined || rootPath == null || rootPath == "/") {
                        rootPath = "/";
                    }
                    window.location.href = rootPath + "Users/Account/SignOut";
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
            }
        });
        return isValid;
    };

    $.isNullOrEmpty = function (data) {
        var result = false;
        if (data == undefined ||
            data == null ||
            (typeof (data) == 'string' && data == '') ||
            ($.isArray(data) && data.length == 0)) {
            result = true;
        }
        return result;
    };

    $.toUrlString = function (data) {
        var nData = "";
        if (data != undefined && data != null && data != "") {
            nData = encodeURIComponent(data.toString());
        }
        return nData;
    };

    $.fn.validTableRow = function () {
        var result = true;
        var $row = this;
        var extend = function (i, element) {
            var valid = true;
            // If node with same name exists already, need to convert it to an array as it
            // is a multi-value field (i.e., checkboxes)
            var item = $(element);
            var isrequired = item.data('isrequired');
            var validIdCard = item.data('valididcard');

            if (element.type == 'file') {
                var gridName = element.id.substring(1, element.id.length);
                if ($('#validError' + gridName, $row).length > 0) {
                    $('#validError' + gridName, $row).remove();
                }
            }
            else {
                if ($('#validError' + element.name, $row).length > 0) {
                    $('#validError' + element.name, $row).remove();
                }
            }

            if (isrequired) {
                var text = "โปรดระบุ";
                if ($("#hdSystemLang").val() == "en-US") {
                    text = "should not be empty";
                }
                if (element.type == 'file') {
                    var gridName = element.id.substring(1, element.id.length);
                    var gridFile = window['Grid' + gridName];
                    if (gridFile != undefined) {
                        var data = window['Grid' + gridName].rows().data();
                        if (data.length == 0 || data == undefined) {
                            valid = false;
                            $('<span id ="validError' + gridName + '" class="text-danger">' + text + '</span>').insertAfter(item);
                            item.closest('td').click(function () {
                                if (item.closest('td').find('#validError' + gridName).length > 0) {
                                    $('#validError' + gridName, this).remove();
                                }
                            });
                        }
                    }
                }
                else if ($.isNullOrEmpty(item.val())) {
                    valid = false;
                    item.after('<span id ="validError' + element.name + '" class="text-danger">' + text + '</span>');
                    item.on("change blur keyup", function () {
                        if (item.closest('td').find('#validError' + element.name).length > 0) {
                            $('#validError' + element.name, item.closest('td')).remove();
                        }
                    });
                }
            }
            if (validIdCard && valid && !$.validIdCard(item.val())) {
                valid = false;
                var text = "รูปแบบหมายเลขบัตรไม่ถูกต้อง";
                if ($("#hdSystemLang").val() == "en-US") {
                    text = "Invalid id card number.";
                }
                item.after('<span id ="validError' + element.name + '" class="text-danger">' + text + '</span>');
                item.on("change blur keyup", function () {
                    if (item.closest('td').has('#validError' + element.name)) {
                        $('#validError' + element.name, item.closest('td')).remove();
                    }
                });
            }
            if (!valid && result) {
                result = false;
            }
        };
        var rselectTextarea = /^(?:select|textarea)/i;
        var rinput = /^(?:color|date|datetime|datetime-local|email|hidden|month|number|password|range|search|tel|text|time|url|week|file)$/i;
        var rCRLF = /\r?\n/g;
        var elems = $("input,select,textarea", this).not($("input,select,textarea", $('.dataTables_wrapper', this))).filter(function () {
            return this.name && !this.disabled && (this.checked || rselectTextarea.test(this.nodeName) || rinput.test(this.type) || this.type == "checkbox");
        });

        $.each(elems, extend);

        return result;
    };

    $.fn.BindSelect = function (options) {
        var settings = {
            Value: undefined,
            ValueField: 'Value',
            TextField: 'Text',
            Url: undefined,//required
            Parameters: []
            //{
            // type: 'ControlId',//ControlId,FixValue
            // name:'',
            // value:''
            //}    
        };
        $.extend(settings, options);
        var ctrl = this;

        if ($.isNullOrEmpty(settings.Url)) {
            return false;
        }
        ctrl.prop('disabled', true);
        var data = {
        };
        if (settings.Parameters.length > 0) {
            $.each(settings.Parameters, function (i, item) {
                var val = item.value;
                if (item.type == 'ControlId') {
                    val = $('#' + item.value).val();
                }
                data[item.name] = val;
            });
        }
        $.ajax({
            url: settings.Url,
            data: data,
            type: 'post',
            success: function (response) {
                ctrl.empty();
                var opt = '<option value="">';
                if (ctrl.data('issearch')) {
                    opt += $("#hdSysSelectDefaultSearch").val();
                }
                else if (ctrl.data('isrequired')) {
                    opt += $("#hdSysSelectDefaultRequired").val();
                }
                opt += '</option>';
                ctrl.append(opt);
                $.each(response.data, function (i, item) {
                    opt = '<option value="' + item[settings.ValueField] + '"';
                    if (item[settings.ValueField] == settings.Value) {
                        opt += ' selected';
                    }
                    opt += '>' + item[settings.TextField] + '</option>';
                    ctrl.append(opt);
                });
                ctrl.prop('disabled', false);
                if (settings.onAfterSuccess != undefined && $.isFunction(settings.onAfterSuccess)) {
                    settings.onAfterSuccess(response);
                }
            },
            error: function (request, status, error) {
                if (request.status == 440 || request.responseText.indexOf('The provided anti-forgery token') != -1) {//440 Login Timeout
                    window.location.href = rootPath + "Users/Account/SignOut";
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
            }
        });
    };

    $.fn.BindMultiSelect = function (options) {
        var settings = {
            Value: undefined,
            ValueField: 'Value',
            TextField: 'Text',
            Url: undefined,//required
            Parameters: []
            //{
            // type: 'ControlId',//ControlId,FixValue
            // name:'',
            // value:''
            //}    
        };
        $.extend(settings, options);
        var ctrl = this;

        if ($.isNullOrEmpty(settings.Url)) {
            return false;
        }
        ctrl.multiselect('disable');
        var data = {
        };
        if (settings.Parameters.length > 0) {
            $.each(settings.Parameters, function (i, item) {
                var val = item.value;
                if (item.type == 'ControlId') {
                    val = $('#' + item.value).val();
                }
                data[item.name] = val;
            });
        }
        $.ajax({
            url: settings.Url,
            data: data,
            type: 'post',
            success: function (response) {
                ctrl.empty();
                var nData = [];
                $.each(response.data, function (i, item) {
                    opt = '<option value="' + item[settings.ValueField] + '"';
                    if (item[settings.ValueField] == settings.Value) {
                        opt += ' selected';
                    }
                    opt += '>' + item[settings.TextField] + '</option>';
                    ctrl.append(opt);

                    var data = { value: item[settings.ValueField], label: item[settings.TextField], selected: (item[settings.ValueField] == settings.Value) };
                    nData.push(data);
                });
                ctrl.multiselect('dataprovider', nData);
                ctrl.multiselect('enable');
                if (settings.onAfterSuccess != undefined && $.isFunction(settings.onAfterSuccess)) {
                    settings.onAfterSuccess(response);
                }
            },
            error: function (request, status, error) {
                if (request.status == 440 || request.responseText.indexOf('The provided anti-forgery token') != -1) {//440 Login Timeout
                    window.location.href = rootPath + "Users/Account/SignOut";
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
            }
        });
    };

    $.fn.BetweenDate = function (options) {
        var val_more = $("[name=" + options.from_ctrl + "]").val();
        var val_less = $("[name=" + options.to_ctrl + "]").val();

        if (val_more != undefined && val_more != null && val_more != "") {
            $("[name=" + options.to_ctrl + "]").datepicker("setStartDate", $.stringToDate(val_more));
        }
        else {
            $("[name=" + options.to_ctrl + "]").datepicker("setStartDate", -Infinity);
        }
        if (val_less != undefined && val_less != null && val_less != "") {
            $("[name=" + options.from_ctrl + "]").datepicker("setEndDate", $.stringToDate(val_less));
        }
        else {
            $("[name=" + options.from_ctrl + "]").datepicker("setEndDate", Infinity);
        }


        $("[name=" + options.from_ctrl + "]").datepicker({
            locale: "en",
            format: "dd/mm/yyyy",
            autoclose: true,
            todayHighlight: true,
            orientation: 'bottom'
        }).on("hide", function (selected) {
            var val = $(this).val();
            if (val != undefined && val != null && val != "") {
                $("[name=" + options.to_ctrl + "]").datepicker("setStartDate", $.stringToDate(val));
            }
            else {
                $("[name=" + options.to_ctrl + "]").datepicker("setStartDate", -Infinity);
            }
        }).next().on(ace.click_event, function () {
            $(this).prev().focus();
        });

        $("[name=" + options.to_ctrl + "]").datepicker({
            locale: "en",
            format: "dd/mm/yyyy",
            autoclose: true,
            todayHighlight: true,
            orientation: 'bottom'
        }).on("hide", function (selected) {
            var val = $(this).val();
            if (val != undefined && val != null && val != "") {
                $("[name=" + options.from_ctrl + "]").datepicker("setEndDate", $.stringToDate(val));
            }
            else {
                $("[name=" + options.from_ctrl + "]").datepicker("setEndDate", Infinity);
            }
        }).next().on(ace.click_event, function () {
            $(this).prev().focus();
        });
    };

    $.SlidingTime = function () {
        var rootPath = $("#hdSysRootPath").val();
        if (rootPath == undefined || rootPath == null || rootPath == "/") {
            rootPath = "/";
        }
        $.ajax({
            url: rootPath + 'Ux/Other/SlidingTime',
            type: 'post',
            error: function (request, status, error) {
                if (request.status == 440 || request.responseText.indexOf('The provided anti-forgery token') != -1) {//440 Login Timeout
                    window.location.href = rootPath + "Users/Account/SignOut";
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
            }
        });
    };

    $.validIdCard = function (value) {
        var isValid = true;
        if (value.length != 13) {
            isValid = false;
        }

        for (i = 0, sum = 0; i < 12; i++) {
            sum += parseFloat(value.charAt(i)) * (13 - i);
        }
        if ((11 - sum % 11) % 10 != parseFloat(value.charAt(12))) {
            isValid = false;
        }
        return isValid;
    }

    $.fn.hasRowCtrl = function () {
        var result = false;
        var trs = this.find('tbody tr');
        $.each(trs, function (i, item) {
            if ($(item).data('rowctrl')) {
                result = true;
                return false;
            }
        });
        return result;
    };

    $.clearValidError = function (ctrlName) {
        if ($.isNullOrEmpty(ctrlName)) {
            return;
        }
        if ($('#validError' + ctrlName).length > 0) {
            $('#validError' + ctrlName).remove();
        }
    };

    $.fn.vsmMultiselect = function (options) {
        return this.each(function () {
            var elem_item = $(this);

            var settings = {
                enableFiltering: true,
                buttonClass: 'btn btn-white',
                buttonWidth: "100%",
                maxHeight: 400,
                enableCaseInsensitiveFiltering: true,
                numberDisplayed: 1,
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text"></span> <b class="fa fa-caret-down"></b></button>',
                    filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="fa fa-search"></i></span><input class="form-control multiselect-search" type="text"></div></li>',
                    filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default btn-white btn-grey multiselect-clear-filter" type="button"><i class="fa fa-times-circle red2"></i></button></span>',
                }
            };

            var dropRight = elem_item.data('dropright');
            if (dropRight) {
                settings['dropRight'] = dropRight;
            }

            var type = elem_item.data('multiselect_type');
            if (type == 'search') {
                settings['nonSelectedText'] = $("#hdSysSelectDefaultSearch").val();
                settings['allSelectedText'] = $("#hdSysSelectDefaultSearch").val();
            }
            else if (type == 'required') {
                settings['nonSelectedText'] = $("#hdSysSelectDefaultRequired").val();
                settings['allSelectedText'] = $("#hdSysSelectDefaultSearch").val();
            }

            $.extend(settings, options);

            $(elem_item).multiselect(settings);
        });
    };

    $.vsmAjax = function (options) {
        var settings = {
            Url: undefined,//required
            formId: undefined,
            Parameters: [],
            //{
            // type: 'ControlId',//ControlId,FixValue
            // name:'',
            // value:''
            //}    
            data: {},
            onSuccess: undefined//function(response)
        };
        $.extend(settings, options);
        if ($.isNullOrEmpty(settings.Url)) {
            return false;
        }
        var formData = new FormData();
        if (!$.isNullOrEmpty(settings.formId)) {
            formData = $('form[id=' + settings.formId + ']').serializeFormData();
        }
        if (settings.Parameters.length > 0) {
            $.each(settings.Parameters, function (i, item) {
                var val = item.value;
                if (item.type == 'ControlId') {
                    val = $('#' + item.value).val();
                }
                settings.data[item.name] = val;
            });
        }
        if ($.isEmptyObject(settings.data)) {
            $.each(settings.data, function (key, value) {
                if (isArray(value)) {
                    $.each(value, function (i, data) {
                        $.each(data, function (k, v) {
                            formData.set(key + '[' + i + '].' + k, v);
                        });
                    });
                }
                else {
                    formData.set(key, value);
                }
            });
        }
        $.ajax({
            url: settings.Url,
            data: formData,
            type: 'post',
            contentType: false,
            processData: false,
            success: function (response) {
                if (settings.onSuccess != undefined && $.isFunction(settings.onSuccess)) {
                    settings.onSuccess(response);
                }
            },
            error: OnAjaxError
        });
    };
}(jQuery));

