
var ServiceURL = "/Account/";

var app = angular.module("Account", []);


app.service("AccountService", function ($http) {

    this.Registration = function (Register) {
        var response = $http({
            method: "post",
            url: ServiceURL + "UserRegistration",
            data: JSON.stringify(Register),
            dataType: "json"
        });
        return response;
    }

    this.UserLogin = function (Login) {
        var response = $http({
            method: "post",
            url: ServiceURL + "UserLogin",
            data: JSON.stringify(Login),
            dataType: "json"
        });
        return response;
    }

    this.ForgetPassword = function (Login) {
        var response = $http({
            method: "post",
            url: ServiceURL + "ForgetPassword",
            data: JSON.stringify(Login),
            dataType: "json"
        });
        return response;
    }

    this.Svc_Contactus = function (Login) {
        var response = $http({
            method: "post",
            url: "/Home/" + "SaveContactus",
            data: JSON.stringify(Login),
            dataType: "json"
        });
        return response;
    }

});


app.controller("AccountController", function ($scope, AccountService) {

    $scope.UserType = 4;

    $scope.RegistrationUser = function () {
        debugger;
        var Register = {
            Email: $scope.Email,
            Password: $scope.Password,
            UserType: $scope.UserType,
            CompanyName: $scope.CompanyName,
            captcha: $scope.captcha
        };

        if ($scope.Email == "" || $scope.Email === undefined || $scope.Password == "" || $scope.Password === undefined) {
            $scope.color = "#Ff0000";
            $scope.message = "All fields are required or email address incorrect";

        }
        else if ($scope.Password != $scope.rPassword) {
            $scope.message = "Repeat password not matched";
            $scope.color = "#F00";
        }
        else {
            var ResponseRegistration = AccountService.Registration(Register);
            ResponseRegistration.then(function (msg) {
                debugger;
                $scope.message = msg.data.RMessage;
                $scope.color = "#58BA2B";

                if (msg.data.RCode == 1) {
                    $scope.ClearFields();
                    window.location.replace(msg.data.RURL);
                }


            }, function (msg) {
                debugger;
                alert('Error in adding book record');
            });
        }


    }

    $scope.ClearFields = function () {
        $scope.Email = "";
        $scope.Password = "";
        $scope.rPassword = "";

    }


    $scope.Employer = function () {
        $scope.UserType = 4;
        $('#cmd_employer').addClass('active');
        $('#cmd_jobseeker').removeClass('active');
        $('#cmd_recuritor').removeClass('active');
        $('#lblform').text("Employer");
        $('#fieldCom').show();
    }

    $scope.JobSeeker = function () {
        $scope.UserType = 2;
        $('#cmd_employer').removeClass('active');
        $('#cmd_jobseeker').addClass('active');
        $('#cmd_recuritor').removeClass('active');
        $('#lblform').text("Job Seeker");
        $('#fieldCom').hide();
    }

    $scope.Recuritor = function () {
        $scope.UserType = 3;
        $('#cmd_employer').removeClass('active');
        $('#cmd_jobseeker').removeClass('active');
        $('#cmd_recuritor').addClass('active');
        $('#lblform').text("Recuritor");
        $('#fieldCom').show();
    }



});

