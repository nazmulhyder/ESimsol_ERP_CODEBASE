var _isSave = true;
var _oDOTypes = [];
var _oMktPersons = [];
var _oCurrencys = [];
var _oContractorTypes = [];
var _sErrorMessage = "";
var _isClickUpdateBtn = false;
function InitializeSUDeliveryProgramsEvents() {

    $('#txtDONoSUDO').keyup(function (e) {
        if (e.keyCode === 13) {
            
            var oSUDeliveryOrder = {
                Remarks: $("#txtDONoSUDO").val()
                         + "~" + 0
                         + "~" + 0
                         + "~" + 0
                         + "~" + 0
                         + "~" + "False"
                         + "~" + icsdateformat(new Date())
                         + "~" + icsdateformat(new Date())
                         + "~" + "False"
                         + "~" + icsdateformat(new Date())
                         + "~" + icsdateformat(new Date())
                         + "~" + ""
                         + "~" + 0
            };

            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oSUDeliveryOrder,
                ControllerName: "SUDeliveryOrder",
                ActionName: "AdvSearch",
                IsWinClose: false
            };

            $.icsDataGets(obj, function (response) {
                if (response.status && response.objs.length > 0) {
                    var oSUDeliveryOrders = response.objs;
                    DynamicRefreshList(oSUDeliveryOrders, "tblSUDOs");

                } else {
                    DynamicRefreshList([], "tblSUDOs");
                    alert("No List Found.");
                }
            });
        }

    });
    $("#btnAdvSearchSUDO").click(function () {
        ResetAdvSearchWindow();
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
        GetSUDeliveryOrderInformation(oSUDeliveryOrder);
        RefreshSUDeliveryOrderLayout("btnViewSUDeliveryOrder");
    });
    $("#btnHistorySUDO").click(function () {
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
                    }
                    else {
                        alert("Sorry, This DO has No Delivery Challan.");
                        DynamicRefreshList([], "tblSUDChallan");
                    }
                }
                else {
                    alert("Invalid DO.");
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
    $("#btnAddToListSUDO").click(function () {
         
        var oSUDeliveryOrder = $("#tblSUDOs").datagrid("getSelected");
        if (oSUDeliveryOrder == null || oSUDeliveryOrder.SUDeliveryOrderID <= 0) { alert("Please select a Delivery Order from the left list!"); return false; }
        if (oSUDeliveryOrder.DOStatusInInt != 3) { alert("Selected Delivery Order is not approved. \nPlease select another and Try Again!"); return false; }

        var newDate = icsdateformat(new Date());
        newDate = icsdateparser(newDate);

        if (icsdateparser($.trim($('#txtProgramDateSUDP').datebox('getValue'))) < newDate) { alert("Can not add Program on Past Date, Please Change Date!"); return false; }
        
        var oSUDeliveryProgram = { SUDeliveryOrderID: 0, ProgramDate: '' };
        oSUDeliveryProgram.SUDeliveryOrderID = oSUDeliveryOrder.SUDeliveryOrderID;
        oSUDeliveryProgram.ProgramDate = $.trim($('#txtProgramDateSUDP').datebox('getValue'));

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

    $("#btnRefreshSUDP").click(function () {
        if ($.trim($("#txtProgramDateSUDP").datebox('getValue')) == null || $.trim($("#txtProgramDateSUDP").datebox('getValue')) == '') {
            alert('Please Select Date First!');
            return false;
        }
        return RefreshSUDPs();
    });
    $("#btnPendingSUDP").click(function () {
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: {},
            ControllerName: "SUDeliveryProgram",
            ActionName: "GetsPendingProgram",
            IsWinClose: false
        }
        $.icsDataGets(obj, function (response) {

            if (response.status) {
                if (response.objs.length > 0) {
                    _oDBSUDeliveryPrograms = response.objs;
                    DynamicRefreshList(_oDBSUDeliveryPrograms, 'tblSUDPs');
                }
                else {
                    alert('No Data Found!');
                    DynamicRefreshList([], 'tblSUDPs');
                }
            }
        });
    });
    $("#btnUpdateSUDP").click(function () {
        //ratin
        
        var oSUDP = $("#tblSUDPs").datagrid("getSelected");
        if (oSUDP == null) { alert("Please select an item from list!"); return false; }
        _isClickUpdateBtn = true;
        var nRowIndex = $("#tblSUDPs").datagrid("getRowIndex", oSUDP);
        $('#tblSUDPs').datagrid('endEdit', nRowIndex);
        
        //Remaining Part Call From Save() function depand on _isClickUpdateBtn
        var oSUDP1 = $("#tblSUDPs").datagrid("getSelected");
        if (oSUDP1.ProgramQty <= 0)
        {
            alert("Please give Program Qty.");
            return false;
        }
        if (oSUDP1.SUDeliveryProgramID>0)
        {
            OpenUpdatePicker();
        }
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
            _nIndex = oBaseSUDPs.length - 1;//for enabling cell edit.
        }
    });
   
    $("#btnPrintList").click(function () {
        var aa = 'Arguments;' + $.trim($("#txtProgramDateSUDP").datebox('getValue')) + '~';
        //var aa = 'Faruk';
        window.open(_sBaseAddress + '/SUDeliveryProgram/PrintSUDeiveryPrograms?sarguments='+aa, "_blank");
    });
    GenerateTableColumnsSUDeliveryProgram();
    GenerateTableColumnsSUDeliveyOrder();
    DynamicRefreshList(_oDBSUDeliveryPrograms, 'tblSUDPs');
    DynamicRefreshList(_oDeliveryOrders, 'tblSUDOs');
    $('#txtProgramDateSUDP').datebox({ onSelect: function (date) { RefreshSUDPs(); } });
    $('#txtProgramDateSUDP').datebox('setValue', icsdateformat(new Date()));
    SUDOReviseHistory();
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
                        DynamicRefreshList(oSUDeliveryOrders, "tblSUDOs");
                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSUDOs");
                    }

                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblSUDOs");
                }
            }
        });
    });
    $("#txtDODateSUDeliveryOrders").datebox('setValue', icsdateformat(new Date()));

  

    $("#btnClosePCH").click(function () {
        $("#winProgramChallanHistory").icsWindow("close");
        ResetAllTables("tblProgramHistory", "tblChallanHistory");
    });

    $("#btnGetAllProgramsSUDO").click(function () {
        var oSUDO = $("#tblSUDOs").datagrid("getSelected");
        if (oSUDO == null || oSUDO.SUDeliveryOrderID <= 0) {
            alert("Please select an item from list.");
            return false;
        }

        var oSUDeliveryProgram = {
            SUDeliveryOrderID: oSUDO.SUDeliveryOrderID,
            IsWithRemaingQty: false
        };

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSUDeliveryProgram,
            ControllerName: "SUDeliveryProgram",
            ActionName: "GetsProgramBySUDO",
            IsWinClose: false
        };

        $.icsDataGets(obj, function (response) {
            var oSUDeliveryPrograms = response.objs;
            if (oSUDeliveryPrograms.length > 0) {
                DynamicRefreshList([], "tblSUDPs");
                DynamicRefreshList(oSUDeliveryPrograms, "tblSUDPs");
            }
            else {
                alert("No List Found");
            }
        });

    });

    $("#btnProgramHistorySUDO").click(function () {
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
            
            ResetAllTables("tblProgramHistory", "tblChallanHistory");
            var oSUDeliveryProgram = response.objs;
            if (oSUDeliveryProgram.SUDeliveryPrograms.length > 0) {
                $("#tabProgramChallanHistory").tabs("select", 0);
                $("#winProgramChallanHistory").icsWindow("open", "DO No : " + oSUDeliveryProgram.DONo + " @ Qty : " + formatPrice(oSUDeliveryProgram.DOTotalQty) + " KG");


                if (oSUDeliveryProgram.SUDeliveryPrograms.length > 0) {
                    
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
}

function IntializePickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        SetPickerValueAssign(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which === 13) {
            SetPickerValueAssign(oPickerobj);
        }
    });
}

