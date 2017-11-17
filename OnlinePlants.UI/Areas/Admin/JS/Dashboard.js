
var ServiceURL = "/Admin/";

var app = angular.module("Dashboard", []);



app.service("DashboardService", function ($http) {

    this.GetUserList = function (Param) {
        var response = $http({
            method: "post",
            url: ServiceURL + "Dashboard/GetUserList",
            data: Param,
            dataType: "json"
        });
        return response;
    }

    this.UpdateUserStatus = function (Param) {
        var response = $http({
            method: "post",
            url: ServiceURL + "Dashboard/UpdateUserStatus",
            data: Param,
            dataType: "json"
        });
        return response;
    }

    this.GetUserProfile = function (ProfileObject) {
        var response = $http({
            method: "post",
            url: ServiceURL + "Dashboard/GetProfile",
            data: ProfileObject,
            dataType: "json"
        });
        return response;
    }


});

app.controller("DashboardCTL", function ($scope, DashboardService) {

    $scope.PageNo = 1;
    $scope.UserTypeList = [];
    $('#lblalreadyAppliedmsg').hide();
    $scope.UserTypeId = 2;

    $scope.fn_GetUserList = function (UserTypeId) {

        var Params = {

            UserTypeId: UserTypeId,
            PageNo: $scope.PageNo,
            JobSearchTerm: "",

        };


        var response = DashboardService.GetUserList(Params);
        response.then(function (msg) {
            debugger;

            $scope.UserTypeList = msg.data.objectList;
            $scope.TotalPostedJobs = msg.data.TotalListRecords;
            $scope.TotalPages = msg.data.TotalPages;


        }, function (msg) {

            
            console.log('Error in Profile');
        });
    }

    $scope.fn_UpdateUserStatus = function (UserTypeId, UserId) {

        var Params = {

            UserTypeId: UserTypeId,
            UserId: UserId,

        };

        var response = DashboardService.UpdateUserStatus(Params);
        response.then(function (msg) {
            debugger;
            $scope.fn_GetUserList($scope.UserTypeId);
            $('#lblalreadyAppliedmsg').text(msg.data.RMessage);
            $('#lblalreadyAppliedmsg').show();
            $('#lblalreadyAppliedmsg').fadeIn('fast').delay(5000).fadeOut('fast');


        }, function (msg) {

            console.log('Error in Profile');
        });
    }

    $scope.GetProfile = function (UserId) {

        var Params = {
            UserId: UserId
        };

        var ProfileResponse = DashboardService.GetUserProfile(Params);
        ProfileResponse.then(function (msg) {
            debugger;

            if (msg == null) {
                console.log("data null");
            }

            if (msg.data.RCode == 1) {
                $scope.Name = msg.data.classobject.Name;
                $scope.Email = msg.data.classobject.Email;
                $scope.Phone = msg.data.classobject.Phone;
                $scope.ProfileURL = msg.data.classobject.ProfileURL != null ? msg.data.classobject.ProfileURL : "avatar-placeholder.png";
                $scope.IsProfilePrivate = msg.data.classobject.IsProfilePrivate;
                $scope.IsNotificatioOn = msg.data.classobject.IsNotificationOn;
                $scope.IsEmailForwardOn = msg.data.classobject.IsEmailForwardOn;
                $scope.IsMessageOn = msg.data.classobject.IsMobileMessageOn;
            }




        }, function (msg) {

            console.log('Error in Profile');
        });
    }

    $scope.LoadProfile = function () {
        $scope.UserId = getParameterByName("id");
        console.log($scope.UserId);
        $scope.GetProfile($scope.UserId);
    }





    $scope.fn_NextPage = function () {

        var PreviousPage = $scope.PageNo;
        $scope.PageNo = PreviousPage + 1;

        if ($scope.TotalPages >= $scope.PageNo) {
            $scope.fn_GetUserList($scope.UserTypeId);


        }
        if ($scope.TotalPages < $scope.PageNo) {
            alert("No more pages.");
            $scope.PageNo = $scope.PageNo - 1;
        }

    }

    $scope.fn_PrePage = function () {

        var PreviousPage = $scope.PageNo;
        $scope.PageNo = PreviousPage - 1;


        if ($scope.TotalPages >= $scope.PageNo && $scope.PageNo != 0) {
            $scope.fn_GetUserList($scope.UserTypeId);
        }
        if ($scope.TotalPages < $scope.PageNo || $scope.PageNo == 0) {
            alert("No more pages.");
            $scope.PageNo = $scope.PageNo + 1;
        }


    }



});

