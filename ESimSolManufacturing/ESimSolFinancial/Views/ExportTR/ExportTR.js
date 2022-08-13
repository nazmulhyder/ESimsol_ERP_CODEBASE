
function LoadExportTREvents() {


    $("#btnSaveExportTR").click(function (e) {

        if (!ValidateInputExportTR()) return;
        var oExportTR = RefreshObjectExportTR();
        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportTR,
                ObjectId: oExportTR.ExportTRID,
                ControllerName: "ExportTR",
                ActionName: "Save",
                TableId: "tblExportTRs",
                IsWinClose: true
            };
        $.icsSave(obj);
    });

    $("#btnCloseExportTR").click(function () {
        $("#winExportTR").icsWindow("close");
        $("#winExportTR input").val("");
        $("#winExportTR select").val(0);
    });
}

function RefreshObjectExportTR() {

    var oExportTR = {
        ExportTRID: (_oExportTR != null) ? _oExportTR.ExportTRID : 0,
        TruckReceiptNo: $("#txtTruckReceiptNo").val(),
        TruckReceiptDate: $('#txtTruckReceiptDate').datebox('getValue'),
        BUID:_nBUID,
        Carrier: $("#txtCarrier").val(),
        TruckNo: $("#txtTruckNo").val(),
        DriverName: $("#txtDriverName").val()
     
    };
    return oExportTR;
}


function ValidateInputExportTR() {

    if (!$.trim($('#txtTruckReceiptNo').val()).length) {
        alert("Please enter ExportTR name."); $('#txtTruckReceiptNo').focus();
        $('#txtTruckReceiptNo').css("border", "1px solid #c00");
        return false;
    } else { $('#txtTruckReceiptNo').css("border", ""); }

  

    if (!$.trim($('#txtCarrier').val()).length) {
        alert("Please enter origin name."); $('#txtCarrier').focus();
        $('#txtCarrier').css("border", "1px solid #c00");
        return false;
    } else { $('#txtCarrier').css("border", ""); }

    if (!$.trim($('#txtTruckNo').val()).length) {
        alert("Please enter ExportTR address."); $('#txtTruckNo').focus();
        $('#txtTruckNo').css("border", "1px solid #c00");
        return false;
    } else { $('#txtTruckNo').css("border", ""); }

    if (!$.trim($('#txtDriverName').val()).length) {
        alert("Please enter ExportTR phone no."); $('#txtDriverName').focus();
        $('#txtDriverName').css("border", "1px solid #c00");
        return false;
    } else { $('#txtDriverName').css("border", ""); }

 
    return true;

}

