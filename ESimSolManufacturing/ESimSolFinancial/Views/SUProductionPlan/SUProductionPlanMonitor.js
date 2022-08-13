function InitializeSUProductionPlansEvents() {

    $('#txtYarnCountSUProductionPlan').keyup(function (e) {
        var obj =
        {
            Event: e,
            SearchProperty: "ProductShortName",
            GlobalObjectList: _oDBSUProductionPlans,
            TableId: "tblSUProductionPlans"
        };
        $('#txtYarnCountSUProductionPlan').icsSearchByText(obj);
    });

    $("#btnRefreshSUProductionPlan").click(function () {
        
        var oSUProductionPlan = { ErrorMessage: 'Arguments;' };
        
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUProductionPlan/Gets",
            traditional: true,
            data: JSON.stringify(oSUProductionPlan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                
                if (data.length > 0) {
                    _oDBSUProductionPlans = [];
                    _oDBSUProductionPlans = jQuery.parseJSON(data);
                    DynamicRefreshList(_oDBSUProductionPlans, 'tblSUProductionPlans');

                    $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUProductionPlans));

                    RefreshLayoutSUProductionPlan("load");
                }
            }
        });
    });

    $("#btnNewSUProductionPlan").click(function () {
        _oSUProductionPlan = {IsActive : true};
       ResetAllFields("winSUProductionPlan");
       RefreshControlSUProductionPlan(_oSUProductionPlan);
       $("#winSUProductionPlan").icsWindow("open", "Configure New Plan");
       RefreshLayoutSUProductionPlan("btnNewSUProductionPlan"); //button id as parameter
       $("#txtProductSUProductionPlan").focus();
    });

    $("#btnEditSUProductionPlan").click(function () {
        var oSUProductionPlan = $("#tblSUProductionPlans").datagrid("getSelected");
        if (oSUProductionPlan == null || oSUProductionPlan.SUProductionPlanID <= 0) { alert("Please select an item from list!"); return; }
        _oSUProductionPlan = oSUProductionPlan;
        RefreshControlSUProductionPlan(_oSUProductionPlan);
        $("#winSUProductionPlan").icsWindow('open', "Edit Plan");
        RefreshLayoutSUProductionPlan("btnEditSUProductionPlan");
        $("#txtProductSUProductionPlan").focus();
    });

    $("#btnViewSUProductionPlan").click(function () {
        var oSUProductionPlan = $("#tblSUProductionPlans").datagrid("getSelected");
        if (oSUProductionPlan == null || oSUProductionPlan.SUProductionPlanID <= 0) { alert("Please select an item from list!"); return; }
        _oSUProductionPlan = oSUProductionPlan;
        RefreshControlSUProductionPlan(_oSUProductionPlan);
        $("#winSUProductionPlan").icsWindow('open', "View Plan");
        RefreshLayoutSUProductionPlan("btnViewSUProductionPlan");
        $("#btnCloseSUProductionPlan").focus();
    });

    $("#btnActiveSUProductionPlan").click(function () {
        var oSUProductionPlan = $("#tblSUProductionPlans").datagrid("getSelected");
        if (oSUProductionPlan == null || oSUProductionPlan.SUProductionPlanID <= 0) { alert("Please select an item from list!"); return; }
        var nRowIndex1 = $("#tblSUProductionPlans").datagrid("getRowIndex", oSUProductionPlan);
        _oSUProductionPlan = oSUProductionPlan;
        var msg = _oSUProductionPlan.IsActive ? "Confirm to In-Active?" : "Confirm to Active?";
        if (!confirm(msg)) return false;
        _oSUProductionPlan.IsActive = !_oSUProductionPlan.IsActive;
        if (msg == "Confirm to In-Active?")
        {
            _oSUProductionPlan.MachineQty = 0;
        }
        var defaultSettings = {
            BaseAddress: _sBaseAddress,
            Object: _oSUProductionPlan,
            ObjectId: _oSUProductionPlan.SUProductionPlanID,
            ControllerName: "SUProductionPlan",
            ActionName: "Save",
            TableId: "tblSUProductionPlans",
            IsWinClose: false,
            Message: (msg == "Confirm to In-Active?" ? "Inactive Successful." : "Active Successful.")
        };
        $.icsSave(defaultSettings, function (response) {
            if (response.status) {
                //Ratin
                debugger;
                var oSelectedObject = $("#tblSUProductionPlans").datagrid("getSelected");
                var nRowIndex = $("#tblSUProductionPlans").datagrid("getRowIndex", oSelectedObject);
                var oSUProductionPlan = response.obj;
                RowSelect(nRowIndex, oSUProductionPlan);

                $("#tblSUProductionPlans").datagrid("updateRow", { index: nRowIndex1, row: oSUProductionPlan });
                var oSUProductionPlans = $("#tblSUProductionPlans").datagrid("getRows");
                DynamicRefreshList([], "tblSUProductionPlans");
                DynamicRefreshList(oSUProductionPlans, "tblSUProductionPlans");
            }
        });

    });

    $("#btnInformationsSUProductionPlan").click(function () {
        var oSUProductionPlan = $("#tblSUProductionPlans").datagrid("getSelected");
        if (oSUProductionPlan == null || oSUProductionPlan.SUProductionPlanID <= 0) { alert("Please select an item from list!"); return; }
        _oSUProductionPlan = oSUProductionPlan;
        $("#winDetailsSUPP").icsWindow('open', "View Supporting Informations for " + oSUProductionPlan.ProductName);
        $("#btnRefreshLotsSUPP").focus();
    });

    $("#btnPrintList").click(function () {
        BtnPrintBtnClick(1);
        return false;
    });

    QtyFromatPopUpMenuLoad(1);
    $("#btnOkPrintFormat1").click(function () {
        $("#winPrintFormat1").icsWindow("close");
        var bPrintFormat = true;
        if ($("#chkInLBS1").is(':checked')) {
            bPrintFormat = false;
        }

        _oDBSUProductionPlans = $('#tblSUProductionPlans').datagrid('getRows');
        if (_oDBSUProductionPlans.length < 0)
        {
            alert("Sorry No List Found.");
            return false;
        }
      
        for (var i = 0; i < _oDBSUProductionPlans.length; i++) {
            _oDBSUProductionPlans[i].IsPrintInKg = bPrintFormat;
        }
        $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUProductionPlans));

    });

    //$("#btnPrintList").click(function () {
    //    
    //    var oSUPPs = $('#tblSUProductionPlans').datagrid('getRows');
    //    $('#txtCollectionPrintText').val(JSON.stringify(oSUPPs));
    //    if (!oSUPPs.length || !$.trim($('#txtCollectionPrintText').val()).length) return false;
    //});
    GenerateTableColumnsSUProductionPlan();
    
    DynamicRefreshList(_oDBSUProductionPlans, 'tblSUProductionPlans');
    RefreshLayoutSUProductionPlan("load");
}

