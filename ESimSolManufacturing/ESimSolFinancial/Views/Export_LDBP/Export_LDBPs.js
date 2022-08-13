

function LoadExport_LDBPsEvents()
{
    DynamicRefreshList(_oExport_LDBPs, "tblExport_LDBPs");
    
    
    $("#btnAddExport_LDBP").click(function () {
        $("#winExport_LDBP").icsWindow("open", "Add Export_LDBP");
        ResetAllFields("winExport_LDBP");
        _oExport_LDBP = NewoExport_LDBP();
        RefreshExport_LDBPControl(_oExport_LDBP);
        RefreshExport_LDBPLayout("btnAddExport_LDBP"); //button id as parameter
    });

    $("#btnEditExport_LDBP").click(function () {
        var oExport_LDBP = $("#tblExport_LDBPs").datagrid("getSelected");
        if (oExport_LDBP == null || oExport_LDBP.Export_LDBPID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExport_LDBP").icsWindow('open', "Edit Export_LDBP");
        _oExport_LDBP = oExport_LDBP;
        RefreshExport_LDBPLayout("btnEditExport_LDBP");
        GetExport_LDBPInformation(oExport_LDBP);
    });

    $("#btnReceiveLDBP").click(function () {
        var oExport_LDBP = $("#tblExport_LDBPs").datagrid("getSelected");
        if (oExport_LDBP == null || oExport_LDBP.Export_LDBPID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExport_LDBP").icsWindow('open', "View Export_LDBP");
        RefreshExport_LDBPLayout("btnReceiveLDBP");
        GetExport_LDBPInformation(oExport_LDBP);
    });
    $("#btnApprovedExport_LDBP").click(function () {
        var oExport_LDBP = $("#tblExport_LDBPs").datagrid("getSelected");
        if (oExport_LDBP == null || oExport_LDBP.Export_LDBPID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExport_LDBP").icsWindow('open', "View Export_LDBP");
        RefreshExport_LDBPLayout("btnApprovedExport_LDBP");
        GetExport_LDBPInformation(oExport_LDBP);
    });

    $("#btnDeleteExport_LDBP").click(function () {
        var oExport_LDBP = $("#tblExport_LDBPs").datagrid("getSelected");
        if (oExport_LDBP == null || oExport_LDBP.Export_LDBPID <= 0) { alert("Please select an item from list!"); return; }
        if (!confirm("Confirm to Delete?")) return false;
      

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExport_LDBP,
            ControllerName: "Export_LDBP",
            ActionName: "Delete",
            TableId: "tblExport_LDBPs",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });
    $('#btnReloadPage').click(function () {
        location.reload();
    });
    $('#btnRemoveExport_LDBPDetail').click(function () {
        debugger;
        var oExport_LDBPDetail = $('#tblExport_LDBPDetail').datagrid('getSelected');
        var SelectedRowIndex = $('#tblExport_LDBPDetail').datagrid('getRowIndex', oExport_LDBPDetail);
        if (oExport_LDBPDetail == null) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return;
        $('#tblExport_LDBPDetail').datagrid('deleteRow', SelectedRowIndex);
    });
  
    $("#btnPrintLetter").click(function () {
        var oExport_LDBP = $("#tblExport_LDBPs").datagrid("getSelected");
        if (oExport_LDBP == null || oExport_LDBP.Export_LDBPID <= 0) { alert("Please select an item from list!"); return; }
        
        window.open(_sBaseAddress + '/Export_LDBP/PrintPriviewExport_LDBP?id=' + oExport_LDBP.Export_LDBPID, "_blank");
    });


    $('#txtSearchbyLDBCNo').keypress(function (e) {
      
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            
            var oExportBill =
          {
              ExportBillID: 0,
              LDBCNo: document.getElementById('txtSearchbyLDBCNo').value
          };
            Gets_byLDBCNo(oExportBill);
        }

    });
    
    $("#btnAdvSearchExportBill").click(function () {
        $("#winAdvSearchExportBill").icsWindow("open", "Advance Search");
        DynamicRefreshList([], "tblExportBillAdvSearch");
        //DateActions();
        //ResetAdvSearch();
        //UnselectAllRowsOfATable();
    });

    ////////////// Add LDBP
    $("#btnAddLDBP").click(function () {
        var oExport_LDBPDetail = $("#tblExport_LDBPDetail").datagrid("getSelected");
        if (oExport_LDBPDetail == null || oExport_LDBPDetail.Export_LDBPDetailID <= 0) { alert("Please select an item from list!"); return; }
        $("#winAddExportLDBP").icsWindow('open', "Export Bill(LDBP Entry)");
        GetExport_LDBPDetail(oExport_LDBPDetail);
    });
    $("#btnloadBankAccountTwo,#btnloadBankAccount").click(function () {
        LoadBankBranchAccounts();
    });
    
}

function GetExport_LDBPInformation(oExport_LDBP) {

    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExport_LDBP,
        ControllerName: "Export_LDBP",
        ActionName: "GetExport_LDBP",
        IsWinClose: false
    };
    debugger;
    $.icsDataGet(obj, function (response) {

        if (response.status && response.obj != null) {

            if (response.obj.Export_LDBPID > 0) { RefreshExport_LDBPControl(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else { alert("No information found."); }
    });
}
function RefreshExport_LDBPLayout(buttonId) {
  
    if (buttonId === "btnApprovedExport_LDBP") {
        $("#winExport_LDBP input").prop("disabled", true);
        $("#winExport_LDBP select").prop("disabled", true);
        $('.tdInput input').css("border-color", "");
        $("#btnSaveExport_LDBP").hide();
        $("#btnApproved_ELDBP").show();
        $("#btnSaveExport_LDBP").hide();
        $("#btnRemoveExport_LDBPDetail").hide();
        $("#btnRefreshExport_LDBPDetail").hide();
        $("#btnCancelExport_LDBPDetail").hide();
        $("#btnAddLDBP").hide();
    }
    else if (buttonId === "btnReceiveLDBP") {
        $("#winExport_LDBP input").prop("disabled", false);
        $("#winExport_LDBP select").prop("disabled", false);
        $('.tdInput input').css("border-color", "");
        $("#btnSaveExport_LDBP").hide();
        $("#btnApproved_ELDBP").hide();
        $("#btnSaveExport_LDBP").hide();
        $("#btnRemoveExport_LDBPDetail").hide();
        $("#btnRefreshExport_LDBPDetail").hide();
        $("#btnCancelExport_LDBPDetail").show();
        $("#btnAddLDBP").show();
    }
    else if (buttonId === "btnEditExport_LDBP") {
        $("#winExport_LDBP input").not("#txtRefNo").prop("disabled", false);
        $("#winExport_LDBP select").prop("disabled", false);
        $("#btnSaveExport_LDBP").show();
        $("#btnApproved_ELDBP").hide();
        $("#btnRemoveExport_LDBPDetail").show();
        $("#btnRefreshExport_LDBPDetail").show();
        $("#btnCancelExport_LDBPDetail").hide();
        $("#btnAddLDBP").hide();
    }
    else {
        $("#winExport_LDBP input").not("#txtRefNo").prop("disabled", false);
        $("#winExport_LDBP select").prop("disabled", false);
        $("#btnSaveExport_LDBP").show();
        $("#btnApproved_ELDBP").hide();
        $("#btnCancelExport_LDBPDetail").hide();
        $("#btnAddLDBP").hide();
    }
  
}


function RefreshExport_LDBPControl(oExport_LDBP) {
    _oExport_LDBP = oExport_LDBP;
    DynamicRefreshList([], "tblExport_LDBPDetail");
    DynamicRefreshList(_oExport_LDBP.Export_LDBPDetails, "tblExport_LDBPDetail");
  
    //RefreshTotalSummery();
    debugger;

    document.getElementById("chkLocal").checked = true;
    document.getElementById("chkForeign").checked = false;

    if (_oExport_LDBP.CurrencyType == false) {
        document.getElementById("chkLocal").checked = true;
        document.getElementById("chkForeign").checked = false;
    }

    if (_oExport_LDBP.CurrencyType == true) {
        document.getElementById("chkLocal").checked = false;
        document.getElementById("chkForeign").checked = true;
    }
 
    
    $("#cboBankBranch_Nego").val(_oExport_LDBP.BankBranchID);
    $("#cboBankAccount").val(_oExport_LDBP.BankAccountID);

    $("#txtRefNo").val(_oExport_LDBP.RefNo);
    $("#txtNote").val(_oExport_LDBP.Note);
    $('#txtIssueDate').datebox('setValue', _oExport_LDBP.LetterIssueDateInSt);

    RefreshTotalSummery();

}

function Gets_byLDBCNo(oExportBill) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "Export_LDBP",
        ActionName: "GetbyLDBCNo",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) { DynamicRefreshList(response.objs, "tblExport_LDBPs"); }
            else { DynamicRefreshList([], "tblExport_LDBPs"); }

        }
    });
}




