var SULCcservice = angular.module('SCLC.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
SULCcservice.service('SCLCservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/SalesCommissionLC/AdvSearchSCLC',
            controller: 'ModalSCLCAdvanceSearchCtrl',
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
            
            $scope.chkYetToDistribute = $scope.chkYetToPayable = $scope.chkYetToPaid = $scope.chkMaturityReceived = $scope.chkPaymentReceived = false;
            $scope.lblLoadingMessage = true;
            $scope.PickerTitle = " Sales Commission ";
            $scope.SearchKeyDownBuyerName = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var BuyerName = $.trim($scope.BuyerName);
                    if (BuyerName == "" || BuyerName == null) {
                        //alert("Type Buyer Name and Press Enter");
                        msModal.Message({ headerTitle: '', bodyText: 'Type Buyer Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                        return;
                    }
                    $scope.PickBuyer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.BuyerName = '';
                    $scope.BuyerIDs = '';
                }
            };
            $scope.PickBuyer = function () {
                debugger;
                var oContractor = {
                    Params: '2' + '~' + $.trim($scope.BuyerName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Buyer Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'SCLCController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Buyer List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.BuyerName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            $scope.BuyerIDs = icsMethod.ICS_PropertyConcatation(result, 'ContractorID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.SearchKeyDownContractorName = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ContractorName = $.trim($scope.ContractorName);
                    if (ContractorName == "" || ContractorName == null) {
                        // alert("Type ContractorName Name and Press Enter");
                        msModal.Message({ headerTitle: '', bodyText: 'Type Contractor Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                        return;
                    }
                    $scope.PickContractor();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.ContractorName = '';
                    $scope.ContractorIDs = '';
                }
            };
            $scope.PickContractor = function () {
                debugger;
                var oContractor = {
                    Params: '3' + '~' + $.trim($scope.ContractorName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Contractor Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'SCLCController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Contractor List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.ContractorName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            $scope.ContractorIDs = icsMethod.ICS_PropertyConcatation(result, 'ContractorID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };
           

            $scope.PIDateChange = function () {
                $scope.PIDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboPIDate);
            }
            $scope.AmmendentDateChange = function () {
                $scope.AmendmentDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboAmendmentDate);
            }
            $scope.LCOpenDateChange = function () {
                $scope.LCOpenDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboLCOpenDate);
            }
           
            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
                if ($scope.cboPIDate == 0 && $scope.cboLCOpenDate == 0 && $scope.cboAmendmentDate == 0 && ($scope.LCNo == undefined ? "" : $scope.LCNo) == "" && ($scope.PINo == undefined ? "" : $scope.PINo) == "" && $scope.chkYetToDistribute == false && $scope.chkYetToPayable == false && $scope.chkYetToPaid == false && $scope.chkMaturityReceived == false && $scope.chkPaymentReceived == false && ($scope.BuyerIDs == undefined ? "" : $scope.BuyerIDs) == "" && ($scope.ContractorIDs == undefined ? "" : $scope.ContractorIDs) == "")
                {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    //alert("Please Select at least one Criteria !!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }
                
                 
                var sTempString = $scope.cboPIDate + '~' + $scope.PIDateStart + '~' + $scope.PIDateEnd + '~' + $scope.cboLCOpenDate + '~' + $scope.LCOpenDateStart + '~' + $scope.LCOpenDateEnd + '~' + $scope.cboAmendmentDate + '~' + $scope.AmendmentDateStart + '~' + $scope.AmendmentDateEnd + '~' + ($scope.PINo == undefined ? "" : $scope.PINo) + '~' + ($scope.LCNo == undefined ? "" : $scope.LCNo) + '~' + ($scope.BuyerIDs == undefined ? "" : $scope.BuyerIDs) + '~' + ($scope.ContractorIDs == undefined ? "" : $scope.ContractorIDs) + '~' + $scope.chkYetToDistribute + '~' + $scope.chkYetToPayable + '~' + $scope.chkYetToPaid + '~' + $scope.chkMaturityReceived + '~' + $scope.chkPaymentReceived +'~'+ sessionStorage.getItem('BUID');
              
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(_sBaseAddress + '/SalesCommissionLC/AdvSearch', { params: { sTemp: sTempString } }, config).then(
                            function (response) {
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.length > 0) {
                                    $uibModalInstance.close(results);
                                } else {
                                    msModal.Message({ headerTitle: '', bodyText: 'No Data Found !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                                   // alert("Data Not Found.");
                                    return;
                                }

                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );
            };
            $scope.reset = function () {
              
                $scope.CompareOperators = obj.CompareOperators;
                $scope.LCNo = $scope.PINo = $scope.ContractorName = $scope.BuyerName ="";
                $scope.BuyerIDs = "";
                $scope.ContractorIDs ="";
                $scope.chkYetToDistribute = $scope.chkYetToPayable = $scope.chkYetToPaid = $scope.chkMaturityReceived = $scope.chkPaymentReceived = false;
                $scope.cboPIDate = $scope.cboLCOpenDate = $scope.cboAmendmentDate= $scope.CompareOperators[0].id;
                $scope.PIDateEndDisabled = $scope.LCOpenDateEndDisabled = $scope.AmendmentDateEndDisabled = true;
                $scope.PIDateStart = $scope.PIDateEnd = $scope.LCOpenDateStart = $scope.LCOpenDateEnd= $scope.AmendmentDateStart = $scope.AmendmentDateEnd =icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
          
        }
        return $uibModal.open(modalInstance);
    }


});