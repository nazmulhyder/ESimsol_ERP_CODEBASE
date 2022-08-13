var _oExportBill = null;
var _oExportBill = null;


function LoadExportBillsEvents()
{

    
    $("#btnAddExportBill").click(function () {
        _oExportBill = null;
        var oExportBill = NewExportBill();
        DynamicRefreshList([], "tblExportBillDetail");
        $("#winExportBill").icsWindow('open', "Add Export Bill");       
        RefreshExportBillControl(oExportBill);
        $("#txtExportBillNo").prop("disabled", true);
    });

    $("#btnEditExportBill").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || parseInt(oExportBill.ExportBillID) <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBill").icsWindow('open', "Edit ExportBill");
        RefreshExportLCLayout("btnEditExportLC");
        _oExportBill = oExportBill;
        GetExportBillInformation(oExportBill);
        $("#txtExportBillNo").prop("disabled", false);
    });

    $("#btnPickExportBillDetails").click(function () {
       
        var oExportBill = {
            ExportLCID: _oExportLC.ExportLCID,
        };

        if (oExportBill == null || oExportBill.ExportLCID <= 0) {
            alert("Please select an item from list!"); return;
        }
        endEditing();
        $("#winExportBillDetails").icsWindow('open', "Export Bill");
        ///   RefreshExportBillLayout("btnAddRecedFromParty");
        
        Gets_ExportBillDetails(oExportBill);
    });


    $("#btnOkExportBillDetails").click(function (e) {
      
        var oExportBillDetail = $("#tblPIDetails").icsGetSelectedItem();
        if (oExportBillDetail == null && oExportBillDetail.ExportPILCMappingID <= 0) {
            alert("Please select a PI Product!");
            return;
        }
        debugger;

        var dBillDate = $('#txtStartDateSt02').datebox('getValue');
        if ($.trim(dBillDate) == "")
        {
            $('#txtStartDateSt02').datebox('setValue', icsdateformat(new Date()));
        }

        var oExportBill = RefreshObjectExportBill();

        if (_oExportBill == null || parseInt(_oExportBill.ExportBillID) <= 0) {
            oExportBillDetail.ExportBill = oExportBill;
        }
        else
        {
            oExportBillDetail.ExportBillID = _oExportBill.ExportBillID;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportBillDetail,
            ObjectId: oExportBillDetail.ExportBillDetailID,
            ControllerName: "ExportBill",
            ActionName: "Save_ExportBillDetail",
            TableId: "tblExportBillDetail",
            IsWinClose: false,
            Message:""
        };

        $.icsSave(obj, function (response) {
          
            if (response.status && response.obj != null) {
                var oExportBillDetail = response.obj;
                if ($.trim(oExportBillDetail.ErrorMessage) == "") {
                    if (oExportBillDetail.ExportBillDetailID > 0) {
                        var oExportBillDetails = $("#tblExportBillDetail").datagrid("getRows");
                        
                        var nTotalValue = 0;
                        var sCurrency = "";
                        for (var i = 0; i < oExportBillDetails.length; i++) {
                            nTotalValue = nTotalValue + parseFloat(oExportBillDetails[i].Amount);
                            sCurrency = oExportBillDetails[i].Currency;
                        }
                        debugger;
                        if (oExportBillDetail.ExportBill != null) {
                            _oExportBill = oExportBillDetail.ExportBill;
                            _oExportBill.Amount = parseFloat(nTotalValue);
                            _oExportBill.AmountSt = sCurrency + "" + formatPrice(nTotalValue);
                            var oExportBills = $("#tblExportBills").datagrid("getRows");
                            var nIndexEBill = oExportBills.length;
                            $("#tblExportBills").datagrid("appendRow", _oExportBill);
                            $("#tblExportBills").datagrid("selectRow", nIndexEBill);
                        }
                        else {
                            var oExportBilltemp = $('#tblExportBills').datagrid('getSelected');
                            var SelectedRowIndex_EBill = $('#tblExportBills').datagrid('getRowIndex', oExportBilltemp);
                            oExportBilltemp.Amount = parseFloat(nTotalValue);
                            oExportBilltemp.AmountSt = sCurrency + "" + formatPrice(nTotalValue);
                            $('#tblExportBills').datagrid('updateRow', { index: SelectedRowIndex_EBill, row: oExportBilltemp });
                        }
                    
                        _oExportBill.ExportBillID = oExportBillDetail.ExportBillID;
                        _oExportBill.ExportBillDetails = oExportBillDetails;
                        RefreshExportBillControl(_oExportBill);
                        RefreshTotalSummery_EBillDetail();
                        RefreshTotalSummery_EBill();
                        
                        SetRemainingBillAmountLC(_oExportLC);
                    }
                }
            }
        });
    });
  
  
    $("#btnSaveExportBill").click(function (e) {
        //  if (!ValidateInputExportLC(false)) return;
        endEditing();
        debugger;
        var oExportBill = RefreshObjectExportBill();
        //Ratin

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveExportBill",
            TableId: "tblExportBills",
            IsWinClose: true
        };

        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                var oExportBill = response.obj;
                if ($.trim(oExportBill.ErrorMessage) == "") {
                    if (oExportBill.ExportBillID > 0) {
                        _oExportLC = $('#tblExportLCs').datagrid('getSelected');
                        var SelectedRowIndex = $('#tblExportLCs').datagrid('getRowIndex', _oExportLC);
                        _oExportBill = oExportBill;
                        if (_oExportBill.ExportBillID > 0) {

                            var oExportBillDetails = $("#tblExportBillDetail").datagrid("getRows");
                            var nTotalValue = 0;
                            var sCurrency = "";
                            for (var i = 0; i < oExportBillDetails.length; i++) {
                                nTotalValue = nTotalValue + parseFloat(oExportBillDetails[i].Amount);
                                sCurrency = oExportBillDetails[i].Currency;

                            }
                            RefreshTotalSummery_EBill();
                            
                            SetRemainingBillAmountLC(_oExportLC);
                        }
                        else {
                            alert(_oExportBill.ErrorMessage);
                        }
                    }
                }
                else {
                    alert(oExportBill.ErrorMessage);
                }
            }
        });












        
      
       /* $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportBill/SaveExportBill",
            traditional: true,
            data: JSON.stringify(oExportBill),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
               
                _oExportLC = $('#tblExportLCs').datagrid('getSelected');
                var SelectedRowIndex = $('#tblExportLCs').datagrid('getRowIndex', _oExportLC);
                _oExportBill = jQuery.parseJSON(data);
                if (_oExportBill.ExportBillID > 0) {
                   
                    var oExportBillDetails = $("#tblExportBillDetail").datagrid("getRows");
                    var nTotalValue = 0;
                    var sCurrency = "";
                    for (var i = 0; i < oExportBillDetails.length; i++) {
                        nTotalValue = nTotalValue + parseFloat(oExportBillDetails[i].Qty * oExportBillDetails[i].UnitPrice);
                        sCurrency = oExportBillDetails[i].Currency;

                    }

                    _oExportBill.AmountSt = sCurrency + "" + formatPrice(nTotalValue);
                    if (!IsExists_EBill(_oExportBill))
                    {
                        var oExportBills = $("#tblExportBills").datagrid("getRows");
                        var nIndex = oExportBills.length;
                        $("#tblExportBills").datagrid("appendRow", _oExportBill);
                        $("#tblExportBills").datagrid("selectRow", nIndex);
                    }
                    else{
                        var oExportBilltemp = $('#tblExportBills').datagrid('getSelected');
                        var SelectedRowIndex_EBill = $('#tblExportBills').datagrid('getRowIndex', oExportBilltemp);
                        oExportBilltemp.AmountSt = sCurrency + "" + formatPrice(nTotalValue);
                        oExportBilltemp.Amount = parseFloat(nTotalValue);
                        $('#tblExportBills').datagrid('updateRow', { index: SelectedRowIndex_EBill, row: oExportBilltemp });
                        
                          $('#tblExportLCs').datagrid('updateRow', { index: SelectedRowIndex, row: _oExportLC });
                    }
                    RefreshTotalSummery_EBill();
                    $("#winExportBill").icsWindow("close");
                    
                    if (_oExportLC.ExportLCID > 0)
                    {
                        CheckRemainingBillAmountLC();
                    }
                }
                else {
                    CheckRemainingBillAmountLC();
                    alert(_oExportBill.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                CheckRemainingBillAmountLC();
                alert(error);
            }
        });*/
    });
   
    $("#btnDeleteExportBillDetails").click(function () {
        var oExportBillDetail = $("#tblExportBillDetail").datagrid("getSelected");
        if (oExportBillDetail == null || oExportBillDetail.ExportBillDetailID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        oExportBillDetail.ExportBill = $("#tblExportBills").datagrid("getSelected");
        oExportBillDetail.ExportBill.ExportBillDetails = [];
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBillDetail,
            ObjectId: oExportBillDetail.ExportBillDetailID,
            ControllerName: "ExportBill",
            ActionName: "DeleteExportBillDetail",
            TableId: "tblExportBillDetail",
            IsWinClose: false
        };
        $.icsDelete(obj, function (response) {
            if (response.Message == "deleted")
            {
                var oExportBill = $("#tblExportBills").datagrid("getSelected");
                var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oExportBill);
                oExportBill.Amount = parseFloat(oExportBill.Amount) - parseFloat(oExportBillDetail.Amount);
                oExportBill.AmountSt = oExportBill.Currency + " " + formatPrice(oExportBill.Amount);
                $("#tblExportBills").datagrid("updateRow", { index: nRowIndex, row: oExportBill });
            }
            
          
            RefreshTotalSummery_EBillDetail();
            RefreshTotalSummery_EBill();
            SetRemainingBillAmountLC(_oExportLC);
            editIndex = undefined;
            endEditing();
        });
    });

    $("#btnDeleteExportBill").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var oExportBilltemp = { ExportBillID: oExportBill.ExportBillID };
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBilltemp,
            ObjectId: oExportBilltemp.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "Delete",
            TableId: "tblExportBills",
            IsWinClose: false
        };
        $.icsDelete(obj, function (response) {
            RefreshTotalSummery_EBill();
            SetRemainingBillAmountLC(_oExportLC);
        });
    });


    $("#btnCloseExportBills").click(function () {
        $("#winExportBills").icsWindow("close");
    });
}

