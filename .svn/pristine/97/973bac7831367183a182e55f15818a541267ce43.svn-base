var IICService = angular.module('DUProGuideLine.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
IICService.service('DUProGuideLineservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/DUProGuideLine/AdvSearchDUProGuideLine',
            controller: 'ModalDUProGuideLineAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        OrderTypes: (modalProperty.OrderTypes == undefined) ? [] : modalProperty.OrderTypes,
                        ProductTypes: (modalProperty.ProductTypes == undefined) ? [] : modalProperty.ProductTypes,
                        ReceiveStores: (modalProperty.ReceiveStores == undefined) ? [] : modalProperty.ReceiveStores,
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

            $scope.ProductionDateChange = function () {
                $scope.ProductionDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboProductionDate);
            }
            
            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
             
                if ($scope.OrderNo == "" && $scope.SLNo == "" && $scope.LotNo == "" && $scope.cboProductionDate == 0 && $scope.cboProductType == 0 && $scope.cboReceiveStore == 0 && $scope.cboOrderType == 0) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }

                var sTempString = $scope.cboProductionDate + '~' + $scope.ProductionDateStart + '~' + $scope.ProductionDateEnd + '~' + ($scope.cboReceiveStore == undefined ? 0 : $scope.cboReceiveStore) + '~' + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive) + '~' + sessionStorage.getItem('BUID') + '~' + ($scope.cboOrderType == undefined ? 0 : $scope.cboOrderType) + '~' + ($scope.OrderNo == undefined ? "" : $scope.OrderNo) + '~' + ($scope.cboProductType == undefined ? 0 : $scope.cboProductType) + '~' + ($scope.SLNo == undefined ? "" : $scope.SLNo) + '~' + ($scope.LotNo == undefined ? "" : $scope.LotNo);
                
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(_sBaseAddress + '/DUProGuideLine/AdvSearch', { params: { sTemp: sTempString } }, config).then(
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
                $scope.ProductTypes = obj.ProductTypes;
                $scope.ReceiveStores = obj.ReceiveStores;
                $scope.OrderTypes = obj.OrderTypes;
                $scope.UserIDs = $scope.UserName = $scope.OrderNo = $scope.SLNo = "";
                $scope.cboProductionDate = $scope.CompareOperators[0].id;
                $scope.ProductionDateEndDisabled = true;
                $scope.ProductionDateStart = $scope.ProductionDateEnd = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});