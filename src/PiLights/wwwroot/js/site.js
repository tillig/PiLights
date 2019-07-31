function pollSmartPlugStatus() {
    (function p() {
        $.ajax({
            url: "/power/status",
            dataType: "json"
        }).done(function (data) {
            console.log("Light on? " + data.on);
        }).fail(function (xhr, status, err) {
            console.log("Error: " + err);
        });
        setTimeout(p, 1000);
    })();
}

$(function () {
    pollSmartPlugStatus();
});