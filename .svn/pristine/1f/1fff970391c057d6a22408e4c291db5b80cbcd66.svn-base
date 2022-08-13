function onKeyDwonPickRefID(event) {
    debugger;
    if (event.which == 13) {
        if ($('#cboRefType').val() == 0)
        {
            alert("Please Select The Type First !!");
            $('#cboRefType').focus();
            return;
        }
        PickRefID($("#txtRefID").val());
    }
    else if (event.which == 8) {
        $("#txtRefID").val("");
        _nFabricID = 0;
    }
}
function PickRefID(txtFabric) {
    if ($('#cboRefType').val() == 0) {
        alert("Please Select The Type First !!");
        $('#cboRefType').focus();
        return;
    }
    var sSTRING = $("#txtRefID").val();
    var oFabric = { FabricNo: sSTRING };

    var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oFabric,
            ControllerName: "Fabric", //TechnicalSheet
            ActionName: "GetsFabricByFabricNo",//ViewStyleSearch
            IsWinClose: false
        };
    debugger;
    var tblColums = [];
    var oColumn = { field: "FabricNo", title: "FabricNo", width: "330px", align: "left" }; tblColums.push(oColumn);

    var pickerObj =
        {
            paramObj: obj,
            pickerName: 'FabricPicker',
            tblColums: tblColums,
            multipleReturn: false,
            searchingField: 'FabricNo',
            pkID: 'FabricID',
            callBack: SetFabric
        }

    DynamicPiker(pickerObj);
    ////////////// pickerName(unique),   obj,	table,    		multiReturn,	SearchingField,		pkID    
}
function SetFabric(oResult) {
    debugger;
    _nFabricID = oResult.FabricID;
    $("#txtRefID").val(oResult.FabricNo);
}
function SetPickerValueAssignMeeting(oPickerobj) {
    debugger;
    var oResult;
    if (oPickerobj.multiplereturn) {
        oResult = $('#' + oPickerobj.tableid).datagrid('getChecked');
    }
    else {
        oResult = $('#' + oPickerobj.tableid).datagrid('getSelected');
    }
    oPickerobj.callBack(oResult);
    //if (oPickerobj.winid == 'winCompanyPicker') 
    //{
    //    SetComBeneficiary(oResult);
    //}
    //if (oPickerobj.winid == 'winContractorPicker') 
    //{
    //    SetContractor(oResult);
    //}
    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
}
function DynamicPiker(pickerObj) {
    debugger;
    $("#progressbar").progressbar({ value: 0 });
    $("#progressbarParent").show();
    //setInterval(updateProgress,250);

    $.icsDataGets(pickerObj.paramObj, function (response) {

        debugger;
        if (response.status && response.objs.length > 0) {
            if (response.objs[0][pickerObj.pkID] > 0) {
                debugger;
                var tblColums = pickerObj.tablecolumns;
                var oPickerParam = {
                    winid: 'win' + pickerObj.pickerName,
                    winclass: 'cls' + pickerObj.pickerName,
                    winwidth: 400,
                    winheight: 460,
                    tableid: 'tbl' + pickerObj.pickerName + 's',
                    tablecolumns: pickerObj.tblColums,
                    datalist: response.objs,
                    multiplereturn: pickerObj.multipleReturn,
                    searchingbyfieldName: pickerObj.searchingField,
                    windowTittle: pickerObj.pickerName + ' List',
                    callBack: pickerObj.callBack
                };
                $.icsPicker(oPickerParam);
                $("#progressbar").progressbar({ value: 0 });//hide
                $("#progressbarParent").hide();
                IntializePickerbuttonMeeting(oPickerParam);
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
        else {
            alert("Data Not Found.");
            $("#progressbar").progressbar({ value: 0 });//hide
            $("#progressbarParent").hide();
            return;
        }
    });
}
function IntializePickerbuttonMeeting(oPickerobj) {
    debugger;
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        debugger;
        //for Single Select
        SetPickerValueAssignMeeting(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which == 13)//enter=13
        {
            SetPickerValueAssignMeeting(oPickerobj);
        }
    });
}
function ClearRefID()
{
    $('#txtRefID').val("");
    $('#cboRefType').val(0);
    _nFabricID = 0;
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function InitializeMeetingSummaryEvents() {


    //debugger;
    $("#btnAddMeetingSummary").click(function () {
        debugger;

        if (!ValidateInputMeetingSummary()) return;
        var oMeetingSummary = RefreshObjectMeetingSummary();

        var defaultSettings = {
            BaseAddress: _sBaseAddress,
            Object: oMeetingSummary,
            ObjectId: oMeetingSummary.MeetingSummaryID,
            ControllerName: "MeetingSummary",
            ActionName: "Save",
            TableId: "tblMeetingSummarys",
            IsWinClose: false
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var obj = jQuery.parseJSON(data);
                if (obj != null) {
                    if (!$.trim(obj.ErrorMessage).length) {
                        alert("Save Successfully.");

                        //debugger;

                        var oMeetingSummarys = [];
                        oMeetingSummarys.push(obj);
                        if (_oMeetingSummarys.length > 0) {
                            for (var i = 0; i < _oMeetingSummarys.length; i++) {
                                oMeetingSummarys.push(_oMeetingSummarys[i]);
                            }
                        }
                        _oMeetingSummarys = [];
                        _oMeetingSummarys = oMeetingSummarys;
                        RefreshListMeetingSummarys(_oMeetingSummarys);
                        $('#txtMeetingSummaryText').val('');
                        /////////////////update by akram
                        $('#txtPrice').val("");
                        $('#cboCurrency').val(0);
                        $('#cboRefType').val(0);
                        ClearRefID();
                        /////////////////End Edit////////////
                        debugger;
                        var oMS = {};
                        var oMSs = $("#tblMarketingSchedules").datagrid('getRows');
                        for (var i = 0;i<oMSs.length;i++)
                        {
                            if(oMSs[i].MarketingScheduleID==obj.MarketingScheduleID)
                            {
                                oMS = oMSs[i];
                                oMS.MeetingSummaryText = obj.MeetingSummaryText;
                            }
                        }
                        var nRowIndexSchedule = $("#tblMarketingSchedules").datagrid("getRowIndex", oMS);
                        $("#tblMarketingSchedules").datagrid("updateRow", { index: nRowIndexSchedule, row: oMS });
                    }
                    else { alert(obj.ErrorMessage); }
                }
                else { alert("Operation Unsuccessful."); }
                if (defaultSettings.IsWinClose) { IcsWindowClose(oActiveWindows); };
                
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });

    });
    
    $("#btnRefreshMeetingSummary").click(function () {
        debugger;
        _oMeetingSummary = { ErrorMessage: '' };
        RefreshMeetingSummaryArguments()

        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MeetingSummary/Gets",
            traditional: true,
            data: JSON.stringify(_oMeetingSummary),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oMeetingSummarys = (jQuery.parseJSON(data) == null) ? [] : jQuery.parseJSON(data);
                RefreshListMeetingSummarys(_oMeetingSummarys);
            }
        });
    });

    $("#btnCloseMeetingSummary").click(function () {
        $("#winMarketingSummarys").icsWindow('close');
        _nRefID = 0;
        $('#txtRefID').val("");
        $('#txtPrice').val("");
        $('#cboRefType').val(0);
        $('#cboCurrency').val(0);
    });

    //$("#btnPrintListMeetingSummary").click(function () {
    //    window.open(_sBaseAddress + "/MeetingSummary/PrintMeetingSummarys", "_blank");
    //});

    //$("#btnPrintInXLMeetingSummary").click(function () {
    //    window.open(_sBaseAddress + "/MeetingSummary/PrintMeetingSummarysInXL", "_blank");
    //});

    //DynamicRefreshList(_oMeetingSummarys, "tblMeetingSummarys");

    //RefreshMeetingSummaryLayout("load");
}
function RefreshMeetingSummaryArguments() {
    var arg = 'Arguments;';

    
    if (_oMarketingSchedule.MarketingScheduleID != null) {
        arg = arg + _oMarketingSchedule.MarketingScheduleID + '~';
    }

    _oMeetingSummary.ErrorMessage = arg;
}

