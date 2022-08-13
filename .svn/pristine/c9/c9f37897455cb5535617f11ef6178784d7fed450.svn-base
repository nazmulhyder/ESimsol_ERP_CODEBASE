
function InitializeLCTermEvents() {   

    $("#btnSaveLCTerm").click(function (e) {
        if (!ValidateInputLCTerm()) return;
        var oLCTerm = RefreshObjectLCTerm();
        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oLCTerm,
                ObjectId: oLCTerm.LCTermID,
                ControllerName: "LCTerm",
                ActionName: "Save",
                TableId: "tblLCTerms",
                IsWinClose: true
            };
        $.icsSave(obj);
    });

    $("#btnCloseLCTerm").click(function () {
        $("#winLCTerm").icsWindow("close");
        $("#winLCTerm input").val("");
        $("#winLCTerm select").val(0);
    });    
}

function RefreshObjectLCTerm() {
    var nDays = 0;
    if ($('#chkIncludeDays').is(":checked")) {
        nDays = parseInt($('#txtDays').numberbox('getValue'));
    }
    var oLCTerm = {
        LCTermID: (_oLCTerm != null) ? _oLCTerm.LCTermID : 0,        
        Name: $("#txtName").val(),
        Days:nDays,
        Description: $("#txtDescription").val()
    };
    return oLCTerm;
}

function IsChecked()
{
    if ($('#chkIncludeDays').is(":checked")) {
        $("#lblDays").show();
        $("#divDays").show();
    }
    else {
        $("#lblDays").hide();
        $("#divDays").hide();
    }
}

function ValidateInputLCTerm() {
    debugger;
    //if (!$.trim($('#txtName').val()).length) {
    //    alert("Please enter LCTerm."); $('#txtName').focus();
    //    $('#txtName').css("border", "1px solid #c00");
    //    return false;
    //} else { $('#txtName').css("border", ""); }
    
    if ($('#chkIncludeDays').is(":checked")) {
        var nDays = $('#txtDays').numberbox('getValue');
        if (nDays==""||parseInt(nDays) <= 0) {
            alert("Please enter Days."); $('#txtDays').focus();
            $('#txtDays').css("border", "1px solid #c00");
            return false;
        } else { $('#txtDays').css("border", ""); }
    }
    return true;
}

