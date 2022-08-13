
function InitializeSUVehicleEvents() {
    
    

    $("#btnSaveSUVehicle").click(function () {
        debugger;
        if (!ValidateInputSUVehicle()) return;
        var oSUVehicle = RefreshObjectSUVehicle();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSUVehicle,
            ObjectId: oSUVehicle.SUVehicleID,
            ControllerName: "SUVehicle",
            ActionName: "Save",
            TableId: "tblSUVehicles",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if(response.status)
            {
                _oDBSUVehicles = [];
                _oDBSUVehicles = $("#tblSUVehicles").datagrid('getRows');
                $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUVehicles));
            }
        });
    
    });
    $("#btnCloseSUVehicle").click(function () {
        $("#txtVehicleNoSUVehicle").val('');
        $("#txtDriverNameSUVehicle").val('');
        $("#txtDriverContactNoSUVehicle").val('');
        $("#txtRemarksSUVehicle").val('');
        $("#winSUVehicle").icsWindow('close');
    });


    
    
    
}




function RefreshControlSUVehicle(oSUVehicle) {
    debugger;
    $('#txtVehicleNoSUVehicle').val(oSUVehicle.VehicleNo);
    $("#txtDriverNameSUVehicle").val(oSUVehicle.DriverName);
    $("#txtDriverContactNoSUVehicle").val(oSUVehicle.DriverContactNo);
    $("#txtRemarks").val(oSUVehicle.Remarks);
}
function RefreshLayoutSUVehicle(buttonId) {
    if (buttonId === "btnViewSUVehicles") {
        $("#winSUVehicle input").prop("disabled", true);
        $("#winSUVehicle select").prop("disabled", true);
        $("#btnSaveSUVehicle").hide();
    }
    else {
        $("#winSUVehicle input").prop("disabled", false);
        $("#winSUVehicle select").prop("disabled", false);
        $("#btnSaveSUVehicle").show();
    }
}

function RefreshObjectSUVehicle() {
    debugger;
    var nSUVehicleId = (_oSUVehicle == null ? 0 : (_oSUVehicle.SUVehicleID == null) ? 0 : _oSUVehicle.SUVehicleID);
    var oSUVehicle = {
        SUVehicleID: nSUVehicleId,
        VehicleNo: $.trim($("#txtVehicleNoSUVehicle").val()),
        DriverName: $.trim($("#txtDriverNameSUVehicle").val()),
        DriverContactNo: $.trim($("#txtDriverContactNoSUVehicle").val()),
        Remarks: $.trim($("#txtRemarksSUVehicle").val())
    };
    return oSUVehicle;
}

function ValidateInputSUVehicle() {

    if (!$.trim($("#txtVehicleNoSUVehicle").val()).length) {
        alert("Please Enter Vehicle Number!");
        $('#txtVehicleNoSUVehicle').val("");
        $('#txtVehicleNoSUVehicle').focus();
        $('#txtVehicleNoSUVehicle').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtVehicleNoSUVehicle').css("border", "");
    }
    if (!$.trim($("#txtDriverNameSUVehicle").val()).length) {
        alert("Please Enter Driver Name!");
        $('#txtDriverNameSUVehicle').val("");
        $('#txtDriverNameSUVehicle').focus();
        $('#txtDriverNameSUVehicle').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtDriverNameSUVehicle').css("border", "");
    }
    if (!$.trim($("#txtDriverContactNoSUVehicle").val()).length) {
        alert("Please Enter Driver Contact Number!");
        $('#txtDriverContactNoSUVehicle').val("");
        $('#txtDriverContactNoSUVehicle').focus();
        $('#txtDriverContactNoSUVehicle').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtDriverContactNoSUVehicle').css("border", "");
    }

    return true;
}


