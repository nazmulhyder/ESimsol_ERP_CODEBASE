var _isSave = true;
var _oDOTypes = [];
var _oMktPersons = [];
var _oCurrencys = [];
var _oContractorTypes = [];
var _oSUDO = { ErrorMessage: '' };
var _nBuyerId = 0;
var _nDeliverToId = 0;
var _isAddDCD = true;
var _btnName = "";
var _nRowIndex = -1;
function InitializeSUDPChallanPrepareEvents() {


    $('#txtDONoSUDOrder').keydown(function (e) {
        if (e.keyCode === 13) {
          
            _oSUDO.ErrorMessage = "Arguments;" + $.trim($("#txtDONoSUDOrder").val()) + "~" + "~" + "~";
            LoadBySearchingCriteria();
        }
    });

    $('#txtBuyerNameSUDO').keydown(function (e) {
        if (e.keyCode === 13) {
            _oSUDO.ErrorMessage = "Arguments;" + "~" + $.trim($("#txtBuyerNameSUDO").val()) + "~" + "~";
            LoadBySearchingCriteria();
        }
    });

    $('#txtDeliveredToNameSUDO').keydown(function (e) {
        if (e.keyCode === 13) {
            _oSUDO.ErrorMessage = "Arguments;" + "~" + "~" + $.trim($("#txtDeliveredToNameSUDO").val()) + "~";
            LoadBySearchingCriteria();
        }
    });


    $("#btnAdvSearchSUDO").click(function () {
        $("#winAdvSearchSUDeliveryOrder").icsWindow("open", "Advance Search");
    });

    $("#btnOkAdvSearchSUDO").click(function () {
        var oSUDOs = $("#tblSUDeliveryOrderAdvSearch").datagrid("getChecked");
        if (oSUDOs.length == 0) {
            alert("Please select atleast one item.");
            return false;
        }
        DynamicRefreshList(oSUDOs, "tblSUDOs");
        $("#winAdvSearchSUDeliveryOrder").icsWindow("close");
    });

    $("#btnViewSUDO").click(function () {
        var oSUDeliveryOrder = $("#tblSUDOs").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        if (parseInt(oSUDeliveryOrder.PIStatusInInt) > 0) { alert("Please select an Initialize item from list!"); return; }
        $("#winSUDeliveryOrder").icsWindow('open', "View Delivery Order");
        $("#txtProductNameSUDeliveryOrderDetail").val("");
        $("#txtProductNameSUDeliveryOrderDetail").removeClass("errorFieldBorder");
        $("#txtProductNameSUDeliveryOrderDetail").removeClass("fontColorOfPickItem");
        _isSave = false;
        RefreshSUDeliveryOrderLayout("btnViewSUDeliveryOrder");
        GetSUDeliveryOrderInformation(oSUDeliveryOrder);
        $("#btnCloseSUDeliveryOrder").focus();
    });

    $("#btnViewProgramSUDO").click(function () {
        var oSUDeliveryOrder = $("#tblSUDOs").datagrid("getSelected");
        if (oSUDeliveryOrder == null || parseInt(oSUDeliveryOrder.SUDeliveryOrderID) <= 0) { alert("Please select an item from list!"); return; }
        if (parseInt(oSUDeliveryOrder.PIStatusInInt) > 0) { alert("Please select an Initialize item from list!"); return; }
        $("#winSUDO").icsWindow('open', "View Delivery Order");
        $("#txtProductNameSUDeliveryOrderDetail").val("");
        $("#txtProductNameSUDeliveryOrderDetail").removeClass("errorFieldBorder");
        $("#txtProductNameSUDeliveryOrderDetail").removeClass("fontColorOfPickItem");
        _isSave = false;
        RefreshSUDOLayout("btnViewSUDO");
        GetSUDOInformation(oSUDeliveryOrder);
    });

    $("#btnCloseSUDO").click(function () {
        $("#winSUDOforSUDP").icsWindow("close");
    });

    $("#btnHistorySUDO").click(function () {
        var oSUDeliveryOrder = $("#tblSUDOs").datagrid("getSelected");
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

    $("#btnAddToListSUDO").click(function () {
         
        var oSUDeliveryOrder = $("#tblSUDOs").datagrid("getSelected");
        if (oSUDeliveryOrder == null || oSUDeliveryOrder.SUDeliveryOrderID <= 0) { alert("Please select a Delivery Order from the left list!"); return false; }

        var newDate = icsdateformat(new Date());
        newDate = icsdateparser(newDate);
        if (icsdateparser($.trim($('#txtProgramDateSUDO').datebox('getValue'))) < newDate) { alert("Can not add Program on Past Date, Please Change Date!"); return false; }
        
        var oSUDeliveryProgram = { SUDeliveryOrderID: 0, ProgramDate: '' };
        oSUDeliveryProgram.SUDeliveryOrderID = oSUDeliveryOrder.SUDeliveryOrderID;
        oSUDeliveryProgram.ProgramDate = $.trim($('#txtProgramDateSUDO').datebox('getValue'));

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSUDeliveryProgram,
            ControllerName: "SUDeliveryProgram",
            ActionName: "PrepareSUDP",
            IsWinClose: false
        }
        $.icsDataGets(obj, function (response) {
            if(response.status)
            {
                if (response.objs.length > 0) {
                    var oSUDPs = [];
                    var oBaseSUDPs = $("#tblSUDPs").datagrid('getRows');
                    for (var i = 0; i < oBaseSUDPs.length; i++) {
                        if (oBaseSUDPs[i].SUDeliveryOrderID != response.objs[0].SUDeliveryOrderID) {
                            oSUDPs.push(oBaseSUDPs[i]);
                        }
                    }
                    for (var j = 0; j < response.objs.length; j++) {
                        oSUDPs.push(response.objs[j]);
                    }
                    oBaseSUDPs = [];
                    oBaseSUDPs = oSUDPs;

                    DynamicRefreshList(oBaseSUDPs, 'tblSUDPs');

                    $('#tblSUDPs').datagrid('selectRow', oBaseSUDPs.length-1);
                }
            }
        });
    });

    $("#btnSearchSUDO").click(function () {
        if ($('#chkProgramDateSUDO').prop('checked')) {
            if ($.trim($("#txtProgramDateSUDO").datebox('getValue')) == null || $.trim($("#txtProgramDateSUDO").datebox('getValue')) == '') {
                alert('Please Select Date First!');
                return false;
            }
        }
        return RefreshSUDOs();
    });

    $('#btnRemoveSUDP').click(function () {
         
        var oSUDP = $("#tblSUDPs").datagrid("getSelected");
        if (oSUDP == null) { alert("Please select an item from list!"); return false; }
        if (!confirm("Confirm to Remove?")) return false;
        if (oSUDP.SUDeliveryProgramID > 0) {
            var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oSUDP,
                ObjectId: oSUDP.SUDeliveryProgramID,
                ControllerName: "SUDeliveryProgram",
                ActionName: "Delete",
                TableId: "tblSUDPs",
                IsWinClose: false
            };
            $.icsDelete(obj);
        }
        else {
            var oSUDPs = [];
            var oBaseSUDPs = $("#tblSUDPs").datagrid('getRows');
            for (var i = 0; i < oBaseSUDPs.length; i++) {
                if (oBaseSUDPs[i].SUDeliveryOrderDetailID != oSUDP.SUDeliveryOrderDetailID) {
                    oSUDPs.push(oBaseSUDPs[i]);
                }
            }
            
            oBaseSUDPs = [];
            oBaseSUDPs = oSUDPs;

            DynamicRefreshList(oBaseSUDPs, 'tblSUDPs');

            $('#tblSUDPs').datagrid('selectRow', oBaseSUDPs.length - 1);
        }
    });

    $("#btnPrintList").click(function () {
        if (_oDeliveryOrders.length == 0)
        {
            alert("No List Found");
            return false;
        }
        else
        {
            var sIDs = "";
            for (var i = 0; i < _oDeliveryOrders.length; i++)
            {
                sIDs = _oDeliveryOrders[i].SUDeliveryOrderID + "," + sIDs;
            }
            IDs = sIDs.substring(0, sIDs.length - 1);
        }
        $("#txtCollectionPrintText").val(IDs);
    });
    
    $("#btnNewChallanSUDO").click(function () {
        var oDeliveryOrder = $("#tblSUDOs").datagrid("getSelected");
        if (oDeliveryOrder == null || oDeliveryOrder.SUDeliveryOrderID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        _nRowIndex = $('#tblSUDOs').datagrid('getRowIndex', oDeliveryOrder);
        $("#cboFinishGoodsStoreSUDeliveryChallan").removeClass("errorFieldBorder");
        _btnName = "btnNewChallanSUDO";
        _isAddDCD = true;

        oDeliveryOrder.ProgramDateSUDP = oDeliveryOrder.ProgramDateSUDPInString;
        SetValueInNewChallanPiker(oDeliveryOrder);
        RefreshNewChallanSUDOLayout();
        endEditing();
        $("#cboGoodsCheckedBySUDeliveryChallan").focus();
        $("#cboGoodsCheckedBySUDeliveryChallan").addClass("ComboFocusBorderClass");
        GetPreviousRecord();

    });

    $("#cboGoodsCheckedBySUDeliveryChallan").focusout(function (e) {
        $("#cboGoodsCheckedBySUDeliveryChallan").removeClass("ComboFocusBorderClass");
    });

    $("#btnChallanListSUDO").click(function () {
        var oDeliveryOrder = $("#tblSUDOs").datagrid("getSelected");
        if (oDeliveryOrder == null || oDeliveryOrder.SUDeliveryOrderID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        var oDeliveryChallan = {
            SUDeliveryOrderID: parseInt(oDeliveryOrder.SUDeliveryOrderID)
        }
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryChallan/GetsBySUDeliveryOrder",
            traditional: true,
            data: JSON.stringify(oDeliveryChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryOrders = jQuery.parseJSON(data);
                if (oSUDeliveryOrders != null) {
                    if (oSUDeliveryOrders.length > 0) {
                        DynamicRefreshList(oSUDeliveryOrders, "tblSUDChallan");
                        $("#winSUDChallan").icsWindow("open", "Delivery Challans");
                        $("#btnViewSUDChallan").focus();
                    }
                    else {
                        alert("Sorry, No List Found.");
                        DynamicRefreshList([], "tblSUDChallan");
                    }
                }
                else {
                    alert("Invalid Object List.");
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });

    });

    $("#btnCloseSUDChallan").click(function () {
        $("#winSUDChallan").icsWindow("close");
    });

    $("#btnViewSUDChallan").click(function () {
        var oSUDeliveryChallan = $("#tblSUDChallan").datagrid("getSelected");
        if (oSUDeliveryChallan == null || oSUDeliveryChallan.SUDeliveryChallanID <= 0) {
            alert("Please select an item from list.");
            return false;
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryChallan/GetSUDeliveryChallan",
            traditional: true,
            data: JSON.stringify(oSUDeliveryChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryChallan = jQuery.parseJSON(data);
                if (oSUDeliveryChallan != null) {
                    if (oSUDeliveryChallan.SUDeliveryChallanID > 0) {
                        LoadSUDeliveryChallanInfo(oSUDeliveryChallan);
                        $("#btnCloseSUDeliveryChallan").focus();
                    }
                    else {
                        DynamicRefreshList([], "tblSUDeliveryChallanDetails");
                    }
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });

    });

    DynamicRefreshList(_oDeliveryOrders, 'tblSUDOs');
    for (var i = 0; i < _oDeliveryOrders.length; i++) {
        _oDeliveryOrders[i].DODate = _oDeliveryOrders[i].DODateSt;
        _oDeliveryOrders[i].DeliveryDate = _oDeliveryOrders[i].DeliveryDateSt;
        _oDeliveryOrders[i].ProgramDateSUDP = _oDeliveryOrders[i].ProgramDateSUDPInString;
    }
    $('#txtCollectionPrintText').val(JSON.stringify(_oDeliveryOrders));
    GenerateTableColumnsSUDeliveyOrder();
    DynamicRefreshList(_oDeliveryOrders, 'tblSUDOs');

    
    $('#txtProgramDateSUDO').datebox({ onSelect: function (date) { if ($('#chkProgramDateSUDO').prop('checked')) { RefreshSUDOs(); } }});
    $('#txtProgramDateSUDO').datebox('setValue', icsdateformat(new Date()));

    SUDOReviseHistory();
    
    LoadComboBoxesSUDO();

    $("#btnProgramHistorySUDO").click(function(){
        var oSUDO = $("#tblSUDOs").datagrid("getSelected");
        if (oSUDO == null || oSUDO.SUDeliveryOrderID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        var oSUDeliveryProgram = {
            SUDeliveryOrderID: oSUDO.SUDeliveryOrderID
        };

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSUDeliveryProgram,
            ControllerName: "SUDeliveryProgram",
            ActionName: "GetsProgramAndChallanHistory",
            IsWinClose: false
        };

        $.icsDataGets(obj, function (response) {
            debugger;
            ResetAllTables("tblProgramHistory", "tblChallanHistory");
            var oSUDeliveryProgram = response.objs;
            if (oSUDeliveryProgram.SUDeliveryPrograms.length > 0) {
                $("#tabProgramChallanHistory").tabs("select", 0);
                $("#winProgramChallanHistory").icsWindow("open", "DO No : " + oSUDeliveryProgram.DONo + " @ Qty : " + formatPrice(oSUDeliveryProgram.DOTotalQty) + " KG");

                //Ratin
                if (oSUDeliveryProgram.SUDeliveryPrograms.length > 0)
                {
                    debugger;
                    var nTotalProgramQty = 0;
                    for (var i = 0; i < oSUDeliveryProgram.SUDeliveryPrograms.length; i++) {
                        nTotalProgramQty = parseFloat(oSUDeliveryProgram.SUDeliveryPrograms[i].ProgramQty) + parseFloat(nTotalProgramQty);
                    }

                    var oSUDP = {
                        LotNo: "Total : ",
                        ProgramQtyInString: formatPrice(nTotalProgramQty)
                    };

                    oSUDeliveryProgram.SUDeliveryPrograms.push(oSUDP);
                }

                if (oSUDeliveryProgram.SUDeliveryChallanDetails.length > 0) {
                    debugger;
                    var nTotalChallanQty = 0;
                    for (var i = 0; i < oSUDeliveryProgram.SUDeliveryChallanDetails.length; i++) {
                        nTotalChallanQty = parseFloat(oSUDeliveryProgram.SUDeliveryChallanDetails[i].Qty) + parseFloat(nTotalChallanQty);
                    }

                    var oSUC = {
                        ProductName: "Total : ",
                        QtySt: formatPrice(nTotalChallanQty)
                    };

                    oSUDeliveryProgram.SUDeliveryChallanDetails.push(oSUC);
                }

                DynamicRefreshList(oSUDeliveryProgram.SUDeliveryPrograms, "tblProgramHistory");
                DynamicRefreshList(oSUDeliveryProgram.SUDeliveryChallanDetails, "tblChallanHistory");
            }
            else {
                alert("No List Found");
            }
        });
    });



    $("#btnClosePCH").click(function () {
        $("#winProgramChallanHistory").icsWindow("close");
        ResetAllTables("tblProgramHistory", "tblChallanHistory");
    });
}

function GetPreviousRecord(){
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
                $("#txtDriverNameSUDeliveryChallan").val(oSUDC.DriverName);
                $("#txtDriverContactNoSUDeliveryChallan").val(oSUDC.DriverContactNo);
                $("#txtStorePhoneNoSUDeliveryChallan").val(oSUDC.StorePhoneNo);
            }
        }
    });
}

