var DUPservice = angular.module('DUP.service', ['ms.service', 'ngAnimate', 'ui.bootstrap']);
DUPservice.service('DUPservice', function ($uibModal) {

    //$(document).ready(function () {
    //    $("#progressbar").progressbar({ value: 0 });
    //    $("#progressbarParent").hide();
    //});
     
    this.Instance = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: 'md',
            templateUrl: _sBaseAddress + '/DUProductionYetTo/AdvDUProductionYetTo',
            controller: 'ModalDUProductionYetToCtrl',
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    debugger;
                    return {
                        //OrderType: (modalProperty.OrderType == undefined) ? [] : modalProperty.OrderType,
                        DyeingType: (modalProperty.DyeingType == undefined) ? [] : modalProperty.DyeingType,
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
                $("#progressbar").progressbar({ value: 0 });
                $("#progressbarParent").hide();
            });

          
            $scope.lblLoadingMessage = true;
            $scope.PickerTitle = "DU Production Status";
            $scope.SearchKeyDownBuyerName = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var BuyerName = $.trim($scope.BuyerName);
                    //if (BuyerName == "" || BuyerName == null) {
                    //    //alert("Type Buyer Name and Press Enter");
                    //    msModal.Message({ headerTitle: '', bodyText: 'Type Buyer Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    //    return;
                    //}
                    $scope.PickBuyer();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.ContractorName = '';
                    $scope.ContractorIDs = '';
                }
            };
        
            $scope.PickBuyer = function () {
                
                debugger;
               
                var oBuyerConcern = {
                    //ContractorName: $.trim($scope.ContractorName),
                    //BUID : sessionStorage.getItem("BUID")
                        
                    Params: '2' + '~' + $.trim($scope.ContractorName) + '~' + sessionStorage.getItem('BUID')
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Contractor/ContractorSearchByNameType', $.param(oBuyerConcern), config).then(
                    function (response) {
                        debugger;
                        var oColumns = [];
                        var oColumn = { field: 'ContractorID', name: 'ID', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                        oColumn = { field: 'Name', name: 'Buyer Name', width: '70%', enableSorting: false }; oColumns.push(oColumn);
                        var results = response.data;
                        var modalObj = {
                            size: 'md',
                            modalcontroller: 'ModalCommonListCtrl',
                            appcontroller: 'DUProductionYetToController',
                            objs: results,
                            multiSelect: true,
                            pickertitle: 'Buyer List',
                            searchingbyfieldName: 'Name',
                            columns: oColumns
                        }
                        var modalInstance = msModal.Instance(modalObj);
                        modalInstance.result.then(function (result) {
                            debugger;
                            $scope.ContractorName = result.length > 1 ? result.length + "Item's Selected" : result[0].Name;
                            $scope.ContractorIDs = icsMethod.ICS_PropertyConcatation(result, 'ContractorID');
                                            
                        }, function () {
                            $log.info('Modal dismissed at: ' + new Date());
                        });
                    },
                        function (response) { alert(jQuery.parseJSON(response.data).Message); }
                );
            };

            $scope.SearchKeyDownProductName = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var ProductName = $.trim($scope.ProductName);
                    if (ProductName == "" || ProductName == null) {
                        // alert("Type ContractorName Name and Press Enter");
                        msModal.Message({ headerTitle: '', bodyText: 'Type Product Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                        return;
                    }
                    $scope.PickProduct();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.ProductName = '';
                    $scope.ProductIDs = '';
                }
            }; 
            $scope.PickProduct = function () {
                debugger;
                var oContractor = {
                    ProductName: $.trim($scope.ProductName),
                    BUID : sessionStorage.getItem("BUID")
                };
                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/Product/SearchByProductBUWise', $.param(oContractor), config).then(
                                    function (response) {
                                        debugger;
                                        var oColumns = [];
                                        var oColumn = { field: 'ProductCode', name: 'Code', width: '30%', enableSorting: false }; oColumns.push(oColumn);
                                        oColumn = { field: 'ProductName', name: 'Product Name', width: '70%', enableSorting: false }; oColumns.push(oColumn);
                                        var results = response.data;
                                        var modalObj = {
                                            size: 'md',
                                            modalcontroller: 'ModalCommonListCtrl',
                                            appcontroller: 'DUProductionYetToController',
                                            objs: results,
                                            multiSelect: true,
                                            pickertitle: 'Product List',
                                            searchingbyfieldName: 'Name',
                                            columns: oColumns
                                        }
                                        var modalInstance = msModal.Instance(modalObj);
                                        modalInstance.result.then(function (result) {
                                            debugger;
                                            $scope.ProductName = result.length > 1 ? result.length + "Item's Selected" : result[0].ProductName;
                                            $scope.ProductIDs = icsMethod.ICS_PropertyConcatation(result, 'ProductID');
                                            
                                        }, function () {
                                            $log.info('Modal dismissed at: ' + new Date());
                                        });
                                    },
                                      function (response) { alert(jQuery.parseJSON(response.data).Message); }
                                );
            };



            //pick order type

            $scope.SearchKeyDownOrderType = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    var OrderTypeName = $.trim($scope.OrderTypeName);
                    //if (BuyerName == "" || BuyerName == null) {
                    //    //alert("Type Buyer Name and Press Enter");
                    //    msModal.Message({ headerTitle: '', bodyText: 'Type Buyer Name and Press Enter', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                    //    return;
                    //}
                    $scope.PickOrderType();
                } else if (code == 8) //backspace=8
                {
                    //debugger;
                    $scope.OrderTpyeName = '';
                    $scope.OrderTypeIDs = '';
                }
            };

            $scope.PickOrderType = function () {

                debugger;

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
                $http.post(_sBaseAddress + '/DUProductionYetTo/GetsOrderType', config).then(
                    function (response) {
                        debugger;
                        var oColumns = [];
                        var oColumn = { field: 'OrderName', name: 'Order Name', width: '80%', enableSorting: false }; oColumns.push(oColumn);
                        oColumn = { field: 'OrderType', name: 'Order ID', width: '15%', enableSorting: false }; oColumns.push(oColumn);
                        var results = jQuery.parseJSON(response.data);
                        var modalObj = {
                            size: 'md',
                            modalcontroller: 'ModalCommonListCtrl',
                            appcontroller: 'DUProductionYetToController',
                            objs: results,
                            multiSelect: true,
                            pickertitle: 'Order Type',
                            searchingbyfieldName: 'Name',
                            columns: oColumns
                        }
                        var modalInstance = msModal.Instance(modalObj);
                        modalInstance.result.then(function (result) {
                            debugger;
                            $scope.OrderTypeName = result.length > 1 ? result.length + "Item's Selected" : result[0].OrderName;
                            $scope.OrderTypeIDs = icsMethod.ICS_PropertyConcatation(result, 'OrderType');

                        }, function () {
                            $log.info('Modal dismissed at: ' + new Date());
                        });
                    },
                        function (response) { alert(jQuery.parseJSON(response.data).Message); }
                );
            };


            function updateProgress() {
                var value = $('#progressbar').progressbar('getValue');
                if (value < 90) {
                    value += Math.floor(Math.random() * 15);
                    $('#progressbar').progressbar('setValue', value);
                }
            }
            function hideShow(miliseconds) {
                $("#progressbarParent").hide();
            }


            
            $scope.search = function () {
                debugger;
                //$scope.lblLoadingMessage = false;
                //if () {
                //   //msModal.Message({ headerTitle: '', bodyText: 'All Fields are empty !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false });
                //    alert("Please Select at least one Criteria !!");
                //    $scope.lblLoadingMessage = false;
                //    return false;
                //}

                var sTempString = $scope.PINo + '~' + $scope.OrderNo + '~' + $scope.ProductIDs + '~' + $scope.ContractorIDs + '~' + $scope.OrderTypeIDs + '~' + $scope.OrderDateStart + '~' + $scope.OrderDateEnd + '~' + $scope.cboOrderDate + '~' + $scope.DyeingTypeID + '~' + ($scope.chkYetToProduction == undefined ? false : $scope.chkYetToProduction) + '~' + ($scope.chkYetToDelivery == undefined ? false : $scope.chkYetToDelivery);
               
              
                sessionStorage.setItem("sTempString", sTempString);

                var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };

                $("#progressbar").progressbar({ value: 0 });
                $("#progressbarParent").show();
                var intervalID = setInterval(updateProgress, 250);

                $http.get(_sBaseAddress + '/DUProductionYetTo/AdvSearchDUP', { params: { sTemp: sTempString } }, config).then(
                            function (response) {
                                $scope.lblLoadingMessage = true;
                                var results = jQuery.parseJSON(response.data);
                                if (results.Item1.length > 0) {
                                    $uibModalInstance.close(results);
                                    $("#progressbar").progressbar({ value: 0 });//hide
                                    $("#progressbarParent").hide();
                                } else {
                                    debugger;
                                    //msModal.Message({ headerTitle: '', bodyText: 'No Data Found !!', sucessText: ' Yes', cancelText: ' Close', feedbackType: false, autoClose: false, templateUrl: _sBaseAddress +'/Home/ModalMessage' });
                                    alert("Data Not Found.");
                                    return;
                                }

                            },
                                function (response) { alert(jQuery.parseJSON(response.data).Message); }
                        );
            };
            $scope.OrderDateChange = function () {
                $scope.OrderDateEndDisabled = icsMethod.IsDateBoxdisabled($scope.cboOrderDate);
            }
            $scope.reset = function () {
                debugger;
                $scope.CompareOperators = obj.CompareOperators;
                $scope.DyeingType = obj.DyeingType;
                $scope.cboOrderDate = $scope.CompareOperators[0].id;
                $scope.OrderType = obj.OrderType;
                $scope.OrderTypeIDs = "";
                $scope.ContractorIDs = "";
                $scope.ProductIDs = "";
                $scope.DyeingTypeID = 0;
                $scope.PINo = $scope.ProductName = $scope.ContractorName = $scope.OrderNo = $scope.OrderTypeName = "";
                $scope.OrderDateStart = $scope.OrderDateEnd = icsdateformat(new Date());
            }
            $scope.reset();
            $scope.closeAdvanceSearchModal = function () {
                $uibModalInstance.dismiss('cancel');
            };

        }
        return $uibModal.open(modalInstance);
    }


});