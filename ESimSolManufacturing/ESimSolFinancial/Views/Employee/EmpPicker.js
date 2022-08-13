var _oEmployeeTypes = [];
var _oShifts = [];
var _oEmployeeCSs = [];
var _oEmployeeWSs = [];
var _EnumSettleMentTypes = [];
var _oBusinessUnits = [];

var _sEmployeeIDs = "";
var _sAttendanceSchemeID = "";
var _sBusinessUnitIds = "";
var _sDepartmentNames = "";
var _sDepartmentIds = "";
var _sDesignationNames = "";
var _sDesignationIds = "";
var _sLocationID = "";
var _oEmployeeCategorys = [];
var _sBlockNames = "";
var _sBMMIDs = "";

var _nLastEmployeeID = 0;
var _bNext = false;
var _nLoadRecords = 0;
var _nRowLength = 0;
var btnLocation = "";
var btnDesignation = "";
var btnEmployee = "";

function InitializeEmpPickerEvents() {
    LoadCateGory();
    LoadBU();
    InitializeAdvSearch();

    /*---------------- emp Search Start click----------------*/

    $("#cboDateType").change(function (e) {
        if ($("#cboDateType").val() == 1) {
            $("#regionDateTo").hide();
        }
        else {
            $("#regionDateTo").show();
        }
    });

    $("#cboMonthFrom").change(function (e) {
        var nDaysInMonth = (new Date((new Date()).getFullYear(), parseInt($("#cboMonthFrom").val()) + 1, 0)).getDate();
        var oDays = [];
        for (var i = 1; i <= nDaysInMonth; i++) {
            oDays.push({ DayID: i, Day: i });
        }
        $("#cboDayFrom").icsLoadCombo({
            List: oDays,
            OptionValue: "DayID",
            DisplayText: "Day",
            InitialValue: ''
        });
    });

    $("#cboMonthTo").change(function (e) {
        var nDaysInMonth = (new Date((new Date()).getFullYear(), parseInt($("#cboMonthTo").val()) + 1, 0)).getDate();
        var oDays = [];
        for (var i = 1; i <= nDaysInMonth; i++) {
            oDays.push({ DayID: i, Day: i });
        }
        $("#cboDayTo").icsLoadCombo({
            List: oDays,
            OptionValue: "DayID",
            DisplayText: "Day",
            InitialValue: ''
        });
    });

    $("#btnEmployee,#btnEmployee_Collection").click(function () {
        btnEmployee = $(this).attr("id");
        EmployeePickerReset();
        $("#winEmployeePicker").icsWindow("open", " Employee Picker");
    });

    $('#chkRange').click(function () {
        if (document.getElementById("chkRange").checked == true) {
            $("#lblRange1").show();
        }
        else {
            $("#lblRange1").hide();
        }
    });

    $('#btnEmpPickerSearch').click(function () {
        _bNext = false;
        AdvSearch();
    });

    $("#btnEmployeePickerOk").click(function () {
        debugger;
        var oEmployees = [];
        if (btnEmployee == "btnEmployee_Collection")
        {
            oEmployees = $('#tblEmployeesForPicker').datagrid('getChecked');
        }
        else
        {
            var oEmployee = $('#tblEmployeesForPicker').datagrid('getSelected');
            if (oEmployee!= null) oEmployees.push(oEmployee);
        }
        if (oEmployees.length <= 0) { alert("please select atleast one item"); return; }
        var sEmpIDs = "";
        var sEmpNames = "";
        for (var i = 0; i < oEmployees.length; i++) {
            sEmpIDs = sEmpIDs + oEmployees[i].EmployeeID + ",";
            sEmpNames = sEmpNames + oEmployees[i].Name + ",";
        }
        _sEmployeeIDs = sEmpIDs.substring(0, sEmpIDs.length - 1);
        sEmpNames = sEmpNames.substring(0, sEmpNames.length - 1);
        $("#txtEmployee").val(sEmpNames);
        $("#winEmployeePicker").icsWindow('close');
    });

    $("#btnEmployeePickerClose").click(function () {
        $("#winEmployeePicker").icsWindow('close');
    });

    $('#btnCEmployee').click(function (e) {
        document.getElementById("txtEmployee").value = "";
        _sEmployeeIDs = "";
    });

    /*-------------Attendance Schema Picker----------------*/
    $("#btnAttScheme").click(function (e) {
        var oAttendanceScheme = { AttendanceSchemeID: 0 };
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oAttendanceScheme,
            ControllerName: "AttendanceScheme",
            ActionName: "GetsAttendanceSchemeCurrentSession",
            IsWinClose: false
        };
        $.icsDataGets(obj, function (response) {

            if (response.status && response.objs.length > 0) {
                if (response.objs[0].AttendanceSchemeID > 0) {
                    debugger;
                    var tblColums = [];
                    var oColumn = { field: "Name", title: "Name", width: 150, align: "left" }; tblColums.push(oColumn);
                    oColumn = { field: "EmployeeType", title: "EmployeeType", width: 100, align: "left" }; tblColums.push(oColumn);
                    oColumn = { field: "DayOff", title: "DayOff", width: 115, align: "left" }; tblColums.push(oColumn);

                    var oPickerParam = {
                        winid: 'winAttendanceScheme',
                        winclass: 'clsAttendanceScheme',
                        winwidth: 420,
                        winheight: 460,
                        tableid: 'tblAttendanceSchemePicker',
                        tablecolumns: tblColums,
                        datalist: response.objs,
                        multiplereturn: false,
                        searchingbyfieldName: 'Name',
                        windowTittle: 'Attendance Scheme List'
                    };
                    $.icsPicker(oPickerParam);
                    IntializeAttendancePickerbutton(oPickerParam);//multiplereturn, winclassName
                }
                else { alert(response.objs[0].ErrorMessage); }
            }
        });
    });

    $("#btnResetAttScheme").click(function (e) {
        $('#txtAttendanceScheme').val("");
        _sAttendanceSchemeID = "";
    });

    /*-------------Start Business Unit Picker----------------*/
    $("#btnBusinessUnitPicker_Collection").click(function (e) {
        BusinessUnitPicker();
    });

    $("#txtBusinessUnit_Collection").keypress(function (e) {
        if (e.which == 13)//enter=13
        {
            BusinessUnitPicker();
        }
    });

    $("#btnResetBusinessUnitPicker_Collection").click(function (e) {
        $('#txtBusinessUnit_Collection').val("");
        _sBusinessUnitIds = "";
    });
    /*-------------End Business Unit Picker----------------*/

    /*-------------Location Picker----------------*/
    $("#btnLocationPicker,#btnLocationPicker_Colection").click(function (e) {
        debugger;
        btnLocation = $(this).attr("id");
        var sBusinessUnitIDs = "";
        var nBusinessUnitID = 0;
        if (btnLocation == "btnLocationPicker") {
            nBusinessUnitID = $("#cboBU").val();
            sBusinessUnitIDs = String(nBusinessUnitID);
        }
        else { sBusinessUnitIDs = _sBusinessUnitIds; }
        $("#winLocationPicker").icsWindow('open');
        var oLocation = { LocationID: 0, BusinessUnitIDs: sBusinessUnitIDs };
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oLocation,
            ControllerName: "Location",
            ActionName: "GetsLocationMenuTree",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.TLocation.id > 0) {
                    $('#locationtree').tree({ data: [response.obj.TLocation] });
                }
            }
        });
    });

    $("#btnLocationPickerOk").click(function (e) {
        if (btnLocation == "btnLocationPicker") {
            var oLocation = $('#locationtree').tree('getSelected');
            if (oLocation != null && oLocation.id > 0) {
                $("#winLocationPicker").icsWindow('close');
                _sLocationID = oLocation.id;
                $('#txtLocation').val(oLocation.text);
            }
            else {
                alert("Please select a location.");
            }
        }
        else {
            var oLocations = $('#locationtree').tree('getChecked');
            if (oLocations != null && oLocations.length > 0) {
                var LocationName = "";
                for (var i = 0; i < oLocations.length; i++) {
                    if (oLocations[i].id != 1) {
                        LocationName += oLocations[i].text + ",";
                        _sLocationID += oLocations[i].id + ",";
                    }
                }

                LocationName = LocationName.substring(0, LocationName.length - 1);
                _sLocationID = _sLocationID.substring(0, _sLocationID.length - 1);
                $("#winLocationPicker").icsWindow('close');
                $('#txtLocation_Colection').val(LocationName);
            }
            else {
                alert("Please select a location.");
            }
        }
    });

    $('#txtLocation_Colection').keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            //$("#winLocationPicker").icsWindow('open');
            //var oLocation={LocationID:0};
            btnLocation = $(this).attr("id");
            var sBusinessUnitIDs = "";
            var nBusinessUnitID = 0;
            if (btnLocation == "btnLocationPicker") {
                nBusinessUnitID = $("#cboBU").val();
                sBusinessUnitIDs = String(nBusinessUnitID);
            }
            else { sBusinessUnitIDs = _sBusinessUnitIds; }
            $("#winLocationPicker").icsWindow('open');
            var oLocation = { LocationID: 0, BusinessUnitIDs: sBusinessUnitIDs };

            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oLocation,
                ControllerName: "Location",
                ActionName: "GetsLocationMenuTree",
                IsWinClose: false
            };
            $.icsDataGet(obj, function (response) {

                if (response.status && response.obj != null) {
                    if (response.obj.TLocation.id > 0) {
                        $('#locationtree').tree({ data: [response.obj.TLocation] });
                    }
                }
            });
        }
    });

    $("#btnLocationPickerClose").click(function (e) {
        $("#winLocationPicker").icsWindow('close');
    });

    $("#btnResetLocationPicker,#btnResetLocationPicker_Colection").click(function (e) {
        $('#txtLocation').val("");
        $('#txtLocation_Colection').val("");
        _sLocationID = "";
    });

    $("#btnLocationPickerClose").click(function (e) {
        $("#winLocationPicker").icsWindow('close');
    });

    /*-------------Department Picker----------------*/

    $("#btnDepartmentPicker").click(function (e) {
        $("#winDepartmentPicker").icsWindow('open');
        var oDepartment = { DepartmentID: 0, BusinessUnitIDs: _sBusinessUnitIds, LocationIDs: _sLocationID };
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oDepartment,
            ControllerName: "Department",
            ActionName: "GetsDepartments",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.id > 0) {
                    $('#departmenttree').tree({ data: [response.obj] });
                }
            }
        });
    });

    $("#btnDepartmentPickerOk").click(function (e) {
        var oDepartment = $('#departmenttree').tree('getSelected');
        if (oDepartment != null && oDepartment.id > 0) {
            $("#winDepartmentPicker").icsWindow('close');
            $('#txtDepartment').val(oDepartment.text);
            _sDepartmentIds = oDepartment.id;
        }
        else {
            alert("Please select a department.");
        }
    });

    $("#btnDepartmentPickerClose").click(function (e) {
        $("#winDepartmentPicker").icsWindow('close');
    });

    $("#btnDepartmentPickerClose_Collection").click(function (e) {
        $("#winDepartmentPicker_Collection").icsWindow('close');
    });

    $("#btnResetDepartmentPicker,#btnResetDepartmentPicker_Collection").click(function (e) {
        $('#txtDepartment,#txtDepartment_Collection').val("");
        _sDepartmentIds = "";
        _sDepartmentNames = "";
    });

    /*-------------Designation Picker----------------*/
 
    $("#btnDesignationPicker,#btnDesignationPicker_Collection").click(function (e) {
        btnDesignation = $(this).attr("id");
        DesignationPicker();
    });

    $("#txtDesignation_Collection").keypress(function (e) {
        if (e.which == 13)//enter=13
        {
            btnDesignation = "btnDesignationPicker_Collection";
            DesignationPicker();
        }
    });

    $("#btnResetDesignationPicker,#btnResetDesignationPicker_Collection").click(function (e) {
        $('#txtDesignation').val("");
        $('#txtDesignation_Collection').val("");
        _sDesignationIds = "";
        _sDesignationNames = "";
    });

    /*------------- Start Block Picker----------------*/

    $("#btnBlock").click(function (e) {
        BlockPicker();
    });
    $("#btnResetBlock").click(function (e) {
        $('#txtBlock').val("");
        _sBlockNames = "";
        _sBMMIDs = "";
    });

    $("#txtBlock").keypress(function (e) {
        if (e.which == 13) {
            BlockPicker();
        }
    });
    /*------------- End Block Picker----------------*/

    /*-------------Department Picker start Collection----------------*/
    $("#btnDepartmentPicker_Collection").click(function (e) {
        DepartmentPicker();
    });

    $("#txtDepartment_Collection").keypress(function (e) {
        if (e.which == 13)//enter=13
        {
            DepartmentPicker();
        }
    });

    $("#btnDepartmentPickerOk_Collection").click(function (e) {
        var oDepartments = $('#departmenttree_Collection').tree('getChecked');
        if (oDepartments != null && oDepartments.length > 0) {
            $("#winDepartmentPicker_Collection").icsWindow('close');
            for (var i = 0; i < oDepartments.length; i++) {
                _sDepartmentNames += oDepartments[i].text + ",";
                _sDepartmentIds += oDepartments[i].id + ",";
            }

            _sDepartmentNames = _sDepartmentNames.substring(0, _sDepartmentNames.length - 1);
            _sDepartmentIds = _sDepartmentIds.substring(0, _sDepartmentIds.length - 1);
            $("#txtDepartment_Collection").val(_sDepartmentNames);
        }
        else {
            alert("Please select a department.");
        }
    });

    /*---------------- emp Search End click ----------------*/

    /*---------------- emp Search Start keyprees----------------*/
    $('#txtEmployee').keypress(function (e)
    {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            var sCodeName = $.trim($("#txtEmployee").val());
            if(sCodeName==''){alert("Please enter name or code to search.");$("#txtEmployee").focus(); return;}
            SearchEmployeeByText(sCodeName);
        }
    });

    function SearchEmployeeByText(sEmpNameCode){
        var oEmployee = { Params: sEmpNameCode +'~'+ 0};
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oEmployee,
            ControllerName: "EmployeeSettlement",
            ActionName: "GetsByEmpCode",
            IsWinClose: false
        };
        $.icsDataGets(obj, function (response) {
            debugger;
            if (response.status && response.objs.length > 0) {
                if (response.objs[0].EmployeeID > 0) {

                    var tblColums = [];var oColumn = { field: "Code", title: "Code", width: 70, align: "left" };tblColums.push(oColumn);
                    oColumn = { field: "Name", title: "Name", width: 120, align: "left" };tblColums.push(oColumn);
                    oColumn = { field: "DepartmentName", title: "Department", width: 100, align: "left" };tblColums.push(oColumn);
                    oColumn = { field: "DesignationName", title: "Designation", width: 100, align: "left" };tblColums.push(oColumn);
                    var oPickerParam = {
                        winid: 'winEmployeePickerTextSearch',
                        winclass:'clsEmployeePickerTextSearch',
                        winwidth: 455,
                        winheight: 460,
                        tableid: 'tblEmployeePickerTextSearch',
                        tablecolumns: tblColums,
                        datalist: response.objs,
                        multiplereturn: false,
                        searchingbyfieldName:'Name',
                        windowTittle: 'Employee list'
                    };

                    $.icsPicker(oPickerParam);
                    IntializeEmployeePickerTextSearch(oPickerParam);//multiplereturn, winclassName

                }
                else { alert(response.objs[0].ErrorMessage); }
            }
        });
    }

    function IntializeEmployeePickerTextSearch(oPickerobj)
    {
        $("#" + oPickerobj.winid).find("#btnOk").click(function () {
            var oreturnObj = $('#'+oPickerobj.tableid).datagrid('getSelected');
            if(oreturnObj == null || oreturnObj.EmployeeID<=0){  alert("please select an employee."); return false;}

            $("#"+oPickerobj.winid).icsWindow("close");
            $("#" + oPickerobj.winid).remove();
            if (oPickerobj.winid == 'winEmployeePickerTextSearch')
            {
                if (oreturnObj != null && oreturnObj.EmployeeID > 0)
                {
                    _sEmployeeIDs = oreturnObj.EmployeeID;
                    $("#txtEmployee").val(oreturnObj.Name);
                }
            }
        });

        $(document).find('.' +oPickerobj.winclass).keydown(function (e) {
            if (e.which == 13)//enter=13
            {
                var oreturnObj = $('#'+oPickerobj.tableid).datagrid('getSelected');
                if(oreturnObj == null || oreturnObj.EmployeeID<=0){  alert("please select an employee."); return false;}

                $("#"+oPickerobj.winid).icsWindow("close");
                $("#" + oPickerobj.winid).remove();

                if (oPickerobj.winclass == 'clsEmployeePickerTextSearch')
                {
                    if (oreturnObj != null && oreturnObj.EmployeeID > 0)
                    {
                        _sEmployeeIDs = oreturnObj.EmployeeID;
                        $("#txtEmployee").val(oreturnObj.Name);
                    }
                }
            }
        });
    }
}

