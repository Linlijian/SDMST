
$(document).ready(function () {

    if (!$.browser.msie) {
        try {
            showPrintButton();
        }
        catch (e) {
            alert(e);
        }
    }
});

function addExportOption() {
    var tA = $("a[title='Excel']");
    var parentA = $(tA).parents('div').parents('div').first();
    parentA.append('<div style="border: 1px solid rgb(51, 102, 153); border-image: none; cursor: pointer; background-color: rgb(221, 238, 247);"><a title="Text" style="padding: 3px 8px 3px 32px; color: rgb(51, 102, 204); font-family: Verdana; font-size: 8pt; text-decoration: none; display: block; white-space: nowrap;" onclick="CustomExport(\'Text\');"  alt="Text">Text</a></div>');
}

function showPrintButton() {

    var table = $("table[title='Refresh']");
    var parentTable = $(table).parents('table');
    var parentDiv = $(parentTable).parents('div').parents('div').first();
    var rootPath = $("#hdSysRootPath").val();
    if (rootPath == undefined || rootPath == null || rootPath == "") {
        rootPath = "/";
    }
    parentDiv.append('<input id="btnPrintReport" title="Print" style="margin-top:2px;padding: 3px;" type="image" alt="Print" src="' + rootPath + 'Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=12.0.2402.20&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" onclick="PrintReport();">');
}

// Print Report function
function PrintReport() {
    $("#frmPrint").get(0).contentWindow.print();
}

function CustomExport(exportType) {
    if (exportType == "Text") {
        GenarateExportText(exportType);
    }
}

function GenarateExportText(exportType) {
    $.ajax({
        type: "POST",
        url: "../Ashx/GenerateTextFile.ashx",
        dataType: "json",
        data: $(location).attr('search').substr(1),
        beforeSend: function () {
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });
        },
        success: function (result) {
            if (result.Status) {
                $.unblockUI();
                __doPostBack("Report_CustomExport", exportType);
            }
            else {
                alert(result.Message)
            }
        },
        error: function (jqXHR, exception) {
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            alert(msg)
        }
    });
    return true;
}