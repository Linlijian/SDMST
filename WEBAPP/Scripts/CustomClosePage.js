$(document).ready(function () {
    //$(window).on("beforeunload", SignOutOnClose);
});

function SignOutOnClose() {
    $.ajax({
        type: "post",
        url: "/Users/Account/SignOutOnClose",
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