function LoadCateGory() {
    $("#cboCategory").icsLoadCombo({
        List: _oEmployeeCategorys,
        OptionValue: "Value",
        DisplayText: "Text"
    });
}

function LoadBU() {
    $('#cboBU_Collection').combobox({
        data: _oBusinessUnits,
        valueField: 'BusinessUnitID',
        textField: 'Name',
        multiple: true
    });
    $('#cboBU_Collection').combobox('setValue', _oBusinessUnits.length > 0 ? _oBusinessUnits[0].BusinessUnitID : 0);
}

function InitializeAdvSearch() {
    $('#dtDateFrom').datebox('setValue', icsdateformat(new Date()));
    $('#dtDateTo').datebox('setValue', icsdateformat(new Date()));

    $("#cboEmployeeType").icsLoadCombo({
        List: _oEmployeeTypes,
        OptionValue: "EmployeeTypeID",
        DisplayText: "Name"
    });
    $("#cboShift_EmpSearch").icsLoadCombo({
        List: _oShifts,
        OptionValue: "ShiftID",
        DisplayText: "ShiftWithDuration"
    });
    $("#cboEmployeeCardStatus").icsLoadCombo({
        List: _oEmployeeCSs,
        OptionValue: "Value",
        DisplayText: "Text"
    });
    $("#cboEmployeeWorkigStatus").icsLoadCombo({
        List: _oEmployeeWSs,
        OptionValue: "Value",
        DisplayText: "Text"
    });
    $("#cboBU").icsLoadCombo({
        List: _oBusinessUnits,
        OptionValue: "BusinessUnitID",
        DisplayText: "Name"
    });
    $('#txtFrom,#txtTo').numberbox({ min: 0, precision: 0 });
    $('#txtFrom').numberbox('setValue', 1);
    $('#txtTo').numberbox('setValue', 50);
    $("#lblRange1").hide();

    var oMonths = [{ MonthID: 0, Name: 'Jan' }, { MonthID: 1, Name: 'Feb' }, { MonthID: 2, Name: 'Mar' }, { MonthID: 3, Name: 'Apr' },
                { MonthID: 4, Name: 'May' }, { MonthID: 5, Name: 'Jun' }, { MonthID: 6, Name: 'Jul' }, { MonthID: 7, Name: 'Aug' },
                { MonthID: 8, Name: 'Sep' }, { MonthID: 9, Name: 'Oct' }, { MonthID: 10, Name: 'Nov' }, { MonthID: 11, Name: 'Dec' }, ];

    $("#cboMonthFrom,#cboMonthTo").icsLoadCombo({
        List: oMonths,
        OptionValue: "MonthID",
        DisplayText: "Name",
        InitialValue: ''
    });

    LoadDaysInMonth();
}

