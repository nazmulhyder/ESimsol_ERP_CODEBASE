var _oLCInHands = [];
var _oPendingPartyacceptances = [];
var _oPendingSubmitToBanks = [];
var _oPendingLDBCs = [];
var _oPendingMaturitys = [];
var _oPendingOverdues = [];
var _oPendingPayments = [];

function InitializeExportBillPendingReportsEvents() {

    //Start LC In Hand
    $("#txtContractorLCInHand").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "ApplicantName",
            GlobalObjectList: _oLCInHands,
            TableId: "tblLCInHand"
        });
    });

    $("#txtBankBranchLCInHand").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "BankName_Nego",
            GlobalObjectList: _oLCInHands,
            TableId: "tblLCInHand"
        });
    });

    $("#txtMktPersionLCInHand").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "MKTPName",
            GlobalObjectList: _oLCInHands,
            TableId: "tblLCInHand"
        });
    });

    $("#btnSrc_LCInHand").click(function() {
        var oExportBillPendingReport =
        {
            ExportBillID: 0,
            ReportType: 1,

        };
        Gets_LCInHand(oExportBillPendingReport);
    });

    $("#btnPrintBankWiseLCInHand").click(function () {
        PrintBankWise(1,0);
    });

    $("#btnPrintPartyWiseLCInHand").click(function () {
        PrintPartyWise(1,0);
    });

    $("#btnPrintXLLCInHand").click(function () {
        PrintXL(1,0);
    });
    //End LC In Hand


    //Start Pending Party acceptance
    $("#txtContractorPendingPartyacceptance").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "ApplicantName",
            GlobalObjectList: _oPendingPartyacceptances,
            TableId: "tblPendingPartyacceptance"
        });
    });

    $("#txtBankBranchPendingPartyacceptance").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "BankName_Nego",
            GlobalObjectList: _oPendingPartyacceptances,
            TableId: "tblPendingPartyacceptance"
        });
    });

    $("#txtMktPersionPendingPartyacceptance").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "MKTPName",
            GlobalObjectList: _oPendingPartyacceptances,
            TableId: "tblPendingPartyacceptance"
        });
    });

    $("#btnSrc_PendingPartyacceptance").click(function() {
        var oExportBillPendingReport =
        {
            ExportBillID: 0,
            ReportType: 3
        };
        Gets_PendingPartyacceptance(oExportBillPendingReport);
    });

    $("#btnPrintBankWisePendingPartyacceptance").click(function () {
        PrintBankWise(3,0);
    });

    $("#btnPrintPartyWisePendingPartyacceptance").click(function () {
        PrintPartyWise(3,0);
    });

    $("#btnPrintXLPendingPartyacceptance").click(function () {
        PrintXL(3, 0);
    });
    //End Pending Party acceptance


    //Start Pending Submit To Bank
    $("#txtContractorPendingSubmitToBank").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "ApplicantName",
            GlobalObjectList: _oPendingSubmitToBanks,
            TableId: "tblPendingSubmitToBank"
        });
    });

    $("#txtBankBranchPendingSubmitToBank").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "BankName_Nego",
            GlobalObjectList: _oPendingSubmitToBanks,
            TableId: "tblPendingSubmitToBank"
        });
    });

    $("#txtMktPersionPendingSubmitToBank").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "MKTPName",
            GlobalObjectList: _oPendingSubmitToBanks,
            TableId: "tblPendingSubmitToBank"
        });
    });

    $("#btnSrc_PendingSubmitToBank").click(function () {
        var oExportBillPendingReport =
        {
            ExportBillID: 0,
            ReportType: 4
        };
        Gets_PendingSubmitToBank(oExportBillPendingReport);
    });

    $("#btnPrintBankWisePendingSubmitToBank").click(function () {
        PrintBankWise(4,0);
    });

    $("#btnPrintPartyWisePendingSubmitToBank").click(function () {
        PrintPartyWise(4, 0);
    });

    $("#btnPrintXLPendingSubmitToBank").click(function () {
        PrintXL(4, 0);
    });
    //End Pending Submit To Bank



    //Start Pending LDBC
    $("#txtContractorPendingLDBC").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "ApplicantName",
            GlobalObjectList: _oPendingLDBCs,
            TableId: "tblPendingLDBC"
        });
    });

    $("#txtBankBranchPendingLDBC").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "BankName_Nego",
            GlobalObjectList: _oPendingLDBCs,
            TableId: "tblPendingLDBC"
        });
    });

    $("#txtMktPersionPendingLDBC").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "MKTPName",
            GlobalObjectList: _oPendingLDBCs,
            TableId: "tblPendingLDBC"
        });
    });

    $("#btnSrc_PendingLDBC").click(function () {
        var oExportBillPendingReport =
        {
            ExportBillID: 0,
            ReportType: 5
        };
        Gets_PendingLDBC(oExportBillPendingReport);
    });

    $("#btnPrintBankWisePendingLDBC").click(function () {
        PrintBankWise(5,0);
    });

    $("#btnPrintPartyWisePendingLDBC").click(function () {
        PrintPartyWise(5, 0);
    });

    $("#btnPrintXLPendingLDBC").click(function () {
        PrintXL(5, 0);
    });
    //End Pending LDBC



    //Start Pending Maturity
    $("#txtContractorPendingMaturity").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "ApplicantName",
            GlobalObjectList: _oPendingMaturitys,
            TableId: "tblPendingMaturity"
        });
    });

    $("#txtBankBranchPendingMaturity").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "BankName_Nego",
            GlobalObjectList: _oPendingMaturitys,
            TableId: "tblPendingMaturity"
        });
    });

    $("#txtMktPersionPendingMaturity").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "MKTPName",
            GlobalObjectList: _oPendingMaturitys,
            TableId: "tblPendingMaturity"
        });
    });

    $("#btnSrc_PendingMaturity").click(function () {
        var oExportBillPendingReport =
        {
            ExportBillID: 0,
            ReportType: 6
        };
        Gets_PendingMaturity(oExportBillPendingReport);
    });

    $("#btnPrintBankWisePendingMaturity").click(function () {
        PrintBankWise(6,0);
    });

    $("#btnPrintPartyWisePendingMaturity").click(function () {
        PrintPartyWise(6, 0);
    });

    $("#btnPrintXLPendingMaturity").click(function () {
        PrintXL(6, 0);
    });
    //End Pending Maturity


    //Start Over due Payment
    $("#txtContractorPendingOverdue").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "ApplicantName",
            GlobalObjectList: _oPendingOverdues,
            TableId: "tblPendingOverdue"
        });
    });

    $("#txtBankBranchPendingOverdue").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "BankName_Nego",
            GlobalObjectList: _oPendingOverdues,
            TableId: "tblPendingOverdue"
        });
    });

    $("#txtMktPersionPendingOverdue").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "MKTPName",
            GlobalObjectList: _oPendingOverdues,
            TableId: "tblPendingOverdue"
        });
    });

    $("#btnSrc_OverduePayment").click(function () {
        var oExportBillPendingReport =
        {
            ExportBillID: 0,
            ReportType: 7,
            DiscountType: parseInt($("#cboDiscountType").val())
        };
        Gets_PendingOverdue(oExportBillPendingReport);
    });

    $("#btnPrintBankWisePendingOverdue").click(function () {
        var nDiscountType = parseInt($("#cboDiscountType").val());
        PrintBankWise(7, nDiscountType);
    });

    $("#btnPrintPartyWisePendingOverdue").click(function () {
        var nDiscountType = parseInt($("#cboDiscountType").val());
        PrintPartyWise(7, nDiscountType);
    });

    $("#btnPrintXLPendingOverdue").click(function () {
        var nDiscountType = parseInt($("#cboDiscountType").val());
        PrintXL(7, nDiscountType);
    });
    //End Over due Payment


    //Start Pending Payment
    $("#txtContractorPendingPayment").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "ApplicantName",
            GlobalObjectList: _oPendingPayments,
            TableId: "tblPendingPayment"
        });
    });

    $("#txtBankBranchPendingPayment").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "BankName_Nego",
            GlobalObjectList: _oPendingPayments,
            TableId: "tblPendingPayment"
        });
    });

    $("#txtMktPersionPendingPayment").keyup(function (e) {
        $(this).icsSearchByText({
            Event: e,
            SearchProperty: "MKTPName",
            GlobalObjectList: _oPendingPayments,
            TableId: "tblPendingPayment"
        });
    });

    $("#btnSrc_PendingPayment").click(function () {
        var oExportBillPendingReport =
        {
            ExportBillID: 0,
            ReportType: 8,
            DiscountType: parseInt($("#cboDiscountType").val())
        };
        Gets_PendingPayment(oExportBillPendingReport);
    });

    $("#btnPrintBankWisePendingPayment").click(function () {
        var nDiscountType = parseInt($("#cboDiscountTypeTwo").val());
        PrintBankWise(8, nDiscountType);
    });

    $("#btnPrintPartyWisePendingPayment").click(function () {
        var nDiscountType = parseInt($("#cboDiscountTypeTwo").val());
        PrintPartyWise(8, nDiscountType);
    });

    $("#btnPrintXLPendingPayment").click(function () {
        var nDiscountType = parseInt($("#cboDiscountTypeTwo").val());
        PrintXL(8, nDiscountType);
    });
    //End Pending Payment


    var oDiscountType =
    {
        Value: 1,
        Text: "Discounted"
    };
    var oDiscountTypes = [];
    oDiscountTypes.push(oDiscountType);
    var oDiscountType =
      {
          Value: 2,
          Text: "Non-Discounted"
      };
    oDiscountTypes.push(oDiscountType);

    $("#cboDiscountType").icsLoadCombo({
        List: oDiscountTypes,
        OptionValue: "Value",
        DisplayText: "Text"
    });
    $("#cboDiscountTypeTwo").icsLoadCombo({
        List: oDiscountTypes,
        OptionValue: "Value",
        DisplayText: "Text"
    });
}

