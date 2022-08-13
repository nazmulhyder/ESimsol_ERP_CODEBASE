var _nBuyerIDAdvSearch = 0;
var _oBuyersAdvSearch = [];
var _oDOStatus = [];
var _nExportPIIDAdv = 0;
var _nExportLCIDAdv = 0;
function InitializeAdvSearchSUDeliveryOrderEvents() {
    debugger;
    $(".lblLoadingMessage").hide();
    ChangeDODates();
    ChangeDeliveryDates();
    LoadComboboxesAdvSearchSUDeliveryOrder();
    ResetAdvSearchWindow();

    DynamicRefreshList(_oDOStatus, "tblStatusAdvSearch");

    $("#btnResetAdvSearch").click(function () {
        ResetAdvSearchWindow();
    });

    $("#btnSearchAdvSearch").click(function () {
        $(".lblLoadingMessage").show();
        DynamicRefreshList([], "tblSUDeliveryOrderAdvSearch");
        var sDONo = $("#txtDONoAdvSearch").val();
        var nCboMkPerson = parseInt($("#cboMKTPersonAdvSearch").val());
        var nCboDOType = parseInt($("#cboDOTypeAdvSearch").val());

        var bChkDODate = $("#chkDODate").is(':checked');
        var dFromDODate = $('#txtFromDODateAdvSearch').datebox('getValue');
        var dToDODate = $('#txtToDODateAdvSearch').datebox('getValue');

        var bChkDeliveryDate = $("#chkDeliveryDate").is(':checked');
        var dFromDeliveryDate = $('#txtFromDeliveryDateAdvSearch').datebox('getValue');
        var dToDeliveryDate = $('#txtToDeliveryDateAdvSearch').datebox('getValue');

        if (bChkDODate) {
            if (dFromDODate > dToDODate) {
                alert("From DO Date must greater than To DO Date.");
                return false;
            }
        }

        if (bChkDeliveryDate) {
            if (dFromDeliveryDate > dToDeliveryDate) {
                alert("From Delivery Date must greater than To Delivery Date.");
                return false;
            }
        }

        
        var sCheckedStatus = "";
        var oCheckedStatus = $("#tblStatusAdvSearch").datagrid("getChecked");
        for (var i = 0; i < oCheckedStatus.length; i++)
        {
            sCheckedStatus = oCheckedStatus[i].id + "," + sCheckedStatus;
        }
        sCheckedStatus = sCheckedStatus.substring(0, sCheckedStatus.length - 1);

        var sParams = sDONo + "~"
                    + _nBuyerIDAdvSearch + "~"
                    + _nExportPIIDAdv + "~"
                    + nCboMkPerson + "~"
                    + nCboDOType + "~"
                    + bChkDODate + "~"
                    + dFromDODate + "~"
                    + dToDODate + "~"
                    + bChkDeliveryDate + "~"
                    + dFromDeliveryDate + "~"
                    + dToDeliveryDate + "~"
                    + sCheckedStatus + "~"
                    + _nExportLCIDAdv;

        var oSUDeliveryOrder = {
            Remarks: sParams
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUDeliveryOrder/AdvSearch",
            traditional: true,
            data: JSON.stringify(oSUDeliveryOrder),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                
                var oSUDeliveryOrders = jQuery.parseJSON(data);
                if (oSUDeliveryOrders != null) {
                    if (oSUDeliveryOrders.length > 0) {
                        DynamicRefreshList(oSUDeliveryOrders, "tblSUDeliveryOrderAdvSearch");
                    } else {
                        $(".lblLoadingMessage").hide();
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSUDeliveryOrderAdvSearch");
                    }
                } else {
                    $(".lblLoadingMessage").hide();
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblSUDeliveryOrderAdvSearch");
                }
                $(".lblLoadingMessage").hide();
            }
        });
    });

    $("#btnCloseAdvSearch").click(function () {
        $("#winAdvSearchSUDeliveryOrder").icsWindow("close");
    });

    $("#btnOkAdvSearch").click(function () {
        debugger;
        var oSUDOs = $("#tblSUDeliveryOrderAdvSearch").datagrid("getChecked");
        if (oSUDOs.length == 0) {
            alert("Please select atleast one item.");
            return false;
        }
        DynamicRefreshList(oSUDOs, "tblSUDeliveryOrders"); //For Adv Search in SU Delivery Order
        DynamicRefreshList(oSUDOs, "tblSUDOs"); //For Adv Search in SU Delivery Program
        $("#winAdvSearchSUDeliveryOrder").icsWindow("close");
        _oSUDeliveryOrders = oSUDOs;
    });

    $("#txtPINoAdvSearch").keydown(function (e) {
        if (e.keyCode === 13)
        {
            ExportPIPickerAdv();
        }
        else if (e.keyCode === 8)
        {
            ClearPIInfoAdv();
        }
    });

    $("#btnClrPINoAdvSearch").click(function () {
        ClearPIInfoAdv();
    });

    $("#btnPickPINoAdvSearch").click(function () {
        ExportPIPickerAdv()
    });

    $("#txtLCNoAdvSearch").keydown(function (e) {
        if (e.keyCode === 13) {
            ExportLCPickerAdv();
        }
        else if (e.keyCode === 8) {
            ClearLCInfoAdv();
        }
    });

    $("#btnClrLCNoAdvSearch").click(function () {
        ClearLCInfoAdv();
    });

    $("#btnPickLCNoAdvSearch").click(function () {
        ExportLCPickerAdv()
    });

    $("#txtSearchByBuyersAdvSearch").keydown(function (e) {
        if (e.keyCode === 13) {
            BuyerPickerAdv();
        }
        else if (e.keyCode === 8) {
            ClearBuyerInfoAdv();
        }
    });

    $("#btnClrBuyerAdvSearch").click(function () {
        ClearBuyerInfoAdv();
    });

    $("#btnPickBuyerAdvSearch").click(function () {
        BuyerPickerAdv()
    });


}

