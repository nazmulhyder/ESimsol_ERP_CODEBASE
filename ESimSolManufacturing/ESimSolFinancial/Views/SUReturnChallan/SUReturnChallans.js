var _isAddDCD = true;
function InitializeSUReturnChallansEvents() {
    DynamicRefreshList(_oSUReturnChallans, "tblSURCs");

    $("#btnAdvSearchSURCs").click(function () {
        ResetAdvSearchWindow();
        ResetAllTables("tblSUReturnChallanAdvSearch");
        $("#winAdvSearchSUReturnChallan").icsWindow("open", "Advance Search");
    });

    $("#btnPendingSURCs").click(function () {
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: {},
            ControllerName: "SUReturnChallan",
            ActionName: "GetsPendingChallan",
            IsWinClose: false
        }
        $.icsDataGets(obj, function (response) {

            if (response.status) {
                if (response.objs.length > 0) {
                    _oSUReturnChallans = [];
                    _oSUReturnChallans = response.objs;
                    DynamicRefreshList(_oSUReturnChallans, 'tblSURCs');
                }
                else {
                    alert('Sorry, No data found.');
                    DynamicRefreshList([], 'tblSURCs');
                }
            }
        });
    });

    $("#btnEditSURCs").click(function () {
        var oSUReturnChallan = $("#tblSURCs").datagrid("getSelected");
        if (oSUReturnChallan == null || oSUReturnChallan.SUReturnChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (oSUReturnChallan.ReturnStatusInInt > 0) {
            alert("Sorry, Only Initialize Return Challan can edit.");
            return false;
        }
        _isAddDCD = false;
        GetSUReturnChallanInformation(oSUReturnChallan);
        $("#winSUReturnChallan").icsWindow("open", "Edit Return Challan");
        RefreshSUReturnChallanLayout("btnEditSURCs");
        endEditing1();
    });

    $("#btnViewSURCs").click(function () {
        var oSUReturnChallan = $("#tblSURCs").datagrid("getSelected");
        if (oSUReturnChallan == null || oSUReturnChallan.SUReturnChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        _isAddDCD = false;
        GetSUReturnChallanInformation(oSUReturnChallan);
        $("#winSUReturnChallan").icsWindow("open", "View Delivery Challan");
        RefreshSUReturnChallanLayout("btnViewSURCs");
    });

    $("#btnDeleteSURCs").click(function () {
        var oSUReturnChallan = $("#tblSURCs").datagrid("getSelected");
        if (oSUReturnChallan == null || oSUReturnChallan.SUReturnChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (parseInt(oSUReturnChallan.ChallanStatusInInt) > 0) {
            alert("Only Initialize Delivery Challan can delete.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oSUReturnChallan,
            ObjectId: oSUReturnChallan.SUReturnChallanID,
            ControllerName: "SUReturnChallan",
            ActionName: "DeleteSUReturnChallan",
            TableId: "tblSURCs",
            IsWinClose: true
        };
        $.icsDelete(obj);
    });

    $("#btnApproveSURCs").click(function () {
        var oSUReturnChallan = $("#tblSURCs").datagrid("getSelected");
        if (oSUReturnChallan == null || oSUReturnChallan.SUReturnChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (oSUReturnChallan.ReturnStatusInInt != 0) {
            alert("Sorry, Only Initialize Delivery Challan can approve.");
            return false;
        }
        if (oSUReturnChallan.ReturnStatusInInt == 1) {
            alert("Sorry, Already approved.");
            return false;
        }

        _isAddDCD = false;
        GetSUReturnChallanInformation(oSUReturnChallan);
        $("#winSUReturnChallan").icsWindow("open", "Approve Delivery Challan");
        RefreshSUReturnChallanLayout("btnApproveSURCs");
        endEditing1();


    });

    $("#btnReceiveSURCs").click(function () {
        var oSUReturnChallan = $("#tblSURCs").datagrid("getSelected");
        if (oSUReturnChallan == null || oSUReturnChallan.SUReturnChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (oSUReturnChallan.ReturnStatusInInt != 1) {
            alert("Sorry, Only Approve Delivery Challan can Disburse.");
            return false;
        }
        if (oSUReturnChallan.ReturnStatusInInt == 2) {
            alert("Sorry, Already Disbursed.");
            return false;
        }
        _isAddDCD = false;
        GetSUReturnChallanInformation(oSUReturnChallan);
        $("#winSUReturnChallan").icsWindow("open", "Disburse Delivery Challan");
        RefreshSUReturnChallanLayout("btnReceiveSURCs");
        endEditing1();
    });

    $("#txtSearchByReturnNoSURCs").keyup(function (e) {
        if (e.keyCode == 13) {
            var oSUReturnChallan = {
                ChallanNo: $("#txtSearchByReturnNoSURCs").val()
            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/SUReturnChallan/GetsBySearchKey",
                traditional: true,
                data: JSON.stringify(oSUReturnChallan),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var oSUReturnChallans = jQuery.parseJSON(data);
                    if (oSUReturnChallans != null) {
                        if (oSUReturnChallans.length > 0) {
                            DynamicRefreshList(oSUReturnChallans, "tblSURCs");
                        }
                        else {
                            DynamicRefreshList([], "tblSURCs");
                        }
                    } else {
                        DynamicRefreshList([], "tblSURCs");
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
        else {
            $(this).icsSearchByText({
                Event: e,
                SearchProperty: "ReturnNo",
                GlobalObjectList: _oSUReturnChallans,
                TableId: "tblSURCs"
            });
        }
    });

    $("#btnPreviewSURCs").click(function () {
        var oSUReturnChallan = $("#tblSURCs").datagrid("getSelected");
        if (oSUReturnChallan == null || oSUReturnChallan.SUReturnChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        var nts = ((new Date()).getTime()) / 1000;
        window.open(_sBaseAddress + '/SUReturnChallan/PreviewSUReturnChallan?&id=' + oSUReturnChallan.SUReturnChallanID, "_blank");
    });


    $("#btnWaitingApprovalSURCs").click(function () {
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: {},
            ControllerName: "SUReturnChallan",
            ActionName: "GetsWaitingApprovalChallan",
            IsWinClose: false
        }
        $.icsDataGets(obj, function (response) {

            if (response.status) {
                if (response.objs.length > 0) {
                    _oSUReturnChallans = [];
                    _oSUReturnChallans = response.objs;
                    DynamicRefreshList(_oSUReturnChallans, 'tblSURCs');
                }
                else {
                    alert('Sorry, No data found.');
                    DynamicRefreshList([], 'tblSURCs');
                }
            }
        });
    });

    $("#btnSearchByDateSURCs").click(function () {
        var sReturnDate = $("#txtReturnDateSURCs").datebox('getValue');

        var sParams = "~~~~~" + true + "~" + sReturnDate + "~" + sReturnDate;

        var oSURC = {
            ErrorMessage: sParams
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUReturnChallan/AdvSearch",
            traditional: true,
            data: JSON.stringify(oSURC),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSURCs = jQuery.parseJSON(data);
                if (oSURCs != null) {
                    if (oSURCs.length > 0) {
                        DynamicRefreshList(oSURCs, "tblSURCs");
                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSURCs");
                    }

                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblSURCs");
                }
            }
        });
    });

    $("#btnPrintSURCs").click(function () {
        var oSUReturnChallans = $("#tblSURCs").datagrid("getRows");
        for (var i = 0; i < oSUReturnChallans.length; i++) {
            oSUReturnChallans[i].ReturnDate = oSUReturnChallans[i].ReturnDateSt;
            oSUReturnChallans[i].ChallanDate = oSUReturnChallans[i].ChallanDateSt;
        }

        $('#txtSURCsCollectionList').val(JSON.stringify(oSUReturnChallans));
        if (!oSUReturnChallans.length || !$.trim($('#txtSURCsCollectionList').val()).length) return false;
    });

    $("#txtReturnDateSURCs").datebox('setValue', icsdateformat(new Date()));
}

function RefreshSUReturnChallanControl(oSUReturnChallan) {
    _oSUReturnChallan = oSUReturnChallan;
    $("#txtReturnNoSUReturnChallan").val(oSUReturnChallan.ReturnNo);
    $("#txtDONoSUReturnChallan").val(oSUReturnChallan.DONo);
    $("#txtChallanNoSUReturnChallan").val(oSUReturnChallan.ChallanNo);
    $("#txtSearchByBuyerSUReturnChallan").val(oSUReturnChallan.BuyerName);
    
    $("#txtPartyChallanNoSUReturnChallan").val(oSUReturnChallan.PartyChallanNo);
    $("#txtRemarksSUReturnChallan").val(oSUReturnChallan.Remarks);
    
    $("#cboBuyerSUReturnChallan").val(oSUReturnChallan.BuyerContractorType);
    
    $("#cboFinishGoodsStoreSUReturnChallan").val(oSUReturnChallan.StoreID);
    $("#txtReturnDateSUReturnChallan").datebox("setValue", oSUReturnChallan.ReturnDateSt);
    DynamicRefreshList(oSUReturnChallan.SUReturnChallanDetails, "tblSUReturnChallanDetails");
    RefreshTotalSummerySURCD();

    _nBuyerId = oSUReturnChallan.BuyerID;
    
}

function ResetVariable() {
    _nBuyerId = 0;
    _nDeliverToId = 0;
}

function GetSUReturnChallanInformation(oSUReturnChallan) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oSUReturnChallan,
        ControllerName: "SUReturnChallan",
        ActionName: "GetSUReturnChallan",
        IsWinClose: false
    };
    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.SUReturnChallanID > 0) {
                RefreshSUReturnChallanControl(response.obj);
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

function RefreshSUReturnChallanLayout(buttonId) {
    $(".enableDisable").prop('disabled', true);
    $("#txtReturnDateSUReturnChallan").datebox({ disabled: false });
    $("#txtReturnDateSUReturnChallan").datebox("setValue", icsdateformat(new Date()));

    if (buttonId == "btnEditSURCs") {
        $("#toolbarSUDeliveryChallanDetail a").show();
        $("#btnSaveSUReturnChallan").show();
        $("#btnReceiveSUReturnChallan").hide();
        $("#btnApproveSUReturnChallan").hide();
        $("#txtRemarksSUReturnChallan,#txtPartyChallanNoSUReturnChallan").prop('disabled', false);
    }
    else if (buttonId == "btnViewSURCs") {
        $("#toolbarSUDeliveryChallanDetail a").hide();
        $("#btnSaveSUReturnChallan").hide();
        $("#btnReceiveSUReturnChallan").hide();
        $("#btnApproveSUReturnChallan").hide();
        $("#txtRemarksSUReturnChallan,#txtPartyChallanNoSUReturnChallan").prop('disabled', true);
    }
    else if (buttonId == "btnReceiveSURCs") {
        $("#toolbarSUDeliveryChallanDetail a").show();
        $("#btnSaveSUReturnChallan").hide();
        $("#btnReceiveSUReturnChallan").show();
        $("#btnApproveSUReturnChallan").hide();
        $("#txtRemarksSUReturnChallan,#txtPartyChallanNoSUReturnChallan").prop('disabled', false);
    }
    else if (buttonId == "btnApproveSURCs") {
        $("#toolbarSUDeliveryChallanDetail a").show();
        $("#btnSaveSUReturnChallan").hide();
        $("#btnReceiveSUReturnChallan").hide();
        $("#btnApproveSUReturnChallan").show();
        $("#txtRemarksSUReturnChallan,#txtPartyChallanNoSUReturnChallan").prop('disabled', false);
    }
    $("#cboBuyerSUReturnChallan,#cboDeliveryToSUReturnChallan").hide();
    $("#txtSearchByBuyerSUReturnChallan,#txtSearchByDeliveryToSUReturnChallan").css("width", "98%");
    $("#txtSearchByBuyerSUReturnChallan,#txtSearchByDeliveryToSUReturnChallan").addClass("fontColorOfPickItem");
}

function isValidSUReturnChallan() {
    if (_oSUReturnChallans.length <= 0) {
        alert("Empty List");
        return false;
    }
    else {
        for (var i = 0; i < _oSUReturnChallans.length; i++) {
            _oSUReturnChallans[i].ReturnDate = _oSUReturnChallans[i].ReturnDateSt;
            _oSUReturnChallans[i].ChallanDate = _oSUReturnChallans[i].ChallanDateSt;
        }
        $("#txtSUReturnChallanCollectionList").val(JSON.stringify(_oSUReturnChallans));
    }
}