function RefreshMeetingSummaryLayout(buttonId) {
    if (buttonId === "btnViewMeetingSummary") {
        $("#winMeetingSummary input").prop("disabled", true);
        $("#winMeetingSummary select").prop("disabled", true);
        $("#btnSaveMeetingSummary").hide();
    }
    else {
        $("#winMeetingSummary input").not("#txtMeetingSummaryCode").prop("disabled", false);
        $("#winMeetingSummary select").prop("disabled", false);
        $("#btnSaveMeetingSummary").show();
    }
    $("#txtNumebr").icsNumberField({ precision: 2 });
}
function ValidateInputMeetingSummary() {

    debugger;
    if (!$.trim($("#txtMeetingSummaryText").val()).length)
    {
        alert("Please enter Summary Point!");
        $('#txtMeetingSummaryText').val("");
        $('#txtMeetingSummaryText').focus();
        $('#txtMeetingSummaryText').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtMeetingSummaryText').css("border", "");
    }
    //if ($('#cboRefType').val() > 0 && _nFabricID == "")
    //{
    //    alert("Please Pick Any Fabric Or Select None The Fabric");
    //    $('#txtRefID').focus();
    //    return false;
    //}
    //if ($('#cboCurrency').val() > 0 && $('#txtPrice').val() == "") {
    //    alert("Please Enter The Price");
    //    $('#txtPrice').focus();
    //    return false;
    //}
    //if ($('#cboCurrency').val() == 0 && $('#txtPrice').val() != "") {
    //    alert("Please Select the Currency");
    //    $('#cboCurrency').focus();
    //    return false;
    //}

    return true;
}
function RefreshObjectMeetingSummary() {
    var nMeetingSummaryId = (_oMeetingSummary == null ? 0 : (_oMeetingSummary.MeetingSummaryID == null) ? 0 : _oMeetingSummary.MeetingSummaryID);
    debugger;
    var s1 = "";
    var s2 = "";
    if ($('#cboRefType').val() > 0) 
    {
        s1 = ',  ' + $("#cboRefType :selected").text() + ' : ' + $('#txtRefID').val();
    }
    if ($('#cboCurrency').val() > 0 && $('#cboRefType').val() == 0)
    {
        alert("Please Select Fabric First");
        $('#cboRefType').focus();
        return
    }
    if ($('#cboCurrency').val() > 0 && $('#cboRefType').val() > 0) {
        s2 = ',  ' + $("#cboCurrency :selected").text() + ' ' + $('#txtPrice').val();
    }
    var oMeetingSummary = {
        MeetingSummaryID: nMeetingSummaryId,
        MarketingScheduleID: _oMarketingSchedule.MarketingScheduleID,
        MeetingSummarizeBy: _oCurrentUser.UserID,
        RefID: 0,
        RefType: $('#cboRefType').val(),
        CurrencyID: $('#cboCurrency').val(),
        Price: $('#txtPrice').val(),
        MeetingSummaryText: $.trim($("#txtMeetingSummaryText").val()) +s1 + s2
    };
    return oMeetingSummary;
}
function RefreshControlMeetingSummary(oMarketingSchedule) {
    $("#txtMKTPersonMeetingSummary").val(oMarketingSchedule.MKTPersonName);
    $("#txtBuyerMeetingSummary").val(oMarketingSchedule.BuyerName);
    $("#txtMeetingSummaryText").val('');
    $('#ListMeetingSummary').empty();
    
    $("#txtMKTPersonMeetingSummary").prop('disabled', true);
    $("#txtBuyerMeetingSummary").prop('disabled', true);
    _oMeetingSummarys = oMarketingSchedule.MeetingSummarys;
    RefreshListMeetingSummarys(oMarketingSchedule.MeetingSummarys);

}
function RefreshListMeetingSummarys(oMeetingSummarys)
{
    debugger;
    $('#ListMeetingSummary').empty();
    if (oMeetingSummarys.length <= 0) { return; }
    
    for (var i = 0; i < oMeetingSummarys.length; i++) {
        
        var sMKTPersonName = '';
        var sText = '';
        var sLocationTime = '';


        sMKTPersonName = oMeetingSummarys[i].MeetingSummarizeByName + ' :';
        var summ = oMeetingSummarys[i].MeetingSummaryText;
        if ($.trim(summ).length > 150) {
            sText = sText + summ.substring(0, 146) + '....<b class="easyui-tooltip" title="'+oMeetingSummarys[i].MeetingSummaryText+'">more</b>';
        }
        else {
            sText = sText + summ;
        }
        var loc = _oMarketingSchedule.MeetingLocation;
        if ($.trim(loc).length > 15) {
            sLocationTime = sLocationTime + loc.substring(0, 11) + '....@' + oMeetingSummarys[i].DBServerDateTimeInString;
        }
        else {
            sLocationTime = sLocationTime + loc + '@' + oMeetingSummarys[i].DBServerDateTimeInString;
        }
        var string1 = "";
        var string2 = "";

        if ($('#cboRefType').val > 0 && $('#txtRefID').val != "")
        {
            string1 = 'Ref Type' + $("#cboRefType :selected").text() + ', RefID ' + $('#txtRefID').val();
        }
        if ($('#cboCurrency').val() > 0 && $('#txtPrice').val() == "")
        {
            string2 = 'Price :' + $('#txtPrice').val() + ' '+ $("#cboCurrency :selected").text();
        }


        var sSummaryDiv = '<div class="meetingsummary">'
            + '<p><b style="text-decoration:underline;">' + sMKTPersonName + '</b></p>'
            + '<p>' + sText + '</p>'
            + '<p>' + string1 + '</p>'
            + '<p>' + string2 + '</p>'
            + '<p><b>' + sLocationTime + '</b></p>'
            + '</div>';

        $('#ListMeetingSummary').append(sSummaryDiv);
    }
}