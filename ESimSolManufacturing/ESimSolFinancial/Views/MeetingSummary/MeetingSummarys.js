function InitializeMeetingSummarysEvents() {


    //debugger;
    $("#btnPrintList").click(function () {
        debugger;
        if (!_oMeetingSummarys.length || !$.trim($('#txtCollectionPrintText').val()).length) return false;
    });
    $("#btnPrint").click(function () {
        //debugger;

        $('#btnPrintList')[0].click();//trigger('submit');
    });
    
    
    $("#btnRefresh").click(function () {
        debugger;
        if (!ValidateInputMeetingSummary()) { return false; }
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
                DynamicRefreshList(_oMeetingSummarys, "tblMeetingSummarys");

                for(var i=0;i<_oMeetingSummarys.length;i++)
                {
                    _oMeetingSummarys[i].DBServerDateTime = _oMeetingSummarys[i].DBServerDateTimeInString;
                }
                $('#txtCollectionPrintText').val(JSON.stringify(_oMeetingSummarys));

                $('#txtBuyerNamePrint').val($.trim($('#txtSearchByBuyerPI').val()));
                $('#txtDateFromPrint').val($.trim($('#txtDateFromMeetingSummary').datebox('getValue')));
                $('#txtDateToPrint').val($.trim($('#txtDateToMeetingSummary').datebox('getValue')));
            }
        });
    });

    


    $("#txtSearchByBuyerPI").keydown(function (e) {
        debugger;
        if (e.keyCode == 13) // Enter Press
        {
            var oContractor = {
                Params: 2 + '~' + $.trim($("#txtSearchByBuyerPI").val())
            };

            /* Multi Select */
            //$("#tblContractorsPicker").datagrid({ selectOnCheck: false, checkOnSelect: false });

            IntitializeContractorSearchByTextPicker();

            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oContractor,
                ControllerName: "Contractor",
                ActionName: "ContractorSearchByNameType",
                IsWinClose: false
            };

            $.icsDataGets(obj, function (response) {
                debugger;
                if (response.status && response.objs.length > 0) {
                    if (response.objs[0].ContractorID > 0) { _oContractorsPicker = response.objs; DynamicRefreshList(response.objs, "tblContractorsPicker"); }
                    else { _oContractorsPicker = []; DynamicRefreshList([], "tblContractorsPicker"); alert(response.objs[0].ErrorMessage); }
                    $("#winContractorPicker").icsWindow('open', "Buyer Search");
                    $("#divContractorPicker").focus();
                    $("#winContractorPicker input").val("");
                }
            });
        }
        else if (e.keyCode === 08 || e.keyCode === 46) {
            $("#txtSearchByBuyerPI").removeClass("fontColorOfPickItem");
            _oBuyer = { ContractorID: 0 };
        }




    });
    $("#btnPickBuyerPI").click(function () {
        debugger;
        var oContractor = {
            Params: 2 + '~' + $.trim($("#txtSearchByBuyerPI").val())
        };

        /* Multi Select */
        //$("#tblContractorsPicker").datagrid({ selectOnCheck: false, checkOnSelect: false });

        IntitializeContractorSearchByTextPicker();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oContractor,
            ControllerName: "Contractor",
            ActionName: "ContractorSearchByNameType",
            IsWinClose: false
        };

        $.icsDataGets(obj, function (response) {
            debugger;
            if (response.status && response.objs.length > 0) {
                if (response.objs[0].ContractorID > 0) { _oContractorsPicker = response.objs; DynamicRefreshList(response.objs, "tblContractorsPicker"); }
                else { _oContractorsPicker = []; DynamicRefreshList([], "tblContractorsPicker"); alert(response.objs[0].ErrorMessage); }
                $("#winContractorPicker").icsWindow('open', "Buyer Search");
                $("#divContractorPicker").focus();
                $("#winContractorPicker input").val("");
            }
        });
    });
    $("#btnClrBuyerPI").click(function () {
        _oBuyer = { ContractorID: 0 };
        $("#txtSearchByBuyerPI").val('');
        $("#txtSearchByBuyerPI").removeClass("fontColorOfPickItem");
    });
    $("#btnOkContractorPicker").click(function () {
        debugger;

        /* Multi Select*/
        //var oContractors = $("#tblContractorsPicker").icsGetCheckedItem();
        //if (oContractors != null && oContractors.length > 0) {
        //    var sContractorIDs = "";
        //    for (var i = 0; i < oContractors.length; i++) {
        //        sContractorIDs = sContractorIDs + oContractors.ContractorID + ",";
        //    }
        //    sContractorIDs = sContractorIDs.substring(0, sContractorIDs.length - 1);
        //    _sBaseBuyerIDs = sContractorIDs;
        //    $('#txtSearchByBuyerPI').val((oContractors.length > 1) ? oContractors.length + " Items selected." : oContractors.length + " Item selected.");
        //}


        /* Single Select*/
        var oContractor = $("#tblContractorsPicker").icsGetSelectedItem();
        debugger;
        if (oContractor != null && oContractor.ContractorID > 0) {
            $('#txtSearchByBuyerPI').val(oContractor.Name);
            $('#txtSearchByBuyerPI').addClass("fontColorOfPickItem");
            $('#txtSearchByBuyerPI').css("border", "");
            _oBuyer = oContractor;
        }


    });
    $("#btnCloseContractorPicker").click(function () {
        $("#winContractorPicker").icsWindow("close");
        _oContractorsPicker = [];
        $("#winContractorPicker input").val("");
        DynamicRefreshList([], "tblContractorsPicker");
    });

   

    
    
    $("#txtDateFromMeetingSummary").datebox('setValue', icsdateformat(new Date()));
    $("#txtDateToMeetingSummary").datebox('setValue', icsdateformat(new Date()));
}
function RefreshMeetingSummaryArguments() {
    var arg = 'Arguments;~';

    
    
    if (_oBuyer.ContractorID != null) {
        arg = arg + _oBuyer.ContractorID + '~';
    }
    var txtDateFromMeetingSummary = $.trim($("#txtDateFromMeetingSummary").datebox('getValue'));
    if (txtDateFromMeetingSummary != null) {
        arg = arg + txtDateFromMeetingSummary + '~';
    }
    var txtDateToMeetingSummary = $.trim($("#txtDateToMeetingSummary").datebox('getValue'));
    if (txtDateToMeetingSummary != null) {
        arg = arg + txtDateToMeetingSummary + '~';
    }


    _oMeetingSummary.ErrorMessage = arg;
}


