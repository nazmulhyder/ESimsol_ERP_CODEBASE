var _oBuyers = [];
var _oDeliveryTos = [];
var _oProducts = [];
var _oSelectedProduct = null;
var _nBuyerID = 0;
var _nDeliveryTo = 0;
var _nExportPIID = 0;
var _oExportPIs = [];
var _oSelectedProductSUDODetail = null;
var _oProductsSUDODetail = [];
var _oContactPersonnels = [];
var _oExportPISUDO = null;
var _isValidAmount = true;
var _oTempObjForAlert = {
    Currency: "",
    DetailTotalAmount: "",
    TotalPIAmount: ""
}
function InitializeSUDeliveryOrderEvents() {
    $("#btnCloseSUDeliveryOrder").click(function () {
        $("#winSUDeliveryOrder").icsWindow("close");
        _oSUDeliveryOrder = null;
    });

    LoadComboBoxes();
    SelectBuyerFromPiker();
    SelectDeliveryToFromPiker();
    SelectPIFromPiker();

    $("#btnPINoSUDeliveryOrder").click(function () {
        if (parseInt($("#cboDOTypeSUDeliveryOrder").val()) == 0) {
            alert("Please select a DO Type.");
            $("#cboDOTypeSUDeliveryOrder").addClass("errorFieldBorder");
            $("#cboDOTypeSUDeliveryOrder").focus();
            return false;
        } else {
            $("#cboDOTypeSUDeliveryOrder").removeClass("errorFieldBorder");
        }
        if (_nBuyerID <= 0) {
            alert("Please select a buyer.");
            $("#txtSearchByBuyerSUDeliveryOrder").addClass("errorFieldBorder");
            $("#txtSearchByBuyerSUDeliveryOrder").focus();
            return false;
        }
        $("#txtSearchByBuyerSUDeliveryOrder").removeClass("errorFieldBorder");
        var nDOType=parseInt($("#cboDOTypeSUDeliveryOrder").val());
        var nPaymentTypeInInt=0;
        if (nDOType === 2)
        {
            nPaymentTypeInInt = 2;
        }

        var oExportPI = {
            ContractorID: parseInt(_nBuyerID),
            PaymentTypeInInt: nPaymentTypeInInt
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportPI/GetsByContractorForSU",
            traditional: true,
            data: JSON.stringify(oExportPI),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oExportPIs = jQuery.parseJSON(data);
                if (oExportPIs.length > 0) {
                    _oExportPIs = oExportPIs;
                    DynamicRefreshList(oExportPIs, "tblPINosPicker");
                    $("#winPINo").icsWindow("Open", "Export PI of Buyer " + $("#txtSearchByBuyerSUDeliveryOrder").val());
                    $("#divPINoPicker").focus();
                }
                else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblPINosPicker");
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnClearPINoSUDeliveryOrder").click(function () {
        $("#txtPINoSUDeliveryOrder").removeClass("fontColorOfPickItem");
        $("#txtPINoSUDeliveryOrder").val("");
        _nExportPIID = 0;
        _oExportPISUDO = null;
        _oExportPIs = [];
        $(".buyer").prop("disabled", false);
        $("#txtContactInfoSUDeliveryOrder").prop("disabled", true);
    });

    $("#btnSaveSUDeliveryOrder").click(function () {

        if (!ValidateInputSUDeliveryOrder()) return;
        
        var oSUDeliveryOrder = RefreshObjectSUDeliveryOrder();

        if (!_isValidAmount) {
            alert("Sorry, Total PI Amount is " + _oTempObjForAlert.Currency + " " + _oTempObjForAlert.DetailTotalAmount + " But Your Given Product(s) Total Amount is " + _oTempObjForAlert.TotalPIAmount);
            return false;
        }
       
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oSUDeliveryOrder,
            ObjectId: oSUDeliveryOrder.SUDeliveryOrderID,
            ControllerName: "SUDeliveryOrder",
            ActionName: "SaveSUDeliveryOrder",
            TableId: "tblSUDeliveryOrders",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            debugger;
            if (response.status && response.obj != null) {
                if (response.obj.SUDeliveryOrderID > 0) {
                    _oSUDeliveryOrder = null;
                }
            }
        });
    });

    $("#btnSaveAcceptReviseSUDeliveryOrder").click(function () {
        if (!ValidateInputSUDeliveryOrder(false)) return;
        var oSUDO = $("#tblSUDeliveryOrders").datagrid("getSelected");
        var nRowIndex = $('#tblSUDeliveryOrders').datagrid('getRowIndex', oSUDO);
        var oSUDeliveryOrder = RefreshObjectSUDeliveryOrder();
        var oSUDeliveryOrderDetails = $('#tblSUDeliveryOrderDetails').datagrid('getRows');

        if (oSUDeliveryOrderDetails.length == 0) {
            alert("Please add minimum one product.");
            return false;
        }
        else {
            for (var i = 0; i < oSUDeliveryOrderDetails.length; i++) {
                if (oSUDeliveryOrderDetails[i].Qty == 0 || oSUDeliveryOrderDetails[i].UnitPrice == 0) {
                    alert("Qty or Unit Price of any product cannot be zero.");
                    return false;
                }
            }
        }
        oSUDeliveryOrder.SUDeliveryOrderDetails = oSUDeliveryOrderDetails;
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUDeliveryOrder/AcceptDORevise",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrder),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryOrder = jQuery.parseJSON(data);
                if (oSUDeliveryOrder != null) {
                    if (oSUDeliveryOrder.SUDeliveryOrderID > 0) {
                        alert("Accept Revise Successful.");
                        $('#tblSUDeliveryOrders').datagrid('updateRow', { index: nRowIndex, row: oSUDeliveryOrder });
                        $("#winSUDeliveryOrder").icsWindow("close");
                    }
                    else {
                        alert("Error Found.");
                        return false;
                    }
                }
                else {
                    alert("Not found.");
                    return false;
                }
            }
        });


    });

    $('#cboDOTypeSUDeliveryOrder').change(function (e) {
        var oSUDODs = $('#tblSUDeliveryOrderDetails').datagrid('getRows');
        if (parseInt($('#cboDOTypeSUDeliveryOrder').val()) === 6) {
            GenerateTableColumnsSUDODForSampleDO();
            $('#TotalSummerySUDOD').hide();
            $('#TotalSummerySUDODForSampleDO').show();
        }
        else {
            GenerateTableColumnsSUDOD();
            $('#TotalSummerySUDOD').show();
            $('#TotalSummerySUDODForSampleDO').hide();
        }
        DynamicRefreshList(oSUDODs, "tblSUDeliveryOrderDetails");


        var oTempSUDODetails = [];
        if (parseInt($("#cboDOTypeSUDeliveryOrder").val()) == 1) {
            CheckForAdvanceDO();
        }
        else {
            DynamicRefreshList(oSUDODs, "tblSUDeliveryOrderDetails");
            RefreshTotalSummery();

            $("#btnDisplayAdvanceDOs").hide();
            $("#txtPINoSUDeliveryOrder").width("70%");
        }

    });

    //function CheckForAdvanceDO() {
    //    if (_nExportPIID == 0) {
    //        alert("Invalid Export PI");
    //        return false;
    //    }
    //    var oSUDeliveryOrder = {
    //        ExportPIID: _nExportPIID
    //    };
    //    $.icsDataGets({
    //        BaseAddress: _sBaseAddress,
    //        Object: oSUDeliveryOrder,
    //        ControllerName: 'SUDeliveryOrder',
    //        ActionName: 'GetsByExportPI',
    //        IsWinClose: false
    //    }, function (response) {
    //        if (response.status) {
    //            if (response.objs.length > 0) {
    //                var oSUDeliveryOrderDetails = response.objs;
    //                if (oSUDeliveryOrderDetails.length > 0) {
    //                    _oSUDeliveryOrderDetails = oSUDeliveryOrderDetails;
    //                    $("#btnDisplayAdvanceDOs").show();
    //                    $("#txtPINoSUDeliveryOrder").width("53%");
    //                }
    //                else {
    //                    _oSUDeliveryOrderDetails = [];
    //                    $("#btnDisplayAdvanceDOs").hide();
    //                    $("#txtPINoSUDeliveryOrder").width("71%");
    //                }
    //            }
    //        }
    //    });
    //}

    //SU Delivery Order

    PickPIProdcutsSUDODetail();

    PickProductSUDODetail();

    SUDODetailRelated();

    $("#cboContactPersonnelSUDeliveryOrder").change(function () {
        var nBPCID = $(this).val();
        GetContactPersonnelInfo(nBPCID);
    });



    //End SU Delivery Order
}