function RowSelect(rowIndex, rowData)
{
    if (rowData == null || rowData.SUProductionPlanID <= 0) { return; }

    _oSUProductionPlan = rowData;
    _oSUProductionPlan.IsActive ? $('#btnActiveSUProductionPlan').linkbutton({ text: 'In-Active', iconCls: 'icon-Inactive', plain: 'true' }) : $('#btnActiveSUProductionPlan').linkbutton({ text: 'Active', iconCls: 'icon-active', plain: 'true' });
    
}

function GenerateTableColumnsSUProductionPlan() {
    _otblColumns = [];

    var oColMachineQtyInString = { field: "MachineQty", width: "6%", title: "M/C", align: 'right', frozen: true };
    var oColProductCode = { field: "ProductCode", width: "8%", title: "Product Code", frozen: true };
    var oColProductName = { field: "ProductName", width: "20%", title: "Count/Product", frozen: true };
    var oColReadyStockInHandInString = { field: "ReadyStockInHandInString", width: "10%", title: "Current Stock", align: 'right', frozen: true };
    var oColPIIssueQtyKGInString = { field: "PIIssueQtyKGInString", width: "10%", title: "PI Issue", align: 'right', frozen: true };
    var oColAdvancedDOQtyInString = { field: "AdvancedDOQtyInString", width: "10%", title: "Adv. Issue Qty", align: 'right', frozen: true };
    var oColRegularDOQtyInString = { field: "RegularDOQtyInString", width: "10%", title: "Order In Hand", align: 'right', frozen: true };
    var oColProductionQtyInString = { field: "ProductionQtyInString", width: "10%", title: "For Sale", align: 'right', frozen: true };
    var oColIsActive = { field: "IsActiveInString", width: "6%", title: "Is Active", frozen: true };
    var oColRemarks = { field: "Remarks", width: "16%", title: "Remarks", frozen: true };

    _otblColumns.push(oColMachineQtyInString, oColProductCode, oColProductName, oColReadyStockInHandInString, oColPIIssueQtyKGInString, oColAdvancedDOQtyInString, oColRegularDOQtyInString, oColProductionQtyInString,
                       oColIsActive,oColRemarks);

    $('#tblSUProductionPlans').datagrid({ columns: [_otblColumns] });
    $('#tblSUProductionPlans').datagrid({ onSelect: function (rowIndex, rowData) { RowSelect(rowIndex, rowData); } });
}









