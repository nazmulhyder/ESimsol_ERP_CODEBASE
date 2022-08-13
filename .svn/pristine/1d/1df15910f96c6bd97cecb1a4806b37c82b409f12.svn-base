var _isAddDCD = true;
function InitializeSUDeliveryChallansEvents() {
    var oSUDCs1 = _oSUDeliveryChallans;
    oSUDCs1 = TotalQtyInMainInterface(oSUDCs1);
    DynamicRefreshList(oSUDCs1, "tblSUDeliveryChallans");

    $("#btnAdvSearchSUDeliveryChallan").click(function () {
        ResetAdvSearchWindow();
        ResetAllTables("tblSUDeliveryChallanAdvSearch");
        $("#winAdvSearchSUDeliveryChallan").icsWindow("open", "Advance Search");
        $("#txtDONoAdvSearch").focus();
    });

    $("#btnPendingChallanSUDeliveryChallan").click(function () {
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: {},
            ControllerName: "SUDeliveryChallan",
            ActionName: "GetsPendingChallan",
            IsWinClose: false
        }
        $.icsDataGets(obj, function (response) {

            if (response.status) {
                if (response.objs.length > 0) {
                    _oSUDeliveryChallans = [];
                    _oSUDeliveryChallans = response.objs;

                    var oSUDCs1 = _oSUDeliveryChallans;
                    oSUDCs1 = TotalQtyInMainInterface(oSUDCs1);
                    DynamicRefreshList(oSUDCs1, 'tblSUDeliveryChallans');

                }
                else {
                    alert('Sorry, No data found.');
                    DynamicRefreshList([], 'tblSUDeliveryChallans');
                }
            }
        });
    });

    $("#btnEditSUDeliveryChallan").click(function () {

        var oDeliveryChallan = $("#tblSUDeliveryChallans").datagrid("getSelected");
        if (oDeliveryChallan == null || oDeliveryChallan.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (oDeliveryChallan.ChallanStatusInInt > 0) {
            alert("Sorry, Only Initialize Delivery Challan can edit.");
            return false;
        }
        _isAddDCD = false;

        GetSUDeliveryChallanInformation(oDeliveryChallan);
        $("#winSUDeliveryChallan").icsWindow("open", "Edit Delivery Challan");
        RefreshSUDeliveryChallanLayout("btnEditSUDeliveryChallan");
        RefreshTotalSummeryDC();
        debugger;
        endEditing();

        $("#cboGoodsCheckedBySUDeliveryChallan").focus();
        $("#cboGoodsCheckedBySUDeliveryChallan").addClass("ComboFocusBorderClass");

        GetPreviousRecord1();
    });

    $("#btnViewSUDeliveryChallan").click(function () {
        var oDeliveryChallan = $("#tblSUDeliveryChallans").datagrid("getSelected");
        if (oDeliveryChallan == null || oDeliveryChallan.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        _isAddDCD = false;
        GetSUDeliveryChallanInformation(oDeliveryChallan);
        $("#winSUDeliveryChallan").icsWindow("open", "View Delivery Challan");
        RefreshSUDeliveryChallanLayout("btnViewSUDeliveryChallan");
        RefreshTotalSummeryDC();
        $("#btnCloseSUDeliveryChallan").focus();
    });

    $("#btnDeleteSUDeliveryChallan").click(function () {
        var oDeliveryChallan = $("#tblSUDeliveryChallans").datagrid("getSelected");
        if (oDeliveryChallan == null || oDeliveryChallan.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (parseInt(oDeliveryChallan.ChallanStatusInInt) > 0) {
            alert("Only Initialize Delivery Challan can delete.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oDeliveryChallan,
            ObjectId: oDeliveryChallan.SUDeliveryChallanID,
            ControllerName: "SUDeliveryChallan",
            ActionName: "DeleteSUDeliveryChallan",
            TableId: "tblSUDeliveryChallans",
            IsWinClose: true
        };
        $.icsDelete(obj);
    });

    $("#btnChallanApproveSUDeliveryChallan").click(function () {
        var oDeliveryChallan = $("#tblSUDeliveryChallans").datagrid("getSelected");
        if (oDeliveryChallan == null || oDeliveryChallan.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (oDeliveryChallan.ChallanStatusInInt != 0) {
            alert("Sorry, Only Initialize Delivery Challan can approve.");
            return false;
        }
        if (oDeliveryChallan.ChallanStatusInInt == 1) {
            alert("Sorry, Already approved.");
            return false;
        }

        _isAddDCD = false;
        $("#winSUDeliveryChallan").icsWindow("open", "Approve Delivery Challan");
        RefreshSUDeliveryChallanLayout("btnChallanApproveSUDeliveryChallan");
        GetSUDeliveryChallanInformation(oDeliveryChallan);
        endEditing();
        $("#cboGoodsCheckedBySUDeliveryChallan").focus();
        $("#cboGoodsCheckedBySUDeliveryChallan").addClass("ComboFocusBorderClass");
    });

    $("#btnChallanDisburseSUDeliveryChallan").click(function () {
        var oDeliveryChallan = $("#tblSUDeliveryChallans").datagrid("getSelected");
        if (oDeliveryChallan == null || oDeliveryChallan.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (oDeliveryChallan.ChallanStatusInInt != 1) {
            alert("Sorry, Only Approve Delivery Challan can Disburse.");
            return false;
        }
        if (oDeliveryChallan.ChallanStatusInInt == 2) {
            alert("Sorry, Already Disbursed.");
            return false;
        }
        _isAddDCD = false;
        GetSUDeliveryChallanInformation(oDeliveryChallan);
        $("#winSUDeliveryChallan").icsWindow("open", "Disburse Delivery Challan");
        RefreshSUDeliveryChallanLayout("btnChallanDisburseSUDeliveryChallan");
        endEditing();
        $("#cboGoodsCheckedBySUDeliveryChallan").focus();
        $("#cboGoodsCheckedBySUDeliveryChallan").addClass("ComboFocusBorderClass");
    });

    $("#txtSearchByChallanNoSUDeliveryChallan").keyup(function (e) {
        if (e.keyCode == 13) {

            var oSUDeliveryChallan = {
                ChallanNo: $.trim($("#txtSearchByChallanNoSUDeliveryChallan").val()),
                DONo:""
            };
            SearchInCollectionPageSUDC(oSUDeliveryChallan);
        }
        else {
            $(this).icsSearchByText({
                Event: e,
                SearchProperty: "ChallanNo",
                GlobalObjectList: _oSUDeliveryChallans,
                TableId: "tblSUDeliveryChallans"
            });
        }
    });

    $("#txtSearchByDoNoSUDeliveryChallan").keyup(function (e) {
        if (e.keyCode == 13) {
            var oSUDeliveryChallan = {
                DONo: $.trim($("#txtSearchByDoNoSUDeliveryChallan").val()),
                ChallanNo: ""
            };
            //Ratin
            SearchInCollectionPageSUDC(oSUDeliveryChallan);
        }
        else {
            $(this).icsSearchByText({
                Event: e,
                SearchProperty: "DONo",
                GlobalObjectList: _oSUDeliveryChallans,
                TableId: "tblSUDeliveryChallans"
            });
        }
    });

    $("#btnPreviewSUDeliveryChallan").click(function () {
        var oDeliveryChallan = $("#tblSUDeliveryChallans").datagrid("getSelected");
        if (oDeliveryChallan == null || oDeliveryChallan.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        var nts = ((new Date()).getTime()) / 1000;
        window.open(_sBaseAddress + '/SUDeliveryChallan/PreviewSUDeliveryChallan?id=' + oDeliveryChallan.SUDeliveryChallanID + "&nPreviewType=" + 0, "_blank");
        nts = ((new Date()).getTime()) / 1000;
        window.open(_sBaseAddress + '/SUDeliveryChallan/PreviewGatePass?id=' + oDeliveryChallan.SUDeliveryChallanID + "&nPreviewType=" + 1, "_blank");
    });

   

    $("#btnReturnChallanSUDeliveryChallan").click(function () {
        var oSUDC = $("#tblSUDeliveryChallans").datagrid("getSelected");
        if (oSUDC == null || oSUDC.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (oSUDC.ChallanStatusInInt != 2) { alert("This item has not been Disbured, Please select another from list and try again."); return false; }
        _isAddDCD = true;
        SetValueForNewReturn(oSUDC);
        RefreshNewReturnLayout();
        endEditing();
        $("#txtRemarksSUReturnChallan").focus();
    });

    $("#btnWaitingApprovalSUDeliveryChallan").click(function () {
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: {},
            ControllerName: "SUDeliveryChallan",
            ActionName: "GetsWaitingApprovalChallan",
            IsWinClose: false
        }
        $.icsDataGets(obj, function (response) {

            if (response.status) {
                if (response.objs.length > 0) {
                    _oSUDeliveryChallans = [];
                    _oSUDeliveryChallans = response.objs;

                    var oSUDCs1 = _oSUDeliveryChallans;
                    oSUDCs1 = TotalQtyInMainInterface(oSUDCs1);
                    DynamicRefreshList(oSUDCs1, "tblSUDeliveryChallans");

                    //DynamicRefreshList(_oSUDeliveryChallans, 'tblSUDeliveryChallans');
                }
                else {
                    alert('Sorry, No data found.');
                    DynamicRefreshList([], 'tblSUDeliveryChallans');
                }
            }
        });
    });

    $("#btnSearchByDateSUDeliveryChallans").click(function () {
        var sChallanDateFrom = $("#txtChallanDateSUDeliveryChallansFrom").datebox('getValue');
        var sChallanDateTo = $("#txtChallanDateSUDeliveryChallansTo").datebox('getValue');

        var sParams = $("#chkDeliveryComplete").is(':checked') + "~" + true + "~" + sChallanDateFrom + "~" + sChallanDateTo;

        var oSUDeliveryChallan = {
            Remarks: sParams
            //SUDeliveryOrderID: oSUDeliveryChallan.SUDeliveryOrderID
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryChallan/Search",
            traditional: true,
            data: JSON.stringify(oSUDeliveryChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryChallans = jQuery.parseJSON(data);
                _oSUDeliveryChallans = oSUDeliveryChallans;
                if (oSUDeliveryChallans != null) {
                    if (oSUDeliveryChallans.length > 0) {
                        oSUDeliveryChallans = TotalQtyInMainInterface(oSUDeliveryChallans);
                        DynamicRefreshList(oSUDeliveryChallans, "tblSUDeliveryChallans");
                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSUDeliveryChallans");
                        _oSUDeliveryChallans = [];
                    }

                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblSUDeliveryChallans");
                    _oSUDeliveryChallans = [];
                }
            }
        });
    });
    $("#txtChallanDateSUDeliveryChallans").datebox('setValue', icsdateformat(new Date()));
}

function SearchInCollectionPageSUDC(oSUDeliveryChallan)
{
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SUDeliveryChallan/GetsBySearchKey",
        traditional: true,
        data: JSON.stringify(oSUDeliveryChallan),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var oSUDeliveryChallans = jQuery.parseJSON(data);
            _oSUDeliveryChallans = oSUDeliveryChallans;
            oSUDeliveryChallans = TotalQtyInMainInterface(oSUDeliveryChallans);
            if (oSUDeliveryChallans != null) {
                if (oSUDeliveryChallans.length > 0) {
                    DynamicRefreshList(oSUDeliveryChallans, "tblSUDeliveryChallans");
                }
                else {
                    DynamicRefreshList([], "tblSUDeliveryChallans");
                }
            } else {
                DynamicRefreshList([], "tblSUDeliveryChallans");
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function TotalQtyInMainInterface(oSUDeliveryChallans)
{
    var nTotalQty = 0;
    for (var i = 0; i < oSUDeliveryChallans.length; i++)
    {
        nTotalQty = parseFloat(oSUDeliveryChallans[i].Qty) + parseFloat(nTotalQty);
    }
    var oSUDeliveryChallan = {
        DeliveredByName : "Total : ",
        Qty: nTotalQty,
        QtySt: formatPrice(nTotalQty)
    };
    oSUDeliveryChallans.push(oSUDeliveryChallan);
    return oSUDeliveryChallans;
}

function SetValueForNewReturn(oSUDC) {
    $("#txtDONoSUReturnChallan").val(oSUDC.DONo);
    $("#txtChallanNoSUReturnChallan").val(oSUDC.ChallanNo);
    $("#txtSearchByBuyerSUReturnChallan").val(oSUDC.BuyerName);
    $("#cboFinishGoodsStoreSUReturnChallan").val(oSUDC.StoreID);
    $("#cboBuyerSUReturnChallan").val(oSUDC.BuyerContractorType);
   
    $("#txtRemarksSUDeliveryChallan").val(oSUDC.Remarks);
    
    if (_nBuyerId > 0) {
        $("#txtSearchByBuyerSUDeliveryChallan").addClass("fontColorOfPickItem");
    } else {
        $("#txtSearchByBuyerSUDeliveryChallan").removeClass("fontColorOfPickItem");
    }
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oSUDC,
        ControllerName: "SUReturnChallan",
        ActionName: "GetSUDC",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.SUDeliveryOrderID > 0) {
                debugger;
                var oSUDC = response.obj;
                var oSUDCDetails = oSUDC.SUDeliveryChallanDetails;
                _oSUReturnChallan = {
                    SUReturnChallanID: 0,
                    SUDeliveryChallanID: oSUDC.SUDeliveryChallanID,
                    ReturnDate: icsdateformat(new Date()),
                    BuyerID: oSUDC.BuyerID,
                    StoreID: oSUDC.StoreID,
                    ReturnStatus: 0
                };
                var oSUReturnChallanDetails = [];
                for (var i = 0; i < oSUDCDetails.length; i++) {
                    var oSUReturnChallanDetail = {
                        SUReturnChallanDetailID: 0,
                        SUReturnChallanID: 0,
                        SUDeliveryChallanDetailID: oSUDCDetails[i].SUDeliveryChallanDetailID,
                        ProductID: oSUDCDetails[i].ProductID,
                        ProductName: oSUDCDetails[i].ProductName,
                        ProductCode: oSUDCDetails[i].ProductCode,
                        LotID: oSUDCDetails[i].LotID,
                        LotNo: oSUDCDetails[i].LotNo,
                        MUnitID: oSUDCDetails[i].MUnitID,
                        Qty: oSUDCDetails[i].RemainingQty,
                        QtyLbsSt: GetLBS(oSUDCDetails[i].Qty, 2),
                        SUDCDQty: oSUDCDetails[i].Qty,
                        SUDeliveryProgramID: oSUDCDetails[i].SUDeliveryProgramID
                    };
                    oSUReturnChallanDetails.push(oSUReturnChallanDetail);
                }
                DynamicRefreshList(oSUReturnChallanDetails, "tblSUReturnChallanDetails");
                RefreshTotalSummerySURCD();
            }
            else {
                alert(response.obj.ErrorMessage);
            }
        }
        else {
            alert("No information found.");
        }
    });
}

function RefreshNewReturnLayout() {
    $(".enableDisable").prop('disabled', true);
    $("#txtReturnDateSUReturnChallan").datebox({ disabled: true });
    $("#txtReturnDateSUReturnChallan").datebox("setValue", icsdateformat(new Date()));
    $("#toolbarSUDeliveryChallanDetail a").show();
    $("#btnSaveSUDeliveryChallan").show();
    $("#btnReceiveSUReturnChallan").hide();
    $("#btnApproveSUReturnChallan").hide();
    $("#txtRemarksSUReturnChallan").prop('disabled', false);

    $("#winSUReturnChallan").icsWindow("open", "Add Return Challan");
}

function RefreshSUDeliveryChallanControl(oSUDeliveryChallan) {
    _oSUDeliveryChallan = oSUDeliveryChallan;
    $("#txtChallanNoSUDeliveryChallan").val(oSUDeliveryChallan.ChallanNo);
    $("#txtDONoSUDeliveryChallan").val(oSUDeliveryChallan.DONo);
    $("#txtSearchByBuyerSUDeliveryChallan").val(oSUDeliveryChallan.BuyerName);
    $("#txtSearchByDeliveryToSUDeliveryChallan").val(oSUDeliveryChallan.DeliveryToName);
    $("#txtDriverNameSUDeliveryChallan").val(oSUDeliveryChallan.DriverName);
    $("#txtDriverContactNoSUDeliveryChallan").val(oSUDeliveryChallan.DriverContactNo);
    $("#txtGatePassNoSUDeliveryChallan").val(oSUDeliveryChallan.GatePassNo);
    $("#txtRemarksSUDeliveryChallan").val(oSUDeliveryChallan.Remarks);
    $("#txtDriverNameSUDeliveryChallan").val(oSUDeliveryChallan.DriverName);
    $("#txtStorePhoneNoSUDeliveryChallan").val(oSUDeliveryChallan.StorePhoneNo);
    $("#txtVehicleNoSUDeliveryChallan").val(oSUDeliveryChallan.VehicleNo);
    $("#cboBuyerSUDeliveryChallan").val(oSUDeliveryChallan.BuyerContractorType);
    $("#cboDeliveryToSUDeliveryChallan").val(oSUDeliveryChallan.DeliveryToContractorType);
    $("#cboGoodsCheckedBySUDeliveryChallan").val(oSUDeliveryChallan.CheckedBy);
    $("#cboFinishGoodsStoreSUDeliveryChallan").val(oSUDeliveryChallan.StoreID);
    $("#txtChallanDateSUDeliveryChallan").datebox("setValue", oSUDeliveryChallan.ChallanDateSt);
    
    DynamicRefreshList(oSUDeliveryChallan.SUDeliveryChallanDetails, "tblSUDeliveryChallanDetails");
    RefreshTotalSummeryDC();

    _nBuyerId = oSUDeliveryChallan.BuyerID;
    _nDeliverToId = oSUDeliveryChallan.DeliveryTo;

    if ($.trim($("#txtGatePassNoSUDeliveryChallan").val()) == "")
    {
        var sChallanNo = $("#txtChallanNoSUDeliveryChallan").val();
        $("#txtGatePassNoSUDeliveryChallan").val(sChallanNo.replace("DC","GP"));
    }

    var nLot = 0;
    var oSUDeliveryChallanDetails = oSUDeliveryChallan.SUDeliveryChallanDetails;
    if (oSUDeliveryChallanDetails.length > 0)
    {
        for (var i = 0; i < oSUDeliveryChallanDetails.length; i++) {
            if (oSUDeliveryChallanDetails[i].LotID > 0) {
                nLot = oSUDeliveryChallanDetails[i].LotID;
                $("#cboFinishGoodsStoreSUDeliveryChallan").prop("disabled", true);
                break;
            }
        }
    }
    else{
        $("#cboFinishGoodsStoreSUDeliveryChallan").prop("disabled", false);
    }

}

function ResetVariable() {
    _nBuyerId = 0;
    _nDeliverToId = 0;
}

function GetSUDeliveryChallanInformation(oSUDeliveryChallan) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oSUDeliveryChallan,
        ControllerName: "SUDeliveryChallan",
        ActionName: "GetSUDeliveryChallan",
        IsWinClose: false
    };
    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.SUDeliveryChallanID > 0) {
                RefreshSUDeliveryChallanControl(response.obj);
            }
            else {
                alert(response.obj.ErrorMessage);
            }
        }
        else {
            alert("No Information found.");
        }
    });
}

