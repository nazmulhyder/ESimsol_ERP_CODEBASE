
var PackageTemplateservice = angular.module('PackageTemplate.service', ['ms.service', 'ngAnimate', 'ui.bootstrap', 'ui.grid']);
PackageTemplateservice.service('PackageTemplateservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: sessionStorage.getItem('BaseAddress') + '/PackageTemplate/PackageTemplatePicker',
            controller: 'ModalPackageTemplateCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller
        }

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants,msModal,icsMethod) {
            debugger; 
                $scope.PackageTemplates = [];
                $scope.PackageTemplateDetails = [];
  
            $scope.gridOptionsPackageTemplate = {
                //showColumnFooter: true,
                multiSelect: false,
                enableRowSelection: true,
                enableSelectAll: false,
                enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
                columnDefs: [
                  { field: 'PackageNo', name: 'Package No', cellClass: 'text-left', width: '40%', enableCellEdit: false },
                  { field: 'PackageName', name: 'Package Name', cellClass: 'text-left', width: '60%', enableCellEdit: false }
                ],
                data: $scope.PackageTemplates,
                onRegisterApi: function (gridApi) {
                    $scope.gridApiPackageDetails = gridApi;
                    gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                        debugger;
                        $scope.RowSelect(row.entity);
                    });
                }
            };

            $scope.RowSelect = function (oPackageTemplate) {
                var oPackageTemplateDetails = [];
                for (var i = 0; i < $scope.PackageTemplateDetails.length; i++)
                {
                    if (parseInt($scope.PackageTemplateDetails[i].PackageTemplateID) == parseInt(oPackageTemplate.PackageTemplateID)) {
                        oPackageTemplateDetails.push($scope.PackageTemplateDetails[i]);
                    }
                }
                $scope.gridOptionsPackageTemplateDetail.data = oPackageTemplateDetails;
            };


            $scope.gridOptionsPackageTemplateDetail = {
                //showColumnFooter: true,
                multiSelect: false,
                enableRowSelection: true,
                enableSelectAll: false,
                enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
                columnDefs: [
                  { field: 'ProductName', name: 'Product Name', cellClass: 'text-left', width: '40%', enableCellEdit: false },
                  { field: 'Quantity', name: 'Quantity', cellClass: 'text-right', width: '60%', enableCellEdit: false }
                ],
                data: $scope.PackageTemplateDetails,
                onRegisterApi: function (gridApi) {
                    $scope.gridApiPackageDetails = gridApi;
                   
                }
            };


            $scope.LoadPackageTemplates = function () {
                var oPackageTemplate = {};
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/PackageTemplate/GetPackageTemplates', $.param(oPackageTemplate), config).then(
                                    function (response)
                                    {
                                        var oPackageTemplate = jQuery.parseJSON(response.data);
                                        $scope.PackageTemplates = oPackageTemplate.PackageTemplateList;
                                        $scope.PackageTemplateDetails = oPackageTemplate.PackageTemplateDetails;
                                        $scope.gridOptionsPackageTemplate.data = $scope.PackageTemplates;
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };
         
            $scope.OkButtonClick = function () {
                debugger;
                if ($scope.gridOptionsPackageTemplateDetail.data == null)
                {
                    alert("Sorry, There is no Package Template Details. Please select Package Template");
                    return;
                }
                $uibModalInstance.close($scope.gridOptionsPackageTemplateDetail.data);
            };
          
            $scope.LoadPackageTemplates();
            $scope.Close = function () {
                debugger;
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