function PrintXL(nReportType, nDiscountType) {
    var nts = ((new Date()).getTime()) / 1000;
    window.open(_sBaseAddress + '/ExportBillPendingReport/PrintXL?nReportType=' + nReportType + '&nDiscountType=' + nDiscountType + " &nts=" + nts, "_blank");
}

function PrintBankWise(nReportType, nDiscountType) {
    var nts = ((new Date()).getTime()) / 1000;
    window.open(_sBaseAddress + '/ExportBillPendingReport/PrintBankWise?nReportType=' + nReportType + '&nDiscountType=' + nDiscountType + " &nts=" + nts, "_blank");
}

function PrintPartyWise(nReportType, nDiscountType) {
    var nts = ((new Date()).getTime()) / 1000;
    window.open(_sBaseAddress + '/ExportBillPendingReport/PrintPartyWise?nReportType=' + nReportType + '&nDiscountType=' + nDiscountType + " &nts=" + nts, "_blank");
}

function Gets_LCInHand(oExportBillPendingReport) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillPendingReport,
        ControllerName: "ExportBillPendingReport",
        ActionName: "GetsReport",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0)
            { 
                DynamicRefreshList(response.objs, "tblLCInHand");
                _oLCInHands = response.objs;
            }
            else
            {
                alert("Sorry, No data found.");
                DynamicRefreshList([], "tblLCInHand");
                _oLCInHands = [];
            }
        }
    });
}