function RefreshObject_Export_LDBP() {

    var oExport_LDBP = {
        Export_LDBPID: (_oExport_LDBP != null) ? _oExport_LDBP.Export_LDBPID : 0,
        RefNo: document.getElementById("txtRefNo").value,
        BankAccountID: parseInt($("#cboBankAccount").val()),
        LetterIssueDate: $('#txtIssueDate').datebox('getValue'),
        //RequestBy: (_oExport_LDBP.RequestBy != null) ? _oExport_LDBP.RequestBy : 0,
        //ApprovedBy: (_oExport_LDBP.ApprovedBy != null) ? _oExport_LDBP.ApprovedBy : 0,
        CurrencyType: $("#chkForeign").is(":checked"),
        Note: document.getElementById("txtNote").value,
        Export_LDBPDetails: $('#tblExport_LDBPDetail').datagrid('getRows')
    };
    return oExport_LDBP;
}

function NewoExport_LDBP() {
    var oExport_LDBP = {
        Export_LDBPID: 0,
        RefNo: "",
        BankAccountID: 0,
        LetterIssueDate: new Date(),
        CurrencyType: 0,
        Note: "",
        LetterIssueDateInSt: icsdateformat(new Date()),
        Export_LDBPDetails:[]
     
    };
    return oExport_LDBP;
}


