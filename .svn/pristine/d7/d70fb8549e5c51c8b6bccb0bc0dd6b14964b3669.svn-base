
var CommonHistory = angular.module('CommonHistory.service', ['ngAnimate', 'ui.bootstrap', 'ui.grid.resizeColumns']);
CommonHistory.service('commonhistoryservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: (modalProperty.size == undefined) ? 'sm' : modalProperty.size,
            templateUrl: sessionStorage.getItem('BaseAddress') + '/CommonModal/CommonHistoryModal',
            controller: (modalProperty.modalcontroller == undefined) ? 'ModalCommonHistoryCtrl' : modalProperty.modalcontroller,
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller
        }

        modalInstance.controller = function ($scope, $log, $http, $uibModalInstance, uiGridConstants)
        {
            debugger;
            $scope.PickerTitle = (modalProperty.pickertitle != null) ? modalProperty.pickertitle : "History List";
            $scope.CommonHistorygridOptions = {
                showColumnFooter: modalProperty.showColumnFooter,
                enableSorting: true,
                enableRowSelection: true,
                enableColumnResizing: true,
                enableSelectAll: modalProperty.multiSelect,
                multiSelect: modalProperty.multiSelect,
                columnDefs: modalProperty.columnDefs,
                data: modalProperty.result,
                onRegisterApi: function (gridApi) {
                    $scope.gridApiCommonHistoryModal = gridApi;
                }
            };

            $scope.PrintPreview = function () {
                debugger;
                var oSlectedObject = $scope.gridApiCommonHistoryModal.selection.getSelectedRows()[0];
                //ProformaInvoiceLogID 
                if (oSlectedObject == null || oSlectedObject[modalProperty.objectFieldName] <= 0) {
                    alert("Please select a item from list!");
                    return;
                }
                //controlleractionName
                window.open(sessionStorage.getItem('BaseAddress') + '/' + modalProperty.controllerName + '/' + modalProperty.controlleractionName + '?id=' + oSlectedObject[modalProperty.objectFieldName]);
                $uibModalInstance.dismiss('cancel');
            };

            $scope.close = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }

});