function RefreshSUDeliveryChallanLayout(buttonId) {
    if (buttonId == "btnEditSUDeliveryChallan") {
        $(".enableDisable").prop('disabled', true);
        $("#txtChallanDateSUDeliveryChallan").datebox({ disabled: true });
        $("#txtChallanDateSUDeliveryChallan").datebox("setValue", icsdateformat(new Date()));
        $("#toolbarSUDeliveryChallanDetail a").show();
        $("#btnSaveSUDeliveryChallan").show();
        $("#btnDisburseSUDeliveryChallan").hide();
        $("#btnApproveSUDeliveryChallan").hide();
        $("#txtRemarksSUDeliveryChallan,#txtVehicleNoSUDeliveryChallan").prop('disabled', false);
        $("#cboGoodsCheckedBySUDeliveryChallan,#cboFinishGoodsStoreSUDeliveryChallan").prop("disabled", false);
        $("#txtRemarksSUDeliveryChallan,#txtDriverNameSUDeliveryChallan,#txtDriverContactNoSUDeliveryChallan,#txtStorePhoneNoSUDeliveryChallan,#txtGatePassNoSUDeliveryChallan,#cboGoodsCheckedBySUDeliveryChallan,#cboFinishGoodsStoreSUDeliveryChallan,#txtVehicleNoSUDeliveryChallan").prop('disabled', false);
    }
    else if (buttonId == "btnViewSUDeliveryChallan") {
        $("#winSUDeliveryChallan").find("input, select").prop("disabled", true);
        $("#txtChallanDateSUDeliveryChallan").datebox({ disabled: true });
        $("#txtChallanDateSUDeliveryChallan").datebox("setValue", icsdateformat(new Date()));
        $("#toolbarSUDeliveryChallanDetail a").hide();
        $("#btnSaveSUDeliveryChallan,#btnDisburseSUDeliveryChallan,#btnApproveSUDeliveryChallan").hide();
    }
    else if (buttonId == "btnChallanDisburseSUDeliveryChallan") {
        $(".enableDisable").prop('disabled', true);
        $("#txtChallanDateSUDeliveryChallan").datebox({ disabled: true });
        $("#txtChallanDateSUDeliveryChallan").datebox("setValue", icsdateformat(new Date()));
        $("#toolbarSUDeliveryChallanDetail a").show();
        $("#btnDisburseSUDeliveryChallan").show();
        $("#btnSaveSUDeliveryChallan").hide();
        $("#btnApproveSUDeliveryChallan").hide();
        $("#txtRemarksSUDeliveryChallan,#txtVehicleNoSUDeliveryChallan").prop('disabled', false);
        $("#cboGoodsCheckedBySUDeliveryChallan,#cboFinishGoodsStoreSUDeliveryChallan").prop("disabled", false);
        $("#txtRemarksSUDeliveryChallan,#txtDriverNameSUDeliveryChallan,#txtDriverContactNoSUDeliveryChallan,#txtStorePhoneNoSUDeliveryChallan,#txtGatePassNoSUDeliveryChallan,#cboGoodsCheckedBySUDeliveryChallan,#cboFinishGoodsStoreSUDeliveryChallan,#txtVehicleNoSUDeliveryChallan").prop('disabled', false);
    }
    else if (buttonId == "btnChallanApproveSUDeliveryChallan") {
        $(".enableDisable").prop('disabled', true);
        $("#txtChallanDateSUDeliveryChallan").datebox({ disabled: true });
        $("#txtChallanDateSUDeliveryChallan").datebox("setValue", icsdateformat(new Date()));
        $("#toolbarSUDeliveryChallanDetail a").show();
        $("#btnApproveSUDeliveryChallan").show();
        $("#btnSaveSUDeliveryChallan").hide();
        $("#btnDisburseSUDeliveryChallan").hide();
        $("#txtRemarksSUDeliveryChallan,#txtVehicleNoSUDeliveryChallan").prop('disabled', false);
        $("#cboGoodsCheckedBySUDeliveryChallan,#cboFinishGoodsStoreSUDeliveryChallan").prop("disabled", false);
        $("#txtRemarksSUDeliveryChallan,#txtDriverNameSUDeliveryChallan,#txtDriverContactNoSUDeliveryChallan,#txtStorePhoneNoSUDeliveryChallan,#txtGatePassNoSUDeliveryChallan,#cboGoodsCheckedBySUDeliveryChallan,#cboFinishGoodsStoreSUDeliveryChallan,#txtVehicleNoSUDeliveryChallan").prop('disabled', false);
    }
    $("#cboBuyerSUDeliveryChallan,#cboDeliveryToSUDeliveryChallan").hide();
    $("#txtSearchByBuyerSUDeliveryChallan,#txtSearchByDeliveryToSUDeliveryChallan").css("width", "98%");
    $("#txtSearchByBuyerSUDeliveryChallan,#txtSearchByDeliveryToSUDeliveryChallan").addClass("fontColorOfPickItem");
    $("#txtGatePassNoSUDeliveryChallan").prop("disabled",true);
}

