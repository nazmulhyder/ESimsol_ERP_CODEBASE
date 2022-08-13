var _nBuyerId = 0;
var _nDeliverToId = 0;
var _isAddDCD = true;
var _btnName = "";
var _nRowIndex = -1;
function InitializeSUDeliveryChallanEvents() {
    LoadComboboxesDC1();

    $("#btnCloseSUDeliveryChallan").click(function () {
        $("#winSUDeliveryChallan").icsWindow("close");
    });

    LotAssign();

    $("#btnRefreshSUDeliveryChallanDetail").click(function () {
        endEditing();
    });

    $("#btnProductRemoveSUDeliveryChallanDetail").click(function () {
        var oDeliveryChallanDetail = $("#tblSUDeliveryChallanDetails").datagrid("getSelected");
        if (oDeliveryChallanDetail == null || oDeliveryChallanDetail.DeliveryChallanDetailID == 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var nRowIndex = $("#tblSUDeliveryChallanDetails").datagrid("getRowIndex", oDeliveryChallanDetail);
        $("#tblSUDeliveryChallanDetails").datagrid("deleteRow", nRowIndex);
        RefreshTotalSummeryDC();


        var oDCDs = $("#tblSUDeliveryChallanDetails").datagrid("getRows");
        if (oDCDs.length == 0) {
            $("#cboFinishGoodsStoreSUDeliveryChallan").prop("disabled", false);
        }
    });

    $("#btnSaveSUDeliveryChallan").click(function () {
        
        if (!endEditing()) { return false; }
        if (!ValidateInputSUDeliveryChallan()) { return false; }
        
        var oSUDeliveryChallan = RefreshObjectSUDeliveryChallan();
        if (!_isAddDCD) {
            var oSUDC = $("#tblSUDeliveryChallans").datagrid("getSelected");
            var nRowIndex = $('#tblSUDeliveryChallans').datagrid('getRowIndex', oSUDC);
        }
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryChallan/SaveSUDeliveryChallan",
            traditional: true,
            data: JSON.stringify(oSUDeliveryChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                var oSUDeliveryChallan = jQuery.parseJSON(data);
                if (oSUDeliveryChallan != null) {
                    if (oSUDeliveryChallan.SUDeliveryChallanID > 0) {
                        alert("Data Saved Successful");
                        $("#winSUDeliveryChallan").icsWindow("close");
                        if (!_isAddDCD) {
                            $('#tblSUDeliveryChallans').datagrid('updateRow', { index: nRowIndex, row: oSUDeliveryChallan });
                        }
                        if (_nRowIndex > -1)
                        {
                            $('#tblSUDOs').datagrid('updateRow', { index: _nRowIndex, row: oSUDeliveryChallan }); //For ViewSUDPChallanPrepare
                        }
                    }
                    else {
                        alert(oSUDeliveryChallan.ErrorMessage);
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

    $("#btnChallanUndoApproveSUDeliveryChallan").click(function () {
        debugger;
        var oSUDeliveryChallan = $("#tblSUDeliveryChallans").datagrid("getSelected");
        if (oSUDeliveryChallan == null || oSUDeliveryChallan.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
      
        if (oSUDeliveryChallan.ChallanStatusInInt != 1) {
            alert("Please select an Approved Challan.");
            return false;
        }

        if (!confirm("Confirm Undo Approve?")) return false;

        var nRowIndex = $('#tblSUDeliveryChallans').datagrid('getRowIndex', oSUDeliveryChallan);

        oSUDeliveryChallan.ChallanStatusInInt = 0;

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryChallan/UpdateOnlyStatus",
            traditional: true,
            data: JSON.stringify(oSUDeliveryChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryChallan = jQuery.parseJSON(data);
                if (oSUDeliveryChallan != null) {
                    if (oSUDeliveryChallan.SUDeliveryChallanID > 0) {
                        alert("Undo Approved Successful");
                        $('#tblSUDeliveryChallans').datagrid('updateRow', { index: nRowIndex, row: oSUDeliveryChallan });
                    }
                    else {
                        alert(oSUDeliveryChallan.ErrorMessage);
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

    $("#btnApproveSUDeliveryChallan").click(function () {
        if (!endEditing()) { return false; }
        if (!ValidateInputSUDeliveryChallan()) { return false; }
        if (!IsValidForApproveSUDeliveryChallan()) { return false; }
        if (!confirm("Confirm Approve?")) return false;
        var oSUDeliveryChallan = RefreshObjectSUDeliveryChallan();
        oSUDeliveryChallan.ChallanStatusInInt = 1;
        
        if (!_isAddDCD) {
            var oSUDC = $("#tblSUDeliveryChallans").datagrid("getSelected");
            var nRowIndex = $('#tblSUDeliveryChallans').datagrid('getRowIndex', oSUDC);
        }
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryChallan/SaveSUDeliveryChallan",
            traditional: true,
            data: JSON.stringify(oSUDeliveryChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryChallan = jQuery.parseJSON(data);
                if (oSUDeliveryChallan != null) {
                    if (oSUDeliveryChallan.SUDeliveryChallanID > 0) {
                        alert("Data Saved Successful");
                        $("#winSUDeliveryChallan").icsWindow("close");
                        if (!_isAddDCD) {
                            $('#tblSUDeliveryChallans').datagrid('updateRow', { index: nRowIndex, row: oSUDeliveryChallan });
                        }
                    }
                    else {
                        alert(oSUDeliveryChallan.ErrorMessage);
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

    $("#btnDisburseSUDeliveryChallan").click(function () {
        if (!IsValidForDisburseSUDeliveryChallan()) { return false; }

        if (!confirm("Confire Disburse?")) return false;

        var oSUDC = $("#tblSUDeliveryChallans").datagrid("getSelected");
        var nRowIndex = $('#tblSUDeliveryChallans').datagrid('getRowIndex', oSUDC);
        var oSUDeliveryChallan = {
            SUDeliveryChallanID: oSUDC.SUDeliveryChallanID
        }
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryChallan/SUDeliveryChallanDisburse",
            traditional: true,
            data: JSON.stringify(oSUDeliveryChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryChallan = jQuery.parseJSON(data);
                if (oSUDeliveryChallan.ErrorMessage == "") {
                    if (oSUDeliveryChallan.SUDeliveryChallanID > 0) {
                        alert("Disburse Successful.");
                        $('#tblSUDeliveryChallans').datagrid('updateRow', { index: nRowIndex, row: oSUDeliveryChallan });
                        $("#winSUDeliveryChallan").icsWindow("close");
                    }
                    else {
                        alert("Invalid Delivery Challan.");
                    }
                }
                else {
                    alert(oSUDeliveryChallan.ErrorMessage);
                }

            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnCopyItemSUDeliveryChallanDetail").click(function () {
        var oSUDeliveryChallanDetail = $("#tblSUDeliveryChallanDetails").datagrid("getSelected");
        if (oSUDeliveryChallanDetail == null || oSUDeliveryChallanDetail.ProductID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        var oCollections = $("#tblSUDeliveryChallanDetails").datagrid("getRows");
        var nIndex = oCollections.length;
        var oSUDC = {
            SUDeliveryChallanDetailID: 0,
            SUDeliveryChallanID: oSUDeliveryChallanDetail.SUDeliveryChallanID,
            SUDeliveryOrderDetailID: oSUDeliveryChallanDetail.SUDeliveryOrderDetailID,
            ProductID: oSUDeliveryChallanDetail.ProductID,
            MUnitID: oSUDeliveryChallanDetail.MUnitID,
            Qty: 0,//oSUDeliveryChallanDetail.Qty,
            ProductCode: oSUDeliveryChallanDetail.ProductCode,
            ProductName: oSUDeliveryChallanDetail.ProductName,
            WorkingUnitID: oSUDeliveryChallanDetail.WorkingUnitID,
            LotID: 0,
            LotNo: "",
            QtySt:  "0",
            QtyLbsSt: "0",
            ProgramQty: oSUDeliveryChallanDetail.ProgramQty
        };
        $("#tblSUDeliveryChallanDetails").datagrid("appendRow", oSUDC);
        $("#tblSUDeliveryChallanDetails").datagrid("selectRow", nIndex);
        RefreshTotalSummeryDC();
    });

    $("#cboGoodsCheckedBySUDeliveryChallan").focusout(function (e) {
        $("#cboGoodsCheckedBySUDeliveryChallan").removeClass("ComboFocusBorderClass");
    });
}

function RefreshObjectSUDeliveryChallan() {
    var oSUDeliveryChallan = {
        SUDeliveryChallanID: (_oSUDeliveryChallan == null ? 0 : _oSUDeliveryChallan.SUDeliveryChallanID),
        SUDeliveryOrderID : (_oSUDeliveryChallan == null ? 0 : _oSUDeliveryChallan.SUDeliveryOrderID),
        ChallanNo: $.trim($("#txtChallanNoSUDeliveryChallan").val()),
        ChallanStatusInInt: (_oSUDeliveryChallan == null ? 0 : _oSUDeliveryChallan.ChallanStatusInInt),
        BuyerID: parseInt(_nBuyerId),
        DeliveryTo: parseInt(_nDeliverToId),
        ChallanDate: $('#txtChallanDateSUDeliveryChallan').datebox('getValue'),
        CheckedBy: parseInt($("#cboGoodsCheckedBySUDeliveryChallan").val()),
        StoreID: parseInt($("#cboFinishGoodsStoreSUDeliveryChallan").val()),
        VehicleNo: $.trim($("#txtVehicleNoSUDeliveryChallan").val()),
        Remarks: $.trim($("#txtRemarksSUDeliveryChallan").val()),
        DriverName: $.trim($("#txtDriverNameSUDeliveryChallan").val()),
        DriverContactNo: $.trim($("#txtDriverContactNoSUDeliveryChallan").val()),
        StorePhoneNo: $.trim($("#txtStorePhoneNoSUDeliveryChallan").val()),
        GatePassNo: $.trim($("#txtGatePassNoSUDeliveryChallan").val()),
        SUDeliveryChallanDetails: $("#tblSUDeliveryChallanDetails").datagrid("getRows")
    };
    return oSUDeliveryChallan;
}
function IsValidForApproveSUDeliveryChallan() {
    if ($.trim($("#txtDriverNameSUDeliveryChallan").val()) == 0) {
        alert("Please select Driver Name.");
        $("#txtDriverNameSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtDriverNameSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtDriverNameSUDeliveryChallan").removeClass("errorFieldBorder");
    }
    if ($.trim($("#txtDriverContactNoSUDeliveryChallan").val()) == 0) {
        alert("Please select Driver Mobile No.");
        $("#txtDriverContactNoSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtDriverContactNoSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtDriverContactNoSUDeliveryChallan").removeClass("errorFieldBorder");
    }
    if ($.trim($("#txtVehicleNoSUDeliveryChallan").val()) == 0) {
        alert("Please select Vehicle No.");
        $("#txtVehicleNoSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtVehicleNoSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtVehicleNoSUDeliveryChallan").removeClass("errorFieldBorder");
    }
    //if ($.trim($("#txtGatePassNoSUDeliveryChallan").val()) == 0) {
    //    alert("Please select Gate Pass No.");
    //    $("#txtGatePassNoSUDeliveryChallan").addClass("errorFieldBorder");
    //    $("#txtGatePassNoSUDeliveryChallan").focus();
    //    return false;
    //} else {
    //    $("#txtGatePassNoSUDeliveryChallan").removeClass("errorFieldBorder");
    //}

    var oDeliveryChallanDetails = $("#tblSUDeliveryChallanDetails").datagrid("getRows");
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
function IsValidForDisburseSUDeliveryChallan() {

    if ($.trim($("#txtDriverNameSUDeliveryChallan").val()) == 0) {
        alert("Please select Driver Name.");
        $("#txtDriverNameSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtDriverNameSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtDriverNameSUDeliveryChallan").removeClass("errorFieldBorder");
    }
    if ($.trim($("#txtDriverContactNoSUDeliveryChallan").val()) == 0) {
        alert("Please select Driver Mobile No.");
        $("#txtDriverContactNoSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtDriverContactNoSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtDriverContactNoSUDeliveryChallan").removeClass("errorFieldBorder");
    }
    if ($.trim($("#txtVehicleNoSUDeliveryChallan").val()) == 0) {
        alert("Please select Vehicle No.");
        $("#txtVehicleNoSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtVehicleNoSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtVehicleNoSUDeliveryChallan").removeClass("errorFieldBorder");
    }
    if ($.trim($("#txtGatePassNoSUDeliveryChallan").val()) == 0) {
        alert("Please select Gate Pass No.");
        $("#txtGatePassNoSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtGatePassNoSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtGatePassNoSUDeliveryChallan").removeClass("errorFieldBorder");
    }
    return true;
}
function ValidateInputSUDeliveryChallan() {
    if (_nBuyerId == 0) {
        alert("Please select Buyer.");
        $("#txtSearchByBuyerSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtSearchByBuyerSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtSearchByBuyerSUDeliveryChallan").removeClass("errorFieldBorder");
    }

    if (_nDeliverToId == 0) {
        alert("Please select delivery to.");
        $("#txtSearchByDeliveryToSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtSearchByDeliveryToSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtSearchByDeliveryToSUDeliveryChallan").removeClass("errorFieldBorder");
    }

    if (parseInt($("#cboGoodsCheckedBySUDeliveryChallan").val()) == 0) {
        alert("Please select Goods Checked By.");
        $("#cboGoodsCheckedBySUDeliveryChallan").addClass("errorFieldBorder");
        $("#cboGoodsCheckedBySUDeliveryChallan").focus();
        return false;
    } else {
        $("#cboGoodsCheckedBySUDeliveryChallan").removeClass("errorFieldBorder");
    }

    if (parseInt($("#cboFinishGoodsStoreSUDeliveryChallan").val()) == 0) {
        alert("Please select Finish Goods Store.");
        $("#cboFinishGoodsStoreSUDeliveryChallan").addClass("errorFieldBorder");
        $("#cboFinishGoodsStoreSUDeliveryChallan").focus();
        return false;
    } else {
        $("#cboFinishGoodsStoreSUDeliveryChallan").removeClass("errorFieldBorder");
    }

    var dChallanDate = $('#txtChallanDateSUDeliveryChallan').datebox('getValue');
    if (dChallanDate == null || dChallanDate == "") {
        alert("Please select Challan Date.");
        $("#txtChallanDateSUDeliveryChallan").addClass("errorFieldBorder");
        $("#txtChallanDateSUDeliveryChallan").focus();
        return false;
    } else {
        $("#txtChallanDateSUDeliveryChallan").removeClass("errorFieldBorder");
    }


    var oDeliveryChallanDetails = $("#tblSUDeliveryChallanDetails").datagrid("getRows");
    if (oDeliveryChallanDetails.length == 0) {
        alert("Please add minimum one Item.");
        return false;
    }
    else {
        for (var i = 0; i < oDeliveryChallanDetails.length; i++) {
            if (oDeliveryChallanDetails[i].Qty <= 0) {
                alert("All products must have Qty.");
                return false;
            }
        }
    }
    return true;
}

function LoadComboboxesDC1() {
    $("#cboFinishGoodsStoreSUDeliveryChallan").icsLoadCombo({
        List: _oReceivedStores,
        OptionValue: "WorkingUnitID",
        DisplayText: "OperationUnitName"
    });

    $("#cboBuyerSUDeliveryChallan,#cboDeliveryToSUDeliveryChallan").icsLoadCombo({
        List: _oContractorTypes,
        OptionValue: "Value",
        DisplayText: "Text"
    });

    $("#cboGoodsCheckedBySUDeliveryChallan").icsLoadCombo({
        List: _oCheckedByUsers,
        OptionValue: "UserID",
        DisplayText: "UserName"
    });
}

function LotAssign() {
    $("#btnLotAssignSUDeliveryChallanDetail").click(function () {
        if (parseInt($("#cboFinishGoodsStoreSUDeliveryChallan").val()) == 0) {
            alert("Please select Finish Goods Store.");
            $("#cboFinishGoodsStoreSUDeliveryChallan").addClass("errorFieldBorder");
            $("#cboFinishGoodsStoreSUDeliveryChallan").focus();
            return false;
        } else {
            $("#cboFinishGoodsStoreSUDeliveryChallan").removeClass("errorFieldBorder");
        }

        var oDeliveryChallanDetail = $("#tblSUDeliveryChallanDetails").datagrid("getSelected");
        if (oDeliveryChallanDetail == null || oDeliveryChallanDetail.ProductID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        var oSUDeliveryChallanDetail = {
            WorkingUnitID: parseInt($("#cboFinishGoodsStoreSUDeliveryChallan").val()),
            ProductID: oDeliveryChallanDetail.ProductID
        };

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/Lot/GetsLotForDeliveryChallanDetailWithBalance",
            traditional: true,
            data: JSON.stringify(oSUDeliveryChallanDetail),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oLots = jQuery.parseJSON(data);
                if (oLots != null) {
                    if (oLots.length > 0) {
                        if (oLots[0].ErrorMessage == "") {
                            var nFinishGoodsStoreId = $("#cboFinishGoodsStoreSUDeliveryChallan").val();
                            var sFinishGoodsStoreName = $("#cboFinishGoodsStoreSUDeliveryChallan :selected").text();
                            $("#winLotAssignSUDeliveryChallan").icsWindow("open", "Selected Store : " + sFinishGoodsStoreName + ", Selected Product : " + oDeliveryChallanDetail.ProductName);
                            DynamicRefreshList(oLots, "tblLotAssign");
                        }
                        else {
                            alert(oLots[0].ErrorMessage);
                            DynamicRefreshList([], "tblLotAssign");
                        }
                    }
                    else {
                        alert("Sorry, No Lot Found.");
                        DynamicRefreshList([], "tblLotAssign");
                    }
                }
                else {
                    alert("Sorry, No Data Found.");
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnCloseLotAssign").click(function () {
        $("#winLotAssignSUDeliveryChallan").icsWindow("close");
    });

    $("#btnOkLotAssign").click(function () {
        var oLot = $("#tblLotAssign").datagrid("getSelected");
        if (oLot == null || oLot.LotID == 0) {
            alert("Please select a Lot.");
            return false;
        }
        $("#cboFinishGoodsStoreSUDeliveryChallan").prop("disabled",true);
        var oDeliveryChallanDetail = $("#tblSUDeliveryChallanDetails").datagrid("getSelected");
        var nRowIndex = $("#tblSUDeliveryChallanDetails").datagrid("getRowIndex", oDeliveryChallanDetail);
        
        oDeliveryChallanDetail.LotID = oLot.LotID;
        oDeliveryChallanDetail.LotNo = oLot.LotNo;
        oDeliveryChallanDetail.WUName = $('#cboFinishGoodsStoreSUDeliveryChallan option:selected').text();
        oDeliveryChallanDetail.LotNoWithStore = oLot.LotNo + " (" + oDeliveryChallanDetail.WUName + ")";
        $('#tblSUDeliveryChallanDetails').datagrid('updateRow', { index: nRowIndex, row: oDeliveryChallanDetail });
        $("#winLotAssignSUDeliveryChallan").icsWindow("close");

        if (_btnName != "btnNewChallanSUDO")
        {
            var oSUDeliveryChallanDetail = $('#tblSUDeliveryChallanDetails').datagrid('getSelected');

            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oSUDeliveryChallanDetail,
                ObjectId: oSUDeliveryChallanDetail.SUDeliveryChallanDetailID,
                ControllerName: "SUDeliveryChallan",
                ActionName: "SaveSUDeliveryChallanDetail",
                TableId: "tblSUDeliveryChallanDetails",
                IsWinClose: false,
                Message: ""
            };

            $.icsSave(obj, function (response) {
                
                if (response.status && response.obj != null) {
                    var oSUDCDetail = response.obj;
                    if (oSUDCDetail.SUDeliveryChallanDetailID > 0) {
                        $('#tblSUDeliveryChallanDetails').datagrid('updateRow', { index: nRowIndex, row: oSUDCDetail });
                    }
                }
            });
        }
        endEditing();
    });
}

function RefreshTotalSummeryDC() {
    var nTotalQtyKg = 0;
    var nTotalQtyLbs = 0;
    var nTotalProgramQty = 0;
    var nTotalBags = 0;
    var oSUDeliveryChallanDetails = $('#tblSUDeliveryChallanDetails').datagrid('getRows');
    for (var i = 0; i < oSUDeliveryChallanDetails.length; i++) {
        nTotalQtyKg = parseFloat(nTotalQtyKg) + parseFloat(oSUDeliveryChallanDetails[i].Qty);
        var nQtyLbs = GetLBS(parseFloat(oSUDeliveryChallanDetails[i].Qty), 2);
        nTotalQtyLbs = parseFloat(nTotalQtyLbs) + parseFloat(nQtyLbs);
        nTotalProgramQty += parseFloat(oSUDeliveryChallanDetails[i].ProgramQty);
        nTotalBags += parseInt(oSUDeliveryChallanDetails[i].Bags);
    }
    $(".lblTotalQty").text(formatPrice(nTotalQtyKg) + "(KG)");
    $(".lblTotalQtyLbs").text(formatPrice(nTotalQtyLbs) + "(LBS)");
    $("#lblTotalProgramQty").text(formatPrice(nTotalProgramQty) + "(KG)");
    if (isNaN(nTotalBags))
    {
        nTotalBags = 0;
    }
    $("#lblTotalBags").text(nTotalBags);
}

var editIndex = undefined;
function onClickRowSUDC(index) {
    if (editIndex != index) {
        if (endEditing()) {
            $('#tblSUDeliveryChallanDetails').datagrid('selectRow', index).datagrid('beginEdit', index);
            var oSUDeliveryChallanDetail = $('#tblSUDeliveryChallanDetails').datagrid('getSelected');
            editIndex = index;
        }
        else {
            $('#tblSUDeliveryChallanDetails').datagrid('selectRow', editIndex);
        }
    }
}

function endEditing() {
    if (editIndex == undefined) {
        return true;
    }
    if ($('#tblSUDeliveryChallanDetails').datagrid('validateRow', editIndex)) {
        $('#tblSUDeliveryChallanDetails').datagrid('endEdit', editIndex);
        $('#tblSUDeliveryChallanDetails').datagrid('selectRow', editIndex);
        var oSUDeliveryChallanDetail = $('#tblSUDeliveryChallanDetails').datagrid('getSelected');
        if (oSUDeliveryChallanDetail != null) {
            var nTotalQty = 0;
            var oSUDeliveryChallanDetails = $('#tblSUDeliveryChallanDetails').datagrid('getRows');
            for (var i = 0; i < oSUDeliveryChallanDetails.length; i++) {
                if (oSUDeliveryChallanDetails[i].ProductID == oSUDeliveryChallanDetail.ProductID) {
                    nTotalQty += parseFloat(oSUDeliveryChallanDetails[i].Qty);
                }
            }
            
            if (nTotalQty > oSUDeliveryChallanDetail.ProgramQty) {
                var sMessage = "Sorry, Your Entered Qty (" + formatPrice(nTotalQty, 2) + ") cannot be greater than Program Qty (" + formatPrice(oSUDeliveryChallanDetail.ProgramQty, 2) + ") ";
                alert(sMessage);
                //alert("Sorry, Maximum Program Qty of this prodcut is " + formatPrice(oSUDeliveryChallanDetail.ProgramQty, 2) + "\n but your entered total qty for product " + oSUDeliveryChallanDetail.ProductName + " is " + formatPrice(nTotalQty, 2));
                $('#tblSUDeliveryChallanDetails').datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
                return false;
            }
            oSUDeliveryChallanDetail.QtyLbsSt = GetLBS(parseFloat(oSUDeliveryChallanDetail.Qty), 2);
            oSUDeliveryChallanDetail.Bags = parseInt(oSUDeliveryChallanDetail.Bags);
            $('#tblSUDeliveryChallanDetails').datagrid('updateRow', { index: editIndex, row: oSUDeliveryChallanDetail });
        }
        editIndex = undefined;
        RefreshTotalSummeryDC();
        return true;
    }
    else {
        return false;
    }
}
