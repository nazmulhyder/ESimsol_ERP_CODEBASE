
var _nContractorIDAdvSearch = 0;

function LoadExportBillReportEvents_AdvSearch() {

    debugger;
    $("#cboBankBranch_Nego").icsLoadCombo({
        List: _oBankBranchs_Nego,
        OptionValue: "BankBranchID",
        DisplayText: "BankBranchName"
    });

    DynamicRefreshList(_oLCBillEventObj, "tblCurrenctState");
 
    $("#cboState").icsLoadCombo({
        List: _oLCBillEventObj,
        OptionValue: "id",
        DisplayText: "Value"
    });

    
    $("#cboDateOptionAdvSearch").icsLoadCombo({
        List: _oCompareOperators,
        OptionValue: "Value",
        DisplayText: "Text"
    });

    $("#cboBillDate").icsLoadCombo({
        List: _oCompareOperators,
        OptionValue: "Value",
        DisplayText: "Text"
    });
    $("#cobBillAmount").icsLoadCombo({
        List: _oCompareOperators,
        OptionValue: "Value",
        DisplayText: "Text"
    });


    //Account Of Picker

    $("#winAccountOfPickerAdvSearch").on("keydown", function (e) {
        var oContractor = $('#tblAccountsPickerAdvSearch').datagrid('getSelected');
        var nIndex = $('#tblAccountsPickerAdvSearch').datagrid('getRowIndex', oContractor);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblAccountsPickerAdvSearch').datagrid('selectRow', 0);
            }
            else {
                $('#tblAccountsPickerAdvSearch').datagrid('selectRow', nIndex - 1);
            }
            $('#txtSearchByAccountOfAdvSearch').blur();
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblAccountsPickerAdvSearch').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblAccountsPickerAdvSearch').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblAccountsPickerAdvSearch').datagrid('selectRow', nIndex + 1);
            }
            $('#txtSearchByAccountOfAdvSearch').blur();
        }
        if (e.which == 13)//enter=13
        {
            var oContractor = $("#tblAccountsPickerAdvSearch").icsGetSelectedItem();
            if (oContractor != null && oContractor.ContractorID > 0) {
                $('#txtSearchByAccountOfAdvSearch').val(oContractor.Name);
                $("#txtSearchByAccountOfAdvSearch").addClass("fontColorOfPickItem");
                _nContractorIDAdvSearch = oContractor.ContractorID;
                $("#txtSearchByBuyersAdvSearch").focus();
            }
        }
    });

    $("#txtSearchByAccountOfAdvSearch").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            PickAccountOfAdvSearch();
        }
        else if (e.keyCode === 08) {
            $("#txtSearchByAccountOfAdvSearch").removeClass("fontColorOfPickItem");
            _nContractorIDAdvSearch = 0;
        }
    });

    $("#btnClrSearchByAccountOAdvSearch").click(function () {
        $("#txtSearchByAccountOfAdvSearch").removeClass("fontColorOfPickItem");
        $("#txtSearchByAccountOfAdvSearch").val("");
        _nContractorIDAdvSearch = 0;
    });

    $("#btnPickSearchByAccountOAdvSearch").click(function () {
        PickAccountOfAdvSearch();
    });

    $("#btnOkAccountPickerAdvSearch").click(function () {
        var oContractor = $("#tblAccountsPickerAdvSearch").icsGetSelectedItem();
        if (oContractor != null && oContractor.ContractorID > 0) {
            $('#txtSearchByAccountOfAdvSearch').val(oContractor.Name);
            $("#txtSearchByAccountOfAdvSearch").addClass("fontColorOfPickItem");
            _nContractorIDAdvSearch = oContractor.ContractorID;
        }
    });

    $("#btnCloseAccountPickerAdvSearch").click(function () {
        $("#winAccountOfPickerAdvSearch").icsWindow("close");
    });

    $("#txtSearchByAccountNamePickerAdvSearch").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oAccountsOf,
            TableId: "tblAccountsPickerAdvSearch"
        };
        $('#txtSearchByAccountNamePickerAdvSearch').icsSearchByText(obj);
    });

    //End Account Of Picker

    $("#btnResetAdvSearch").click(function() {
        ResetAdvSearch();
    });

    $("#btnSearchAdvSearch").click(function () {
       
        debugger;
        var oCurrenctStates = $('#tblCurrenctState').datagrid('getSelections');
        var sCurrenctStates = "";
        if (oCurrenctStates.length > 0) {
            sCurrenctStates = MakeStringIDs(oCurrenctStates);
        }

        var _nBBranchID_Issue = 0;

        var _nApplicantIDs = "";
        if (parseInt(_nContractorIDAdvSearch) > 0) {
            _nApplicantIDs = parseInt(_nContractorIDAdvSearch);
        }
        else {
            _nApplicantIDs = "";
        }
        var oExportBillReport = {
            ApplicantName: _nApplicantIDs,
            CurrentStateIDs: sCurrenctStates,
            BankBranchID_Advice: parseInt($("#cboBankBranch_Nego").val()),
            BankBranchID_Issue: parseInt(_nBBranchID_Issue),

            SearchAmountType: parseInt($("#cobBillAmount").val()),
            FromAmount: parseFloat($("#txtFromBillAmount").val()),
            ToAmount: parseFloat($("#txtToBillAmount").val()),

            DateType: $("#cboDateCriteria").val(),
            DateSearchCriteria: parseInt($("#cboBillDate").val()),
            StartDateCritaria: new Date($('#txtBillDateStart').datebox('getValue')),
            EndDateCritaria: new Date($('#txtBillDateEnd').datebox('getValue')),

            StateDateType: parseInt($("#cboState").val()),
            DateSearchState: parseInt($("#cboDateOptionAdvSearch").val()),
            StartDateState: new Date($('#txtFromDateAdvSearch').datebox('getValue')),
            EndDateState: new Date($('#txtToDateAdvSearch').datebox('getValue'))
            
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportBillReport/AdvanceSearch",
            traditional: true,
            data: JSON.stringify(oExportBillReport),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oExportBillReports = jQuery.parseJSON(data);
                if (oExportBillReports != null) {
                    DynamicRefreshList(oExportBillReports, "tblExportBillAdvSearch");
                   
                }
            }
        });
    });

    $("#btnCloseAdvSearch").click(function () {
        $("#winAdvSearchExportBillReport").icsWindow("close");
    });
   
 

    $("#btnOkAdvSearch").click(function () {
        var oExportBillReports = $("#tblExportBillAdvSearch").datagrid('getChecked');
        if (oExportBillReports.length > 0) {
            $("#winAdvSearchExportBillReport").icsWindow("close");
            DynamicRefreshList(oExportBillReports, "tblExportBillReports");
            ColumnHide();
        }
        else {
            alert("Please select minimum one item from list.");
            return false;
        }
    });

    WindowIndexController("tblExportBillReports");
}

