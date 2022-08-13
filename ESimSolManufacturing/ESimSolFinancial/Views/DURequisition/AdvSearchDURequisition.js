var IICService = angular.module('DURequisition.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
IICService.service('DURequisitionservice', function ($uibModal) {
    _sBaseAddress = sessionStorage.getItem("BaseAddress");
    
    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/DURequisition/AdvSearchDURequisition',
            controller: 'ModalDURequisitionAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        RequisitionTypes: (modalProperty.RequisitionTypes == undefined) ? [] : modalProperty.RequisitionTypes,
                        IssueStores: (modalProperty.IssueStores == undefined) ? [] : modalProperty.IssueStores,
                        ReceiveStores: (modalProperty.ReceiveStores == undefined) ? [] : modalProperty.ReceiveStores,
                        OrderTypes: (modalProperty.OrderTypes == undefined) ? [] : modalProperty.OrderTypes,
                        StatusList: (modalProperty.StatusList == undefined) ? [] : modalProperty.StatusList,
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

          
            $scope.lblLoadingMessage = true;
            $scope.PickerTitle = "Search Requisition";

            $scope.SearchKeyDownUserName = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var UserName = $.trim($scope.UserName);
                    if (UserName == "" || UserName == null) {
                        // alert("Type UserName Name and Press Enter");
                        msModal.Message({ headerTitle: '', bodyText: 'Type User Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                        return;
                    }
                    $scope.PickUser();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.UserName = '';
                    $scope.UserIDs = '';
                }
            };
            $scope.PickUser = function () {
                debugger;
                var oUser = {
                    Params: '2,3' + '~' + $.trim($scope.UserName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/User/UserSearchByNameType', $.param(oUser), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'UserName', name: 'Name', width: '50%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'LocationName', name: 'Location Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'ImportInvoiceRequisitionController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'User List',
                                            searchingbyfieldName: 'UserName',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.UserName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            $scope.UserIDs = icsMethod.ICS_PropertyConcatation(result, 'UserID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.SearchKeyDownCPName = function (e) {
                debugger;

                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var CPName = $.trim($scope.CPName);
                    
                    $scope.PickCPName();
                } else if (code == 8) //backspace=8
                {
                    $scope.CPName = '';
                    $scope.CPIDs = '';
                }
            };
            $scope.PickCPName = function () {
               
                var sUserID = '';
                if ($scope.UserIDs.length > 0) {
                    sUserID = $scope.UserIDs;
                }
                debugger;
                var oBuyerConcern = {
                    UserName: sUserID,
                    Name: $.trim($scope.CPName)
                };

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/UserPersonal/GetsByUsers', $.param(oBuyerConcern), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ContactPersonnelID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Personnel Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'UserName', name: 'User Name', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'ImportInvoiceRequisitionController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Person List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }

                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.CPName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            
                                            $scope.CPIDs = icsMethod.ICS_PropertyConcatation(result, 'ContactPersonnelID');
                                            
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.SearchKeyDownYarnName = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var YarnName = $.trim($scope.YarnName);
                    if (YarnName == "" || YarnName == null) {
                        // alert("Type YarnName Name and Press Enter");
                        msModal.Message({ headerTitle: '', bodyText: 'Type Yarn Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                        return;
                    }
                    $scope.PickYarn();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.YarnName = '';
                    $scope.YarnIDs = '';
                }
            };
            $scope.PickYarn = function () {
                debugger;
                var oYarn = {
                    Params: $.trim($scope.YarnName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/DURequisition/YarnSearchByName', $.param(oYarn), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ProductName', name: 'Name', width: '70%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'ProductCode', name: 'Code', width: '25%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = jQuery.parseJSON(response.data);
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'ImportInvoiceRequisitionController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Yarn List',
                                            searchingbyfieldName: 'ProductName',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.YarnName = result.length > 1 ? result.length + " Item's Selected" : result[0].ProductName;
                                            $scope.YarnIDs = icsMethod.ICS_PropertyConcatation(result, 'ProductID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };

            $scope.SearchKeyDownContractorName = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ContractorName = $.trim($scope.ContractorName);
                    if (ContractorName == "" || ContractorName == null) {
                        // alert("Type ContractorName Name and Press Enter");
                        msModal.Message({ headerTitle: '', bodyText: 'Type Contractor Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                        return;
                    }
                    $scope.PickContractor();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.ContractorName = '';
                    $scope.ContractorIDs = '';
                }
            };
            $scope.PickContractor = function () {
                debugger;
                var oContractor = { Params: '2,3' + '~' + (typeof ($.trim($scope.ContractorName)) != 'undefined' ? $.trim($scope.ContractorName) : "") };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'Name', name: 'Name', width: '40%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '25%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'ShortName', name: 'ShortName', width: '25%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = response.data;
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'ImportInvoiceRequisitionController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Buyer List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.ContractorName = result.length > 1 ? result.length + " Item's Selected" : result[0].Name;
                                            $scope.ContractorIDs = icsMethod.ICS_PropertyConcatation(result, 'ContractorID');
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };
           
            $scope.RequisitionDateChange = function () {
                $scope.RequisitionDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboRequisitionDate);
            }

            $scope.ReceiveDateChange = function () {
                $scope.ReceiveDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboReceiveDate);
            }
            $scope.ApproveDateChange = function () {
                $scope.ApproveDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboApproveDate);
            }
            
            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
             
                //if ($scope.RequisitionNo == "" && $scope.LotNo == "" && $scope.OrderNo == "" && $scope.cboOrderType == 0 && $scope.cboRequisitionDate == 0 && $scope.cboIssueStore == 0 && $scope.cboReceiveStore == 0 && ($scope.UserIDs == undefined ? "" : $scope.UserIDs) == "" && $scope.cboRequisitionType == 0) {
                //    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                //    alert("Please Select At Least One Searching Criteria!!");
                //    $scope.lblLoadingMessage = false;
                //    return false;
                //}

                if ($scope.RequisitionNo == "" && $scope.cboRequisitionType == 0 && $scope.cboOrderType == 0 && $scope.OrderNo == "" && $scope.LotNo == "" && $scope.cboIssueStore == 0 && $scope.cboReceiveStore == 0 && $scope.cboRequisitionDate == 0 && $scope.cboStatus == 0 && $scope.cboReceiveDate == 0 && ($scope.YarnIDs == undefined ? "" : $scope.YarnIDs) == "" && $scope.cboApproveDate == 0) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }

                if(($scope.OrderNo == undefined ? "" : $scope.OrderNo) !="" && ($scope.cboOrderType == undefined ? 0 : $scope.cboOrderType)==0){
                    alert("Please Select Order Type"); return;
                }

                //var sTempString = $scope.cboRequisitionDate + '~' + $scope.RequisitionDateStart + '~' + $scope.RequisitionDateEnd + '~' + ($scope.cboIssueStore == undefined ? 0 : $scope.cboIssueStore) + '~' + ($scope.cboReceiveStore == undefined ? 0 : $scope.cboReceiveStore) + '~' + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive) + '~' + sessionStorage.getItem('BUID') + '~' + ($scope.cboRequisitionType == undefined ? 0 : $scope.cboRequisitionType) + '~' + ($scope.RequisitionNo == undefined ? "" : $scope.RequisitionNo) + '~' + ($scope.OrderNo == undefined ? "" : $scope.OrderNo) + '~' + ($scope.cboOrderType == undefined ? 0 : $scope.cboOrderType) + '~' + ($scope.LotNo == undefined ? "" : $scope.LotNo);
             
                var sTempString = $scope.RequisitionNo + '~' + ($scope.cboRequisitionType == undefined ? 0 : $scope.cboRequisitionType) + '~' + $scope.cboOrderType + '~' + $scope.OrderNo + '~' + $scope.LotNo + '~' + ($scope.cboIssueStore == undefined ? 0 : $scope.cboIssueStore) + '~' + ($scope.cboReceiveStore == undefined ? 0 : $scope.cboReceiveStore) + '~' + $scope.cboRequisitionDate + '~' + $scope.RequisitionDateStart + '~' + $scope.RequisitionDateEnd + '~' + ($scope.cboStatus == undefined ? 0 : $scope.cboStatus) + '~' + $scope.cboReceiveDate + '~' + $scope.ReceiveDateStart + '~' + $scope.ReceiveDateEnd + '~' + ($scope.YarnIDs == undefined ? "" : $scope.YarnIDs) + '~' + ($scope.ContractorIDs == undefined ? "" : $scope.ContractorIDs) + '~' + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive) + '~' + sessionStorage.getItem('BUID') + '~' + $scope.cboApproveDate + '~' + $scope.ApproveDateStart + '~' + $scope.ApproveDateEnd + "~" + _nOptType;
                
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(_sBaseAddress + '/DURequisition/AdvSearch', { params: { sTemp: sTempString } }, config).then(
                            function (response) {
                                debugger;
                                //alert(sTempString); return;
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.length > 0) {
                                    $uibModalInstance.close(results);
                                } else
                                {
                                    msModal.Message({ headerTitle: '', bodyText: 'No Data Found !!', sucessText: 'Yes', cancelText: 'Close', feedbackType: false, autoClose: false });
                                    // alert("Data Not Found.");
                                    return;
                                }

                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );
            };
            $scope.reset = function () {
                debugger;
                $scope.CompareOperators = obj.CompareOperators;
                $scope.IssueStores = obj.IssueStores;
                $scope.ReceiveStores = obj.ReceiveStores;
                $scope.OrderTypes = obj.OrderTypes; 
                $scope.cboOrderType = 0;
                $scope.OrderNo = $scope.RequisitionNo = $scope.LotNo = "";
                $scope.RequisitionTypes = obj.RequisitionTypes;
                //$scope.cboRequisitionType = $scope.RequisitionTypes[-1].InOutType;
                $scope.StatusList = obj.StatusList;
                //$scope.cboStatus = $scope.StatusList[0].id;
                $scope.UserIDs = "";
                $scope.UserName = "";
                $scope.cboRequisitionDate = $scope.CompareOperators[0].id;
                $scope.RequisitionDateEndDisabled = true;
                $scope.RequisitionDateStart = $scope.RequisitionDateEnd = icsdateformat(new Date());
                $scope.cboReceiveDate = $scope.CompareOperators[0].id;
                $scope.ReceiveDateEndDisabled = true;
                $scope.ReceiveDateStart = $scope.ReceiveDateEnd = icsdateformat(new Date());
                $scope.cboApproveDate = $scope.CompareOperators[0].id;
                $scope.ApproveDateEndDisabled = true;
                $scope.ApproveDateStart = $scope.ApproveDateEnd = icsdateformat(new Date());
                $scope.YarnName = '';
                $scope.YarnIDs = '';
                $scope.ContractorName = '';
                $scope.ContractorIDs = '';
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});