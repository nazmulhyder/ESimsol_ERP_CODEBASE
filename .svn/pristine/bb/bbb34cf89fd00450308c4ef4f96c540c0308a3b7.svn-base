function InitializeMarketingScheduleEvents() {
    
    $("#btnPrintList").click(function () {
        debugger;
        if (!_oMarketingSchedules.length || !$.trim($('#txtCollectionPrintText').val()).length) return false;
    });
    //MarketingSchedule

    $("#btnAddMarketingSchedule").click(function () {
        if (!ValidateInputMarketingSchedule()) return;
        var oMarketingSchedule = RefreshObjectMarketingSchedule();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oMarketingSchedule,
            ObjectId: oMarketingSchedule.MarketingScheduleID,
            ControllerName: "MarketingSchedule",
            ActionName: "Save",
            TableId: "tblMarketingSchedules",
            IsWinClose: false
        };
        $.icsSave(obj,function (response) {
            debugger;
            if (response.status) {
                if (response.obj.MarketingScheduleID > 0) {
                    _oDBMarketingSchedules.push(response.obj);

                    _oMarketingSchedules = [];
                    _oMarketingSchedules = $('#tblMarketingSchedules').datagrid('getRows');
                    for (var i = 0; i < _oMarketingSchedules.length; i++) {
                        _oMarketingSchedules[i].ScheduleDateTime = _oMarketingSchedules[i].ScheduleDateTimeInString;
                    }
                    $('#txtCollectionPrintText').val(JSON.stringify(_oMarketingSchedules));

                    if (_bIsFromCalendar) {
                        RefreshCalendarShcedule(response.obj.ScheduleDateInString);
                    }
                    else {
                        RefreshListSchedule(response.obj);
                    }


                }
            }
        });
    
    });
    $("#btnRemoveMarketingSchedule").click(function () {
        //debugger;
        var oMarketingSchedule = $("#tblMarketingSchedules").datagrid("getSelected");
        if (oMarketingSchedule == null || oMarketingSchedule.MarketingScheduleID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;
        

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oMarketingSchedule,
            ControllerName: "MarketingSchedule",
            ActionName: "Delete",
            TableId: "tblMarketingSchedules",
            IsWinClose: false
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: obj.BaseAddress + "/" + obj.ControllerName + "/" + obj.ActionName,
            traditional: true,
            data: JSON.stringify(obj.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                //debugger;
                var sMessage = jQuery.parseJSON(data);

                if (sMessage != null) {
                    if (sMessage.toLowerCase() == "deleted") {
                        alert("Delete successfully.");
                        var oSelectedObject = $("#" + obj.TableId).datagrid("getSelected");
                        var nRowIndexBank = $("#" + obj.TableId).datagrid("getRowIndex", oSelectedObject);
                        $("#" + obj.TableId).datagrid("deleteRow", nRowIndexBank);

                        debugger;
                        
                        var oMarketingSchedules = [];
                        for (var i = 0; i < _oDBMarketingSchedules.length; i++) {
                            if (_oDBMarketingSchedules[i].MarketingScheduleID != oSelectedObject.MarketingScheduleID) {
                                oMarketingSchedules.push(_oDBMarketingSchedules[i]);
                            }
                        }
                        _oDBMarketingSchedules = [];
                        _oDBMarketingSchedules = oMarketingSchedules;

                        _oMarketingSchedules = [];
                        _oMarketingSchedules = $('#tblMarketingSchedules').datagrid('getRows');
                        for (var i = 0; i < _oMarketingSchedules.length; i++) {
                            _oMarketingSchedules[i].ScheduleDateTime = _oMarketingSchedules[i].ScheduleDateTimeInString;
                        }
                        $('#txtCollectionPrintText').val(JSON.stringify(_oMarketingSchedules));
                        
                        if (_bIsFromCalendar) {
                            RefreshCalendarShcedule();
                        }
                        else {
                            RefreshListSchedule(oSelectedObject);
                        }
                        

                    }
                    else { alert(sMessage); }
                }
                else { alert("Operation Unsuccessful."); }

                if (obj.IsWinClose) { IcsWindowClose(oActiveWindows); };
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
        
    });

    $("#btnRefreshMarketingSchedule").click(function () {
        debugger;
        RefreshMarketingScheduleArguments();
        

        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MarketingSchedule/Gets",
            traditional: true,
            data: JSON.stringify(_oMarketingSchedule),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oMarketingSchedules = (jQuery.parseJSON(data) == null) ? [] : jQuery.parseJSON(data);

               

                DynamicRefreshList(_oMarketingSchedules, 'tblMarketingSchedules');
                for (var i = 0; i < _oMarketingSchedules.length; i++)
                {
                    _oMarketingSchedules[i].ScheduleDateTime = _oMarketingSchedules[i].ScheduleDateTimeInString;
                }
                $('#txtCollectionPrintText').val(JSON.stringify(_oMarketingSchedules));
            }
        });
    });

   
    $("#btnClose").click(function () {
        $('#txtMKTPerson').val('');
        $("#txtMKTPerson").removeClass("fontColorOfPickItem");
        $('#txtBuyer').val('');
        $('#txtBuyer').removeClass("fontColorOfPickItem");
        $("#winMarketingSchedule").icsWindow('close');
       
    });
    $("#txtBuyer").keydown(function (e) {
        debugger
        
        if (e.keyCode === 13) // Enter Press
        {
            PickContractor();
        }
        else if (e.keyCode === 08) {
            $("#txtBuyer").removeClass("fontColorOfPickItem");
            _oBuyer = null;
        }
    });

    $("#btnClrContractor_ELC").click(function () {
        $("#txtBuyer").removeClass("fontColorOfPickItem");
        $("#txtBuyer").val("");
        _oBuyer = null;
       
    });

    $("#btnPickContractor_ELC").click(function () {
        PickContractor();
    });

    $("#btnResetMktPerson").click(function () {

        $("#txtMKTPerson").val("");
        _oMKTPerson=null;
    });

    $("#btnPickMktAccount").click(function () {
        debugger;
        var sName = $.trim($("#txtMKTPerson").val());
        GetMktPersons(sName);
    });

    $("#txtMKTPerson").keydown(function (e) {
        var nkeyCode = e.keyCode || e.which;
        if (nkeyCode == 13) {
            var sName = $.trim($("#txtMKTPerson").val());
            if($('#txtMKTPerson').val()==null || $('#txtMKTPerson').val()=="")
            {
                alert("Please Type Name or part Name and Press Enter.");
                $('#txtMKTPerson').focus();
                return;
            }
            GetMktPersons(sName);
        }
        else if (nkeyCode == 8) {
            $("#txtMKTPerson").val("");
            _oFabricSalesContract.MktAccountID = 0;
        }
    });


}
function GenerateTableColumnsMarketingSchedule() {
    debugger;
    var otblColumns = [];
    var oColMKTPersonName = { field: 'MKTPersonName', width: "118px", title: 'MKT Person' };
    var oColBuyerName = { field: 'BuyerName', width: "118px", title: 'Buyer' };
    var oColScheduleDateTimeInString = { field: 'ScheduleDateTimeInString', width: "148px", title: 'Date Time' };
    var oColScheduleAssignByName = { field: 'ScheduleAssignByName', width: "118px", title: 'Assign By' };
    var oColMeetingDuration = { field: 'MeetingDurationHour', width: "70px", title: 'Duration' };
    var oColMeetingLocation = { field: 'MeetingLocation', width: "108px", title: 'Location' };
    var oColRemarks = { field: 'Remarks', width: "108px", title: 'Remarks' };

    var oColMeetingSummaryText = { field: 'MeetingSummaryText', width: "110px", formatter: function (value, row, index) { return FormatDetailMarketingSchedule(value, index); }, title: 'Summary Text' };
    otblColumns.push(
        oColMKTPersonName,
        oColBuyerName,
        oColScheduleDateTimeInString,
        oColScheduleAssignByName,
        oColMeetingDuration,
        oColMeetingLocation,
        oColRemarks,
        oColMeetingSummaryText
        );

    $('#tblMarketingSchedules').datagrid({ columns: [otblColumns] });
    _nbtnIDNo = 0;
}