function CheckForAdvanceDO() {
    if (_oSUDeliveryOrder.ExportPIID == 0) {
        return false;
    }
    var oSUDeliveryOrder = {
        ExportPIID: _oSUDeliveryOrder.ExportPIID
    };
    $.icsDataGets({
        BaseAddress: _sBaseAddress,
        Object: oSUDeliveryOrder,
        ControllerName: 'SUDeliveryOrder',
        ActionName: 'GetsByExportPI',
        IsWinClose: false
    }, function (response) {
        if (response.status) {
            if (response.objs.length > 0) {
                var oSUDeliveryOrderDetails = response.objs;
                if (oSUDeliveryOrderDetails.length > 0) {
                    _oSUDeliveryOrderDetails = oSUDeliveryOrderDetails;
                    $("#btnDisplayAdvanceDOs").show();
                    $("#txtPINoSUDeliveryOrder").width("53%");

                    var oTempSUDODetails = [];
                    for (var i = 0; i < _oSUDODetailsMainList.length; i++) {
                        var oTempObj = SetRemainingQty(_oSUDODetailsMainList[i]);

                        var oTempSUDODetail = {
                            SUDeliveryOrderDetailID: oTempObj.SUDeliveryOrderDetailID,
                            SUDeliveryOrderID: (_oSUDeliveryOrder == null ? 0 : oTempObj.SUDeliveryOrderID),
                            ProductID: oTempObj.ProductID,
                            ExportPIDetailID: oTempObj.ExportPIDetailID,
                            MUnitID: oTempObj.MUnitID,
                            Qty: oTempObj.Qty,
                            UnitPrice: oTempObj.UnitPrice,
                            Amount: parseFloat(oTempObj.Qty * oTempObj.UnitPrice),
                            AmountSt: formatPrice(oTempObj.Qty * oTempObj.UnitPrice, 2),
                            ProductCode: oTempObj.ProductCode,
                            ProductName: oTempObj.ProductName,
                            QtyInLbs: GetLBS(oTempObj.Qty),
                            UnitPriceInLbs: parseFloat((oTempObj.UnitPrice / 2.20462).toFixed(2))
                        };
                        oTempSUDODetails.push(oTempSUDODetail);
                    }
                    DynamicRefreshList(oTempSUDODetails, "tblSUDeliveryOrderDetails");
                    RefreshTotalSummery();
                }
                else {
                    _oSUDeliveryOrderDetails = [];
                    $("#btnDisplayAdvanceDOs").hide();
                    $("#txtPINoSUDeliveryOrder").width("70%");
                }
            }
        }
    });
}


function GetContactPersonnelInfo(nBPCID) {
    for (var i = 0; i < _oContactPersonnels.length; i++) {
        if (_oContactPersonnels[i].ContactPersonnelID == parseInt(nBPCID)) {
            $("#txtContactInfoSUDeliveryOrder").val(_oContactPersonnels[i].Phone);
            break;
        }
    }
}

function LoadContactPersonnel(oContractor, nBCPID) {
    $.icsDataGets({
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: 'SUDeliveryOrder',
        ActionName: 'GetContractorPersonnel',
        IsWinClose: false
    }, function (response) {
        if (response.status) {
            if (response.objs.length > 0) {
                _oContactPersonnels = response.objs;
                $("#cboContactPersonnelSUDeliveryOrder").icsLoadCombo({
                    List: _oContactPersonnels,
                    OptionValue: "ContactPersonnelID",
                    DisplayText: "Name",
                    InitialValue: "No Select one option"
                });
                $("#cboContactPersonnelSUDeliveryOrder").val(nBCPID);
                GetContactPersonnelInfo(nBCPID);
            }
        }
    });
}

function RefreshObjectSUDeliveryOrder() {
    endEditing();
    var oSUDeliveryOrde = {
        SUDeliveryOrderID: (_oSUDeliveryOrder != null) ? _oSUDeliveryOrder.SUDeliveryOrderID : 0,
        DONo: $("#txtDONoSUDeliveryOrder").val(),
        DOTypeInInt: $("#cboDOTypeSUDeliveryOrder").val(),
        BuyerID: parseInt(_nBuyerID),
        BCPID: parseInt($("#cboContactPersonnelSUDeliveryOrder").val()),
        DeliveryTo: parseInt(_nDeliveryTo),
        DeliveryPoint: $("#txtDeliverPointSUDeliveryOrder").val(),
        Remarks: $("#txtRemarksSUDeliveryOrder").val(),
        DODate: $('#txtDODateSUDeliveryOrder').datebox('getValue'),
        DeliveryDate: $('#txtDeliveryDateSUDeliveryOrder').datebox('getValue'),
        MKTPersonID: $("#cboMKTPersonSUDeliveryOrder").val(),
        ExportPIID: parseInt(_nExportPIID),
        CurrencyID: $("#cboCurrencySUDeliveryOrder").val()
    };
    return oSUDeliveryOrde;
}

function ValidateInputSUDeliveryOrder() {
    if (parseInt($("#cboDOTypeSUDeliveryOrder").val()) == 0) {
        alert("Please select DO Type.");
        $("#cboDOTypeSUDeliveryOrder").addClass("errorFieldBorder");
        $("#cboDOTypeSUDeliveryOrder").focus();
        return false;
    } else {
        $("#cboDOTypeSUDeliveryOrder").removeClass("errorFieldBorder");
    }

    if (_nBuyerID == 0) {
        alert("Please select buyer.");
        $("#txtSearchByBuyerSUDeliveryOrder").addClass("errorFieldBorder");
        $("#txtSearchByBuyerSUDeliveryOrder").focus();
        return false;
    } else {
        $("#txtSearchByBuyerSUDeliveryOrder").removeClass("errorFieldBorder");
    }
  
    if (_nDeliveryTo == 0) {
        alert("Please select delivery to.");
        $("#txtSearchByDeliveryToSUDeliveryOrder").addClass("errorFieldBorder");
        $("#txtSearchByDeliveryToSUDeliveryOrder").focus();
        return false;
    } else {
        $("#txtSearchByDeliveryToSUDeliveryOrder").removeClass("errorFieldBorder");
    }

    if ($.trim($("#txtDeliverPointSUDeliveryOrder").val()) == 0) {
        alert("Please select delivery point.");
        $("#txtDeliverPointSUDeliveryOrder").addClass("errorFieldBorder");
        $("#txtDeliverPointSUDeliveryOrder").focus();
        return false;
    } else {
        $("#txtDeliverPointSUDeliveryOrder").removeClass("errorFieldBorder");
    }

    if (parseInt($("#cboContactPersonnelSUDeliveryOrder").val()) == 0) {
        alert("Please select Contact Personnel.");
        $("#cboContactPersonnelSUDeliveryOrder").addClass("errorFieldBorder");
        $("#cboContactPersonnelSUDeliveryOrder").focus();
        return false;
    } else {
        $("#cboContactPersonnelSUDeliveryOrder").removeClass("errorFieldBorder");
    }

    if (parseInt($("#cboMKTPersonSUDeliveryOrder").val()) == 0) {
        alert("Please select MKT Person.");
        $("#cboMKTPersonSUDeliveryOrder").addClass("errorFieldBorder");
        $("#cboMKTPersonSUDeliveryOrder").focus();
        return false;
    } else {
        $("#cboMKTPersonSUDeliveryOrder").removeClass("errorFieldBorder");
    }

    if (parseInt($("#cboDOTypeSUDeliveryOrder").val()) == 1 || parseInt($("#cboDOTypeSUDeliveryOrder").val()) == 2) {
        if (_nExportPIID == 0) {
            alert("Please select Export PI No.");
            $("#btnPINoSUDeliveryOrder").addClass("errorFieldBorder");
            $("#btnPINoSUDeliveryOrder").focus();
            return false;
        } else {
            $("#btnPINoSUDeliveryOrder").removeClass("errorFieldBorder");
        }
    }

    if (parseInt($("#cboCurrencySUDeliveryOrder").val()) == 0) {
        alert("Please select Currency.");
        $("#cboCurrencySUDeliveryOrder").addClass("errorFieldBorder");
        $("#cboCurrencySUDeliveryOrder").focus();
        return false;
    } else {
        $("#cboCurrencySUDeliveryOrder").removeClass("errorFieldBorder");
    }

    return true;
}


