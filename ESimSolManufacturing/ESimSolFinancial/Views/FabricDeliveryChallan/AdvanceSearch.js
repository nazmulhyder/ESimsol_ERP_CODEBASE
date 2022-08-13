debugger;
var FabricDeliveryChallanservice = angular.module('FabricDeliveryChallan.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
FabricDeliveryChallanservice.service('advsearchFdcService', function ($uibModal) {

    debugger;
   
    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/FabricDeliveryChallan/AdvanceSearch',
            controller: 'ModalAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        FDOSetups: (modalProperty.FDOSetups == undefined) ? [] : modalProperty.FDOSetups,
                        IsSample: (modalProperty.IsSample == undefined) ? false : modalProperty.IsSample
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
            $scope.PickerTitle ="Challan Search";
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
                                            appcontroller: 'FabricDeliveryChallanController',
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
                $scope.txtDCDateFrom = $scope.txtDCDateTo = icsdateformat(new Date());
                $scope.txtDCDateToDisabled = icsMethod.IsDateBoxdisabled($scope.cboDCDate);
            }
           
            $scope.search = function ()
            {
                debugger;
                $scope.lblLoadingMessage = false;
                if ($scope.cboDCDate == 0 && $scope.cboFDOType == 0 && ($scope.ChallanNo == undefined ? "" : $scope.ChallanNo) == "" && ($scope.ApplicantIDs == undefined ? "" : $scope.ApplicantIDs) == "" && ($scope.FDONo == undefined ? "" : $scope.FDONo) == "" && ($scope.FabricNo == undefined ? "" : $scope.FabricNo) == "" && ($scope.LotNo == undefined ? "" : $scope.LotNo) == "" && ($scope.OrderNo == undefined ? "" : $scope.OrderNo) == "" && ($scope.MKTPersonIDs == undefined ? "" : $scope.MKTPersonIDs) == "" && ($scope.MKTGroupIDs == undefined ? "" : $scope.MKTGroupIDs) == "")
                {
                    alert("Please Select at least one Criteria !!");
                    return;
                }
                var sTempString = $scope.cboDCDate
                                  + '~' + $scope.txtDCDateFrom
                                  + '~' + $scope.txtDCDateTo + '~'
                                  + ($scope.ChallanNo == undefined ? "" : $scope.ChallanNo)
                                  + '~' + ($scope.FDONo == undefined ? "" : $scope.FDONo)
                                  + '~' + ($scope.ApplicantIDs == undefined ? "" : $scope.ApplicantIDs)
                                  + '~' + ($scope.FabricNo == undefined ? "" : $scope.FabricNo)
                                  + '~' + ($scope.LotNo == undefined ? "" : $scope.LotNo)
                                  + '~' + ($scope.OrderNo == undefined ? "" : $scope.OrderNo)
                                  + '~' + ($scope.IsSample == undefined ? false : $scope.IsSample)
                                  + '~' + ($scope.cboFDOType == undefined ? 0 : $scope.cboFDOType)
                                  + '~' + ($scope.PINo == undefined ? "" : $scope.PINo)
                                  + '~' + ($scope.MKTPersonIDs == undefined ? "" : $scope.MKTPersonIDs)
                                  + '~' + ($scope.MKTGroupIDs == undefined ? "" : $scope.MKTGroupIDs);

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                var a = _sBaseAddress;
                var a1 = sessionStorage.getItem('BaseAddress');
                $http.get(_sBaseAddress + '/FabricDeliveryChallan/AdvSearch', { params: { sTemp: sTempString } }, config).then(
                            function (response)
                            {
                                $scope.lblLoadingMessage = true;
                                //var results = jQuery.parseJSON(response.data);
                                var results = response.data;
                                if (results.length > 0)
                                {
                                    if (results[0].ErrorMessage != '')
                                    {
                                        alert(results[0].ErrorMessage);return;
                                    }
                                    $uibModalInstance.close(results);
                                }
                                else
                                {
                                    alert("Data Not Found."); return;
                                }
                                
                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );
            };
            $scope.reset = function ()
            {
               // //$scope.Sessions = obj.BussinessSessions;//Load BussinessSessions
                $scope.CompareOperators = obj.CompareOperators;
                $scope.FDOSetups = obj.FDOSetups;
                $scope.IsSample = obj.IsSample;

                $scope.ChallanNo = $scope.FDONo = $scope.ApplicantName = $scope.MKTPersonName = $scope.MKTGroupName = '';
                $scope.ApplicantIDs = $scope.MKTPersonIDs = $scope.MKTGroupIDs = "";
                $scope.cboDCDate = $scope.CompareOperators[0].id;
                $scope.cboFDOType = $scope.FDOSetups[0].id;
                $scope.txtDCDateFrom = $scope.txtDCDateTo = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }
        return $uibModal.open(modalInstance);
    }


});




