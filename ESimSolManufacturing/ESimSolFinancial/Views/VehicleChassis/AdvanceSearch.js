
var advanceSearchservice = angular.module('advanceSearch.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
advanceSearchservice.service('advanceSearchservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl:sessionStorage.getItem('BaseAddress') + '/VehicleChassis/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators
                    };
                }
                
            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj)
        {
            debugger;
            $scope.lblLoadingMessage = true;
            $scope.PickerTitle ="Vehicle Chassis Search";
            $scope.SearchKeyDownManufacturer = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ManufacturerName = $.trim($scope.ManufacturerName);
                    if (ManufacturerName == "" || ManufacturerName == null)
                    {
                        alert("Type Manufacturer Name and Press Enter");
                        return;
                    }
                    $scope.PickManufacturer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.ManufacturerName = '';
                    $scope.ManufacturerIDs ='';
                }
            };

            $scope.PickManufacturer = function () {
                debugger;
                var oContractor = {
                    Params: '1' + '~' + "" + '~' + 0
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Manufacturer Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCc',
                                            appcontroller: 'CCategoryController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Manufacturer List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.ManufacturerName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            $scope.ManufacturerIDs =icsMethod.ICS_PropertyConcatation(result,'ContractorID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.search = function ()
            {
                debugger;
                $scope.lblLoadingMessage = false;
                if (($scope.ManufacturerName == undefined ? "" : $scope.ManufacturerName) && ($scope.EngineLayout == undefined ? "" : $scope.EngineLayout)
                    && ($scope.Steering == undefined ? "" : $scope.Steering) &&
                   ($scope.ManufacturerIDs == undefined ? "" : $scope.ManufacturerIDs) == "" && ($scope.FileNo == undefined ? "" : $scope.FileNo) == "" &&
                   ($scope.GearBox == undefined ? "" : $scope.GearBox) == "" && ($scope.ChassisNo == undefined ? "" : $scope.ChassisNo) == "")
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                debugger;
                var sTempString = ($scope.FileNo == undefined ? "" : $scope.FileNo) + '~' + ($scope.ChassisNo == undefined ? "" : $scope.ChassisNo) + '~'
                                + ($scope.ManufacturerIDs == undefined ? "" : $scope.ManufacturerIDs) + '~' + ($scope.EngineLayout == undefined ? "" : $scope.EngineLayout) + '~' + ($scope.DriveWheels == undefined ? "" : $scope.DriveWheels) + '~'
                                + ($scope.Steering == undefined ? "" : $scope.Steering) +'~'+ ($scope.GearBox == undefined ? "" : $scope.GearBox);
                                
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(sessionStorage.getItem('BaseAddress') + '/VehicleChassis/Search', {params:{sTemp:sTempString } }, config).then(
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
                $scope.ChassisNo = $scope.FileNo = $scope.ManufacturerName = $scope.EngineLayout = $scope.DriveWheels = $scope.GearBox= '';
                $scope.ManufacturerIDs = "";
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




