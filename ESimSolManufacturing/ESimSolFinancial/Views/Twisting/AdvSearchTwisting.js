var IICService = angular.module('Twisting.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
IICService.service('Twistingservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/Twisting/AdvSearchTwisting',
            controller: 'ModalTwistingAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    debugger;
                    return {
                        
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        OrderTypes: (modalProperty.OrderTypes == undefined) ? [] : modalProperty.OrderTypes
                    };
                }

            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj) {
            debugger;
            $(document).on('mousemove', '.modal-body', function () {
                $('.date-container').datepicker({
                    format: "dd M yyyy",
                    calendarWeeks: true,
                    autoclose: true,
                    todayHighlight: true
                });
            });

            $scope.lblLoadingMessage = true;
            $scope.PickerTitle = "Search Requisition";

            $scope.OrderDateChange = function () {
                $scope.OrderDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboOrderDate);
            }

            $scope.ReceiveDateChange = function () {
                $scope.ReceiveDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboReceiveDate);
            }
            $scope.ApproveDateChange = function () {
                $scope.ApproveDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboApproveDate);
            }
            $scope.CompletedDateChange = function () {
                $scope.CompletedDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboCompletedDate);
            }

            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
                var nVal = $scope.OrderType;
                if ($scope.OrderNo == "" && $scope.LotNo == "" && $scope.OrderType == 0 && $scope.cboOrderDate == 0 && $scope.cboReceiveDate == 0 && $scope.ApproveDate == 0 && $scope.CompletedDate == 0 && $scope.YetToApprove == false && $scope.YetToIssue == false && $scope.YetToReceive == false) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }

                var sTempString = $scope.cboOrderDate + '~' + $scope.OrderDateStart + '~' + $scope.OrderDateEnd + '~'
                                + $scope.cboReceiveDate + '~' + $scope.ReceiveDateStart + '~' + $scope.ReceiveDateEnd + '~'
                                + $scope.cboApproveDate + '~' + $scope.ApproveDateStart + '~' + $scope.ApproveDateEnd + '~'
                                + $scope.cboCompletedDate + '~' + $scope.CompletedDateStart + '~' + $scope.CompletedDateEnd + '~'
                                + ($scope.OrderNo == undefined ? "" : $scope.OrderNo) + '~' + ($scope.LotNo == undefined ? "" : $scope.LotNo) + '~'
                                + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive)+'~'
                                + $scope.OrderType + '~' + sessionStorage.getItem('BUID');
                
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(_sBaseAddress + '/Twisting/AdvSearch', { params: { sTemp: sTempString } }, config).then(
                            function (response) {
                                debugger;
                                //alert(sTempString); return;
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.length > 0) {
                                    $uibModalInstance.close(results);
                                } else
                                {
                                    msModal.Message({ headerTitle: '', bodyText: 'No Data Found !!', sucessText: 'Yes', cancelText: 'Close', feedbackType: false, autoClose: false });
                                    // alert("Data Not Found.");
                                    return;
                                }

                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );
            };

            $scope.printTwistingsPDF = function () {
                debugger;
                $scope.lblLoadingMessage = false;
                var nVal = $scope.OrderType;
                if ($scope.OrderNo == "" && $scope.LotNo == "" && $scope.OrderType == 0 && $scope.cboOrderDate == 0 && $scope.cboReceiveDate == 0 && $scope.ApproveDate == 0 && $scope.CompletedDate == 0 && $scope.YetToApprove == false && $scope.YetToIssue == false && $scope.YetToReceive == false) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }
                var sTempString = $scope.cboOrderDate + '~' + $scope.OrderDateStart + '~' + $scope.OrderDateEnd + '~'
                                + $scope.cboReceiveDate + '~' + $scope.ReceiveDateStart + '~' + $scope.ReceiveDateEnd + '~'
                                + $scope.cboApproveDate + '~' + $scope.ApproveDateStart + '~' + $scope.ApproveDateEnd + '~'
                                + $scope.cboCompletedDate + '~' + $scope.CompletedDateStart + '~' + $scope.CompletedDateEnd + '~'
                                + ($scope.OrderNo == undefined ? "" : $scope.OrderNo) + '~' + ($scope.LotNo == undefined ? "" : $scope.LotNo) + '~'
                                + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive) + '~'
                                + $scope.OrderType + '~' + sessionStorage.getItem('BUID');

                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                window.open(_sBaseAddress + '/Twisting/PrintTwistings?sValue='+sTempString);
            };
            $scope.printTwistingsXL = function () {
                debugger;
                $scope.lblLoadingMessage = false;
                var nVal = $scope.OrderType;
                if ($scope.OrderNo == "" && $scope.LotNo == "" && $scope.OrderType == 0 && $scope.cboOrderDate == 0 && $scope.cboReceiveDate == 0 && $scope.ApproveDate == 0 && $scope.CompletedDate == 0 && $scope.YetToApprove == false && $scope.YetToIssue == false && $scope.YetToReceive == false) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }
                var sTempString = $scope.cboOrderDate + '~' + $scope.OrderDateStart + '~' + $scope.OrderDateEnd + '~'
                                + $scope.cboReceiveDate + '~' + $scope.ReceiveDateStart + '~' + $scope.ReceiveDateEnd + '~'
                                + $scope.cboApproveDate + '~' + $scope.ApproveDateStart + '~' + $scope.ApproveDateEnd + '~'
                                + $scope.cboCompletedDate + '~' + $scope.CompletedDateStart + '~' + $scope.CompletedDateEnd + '~'
                                + ($scope.OrderNo == undefined ? "" : $scope.OrderNo) + '~' + ($scope.LotNo == undefined ? "" : $scope.LotNo) + '~'
                                + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive) + '~'
                                + $scope.OrderType + '~' + sessionStorage.getItem('BUID');

                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                window.open(_sBaseAddress + '/Twisting/PrintTwistingsXL?sValue=' + sTempString);
            };


            $scope.reset = function () {
                debugger;
                $scope.CompareOperators = obj.CompareOperators;
                $scope.cboOrderDate = $scope.CompareOperators[0].id;
                $scope.OrderDateEndDisabled = true;
                $scope.OrderDateStart = $scope.OrderDateEnd = icsdateformat(new Date());
                $scope.cboReceiveDate = $scope.CompareOperators[0].id;
                $scope.ReceiveDateEndDisabled = true;
                $scope.ReceiveDateStart = $scope.ReceiveDateEnd = icsdateformat(new Date());
                $scope.cboApproveDate = $scope.CompareOperators[0].id;
                $scope.ApproveDateEndDisabled = true;
                $scope.ApproveDateStart = $scope.ApproveDateEnd = icsdateformat(new Date());
                $scope.cboCompletedDate = $scope.CompareOperators[0].id;
                $scope.CompletedDateEndDisabled = true;
                $scope.CompletedDateStart = $scope.CompletedDateEnd = icsdateformat(new Date());
                $scope.OrderNo = "";
                $scope.LotNo = "";
                $scope.YetToApprove = $scope.YetToIssue = $scope.YetToReceive = false;
                $scope.OrderTypes = obj.OrderTypes;
                $scope.OrderType = 0;
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});