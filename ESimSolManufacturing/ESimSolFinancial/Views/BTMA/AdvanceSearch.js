debugger;
var BTMAservice = angular.module('BTMA.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
BTMAservice.service('advsearchBTMAService', function ($uibModal) {

    debugger;
   
    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/BTMA/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function ()
                {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        VehicleTypes: (modalProperty.VehicleTypes == undefined) ? [] : modalProperty.VehicleTypes,
                        BusinessUnits: (modalProperty.BusinessUnits == undefined) ? [] : modalProperty.BusinessUnits,
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
            $scope.PickerTitle ="BTMA Search";
            $scope.SearchKeyDownSupplier = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var SupplierName = $.trim($scope.SupplierName);
                    if (SupplierName == "" || SupplierName == null)
                    {
                        alert("Type Supplier Name and Press Enter");
                        return;
                    }
                    $scope.PickSupplier();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.SupplierName = '';
                    $scope.SupplierIDs ='';
                }
            };
            $scope.PickSupplier = function () {
                debugger;
                var oContractor = {
                    Params: '1' + '~' + $.trim($scope.SupplierName) + '~' + _nBUID
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Supplier Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'BTMAController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Supplier List',
                                            searchingbyfieldName: 'Name',
                                            title:'abc',
                                            columns: oColumns
                                        }
                                           var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.SupplierName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            $scope.SupplierIDs =icsMethod.ICS_PropertyConcatation(result,'ContractorID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.LCDateChange = function()
            {
                $scope.txtLCDateFrom = $scope.txtLCDateTo = icsdateformat(new Date());
                $scope.txtLCDateToDisabled = icsMethod.IsDateBoxdisabled($scope.cboLCDate);
            }
            $scope.ExpiryDateChange = function () {
                $scope.txtExpiryDateFrom = $scope.txtExpiryDateTo = icsdateformat(new Date());
                $scope.txtExpiryDateToDisabled = icsMethod.IsDateBoxdisabled($scope.cboExpiryDate);
            }
            $scope.InvoiceDateChange = function () {
                $scope.txtInvoiceDateFrom = $scope.txtInvoiceDateTo = icsdateformat(new Date());
                $scope.txtInvoiceDateToDisabled = icsMethod.IsDateBoxdisabled($scope.cboInvoiceDate);
            }
            $scope.MushakDateChange = function () {
                $scope.txtMushakDateFrom = $scope.txtMushakDateTo = icsdateformat(new Date());
                $scope.txtMushakDateToDisabled = icsMethod.IsDateBoxdisabled($scope.cboMushakDate);
            }
            $scope.GatePassDateChange = function () {
                $scope.txtGatePassDateFrom = $scope.txtGatePassDateTo = icsdateformat(new Date());
                $scope.txtGatePassDateToDisabled = icsMethod.IsDateBoxdisabled($scope.cboGatePassDate);
            }

            function GetData(data)
            {
                data=(data == undefined ? "" : data);
                return data;
            }
            $scope.search = function ()
            {
                debugger;
                //($scope.VehicleTypeInInt == undefined ? 0 : $scope.VehicleTypeInInt) <= 0 &&
                if ($scope.cboLCDate == 0 && $scope.cboInvoiceDate == 0 && $scope.cboMushakDate == 0 && $scope.cboGatePassDate == 0 && $scope.cboExpiryDate == 0
                    && GetData($scope.ExportLCNo) == "" && GetData($scope.ExportBillNo) == "" && GetData($scope.MLCNo) == "" && GetData($scope.MushakNo) == "" && GetData($scope.GatePassNo) == ""
                    && GetData($scope.SupplierIDs) == "")
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                $scope.lblLoadingMessage = false;
                var sTempString = $scope.cboLCDate + '~' + $scope.txtLCDateFrom + '~' + $scope.txtLCDateTo + '~'
                    + $scope.cboExpiryDate + '~' + $scope.txtExpiryDateFrom + '~' + $scope.txtExpiryDateTo + '~'
                    + $scope.cboInvoiceDate + '~' + $scope.txtInvoiceDateFrom + '~' + $scope.txtInvoiceDateTo + '~'
                    + $scope.cboMushakDate + '~' + $scope.txtMushakDateFrom + '~' + $scope.txtMushakDateTo + '~'
                    + $scope.cboGatePassDate + '~' + $scope.txtGatePassDateFrom + '~' + $scope.txtGatePassDateTo + '~'
                    + ($scope.SupplierIDs == undefined ? "" : $scope.SupplierIDs);

                var oBTMA = {
                    MasterLCNos: GetData($scope.MLCNo),
                    ExportLCNo: GetData($scope.ExportLCNo),
                    ExportBillNo: GetData($scope.ExportBillNo),
                    MushakNo: GetData($scope.MushakNo),
                    GatePassNo: GetData($scope.GatePassNo),
                    ErrorMessage: sTempString
                }
                console.log(oBTMA);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/BTMA/AdvSearch', JSON.stringify(oBTMA)).then(
                            function (response)
                            {
                                $scope.lblLoadingMessage = true;
                                var results = (response.data);
                                if (results.length > 0)
                                {
                                    if (results[0].ErrorMessage == '')
                                        $uibModalInstance.close(results);
                                    else
                                        alert(results[0].ErrorMessage);
                                }
                                else
                                {
                                    alert("Data Not Found.");
                                    return;
                                }
                                
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );
            };
            $scope.reset = function ()
            {
               // //$scope.Sessions = obj.BussinessSessions;//Load BussinessSessions
                $scope.CompareOperators = obj.CompareOperators;
                $scope.BUID = 0; $scope.SLNo = $scope.LCNo = "";
                $scope.cboLCDate = $scope.CompareOperators[0].Value;
                $scope.txtLCDateFrom = $scope.txtLCDateTo = icsdateformat(new Date());
                $scope.cboExpiryDate = $scope.CompareOperators[0].Value;
                $scope.txtExpiryDateFrom = $scope.txtExpiryDateTo = icsdateformat(new Date());
                $scope.cboInvoiceDate = $scope.CompareOperators[0].Value;
                $scope.txtInvoiceDateFrom = $scope.txtInvoiceDateTo = icsdateformat(new Date());

                $scope.cboMushakDate = $scope.CompareOperators[0].Value;
                $scope.txtMushakDateFrom = $scope.txtMushakDateTo = icsdateformat(new Date());
               
                $scope.cboGatePassDate = $scope.CompareOperators[0].Value;
                $scope.txtGatePassDateFrom = $scope.txtGatePassDateTo = icsdateformat(new Date());
                $scope.txtMushakDateToDisabled = $scope.txtLCDateToDisabled =$scope.txtInvoiceToDisabled = $scope.txtInvoiceDateToDisabled =$scope.txtGatePassDateToDisabled = true;
                debugger;
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




