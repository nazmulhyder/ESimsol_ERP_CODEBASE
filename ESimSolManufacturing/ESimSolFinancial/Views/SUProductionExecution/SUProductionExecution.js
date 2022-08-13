var _oSUPE = { ErrorMessage: "", SUPEDetails: [], OperationUnitName: '', ReceivedStoreID: 0, SUProductionExecutionID: 0, ExecutionStatus: 0,ExecutionNo:'' };
var _oDBSUPEDs = [];
var _oSUPED = { ErrorMessage: "", ProductShortName: '', ProductID: 0, SUProductionExecutionDetailID: 0 };
var _oSUPEDs = [];
var _oProduct = {ProductID:0,MeasurementUnitID:0};
var _oLot = {LotID:0};
var _otblColumns = [];
var _bEditable = false;
var _oReceivedStores = [];

function InitializeSUPEEvents() {

    $("#btnReceiveConfirmSUPE").click(function () {
        if (!ValidateInputSUPE()) { return false; }
        if (_oSUPE == null || parseInt(_oSUPE.SUProductionExecutionID) <= 0) { alert("Invalid Production Execution! \nPlease Refresh and try Again."); return false; }
        if (parseInt(_oSUPE.ExecutionStatusInInt) != 3) { alert("Production Execution is not yet Approved!"); return false; }
        if (!confirm("Confirm to Receive Finished Goods?")) return false;
        var sSuccessMessage = "Successfully Received Finished Goods";
        _oSUPE.ExecutionStatus = 4;
        ReceiveFinishedGoods(_oSUPE, sSuccessMessage);
    });
    $("#btnSaveSUPE").click(function () {
        if (!ValidateInputSUPE()) { return false; }
        var oSUPE = RefreshObjectSUPE();
        $('#tblSUPEDs').datagrid('endEdit', _nIndex);
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSUPE,
            ObjectId: oSUPE.SUProductionExecutionID,
            ControllerName: "SUProductionExecution",
            ActionName: "Save",
            TableId: "tblSUPEs",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if(response.status)
            {
                _oDBSUPEs = [];
                _oDBSUPEs = $("#tblSUPEs").datagrid('getRows');
                for (var i = 0; i < _oDBSUPEs.length; i++) {
                    _oDBSUPEs[i].ExecutionDate = _oDBSUPEs[i].ExecutionDateInString;
                }
                $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUPEs));
                $("#txtExecutionNoSUPE").val('');
                $("#txtExecutionDateSUPE").val('');
                $("#txtRemarks").val('');
                $('#cboFinishStoresSUPE').empty();
                _oSUPE = { ErrorMessage: "", SUPEDetails: [], OperationUnitName: '', ReceivedStoreID: 0, SUProductionExecutionID: 0 };
                _oDBSUPEDs = [];
                _oSUPED = { ErrorMessage: "", ProductShortName: '', ProductID: 0, SUProductionExecutionDetailID: 0 };
                _oSUPEDs = [];
                _oProduct = { ProductID: 0, MeasurementUnitID: 0 };
                _oLot = { LotID: 0 };
                _otblColumns = [];
            }
        });
    });
    $("#btnCloseSUPE").click(function () {
        $("#txtExecutionNoSUPE").val('');
        $("#txtExecutionDateSUPE").val('');
        $("#txtRemarks").val('');
        $('#cboFinishStoresSUPE').empty();
        _oSUPE = { ErrorMessage: "", SUPEDetails: [], OperationUnitName: '', ReceivedStoreID: 0, SUProductionExecutionID: 0 };
        _oDBSUPEDs = [];
        _oSUPED = { ErrorMessage: "", ProductShortName: '', ProductID: 0, SUProductionExecutionDetailID: 0 };
        _oSUPEDs = [];
        _oProduct = { ProductID: 0, MeasurementUnitID: 0 };
        _oLot = { LotID: 0 };
        _otblColumns = [];
        $("#winSUPE").icsWindow('close');
    });

    $("#txtPickProductSUPED").keydown(function (e) {
        if (e.keyCode === 13)
        {
            if (!ValidateInputSUPE()) {
                return false;
            }
            var sDBObjectName = "SUProductionExecution";
            var nTriggerParentsType = 110;
            var nOperationalEvent = 712;
            var nInOutType = 101;
            var nDirections = 0;
            var nStoreID = 0;
            var nMapStoreID = 0;
            var sProductName = '';
            var oProduct = {
                Params: $.trim($("#txtPickProductSUPED").val()) + '~' + sDBObjectName + '~' + nTriggerParentsType + '~' + nOperationalEvent + '~' + nInOutType + '~' + nDirections + '~' + nStoreID + '~' + nMapStoreID
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
                        DynamicRefreshList(response.objs, "tblProductPicker");
                        $("#winProductPickerByName").icsWindow('open', "Product Search");
                        $("#divProductPickerByName").focus();
                        $("#winProductPickerByName input").val("");
                    }
                    else {
                        _oProductsPicker = [];
                        DynamicRefreshList([], "tblLotPicker");
                        alert(response.objs[0].Errormessage);
                    }
                }
                else {
                    alert("Sorry, No data found, Try again.");
                }
            });
        }
    });

    $('#txtSearchByProductNamePicker').keyup(function (e) {
        var obj =
        {
            Event: e,
            SearchProperty: "ProductName",
            GlobalObjectList: _oProductsPicker,
            TableId: "tblProductPicker"
        };
        $('#txtSearchByProductNamePicker').icsSearchByText(obj);
    });

    $("#winProductPickerByName").on("keydown", function (e) {
        var oProduct = $('#tblProductPicker').datagrid('getSelected');
        var nIndex = $('#tblProductPicker').datagrid('getRowIndex', oProduct);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblProductPicker').datagrid('selectRow', 0);
            }
            else {
                $('#tblProductPicker').datagrid('selectRow', nIndex - 1);
            }
            
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblProductPicker').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblProductPicker').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblProductPicker').datagrid('selectRow', nIndex + 1);
            }
            
        }
        if (e.which == 13)//enter=13
        {
            PickProductFromProductPicker1();
        }
    });

    

    $("#btnOkProductPickerByName").click(function () {
        PickProductFromProductPicker1();
    });

    $("#btnPickLotSUPED").click(function () {
        var oSUPED = $("#tblSUPEDs").datagrid("getSelected");
        if (oSUPED == null || oSUPED.SUProductionExecutionDetailID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        _oSUPED = oSUPED
        var oParams = { ProductIDs: '', WorkingUnitIDs: '' };
        oParams.ProductIDs = '' + _oSUPED.ProductID;
        oParams.WorkingUnitIDs = '' + _oSUPE.ReceivedStoreID;
        _nProductID = _oSUPED.ProductID;
        _nWorkingUnitID = _oSUPE.ReceivedStoreID;
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oParams,
            ControllerName: "Lot",
            ActionName: "ATMLNewGetsby",
            TableId: "tblLotPicker",
            IsWinClose: false
        };
        $.icsDataGets(obj, function (response) {
            if (response.status) {
                _oLotsPicker = response.objs.length > 0 ? response.objs : [];
                DynamicRefreshList(_oLotsPicker, "tblLotPicker");
                $("#winLotPicker").icsWindow('open', 'Selected Store: ' + _oSUPE.OperationUnitName + ', Selected Product: ' + _oSUPED.ProductShortName);
                $("#txtSearchByLotNoPicker").focus();
                $("#winLotPicker input").val("");
                //$("#txtUnitPricePicker").val('1');
            }
        });
    });

    $("#winLotPicker").on("keydown", function (e) {
        var oLot = $('#tblLotPicker').datagrid('getSelected');
        var nIndex = $('#tblLotPicker').datagrid('getRowIndex', oLot);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblLotPicker').datagrid('selectRow', 0);
            }
            else {
                $('#tblLotPicker').datagrid('selectRow', nIndex - 1);
            }
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblLotPicker').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblLotPicker').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblLotPicker').datagrid('selectRow', nIndex + 1);
            }

        }
    });

    $("#btnOkLotPicker").click(function () {
        var oLot = $("#tblLotPicker").icsGetSelectedItem();
        if (oLot == null || oLot.LotID <= 0) {
            return false;
        }
        _oLot = oLot;
        _oSUPED.LotID = _oLot.LotID;
        SaveSUPED(_oSUPED);//ajax call, finish work before it executes.
    });

    $('#btnRemoveSUPED').click(function () {
        var oSUPED = $("#tblSUPEDs").datagrid("getSelected");
        if (oSUPED == null || oSUPED.SUProductionExecutionDetailID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oSUPED,
            ObjectId: oSUPED.SUProductionExecutionDetailID,
            ControllerName: "SUProductionExecution",
            ActionName: "DeleteSUPED",
            TableId: "tblSUPEDs",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $('#btnRefreshSUPED').click(function () {
        $('#tblSUPEDs').datagrid('endEdit', _nIndex);
        var oSUPED = { ErrorMessage: '' };
        oSUPED.ErrorMessage = 'Arguments;' + _oSUPE.SUProductionExecutionID+'~';
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oSUPED,
            ObjectId: oSUPED.SUPEDID,
            ControllerName: "SUProductionExecution",
            ActionName: "GetsSUPED",
            TableId: "tblSUPEDs",
            IsWinClose: false
        };
        $.icsDataGets(obj, function (response) {
             
            if (response.status) {
                GenerateTableColumnsSUPED();
                DynamicRefreshList(response.objs, 'tblSUPEDs');
                RefreshTotalSummery();
            }
        });
    });
    /////End SUPED
}

function SaveSUPED(oSUPED) {
    if (!_bEditable) { return false; }
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SUProductionExecution/SaveSUPED",
        traditional: true,
        data: JSON.stringify(oSUPED),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var oSUPEDetail = jQuery.parseJSON(data);
            if (parseInt(oSUPEDetail.SUProductionExecutionDetailID) > 0) {
                if (oSUPED.SUProductionExecutionDetailID > 0) {
                    var nRowIndex = $('#tblSUPEDs').datagrid("getRowIndex", oSUPED);
                    $('#tblSUPEDs').datagrid("updateRow", { index: nRowIndex, row: oSUPEDetail });
                }
                else {
                    var oSUPEDs = $("#tblSUPEDs").datagrid("getRows");
                    var nIndex = oSUPEDs.length;
                    $("#tblSUPEDs").datagrid("appendRow", oSUPEDetail);
                    $("#tblSUPEDs").datagrid("selectRow", nIndex);
                }
                if (oSUPED.SUProductionExecutionID <= 0) {
                    if (oSUPEDetail.SUPE.SUProductionExecutionID > 0) {
                        _oSUPE = oSUPEDetail.SUPE;
                        var oSUPEs = $("#tblSUPEs").datagrid("getRows");
                        var nIndexSUPE = oSUPEs.length;
                        $("#tblSUPEs").datagrid("appendRow", oSUPEDetail.SUPE);
                        $("#tblSUPEs").datagrid("selectRow", nIndexSUPE);
                        oSUPEDetail.SUPE = null;
                    }
                }
                $("#txtPickProductSUPED").val("");
                RefreshTotalSummery();

                _oProduct = null;

                $("#cboFinishStoresSUPE").prop("disabled", true);

            }
            else {
                alert(oSUPEDetail.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function RefreshTotalSummery() {
    var nTotalQtyKG = 0;
    var nTotalQtyLBS = 0;
    var oSUPEs = $('#tblSUPEDs').datagrid('getRows');
    for (var i = 0; i < oSUPEs.length; i++) {
        nTotalQtyKG = nTotalQtyKG + parseFloat(oSUPEs[i].Qty);
        nTotalQtyLBS = nTotalQtyLBS + parseFloat(oSUPEs[i].QtyInPounds);
    }
    $("#lblQTyKG").html(formatPrice(nTotalQtyKG) + ' KG');
    $("#lblQTyLBS").html(formatPrice(nTotalQtyLBS) + ' LBS');
}


function PickProductFromProductPicker1() {
    var oProduct = $("#tblProductPicker").icsGetSelectedItem();
    if (oProduct == null || oProduct.ProductID <= 0) {
        return false;
    }
    _oProduct = oProduct;

    if (!ValidateInputSUPE()) return false;

    _oSUPED = null;
    var oSUPED = RefreshObjectSUPED();
    if (_oSUPE == null || parseInt(_oSUPE.SUProductionExecutionID) <= 0) {
        oSUPED.SUPE = RefreshObjectSUPE();
    }
    SaveSUPED(oSUPED);//ajax call, finish work before it executes.
}

//function onClickCell(index, field, value) {
//    
//    $('#tblSUPEDs').datagrid('selectRow', index);
//    var oSUPED = $('#tblSUPEDs').datagrid('getSelected');
    
//    if (_nIndex != index && _nIndex != -1) {
        
//        $('#tblSUPEDs').datagrid('endEdit', _nIndex);
//        if (field == 'Qty') {
//            if (oSUPED.LotID <= 0) {
//                alert('Please Assign Lot for :' + oSUPED.ProductShortName + ' and Try Again!');
//                return false;
//            }
//            _nQty = parseFloat(value);
            
//            $('#tblSUPEDs').datagrid('beginEdit', index);
//        }
//    }
//    else {
//        if (field != 'Qty') {
//            $('#tblSUPEDs').datagrid('endEdit', index);
//        }
//        else {
//            if (oSUPED.LotID <= 0) {
//                alert('Please Assign Lot for :' + oSUPED.ProductShortName + ' and Try Again!');
//                return false;
//            }
//            _nQty = parseFloat(value);
            
//            $('#tblSUPEDs').datagrid('beginEdit', index);
//        }
//    }
//    _nIndex = index;
//    return true;
//}

var _nQty = 0;
var _nIndex = -1;
var _nBag = 0;

function onClickCell(index, field, value) {
    $('#tblSUPEDs').datagrid('selectRow', index);
    var oSUPED = $('#tblSUPEDs').datagrid('getSelected');
    
    if (_nIndex != index && _nIndex != -1) {
        $('#tblSUPEDs').datagrid('endEdit', _nIndex);

        if (field == 'Qty') {
            if (oSUPED.LotID <= 0) {
                alert('Please Assign Lot for :' + oSUPED.ProductShortName + ' and Try Again!');
                return false;
            }
            _nQty = parseFloat(value);

            $('#tblSUPEDs').datagrid('beginEdit', index);
        }
    }
    else {
        
        if (field != 'Qty' && field != 'Bag') {
            $('#tblSUPEDs').datagrid('endEdit', index);
        }
        else if(field == 'Qty'){
            if (oSUPED.LotID <= 0) {
                alert('Please Assign Lot for :' + oSUPED.ProductShortName + ' and Try Again!');
                return false;
            }
            _nQty = parseFloat(value);
            $('#tblSUPEDs').datagrid('beginEdit', index);
        }
        else if (field == 'Bag') {
            _nBag = parseFloat(value);
            $('#tblSUPEDs').datagrid('beginEdit', index);
        }
    }
    _nIndex = index;
    return true;
}


function EndEdit(index, row, changes) {
    var oSUPED = row;
    if (changes.Qty != null && _nQty != parseFloat(changes.Qty) && changes.Bag != null && _nBag != parseFloat(changes.Bag))
    {
        oSUPED.Qty = parseFloat(changes.Qty);
        oSUPED.Bag = parseFloat(changes.Bag);
        SaveSUPED(oSUPED);
        return true;
    }
    else if (changes.Qty != null && _nQty != parseFloat(changes.Qty)) {
        oSUPED.Qty = parseFloat(changes.Qty);
        SaveSUPED(oSUPED);
        return true;
    }
    else if (changes.Bag != null && _nBag != parseFloat(changes.Bag)) {
        oSUPED.Bag = parseFloat(changes.Bag);
        SaveSUPED(oSUPED);
        return true;
    }
  
}

function GenerateTableColumnsSUPED() {
    _otblColumns = [];
    var oColProductCode = { field: "ProductCode", width: "10%", title: "Code", frozen: true };
    var oColProductShortName = { field: "ProductShortName", width: "14%", title: "Count/Product", frozen: true };
    var oColProductName = { field: "ProductName", width: "30%", title: "Description", frozen: true };
    var oColLotNo = { field: "LotNo", width: "10%", title: "Lot No", frozen: true };
    var oColQty = { field: "Qty", width: "18%", title: "Qty(KG)", editor: { type: 'numberbox', options: { precision: 2 } }, align: 'right', frozen: true };
    var oColQtyInPounds = { field: "QtyInPounds", width: "18%", title: "Qty(LBS)", align: 'right', frozen: true };
    var oColBag = { field: "Bag", width: "16%", title: "Bag", editor: { type: 'numberbox', options: { precision: 2 } }, align: 'right', frozen: true };
    _otblColumns.push(oColProductCode, oColProductShortName, oColProductName, oColLotNo, oColQty, oColQtyInPounds, oColBag);
    $('#tblSUPEDs').datagrid({ columns: [_otblColumns] });
    $('#tblSUPEDs').datagrid({ onClickCell: function (index, field, value) { return onClickCell(index, field, value); }, onEndEdit: function (index, row, changes) { return EndEdit(index, row, changes);} });
}

function RefreshcboReceivedStores(oReceiveStores)
{
    $('#cboFinishStoresSUPE').empty();
    var listItems;
    listItems += '<option value="' + 0 + '">--Select Store--</option>';
    for (var i = 0; i < oReceiveStores.length; i++) {
        listItems += '<option value="' + oReceiveStores[i].WorkingUnitID + '">' + oReceiveStores[i].OperationUnitName + '</option>';
    }
    $('#cboFinishStoresSUPE').html(listItems);
    $('#cboFinishStoresSUPE').val(_oSUPE.ReceivedStoreID);
}

function RefreshControlSUPE(oSUPE) {
    _oSUPE = oSUPE;
    $('#txtExecutionNoSUPE').val('');
    $("#txtExecutionNoSUPE").removeClass("fontColorOfPickItem");
    $("#txtExecutionNoSUPE").val(_oSUPE.ExecutionNo);
    RefreshcboReceivedStores(_oReceivedStores);
    $("#txtExecutionDateSUPE").val(icsdateformat(new Date()));
    $("#cboFinishStoresSUPE").val(_oSUPE.ReceivedStoreID);
    $("#txtRemarks").val(_oSUPE.Remarks);
    ////region SUPED
    GenerateTableColumnsSUPED();
    DynamicRefreshList(_oSUPE.SUPEDetails, 'tblSUPEDs');
    RefreshTotalSummery();
    ////end region SUPED
}

function RefreshLayoutSUPE(buttonId) {
    if (buttonId === "btnViewSUPE" || buttonId === "btnReqforApproveSUPE") {
        $("#winSUPE input").prop("disabled", true);
        $("#winSUPE select").prop("disabled", true);
        $("#btnSaveSUPE").hide();
        $("#btnReceiveConfirmSUPE").hide();
        $("#txtPickProductSUPED").hide();
        $("#btnPickLotSUPED").hide();
        $("#btnRefreshSUPED").hide();
        $("#btnRemoveSUPED").hide();
    }
    else {
        $("#winSUPE input").not(".txtDisabledSUPE").prop("disabled", false);
        $("#winSUPE select").prop("disabled", false);
        $("#btnSaveSUPE").show();
        $("#btnReceiveConfirmSUPE").show();
        $("#txtPickProductSUPED").show();
        $("#btnPickLotSUPED").show();
        $("#btnRefreshSUPED").show();
        $("#btnRemoveSUPED").show();
    }
    if (_oSUPE.SUPEDetails.length > 0) {
        $("#cboFinishStoresSUPE").prop("disabled", true);
    }
}

function RefreshObjectSUPE() {
    var nSUPEId = (_oSUPE == null ? 0 : (_oSUPE.SUProductionExecutionID == null) ? 0 : _oSUPE.SUProductionExecutionID);
    var oSUPE = {
        SUProductionExecutionID: nSUPEId,
        ExecutionNo: _oSUPE.ExecutionNo,
        ExecutionDate: icsdatetimeformat(new Date),
        ExecutionStatus: _oSUPE.ExecutionStatus,
        ReceivedStoreID: $.trim($('#cboFinishStoresSUPE').val()),
        Remarks: $.trim($("#txtRemarks").val())
    };
    return oSUPE;
}

function RefreshObjectSUPED() {
    var nSUPEDId = (_oSUPED == null ? 0 : (_oSUPED.SUProductionExecutionDetailID == null) ? 0 : _oSUPED.SUProductionExecutionDetailID);
    var oSUPED = {
        SUProductionExecutionDetailID: nSUPEDId,
        SUProductionExecutionID: _oSUPE.SUProductionExecutionID,
        ProductID: _oProduct.ProductID,
        LotID: 0,
        MUnitID: _oProduct.MeasurementUnitID,
        Qty: 0,
        Bag: 0,
        SUPE: null
    };
    return oSUPED;
}

function ValidateInputSUPE() {
    var nRecevedStoreID = $('#cboFinishStoresSUPE').val();
    if (nRecevedStoreID == null || nRecevedStoreID <= 0)
    {
        alert("Please Select a Store!");
        $('#cboFinishStoresSUPE').val("");
        $('#cboFinishStoresSUPE').focus();
        $('#cboFinishStoresSUPE').css("border", "1px solid #c00");
        return false;
    } else {
        $('#cboFinishStoresSUPE').css("border", "");
    }
    return true;
}

function ReceiveFinishedGoods(oSUPE, sSuccessMessage) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SUProductionExecution/ReceiveFinishedGoods",
        traditional: true,
        data: JSON.stringify(oSUPE),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var oSUPE = jQuery.parseJSON(data);
            if (oSUPE.ErrorMessage == "" || oSUPE.ErrorMessage == null) {
                alert(sSuccessMessage);
                var oSelectedObject = $('#tblSUPEs').datagrid("getSelected");
                var nRowIndex = $('#tblSUPEs').datagrid("getRowIndex", oSelectedObject);
                $('#tblSUPEs').datagrid('updateRow', { index: nRowIndex, row: oSUPE });
                _oDBSUPEs = [];
                _oDBSUPEs = $("#tblSUPEs").datagrid('getRows');
                for (var i = 0; i < _oDBSUPEs.length; i++) {
                    _oDBSUPEs[i].ExecutionDate = _oDBSUPEs[i].ExecutionDateInString;
                }
                $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUPEs));
                $("#winSUPE").icsWindow('close');
            } else {
                alert(oSUPE.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}