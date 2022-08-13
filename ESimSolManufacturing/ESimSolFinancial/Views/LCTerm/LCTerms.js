

function InitializeLCTermsEvents() {

    DynamicRefreshList(_oLCTerms, "tblLCTerms");

    $('#txtSearchByLCTerm').keyup(function (e) {
        var obj =
                {
                    Event: e,
                    SearchProperty: "Name",
                    GlobalObjectList: _oLCTerms,
                    TableId: "tblLCTerms"
                };
        $('#txtSearchByLCTerm').icsSearchByText(obj);
    });

    $("#btnAddLCTerm").click(function () {
        $("#winLCTerm").icsWindow('open', "Add LCTerm");
        $("#winLCTerm input").val("");
        $("#winLCTerm input[type='checkbox']").prop('checked', false);
        _oLCTerm = null;
        RefreshLCTermLayout("btnAddLCTerm");
    });

    $("#btnEditLCTerm").click(function () {
        var oLCTerm = $("#tblLCTerms").datagrid("getSelected");
        if (oLCTerm == null || parseInt(oLCTerm.LCTermID) <= 0) { alert("Please select an item from list!"); return; }
        $("#winLCTerm").icsWindow('open', "Edit LCTerm");
        RefreshLCTermLayout("btnEditLCTerm");
        debugger;
        GetLCTermInformation(oLCTerm);
    });

    $("#btnViewLCTerm").click(function () {
        var oLCTerm = $("#tblLCTerms").datagrid("getSelected");
        if (oLCTerm == null || oLCTerm.LCTermID <= 0) { alert("Please select an item from list!"); return; }
        $("#winLCTerm").icsWindow('open', "View LCTerm");
        RefreshLCTermLayout("btnViewLCTerm");
        RefreshLCTermControl(oLCTerm);
    });

    $("#btnDeleteLCTerm").click(function () {

        var oLCTerm = $("#tblLCTerms").datagrid("getSelected");
        if (!confirm("Confirm to Delete?")) return false;
        if (oLCTerm == null || oLCTerm.LCTermID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oLCTerm,
            ObjectId: oLCTerm.LCTermID,
            ControllerName: "LCTerm",
            ActionName: "Delete",
            TableId: "tblLCTerms",
            IsWinClose: true
        };
        $.icsDelete(obj);
    });
}

function GetLCTermInformation(oLCTerm) {
    debugger;
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oLCTerm,
        ControllerName: "LCTerm",
        ActionName: "Get",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        debugger;
        if (response.status && response.obj != null) {
            if (response.obj.LCTermID > 0) { RefreshLCTermControl(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else { alert("No information found."); }
    });
}

function RefreshLCTermLayout(buttonId) {
    if (buttonId === "btnViewLCTerm") {
        $("#winLCTerm input").prop("disabled", true);
        $("#btnSaveLCTerm").hide();
    }
    else {
        $("#winLCTerm input").prop("disabled", false);
        $("#btnSaveLCTerm").show();
    }
    $("#lblDays").hide();
    $("#divDays").hide();
}

function RefreshLCTermControl(oLCTerm) {
    debugger;
    _oLCTerm = oLCTerm;
    $("#txtName").val(_oLCTerm.Name);
    $("#txtDescription").val(_oLCTerm.Description);
    if (oLCTerm.Days > 0) {
        $("#lblDays").show();
        $("#divDays").show();
        $('#txtDays').numberbox('setValue', _oLCTerm.Days);
        $("#chkIncludeDays").prop('checked', true);
    }
    else {
        $("#lblDays").hide();
        $("#divDays").hide();
        $("#chkIncludeDays").prop('checked', false);
    }
}
