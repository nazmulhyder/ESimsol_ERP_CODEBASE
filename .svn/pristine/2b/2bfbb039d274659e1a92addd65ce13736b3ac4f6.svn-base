
var _oExportBill = [];

function LoadExport_LDBPEvents() {


    $("#cboBankBranch_Nego").icsLoadCombo({
        List: _oBankBranchs_Nego,
        OptionValue: "BankBranchID",
        DisplayText: "BankBranchName"
    });

    $("#cboBankAccount").icsLoadCombo({
        List: _oBankAccounts,
        OptionValue: "BankAccountID",
        DisplayText: "AccountNameandNo"
    });
    $("#cboBankAccount_LDBPNo").icsLoadCombo({
        List: _oBankAccounts,
        OptionValue: "BankAccountID",
        DisplayText: "AccountNameandNo"
    });

    $("#cboCurrency").icsLoadCombo({
        List: _oCurrencys,
        OptionValue: "CurrencyID",
        DisplayText: "CurrencyName"
    });

    function RefreshObject_Export_LDBP() {
        debugger;
        var oExport_LDBP = {
            Export_LDBPID: (_oExport_LDBP != null) ? _oExport_LDBP.Export_LDBPID : 0,
            RefNo: document.getElementById("txtRefNo").value,
            BankAccountID: parseInt($("#cboBankAccount").val()),
            BUID: _buid,
            LetterIssueDate: $('#txtIssueDate').datebox('getValue'),
            //RequestBy: (_oExport_LDBP.RequestBy != null) ? _oExport_LDBP.RequestBy : 0,
            //ApprovedBy: (_oExport_LDBP.ApprovedBy != null) ? _oExport_LDBP.ApprovedBy : 0,
            CurrencyType: $("#chkForeign").is(":checked"),
            Note: document.getElementById("txtNote").value,
            Export_LDBPDetails: $('#tblExport_LDBPDetail').datagrid('getRows')
        };
        return oExport_LDBP;
    }

    $("#btnSaveExport_LDBP").click(function () {
        //    if (!ValidateInputBank()) return;
        var oExport_LDBP = RefreshObject_Export_LDBP();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExport_LDBP,
            ObjectId: oExport_LDBP.Export_LDBPID,
            ControllerName: "Export_LDBP",
            ActionName: "Save",
            TableId: "tblExport_LDBPs",
            IsWinClose: true
        };
        $.icsSave(obj);
    });
    $("#btnApproved_ELDBP").click(function () {
        //    if (!ValidateInputBank()) return;
        var oExport_LDBP = RefreshObject_Export_LDBP();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExport_LDBP,
            ObjectId: oExport_LDBP.Export_LDBPID,
            ControllerName: "Export_LDBP",
            ActionName: "Approved",
            TableId: "tblExport_LDBPs",
            IsWinClose: true
        };
        $.icsSave(obj);
    });


    $("#btnSaveLDBP").click(function () {
        //    if (!ValidateInputBank()) return;
        debugger;
    
        _oExport_LDBPDetail.Export_LDBPDetailID
        var oExport_LDBPDetail = RefreshObject_AddLDBP();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExport_LDBPDetail,
            ObjectId: oExport_LDBPDetail.Export_LDBPDetailID,
            ControllerName: "Export_LDBP",
            ActionName: "Save_LDBP",
            TableId: "tblExport_LDBPDetail",
            IsWinClose: true
        };
        $.icsSave(obj);
    });

    $("#btnCloseExport_LDBP").click(function () {
        $("#winExport_LDBP").icsWindow('close');
    });

    $("#btnCloseLDBP").click(function () {
        $("#winAddExportLDBP").icsWindow('close');
    });

      //Start Account Of Search
    //Start Account Of Search
    $("#winExportBillsPicker").on("keydown", function (e) {
        var oExport_LDBPDetail = $('#tblExportBillsPicker').datagrid('getSelected');
        var nIndex = $('#tblExportBillsPicker').datagrid('getRowIndex', oExport_LDBPDetail);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblExportBillsPicker').datagrid('selectRow', 0);
            }
            else {
                $('#tblExportBillsPicker').datagrid('selectRow', nIndex - 1);
            }
          
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblExportBillsPicker').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblExportBillsPicker').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblExportBillsPicker').datagrid('selectRow', nIndex + 1);
            }
          
        }
        if (e.which == 13)//enter=13
        {
            var oExport_LDBPDetail = $("#tblExportBillsPicker").icsGetSelectedItem();
            if (oExport_LDBPDetail != null && oExport_LDBPDetail.ExportBillID > 0) {
                RefreshTotalSummery();
                //$('#txtSearchByAccountOfPI').val(oContractor.Name);
                //$("#txtSearchByAccountOfPI").addClass("fontColorOfPickItem");
                //_oExportPI.ContractorID = oContractor.ContractorID;
                //$("#txtSearchByBuyerPI").focus();
            }
        }
    });

  

    $("#btnPickExportLC").click(function () {
        PickExportBills();
    });
    $("#btnOkExportBillsPicker").click(function (e) {


        var oExport_LDBPDetail = $("#tblExportBillsPicker").icsGetSelectedItem();
        if (oExport_LDBPDetail == null || oExport_LDBPDetail.ExportBillID <= 0)
        {
            alert("Please select a PI Product!");
            return;
        }
        var oExport_LDBPDetails = $("#tblExport_LDBPDetail").datagrid("getRows");
        var nIndex = oExport_LDBPDetails.length;
        $("#tblExport_LDBPDetail").datagrid("appendRow", oExport_LDBPDetail);
        $("#tblExport_LDBPDetail").datagrid("selectRow", nIndex);

        RefreshTotalSummery();
        debugger;
     
    });

    $("#txtSearchByLDBCNoPicker").keydown(function (e) {

        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oAccountsOf,
            TableId: "tblExportBillsPicker"
        };
        $('#txtSearchByAccountNamePicker').icsSearchByText(obj);

    });

    $("#btnCloseExportBillsPicker").click(function () {
        $("#winExportBillsPicker").icsWindow("close");
        _oAccountsOf = [];
        $("#winExportBillsPicker input").val("");
        DynamicRefreshList([], "tblExportBillsPicker");
    });
  
  
    $('#txtLDBCNoPick').keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            var oExport_LDBP = {
                BankBranchID:0,
                LDBCNo: $.trim($("#txtLDBCNoPick").val()),
                ExportLCNo:"",
                ExportBillNo: "",
                ErrorMessage: "",
                BUID: _buid
            };
            GetsExportBills(oExport_LDBP)
        }
       
    });
    $('#txtExportLCNoPick').keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            var oExport_LDBP = {
                BankBranchID: 0,
                LDBCNo: "",
                ExportLCNo: $.trim($("#txtExportLCNoPick").val()),
                ExportBillNo: "",
                ErrorMessage: "",
                BUID: _buid
            };
            GetsExportBills(oExport_LDBP)
        }

    });
    $('#txtBillNoPick').keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            var oExport_LDBP = {
                BankBranchID: 0,
                LDBCNo: "",
                ExportLCNo: "",
                ExportBillNo: $.trim($("#txtBillNoPick").val()),
                ErrorMessage: "",
                BUID: _buid
            };
            GetsExportBills(oExport_LDBP)
        }

    });
}




