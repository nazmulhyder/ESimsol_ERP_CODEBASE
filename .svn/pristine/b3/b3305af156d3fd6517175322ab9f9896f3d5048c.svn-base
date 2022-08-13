var _nBuyerIDAdvSearch = 0;
var _oBuyersAdvSearch = [];
function InitializeAdvSearchSUReturnChallanEvents() {
    ChangeChallanDates();
    ChangeReturnDates();
    
    BuyerPickerAdvSearch();

    $("#btnResetAdvSearch").click(function () {
        ResetAdvSearchWindow();
    });

    $("#btnSearchAdvSearch").click(function () {
        var sChallanNo = $("#txtChallanNoAdvSearch").val();
        var nBuyerID = parseInt(_nBuyerIDAdvSearch);
        
        var bChkChallanDate = $("#chkChallanDate").is(':checked');
        var dFromChallanDate = $('#txtFromChallanDateAdvSearch').datebox('getValue');
        var dToChallanDate = $('#txtToChallanDateAdvSearch').datebox('getValue');

        var bChkReturnDate = $("#chkReturnDate").is(':checked');
        var dFromReturnDate = $('#txtFromReturnDateAdvSearch').datebox('getValue');
        var dToReturnDate = $('#txtToReturnDateAdvSearch').datebox('getValue');

        if (bChkReturnDate) {
            if (dFromChallanDate > dToChallanDate) {
                alert("From DO Date must greater than To DO Date.");
                return false;
            }
        }

        if (bChkReturnDate) {
            if (dFromReturnDate > dToReturnDate) {
                alert("From Challan Date must greater than To Challan Date.");
                return false;
            }
        }

        var sParams = sChallanNo + "~" + nBuyerID + "~" + bChkChallanDate + "~" + dFromChallanDate + "~" + dToChallanDate + "~" + bChkReturnDate + "~" + dFromReturnDate + "~" + dToReturnDate;

        var oSUReturnChallan = {
            ErrorMessage: sParams
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUReturnChallan/AdvSearch",
            traditional: true,
            data: JSON.stringify(oSUReturnChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUReturnChallans = jQuery.parseJSON(data);
                if (oSUReturnChallans != null) {
                    if (oSUReturnChallans.length > 0) {
                        DynamicRefreshList(oSUReturnChallans, "tblSUReturnChallanAdvSearch");
                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSUReturnChallanAdvSearch");
                    }
                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblSUReturnChallanAdvSearch");
                }
            }
        });
    });

    $("#btnCloseAdvSearch").click(function () {
        ResetAdvSearchWindow();
        $("#winAdvSearchSUReturnChallan").icsWindow("close");
    });

    $("#btnOkAdvSearch").click(function () {
        var oSURCs = $("#tblSUReturnChallanAdvSearch").datagrid("getChecked");
        if (oSURCs.length == 0) {
            alert("Please select atleast one item.");
            return false;
        }
        DynamicRefreshList(oSURCs, "tblSURCs");
        $("#winAdvSearchSUReturnChallan").icsWindow("close");
        _oSUReturnChallans = oSURCs;
    });
}

function ResetAdvSearchWindow() {
    $("#winAdvSearchSUReturnChallan input").not("input[type='button']").val("");
    $("#winAdvSearchSUReturnChallan input").removeClass("fontColorOfPickItem");
    $("#winAdvSearchSUReturnChallan select").val(0);
    $('#chkChallanDate').prop('checked', false);
    $('#chkReturnDate').prop('checked', false);
    $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch,#txtFromReturnDateAdvSearch,#txtToReturnDateAdvSearch").datebox({ disabled: true });
    $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch,#txtFromReturnDateAdvSearch,#txtToReturnDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    DynamicRefreshList([], "tblSUReturnChallanAdvSearch");
}