function DateActions() {

    var nDateOptionVal = $("#cboDateOptionAdvSearch").val();
    if (parseInt(nDateOptionVal) == 0) {
        $("#txtFromDateAdvSearch").datebox({ disabled: true });
        $("#txtFromDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        $("#txtToDateAdvSearch").datebox({ disabled: true });
        $("#txtToDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) >= 1 && parseInt(nDateOptionVal) <= 4) {
        $("#txtFromDateAdvSearch").datebox({ disabled: false });
        $("#txtFromDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        $("#txtToDateAdvSearch").datebox({ disabled: true });
        $("#txtToDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) == 5 || parseInt(nDateOptionVal) == 6) {
        $("#txtFromDateAdvSearch").datebox({ disabled: false });
        $("#txtFromDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        $("#txtToDateAdvSearch").datebox({ disabled: false });
        $("#txtToDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    }

    var nDateOptionValTwo = $("#cboBillDate").val();
    if (parseInt(nDateOptionValTwo) == 0) {
        $("#txtBillDateStart").datebox({ disabled: true });
        $("#txtBillDateStart").datebox("setValue", icsdateformat(new Date()));
        $("#txtBillDateEnd").datebox({ disabled: true });
        $("#txtBillDateEnd").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionValTwo) >= 1 && parseInt(nDateOptionValTwo) <= 4) {
        $("#txtBillDateStart").datebox({ disabled: false });
        $("#txtBillDateStart").datebox("setValue", icsdateformat(new Date()));
        $("#txtBillDateEnd").datebox({ disabled: true });
        $("#txtBillDateEnd").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionValTwo) == 5 || parseInt(nDateOptionValTwo) == 6) {
        $("#txtBillDateStart").datebox({ disabled: false });
        $("#txtBillDateStart").datebox("setValue", icsdateformat(new Date()));
        $("#txtBillDateEnd").datebox({ disabled: false });
        $("#txtBillDateEnd").datebox("setValue", icsdateformat(new Date()));
    }
  
   

}

function DateActions_Amount()
{
    var nDateOptionValTwo = $("#cobBillAmount").val();
    if (parseInt(nDateOptionValTwo) == 0) {
        $("#txtFromBillAmount").prop('disabled', true);
        $("#txtToBillAmount").prop('disabled', true);
        $("#txtFromBillAmount").val(0);
        $("#txtToBillAmount").val(0);
    }
    else if (parseInt(nDateOptionValTwo) >= 1 && parseInt(nDateOptionValTwo) <= 4) {
        $("#txtFromBillAmount").prop('disabled', false);
        $("#txtToBillAmount").prop('disabled', true);

    }
    else if (parseInt(nDateOptionValTwo) == 5 || parseInt(nDateOptionValTwo) == 6) {
        $("#txtFromBillAmount").prop('disabled', false);
        $("#txtToBillAmount").prop('disabled', false);
        $("#txtToBillAmount").val(0);

    }
}


function MakeStringIDs(StringIDs) {
    debugger;
    var IDs = "";
    for (var i = 0; i < StringIDs.length; i++) {
        IDs = StringIDs[i].id + "," + IDs;
    }
    var length = 0;
    length = parseInt(IDs.length) - 1;
    IDs = IDs.substring(0, parseInt(length));
    return IDs;
}

function ColumnHide() {
    debugger
    if (oExportBillReportColumns.length > 0) {
        var sColumns = ["ExportLCNo", "ApplicantName", "BankName_Advice", "BankName_Nego", "ExportBillNo", "AmountSt", "StateSt", "LCRecivedDateSt", "StartDateSt", "SendToPartySt", "RecdFromPartySt", "SendToBankDateSt", "RecedFromBankDateSt", "LDBCDateSt", "LDBCNo", "AcceptanceDateSt", "MaturityReceivedDateSt", "MaturityDateSt", "DiscountedDateSt", "RelizationDateSt", "BankFDDRecDateSt", "EncashmentDateSt"]
        for (var i = 0; i < sColumns.length; i++) {
            if (!IsExist(sColumns[i])) {
                $('#tblExportBillReports').datagrid('hideColumn', sColumns[i]);
            }
        }


        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag1');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag2');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag3');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag4');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag5');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag6');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag7');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag8');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag9');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag10');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag11');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag12');
        $('#tblExportBillReports').datagrid('hideColumn', 'TimeLag13');


    }
}