function CheckAmountSUDO() {
    //Total Amount Can't be greater than PI Amount
    
    var nDOType = $("#cboDOTypeSUDeliveryOrder").val();
    if (parseInt(nDOType) == 1 || parseInt(nDOType) == 2) //Export DO = 1 & Local DO = 2
    {
        var nTotalAmount = 0;
        
        var oSUDeliveryOrderDetails = $("#tblSUDeliveryOrderDetails").datagrid("getRows");
        if (oSUDeliveryOrderDetails.length > 0) {
            for (var i = 0; i < oSUDeliveryOrderDetails.length; i++) {
                var nAmount = parseFloat(oSUDeliveryOrderDetails[i].Qty) * parseFloat(oSUDeliveryOrderDetails[i].UnitPrice);
                nTotalAmount = parseFloat(nTotalAmount) + parseFloat(nAmount);
            }
        }
        else {
            _isValidAmount = true;
        }
        if (_oExportPISUDO != null) {
            if (parseFloat(nTotalAmount) > parseFloat(_oExportPISUDO.Amount)) {
                _oTempObjForAlert = {
                    Currency: _oExportPISUDO.Currency,
                    DetailTotalAmount: formatPrice(_oExportPISUDO.Amount, 2),
                    TotalPIAmount: formatPrice(nTotalAmount, 2)
                }
                //alert("Sorry, Total PI Amount is " + _oExportPISUDO.Currency + " " + formatPrice(_oExportPISUDO.Amount, 2) + " But Your Given Total Product Amount is " + formatPrice(nTotalAmount, 2));
                _isValidAmount = false;
            }
            else {
                _isValidAmount = true;
            }
        }
    }
    else {
        _isValidAmount = true;
    }
}

function PickPIProdcutsSUDODetail() {
    $("#btnPIProductsSUDeliveryOrderDetail").click(function () {
        if (_nExportPIID <= 0) {
            alert("Please select a PI.");
            $("#btnPINoSUDeliveryOrder").addClass("errorFieldBorder");
            $("#btnPINoSUDeliveryOrder").focus();
            return false;
        }
        else {
            $("#btnPINoSUDeliveryOrder").removeClass("errorFieldBorder");
        }
        if (!ValidateInputSUDeliveryOrder()) { return false; }
        
        var oExportPI = {
            ExportPIID: parseInt(_nExportPIID)
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportPI/GetsExportPIDetailForSU",
            traditional: true,
            data: JSON.stringify(oExportPI),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oExportPIDetails = jQuery.parseJSON(data);
                if (oExportPIDetails.length > 0) {
                    DynamicRefreshListForMultipleSelection(oExportPIDetails, "tblExportPIProductsPicker");
                    $("#winExportPIProducts").icsWindow("Open", "Product");
                    $("#divExportPIProductsPicker").focus();
                }
                else {
                    alert('No Data Found.');
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnCloseExportPIProductsPicker").click(function () {
        $("#winExportPIProducts").icsWindow("close");
    });

    $("#btnOkExportPIProductsPicker").click(function () {
        var oExportDetails = $("#tblExportPIProductsPicker").datagrid("getChecked");
        if (oExportDetails.length == 0) {
            alert("Please select minimum one item from list.");
            return false;
        }
        $("#winExportPIProducts").icsWindow("close");
         
        var oEDs = [];
        if (_oSUDeliveryOrder == null) {
            if (!ValidateInputSUDeliveryOrder()) return;
            var oSUDeliveryOrder = RefreshObjectSUDeliveryOrder();
            _oSUDeliveryOrder = oSUDeliveryOrder;
            for (var i = 0; i < oExportDetails.length; i++) {
                if (i == 0) {
                    oExportDetails[i].SUDeliveryOrder = oSUDeliveryOrder;
                } else {
                    oExportDetails[i].SUDeliveryOrder = null;
                }
                oEDs.push(oExportDetails[i]);
            }
        }
        else {
            for (var i = 0; i < oExportDetails.length; i++) {
                oExportDetails[i].SUDeliveryOrderID = _oSUDeliveryOrder.SUDeliveryOrderID;
                oEDs.push(oExportDetails[i]);
            }
        }

        var oSUDeliveryOrderDetail = {
            SUDeliveryOrderDetailID: 0,
            SUDeliveryOrderID : (_oSUDeliveryOrder == null) ? 0 : _oSUDeliveryOrder.SUDeliveryOrderID,
            SUDeliveryOrderDetails : oEDs
        };

        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUDeliveryOrder/SaveSUDeliveryOrderDetail",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrderDetail),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                
                var oSUDOs = [];
                var oSUDO = jQuery.parseJSON(data);
                oSUDOs = oSUDO.SUDeliveryOrderDetails;
                if (oSUDOs != null) {
                    
                    if (oSUDOs.length > 0) {
                        var oSUDeliveryOrderDetails = $("#tblSUDeliveryOrderDetails").datagrid("getRows");
                        var nIndex = oSUDeliveryOrderDetails.length;
                        if (oSUDeliveryOrderDetail.SUDeliveryOrderID <= 0) {
                            
                            if (oSUDOs[0].SUDeliveryOrder.SUDeliveryOrderID > 0) {
                                _oSUDeliveryOrder = oSUDOs[0].SUDeliveryOrder;
                                $("#txtDONoSUDeliveryOrder").val(_oSUDeliveryOrder.DONo);
                                var oSUDeliveryOrders = $("#tblSUDeliveryOrders").datagrid("getRows");
                                var nIndexSO = oSUDeliveryOrders.length;
                                $("#tblSUDeliveryOrders").datagrid("appendRow", oSUDOs[0].SUDeliveryOrder);
                                $("#tblSUDeliveryOrders").datagrid("selectRow", nIndexSO);
                            }
                        }
                        
                        for (var i = 0; i < oSUDOs.length; i++) {
                            $("#tblSUDeliveryOrderDetails").datagrid("appendRow", oSUDOs[i]);
                            $("#tblSUDeliveryOrderDetails").datagrid("selectRow", nIndex);
                            nIndex++;
                        }
                        RefreshTotalSummery();
                        //DynamicRefreshList(oSUDOs, "tblSUDeliveryOrderDetails");
                    }
                    else {
                        alert("No data found.");
                    }
                }
                else {
                    alert(oSUDO.ErrorMessage);
                }
            }
        });

    });
}

