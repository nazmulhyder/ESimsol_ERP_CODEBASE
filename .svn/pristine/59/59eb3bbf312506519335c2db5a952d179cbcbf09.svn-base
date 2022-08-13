
function LoadExportPIPrintSetupEvents() {

    $("#cboBU").icsLoadCombo({
        List: _oBusinessUnits,
        OptionValue: "BusinessUnitID",
        DisplayText: "BUTypeSt"
    });

    $("#btnSaveExportPIPrintSetup").click(function (e) {

        if (!ValidateInputExportPIPrintSetup()) return;
        var oExportPIPrintSetup = RefreshObjectExportPIPrintSetup();
        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportPIPrintSetup,
                ObjectId: oExportPIPrintSetup.ExportPIPrintSetupID,
                ControllerName: "ExportPIPrintSetup",
                ActionName: "Save",
                TableId: "tblExportPIPrintSetups",
                IsWinClose: true
            };
        $.icsSave(obj);
    });

    $("#btnCloseExportPIPrintSetup").click(function () {
        $("#winExportPIPrintSetup").icsWindow("close");
        $("#winExportPIPrintSetup input").val("");
        $("#winExportPIPrintSetup select").val(0);
    });
}

function RefreshObjectExportPIPrintSetup() {

    if (_nBUID <= 0 || _nBUID == null) {
        _nBUID = $("#cboBU").val();
    }
    var oExportPIPrintSetup = {
        ExportPIPrintSetupID: (_oExportPIPrintSetup != null) ? _oExportPIPrintSetup.ExportPIPrintSetupID : 0,
        SetupNo: $("#txtSetupNo").val(),
        Date: $('#txtDate').datebox('getValue'),
        Note: $("#txtSetupNote").val(),
        Preface: $("#txtPreface").val(),
        PartShipment: $("#txtPartShipment").val(),
        ShipmentBy: $("#txtShipmentBy").val(),
        PlaceOfShipment: $("#txtPlaceofShipment").val(),
        PlaceOfDelivery: $("#txtPlaceofDelivery").val(),
        IsPrintHeader: $("#chkIsPrintHeader").is(":checked"),
        AcceptanceBy: $("#txtAcceptanceBy").val(),
        BUID: _nBUID,
       
        For: $("#txtFor").val()
     
    };
    return oExportPIPrintSetup;
}


function ValidateInputExportPIPrintSetup()
{

    if (!$.trim($('#txtSetupNote').val()).length) {
        alert("Please enter ExportPIPrintSetup name."); $('#txtSetupNote').focus();
        $('#txtSetupNote').css("border", "1px solid #c00");
        return false;
    } else { $('#txtSetupNote').css("border", ""); }
    return true;
}

