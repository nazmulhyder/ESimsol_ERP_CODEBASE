

var engineservice = angular.module('engine.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
engineservice.service('engineservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size:'lg',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/VehicleEngine/ViewVehicleEngine?id=0',
            controller: 'CCategoryController',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        FuelTypes: (modalProperty.FuelTypes == undefined) ? [] : modalProperty.FuelTypes,
                        VehicleEngine: (modalProperty.VehicleEngine == undefined) ? null : modalProperty.VehicleEngine,
                        Sessions: (modalProperty.Sessions == undefined) ? [] : modalProperty.Sessions
                    };
                }
            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj) {
            debugger;
            debugger;
            sessionStorage.setItem('BaseAddress', _sBaseAddress);
            $scope.RefreshControl= function()
            {
                $scope.VehicleEngine = obj.VehicleEngine;
                $scope.FuelTypes = obj.FuelTypes;
                $scope.Sessions = obj.Sessions;
            }

            
            $scope.Save=function()
            {
                debugger;
                if(!$scope.ValidateInput()) return;
                var oVehicleEngine=$scope.VehicleEngine;
                $http.post(sessionStorage.getItem('BaseAddress')+'/VehicleEngine/Save',oVehicleEngine).then(
                      function (response)
                      {
                          var oVehicleEngine= jQuery.parseJSON(response.data);
                          if (oVehicleEngine.VehicleEngineID>0)
                          {
                              alert("Data Save Successfully!!");
                              $uibModalInstance.close(oVehicleEngine);

                          }
                          else
                          {
                              alert(oVehicleEngine.ErrorMessage);
                          }

                      },
                          function (response) { alert(jQuery.parseJSON(response.data).Message);$scope.MeasurementUnits=[];}
                  );
            }
            $scope.ValidateInput = function()
            {
                debugger;
                if($scope.VehicleEngine.EngineNo==null || $scope.VehicleEngine.EngineNo=="")
                {
                    alert("Please enter Engine No!");
                    return false;
                }
                if($scope.VehicleEngine.ManufacturerID==null || $scope.VehicleEngine.ManufacturerID<=0)
                {
                    alert("Please enter Menufacturer!");
                    return false;
                }
                if ($scope.VehicleEngine.FuelType == null || $scope.VehicleEngine.FuelType <= 0) {
                    alert("Please Select Fuel Type!");
                    return false;
                }
                return true;
            }
            if(sessionStorage.getItem("VehicleEngineHeader")=="View Vehicle Engine")
            {
                $("#divCC :input").prop('disabled', true);
                $('#btnSave').prop('disabled', true);
            }
            
            //// Search Manufacturer Start
            $scope.SearchKeyDownManufacturer=function (e)
            {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13)
                {
                    var ManufacturerName = $.trim($scope.VehicleEngine.ManufacturerName);
                    if(ManufacturerName==""||ManufacturerName==null)
                    {
                        alert("Type Manufacturer Name and Press Enter");
                        return;
                    }
                    $scope.PickManufacturer();
                }else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.VehicleEngine.ManufacturerName='';
                    $scope.VehicleEngine.Manufacturer = 0;
                }
            };
            $scope.PickManufacturer= function () {
                debugger;
                var oContractor = {
                    Params: '1' + '~' + ""+'~'+0
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress')+'/Contractor/ContractorSearchByNameType',$.param(oContractor), config).then(
                                    function (response)
                                    {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code',width: '20%'  };oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Manufacturer Name',width: '50%', enableSorting: false  };oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address',width: '30%', enableSorting: false  };oColumns.push(oColumn);
                                        var results=(response.data);
                                        var modalObj={
                                            size:'md',
                                            modalcontroller:'ModalCc',
                                            appcontroller:'CCategoryController',
                                            objs:results,
                                            multiSelect:false,
                                            pickertitle:'Manufacturer List',
                                            searchingbyfieldName:'Name',
                                            columns:oColumns
                                        }
                                        var modalInstance=msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result)
                                        {
                                            debugger;
                                            $scope.VehicleEngine.ManufacturerName=result.Name;
                                            $scope.VehicleEngine.ManufacturerID=result.ContractorID;
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message);}
                                );
            };
            $scope.ClearManufacturer = function ()
            {
                $scope.VehicleEngine.ManufacturerName = "";
                $scope.VehicleEngine.ManufacturerID = 0;
            };

            //// Search VehicleModel Start
            $scope.SearchKeyDownVehicleModel = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var VehicleModelName = $.trim($scope.VehicleEngine.VehicleModelName);
                    if (VehicleModelName == "" || VehicleModelName == null) {
                        alert("Type Model Name and Press Enter");
                        return;
                    }
                    $scope.PickVehicleModel();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.VehicleEngine.VehicleModelName = '';
                    $scope.VehicleEngine.VehicleModel = 0;
                }
            };
            $scope.PickVehicleModel = function () {
                var oVehicleModel = {
                    ModelNo: $.trim($scope.VehicleEngine.VehicleModelName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress')+'/VehicleModel/GetVehicleModels',$.param(oVehicleModel), config).then(
                            function (response)
                            {
                                var oColumns = [];
                                var oColumn = { field: 'ModelNo', name: 'ModelNo',width: '30%'  };oColumns.push(oColumn);
                                oColumn = { field: 'CategoryName', name: 'CategoryName',width: '40%', enableSorting: false  };oColumns.push(oColumn);
                                var results=jQuery.parseJSON(response.data);
                                var modalObj={
                                    size:'md',
                                    modalcontroller:'',
                                    appcontroller:'',
                                    objs:results,
                                    multiSelect:false,
                                    pickertitle:'Model List',
                                    searchingbyfieldName:'ModelNo',
                                    columns:oColumns
                                }
                                var modalInstance=msModal.Instance(modalObj);
                                modalInstance.result.then(function (result) {
                                    debugger;
                                    $scope.VehicleEngine.VehicleModelName = result.Name;
                                    $scope.VehicleEngine.VehicleModelID = result.ContractorID;

                                    $scope.VehicleEngine.EngineType = result.EngineType;
                                    $scope.VehicleEngine.MaxPowerOutput = result.MaxPowerOutput;
                                    $scope.VehicleEngine.MaximumTorque = result.MaximumTorque;
                                    $scope.VehicleEngine.Transmission = result.Transmission;
                                    //$scope.VehicleEngine.DisplacementCC = result.ContractorID;
                                    //$scope.VehicleEngine.TopSpeed = result.ContractorID;
                                    //$scope.VehicleEngine.Acceleration = result.ContractorID;

                                }, function () {
                                    $log.info('Modal dismissed at: ' + new Date());
                                });
                            },
                               function (response) { alert(jQuery.parseJSON(response.data).Message); }
            )};
            $scope.ClearVehicleModel = function () {
                $scope.VehicleEngine.VehicleModelName = "";
                $scope.VehicleEngine.VehicleModelID = 0;
            };

            $scope.RefreshControl();
            $scope.Close = function()
            {
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