function ValidateInputMeetingSummary() {

    
    if (!$.trim($("#txtDateFromMeetingSummary").datebox('getValue')).length) {
        alert("Please enter Date Range!");
        
        $('#txtDateFromMeetingSummary').focus();
        $('#txtDateFromMeetingSummary').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtDateFromMeetingSummary').css("border", "");
    }
    if (!$.trim($("#txtDateToMeetingSummary").datebox('getValue')).length) {
        alert("Please enter Date range!");
        
        $('#txtDateToMeetingSummary').focus();
        $('#txtDateToMeetingSummary').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtDateToMeetingSummary').css("border", "");
    }

    var txtDateFromMeetingSummary = $.trim($("#txtDateFromMeetingSummary").datebox('getValue'));
    var txtDateToMeetingSummary = $.trim($("#txtDateToMeetingSummary").datebox('getValue'));
    if (txtDateFromMeetingSummary != null && txtDateToMeetingSummary != null && new Date(txtDateFromMeetingSummary) > new Date(txtDateToMeetingSummary)) {
        alert("Invaild range!\nSecond value must be greater or equal.");
        $('#txtDateFromMeetingSummary').focus();
        $('#txtDateFromMeetingSummary').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtDateFromMeetingSummary').css("border", "");
    }

    if (_oBuyer == null || _oBuyer.ContractorID<=0) {
        alert("Please Select A Buyer!");

        $('#txtSearchByBuyerPI').focus();
        $('#txtSearchByBuyerPI').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtSearchByBuyerPI').css("border", "");
    }


    return true;
}


