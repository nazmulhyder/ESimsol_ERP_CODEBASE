
var ExportDocTnCservice = angular.module('PrintDocument.service', ['ms.service', 'ngAnimate', 'ui.bootstrap', 'ui.grid.edit']);
ExportDocTnCservice.service('PrintDocumentservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/CommercialInvoice/PrintDocument',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        ExportDocSetups: (modalProperty.ExportDocSetups == undefined) ? [] : modalProperty.ExportDocSetups,
                        InvoiceID: (modalProperty.InvoiceID == undefined) ? null : modalProperty.InvoiceID,
                        ObjectControllerName: (modalProperty.ObjectControllerName == undefined) ? '' : modalProperty.ObjectControllerName,
                        ObjectActionName: (modalProperty.ObjectActionName == undefined) ? '' : modalProperty.ObjectActionName
                    };
                }
                
            }
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj) {
            debugger;
 
           $scope.Preview = function ()
            {
               debugger;
               if (!$scope.chkPrintNormal && !$scope.chkPrintTitleInPAD && !$scope.chkPrintTitleInImg) {
                   alert("Please select one format.");
                   return;
               }
               var nPrintType = 0;
               if ($scope.chkPrintNormal)
               {
                   nPrintType = 1;
               } else if ($scope.chkPrintTitleInPAD)
               {
                   nPrintType = 2;
               } else if ($scope.chkPrintTitleInImg)
               {
                   nPrintType = 3;
               }

               if (nPrintType == 0) {
                   alert("Please select one format. ");
                   return;
               }
               var nUnitType = parseInt($scope.rdoUnit);
               var nPageSize = parseInt($scope.rdoPageSize);
               if (obj.InvoiceID == null || obj.InvoiceID <= 0) {
                   alert("Sorry, There is no Invoice. Please contract to Developer For solving this Issue.");
                   return;
               }

               if (obj.ObjectControllerName == '') {
                   alert("Sorry, There is no Object Controller Name. Please contract to Developer For solving this Issue.");
                   return;
               }
               if (obj.ObjectActionName == '') {
                   alert("Sorry, There is no Object Action Name. Please contract to Developer For solving this Issue.");
                   return;
               }
               
               var oExportDocs = $scope.gridApiPrintFormat.selection.getSelectedRows();
               if (oExportDocs == null || oExportDocs.length <= 0) { alert("Please select at least one Export Document Setup."); return; }
               $uibModalInstance.close();
               for (var i = 0; i < oExportDocs.length; i++)
               {
                   window.open(sessionStorage.getItem('BaseAddress') + '/' + obj.ObjectControllerName + '/' + obj.ObjectActionName + '?id=' + obj.InvoiceID + "&nDocType=" + oExportDocs[i].ExportDocSetupID + "&nPrintType=" + nPrintType + "&nUnitType=" + nUnitType + "&nPageSize=" + nPageSize, "_blank");
               }
               
            };
           $scope.setcontrol = function ()
            {
               debugger;
               $scope.ExportDocSetups = obj.ExportDocSetups;
               $scope.chkPrintNormal = true;//Normal print
               $scope.rdoUnit = 2;//LBS
               $scope.rdoPageSize = 1;//A4

               $scope.gridOptionsPrintFormat = {
                   enableRowSelection: true,
                   enableSelectAll: true,
                   multiSelect: true,
                   enableRowSelection: true,
                   columnDefs: [
                     { field: 'DocName', name: 'Doc Name', width: '100%', cellClass: 'text-left', enableSorting: false }
                   ],
                   data: $scope.ExportDocSetups,
                   onRegisterApi: function (gridApi) {
                       debugger;
                       $scope.gridApiPrintFormat = gridApi;

                   }
               };

            }
           $scope.setcontrol();
            $scope.closedocument = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