function GetExport_LDBPDetail(oExport_LDBPDetail) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExport_LDBPDetail,
        ControllerName: "Export_LDBP",
        ActionName: "GetExport_LDBPDetail",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {

        if (response.status && response.obj != null) {
            if (response.obj.Export_LDBPDetailID > 0) { RefreshControl_Export_LDBPDetail(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}


function RefreshControl_Export_LDBPDetail(oExport_LDBPDetail) {
    _oExport_LDBPDetail = oExport_LDBPDetail;


    if (_oExport_LDBPDetail.LDBPDatest == '--' || _oExport_LDBPDetail.LDBPDatest == '') {
        $('#txtLDBPDate').datebox('setValue', icsdateformat(new Date()));
    }
    else {
        $('#txtLDBPDate').datebox('setValue', _oExport_LDBPDetail.LDBPDatest);
    }
    

    document.getElementById("txtBankName").value = _oExport_LDBPDetail.BankName_Issue;
    document.getElementById("txtBankAccountName").value = _oExport_LDBPDetail.AccountNo;
    document.getElementById("txtLDBCNo").value = _oExport_LDBPDetail.LDBCNo;
    document.getElementById("txtLDBPNo").value = _oExport_LDBPDetail.LDBPNo;
    debugger;
    if (_oExport_LDBP.CurrencyType == true) {
        document.getElementById("txtDiscountType").value = "Foreign";
    }
    else
    {
        document.getElementById("txtDiscountType").value = "Local";
    }
    $('#txtLDBPAmount').numberbox('setValue', _oExport_LDBPDetail.LDBPAmount);
    $('#txtCCRate').numberbox('setValue', _oExport_LDBPDetail.CCRate);
    $('#cboCurrency').val(_oExport_LDBPDetail.CurrencyID);

    if (_oExport_LDBPDetail.Export_LDBPDetailID>0) {
        $("#cboBankAccount_LDBPNo").val(_oExport_LDBPDetail.BankAccountIDRecd);
    }
    else {

        $("#cboBankAccount_LDBPNo").val(_oExport_LDBP.BankAccountID);
    }

}

function LoadBankBranchAccounts() {
    debugger;
    var nBankBranchID = parseInt($("#cboBankBranch_Nego").val());
    if (parseInt(nBankBranchID) > 0) {
        var oBankAccount = {
            BankBranchID: parseInt(nBankBranchID)
        };
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oBankAccount,
            ControllerName: "BankAccount",
            ActionName: "GetsByBranchAndAccount",
            IsWinClose: false
        };
        $.icsDataGets(obj, function (response) {
            if (response.status && response.objs.length > 0) {
                $("#cboBankAccount").empty();
                $("#cboBankAccount").icsLoadCombo({
                    List: response.objs,
                    OptionValue: "BankAccountID",
                    DisplayText: "AccountNo"
                    //InitialValue: "Customize"
                });

            }
            else {
                $("#cboBankAccount").empty();
            }

        });
    }
}
