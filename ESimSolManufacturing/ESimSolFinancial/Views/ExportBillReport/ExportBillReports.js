

var oExportBillReportColumns = new Object();
function LoadExportBillReportsEvents()
{
    DynamicRefreshList(_oExportBillReports, "tblExportBillReports");
    
    
    //$("#btnExportBillReportHistory").click(function () {
    //    var oExportBillReport = $("#tblExportBillReport").datagrid("getSelected");
    //    if (oExportBillReport == null || oExportBillReport.ExportBillReportID <= 0) { alert("Please select an item from list!"); return; }
    //    $("#winExportDBillHistory").icsWindow('open', "Export Bill History");
    //    ///   RefreshExportBillReportLayout("btnAddRecedFromParty");
        
    //    Gets_ExportBillReportHistory(oExportBillReport);
    //});


    $("#btnPrint").click(function () {
          var oExportBillReports = $('#tblExportBillReports').datagrid('getRows');
        if (oExportBillReports.length <= 0) {
            alert("There is no data to print !");
            return;
        }
        if (oExportBillReportColumns.length <= 0) {
            alert("Please Configure Fields!");
            return;
        }

        var sIDs = "";
        for (var i = 0; i < oExportBillReports.length; i++) {
            sIDs += oExportBillReports[i].ExportBillID + ",";
        }
        sIDs = sIDs.substring(0, sIDs.length - 1);
        debugger;
        var sFieldName = "";
        var sCaption = "";
        for (var i = 0; i < oExportBillReportColumns.length; i++) {
            sFieldName += oExportBillReportColumns[i].FieldName + ",";
            sCaption += oExportBillReportColumns[i].Caption + ",";
        }
        sFieldName = sFieldName.substring(0, sFieldName.length - 1);
        sCaption = sCaption.substring(0, sCaption.length - 1);

        var sExportBillFieldST = sFieldName + "~" + sCaption;

        window.open(_sBaseAddress + '/ExportBillReport/Print_ExportBillReport?sIDs=' + sIDs + '&sExportBillFieldST=' + sExportBillFieldST, "_blank")
    });

    $("#btnPrintXL").click(function () {
        var oExportBillReports = $('#tblExportBillReport').datagrid('getRows');
        if (oExportBillReports.length <= 0) {
            alert("There is no data to print !");
            return;
        }
        var sIDs = "";
        for (var i = 0; i < oExportBillReports.length; i++) {
            sIDs += oExportBillReports[i].ExportBillID + ",";
        }
        sIDs = sIDs.substring(0, sIDs.length - 1);
        window.open(_sBaseAddress + '/ExportBillReport/Print_ExportBillReportXL?sIDs=' + sIDs, "_blank")
    });

    $('#txtSearchbyLDBCNo').keypress(function (e) {
      
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            
            var oExportBillReport =
          {
              LDBCNo: document.getElementById('txtSearchbyLDBCNo').value
          };
            Gets_byLDBCNo(oExportBillReport);
        }

    });
    $('#txtSearchbyLCNo').keypress(function (e) {

        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            
            var oExportBillReport =
          {
              ExportLCNo: document.getElementById('txtSearchbyLCNo').value
          };
            Gets_byLCNo(oExportBillReport);
        }

    });
    $("#btnAdvSearchExportBillReport").click(function () {
        $("#winAdvSearchExportBillReport").icsWindow("open", "Advance Search");
        DynamicRefreshList([], "tblExportBillReportAdvSearch");
        //DateActions();
        //ResetAdvSearch();
        //UnselectAllRowsOfATable();
    });

    $("#btnConfiguration").click(function () {
     
        $("#winExportBillReportColumn").icsWindow('open', "Export Bill->Configuration");
        GetExportBill_ColConfiguration();
    });

    $("#btnOk_ExportBillReportColumn").click(function (e)
    {
        debugger;
        oExportBillReportColumns = $("#tblExportBillReportColumn").icsGetCheckedItem();
    });

    $("#btnRefresh").click(function (e) {
        debugger;
        var sColumns = ["ExportLCNo", "ApplicantName", "BankName_Advice", "BankName_Nego", "ExportBillNo", "AmountSt", "StateSt", "LCRecivedDateSt", "StartDateSt", "SendToPartySt", "RecdFromPartySt", "SendToBankDateSt", "RecedFromBankDateSt", "LDBCDateSt", "LDBCNo", "AcceptanceDateSt", "MaturityReceivedDateSt", "MaturityDateSt", "DiscountedDateSt", "RelizationDateSt", "BankFDDRecDateSt", "EncashmentDateSt"]
        for (var i = 0; i < sColumns.length; i++) {
            $('#tblExportBillReports').datagrid('showColumn', sColumns[i]);
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

        DynamicRefreshList(_oExportBillReports, "tblExportBillReports");
    });
}



