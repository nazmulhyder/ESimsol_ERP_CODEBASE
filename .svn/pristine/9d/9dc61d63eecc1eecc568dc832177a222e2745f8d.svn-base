

function LoadExportBillParticularsEvents()
{
    DynamicRefreshList(_oExportBillParticulars, "tblExportBillParticulars");
        
    $("#btnAddExportBillParticular").click(function () {
        $("#winExportBillParticular").icsWindow('open',"Add ExportBillParticular");
        $("#winExportBillParticular input").val("");
        $("#winExportBillParticular select").val(0);
        _oExportBillParticular = null;
        RefreshExportBillParticularLayout("btnAddExportBillParticular");
    });


    $("#btnEditExportBillParticular").click(function () {
        var oExportBillParticular = $("#tblExportBillParticulars").datagrid("getSelected");
        if (oExportBillParticular == null || oExportBillParticular.ExportBillParticularID <= 0) { alert("Please select an item from list!"); return; }
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/ExportBillParticular/Get",
            traditional: true,
            data: JSON.stringify(oExportBillParticular),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oExportBillParticular = jQuery.parseJSON(data);
                if (oExportBillParticular.ExportBillParticularID > 0) {

                    RefreshExportBillParticularLayout("btnEditExportBillParticular");
                    RefreshExportBillParticularControl(oExportBillParticular);
                    $("#winExportBillParticular").icsWindow("open", "Edit ExportBillParticular");
                    
                }
            }
        });
    });

    $("#btnViewExportBillParticular").click(function () {
        var oExportBillParticular = $("#tblExportBillParticulars").datagrid("getSelected");
        if (oExportBillParticular == null || oExportBillParticular.ExportBillParticularID <= 0) {alert("Please select an item from list!");return; }
        $("#winExportBillParticular").icsWindow('open', "View ExportBillParticular");
        RefreshExportBillParticularLayout("btnViewExportBillParticular");
        GetExportBillParticularInformation(oExportBillParticular);
    });

    $("#btnDeleteExportBillParticular").click(function () {

        var oExportBillParticular = $("#tblExportBillParticulars").datagrid("getSelected");
        if (!confirm("Confirm to Delete?")) return false;
        if (oExportBillParticular == null || oExportBillParticular.ExportBillParticularID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBillParticular,
            ObjectId: oExportBillParticular.ExportBillParticularID,
            ControllerName: "ExportBillParticular",
            ActionName: "Delete",
            TableId: "tblExportBillParticulars",
            IsWinClose: true
        };
        $.icsDelete(obj);
    });

    $("#btnActiveInactive").click(function () {
        debugger;
   
        var oExportBillParticular = $("#tblExportBillParticulars").datagrid("getSelected");
        if (oExportBillParticular.Activaty == true) {
            if (!confirm("Confirm to In Active?")) return false;
        }
        else {
            if (!confirm("Confirm to Active?")) return false;
        }
        if (oExportBillParticular == null || oExportBillParticular.ExportBillParticularID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        Activaty(oExportBillParticular);
    });
 

    $("#btnActiveInactive").click(function () {
        debugger;
        var oExportBillParticular = $("#tblExportBillParticulars").datagrid("getSelected");
        if (oExportBillParticular.Activaty == true) {
            if (!confirm("Confirm to In Active?")) return false;
        }
        else {
            if (!confirm("Confirm to Active?")) return false;
        }
        if (oExportBillParticular == null || oExportBillParticular.ExportBillParticularID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportBillParticular,
            ObjectId: oExportBillParticular.ExportBillParticularID,
            ControllerName: "ExportBillParticular",
            ActionName: "ActiveInactive",
            TableId: "tblExportBillParticulars",
            IsWinClose: false
        };
        $.icsSave(obj);
        var SelectedRowIndex = $('#tblExportBillParticulars').datagrid('getRowIndex', oExportBillParticular);
        $('#tblExportBillParticulars').datagrid('updateRow', { index: SelectedRowIndex, row: oExportBillParticular });
    });

}

function Activaty(oExportBillParticular) {
    debugger;
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillParticular,
        ObjectId: oExportBillParticular.ExportBillParticularID,
        ControllerName: "ExportBillParticular",
        ActionName: "ActiveInactive",
        TableId: "tblExportBillParticulars",
        IsWinClose: false
    };
}
function GetExportBillParticularInformation(oExportBillParticular) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillParticular,
        ControllerName: "ExportBillParticular",
        ActionName: "Get",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        debugger;
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillParticularID > 0) { RefreshExportBillParticularControl(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function RefreshExportBillParticularLayout(buttonId) {
    if (buttonId === "btnViewExportBillParticular") {
        $("#winExportBillParticular input").prop("disabled", true);
        $("#btnSaveExportBillParticular").hide();
    }
    else {
        $("#winExportBillParticular input").prop("disabled", false);
        $("#btnSaveExportBillParticular").show();
    }
    $(".disabled input").prop("disabled", true);
}

function RefreshExportBillParticularControl(oExportBillParticular)
{
    _oExportBillParticular = oExportBillParticular;
    $("#txtName").val(_oExportBillParticular.Name);
    $("#cboInOutType").val(_oExportBillParticular.InOutTypeInInt);
}
