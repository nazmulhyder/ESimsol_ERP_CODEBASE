var _nContractorIDAdvSearch = 0;

function InitializeExportPIReportEvents() {
  

    //$("#cboTextileUnit").icsLoadCombo({
    //    List: _oTextileUnits,
    //    OptionValue: "id",
    //    DisplayText: "Value"
    //});

    $("#cboDateSearch").icsLoadCombo({
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

    $("#btnCloseAdvSearch").click(function () {
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

    //Search 
    $("#btnSearch").click(function () {

        debugger;
        /* Issue Date*/
        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dPIDate = $('#txtPIDate').datebox('getValue');
        var dPIDateEnd = $('#txtPIDateEnd').datebox('getValue');

        var dFromValidityDate = "";
        var dToValidityDate = "";
        var nCboLCDate = 0;
        var dFromLCDate = "";
        var dToLCDate = "";
        var nCboPIBank = "";
        var nCboMkPerson = "";
        var sCurrentStatus = "";

        //Ratin
        var chkResult = CheckEmpty();

        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = _nContractorIDAdvSearch + "~" + "" + "~" + nDateSearch + "~" + dPIDate + "~" + dPIDateEnd + "~"
          + 0 + "~" + dFromValidityDate + "~" + dToValidityDate + "~" + nCboLCDate + "~" + dFromLCDate + "~" + dToLCDate
          + "~" + nCboPIBank + "~" + nCboMkPerson + "~" + sCurrentStatus + "~" + parseInt($("#cboPIType").val());

        var oExportPIReport = {
            ErrorMessage: sParams
        }


        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportPIReport/AdvSearch",
            traditional: true,
            data: JSON.stringify(oExportPIReport),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oExportPIReports = jQuery.parseJSON(data);
                if (oExportPIReports.length > 0) {
                    DynamicRefreshList(oExportPIReports, "tblExportPIReports");
                }
                else {
                    alert("No List Found.");
                    DynamicRefreshList([], "tblExportPIReports");
                }
            }
        });
    });

    $("#btnPrint").click(function () {

        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dPIDate = $('#txtPIDate').datebox('getValue');
        var dPIDateEnd = $('#txtPIDateEnd').datebox('getValue');

        var dFromValidityDate = "";
        var dToValidityDate = "";
        var nCboLCDate = 0;
        var dFromLCDate = "";
        var dToLCDate = "";
        var nCboPIBank = "";
        var nCboMkPerson = "";
        var sCurrentStatus = "";

        var chkResult = CheckEmpty();
        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = _nContractorIDAdvSearch + "~" + "" + "~" + nDateSearch + "~" + dPIDate + "~" + dPIDateEnd + "~"
          + 0 + "~" + dFromValidityDate + "~" + dToValidityDate + "~" + nCboLCDate + "~" + dFromLCDate + "~" + dToLCDate
          + "~" + nCboPIBank + "~" + nCboMkPerson + "~" + sCurrentStatus + "~" + parseInt($("#cboPIType").val());

        window.open(_sBaseAddress + '/ExportPIReport/Print_ExportPIReport?sTempString=' + sParams, "_blank");
    });
    $("#btnPrintXL").click(function () {


        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dPIDate = $('#txtPIDate').datebox('getValue');
        var dPIDateEnd = $('#txtPIDateEnd').datebox('getValue');

        var dFromValidityDate = "";
        var dToValidityDate = "";
        var nCboLCDate = 0;
        var dFromLCDate = "";
        var dToLCDate = "";
        var nCboPIBank = "";
        var nCboMkPerson = "";
        var sCurrentStatus = "";

        var chkResult = CheckEmpty();
        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = _nContractorIDAdvSearch + "~" + "" + "~" + nDateSearch + "~" + dPIDate + "~" + dPIDateEnd + "~"
          + 0 + "~" + dFromValidityDate + "~" + dToValidityDate + "~" + nCboLCDate + "~" + dFromLCDate + "~" + dToLCDate
          + "~" + nCboPIBank + "~" + nCboMkPerson + "~" + sCurrentStatus + "~" + parseInt($("#cboPIType").val());

        window.open(_sBaseAddress + '/ExportPIReport/Print_PIReportXL?sTempString=' + sParams, "_blank");
    });
    $("#btnBankWisePrint").click(function () {


        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dPIDate = $('#txtPIDate').datebox('getValue');
        var dPIDateEnd = $('#txtPIDateEnd').datebox('getValue');

        var dFromValidityDate = "";
        var dToValidityDate = "";
        var nCboLCDate = 0;
        var dFromLCDate = "";
        var dToLCDate = "";
        var nCboPIBank = "";
        var nCboMkPerson = "";
        var sCurrentStatus = "";

        var chkResult = CheckEmpty();
        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = _nContractorIDAdvSearch + "~" + "" + "~" + nDateSearch + "~" + dPIDate + "~" + dPIDateEnd + "~"
          + 0 + "~" + dFromValidityDate + "~" + dToValidityDate + "~" + nCboLCDate + "~" + dFromLCDate + "~" + dToLCDate
          + "~" + nCboPIBank + "~" + nCboMkPerson + "~" + sCurrentStatus + "~" + parseInt($("#cboPIType").val());

        window.open(_sBaseAddress + '/ExportPIReport/Print_BankWisePIReport?sTempString=' + sParams, "_blank");
    });
    $("#btnMKTPersonWisePrint").click(function () {


        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dPIDate = $('#txtPIDate').datebox('getValue');
        var dPIDateEnd = $('#txtPIDateEnd').datebox('getValue');

        var dFromValidityDate = "";
        var dToValidityDate = "";
        var nCboLCDate = 0;
        var dFromLCDate = "";
        var dToLCDate = "";
        var nCboPIBank = "";
        var nCboMkPerson = "";
        var sCurrentStatus = "";

        var chkResult = CheckEmpty();
        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = _nContractorIDAdvSearch + "~" + "" + "~" + nDateSearch + "~" + dPIDate + "~" + dPIDateEnd + "~"
          + 0 + "~" + dFromValidityDate + "~" + dToValidityDate + "~" + nCboLCDate + "~" + dFromLCDate + "~" + dToLCDate
          + "~" + nCboPIBank + "~" + nCboMkPerson + "~" + sCurrentStatus + "~" + parseInt($("#cboPIType").val());

        window.open(_sBaseAddress + '/ExportPIReport/Print_MKTPersonWisePIReport?sTempString=' + sParams, "_blank");
    });
    $("#btnPartyWisePrint").click(function () {

        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dPIDate = $('#txtPIDate').datebox('getValue');
        var dPIDateEnd = $('#txtPIDateEnd').datebox('getValue');

        var dFromValidityDate = "";
        var dToValidityDate = "";
        var nCboLCDate = 0;
        var dFromLCDate = "";
        var dToLCDate = "";
        var nCboPIBank = "";
        var nCboMkPerson = "";
        var sCurrentStatus = "";

        var chkResult = CheckEmpty();
        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = _nContractorIDAdvSearch + "~" + "" + "~" + nDateSearch + "~" + dPIDate + "~" + dPIDateEnd + "~"
          + 0 + "~" + dFromValidityDate + "~" + dToValidityDate + "~" + nCboLCDate + "~" + dFromLCDate + "~" + dToLCDate
          + "~" + nCboPIBank + "~" + nCboMkPerson + "~" + sCurrentStatus + "~" + parseInt($("#cboPIType").val());

        window.open(_sBaseAddress + '/ExportPIReport/Print_PartyWisePIReport?sTempString=' + sParams, "_blank");
    });
}


function DateActions_PIDate() {

    var nDateOptionVal = $("#cboDateSearch").val();
    if (parseInt(nDateOptionVal) == 0) {
        $("#txtPIDate").datebox({ disabled: true });
        $("#txtPIDate").datebox("setValue", icsdateformat(new Date()));
        $("#txtPIDateEnd").datebox({ disabled: true });
        $("#txtPIDateEnd").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) >= 1 && parseInt(nDateOptionVal) <= 4) {
        $("#txtPIDate").datebox({ disabled: false });
        $("#txtPIDate").datebox("setValue", icsdateformat(new Date()));
        $("#txtPIDateEnd").datebox({ disabled: true });
        $("#txtPIDateEnd").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) == 5 || parseInt(nDateOptionVal) == 6) {
        $("#txtPIDate").datebox({ disabled: false });
        $("#txtPIDate").datebox("setValue", icsdateformat(new Date()));
        $("#txtPIDateEnd").datebox({ disabled: false });
        $("#txtPIDateEnd").datebox("setValue", icsdateformat(new Date()));
    }
}


function PickAccountOfPIAdvSearch() {
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

function CheckEmpty() {

    var scboDateSearch = document.getElementById("cboDateSearch");
    var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
   

    if (nDateSearch == 0)
    {
        if (_nContractorIDAdvSearch <= 0) {
            if (parseInt($("#cboPIType").val()) == 0)
            {
               return false;
            }
        }
        return true;
    }

    return true;
}

