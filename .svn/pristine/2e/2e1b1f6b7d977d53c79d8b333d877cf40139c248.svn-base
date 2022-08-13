var SCPservice = angular.module('SCP.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
SCPservice.service('SCPservice', function ($uibModal) {
     
    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/SalesCommissionPayable/AdvSalesComissionPayable',
            controller: 'ModalSCPAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        SalesCommissionStatus: (modalProperty.SalesCommissionStatus == undefined) ? [] : modalProperty.SalesCommissionStatus
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
            $scope.PickerTitle = " Sales Commission Payable ";
            $scope.SearchKeyDownBuyerName = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var BuyerName = $.trim($scope.BuyerName);
                    //if (BuyerName == "" || BuyerName == null) {
                    //    //alert("Type Buyer Name and Press Enter");
                    //    msModal.Message({ headerTitle: '', bodyText: 'Type Buyer Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    //    return;
                    //}
                    $scope.PickBuyer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.BuyerName = '';
                    $scope.BuyerIDs = '';
                }
            };
        
            $scope.PickBuyer = function () {
                var sContractorID = '';
                if ($scope.ContractorIDs.length > 0) {
                    sContractorID = $scope.ContractorIDs;
                }
                debugger;
               
                var oBuyerConcern = {
                    ContractorName: sContractorID,
                    Name: $.trim($scope.BuyerName)
                        
                //  Params: '2' + '~' + $.trim($scope.BuyerName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/ContractorPersonal/GetsByContractors', $.param(oBuyerConcern), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContactPersonnelID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Personnel Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'ContractorName', name: 'Contractor Name', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'SalesCommissionPayableController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Buyer Concern List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.BuyerName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            $scope.BuyerIDs = icsMethod.ICS_PropertyConcatation(result, 'ContactPersonnelID');
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
                    Params: '2,3' + '~' + $.trim($scope.ContractorName) + '~' + sessionStorage.getItem('BUID')
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
                                            appcontroller: 'SalesCommissionPayableController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Party List',
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
            $scope.MaturityReceivedDateChange = function () {
                $scope.MaturityReceivedDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboMaturityRcvDate);
            }
            $scope.MaturityDateChange = function () {
                $scope.MaturityDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboMaturityDate);
            }
            $scope.RelizationDateChange = function () {
                $scope.RelizationDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboRelizationDate);
            }

            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
                if ( $scope.cboPIDate == 0 && $scope.cboLCOpenDate == 0 && $scope.cboAmendmentDate == 0 && $scope.cboMaturityRcvDate == 0 && $scope.cboMaturityDate == 0 && $scope.cboRelizationDate == 0 && ($scope.LCNo == undefined ? "" : $scope.LCNo) == "" && ($scope.PINo == undefined ? "" : $scope.PINo) == ""  &&  ($scope.BuyerIDs == undefined ? "" : $scope.BuyerIDs) == "" && ($scope.ContractorIDs == undefined ? "" : $scope.ContractorIDs) == "" && ($scope.LDBCNo == undefined ? "" : $scope.LDBCNo) == "" && ($scope.StatusID == undefined ? "" :$scope.StatusID)== 0) {
                   //msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select at least one Criteria !!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }


                var sTempString = $scope.cboPIDate + '~' + $scope.PIDateStart + '~' + $scope.PIDateEnd + '~' + $scope.cboLCOpenDate + '~' + $scope.LCOpenDateStart + '~' + $scope.LCOpenDateEnd + '~' + $scope.cboAmendmentDate + '~' + $scope.AmendmentDateStart + '~' + $scope.AmendmentDateEnd + '~' + $scope.cboMaturityRcvDate + '~' + $scope.MaturityReceivedDateStart + '~' + $scope.MaturityReceivedDateEnd + '~' + $scope.cboMaturityDate + '~' + $scope.MaturityDateStart + '~' + $scope.MaturityDateEnd + '~' + $scope.cboRelizationDate + '~' + $scope.RelizationDateStart + '~' + $scope.RelizationDateEnd + '~' + ($scope.PINo == undefined ? "" : $scope.PINo) + '~' + ($scope.LCNo == undefined ? "" : $scope.LCNo) + '~' + ($scope.BuyerIDs == undefined ? "" : $scope.BuyerIDs) + '~' + ($scope.ContractorIDs == undefined ? "" : $scope.ContractorIDs) + '~' + ($scope.LDBCNo == undefined ? "" : $scope.LDBCNo) + '~' + sessionStorage.getItem('BUID') + '~' + ($scope.StatusID == undefined ? 0 : $scope.StatusID);
                sessionStorage.setItem("sTempString", sTempString);

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
               
                $http.get(_sBaseAddress + '/SalesCommissionPayable/AdvSearchSCP', { params: { sTemp: sTempString } }, config).then(
                            function (response) {
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.Item1.length > 0) {
                                    $uibModalInstance.close(results);
                                } else {
                                    debugger;
                                    //msModal.Message({ headerTitle: '', bodyText: 'No Data Found !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false, templateUrl: _sBaseAddress +'/Home/ModalMessage' });
                                    alert("Data Not Found.");
                                    return;
                                }

                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );
            };
            $scope.reset = function () {

                $scope.CompareOperators = obj.CompareOperators; 
                $scope.SalesCommissionStatus = obj.SalesCommissionStatus;
                $scope.LCNo = $scope.PINo = $scope.ContractorName = $scope.BuyerName = $scope.LDBCNo = "";
                $scope.BuyerIDs = "";
                $scope.ContractorIDs = "";
                $scope.StatusID = $scope.SalesCommissionStatus[0].id;
                
                $scope.cboPIDate = $scope.cboLCOpenDate = $scope.cboAmendmentDate = $scope.cboMaturityRcvDate = $scope.cboMaturityDate = $scope.cboRelizationDate = $scope.CompareOperators[0].id;
                $scope.PIDateEndDisabled = $scope.LCOpenDateEndDisabled = $scope.AmendmentDateEndDisabled = $scope.MaturityReceivedDateEndDisabled = $scope.MaturityDateEndDisabled = $scope.RelizationDateEndDisabled = true;
                     
                $scope.PIDateStart = $scope.PIDateEnd = $scope.LCOpenDateStart = $scope.LCOpenDateEnd = $scope.AmendmentDateStart = $scope.AmendmentDateEnd = $scope.MaturityReceivedDateStart = $scope.MaturityReceivedDateEnd = $scope.MaturityDateStart = $scope.MaturityDateEnd = $scope.RelizationDateStart = $scope.RelizationDateEnd = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});