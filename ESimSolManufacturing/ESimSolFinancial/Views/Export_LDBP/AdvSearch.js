var _oAccountsOf = 0;
var _nContractorIDAdvSearch = 0;

function LoadExport_LDBPEvents_AdvSearch() {

    debugger;
 

    $("#cboBankBranch_ADS").icsLoadCombo({
        List: _oBankBranchs_Nego,
        OptionValue: "BankBranchID",
        DisplayText: "BankBranchName"
    });

    DynamicRefreshList(_oBankAccounts, "tblBankAccounts");

  


    $("#cboLetterIssueDate").icsLoadCombo({
        List: _oCompareOperators,
        OptionValue: "Value",
        DisplayText: "Text"
    });

    $("#btnAdvSearch").click(function () {
        $("#winAdvSearchExport_LDBP").icsWindow("open", "Advance Search");
        DynamicRefreshList([], "tblExport_LDBPAdvSearch");
        //DateActions();
        ResetAdvSearch();
        //UnselectAllRowsOfATable();
    });

    $("#btnResetAdvSearch").click(function () {
        ResetAdvSearch();
    });
    function ResetAdvSearch()
    {
        $('#cboBankBranch_ADS').val(0);
        $('#cboLetterIssueDate').val(0);
    }

    $("#btnSearchAdvSearch").click(function () {

        debugger;
        var oBankAccounts = $('#tblBankAccounts').datagrid('getSelections');
        var sBankAccounts = "";
        if (oBankAccounts.length > 0) {
            sBankAccounts = MakeStringIDs(oBankAccounts);
        }
        var oExport_LDBP = {
            AccountNo: sBankAccounts,
            BankBranchID: parseInt($("#cboBankBranch_ADS").val()),
            SelectedOption: parseInt($("#cboBankBranch_ADS").val()),
            LetterIssueDate: $('#txtLetterIssueDateStart').datebox('getValue'),
            LetterIssueDate_end: $('#txtLetterIssueDateEnd').datebox('getValue'),
            BUID:_buid
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/Export_LDBP/GetsSearchedData",
            traditional: true,
            data: JSON.stringify(oExport_LDBP),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oExport_LDBPs = jQuery.parseJSON(data);
                if (oExport_LDBPs != null) {
                    //DynamicRefreshList(oExport_LDBPs, "tblExport_LDBPAdvSearch");
                    DynamicRefreshList(oExport_LDBPs, "tblExport_LDBPs");
                    $("#winAdvSearchExport_LDBP").icsWindow("close");
                }
            }
        });
    });


    $("#btnCloseAdvSearch").click(function () {
        $("#winAdvSearchExport_LDBP").icsWindow("close");
    });


    $("#btnOkAdvSearch").click(function () {
        var oExport_LDBPs = $("#tblExport_LDBPAdvSearch").datagrid('getChecked');
        if (oExport_LDBPs.length > 0) {
            $("#winAdvSearchExport_LDBP").icsWindow("close");
            DynamicRefreshList(oExport_LDBPs, "tblExport_LDBPs");
        }
        else {
            alert("Please select minimum one item from list.");
            return false;
        }
    });

    WindowIndexController("tblExport_LDBPs");
}

function DateActions() {

    var nDateOptionVal = $("#cboLetterIssueDate").val();
    if (parseInt(nDateOptionVal) == 0) {
        $("#txtLetterIssueDateStart").datebox({ disabled: true });
        $("#txtLetterIssueDateStart").datebox("setValue", icsdateformat(new Date()));
        $("#txtLetterIssueDateEnd").datebox({ disabled: true });
        $("#txtLetterIssueDateEnd").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) >= 1 && parseInt(nDateOptionVal) <= 4) {
        $("#txtLetterIssueDateStart").datebox({ disabled: false });
        $("#txtLetterIssueDateStart").datebox("setValue", icsdateformat(new Date()));
        $("#txtLetterIssueDateEnd").datebox({ disabled: true });
        $("#txtLetterIssueDateEnd").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) == 5 || parseInt(nDateOptionVal) == 6) {
        $("#txtLetterIssueDateStart").datebox({ disabled: false });
        $("#txtLetterIssueDateStart").datebox("setValue", icsdateformat(new Date()));
        $("#txtLetterIssueDateEnd").datebox({ disabled: false });
        $("#txtLetterIssueDateEnd").datebox("setValue", icsdateformat(new Date()));
    }

  



}


function MakeStringIDs(StringIDs) {
    debugger;
    var IDs = "";
    for (var i = 0; i < StringIDs.length; i++) {
        IDs = StringIDs[i].BankAccountID + "," + IDs;
    }
    var length = 0;
    length = parseInt(IDs.length) - 1;
    IDs = IDs.substring(0, parseInt(length));
    return IDs;
}