function FormatDetailMarketingSchedule(value, index) {
    debugger;
    var sText = '';
    sText = (value.length > 15) ? sText + value.substring(0, 11) + '....' : sText = sText + value;
    var s = ' <label id="lblDetail' + (++_nbtnIDNo) + '" style="192px;" onclick="DetailViewMarketingSchedule(' + index + ')">' + sText + '</label>'
            + ' <input type="image" src="' + _sBaseAddress + '/Content/CSS/icons/pencil.png", id="btnDetail' + (++_nbtnIDNo) + '", onclick="DetailViewMarketingSchedule(' + index + ')", value=""/>';
        

    return s;
}
function DetailViewMarketingSchedule(index) {
    debugger;
    //if (nObjectID == null || nObjectID <= 0) {
    //    alert("Please select a item from list!");
    //    return;
    //}
    $('#tblMarketingSchedules').datagrid("selectRow", index);
    _oMarketingSchedule = $('#tblMarketingSchedules').datagrid("getSelected");

    if (_oMarketingSchedule.MarketingScheduleID <= 0) {
        alert("Please select a item from list!");
        return;
    }
    
     
    
   
    $.ajax({
        type: "POST",
        url: _sBaseAddress + "/MarketingSchedule/Get",
        traditional: true,
        data: JSON.stringify(_oMarketingSchedule),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            _oMarketingSchedule = { ErrorMessage: "", MKTPersonID: 0, ScheduleDateTime: ''};
            _oMarketingSchedule = jQuery.parseJSON(data);
            if (_oMarketingSchedule.MarketingScheduleID > 0) {
                RefreshControlMeetingSummary(_oMarketingSchedule);
                $("#winMarketingSummarys").icsWindow("open", "Marketing Meeting Summary @" + _oMarketingSchedule.ScheduleDateTimeInString);
            }
        }
    });
}