function BuyerPickerAdvSearch() {
    $("#winBuyerPickerAdvSearch").on("keydown", function (e) {
        var oContractor = $('#tblBuyersPickerAdvSearch').datagrid('getSelected');
        var nIndex = $('#tblBuyersPickerAdvSearch').datagrid('getRowIndex', oContractor);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblBuyersPickerAdvSearch').datagrid('selectRow', 0);
            }
            else {
                $('#tblBuyersPickerAdvSearch').datagrid('selectRow', nIndex - 1);
            }
            $('#txtSearchByBuyersAdvSearch').blur();
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblBuyersPickerAdvSearch').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblBuyersPickerAdvSearch').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblBuyersPickerAdvSearch').datagrid('selectRow', nIndex + 1);
            }
            $('#txtSearchByBuyersAdvSearch').blur();
        }
        if (e.which == 13)//enter=13
        {
            var oContractor = $("#tblBuyersPickerAdvSearch").icsGetSelectedItem();
            if (oContractor != null && oContractor.ContractorID > 0) {
                $('#txtSearchByBuyersAdvSearch').val(oContractor.Name);
                $("#txtSearchByBuyersAdvSearch").addClass("fontColorOfPickItem");
                _nBuyerIDAdvSearch = oContractor.ContractorID;
                $("#txtPINoAdvSearch").focus();
            }
        }
    });

    $("#txtSearchByBuyersAdvSearch").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            if ($.trim($("#txtSearchByBuyersAdvSearch").val()).length === 0) {
                alert("Please enter some text.");
                return false;
            }
            PickBuyerSUReturnChallanAdvSearch();
        }
        else if (e.keyCode === 08) {
            $("#txtSearchByBuyersAdvSearch").removeClass("fontColorOfPickItem");
            _nBuyerIDAdvSearch = 0;
        }
    });

    $("#btnClrSearchByBuyersAdvSearch").click(function () {
        $("#txtSearchByBuyersAdvSearch").removeClass("fontColorOfPickItem");
        $("#txtSearchByBuyersAdvSearch").val("");
        _nBuyerIDAdvSearch = 0;
    });

    $("#btnPickSearchByBuyersAdvSearch").click(function () {
        PickBuyerSUReturnChallanAdvSearch();
    });

    $("#btnOkBuyerPickerAdvSearch").click(function () {
        var oContractor = $("#tblBuyersPickerAdvSearch").icsGetSelectedItem();
        if (oContractor != null && oContractor.ContractorID > 0) {
            $('#txtSearchByBuyersAdvSearch').val(oContractor.Name);
            $("#txtSearchByBuyersAdvSearch").addClass("fontColorOfPickItem");
            _nBuyerIDAdvSearch = oContractor.ContractorID;
        }
    });

    $("#txtSearchByBuyerNamePickerAdvSearch").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oBuyersAdvSearch,
            TableId: "tblBuyersPickerAdvSearch"
        };
        $('#txtSearchByBuyerNamePickerAdvSearch').icsSearchByText(obj);
    });

    $("#btnCloseBuyerPickerAdvSearch").click(function () {
        $("#winBuyerPickerAdvSearch").icsWindow("close");
        _oBuyersAdvSearch = [];
        $("#winBuyerPickerAdvSearch input").val("");
        DynamicRefreshList([], "tblBuyersPickerAdvSearch");
    });
}
function PickBuyerSUReturnChallanAdvSearch() {
    var oContractor = {
        Params: "2" + '~' + $.trim($("#txtSearchByBuyerSUReturnChallan").val())
    };

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByNameType",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblContractorsPickerAdvSearch");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                _oBuyersAdvSearch = [];
                _oBuyersAdvSearch = response.objs;
                DynamicRefreshList(response.objs, "tblBuyersPickerAdvSearch");
                $("#winBuyerPickerAdvSearch").icsWindow('open', "Contractor Search");
                $("#divBuyerPickerAdvSearch").focus();
                $("#winBuyerPickerAdvSearch input").val("");
            }
            else {
                DynamicRefreshList([], "tblBuyersPickerAdvSearch");
                alert(response.objs[0].ErrorMessage);
                $("#txtSearchByBuyersAdvSearch").removeClass("fontColorOfPickItem");
                _nBuyerIDAdvSearch = 0;
            }
        }
        else {
            alert("Sorry, No data found, Try again.");
            $("#txtSearchByBuyersAdvSearch").removeClass("fontColorOfPickItem");
            _nBuyerIDAdvSearch = 0;
        }
    });
}

function ChangeChallanDates() {
    var bChkChallanDate = $("#chkChallanDate").is(':checked');
    if (bChkChallanDate) {
        $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox({ disabled: false });
        $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    } else {
        $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox({ disabled: true });
        $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    }
}

function ChangeReturnDates() {
    var bChkReturnDate = $("#chkReturnDate").is(':checked');
    if (bChkReturnDate) {
        $("#txtFromReturnDateAdvSearch,#txtToReturnDateAdvSearch").datebox({ disabled: false });
        $("#txtFromReturnDateAdvSearch,#txtToReturnDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    } else {
        $("#txtFromReturnDateAdvSearch,#txtToReturnDateAdvSearch").datebox({ disabled: true });
        $("#txtFromReturnDateAdvSearch,#txtToReturnDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    }
}