var IICService = angular.module('ServiceOrder.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
IICService.service('ServiceOrderservice', function ($uibModal) {
    debugger;
    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/ServiceOrder/AdvSearchServiceOrder',
            controller: 'ModalServiceOrderAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        ServiceOrderTypes: (modalProperty.ServiceOrderTypes == undefined) ? [] : modalProperty.ServiceOrderTypes,
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

            $scope.ServiceOrderDateChange = function () {
                $scope.ServiceOrderDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboServiceOrderDate);
            }
            
            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
             
                if ($scope.OrderNo == "" && $scope.RegNo == "" && $scope.cboServiceOrderDate == 0 && $scope.cboServiceOrderType == 0) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }

                var sTempString = $scope.cboServiceOrderDate + '~' + $scope.ServiceOrderDateStart + '~' + $scope.ServiceOrderDateEnd + '~' +  + sessionStorage.getItem('BUID') + '~' + ($scope.cboServiceOrderType == undefined ? 0 : $scope.cboServiceOrderType) + '~' + ($scope.OrderNo == undefined ? "" : $scope.OrderNo) + '~' +  ($scope.RegNo == undefined ? "" : $scope.RegNo);
                
                var oServiceOrder = {
                    Params: sTempString
                };
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/ServiceOrder/AdvSearch', JSON.stringify(oServiceOrder), config).then(
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
                $scope.ServiceOrderTypes = obj.ServiceOrderTypes;
                $scope.UserIDs = $scope.UserName = $scope.OrderNo = $scope.RegNo = "";
                $scope.cboServiceOrderDate = 0;//$scope.CompareOperators[0].id;
                $scope.ServiceOrderDateEndDisabled = true;
                $scope.ServiceOrderDateStart = $scope.ServiceOrderDateEnd = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});