function BuyerPickerAdv() {
    $(".lblLoadingMessage").show();
    var oContractor = {
        Params: "2" + '~' + $.trim($("#txtSearchByBuyersAdvSearch").val())
    }

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByNameType",
        IsWinClose: false
    }
    $.icsDataGets(obj, function (response) {
        $(".lblLoadingMessage").hide();
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                var tblColums = []; var oColumn = { field: "Name", title: "Name", width: 200, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ContractorTypeInString", title: "Type", width: 100, align: "left" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winContractors',
                    winclass: 'clsContractor',
                    winwidth: 400,
                    winheight: 460,
                    tableid: 'tblContractors',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Contractor List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbuttonAdv(oPickerParam);
            }
            else {
                alert(response.objs[0].ErrorMessage);
            }
        }
        else {
            alert("Sorry, No Contractor Found.");
        }
    });
}

function ExportPIPickerAdv() {
    $(".lblLoadingMessage").show();
    var oExportPI = {
        Params: $.trim($("#txtPINoAdvSearch").val()) + "~" + 0
    }

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oExportPI,
        ControllerName: "ExportPI",
        ActionName: "GetsPI",
        IsWinClose: false
    }
    $.icsDataGets(obj, function (response) {
        $(".lblLoadingMessage").hide();
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ExportPIID > 0) {
                var tblColums = []; var oColumn = { field: "PINo", title: "PI No", width: 200, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "PaymentTypeSt", title: "Payment Type", width: 100, align: "left" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winExportPIs',
                    winclass: 'clsExportPI',
                    winwidth: 400,
                    winheight: 460,
                    tableid: 'tblExportPIs',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'PINo',
                    windowTittle: 'Export PI List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbuttonAdv(oPickerParam);
            }
            else {
                alert(response.objs[0].ErrorMessage);
            }
        }
        else {
            alert("Sorry, No Export PI Found.");
        }
    });
}

function ExportLCPickerAdv() {
    $(".lblLoadingMessage").show();

    var oExportLC = {
        ExportLCNo: $.trim($("#txtLCNoAdvSearch").val())
    }

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oExportLC,
        ControllerName: "ExportLC",
        ActionName: "GetbyLCNoMax",
        IsWinClose: false
    }
    $.icsMaxDataGets(obj, function (response) {
        $(".lblLoadingMessage").hide();
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ExportLCID > 0) {
                var tblColums = []; var oColumn = { field: "ExportLCNo", title: "LC No", width: 200, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "OpeningDateST", title: "Opening Date", width: 100, align: "center" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winExportLCs',
                    winclass: 'clsExportLC',
                    winwidth: 400,
                    winheight: 460,
                    tableid: 'tblExportLCs',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'ExportLCNo',
                    windowTittle: 'Export LC List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbuttonAdv(oPickerParam);
            }
            else {
                alert(response.objs[0].ErrorMessage);
            }
        }
        else {
            alert("Sorry, No Export LC Found.");
        }
    });
}

function IntializePickerbuttonAdv(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        SetPickerValueAssignAdv(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which === 13) {
            SetPickerValueAssignAdv(oPickerobj);
        }
    });
}