function SelectPIFromPiker() {
    $("#winPINo").on("keydown", function (e) {
        var oExportPI = $('#tblPINosPicker').datagrid('getSelected');
        var nIndex = $('#tblPINosPicker').datagrid('getRowIndex', oExportPI);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblPINosPicker').datagrid('selectRow', 0);
            }
            else {
                $('#tblPINosPicker').datagrid('selectRow', nIndex - 1);
            }
            $('#txtPINoSUDeliveryOrder').blur();
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblPINosPicker').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblPINosPicker').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblPINosPicker').datagrid('selectRow', nIndex + 1);
            }
            $('#txtPINoSUDeliveryOrder').blur();
        }
        if (e.which == 13)//enter=13
        {
            if (!SelectAnExportPI()) return false;
        }
    });

    $("#btnOkPINoPicker").click(function () {
        if (!SelectAnExportPI()) return false;
    });

    $("#btnClosePINoPicker").click(function () {
        $("#winPINo").icsWindow("close");
    });

    $("#txtSearchByPINoNamePicker").keyup(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "PINo",
            GlobalObjectList: _oExportPIs,
            TableId: "tblPINosPicker"
        };
        $('#txtSearchByPINoNamePicker').icsSearchByText(obj);
    });

    $("#btnRefreshPINoPicker").click(function () {
        if (parseInt($("#cboDOTypeSUDeliveryOrder").val()) == 0) {
            alert("Please select a DO Type.");
            $("#cboDOTypeSUDeliveryOrder").addClass("errorFieldBorder");
            $("#cboDOTypeSUDeliveryOrder").focus();
            return false;
        } else {
            $("#cboDOTypeSUDeliveryOrder").removeClass("errorFieldBorder");
        }
        if (_nBuyerID <= 0) {
            alert("Please select a buyer.");
            $("#txtSearchByBuyerSUDeliveryOrder").addClass("errorFieldBorder");
            $("#txtSearchByBuyerSUDeliveryOrder").focus();
            return false;
        }
        $("#txtSearchByBuyerSUDeliveryOrder").removeClass("errorFieldBorder");

        var nDOType = parseInt($("#cboDOTypeSUDeliveryOrder").val());
        var nPaymentTypeInInt = 0;
        if (nDOType === 2) {
            nPaymentTypeInInt = 2;
        }

        var oExportPI = {
            ContractorID: parseInt(_nBuyerID),
            PaymentTypeInInt: nPaymentTypeInInt
        }

        //var oExportPI = {
        //    ContractorID: parseInt(_nBuyerID),
        //    ContractorName: $("#txtSearchByPINoNamePicker").val()
        //}

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportPI/GetsByContractorForSU",
            traditional: true,
            data: JSON.stringify(oExportPI),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oExportPIs = jQuery.parseJSON(data);
                if (oExportPIs.length > 0) {
                    _oExportPIs = oExportPIs;
                    DynamicRefreshList(oExportPIs, "tblPINosPicker");
                    $("#winPINo").icsWindow("Open", "Export PI of Buyer " + $("#txtSearchByBuyerSUDeliveryOrder").val());
                    $("#divPINoPicker").focus();
                }
                else {
                    alert(oExportPIs[0].ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });
}

function SelectAnExportPI() {
    
    var oExportPI = $("#tblPINosPicker").datagrid("getSelected");
    if (oExportPI != null && oExportPI.ExportPIID > 0) {
        var nOrderType = parseInt($("#cboDOTypeSUDeliveryOrder").val());
        if (nOrderType == 1) //Export DO
        {
            if (oExportPI.LCID == 0) {
                alert("Sorry, No LC found for this PI.");
                $(".LCNoRelated").hide();
                return false;
            }
            else {
                SetExportPISUDO(oExportPI);
                $(".LCNoRelated").show();
                $("#lblLCNo").text(oExportPI.LCNo);
            }
        }
        else {
            SetExportPISUDO(oExportPI);
            $(".LCNoRelated").hide();
        }
    }
    else {
        alert("Please select an Export PI.");
        $(".LCNoRelated").hide();
    }
    return true;
}

function SetExportPISUDO(oExportPI) {
    $('#txtPINoSUDeliveryOrder').val(oExportPI.PINo);
    $("#txtPINoSUDeliveryOrder").addClass("fontColorOfPickItem");
    _oExportPISUDO = oExportPI;
    _nExportPIID = oExportPI.ExportPIID;
    $("#cboCurrencySUDeliveryOrder").focus();
    $(".buyer").prop("disabled", true);
    $("#cboCurrencySUDeliveryOrder").val(oExportPI.CurrencyID);
    var sCurrency = $("#cboCurrencySUDeliveryOrder option:selected").text();
    $("#lblCurrency").text(sCurrency);
    $("#winPINo").icsWindow("close");
}

function LoadComboBoxes() {
    $("#cboDOTypeSUDeliveryOrder").icsLoadCombo({
        List: _oDOTypes,
        OptionValue: "id",
        DisplayText: "Value"
    });
    $("#cboMKTPersonSUDeliveryOrder").icsLoadCombo({
        List: _oMktPersons,
        OptionValue: "EmployeeID",
        DisplayText: "Name"
    });
    $("#cboCurrencySUDeliveryOrder").icsLoadCombo({
        List: _oCurrencys,
        OptionValue: "CurrencyID",
        DisplayText: "CurrencyName",
        InitialValue:"No select one"
    });

    var oContractorTypes = [];
    for (var i = 0; i < _oContractorTypes.length; i++) {
        if (_oContractorTypes[i].Value == 2 || _oContractorTypes[i].Value == 3) {
            oContractorTypes.push(_oContractorTypes[i]);
        }
    }
    LoadCboItems(oContractorTypes);
}

function LoadCboItems(Items) {
    var listItems = "";
    for (i = 0; i < Items.length; i++) {
        if (Items[i].UserID != 0) {
            listItems += "<option value='" + Items[i].Value + "'>" + Items[i].Text + "</option>";
        }
    }
    $("#cboBuyerSUDeliveryOrder").html(listItems);
    $("#cboDeliveryToSUDeliveryOrder").html(listItems);
}

function SelectBuyerFromPiker() {
    $("#winBuyerPicker").on("keydown", function (e) {
        var oContractor = $('#tblBuyersPicker').datagrid('getSelected');
        var nIndex = $('#tblBuyersPicker').datagrid('getRowIndex', oContractor);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblBuyersPicker').datagrid('selectRow', 0);
            }
            else {
                $('#tblBuyersPicker').datagrid('selectRow', nIndex - 1);
            }
            $('#txtSearchByBuyerSUDeliveryOrder').blur();
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblBuyersPicker').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblBuyersPicker').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblBuyersPicker').datagrid('selectRow', nIndex + 1);
            }
            $('#txtSearchByBuyerSUDeliveryOrder').blur();
        }
        if (e.which == 13)//enter=13
        {
            GetSelectedBuyer();
        }
    });

    $("#txtSearchByBuyerSUDeliveryOrder").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            if ($.trim($("#txtSearchByBuyerSUDeliveryOrder").val()).length === 0) {
                alert("Please enter some text.");
                return false;
            }
            PickBuyerSUDeliveryOrder();
        }
        else if (e.keyCode === 08) {
            ClearBuyerInfoSUDO();
        }
    });

    $("#btnClrBuyerSUDeliveryOrder").click(function () {
        $("#txtSearchByBuyerSUDeliveryOrder").val("");
        ClearBuyerInfoSUDO();
    });

    $("#btnPickBuyerSUDeliveryOrder").click(function () {
        PickBuyerSUDeliveryOrder();
    });

    $("#btnOkBuyerPicker").click(function () {
        GetSelectedBuyer();
    });

    $("#txtSearchByBuyerNamePicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oBuyers,
            TableId: "tblBuyersPicker"
        };
        $('#txtSearchByBuyerNamePicker').icsSearchByText(obj);
    });

    $("#btnCloseBuyerPicker").click(function () {
        $("#winBuyerPicker").icsWindow("close");
        _oBuyers = [];
        $("#winBuyerPicker input").val("");
        DynamicRefreshList([], "tblBuyersPicker");
    });
}

function ClearBuyerInfoSUDO() {
    $("#txtSearchByBuyerSUDeliveryOrder").removeClass("fontColorOfPickItem");
    _nBuyerID = 0;
    $("#cboContactPersonnelSUDeliveryOrder").empty();
    $("#txtContactInfoSUDeliveryOrder").val("");
}

function GetSelectedBuyer() {
    var oContractor = $("#tblBuyersPicker").icsGetSelectedItem();
    if (oContractor != null && oContractor.ContractorID > 0) {
        $('#txtSearchByBuyerSUDeliveryOrder').val(oContractor.Name);
        $("#txtSearchByBuyerSUDeliveryOrder").addClass("fontColorOfPickItem");
        _nBuyerID = oContractor.ContractorID;
        $('#txtSearchByBuyerSUDeliveryOrder').focus();
        $.icsDataGet({
            BaseAddress: _sBaseAddress,
            Object: oContractor,
            ControllerName: 'SUDeliveryOrder',
            ActionName: 'GetContractorAddress',
            IsWinClose: false
        }, function (response) {
            if (response.status) {
                if (_nDeliveryTo == 0) {
                    _nDeliveryTo = _nBuyerID;
                    $('#txtSearchByDeliveryToSUDeliveryOrder').val(oContractor.Name);
                    $("#txtSearchByDeliveryToSUDeliveryOrder").addClass("fontColorOfPickItem");
                    $("#cboDeliveryToSUDeliveryOrder").val($("#cboBuyerSUDeliveryOrder").val());
                    GetDeliveryToContractorInfo();
                }
                //if (response.obj.ContractorAddressID > 0) {
                //    var oContractorAddress = response.obj;
                //    if (oContractorAddress.AddressType === 2) {
                //        $('#txtDeliverPointSUDeliveryOrder').val(oContractorAddress.Address === '' ? oContractor.Address : oContractorAddress.Address);
                //    }
                //}
                //else {
                //    $('#txtDeliverPointSUDeliveryOrder').val(oContractor.Address);
                //}
                _oContactPersonnels = response.obj.ContactPersonnels;
                $("#cboContactPersonnelSUDeliveryOrder").icsLoadCombo({
                    List: _oContactPersonnels,
                    OptionValue: "ContactPersonnelID",
                    DisplayText: "Name",
                    InitialValue: "No Select one option"
                });
                GetContactPersonnelInfo(parseInt($("#cboContactPersonnelSUDeliveryOrder").val()));
            }
        });
    }
}

