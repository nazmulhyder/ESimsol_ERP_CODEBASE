var IICService = angular.module('DUSoftWindingOpen.service', ['ms.service', 'ngAnimate', 'ui.bootstrap' ]);
IICService.service('DUSoftWindingOpenservice', function ($uibModal) {

    _sBaseAddress = sessionStorage.getItem('BaseAddress');


    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'lg',
            templateUrl: _sBaseAddress + '/DUSoftWinding/AdvSearchDUSoftWinding_Open',
            controller: 'ModalDUSoftWindingAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators,
                        RouteSheetSetup: (modalProperty.RouteSheetSetup == undefined) ? null : modalProperty.RouteSheetSetup
                    };
                }

            }
        }


        modalInstance.controller = function ($http, $scope, $log, $uibModalInstance, uiGridConstants, msModal, icsMethod, obj) {
            debugger;
            $(document).on('mousemove', '.modal-body', function () {
                $('.date-time-container').datetimepicker({
                    format: "DD MMM YYYY HH:mm A" //'LT'
                });
                $('.date-container').datepicker({
                    format: "dd M yyyy",
                    calendarWeeks: true,
                    autoclose: true,
                    todayHighlight: true
                });
            });

            
            $scope.lblLoadingMessage = true;
            $scope.PickerTitle = "Search Requisition";

            $scope.OrderDateChange = function () {
                $scope.OrderDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboOrderDate);
            }
            $scope.ReceiveDateChange = function () {
                $scope.ReceiveDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboReceiveDate);
            }


            //customer
            $scope.PickBuyer = function () {
                var oContractor = {
                    Params: '2' + '~' + $.trim($scope.Contractor.BuyerName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Contractor/ContractorSearchByNameType', $.param(oContractor), config).then(
                                    function (response) {
                                        var oColumns = [];
                                        var oColumn = { field: 'ContractorID', name: 'Code', width: '20%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'Name', name: 'Applicant Name', width: '50%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'Address', name: 'Address', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = response.data;
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'ContractorController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Buyer List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            if (result.length > 0)
                                            {
                                                $scope.Contractor.BuyerName = result[0].Name;
                                                $scope.Contractor.BuyerIDs = result[0].ContractorID;
                                                for (var i = 1; i < result.length; i++)
                                                {
                                                    $scope.Contractor.BuyerIDs += ","+result[i].ContractorID;
                                                }
                                                if (result.length > 1) $scope.Contractor.BuyerName = result.length + " Items are selected.";
                                            }
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };
            $scope.SearchKeyDownBuyer = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var RefNo = $.trim($scope.Contractor.BuyerName);
                    if (RefNo == "" || RefNo == null) {
                        alert("Type Buyer and Press Enter");
                        return;
                    }
                    $scope.PickBuyer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.Contractor.BuyerIDs = "";
                }
            };
            $scope.ClearBuyer = function () {
                $scope.Contractor.BuyerIDs = "";
                $scope.Contractor.BuyerName = '';
            }

            //Yarn
            $scope.PickYarn = function () {
                debugger;
                var oProduct = {
                    BUID:sessionStorage.getItem('BUID'),
                    ProductName: $.trim($scope.YarnName)
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Product/SearchByProductBUWise', $.param(oProduct), config).then(
                                    function (response) {
                                        var oColumns = [];
                                        var oColumn = { field: 'ProductName', name: 'Name', width: '50%' }; oColumns.push(oColumn);
                                        oColumn = { field: 'ProductCode', name: 'Code', width: '40%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = response.data;
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'ProductController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Yarn List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            if (result.length > 0) {
                                                $scope.YarnName = result[0].ProductName;
                                                $scope.YarnIDs = result[0].ProductID;
                                                for (var i = 1; i < result.length; i++) {
                                                    $scope.YarnIDs += "," + result[i].ProductID;
                                                }
                                                if (result.length > 1) $scope.YarnName = result.length + " Items are selected.";
                                            }
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };
            $scope.SearchKeyDownYarn = function (e) {
                //debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var RefNo = $.trim($scope.YarnName);
                    if (RefNo == "" || RefNo == null) {
                        alert("Type Yarn and Press Enter");
                        return;
                    }
                    $scope.PickYarn();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.YarnIDs = "";
                }
            };
            $scope.ClearYarn = function () {
                $scope.YarnIDs = "";
                $scope.YarnName = '';
            }

            $scope.search = function () {
                
                $scope.lblLoadingMessage = false;
             
                if ($scope.LotNo == "" && $scope.cboReceiveDate == 0 && $scope.YetToApprove == false && $scope.YetToIssue == false && $scope.YetToReceive == false && $scope.YarnIDs == "") {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = true;
                    return false;
                }
                debugger;
                var dStartDate = new Date($scope.ReceiveDateStart);
                dStartDate.setHours($("#cboHourStart").val());
                dStartDate.setMinutes($("#cboMinStart").val());
                $scope.ReceiveDateStart = icsdatetimeformat(dStartDate);

                var dEndDate = new Date($scope.ReceiveDateEnd);
                dEndDate.setHours($("#cboHourEnd").val());
                dEndDate.setMinutes($("#cboMinEnd").val());
                $scope.ReceiveDateEnd = icsdatetimeformat(dEndDate);
                
                var sTempString = $scope.cboReceiveDate + '~' + $scope.ReceiveDateStart + '~' + $scope.ReceiveDateEnd + '~'
                                + ($scope.LotNo == undefined ? "" : $scope.LotNo) + '~'
                                + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive)
                                + '~' + sessionStorage.getItem('BUID')
                                + '~' + 1
                                + '~' + $scope.YarnIDs;
                
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(_sBaseAddress + '/DUSoftWinding/AdvSearch_Open', { params: { sTemp: sTempString } }, config).then(
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
                $scope.cboOrderDate = $scope.CompareOperators[0].id;
                $scope.OrderDateEndDisabled = true;
                $scope.OrderDateStart = $scope.OrderDateEnd = icsdateformat(new Date());
                $scope.cboReceiveDate = $scope.CompareOperators[0].id;
                $scope.ReceiveDateEndDisabled = true;
                $scope.ReceiveDateStart = $scope.ReceiveDateEnd = icsdateformat(new Date());//icsdatetimeformat
                $scope.OrderNo = "";
                $scope.LotNo = "";
                $scope.YetToApprove = $scope.YetToIssue = $scope.YetToReceive = false;
                $scope.Contractor = { BuyerIDs: '' };
                $scope.YarnIDs = "";
                $scope.YarnName = '';
                //var d = new Date($scope.RouteSheetSetup.BatchTimeSt);
                debugger;
                //var hh = parseInt(obj.RouteSheetSetup.BatchTimeSt.split(" : ")[0]);
                //var mm = obj.RouteSheetSetup.BatchTimeSt.split(" : ")[1];
                ////$scope.cboHourStart = hh;
                //$("#cboHourStart").val(hh);
                //$("#cboMinStart").val(mm);
                //$("#cboHourEnd").val(hh);
                //$("#cboMinEnd").val(mm);
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});