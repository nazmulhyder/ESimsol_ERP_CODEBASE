

var CommonRequest = angular.module('CommonRequest.service', ['ngAnimate', 'ui.bootstrap']);
CommonRequest.service('CommonRequestservice', function ($uibModal) {
    debugger;
    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: (modalProperty.size == undefined) ? 'sm' : modalProperty.size,
            templateUrl: sessionStorage.getItem('BaseAddress') + '/CommonModal/CommonRequestModal',
            controller: (modalProperty.modalcontroller == undefined) ? 'ModalCommonRequestCtrl' : modalProperty.modalcontroller,
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        Users: (modalProperty.Users == undefined) ? [] : modalProperty.Users,
                        pickertitle: (modalProperty.pickertitle == undefined) ? null : modalProperty.pickertitle,
                        objectFieldName: (modalProperty.objectFieldName == undefined) ? null : modalProperty.objectFieldName,
                        objectID: (modalProperty.objectID == undefined) ? 0 : modalProperty.objectID,
                        requestactionType: (modalProperty.requestactionType == undefined) ? null : modalProperty.requestactionType,
                        controllerName: (modalProperty.controllerName == undefined) ? null : modalProperty.controllerName
                    };
                }

            }

        }

        modalInstance.controller = function ($scope, $http, $uibModalInstance, uiGridConstants, obj)
        {
            debugger;
            $scope.PickerTitle = (obj.pickertitle != null) ? obj.pickertitle : "Request For Approve";
            $scope.Users = obj.Users;//Load Users
            $scope.confirmCommonRequestModal = function ()
            {

                if ($scope.RequestTo <= 0) {
                    alert('Please Select User');
                    return;
                };
                if (obj.objectFieldName == null || obj.objectID == 0) {
                    alert('Sorry there is no Object. Please Select & Request Again.');
                    return;
                };
                if (obj.requestactionType == null) {
                    alert('Sorry there is no Request Action Type. Please Inform to Developer. Thanks');
                    return;
                };
                if (obj.controllerName == null) {
                    alert('Sorry there is no Controller Name. Please Inform to Developer. Thanks');
                    return;
                };

                if (!confirm("Confirm to Request?")) return;
                debugger;
                var oApprovalRequest = {
                    ApprovalRequestID: 0,
                    OperationObjectID: obj.objectID,
                    RequestTo: $scope.RequestTo,
                    Note: $scope.Note
                };
                var oSendingObj = {
                    ActionTypeExtra: obj.requestactionType,
                    ApprovalRequest: oApprovalRequest
                };
                //MasterLCID: obj.ObjectID, //MasterLC ID 
                oSendingObj[obj.objectFieldName] = obj.objectID; //MasterLC ID 
   
                $.ajax
                 ({
                     type: "POST",
                     dataType: "json",
                     url: sessionStorage.getItem('BaseAddress') + "/" + obj.controllerName + "/ChangeStatus",
                     data: JSON.stringify(oSendingObj),
                     contentType: "application/json; charset=utf-8",
                     success: function (data) {
                         //debugger;
                         var oReturnObj = jQuery.parseJSON(data);
                         if (oReturnObj != null) {
                             if (oReturnObj.ErrorMessage == "" || oReturnObj.ErrorMessage == null) {
                                 alert("Successfully Requested");
                                 $uibModalInstance.close(oReturnObj);
                             }
                             else {
                                 alert(oReturnObj.ErrorMessage);
                             }
                         }
                         else {
                             alert(oReturnObj.ErrorMessage);
                         }
                     },
                     error: function (xhr, status, error) {
                         alert(error);
                     }

                 });
            };

      
            $scope.closeCommonRequestModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }

});




//var conPickerApp = angular.module('CommonRequestModalApp', ['MasterLCApp']);
//conPickerApp.controller('ModalCommonRequestCtrl', function ($scope, $http, $uibModalInstance, uiGridConstants , obj) {
//    debugger;
//    $scope.PickerTitle = (obj.pickertitle != null) ? obj.pickertitle : "Request For Approve";
//    $scope.Users = obj.result;//Load Users
//    var com = conPickerApp;
//    $scope.confirmCommonRequestModal = function ()
//    {
      
//        if ($scope.RequestTo <= 0) {
//            alert('Please Select User');
//            return;
//        };
//        if (obj.objectFieldName == null || obj.objectID == 0) {
//            alert('Sorry there is no Object. Please Select & Request Again.');
//            return;
//        };

//        if (obj.requestactionType == null)
//        {
//            alert('Sorry there is no Request Action Type. Please Inform to Developer. Thanks');
//            return;
//        };
//        if (obj.controllerName == null) {
//            alert('Sorry there is no Controller Name. Please Inform to Developer. Thanks');
//            return;
//        };
                
//        if (!confirm("Confirm to Request?")) return;
//        debugger;
//        var oApprovalRequest = {
//            ApprovalRequestID: 0,
//            OperationObjectID:obj.objectID,
//            RequestTo: $scope.RequestTo,
//            Note: $scope.Note
//        };
//        var oSendingObj = {
//            ActionTypeExtra: obj.requestactionType,
//            ApprovalRequest: oApprovalRequest
//        };
//        //MasterLCID: obj.ObjectID, //MasterLC ID 
//        oSendingObj[obj.objectFieldName]= obj.objectID; //MasterLC ID 
//        //oSendingObj[ActionTypeExtra] = obj.requestactionType;//  "RequestForApproved",
//        //oSendingObj[ApprovalRequest]= oApprovalRequest;
//        $.ajax
//         ({
//             type: "POST",
//             dataType: "json",
//             url: sessionStorage.getItem('BaseAddress') + "/" + obj.controllerName + "/ChangeStatus",
//             data: JSON.stringify(oSendingObj),
//             contentType: "application/json; charset=utf-8",
//             success: function (data) {
//                 //debugger;
//                 var oReturnObj = jQuery.parseJSON(data);
//                 if (oReturnObj != null) {
//                     if (oReturnObj.ErrorMessage == "" || oReturnObj.ErrorMessage == null) {
//                         alert("Successfully Requested");
//                         $uibModalInstance.close(oReturnObj);
//                     }
//                     else {
//                         alert(oReturnObj.ErrorMessage);
//                     }
//                 }
//                 else {
//                     alert(oReturnObj.ErrorMessage);
//                 }
//             },
//             error: function (xhr, status, error) {
//                 alert(error);
//             }

//         }); 
//    };

//    $scope.closeCommonRequestModal = function () {
//        $uibModalInstance.dismiss('cancel');
//    };
//});