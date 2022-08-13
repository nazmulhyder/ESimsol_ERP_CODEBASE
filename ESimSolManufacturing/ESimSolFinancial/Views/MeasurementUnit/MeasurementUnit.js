function LoadMeasurementUnitEvents() {

    $("#cboUnitType").icsLoadCombo({
        List: _oUniteTypes,
        OptionValue: "Value",
        DisplayText: "Text"
    });
    

    //MUnit

    $("#btnSaveMUnit").click(function () {
        if (!ValidateInputMUnit()) return;
        var oMUnit = RefreshObjectMUnit();

        $("#winMeasurementUnit").icsSave({
            BaseAddress: _sBaseAddress,
            Object: oMUnit,
            ObjectId: oMUnit.MeasurementUnitID,
            ControllerName: "MeasurementUnit",
            ActionName: "Save",
            TableId: "tblMeasurementUnits"
        });
    });

    $("#btnCloseMUnit").click(function () {
        $("#winMeasurementUnit").window("close");
        $(".wininputfieldstyle input").val("");
        $("#radioBtnIsRoundNo").prop("checked", true);
        $("#radioBtnIsRoundYes").prop("checked", false);
        _bIsRound = false;
    });

    $("#btnAdd1").click(function () {
        debugger;
        $("#winTest").icsWindow({ Property: 'open' });
    });

    
}


function RefreshMUnitControl(oMUnit) {
    _oMUnit = oMUnit;
    $('#txtUnitName').val(oMUnit.UnitName)
    $('#cboUnitType').val(oMUnit.UnitType);
    $('#txtSymbol').val(oMUnit.Symbol);
    $('#txtNote').val(oMUnit.Note);
    _bIsRound = oMUnit.IsRound;
    if (oMUnit.IsRound) { $('#radioBtnIsRoundYes').prop("checked", true); $('#radioBtnIsRoundNo').prop("checked", false); }
    else { $('#radioBtnIsRoundNo').prop("checked", true); $('#radioBtnIsRoundYes').prop("checked", false); }
}

function RefreshObjectMUnit() {
    
    if ($('#radioBtnIsRoundYes').prop("checked") == true) { _bIsRound = true; }
    else if ($('#radioBtnIsRoundNo').prop("checked") == true) { _bIsRound = false; }

    var oMUnit = {
        MeasurementUnitID: (_oMUnit != null) ? _oMUnit.MeasurementUnitID : 0,
        UnitType: $("#cboUnitType").val(),
        UnitName: document.getElementById('txtUnitName').value,
        Symbol: document.getElementById('txtSymbol').value,
        Note: document.getElementById('txtNote').value,
        IsRound: _bIsRound
    };
    return oMUnit;
}

function ValidateInputMUnit() {


    if ($('#cboUnitType').val() <= 0) {
        alert("Please select unit type."); $('#cboUnitType').focus();
        $('#cboUnitType').css("border", "1px solid #c00");
        return false;
    } else {
        $('#cboUnitType').css("border", "");
    }

    if (!$.trim($("#txtUnitName").val()).length) {
        alert("Please enter unit name.");
        $('#txtUnitName').val("");
        $('#txtUnitName').focus();
        $('#txtUnitName').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtUnitName').css("border", "");
    }

    if (!$.trim($("#txtSymbol").val()).length) {
        alert("Please enter symbol.");
        $('#txtSymbol').val("");
        $('#txtSymbol').focus();
        $('#txtSymbol').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtSymbol').css("border", "");
    }
    return true;
}




