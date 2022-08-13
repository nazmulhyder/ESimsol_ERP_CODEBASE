
var RMRequisitionservice = angular.module('RMAdvanceSearch.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
RMRequisitionservice.service('RMAdvanceSearchservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl:sessionStorage.getItem('BaseAddress')+'/RMRequisition/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        WorkingUnits: (modalProperty.WorkingUnits == undefined) ? [] : modalProperty.WorkingUnits,
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators
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
            $scope.PickerTitle ="Requisition Search";
            $scope.SearchKeyDownProduct = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ProductName = $.trim($scope.ProductName);
                    if (ProductName == "" || ProductName == null)
                    {
                        alert("Type Product Name and Press Enter");
                        return;
                    }
                    $scope.PickProduct();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.ProductName = '';
                    $scope.ProductIDs = '';
                }
            };
            $scope.PickProduct = function () {
                debugger;
                var oProduct = {
                    BUID: sessionStorage.getItem('BUID'),
                    ProductName: $.trim($scope.ProductName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/ProductionSheet/GetProducts', $.param(oProduct), config).then(
                            function (response) {
                                var oColumns = [];
                                var oColumn = { field: 'ProductCode', name: 'Code', width: '30%' }; oColumns.push(oColumn);
                                oColumn = { field: 'ProductName', name: 'Product Name', width: '40%', enableSorting: false }; oColumns.push(oColumn);
                                oColumn = { field: 'MUnitName', name: 'Unit Name', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'RMRequisitionController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Product List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.ProductName = result.length > 1 ? result.length + "Item's Selected" : result[0].ProductName;
                                            $scope.ProductIDs =icsMethod.ICS_PropertyConcatation(result,'ProductID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.SearchKeyDownProductionSheet = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var SheetNo = $.trim($scope.SheetNo);
                    if (SheetNo == "" || SheetNo == null) {
                        alert("Type Advice bank Account and Press Enter");
                        return;
                    }
                    $scope.ProductionSheet();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.SheetNo= '';
                    $scope.ProductionSheetIDs= 0;
                }
            };
            $scope.ProductionSheet = function () {
                var oProductionSheet = {
                    BUID: sessionStorage.getItem('BUID'),
                    SheetNo: $.trim($scope.SheetNo)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/ProductionSheet/GetsProductionSheets', $.param(oProductionSheet), config).then(
                            function (response) {
                                var oColumns = [];
                                var oColumn = { field: 'SheetNo', name: 'Sheet No', width: '30%' }; oColumns.push(oColumn);
                                oColumn = { field: 'ModelReferencenName', name: 'Machine Name', width: '40%', enableSorting: false }; oColumns.push(oColumn);
                                oColumn = { field: 'ExportPINo', name: 'PI No', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                var results = jQuery.parseJSON(response.data);
                                var modalObj = {
                                    size: 'md',
                                    modalcontroller: 'ModalCommonListCtrl',
                                    appcontroller: 'RMRequisitionController',
                                    objs: results,
                                    multiSelect: true,
                                    pickertitle: 'Production Sheet List',
                                    searchingbyfieldName: 'SheetNo',
                                    columns: oColumns
                                }
                                var modalInstance = msModal.Instance(modalObj);
                                modalInstance.result.then(function (result) {
                                    debugger;
                                    $scope.SheetNo = result.length>1 ? result.length + " Sheet's Selected" : result[0].SheetNo;
                                    $scope.ProductionSheetIDs = icsMethod.ICS_PropertyConcatation(result,'ProductionSheetID');

                                }, function () {
                                    $log.info('Modal dismissed at: ' + new Date());
                                });
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );

            };

         
            $scope.RequisitionDateChange = function()
            {
                $scope.RequisitionDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboRequisitionDate);
            }
           
            $scope.search = function ()
            {
                debugger;
                $scope.lblLoadingMessage = false;
                if ($scope.cboRequisitionDate == 0 && ($scope.ProductionSheetIDs == undefined ? "" : $scope.ProductionSheetIDs) == ""  && ($scope.RefNo == undefined ? "" : $scope.RefNo) == "" && ($scope.ProductIDs == undefined ? "" : $scope.ProductIDs) == "")
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                var sTempString = $scope.cboRequisitionDate + '~' + $scope.RequisitionDateStart + '~' + $scope.RequisitionDateEnd + '~' + ($scope.RefNo == undefined ? "" : $scope.RefNo) + '~' + ($scope.ProductIDs == undefined ? "" : $scope.ProductIDs) + '~' + ($scope.ProductionSheetIDs == undefined ? "" : $scope.ProductionSheetIDs)  + '~' + sessionStorage.getItem('BUID') ;
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(sessionStorage.getItem('BaseAddress') + '/RMRequisition/Search', { params: { sTemp: sTempString } }, config).then(
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
                $scope.CompareOperators = obj.CompareOperators;
                $scope.RefNo = $scope.ProductName = $scope.SheetNo = '';
                $scope.ProductIDs = $scope.ProductionSheetIDs = "";   
                $scope.cboRequisitionDate =  $scope.CompareOperators[0].id;
                $scope.RequisitionDateEndDisabled =  true;
                $scope.RequisitionDateStart = $scope.RequisitionDateEnd = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }
});




