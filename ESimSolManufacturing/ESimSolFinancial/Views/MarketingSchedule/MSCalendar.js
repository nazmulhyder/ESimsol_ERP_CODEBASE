function InitializeMSCalendarEvents() {
    debugger;
   

    $("#btnPreviousMarketingSchedule").click(function () {
        var sDate = $('#txtDateScheduleCalendar').datebox('getValue');
        var date = icsmonthparser(sDate);
        var year = date.getFullYear();
        var month = date.getMonth() - 1;


        $('#txtDateScheduleCalendar').datebox('setValue', icsmonthformat(new Date(year, month, 1)));

        sDate = $('#txtDateScheduleCalendar').datebox('getValue');
        if (sDate == null || sDate == "") {
            alert("Please select a date!");
            return;
        }
        _sCalendarDate = '';
        RefreshMSCalendarArguments();

        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MarketingSchedule/Gets",
            traditional: true,
            data: JSON.stringify(_oBaseMarketingSchedule),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oDBMarketingSchedules = (jQuery.parseJSON(data) == null) ? [] : jQuery.parseJSON(data);


                var sDate = $('#txtDateScheduleCalendar').datebox('getValue');
                var dCurrentDate = new Date();
                var sDay = icsCustomStringFormat(dCurrentDate.getDate(), '00');
                sDate = sDay + " " + sDate;
                RefreshCalendarShcedule(sDate);

            }
        });
        
    });

    $("#btnNextMarketingSchedule").click(function () {
        var sDate = $('#txtDateScheduleCalendar').datebox('getValue');
        var date = icsmonthparser(sDate);
        var year = date.getFullYear();
        var month = date.getMonth() + 1;

        $('#txtDateScheduleCalendar').datebox('setValue', icsmonthformat(new Date(year, month, 1)));

        sDate = $('#txtDateScheduleCalendar').datebox('getValue');
        if (sDate == null || sDate == "") {
            alert("Please select a date!");
            return;
        }

        _sCalendarDate = '';

        RefreshMSCalendarArguments();


        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MarketingSchedule/Gets",
            traditional: true,
            data: JSON.stringify(_oBaseMarketingSchedule),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oDBMarketingSchedules = (jQuery.parseJSON(data) == null) ? [] : jQuery.parseJSON(data);


                var sDate = $('#txtDateScheduleCalendar').datebox('getValue');
                var dCurrentDate = new Date();
                var sDay = icsCustomStringFormat(dCurrentDate.getDate(), '00');
                sDate = sDay + " " + sDate;
                RefreshCalendarShcedule(sDate);

            }
        });


    });


    $("#btnRefresh").click(function () {
        debugger;
        var sDate = $('#txtDateScheduleCalendar').datebox('getValue');
        if (sDate == null || sDate == "") {
            alert("Please select a date!");
            return;
        }

        _sCalendarDate = '';

        RefreshMSCalendarArguments();


        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MarketingSchedule/Gets",
            traditional: true,
            data: JSON.stringify(_oBaseMarketingSchedule),
            contentType: "application/json; charset=utf-8",
            success: function (data) {


                _oDBMarketingSchedules = (jQuery.parseJSON(data) == null) ? [] : jQuery.parseJSON(data);

                var sDate = $('#txtDateScheduleCalendar').datebox('getValue');
                var dCurrentDate = new Date();
                var sDay = icsCustomStringFormat(dCurrentDate.getDate(), '00');
                sDate = sDay + " " + sDate;
                RefreshCalendarShcedule(sDate);


            }
        });
    });

  
    
    $("#btnPickMktPersonPI").click(function () {
        debugger;
        var sName = $.trim($("#txtMktPersonPI").val());
        GetMktPersons_Base(sName);
    });

    $("#txtMktPersonPI").keydown(function (e) {
        var nkeyCode = e.keyCode || e.which;
        if (nkeyCode == 13) {
            var sName = $.trim($("#txtMktPersonPI").val());
            //if($('#txtMKTPerson').val()==null || $('#txtMKTPerson').val()=="")
            //{
            //    alert("Please Type Name or part Name and Press Enter.");
            //    $('#txtMKTPerson').focus();
            //    return;
            //}
            GetMktPersons_Base(sName);
        }
        else if (nkeyCode == 8) {
            $("#txtMktPersonPI").val("");
            _oBaseMKTPerson = { MarketingAccountID: 0 };
        }
    });
    $("#btnClrMktPersonPI").click(function () {
        _oBaseMKTPerson = { MarketingAccountID: 0 };
        $("#txtMktPersonPI").val('');
        $("#txtMktPersonPI").removeClass("fontColorOfPickItem");
    });
    $("#txtContractor_Base").keydown(function (e) {
        debugger
        if (e.keyCode === 13) // Enter Press
        {
            PickContractor_Base();
        }
        else if (e.keyCode === 08) {
            $("#txtContractor_Base").removeClass("fontColorOfPickItem");
            _oBaseBuyer = { ContractorID: 0 };
        }
    });



    $("#btnPickContractor_Base").click(function () {
        PickContractor_Base();
    });

    $("#btnClrContractor_Base").click(function () {
        _oBaseMKTPerson = { MarketingAccountID: 0 };
        $("#txtContractor_Base").val('');
        $("#txtContractor_Base").removeClass("fontColorOfPickItem");
        _oBaseBuyer = { ContractorID: 0 };
    });
    

    
    $('#txtDateScheduleCalendar').datebox('setValue', icsmonthformat(new Date()));
    var sDate = $('#txtDateScheduleCalendar').datebox('getValue');
    var dCurrentDate = new Date();
    var sDay = icsCustomStringFormat(dCurrentDate.getDate(), '00');
    sDate = sDay + " " + sDate;
    RefreshCalendarShcedule(sDate);
}
function RefreshMSCalendarArguments() {
    debugger;
    var arg = 'Arguments;';


    var txtDateScheduleCalendar = $("#txtDateScheduleCalendar").datebox('getValue');
    if (_sCalendarDate == null || _sCalendarDate == '') {
        if (txtDateScheduleCalendar != null) {
            arg = arg + txtDateScheduleCalendar + '~';
        }
    }
    else if (_sCalendarDate != null)
    {
        arg = arg + _sCalendarDate + '~';
    }

    if (_oBaseMKTPerson.MarketingAccountID != null) {
        arg = arg + _oBaseMKTPerson.MarketingAccountID + '~';
    }

    if (_oBaseBuyer.ContractorID != null) {
        arg = arg + _oBaseBuyer.ContractorID + '~';
    }



    _oBaseMarketingSchedule.ErrorMessage = arg;
    _oBaseMarketingSchedule.IsForBaseCollection = true;
    _oBaseMarketingSchedule.IsForBaseCalendar = true;

}
function DetailViewCalendar(sDate) {
    debugger;

    if (sDate == null || sDate == '') {
        alert("Please select a Date!");
        return;
    }

    $('#divCalendar').calendar('moveTo', new Date(sDate));

    _oBaseMarketingSchedule = { ErrorMessage: "", MKTPersonID: 0, ScheduleDateTime: '' };
    _sCalendarDate = sDate;
    RefreshMSCalendarArguments();
    _oBaseMarketingSchedule.ScheduleDateTime = sDate;
    _oBaseMarketingSchedule.IsForBaseCollection = false;

    $.ajax({
        type: "POST",
        url: _sBaseAddress + "/MarketingSchedule/GetForCalendar",
        traditional: true,
        data: JSON.stringify(_oBaseMarketingSchedule),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            _oBaseMarketingSchedule = { ErrorMessage: "", MKTPersonID: 0, ScheduleDateTime: '' };
            _oBaseMarketingSchedule = jQuery.parseJSON(data);
            _oBaseMarketingSchedule.MKTPersonID = _oBaseMKTPerson.MarketingAccountID;
            _oBaseMarketingSchedule.MKTPersonName = _sBaseMKTPersonName;

            RefreshControlMarketingSchedule(_oBaseMarketingSchedule);
            $("#winMarketingSchedule").icsWindow("open", "Schedule Entry");
        }
    });
}
function GenerateCalendarCellValue(Date) {
  
    var sDateString = icsdateformat(Date);

    var sDay = icsCustomStringFormat(Date.getDate(), '00');
    var sParam = "'" + sDateString + "'";
    for (var i = 0; i < _oDBMarketingSchedules.length; i++) {
        if (_oDBMarketingSchedules[i].ScheduleDateInString == sDateString) {
            return '<div class="icon-details md" onclick="DetailViewCalendar(' + sParam + ')">' + sDay + '</div>';
        }
    }
    return '<div class="icon-edit md" onclick="DetailViewCalendar(' + sParam + ')">' + sDay + '</div>';
}