function LoadDaysInMonth() {
    $("#cboMonthFrom,#cboMonthTo").val((new Date()).getMonth());
    var nDaysInMonth = (new Date((new Date()).getFullYear(), parseInt((new Date()).getMonth()) + 1, 0)).getDate();

    var oDays = [];
    for (var i = 1; i <= nDaysInMonth; i++) {
        oDays.push({ DayID: i, Day: i });
    }
    $("#cboDayFrom,#cboDayTo").icsLoadCombo({
        List: oDays,
        OptionValue: "DayID",
        DisplayText: "Day",
        InitialValue: ''
    });
}

function EmployeePickerReset() {
    _nLastEmployeeID = 0;
    _sEmployeeIDs = "";
    _bNext = false;
    _nLoadRecords = 0;
    _nRowLength = 0;
    _sAttendanceSchemeID = "";
    _sLocationID = "";
    _sDepartmentIds = "";
    _sDesignationIds = "";
    $(".txtReset").val("");
    $(".cboReset").val(0);
    $(".chkReset").prop("checked", false);

    $("#cboMonthFrom,#cboMonthTo").val((new Date()).getMonth());
    LoadDaysInMonth();

    DynamicRefreshList([], "tblEmployeesForPicker");
    $("#lblcount").html("");
    $('#txtFrom,#txtTo').numberbox({ min: 0, precision: 0 });
    $('#txtFrom').numberbox('setValue', 1);
    $('#txtTo').numberbox('setValue', 50);
    $("#lblRange1").hide();
}
function Next() {
    var oEmployees = [];
    oEmployees = $('#tblEmployeesForPicker').datagrid('getRows');
    if (oEmployees.length <= 0) { alert('Please Select Criteria and click on "Search" to find information.!!'); return; }
    _nRowLength = oEmployees.length;
    _bNext = true;
    AdvSearch();
}

