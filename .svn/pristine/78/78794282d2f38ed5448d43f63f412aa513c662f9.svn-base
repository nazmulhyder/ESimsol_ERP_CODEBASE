var _nContractorIDAdvSearch = 0;
var _nBuyerIDAdvSearch = 0;
function InitializeAdvSearchExportPIReportEvents() {
    $('#tblExportPIAdvSearch').datagrid({ selectOnCheck: false, checkOnSelect: true });
    //DynamicRefreshList(_oPIStatusObjs, "tblCurrentStatusAdvSearch");
   

    //$("#cboIssueDateAdvSearch, #cboValidityDateAdvSearch, #cboLCDateAdvSearch").icsLoadCombo({
    //    List: _oCompareOperators,
    //    OptionValue: "Value",
    //    DisplayText: "Text"
    //});
 
    //$("#cboPIBankAdvSearch").icsLoadCombo({
    //    List: _oBankBranchs,
    //    OptionValue: "BankBranchID",
    //    DisplayText: "BankBranchName"
    //});

    //$("#cboMkPersonAdvSearch").icsLoadCombo({
    //    List: _oMktPersons,
    //    OptionValue: "EmployeeID",
    //    DisplayText: "Name"
    //});


    
    //DateActionsIssueDateAdvSearch();
    //DateActionsValidityDateAdvSearch();
    //DateActionsLCDateAdvSearch();

    $("#tblCurrentStatusAdvSearch").datagrid("checkAll");

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

    $("#btnCloseAdvSearch").click(function() {
        $("#winAdvSearchExportPI").icsWindow("close");
    });

    $("#txtSearchByAccountOfAdvSearch").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            PickAccountOfPIAdvSearch();
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
        PickAccountOfPIAdvSearch();
    });

    $("#btnOkAccountPickerAdvSearch").click(function () {
        var oContractor = $("#tblAccountsPickerAdvSearch").icsGetSelectedItem();
        if (oContractor != null && oContractor.ContractorID > 0) {
            $('#txtSearchByAccountOfAdvSearch').val(oContractor.Name);
            $("#txtSearchByAccountOfAdvSearch").addClass("fontColorOfPickItem");
            _nContractorIDAdvSearch = oContractor.ContractorID;
        }
    });

    $("#btnCloseAccountPickerAdvSearch").click(function() {
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



  

    $("#btnResetAdvSearch").click(function () {
        ResetAdvSearchWindow();
        DateActionsIssueDateAdvSearch();
        DateActionsValidityDateAdvSearch();
        DateActionsLCDateAdvSearch();
    });

    $("#btnSearchAdvSearch").click(function () {
        var checkDate = CheckFromAndToDateValidation("cboIssueDateAdvSearch", "txtFromIssueDateAdvSearch", "txtToIssueDateAdvSearch");
        if (!checkDate) {
            alert("In Issue Date To Date must be greater then From Date");
            return false;
        }
        checkDate = CheckFromAndToDateValidation("cboValidityDateAdvSearch", "txtFromIssueDateAdvSearch", "txtToValidityDateAdvSearch");
        if (!checkDate) {
            alert("In Validity Date To Date must be greater then From Date");
            return false;
        }
        checkDate = CheckFromAndToDateValidation("cboLCDateAdvSearch", "txtFromLCDateAdvSearch", "txtToLCDateAdvSearch");
        if (!checkDate) {
            alert("In LC Date To Date must be greater then From Date");
            return false;
        }


        var nContractorID = parseInt(_nContractorIDAdvSearch);
        var nBuyerID = parseInt(_nBuyerIDAdvSearch);
        var nCboIssueDate = parseInt($("#cboIssueDateAdvSearch").val());
        var dFromIssueDate = $('#txtFromIssueDateAdvSearch').datebox('getValue');
        var dToIssueDate = $('#txtToIssueDateAdvSearch').datebox('getValue');

        var nCboValidityDate = parseInt($("#cboValidityDateAdvSearch").val());
        var dFromValidityDate = $('#txtFromValidityDateAdvSearch').datebox('getValue');
        var dToValidityDate = $('#txtToValidityDateAdvSearch').datebox('getValue');

        var nCboLCDate = parseInt($("#cboLCDateAdvSearch").val());
        var dFromLCDate = $('#txtFromLCDateAdvSearch').datebox('getValue');
        var dToLCDate = $('#txtToLCDateAdvSearch').datebox('getValue');

        var nCboPIBank = parseInt($("#cboPIBankAdvSearch").val());

        var nCboMkPerson = parseInt($("#cboMkPersonAdvSearch").val());

        var sCurrentStatus = "";
        var oPIStatus = $("#tblCurrentStatusAdvSearch").datagrid("getChecked");
        for (var i = 0; i < oPIStatus.length; i++) {
            sCurrentStatus = oPIStatus[i].id + "," +sCurrentStatus;
        }
        sCurrentStatus = sCurrentStatus.substring(0, sCurrentStatus.length - 1);


        var sParams = nContractorID + "~" + nBuyerID + "~" + nCboIssueDate + "~" + dFromIssueDate + "~" + dToIssueDate + "~"
            + nCboValidityDate + "~" + dFromValidityDate + "~" + dToValidityDate + "~" + nCboLCDate + "~" + dFromLCDate + "~" + dToLCDate
            + "~" + nCboPIBank + "~" + nCboMkPerson + "~" + sCurrentStatus;

        var oExportPI = {
            Note : sParams
        };


        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportPI/AdvSearch",
            traditional: true,
            data: JSON.stringify(oExportPI),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oExportPIs = jQuery.parseJSON(data);
                if (oExportPIs != null) {
                    if (oExportPIs.length > 0) {
                        DynamicRefreshList(oExportPIs, "tblExportPIAdvSearch");
                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblExportPIAdvSearch");
                    }

                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblExportPIAdvSearch");
                }
            }
        });
    });

    $("#btnOkAdvSearch").click(function() {
        var oExportPIs = $("#tblExportPIAdvSearch").datagrid('getChecked');
        if (oExportPIs.length > 0) {
            $("#winAdvSearchExportPI").icsWindow("close");
            DynamicRefreshList(oExportPIs, "tblExportPIs");
        }
        else {
            alert("Please select minimum one item from list.");
            return false;
        }
    });

}
