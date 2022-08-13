var _nContractorIDAdvSearch = 0;
var _nIssueBankAdvSearch = 0;

function LoadExportLCEvents_AdvSearch() {
    LoadStatusTableAdvSearch();

    $(".lblLoadingMessage").hide();
    $("#winAdvSearchExportLC").find(".easyui-datebox").datebox({ disabled: true });
    $("#winAdvSearchExportLC").find(".easyui-datebox").datebox("setValue", icsdateformat(new Date()));

    LoadComboboxesAdvSearchExportLC();

    $("#btnResetAdvSearch").click(function() {
        ResetAdvSearch();
    });

    $("#btnSearchAdvSearch").click(function () {
        $(".lblLoadingMessage").show();
        var sStatusIds = "";
        var oLCStatus = $("#tblExportLCStatusAdvSearch").datagrid("getChecked");
        if (oLCStatus.length > 0)
        {
            for (var i = 0;i<oLCStatus.length; i++){
                sStatusIds = oLCStatus[i].id + "," + sStatusIds;
            }
            sStatusIds = sStatusIds.substring(0, sStatusIds.length - 1);
        }

        DynamicRefreshList([], "tblExportLCAdvSearch");
        var sTempString = $.trim($("#txtExportLCNoAS").val()) + '~'
                        + $.trim($("#txtFileNoAS").val()) + '~'
                        + _nContractorIDAdvSearch + '~'
                        + $("#cboBankBranch_NegoAS").val() + '~'
                        + $("#cboBankBranch_AdviseAS").val() + '~'
                        + _nIssueBankAdvSearch + '~'
                        + $("#chkLCOpenDate").is(':checked') + '~'
                        + $("#txtOpeningDateStart").datebox("getValue") + '~'
                        + $("#txtOpeningDateEnd").datebox("getValue") + '~'
                        + $("#chkReceiveDate").is(':checked') + '~'
                        + $("#txtReceiveStartDate").datebox("getValue") + '~'
                        + $("#txtReceiveEndDate").datebox("getValue") + '~'
                        + $("#chkShipmentDate").is(':checked') + '~'
                        + $("#txtShipmentStartDate").datebox("getValue") + '~'
                        + $("#txtShipmentEndDate").datebox("getValue") + '~'
                        + $("#chkExpireDate").is(':checked') + '~'
                        + $("#txtExpireStartDate").datebox("getValue") + '~'
                        + $("#txtExpireEndDate").datebox("getValue") + "~"
                        + $("#chkGetOriginalCopy").is(':checked') + "~"
                        + $("#chkExportDoIsntCreateYet").is(':checked') + "~"
                        + $("#chkExportPIIsntCreateYet").is(':checked') + "~"
                        + _nBUID + "~"
                        + sStatusIds + "~"
                        + $("#chkDeliveryChallanIssueButBillNotCreated").is(':checked') + "~"

        var oExportLC = {
            ErrorMessage: sTempString
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportLC/AdvanchSearch",
            traditional: true,
            data: JSON.stringify(oExportLC),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                var oExportLCs = data;
                if (oExportLCs.length > 0) {
                    DynamicRefreshList(oExportLCs, "tblExportLCAdvSearch");
                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblExportLCAdvSearch");
                }
                $(".lblLoadingMessage").hide();
            }
        });
    });

    $("#btnCloseAdvSearch").click(function() {
        $("#winAdvSearchExportLC").icsWindow("close");
    });

    $("#btnOkAdvSearch").click(function () {
        var oExportLCs = $("#tblExportLCAdvSearch").datagrid('getChecked');
        if (oExportLCs.length > 0) {
            $("#winAdvSearchExportLC").icsWindow("close");
            DynamicRefreshList(oExportLCs, "tblExportLCs");
        }
        else {
            alert("Please select minimum one item from list.");
            return false;
        }
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
    $("#btnPickSearchByBuyerAdvSearch").click(function () {
        PickBankBranch_ELCAdvSearch();
    });

    DateFieldsAdvSearch();
}

function LoadStatusTableAdvSearch() {
    var oLCStatus = [];
    for (var i = 1; i < _oLCStatus.length; i++) {
        oLCStatus.push(_oLCStatus[i]);
    }
    DynamicRefreshListForMultipleSelection(oLCStatus, "tblExportLCStatusAdvSearch");
}

function DateFieldsAdvSearch() {
    $("#chkLCOpenDate").change(function () {
        if (this.checked) {
            $("#txtOpeningDateStart,#txtOpeningDateEnd").datebox({ disabled: false });
        } else {
            $("#txtOpeningDateStart,#txtOpeningDateEnd").datebox({ disabled: true });
        }
        $("#txtOpeningDateStart,#txtOpeningDateEnd").datebox("setValue", icsdateformat(new Date()));
    });

    $("#chkReceiveDate").change(function () {
        if (this.checked) {
            $("#txtReceiveStartDate,#txtReceiveEndDate").datebox({ disabled: false });
        } else {
            $("#txtReceiveStartDate,#txtReceiveEndDate").datebox({ disabled: true });
        }
        $("#txtReceiveStartDate,#txtReceiveEndDate").datebox("setValue", icsdateformat(new Date()));
    });

    $("#chkShipmentDate").change(function () {
        if (this.checked) {
            $("#txtShipmentStartDate,#txtShipmentEndDate").datebox({ disabled: false });
        } else {
            $("#txtShipmentStartDate,#txtShipmentEndDate").datebox({ disabled: true });
        }
        $("#txtShipmentStartDate,#txtShipmentEndDate").datebox("setValue", icsdateformat(new Date()));
    });

    $("#chkExpireDate").change(function () {
        if (this.checked) {
            $("#txtExpireStartDate,#txtExpireEndDate").datebox({ disabled: false });
        } else {
            $("#txtExpireStartDate,#txtExpireEndDate").datebox({ disabled: true });
        }
        $("#txtExpireStartDate,#txtExpireEndDate").datebox("setValue", icsdateformat(new Date()));
    });
}

function PickBankBranch_ELCAdvSearch()
{
    function PickBankBranch_ELC() {
        var oBankBranch = {
            BankName: $.trim($("#btnPickSearchByBuyerAdvSearch").val())
        };
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oBankBranch,
            ControllerName: "BankBranch",
            ActionName: "GetsBankBranchSearchByBankName",
            IsWinClose: false
        };
        DynamicRefreshList([], "tblBankBranchsPicker");
        $.icsDataGets(obj, function (response) {
            if (response.status && response.objs.length > 0) {
                if (response.objs[0].BankBranchID > 0) {
                    var tblColums = []; var oColumn = { field: "BankName", title: "Bank Name", width: 200, align: "left" }; tblColums.push(oColumn);
                    oColumn = { field: "BranchName", title: "Branch Name", width: 120, align: "left" }; tblColums.push(oColumn);
                    var oPickerParam = {
                        winid: 'winBankBranch',
                        winclass: 'clsBankBranch',
                        winwidth: 400,
                        winheight: 460,
                        tableid: 'tblBankBranch',
                        tablecolumns: tblColums,
                        datalist: response.objs,
                        multiplereturn: false,
                        searchingbyfieldName: 'BankName',
                        windowTittle: 'Bank Branches'
                    };
                    $.icsPicker(oPickerParam);
                    IntializePickerbutton1(oPickerParam);
                }
                else {
                    alert(response.objs[0].ErrorMessage);
                }
            }
            else {
                alert("Sorry, No Bank Found.");
            }
        });
    }
}