function GetDeliveryToContractorInfo() {
    if (_nDeliveryTo > 0) {
        var oContractor = {
            ContractorID: _nDeliveryTo
        };

        $.icsDataGet({
            BaseAddress: _sBaseAddress,
            Object: oContractor,
            ControllerName: 'Contractor',
            ActionName: 'Get',
            IsWinClose: false
        }, function (response) {
            if (response.status) {
                if (response.obj.ContractorID > 0) {
                    var oContractor = response.obj;
                    _nDeliveryTo = oContractor.ContractorID;
                    $('#txtSearchByDeliveryToSUDeliveryOrder').val(oContractor.Name);
                    $("#txtSearchByDeliveryToSUDeliveryOrder").addClass("fontColorOfPickItem");
                    $("#txtDeliverPointSUDeliveryOrder").val(oContractor.Address);
                    $("#cboDeliveryToSUDeliveryOrder").val(oContractor.ContractorType);
                }
            }
        });
    }
}

function LoadContractorAddress() {
   
}
function PickBuyerSUDeliveryOrder() {
    var oContractor = null;
    var cboBuyer = parseInt($("#cboBuyerSUDeliveryOrder").val());
    var oContractor = {
        Params: cboBuyer + '~' + $.trim($("#txtSearchByBuyerSUDeliveryOrder").val())
    };
   
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByNameType",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblContractorsPicker");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                _oBuyers = [];
                _oBuyers = response.objs;
                DynamicRefreshList(response.objs, "tblBuyersPicker");
                $("#winBuyerPicker").icsWindow('open', "Contractor Search");
                $("#divBuyerPicker").focus();
                $("#winBuyerPicker input").val("");
            }
            else {
                DynamicRefreshList([], "tblBuyersPicker");
                alert(response.objs[0].ErrorMessage);
                $("#txtSearchByBuyerSUDeliveryOrder").removeClass("fontColorOfPickItem");
                _nBuyerID = 0;
            }
        }
        else {
            alert("Sorry, No data found, Try again.");
            $("#txtSearchByBuyerSUDeliveryOrder").removeClass("fontColorOfPickItem");
            _nBuyerID = 0;
        }
    });
}

function SelectDeliveryToFromPiker() {
    $("#winDeliveryToPicker").on("keydown", function (e) {
        var oContractor = $('#tblDeliveryTosPicker').datagrid('getSelected');
        var nIndex = $('#tblDeliveryTosPicker').datagrid('getRowIndex', oContractor);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblDeliveryTosPicker').datagrid('selectRow', 0);
            }
            else {
                $('#tblDeliveryTosPicker').datagrid('selectRow', nIndex - 1);
            }
            $('#txtSearchByDeliveryToSUDeliveryOrder').blur();
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblDeliveryTosPicker').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblDeliveryTosPicker').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblDeliveryTosPicker').datagrid('selectRow', nIndex + 1);
            }
            $('#txtSearchByDeliveryToSUDeliveryOrder').blur();
        }
        if (e.which == 13)//enter=13
        {
            SelectDeliveyToContractor();
        }
    });

    function SelectDeliveyToContractor() {
        var oContractor = $("#tblDeliveryTosPicker").datagrid("getSelected");
        if (oContractor != null && oContractor.ContractorID > 0) {
            $('#txtSearchByDeliveryToSUDeliveryOrder').val(oContractor.Name);
            $("#txtSearchByDeliveryToSUDeliveryOrder").addClass("fontColorOfPickItem");
            $("#txtDeliverPointSUDeliveryOrder").val(oContractor.Address);
            _nDeliveryTo = oContractor.ContractorID;
            $("#winDeliveryToPicker").icsWindow("close");
        }
        else {
            alert("Please select an item from list.");
        }
    }

    $("#txtSearchByDeliveryToSUDeliveryOrder").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            if ($.trim($("#txtSearchByDeliveryToSUDeliveryOrder").val()).length === 0) {
                alert("Please enter some text.");
                return false;
            }
            PickDeliveryToSUDeliveryOrder();
        }
        else if (e.keyCode === 08) {
            $("#txtSearchByDeliveryToSUDeliveryOrder").removeClass("fontColorOfPickItem");
            _nDeliveryTo = 0;
        }
    });

    $("#btnClrDeliveryToSUDeliveryOrder").click(function () {
        $("#txtSearchByDeliveryToSUDeliveryOrder").removeClass("fontColorOfPickItem");
        $("#txtSearchByDeliveryToSUDeliveryOrder").val("");
        _nDeliveryTo = 0;
    });

    $("#btnPickDeliveryToSUDeliveryOrder").click(function () {
        PickDeliveryToSUDeliveryOrder();
    });

    $("#btnOkDeliveryToPicker").click(function () {
        SelectDeliveyToContractor();
    });

    $("#txtSearchByDeliveryToNamePicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oDeliveryTos,
            TableId: "tblDeliveryTosPicker"
        };
        $('#txtSearchByDeliveryToNamePicker').icsSearchByText(obj);
    });

    $("#btnCloseDeliveryToPicker").click(function () {
        $("#winDeliveryToPicker").icsWindow("close");
        _oDeliveryTos = [];
        $("#winDeliveryToPicker input").val("");
        DynamicRefreshList([], "tblDeliveryTosPicker");
    });
}

function PickDeliveryToSUDeliveryOrder() {
    var oContractor = null;
    var cboDeliveryTo = parseInt($("#cboDeliveryToSUDeliveryOrder").val());
    var oContractor = {
        Params: cboDeliveryTo + '~' + $.trim($("#txtSearchByDeliveryToSUDeliveryOrder").val())
    };

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByNameType",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblContractorsPicker");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                _oDeliveryTos = [];
                _oDeliveryTos = response.objs;
                DynamicRefreshList(response.objs, "tblDeliveryTosPicker");
                $("#winDeliveryToPicker").icsWindow('open', "Contractor Search");
                $("#divDeliveryToPicker").focus();
                $("#winDeliveryToPicker input").val("");
            }
            else {
                DynamicRefreshList([], "tblDeliveryTosPicker");
                alert(response.objs[0].ErrorMessage);
                $("#txtSearchByDeliveryToSUDeliveryOrder").removeClass("fontColorOfPickItem");
                _nDeliveryTo = 0;
            }
        }
        else {
            alert("Sorry, No data found, Try again.");
            $("#txtSearchByDeliveryToSUDeliveryOrder").removeClass("fontColorOfPickItem");
            _nDeliveryTo = 0;
        }
    });
}

function PickProductInSUDeliveryOrder() {
    if ($.trim($("#txtProductName").val()).length === 0) {
        alert("Please enter product name.");
        return false;
    }

    var oProduct = {
        Params: $.trim($("#txtProductName").val()) + '~' + 0 + ''
    };
    DynamicRefreshList([], "tblProductPickerByName");

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oProduct,
        ControllerName: "Product",
        ActionName: "ATMLNewSearchByProductNameOnlyForPI",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ProductID > 0) {
                _oProducts = response.objs;
                DynamicRefreshList(response.objs, "tblProductPickerByName");
                $("#winProductPickerByName").icsWindow('open', "Product Search");
                $("#divProductPickerByName").focus();
                $("#winProductPickerByName input").val("");
            }
            else {
                _oProducts = [];
                DynamicRefreshList([], "tblProductPickerByName");
                alert(response.objs[0].Errormessage);
                $("#txtProductName").removeClass("fontColorOfPickItem");
                _oSelectedProduct = null;
            }
        }
        else {
            alert("Sorry, No data found, Try again.");
            $("#txtProductName").removeClass("fontColorOfPickItem");
            _oSelectedProduct = null;
        }
    });
}