function LoadSUDeliveryChallanInfo(oSUDeliveryChallan) {
    $("#winSUDeliveryChallan").icsWindow("open", "Delivery Challan");

    $("#txtChallanNoSUDeliveryChallan").val(oSUDeliveryChallan.ChallanNo);
    $("#txtDONoSUDeliveryChallan").val(oSUDeliveryChallan.DONo);
    $("#txtSearchByBuyerSUDeliveryChallan").val(oSUDeliveryChallan.BuyerName);
    $("#txtSearchByDeliveryToSUDeliveryChallan").val(oSUDeliveryChallan.DeliveryToName);
    $("#txtRemarksSUDeliveryChallan").val(oSUDeliveryChallan.Remarks);
    $("#txtDriverNameSUDeliveryChallan").val(oSUDeliveryChallan.DriverName);
    $("#txtDriverContactNoSUDeliveryChallan").val(oSUDeliveryChallan.DriverContactNo);
    $("#txtStorePhoneNoSUDeliveryChallan").val(oSUDeliveryChallan.StorePhoneNo);
    $("#txtGatePassNoSUDeliveryChallan").val(oSUDeliveryChallan.GatePassNo);
    $("#cboGoodsCheckedBySUDeliveryChallan").val(oSUDeliveryChallan.CheckedBy);
    $("#cboFinishGoodsStoreSUDeliveryChallan").val(oSUDeliveryChallan.StoreID);
    $("#txtVehicleNoSUDeliveryChallan").val(oSUDeliveryChallan.VehicleNo);
    $("#cboBuyerSUDeliveryChallan").val(oSUDeliveryChallan.BuyerContractorType);
    $("#cboDeliveryToSUDeliveryChallan").val(oSUDeliveryChallan.DeliveryToContractorType);
    $('#txtChallanDateSUDeliveryChallan').datebox('setValue', oSUDeliveryChallan.ChallanDateSt);
    DynamicRefreshList(oSUDeliveryChallan.SUDeliveryChallanDetails, "tblSUDeliveryChallanDetails");

    $("#winSUDeliveryChallan").find("input,select").prop("disabled", true);
    $("#btnSaveSUDeliveryChallan,#btnDisburseSUDeliveryChallan,#btnLotAssignSUDeliveryChallanDetail,#btnRefreshSUDeliveryChallanDetail,#btnProductRemoveSUDeliveryChallanDetail,#btnCopyItemSUDeliveryChallanDetail,#btnRefreshSUDeliveryOrderDetail").hide();
}

