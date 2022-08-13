var _oSUDeliveryProgram = {
    SUDeliveryProgramID: 0,
    ProgramDate: '',
    LotID: 0,
    SUVehicleID: 0,
    Remarks: '',
    ProgramQty : 0
};


function CheckProgramQty()
{
    var nYetToProgram = parseFloat($("#txtYetToProgramSUDeliveryProgram").val());
    var nProgramQty = parseFloat($("#txtProgramQtySUDeliveryProgram").val());

    if (nYetToProgram < nProgramQty)
    {
        alert("Program Qty cannot be greater than Yet To Program Quantity.");
        return false;
    }

    return true;
}

function InitializeSUDeliveryProgramEvents() {
    $("#btnSaveSUDeliveryProgram").click(function () {
        if (!ValidateInputSUDeliveryProgram()) return false;
        //if (!CheckProgramQty()) return false;
        if (!confirm("Confirm to Update Program?")) return false;
        RefreshObjectSUDeliveryProgram();
        
        _oSUDeliveryProgram.ProgramQty = parseFloat($("#txtProgramQtySUDeliveryProgram").val());

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryProgram/UpdateInfo",
            traditional: true,
            data: JSON.stringify(_oSUDeliveryProgram),
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                var oSUDProgram = jQuery.parseJSON(data);

                debugger;
                

                if (parseInt(oSUDProgram.SUDeliveryProgramID) > 0) {
                    alert("Program Updated Successfully!");
                    var oSUDeliveryProgram = $('#tblSUDPs').datagrid("getSelected");
                    var nRowIndex = $('#tblSUDPs').datagrid("getRowIndex", oSUDeliveryProgram);
                    $('#tblSUDPs').datagrid("updateRow", { index: nRowIndex, row: oSUDProgram });


                    $('#txtProgramDateSUDeliveryProgram').datebox('setValue', icsdateformat(new Date()));
                    $("#cboLotSUDeliveryProgram").icsLoadCombo({
                        List: [],
                        OptionValue: "LotID",
                        DisplayText: "LotNo"
                    });
                    $("#cboVehicleSUDeliveryProgram").icsLoadCombo({
                        List: [],
                        OptionValue: "SUVehicleID",
                        DisplayText: "VehicleNo"
                    });
                    $("#txtRemarksSUDeliveryProgram").val('');
                    $("#winSUDeliveryProgram").icsWindow('close');
                }
                else {
                    alert(oSUDProgram.ErrorMessage);
                }

            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    
    });
    $("#btnCloseSUDeliveryProgram").click(function () {
        $('#txtProgramDateSUDeliveryProgram').datebox('setValue', icsdateformat(new Date()));
        $("#cboLotSUDeliveryProgram").icsLoadCombo({
            List: [],
            OptionValue: "LotID",
            DisplayText: "LotNo"
        });
        $("#cboVehicleSUDeliveryProgram").icsLoadCombo({
            List: [],
            OptionValue: "SUVehicleID",
            DisplayText: "VehicleNo"
        });
        $("#txtRemarksSUDeliveryProgram").val('');
        $("#winSUDeliveryProgram").icsWindow('close');
    });


    
    
    $('#txtProgramDateSUDeliveryProgram').datebox('setValue', icsdateformat(new Date()));
}




function RefreshControlSUDeliveryProgram(oSUDeliveryProgram) {
    
    _oSUDeliveryProgram = oSUDeliveryProgram;

    $('#txtProgramDateSUDeliveryProgram').datebox('setValue', _oSUDeliveryProgram.ProgramDateInString);

    $("#cboLotSUDeliveryProgram").icsLoadCombo({
        List: _oSUDeliveryProgram.Lots,
        OptionValue: "LotID",
        DisplayText: "LotNoWithOUName"
    });
    $("#cboVehicleSUDeliveryProgram").icsLoadCombo({
        List: _oSUDeliveryProgram.SUVehicles,
        OptionValue: "SUVehicleID",
        DisplayText: "VehicleNo"
    });
   
    $("#cboLotSUDeliveryProgram").val(_oSUDeliveryProgram.LotID);
    $("#cboVehicleSUDeliveryProgram").val(_oSUDeliveryProgram.SUVehicleID);
    $("#txtRemarksSUDeliveryProgram").val(_oSUDeliveryProgram.Remarks);

    $("#txtYetToProgramSUDeliveryProgram").val(_oSUDeliveryProgram.YetToProgramOfCount);
    $("#txtProgramQtySUDeliveryProgram").val(_oSUDeliveryProgram.ProgramQty);
    $("#txtChallanQtySUDeliveryProgram").val(_oSUDeliveryProgram.ChallanQty);
}
function RefreshLayoutSUDeliveryProgram(buttonId) {
    if (buttonId === "btnViewSUDeliveryPrograms") {
        $("#winSUDeliveryProgram input").prop("disabled", true);
        $("#winSUDeliveryProgram select").prop("disabled", true);
        $("#btnSaveSUDeliveryProgram").hide();
    }
    else {
        $("#winSUDeliveryProgram input").prop("disabled", false);
        $("#winSUDeliveryProgram select").prop("disabled", false);
        $("#btnSaveSUDeliveryProgram").show();
    }

    $("#txtYetToProgramSUDeliveryProgram,#txtChallanQtySUDeliveryProgram").prop("disabled", true);
}

function RefreshObjectSUDeliveryProgram() {
    
    _oSUDeliveryProgram.ProgramDate = $.trim($('#txtProgramDateSUDeliveryProgram').datebox('getValue'));
    _oSUDeliveryProgram.LotID = $.trim($('#cboLotSUDeliveryProgram').val());
    _oSUDeliveryProgram.SUVehicleID = $.trim($('#cboVehicleSUDeliveryProgram').val());
    _oSUDeliveryProgram.Remarks = $.trim($('#txtRemarksSUDeliveryProgram').val());
}

function ValidateInputSUDeliveryProgram() {

    if (icsdateparser($.trim($('#txtProgramDateSUDeliveryProgram').datebox('getValue'))) < icsdateparser(icsdateformat(new Date()))) {
        alert("Can not Update Program Date to Past Date. \n Please Change Date!");
        $('#txtProgramDateSUDeliveryProgram').datebox('setValue', icsdateformat(new Date()));
        $('#txtProgramDateSUDeliveryProgram').focus();
        $('#txtProgramDateSUDeliveryProgram').css("border", "1px solid #c00");
        return false;
    }
    else {
        $('#txtProgramDateSUDeliveryProgram').css("border", "");
    }

    return true;
}