function isValidSUDeliveryChallan() {
    _oSUDeliveryChallans = $("#tblSUDeliveryChallans").datagrid("getRows");
    if (_oSUDeliveryChallans.length <= 0) {
        alert("Empty List");
        return false;
    }
    else {
        for (var i = 0; i < _oSUDeliveryChallans.length; i++) {
            _oSUDeliveryChallans[i].ChallanDate = _oSUDeliveryChallans[i].ChallanDateSt;
            _oSUDeliveryChallans[i].DODate = _oSUDeliveryChallans[i].DODateSt;
            _oSUDeliveryChallans[i].IsUnitPriceAndAmount = $("#chkPrintWithUnitPriceAndAmount").is(':checked')
        }
        $("#txtSUDeliveryChallanCollectionList").val(JSON.stringify(_oSUDeliveryChallans));
    }
}

function isValidDeliveryComplete() {
    _oSUDeliveryChallans = $("#tblSUDeliveryChallans").datagrid("getRows");
    if ((_oSUDeliveryChallans.length - 1) <= 0) {
        alert("Empty List");
        return false;
    }
    else {
        for (var i = 0; i < (_oSUDeliveryChallans.length - 1); i++) {
            _oSUDeliveryChallans[i].ChallanDate = _oSUDeliveryChallans[i].ChallanDateSt;
            _oSUDeliveryChallans[i].DODate = _oSUDeliveryChallans[i].DODateSt;
        }
        $("#txtDeliveryComplete").val(JSON.stringify(_oSUDeliveryChallans));
    }
}


function GetPreviousRecord1() {
    var oSUDeliveryChallan = {};
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oSUDeliveryChallan,
        ControllerName: "SUDeliveryChallan",
        ActionName: "GetLastSUDeliveryChallan",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            var oSUDC = response.obj;
            if (oSUDC.SUDeliveryChallanID > 0) {
                if ($.trim($("#txtGatePassNoSUDeliveryChallan").val()) == "")
                {
                    var sDONo = $.trim($("#txtChallanNoSUDeliveryChallan").val());
                    $("#txtGatePassNoSUDeliveryChallan").val(sDONo.replace("DC", "GP"));
                }
            }
        }
    });
}