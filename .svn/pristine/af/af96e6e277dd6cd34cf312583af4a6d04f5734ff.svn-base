var DUPservice = angular.module('RSM.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
DUPservice.service('RSMservice', function ($uibModal, $log) {

    //$(document).ready(function () {
    //    $("#progressbar").progressbar({ value: 0 });
    //    $("#progressbarParent").hide();
    //});
     
    this.Instance = function (modalProperty)
    {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'lg',
            templateUrl: _sBaseAddress + '/RouteSheet/AdvRouteSheetYarnOut',
            controller: 'ModalDUPSCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    debugger;
                    return {
                        OrderType: (modalProperty.OrderType == undefined) ? [] : modalProperty.OrderType,
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        Locations: (modalProperty.Locations == undefined) ? [] : modalProperty.Locations,
                        MachineTypes: (modalProperty.MachineTypes == undefined) ? [] : modalProperty.MachineTypes,
                        ProductionScheduleStatusList: (modalProperty.ProductionScheduleStatusList == undefined) ? [] : modalProperty.ProductionScheduleStatusList,
                        RSStates: (modalProperty.RSStates == undefined) ? [] : modalProperty.RSStates,
                        RSShifts: (modalProperty.RSShifts == undefined) ? [] : modalProperty.RSShifts,
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
            $scope.PickerTitle = "Advance Search";

            $scope.MachineIDs = "";
            $scope.SearchKeyDownMachine = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    $scope.PickMachine();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.MachineName = ''; $scope.MachineIDs = "";
                }
            };
            $scope.PickMachine = function () {
                
                //if ($scope.MachineTypeID <= 0)
                //{
                //    alert("Please Select machine Type and try again!"); return;
                //}

                var oMechine = {
                    Name: $scope.MachineName,
                    MachineTypeID: $scope.MachineTypeID,
                    BUID: sessionStorage.getItem('BUID')
                };

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/DUPSchedule/SearchMachineType', $.param(oMechine), config).then(
                    function (response) {
                        debugger;
                        var oColumns = [];
                        var oColumn = { field: "Code", name: "Code", width: 100, align: "left" }; oColumns.push(oColumn);
                        oColumn = { field: "Name", name: "Name", width: 180, align: "left" }; oColumns.push(oColumn);
                        oColumn = { field: "MachineTypeName", name: "Machine Type ", width: 110, align: "left" }; oColumns.push(oColumn);
                        var results = jQuery.parseJSON(response.data);
                        var modalObj = {
                            size: 'md',
                            modalcontroller: 'ModalDUPSCtrl',
                            appcontroller: 'DUPScheduleController',
                            objs: results,
                            multiSelect: true,
                            pickertitle: 'Machine',
                            searchingbyfieldName: 'Name',
                            columns: oColumns
                        }
                        var modalInstance = msModal.Instance(modalObj);
                        modalInstance.result.then(function (result) {
                            debugger;
                            if (result.length > 0)
                            {
                                var oMechines = result;
                                _sMachineIDs = oMechines[0].MachineID;
                                for (var i = 1; i < oMechines.length; i++) {
                                    _sMachineIDs += "," + oMechines[i].MachineID;
                                }
                                $scope.MachineName = oMechines.length + " item(s) select";
                                $scope.MachineIDs = _sMachineIDs;
                            }
                        }, function () {
                            $log.info('Modal dismissed at: ' + new Date());
                        });
                    },
                        function (response) { alert(jQuery.parseJSON(response.data).Message); }
                );
            };

            $scope.BuyerIDs = "";
            $scope.SearchKeyDownBuyer = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    $scope.PickBuyer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.BuyerName = ''; $scope.BuyerIDs = "";
                }
            };
            $scope.PickBuyer = function () {

                var oContractor = {
                    Params: '2' + '~' + $.trim($scope.BuyerName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) 
                                    {
                                        var oColumns = [];
                                        var
                                        oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Applicant Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                        var results = response.data;
                        var modalObj = {
                            size: 'md',
                            modalcontroller: 'ModalDUPSCtrl',
                            appcontroller: 'DUPScheduleController',
                            objs: results,
                            multiSelect: true,
                            pickertitle: 'Buyer',
                            searchingbyfieldName: 'Name',
                            columns: oColumns
                        }
                        var modalInstance = msModal.Instance(modalObj);
                        modalInstance.result.then(function (result) {
                            debugger;
                            if (result.length > 0) {
                                var oBuyers = result;
                                _sBuyerIDs = oBuyers[0].ContractorID;
                                for (var i = 1; i < oBuyers.length; i++) {
                                    _sBuyerIDs += "," + oBuyers[i].ContracorID;
                                }
                                $scope.BuyerName = oBuyers.length + " item(s) select";
                                $scope.BuyerIDs = _sBuyerIDs;
                            }
                        }, function () {
                            $log.info('Modal dismissed at: ' + new Date());
                        });
                    },
                        function (response) { alert(jQuery.parseJSON(response.data).Message); }
                );
            };

            $scope.PickScheduleStatus = function () {

                debugger;
                var oColumns = [];
                var oColumn = { field: "Value", name: "Schedule Status", width: '100%', align: "left" }; oColumns.push(oColumn);
               
                var modalObj = {
                    size: 'sm',
                    modalcontroller: 'ModalDUPSCtrl',
                    appcontroller: 'DUPScheduleController',
                    objs: obj.ProductionScheduleStatusList,
                    multiSelect: true,
                    pickertitle: 'Schedule Status',
                    searchingbyfieldName: 'Name',
                    columns: oColumns
                }
                var modalInstance = msModal.Instance(modalObj);
                modalInstance.result.then(function (result) {
                    debugger;
                    if (result.length > 0) {
                        var oScheduleStatuss = result;
                        var sStatusIDs = oScheduleStatuss[0].id;
                        $scope.ScheduleStatus = oScheduleStatuss[0].Value;
                        for (var i = 1; i < oScheduleStatuss.length; i++) {
                            sStatusIDs += "," + oScheduleStatuss[i].id;
                        }
                        $scope.ScheduleStatus = oScheduleStatuss.length + " item(s) select";
                        $scope.StatusIDs = sStatusIDs;
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            $scope.ClearScheduleStatus = function () {
                $scope.ScheduleStatus = $scope.StatusIDs = "";
            }

            $scope.PickRSState = function ()
            {
                debugger;
                var oColumns = [];
                var oColumn = { field: "Value", name: "Schedule Status", width: '100%', align: "left" }; oColumns.push(oColumn);

                var modalObj =
                    {
                    size: 'sm',
                    modalcontroller: 'ModalDUPSCtrl',
                    appcontroller: 'DUPScheduleController',
                    objs: obj.RSStates,
                    multiSelect: true,
                    pickertitle: 'RS State',
                    searchingbyfieldName: 'Name',
                    columns: oColumns
                }
                var modalInstance = msModal.Instance(modalObj);
                modalInstance.result.then(function (result) {
                    debugger;
                    if (result.length > 0) {
                        var oRSStates = result;
                        var sStatusIDs = oRSStates[0].id;
                        $scope.ScheduleStatus = oRSStates[0].Value;
                        for (var i = 1; i < oRSStates.length; i++) {
                            sStatusIDs += "," + oRSStates[i].id;
                        }
                        $scope.RSState = oRSStates.length + " item(s) select";
                        $scope.RSStateIDs = sStatusIDs;
                    }
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
            $scope.ClearRSState = function ()
            {
                $scope.RSState = $scope.RSStateIDs = ""; $scope.cboRSDate=0;
            }

            $scope.SearchKeyDownOrderNo = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    $scope.PickOrderNo();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.OrderNo = '';
                    $scope.DODID = '';
                }
            };

            $scope.PickOrderNo = function () {


                debugger;
                var oOrder = {
                    OrderType: $.trim($scope.OrderTypeID),
                    OrderNo: $.trim($scope.OrderNo),
                    LabDipDetailID: 0
                };

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/DUPSchedule/GetsDyeingOrder', $.param(oOrder), config).then(
                    function (response) {
                        debugger;
                        var oColumns = [];
                        var oColumn = { field: 'OrderNo', name: 'Order No', width: '25%', enableSorting: false }; oColumns.push(oColumn);
                        oColumn = { field: 'OrderQty', name: 'Qty', width: '15%', enableSorting: false }; oColumns.push(oColumn);
                        oColumn = { field: 'ProductName', name: 'ProductName', width: '15%', enableSorting: false }; oColumns.push(oColumn);
                        oColumn = { field: 'ColorName', name: 'ColorName', width: '15%', enableSorting: false }; oColumns.push(oColumn);
                        oColumn = { field: 'ShadeSt', name: 'ShadeSt', width: '15%', enableSorting: false }; oColumns.push(oColumn);
                        oColumn = { field: 'OrderTypeSt', name: 'OrderTypeSt', width: '15%', enableSorting: false }; oColumns.push(oColumn);
                        var results = jQuery.parseJSON(response.data);
                        var modalObj = {
                            size: 'md',
                            modalcontroller: 'ModalDUPSCtrl',
                            appcontroller: 'DUPScheduleController',
                            objs: results,
                            multiSelect: true,
                            enableColumnResizing: true,
                            pickertitle: 'Order List',
                            searchingbyfieldName: 'OrderNo',
                            columns: oColumns
                        }
                        var modalInstance = msModal.Instance(modalObj);
                        modalInstance.result.then(function (result) {
                            debugger;
                            $scope.OrderNo = result.length > 1 ? result.length + "Item's Selected" : result[0].OrderNo;
                            $scope.DODID = icsMethod.ICS_PropertyConcatation(result, 'DODID');
                        }, function () {
                            $log.info('Modal dismissed at: ' + new Date());
                        });
                    },
                        function (response) { alert(jQuery.parseJSON(response.data).Message); }
                );
            };

            $scope.search = function () {
                debugger;
                //$scope.lblLoadingMessage = false;
                //if () {
                //   //msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                //    alert("Please Select at least one Criteria !!");
                //    $scope.lblLoadingMessage = false;
                //    return false;
                //}

                var dStartDate = new Date($scope.StartTime);
                dStartDate.setHours($("#cboHourStart").val());
                dStartDate.setMinutes($("#cboMinStart").val());
                $scope.StartTime = icsdatetimeformat(dStartDate);

                var dEndDate = new Date($scope.EndTime);
                dEndDate.setHours($("#cboHourEnd").val());
                dEndDate.setMinutes($("#cboMinEnd").val());
                $scope.EndTime = icsdatetimeformat(dEndDate);

                $scope.IsInHouse = ($scope.IsInHouse == true ? true : false);
                $scope.IsOutSide = ($scope.IsOutSide == true ? true : false);

                var sTempString = $scope.cboDUPSDate + '~' + $scope.StartTime + '~' + $scope.EndTime + '~' + $scope.LocationID + '~' + $scope.MachineIDs + '~' + $scope.DODID + "~" + $scope.MachineTypeID + "~" + $scope.StatusIDs + "~" + $scope.RSStateIDs + "~" + $scope.cboRSDate + '~' + $scope.StartTime_RS + '~' + $scope.EndTime_RS + '~' + $scope.BuyerIDs + '~' + $scope.RSShiftID + '~' + $scope.IsInHouse + '~' + $scope.IsOutSide;
              
                sessionStorage.setItem("sTempString", sTempString);
                sSearchString = sTempString;

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };

                $http.get(_sBaseAddress + '/RouteSheet/AdvSearch_YarnOut', { params: { sTemp: sTempString } }, config).then(
                            function (response) {
                                $scope.lblLoadingMessage = true;
                                var results = response.data;
                                if (results.length > 0)
                                {
                                    $uibModalInstance.close(results);
                                } else {
                                    debugger;
                                    alert("Data Not Found.");
                                    return;
                                }

                            },
                                function (response) { alert(response.statusText); }
                        );
            };
            $scope.OrderDateChange = function () {
                $scope.OrderDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboDUPSDate);
            }
            $scope.reset = function () {
                debugger;
                $scope.CompareOperators = obj.CompareOperators;
                $scope.cboDUPSDate = $scope.CompareOperators[0].id;
                $scope.cboRSDate = $scope.CompareOperators[0].id;
                $scope.OrderType = obj.OrderType;
                $scope.RSShifts = obj.RSShifts;
                $scope.Locations = obj.Locations;
                $scope.MachineTypes = obj.MachineTypes;
                $scope.StatusIDs = "";
                $scope.ScheduleStatus = "";
                $scope.RSStateIDs = "";
                $scope.RSState = "";
                $scope.MachineName = "";

                $scope.RSShiftID = 0;
                $scope.MachineIDs = "";
                $scope.BuyerIDs = "";
                $scope.LocationID = "";
                $scope.DODID = "";
                $scope.OrderNo = "";
                $scope.OrderTypeID = "";
                $scope.MachineTypeID = 0;
                $scope.StartTime = $scope.EndTime = $scope.StartTime_RS = $scope.EndTime_RS = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});