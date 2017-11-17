
var ServiceURL = "/Account/";

var Local = "6LdJ8AYUAAAAAJt9XZdk4AcwmWPf35KW_JXgulK2";
var Dev = "6Lc7DigTAAAAAAtBNM1OW_xGAooVMEIpzgFzNlzM";
var EnvirementCaptcha = "";

function GetCaptcha() {
    if (window.document.location.hostname == "localhost") {
        EnvirementCaptcha = Local;
    }
    else {
        EnvirementCaptcha = Dev;
    }

    return EnvirementCaptcha;
}

(function () {
    angular.module('angularRecaptcha', ['vcRecaptcha'])

	.controller('recapCtrl', ['vcRecaptchaService', '$http', function (vcRecaptchaService, $http) {
	    var vm = this;
	    vm.publicKey = GetCaptcha();
	    vm.UserType = 3;
	    vm.IsFormValidated = true;
	    vm.Password = "";



	    vm.fn_ValidateOnleave = function (Field) {
	        if (Field == 1) {
	            if (vm.Name == undefined || vm.Name == "") {
	                vm.lblName = "Please enter company name";
	            }
	            else {
	                vm.lblName = "";
	            }
	        }
	        if (Field == 2) {
	            if (vm.Email == undefined || vm.Email == "") {
	                vm.lblEmail = "Please enter email address";
	            }
	            else if (vm.Email != "") {
	                if (validateEmail(vm.Email) == false) {
	                    vm.lblEmail = "Please enter valid email address";
	                } else {
	                    vm.lblEmail = "";
	                }
	            }
	            else {
	                vm.lblEmail = "";
	            }
	        }

	        if (Field == 3) {
	            if (vm.Password == "") {
	                vm.lblPassword = "";
	                vm.lblPassword = "Please enter password and it should contain letters in Uppercase, Lowercase with a Number";
	            } else if (vm.Password.length < 12) {
	                vm.lblPasswordValidation = "Password should be minimum 12 character";
	                vm.IsFormValidated = false;
	            } else if (checkPassword(vm.Password) == false) {
	                vm.lblPassword = "";
	                vm.lblPasswordValidation = "Please enter password and it should contain letters in Uppercase, Lowercase with a Number";
	            } else {
	                vm.lblPassword = "";
	                vm.lblPasswordValidation = "";
	            }
	        }

	        if (Field == 4) {
	            if (vm.rPassword == undefined || vm.rPassword == "") {
	                vm.lblRepeatpassword = "Please enter password confirmation";
	            }
	            else if (vm.Password != vm.rPassword) {
	                vm.lblRepeatpassword = "Confirm Password does not match";
	            }
	            else {
	                vm.lblRepeatpassword = "";

	            }
	        }
	    }

	    vm.fn_ValidateForm = function () {
	        if (vm.Name == undefined || vm.Name == "") {
	            vm.lblName = "Please enter name";
	            vm.IsFormValidated = false;
	            return;
	        }
	        if (vm.Email == undefined || vm.Email == "") {
	            vm.lblEmail = "Please enter email address";
	            vm.IsFormValidated = false;
	            return;
	        }
	        if (vm.Email != "" && vm.Email != undefined) {
	            if (validateEmail(vm.Email) == false) {
	                vm.lblEmail = "Please enter valid email address";
	                vm.IsFormValidated = false;
	                return;
	            }
	        }
	           
	        if (vm.Password == undefined || vm.Password == "") {
	            vm.lblPassword = "";
	            vm.lblPassword = "Please enter password and it should contains uppercase, lower case and number";
	            vm.IsFormValidated = false;
	            return;
	        }

	        if (vm.Password != "") {

	            if (checkPassword(vm.Password) == false) {
	                vm.lblPasswordValidation = "Password should contains uppercase, lower case and number";
	                vm.IsFormValidated = false;
	                return;
	            }
	        }


	        if (vm.rPassword == undefined || vm.rPassword == "") {
	            vm.lblRepeatpassword = "Please enter confirm password";
	            vm.IsFormValidated = false;
	            return;
	        }
	        if (vm.Password != vm.rPassword) {
	            vm.message = "Password and Confirm Repeat Password should be matched";
	            vm.IsFormValidated = false;
	            return;
	        }
	        if (vm.rPassword != undefined || vm.rPassword != "") {
	            if (vm.Password.length < 12) {
	                vm.lblPasswordLength = "Password should be minimum 12 character";
	                vm.IsFormValidated = false;
	                return;
	            }
	        }

	        if (vm.UserType !== 2 && vm.CompanyName === "") {
	            vm.IsFormValidated = true;
	            return;
	        }

	        if ((vm.Email != "") && (validateEmail(vm.Email) == true) && (vm.Password != "") && (checkPassword(vm.Password) == true)
                && (vm.rPassword != "") && (vm.Password == vm.rPassword)) {
	            vm.IsFormValidated = true;
	        }

	        if (vm.IsFormValidated == true) {
	            vm.signup();
	        }
	    }

	    vm.signup = function () {


	        if (vcRecaptchaService.getResponse() === "") { //if string is empty
	            alert("Please resolve the captcha and submit!")
	        } else {

	            swal({
	                title: "Wait",
	                text: "Please wait your request is processing...",
	                showConfirmButton: false
	            });

	            vm.message = "Please wait...";
	            vm.color = "#0000ff";

	            var post_data = {  //prepare payload for request
	                Email: vm.Email,
	                Password: vm.Password,
	                UserType: vm.UserType,
	                Name: vm.Name,
	                'g-recaptcha-response': vcRecaptchaService.getResponse()  //send g-captcah-reponse to our server
	            }

	            /* Make Ajax request to our server with g-captcha-string */
	            $http.post(ServiceURL + 'UserRegistration', post_data).success(function (msg) {
	                debugger;

	                if (msg.RCode == 1) {
	                    swal("Registration successful", msg.RMessage, "success");
	                }
	                if (msg.RCode == 2) {
	                    swal("Registration Unsuccessful", msg.RMessage, "error");
	                }


	                vm.message = msg.RMessage;
	                vm.color = "#58BA2B";

	                if (msg.RCode == 1) {
	                    vm.ClearFields();
	                    window.location.replace(msg.RURL);
	                }
	            })
                .error(function (error) {

                })
	        }
	    }


	    vm.ClearFields = function () {
	        vm.Email = "";
	        vm.Password = "";
	        vm.rPassword = "";
	        vm.Name = "";
	    }   
	}])
})()



function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}


function checkPassword(str) {
    // at least one number, one lowercase and one uppercase letter
    // at least six characters
    var re = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}/;
    return re.test(str);
}