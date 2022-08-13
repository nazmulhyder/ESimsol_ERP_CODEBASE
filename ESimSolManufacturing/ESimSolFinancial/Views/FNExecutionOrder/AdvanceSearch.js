
var masterfnppservice = angular.module('FNExecutionOrder.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
masterfnppservice.service('fneoservice', function ($uibModal) {


    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/FNExecutionOrder/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        BussinessSessions: (modalProperty.BussinessSessions == undefined) ? [] : modalProperty.BussinessSessions,
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        FabricProcessTypes: (modalProperty.FabricProcessTypes == undefined) ? [] : modalProperty.FabricProcessTypes
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
            $scope.PickerTitle ="Order Search";
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
                    $scope.BuyerIDs ='';
                }
            };
            $scope.PickBuyer = function () {
                debugger;
                var oContractor = {
                    Params:'2~'+$.trim($scope.BuyerName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress')+'/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                     function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Buyer Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = (response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'mainController',
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

            $scope.SearchKeyDownFactory = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var FactoryName = $.trim($scope.FactoryName);
                    if (FactoryName == "" || FactoryName == null) {
                        alert("Type Factory Name and Press Enter");
                        return;
                    }
                    $scope.PickFactory();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.FactoryName = '';
                    $scope.FactoryIDs = '';
                }
            };
            $scope.PickFactory = function () {
                debugger;
                var oContractor = {
                    Params: '3~' + $.trim($scope.FactoryName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                     function (response) {
                         debugger;
                         var oColumns = [];
                         var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                         oColumn = { field: 'Name', name: 'Factory Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                         oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                         var results = (response.data);
                         var modalObj = {
                             size: 'md',
                             modalcontroller: 'ModalCommonListCtrl',
                             appcontroller: 'mainController',
                             objs: results,
                             multiSelect: true,
                             pickertitle: 'Factory List',
                             searchingbyfieldName: 'Name',
                             columns: oColumns
                         }
                         var modalInstance = msModal.Instance(modalObj);
                         modalInstance.result.then(function (result) {
                             debugger;
                             $scope.FactoryName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                             $scope.FactoryIDs = icsMethod.ICS_PropertyConcatation(result, 'ContractorID');
                         }, function () {
                             $log.info('Modal dismissed at: ' + new Date());
                         });
                     },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };
            $scope.IssueDateChange = function()
            {
                $scope.IssueDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboIssueDate);
            }
         
            $scope.search = function ()
            {
                debugger;
                $scope.lblLoadingMessage = false;
                if ($scope.cboIssueDate == 0 && $scope.cboFabricProcessType == 0 && ($scope.BuyerIDs == undefined ? "" : $scope.BuyerIDs) == "" && ($scope.FactoryIDs == undefined ? "" : $scope.FactoryIDs) == "" && $scope.SCNo == "" && $scope.DispoNo == "")
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }

                var sTempString = '~' + $scope.cboIssueDate + '~' + $scope.IssueDateStart + '~' + $scope.IssueDateEnd + '~' + ($scope.cboFabricProcessType == undefined ? 0 : $scope.cboFabricProcessType) + '~' + ($scope.BuyerIDs == undefined ? "" : $scope.BuyerIDs) + '~' + ($scope.FactoryIDs == undefined ? "" : $scope.FactoryIDs) + '~' + ($scope.chkApproved == undefined ? false : $scope.chkApproved) + '~' + ($scope.chkUnapproved == undefined ? false : $scope.chkUnapproved) + '~' + $scope.SCNo + '~' + $scope.DispoNo;


                var oFNExecutionOrderStatus = { Params: sTempString };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/FNExecutionOrder/GetsStatusForFNExONo', $.param(oFNExecutionOrderStatus), config).then(
                            function (response)
                            {
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.length > 0)
                                {
                                    $uibModalInstance.close(results);
                                    _sStringExcel = sTempString;
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
                $scope.FabricProcessTypes = obj.FabricProcessTypes;
                $scope.PlanNo  =  $scope.BuyerName = $scope.FactoryName = '';
                $scope.BuyerIDs = $scope.FactoryIDs = "";
                $scope.cboIssueDate =$scope.CompareOperators[0].id;
                $scope.IssueDateEndDisabled = true;
                $scope.IssueDateStart = $scope.IssueDateEnd = icsdateformat(new Date());

                $scope.DispoNo = '';
                $scope.SCNo = '';
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




