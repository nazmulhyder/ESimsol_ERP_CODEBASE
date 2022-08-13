var _nBuyerIDAdvSearch = 0;
var _oBuyersAdvSearch = [];
function InitializeAdvSearchSUDeliveryChallanEvents() {
    $(".lblLoadingMessage").hide();
    ChangeDODates();
    ChangeChallanDates();
    LoadComboboxesAdvSearchSUDeliveryChallan();
    BuyerPickerAdvSearch();

    $("#btnResetAdvSearch").click(function () {
        ResetAdvSearchWindow();
    });

    $("#btnSearchAdvSearch").click(function () {
        var sDONo = $("#txtDONoAdvSearch").val();
        var nBuyerID = parseInt(_nBuyerIDAdvSearch);
        var sPINo = $("#txtPINoAdvSearch").val();
        var nCboMkPerson = parseInt($("#cboMKTPersonAdvSearch").val());
        var nCboDOType = parseInt($("#cboDOTypeAdvSearch").val());

        var bChkDODate = $("#chkDODate").is(':checked');
        var dFromDODate = $('#txtFromDODateAdvSearch').datebox('getValue');
        var dToDODate = $('#txtToDODateAdvSearch').datebox('getValue');

        var bChkDeliveryDate = $("#chkDeliveryDate").is(':checked');
        var dFromChallanDate = $('#txtFromChallanDateAdvSearch').datebox('getValue');
        var dToChallanDate = $('#txtToChallanDateAdvSearch').datebox('getValue');

        var sLCNo = $("#txtLCNoAdvSearch").val();

        if (bChkDODate) {
            if (new Date(dFromDODate) > new Date(dToDODate)) {
                alert("Start date must be greater than end date.");
                return false;
            }
        }

        if (bChkDeliveryDate) {
            if (new Date(dFromChallanDate) > new Date(dToChallanDate)) {
                alert("Start date must be greater than end date.");
                return false;
            }
        }

        $(".lblLoadingMessage").show();

        var sParams = sDONo + "~" +
                      nBuyerID + "~" +
                      sPINo + "~" +
                      nCboMkPerson + "~" +
                      nCboDOType + "~" +
                      bChkDODate + "~" +
                      dFromDODate + "~" +
                      dToDODate + "~" +
                      bChkDeliveryDate + "~" +
                      dFromChallanDate + "~" +
                      dToChallanDate + "~" +
                      sLCNo;

        var oSUDeliveryChallan = {
            Remarks: sParams
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryChallan/AdvSearch",
            traditional: true,
            data: JSON.stringify(oSUDeliveryChallan),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUDeliveryChallans = jQuery.parseJSON(data);
                $(".lblLoadingMessage").hide();
                if (oSUDeliveryChallans != null) {
                    if (oSUDeliveryChallans.length > 0) {
                        DynamicRefreshList(oSUDeliveryChallans, "tblSUDeliveryChallanAdvSearch");
                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSUDeliveryChallanAdvSearch");
                    }
                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblSUDeliveryChallanAdvSearch");
                }
            }
        });
    });

    $("#btnCloseAdvSearch").click(function () {
        $("#winAdvSearchSUDeliveryChallan").icsWindow("close");
    });

    $("#btnOkAdvSearch").click(function () {
        var oSUDCs = $("#tblSUDeliveryChallanAdvSearch").datagrid("getChecked");
        if (oSUDCs.length == 0) {
            alert("Please select atleast one item.");
            return false;
        }

        _oSUDeliveryChallans = oSUDCs;

        oSUDCs = TotalQtyInMainInterface(oSUDCs);
        DynamicRefreshList(oSUDCs, "tblSUDeliveryChallans");
        $("#winAdvSearchSUDeliveryChallan").icsWindow("close");
    });
}

function ResetAdvSearchWindow() {
    $("#winAdvSearchSUDeliveryChallan input").not("input[type='button']").val("");
    $("#winAdvSearchSUDeliveryChallan input").removeClass("fontColorOfPickItem");
    $("#winAdvSearchSUDeliveryChallan select").val(0);
    $('#chkDODate').prop('checked', false);
    $('#chkDeliveryDate').prop('checked', false);
    $("#txtFromDODateAdvSearch,#txtToDODateAdvSearch,#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox({ disabled: true });
    $("#txtFromDODateAdvSearch,#txtToDODateAdvSearch,#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    DynamicRefreshList([],"tblSUDeliveryChallanAdvSearch");
}

function LoadComboboxesAdvSearchSUDeliveryChallan() {
    $("#cboMKTPersonAdvSearch").icsLoadCombo({
        List: _oMktPersons,
        OptionValue: "EmployeeID",
        DisplayText: "Name"
    });

    $("#cboDOTypeAdvSearch").icsLoadCombo({
        List: _oDOTypes,
        OptionValue: "id",
        DisplayText: "Value"
    });
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
            PickBuyerSUDeliveryChallanAdvSearch();
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
        PickBuyerSUDeliveryChallanAdvSearch();
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
function PickBuyerSUDeliveryChallanAdvSearch() {
    var oContractor = {
        Params: "2" + '~' + $.trim($("#txtSearchByBuyerSUDeliveryChallan").val())
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

function ChangeDODates() {
    var bChkDODate = $("#chkDODate").is(':checked');
    if (bChkDODate) {
        $("#txtFromDODateAdvSearch,#txtToDODateAdvSearch").datebox({ disabled: false });
        $("#txtFromDODateAdvSearch,#txtToDODateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    } else {
        $("#txtFromDODateAdvSearch,#txtToDODateAdvSearch").datebox({ disabled: true });
        $("#txtFromDODateAdvSearch,#txtToDODateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    }
}

function ChangeChallanDates() {
    var bChkDeliveryDate = $("#chkDeliveryDate").is(':checked');
    if (bChkDeliveryDate) {
        $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox({ disabled: false });
        $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    } else {
        $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox({ disabled: true });
        $("#txtFromChallanDateAdvSearch,#txtToChallanDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    }
}