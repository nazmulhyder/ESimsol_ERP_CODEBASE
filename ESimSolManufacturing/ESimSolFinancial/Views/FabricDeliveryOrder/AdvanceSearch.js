﻿debugger;
var FabricDeliveryOrderservice = angular.module('FabricDeliveryOrder.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
FabricDeliveryOrderservice.service('advsearchFdoService', function ($uibModal) {

    debugger;
   
    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/FabricDeliveryOrder/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        FDOTypes: (modalProperty.FDOTypes == undefined) ? [] : modalProperty.FDOTypes
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
            $scope.PickerTitle = "DO Search";

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
                                            appcontroller: 'FabricDeliveryOrderController',
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

            $scope.SearchKeyDownMKTPerson = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var MKTPersonName = $.trim($scope.MKTPersonName);
                    if (MKTPersonName == "" || MKTPersonName == null) {
                        alert("Type MKTPerson Name and Press Enter");
                        return;
                    }
                    $scope.PickMKTPerson();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.MKTPersonName = '';
                    $scope.MKTPersonIDs = '';
                }
            };
            $scope.PickMKTPerson = function () {
                debugger;
                var oMarketingAccount_BU = {
                    Name: $.trim($scope.MKTPersonName),
                    //BUID: _nBUID
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/MarketingAccount/GetsMarketingAccount', $.param(oMarketingAccount_BU), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'Name', name: 'Name', width: '50%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '40%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'FabricDeliveryOrderController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'MKT Person List',
                                            searchingbyfieldName: 'Name',
                                            title: 'abc',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.MKTPersonName = result.length > 1 ? result.length + " Item's Selected" : result[0].Name;
                                            $scope.MKTPersonIDs = icsMethod.ICS_PropertyConcatation(result, 'MarketingAccountID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.SearchKeyDownMKTGroup = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var MKTGroupName = $.trim($scope.MKTGroupName);
                    if (MKTGroupName == "" || MKTGroupName == null) {
                        alert("Type MKT Group Name and Press Enter");
                        return;
                    }
                    $scope.PickMKTGroup();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.MKTGroupName = '';
                    $scope.MKTGroupIDs = '';
                }
            };
            $scope.PickMKTGroup = function () {
                debugger;
                var oMarketingAccount_BU = {
                    Name: $.trim($scope.MKTGroupName),
                    //BUID: _nBUID
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/MarketingAccount/GetsGroup', $.param(oMarketingAccount_BU), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'Name', name: 'Name', width: '50%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '40%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'FabricDeliveryOrderController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'MKT Person List',
                                            searchingbyfieldName: 'Name',
                                            title: 'abc',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.MKTGroupName = result.length > 1 ? result.length + " Item's Selected" : result[0].Name;
                                            $scope.MKTGroupIDs = icsMethod.ICS_PropertyConcatation(result, 'MarketingAccountID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.IssueDateChange = function()
            {
                $scope.txtDODateFrom = $scope.txtDODateTo = icsdateformat(new Date());
                $scope.txtDODateToDisabled = icsMethod.IsDateBoxdisabled($scope.cboDODate);
            }
           
            $scope.search = function ()
            {
                debugger;
                if (($scope.FDOTypeInInt == undefined ? 0 : $scope.FDOTypeInInt) <= 0 && $scope.cboDODate == 0 && ($scope.PINo == undefined ? "" : $scope.PINo) == "" && ($scope.LCNo == undefined ? "" : $scope.LCNo) == "" && ($scope.FDONo == undefined ? "" : $scope.FDONo) == "" && ($scope.ApplicantIDs == undefined ? "" : $scope.ApplicantIDs) == "" && ($scope.SCNo == undefined ? "" : $scope.SCNo) == "" && ($scope.MKTPersonIDs == undefined ? "" : $scope.MKTPersonIDs) == "" && ($scope.MKTGroupIDs == undefined ? "" : $scope.MKTGroupIDs) == "")
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                $scope.lblLoadingMessage = false;
                var sTempString = $scope.cboDODate + '~' + $scope.txtDODateFrom + '~' + $scope.txtDODateTo + '~' + ($scope.FDONo == undefined ? "" : $scope.FDONo) + '~' + ($scope.SCNo == undefined ? "" : $scope.SCNo) + '~' + ($scope.ApplicantIDs == undefined ? "" : $scope.ApplicantIDs) + '~' + ($scope.PINo == undefined ? "" : $scope.PINo) + '~' + ($scope.LCNo == undefined ? "" : $scope.LCNo) + '~' + ($scope.FDOTypeInInt == undefined ? 0 : $scope.FDOTypeInInt) + '~' + ($scope.MKTPersonIDs == undefined ? "" : $scope.MKTPersonIDs) + '~' + ($scope.MKTGroupIDs == undefined ? "" : $scope.MKTGroupIDs);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                var a = _sBaseAddress;
                var a1 = sessionStorage.getItem('BaseAddress');
                $http.get(_sBaseAddress + '/FabricDeliveryOrder/AdvSearch', { params: { sTemp: sTempString } }, config).then(
                            function (response)
                            {
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.length > 0)
                                {
                                    if (results[0].ErrorMessage == '')
                                        $uibModalInstance.close(results);
                                    else
                                        alert(results[0].ErrorMessage);
                                }
                                else
                                {
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
                $scope.FDOTypes = obj.FDOTypes;
                $scope.FDONo = $scope.SCNo = $scope.ApplicantName = $scope.PINo = $scope.LCNo = $scope.MKTPersonName = $scope.MKTGroupName = '';
                $scope.ApplicantIDs = $scope.MKTPersonIDs = $scope.MKTGroupIDs =  "";
                $scope.cboDODate = $scope.CompareOperators[0].id;
                $scope.txtDODateFrom = $scope.txtDODateTo = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