function SetRemainingBillAmountLC(oExportLC) {
    if (oExportLC.ExportLCID > 0)
    {
        var nTotalRemainingBillAmount = 0;
        var oExportBills = $("#tblExportBills").datagrid("getRows");
        for (var i = 0; i < oExportBills.length; i++) {
            nTotalRemainingBillAmount = parseFloat(oExportBills[i].Amount) + parseFloat(nTotalRemainingBillAmount);
        }
        nTotalRemainingBillAmount = parseFloat(oExportLC.Amount) - parseFloat(nTotalRemainingBillAmount);
        $("#lblRemaingBill").text(oExportLC.Currency + " " + formatPrice(nTotalRemainingBillAmount));
    }
}

function Gets_ExportBillDetails(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "GetsExportBillDetails",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        DynamicRefreshList([], "tblPIDetails");
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblPIDetails");

            }
            else { DynamicRefreshList([], "tblPIDetails"); }
        }
    });
}

function GetExportBillInformation(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "GetExportBill",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) {
                RefreshExportBillControl(response.obj);
            }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}



function RefreshExportBillLayout(buttonId) {
    if (buttonId === "btnViewExportBill") {
        $("#winExportBill input").prop("disabled", true);
        $("#btnSaveExportBill").hide();
    }
    else {
        $("#winExportBill input").prop("disabled", false);
        $("#btnSaveExportBill").show();
    }
    $(".disabled input").prop("disabled", true);
}

