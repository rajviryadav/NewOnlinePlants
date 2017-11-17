
var ServiceURL = "/Packages/";

var app = angular.module("Packages", []);


app.service("PackagesService", function ($http) {

    this.UpdatePackage = function (Package) {
        var response = $http({
            method: "post",
            url: ServiceURL + "UpdatePackage",
            data: JSON.stringify(Package),
            dataType: "json"
        });
        return response;
    }

    this.UpdateAgreement = function (Package) {
        var response = $http({
            method: "post",
            url: ServiceURL + "UpdateAgreement",
            data: JSON.stringify(Package),
            dataType: "json"
        });
        return response;
    }

    this.ProcessThePayment = function (Package) {
        var response = $http({
            method: "post",
            url: ServiceURL + "ProcessPayment",
            data: JSON.stringify(Package),
            dataType: "json"
        });
        return response;
    }

    this.CustomerCardDetails = function () {
        var response = $http({
            method: "Get",
            url: ServiceURL + "GetCustomerCardDetails",
            dataType: "json"
        });
        return response;
    }
    this.UpdateCustomerCardDetails = function (card) {
        var response = $http({
            method: "post",
            url: ServiceURL + "UpdateCustomerCardDetails ",
            data: JSON.stringify(card),
            dataType: "json"
        });
        return response;
    }
    this.DeleteCustomerCard = function (card) {
        var response = $http({
            method: "post",
            url: ServiceURL + "DeleteCustomerCard",
            data: JSON.stringify(card),
            dataType: "json"
        });
        return response;
    }
    this.CustomerPaymentDetails = function () {
        var response = $http({
            method: "post",
            url: ServiceURL + "GetPaymentDetails",
            dataType: "json"
        });
        return response;
    }
});


app.controller("PackageController", function ($scope, PackagesService) {

    $scope.PackageID = 1;
    $scope.Amount = 19;

    $scope.UserSelectedPackage = function () {

        $scope.fn_PackageSelectionValues();
        window.location.href = "/Packages/Payment";

    }

    $scope.fn_PackageSelectionValues = function () {

        localStorage.setItem("PackageID", $scope.PackageID);
        localStorage.setItem("Amount", $scope.Amount);
    }

    $scope.package1 = function () {
        $scope.PackageID = 1;
        $scope.Amount = 19;
    }

    $scope.package2 = function () {
        $scope.PackageID = 2;
        $scope.Amount = 59;
    }

    $scope.package3 = function () {
        $scope.PackageID = 3;
        $scope.Amount = 99;
    }


    $scope.PaymentProcessing = function () {

        var Package = {
            Token: $scope.Token

        };


        var ResponsePackage = PackagesService.ProcessThePayment(Package);
        ResponsePackage.then(function (msg) {
            debugger;



        }, function (msg) {
            debugger;
            alert('Error in updating package');
        });


    }



});

app.controller("AgreementController", function ($scope, PackagesService) {

    $scope.IsAgree = 0;

    $scope.fn_GetAgreementStatus = function () {
        debugger;

        $scope.message = "Please wait...";
        $scope.color = "#0000ff";

        var Package = {
            IsAgree: $scope.IsAgree
        };




        var ResponsePackage = PackagesService.UpdateAgreement(Package);
        ResponsePackage.then(function (msg) {
            debugger;

            if (msg.data != null) {
                $scope.message = msg.data.RMessage;
                $scope.color = "#58BA2B";

                if (msg.data.RCode == 1) {
                    window.location.replace(msg.data.RURL);
                }
            }
            else {
                window.location.replace("/Account/Login");
            }



        }, function (msg) {
            debugger;
            alert('Error in updating package');
        });


    }


    $scope.fn_CheckIsAgree = function () {

        if ($scope.IsAgree == 0) {
            $scope.message = "Please accept agreement";
            $scope.color = "#f00";
        }
        if ($scope.IsAgree == 1) {

            $scope.fn_GetAgreementStatus();
        }

    }


});
app.controller("BillingController", function ($scope, PackagesService) {
    GetCard();
    $scope.cardList = [];
    function GetCard() {
        var response = PackagesService.CustomerCardDetails();
        $scope.cardId = "";
        $('#lblCardMessage').hide();
        $scope.CardMessage = '';
        $scope.showMessage = false;
        response.then(function (msg) {
            debugger;
            if (msg.data.classobject.length == 0) {
                $('#lblNoCardsAdded').show();
            }
            if (msg.data.classobject.length > 0) {
                $('#lblNoCardsAdded').hide();
            }
            if (msg == null) {
                console.log("data null");
            }
            else {
                $scope.cardList = msg.data.classobject;
                $scope.specialChar = "************";
            }
        }, function (msg) {
            console.log('Error in job');
        });
    }
    $scope.fn_updateCard = function (cardId, customerId, expirationMonth, expirationYear, name) {
        var Card = {
            CardId: cardId,
            CustomerId: customerId,
            ExpirationMonth: expirationMonth,
            ExpirationYear: expirationYear,
            Name: name

        }
        var response = PackagesService.UpdateCustomerCardDetails(Card);
        response.then(function (msg) {
            debugger;

            if (msg.data.RCode == 0) {
                alert(msg.data.RException);
            }
            else {
                $scope.CardMessage = "Your card has been updated";
                $scope.showMessage = true;
                $('#lblCardMessage').show();
                $('#lblCardMessage').fadeIn('fast').delay(5000).fadeOut('fast');
                //GetCard();

            }
        }, function (msg) {
            alert(msg.data.RException);
        });
    }


    $scope.fn_deleteCard = function (cardId, customerId) {
        var Card = {
            CardId: cardId,
            CustomerId: customerId
        }
        var response = PackagesService.DeleteCustomerCard(Card);
        response.then(function (msg) {
            debugger;
           
            if (msg == null) {
                console.log("data null");
            }
            else {
                $scope.CardMessage = "Your card has been deleted";
                $('#lblCardMessage').show();
                $('#lblCardMessage').fadeIn('fast').delay(5000).fadeOut('fast');
                GetCard();
            }
        }, function (msg) {
            console.log('Error in job');
        });
    }

    $scope.fn_addCard = function () {
        window.location.href = "/Packages/Payment?IsLoggedInUser=true";

    }


});
app.controller("InvoiceController", function ($scope, PackagesService) {
    //GetCard();
    GetInvoiceForCustomer();
    function GetInvoiceForCustomer() {
        $scope.invoiceList = [];
        var response = PackagesService.CustomerPaymentDetails();
        response.then(function (msg) {
            debugger;
            if (msg.data.classobject.length == 0) {
                $('#lblNoInvoice').show();
            }
            if (msg.data.classobject.length > 0) {
                $('#lblNoInvoice').hide();
            }
            if (msg == null) {
                console.log("data null");
            }
            else {
                $scope.invoiceList = msg.data.classobject;

            }
        }, function (msg) {
            console.log('Error in job');
        });
    }



});


