var _oLotsPicker = [];
var _oLot = {};
var _nProductID = 0;
var _nWorkingUnitID = 0;

function InitializeLotPickerEvents() {
    $('#txtSearchByLotNoPicker').keyup(function (e) {
        var obj =
        {
            Event: e,
            SearchProperty: "LotNo",
            GlobalObjectList: _oLotsPicker,
            TableId: "tblLotPicker"
        };
        $('#txtSearchByLotNoPicker').icsSearchByText(obj);
    });

    $("#btnAddLotPicker").click(function () {
        if (!ValidateInputLot()) { return false; }
        var _oLot = RefreshObjectLot();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: _oLot,
            ObjectId: _oLot.LotID,
            ControllerName: "Lot",
            ActionName: "ATMLNewIUD",
            TableId: "tblLotPicker",
            IsWinClose: false
        };
        $.icsSave(obj, function (response) { if (response.status) { _oLotsPicker.push(response.obj); $("#winLotPicker input").val(""); } });
    });

    $("#btnCloseLotPicker").click(function () {
        $("#winLotPicker input").val("");
        _nProductID = 0;
        _nWorkingUnitID = 0;
        _oLot = {};
        _oLotsPicker = [];
        DynamicRefreshList(_oLotsPicker, "tblLotPicker");
        $("#winLotPicker").icsWindow('close');
    });

    $("#btnRemoveLotPicker").click(function () {
        var oLot = $("#tblLotPicker").datagrid("getSelected");
        if (oLot == null || oLot.LotID <= 0)
        {
            alert("Please select an item from list.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        oLot.LotTableName = "LotA";
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oLot,
            ControllerName: "Lot",
            ActionName: "Delete",
            TableId: "tblLotPicker",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });
    
}

function RefreshObjectLot() {
    //var nLotID = (_oLot == null ? 0 : (_oLot.LotID == null) ? 0 : _oLot.LotID);
    var oLot = {
        LotID: 0,
        ProductID: _nProductID,
        LotNo: $.trim($("#txtSearchByLotNoPicker").val()),
        UnitPrice: $.trim($("#txtUnitPricePicker").val()),
        WorkingUnitID: _nWorkingUnitID,
        NParentType: 110,
        Balance: 0,
        Amount: 0,
        LogNo: $.trim($("#txtLotNote").val())
    };
    return oLot;
}

function ValidateInputLot() {
    if (!$.trim($("#txtSearchByLotNoPicker").val()).length) {
        alert("Please Enter Lot Number!");
        $('#txtSearchByLotNoPicker').val("");
        $('#txtSearchByLotNoPicker').focus();
        $("#txtSearchByLotNoPicker").addClass("errorFieldBorder");
        return false;
    } else {
        $("#txtSearchByLotNoPicker").removeClass("errorFieldBorder");
    }
    //if (!$.trim($("#txtUnitPricePicker").val()).length || parseFloat($.trim($("#txtUnitPricePicker").val())) < 0) {
    //    alert("Please Enter Unit Price!");
    //    $('#txtUnitPricePicker').val("");
    //    $('#txtUnitPricePicker').focus();
    //    $("#txtUnitPricePicker").addClass("errorFieldBorder");
    //    return false;
    //} else {
    //    $("#txtUnitPricePicker").removeClass("errorFieldBorder");
    //}
    return true;
}