function RefreshExportBillControl(oExportBill)
{
  
    debugger;
    _oExportBill = oExportBill;

    DynamicRefreshList(_oExportBill.ExportBillDetails, "tblExportBillDetail");

    $("#txtExportBillNo").val(_oExportBill.ExportBillNo);
    $("#txtExportLCNo02").val(_oExportBill.ExportLCNo);
    $("#txtApplicantName02").val(_oExportBill.ApplicantName);
    $("#txtNoOfPackages").val(_oExportBill.NoOfPackages);
    $("#txtNetWeight").val(_oExportBill.NetWeight);
    $("#txtGrossWeight").val(_oExportBill.GrossWeight);
    $("#txtNote_ExportBill").val(_oExportBill.NoteCarry);
  
    if (_oExportBill.StartDateSt == "--" || _oExportBill.StartDateSt == "")
    {
        $("#txtStartDateSt02").datebox("setValue", icsdateformat(new Date()));
    }
    else
    {
        $("#txtStartDateSt02").datebox("setValue", _oExportBill.StartDateSt);
    }
    RefreshTotalSummery_EBillDetail();

}


function NewExportBill() {
    
    var oExportBill = {
        ExportBillID: 0,
        ExportLCNo: _oExportLC.ExportLCNo,
        ApplicantName: _oExportLC.ApplicantName,
        ExportBillNo: "",
        ExportBillDetails:[]
     
    };
    return oExportBill;
}



