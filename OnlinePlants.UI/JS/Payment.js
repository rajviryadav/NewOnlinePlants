


$(function () {

    var isForPayment = "";

    $(function () {
        $.fn.serializeObject = function () {
            var o = {};
            var a = this.serializeArray();
            $.each(a, function () {
                if (o[this.name] !== undefined) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        };
    })
});

$(function () {
    $('#cmd_Payment').click(function () {
        var $form = $('#payment-form');

        $('#month').val($('#expiredate').val().split('/')[0]);
        $('#year').val($('#expiredate').val().split('/')[1]);


        $form.submit(function (event) {
            // Disable the submit button to prevent repeated clicks:
            $('#lblmessage').text("Please wait your request is processing...");
            $('#lblmessage').css("color", "#58BA2B");
            $form.find('.submit').prop('disabled', true);
            isForPayment = "true";
            // Request a token from Stripe:
            Stripe.card.createToken($form, stripeResponseHandler);

            // Prevent the form from being submitted:
            return false;
        });

    })

    $('#cmd_AddCard').click(function () {
        var $form = $('#payment-form');

        $('#month').val($('#expiredate').val().split('/')[0]);
        $('#year').val($('#expiredate').val().split('/')[1]);


        $form.submit(function (event) {
            // Disable the submit button to prevent repeated clicks:
            $('#lblmessage').text("Please wait your request is processing...");
            $('#lblmessage').css("color", "#58BA2B");
            $form.find('.submit').prop('disabled', true);
            isForPayment = "false";
            // Request a token from Stripe:
            Stripe.card.createToken($form, stripeResponseHandler);

            // Prevent the form from being submitted:
            return false;
        });

    })
})


function stripeResponseHandler(status, response) {
    debugger;
    // Grab the form:
    var $form = $('#payment-form');

    if (response.error) { // Problem!

        // Show the errors on the form:
        $form.find('.payment-errors').text(response.error.message);
        console.log(response.error.message);
        $form.find('.submit').prop('disabled', false); // Re-enable submission

    } else { // Token was created!

        // Get the token ID:
        var token = response.id;

        // Insert the token ID into the form so it gets submitted to the server:
        $('#Token').val(token);
        $form.append($('<input type="hidden" name="stripeToken">').val(token));       
        if (isForPayment == "false") {
            SendTokenToAddCard();
        }
        else
        {
            // Submit the form:
            SendTokenForPayment();
        }
            
    }
};

function SendTokenForPayment() {

    GetPackageValues();

    var data = JSON.stringify($('form').serializeObject());

    console.log(data);

    $.ajax({
        type: "POST",
        url: "/Packages/ProcessPayment",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        async: true,
        beforeSend: function () {
            $("#Loader").show();
        },
        success: function (msg, b, c) {
            debugger;
            if (msg.RCode == 1) {
                $('#lblmessage').text(msg.RMessage);
                $('#lblmessage').css("color", msg.RColorCode);
                window.location.href = msg.RURL;
            }
            if (msg.RCode == 0) {
                $('#lblmessage').text(msg.RMessage);
                $('#lblmessage').css("color", msg.RColorCode);

            }
        },
        error: function (a, b, c) {
            $("#Loader").hide();
        }
    });


}

function SendTokenToAddCard() {

    var Card = {
        Token:$('#Token').val()
    }
    var data = JSON.stringify($('form').serializeObject());
    $.ajax({
        type: "POST",
        url: "/Packages/AddCustomerCard",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        async: true,
        beforeSend: function () {
            $("#Loader").show();
        },
        success: function (msg, b, c) {
            debugger;
            if (msg.RCode == 1) {
                $('#lblmessage').text(msg.RMessage);
                $('#lblmessage').css("color", msg.RColorCode);
                window.location.href = "/Packages/Cards";
            }
            if (msg.RCode == 0) {
                $('#lblmessage').text(msg.RMessage);
                $('#lblmessage').css("color", msg.RColorCode);

            }
        },
        error: function (a, b, c) {
            $("#Loader").hide();
        }
    });


}
function GetPackageValues() {

    $('#Amount').val(localStorage.getItem("Amount"));
    $('#PackageID').val(localStorage.getItem("PackageID"));

}