function SetPickerValueAssign(oPickerobj) {
    var oreturnObj = null, oreturnobjs = [];
    if (oPickerobj.multiplereturn) {
        oreturnobjs = $('#' + oPickerobj.tableid).datagrid('getChecked');
    } else {
        oreturnObj = $('#' + oPickerobj.tableid).datagrid('getSelected');
    }

    if (oPickerobj.winid == 'winSUDPs') {
        if (oreturnobjs != null && oreturnobjs.length > 0) {

        }
    }
    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
}

function OpenUpdatePicker() {
    var oSUDP1 = $("#tblSUDPs").datagrid("getSelected");

    
    $.icsDataGet({
        BaseAddress: _sBaseAddress,
        Object: oSUDP1,
        ControllerName: "SUDeliveryProgram",
        ActionName: "Get",
        IsWinClose: false
    }, function (response) {
        if (response.status && response.obj.SUDeliveryProgramID > 0) {
            RefreshControlSUDeliveryProgram(response.obj);
            $("#winSUDeliveryProgram").icsWindow('open', "Update Delivery Program");
            RefreshLayoutSUDeliveryProgram("btnUpdateSUDP");
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
    DynamicRefreshList(_oSUDeliveryOrder.SUDeliveryOrderDetails, "tblSUDeliveryOrderDetails");
    RefreshTotalSummery();
    $("#txtDONoSUDeliveryOrder").val(_oSUDeliveryOrder.DONo);
    _nBuyerID = _oSUDeliveryOrder.BuyerID;
    $("#txtSearchByBuyerSUDeliveryOrder").val(_oSUDeliveryOrder.BuyerName);
    _nDeliveryTo = _oSUDeliveryOrder.DeliveryTo;
    $("#txtSearchByDeliveryToSUDeliveryOrder").val(_oSUDeliveryOrder.DeliveredToName);
    $("#txtDeliverPointSUDeliveryOrder").val(_oSUDeliveryOrder.DeliveryPoint);
    $("#txtRemarksSUDeliveryOrder").val(_oSUDeliveryOrder.Remarks);
    $('#txtDODateSUDeliveryOrder').datebox('setValue', _oSUDeliveryOrder.DODateSt);
    $('#txtDeliveryDateSUDeliveryOrder').datebox('setValue', _oSUDeliveryOrder.DeliveryDateSt);
    $("#cboMKTPersonSUDeliveryOrder").val(_oSUDeliveryOrder.MKTPersonID);
    $("#cboDOTypeSUDeliveryOrder").val(_oSUDeliveryOrder.DOTypeInInt);
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
function RefreshSUDPs() {
     
    var oSUDeliveryProgram = { ErrorMessage: '' };
    oSUDeliveryProgram.ErrorMessage = 'Arguments;';
    
    oSUDeliveryProgram.ErrorMessage = oSUDeliveryProgram.ErrorMessage + $.trim($("#txtProgramDateSUDP").datebox('getValue')) + '~';

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oSUDeliveryProgram,
        ControllerName: "SUDeliveryProgram",
        ActionName: "Gets",
        IsWinClose: false
    }
    $.icsDataGets(obj, function (response) {
         
        if (response.status) {
            if (response.objs.length > 0) {
                _oDBSUDeliveryPrograms = response.objs;
                DynamicRefreshList(_oDBSUDeliveryPrograms, 'tblSUDPs');
            }
            else {
                alert('No Data Found!');
                DynamicRefreshList([], 'tblSUDPs');
            }
        }
    });
}
var _nQty = 0;
var _nIndex = -1;
var _bIsValid = false;
function onClickCell(index, field, value) {
    _isClickUpdateBtn = false;
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
    //if (icsdateparser(oSUDP.ProgramDateInString) < icsdateparser(icsdateformat(new Date()))) { alert('Can not Change Previous Program!'); return false; } else { _bIsValid = true; }
    
    if (changes.ProgramQty != null) {
        debugger;
        if (_nQty != parseFloat(changes.ProgramQty)) {
            if (oSUDP.YetToDOProgram < parseFloat(changes.ProgramQty)) {
                if ($.trim(_sErrorMessage) == "") {
                    //_sErrorMessage = 'Your Entered Program Qty: ' + changes.ProgramQty + ' is More than Yet To Program Qty: ' + oSUDP.YetToDOProgram + '.\n Maximum Program Qty: ' + oSUDP.YetToDOProgram + ' will be saved!';
                    _sErrorMessage = "Your Entered Program Qty (" + formatPrice(changes.ProgramQty) + ") Cannot be greater than DO Qty (" + formatPrice(oSUDP.YetToDOProgram) + ")";
                    alert(_sErrorMessage);
                    changes.ProgramQty = oSUDP.YetToDOProgram;
                    oSUDP.ProgramQty = oSUDP.YetToDOProgram;
                    _bIsValid = false;
                    $('#tblSUDPs').datagrid('updateRow', { index: index, row: oSUDP });
                }
            }
            else if (oSUDP.YetToDelivery < parseFloat(changes.ProgramQty)) {
                if ($.trim(_sErrorMessage) == "") {
                    //_sErrorMessage = 'Your Entered Program Qty: ' + changes.ProgramQty + ' is More than Yet To Delivery Qty: ' + oSUDP.YetToDelivery + '.\n Maximum Program Qty: ' + oSUDP.YetToDelivery + ' will be saved!';
                    _sErrorMessage = "Sorry, Remaining Delivery Qty is " + formatPrice(oSUDP.YetToDelivery);
                    alert(_sErrorMessage);
                    changes.ProgramQty = oSUDP.YetToDelivery;
                    oSUDP.ProgramQty = oSUDP.YetToDelivery;
                    _bIsValid = false;
                    $('#tblSUDPs').datagrid('updateRow', { index: index, row: oSUDP });
                }
            }
            //else if (oSUDP.ChallanQty < parseFloat(changes.ProgramQty)) {
            //    if ($.trim(_sErrorMessage) == "") {
            //        //_sErrorMessage = 'Your Entered Program Qty: ' + changes.ProgramQty + ' is More than Yet To Delivery Qty: ' + oSUDP.YetToDelivery + '.\n Maximum Program Qty: ' + oSUDP.YetToDelivery + ' will be saved!';
            //        _sErrorMessage = "Sorry, Already " + formatPrice(oSUDP.ChallanQty) + " Qty of Challan is Done.";
            //        alert(_sErrorMessage);
            //        changes.ProgramQty = oSUDP.ChallanQty;
            //        oSUDP.ProgramQty = oSUDP.ChallanQty;
            //        _bIsValid = false;
            //        $('#tblSUDPs').datagrid('updateRow', { index: index, row: oSUDP });
            //    }
            //}
            else {
                _bIsValid = true;
                _sErrorMessage = "";
            }
            if (oSUDP.ProgramQty > 0) {
                oSUDP.ProgramQty = parseFloat(changes.ProgramQty);
                oSUDP.ProgramDate = $.trim($('#txtProgramDateSUDP').datebox('getValue'));
                $('#tblSUDPs').datagrid('updateRow', { index: index, row: oSUDP });
                Save(oSUDP, index);
            }
            _sErrorMessage = "";
            return true;
        }
        else if (_nQty == parseFloat(changes.ProgramQty)) {
            oSUDP.ProgramQty = parseFloat(changes.ProgramQty);
            $('#tblSUDPs').datagrid("updateRow", { index: index, row: oSUDP });
        }
        _sErrorMessage = "";
    }
}



function Save(oSUDP, index) {
    
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SUDeliveryProgram/Save",
        traditional: true,
        data: JSON.stringify(oSUDP),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            
            var oSUDProgram = jQuery.parseJSON(data);
            $("#tblSUDPs").datagrid("selectRow", index);
            if (parseInt(oSUDProgram.SUDeliveryProgramID) > 0) {
                if ($.trim(oSUDProgram.ErrorMessage) == "")
                {
                    var nRowIndex = $('#tblSUDPs').datagrid("getRowIndex", oSUDP);
                    $('#tblSUDPs').datagrid("updateRow", { index: nRowIndex, row: oSUDProgram });
                    if (_isClickUpdateBtn)
                    {
                        OpenUpdatePicker();
                        _bIsValid = false;
                    }
                }
            }
            else {
                var oSUDP1 = $("#tblSUDPs").datagrid("getSelected");
                oSUDP1.ProgramQty = _nQty;
                var nRowIndex = $('#tblSUDPs').datagrid("getRowIndex", oSUDP);
                $("#tblSUDPs").datagrid("selectRow", nRowIndex);
                $('#tblSUDPs').datagrid("updateRow", { index: nRowIndex, row: oSUDP1 });
                alert(oSUDProgram.ErrorMessage);
            }
            
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function GenerateTableColumnsSUDeliveryProgram() {
    _otblColumns = [];

    var oColDONo = { field: "DONo", width: "12%", title: "DO No", frozen: true };
    var oColProductName = { field: "ProductName", width: "12%", title: "Item Description", frozen: true };
    var oColPIQty = { field: "PIQty", width: "10%", title: "PI Qty", align: 'right', frozen: true };
    var oColDOQty = { field: "DOQty", width: "10%", title: "DO Qty", align: 'right', frozen: true };
    //var oColYetToDOProgram = { field: "YetToDOProgram", width: "10%", title: "Yet To Program", align: 'right', frozen: true };
    //var oColYetToDelivery = { field: "YetToDelivery", width: "10%", title: "Yet To Delivery", align: 'right', frozen: true };
    var oColChallanQty = { field: "ChallanQty", width: "10%", title: "Challan Qty", align: 'right', frozen: true };
    var oColReadyStockInHand = { field: "ReadyStockInHand", width: "10%", title: "Stock In Hand", align: 'right', frozen: true };
    var oColProgramDateInString = { field: "ProgramDateInString", width: "15%", title: "Program Date", frozen: true };
    var oColProgramQty = { field: "ProgramQty", width: "13%", title: "Program Qty", editor: { type: 'numberbox', options: { precision: 2 } }, align: 'right', frozen: true };
    
    //_otblColumns.push(oColDONo, oColProductName, oColPIQty, oColDOQty, oColYetToDOProgram, oColYetToDelivery, oColReadyStockInHand, oColProgramDateInString, oColProgramQty);
    _otblColumns.push(oColDONo, oColProductName, oColPIQty, oColDOQty, oColChallanQty, oColReadyStockInHand, oColProgramDateInString, oColProgramQty);

    $('#tblSUDPs').datagrid({ columns: [_otblColumns] });
    $('#tblSUDPs').datagrid({ onClickCell: function (index, field, value) { return onClickCell(index, field, value); }, onEndEdit: function (index, row, changes) { return EndEdit(index, row, changes); } });
}

function GenerateTableColumnsSUDeliveyOrder() {
    _otblColumns = [];
    var oColDONo = { field: "DONo", width: "25%", title: "DO No", frozen: true };
    var oColDOTypeSt = { field: "DOTypeSt", width: "12%", title: "DO Type", frozen: true };
    var oColDOStatusSt = { field: "DOStatusSt", width: "12%", title: "DO Status", frozen: true };
    var oColDODateSt = { field: "DODateSt", width: "18%", title: "DO Date", frozen: true };
    var oColDeliveryDateSt = { field: "DeliveryDateSt", width: "18%", title: "Delivery Date", frozen: true };
    var oColBuyerName = { field: "BuyerName", width: "15%", title: "Buyer", frozen: true };
    _otblColumns.push(oColDONo, oColDOTypeSt, oColDOStatusSt, oColDODateSt, oColDeliveryDateSt, oColBuyerName);
    $('#tblSUDOs').datagrid({ columns: [_otblColumns] });
}