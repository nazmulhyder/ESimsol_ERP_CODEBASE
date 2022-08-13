
var ExportDocTnCservice = angular.module('ExportDocTnC.service', ['ms.service', 'ngAnimate', 'ui.bootstrap', 'ui.grid.edit']);
ExportDocTnCservice.service('DocumentSetupService', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'lg',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/ExportDocTnC/ViewExportDocTAndC',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        ExportDocTnC: (modalProperty.ExportDocTnC == undefined) ? null : modalProperty.ExportDocTnC
                    };
                }
                
            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj) {
            debugger;
            $('.date-container').datepicker({
                format: "dd M yyyy",
                calendarWeeks: true,
                autoclose: true,
                todayHighlight: true
            });
            $scope.PickerTitle = "Master LC Format Setup";

            $scope.PickTruckReceipt = function () {
                debugger;
                var oExportTR = {
                    ExportTRID: 0,
                    BUID: sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/ExportTR/PickExportTRs', $.param(oExportTR), config).then(
                                    function (response) {
                                        debugger;
                                        var results = jQuery.parseJSON(response.data);
                                        if (results[0].ErrorMessage != "" && results[0].ErrorMessage != null)
                                        {
                                            alert(results[0].ErrorMessage);
                                            return;
                                        }
                                        var oColumns = [];
                                        var oColumn = { field: 'TruckReceiptNo', name: 'Receipt No', width: '30%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Carrier', name: 'Carrier', width: '25%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'TruckNo', name: 'TruckNo', width: '25%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'DriverName', name: 'Driver Name', width: '20%', enableSorting: false }; oColumns.push(oColumn);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCtrl',
                                            appcontroller: 'ExportDocTnCController',
                                            objs: results,
                                            multiSelect: false,
                                            pickertitle: 'Truck Receipt List',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result)
                                        {
                                            debugger;
                                            $scope.ExportDocTnC.ExportTRID = result.ExportTRID;
                                            $scope.ExportDocTnC.TruckReceiptNo = result.TruckReceiptNo;
                                            $scope.ExportDocTnC.TruckReceiptDateInString = result.TruckReceiptDateInString;
                                            $scope.ExportDocTnC.Carrier = result.Carrier;
                                            $scope.ExportDocTnC.TruckNo = result.TruckNo;
                                            $scope.ExportDocTnC.DriverName = result.DriverName;
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.save = function ()
            {
               debugger;
               var oExportDocTnC = $scope.ExportDocTnC;
               oExportDocTnC.ExportPartyInfoBills = $scope.gridApiPartyInfo.selection.getSelectedRows();
               oExportDocTnC.ExportDocForwardings = $scope.gridApiForwarding.selection.getSelectedRows();
               $.ajax({
                   type: "POST",
                   dataType: "json",
                   url: sessionStorage.getItem('BaseAddress') + "/ExportDocTnC/Save",
                   traditional: true,
                   data: JSON.stringify(oExportDocTnC),
                   contentType: "application/json; charset=utf-8",
                   success: function (data) {
                       //debugger;
                       var result = jQuery.parseJSON(data);
                       if (parseInt(result.ExportDocTnCID) > 0 && (result.ErrorMessage == null || result.ErrorMessage == ""))
                       {
                           alert("Save Successfully.");
                           $uibModalInstance.close();
                       } else {
                           alert(result.ErrorMessage);
                           return;
                       }
                      
                   },
                   error: function (xhr, status, error) {
                       alert(error);
                   }
               });



                //var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                //    $http.post(sessionStorage.getItem('BaseAddress')+'/ExportDocTnC/Save',$.param(oExportDocTnC), config).then(
                //            function (response)
                //            {
                //                var result = jQuery.parseJSON(response.data);
                //                if (parseInt(result.ExportDocTnCID) > 0 && (result.ErrorMessage == null || result.ErrorMessage == ""))
                //                {
                //                    alert("Save Successfully.");
                //                    $uibModalInstance.close();
                //                } else {
                //                    alert(result.ErrorMessage);
                //                    return;
                //                }
                                
                //            },
                //                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                //        );
            };
           $scope.setcontrol = function ()
            {
               debugger;
               $scope.ExportLC = obj.ExportDocTnC.ExportLC;
               $scope.ExportDocTnC = obj.ExportDocTnC;
               
               $scope.gridOptionsDocForWarding = {
                   enableRowSelection: true,
                   enableSelectAll: true,
                   multiSelect: true,
                   enableRowSelection: true,
                   columnDefs: [
                     { field: 'Name_Print', name: 'Doc Name', width: '55%', cellClass: 'text-left', enableCellEdit: true, enableSorting: false },
                     { field: 'Copies', name: 'Copies', width: '15%', cellClass: 'text-center', enableCellEdit: true, enableSorting: false },
                     { field: 'Name_Doc', name: 'Doc. Name', width: '30%', cellClass: 'text-left', enableCellEdit: false, enableSorting: false }
                   ],
                   data: $scope.ExportDocTnC.ExportDocForwardings,
                   onRegisterApi: function (gridApi) {
                       debugger;
                       $scope.gridApiForwarding = gridApi;
                       $scope.gridApiForwarding.grid.modifyRows($scope.ExportDocTnC.ExportDocForwardings);
                       for (var i = 0; i < $scope.ExportDocTnC.ExportDocForwardings.length; i++)
                       {
                           if (Boolean($scope.ExportDocTnC.ExportDocForwardings[i].Selected))
                           { 
                               $scope.gridApiForwarding.selection.selectRow($scope.ExportDocTnC.ExportDocForwardings[i]);
                           }
                       }
                       
                   }
               };
                          
               //forwarding

             //  CheckExistingForwardingInfo();
               //party info set
               $scope.gridOptionsPartyInfoBill = {
                   enableRowSelection: true,
                   enableSelectAll: true,
                   multiSelect: true,
                   enableRowSelection: true,
                   columnDefs: [
                     { field: 'PartyInfo', name: 'Party Info', width: '90%' }
                     
                   ],
                   data: $scope.ExportDocTnC.ExportPartyInfoBills,
                   onRegisterApi: function (gridApi) {
                       debugger;
                       $scope.gridApiPartyInfo = gridApi;
                       $scope.gridApiPartyInfo.grid.modifyRows($scope.ExportDocTnC.ExportPartyInfoBills);
                       for (var i = 0; i < $scope.ExportDocTnC.ExportPartyInfoBills.length; i++) {
                           if (Boolean($scope.ExportDocTnC.ExportPartyInfoBills[i].Selected)) {
                               $scope.gridApiPartyInfo.selection.selectRow($scope.ExportDocTnC.ExportPartyInfoBills[i]);
                           }
                       }
                       
                   }
               };

               
               
               //CheckExistingPartyInfo();


          
                //$scope.ExportPartyInfoBills = obj.ExportPartyInfoBills;
                //$scope.ApplicantIDs = "";
                //$scope.BankAccountID = 0;
                
            }
            $scope.setcontrol();
            $scope.closedocument = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




