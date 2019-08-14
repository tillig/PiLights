function pollSmartPlugStatus() {
    var powerIsOn = false;
    (function p() {
        $.ajax({
            url: "/power/status",
            dataType: "json"
        }).done(function (data) {
            if (data.on) {
                if (!powerIsOn) {
                    console.log("Power turned on.");
                    powerIsOn = true;
                }
                $("#powerForm").attr("action", "/power/shutdown");
                $('.power-off-button').show();
                $('.power-on-button').hide();
            } else {
                if (powerIsOn) {
                    console.log("Power turned off.");
                    powerIsOn = true;
                }
                $("#powerForm").attr("action", "/power/startup");
                $('.power-on-button').show();
                $('.power-off-button').hide();
            }
        }).fail(function (xhr, status, err) {
            console.log("Error: " + err);
        });
        setTimeout(p, 1000);
    })();
}

$(function () {
    $('#powerForm').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: $("#powerForm").attr("action"),
            type: 'post',
            data: $('#powerForm').serialize(),
            success: function () {
                $('#powerOnModal').modal('hide');
                $('#powerOffModal').modal('hide');
            }
        });
    });

    pollSmartPlugStatus();
});