function RefreshObjectExportBill() {
  
    var oExportBill = {
        ExportBillID: (_oExportBill == null ? 0 : _oExportBill.ExportBillID),
        ExportLCID: _oExportLC.ExportLCID,
        ExportBillNo: document.getElementById("txtExportBillNo").value,
        StartDate: $('#txtStartDateSt02').datebox('getValue'),
        Amount: 0,
        NoOfPackages : $.trim($('#txtNoOfPackages').val()),
        NetWeight : $.trim($('#txtNetWeight').val()),
        GrossWeight : $.trim($('#txtGrossWeight').val()),
        NoteCarry : $.trim($('#txtNote_ExportBill').val())
    };
    return oExportBill;
}

function IsExists_EBill(oExportBill) {
    var oExportBills = $('#tblExportBills').datagrid('getRows');
    for (var i = 0; i < oExportBills.length; i++) {
        if (parseInt(oExportBills[i].ExportBillID) == parseInt(oExportBill.ExportBillID)) {
            return true;
        }
    }
    return false;
}

//123
function RefreshTotalSummery_EBillDetail() {
    var nTotalQty = 0;
    var nTotalValue = 0;
    var sCurrency = "";
    var sMUnit = "";
    var oExportBillDetails = $('#tblExportBillDetail').datagrid('getRows');
    for (var i = 0; i < oExportBillDetails.length; i++) {
        nTotalQty = nTotalQty + parseFloat(oExportBillDetails[i].Qty);
        nTotalValue = nTotalValue + parseFloat(oExportBillDetails[i].Amount);
        sCurrency = oExportBillDetails[i].Currency;
        sMUnit = oExportBillDetails[i].MUName;
    }
    $("#lblTotalQty_BillDetail").text(formatPrice(nTotalQty) + "" + sMUnit);
    $("#lblTotalValue_BillDetail").text(sCurrency + "" + formatPrice(nTotalValue));
}

function RefreshTotalSummery_EBill() {
    var nTotalValue = 0;
    var sCurrency = "";
    var oExportBills = $('#tblExportBills').datagrid('getRows');
    for (var i = 0; i < oExportBills.length; i++) {
     
        nTotalValue = nTotalValue + parseFloat(oExportBills[i].Amount);
        sCurrency = oExportBills[i].Currency;
    }
    $("#lblTotalValue_Bill").text(sCurrency + "" + formatPrice(nTotalValue));
}




