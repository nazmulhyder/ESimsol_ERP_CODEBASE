

function InitializeSUProductionPlanDetailsEvents() {



    $("#btnRefreshLotsSUPP").click(function () {
        
        var oLot = { ProductID: _oSUProductionPlan.ProductID };
        


        $.icsDataGets({
            BaseAddress: _sBaseAddress,
            Object: oLot,
            ControllerName: "SUProductionPlan",
            ActionName: "GetsLot",
            IsWinClose: false
        }, function (response) {
            if (response.status) {
                if (response.objs.length) {
                    DynamicRefreshList(response.objs, 'tblLotsSUPP');
                }
                else {
                    alert('No Data Found.');
                    DynamicRefreshList([], 'tblLotsSUPP');
                }
                RefreshTotalSummeryLots();
            }
        });
    });

    $("#btnRefreshExportPISUPP").click(function () {
        var oExportPIDetail = {
            ProductID: _oSUProductionPlan.ProductID
        };
        $.icsDataGets({
            BaseAddress: _sBaseAddress,
            Object: oExportPIDetail,
            ControllerName: "SUProductionPlan",
            ActionName: "GetsExportPI",
            IsWinClose: false
        }, function (response) {
            if (response.status) {
                if (response.objs.length) {
                    DynamicRefreshList(response.objs, 'tblExportPISUPP');
                }
                else {
                    alert('No Data Found.');
                    DynamicRefreshList([], 'tblExportPISUPP');
                }
                RefreshTotalSummeryExPI();
            }
        });
    });

    $("#btnRefreshAdvOrderSUPP").click(function () {
        var oSUDeliveryOrderDetail = { ProductID: _oSUProductionPlan.ProductID };
        $.icsDataGets({
            BaseAddress: _sBaseAddress,
            Object: oSUDeliveryOrderDetail,
            ControllerName: "SUProductionPlan",
            ActionName: "GetsAdvSUDO",
            IsWinClose: false
        }, function (response) {
            if (response.status) {
                if (response.objs.length) {
                    DynamicRefreshList(response.objs, 'tblAdvOrderSUPP');
                }
                else {
                    alert('No Data Found.');
                    DynamicRefreshList([], 'tblAdvOrderSUPP');
                }
                RefreshTotalSummeryAdvSUDO();
            }
        });
    });

    $("#btnRefreshRegularOrderSUPP").click(function () {
        var oSUDeliveryOrderDetail = { ProductID: _oSUProductionPlan.ProductID };
        $.icsDataGets({
            BaseAddress: _sBaseAddress,
            Object: oSUDeliveryOrderDetail,
            ControllerName: "SUProductionPlan",
            ActionName: "GetsRegularSUDO",
            IsWinClose: false
        }, function (response) {
            if (response.status) {
                if (response.objs.length) {
                    DynamicRefreshList(response.objs, 'tblRegularOrderSUPP');
                }
                else {
                    alert('No Data Found.');
                    DynamicRefreshList([], 'tblRegularOrderSUPP');
                }
                RefreshTotalSummeryRegularSUDO();
            }
        });
    });

    $("#btnCloseDetailsSUPP").click(function () {
        DynamicRefreshList([], 'tblLotsSUPP');
        RefreshTotalSummeryLots();
        DynamicRefreshList([], 'tblExportPISUPP');
        RefreshTotalSummeryExPI();
        DynamicRefreshList([], 'tblAdvOrderSUPP');
        RefreshTotalSummeryAdvSUDO();
        DynamicRefreshList([], 'tblRegularOrderSUPP');
        GenerateTableColumnsRegularOrderSUPP();

        $('.easyui-tabs').tabs('select', 0);

        $("#winDetailsSUPP").icsWindow('close');

    });

    //$("#btnPrintList").click(function () {
    //    
    //    var oSUPPs = $('#tblSUProductionPlans').datagrid('getRows');
    //    $('#txtCollectionPrintText').val(JSON.stringify(oSUPPs));
    //    if (!oSUPPs.length || !$.trim($('#txtCollectionPrintText').val()).length) return false;
    //});
    

    GenerateTableColumnsLotsSUPP();
    GenerateTableColumnsExportPISUPP();
    GenerateTableColumnsAdvOrderSUPP();
    GenerateTableColumnsRegularOrderSUPP();
    
    //
   
}
function RefreshTotalSummeryLots() {
    var nTotalQtyKg = 0;
    var nTotalQtyLbs = 0;
    
    var oLots = $('#tblLotsSUPP').datagrid('getRows');
    for (var i = 0; i < oLots.length; i++) {
        nTotalQtyKg += parseFloat(oLots[i].Balance);
        var nQtyLbs = GetLBS(parseFloat(oLots[i].Balance), 2);
        nTotalQtyLbs += parseFloat(nQtyLbs);
        
    }
    $("#lblLotsTotalKGQty").text(formatPrice(nTotalQtyKg));
    $("#lblLotsTotalLBSQty").text(formatPrice(nTotalQtyLbs));
    
}
function RefreshTotalSummeryExPI() {
    var nTotalQtyKg = 0;
    

    var oExPIs = $('#tblExportPISUPP').datagrid('getRows');
    for (var i = 0; i < oExPIs.length; i++) {
        nTotalQtyKg += parseFloat(_oSUProductionPlan.MeasurementUnitName === 'Pound' && _oSUProductionPlan.MeasurementUnitSymbol === 'LBS' ? oExPIs[i].TotalQtyKG : oExPIs[i].Qty);
        

    }
    $("#lblExPITotalKGQty").text(formatPrice(nTotalQtyKg));
    

}
function RefreshTotalSummeryAdvSUDO() {
    var nTotalQtyKg = 0;


    var oSUDOs = $('#tblAdvOrderSUPP').datagrid('getRows');
    for (var i = 0; i < oSUDOs.length; i++) {
        nTotalQtyKg += parseFloat(oSUDOs[i].Qty);


    }
    $("#lblAdvSUDOTotalKGQty").text(formatPrice(nTotalQtyKg));


}
function RefreshTotalSummeryRegularSUDO() {
    var nTotalQtyKg = 0;


    var oSUDOs = $('#tblRegularOrderSUPP').datagrid('getRows');
    for (var i = 0; i < oSUDOs.length; i++) {
        nTotalQtyKg += parseFloat(oSUDOs[i].Qty);


    }
    $("#lblRegularSudoTotalKGQty").text(formatPrice(nTotalQtyKg));


}

