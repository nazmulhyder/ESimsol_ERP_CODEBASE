

var chassisservice = angular.module('chassis.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
chassisservice.service('chassisservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'lg',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/VehicleChassis/ViewVehicleChassis?id=0',
            controller: 'CCategoryController',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        //BussinessSessions: (modalProperty.BussinessSessions == undefined) ? [] : modalProperty.BussinessSessions,
                        VehicleChassis: (modalProperty.VehicleChassis == undefined) ? null : modalProperty.VehicleChassis
                    };
                }
            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj) {
            //debugger;
            sessionStorage.setItem('BaseAddress', _sBaseAddress);
            $scope.RefreshControl= function()
            {
                $scope.VehicleChassis = obj.VehicleChassis;
            }
            sessionStorage.setItem('BaseAddress', _sBaseAddress);
            //$scope.VehicleChassis = oVehicleChassis;
            $scope.RefreshControl();
            $scope.Save = function () {
               // debugger;
                if (!$scope.ValidateInput()) return;
                var oVehicleChassis = $scope.VehicleChassis;
                $http.post(sessionStorage.getItem('BaseAddress') + '/VehicleChassis/Save', oVehicleChassis).then(
                      function (response) {
                          var oVehicleChassis = jQuery.parseJSON(response.data);
                          if (oVehicleChassis.ErrorMessage == "" || oVehicleChassis.ErrorMessage == null) {
                              debugger;
                              alert("Data Save Successfully!!");
                              $uibModalInstance.close(oVehicleChassis);
                          }
                          else {
                              alert(oVehicleChassis.ErrorMessage);
                          }

                      },
                          function (response) { alert(jQuery.parseJSON(response.data).Message); $scope.MeasurementUnits = []; }
                  );
            }
            $scope.ValidateInput = function () {
                debugger;
                if ($scope.VehicleChassis.ChassisNo == null || $scope.VehicleChassis.ChassisNo == "") {
                    alert("Please enter Chassis No!");
                    return false;
                }
                if ($scope.VehicleChassis.ManufacturerID == null || $scope.VehicleChassis.ManufacturerID <= 0) {
                    alert("Please enter Menufacturer!");
                    return false;
                }
                return true;
            }
            if (sessionStorage.getItem("ChassisHeader") == "View Vehicle Chassis") {
                $("#divCC :input").prop('disabled', true);
                $('#btnClose').prop('disabled', false);
            }

            // Search Manufacturer Start
            $scope.SearchKeyDownManufacturer = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ManufacturerName = $.trim($scope.VehicleChassis.ManufacturerName);
                    if (ManufacturerName == "" || ManufacturerName == null) {
                        alert("Type Manufacturer Name and Press Enter");
                        return;
                    }
                    $scope.PickManufacturer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.VehicleChassis.ManufacturerName = '';
                    $scope.VehicleChassis.Manufacturer = 0;
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
                                        var results = (response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCc',
                                            appcontroller: 'CCategoryController',
                                            objs: results,
                                            multiSelect: false,
                                            pickertitle: 'Manufacturer List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.VehicleChassis.ManufacturerName = result.Name;
                                            $scope.VehicleChassis.ManufacturerID = result.ContractorID;
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.ClearManufacturer = function () {
                debugger;
                $scope.VehicleChassis.ManufacturerName = "";
                $scope.VehicleChassis.ManufacturerID = 0;
            };

            $scope.RefreshControl();
            $scope.Close = function()
            {
                debugger;
               // window.location.href = sessionStorage.getItem("BackLink");
                $uibModalInstance.dismiss('cancel');
            }
            $scope.keydown = function(e)
            {
                if(e.which == 27)//escape=27
                {
                    //debugger;
                   // window.location.href = sessionStorage.getItem("BackLink");
                    $uibModalInstance.dismiss('cancel');

                }
            }
           
        }
        return $uibModal.open(modalInstance);
    }


});




