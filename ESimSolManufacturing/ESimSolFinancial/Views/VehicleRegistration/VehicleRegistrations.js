

var vregistrationservice = angular.module('vregistration.service', ['ms.service', 'ngAnimate', 'ui.bootstrap', 'chassis.service', 'engine.service']);//,'chassis.service','engine.service'
vregistrationservice.service('vregistrationservice', function ($uibModal) {//, chassisservice, engineservice
    debugger;
    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'lg',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/VehicleRegistration/ViewVehicleRegistration?id=0',
            controller: 'CCategoryController',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        //BussinessSessions: (modalProperty.BussinessSessions == undefined) ? [] : modalProperty.BussinessSessions,
                        VehicleRegistration: (modalProperty.VehicleRegistration == undefined) ? null : modalProperty.VehicleRegistration,
                        VehicleTypes: (modalProperty.VehicleTypes == undefined) ? null : modalProperty.VehicleTypes,
                        FuelTypes: (modalProperty.FuelTypes == undefined) ? null : modalProperty.FuelTypes,
                        Sessions: (modalProperty.Sessions == undefined) ? null : modalProperty.Sessions,
                        VehicleRegistrationTypes: (modalProperty.VehicleRegistrationTypes == undefined) ? null : modalProperty.VehicleRegistrationTypes
                    };
                }
            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj, chassisservice, engineservice) {//, chassisservice, engineservice
            //debugger
            sessionStorage.setItem('BaseAddress', _sBaseAddress);
            $scope.RefreshControl= function()
            {
                $scope.VehicleRegistration = obj.VehicleRegistration;
                $scope.VehicleTypes = obj.VehicleTypes;
                $scope.FuelTypes = obj.FuelTypes;
                $scope.Sessions = obj.Sessions;
                $scope.VehicleRegistrationTypes = obj.VehicleRegistrationTypes;
            }
            sessionStorage.setItem('BaseAddress', _sBaseAddress);
            //$scope.VehicleRegistration = oVehicleRegistration;
            $scope.RefreshControl();
            $scope.Save = function () {
               // debugger;
                if (!$scope.ValidateInput()) return;
                var oVehicleRegistration = $scope.VehicleRegistration;
                console.log(oVehicleRegistration);
                $http.post(sessionStorage.getItem('BaseAddress') + '/VehicleRegistration/Save', oVehicleRegistration).then(
                      function (response) {
                          var oVehicleRegistration = jQuery.parseJSON(response.data);
                          if (oVehicleRegistration.ErrorMessage == "" || oVehicleRegistration.ErrorMessage == null) {
                              debugger;
                              alert("Data Save Successfully!!");
                              $uibModalInstance.close(oVehicleRegistration);
                          }
                          else {
                              alert(oVehicleRegistration.ErrorMessage);
                          }

                      },
                          function (response) { alert(jQuery.parseJSON(response.data).Message); $scope.MeasurementUnits = []; }
                  );
            }
            $scope.ValidateInput = function () {
                debugger;
                //if ($scope.VehicleRegistration.VehicleRegNo == null || $scope.VehicleRegistration.VehicleRegNo == "") {
                //    alert("Please enter Registration No!");
                //    return false;
                //}
                if ($scope.VehicleRegistration.CustomerID == null || $scope.VehicleRegistration.CustomerID <= 0) {
                    alert("Please Select Customer!");
                    return false;
                }
                if (parseInt($scope.VehicleRegistration.VehicleRegistrationType) != 2)  //out = 2
                {
                    if ($scope.VehicleRegistration.ContactPersonID == null || $scope.VehicleRegistration.ContactPersonID <= 0) {
                        alert("Please Select Contact Person!");
                        return false;
                    }
                    if ($scope.VehicleRegistration.VehicleTypeID == null || $scope.VehicleRegistration.VehicleTypeID <= 0) {
                        alert("Please Select Vehicle Type!");
                        return false;
                    }
                }
                
                return true;
            }
            if (sessionStorage.getItem("VehicleRegistrationHeader") == "View Vehicle Registration") {
                $scope.disabled = true;
            }

            // Search Customer Start
            $scope.SearchKeyDownCustomer = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var CustomerName = $.trim($scope.VehicleRegistration.CustomerName);
                    if (CustomerName == "" || CustomerName == null) {
                        alert("Type Customer Name and Press Enter");
                        return;
                    }
                    $scope.PickCustomer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.VehicleRegistration.CustomerName = '';
                    $scope.VehicleRegistration.Customer = 0;
                }
            };
            $scope.PickCustomer = function () {
                debugger;
                var oContractor = {
                    Params: '2' + '~' + "" + '~' + 0
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Customer Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                   
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCc',
                                            appcontroller: 'CCategoryController',
                                            objs: response.data,
                                            multiSelect: false,
                                            pickertitle: 'Customer List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.VehicleRegistration.CustomerID = result.ContractorID;
                                            $scope.GetContactPersonel();
                                            $scope.VehicleRegistration.CustomerName = result.Name;
                                           
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert((response.data).Message); }
                                );
            };
            $scope.ClearCustomer = function () {
                debugger;
                $scope.ContactPersonnels = [];
                $scope.VehicleRegistration.CustomerName = "";
                $scope.VehicleRegistration.CustomerID = 0;
            };
          
            $scope.NewContactPerson = function NewContactPerson(nConType) {
                if ($scope.VehicleRegistration.CustomerID <= 0) {
                    alert("Please Pick A Customer And Try Again!")
                    return;
                }

                sessionStorage.setItem("ContractorBackTo", "");
                sessionStorage.setItem("SelectedRowIndex", 0);
                sessionStorage.setItem("ContractorHeader", "Add Contact Personnel");
                window.open(_sBaseAddress + "/Contractor/ViewContactPersonnel?id=" + $scope.VehicleRegistration.CustomerID, "_blank");
            }
            $scope.GetContactPersonel = function ()
            {
                $scope.ContactPersonnels = [];
                debugger;
                var oContractor = {
                    ContractorID: $scope.VehicleRegistration.CustomerID
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/Contractor/GetContactPersonnels', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        result = JSON.parse(response.data);
                                        $scope.ContactPersonnels = result;
                                    }, function () {
                                        $log.info('GetVehicleOrderDetails Dismissed at: ' + new Date());
                                    });
            }
            $scope.GetContactPersonel();
            //Color
            $scope.PickColorName = function () {
                var sColorName = $.trim($scope.VehicleRegistration.ColorName);
               
                var oVehicleColor = {
                    ColorName: sColorName,
                    ColorType: 1 //Exterior
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/VehicleColor/SearchColor', $.param(oVehicleColor), config).then(
                            function (response) {
                                var oColumns = [];
                                var oColumn = { field: 'ColorCode', name: 'Code', width: '35%' }; oColumns.push(oColumn);
                                oColumn = { field: 'ColorName', name: 'Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                var results = jQuery.parseJSON(response.data);
                                var modalObj = {
                                    size: 'sm',
                                    modalcontroller: '',
                                    appcontroller: '',
                                    objs: results,
                                    multiSelect: false,
                                    pickertitle: 'Color List',
                                    searchingbyfieldName: 'ColorName',
                                    columns: oColumns
                                }
                                var modalInstance = msModal.Instance(modalObj);
                                modalInstance.result.then(function (result) {
                                    $scope.VehicleRegistration.ColorName = result.ColorName;
                                    $scope.VehicleRegistration.VehicleColorID = result.VehicleColorID;
                                    
                                }, function () {
                                    $log.info('Modal dismissed at: ' + new Date());
                                });
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );

            };
            $scope.SearchKeyDownColor = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ColorName = $.trim($scope.VehicleRegistration.ColorName);
                    if (ColorName == "" || ColorName == null) {
                        alert("Type Color Name and Press Enter");
                        return;
                    }
                    $scope.PickColorName(1);//Exterior color
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.VehicleRegistration.ColorName = '';
                    $scope.VehicleRegistration.VehicleColorID = 0;
                }
            }
            //Engine 
            $scope.PickChassisNo = function () {
                var oVehicleChassis = {
                    ChassisNo: $.trim($scope.VehicleRegistration.ChassisNo)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/VehicleChassis/SearchByChassisNo', $.param(oVehicleChassis), config).then(
                            function (response) {
                                var oColumns = [];
                                var oColumn = { field: 'FileNo', name: 'File No', width: '30%' }; oColumns.push(oColumn);
                                oColumn = { field: 'ChassisNo', name: 'ChassisNo', width: '60%', enableSorting: false }; oColumns.push(oColumn);
                                var results = jQuery.parseJSON(response.data);
                                var modalObj = {
                                    size: 'md',
                                    modalcontroller: '',
                                    appcontroller: '',
                                    objs: results,
                                    multiSelect: false,
                                    pickertitle: 'Chassis List',
                                    searchingbyfieldName: 'ChassisNo',
                                    columns: oColumns
                                }
                                var modalInstance = msModal.Instance(modalObj);
                                modalInstance.result.then(function (result) {
                                    debugger;
                                    $scope.VehicleRegistration.ChassisNo = result.ChassisNo;
                                    $scope.VehicleRegistration.VehicleChassisID = result.VehicleChassisID;

                                    var oKomm = {
                                        ChassisID: $scope.VehicleRegistration.VehicleChassisID,
                                    };

                                    $scope.GetKommFile(oKomm);
                                }, function () {
                                    $log.info('Modal dismissed at: ' + new Date());
                                });
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );

            };
            $scope.SearchKeyDownChassisNo = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ChassisNo = $.trim($scope.VehicleRegistration.ChassisNo);
                    if (ChassisNo == "" || ChassisNo == null) {
                        alert("Type Chassis No and Press Enter");
                        return;
                    }
                    $scope.PickChassisNo();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.VehicleRegistration.ChassisNo = '';
                    $scope.VehicleRegistration.VehicleChassisID = 0;
                }
            };
            $scope.PickEngineNo = function () {
                var oVehicleEngine = {
                    EngineNo: $.trim($scope.VehicleRegistration.EngineNo)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/VehicleEngine/SearchByEngineNo', $.param(oVehicleEngine), config).then(
                            function (response) {
                                var oColumns = [];
                                var oColumn = { field: 'FileNo', name: 'File No', width: '30%' }; oColumns.push(oColumn);
                                oColumn = { field: 'EngineNo', name: 'EngineNo', width: '60%', enableSorting: false }; oColumns.push(oColumn);
                                var results = jQuery.parseJSON(response.data);
                                var modalObj = {
                                    size: 'md',
                                    modalcontroller: '',
                                    appcontroller: '',
                                    objs: results,
                                    multiSelect: false,
                                    pickertitle: 'Engine List',
                                    searchingbyfieldName: 'EngineNo',
                                    columns: oColumns
                                }
                                var modalInstance = msModal.Instance(modalObj);
                                modalInstance.result.then(function (result) {
                                    debugger;
                                    $scope.VehicleRegistration.EngineNo = result.EngineNo;
                                    $scope.VehicleRegistration.VehicleEngineID = result.VehicleEngineID;

                                    var oKomm = {
                                        EngineID: $scope.VehicleRegistration.VehicleEngineID,
                                    };
                                    $scope.GetKommFile(oKomm);
                                }, function () {
                                    $log.info('Modal dismissed at: ' + new Date());
                                });
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );

            };
            $scope.SearchKeyDownEngineNo = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var EngineNo = $.trim($scope.VehicleRegistration.EngineNo);
                    if (EngineNo == "" || EngineNo == null) {
                        alert("Type Engine No and Press Enter");
                        return;
                    }
                    $scope.PickEngineNo();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.VehicleRegistration.EngineNo = '';
                    $scope.VehicleRegistration.VehicleEngineID = 0;
                }
            };
            //Chassis
            $scope.PickNewChassis = function () {
                debugger;
                var result = null;
                var oVehicleChassis = {
                    VehicleChassisID: 0
                };

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/VehicleChassis/GetByID', $.param(oVehicleChassis), config).then(
                                    function (response) {
                                        debugger;
                                        result = jQuery.parseJSON(response.data);

                                        var modalObj = {
                                            modalcontroller: 'CCategoryapp',
                                            appcontroller: 'VehicleChassisController',
                                            VehicleChassis: result
                                            //BussinessSessions:$scope.BussinessSessions
                                        }
                                        var modalInstance = chassisservice.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.gridOptions.data = result;
                                            $scope.selectedRowIndex = parseInt(userSession.getData("SelectedRowIndex"));
                                            //$scope.gridOptions.selectRow(selectedRowIndex), true);
                                            //$scope.gridApi.selection.selectRow($scope.gridOptions.data[selectedRowIndex]);
                                            // $scope.gridApi.selection.selectRow($scope.gridOptions.data[$scope.selectedRowIndex]);

                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    }
                                );
            }
            $scope.PickNewEngine = function () {
                debugger;
                var result = null;
                var oVehicleEngine = {
                    VehicleEngineID: 0
                };

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/VehicleEngine/GetByID', $.param(oVehicleEngine), config).then(
                                    function (response) {
                                        debugger;
                                        result = jQuery.parseJSON(response.data);

                                        var modalObj = {
                                            modalcontroller: 'CCategoryapp',
                                            appcontroller: 'VehicleEngineController',
                                            VehicleEngine: result,
                                            FuelTypes: $scope.FuelTypes,
                                            Sessions: $scope.Sessions
                                        }
                                        var modalInstance = engineservice.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.gridOptions.data = result;

                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    }
                                );
            }

            $scope.GetKommFile = function (oKomm)
            {
                debugger;
              
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/KommFile/SearchByRef', $.param(oKomm), config).then(
                                    function (response) {
                                        debugger;
                                        data = JSON.parse(response.data);
                                        result = data[0];
                                        if (result!=undefined && result.KommFileID > 0)
                                        {
                                            console.log(result);
                                            $scope.VehicleRegistration.ChassisNo = result.ChassisNo;
                                            $scope.VehicleRegistration.EngineNo = result.EngineNo;
                                            $scope.VehicleRegistration.VehicleChassisID = result.ChassisID;
                                            $scope.VehicleRegistration.VehicleEngineID = result.EngineID;
                                            $scope.VehicleRegistration.VehicleColorID = result.ExteriorColorID;
                                            $scope.VehicleRegistration.ColorName = result.ExteriorColorName;
                                        }
                                    }, function () {
                                        $log.info('GetVehicleOrderDetails Dismissed at: ' + new Date());
                                    });
            }

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