function PickProductSUDODetail() {
    $("#winProductPicker").on("keydown", function (e) {
        var oExportPIDetail = $('#tblProductsPicker').datagrid('getSelected');
        var nIndex = $('#tblProductsPicker').datagrid('getRowIndex', oExportPIDetail);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblProductsPicker').datagrid('selectRow', 0);
            }
            else {
                $('#tblProductsPicker').datagrid('selectRow', nIndex - 1);
            }
            $('#txtSearchByProductNamePicker').blur();
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblProductsPicker').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblProductsPicker').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblProductsPicker').datagrid('selectRow', nIndex + 1);
            }
            $('#txtSearchByProductNamePicker').blur();
        }
        if (e.which == 13)//enter=13
        {
            var oProduct = $("#tblProductsPicker").icsGetSelectedItem();
            if (oProduct != null && oProduct.ProductID > 0) {
                $("#txtProductNameSUDeliveryOrderDetail").val(oProduct.ProductName);
                $("#txtProductNameSUDeliveryOrderDetail").addClass("fontColorOfPickItem");
                _oSelectedProductSUDODetail = oProduct
            }
        }
    });

    $("#txtProductNameSUDeliveryOrderDetail").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            PickProductInSUDODetail();
        }
        else if (e.keyCode === 08) {
            $("#txtProductNameSUDeliveryOrderDetail").removeClass("fontColorOfPickItem");
            _oSelectedProductSUDODetail = null;
        }
    });

    $("#btnClrProductSUDeliveryOrderDetail").click(function () {
        $("#txtProductNameSUDeliveryOrderDetail").removeClass("fontColorOfPickItem");
        $("#txtProductNameSUDeliveryOrderDetail").val("");
        _oSelectedProductSUDODetail = null;
    });

    $("#btnOkProductPicker").click(function () {
        var oProduct = $("#tblProductsPicker").icsGetSelectedItem();
        if (oProduct != null && oProduct.ProductID > 0) {
            $("#txtProductNameSUDeliveryOrderDetail").val(oProduct.ProductName);
            $("#txtProductNameSUDeliveryOrderDetail").addClass("fontColorOfPickItem");
            _oSelectedProductSUDODetail = oProduct
        }
    });

    $("#txtSearchByProductNamePicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "ProductName",
            GlobalObjectList: _oProductsSUDODetail,
            TableId: "tblProductsPicker"
        };
        $('#txtSearchByProductNamePicker').icsSearchByText(obj);
    });

    $("#btnCloseProductPicker").click(function () {
        DynamicRefreshList([], "tblProductsPicker");
        $("#winProductPicker input").val("");
        _oProductsSUDODetail = [];
    });

    $("#btnPickProductSUDeliveryOrderDetail").click(function () {
        PickProductInSUDODetail();
    });
}

function PickProductInSUDODetail() {
    var sDBObjectName = "SUDeliveryOrderDetail";
    var nTriggerParentsType = 109; //_ViewProduct
    var nOperationalEvent = 704; // _View
    var nInOutType = 100;  // None
    var nDirections = 0;
    var nStoreID = 0;
    var nMapStoreID = 0;
    var sProductName = $("#txtProductNameSUDeliveryOrderDetail").val();
    var oProduct = {
        Params: $.trim(sProductName) + '~' + sDBObjectName + '~' + nTriggerParentsType + '~' + nOperationalEvent + '~' + nInOutType + '~' + nDirections + '~' + nStoreID + '~' + nMapStoreID
    };
    IntitializeProductSearchingPicker();
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oProduct,
        ControllerName: "Product",
        ActionName: "ATMLNewSearchByProductName",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ProductID > 0) {
                _oProductsPicker = response.objs;
                _oProductsSUDODetail = response.objs;
                DynamicRefreshList(response.objs, "tblProductsPicker");
                $("#winProductPicker").icsWindow('open', "Product Search");
                $("#divProductPicker").focus();
                $("#winProductPicker input").val("");
            }
            else {
                _oProductsPicker = [];
                DynamicRefreshList([], "tblProductsPicker");
                alert(response.objs[0].Errormessage);
                $("#txtProductNameSUDeliveryOrderDetail").removeClass("fontColorOfPickItem");
                _oSelectedProductSUDODetail = null;
            }
        }
        else {
            alert("Sorry, No data found, Try again.");
            $("#txtProductNameSUDeliveryOrderDetail").removeClass("fontColorOfPickItem");
            _oSelectedProductSUDODetail = null;
        }
    });
}