function RefreshMarketingScheduleArguments() {
    var arg = 'Arguments;';


    var txtScheduleDate = icsdateformat(icsdatetimeparser($("#txtScheduleDate").datetimebox('getValue')));
    if (txtScheduleDate != null) {
        arg = arg + txtScheduleDate + '~';
    }
    if (_oMKTPerson.MarketingAccountID != null) {
        arg = arg + _oMKTPerson.MarketingAccountID + '~';
    }
    
    if (_oBuyer.ContractorID != null) {
        arg = arg + _oBuyer.ContractorID + '~';
    }
    
    

    _oMarketingSchedule.ErrorMessage = arg;
    _oMarketingSchedule.IsForBaseCollection = false;
}

function RefreshControlMarketingSchedule(oMarketingSchedule) {
    debugger;
    _oMKTPerson = { MarketingAccountID: 0 };
    _oBuyer = { ContractorID: 0 };
    _oMKTPerson.MarketingAccountID = oMarketingSchedule.MKTPersonID;

    $('#txtMKTPerson').val('');
    $("#txtMKTPerson").removeClass("fontColorOfPickItem");
    $("#txtMKTPerson").val(oMarketingSchedule.MKTPersonName);
    if (oMarketingSchedule.MKTPersonName != null && oMarketingSchedule.MKTPersonName != "")
    {
        $("#txtMKTPerson").addClass("fontColorOfPickItem");
    }
    

    $('#txtScheduleDate').datetimebox('setValue', oMarketingSchedule.ScheduleDateTimeInString);
    GenerateTableColumnsMarketingSchedule();

    _oMarketingSchedules = [];
    _oMarketingSchedules = oMarketingSchedule.MarketingSchedules;
    DynamicRefreshList(_oMarketingSchedules, 'tblMarketingSchedules');
    for (var i = 0; i < _oMarketingSchedules.length; i++) {
        _oMarketingSchedules[i].ScheduleDateTime = _oMarketingSchedules[i].ScheduleDateTimeInString;
    }
    $('#txtCollectionPrintText').val(JSON.stringify(_oMarketingSchedules));

    $('#txtBuyer').val('');
    $('#txtBuyer').removeClass("fontColorOfPickItem");

    $('#txtMeetingLocation').val('');
    $('#txtMeetingDuration').val('');
    $("#txtHour").prop('disabled', true);
    $('#txtRemark').val('');
}

function RefreshObjectMarketingSchedule() {
    debugger;
    //var nMarketingScheduleId = (_oMarketingSchedule == null ? 0 : (_oMarketingSchedule.MarketingScheduleID == null) ? 0 : _oMarketingSchedule.MarketingScheduleID);
    var oMarketingSchedule = {
        MarketingScheduleID: 0,

        MKTPersonID : _oMKTPerson.MarketingAccountID,
        BuyerID : _oBuyer.ContractorID,
        ScheduleDateTime : $.trim($("#txtScheduleDate").datetimebox('getValue')),
        MeetingLocation : $.trim($("#txtMeetingLocation").val()),
        MeetingDuration :$.trim($("#txtMeetingDuration").val()),
        Remarks :$.trim($("#txtRemark").val()),
        ScheduleAssignBy: _oCurrentUser.UserID
        };
    return oMarketingSchedule;
}

function ValidateInputMarketingSchedule() {

    


    if (!$.trim($("#txtMKTPerson").val()).length || _oMKTPerson.MarketingAccountID<=0) {
        alert("Please Select a Marketing Person!");
        $('#txtMKTPerson').val("");
        $('#txtMKTPerson').focus();
        $('#txtMKTPerson').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtMKTPerson').css("border", "");
    }

    if (!$.trim($("#txtBuyer").val()).length || _oBuyer.ContractorID <= 0) {
        alert("Please select a Buyer!");
        $('#txtBuyer').val("");
        $('#txtBuyer').focus();
        $('#txtBuyer').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtBuyer').css("border", "");
    }

    if (!$.trim($("#txtScheduleDate").datetimebox('getValue')).length) {
        alert("Please select a Date!");
        $('#txtScheduleDate').datetimebox('setValue', icsdateformat(new Date()));
        $('#txtScheduleDate').focus();
        $('#txtScheduleDate').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtScheduleDate').css("border", "");
    }

    return true;
}

