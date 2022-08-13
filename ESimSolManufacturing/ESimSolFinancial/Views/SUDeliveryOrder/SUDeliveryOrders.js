var _isSave = true;
var _oCheckSUDODs = [];
var _oExportPISUDO = null;
var _isValidAmount = true;
function InitializeSUDeliveryOrdersEvents() {
    $('#tblSUDeliveryOrders').datagrid({ onSelect: function (rowIndex, rowData) { OperationPerforms(rowIndex, rowData); } });
    OperationPerforms(0, null);

    DynamicRefreshList(_oSUDeliveryOrders, "tblSUDeliveryOrders");
    
    $("#btnAdvSearchSUDeliveryOrder").click(function () {
        ResetAdvSearchWindow();
        $("#winAdvSearchSUDeliveryOrder").icsWindow("open", "Advance Search");
    });

    $("#txtSearchBySUDeliveryOrder").keyup(function (e) {
        if (e.keyCode == 13) {
            var oSUDeliveryOrder = {
                DONo: $("#txtSearchBySUDeliveryOrder").val()
            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/SUDeliveryOrder/GetsBySearchKey",
                traditional: true,
                data: JSON.stringify(oSUDeliveryOrder),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var oSUDeliveryOrders = jQuery.parseJSON(data);
                    if (oSUDeliveryOrders != null) {
                        if (oSUDeliveryOrders.length > 0) {
                            DynamicRefreshList(oSUDeliveryOrders, "tblSUDeliveryOrders");
                        }
                        else {
                            DynamicRefreshList([], "tblSUDeliveryOrders");
                        }
                    } else {
                        DynamicRefreshList([], "tblSUDeliveryOrders");
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
        else {
            var txtSearchBy = $("#txtSearchBySUDeliveryOrder").val();
            var oCurrentLists = $("#tblSUDeliveryOrders").datagrid("getRows");
            var sTempSearchString = "";
            var oSearchedLists = [];
            if (e.keyCode == 8) {
                oCurrentLists = _oSUDeliveryOrders;
            }
            for (var i = 0; i < oCurrentLists.length; i++) {
                sTempSearchString = oCurrentLists[i].DONo;
                var n = sTempSearchString.toUpperCase().indexOf(txtSearchBy.toUpperCase());
                if (n != -1) {
                    oSearchedLists.push(oCurrentLists[i]);
                }
                else {
                    oSearchedLists.push(oCurrentLists[i]);
                }
            }
            DynamicRefreshList(oSearchedLists, "tblSUDeliveryOrders");
        }
    });


    $("#btnDeleteSUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || oSUDeliveryOrder.SUDeliveryOrderID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (parseInt(oSUDeliveryOrder.DOStatusInInt) > 0) {
            alert("Please select an Initialize item from list!");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oSUDeliveryOrder,
            ObjectId: oSUDeliveryOrder.SUDeliveryOrderID,
            ControllerName: "SUDeliveryOrder",
            ActionName: "DeleteSUDeliveryOrder",
            TableId: "tblSUDeliveryOrders",
            IsWinClose: true
        };
        $.icsDelete(obj);
    });

    $("#btnReqForApproveSUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        if (parseInt(oSUDeliveryOrder.DOStatusInInt) != 0) { alert("Please select an initialize item!"); return; }
        var nRowIndex = $('#tblSUDeliveryOrders').datagrid('getRowIndex', oSUDeliveryOrder);
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryOrder/GetSUDeliveryOrder",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrder),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryOrder = jQuery.parseJSON(data);
                _oCheckSUDODs = oSUDeliveryOrder.SUDeliveryOrderDetails;
                if (_oCheckSUDODs.length == 0) {
                    alert("Sorry, Delivery Order must have minimum one Delivery Order Detail.");
                    _oCheckSUDODs = [];
                    return false;
                }
                else if (_oCheckSUDODs.length > 0) {
                    debugger;
                    if (parseInt(oSUDeliveryOrder.DOTypeInInt) != 6) //Sample Yarn Requisition
                    {
                        for (var i = 0; i < _oCheckSUDODs.length; i++) {
                            if (_oCheckSUDODs[i].Amount == 0) {
                                alert("Sorry, Not All Delivery Order Detail of this Delivery Order have Qty and Unit Price.");
                                _oCheckSUDODs = [];
                                return false;
                            }
                        }
                    }
                   
                    if (!confirm("Confirm to Request for Approved?")) return false;
                    var sSuccessMessage = "Successfully Request for Approved";
                    oSUDeliveryOrder.DOStatusInInt = 1;
                    UpdateDOStatus(oSUDeliveryOrder, sSuccessMessage, nRowIndex);
                    _oCheckSUDODs = [];
                }
            }
        });
    });

    $("#btnApproveSUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        if (parseInt(oSUDeliveryOrder.DOStatusInInt) != 1) { alert("Please select an Req for Approved item!"); return; }
        if (!confirm("Confirm to Approved?")) return false;
        var nRowIndex = $('#tblSUDeliveryOrders').datagrid('getRowIndex', oSUDeliveryOrder);
        var sSuccessMessage = "Successfully Approved";
        oSUDeliveryOrder.DOStatusInInt = 3;
        UpdateDOStatus(oSUDeliveryOrder, sSuccessMessage, nRowIndex);
    });

    $("#btnUndoApproveSUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        if (parseInt(oSUDeliveryOrder.DOStatusInInt) != 3) { alert("Please select an Approved item!"); return; }
        if (!confirm("Confirm to Undo Approved?")) return false;
        var nRowIndex = $('#tblSUDeliveryOrders').datagrid('getRowIndex', oSUDeliveryOrder);
        var sSuccessMessage = "Successfully Undo Approved";
        oSUDeliveryOrder.DOStatusInInt = 4;
        UpdateDOStatus(oSUDeliveryOrder, sSuccessMessage, nRowIndex);
    });

    $("#btnReqForReviseSUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        if (parseInt(oSUDeliveryOrder.DOStatusInInt) == 0 || parseInt(oSUDeliveryOrder.DOStatusInInt) == 1 || parseInt(oSUDeliveryOrder.DOStatusInInt) == 6 || parseInt(oSUDeliveryOrder.DOStatusInInt) == 7 || parseInt(oSUDeliveryOrder.DOStatusInInt) == 8) {
            alert("In this stage DO Can't Revise!"); return;
        }
        if (!confirm("Confirm to Req for Revise?")) return false;
        var nRowIndex = $('#tblSUDeliveryOrders').datagrid('getRowIndex', oSUDeliveryOrder);
        var sSuccessMessage = "Successfully Req for Revise";
        oSUDeliveryOrder.DOStatusInInt = 7;
        UpdateDOStatus(oSUDeliveryOrder, sSuccessMessage, nRowIndex);
    });

    //$("#btnAcceptReviseSUDeliveryOrder").click(function () {
    //    var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
    //    if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) {
    //        alert("Please select an item from list!"); return;
    //    }
    //    else if (parseInt(oSUDeliveryOrder.DOStatusInInt) == 8) {
    //        alert("Sorry, This Delivery Order already closed.");
    //        return false;
    //    }
    //    else if (parseInt(oSUDeliveryOrder.DOStatusInInt) != 7) {
    //        alert("Sorry, Delivery Order Status must be Request For Revise for this action.");
    //        return false;
    //    }
    //    $("#winSUDeliveryOrder").icsWindow('open', "Accept Revise Delivery Order(DO)");
    //    $("#txtProductNameSUDeliveryOrderDetail").val("");
    //    $("#txtProductNameSUDeliveryOrderDetail").removeClass("errorFieldBorder");
    //    $("#txtProductNameSUDeliveryOrderDetail").removeClass("fontColorOfPickItem");
    //    _isSave = false;
    //    GetSUDeliveryOrderInformation(oSUDeliveryOrder);
    //    RefreshSUDeliveryOrderLayout("btnEditSUDeliveryOrder");
    //    $("#btnSaveSUDeliveryOrder").hide();
    //    $("#btnSaveAcceptReviseSUDeliveryOrder").show();
    //});

    SUDOReviseHistory();

    $("#btnCancelSUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        if (parseInt(oSUDeliveryOrder.DOStatusInInt) == 5 || parseInt(oSUDeliveryOrder.DOStatusInInt) == 6 || parseInt(oSUDeliveryOrder.DOStatusInInt) == 7) { alert("In this stage Delivery Order Can't Cancle!"); return; }
        if (!confirm("Confirm to Cancel Delivery Order?")) return false;
        var nRowIndex = $('#tblSUDeliveryOrders').datagrid('getRowIndex', oSUDeliveryOrder);
        var sSuccessMessage = "Successfully Delivery Order Cancel";
        oSUDeliveryOrder.DOStatusInInt = 8;
        UpdateDOStatus(oSUDeliveryOrder, sSuccessMessage, nRowIndex);
    });

    $("#btnUndoReqSUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        if (parseInt(oSUDeliveryOrder.DOStatusInInt) != 1) { alert("Please select an Req for Approved item!"); return; }
        if (!confirm("Confirm to Undo Request?")) return false;
        var nRowIndex = $('#tblSUDeliveryOrders').datagrid('getRowIndex', oSUDeliveryOrder);
        var sSuccessMessage = "Successfully Undo Request";
        oSUDeliveryOrder.DOStatusInInt = 2;
        UpdateDOStatus(oSUDeliveryOrder, sSuccessMessage, nRowIndex);
    });

    //$("#btnPreviewSUDeliveryOrder").click(function () {
    //    var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
    //    if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
    //    var nts = ((new Date()).getTime()) / 1000;
    //    window.open(_sBaseAddress + '/SUDeliveryOrder/PreviewSUDeliveryOrder?nSUDeliveryOrderID=' + oSUDeliveryOrder.SUDeliveryOrderID + "&nts=" + nts, "_blank");
    //});

    $("#btnPreviewSUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        BtnPrintBtnClick(2);
        return false;
    });

    QtyFromatPopUpMenuLoad(2);
    $("#btnOkPrintFormat2").click(function () {
        $("#winPrintFormat2").icsWindow("close");
        var bPrintFormat = true;
        if ($("#chkInLBS2").is(':checked')) {
            bPrintFormat = false;
        }
        var bIsInText = true;
        if ($("#chkPrintTitleInImg2").is(':checked')) {
            bIsInText = false;
        }
        var nts = ((new Date()).getTime()) / 1000;
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        window.open(_sBaseAddress + '/SUDeliveryOrder/PreviewSUDeliveryOrder?nSUDeliveryOrderID=' + oSUDeliveryOrder.SUDeliveryOrderID + "&bPrintFormat=" + bPrintFormat + "&bIsInText=" + bIsInText + "&nts=" + nts, "_blank");
    });

    $("#btnWaitingApprovalSUDeliveryOrders").click(function () {
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: {},
            ControllerName: "SUDeliveryOrder",
            ActionName: "GetsWaitingApprovalSUDP",
            IsWinClose: false
        }
        $.icsDataGets(obj, function (response) {

            if (response.status) {
                if (response.objs.length > 0) {
                    _oSUDeliveryChallans = [];
                    _oSUDeliveryChallans = response.objs;
                    DynamicRefreshList(_oSUDeliveryChallans, 'tblSUDeliveryOrders');
                }
                else {
                    alert('Sorry, No data found.');
                    DynamicRefreshList([], 'tblSUDeliveryOrders');
                }
            }
        });
    });

    $("#btnSearchByDateSUDeliveryOrders").click(function () {
        var oSUDeliveryOrder = {
            DODate: $("#txtDODateSUDeliveryOrders").datebox('getValue')
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryOrder/GetDOByDODate",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrder),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryOrders = jQuery.parseJSON(data);
                if (oSUDeliveryOrders != null) {
                    if (oSUDeliveryOrders.length > 0) {
                        DynamicRefreshList(oSUDeliveryOrders, "tblSUDeliveryOrders");
                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSUDeliveryOrders");
                    }

                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblSUDeliveryOrders");
                }
            }
        });
    });

    $("#txtDODateSUDeliveryOrders").datebox('setValue', icsdateformat(new Date()));

    $("#btnPrintSUDeliveryOrder").click(function () {
        BtnPrintBtnClick(1);
        return false;
    });

    QtyFromatPopUpMenuLoad(1);
    $("#btnOkPrintFormat1").click(function () {
        $("#winPrintFormat1").icsWindow("close");
        var bPrintFormat = true;
        if ($("#chkInLBS1").is(':checked')) {
            bPrintFormat = false;
        }

        _oSUDeliveryOrders = $('#tblSUDeliveryOrders').datagrid('getRows');
        if (_oSUDeliveryOrders.length == 0) {
            alert("No List Found.");
            return false;
        }
   
        if (_oSUDeliveryOrders.length <= 0) {
            alert("Empty List");
            return false;
        }
        else {
            var sSUDOIDs = "";
            for (var i = 0; i < _oSUDeliveryOrders.length; i++) {

                for (var i = 0; i < _oSUDeliveryOrders.length; i++) {
                    sSUDOIDs = _oSUDeliveryOrders[i].SUDeliveryOrderID + "," + sSUDOIDs;
                }
                sSUDOIDs = sSUDOIDs.substring(0, sSUDOIDs.length - 1);
            }
            $("#txtSUDeliveryOrderCollectionList").val(sSUDOIDs + "~" + bPrintFormat);
        }
    });
}