function SetPickerValueAssignAdv(oPickerobj) {
    var oreturnObj = null, oreturnobjs = [];
    
    if (oPickerobj.multiplereturn) {
        oreturnObjs = $('#' + oPickerobj.tableid).datagrid('getChecked');
    } else {
        oreturnObj = $('#' + oPickerobj.tableid).datagrid('getSelected');
    }

    if (oPickerobj.winid == 'winExportPIs') {
        if (oreturnObj != null && oreturnObj.ExportPIID > 0) {
            var oExportPI = oreturnObj;
            _nExportPIIDAdv = oExportPI.ExportPIID;
            $("#txtPINoAdvSearch").val(oExportPI.PINo);
            $("#txtPINoAdvSearch").addClass("fontColorOfPickItem");
            $("#txtLCNoAdvSearch").focus();
        }
        else {
            alert("Please select an item from list.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winExportLCs') {
        if (oreturnObj != null && oreturnObj.ExportLCID > 0) {
            var oExportLC = oreturnObj;
            _nExportLCIDAdv = oExportLC.ExportLCID;
            $("#txtLCNoAdvSearch").val(oExportLC.ExportLCNo);
            $("#txtLCNoAdvSearch").addClass("fontColorOfPickItem");
            $("#cboMKTPersonAdvSearch").focus();
        }
        else {
            alert("Please select an item from list.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winContractors') {
        if (oreturnObj != null && oreturnObj.ContractorID > 0) {
            var oContractor = oreturnObj;
            _nBuyerIDAdvSearch = oContractor.ContractorID;
            $("#txtSearchByBuyersAdvSearch").val(oContractor.Name);
            $("#txtSearchByBuyersAdvSearch").addClass("fontColorOfPickItem");
            $("#txtPINoAdvSearch").focus();
        }
        else {
            alert("Please select an item from list.");
            return false;
        }
    }
    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
}

function ClearPIInfoAdv()
{
    _nExportPIIdAdv = 0;
    $("#txtPINoAdvSearch").val("");
    $("#txtPINoAdvSearch").removeClass("fontColorOfPickItem");
}

function ClearLCInfoAdv() {
    _nExportLCIdAdv = 0;
    $("#txtLCNoAdvSearch").val("");
    $("#txtLCNoAdvSearch").removeClass("fontColorOfPickItem");
}

function ClearBuyerInfoAdv() {
    _nBuyerIDAdvSearch = 0;
    $("#txtSearchByBuyersAdvSearch").val("");
    $("#txtSearchByBuyersAdvSearch").removeClass("fontColorOfPickItem");
}

function ResetAdvSearchWindow() {
    $("#winAdvSearchSUDeliveryOrder input").not("input[type='button']").val("");
    $("#winAdvSearchSUDeliveryOrder input").removeClass("fontColorOfPickItem");
    $("#winAdvSearchSUDeliveryOrder select").val(0);
    $('#chkDODate').prop('checked', false);
    $('#chkDeliveryDate').prop('checked', false);
    $("#txtFromDODateAdvSearch,#txtToDODateAdvSearch,#txtFromDeliveryDateAdvSearch,#txtToDeliveryDateAdvSearch").datebox({ disabled: true });
    $("#txtFromDODateAdvSearch,#txtToDODateAdvSearch,#txtFromDeliveryDateAdvSearch,#txtToDeliveryDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    _nBuyerIDAdvSearch = 0;
    DynamicRefreshList([], "tblSUDeliveryOrderAdvSearch");
    DynamicRefreshList([], "tblStatusAdvSearch");
    DynamicRefreshList(_oDOStatus, "tblStatusAdvSearch");
    ClearPIInfoAdv();
    ClearLCInfoAdv();
    ClearBuyerInfoAdv();
}

function LoadComboboxesAdvSearchSUDeliveryOrder() {
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

function ChangeDeliveryDates() {
    var bChkDeliveryDate = $("#chkDeliveryDate").is(':checked');
    if (bChkDeliveryDate) {
        $("#txtFromDeliveryDateAdvSearch,#txtToDeliveryDateAdvSearch").datebox({ disabled: false });
        $("#txtFromDeliveryDateAdvSearch,#txtToDeliveryDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    } else {
        $("#txtFromDeliveryDateAdvSearch,#txtToDeliveryDateAdvSearch").datebox({ disabled: true });
        $("#txtFromDeliveryDateAdvSearch,#txtToDeliveryDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    }
}