function SUDODetailRelated() {
    $("#btnProductAddSUDeliveryOrderDetail").click(function () {
        if (_oSelectedProductSUDODetail == null) {
            alert("Please select product.");
            return false;
        }

        var nExportPIDetailID = 0;
        if (typeof _oSelectedProductSUDODetail.ExportPIDetailID === "undefined") {
            nExportPIDetailID = 0;
        }
        else {
            nExportPIDetailID = _oSelectedProductSUDODetail.ExportPIDetailID;
        }

        var nSUDeliveryOrderDetailId = (_oSUDeliveryOrderDetail == null ? 0 : _oSUDeliveryOrderDetail.SUDeliveryOrderDetailID);
        var nSUDeliveryOrderId = (_oSUDeliveryOrder == null ? 0 : _oSUDeliveryOrder.SUDeliveryOrderID);
        var oSUDeliveryOrderDetail = {
            SUDeliveryOrderDetailID: parseInt(nSUDeliveryOrderDetailId),
            SUDeliveryOrderID: parseInt(nSUDeliveryOrderId),
            ProductID: parseInt(_oSelectedProductSUDODetail.ProductID),
            ExportPIDetailID: parseInt(nExportPIDetailID),
            MUnitID: parseInt(_oSelectedProductSUDODetail.MeasurementUnitID),
            Qty: 0,
            UnitPrice: parseInt($('#cboDOTypeSUDeliveryOrder').val()) === 6 ? 1 : 0
        };


        var oExportDetails = [];
        oExportDetails.push(oSUDeliveryOrderDetail);
      
        var oEDs = [];
        if (_oSUDeliveryOrder == null) {
            if (!ValidateInputSUDeliveryOrder()) return;
            var oSUDeliveryOrder = RefreshObjectSUDeliveryOrder();
            _oSUDeliveryOrder = oSUDeliveryOrder;
            for (var i = 0; i < oExportDetails.length; i++) {
                if (i == 0) {
                    oExportDetails[i].SUDeliveryOrder = oSUDeliveryOrder;
                } else {
                    oExportDetails[i].SUDeliveryOrder = null;
                }
                oEDs.push(oExportDetails[i]);
            }
        }
        else {
            for (var i = 0; i < oExportDetails.length; i++) {
                oExportDetails[i].SUDeliveryOrderID = _oSUDeliveryOrder.SUDeliveryOrderID;
                oEDs.push(oExportDetails[i]);
            }
        }

        var oSUDeliveryOrderDetail = {
            SUDeliveryOrderDetailID: 0,
            SUDeliveryOrderID: (_oSUDeliveryOrder == null) ? 0 : _oSUDeliveryOrder.SUDeliveryOrderID,
            SUDeliveryOrderDetails: oEDs
        };

       
        
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUDeliveryOrder/SaveSUDeliveryOrderDetail",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrderDetail),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
               
                var oSUDOs = [];
                var oSUDO = jQuery.parseJSON(data);
                oSUDOs = oSUDO.SUDeliveryOrderDetails;
                if (oSUDOs != null) {
                    if (oSUDOs.length > 0) {
                       
                        var oSUDeliveryOrderDetails = $("#tblSUDeliveryOrderDetails").datagrid("getRows");
                        var nIndex = oSUDeliveryOrderDetails.length;
                        if (oSUDeliveryOrderDetail.SUDeliveryOrderID <= 0) {
                            if (oSUDOs[0].SUDeliveryOrder.SUDeliveryOrderID > 0) {
                                _oSUDeliveryOrder = oSUDOs[0].SUDeliveryOrder;
                                $("#txtDONoSUDeliveryOrder").val(_oSUDeliveryOrder.DONo);
                                var oSUDeliveryOrders = $("#tblSUDeliveryOrders").datagrid("getRows");
                                var nIndexSO = oSUDeliveryOrders.length;
                                $("#tblSUDeliveryOrders").datagrid("appendRow", oSUDOs[0].SUDeliveryOrder);
                                $("#tblSUDeliveryOrders").datagrid("selectRow", nIndexSO);
                            }
                        }
                       
                        for (var i = 0; i < oSUDOs.length; i++) {
                            $("#tblSUDeliveryOrderDetails").datagrid("appendRow", oSUDOs[i]);
                            $("#tblSUDeliveryOrderDetails").datagrid("selectRow", nIndex);
                            nIndex++;
                        }
                        $("#txtProductNameSUDeliveryOrderDetail").val("");
                        $("#txtProductNameSUDeliveryOrderDetail").removeClass("fontColorOfPickItem");
                        _oSelectedProductSUDODetail = null;
                    }
                    else {
                        alert("No data found.");
                    }
                }
                else {
                    alert("No data found.");
                }
                RefreshTotalSummery();
            }
        });
    });

    $("#btnProductRemoveSUDeliveryOrderDetail").click(function () {
        var oSUDeliveryOrderDetail = $("#tblSUDeliveryOrderDetails").datagrid("getSelected");
        if (oSUDeliveryOrderDetail == null || oSUDeliveryOrderDetail.SUDeliveryOrderDetailID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var SelectedRowIndex = $('#tblSUDeliveryOrderDetails').datagrid('getRowIndex', oSUDeliveryOrderDetail);
        if (_isSave) {
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/SUDeliveryOrder/DeleteSUDeliveryOrderDetail",
                traditional: true,
                data: JSON.stringify(oSUDeliveryOrderDetail),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var sFeedBackMessage = jQuery.parseJSON(data);
                    if (sFeedBackMessage == "Deleted") {
                        $('#tblSUDeliveryOrderDetails').datagrid('deleteRow', SelectedRowIndex);
                        RefreshTotalSummery();
                    }
                    else {
                        alert(sFeedBackMessage);
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        } else {
            $('#tblSUDeliveryOrderDetails').datagrid('deleteRow', SelectedRowIndex);
            RefreshTotalSummery();
        }
    });

    $("#btnRefreshSUDeliveryOrderDetail").click(function (e) {
        endEditing();
        var oSUDOs = $('#tblSUDeliveryOrderDetails').datagrid('getRows');
        DynamicRefreshList(oSUDOs, "tblSUDeliveryOrderDetails");
        RefreshTotalSummery();
    });

    $("#cboCurrencySUDeliveryOrder").change(function () {
        var sCurrency = $("#cboCurrencySUDeliveryOrder option:selected").text();
        $("#lblCurrency").text(sCurrency);
    });
}

function ResetAllGlobalValueSUDO() {
    _oBuyers = [];
    _oDeliveryTos = [];
    _oProducts = [];
    _oSelectedProduct = null;
    _nBuyerID = 0;
    _nDeliveryTo = 0;
    _nExportPIID = 0;
    _oExportPISUDO = null;
    _oExportPIs = [];
    _oSelectedProductSUDODetail = null;
    _oProductsSUDODetail = [];
}

function SaveSUDeliveryOrder(oSUDeliveryOrder) {
    var nSUDeliveryOrderDetailId = (_oSUDeliveryOrderDetail == null ? 0 : _oSUDeliveryOrderDetail.SUDeliveryOrderDetailID);
    var oSUDeliveryOrderDetail = {
        SUDeliveryOrderDetailID: parseInt(nSUDeliveryOrderDetailId),
        SUDeliveryOrderID: parseInt(_oSUDeliveryOrder.SUDeliveryOrderID),
        ProductID: parseInt(_oSelectedProductSUDODetail.ProductID),
        //ExportPIDetailID: parseInt(_oSelectedProductSUDODetail.ExportPIDetailID),
        MUnitID: parseInt(_oSelectedProductSUDODetail.MeasurementUnitID),
        Qty: 0,
        UnitPrice: 0,
        SUDeliveryOrder: oSUDeliveryOrder
    };

    $.ajax({
        type: "POST",
        url: _sBaseAddress + "/SUDeliveryOrder/SaveSUDeliveryOrderDetail",
        traditional: true,
        data: JSON.stringify(oSUDeliveryOrderDetail),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var oSUDO = jQuery.parseJSON(data);
            if (oSUDO != null) {
                if (!$.trim(oSUDO.ErrorMessage).length) {
                    var oSUDeliveryOrderDetails = $("#tblSUDeliveryOrderDetails").datagrid("getRows");
                    var nIndex = oSUDeliveryOrderDetails.length;
                    if (oSUDeliveryOrderDetail.SUDeliveryOrderID <= 0) {
                        if (oSUDO.SUDeliveryOrder.SUDeliveryOrderID > 0) {
                            _oSUDeliveryOrder = oSUDO.SUDeliveryOrder;
                            $("#txtDONoSUDeliveryOrder").val(_oSUDeliveryOrder.DONo);
                            var oSUDeliveryOrders = $("#tblSUDeliveryOrders").datagrid("getRows");
                            var nIndexSO = oSUDeliveryOrders.length;
                            $("#tblSUDeliveryOrders").datagrid("appendRow", oSUDO.SUDeliveryOrder);
                            $("#tblSUDeliveryOrders").datagrid("selectRow", nIndexSO);
                        }
                    }
                    $("#tblSUDeliveryOrderDetails").datagrid("appendRow", oSUDO);
                    $("#tblSUDeliveryOrderDetails").datagrid("selectRow", nIndex);

                } else {
                    alert(oSUDeliveryOrder.ErrorMessage);
                }
            }
            else {
                alert(oSUDeliveryOrder.ErrorMessage);
            }
        }
    });
}


function RefreshTotalSummery() {
    var nTotalQty = 0;
    var nTotalQtyLBS = 0;
    var nTotalValue = 0;
    var oSUDeliveryOrderDetails = $('#tblSUDeliveryOrderDetails').datagrid('getRows');
    for (var i = 0; i < oSUDeliveryOrderDetails.length; i++) {
        nTotalQty = nTotalQty + parseFloat(oSUDeliveryOrderDetails[i].Qty);
        nTotalQtyLBS = nTotalQtyLBS + parseFloat(oSUDeliveryOrderDetails[i].QtyInLbs);
        nTotalValue = nTotalValue + parseFloat(oSUDeliveryOrderDetails[i].UnitPrice * oSUDeliveryOrderDetails[i].Qty);
    }
    var sCurrency = "";
    var nCurrency = parseInt($("#cboCurrencySUDeliveryOrder").val());
    if (nCurrency > 0) {
        sCurrency = $("#cboCurrencySUDeliveryOrder option:selected").text();
    }
    $("#lblTotalQty").text(formatPrice(nTotalQty) + "(KG)");
    $("#lblTotalQtyLBS").text(formatPrice(nTotalQtyLBS) + "(LBS)");
    $("#lblCurrency").text(sCurrency);
    $("#lblTotalValue").text(formatPrice(nTotalValue));
}

function RefreshTotalSummeryForSampleDO() {
    var nlblTotalCones = 0;
    var nlblTotalQtyinKG = 0;
    var nlblTotalQtyinLBS = 0;
    var oSUDeliveryOrderDetails = $('#tblSUDeliveryOrderDetails').datagrid('getRows');
    for (var i = 0; i < oSUDeliveryOrderDetails.length; i++) {
        nlblTotalCones = nlblTotalCones + parseFloat(oSUDeliveryOrderDetails[i].NumberOfCone);
        nlblTotalQtyinKG = nlblTotalQtyinKG + parseFloat(oSUDeliveryOrderDetails[i].Qty);
        nlblTotalQtyinLBS = nlblTotalQtyinLBS + parseFloat(GetLBS(oSUDeliveryOrderDetails[i].Qty, 2));
    }
    var sCurrency = "";
    $("#lblTotalCones").text(nlblTotalCones);
    $("#lblTotalQtyinKG").text(nlblTotalQtyinKG + "(KG)");
    $("#lblTotalQtyinLBS").text(nlblTotalQtyinLBS + "(LBS)");
}

