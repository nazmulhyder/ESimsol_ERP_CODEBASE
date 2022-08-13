
var masterlcservice = angular.module('masterlc.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
masterlcservice.service('lcservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl:sessionStorage.getItem('BaseAddress') + '/MasterLC/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        BussinessSessions: (modalProperty.BussinessSessions == undefined) ? [] : modalProperty.BussinessSessions,
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
            $scope.PickerTitle ="Master LC Search";
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
                    Params: '2' + '~' + $.trim($scope.ApplicantName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Applicant Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = response.data;
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'MasterLCController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Applicant List',
                                            searchingbyfieldName: 'Name',
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

            $scope.SearchKeyDownAdviceBank = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var AdviceBankName = $.trim($scope.AdviceBankName);
                    if (AdviceBankName == "" || AdviceBankName == null) {
                        alert("Type Advice bank Account and Press Enter");
                        return;
                    }
                    $scope.AdviceBank();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope. AdviceBankName = '';
                    $scope. AdviceBankID = 0;
                }
            };
            $scope.AdviceBank = function () {
                var oBankAccount = {
                    BusinessUnitID: sessionStorage.getItem('BUID'),
                    OperationalDeptInInt: '4',//EnumOperationalDept : Export_Own=4
                    AccountNo: $.trim($scope. AdviceBankName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(sessionStorage.getItem('BaseAddress') + '/BankAccount/GetsBUWithDeptWise', $.param(oBankAccount), config).then(
                            function (response) {
                                var oColumns = [];
                                var oColumn = { field: 'BankNameAccountNo', name: 'Account No', width: '20%' }; oColumns.push(oColumn);
                                oColumn = { field: 'BranchName', name: 'Branch Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                var results = jQuery.parseJSON(response.data);
                                var modalObj = {
                                    size: 'md',
                                    modalcontroller: 'ModalCommonListCtrl',
                                    appcontroller: 'MasterLCController',
                                    objs: results,
                                    multiSelect: false,
                                    pickertitle: 'Bank Account List',
                                    searchingbyfieldName: 'BankNameAccountNo',
                                    columns: oColumns
                                }
                                var modalInstance = msModal.Instance(modalObj);
                                modalInstance.result.then(function (result) {
                                    debugger;
                                    $scope.AdviceBankName = result.BankNameAccountNo;
                                    $scope.AdviceBankID = result.BankAccountID;

                                }, function () {
                                    $log.info('Modal dismissed at: ' + new Date());
                                });
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );

            };


            $scope.IssueDateChange = function()
            {
                $scope.LCIssueDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboIssueDate);
            }
            $scope.ExpireDateChange = function () {
                $scope.LCExpireDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboExpireDate);
            }
            $scope.ReceiveDateChange = function () {
                $scope.LCReceiveDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboReceiveDate);
            }
            $scope.ShipmentDateChange = function () {
                $scope.LCShipmentDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboShipmentDate);
            }
            $scope.search = function ()
            {
                debugger;
                $scope.lblLoadingMessage = false;
                if ($scope.cboIssueDate == 0 && $scope.cboReceiveDate == 0 && $scope.cboShipmentDate == 0 && $scope.cboExpireDate == 0 && ($scope.BankAccountID == undefined ? 0 : $scope.BankAccountID) == 0 && $scope.cboSession == 0 && ($scope.LCNo == undefined ? "" : $scope.LCNo)== "" && ($scope.ApplicantIDs== undefined ? "" : $scope.ApplicantIDs) == "" && ($scope.FileNo == undefined ? "" : $scope.FileNo)== "" && ($scope.PINo== undefined ? "" : $scope.PINo) == "")
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                var sTempString = $scope.cboIssueDate + '~' + $scope.LCIssueDateStart + '~' + $scope.LCIssueDateEnd + '~' + $scope.cboReceiveDate + '~' + $scope.LCReceiveDateStart + '~' + $scope.LCReceiveDateEnd + '~' + $scope.cboShipmentDate + '~' + $scope.LCShipmentDateStart + '~' + $scope.LCShipmentDateEnd + '~' + $scope.cboExpireDate + '~' + $scope.LCExpireDateStart + '~' + $scope.LCExpireDateEnd + '~' +( $scope.LCNo == undefined ?"": $scope.LCNo) + '~' + ($scope.FileNo == undefined ? "" : $scope.FileNo) + '~' + ($scope.ApplicantIDs == undefined ? "" : $scope.ApplicantIDs) + '~' + ($scope.PINo == undefined ? "" : $scope.PINo )+ '~' + ($scope.BankAccountID == undefined ? 0 : $scope.BankAccountID)+ '~' + ($scope.cboSession==undefined ?0:$scope.cboSession)+'~'+sessionStorage.getItem('BUID');
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(sessionStorage.getItem('BaseAddress') + '/MasterLC/Search', {params:{sTemp:sTempString } }, config).then(
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
                $scope.Sessions = obj.BussinessSessions;//Load BussinessSessions
                $scope.CompareOperators = obj.CompareOperators;
                $scope.LCNo = $scope.FileNo = $scope.PINo = $scope.ApplicantName = $scope.AdviceBankName = '';
                $scope.ApplicantIDs = "";
                $scope.BankAccountID = 0;
                $scope.cboSession = $scope.Sessions[$scope.Sessions.length - 1].BusinessSessionID;
                $scope.cboIssueDate = $scope.cboReceiveDate = $scope.cboShipmentDate = $scope.cboExpireDate = $scope.CompareOperators[0].id;
                $scope.LCIssueDateEndDisabled = $scope.LCExpireDateEndDisabled = $scope.LCReceiveDateEndDisabled = $scope.LCShipmentDateEndDisabled = true;
                $scope.LCIssueDateStart = $scope.LCIssueDateEnd = $scope.LCExpireDateStart = $scope.LCExpireDateEnd = $scope.LCShipmentDateStart = $scope.LCShipmentDateEnd = $scope.LCReceiveDateStart = $scope.LCReceiveDateEnd = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