function PickContractor() {

    debugger;
    var oContractor = {
        Params: '2,3' + '~' + $.trim($("#txtBuyer").val()) + '~' + _nBUID
    };
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
                var tblColums = []; var oColumn = { field: "ContractorID", title: "Code", width: 50, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Name", title: "Name", width: 120, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Address", title: "Address", width: 150, align: "left" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winContractors',
                    winclass: 'clsContractors',
                    winwidth: 400,
                    winheight: 460,
                    tableid: 'tblContractors',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Applicant List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);
            }
            else {
                alert(response.objs[0].ErrorMessage);
            }
        }
        else {
            alert("Sorry, No Applicant Found.");
        }
    });
}
function GetMktPersons(sName) {
    debugger;
    var oMarketingAccount_BU = {
        Name: sName,
        BUID: _nBUID
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oMarketingAccount_BU,
        ControllerName: "MarketingAccount",
        ActionName: "MarketingAccountSearchByName",
        IsWinClose: false
    };
    $("#progressbar").progressbar({ value: 0 });
    $("#progressbarParent").show();
    var intervalID = setInterval(updateProgress, 250);
    $.icsDataGets(obj, function (response) {
        clearInterval(intervalID);
        $("#progressbarParent").hide();
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].MarketingAccountID > 0) {
                debugger;
                var tblColums = [];
                var oColumn = { field: "MarketingAccountID", title: "Code", width: 50, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Name", title: "Name", width: 180, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "GroupName", title: "Group Name", width: 120, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Phone", title: "Phone", width: 100, align: "left" }; tblColums.push(oColumn);

                var oPickerParam = {
                    winid: 'winMktAccount',
                    winclass: 'clsMktPersonPicker',
                    winwidth: 460,
                    winheight: 460,
                    tableid: 'tblMktPersonPicker',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
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
        SetPickerValueAssign(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which === 13) {
            SetPickerValueAssign(oPickerobj);
        }
    });
}

function SetPickerValueAssign(oPickerobj) {
    var oreturnObj = null, oreturnObjs = [];
    if (oPickerobj.multiplereturn) {
        oreturnObjs = $('#' + oPickerobj.tableid).datagrid('getChecked');
    } else {
        oreturnObj = $('#' + oPickerobj.tableid).datagrid('getSelected');
    }

    if (oPickerobj.winid == 'winContractors')
    {
    
        if (oreturnObj != null && oreturnObj.ContractorID > 0) {
            var oContractor = oreturnObj;
            $('#txtBuyer').val(oContractor.Name);
            $("#txtBuyer").addClass("fontColorOfPickItem");
            $("#txtBuyer").focus();
            $('#txtBuyer').css("border", "");
            _oBuyer = oContractor;
           
        } else {
            alert("Please select an Applicant.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winMktAccount')
    {
        debugger;
     
        if (oreturnObj != null && parseInt(oreturnObj.MarketingAccountID) > 0) {
            var txtMKTPerson = document.getElementById("txtMKTPerson");
            txtMKTPerson.style.color = "blue";
            txtMKTPerson.style.fontWeight = "bold";
            $('#txtMKTPerson').val(oreturnObj.Name);
            _oMKTPerson = oreturnObj;
        }
        else {
            alert("Data Not Found.");
        }
    }
   
    else if (oPickerobj.winid == 'winContractors_Base') {
        if (oreturnObj != null && oreturnObj.ContractorID > 0) {
            var oContractor = oreturnObj;
            $('#txtContractor_Base').val(oContractor.Name);
            $("#txtContractor_Base").addClass("fontColorOfPickItem");
            $("#txtContractor_Base").focus();
            $('#txtContractor_Base').css("border", "");
            _oBaseBuyer = oContractor;

        } else {
            alert("Please select an Applicant.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winMktAccount_Base') {
        debugger;

        if (oreturnObj != null && parseInt(oreturnObj.MarketingAccountID) > 0) {
            var txtMktPersonPI = document.getElementById("txtMktPersonPI");
            txtMktPersonPI.style.color = "blue";
            txtMktPersonPI.style.fontWeight = "bold";
            $('#txtMktPersonPI').val(oreturnObj.Name);
            _oBaseMKTPerson = oreturnObj;
        }
        else {
            alert("Data Not Found.");
        }
    }

    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
}
