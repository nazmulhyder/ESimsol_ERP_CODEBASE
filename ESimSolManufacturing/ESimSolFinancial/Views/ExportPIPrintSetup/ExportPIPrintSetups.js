

function LoadExportPIPrintSetupsEvents()
{
    DynamicRefreshList(_oExportPIPrintSetups, "tblExportPIPrintSetups");

    
    $("#btnAddExportPIPrintSetup").click(function () {
        $("#winExportPIPrintSetup").icsWindow('open',"Add ExportPIPrintSetup");
        $("#winExportPIPrintSetup input").val("");
        $("#winExportPIPrintSetup select").val(0);
        _oExportPIPrintSetup = null;
        RefreshExportPIPrintSetupLayout("btnAddExportPIPrintSetup");
    });

    $("#btnEditExportPIPrintSetup").click(function () {
        var oExportPIPrintSetup = $("#tblExportPIPrintSetups").datagrid("getSelected");
        if (oExportPIPrintSetup == null || oExportPIPrintSetup.ExportPIPrintSetupID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportPIPrintSetup").icsWindow('open', "Edit ExportPIPrintSetup");
        RefreshExportPIPrintSetupLayout("btnEditExportPIPrintSetup");
        GetExportPIPrintSetupInformation(oExportPIPrintSetup);
    });

    $("#btnViewExportPIPrintSetup").click(function () {
        var oExportPIPrintSetup = $("#tblExportPIPrintSetups").datagrid("getSelected");
        if (oExportPIPrintSetup == null || oExportPIPrintSetup.ExportPIPrintSetupID <= 0) {alert("Please select an item from list!");return; }
        $("#winExportPIPrintSetup").icsWindow('open',"View ExportPIPrintSetup");
        RefreshExportPIPrintSetupLayout("btnViewExportPIPrintSetup");
        GetExportPIPrintSetupInformation(oExportPIPrintSetup);
    });

    $("#btnDeleteExportPIPrintSetup").click(function () {
        var oExportPIPrintSetup = $("#tblExportPIPrintSetups").datagrid("getSelected");
        if (oExportPIPrintSetup == null || oExportPIPrintSetup.ExportPIPrintSetupID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportPIPrintSetup,
            ObjectId: oExportPIPrintSetup.ExportPIPrintSetupID,
            ControllerName: "ExportPIPrintSetup",
            ActionName: "Delete",
            TableId: "tblExportPIPrintSetups",
            IsWinClose: true
        };
        $.icsDelete(obj);
    });

    $("#btnActivity").click(function () {
        debugger;
        var oExportPIPrintSetup = $("#tblExportPIPrintSetups").datagrid("getSelected");
        var SelectedRowIndex = $('#tblExportPIPrintSetups').datagrid('getRowIndex', oExportPIPrintSetup);
        if (oExportPIPrintSetup == null || oExportPIPrintSetup.ExportPIPrintSetupID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (oExportPIPrintSetup.Activity == true) {
            if (!confirm("Confirm to In Active?")) return false;
        }
        else {
            if (!confirm("Confirm to Active?")) return false;
        }
      
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportPIPrintSetup,
            ObjectId: oExportPIPrintSetup.ExportPIPrintSetupID,
            ControllerName: "ExportPIPrintSetup",
            ActionName: "ActiveInactive",
            TableId: "tblExportPIPrintSetups",
            IsWinClose: false
        };
        $.icsSave(obj);
       
        $('#tblExportPIPrintSetups').datagrid('updateRow', { index: SelectedRowIndex, row: oExportPIPrintSetup });
    });

 
}

function GetExportPIPrintSetupInformation(oExportPIPrintSetup) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportPIPrintSetup,
        ControllerName: "ExportPIPrintSetup",
        ActionName: "Get",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        debugger;
        if (response.status && response.obj!=null) {
            if (response.obj.ExportPIPrintSetupID > 0) { RefreshExportPIPrintSetupControl(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else
        {

            alert("No information found.");
        }
    });
}

function RefreshExportPIPrintSetupLayout(buttonId) {
    debugger;
    if (buttonId === "btnViewExportPIPrintSetup") {
        $("#winExportPIPrintSetup input").prop("disabled", true);
        $("#btnSaveExportPIPrintSetup").hide();
    }
    else {
        $("#winExportPIPrintSetup input").prop("disabled", false);
        $("#btnSaveExportPIPrintSetup").show();
    }
    $(".disabled input").prop("disabled", true);
}

function RefreshExportPIPrintSetupControl(oExportPIPrintSetup)
{
    _oExportPIPrintSetup = oExportPIPrintSetup;
    $("#txtSetupNo").val(_oExportPIPrintSetup.SetupNo);
    $('#txtDate').datebox('setValue', _oExportPIPrintSetup.DateInString);
    $("#txtSetupNote").val(_oExportPIPrintSetup.Note);
    $("#txtPreface").val(_oExportPIPrintSetup.Preface);
    $("#txtPartShipment").val(_oExportPIPrintSetup.PartShipment);
    $("#txtShipmentBy").val(_oExportPIPrintSetup.ShipmentBy);
    $("#txtPlaceofShipment").val(_oExportPIPrintSetup.PlaceOfShipment);
    $("#txtPlaceofDelivery").val(_oExportPIPrintSetup.PlaceOfDelivery);
    $("#chkIsPrintHeader").prop("checked", _oExportPIPrintSetup.IsPrintHeader);
    $("#cboBU").val(_oExportPIPrintSetup.BUID);
    $("#txtFor").val(_oExportPIPrintSetup.For);
    $("#txtAcceptanceBy").val(_oExportPIPrintSetup.AcceptanceBy);
}

