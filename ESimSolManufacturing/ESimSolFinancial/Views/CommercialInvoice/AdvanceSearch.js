
var commercialinvoiceservice = angular.module('ciAdvanceSearch.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
commercialinvoiceservice.service('ciAdvanceSearchservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl:sessionStorage.getItem('BaseAddress') + '/Commercialinvoice/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        BussinessSessions: (modalProperty.BussinessSessions == undefined) ? [] : modalProperty.BussinessSessions,
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        InvoiceStatusList: (modalProperty.InvoiceStatusList == undefined) ? [] : modalProperty.InvoiceStatusList
                    };
                }
                
            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants,msModal,icsMethod, obj) {
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
            $scope.PickerTitle ="Commercial Invoice Search";
            $scope.SearchKeyDownBuyer = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var BuyerName = $.trim($scope.BuyerName);
                    if (BuyerName == "" || BuyerName == null)
                    {
                        alert("Type Buyer Name and Press Enter");
                        return;
                    }
                    $scope.PickBuyer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.BuyerName = '';
                    $scope.Buyer = 0;
                }
            };
            $scope.PickBuyer = function () {
                debugger;
                var oContractor = {
                    Params: '2' + '~' + $.trim($scope.BuyerName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
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
                                            appcontroller: 'commercialinvoiceController',
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
                                            $scope.BuyerIDs =icsMethod.ICS_PropertyConcatation(result,'ContractorID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.SearchKeyDownAdviceBank = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var AdviceBankName = $.trim($scope.AdviceBankName);
                    if (AdviceBankName == "" || AdviceBankName == null) {
                        alert("Type Advice bank Account and Press Enter");
                        return;
                    }
                    $scope.AdviceBank();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope. AdviceBankName = '';
                    $scope. AdviceBankID = 0;
                }
            };
            $scope.AdviceBank = function () {
                var oBankAccount = {
                    BusinessUnitID: sessionStorage.getItem('BUID'),
                    OperationalDeptInInt: '4',//EnumOperationalDept : Export_Own=4
                    AccountNo: $.trim($scope. AdviceBankName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/BankAccount/GetsBUWithDeptWise', $.param(oBankAccount), config).then(
                            function (response) {
                                var oColumns = [];
                                var oColumn = { field: 'BankNameAccountNo', name: 'Account No', width: '20%' }; oColumns.push(oColumn);
                                oColumn = { field: 'BranchName', name: 'Branch Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                var results = jQuery.parseJSON(response.data);
                                var modalObj = {
                                    size: 'md',
                                    modalcontroller: 'ModalCommonListCtrl',
                                    appcontroller: 'commercialinvoiceController',
                                    objs: results,
                                    multiSelect: true,
                                    pickertitle: 'Bank Account List',
                                    searchingbyfieldName: 'BankNameAccountNo',
                                    columns: oColumns
                                }
                                var modalInstance = msModal.Instance(modalObj);
                                modalInstance.result.then(function (result) {
                                    debugger;
                                    $scope.AdviceBankName = result.length > 1 ? result[0].BankNameAccountNo : result.length+" Bank Accounts Selected";
                                    $scope.BankAccountIDs = icsMethod.ICS_PropertyConcatation(result, 'BankAccountID');

                                }, function () {
                                    $log.info('Modal dismissed at: ' + new Date());
                                });
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );

            };

            $scope.SearchKeyDownIssueBank = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var IssueBankName = $.trim($scope.IssueBankName);
                    if (IssueBankName == "" || IssueBankName == null) {
                        alert("Type Issue bank Name and Press Enter");
                    }
                    $scope.IssueBank();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.IssueBankName = '';
                    $scope.IssueBankID = 0;
                }
            };
            $scope.IssueBank = function () {
                var oBankBranch = {
                    BUID: sessionStorage.getItem('BUID'),
                    DeptIDs: '5',//EnumOperationalDept : Export_Party=5
                    BankName: $.trim($scope.IssueBankName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/BankBranch/GetsBankBranchSearchByBankName', $.param(oBankBranch), config).then(
                            function (response) {
                                var oColumns = [];
                                var oColumn = { field: 'BankName', name: 'Bank Name', width: '20%' }; oColumns.push(oColumn);
                                oColumn = { field: 'BranchName', name: 'Branch Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                var results = jQuery.parseJSON(response.data);
                                var modalObj = {
                                    size: 'md',
                                    modalcontroller: 'ModalCommonListCtrl',
                                    appcontroller: 'MasterLCController',
                                    objs: results,
                                    multiSelect: true,
                                    pickertitle: 'Bank List',
                                    searchingbyfieldName: 'Name',
                                    columns: oColumns
                                }
                                var modalInstance = msModal.Instance(modalObj);
                                modalInstance.result.then(function (result) {
                                    debugger;
                                    $scope.IssueBankName = result.length > 1 ? result[0].BankName : result.length + " Bank Accounts Selected"; 
                                    $scope.IssueBankIDs = icsMethod.ICS_PropertyConcatation(result, 'BankID'); 

                                }, function () {
                                    $log.info('Modal dismissed at: ' + new Date());
                                });
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );

            };


            $scope.InvoiceDateChange = function()
            {
                $scope.InvoiceDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboInvoiceDate);
            }
            $scope.LCRcvDateChange = function () {
                $scope.LCRcvDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboLCRcvDate);
            }
            $scope.SendToBuyerDateChange = function () {
                $scope.SendToBuyerDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboSendToBuyerDate);
            }
            $scope.BuyerAcceptDateChange = function () {
                $scope.BuyerAcceptDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboBuyerAcceptDate);
            }
            $scope.EnCashmentDateChange = function () {
                $scope.EnCashmentDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboEnCashmentDate);
            }
            $scope.BillAmountChange = function () {
                $scope.BillAmountEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboBillAmount);
            }
            $scope.DiscrepencyChange = function () {
                $scope.DiscrepencyEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboDiscrepency);
            }
            $scope.search = function ()
            {
                debugger;
                $scope.lblLoadingMessage = false;
                if ($scope.cboInvoiceDate == 0 && $scope.cboSendToBuyerDate == 0 && $scope.cboBuyerAcceptDate == 0 && $scope.cboLCRcvDate == 0 && $scope.cboEnCashmentDate == 0 && $scope.cboBillAmount == 0 && $scope.cboDiscrepency == 0 && ($scope.BankAccountIDs == undefined ? "" : $scope.BankAccountIDs) == "" && ($scope.IssueBankIDs == undefined ? "" : $scope.IssueBankIDs) == "" && $scope.cboSession == 0 && ($scope.LCNo == undefined ? "" : $scope.LCNo) == "" && ($scope.BuyerIDs == undefined ? "" : $scope.BuyerIDs) == "" && ($scope.InvoiceNo == undefined ? "" : $scope.InvoiceNo) == "" && $scope.gridApi.selection.getSelectedRows().length== 0)
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                var sTempString = $scope.cboInvoiceDate + '~' + $scope.InvoiceDateStart + '~' + $scope.InvoiceDateEnd + '~' + $scope.cboSendToBuyerDate + '~' + $scope.SendToBuyerDateStart + '~' + $scope.SendToBuyerDateEnd + '~' + $scope.cboBuyerAcceptDate + '~' + $scope.BuyerAcceptDateStart + '~' + $scope.BuyerAcceptDateEnd + '~' + $scope.cboLCRcvDate + '~' + $scope.LCRcvDateStart + '~' + $scope.LCRcvDateEnd + '~' + $scope.cboEnCashmentDate + '~' + $scope.EnCashmentDateStart + '~' + $scope.EnCashmentDateEnd + '~' + $scope.cboBillAmount + '~' + $scope.BillAmountStart + '~' + $scope.BillAmountEnd + '~' + $scope.cboDiscrepency + '~' + $scope.DiscrepencyStart + '~' + $scope.DiscrepencyEnd + '~' + ($scope.LCNo == undefined ? "" : $scope.LCNo) + '~' + ($scope.InvoiceNo == undefined ? "" : $scope.InvoiceNo) + '~' + ($scope.BuyerIDs == undefined ? "" : $scope.BuyerIDs) + '~' + ($scope.BankAccountIDs == undefined ? "" : $scope.BankAccountIDs) + '~' + ($scope.IssueBankIDs == undefined ? "" : $scope.IssueBankIDs) + '~' + ($scope.cboSession == undefined ? 0 : $scope.cboSession) + '~' +icsMethod.ICS_PropertyConcatation( $scope.gridApi.selection.getSelectedRows(),'id') + '~' + sessionStorage.getItem('BUID');
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(sessionStorage.getItem('BaseAddress') + '/CommercialInvoice/Search', { params: { sTemp: sTempString } }, config).then(
                            function (response)
                            {
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.length > 0)
                                {
                                    if (results[0].ErrorMessage == "" || results[0].ErrorMessage == null)
                                    {
                                        $uibModalInstance.close(results);
                                    } else {
                                        alert(results[0].ErrorMessage);
                                        return;
                                    }
                                    
                                } else {
                                    alert("Data Not Found.");
                                    return;
                                }
                                
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );
            };
            $scope.reset = function ()
            {
                $scope.Sessions = obj.BussinessSessions;//Load BussinessSessions
                $scope.CompareOperators = obj.CompareOperators;
                $scope.gridOptionsStatus = {
                    enableRowSelection: true,
                    enableSelectAll: true,
                    multiSelect: true,
                    enableRowSelection: true,
                    columnDefs: [
                      { field: 'Value', name: 'Invoice Status', width: '90%' }
                    ],
                    data: obj.InvoiceStatusList,
                    onRegisterApi: function (gridApi) {
                        debugger;
                        $scope.gridApi = gridApi;
                    }
                };

                $scope.LCNo = $scope.InvoiceNo = $scope.BuyerName = $scope.AdviceBankName = '';
                $scope.BuyerIDs = "";
                $scope.IssueBankIDs=$scope.BankAccountIDs = "";
                $scope.cboSession = $scope.Sessions[$scope.Sessions.length - 1].BusinessSessionID;
                $scope.cboInvoiceDate = $scope.cboSendToBuyerDate = $scope.cboBuyerAcceptDate = $scope.cboLCRcvDate = $scope.cboEnCashmentDate = $scope.cboBillAmount = $scope.cboDiscrepency = $scope.CompareOperators[0].id;
                $scope.InvoiceDateEndDisabled = $scope.LCRcvDateEndDisabled = $scope.SendToBuyerDateEndDisabled = $scope.BuyerAcceptDateEndDisabled = $scope.EnCashmentDateEndDisabled = $scope.BillAmountEndDisabled = $scope.DiscrepencyEndDisabled = true;
                $scope.InvoiceDateStart = $scope.InvoiceDateEnd = $scope.LCRcvDateStart = $scope.LCRcvDateEnd = $scope.BuyerAcceptDateStart = $scope.BuyerAcceptDateEnd = $scope.SendToBuyerDateStart = $scope.SendToBuyerDateEnd = $scope.EnCashmentDateStart = $scope.EnCashmentDateEnd = icsdateformat(new Date());
                $scope.BillAmountStart = $scope.BillAmountEnd = $scope.DiscrepencyStart = $scope.DiscrepencyEnd = 0;
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