function IsExist(sColumnName) {
    debugger;
    for (var j = 0; j < oExportBillReportColumns.length; j++) {
        if (oExportBillReportColumns[j].FieldName == sColumnName) {
            return true;
        }
    }
    return false;
}


function PickAccountOfAdvSearch() {
    var oContractor = {
        Params: '2,3' + '~' + $.trim($("#txtSearchByAccountOfAdvSearch").val())
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByNameType",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblAccountsPickerAdvSearch");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                _oAccountsOf = []; _oAccountsOf = response.objs;
                DynamicRefreshList(response.objs, "tblAccountsPickerAdvSearch");
                $("#winAccountOfPickerAdvSearch").icsWindow('open', "Contractor Search");
                $("#divAccountPickerAdvSearch").focus();
                $("#winAccountOfPickerAdvSearch input").val("");
            }
            else {
                _oContractorsPicker = [];
                DynamicRefreshList([], "tblAccountsPickerAdvSearch");
                alert(response.objs[0].ErrorMessage);
                $("#txtSearchByAccountOfAdvSearch").removeClass("fontColorOfPickItem");
                _nContractorIDAdvSearch = 0;
            }
        }
        else {
            alert("Sorry, No data found, Try again.");
            $("#txtSearchByAccountOfAdvSearch").removeClass("fontColorOfPickItem");
            _nContractorIDAdvSearch = 0;
        }
    });
}