function LoadBySearchingCriteria() {
    //Ratin
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: _oSUDO,
        ControllerName: "SUDeliveryProgram",
        ActionName: "GetsbySUDP",
        IsWinClose: false
    }

    $.icsDataGets(obj, function (response) {
        if (response.status) {
            if (response.objs.length > 0) {
                _oDeliveryOrders = response.objs;
                DynamicRefreshList(_oDeliveryOrders, "tblSUDOs");
            }
            else {
                alert("Sorry, No List Found.");
                DynamicRefreshList([], "tblSUDOs");
            }
        }
        else {
            alert("Sorry, No List Found.");
            DynamicRefreshList([], "tblSUDOs");
        }
    });
}

function SetValueInNewChallanPiker(oDeliveryOrder) {
    $("#txtDONoSUDeliveryChallan").val(oDeliveryOrder.DONo);
    $("#txtSearchByBuyerSUDeliveryChallan").val(oDeliveryOrder.BuyerName);
    _nBuyerId = oDeliveryOrder.BuyerID;
    $("#txtSearchByDeliveryToSUDeliveryChallan").val(oDeliveryOrder.DeliveredToName);
    _nDeliverToId = oDeliveryOrder.DeliveryTo;
    $("#txtRemarksSUDeliveryChallan").val(oDeliveryOrder.Remarks);
    $("#txtDriverNameSUDeliveryChallan").val('');
    $("#txtDriverContactNoSUDeliveryChallan,#txtStorePhoneNoSUDeliveryChallan").val('');
    $("#txtGatePassNoSUDeliveryChallan").val('');
    if (_nBuyerId > 0) {
        $("#txtSearchByBuyerSUDeliveryChallan").addClass("fontColorOfPickItem");
    } else {
        $("#txtSearchByBuyerSUDeliveryChallan").removeClass("fontColorOfPickItem");
    }
    if (_nDeliverToId > 0) {
        $("#txtSearchByDeliveryToSUDeliveryChallan").addClass("fontColorOfPickItem");
    } else {
        $("#txtSearchByDeliveryToSUDeliveryChallan").removeClass("fontColorOfPickItem");
    }
  

    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oDeliveryOrder,
        ControllerName: "SUDeliveryProgram",
        ActionName: "GetSUDO",
        IsWinClose: false
    };
    debugger;
    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            debugger;

            if (response.obj.SUDeliveryOrderID > 0) {
                RefreshSUDOControl(response.obj);
                var oDeliveryOrder = response.obj;
                var oDeliveryOrderDetails = oDeliveryOrder.SUDeliveryPrograms;
                _oSUDeliveryChallan = {
                    SUDeliveryChallanID: 0,
                    SUDeliveryOrderID: oDeliveryOrder.SUDeliveryOrderID,
                    ChallanStatusInInt: 0
                };
                var oDeliveryChallanDetails = [];
                for (var i = 0; i < oDeliveryOrderDetails.length; i++) {
                    var oDOD = oDeliveryOrderDetails[i];
                    var oDeliveryChallanDetail = {
                        SUDeliveryChallanDetailID: 0,
                        SUDeliveryChallanID: 0,
                        SUDeliveryOrderDetailID: oDOD.SUDeliveryOrderDetailID,
                        ProductID: oDOD.ProductID,
                        ProductName: oDOD.ProductName,
                        ProductCode: oDOD.ProductCode,
                        MUnitID: oDOD.MUnitID,
                        Qty: oDOD.ProgramQty,
                        QtyLbsSt: GetLBS(oDOD.ProgramQty, 2),
                        ProgramQty: oDOD.ProgramQty,
                        SUDeliveryProgramID: oDOD.SUDeliveryProgramID,
                        LotID: oDOD.LotID,
                        LotNo: oDOD.LotNo,
                        YetToChallan: oDOD.YetToChallan,
                        YetToChallanSt: formatPrice(oDOD.YetToChallan),
                        YetToChallanCountWise: oDOD.YetToChallanCountWise,
                        YetToChallanCountWiseSt: formatPrice(oDOD.YetToChallanCountWise),
                        Bags: 0,
                        LotNoWithStore: (oDOD.LotID > 0 ? oDOD.LotNo + " (" + oDOD.OperationUnitName + ")" : "")
                    };
                    oDeliveryChallanDetails.push(oDeliveryChallanDetail);
                }
                DynamicRefreshList(oDeliveryChallanDetails, "tblSUDeliveryChallanDetails");
                RefreshTotalSummeryDC();
                if (oDeliveryOrderDetails.length > 0) {
                    $("#txtVehicleNoSUDeliveryChallan").val(oDeliveryOrderDetails[0].SUVehicleNo);
                } else {
                    $("#txtVehicleNoSUDeliveryChallan").val("");
                }
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



function RefreshNewChallanSUDOLayout() {
    $(".enableDisable").prop('disabled', true);
    $("#txtChallanDateSUDeliveryChallan").datebox({ disabled: true });
    $("#txtChallanDateSUDeliveryChallan").datebox("setValue", icsdateformat(new Date()));
    $("#toolbarSUDeliveryChallanDetail a").show();
    $("#btnSaveSUDeliveryChallan").show();
    $("#btnDisburseSUDeliveryChallan").hide();
    $("#txtRemarksSUDeliveryChallan,#txtDriverNameSUDeliveryChallan,#txtDriverContactNoSUDeliveryChallan,#txtStorePhoneNoSUDeliveryChallan,#txtGatePassNoSUDeliveryChallan,#txtVehicleNoSUDeliveryChallan").prop('disabled', false);
    $("#cboGoodsCheckedBySUDeliveryChallan,#cboFinishGoodsStoreSUDeliveryChallan").prop("disabled", false);
    $("#winSUDeliveryChallan").icsWindow("open", "Add Delivery Challan");

    $("#cboBuyerSUDeliveryChallan,#cboDeliveryToSUDeliveryChallan").hide();
    $("#txtSearchByBuyerSUDeliveryChallan,#txtSearchByDeliveryToSUDeliveryChallan").css("width","98%");
    $("#txtSearchByBuyerSUDeliveryChallan,#txtSearchByDeliveryToSUDeliveryChallan").addClass("fontColorOfPickItem");
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
                    DynamicRefreshList(oSUDeliveryOrderLog.SUDeliveryOrderDetailLogs, "tblSUDeliveryOrderDetails");
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

function LoadComboBoxesSUDO() {
     
    $("#cboDOTypeSUDO").icsLoadCombo({
        List: _oDOTypes,
        OptionValue: "id",
        DisplayText: "Value"
    });
    $("#cboMKTPersonSUDO").icsLoadCombo({
        List: _oMktPersons,
        OptionValue: "EmployeeID",
        DisplayText: "Name"
    });
    $("#cboCurrencySUDO").icsLoadCombo({
        List: _oCurrencys,
        OptionValue: "CurrencyID",
        DisplayText: "CurrencyName"
    });

    var oContractorTypes = [];
    for (var i = 0; i < _oContractorTypes.length; i++) {
        if (_oContractorTypes[i].Value == 2 || _oContractorTypes[i].Value == 3) {
            oContractorTypes.push(_oContractorTypes[i]);
        }
    }
    LoadCboItemsSUDO(oContractorTypes);
}

function LoadCboItemsSUDO(Items) {
    var listItems = "";
    for (i = 0; i < Items.length; i++) {
        if (Items[i].UserID != 0) {
            listItems += "<option value='" + Items[i].Value + "'>" + Items[i].Text + "</option>";
        }
    }
    $("#cboBuyerSUDO").html(listItems);
    $("#cboDeliveryToSUDO").html(listItems);
}

function RefreshTotalSummerySUDP() {
    var nTotalQtyKG = 0;

    var oSUDPs = $('#tblSUDPs').datagrid('getRows');
    for (var i = 0; i < oSUDPs.length; i++) {
        nTotalQtyKG = nTotalQtyKG + parseFloat(oSUDPs[i].ProgramQty);

    }

    $("#lblTotalQtySUDP").html(formatPrice(nTotalQtyKG) + ' KG');
}

function RefreshSUDOLayout(buttonId) {
     
    if (buttonId === "btnViewSUDO") {
        $("#winSUDO input").prop("disabled", true);
        $("#winSUDO select").prop("disabled", true);
        $("#btnSaveSUDO").hide();
        $("#btnProductRemoveSUDODetail, #btnPIProductsSUDODetail, #btnProductAddSUDODetail").hide();
    }
    else {
        $("#winSUDO input").not("#txtDONoSUDO, #txtPINoSUDO").prop("disabled", false);
        $("#winSUDO select").prop("disabled", false);
        $("#btnSaveSUDO").show();
        $("#btnProductRemoveSUDODetail, #btnPIProductsSUDODetail, #btnProductAddSUDODetail").show();
    }

    if (buttonId === "btnEditSUDO" || buttonId === "btnViewSUDO") {
        $("#btnPINoSUDO,#btnClearPINoSUDO").prop("disabled", true);
    } else {
        $("#btnPINoSUDO,#btnClearPINoSUDO").prop("disabled", false);
    }

}

function RefreshSUDOControl(oSUDO) {
     
    _oSUDO = oSUDO;
    if (_oSUDO.SUDeliveryOrderDetails != null && _oSUDO.SUDeliveryOrderDetails.length > 0) {
        DynamicRefreshList(_oSUDO.SUDeliveryOrderDetails, "tblSUDeliveryOrderDetails");
        RefreshTotalSummery();
    }
    if (_oSUDO.SUDeliveryPrograms != null && _oSUDO.SUDeliveryPrograms.length > 0) {
        DynamicRefreshList(_oSUDO.SUDeliveryPrograms, "tblSUDPs");
        RefreshTotalSummerySUDP();
    }


    $("#txtDONoSUDO").val(_oSUDO.DONo);
    $("#cboDOTypeSUDO").val(_oSUDO.DOTypeInInt);
    _nBuyerID = _oSUDO.BuyerID;
    $("#txtSearchByBuyerSUDO").val(_oSUDO.BuyerName);
    _nDeliveryTo = _oSUDO.DeliveryTo;
    $("#txtSearchByDeliveryToSUDO").val(_oSUDO.DeliveredToName);
    $("#txtDeliverPointSUDO").val(_oSUDO.DeliveryPoint);
    $("#txtRemarksSUDO").val(_oSUDO.Remarks);
    $('#txtDODateSUDO').datebox('setValue', _oSUDO.DODateSt);
    $('#txtDeliveryDateSUDO').datebox('setValue', _oSUDO.DeliveryDateSt);
    $("#cboMKTPersonSUDO").val(_oSUDO.MKTPersonID);
    $("#cboMKTPersonSUDO").val(_oSUDO.MKTPersonID);
    _nExportPIID = _oSUDO.ExportPIID;
    $("#txtPINoSUDO").val(_oSUDO.PINo);
    $("#cboCurrencySUDO").val(_oSUDO.CurrencyID);

    if (_nBuyerID > 0) {
        $("#txtSearchByBuyerSUDO").addClass("fontColorOfPickItem");
    } else {
        $("#txtSearchByBuyerSUDO").removeClass("fontColorOfPickItem");
    }

    if (_nDeliveryTo > 0) {
        $("#txtSearchByDeliveryToSUDO").addClass("fontColorOfPickItem");
    } else {
        $("#txtSearchByDeliveryToSUDO").removeClass("fontColorOfPickItem");
    }

    if (_nExportPIID > 0) {
        $("#txtPINoSUDO").addClass("fontColorOfPickItem");
        $(".buyer").prop("disabled", true);
    } else {
        $("#txtPINoSUDO").removeClass("fontColorOfPickItem");
        $(".buyer").prop("disabled", false);
    }
}

function GetSUDOInformation(oSUDeliveryOrder) {
     
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oSUDeliveryOrder,
        ControllerName: "SUDeliveryProgram",
        ActionName: "GetSUDO",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.SUDeliveryOrderID > 0) {
                
                RefreshSUDOControl(response.obj);
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

function RefreshSUDeliveryOrderLayout(buttonId) {
    if (buttonId === "btnViewSUDeliveryOrder") {
        $("#winSUDeliveryOrder input").prop("disabled", true);
        $("#winSUDeliveryOrder select").prop("disabled", true);
        $("#btnSaveSUDeliveryOrder").hide();
        $("#btnProductRemoveSUDeliveryOrderDetail, #btnPIProductsSUDeliveryOrderDetail, #btnProductAddSUDeliveryOrderDetail,#btnRefreshSUDeliveryOrderDetail").hide();
    }
    else {
        $("#winSUDeliveryOrder input").not("#txtDONoSUDeliveryOrder, #txtPINoSUDeliveryOrder").prop("disabled", false);
        $("#winSUDeliveryOrder select").prop("disabled", false);
        $("#btnSaveSUDeliveryOrder").show();
        $("#btnProductRemoveSUDeliveryOrderDetail, #btnPIProductsSUDeliveryOrderDetail, #btnProductAddSUDeliveryOrderDetail,#btnRefreshSUDeliveryOrderDetail").show();
    }

    if (buttonId === "btnEditSUDeliveryOrder" || buttonId === "btnViewSUDeliveryOrder") {
        $("#btnPINoSUDeliveryOrder,#btnClearPINoSUDeliveryOrder").prop("disabled", true);
    } else {
        $("#btnPINoSUDeliveryOrder,#btnClearPINoSUDeliveryOrder").prop("disabled", false);
    }

}

function RefreshSUDeliveryOrderControl(oSUDeliveryOrder) {
    _oSUDeliveryOrder = oSUDeliveryOrder;
  
    if (_oSUDeliveryOrder.SUDeliveryOrderDetails != null && _oSUDeliveryOrder.SUDeliveryOrderDetails.length > 0) {
        DynamicRefreshList(_oSUDeliveryOrder.SUDeliveryOrderDetails, "tblSUDeliveryOrderDetails");
        RefreshTotalSummery();
    }
    if (_oSUDeliveryOrder.SUDeliveryPrograms != null && _oSUDeliveryOrder.SUDeliveryPrograms.length > 0) {
        DynamicRefreshList(_oSUDeliveryOrder.SUDeliveryPrograms, "tblSUDPs");
        RefreshTotalSummerySUDP();
    }
    
    $("#txtDONoSUDeliveryOrder").val(_oSUDeliveryOrder.DONo);
    $("#cboDOTypeSUDeliveryOrder").val(_oSUDeliveryOrder.DOTypeInInt);
    _nBuyerID = _oSUDeliveryOrder.BuyerID;
    $("#txtSearchByBuyerSUDeliveryOrder").val(_oSUDeliveryOrder.BuyerName);
    _nDeliveryTo = _oSUDeliveryOrder.DeliveryTo;
    $("#txtSearchByDeliveryToSUDeliveryOrder").val(_oSUDeliveryOrder.DeliveredToName);
    $("#txtDeliverPointSUDeliveryOrder").val(_oSUDeliveryOrder.DeliveryPoint);
    $("#txtRemarksSUDeliveryOrder").val(_oSUDeliveryOrder.Remarks);
    $('#txtDODateSUDeliveryOrder').datebox('setValue', _oSUDeliveryOrder.DODateSt);
    $('#txtDeliveryDateSUDeliveryOrder').datebox('setValue', _oSUDeliveryOrder.DeliveryDateSt);
    $("#cboMKTPersonSUDeliveryOrder").val(_oSUDeliveryOrder.MKTPersonID);
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

    if (_nExportPIID > 0) {
        $("#txtPINoSUDeliveryOrder").addClass("fontColorOfPickItem");
        $(".buyer").prop("disabled", true);
    } else {
        $("#txtPINoSUDeliveryOrder").removeClass("fontColorOfPickItem");
        $(".buyer").prop("disabled", false);
    }
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

function RefreshArgumentsSUDO() {
    var arg = 'Arguments;';


    var txtDONoSUDOrder = $("#txtDONoSUDOrder").val();
    if (txtDONoSUDOrder != null) {
        arg = arg + txtDONoSUDOrder + '~';
    }
    var txtBuyerNameSUDO = $("#txtBuyerNameSUDO").val();
    if (txtBuyerNameSUDO != null) {
        arg = arg + txtBuyerNameSUDO + '~';
    }
    var txtDeliveredToNameSUDO = $("#txtDeliveredToNameSUDO").val();
    if (txtDeliveredToNameSUDO != null) {
        arg = arg + txtDeliveredToNameSUDO + '~';
    }
    var txtProgramDateSUDO = $("#txtProgramDateSUDO").datebox('getValue');
    if ($('#chkProgramDateSUDO').prop('checked')) {
        if (txtProgramDateSUDO != null) {
            arg = arg + txtProgramDateSUDO + '~';
        }
    }
    else {
        arg = arg + '' + '~';
    }
    
    _oSUDO.ErrorMessage = '';
    _oSUDO.ErrorMessage = arg;
}

function RefreshSUDOs() {
     
    RefreshArgumentsSUDO();

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: _oSUDO,
        ControllerName: "SUDeliveryProgram",
        ActionName: "GetsbySUDP",
        IsWinClose: false
    }
    $.icsDataGets(obj, function (response) {
         
        if (response.status) {
            
            if (response.objs.length > 0) {
                _oDeliveryOrders = [];
                _oDeliveryOrders = response.objs;
                DynamicRefreshList(_oDeliveryOrders, 'tblSUDOs');
                for (var i = 0; i < _oDeliveryOrders.length; i++) {
                    _oDeliveryOrders[i].DODate = _oDeliveryOrders[i].DODateSt;
                    _oDeliveryOrders[i].DeliveryDate = _oDeliveryOrders[i].DeliveryDateSt;
                    _oDeliveryOrders[i].ProgramDateSUDP = _oDeliveryOrders[i].ProgramDateSUDPInString;
                }
                $('#txtCollectionPrintText').val(JSON.stringify(_oDeliveryOrders));
            }
            else {
                alert('No Data Found!');
                _oDeliveryOrders = [];
                DynamicRefreshList(_oDeliveryOrders, 'tblSUDOs');
                $('#txtCollectionPrintText').val(JSON.stringify(_oDeliveryOrders));
            }
        }
    });
}





var _nQty = 0;
var _nIndex = -1;
var _bIsValid = false;
function onClickCell(index, field, value) {
     
    $('#tblSUDPs').datagrid('selectRow', index);

    var oSUDP = $('#tblSUDPs').datagrid('getSelected');

    if (_nIndex != index && _nIndex != -1) {

        $('#tblSUDPs').datagrid('endEdit', _nIndex);
        if (field == 'ProgramQty') {
            _nQty = parseFloat(value);
            if (_bIsValid) {
                _nIndex = index;
                $('#tblSUDPs').datagrid('beginEdit', _nIndex);
            }
            else {
                $('#tblSUDPs').datagrid('beginEdit', _nIndex);
                return false;
            }
        }
        else {
            if (!_bIsValid && _nQty != 0) {
                $('#tblSUDPs').datagrid('beginEdit', _nIndex);
                return false;
            }
        }
    }
    else {
        if (field != 'ProgramQty') {
            $('#tblSUDPs').datagrid('endEdit', index);
            if (!_bIsValid && _nQty != 0) {
                $('#tblSUDPs').datagrid('beginEdit', index);
            }
        }
        else {
            _nQty = parseFloat(value);
            $('#tblSUDPs').datagrid('beginEdit', index);
        }
        _nIndex = index;
    }
    
    return true;
}

function EndEdit(index, row, changes) {
     
    var oSUDP = row;
    if (icsdateparser(oSUDP.ProgramDateInString) < icsdateparser(icsdateformat(new Date()))) { alert('Can not Change Previous Program!'); return false; } else { _bIsValid = true; }
    
    if (changes.ProgramQty != null && _nQty != parseFloat(changes.ProgramQty)) {
        if (oSUDP.YetToDOProgram < parseFloat(changes.ProgramQty)) { alert('Your Entered Program Qty: ' + changes.ProgramQty + ' is More than Yet To Program Qty: ' + oSUDP.YetToDOProgram + '.\n Please Change Program Qty and Try Again!'); _bIsValid = false; return false; } else { _bIsValid = true; }
        if (oSUDP.YetToDelivery < parseFloat(changes.ProgramQty)) { alert('Your Entered Program Qty: ' + changes.ProgramQty + ' is More than Yet To Delivery Qty: ' + oSUDP.YetToDelivery + '.\n Please Change Program Qty and Try Again!'); _bIsValid = false; return false; } else { _bIsValid = true; }
        oSUDP.ProgramQty = parseFloat(changes.ProgramQty);
        oSUDP.ProgramDate = $.trim($('#txtProgramDateSUDO').datebox('getValue'));
        Save(oSUDP);
        return true;
    }
}

function Save(oSUDP) {
     
    
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SUDeliveryProgram/Save",
        traditional: true,
        data: JSON.stringify(oSUDP),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
             
            var SUDProgram = jQuery.parseJSON(data);
            if (parseInt(SUDProgram.SUDeliveryProgramID) > 0) {
                var nRowIndex = $('#tblSUDPs').datagrid("getRowIndex", oSUDP);
                $('#tblSUDPs').datagrid("updateRow", { index: nRowIndex, row: SUDProgram });
            }
            
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}



function GenerateTableColumnsSUDeliveyOrder() {
    _otblColumns = [];

    var oColDONo = { field: "DONo", width: "13%", title: "DO No", frozen: true };
    var oColDODateSt = { field: "DODateSt", width: "8%", title: "DO Date", frozen: true };
    var oColDOStatusSt = { field: "DOStatusSt", width: "11%", title: "DO Status", frozen: true };
    var oColLastUpdateByNameSUDP = { field: "LastUpdateByNameSUDP", width: "12%", title: "Program By", frozen: true };
    var oColTotalChallanSUDP = { field: "TotalChallanWithSUDOID", width: "8%", title: "Total Challan", frozen: true, align: 'center', formatter: formatTotalChallan };
    var oColProgramDateSUDPInString = { field: "ProgramDateSUDPInString", width: "8%", title: "Program Date", frozen: true };
    var oColBuyerName = { field: "BuyerName", width: "13%", title: "Buyer", frozen: true };
    var oColDOTypeSt = { field: "DOTypeSt", width: "11%", title: "DO Type", frozen: true };
    var oColDeliveredToName = { field: "DeliveredToName", width: "13%", title: "Deliver To", frozen: true };
    var oColDeliveryDateSt = { field: "DeliveryDateSt", width: "8%", title: "Delivery Date", frozen: true };
    var oColMKTPName = { field: "MKTPName", width: "12%", title: "MKT Person", frozen: true };
    var oColQtySt = { field: "QtySt", width: "11%", title: "Qty", frozen: true, align:'right' };
    var oColAmountSt = { field: "AmountSt", width: "11%", title: "Amount", frozen: true, align: 'right' };

    _otblColumns.push(oColDONo, oColDODateSt, oColDOStatusSt, oColLastUpdateByNameSUDP, oColTotalChallanSUDP, oColProgramDateSUDPInString, oColBuyerName,
        oColDOTypeSt, oColDeliveredToName, oColDeliveryDateSt, oColMKTPName, oColQtySt, oColAmountSt);
    $('#tblSUDOs').datagrid({ columns: [_otblColumns] });
    
}


