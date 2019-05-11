$(document).ready(function () {
    $("iframe").iframeAutoHeight();
    $(window).on("beforeunload", DeletePrintFileReport);
});

//$(window).on('unload', function () {
//    if (!$.browser.msie) {
//        DeletePrintFileReport();
//    }
//});

function DeletePrintFileReport() {

    var rootPath = $("#hdSysRootPath").val();
    if (rootPath == undefined || rootPath == null || rootPath == "") {
        rootPath = "/";
    }

    $.ajax({
        type: "post",
        url: rootPath + "Ux/Report/DeletePrintFile",
        /*dataType: "json",
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
            }
            else {
                alert(result.Message)
            }
        },*/
        //success: function (result) {
        //    alert("success");
        //},
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
    return undefined;
}