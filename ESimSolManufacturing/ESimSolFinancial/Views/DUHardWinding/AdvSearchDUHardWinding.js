var IICService = angular.module('DUHardWinding.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
IICService.service('DUHardWindingservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/DUHardWinding/AdvSearchDUHardWinding',
            controller: 'ModalDUHardWindingAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators
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

            $scope.QCDateChange = function () {
                $scope.QCDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboQCDate);
            }

            $scope.ReceiveDateChange = function () {
                $scope.ReceiveDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboReceiveDate);
            }

            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
             
                if ($scope.OrderNo == "" && $scope.LotNo == "" && $scope.cboOrderDate == 0 && $scope.cboQCDate == 0 && $scope.cboReceiveDate == 0 && $scope.YetToApprove == false && $scope.YetToIssue == false && $scope.YetToReceive == false && $scope.YetToColorAssign == false && $scope.IsGreyYarn == false && $scope.IsRewinding == false) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }

                var sTempString = $scope.cboOrderDate + '~' + $scope.OrderDateStart + '~' + $scope.OrderDateEnd + '~'
                                + $scope.cboReceiveDate + '~' + $scope.ReceiveDateStart + '~' + $scope.ReceiveDateEnd + '~'
                                + $scope.cboQCDate + '~' + $scope.QCDateStart + '~' + $scope.QCDateEnd + '~'
                                + ($scope.OrderNo == undefined ? "" : $scope.OrderNo) + '~' + ($scope.LotNo == undefined ? "" : $scope.LotNo) + '~'
                                + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive) + '~' + false
                                + '~' + ($scope.YetToColorAssign == undefined ? false : $scope.YetToColorAssign)
                                + '~' + ($scope.IsRewinding == undefined ? false : $scope.IsRewinding)
                                + '~' + ($scope.IsGreyYarn == undefined ? false : $scope.IsGreyYarn)
                                + '~' +sessionStorage.getItem('BUID') ;
                
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(_sBaseAddress + '/DUHardWinding/AdvSearch', { params: { sTemp: sTempString } }, config).then(
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
            $scope.reset = function () {
                debugger;
                $scope.CompareOperators = obj.CompareOperators;
                $scope.cboOrderDate = $scope.CompareOperators[0].id;
                $scope.OrderDateEndDisabled = true;
                $scope.OrderDateStart = $scope.OrderDateEnd = icsdateformat(new Date());
                $scope.cboReceiveDate = $scope.CompareOperators[0].id;
                $scope.ReceiveDateEndDisabled = true;
                $scope.ReceiveDateStart = $scope.ReceiveDateEnd = icsdateformat(new Date());
                $scope.cboQCDate = $scope.CompareOperators[0].id;
                $scope.QCDateEndDisabled = true;
                $scope.QCDateStart = $scope.QCDateEnd = icsdateformat(new Date());
                $scope.OrderNo = "";
                $scope.LotNo = "";
                $scope.IsGreyYarn = $scope.YetToColorAssign = $scope.YetToApprove = $scope.YetToIssue = $scope.YetToReceive = false;
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});