function GenerateTableColumnsLotsSUPP() {
    _otblColumns = [];

    var oColLotNo = { field: "LotNo", width: "20%", title: "Lot No", frozen: true };
    var oColOperationUnitName = { field: "OperationUnitName", width: "20%", title: "Store", frozen: true };
    var oColBalanceKGInString = { field: "BalanceKGInString", width: "25%", title: "Balance KG", align: 'right', frozen: true };
    var oColBalanceLBSInString = { field: "BalanceLBSInString", width: "25%", title: "Balance LBS", align: 'right', frozen: true };
    
    
    _otblColumns.push(oColLotNo, oColOperationUnitName, oColBalanceKGInString, oColBalanceLBSInString);

    $('#tblLotsSUPP').datagrid({ columns: [_otblColumns] });
    
}

function GenerateTableColumnsExportPISUPP() {
    _otblColumns = [];

    var oColPINo = { field: "PINo", width: "20%", title: "PI No", frozen: true };
    var oColPIStatusSt = { field: "PIStatusSt", width: "20%", title: "Status", frozen: true };
    var oColIssueDateInString = { field: "IssueDateInString", width: "15%", title: "Date", frozen: true };
    var oColBuyerName = { field: "BuyerName", width: "20%", title: "Buying House", frozen: true };
    var oColQtySt = { field: _oSUProductionPlan.MeasurementUnitName === 'Pound' && _oSUProductionPlan.MeasurementUnitSymbol === 'LBS' ? "TotalQtyKGSt" : "QtySt", width: "13%", title: "Qty", align: 'right', frozen: true };
    



    _otblColumns.push(oColPINo, oColPIStatusSt, oColIssueDateInString, oColBuyerName, oColQtySt);

    $('#tblExportPISUPP').datagrid({ columns: [_otblColumns] });
    
}

function GenerateTableColumnsAdvOrderSUPP() {
    _otblColumns = [];

    var oColDONo = { field: "DONo", width: "20%", title: "DONo", frozen: true };
    var oColDODateSt = { field: "DODateSt", width: "10%", title: "DO Date", frozen: true };
    var oColDOTypeSt = { field: "DOTypeSt", width: "20%", title: "Type", frozen: true };
    var oColBuyerName = { field: "BuyerName", width: "20%", title: "Buyer", frozen: true };
    var oColQtyInString = { field: "QtyInString", width: "20%", title: "Qty", align: 'right', frozen: true };
    

    _otblColumns.push(oColDONo, oColDODateSt, oColDOTypeSt, oColBuyerName, oColQtyInString);

    $('#tblAdvOrderSUPP').datagrid({ columns: [_otblColumns] });
    
}

function GenerateTableColumnsRegularOrderSUPP() {
    _otblColumns = [];

    var oColDONo = { field: "DONo", width: "18%", title: "DONo", frozen: true };
    var oColDODateSt = { field: "DODateSt", width: "11%", title: "DO Date", frozen: true };
    var oColDOTypeSt = { field: "DOTypeSt", width: "20%", title: "Type", frozen: true };
    var oColBuyerName = { field: "BuyerName", width: "25%", title: "Buyer", frozen: true };
    var oColQtyInString = { field: "QtyInString", width: "16%", title: "Qty", align: 'right', frozen: true };


    _otblColumns.push(oColDONo, oColDODateSt, oColDOTypeSt, oColBuyerName, oColQtyInString);

    $('#tblRegularOrderSUPP').datagrid({ columns: [_otblColumns] });
    
}