function AdvSearch() {
    if (!_bNext) {
        _nRowLength = 0;
        _nLastEmployeeID = 0;
    }
    _nLoadRecords = document.getElementById("txtTo").value;
    if ($("#chkRange").is(':checked')) {
        if ($('#txtFrom').numberbox('getValue').length == '' || $('#txtTo').numberbox('getValue').length == '') { alert("Please enter valid range.") }
        var nRangeFrom = parseInt($('#txtFrom').numberbox('getValue'));
        var nRangeTo = parseInt($('#txtTo').numberbox('getValue'));
        if (nRangeFrom > nRangeTo) { alert("Invalid Range !"); return; }
        _nRowLength = nRangeFrom - 1;
        _nLoadRecords = nRangeTo - nRangeFrom + 1;
    }
    debugger
    var sName = $.trim($("#txtEmployeeName").val());
    var sCode = $.trim($("#txtEmpCodeForSearch").val());
    var sEnrollNo = $.trim($("#txtEnrollNo").val());
    var bIsnotEnroll = $("#chkNotassignedyet").is(':checked');
    var nEmployeeTypeID = $("#cboEmployeeType").val();
    var nShiftID = $("#cboShift_EmpSearch").val();
    var nCardStatus = $("#cboEmployeeCardStatus").val();
    var nWorkingStatus = $("#cboEmployeeWorkigStatus").val();
    var nDateType = $("#cboDateType").val();
    var sDOBFrom = parseInt($("#cboMonthFrom").val()) + 1 + '-' + $("#cboDayFrom").val();
    var sDOBTo = parseInt($("#cboMonthTo").val()) + 1 + '-' + $("#cboDayTo").val();
    var sGender = $("#cboGender option:selected").text();

    var bIsActive = ($("#chkActive").is(':checked')) ? 1 : 0;
    var bIsInactive = ($("#chkInActive").is(':checked')) ? 1 : 0;
    var bIsUser = ($("#chkUser").is(':checked')) ? 1 : 0;

    var bIsUnOfficial = ($("#chkOfficialNotAssign").is(':checked')) ? 1 : 0;
    var bIsOfficial = ($("#chkOfficialAssign").is(':checked')) ? 1 : 0;

    var bIsCardNotAsigned = ($("#chkCardNotAssigned").is(':checked')) ? 1 : 0;
    var bIsSalarystructureNotAsigned = ($("#chkSalarystructureNotAssigned").is(':checked')) ? 1 : 0;

    var bIsJoiningDate = $("#chkJoiningDate").is(':checked');
    var dtDateFrom = $('#dtDateFrom').datebox('getValue');
    var dtDateTo = $('#dtDateTo').datebox('getValue');

    if (bIsJoiningDate) {
        if (new Date(dtDateFrom) > new Date(dtDateTo)) {
            alert("Invalid Joining Date Range!");
            return;
        }
    }

    var nBusinessUnitID = $("#cboBU").val();
    var nCategory = $('#cboCategory').val();
    if (sName == '' && sCode == "" && sEnrollNo == "" && _sAttendanceSchemeID == "" && _sLocationID == "" && _sDepartmentIds == "" && _sDesignationIds == "" &&
       nEmployeeTypeID == 0 && sGender == "None" && nShiftID == 0 && bIsActive == 0 && bIsUnOfficial == 0 && bIsInactive == 0 && _sEmployeeIDs == "" && bIsUser == 0 &&
       bIsOfficial == 0 && nCardStatus == 0 && bIsCardNotAsigned == 0 && nWorkingStatus == 0 && bIsSalarystructureNotAsigned == 0 && nDateType == 0 && !bIsJoiningDate && !bIsnotEnroll && nCategory <= 0 && nBusinessUnitID <= 0) {
        alert("Please enter your selection criteria."); return false;
    }

    var sParam = sName + '~' + sCode + '~' + _sAttendanceSchemeID + '~' + _sLocationID + '~' + _sDepartmentIds + '~' + _sDesignationIds + '~' +
                 sGender + '~' + nEmployeeTypeID + '~' + nShiftID + '~' + bIsActive + '~' + bIsUnOfficial + '~' + bIsInactive + '~' + _sEmployeeIDs + '~' +
                 bIsUser + '~' + bIsOfficial + '~' + nCardStatus + '~' + bIsCardNotAsigned + "~" + nWorkingStatus + "~" + bIsSalarystructureNotAsigned + "~" +
                 sDOBFrom + "~" + sDOBTo + "~" + nDateType + "~" + _nRowLength + "~" + _nLoadRecords + "~" + bIsJoiningDate + "~" + dtDateFrom + "~" + dtDateTo + "~" + sEnrollNo + "~" + bIsnotEnroll + "~" + nCategory + "~" + nBusinessUnitID;

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/Employee/EmployeeSearch",
        traditional: true,
        data: JSON.stringify({ sParam: sParam }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var oEmps = jQuery.parseJSON(data);
            if (oEmps != null) {
                if (oEmps.length > 0) {
                    if (!_bNext) {
                        DynamicRefreshList(oEmps, 'tblEmployeesForPicker');
                    }
                    else {
                        var bAppend = false;
                        var oTEmps = $('#tblEmployeesForPicker').datagrid('getRows');
                        if (oTEmps.length > 0) {
                            for (var i = 0; i < oEmps.length; i++) {
                                var IsAppend = true;
                                for (var j = 0; j < oTEmps.length; j++) {

                                    if (oEmps[i].EmployeeID == oTEmps[j].EmployeeID) {
                                        IsAppend = false;
                                        break;
                                    }
                                }
                                if (IsAppend) {
                                    bAppend = true;
                                    $('#tblEmployeesForPicker').datagrid('appendRow', oEmps[i]);
                                }
                            }
                        }
                        else {
                            for (var j = 0; j < oEmps.length; j++) {
                                bAppend = true;
                                $('#tblEmployeesForPicker').datagrid('appendRow', oEmps[j]);
                            }
                        }
                        if (!bAppend) {
                            alert("No more data found!");
                        }
                    }

                }
                else {
                    alert("No more data found!");
                }
                var oEmployees = $('#tblEmployeesForPicker').datagrid('getRows');
                document.getElementById("lblcount").innerHTML = " | Count =" + oEmployees.length;
                _bNext = false;
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function IntializeAttendancePickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {

        var oAttendanceScheme = $('#' + oPickerobj.tableid).datagrid('getSelected');
        if (oPickerobj.winid == 'winAttendanceScheme') {
            if (oAttendanceScheme != null && oAttendanceScheme.AttendanceSchemeID > 0) {
                $("#" + oPickerobj.winid).icsWindow("close");
                $("#" + oPickerobj.winid).remove();
                $('#txtAttendanceScheme').val(oAttendanceScheme.Name);
                _sAttendanceSchemeID = oAttendanceScheme.AttendanceSchemeID;
            }
            else {
                alert("Please select a schema.");
            }
        }
    });
}

function BusinessUnitPicker() {
    var oBusinessUnit = {
        BusinessUnitID: 0
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oBusinessUnit,
        ControllerName: "BusinessUnit",
        ActionName: "GetsBusinessUnitWithPermission",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].BusinessUnitID > 0) {
                var tblColums = [];
                var oColumn = { field: "Code", title: "Code", width: 50, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Name", title: "Name", width: 170, align: "left" }; tblColums.push(oColumn);

                var bmultiplereturn = true;

                var oPickerParam = {
                    winid: 'winBusinessUnit',
                    winclass: 'clsBusinessUnit',
                    winwidth: 320,
                    winheight: 400,
                    tableid: 'tblBusinessUnit',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: bmultiplereturn,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Business Unit List'
                };
                $.icsPicker(oPickerParam);
                IntializeBusinessUnitPickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
    });
}

function IntializeBusinessUnitPickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        var oBusinessUnits = [];
        oBusinessUnits = $('#' + oPickerobj.tableid).datagrid('getChecked');
        if (oPickerobj.winid == 'winBusinessUnit') {
            if (oBusinessUnits != null && oBusinessUnits.length > 0) {
                $("#" + oPickerobj.winid).icsWindow("close");
                $("#" + oPickerobj.winid).remove();
                var sBusinessUnitName = "";
                for (var i = 0; i < oBusinessUnits.length; i++) {
                    sBusinessUnitName += oBusinessUnits[i].Name + ",";
                    _sBusinessUnitIds += oBusinessUnits[i].BusinessUnitID + ",";
                }

                sBusinessUnitName = sBusinessUnitName.substring(0, sBusinessUnitName.length - 1);
                _sBusinessUnitIds = _sBusinessUnitIds.substring(0, _sBusinessUnitIds.length - 1);
                $("#txtBusinessUnit_Collection").val(sBusinessUnitName);
            }
            else {
                alert("Please select a Business Unit.");
            }
        }
    });
}

function DesignationPicker() {
    var oDesignation = {
        DesignationID: 0,
        Params: _sBusinessUnitIds + '~' + _sLocationID + '~' + _sDepartmentIds
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oDesignation,
        ControllerName: "Designation",
        ActionName: "Gets",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].DesignationID > 0) {
                var tblColums = [];
                var oColumn = { field: "Code", title: "Code", width: 50, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Name", title: "Name", width: 170, align: "left" }; tblColums.push(oColumn);

                var bmultiplereturn = false;
                if (btnDesignation == "btnDesignationPicker_Collection") { bmultiplereturn = true; }

                var oPickerParam = {
                    winid: 'winDesignation',
                    winclass: 'clsDesignation',
                    winwidth: 320,
                    winheight: 460,
                    tableid: 'tblDesignation',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: bmultiplereturn,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Designation List'
                };
                $.icsPicker(oPickerParam);
                IntializeDesignationPickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
    });
}

function IntializeDesignationPickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        var oDesignations = [];

        if (btnDesignation == "btnDesignationPicker_Collection") {
            oDesignations = $('#' + oPickerobj.tableid).datagrid('getChecked');
        }
        else {
            var oDesignation = $('#' + oPickerobj.tableid).datagrid('getSelected');
            oDesignations.push(oDesignation);
        }

        if (oPickerobj.winid == 'winDesignation') {
            if (oDesignations != null && oDesignations.length > 0) {
                $("#" + oPickerobj.winid).icsWindow("close");
                $("#" + oPickerobj.winid).remove();

                for (var i = 0; i < oDesignations.length; i++) {
                    _sDesignationNames += oDesignations[i].Name + ",";
                    _sDesignationIds += oDesignations[i].DesignationID + ",";
                }

                _sDesignationNames = _sDesignationNames.substring(0, _sDesignationNames.length - 1);
                _sDesignationIds = _sDesignationIds.substring(0, _sDesignationIds.length - 1);
                if (btnDesignation == "btnDesignationPicker_Collection") {
                    $("#txtDesignation_Collection").val(_sDesignationNames);
                }
                else {
                    $("#txtDesignation").val(_sDesignationNames);
                }
            }
            else {
                alert("Please select a designation.");
            }
        }
    });
}

