debugger;
var ImportInvChallanservice = angular.module('ImportInvChallan.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
ImportInvChallanservice.service('advsearchIIChallanService', function ($uibModal) {

    debugger;
   
    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/ImportInvChallan/AdvanceSearch',
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

        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants,msModal,icsMethod, obj) {
            debugger;
            $(document).on('mousemove', '.modal-body', function () {
                $('.date-container').datepicker({
                    format: "dd M yyyy",
                    calendarWeeks: true,
                    autoclose: true,
                    todayHighlight: true
                });
            });
            
            $scope.lblLoadingMessage = true;
            $scope.PickerTitle ="Invoice Challan Search";
            $scope.SearchKeyDownApplicant = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ApplicantName = $.trim($scope.ApplicantName);
                    if (ApplicantName == "" || ApplicantName == null)
                    {
                        alert("Type Applicant Name and Press Enter");
                        return;
                    }
                    $scope.PickApplicant();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.ApplicantName = '';
                    $scope.ApplicantIDs ='';
                }
            };
            $scope.PickApplicant = function () {
                debugger;
                var oContractor = {
                    Params: '2,3' + '~' + $.trim($scope.ApplicantName) + '~' + _nBUID
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Applicant Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'ImportInvChallanController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Applicant List',
                                            searchingbyfieldName: 'Name',
                                            title:'abc',
                                            columns: oColumns
                                        }
                                           var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.ApplicantName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            $scope.ApplicantIDs =icsMethod.ICS_PropertyConcatation(result,'ContractorID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

           

            $scope.IssueDateChange = function()
            {
                $scope.txtChallanDateFrom = $scope.txtChallanDateTo = icsdateformat(new Date());
                $scope.txtChallanDateToDisabled = icsMethod.IsDateBoxdisabled($scope.cboChallanDate);
            }
           
            $scope.search = function ()
            {
                debugger;
                $scope.lblLoadingMessage = false;
                if ($scope.cboChallanDate == 0 && ($scope.ChallanNo == undefined ? "" : $scope.ChallanNo) == "" && ($scope.ImportInvoiceNo == undefined ? "" : $scope.ImportInvoiceNo) == "")
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                var sTempString = $scope.cboChallanDate + '~' + $scope.txtChallanDateFrom + '~' + $scope.txtChallanDateTo + '~' + ($scope.ChallanNo == undefined ? "" : $scope.ChallanNo) + '~' + ($scope.ImportInvoiceNo == undefined ? "" : $scope.ImportInvoiceNo);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
            
                $http.get(_sBaseAddress + '/ImportInvChallan/AdvSearch', { params: { sTemp: sTempString } }, config).then(
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
               // //$scope.Sessions = obj.BussinessSessions;//Load BussinessSessions
               $scope.CompareOperators = obj.CompareOperators;
               $scope.ChallanNo = $scope.ImportInvoiceNo = '';
               $scope.cboChallanDate = $scope.CompareOperators[0].id;
               $scope.txtChallanDateFrom = $scope.txtChallanDateTo = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




