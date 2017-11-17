

var ServiceURL = "/Messages/";



var app = angular.module("Messages", []);


app.service("MessagesService", function ($http) {

    this.SendMessage = function (Message) {
        var response = $http({
            method: "post",
            url: ServiceURL + "SendMessage",
            data: JSON.stringify(Message),
            dataType: "json"
        });
        return response;
    }

    this.GetMessagesList = function () {
        var response = $http({
            method: "post",
            url: ServiceURL + "GetAllMessageList",
            data: '',
            dataType: "json"
        });
        return response;
    }

    this.GetMessagesThread = function (message) {
        var response = $http({
            method: "post",
            url: ServiceURL + "GetAllMessageThread",
            data: JSON.stringify(message),
            dataType: "json"
        });
        return response;
    }

    this.Changepassword = function (message) {
        var response = $http({
            method: "post",
            url: ServiceURL + "Changepassword",
            data: JSON.stringify(message),
            dataType: "json"
        });
        return response;
    }


});


app.controller("MessagesController", function ($scope, MessagesService) {

    $scope.MessageList = [];
    $scope.MessageThread = [];

    $scope.fn_SendMessage = function () {
        debugger;

        $scope.MessageId = 0;

        var Message = {
            MessageId: $scope.MessageId,
            ReceiverId: $scope.ReceiverId,
            Subject: $scope.Subject,
            Description: $scope.Description

        };


        var response = MessagesService.SendMessage();
        response.then(function (msg) {

            $scope.CategoryList = msg.data;

        }, function (msg) {
            alert('Error in Message sending');
        });




    }

    $scope.fn_GetMessageList = function () {
        debugger;

        var response = MessagesService.GetMessagesList();
        response.then(function (msg) {
            $scope.MessageList = msg.data;
        }, function (msg) {
            alert('Error in Message sending');
        });
    }

    $scope.fn_GetMessageThread = function () {
        debugger;
        var JobId = getParameterByName("jobid");
        var ReceiverId = getParameterByName("receiverid");

        var Message =
        {
            JobId: JobId,
            ReceiverId: ReceiverId
        }

        var response = MessagesService.GetMessagesThread(Message);
        response.then(function (msg) {
            $scope.MessageThread = msg.data;
        }, function (msg) {
            alert('Error in Message sending');
        });
    }

    $scope.fn_SendMessage = function () {
        debugger;

        var JobId = getParameterByName("jobid");
        var ReceiverId = getParameterByName("receiverid");

        var Message = {
            ReceiverId: ReceiverId,
            JobId: JobId,
            Description: $scope.Description

        };

        $scope.lblSendMessage = "Please wait...";
        $scope.Color = "#0000ff";

        var response = MessagesService.SendMessage(Message);
        response.then(function (msg) {
            debugger;

            if (msg == null || msg.data == "") {
                window.location.href = "/Account/Login";
            }

            $scope.lblSendMessage = msg.data.RMessage;
            $scope.Color = msg.data.RColorCode;
            $scope.fn_GetMessageThread();


        }, function (msg) {
            alert('Error in job');
        });
    }


    $scope.fn_OpenThread = function (ReceiverId, JobId) {
        var patharray = window.location.pathname.split("/");
        window.location.href = "/" + patharray[1] + "/Dashboard/Message?ReceiverId=" + ReceiverId + "&JobId=" + JobId;
    }



});


app.controller("Changepassword", function ($scope, MessagesService) {

    $scope.message = "";
    $scope.RColorCode = "";
    $scope.Password = "";
    $scope.ConfirmPassword = "";
    $scope.NewPassword = "";

    $scope.fn_Changepassword = function () {
        debugger;

        var param = {
            Password: $scope.Password,
            NewPassword: $scope.NewPassword

        };


        var response = MessagesService.Changepassword(param);
        response.then(function (msg) {

            debugger;
            $scope.message = msg.data.RMessage;
            $scope.RColorCode = msg.data.RColorCode;


        }, function (msg) {
            console.log('Error in changing password');
        });
    }

    $scope.fn_Validate = function () {
        debugger;

        if ($scope.Password == "" || $scope.ConfirmPassword == "" || $scope.NewPassword == "") {
            $scope.message = "All fields are required";
            $scope.RColorCode = "#F00";
            return;
        }
        else if ($scope.Password.length < 8) {
            $scope.message = "Password length should be minimum 8";
            $scope.RColorCode = "#F00";
            return;
        }
        else if ($scope.Password != $scope.ConfirmPassword) {
            $scope.message = "Confirm password should be matched";
            $scope.RColorCode = "#F00";
            return;
        }
        else {
            $scope.fn_Changepassword();
        }

    }










});
