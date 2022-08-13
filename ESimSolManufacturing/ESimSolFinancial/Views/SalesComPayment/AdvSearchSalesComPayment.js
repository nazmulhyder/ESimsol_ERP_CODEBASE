var SULCcservice = angular.module('SalesComPayment.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
SULCcservice.service('SalesComPaymentservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/SalesComPayment/AdvSearchSalesCompayment',
            controller: 'ModalSalesComPaymentAdvanceSearchCtrl',
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

            $scope.ContractorName = '';
            $scope.ContractorIDs = '';
            $scope.lblLoadingMessage = true;
            $scope.PickerTitle = "Sales Commission Payment";

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
                                        var results =response.data;
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
            $scope.SearchKeyDownCPName = function (e) {
                debugger;

                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var CPName = $.trim($scope.CPName);
                    
                    $scope.PickCPName();
                } else if (code == 8) //backspace=8
                {
                    $scope.CPName = '';
                    $scope.CPIDs = '';
                }
            };

            $scope.PickCPName = function () {
               
                var sContractorID = '';
                if ($scope.ContractorIDs.length > 0) {
                    sContractorID = $scope.ContractorIDs;
                }
                debugger;
                var oBuyerConcern = {
                    ContractorName: sContractorID,
                    Name: $.trim($scope.CPName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/ContractorPersonal/GetsByName', $.param(oBuyerConcern), config).then(
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
                                            pickertitle: 'Person List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.CPName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            
                                            $scope.CPIDs = icsMethod.ICS_PropertyConcatation(result, 'ContactPersonnelID');
                                            
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };
           


            $scope.MRDateChange = function () {
                $scope.MRDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboMRDate);
            }
            

            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
             
                if ($scope.cboMRDate == 0 && ($scope.MRNo == undefined ? "" : $scope.MRNo) == "" && ($scope.DOCNo == undefined ? "" : $scope.DOCNo) == "" && ($scope.CPIDs == undefined ? "" : $scope.CPIDs) == "" && ($scope.ContractorIDs == undefined ? "" : $scope.ContractorIDs) == "") {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                   
                    $scope.lblLoadingMessage = false;
                    return false;
                }


                var sTempString = $scope.cboMRDate + '~' + $scope.MRDateStart + '~' + $scope.MRDateEnd + '~' + ($scope.MRNo == undefined ? "" : $scope.MRNo) + '~' + ($scope.DOCNo == undefined ? "" : $scope.DOCNo) + '~' + ($scope.CPIDs == undefined ? "" : $scope.CPIDs) + '~' + ($scope.ContractorIDs == undefined ? "" : $scope.ContractorIDs) + '~' + sessionStorage.getItem('BUID');
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(_sBaseAddress + '/SalesComPayment/AdvSearch', { params: { sTemp: sTempString } }, config).then(
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
                $scope.MRNo = $scope.DOCNo = $scope.CPName = "";
                $scope.CPIDs = "";
           
           
                $scope.cboMRDate = $scope.CompareOperators[0].id;
                $scope.MRDateEndDisabled = true;
                $scope.MRDateStart = $scope.MRDateEnd = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});