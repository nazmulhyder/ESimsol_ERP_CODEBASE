

function InitializeDU_ProductionReceiveDeliveryEvents() {
  

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


 

    //Search 
    $("#btnSearch").click(function () {

        debugger;
        /* Issue Date*/
        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dStartDate = $('#txtStartDate').datebox('getValue');
        var dEndDate = $('#txtEndDate').datebox('getValue');
        var nReportType=0


        var chkResult = CheckEmpty();
        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = nDateSearch + "~" + dStartDate + "~" + dEndDate + "~" + nReportType;

        var oDU_ProductionReceiveDelivery = {
            ErrorMessage: sParams
        }


        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/DU_ProductionReceiveDelivery/AdvSearch",
            traditional: true,
            data: JSON.stringify(oDU_ProductionReceiveDelivery),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oDU_ProductionReceiveDeliverys = jQuery.parseJSON(data);
                if (oDU_ProductionReceiveDeliverys != null) {
                    DynamicRefreshList(oDU_ProductionReceiveDeliverys, "tblDU_ProductionReceiveDeliverys");
                }
            }
        });
    });

    $("#btnPrint").click(function () {
       

        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dStartDate = $('#txtStartDate').datebox('getValue');
        var dEndDate = $('#txtEndDate').datebox('getValue');
        var nReportType = 0
        var chkResult = CheckEmpty();
        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = nDateSearch + "~" + dStartDate + "~" + dEndDate + "~" + nReportType;

        window.open(_sBaseAddress + '/DU_ProductionReceiveDelivery/Print_DU_ProductionReceiveDelivery?sTempString=' + sParams, "_blank");
    });
    $("#btnPrintXL").click(function () {

        var scboDateSearch = document.getElementById("cboDateSearch");
        var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;
        var dStartDate = $('#txtStartDate').datebox('getValue');
        var dEndDate = $('#txtEndDate').datebox('getValue');
        var nReportType = 0
        var chkResult = CheckEmpty();
        if (chkResult != true) {
            alert("Please Select at lease one Criteria !!");
            return;
        }

        var sParams = nDateSearch + "~" + dStartDate + "~" + dEndDate + "~" + nReportType;

        window.open(_sBaseAddress + '/DU_ProductionReceiveDelivery/Print_PIReportXL?sTempString=' + sParams, "_blank");
    });
  
}


function DateActions() {

    var nDateOptionVal = $("#cboDateSearch").val();
    if (parseInt(nDateOptionVal) == 0) {
        $("#txtStartDate").datebox({ disabled: true });
        $("#txtStartDate").datebox("setValue", icsdateformat(new Date()));
        $("#txtEndDate").datebox({ disabled: true });
        $("#txtEndDate").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) >= 1 && parseInt(nDateOptionVal) <= 4) {
        $("#txtStartDate").datebox({ disabled: false });
        $("#txtStartDate").datebox("setValue", icsdateformat(new Date()));
        $("#txtEndDate").datebox({ disabled: true });
        $("#txtEndDate").datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) == 5 || parseInt(nDateOptionVal) == 6) {
        $("#txtStartDate").datebox({ disabled: false });
        $("#txtStartDate").datebox("setValue", icsdateformat(new Date()));
        $("#txtEndDate").datebox({ disabled: false });
        $("#txtEndDate").datebox("setValue", icsdateformat(new Date()));
    }
}


function CheckEmpty() {

    var scboDateSearch = document.getElementById("cboDateSearch");
    var nDateSearch = scboDateSearch.options[scboDateSearch.selectedIndex].index;

    if (nDateSearch == 0)
    {
        return false;
    }

    return true;
}