function Gets_PendingPartyacceptance(oExportBillPendingReport) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillPendingReport,
        ControllerName: "ExportBillPendingReport",
        ActionName: "GetsReport",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0)
            {
                DynamicRefreshList(response.objs, "tblPendingPartyacceptance");
                _oPendingPartyacceptances = response.objs;
            }
            else {
                alert("Sorry, No data found.");
                DynamicRefreshList([], "tblPendingPartyacceptance");
                _oPendingPartyacceptances = [];
            }
        }
    });
}

function Gets_PendingSubmitToBank(oExportBillPendingReport) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillPendingReport,
        ControllerName: "ExportBillPendingReport",
        ActionName: "GetsReport",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblPendingSubmitToBank");
                _oPendingSubmitToBanks = response.objs;
            }
            else {
                alert("Sorry, No data found.");
                DynamicRefreshList([], "tblPendingSubmitToBank");
                _oPendingSubmitToBanks = [];
            }
        }
    });
}

function Gets_PendingLDBC(oExportBillPendingReport) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillPendingReport,
        ControllerName: "ExportBillPendingReport",
        ActionName: "GetsReport",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblPendingLDBC");
                _oPendingLDBCs = response.objs;
            }
            else {
                alert("Sorry, No data found.");
                DynamicRefreshList([], "tblPendingLDBC");
                _oPendingLDBCs = [];
            }
        }
    });
}

function Gets_PendingMaturity(oExportBillPendingReport) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillPendingReport,
        ControllerName: "ExportBillPendingReport",
        ActionName: "GetsReport",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblPendingMaturity");
                _oPendingMaturitys = response.objs;
            }
            else {
                alert("Sorry, No data found.");
                DynamicRefreshList([], "tblPendingMaturity");
                _oPendingMaturitys = [];
            }
        }
    });
}

function Gets_PendingOverdue(oExportBillPendingReport) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillPendingReport,
        ControllerName: "ExportBillPendingReport",
        ActionName: "GetsReport",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblPendingOverdue");
                _oPendingOverdues = response.objs;
            }
            else {
                alert("Sorry, No data found.");
                DynamicRefreshList([], "tblPendingOverdue");
                _oPendingOverdues = [];
            }
        }
    });
}

function Gets_PendingPayment(oExportBillPendingReport) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillPendingReport,
        ControllerName: "ExportBillPendingReport",
        ActionName: "GetsReport",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblPendingPayment");
                _oPendingPayments = response.objs;
            }
            else {
                alert("Sorry, No data found.");
                DynamicRefreshList([], "tblPendingPayment");
                _oPendingPayments = [];
            }
        }
    });
}



//function TestGets_LCInHand(oExportBillPendingReport) {
//    $.ajax({
//        type: "POST",
//        url: _sBaseAddress + "/ExportBillPendingReport/GetsReport",
//        traditional: true,
//        data: JSON.stringify(oExportBillPendingReport),
//        contentType: "application/json; charset=utf-8",
//        success: function (data) {
//            DynamicRefreshList(data, "tblLCInHand");
//            if (data.length == 0) {
//                alert("No data found.");
//            }
//        }
//    });
//}