app.controller("Login", function ($scope, AccountService) {

    $scope.FUsername = "";
    $scope.Username = "";

    $scope.fn_CheckOnBlur = function (val) {
        if (val == 1) {
            if ($scope.Username == "" || $scope.Username == undefined) {
                $scope.lblUsername = "Please enter email address";
            }
            else {
                $scope.lblUsername = "";
            }
        }
        if (val == 2) {
            if ($scope.Password == "" || $scope.Password == undefined) {
                $scope.lblPassword = "Please enter password";
            }
            else {
                $scope.lblPassword = "";
            }
        }

    }

    $scope.fn_Validateform = function () {


        var IsValidated = true;

        if (($scope.Username == "" || $scope.Username == undefined) && ($scope.Password == "" || $scope.Password == undefined)) {
            $scope.lblUsername = "Please enter email address";
            $scope.lblPassword = "Please enter password";
            IsValidated = false;
        }
        if ($scope.Username == "" || $scope.Username == undefined) {
            $scope.lblUsername = "Please enter email address";
            IsValidated = false;
        }
        if ($scope.Password == "" || $scope.Password == undefined) {
            $scope.lblPassword = "Please enter password";
            IsValidated = false;
        }
        if ($scope.Username != "") {
            if (validateEmail($scope.Username) == false) {
                $scope.lblUsername = "Please enter valid email address";
                IsValidated = false;
            }
        }

        return IsValidated;

    }

    $scope.fn_LoginCallBack = function () {
        if ($scope.fn_Validateform() == true) {
            $scope.Login();
        }

    }


    $scope.Login = function () {
        debugger;

        swal({
            title: "Wait",
            text: "Please wait your request is processing...",
            showConfirmButton: false
        });

        var Login = {
            Username: $scope.Username,
            Password: $scope.Password

        };

        if ($scope.Username == "" || $scope.Username === undefined || $scope.Password == "" || $scope.Password === undefined) {
            $scope.color = "#Ff0000";
            $scope.message = "All fields are required or email address incorrect";

        }
        else {



            var LoginResponse = AccountService.UserLogin(Login);
            LoginResponse.then(function (msg) {
                debugger;

                if (msg.data.RCode == 1) {
                    swal("Login Successful", msg.data.RMessage, "success");
                    window.location.replace(msg.data.RURL);
                }

                if (msg.data.RCode == 2) {
                    swal("Login Unsuccessful", msg.data.RMessage, "error");
                }



            }, function (msg) {
                debugger;
                $('#cmd_Login').prop('disabled', false);
                $scope.message = "";
                console.log('Error in logging user');
                $('.my-account').removeClass("disable-form ");
                $('.loader').hide();
            });
        }


    }

    $scope.fn_ForgetPassword = function () {

        $('#cmd_Login').prop('disabled', true);

        $('.my-account').addClass("disable-form ");
        $('.loader').show();

        $scope.Rmessage = "Please wait...";
        $scope.Rcolor = "#0000ff";

        var Login = {
            Username: $scope.FUsername
        };

        var LoginResponse = AccountService.ForgetPassword(Login);
        LoginResponse.then(function (msg) {
            debugger;

            if (msg.data.RCode == 1) {
                $scope.Rmessage = "";
                $scope.Rcolor = "#58BA2B";
                $('.my-account').removeClass("disable-form ");
                $('.loader').hide();
                swal("Success", msg.data.RMessage, "success");
                //// Added by Rahul 13-10-2017 for when a user tries to Login after he is sent a Forgot Password link. ////
                setTimeout(function () {
                    window.location.reload();
                }, 3000)
                //$('#forgetpassword').hide();
                //$('#login').show();
            }

            if (msg.data.RCode == 2) {
                $scope.Rmessage = "";
                $('#cmd_Login').prop('disabled', false);
                $scope.Rcolor = "#58BA2B";
                $('.my-account').removeClass("disable-form ");
                $('.loader').hide();
                swal("Error", msg.data.RMessage, "error");
            }



        }, function (msg) {
            debugger;
            $('#cmd_Login').prop('disabled', false);
            $scope.message = "";
            alert('Error in logging user');
            $('.my-account').removeClass("disable-form ");
            $('.loader').hide();
        });
    }

    $scope.fn_FP_Validation = function () {
        if ($scope.FUsername == "") {
            $scope.Rmessage = "Please enter email address";
        }
        else {
            $scope.fn_ForgetPassword();
        }
    }


});

app.controller("Package", function ($scope, AccountService) {

    $scope.Login = function () {
        debugger;
        var Login = {
            Username: $scope.Username,
            Password: $scope.Password

        };

        if ($scope.Username == "" || $scope.Username === undefined || $scope.Password == "" || $scope.Password === undefined) {
            $scope.color = "#Ff0000";
            $scope.message = "All fields are required or email address incorrect";

        }
        else {
            var LoginResponse = AccountService.UserLogin(Login);
            LoginResponse.then(function (msg) {
                debugger;

                if (msg.data.RCode == 1) {
                    $scope.message = msg.data.RMessage;
                    $scope.color = "#58BA2B";
                    window.location.replace(msg.data.RURL);
                }

                if (msg.data.RCode == 2) {
                    $scope.message = msg.data.RMessage;
                    $scope.color = "#58BA2B";
                }



            }, function (msg) {
                debugger;
                alert('Error in logging user');
            });
        }


    }

});

app.controller("ContactUs", function ($scope, AccountService) {



    $scope.fn_SaveContactus = function () {
        debugger;

        var Login = {
            Name: $scope.Name,
            Email: $scope.Email,
            Message: $scope.Message

        };

        $scope.message = "Please wait your request is processing...";

        var LoginResponse = AccountService.Svc_Contactus(Login);
        LoginResponse.then(function (msg) {
            debugger;

            if (msg.data.RCode == 1) {
                $scope.message = msg.data.RMessage;
                $scope.color = msg.data.RColorCode;
            }

            if (msg.data.RCode == 0) {
                $scope.message = msg.data.RMessage;
                $scope.color = msg.data.RColorCode;
            }

        }, function (msg) {
            debugger;
            alert('Error in logging user');
        });


    }

    $scope.fn_Validateform = function () {

        if ($scope.Name == "" || $scope.Name == undefined) {
            $scope.message = "Please enter name";
            $scope.color = "#f00";
        }
        else if ($scope.Email == "" || $scope.Email == undefined) {
            $scope.message = "Please enter email";
            $scope.color = "#f00";
        }
        else if ($scope.Message == "" || $scope.Message == undefined) {
            $scope.message = "Please enter message";
            $scope.color = "#f00";
        }
        else {
            $scope.fn_SaveContactus();
        }

    }

});



function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}