var editIndex = undefined;
function endEditing() {
    
    if (editIndex == undefined) {
        return true;
    }
    if ($('#tblExportBillDetail').datagrid('validateRow', editIndex)) {
        $('#tblExportBillDetail').datagrid('endEdit', editIndex);
        $('#tblExportBillDetail').datagrid('selectRow', editIndex);
        var oExportBillDetail = $('#tblExportBillDetail').datagrid('getSelected');
        if (oExportBillDetail != null)
        {
            oExportBillDetail.Amount = parseFloat((oExportBillDetail.Qty / oExportBillDetail.RateUnit) * oExportBillDetail.UnitPrice)
            oExportBillDetail.AmountSt = oExportBillDetail.Currency + "" + formatPrice(oExportBillDetail.Amount);

            if (parseFloat(oExportBillDetail.Qty) != _nQty || parseFloat(oExportBillDetail.WtPerBag) != _nWtPerBag) {
                UpdateExportBillDetail(oExportBillDetail, editIndex);
            }
        }
     
        _nQty = 0;
        _nUnitPrice = 0;
        editIndex = undefined;
        return true;
    }
    else {
        return false;
    }
}

var _nQty = 0;
var _nUnitPrice = 0;
var _nWtPerBag = 0;

function onClickRow(index) {
    if (editIndex != index) {
        if (endEditing()) {
            $('#tblExportBillDetail').datagrid('selectRow', index).datagrid('beginEdit', index);
            var oExportBillDetail = $('#tblExportBillDetail').datagrid('getSelected');
            _nQty = parseFloat(oExportBillDetail.Qty);
            _nUnitPrice = parseFloat(oExportBillDetail.UnitPrice);
            _nWtPerBag = parseFloat(oExportBillDetail.WtPerBag);
            editIndex = index;
        }
        else {
            $('#tblExportBillDetail').datagrid('selectRow', editIndex);
        }
    }
}

function UpdateExportBillDetail(oExportBillDetail, SelectedRowIndex) {
    
    oExportBillDetail.ExportBill = null;
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oExportBillDetail,
        ObjectId: oExportBillDetail.ExportBillDetailID,
        ControllerName: "ExportBill",
        ActionName: "Save_ExportBillDetail",
        TableId: "",
        IsWinClose: false,
        Message: ""
    };
    $.icsSave(obj, function (response) {
        if (response.status && response.obj != null) {
            var oExportBillDetail = response.obj;
            if (parseInt(oExportBillDetail.ExportBillDetailID) > 0) {
                $('#tblExportBillDetail').datagrid('updateRow', { index: SelectedRowIndex, row: oExportBillDetail });
                RefreshTotalSummery_EBillDetail();
            }
            //else {
            //    alert(oExportBillDetail.ErrorMessage);
            //    return;
            //}
        }
    });








    //$.ajax({
    //    type: "POST",
    //    dataType: "json",
    //    url: _sBaseAddress + "/ExportBill/Save_ExportBillDetail",
    //    traditional: true,
    //    data: JSON.stringify(oExportBillDetail),
    //    contentType: "application/json; charset=utf-8",
    //    success: function (data) {
    //       
    //        var oExportBillDetail = jQuery.parseJSON(data);
    //        if (parseInt(oExportBillDetail.ExportBillDetailID) > 0) {
    //            $('#tblExportBillDetail').datagrid('updateRow', { index: SelectedRowIndex, row: oExportBillDetail });
    //            RefreshTotalSummery_EBillDetail();
    //        }
    //        else {
    //            alert(oExportBillDetail.ErrorMessage);
    //            return;
    //        }
    //    },
    //    error: function (xhr, status, error) {
    //        alert(error);
    //    }
    //});
}