function SUDOReviseHistory() {
    $("#btnReviseHistorySUDeliveryOrder").click(function () {
        var oSUDeliveryOrder = $("#tblSUDeliveryOrders").datagrid("getSelected");
        if (oSUDeliveryOrder == null || oSUDeliveryOrder.SUDeliveryOrderID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        $("#winReviseHistory").icsWindow("open", "Revise History of DO No : " + oSUDeliveryOrder.DONo);
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryOrder/GetReviseHistory",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrder),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryOrders = jQuery.parseJSON(data);
                if (oSUDeliveryOrders != null) {
                    if (oSUDeliveryOrders.length > 0) {
                        
                        DynamicRefreshList(oSUDeliveryOrders, "tblReviseHistory");
                    }
                    else {
                        DynamicRefreshList([], "tblReviseHistory");
                    }
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnCloseReviseHistory").click(function () {
        $("#winReviseHistory").icsWindow("close");
    });

    $("#btnViewReviseHistory").click(function () {
        var oSUDeliveryOrderLog = $("#tblReviseHistory").datagrid("getSelected");
        if (oSUDeliveryOrderLog == null || oSUDeliveryOrderLog.SUDeliveryOrderLogID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        $("#winSUDeliveryOrder").icsWindow("open", "Revise History Detail of PI No : " + oSUDeliveryOrderLog.DONo);
        $("#btnSaveAcceptReviseSUDeliveryOrder,#btnSaveSUDeliveryOrder").hide();
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryOrder/GetDOAllLogs",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrderLog),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryOrderLog = jQuery.parseJSON(data);
                if (oSUDeliveryOrderLog.SUDeliveryOrderLogID > 0) {
                    RefreshSUDeliveryOrderLayout("btnViewSUDeliveryOrder");
                    RefreshSUDeliveryOrderControl(oSUDeliveryOrderLog);
                    _isSave = false;
                    DynamicRefreshList(oSUDeliveryOrderLog.SUDeliveryOrderDetails, "tblSUDeliveryOrderDetails");
                }
                else {
                    alert("No data found.");
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#txtSearchBySUDeliveryOrder").keyup(function (e) {
        if (e.keyCode == 13) {
            var oSUDeliveryOrder = {
                DONo: $("#txtSearchBySUDeliveryOrder").val()
            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/SUDeliveryOrder/GetsBySearchKey",
                traditional: true,
                data: JSON.stringify(oSUDeliveryOrder),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var oSUDeliveryOrders = jQuery.parseJSON(data);
                    if (oSUDeliveryOrders != null) {
                        if (oSUDeliveryOrders.length > 0) {
                            DynamicRefreshList(oSUDeliveryOrders, "tblSUDeliveryOrders");
                        }
                        else {
                            DynamicRefreshList([], "tblSUDeliveryOrders");
                        }
                    } else {
                        DynamicRefreshList([], "tblSUDeliveryOrders");
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
                SearchProperty: "DONo",
                GlobalObjectList: _oSUDeliveryOrders,
                TableId: "tblSUDeliveryOrders"
            });
        }
    });
}

function UpdateDOStatus(oSUDeliveryOrder, sSuccessMessage, nRowIndex) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SUDeliveryOrder/UpdateDOStatus",
        traditional: true,
        data: JSON.stringify(oSUDeliveryOrder),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            
            var oSUDeliveryOrder = jQuery.parseJSON(data);
            if (oSUDeliveryOrder.ErrorMessage == "" || oSUDeliveryOrder.ErrorMessage == null) {
                alert(sSuccessMessage);
                $('#tblSUDeliveryOrders').datagrid('updateRow', { index: nRowIndex, row: oSUDeliveryOrder });
                OperationPerforms(nRowIndex, oSUDeliveryOrder);
            } else {
                alert(oSUDeliveryOrder.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function RefreshSUDeliveryOrderLayout(buttonId) {
    if (buttonId === "btnViewSUDeliveryOrder") {
        $("#winSUDeliveryOrder input").prop("disabled", true);
        $("#winSUDeliveryOrder select").prop("disabled", true);
        $("#txtSearchByBuyerSUDeliveryOrder,#cboDOTypeSUDeliveryOrder").prop("disabled", true);
        $("#btnSaveSUDeliveryOrder").hide();
        $("#btnRefreshSUDeliveryOrderDetail,#btnProductRemoveSUDeliveryOrderDetail, #btnPIProductsSUDeliveryOrderDetail, #btnProductAddSUDeliveryOrderDetail").hide();
        $("#cboBuyerSUDeliveryOrder,#cboDeliveryToSUDeliveryOrder,#btnClrBuyerSUDeliveryOrder,#btnPickBuyerSUDeliveryOrder,#btnClrDeliveryToSUDeliveryOrder,#btnPickDeliveryToSUDeliveryOrder").hide();
        $("#txtSearchByBuyerSUDeliveryOrder,#txtSearchByDeliveryToSUDeliveryOrder").css("width", "98%");
    }
    else {
        $("#winSUDeliveryOrder input").not("#txtDONoSUDeliveryOrder, #txtPINoSUDeliveryOrder").prop("disabled", false);
        $("#winSUDeliveryOrder select").prop("disabled", false);
        $("#btnSaveSUDeliveryOrder").show();
        $("#btnRefreshSUDeliveryOrderDetail, #btnProductRemoveSUDeliveryOrderDetail, #btnPIProductsSUDeliveryOrderDetail, #btnProductAddSUDeliveryOrderDetail").show();
        $("#cboBuyerSUDeliveryOrder,#cboDeliveryToSUDeliveryOrder,#btnClrBuyerSUDeliveryOrder,#btnPickBuyerSUDeliveryOrder,#btnClrDeliveryToSUDeliveryOrder,#btnPickDeliveryToSUDeliveryOrder").show();
        $("#txtSearchByBuyerSUDeliveryOrder,#txtSearchByDeliveryToSUDeliveryOrder").css('width', '');
    }

    if (buttonId === "btnEditSUDeliveryOrder" || buttonId === "btnViewSUDeliveryOrder") {
        $("#btnPINoSUDeliveryOrder,#btnClearPINoSUDeliveryOrder").prop("disabled", true);
    } else {
        $("#btnPINoSUDeliveryOrder,#btnClearPINoSUDeliveryOrder").prop("disabled", false);
    }

    if (parseInt($('#cboDOTypeSUDeliveryOrder').val()) === 6) {
        $('#TotalSummerySUDOD').hide();
        $('#TotalSummerySUDODForSampleDO').show();
    }
    else {
        $('#TotalSummerySUDOD').show();
        $('#TotalSummerySUDODForSampleDO').hide();
    }

    $("#cboContactPersonnelSUDeliveryOrder").empty();
    $("#txtContactInfoSUDeliveryOrder").prop("disabled", true);
    $("#txtContactInfoSUDeliveryOrder").val();
}

function RefreshSUDeliveryOrderControl(oSUDeliveryOrder) {
    _oSUDeliveryOrder = oSUDeliveryOrder;
    GenerateTableColumnsSUDOD();
    if (_oSUDeliveryOrder.DOTypeInInt === 6) { GenerateTableColumnsSUDODForSampleDO(); }
    
    DynamicRefreshList(_oSUDeliveryOrder.SUDeliveryOrderDetails, "tblSUDeliveryOrderDetails");
    RefreshTotalSummery();
    if (_oSUDeliveryOrder.DOTypeInInt === 6) { RefreshTotalSummeryForSampleDO(); }

    $("#txtDONoSUDeliveryOrder").val(_oSUDeliveryOrder.DONo);

    $("#cboDOTypeSUDeliveryOrder").val(_oSUDeliveryOrder.DOTypeInInt);
    if (parseInt($('#cboDOTypeSUDeliveryOrder').val()) === 6) {
        $('#TotalSummerySUDOD').hide();
        $('#TotalSummerySUDODForSampleDO').show();
    }
    else {
        $('#TotalSummerySUDOD').show();
        $('#TotalSummerySUDODForSampleDO').hide();
    }

    _nBuyerID = _oSUDeliveryOrder.BuyerID;
    $("#txtSearchByBuyerSUDeliveryOrder").val(_oSUDeliveryOrder.BuyerName);

   
    _nDeliveryTo = _oSUDeliveryOrder.DeliveryTo;
    $("#txtSearchByDeliveryToSUDeliveryOrder").val(_oSUDeliveryOrder.DeliveredToName);
    $("#txtDeliverPointSUDeliveryOrder").val(_oSUDeliveryOrder.DeliveryPoint);
    $("#txtRemarksSUDeliveryOrder").val(_oSUDeliveryOrder.Remarks);
    $('#txtDODateSUDeliveryOrder').datebox('setValue', _oSUDeliveryOrder.DODateSt);
    $('#txtDeliveryDateSUDeliveryOrder').datebox('setValue', _oSUDeliveryOrder.DeliveryDateSt);
    $("#cboMKTPersonSUDeliveryOrder").val(_oSUDeliveryOrder.MKTPersonID);
    _nExportPIID = _oSUDeliveryOrder.ExportPIID;
    $("#txtPINoSUDeliveryOrder").val(_oSUDeliveryOrder.PINo);
    $("#cboCurrencySUDeliveryOrder").val(_oSUDeliveryOrder.CurrencyID);

    if (_nBuyerID > 0) {
        $("#txtSearchByBuyerSUDeliveryOrder").addClass("fontColorOfPickItem");
    } else {
        $("#txtSearchByBuyerSUDeliveryOrder").removeClass("fontColorOfPickItem");
    }

    if (_nDeliveryTo > 0) {
        $("#txtSearchByDeliveryToSUDeliveryOrder").addClass("fontColorOfPickItem");
    } else {
        $("#txtSearchByDeliveryToSUDeliveryOrder").removeClass("fontColorOfPickItem");
    }

    if (_oSUDeliveryOrder.DOTypeInInt == 1 || _oSUDeliveryOrder.DOTypeInInt == 2) {
        if (_nExportPIID > 0) {
            $("#txtPINoSUDeliveryOrder").addClass("fontColorOfPickItem");
            $(".buyer").prop("disabled", true);
            $("#cboBuyerSUDeliveryOrder,#btnClrBuyerSUDeliveryOrder,#btnPickBuyerSUDeliveryOrder").hide();
            $("#txtSearchByBuyerSUDeliveryOrder").css("width", "98%");

        } else {
            $("#txtPINoSUDeliveryOrder").removeClass("fontColorOfPickItem");
            $(".buyer").prop("disabled", false);
            $("#cboBuyerSUDeliveryOrder,#btnClrBuyerSUDeliveryOrder,#btnPickBuyerSUDeliveryOrder").show();
            $("#txtSearchByBuyerSUDeliveryOrder").css("width", "");
        }
    }
    _oExportPISUDO = _oSUDeliveryOrder.ExportPI;
    _isValidAmount = true;
    var oContractor = {
        ContractorID: _nBuyerID
    }
    LoadContactPersonnel(oContractor, _oSUDeliveryOrder.BCPID);
}

function GetSUDeliveryOrderInformation(oSUDeliveryOrder) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oSUDeliveryOrder,
        ControllerName: "SUDeliveryOrder",
        ActionName: "GetSUDeliveryOrder",
        IsWinClose: false
    };
    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.SUDeliveryOrderID > 0) {
                RefreshSUDeliveryOrderControl(response.obj);
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

function NewSUDeliveryOrderObj() {
    var oSUDeliveryOrder = {
        SUDeliveryOrderID: 0,
        TextileUnitInInt: 0,
        PINo: "",
        PIStatusInInt: 0,
        IssueDate: new Date(),
        ValidityDate: new Date(),
        ContractorID: 0,
        BuyerID: 0,
        MKTEmpID: 0,
        BankBranchID: 0,
        CurrencyID: 0,
        Qty: 0.00,
        Amount: 0.00,
        IsLIBORRate: false,
        IsBBankFDD: false,
        LCTermID: 0,
        OverdueRate: 0,
        LCOpenDate: new Date(),
        DeliveryDate: new Date(),
        Note: "N/A",
        ApprovedBy: 0,
        ApprovedDate: new Date(),
        LCID: 0,
        SUDeliveryOrderPrintSetupID: 0
    };
    return oSUDeliveryOrder;
}

function UpdatePIStatus(oSUDeliveryOrder, sSuccessMessage, nRowIndex) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SUDeliveryOrder/UpdatePIStatus",
        traditional: true,
        data: JSON.stringify(oSUDeliveryOrder),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            //
            var oSUDeliveryOrder = jQuery.parseJSON(data);
            if (oSUDeliveryOrder.ErrorMessage == "" || oSUDeliveryOrder.ErrorMessage == null) {
                alert(sSuccessMessage);
                $('#tblSUDeliveryOrders').datagrid('updateRow', { index: nRowIndex, row: oSUDeliveryOrder });
            } else {
                alert(oSUDeliveryOrder.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function isValidSUDeliveryOrder() {
    if (_oSUDeliveryOrders.length <= 0) {
        alert("Empty List");
        return false;
    }
    else {
        for (var i = 0; i < _oSUDeliveryOrders.length; i++) {
            _oSUDeliveryOrders[i].DODate = _oSUDeliveryOrders[i].DODateSt;
            _oSUDeliveryOrders[i].DeliveryDate = _oSUDeliveryOrders[i].DeliveryDateSt;
            _oSUDeliveryOrders[i].ProgramDateSUDP = _oSUDeliveryOrders[i].ProgramDateSUDPInString;
            _oSUDeliveryOrders[i].LCOpeningDate = _oSUDeliveryOrders[i].LCOpeningDateInString;
            _oSUDeliveryOrders[i].LCShipmentDate = _oSUDeliveryOrders[i].ProgramDateSUDPInString;
        }
        $("#txtSUDeliveryOrderCollectionList").val(JSON.stringify(_oSUDeliveryOrders));
    }
}



function OperationPerforms(rowIndex, oSUDeliveryOrder)
{
    debugger;
    $("#btnUndoApproveSUDeliveryOrder,#btnEditSUDeliveryOrder,#btnDeleteSUDeliveryOrder,#btnReqForApproveSUDeliveryOrder,#btnUndoReqSUDeliveryOrder,#btnApproveSUDeliveryOrder,#btnReqForReviseSUDeliveryOrder,#btnAcceptReviseSUDeliveryOrder").show();

    if (oSUDeliveryOrder!=null && oSUDeliveryOrder.SUDeliveryOrderID > 0) {
        var nCurrentObjStatus = parseInt(oSUDeliveryOrder.DOStatusInInt);
        if (nCurrentObjStatus == 0) //Initialized
        {
            $("#btnUndoReqSUDeliveryOrder,#btnApproveSUDeliveryOrder,#btnUndoApproveSUDeliveryOrder,#btnReqForReviseSUDeliveryOrder,#btnAcceptReviseSUDeliveryOrder").hide();
        }
        else if (nCurrentObjStatus == 1) //RequestForApproved
        {
            $("#btnUndoApproveSUDeliveryOrder,#btnEditSUDeliveryOrder,#btnDeleteSUDeliveryOrder,#btnReqForApproveSUDeliveryOrder,#btnReqForReviseSUDeliveryOrder,#btnAcceptReviseSUDeliveryOrder").hide();
        }
        else if (nCurrentObjStatus == 3) //Approved
        {
            $("#btnEditSUDeliveryOrder,#btnDeleteSUDeliveryOrder,#btnReqForApproveSUDeliveryOrder,#btnUndoReqSUDeliveryOrder,#btnApproveSUDeliveryOrder,#btnAcceptReviseSUDeliveryOrder").hide();
        }
        /*else if (nCurrentObjStatus == 5) //PartiallyDelivered
        {
        } */
        else if (nCurrentObjStatus == 6) //Delivered
        {
            $("#btnUndoApproveSUDeliveryOrder,#btnEditSUDeliveryOrder,#btnDeleteSUDeliveryOrder,#btnReqForApproveSUDeliveryOrder,#btnUndoReqSUDeliveryOrder,#btnApproveSUDeliveryOrder,#btnAcceptReviseSUDeliveryOrder").hide();
        }
        else if (nCurrentObjStatus == 7) //RequestForRevise
        {
            $("#btnUndoApproveSUDeliveryOrder,#btnEditSUDeliveryOrder,#btnDeleteSUDeliveryOrder,#btnReqForApproveSUDeliveryOrder,#btnUndoReqSUDeliveryOrder,#btnApproveSUDeliveryOrder,#btnReqForReviseSUDeliveryOrder").hide();
        }
        /*else if (nCurrentObjStatus == 8) //Cancel
        {
        }*/
    }
}