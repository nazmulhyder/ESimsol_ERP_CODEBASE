

var engineservice = angular.module('engine.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
engineservice.service('engineservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'lg',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/VehicleEngine/ViewVehicleEngine?id=0',
            controller: 'CCategoryController',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        //BussinessSessions: (modalProperty.BussinessSessions == undefined) ? [] : modalProperty.BussinessSessions,
                        VehicleEngine: (modalProperty.VehicleEngine == undefined) ? null : modalProperty.VehicleEngine
                    };
                }
            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj) {
            debugger;
            debugger;
            sessionStorage.setItem('BaseAddress', _sBaseAddress);
            $scope.RefreshControl = function () {
                $scope.VehicleEngine = obj.VehicleEngine;
            }


            $scope.Save = function () {
                debugger;
                if (!$scope.ValidateInput()) return;
                var oVehicleEngine = $scope.VehicleEngine;
                $http.post(sessionStorage.getItem('BaseAddress') + '/VehicleEngine/Save', oVehicleEngine).then(
                      function (response) {
                          var oVehicleEngine = jQuery.parseJSON(response.data);
                          if (oVehicleEngine.ErrorMessage == "" || oVehicleEngine.ErrorMessage == null) {
                              alert("Data Save Successfully!!");
                              var oVehicleEngines = sessionStorage.getItem("VehicleEngines");
                              var nIndex = parseInt(sessionStorage.getItem("SelectedRowIndex"));
                              if (oVehicleEngines != null) {
                                  oVehicleEngines = jQuery.parseJSON(oVehicleEngines);
                              }
                              else {
                                  oVehicleEngines = [];
                              }
                              if (nIndex != -1) {
                                  oVehicleEngines[nIndex] = oVehicleEngine;
                              }
                              else {
                                  sessionStorage.setItem("SelectedRowIndex", oVehicleEngines.length);
                                  oVehicleEngines.push(oVehicleEngine);
                              }
                              sessionStorage.setItem("VehicleEngines", JSON.stringify(oVehicleEngines));
                              //window.location.href = sessionStorage.getItem("BackLink");
                              $uibModalInstance.dismiss('cancel');

                          }
                          else {
                              alert(oVehicleEngine.ErrorMessage);
                          }

                      },
                          function (response) { alert(jQuery.parseJSON(response.data).Message); $scope.MeasurementUnits = []; }
                  );
            }
            $scope.ValidateInput = function () {
                debugger;
                if ($scope.VehicleEngine.EngineNo == null || $scope.VehicleEngine.EngineNo == "") {
                    alert("Please enter Engine No!");
                    return false;
                }
                if ($scope.VehicleEngine.ManufacturerID == null || $scope.VehicleEngine.ManufacturerID <= 0) {
                    alert("Please enter Menufacturer!");
                    return false;
                }
                return true;
            }
            if (sessionStorage.getItem("VehicleEngineHeader") == "View Vehicle Engine") {
                $("#divCC :input").prop('disabled', true);
                $('#btnSave').prop('disabled', true);
            }

            //// Search Manufacturer Start
            $scope.SearchKeyDownManufacturer = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ManufacturerName = $.trim($scope.VehicleEngine.ManufacturerName);
                    if (ManufacturerName == "" || ManufacturerName == null) {
                        alert("Type Manufacturer Name and Press Enter");
                        return;
                    }
                    $scope.PickManufacturer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.VehicleEngine.ManufacturerName = '';
                    $scope.VehicleEngine.Manufacturer = 0;
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
                                            multiSelect: false,
                                            pickertitle: 'Manufacturer List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.VehicleEngine.ManufacturerName = result.Name;
                                            $scope.VehicleEngine.ManufacturerID = result.ContractorID;
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.RefreshControl();
            $scope.Close = function () {
                // window.location.href = sessionStorage.getItem("BackLink");
                $uibModalInstance.dismiss('cancel');
            }
            $scope.keydown = function (e) {
                if (e.which == 27)//escape=27
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




