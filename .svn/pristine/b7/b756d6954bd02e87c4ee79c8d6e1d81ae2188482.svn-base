function InitializeMarketingSchedulesEvents() {
    $("#btnPreviousMarketingSchedule").click(function () {
        var sDate = $('#txtDateSchedule').datebox('getValue');
        var date = icsmonthparser(sDate);
        var year = date.getFullYear();
        var month = date.getMonth() - 1;


        $('#txtDateSchedule').datebox('setValue', icsmonthformat(new Date(year, month, 1)));

        _oBaseMarketingSchedule.ErrorMessage = 'Arguments;' + $('#txtDateSchedule').datebox('getValue') + '~';
        _oBaseMarketingSchedule.IsForBaseCollection = true;

        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MarketingSchedule/Gets",
            traditional: true,
            data: JSON.stringify(_oBaseMarketingSchedule),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oDBMarketingSchedules = (jQuery.parseJSON(data) == null) ? [] : jQuery.parseJSON(data);

                GenerateTableColumnsSchedule();
                GenerateTableData();

                DynamicRefreshList(_oSchedules, 'tblSchedules');

            }
        });
        //$("#winMarketingSchedule").icsWindow("open","Add MarketingSchedule");
        //$("#winMarketingSchedule input").not("input[type='button']").val("");
        //$("#chkIsOwnMarketingSchedule").prop("checked", false);
        //_oBaseMarketingSchedule = null;
        //RefreshMarketingScheduleLayout("btnPreviousMarketingSchedule"); //button id as parameter
    });

    $("#btnNextMarketingSchedule").click(function () {
        var sDate = $('#txtDateSchedule').datebox('getValue');
        var date = icsmonthparser(sDate);
        var year = date.getFullYear();
        var month = date.getMonth() + 1;

        $('#txtDateSchedule').datebox('setValue', icsmonthformat(new Date(year, month, 1)));

        _oBaseMarketingSchedule.ErrorMessage = 'Arguments;' + $('#txtDateSchedule').datebox('getValue') + '~';
        _oBaseMarketingSchedule.IsForBaseCollection = true;

        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MarketingSchedule/Gets",
            traditional: true,
            data: JSON.stringify(_oBaseMarketingSchedule),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oDBMarketingSchedules = (jQuery.parseJSON(data) == null) ? [] : jQuery.parseJSON(data);

                GenerateTableColumnsSchedule();
                GenerateTableData();

                DynamicRefreshList(_oSchedules, 'tblSchedules');

            }
        });

        
    });


    $("#btnRefresh").click(function () {
        debugger;

        _oBaseMarketingSchedule.ErrorMessage = 'Arguments;' + $('#txtDateSchedule').datebox('getValue') + '~';
        _oBaseMarketingSchedule.IsForBaseCollection = true;

        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MarketingSchedule/Gets",
            traditional: true,
            data: JSON.stringify(_oBaseMarketingSchedule),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oDBMarketingSchedules = (jQuery.parseJSON(data) == null) ? [] : jQuery.parseJSON(data);
                
                GenerateTableColumnsSchedule();
                GenerateTableData();

                DynamicRefreshList(_oSchedules, 'tblSchedules');
                
            }
        });
    });

    $('#txtDateSchedule').datebox('setValue', icsmonthformat(new Date()));

    GenerateTableColumnsSchedule();
    GenerateTableData();
    //debugger;
    DynamicRefreshList(_oSchedules, 'tblSchedules');

    RefreshMarketingScheduleLayout("load");
    
}

