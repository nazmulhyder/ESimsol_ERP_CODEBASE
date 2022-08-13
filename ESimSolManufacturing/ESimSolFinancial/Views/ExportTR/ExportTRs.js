

function LoadExportTRsEvents()
{
    DynamicRefreshList(_oExportTRs, "tblExportTRs");

    //$('#txtSearchByName').keyup(function (e){
    //    var obj =
    //            {
    //                Event: e,
    //                SearchProperty: "Name",
    //                GlobalObjectList: _oExportTRs,
    //                TableId: "tblExportTRs"
    //            };
    //    $('#txtSearchByName').icsSearchByText(obj);
    //});
    
    $("#btnAddExportTR").click(function () {
        $("#winExportTR").icsWindow('open',"Add ExportTR");
        $("#winExportTR input").val("");
        $("#winExportTR select").val(0);
        _oExportTR = null;
        RefreshExportTRLayout("btnAddExportTR");
    });

    $("#btnEditExportTR").click(function () {
        var oExportTR = $("#tblExportTRs").datagrid("getSelected");
        if (oExportTR == null || oExportTR.ExportTRID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportTR").icsWindow('open', "Edit ExportTR");
        RefreshExportTRLayout("btnEditExportTR");
        GetExportTRInformation(oExportTR);
    });

    $("#btnViewExportTR").click(function () {
        var oExportTR = $("#tblExportTRs").datagrid("getSelected");
        if (oExportTR == null || oExportTR.ExportTRID <= 0) {alert("Please select an item from list!");return; }
        $("#winExportTR").icsWindow('open',"View ExportTR");
        RefreshExportTRLayout("btnViewExportTR");
        GetExportTRInformation(oExportTR);
    });

    $("#btnDeleteExportTR").click(function () {

        var oExportTR = $("#tblExportTRs").datagrid("getSelected");
        if (!confirm("Confirm to Delete?")) return false;
        if (oExportTR == null || oExportTR.ExportTRID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportTR,
            ObjectId: oExportTR.ExportTRID,
            ControllerName: "ExportTR",
            ActionName: "Delete",
            TableId: "tblExportTRs",
            IsWinClose: true
        };
        $.icsDelete(obj);
    });

    $("#btnActiveInactive").click(function () {
        debugger;
        var oExportTR = $("#tblExportTRs").datagrid("getSelected");
        var SelectedRowIndex = $('#tblExportTRs').datagrid('getRowIndex', oExportTR);
        if (oExportTR == null || oExportTR.ExportTRID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (oExportTR.Activity == true) {
            if (!confirm("Confirm to In Active?")) return false;
        }
        else {
            if (!confirm("Confirm to Active?")) return false;
        }
      
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportTR,
            ObjectId: oExportTR.ExportTRID,
            ControllerName: "ExportTR",
            ActionName: "ActiveInactive",
            TableId: "tblExportTRs",
            IsWinClose: false
        };
        $.icsSave(obj);
       
        $('#tblExportTRs').datagrid('updateRow', { index: SelectedRowIndex, row: oExportTR });
    });

 
}

function GetExportTRInformation(oExportTR) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportTR,
        ControllerName: "ExportTR",
        ActionName: "Get",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        debugger;
        if (response.status && response.obj!=null) {
            if (response.obj.ExportTRID > 0) { RefreshExportTRControl(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else
        {

            alert("No information found.");
        }
    });
}

function RefreshExportTRLayout(buttonId) {
    if (buttonId === "btnViewExportTR") {
        $("#winExportTR input").prop("disabled", true);
        $("#btnSaveExportTR").hide();
    }
    else {
        $("#winExportTR input").prop("disabled", false);
        $("#btnSaveExportTR").show();
    }
    $(".disabled input").prop("disabled", true);
}

function RefreshExportTRControl(oExportTR)
{
    _oExportTR = oExportTR;
    $("#txtTruckReceiptNo").val(_oExportTR.TruckReceiptNo);
    $("#txtCarrier").val(_oExportTR.Carrier);
    $("#txtTruckNo").val(_oExportTR.TruckNo);
    $("#txtDriverName").val(_oExportTR.DriverName);
    $('#txtTruckReceiptDate').datebox('setValue', _oExportTR.TruckReceiptDate);
}