function UpdateSUDeliveryOrderDetail(oSUDODetail, SelectedRowIndex) {
    var oSUDOs = [];
    oSUDOs.push(oSUDODetail);
   
    
    if (_isValidAmount)
    {
        var oSUDeliveryOrderDetail = {
            SUDeliveryOrderDetails: oSUDOs
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryOrder/SaveSUDeliveryOrderDetail",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrderDetail),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryOrderDetail = jQuery.parseJSON(data);
                if (oSUDeliveryOrderDetail != null && oSUDeliveryOrderDetail.SUDeliveryOrderDetails != null && oSUDeliveryOrderDetail.SUDeliveryOrderDetails.length > 0) {
                    oSUDOs = [];
                    oSUDOs = oSUDeliveryOrderDetail.SUDeliveryOrderDetails;
                    if (parseInt(oSUDOs[0].SUDeliveryOrderDetailID) > 0) {
                        $('#tblSUDeliveryOrderDetails').datagrid('updateRow', { index: SelectedRowIndex, row: oSUDOs[0] });
                        RefreshTotalSummery();
                    }
                    else {
                        alert(oSUDOs[0].ErrorMessage);
                    }
                }
                else {
                    alert(oSUDeliveryOrderDetail.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    }
    else
    {
        $('#tblSUDeliveryOrderDetails').datagrid('updateRow', { index: SelectedRowIndex, row: oSUDODetail });
        RefreshTotalSummery();
    }
    
}

var editIndex = undefined;
function endEditing() {
    if (editIndex == undefined) {
        CheckAmountSUDO();
        return true;
    }
    if ($('#tblSUDeliveryOrderDetails').datagrid('validateRow', editIndex)) {
        $('#tblSUDeliveryOrderDetails').datagrid('endEdit', editIndex);
        $('#tblSUDeliveryOrderDetails').datagrid('selectRow', editIndex);
        var oSUDeliveryOrderDetail = $('#tblSUDeliveryOrderDetails').datagrid('getSelected');
        if (oSUDeliveryOrderDetail != null) {
            if (parseInt($("#cboDOTypeSUDeliveryOrder").val()) === 6) {
                oSUDeliveryOrderDetail.Qty = parseFloat(oSUDeliveryOrderDetail.NumberOfCone * oSUDeliveryOrderDetail.QtyPerCone);
            }
            oSUDeliveryOrderDetail.QtyInLbs = parseFloat(GetLBS(oSUDeliveryOrderDetail.Qty, 2));
            oSUDeliveryOrderDetail.Amount = parseFloat(oSUDeliveryOrderDetail.Qty * oSUDeliveryOrderDetail.UnitPrice)
            oSUDeliveryOrderDetail.AmountSt = formatPrice(oSUDeliveryOrderDetail.Amount);
            
            if (parseFloat(oSUDeliveryOrderDetail.Qty) != _nQty || parseFloat(oSUDeliveryOrderDetail.UnitPrice) != _nUnitPrice || parseFloat(oSUDeliveryOrderDetail.NumberOfCone) != _nNumberOfCone || parseFloat(oSUDeliveryOrderDetail.QtyPerCone) != _nQtyPerCone || oSUDeliveryOrderDetail.RequiredLotNo != _sRequiredLotNo || oSUDeliveryOrderDetail.Remarks != _sRemarks) {
                oSUDeliveryOrderDetail.SUDeliveryOrder = null;
                if (_isSave) {
                    CheckAmountSUDO();
                    UpdateSUDeliveryOrderDetail(oSUDeliveryOrderDetail, editIndex);
                }
            }
            _nQty = 0;
            _nUnitPrice = 0;
            editIndex = undefined;
        }
        return true;
    }
    else {
        return false;
    }
}

var _nQty = 0;
var _nUnitPrice = 0;
var _sRequiredLotNo = "";
var _sRemarks = "";
var _nNumberOfCone = 0;
var _nQtyPerCone = 0;
function onClickRowSUDO(index) {
    if (editIndex != index) {
        if (endEditing()) {
            $('#tblSUDeliveryOrderDetails').datagrid('selectRow', index).datagrid('beginEdit', index);
            var oSUDeliveryOrderDetail = $('#tblSUDeliveryOrderDetails').datagrid('getSelected');
            _nQty = parseFloat(oSUDeliveryOrderDetail.Qty);
            _nUnitPrice = parseFloat(oSUDeliveryOrderDetail.UnitPrice);
            _sRequiredLotNo = oSUDeliveryOrderDetail.RequiredLotNo;
            _sRemarks = oSUDeliveryOrderDetail.Remarks;
            _nNumberOfCone = parseFloat(oSUDeliveryOrderDetail.NumberOfCone);
            _nQtyPerCone = parseFloat(oSUDeliveryOrderDetail.QtyPerCone);
            editIndex = index;
        }
        else {
            $('#tblSUDeliveryOrderDetails').datagrid('selectRow', editIndex);
        }
    }
}

function GenerateTableColumnsSUDOD() {
    _otblColumns = [];
    var oColProductCode = { field: "ProductCode", width: "10%", title: "Code", frozen: true };
    var oColProductName = { field: "ProductName", width: "20%", title: "Description", frozen: true };
    var oColQty = { field: "Qty", width: "15%", title: "Qty(kG)", editor: { type: 'numberbox', options: { precision: 2 } }, align: 'right', frozen: true };
    var oColQtyInLbs = { field: "QtyInLbs", width: "15%", title: "Qty(LBS)", align: 'right', frozen: true };
    var oColUnitPrice = { field: "UnitPrice", width: "15%", title: "Unit Price/KG", editor: { type: 'numberbox', options: { precision: 2 } }, align: 'right', frozen: true };
    var oColUnitPriceInLbs = { field: "UnitPriceInLbs", width: "15%", title: "Unit Price/LBS", align: 'right', frozen: true };
    var oColAmountSt = { field: "AmountSt", width: "10%", title: "Amount", align: 'right', frozen: true };

    _otblColumns.push(oColProductCode, oColProductName, oColQty, oColQtyInLbs, oColUnitPrice, oColUnitPriceInLbs, oColAmountSt);
    $('#tblSUDeliveryOrderDetails').datagrid({ columns: [_otblColumns] });
    $('#tblSUDeliveryOrderDetails').datagrid({ onClickRow: function (index, field, value) { return onClickRowSUDO(index); } });
    editIndex = undefined;
}

function GenerateTableColumnsSUDODForSampleDO() {
    _otblColumns = [];
    var oColProductCode = { field: "ProductCode", width: "10%", title: "Code", frozen: true };
    var oColProductName = { field: "ProductName", width: "20%", title: "Description", frozen: true };
    var oColRequiredLotNo = { field: "RequiredLotNo", width: "15%", title: "Required Lot #", editor: { type: 'text'}, align: 'left', frozen: true };
    var oColRemarks = { field: "Remarks", width: "15%", title: "Remarks", editor: { type: 'text' }, align: 'left', frozen: true };
    var oColNumberOfCone = { field: "NumberOfCone", width: "10%", title: "Cones", editor: { type: 'numberbox', options: { precision: 2 } }, align: 'right', frozen: true };
    var oColQtyPerCone = { field: "QtyPerCone", width: "10%", title: "Qty/Cone(KG)", editor: { type: 'numberbox', options: { precision: 3 } }, align: 'right', frozen: true };
    var oColQty = { field: "Qty", width: "10%", title: "Qty(kG)", align: 'right', frozen: true };
    var oColQtyInLbs = { field: "QtyInLbs", width: "10%", title: "Qty(LBS)", align: 'right', frozen: true };
   

    _otblColumns.push(oColProductCode, oColProductName, oColRequiredLotNo, oColRemarks, oColNumberOfCone, oColQtyPerCone, oColQty, oColQtyInLbs);
    $('#tblSUDeliveryOrderDetails').datagrid({ columns: [_otblColumns] });
    $('#tblSUDeliveryOrderDetails').datagrid({ onClickRow: function (index, field, value) { return onClickRowSUDO(index); } });
    editIndex = undefined;
}