function GenerateTableColumnsSchedule() {
    _otblColumns = [];
    var oColumn = null;
    var sDate = $('#txtDateSchedule').datebox('getValue');
    var date = icsmonthparser(sDate);
    var year = date.getFullYear();
    var month = date.getMonth();
    var days = new Date(year, month + 1, 0).getDate();

    oColumn = { field: "MKTPersonName", width: "130", title: "Marketing Person", frozen: true };
    //_otblColumns.push(oColumn);
    $('#tblSchedules').datagrid({ frozenColumns: [[oColumn]] });

    for (var j = 0; j < days; j++) {
        var DayNo = icsCustomStringFormat((j + 1), '00');
        oColumn =
                {
                    field: 'Day' + DayNo,
                    width: "80",
                    formatter: function (value, row, index) { return FormatDetailSchedule(value, index); },
                    title: icsdateformat(new Date(year, month, j + 1))
                };
        _otblColumns.push(oColumn);
    }
    $('#tblSchedules').datagrid({ columns: [_otblColumns] });
}
function GenerateTableData() {
    debugger;
    var sDate = $('#txtDateSchedule').datebox('getValue');
    var date = icsmonthparser(sDate);
    var year = date.getFullYear();
    var month = date.getMonth();
    var days = new Date(year, month + 1, 0).getDate();
    _oSchedules = [];
    for (var i = 0; i < _oEmployees.length; i++) {

        _oSchedule = NewObject();
        _oSchedule.MKTPersonID = _oEmployees[i].MarketingAccountID;
        _oSchedule.MKTPersonName = _oEmployees[i].Name;



        for (var j = 0; j < days; j++) {
            var DayNo=icsCustomStringFormat((j + 1),'00');
            _oSchedule['Day' + DayNo] = GenerateCellValue(_oSchedule.MKTPersonID, new Date(year, month, j + 1));

        }
        _oSchedules.push(_oSchedule);
    }


}
function NewObject() {
    var oSchedule = { MKTPersonID: 0, MKTPersonName: '', Day01: '', Day02: '', Day03: '', Day04: '', Day05: '', Day06: '', Day07: '', Day08: '', Day09: '', Day10: '', Day11: '', Day12: '', Day13: '', Day14: '', Day15: '', Day16: '', Day17: '', Day18: '', Day19: '', Day20: '', Day21: '', Day22: '', Day23: '', Day24: '', Day25: '', Day26: '', Day27: '', Day28: '', Day29: '', Day30: '', Day31: '' };
    return oSchedule;
}
function GenerateCellValue(MKTPersonID, Date) {
    //debugger;
    var sDateString = icsdateformat(Date);
    var sReturn = MKTPersonID + '~' + sDateString + '~';
    var sBuyersName = '';
    for (var i = 0; i < _oDBMarketingSchedules.length; i++) {
        if (_oDBMarketingSchedules[i].MKTPersonID == MKTPersonID && _oDBMarketingSchedules[i].ScheduleDateInString == sDateString) {
            sBuyersName = sBuyersName + _oDBMarketingSchedules[i].BuyerShortName + ',';
        }
    }

    sBuyersName = sBuyersName.substring(0, sBuyersName.length - 1);
    sReturn =sReturn+ sBuyersName+'~';
    return sReturn;
}
function FormatDetailSchedule(value, index) {
    //debugger;
    var aValues = value.split('~');
    var nObjectID = parseInt(aValues[0]);
    var sDate = aValues[1];
    var sParam = "" + nObjectID + ",'" + sDate + "'," + index + "";
    var s = (aValues[2] == null || aValues[2] == '') ?
        ' <input type="image" src="'+ _sBaseAddress +'/Content/CSS/icons/pencil.png", id="btnDetail' + nObjectID + '", onclick="DetailViewSchedule(' + sParam + ')", value=""/>'
        : ' <label id="btnDetail' + nObjectID + '" onclick="DetailViewSchedule(' + sParam + ')">' + aValues[2] + '</label>';

    return s;
}
function DetailViewSchedule(nObjectID, sDate, index) {
    debugger;
    if (nObjectID == null || nObjectID <= 0) {
        alert("Please select a item from list!");
        return;
    }
    $('#tblSchedules').datagrid("selectRow", index);
    _oSchedule = $('#tblSchedules').datagrid("getSelected");

    if (_oSchedule.MKTPersonID <= 0) {
        alert("Please select a item from list!");
        return;
    }
    //alert("hello~" + nObjectID + '~' + sDate);
    _oBaseMarketingSchedule = { ErrorMessage: "", MKTPersonID: 0, ScheduleDateTime: '' };
    _oBaseMarketingSchedule.MKTPersonID = _oSchedule.MKTPersonID;
    _oBaseMarketingSchedule.ScheduleDateTime = sDate;
    $.ajax({
        type: "POST",
        url: _sBaseAddress + "/MarketingSchedule/Get",
        traditional: true,
        data: JSON.stringify(_oBaseMarketingSchedule),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            _oBaseMarketingSchedule = { ErrorMessage: "", MKTPersonID: 0, ScheduleDateTime: '' };
            _oBaseMarketingSchedule = jQuery.parseJSON(data);

            
            RefreshControlMarketingSchedule(_oBaseMarketingSchedule);
            $("#winMarketingSchedule").icsWindow("open", "Schedule Entry");
            //LoadImage(oMarketingSchedule.Company.CompanyID);

        }
    });
}

function RefreshMarketingScheduleLayout(buttonId) {
    if (buttonId === "btnViewMarketingSchedule") {
        $("#winMarketingSchedule input").prop("disabled", true);
        $("#winMarketingSchedule select").prop("disabled", true);
        $("#btnSaveMarketingSchedule").hide();
    }
    else {
        $("#winMarketingSchedule input").not("#txtMarketingScheduleCode").prop("disabled", false);
        $("#winMarketingSchedule select").prop("disabled", false);
        $("#btnSaveMarketingSchedule").show();
    }
    $("#txtNumebr").icsNumberField({ precision: 2 });
}

function RefreshListSchedule(oMarketingSchedule)
{
    var BaseMonth = (icsmonthparser($('#txtDateSchedule').datebox('getValue'))).getMonth();

    var date = icsdateparser(oMarketingSchedule.ScheduleDateInString);
    var year = date.getFullYear();
    var month = date.getMonth();
    var day = icsCustomStringFormat(date.getDate(), '00');
    var days = new Date(year, month + 1, 0).getDate();
    var oSchedule = {};

    if (month == BaseMonth) {
        for (var i = 0; i < _oSchedules.length; i++) {

            if (_oSchedules[i].MKTPersonID == oMarketingSchedule.MKTPersonID) {
                oSchedule = _oSchedules[i];
                oSchedule['Day' + day] = GenerateCellValue(oSchedule.MKTPersonID, date);
            }
        }
    }

    var nRowIndexSchedule = $("#tblSchedules").datagrid("getRowIndex", oSchedule);
    $("#tblSchedules").datagrid("updateRow", { index: nRowIndexSchedule, row: oSchedule });
}
