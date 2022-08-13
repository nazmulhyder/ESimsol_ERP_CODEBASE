var _oBuyerIdAdvSearch = 0;
var _oProductIdAdvSearch = 0;
var _sMAccountIds = "";
var _sMAGroupIds = "";
function InitializeAdvSearchFabricEvents() {

    $("#btnAdvSearchFabric").click(function () {
        $("#winAdvSearchFabric").icsWindow("open", "Advance Search");
        $("#txtBuyerReferenceFabricAdvSearch").focus();
        _sMAccountIds = "";
        _oBuyerIdAdvSearch = "";
        //DynamicRefreshList([], "tblFabricIDAdvSearch");
        DateActions();
        ResetAdvSearch();
        AllComboLoadsAdvSearchFabric();
        UnselectAllRowsOfATable();
    });

    $("#btnResetAdvSearch").click(function () {
        $(".lblLoadingMessage").hide();
        ResetAdvSearch();
    });


    $("#btnContractorAdv").click(function () {

        var sContractorName = $.trim($("#txtContractorNameAdv").val());
        /// 2 Buyer
        var oContractor = {
            Params: '2' + '~' + sContractorName,
        };
        GetContractorsAdv(oContractor);
    });

    $("#txtContractorNameAdv").keydown(function (e) {
        var nkeyCode = e.keyCode || e.which;
        if (nkeyCode == 13) {
            var sContractorName = $.trim($("#txtContractorNameAdv").val());

            if ($('#txtContractorNameAdv').val() == null || $('#txtContractorNameAdv').val() == "") {
                alert("Please Type Name or part Name and Press Enter.");
                $('#txtContractorNameAdv').focus();
                $("#txtContractorNameAdv").addClass("errorFieldBorder");
                return;
            }

            var oContractor = {
                Params: '2' + '~' + sContractorName,
            };
            GetContractorsAdv(oContractor);
        }
        else if (nkeyCode == 8) {
            $("#txtContractorNameAdv").val("");
            _oBuyerIdAdvSearch = "";

        }
    });

    $("#btnResetMktAccountdv").click(function () {

        $("#txtMktAccountAdv").val("");
        _sMAccountIds = "";
    });
    $("#btnMktAccountAdv").click(function () {
        debugger;
        var sName = $.trim($("#txtMktAccountAdv").val());
        GetMktPersons(sName);
    });
    $("#txtMktAccountAdv").keydown(function (e) {
        var nkeyCode = e.keyCode || e.which;
        if (nkeyCode == 13) {
            var sName = $.trim($("#txtMktAccountAdv").val());
            if ($('#txtMktAccountAdv').val() == null || $('#txtMktAccountAdv').val() == "") {
                alert("Please Type Name or part Name and Press Enter.");
                $('#txtMktAccountAdv').focus();
                return;
            }
            GetMktPersons(sName);
        }
        else if (nkeyCode == 8) {
            $("#txtMktAccount").val("");
            _sMAccountIds = "";
        }
    });

    $("#btnResetMktGroup").click(function () {

        $("#txtMktGroup").val("");
        _sMAGroupIds = "";
    });
    $("#btnPickMktGroup").click(function () {
        debugger;
        var sName = $.trim($("#txtMktGroup").val());
        GetMktGroups(sName);
    });
    $("#txtMktGroup").keydown(function (e) {
        var nkeyCode = e.keyCode || e.which;
        if (nkeyCode == 13) {
            var sName = $.trim($("#txtMktGroup").val());
            if ($('#txtMktGroup').val() == null || $('#txtMktGroup').val() == "") {
                alert("Please Type Name or part Name and Press Enter.");
                $('#txtMktGroup').focus();
                return;
            }
            GetMktGroups(sName);
        }
        else if (nkeyCode == 8) {
            $("#txtMktGroup").val("");
            _sMAGroupIds = "";
        }
    });
    function GetMktGroups(sName) {

        var oMarketingAccount_BU = {
            Name: sName,
            BUID: sessionStorage.getItem("BUID")
        };
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oMarketingAccount_BU,
            ControllerName: "MarketingAccount",
            ActionName: "GetsAllGroup",
            IsWinClose: false
        };

        $.icsDataGets(obj, function (response) {

            if (response.status && response.objs.length > 0) {
                if (response.objs[0].MarketingAccountID > 0) {
                    debugger;
                    var tblColums = [];
                    var oColumn = { field: "MarketingAccountID", title: "Code", width: 50, align: "left" }; tblColums.push(oColumn);
                    oColumn = { field: "Name", title: "Name", width: 180, align: "left" }; tblColums.push(oColumn);
                    oColumn = { field: "GroupName", title: "GroupName", width: 120, align: "left" }; tblColums.push(oColumn);
                    oColumn = { field: "Phone", title: "Phone", width: 100, align: "left" }; tblColums.push(oColumn);

                    var oPickerParam = {
                        winid: 'winMktGroup',
                        winclass: 'clsMktGroupPicker',
                        winwidth: 460,
                        winheight: 460,
                        tableid: 'tblMktGroupicker',
                        tablecolumns: tblColums,
                        datalist: response.objs,
                        multiplereturn: true,
                        searchingbyfieldName: 'Name',
                        windowTittle: 'MKT Group List'
                    };
                    $.icsPicker(oPickerParam);
                    IntializePickerbutton(oPickerParam);//multiplereturn, winclassName
                }
                else { alert(response.objs[0].ErrorMessage); }
            }
            else {
                alert("No marketing person found.");
            }
        });
    }

    $("#chkIssueDateAdvSearch").change(function () {
        if (this.checked) {
            $("#txtFromDateIssueDateAdvSearch,#txtToDateIssueDateAdvSearch").datebox({ disabled: false });
            $("#txtFromDateIssueDateAdvSearch,#txtToDateIssueDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        } else {
            $("#txtFromDateIssueDateAdvSearch,#txtToDateIssueDateAdvSearch").datebox({ disabled: true });
            $("#txtFromDateIssueDateAdvSearch,#txtToDateIssueDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        }
    });

    $("#chkLabDipRequestAdvSearch").change(function () {
        if (this.checked) {
            $("#txtFromDateLabDipRequestAdvSearch,#txtToDateLabDipRequestAdvSearch").datebox({ disabled: false });
            $("#txtFromDateLabDipRequestAdvSearch,#txtToDateLabDipRequestAdvSearch").datebox("setValue", icsdateformat(new Date()));
        } else {
            $("#txtFromDateLabDipRequestAdvSearch,#txtToDateLabDipRequestAdvSearch").datebox({ disabled: true });
            $("#txtFromDateLabDipRequestAdvSearch,#txtToDateLabDipRequestAdvSearch").datebox("setValue", icsdateformat(new Date()));
        }
    });

    $("#chkFabricPatternAdvSearch").change(function () {
        if (this.checked) {
            $("#txtFromDateLabFabricPatternAdvSearch,#txtToDateFabricPatternAdvSearch").datebox({ disabled: false });
            $("#txtFromDateLabFabricPatternAdvSearch,#txtToDateFabricPatternAdvSearch").datebox("setValue", icsdateformat(new Date()));
        } else {
            $("#txtFromDateLabFabricPatternAdvSearch,#txtToDateFabricPatternAdvSearch").datebox({ disabled: true });
            $("#txtFromDateLabFabricPatternAdvSearch,#txtToDateFabricPatternAdvSearch").datebox("setValue", icsdateformat(new Date()));
        }
    });

    $("#chkApproveDateAdvSearch").change(function () {
        if (this.checked) {
            $("#txtFromDateApproveDateAdvSearch,#txtToDateApproveDateAdvSearch").datebox({ disabled: false });
            $("#txtFromDateApproveDateAdvSearch,#txtToDateApproveDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        } else {
            $("#txtFromDateApproveDateAdvSearch,#txtToDateApproveDateAdvSearch").datebox({ disabled: true });
            $("#txtFromDateApproveDateAdvSearch,#txtToDateApproveDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        }
    });

    $("#chkSubmissionDateAdvSearch").change(function () {
        if (this.checked) {
            $("#txtFromDateSubmissionDateAdvSearch,#txtToDateSubmissionDateAdvSearch").datebox({ disabled: false });
            $("#txtFromDateSubmissionDateAdvSearch,#txtToDateSubmissionDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        } else {
            $("#txtFromDateSubmissionDateAdvSearch,#txtToDateSubmissionDateAdvSearch").datebox({ disabled: true });
            $("#txtFromDateSubmissionDateAdvSearch,#txtToDateSubmissionDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        }
    });

    $("#chkReceiveDateAdvSearch").change(function () {
        if (this.checked) {
            $("#txtFromDateReceiveDateAdvSearch,#txtToDateReceiveDateAdvSearch").datebox({ disabled: false });
            $("#txtFromDateReceiveDateAdvSearch,#txtToDateReceiveDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        } else {
            $("#txtFromDateReceiveDateAdvSearch,#txtToDateReceiveDateAdvSearch").datebox({ disabled: true });
            $("#txtFromDateReceiveDateAdvSearch,#txtToDateReceiveDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
        }
    });

    $("#txtConstructionFabric").keydown(function (e) {
        var nkeyCode = e.keyCode || e.which;
        if (nkeyCode == 13) {
            $("#txtConstructionFabricPI").val($.trim($("#txtConstructionFabric").val()));
        }
    });


    $("#btnSearchAdvSearch").click(function () {
        var sFabricNo = $("#txtBuyerReferenceFabricAdvSearch").val();
        var nBuyerID = _oBuyerIdAdvSearch;
        var nProductID = 0;//parseInt(_oProductIdAdvSearch); //Composition
        var sBuyerReference = $("#txtBuyerReferenceAdvSearch").val();
        var sProductName = $("#txtCompositionAdvSearch").val();

        var nFinishTypeInInt = parseInt($("#cboFinishTypeAdvSearch").val());

        var bIsIssueDate = $("#chkIssueDateAdvSearch").is(':checked');
        var dFromIssueDate = $('#txtFromDateIssueDateAdvSearch').datebox('getValue');
        var dToIssueDate = $('#txtToDateIssueDateAdvSearch').datebox('getValue');

        var bIsLabDipReqDate = $("#chkLabDipRequestAdvSearch").is(':checked');
        var dFromLabDipReqDate = $('#txtFromDateLabDipRequestAdvSearch').datebox('getValue');
        var dToLabDipReqDate = $('#txtToDateLabDipRequestAdvSearch').datebox('getValue');

        var bIsFabricPatternDate = $("#chkFabricPatternAdvSearch").is(':checked');
        var dFromFabricPatternDate = $('#txtFromDateLabFabricPatternAdvSearch').datebox('getValue');
        var dToFabricPatternDate = $('#txtToDateFabricPatternAdvSearch').datebox('getValue');

        var bIsApproveDate = $("#chkApproveDateAdvSearch").is(':checked');
        var dFromApproveDate = $('#txtFromDateApproveDateAdvSearch').datebox('getValue');
        var dToApproveDate = $('#txtToDateApproveDateAdvSearch').datebox('getValue');

        var bIsYetToLabDip = $("#chkYetToLabDipAdvSearch").is(':checked');
        var bYetToPattern = $("#chkYetToPatternAdvSearch").is(':checked');

        var nProcessTypeInInt = parseInt($("#cboProcessTypeAdvSearch").val());
        var sConstruction = $.trim($("#txtConstructionAdvSearch").val());

        var bApprove = $("#chkApproveAdvSearch").is(':checked');
        var bUnapprove = $("#chkUnapproveAdvSearch").is(':checked');

        var bIsSubmissionDate = $("#chkSubmissionDateAdvSearch").is(':checked');
        var dFromSubmissionDate = $('#txtFromDateSubmissionDateAdvSearch').datebox('getValue');
        var dToSubmissionDate = $('#txtToDateSubmissionDateAdvSearch').datebox('getValue');

        var bIsReceiveDate = $("#chkReceiveDateAdvSearch").is(':checked');
        var dFromReceiveDate = $('#txtFromDateReceiveDateAdvSearch').datebox('getValue');
        var dToReceiveDate = $('#txtToDateReceiveDateAdvSearch').datebox('getValue');

        var nWeaveTypeInInt = parseInt($("#cboWeaveTypeAdvSearch").val());
        var nFabricDesignInInt = parseInt($("#cboFabricDesignTypeAdvSearch").val());

        var sColor = $.trim($("#txtColorAdvSearch").val());
        var sStyle = $.trim($("#txtStyleAdvSearch").val());

        var oFabric = {
            Remarks: sFabricNo + "~" +
                     nBuyerID + "~" +
                     nProductID + "~" +
                     sBuyerReference + "~" +
                     sProductName + "~" +
                     _sMAccountIds + "~" +
                     nFinishTypeInInt + "~" +
                     bIsIssueDate + "~" +
                     dFromIssueDate + "~" +
                     dToIssueDate + "~" +
                     bIsLabDipReqDate + "~" +
                     dFromLabDipReqDate + "~" +
                     dToLabDipReqDate + "~" +
                     bIsFabricPatternDate + "~" +
                     dFromFabricPatternDate + "~" +
                     dToFabricPatternDate + "~" +
                     bIsApproveDate + "~" +
                     dFromApproveDate + "~" +
                     dToApproveDate + "~" +
                     bIsYetToLabDip + "~" +
                     bYetToPattern + "~" +
                     nProcessTypeInInt + "~" +
                     sConstruction + "~" +
                     bApprove + "~" +
                     bUnapprove + "~" +
                     bIsSubmissionDate + "~" +
                     dFromSubmissionDate + "~" +
                     dToSubmissionDate + "~" +
                     bIsReceiveDate + "~" +
                     dFromReceiveDate + "~" +
                     dToReceiveDate + "~" +
                     nWeaveTypeInInt + "~" +
                     nFabricDesignInInt + "~" +
                     sColor + "~" +
                     sStyle + "~" +
                     _sMAGroupIds
        };

        sessionStorage.setItem("ParamsP", oFabric.Remarks);
        var sParams = sessionStorage.getItem("ParamsP");
        $(".lblLoadingMessage").show();
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/Fabric/AdvSearch",
            traditional: true,
            data: JSON.stringify(oFabric),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".lblLoadingMessage").hide();
                var oFabrics = data;
                if (oFabrics.length > 0) {
                    if ($.trim(oFabrics[0].ErrorMessage) == "") {
                        DynamicRefreshList(oFabrics, "tblFabrics");
                        $("#winAdvSearchFabric").icsWindow("close");
                    }
                    else {
                        alert(oFabrics[0].ErrorMessage);
                        DynamicRefreshList([], "tblFabrics");
                    }
                }
                else {
                    alert("No data found.");
                    DynamicRefreshList([], "tblFabrics");
                }
            }
        });
    });

    $("#btnCloseAdvSearchFabric").click(function () {
        $("#winAdvSearchFabric").icsWindow("close");
    });

    $("#btnResetContractorAdv").click(function () {
        $("#txtContractorNameAdv").val("");
        _oBuyerIdAdvSearch = "";
    });
}

function ResetAdvSearch() {
    debugger;
    $(".lblLoadingMessage").hide();
    $("#txtFromDateApproveDateAdvSearch,#txtToDateApproveDateAdvSearch,#txtFromDateSubmissionDateAdvSearch,#txtToDateSubmissionDateAdvSearch,#txtFromDateLabFabricPatternAdvSearch,#txtToDateFabricPatternAdvSearch,#txtFromDateLabDipRequestAdvSearch,#txtToDateLabDipRequestAdvSearch,#txtFromDateIssueDateAdvSearch,#txtToDateIssueDateAdvSearch,#txtFromDateReceiveDateAdvSearch,#txtToDateReceiveDateAdvSearch").datebox({ disabled: true });
    $("#txtFromDateApproveDateAdvSearch,#txtToDateApproveDateAdvSearch,#txtFromDateSubmissionDateAdvSearch,#txtToDateSubmissionDateAdvSearch,#txtFromDateLabFabricPatternAdvSearch,#txtToDateFabricPatternAdvSearch,#txtFromDateLabDipRequestAdvSearch,#txtToDateLabDipRequestAdvSearch,#txtFromDateIssueDateAdvSearch,#txtToDateIssueDateAdvSearch,#txtFromDateReceiveDateAdvSearch,#txtToDateReceiveDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    $("#txtBuyerReferenceFabricAdvSearch").val("");
    _oBuyerIdAdvSearch = "";
    _sMAccountIds = "";
    _sMAGroupIds = "";
    $("#txtBuyerReferenceAdvSearch").val("");
    $("#txtCompositionAdvSearch").val("");
    $("#txtMktGroup").val("");

    document.getElementById("chkIssueDateAdvSearch").checked = false;
    document.getElementById("chkLabDipRequestAdvSearch").checked = false;
    document.getElementById("chkFabricPatternAdvSearch").checked = false;
    document.getElementById("chkApproveDateAdvSearch").checked = false;
    document.getElementById("chkYetToLabDipAdvSearch").checked = false;
    document.getElementById("chkApproveAdvSearch").checked = false;
    document.getElementById("chkUnapproveAdvSearch").checked = false;
    document.getElementById("chkSubmissionDateAdvSearch").checked = false;
    document.getElementById("chkReceiveDateAdvSearch").checked = false;

    $("#txtColorAdvSearch").val("");
    $("#txtStyleAdvSearch").val("");
    $("#txtConstructionAdvSearch").val("");

    $("#txtContractorNameAdv").val("");
    $("#txtMktAccountAdv").val("");
}

function DateActions() {
    DynamicDateActions("cboDateOptionAdvSearch", "txtFromDateIssueDateAdvSearch", "txtToDateIssueDateAdvSearch");
}

function AllComboLoadsAdvSearchFabric() {

    $("#cboDateOptionAdvSearch").icsLoadCombo({
        List: _oCompareOperators,
        OptionValue: "Value",
        DisplayText: "Text"
    });

    $("#cboFinishTypeAdvSearch").icsLoadCombo({
        List: _oFinishTypes,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });


    $("#cboProcessTypeAdvSearch").icsLoadCombo({
        List: _oProcessTypes,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

    $("#cboWeaveTypeAdvSearch").icsLoadCombo({
        List: _oFabricWeaves,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

    $("#cboFabricDesignTypeAdvSearch").icsLoadCombo({
        List: _oFabricDesigns,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

}

function GetContractorsAdv(oContractor) {

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByNameType",
        IsWinClose: false
    };
    $("#progressbar").progressbar({ value: 0 });
    $("#progressbarParent").show();
    var intervalID = setInterval(updateProgress, 250);
    $.icsDataGets(obj, function (response) {
        clearInterval(intervalID);
        $("#progressbarParent").hide();
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                debugger;
                var tblColums = [];
                var oColumn = { field: "ContractorID", title: "Code", width: 50, align: "center" }; tblColums.push(oColumn);
                oColumn = { field: "Name", title: "Name", width: 190, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Phone", title: "Phone", width: 140, align: "left" }; tblColums.push(oColumn);

                var oPickerParam = {
                    winid: 'winContractorAdvPicker',
                    winclass: 'clsContractorAdvPicker',
                    winwidth: 460,
                    winheight: 460,
                    tableid: 'tblContractorAdvPicker',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: true,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Contractor List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
        else {
            alert("No contractor found.");
        }
    });


}

function GetMktPersons(sName) {

    var oMarketingAccount_BU = {
        Name: sName,
        BUID: sessionStorage.getItem("BUID")
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oMarketingAccount_BU,
        ControllerName: "MarketingAccount",
        ActionName: "MarketingAccountSearchByNameAndGroup",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {

        if (response.status && response.objs.length > 0) {
            if (response.objs[0].MarketingAccountID > 0) {
                debugger;
                var tblColums = [];
                var oColumn = { field: "MarketingAccountID", title: "Code", width: 50, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Name", title: "Name", width: 180, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "GroupName", title: "GroupName", width: 120, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Phone", title: "Phone", width: 100, align: "left" }; tblColums.push(oColumn);

                var oPickerParam = {
                    winid: 'winMktAccount',
                    winclass: 'clsMktPersonPicker',
                    winwidth: 460,
                    winheight: 460,
                    tableid: 'tblMktPersonPicker',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: true,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'MKT Person List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
        else {
            alert("No marketing person found.");
        }
    });


}

function IntializePickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        //for Single Select
        SetPickerValueAssign(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which == 13)//enter=13
        {
            SetPickerValueAssign(oPickerobj);

        }
    });
}

function SetPickerValueAssign(oPickerobj) {
    debugger;
    var oreturnObj = null, oreturnobjs = [];
    if (oPickerobj.multiplereturn) {
        oreturnobjs = $('#' + oPickerobj.tableid).datagrid('getChecked');
    } else {
        oreturnObj = $('#' + oPickerobj.tableid).datagrid('getSelected');
    }
    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
    if (oPickerobj.winid == 'winContractorPicker') {
        if (oreturnObj != null && oreturnObj.ContractorID > 0) {
            $('#txtContractorName').val(oreturnObj.Name);
            _nContractorID = oreturnObj.ContractorID;
        }
        else {
            alert("Data not found");
        }
    }
    else if (oPickerobj.winid == 'winContractorAdvPicker') {
        if (oreturnobjs != null && oreturnobjs.length > 0) {
            debugger;
            _oBuyerIdAdvSearch = ''; var sMessage = '';
            sMessage = (oreturnobjs.length > 1) ? oreturnobjs.length + " Contractors Selected" : oreturnobjs[0].Name;
            $('#txtContractorNameAdv').val(sMessage);
            $("#txtContractorNameAdv").addClass("fontColorOfPickItem");

            for (var i = 0; i < oreturnobjs.length; i++) {
                _oBuyerIdAdvSearch = _oBuyerIdAdvSearch + oreturnobjs[i].ContractorID + ',';
            }
            _oBuyerIdAdvSearch = _oBuyerIdAdvSearch.substring(0, _oBuyerIdAdvSearch.length - 1);

        }
        else {
            alert("Data Not Found.");
            return;
        }
    }
    else if (oPickerobj.winid == 'winMktAccount') {
        if (oreturnobjs != null && oreturnobjs.length > 0) {
            var sMessage = '';
            sMessage = (oreturnobjs.length > 1) ? oreturnobjs.length + "MKT Account(s) Selected" : oreturnobjs[0].Name;
            $('#txtMktAccountAdv').val(sMessage);
            $("#txtMktAccountAdv").addClass("fontColorOfPickItem");
            for (var i = 0; i < oreturnobjs.length; i++) {
                _sMAccountIds = _sMAccountIds + oreturnobjs[i].MarketingAccountID + ',';
            }
            _sMAccountIds = _sMAccountIds.substring(0, _sMAccountIds.length - 1);

        }
        else {
            alert("Please select a MKt Account.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winMktGroup') {
        if (oreturnobjs != null && oreturnobjs.length > 0) {
            var sMessage = '';
            _sMAGroupIds = "";
            sMessage = (oreturnobjs.length > 1) ? oreturnobjs.length + "MKT Group(s) Selected" : oreturnobjs[0].Name;
            $('#txtMktGroup').val(sMessage);
            $("#txtMktGroup").addClass("fontColorOfPickItem");
            for (var i = 0; i < oreturnobjs.length; i++) {
                _sMAGroupIds = _sMAGroupIds + oreturnobjs[i].MarketingAccountID + ',';
            }
            _sMAGroupIds = _sMAGroupIds.substring(0, _sMAGroupIds.length - 1);
        }
        else {
            alert("Please select a MKt Group.");
            return false;
        }
    }

}

