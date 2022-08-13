var IICService = angular.module('DUSoftWinding.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
IICService.service('DUSoftWindingservice', function ($uibModal) {

    _sBaseAddress = sessionStorage.getItem('BaseAddress');

    this.Instance = function (modalProperty) {
        debugger;
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/DUSoftWinding/AdvSearchDUSoftWinding',
            controller: 'ModalDUSoftWindingAdvanceSearchCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        CompareOperators: (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators
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

            $scope.search = function () {
                debugger;
                $scope.lblLoadingMessage = false;
             
                if ($scope.OrderNo == "" && $scope.Contractor.BuyerIDs=="" && $scope.LotNo == "" && $scope.cboOrderDate == 0 && $scope.cboReceiveDate == 0 && $scope.YetToApprove == false && $scope.YetToIssue == false && $scope.YetToReceive == false) {
                    msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    alert("Please Select At Least One Searching Criteria!!");
                    $scope.lblLoadingMessage = true;
                    return false;
                }

                var sTempString = $scope.cboOrderDate + '~' + $scope.OrderDateStart + '~' + $scope.OrderDateEnd + '~'
                                + $scope.cboReceiveDate + '~' + $scope.ReceiveDateStart + '~' + $scope.ReceiveDateEnd + '~'
                                + ($scope.OrderNo == undefined ? "" : $scope.OrderNo) + '~' + ($scope.LotNo == undefined ? "" : $scope.LotNo) + '~'
                                + ($scope.YetToApprove == undefined ? false : $scope.YetToApprove) + '~' + ($scope.YetToIssue == undefined ? false : $scope.YetToIssue) + '~' + ($scope.YetToReceive == undefined ? false : $scope.YetToReceive)
                                + '~' + sessionStorage.getItem('BUID')
                                + '~' + $scope.Contractor.BuyerIDs;
                
                sessionStorage.setItem("sTempString", sTempString);
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.get(_sBaseAddress + '/DUSoftWinding/AdvSearch', { params: { sTemp: sTempString } }, config).then(
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
                $scope.ReceiveDateStart = $scope.ReceiveDateEnd = icsdateformat(new Date());
                $scope.OrderNo = "";
                $scope.LotNo = "";
                $scope.YetToApprove = $scope.YetToIssue = $scope.YetToReceive = false;
                $scope.Contractor = {BuyerIDs:''};
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});