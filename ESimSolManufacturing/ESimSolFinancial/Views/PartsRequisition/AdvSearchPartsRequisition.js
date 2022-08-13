var IICService = angular.module('PartsRequisition.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
IICService.service('PartsRequisitionservice', function ($uibModal) {

    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/PartsRequisition/AdvSearchPartsRequisition',
            controller: 'ModalPartsRequisitionAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        RequisitionTypes: (modalProperty.RequisitionTypes == undefined) ? [] : modalProperty.RequisitionTypes,
                        IssueStores: (modalProperty.IssueStores == undefined) ? [] : modalProperty.IssueStores,
                        RequisitionUser: (modalProperty.RequisitionUser == undefined) ? [] : modalProperty.RequisitionUser,
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

            //$scope.SearchKeyDownUserName = function (e) {
            //    debugger;
            //    var code = (e.keyCode ? e.keyCode : e.which);
            //    if (code == 13) {
            //        var UserName = $.trim($scope.UserName);
            //        if (UserName == "" || UserName == null) {
            //            // alert("Type UserName Name and Press Enter");
            //            msModal.Message({ headerTitle: '', bodyText: 'Type User Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
            //            return;
            //        }
            //        $scope.PickUser();
            //    } else if (code == 8) //backspace=8
            //    {
            //        //debugger;
            //        $scope.UserName = '';
            //        $scope.UserIDs = '';
            //    }
            //};
            //$scope.PickUser = function () {
            //    debugger;
            //    var oUser = {
            //        Params: '2,3' + '~' + $.trim($scope.UserName) + '~' + sessionStorage.getItem('BUID')
            //    };
            //    var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
            //    $http.post(_sBaseAddress + '/User/UserSearchByNameType', $.param(oUser), config).then(
            //                        function (response) {
            //                            debugger;
            //                            var oColumns = [];
            //                            var oColumn = { field: 'UserName', name: 'Name', width: '50%' }; oColumns.push(oColumn);
            //                            oColumn = { field: 'LocationName', name: 'Location Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
            //                            var results = jQuery.parseJSON(response.data);
            //                            var modalObj = {
            //                                size: 'md',
            //                                modalcontroller: 'ModalCommonListCtrl',
            //                                appcontroller: 'ImportInvoiceRequisitionController',
            //                                objs: results,
            //                                multiSelect: true,
            //                                pickertitle: 'User List',
            //                                searchingbyfieldName: 'UserName',
            //                                columns: oColumns
            //                            }
            //                            var modalInstance = msModal.Instance(modalObj);
            //                            modalInstance.result.then(function (result) {
            //                                debugger;
            //                                $scope.UserName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
            //                                $scope.UserIDs = icsMethod.ICS_PropertyConcatation(result, 'UserID');
            //                            }, function () {
            //                                $log.info('Modal dismissed at: ' + new Date());
            //                            });
            //                        },
            //                          function (response) { alert(jQuery.parseJSON(response.data).Message); }
            //                    );
            //};
            //$scope.SearchKeyDownCPName = function (e) {
            //    debugger;

            //    var code = (e.keyCode ? e.keyCode : e.which);
            //    if (code == 13) {
            //        var CPName = $.trim($scope.CPName);
                    
            //        $scope.PickCPName();
            //    } else if (code == 8) //backspace=8
            //    {
            //        $scope.CPName = '';
            //        $scope.CPIDs = '';
            //    }
            //};

            //$scope.PickCPName = function () {
               
            //    var sUserID = '';
            //    if ($scope.UserIDs.length > 0) {
            //        sUserID = $scope.UserIDs;
            //    }
            //    debugger;
            //    var oBuyerConcern = {
            //        UserName: sUserID,
            //        Name: $.trim($scope.CPName)
            //    };

            //    var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
            //    $http.post(_sBaseAddress + '/UserPersonal/GetsByUsers', $.param(oBuyerConcern), config).then(
            //                        function (response) {
            //                            debugger;
            //                            var oColumns = [];
            //                            var oColumn = { field: 'ContactPersonnelID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
            //                            oColumn = { field: 'Name', name: 'Personnel Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
            //                            oColumn = { field: 'UserName', name: 'User Name', width: '30%', enableSorting: false }; oColumns.push(oColumn);
            //                            var results = jQuery.parseJSON(response.data);
            //                            var modalObj = {
            //                                size: 'md',
            //                                modalcontroller: 'ModalCommonListCtrl',
            //                                appcontroller: 'ImportInvoiceRequisitionController',
            //                                objs: results,
            //                                multiSelect: true,
            //                                pickertitle: 'Person List',
            //                                searchingbyfieldName: 'Name',
            //                                columns: oColumns
            //                            }

            //                            var modalInstance = msModal.Instance(modalObj);
            //                            modalInstance.result.then(function (result) {
            //                                debugger;
            //                                $scope.CPName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                                            
            //                                $scope.CPIDs = icsMethod.ICS_PropertyConcatation(result, 'ContactPersonnelID');
                                            
            //                            }, function () {
            //                                $log.info('Modal dismissed at: ' + new Date());
            //                            });
            //                        },
            //                          function (response) { alert(jQuery.parseJSON(response.data).Message); }
            //                    );
            //};
           
            $scope.RequisitionDateChange = function () {
                $scope.RequisitionDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboRequisitionDate);
            }
            
            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
             
                if ($scope.RequisitionNo == "" && $scope.RegistrationNo == "" && $scope.BinNo == "" && $scope.CustomerName == "" && $scope.cboRequisitionDate == 0 && $scope.cboIssueStore == 0 && $scope.cboReceiveStore == 0 && ($scope.UserIDs == undefined ? "" : $scope.UserIDs) == "" && $scope.cboRequisitionType == 0) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = false;
                    return false;
                }
                debugger;
                var sSearchingData = ($scope.RequisitionNo == undefined ? "" : $scope.RequisitionNo) + '~';
                sSearchingData = sSearchingData + $scope.cboRequisitionDate + '~';
                sSearchingData = sSearchingData + $scope.RequisitionDateStart + '~';
                sSearchingData = sSearchingData + $scope.RequisitionDateEnd + '~';
                sSearchingData = sSearchingData + 0 + '~'; //nCRAmount
                sSearchingData = sSearchingData + 0 + '~'; //sCRAmountStart
                sSearchingData = sSearchingData + 0 + '~'; //sCRAmountEnd
                sSearchingData = sSearchingData + 0 + '~'; //nRequsitionBy
                sSearchingData = sSearchingData + ($scope.cboIssueStore == undefined ? '' : $scope.cboIssueStore) + '~'; //sConsumptionUnitIDs
                sSearchingData = sSearchingData + "" + '~'; //sProductIDs
                sSearchingData = sSearchingData + ($scope.cboRequisitionType == undefined ? '' : $scope.cboRequisitionType) + '~';  //sTechnicalSheetIDs
                sSearchingData = sSearchingData + ($scope.RegistrationNo == undefined ? "" : $scope.RegistrationNo) + '~';
                sSearchingData = sSearchingData + ($scope.BinNo == undefined ? "" : $scope.BinNo) + '~';
                sSearchingData = sSearchingData + ($scope.CustomerName == undefined ? "" : $scope.CustomerName) + '~';

                var oPartsRequisition = {
                    Remarks: sSearchingData,
                    BUID: parseInt(sessionStorage.getItem("BUID"))
                };
                //var sTempString = $scope.cboRequisitionDate + '~' + $scope.RequisitionDateStart + '~' + $scope.RequisitionDateEnd + '~' + ($scope.cboIssueStore == undefined ? 0 : $scope.cboIssueStore) + '~' + ($scope.cboReceiveStore == undefined ? 0 : $scope.cboReceiveStore) + '~' + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive) + '~' + sessionStorage.getItem('BUID') + '~' + ($scope.cboRequisitionType == undefined ? 0 : $scope.cboRequisitionType) + '~' + ($scope.RequisitionNo == undefined ? "" : $scope.RequisitionNo);
                
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/PartsRequisition/AdvanceSearch', $.param(oPartsRequisition), config).then(
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
                                function (response) { alert(response.statusText); $scope.lblLoadingMessage = true; }
                        );
            };
            $scope.reset = function () {
                debugger;
                $scope.CompareOperators = obj.CompareOperators;
                $scope.IssueStores = obj.IssueStores;
                $scope.RequisitionUser = obj.RequisitionUser;
                $scope.RequisitionTypes = obj.RequisitionTypes;
                $scope.UserIDs = "";
                $scope.UserName = "";
                $scope.cboRequisitionDate = $scope.CompareOperators[0].id;
                $scope.RequisitionType = obj.RequisitionTypes[0].id;
                $scope.RequisitionDateEndDisabled = true;
                $scope.IssueStore = obj.IssueStores[0].WorkingUnitID;
                $scope.RequisitionDateStart = $scope.RequisitionDateEnd = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});