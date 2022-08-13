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
        debugger;
        var oSUProductionPlan = { ErrorMessage: 'Arguments;' };
        


        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUProductionPlan/Gets",
            traditional: true,
            data: JSON.stringify(oSUProductionPlan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
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
        debugger;
        _oSUProductionPlan = {IsActive : true};
       ResetAllFields("winSUProductionPlan");
      
      
       RefreshControlSUProductionPlan(_oSUProductionPlan);
       $("#winSUProductionPlan").icsWindow("open", "Configure New Plan");
       RefreshLayoutSUProductionPlan("btnNewSUProductionPlan"); //button id as parameter
    });

    $("#btnEditSUProductionPlan").click(function () {
        debugger;
        var oSUProductionPlan = $("#tblSUProductionPlans").datagrid("getSelected");
        if (oSUProductionPlan == null || oSUProductionPlan.SUProductionPlanID <= 0) { alert("Please select an item from list!"); return; }
        
        _oSUProductionPlan = oSUProductionPlan;
        RefreshControlSUProductionPlan(_oSUProductionPlan);
        $("#winSUProductionPlan").icsWindow('open', "Edit Plan");
        RefreshLayoutSUProductionPlan("btnEditSUProductionPlan");
        
    });

    $("#btnViewSUProductionPlan").click(function () {
        debugger;
        var oSUProductionPlan = $("#tblSUProductionPlans").datagrid("getSelected");
        if (oSUProductionPlan == null || oSUProductionPlan.SUProductionPlanID <= 0) { alert("Please select an item from list!"); return; }

        _oSUProductionPlan = oSUProductionPlan;
        RefreshControlSUProductionPlan(_oSUProductionPlan);
        $("#winSUProductionPlan").icsWindow('open', "View Plan");
        RefreshLayoutSUProductionPlan("btnViewSUProductionPlan");
        
    });

    $("#btnActiveSUProductionPlan").click(function () {
        debugger;
        var oSUProductionPlan = $("#tblSUProductionPlans").datagrid("getSelected");
        if (oSUProductionPlan == null || oSUProductionPlan.SUProductionPlanID <= 0) { alert("Please select an item from list!"); return; }

        _oSUProductionPlan = oSUProductionPlan;
        var msg = _oSUProductionPlan.IsActive ? "Confirm to In-Active?" : "Confirm to Active?";
        if (!confirm(msg)) return false;
        _oSUProductionPlan.IsActive = !_oSUProductionPlan.IsActive;
        var defaultSettings = {
            BaseAddress: _sBaseAddress,
            Object: _oSUProductionPlan,
            ObjectId: _oSUProductionPlan.SUProductionPlanID,
            ControllerName: "SUProductionPlan",
            ActionName: "Save",
            TableId: "tblSUProductionPlans",
            IsWinClose: false
        };
        $.icsSave(defaultSettings, function (response) {

            if (response.status) {
                var oSelectedObject = $("#" + defaultSettings.TableId).datagrid("getSelected");
                var nRowIndex = $("#" + defaultSettings.TableId).datagrid("getRowIndex", oSelectedObject);
                RowSelect(nRowIndex, oSelectedObject);
                
            }
        });

    });

    $("#btnPrintList").click(function () {
        debugger;
        if (!_oDBSUProductionPlans.length || !$.trim($('#txtCollectionPrintText').val()).length) return false;
    });
    

    GenerateTableColumnsSUProductionPlan();
    
    //debugger;
    DynamicRefreshList(_oDBSUProductionPlans, 'tblSUProductionPlans');
        
    $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUProductionPlans));

    RefreshLayoutSUProductionPlan("load");
    
    
}
function RowSelect(rowIndex, rowData)
{
    debugger;
    if (rowData == null || rowData.SUProductionPlanID <= 0) { return; }

    _oSUProductionPlan = rowData;
    _oSUProductionPlan.IsActive ? $('#btnActiveSUProductionPlan').linkbutton({ text: 'In-Active', iconCls: 'icon-ok', plain: 'true' }) : $('#btnActiveSUProductionPlan').linkbutton({ text: 'Active', iconCls: 'icon-ok', plain: 'true' });
    
}

function GenerateTableColumnsSUProductionPlan() {
    _otblColumns = [];

    var oColProductCode = { field: "ProductCode", width: "10%", title: "Product Code", frozen: true };
    var oColProductShortName = { field: "ProductShortName", width: "15%", title: "Count/Product", frozen: true };
    var oColMachineQty = { field: "MachineQty", width: "10%", title: "No. of Machines", frozen: true };
    var oColCapacityPerDay = { field: "CapacityPerDayInString", width: "15%", title: "Capacity (Per Day)", frozen: true };
    var oColIsActiveInString = { field: "IsActiveInString", width: "5%", title: "Active", frozen: true };
    var oColRemarks = { field: "Remarks", width: "40%", title: "Remarks", frozen: true };
    


    _otblColumns.push(oColProductCode, oColProductShortName, oColMachineQty, oColCapacityPerDay, oColIsActiveInString, oColRemarks);

    $('#tblSUProductionPlans').datagrid({ columns: [_otblColumns] });
    $('#tblSUProductionPlans').datagrid({ onSelect: function (rowIndex, rowData) { RowSelect(rowIndex, rowData); } });
}









