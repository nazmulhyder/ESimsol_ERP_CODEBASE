var _nBuyerId = 0;
var _nDeliverToId = 0;
var _isAddDCD = true;
function InitializeSUReturnChallanEvents() {
    LoadComboboxesDC();

    $("#btnCloseSUReturnChallan").click(function () {
        $("#winSUReturnChallan").icsWindow("close");
    });

    
    $("#btnRefreshSUReturnChallanDetail").click(function () {
        endEditing1();
    });

    $("#btnProductRemoveSUReturnChallanDetail").click(function () {
        var oDeliveryChallanDetail = $("#tblSUReturnChallanDetails").datagrid("getSelected");
        if (oDeliveryChallanDetail == null || oDeliveryChallanDetail.DeliveryChallanDetailID == 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var nRowIndex = $("#tblSUReturnChallanDetails").datagrid("getRowIndex", oDeliveryChallanDetail);
        $("#tblSUReturnChallanDetails").datagrid("deleteRow", nRowIndex);
        RefreshTotalSummerySURCD();
    });

    $("#btnSaveSUReturnChallan").click(function () {
        if (!endEditing1()) { return false; }
        if (!ValidateInputSUReturnChallan()) { return false; }
        
        _oSUReturnChallan.PartyChallanNo = $.trim($("#txtPartyChallanNoSUReturnChallan").val());
        _oSUReturnChallan.Remarks = $.trim($("#txtRemarksSUReturnChallan").val());
        _oSUReturnChallan.SUReturnChallanDetails = $("#tblSUReturnChallanDetails").datagrid("getRows");
        debugger;
        if (!_isAddDCD) {
            var oSUDC = $("#tblSURCs").datagrid("getSelected");
            var nRowIndex = $('#tblSURCs').datagrid('getRowIndex', oSUDC);
        }
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUReturnChallan/SaveSUReturnChallan",
            traditional: true,
            data: JSON.stringify(_oSUReturnChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUReturnChallan = jQuery.parseJSON(data);
                if (oSUReturnChallan != null) {
                    if (oSUReturnChallan.SUReturnChallanID > 0) {
                        $("#txtReturnNoSUReturnChallan").val(oSUReturnChallan.ReturnNo);
                        alert("Data Saved Successful");
                        $("#winSUReturnChallan").icsWindow("close");
                        if (!_isAddDCD) {
                            $('#tblSURCs').datagrid('updateRow', { index: nRowIndex, row: oSUReturnChallan });
                        }
                    }
                    else {
                        alert(oSUReturnChallan.ErrorMessage);
                    }
                }
                else {
                    alert("Sorry, Error Found.");
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnApproveSUReturnChallan").click(function () {
        if (!endEditing1()) { return false; }
        if (!ValidateInputSUReturnChallan()) { return false; }
        if (!IsValidForApproveSUReturnChallan()) { return false; }

        _oSUReturnChallan.PartyChallanNo = $.trim($("#txtPartyChallanNoSUReturnChallan").val());
        _oSUReturnChallan.Remarks = $.trim($("#txtRemarksSUReturnChallan").val());
        _oSUReturnChallan.SUReturnChallanDetails = $("#tblSUReturnChallanDetails").datagrid("getRows");
        _oSUReturnChallan.ReturnStatus = 1;
        
        if (!_isAddDCD) {
            var oSUDC = $("#tblSURCs").datagrid("getSelected");
            var nRowIndex = $('#tblSURCs').datagrid('getRowIndex', oSUDC);
        }
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUReturnChallan/SaveSUReturnChallan",
            traditional: true,
            data: JSON.stringify(_oSUReturnChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUReturnChallan = jQuery.parseJSON(data);
                if (oSUReturnChallan != null) {
                    if (oSUReturnChallan.SUReturnChallanID > 0) {
                        alert("Data Saved Successful");
                        $("#winSUReturnChallan").icsWindow("close");
                        if (!_isAddDCD) {
                            $('#tblSURCs').datagrid('updateRow', { index: nRowIndex, row: oSUReturnChallan });
                        }
                    }
                    else {
                        alert(oSUReturnChallan.ErrorMessage);
                    }
                }
                else {
                    alert("Sorry, Error Found.");
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnReceiveSUReturnChallan").click(function () {
        if (!IsValidForReceiveSUReturnChallan()) { return false;}
        var oSUDC = $("#tblSURCs").datagrid("getSelected");
        var nRowIndex = $('#tblSURCs').datagrid('getRowIndex', oSUDC);
        var oSUReturnChallan = {
            SUReturnChallanID: oSUDC.SUReturnChallanID
        }
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUReturnChallan/SUReturnChallanReceive",
            traditional: true,
            data: JSON.stringify(oSUReturnChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUReturnChallan = jQuery.parseJSON(data);
                if (oSUReturnChallan.ErrorMessage == "") {
                    if (oSUReturnChallan.SUReturnChallanID > 0) {
                        alert("Receive Successful.");
                        $('#tblSURCs').datagrid('updateRow', { index: nRowIndex, row: oSUReturnChallan });
                        $("#winSUReturnChallan").icsWindow("close");
                    }
                    else {
                        alert("Invalid Delivery Challan.");
                    }
                }
                else {
                    alert(oSUReturnChallan.ErrorMessage);
                }

            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    
}

function RefreshObjectSUReturnChallan() {
    var oSUReturnChallan = {
        SUReturnChallanID: (_oSUReturnChallan == null ? 0 : _oSUReturnChallan.SUReturnChallanID),
        SUDeliveryOrderID : (_oSUReturnChallan == null ? 0 : _oSUReturnChallan.SUDeliveryOrderID),
        ChallanNo: $.trim($("#txtChallanNoSUReturnChallan").val()),
        ChallanStatusInInt: (_oSUReturnChallan == null ? 0 : _oSUReturnChallan.ChallanStatusInInt),
        BuyerID: parseInt(_nBuyerId),
        DeliveryTo: parseInt(_nDeliverToId),
        ReturnDate: $('#txtReturnDateSUReturnChallan').datebox('getValue'),
        CheckedBy: parseInt($("#cboGoodsCheckedBySUReturnChallan").val()),
        StoreID: parseInt($("#cboFinishGoodsStoreSUReturnChallan").val()),
        VehicleNo: $.trim($("#txtVehicleNoSUReturnChallan").val()),
        Remarks: $.trim($("#txtRemarksSUReturnChallan").val()),
        PartyChallanNo: $.trim($("#txtPartyChallanNoSUReturnChallan").val()),
        DriverName: $.trim($("#txtDriverNameSUReturnChallan").val()),
        DriverContactNo: $.trim($("#txtDriverContactNoSUReturnChallan").val()),
        GatePassNo: $.trim($("#txtGatePassNoSUReturnChallan").val()),
        SUReturnChallanDetails: $("#tblSUReturnChallanDetails").datagrid("getRows")
    };
     

    return oSUReturnChallan;
}
function IsValidForApproveSUReturnChallan() {
    var oDeliveryChallanDetails = $("#tblSUReturnChallanDetails").datagrid("getRows");
    if (oDeliveryChallanDetails.length == 0) {
        alert("Please add minimum one Item.");
        return false;
    }
    else {
        for (var i = 0; i < oDeliveryChallanDetails.length; i++) {
            if (oDeliveryChallanDetails[i].LotID <= 0) {
                alert("Please assign Lot for " + oDeliveryChallanDetails[i].ProductName + " and Try Again!");
                return false;
            }
        }
    }
    return true;
}
function IsValidForReceiveSUReturnChallan() {

    var oDeliveryChallanDetails = $("#tblSUReturnChallanDetails").datagrid("getRows");
    if (oDeliveryChallanDetails.length == 0) {
        alert("Please add minimum one Item.");
        return false;
    }
    else {
        for (var i = 0; i < oDeliveryChallanDetails.length; i++) {
            if (oDeliveryChallanDetails[i].LotID <= 0) {
                alert("Please assign Lot for " + oDeliveryChallanDetails[i].ProductName + " and Try Again!");
                return false;
            }
        }
    }
    return true;
}
function ValidateInputSUReturnChallan() {
    

    var dReturnDate = $('#txtReturnDateSUReturnChallan').datebox('getValue');
    if (dReturnDate == null || dReturnDate == "") {
        alert("Please select Return Date.");
        $("#txtReturnDateSUReturnChallan").addClass("errorFieldBorder");
        $("#txtReturnDateSUReturnChallan").focus();
        return false;
    } else {
        $("#txtReturnDateSUReturnChallan").removeClass("errorFieldBorder");
    }


    var oSUReturnChallanDetails = $("#tblSUReturnChallanDetails").datagrid("getRows");
    if (oSUReturnChallanDetails.length == 0) {
        alert("Please add minimum one Item.");
        return false;
    }
    else {
        for (var i = 0; i < oSUReturnChallanDetails.length; i++) {
            if (oSUReturnChallanDetails[i].Qty <= 0) {
                alert("All products must have Qty.");
                return false;
            }
        }
    }
    return true;
}

function LoadComboboxesDC() {
    $("#cboFinishGoodsStoreSUReturnChallan").icsLoadCombo({
        List: _oReceivedStores,
        OptionValue: "WorkingUnitID",
        DisplayText: "OperationUnitName"
    });

    $("#cboBuyerSUReturnChallan").icsLoadCombo({
        List: _oContractorTypes,
        OptionValue: "Value",
        DisplayText: "Text"
    });
}



function RefreshTotalSummerySURCD() {
    var nTotalQtyKg = 0;
    var nTotalQtyLbs = 0;
    var nTotalSUDCDQty = 0;
    var oSUReturnChallanDetails = $('#tblSUReturnChallanDetails').datagrid('getRows');
    for (var i = 0; i < oSUReturnChallanDetails.length; i++) {
        nTotalQtyKg += parseFloat(oSUReturnChallanDetails[i].Qty);
        var nQtyLbs = GetLBS(parseFloat(oSUReturnChallanDetails[i].Qty), 2);
        nTotalQtyLbs += parseFloat(nQtyLbs);
        nTotalSUDCDQty += parseFloat(oSUReturnChallanDetails[i].SUDCDQty);
    }
    $("#lblTotalQty").text(formatPrice(nTotalQtyKg));
    $("#lblTotalQtyLbs").text(formatPrice(nTotalQtyLbs));
    $("#lblTotalSUDCDQty").text(formatPrice(nTotalSUDCDQty));
}


function onClickRowSUDC1(index) {
    debugger;
    if (editIndex1 != index) {
        if (endEditing1()) {
            $('#tblSUReturnChallanDetails').datagrid('selectRow', index).datagrid('beginEdit', index);
            var oSUReturnChallanDetail = $('#tblSUReturnChallanDetails').datagrid('getSelected');
            editIndex1 = index;
        }
        else {
            $('#tblSUReturnChallanDetails').datagrid('selectRow', editIndex1);
        }
    }
}

var editIndex1 = undefined;
function endEditing1() {
    if (editIndex1 == undefined) {
        return true;
    }
    if ($('#tblSUReturnChallanDetails').datagrid('validateRow', editIndex1)) {
        $('#tblSUReturnChallanDetails').datagrid('endEdit', editIndex1);
        $('#tblSUReturnChallanDetails').datagrid('selectRow', editIndex1);
        var oSUReturnChallanDetail = $('#tblSUReturnChallanDetails').datagrid('getSelected');
        if (oSUReturnChallanDetail != null) {
            var nTotalQty = 0;
            var oSUReturnChallanDetails = $('#tblSUReturnChallanDetails').datagrid('getRows');
            for (var i = 0; i < oSUReturnChallanDetails.length; i++) {
                if (oSUReturnChallanDetails[i].ProductID == oSUReturnChallanDetail.ProductID) {
                    nTotalQty += parseFloat(oSUReturnChallanDetails[i].Qty);
                }
            }
            if (nTotalQty > oSUReturnChallanDetail.SUDCDQty) {
                alert("Sorry, Maximum Delivery Qty of this prodcut is " + formatPrice(oSUReturnChallanDetail.SUDCDQty, 2) + "\n but your entered total qty for product " + oSUReturnChallanDetail.ProductName + " is " + formatPrice(nTotalQty, 2));
                $('#tblSUReturnChallanDetails').datagrid('selectRow', editIndex1).datagrid('beginEdit', editIndex1);
                return false;
            }
            oSUReturnChallanDetail.QtyLbsSt = GetLBS(parseFloat(oSUReturnChallanDetail.Qty), 2);
            $('#tblSUReturnChallanDetails').datagrid('updateRow', { index: editIndex1, row: oSUReturnChallanDetail });
        }
        editIndex1 = undefined;
        RefreshTotalSummerySURCD();
        return true;
    }
    else {
        return false;
    }
}