function DepartmentPicker() {
    $("#winDepartmentPicker_Collection").icsWindow('open');
    var oDepartment = { DepartmentID: 0, BusinessUnitIDs: _sBusinessUnitIds, LocationIDs: _sLocationID };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oDepartment,
        ControllerName: "Department",
        ActionName: "GetsDepartments",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.id > 0) {
                $('#departmenttree_Collection').tree({ data: [response.obj] });
            }
        }
    });
}

function BlockPicker() {
    var oBlockMachineMapping = {
        ProductionProcessInt: 0,
        DepartmentID: 0,
        BlockName: ""
    }
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oBlockMachineMapping,
        ControllerName: "BlockMachineMapping",
        ActionName: "BlockMachineMapping_Search",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {

        if (response.status && response.objs.length > 0) {
            if (response.objs[0].BMMID > 0) {
                var tblColums = [];
                var oColumn = { field: "BlockName", title: "Block Name", width: 230, align: "left" }; tblColums.push(oColumn);
                var bmultiplereturn = true;

                var oPickerParam = {
                    winid: 'winBlock',
                    winclass: 'clsBlock',
                    winwidth: 320,
                    winheight: 460,
                    tableid: 'tblShift',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: bmultiplereturn,
                    searchingbyfieldName: 'BlockName',
                    windowTittle: 'Block List'
                };
                $.icsPicker(oPickerParam);
                IntializeBlockPickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
    });
}

function IntializeBlockPickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        var oBlocks = [];
        oBlocks = $('#' + oPickerobj.tableid).datagrid('getChecked');
        if (oPickerobj.winid == 'winBlock') {
            if (oBlocks != null && oBlocks.length > 0) {
                $("#" + oPickerobj.winid).icsWindow("close");
                $("#" + oPickerobj.winid).remove();

                for (var i = 0; i < oBlocks.length; i++) {
                    _sBlockNames += oBlocks[i].BlockName + ",";
                    _sBMMIDs += oBlocks[i].BMMID + ",";

                }
                _sBlockNames = _sBlockNames.substring(0, _sBlockNames.length - 1);
                _sBMMIDs = _sBMMIDs.substring(0, _sBMMIDs.length - 1);
                $("#txtBlock").val(_sBlockNames);
            }
            else {
                alert("Please select a designation.");
            }
        }
    });
}