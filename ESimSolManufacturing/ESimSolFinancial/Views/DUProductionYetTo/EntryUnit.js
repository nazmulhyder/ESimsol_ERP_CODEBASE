var EUservice = angular.module('EU.service', ['ms.service', 'ngAnimate', 'ui.bootstrap', 'ui.grid', 'ui.grid.edit', 'ui.grid.cellNav', 'ui.grid.selection', 'ui.grid.resizeColumns']);
EUservice.service('EUservice', function ($uibModal) {
     
    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/DUProductionYetTo/EntryUnit',
            controller: 'ModalMappingCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                   
                    return {
                        DUDyeingTypeMapping: (modalProperty.DUDyeingTypeMapping == undefined) ? [] : modalProperty.DUDyeingTypeMapping,
                        DyeingType: (modalProperty.DyeingType == undefined) ? [] : modalProperty.DyeingType,
                        Data: (modalProperty.Data == undefined) ? [] : modalProperty.Data
                        
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
            $scope.Data = obj.Data;
            $scope.DUDyeingTypeMapping = obj.DUDyeingTypeMapping;
            $scope.DyeingType = obj.DyeingType;
            var list = [];
            if ($scope.DUDyeingTypeMapping.length < 1) {
                for (var i = 0; i < $scope.DyeingType.length; i++) {
                    var temp = {};
                    temp = {
                        DyeingTypeMappingID: i + 1,
                        DyeingType: $scope.DyeingType[i].id,
                        DyeingTypeStr: $scope.DyeingType[i].Value,
                        ProductID : $scope.Data.ProductID
                    }
                    list.push(temp);
                }
                $scope.DUDyeingTypeMapping = list;
            }
            
            $scope.EntryUnitgridOptions = {
                enableRowHeaderSelection: false,
                enableRowSelection: true,
                enableFullRowSelection: true,
                multiSelect: false,
                enableColumnResizing: true,
                noUnselect: true,
                showColumnFooter: true,
                enableGridMenu: true,
                columnDefs: [
                    { name: ' ', width: '3%', cellTemplate: '<div style="padding-top:5px;"><span>{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</span></div>', cellClass: 'text-center', enableCellEdit: false, enableSorting: false, enableColumnResizing: false, enableColumnMenu: false },
                    { field: 'DyeingTypeStr', name: 'Dyeing Type', width: '50%', cellClass: 'text-left', enableCellEdit: false },
                    { field: 'Unit', name: 'Unit', width: '40%', cellClass: 'text-right',enableCellEdit: true, cellFilter: 'number: 2', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellFilter: 'number:2', footerCellClass: 'text-right' },

                ],
                data: $scope.DUDyeingTypeMapping,
                onRegisterApi: function (EntryUnitgridApi) {
                    $scope.EntryUnitgridApi = EntryUnitgridApi;
                }
            };

          
            $scope.lblLoadingMessage = true;
            $scope.PickerTitle = "Entry Unit";
            $scope.ProductName = $scope.Data.ProductName;
            $scope.ProductID = $scope.Data.ProductID;

            $scope.save = function () {
               
           
                var objs = { DUDyeingTypeMappings: $scope.EntryUnitgridOptions.data }
              

                $http.post(_sBaseAddress + '/DUProductionYetTo/SaveEntry', JSON.stringify(objs)).then(
                    function (response) {
                        var result = jQuery.parseJSON(response.data);
                        if (result.DyeingTypeMappingID > 0) {
                            msModal.Message({ headerTitle: '', bodyText: 'Saved Successfully', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                            $uibModalInstance.close(result);
                        }
                        else {
                            msModal.Message({ headerTitle: '', bodyText: result.ErrorMessage, sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                        }
                    },
                    function (response) { alert(jQuery.parseJSON(response.data).ErrorMessage); }
                );
            };

            $scope.close = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});