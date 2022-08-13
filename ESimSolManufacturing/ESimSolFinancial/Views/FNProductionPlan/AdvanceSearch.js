
var masterfnppservice = angular.module('FNProductionPlan.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
masterfnppservice.service('fnppservice', function ($uibModal) {    
    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/FNProductionPlan/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        BussinessSessions: (modalProperty.BussinessSessions == undefined) ? [] : modalProperty.BussinessSessions,
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
            $scope.PickerTitle ="Production Plan Search";
            $scope.SearchKeyDownFNMachine = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var FNMachineName = $.trim($scope.FNMachineName);
                    if (FNMachineName == "" || FNMachineName == null)
                    {
                        alert("Type FNMachine Name and Press Enter");
                        return;
                    }
                    $scope.PickFNMachine();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.FNMachineName = '';
                    $scope.FNMachineIDs ='';
                }
            };
            $scope.PickFNMachine = function () {
                debugger;
                var oFNMachine = {
                    Name: $.trim($scope.FNMachineName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress')+'/FNMachine/GetFNMachines',$.param(oFNMachine), config).then(
                     function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'Code', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'FNMachineTypeSt', name: 'Type', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'mainController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Machine List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.FNMachineName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            $scope.FNMachineIDs = icsMethod.ICS_PropertyConcatation(result, 'FNMachineID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

    
            $scope.StartDateChange = function()
            {
                $scope.LCStartDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboStartDate);
            }
            $scope.EndDateChange = function () {
                $scope.LCEndDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboEndDate);
            }
           
            $scope.search = function ()
            {
                debugger;
                $scope.lblLoadingMessage = false;
                if ($scope.cboStartDate == 0  && $scope.cboEndDate == 0 && $scope.cboSession == 0 && ($scope.PlanNo == undefined ? "" : $scope.PlanNo)== "" && ($scope.FNMachineIDs== undefined ? "" : $scope.FNMachineIDs) == "" )
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                var sTempString = $scope.cboStartDate + '~' + $scope.LCStartDateStart + '~' + $scope.LCStartDateEnd + '~' +$scope.cboEndDate + '~' + $scope.LCEndDateStart + '~' + $scope.LCEndDateEnd + '~' +( $scope.PlanNo == undefined ?"": $scope.PlanNo) + '~' + ($scope.FNMachineIDs == undefined ? "" : $scope.FNMachineIDs)+'~'+ sessionStorage.getItem('BUID');
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(sessionStorage.getItem('BaseAddress') + '/FNProductionPlan/Search', {params:{sTemp:sTempString } }, config).then(
                            function (response)
                            {
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.length > 0)
                                {
                                    $uibModalInstance.close(results);
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
                $scope.PlanNo  =  $scope.FNMachineName = $scope.AdviceBankName = '';
                $scope.FNMachineIDs = "";
                $scope.cboStartDate =  $scope.cboEndDate = $scope.CompareOperators[0].id;
                $scope.LCStartDateEndDisabled = $scope.LCEndDateEndDisabled = $scope.LCReceiveDateEndDisabled = $scope.LCShipmentDateEndDisabled = true;
                $scope.LCStartDateStart = $scope.LCStartDateEnd = $scope.LCEndDateStart = $scope.LCEndDateEnd =  icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