function IsLocal() {
    document.getElementById("chkLocal").checked = true;
    document.getElementById("chkForeign").checked = false;
}
function IsForeign() {
    document.getElementById("chkLocal").checked = false;
    document.getElementById("chkForeign").checked = true;
}






function SetCCRate() {
    _nBaseCurrencyID = $("#cboCurrency").val();
    if (parseInt(_nBaseCurrencyID) == 1) {
        $("#txtCCRate").val(1);
        $("#txtCCRate").attr("disabled", true);
    }
    else {
        $("#txtCCRate").val("");
        $("#txtCCRate").attr("disabled", false);
    }
}




function RefreshObject_AddLDBP() {
    var nBankAccountID = 0;
    var cboBankAccount = document.getElementById("cboBankAccount_LDBPNo");
    if (cboBankAccount.selectedIndex <= 0) {
        nBankAccountID = 0;
    }
    else {
        nBankAccountID = cboBankAccount.options[cboBankAccount.selectedIndex].value;
    }

    var scboCurrencys = document.getElementById("cboCurrency");
    var nCurrencyID = scboCurrencys.options[scboCurrencys.selectedIndex].value;

    var oExport_LDBPDetail = {
        Export_LDBPDetailID: _oExport_LDBPDetail.Export_LDBPDetailID,
        ExportBillID: _oExport_LDBPDetail.ExportBillID,
        Export_LDBPID: _oExport_LDBPDetail.Export_LDBPID,
        LDBPNo: document.getElementById("txtLDBPNo").value,
        LDBPDate: $('#txtLDBPDate').datebox('getValue'),
        LDBPAmount: $('#txtLDBPAmount').numberbox('getValue'),
        CurrencyID: nCurrencyID,
        CCRate: $('#txtCCRate').numberbox('getValue'),
        BankAccountIDRecd: nBankAccountID
    };
    return oExport_LDBPDetail;
}