function GetExportBill_ColConfiguration() {
 

    var oLists = [];
    var oList = new Object();

    oList.FieldName = "ExportLCNo";
    oList.Caption = "LC No";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "ApplicantName";
    oList.Caption = "Party Name";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "BankName_Advice";
    oList.Caption = "Advice Bank";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "BankName_Nego";
    oList.Caption = "Negotiate Bank";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "ExportBillNo";
    oList.Caption = "Bill No";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "AmountSt";
    oList.Caption = "Amount";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "StateSt";
    oList.Caption = "Current Status";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "LCRecivedDateSt";
    oList.Caption = "LC Recived Date";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "StartDateSt";
    oList.Caption = "Doc Prepare Date";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "SendToPartySt";
    oList.Caption = "Send To Party";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "RecdFromPartySt";
    oList.Caption = "Recd From Party";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "SendToBankDateSt";
    oList.Caption = "Send To Bank";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "RecedFromBankDateSt";
    oList.Caption = "Recd From Bank";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "LDBCDateSt";
    oList.Caption = "LDBC Date";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "LDBCNo";
    oList.Caption = "LDBC No";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "AcceptanceDateSt";
    oList.Caption = "Submit To Party Bank";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "MaturityReceivedDateSt";
    oList.Caption = "Maturity Red Date";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "MaturityDateSt";
    oList.Caption = "Maturity Date";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "DiscountedDateSt";
    oList.Caption = "Discounted Date";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "RelizationDateSt";
    oList.Caption = "Relization Date";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "BankFDDRecDateSt";
    oList.Caption = "BankFDD Recd Date";
    oLists.push(oList);

    oList = new Object();
    oList.FieldName = "EncashmentDateSt";
    oList.Caption = "Encashment Date";
    oLists.push(oList);

    DynamicRefreshList(oLists, "tblExportBillReportColumn");

}



function Gets_byLDBCNo(oExportBillReport) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillReport,
        ControllerName: "ExportBillReport",
        ActionName: "GetbyLDBCNo",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) { DynamicRefreshList(response.objs, "tblExportBillReports"); }
            else { DynamicRefreshList([], "tblExportBillReports"); }

        }
    });
}

function Gets_byLCNo(oExportBillReport) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillReport,
        ControllerName: "ExportBillReport",
        ActionName: "GetbyLCNo",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) { DynamicRefreshList(response.objs, "tblExportBillReports"); }
            else { DynamicRefreshList([], "tblExportBillReports"); }

        }
    });
}

function Gets_ExportBillReportHistory(oExportBillReport) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBillReport,
        ControllerName: "ExportBillReport",
        ActionName: "GetExportBillReportHistorys",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblExportBillReportHistorys");
                
            }
            else { DynamicRefreshList([], "tblExportBillReportHistorys"); }

        }
    });
}



function RefreshExportBillReportLayout(buttonId) {
    if (buttonId === "btnViewExportBillReport") {
        $("#winExportBillReport input").prop("disabled", true);
        $("#btnSaveExportBillReport").hide();
    }
    else {
        $("#winExportBillReport input").prop("disabled", false);
        $("#btnSaveExportBillReport").show();
    }
    $(".disabled input").prop("disabled", true);
}






//////////////