function RefreshCalendarShcedule(sDate)
{
  
    //var sDate = $('#txtDateScheduleCalendar').datebox('getValue');
    //var dCurrentDate = new Date();
    //var sDay = icsCustomStringFormat(dCurrentDate.getDate(), '00');
    //sDate = sDay + " " + sDate;
    $('#divCalendar').calendar('moveTo', new Date(sDate));
    sDate = sDate.substring(3, sDate.length);
    var sLabel = (_sBaseMKTPersonName == null || _sBaseMKTPersonName == '') ? 'Marketing Schedule @ ' + sDate : "Marketing Schedule for " + _sBaseMKTPersonName + ' @ ' + sDate;
    $("#lblMonthHeader").html(sLabel);
}

function GetMktPersons_Base(sName) {

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
                    winid: 'winMktAccount_Base',
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

function PickContractor_Base() {


    var oContractor = {
        Params: '2,3' + '~' + $.trim($("#txtContractor_Base").val()) + '~' + _nBUID
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
                var tblColums = []; var oColumn = { field: "Name", title: "Name", width: 200, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ContractorTypeInString", title: "Type", width: 120, align: "left" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winContractors_Base',
                    winclass: 'clsContractors_Base',
                    winwidth: 400,
                    winheight: 460,
                    tableid: 'tblContractors_Base',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Party List'
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