function RefreshTotalSummery() {
    
    var nTotalValue = 0;
    var sCurrency = "";
    var oExport_LDBPDetail = $('#tblExport_LDBPDetail').datagrid('getRows');
    for (var i = 0; i < oExport_LDBPDetail.length; i++) {
        nTotalQty = 0;
        nTotalValue = nTotalValue + parseFloat(oExport_LDBPDetail[i].Amount);
        sCurrency = oExport_LDBPDetail[i].Currency;
    }
    document.getElementById("lblTotalValue").innerHTML = sCurrency + " " + formatPrice(nTotalValue);
    
}



function PickExportBills() {

    var oExport_LDBP = {
        BankBranchID: parseInt($("#cboBankBranch_Nego").val()),
        LDBCNo: "",
        ExportLCNo: "",
        ExportBillNo: "",
        ErrorMessage: "",
        BUID: _buid
    };
    GetsExportBills(oExport_LDBP)
}

function GetsExportBills(oExport_LDBP)
{
    debugger;

    var oExportBills = $('#tblExport_LDBPDetail').datagrid('getRows');
    var sExportBillID = "";
    if (oExportBills != null && oExportBills.length > 0) {
        for (var i = 0; i < oExportBills.length; i++) {
            sExportBillID = oExportBills[i].ExportBillID + "," + sExportBillID;
        }
        sExportBillID = sExportBillID.substring(0, sExportBillID.length - 1);
    }

    oExport_LDBP.ErrorMessage = sExportBillID;

   
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oExport_LDBP,
        ControllerName: "Export_LDBP",
        ActionName: "GetsExportBills",
        IsWinClose: false
    };
    $("#progressbar").progressbar({ value: 0 });
    $("#progressbarParent").show();
    var intervalID = setInterval(updateProgress, 250);
    $.icsDataGets(obj, function (response) {
        clearInterval(intervalID);
        $("#progressbarParent").hide();
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ExportBillID > 0) {
                debugger;
                var tblColums = [];
                var oColumn = { field: "ExportBillNo", title: "Bill No", width: 80, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "LDBCNo", title: "LDBCNo", width: 80, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ExportLCNo", title: "L/C No", width: 120, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "AmountSt", title: "Amount", width: 100, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "MaturityDateSt", title: "MaturityDate", width: 100, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "BankName_Issue", title: "Issue Bank", width: 100, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "NegoBank", title: "Nego. Bank", width: 100, align: "left" }; tblColums.push(oColumn);

                var oPickerParam = {
                    winid: 'winExportBillsPicker',
                    winclass: 'clsExportBillPicker',
                    winwidth: 850,
                    winheight: 460,
                    tableid: 'tblExportBillPicker',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: true,
                    searchingbyfieldName: 'LDBCNo',
                    windowTittle: 'LDBC No'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
        else {
            alert("No Bill found.");
        }
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
    var oreturnObj = null, oreturnObjs = [];
    if (oPickerobj.multiplereturn) {
        oreturnObjs = $('#' + oPickerobj.tableid).datagrid('getChecked');
    } else {
        oreturnObj = $('#' + oPickerobj.tableid).datagrid('getSelected');
    }

    if (oPickerobj.winid == 'winExportBillsPicker') {


        if (oreturnObjs != null && oreturnObjs.length > 0) {
            ExportBillMapping(oreturnObjs);
        }


        else {
            alert("Please select an Applicant.");
            return false;
        }
    }




    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
}

function ExportBillMapping(oExportBills) {
    debugger;
    var bExists = false;
    for (i = 0; i < oExportBills.length; i++) {
        var bExists = IsExist(oExportBills[i].ExportBillID)
        if (!bExists) {

            $('#tblExport_LDBPDetail').datagrid('appendRow', oExportBills[i]);
        }
    }
    //endEditing();
    //RefreshTotalSummery_EBillDetail();
}
function IsExist(nExportBillID) {
    var oPRDetails = $('#tblExport_LDBPDetail').datagrid('getRows');

    for (var i = 0; i < oPRDetails.length; i++) {
        if (parseInt(oPRDetails[i].ExportBillID) == parseInt(nExportBillID)) {
            return true;
        }
    }

    return false;
}
