$(document).ready(function () {
    addExportOption();
});

function addExportOption() {
    var tA = $("a[title='Excel']");
    var parentA = $(tA).parents('div').parents('div').first();
    parentA.append('<div style="border: 1px solid rgb(51, 102, 153); border-image: none; cursor: pointer; background-color: rgb(221, 238, 247);"><a title="CSV" style="padding: 3px 8px 3px 32px; color: rgb(51, 102, 204); font-family: Verdana; font-size: 8pt; text-decoration: none; display: block; white-space: nowrap;" onclick="GenarateExportCSV(\'CSV\');"  alt="CSV">CSV</a></div>');
}

function CustomExport(exportType) {
    if (exportType == "CSV") {
        GenarateExportCSV(exportType);
    }
}