function IntializePickerbutton1(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        SetPickerValueAssign1(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which === 13) {
            SetPickerValueAssign1(oPickerobj);
        }
    });
}

function SetPickerValueAssign1(oPickerobj) {
    var oreturnObj = null, oreturnobjs = [];
    if (oPickerobj.multiplereturn) {
        oreturnobjs = $('#' + oPickerobj.tableid).datagrid('getChecked');
    } else {
        oreturnObj = $('#' + oPickerobj.tableid).datagrid('getSelected');
    }

    if (oPickerobj.winid == 'winBankBranch') {
        if (oreturnObj != null && oreturnObj.BankBranchID > 0) {
            var oBankBranch = oreturnObj;
            $('#btnPickSearchByBuyerAdvSearch').val(oBankBranch.BankBranchName);
            $("#btnPickSearchByBuyerAdvSearch").addClass("fontColorOfPickItem");
            $("#btnPickSearchByBuyerAdvSearch").focus();
            //ratin
            _nIssueBankAdvSearch = oBankBranch.BankBranchID;
        } else {
            alert("Please select a Bank Branch.");
            return false;
        }
    }
    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
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

function LoadComboboxesAdvSearchExportLC() {
    $("#cboBankBranch_NegoAS, #cboBankBranch_AdviseAS").icsLoadCombo({
        List: _oBankBranchs,
        OptionValue: "BankBranchID",
        DisplayText: "BankBranchName"
    });

  
}

function ResetAdvSearch()
{
    _nContractorIDAdvSearch = 0;
    _nIssueBankAdvSearch = 0;
    $('#chkLCOpenDate,#chkReceiveDate,#chkShipmentDate,#chkExpireDate,#chkGetOriginalCopy,#chkExportDoIsntCreateYet,#chkDeliveryChallanIssueButBillNotCreated,#chkExportPIIsntCreateYet').prop('checked', false);
    $("#txtExportLCNoAS,#txtFileNoAS,#txtSearchByAccountOfAdvSearch,#txtSearchByBuyerAdvSearch").val("");
    $("#winAdvSearchExportLC select").val(0);
    $("#txtSearchByAccountOfAdvSearch,#txtSearchByBuyerAdvSearch").removeClass("fontColorOfPickItem");
    $("#winAdvSearchExportLC").find(".easyui-datebox").datebox({ disabled: true });
    $("#winAdvSearchExportLC").find(".easyui-datebox").datebox("setValue", "");
    DynamicRefreshList([], "tblExportLCAdvSearch");
    $(".lblLoadingMessage").hide();
    LoadStatusTableAdvSearch();
}
