
function LoadExportBillParticularEvents() {

    debugger;

    $("#cboInOutType").icsLoadCombo({
        List: _oInOutTypes,
        OptionValue: "Value",
        DisplayText: "Text"
    });

    $("#btnSaveExportBillParticular").click(function (e) {

        if (!ValidateInputExportBillParticular()) return;
        var oExportBillParticular = RefreshObjectExportBillParticular();
        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportBillParticular,
                ObjectId: oExportBillParticular.ExportBillParticularID,
                ControllerName: "ExportBillParticular",
                ActionName: "Save",
                TableId: "tblExportBillParticulars",
                IsWinClose: true
            };
        $.icsSave(obj);
    });

    $("#btnCloseExportBillParticular").click(function () {
        $("#winExportBillParticular").icsWindow("close");
        $("#winExportBillParticular input").val("");
        $("#winExportBillParticular select").val(0);
    });
}

function RefreshObjectExportBillParticular() {

    var oExportBillParticular = {
        ExportBillParticularID: (_oExportBillParticular != null) ? _oExportBillParticular.ExportBillParticularID : 0,
        Name: $("#txtName").val(),
        InOutTypeInInt: $("#cboInOutType").val()
     
    };
    return oExportBillParticular;
}


function ValidateInputExportBillParticular() {

    if (!$.trim($('#txtName').val()).length) {
        alert("Please enter EParticular name."); $('#txtName').focus();
        $('#txtName').css("border", "1px solid #c00");
        return false;
    } else { $('#txtName').css("border", ""); }

  
    if ($('#cboInOutType').val() <= 0) {
        alert("Please select Add/Deduct type."); $('#cboInOutType').focus();
        $('#cboInOutType').css("border", "1px solid #c00");
        return false;
    } else { $('#cboInOutType').css("border", ""); }

 
    return true;

}

