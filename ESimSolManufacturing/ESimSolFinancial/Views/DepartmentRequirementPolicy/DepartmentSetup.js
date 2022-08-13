var _oShifts = [];
var _oTDesignation = [];
var _oDeptReqPolicy = null;
function InitializeDepartmentSetupEvents() {
    RefreshCboShift();    
    RefreshDepartmentList(_oDepartmentSetUp);

    $('#txtSearchByCode').keypress(function (e) {        
        var c = String.fromCharCode(e.which);
        var txtSearchByCode = $('#txtSearchByCode').val();
        txtSearchByCode = txtSearchByCode + c;

        var bFlag = false;
        var sAccountHeadCode = "";
        var rows = $('#tblDepartmentSetUp').treegrid('getChildren', 1);
        for (i = 0; i < rows.length; ++i) {
            sAccountHeadCode = rows[i]['code'].substring(0, txtSearchByCode.length);
            if (txtSearchByCode.toUpperCase() == sAccountHeadCode.toUpperCase()) {
                var id = rows[i]['id'];
                $('#tblDepartmentSetUp').treegrid('select', id);
                break;
            }
        }
    });
    
    $('#txtSearchByName').keypress(function (e) {        
        var c = String.fromCharCode(e.which);
        var txtSearchByName = $("#txtSearchByName").val();
        txtSearchByName = txtSearchByName + c;
        var bFlag = false;
        var sAccountHeadName = "";
        var rows = $('#tblDepartmentSetUp').treegrid('getChildren', 1);
        for (i = 0; i < rows.length; ++i) {
            sAccountHeadName = rows[i]['text'].substring(0, txtSearchByName.length);
            if (txtSearchByName.toUpperCase() == sAccountHeadName.toUpperCase()) {
                var id = rows[i]['id'];
                $('#tblDepartmentSetUp').treegrid('select', id);
                break;
            }
        }
    });
    
    $("#btnReload").click(function () {
        $('#tblDepartmentSetUp').treegrid('reload');
    });
        
    $("#btnAddSetup").click(function () {
        var oDepartmentSetUp = $("#tblDepartmentSetUp").treegrid("getSelected");
        if (oDepartmentSetUp == null || parseInt(oDepartmentSetUp.LocationID) <= 0)
        {
            alert("Please select an Location from list!");
            return;
        }
        RefreshDeptReqDesignationsList([]);
        $("#winDepartmentSetUp").icsWindow("open", oDepartmentSetUp.text + " Department Setup");
        ResetAllFields("winDepartmentSetUp");
        _oDeptReqPolicy = {
            DepartmentRequirementPolicyID : 0,
            CompanyID: 1,//This is a Hard Qoutted Value
            LocationID: parseInt(oDepartmentSetUp.LocationID),
            BusinessUnitID: parseInt(oDepartmentSetUp.BusinessUnitID),
            LocationName : oDepartmentSetUp.text,
            DepartmentID : 0,
            Code : "",
            Description: "",
            strDepartmentCloseDays:""
        };
    });

    $("#btnEditSetup").click(function () {
        var oDepartmentSetUp = $("#tblDepartmentSetUp").treegrid("getSelected");
        if (oDepartmentSetUp == null || parseInt(oDepartmentSetUp.DeptReqPolicyID) <= 0) {
            alert("Please select an Department from list!");
            return;
        }

        var oDeptReqPolicy = {
            DepartmentRequirementPolicyID: parseInt(oDepartmentSetUp.DeptReqPolicyID)
        };

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/DepartmentRequirementPolicy/GetPolicy",
            traditional: true,
            data: JSON.stringify(oDeptReqPolicy),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oDeptReqPolicy = jQuery.parseJSON(data);
                if (oDeptReqPolicy != null) {
                    if (parseInt(oDeptReqPolicy.DepartmentRequirementPolicyID) > 0)
                    {                        
                        _oDeptReqPolicy = {
                            DepartmentRequirementPolicyID: parseInt(oDeptReqPolicy.DepartmentRequirementPolicyID),
                            CompanyID: 1,//This is a Hard Qoutted Value
                            LocationID: parseInt(oDeptReqPolicy.LocationID),
                            LocationName: oDeptReqPolicy.LocationName,
                            DepartmentID: parseInt(oDeptReqPolicy.DepartmentID),
                            BusinessUnitID: parseInt(oDepartmentSetUp.BusinessUnitID),
                            DepartmentName: oDeptReqPolicy.DepartmentName,
                            Description: oDeptReqPolicy.Description,
                            HeadCount: oDeptReqPolicy.HeadCount,
                            Budget: oDeptReqPolicy.Budget
                        };
                        _oShifts = oDeptReqPolicy.SelectedShifts;
                        DefineDeptReqDesignationDataGrid();
                        RefreshControls(oDeptReqPolicy.DepartmentCloseDays);
                        RefreshDeptReqDesignationsList(oDeptReqPolicy.TempDesignations);
                        $("#winDepartmentSetUp").icsWindow("open", oDeptReqPolicy.DepartmentName + " Department Setup");
                    }
                }
                else {
                    alert('Invalid Operation!')
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnRemoveSetup").click(function () {
        var oDepartmentSetUp = $("#tblDepartmentSetUp").treegrid("getSelected");
        if (oDepartmentSetUp == null || parseInt(oDepartmentSetUp.DeptReqPolicyID) <= 0) {
            alert("Please select an Department from list!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/DepartmentRequirementPolicy/Delete",
            traditional: true,
            data: JSON.stringify(oDepartmentSetUp),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var sFeedbackMessage = jQuery.parseJSON(data);
                if (sFeedbackMessage != null) {
                    if (sFeedbackMessage == "Deleted") {
                        alert("Delete Successfully");
                        window.location.href = _sBaseAddress + '/DepartmentRequirementPolicy/ViewDepartmentSetup?menuid=' + _nMenuID;
                    }
                    else
                    {
                        alert(sFeedbackMessage);
                    }
                }
                else {
                    alert(sFeedbackMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnCloseDepartmentSetUp").click(function () {        
        $("#winDepartmentSetUp").icsWindow('close');
    });

    $("#btnPickDepartment").click(function () {
        var oDepartment = {
            DepartmentID :0
        };

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/Department/GetsDepartments",
            traditional: true,
            data: JSON.stringify(oDepartment),
            contentType: "application/json; charset=utf-8",
            success: function (data) {                
                var oTDepartment = jQuery.parseJSON(data);
                if (oTDepartment != null) {
                    $("#winDepartment").icsWindow("open", "Department List");
                    $('#ttDepartment').tree({
                        data: [oTDepartment]
                    });
                    if (_oDeptReqPolicy.DepartmentID != null) {
                        var nNodeID = parseInt(_oDeptReqPolicy.DepartmentID);
                        if (nNodeID > 0) {
                            var node = $('#ttDepartment').tree('find', nNodeID);
                            $('#ttDepartment').tree('expandTo', node.target);
                            $('#ttDepartment').tree('select', node.target);
                        }
                    }
                }
                else {
                    alert('There is no department exists!')
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });        
    });

    $("#btnOkDepartment").click(function () {
        var oTDepartment = $('#ttDepartment').tree('getSelected');
        if (oTDepartment == null || parseInt(oTDepartment.id)<=0)
        {
            alert("Please select an department!");
            return;
        }
        _oDeptReqPolicy.DepartmentID = parseInt(oTDepartment.id);
        $('#txtDepartment').val(oTDepartment.text);
        $("#winDepartment").icsWindow('close');
    });

    $("#btnClearDepartment").click(function () {
        _oDeptReqPolicy.DepartmentID = 0;
        $('#txtDepartment').val('');
    });

    $("#btnCloseDepartment").click(function () {
        $("#winDepartment").icsWindow('close');
    });

    $("#btnPickDepartmentCloseDay").click(function () {        
        $("#winWeekDays").icsWindow("open", "Days Of Week");
        var oWeekDays = GetWeekDays();
        var data = oWeekDays;
        data = { "total": "" + data.length + "", "rows": data };
        $('#tblDay').datagrid('loadData', data);
        CheckSelectedDays();        
    });

    $("#btnOkWeekDays").click(function () {        
        var oSelectedDays = [];
        oSelectedDays = $('#tblDay').datagrid('getChecked');
        if (oSelectedDays.length <= 0) {
            alert("please select atleast one item");
            return;
        }
        var sWeekDays = "";
        for (var i = 0; i < oSelectedDays.length; i++) {
            sWeekDays += oSelectedDays[i].WeekDay + ',';
        }
        if (sWeekDays.length > 0) {
            sWeekDays = sWeekDays.substring(0,sWeekDays.length - 1);
        }
        $("#txtDepartmentClose").val(sWeekDays);
        $("#winWeekDays").icsWindow('close');
    });

    $("#btnClearDepartmentCloseDay").click(function () {
        $("#txtDepartmentClose").val('');
    });

    $("#btnCloseWeekDays").click(function () {
        $("#winWeekDays").icsWindow('close');
    });

    $("#btnPickDesignation").click(function () {
        var oDesignation = {
            DesignationID: 0
        };

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/Designation/GetsDesignations",
            traditional: true,
            data: JSON.stringify(oDesignation),
            contentType: "application/json; charset=utf-8",
            success: function (data) {                
                var oTDesignation = jQuery.parseJSON(data);
                if (oTDesignation != null) {
                    $("#winDesignation").icsWindow("open", "Designation List");
                    $('#ttDesignation').tree({
                        data: [oTDesignation]
                    });
                    var oAllNodes = $('#ttDesignation').tree('getChecked', 'unchecked');
                    if (oAllNodes != null) {
                        for (var n = 0; n < oAllNodes.length; n++) {
                            if (AlreadySelected(oAllNodes[n].id)) {
                                if ($('#ttDesignation').tree('isLeaf', oAllNodes[n].target)) {
                                    $('#ttDesignation').tree('check', oAllNodes[n].target);
                                }
                            }
                        }
                    }

                }
                else {
                    alert('There is no department exists!')
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnOkDesignation").click(function () {
        var oTDesignations = $('#ttDesignation').tree('getChecked');
        if (oTDesignations == null || oTDesignations.length <= 0) {
            alert("Please select Designations!");
            return;
        }

        _oTDesignations = [];
        for (var i = 0; i < oTDesignations.length; i++)
        {
            if (parseInt(oTDesignations[i].parentid) > 0)
            {
                _oTDesignations.push(oTDesignations[i])
            }
        }
        $('#txtDesignation').val(_oTDesignations.length + ' Item(s) Selected');
        $("#winDesignation").icsWindow('close');
    });

    $("#btnClearDesignation").click(function () {
        _oTDesignations = [];
        $('#txtDesignation').val('');
    });

    $("#btnCloseDesignation").click(function () {
        $("#winDesignation").icsWindow('close');
    });

    $("#btnAddDesignation").click(function () {        
        if (_oTDesignations == null || _oTDesignations.length <= 0)
        {
            alert("Please select Designations!");
            return;
        }
        var oDeptReqDesignations = $("#tblDeptReqDesignations").datagrid("getRows");
        for (var i = 0; i < _oTDesignations.length; i++) {
            if (!DesignationExists(_oTDesignations[i])) {
                var oDeptReqDesignation = {
                    Designation: _oTDesignations[i].text,
                    DesignationID: _oTDesignations[i].id,
                    DesignationResponsibilitys : [],
                    Sequence : 0,
                    Column1: 0,
                    Column2: 0,
                    Column3: 0,
                    Column4: 0,
                    Column5: 0
                };
                oDeptReqDesignations.push(oDeptReqDesignation);
            }
        }
        DefineDeptReqDesignationDataGrid();
        RefreshDeptReqDesignationsList(oDeptReqDesignations);
        _oTDesignations = [];
        $('#txtDesignation').val('');
    });

    $("#btnRemoveDesignation").click(function () {
        var oDeptReqDesignation = $("#tblDeptReqDesignations").datagrid("getSelected");
        if (oDeptReqDesignation == null || parseInt(oDeptReqDesignation.DesignationID) <= 0)
        {
            alert("Please select an designation from list!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblDeptReqDesignations').datagrid('getRowIndex', oDeptReqDesignation);
        $('#tblDeptReqDesignations').datagrid('deleteRow', SelectedRowIndex);
    });

    $("#btnShiftAdd").click(function () {        
        var nShiftID = $("#cboShift option:selected").val();
        var sShiftWithDuration = $("#cboShift option:selected").text();
        if (parseInt(nShiftID) <= 0)
        {
            alert('Please select an shift!');
            return;
        }

        for (var j = 0; j < _oShifts.length; j++) {
            if (parseInt(nShiftID) == parseInt(_oShifts[j].ShiftID)) {
                alert('This Shift Already Exists!!');
                return;
            }
        }
        var oShift = {
            ShiftID: nShiftID,
            ShiftWithDuration: sShiftWithDuration,
            Sequence: 0
        };
        _oShifts.push(oShift);
        var oDeptReqDesignations = $("#tblDeptReqDesignations").datagrid("getRows");
        DefineDeptReqDesignationDataGrid();
        RefreshDeptReqDesignationsList(oDeptReqDesignations);        
        $('#cboShift').val(0);
    });

    $("#btnShiftRemove").click(function () {        
        var nShiftID = $("#cboShift option:selected").val();        
        if (parseInt(nShiftID) <= 0) {
            alert('Please select an shift!');
            return;
        }
        if (!confirm("Confirm to Delete?")) return;

        var oShifts = _oShifts;
        _oShifts = [];
        for (var j = 0; j < oShifts.length; j++) {
            if (parseInt(nShiftID) != parseInt(oShifts[j].ShiftID)) {
                var oShift = {
                    ShiftID: parseInt(oShifts[j].ShiftID),
                    ShiftWithDuration: oShifts[j].ShiftWithDuration,
                    Sequence : 0
                };
                _oShifts.push(oShift);
            }
        }        
        var oDeptReqDesignations = $("#tblDeptReqDesignations").datagrid("getRows");
        DefineDeptReqDesignationDataGrid();
        RefreshDeptReqDesignationsList(oDeptReqDesignations);
        $('#cboShift').val(0);
    });

    $("#btnReloadDeptReqDesignations").click(function () {
        endEditing();
        var oDeptReqDesignations = $("#tblDeptReqDesignations").datagrid("getRows");        
        RefreshDeptReqDesignationsList(oDeptReqDesignations);        
    });

    $("#btnAddHRResponsibility").click(function () {
        debugger;
        endEditing();
        var oDeptReqDesignation = $("#tblDeptReqDesignations").datagrid("getSelected");
        if (oDeptReqDesignation == null || parseInt(oDeptReqDesignation.DesignationID) <= 0) {
            alert("Please select an designation from list!");
            return;
        }

        var oHRResponsibility = {
            HRResponsibilityID: 0
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/DepartmentRequirementPolicy/GetsHRResponsibility",
            traditional: true,
            data: JSON.stringify(oHRResponsibility),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                var oHRResponsibilitys = jQuery.parseJSON(data);
                if (oHRResponsibilitys != null) {

                    var data = oHRResponsibilitys;
                    data = { "total": "" + data.length + "", "rows": data };
                    $('#tblHRResponsibility').datagrid('loadData', data);
                    $('#tblHRResponsibility').datagrid({ selectOnCheck: false, checkOnSelect: false })

                    var oDesignationResponsibilitys = [];
                    oDesignationResponsibilitys = oDeptReqDesignation.DesignationResponsibilitys;
                    if (oDesignationResponsibilitys != null) {
                        if (oDesignationResponsibilitys.length > 0) {
                            var oHRResponsibilitys = $("#tblHRResponsibility").datagrid("getRows");
                            for (var i = 0; i < oHRResponsibilitys.length; i++) {
                                for (var j = 0; j < oDesignationResponsibilitys.length; j++) {
                                    if (parseInt(oHRResponsibilitys[i].HRRID) == parseInt(oDesignationResponsibilitys[j].HRResponsibilityID)) {
                                        $('#tblHRResponsibility').datagrid('checkRow', i);
                                    }
                                }
                            }
                        }
                    }
                    $("#winHRResponsibility").icsWindow("open", "HR Responsibility List");
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnOkHRResponsibility").click(function () {
        debugger;
        var oHRResponsibilitys = $('#tblHRResponsibility').datagrid('getChecked');
        if (oHRResponsibilitys.length <= 0) {
            alert("please select atleast one item");
            return;
        }
        var oDesignationResponsibilitys = [];
        for (var i = 0; i < oHRResponsibilitys.length; i++) {
            var oDesignationResponsibility = {
                DesignationResponsibilityID : 0,
                DepartmentRequirementDesignationID : 0,
                HRResponsibilityID: oHRResponsibilitys[i].HRRID
            };
            oDesignationResponsibilitys.push(oDesignationResponsibility);
        }
        var oDeptReqDesignation = $("#tblDeptReqDesignations").datagrid("getSelected");
        var nSelectedIndex = $("#tblDeptReqDesignations").datagrid("getRowIndex", oDeptReqDesignation);
        oDeptReqDesignation.DesignationResponsibilitys = oDesignationResponsibilitys;
        $('#tblDeptReqDesignations').datagrid('updateRow', { index: nSelectedIndex, row: oDeptReqDesignation });
        $("#winHRResponsibility").icsWindow('close');
    });

    $("#btnCloseHRResponsibility").click(function () {
        $("#winHRResponsibility").icsWindow('close');
    });

    $("#btnSaveDepartmentSetUp").click(function () {
        endEditing();
        if (!ValidateInput()) return;
        var oDeptReqPolicy = RefreshObject();

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/DepartmentRequirementPolicy/Save",
            traditional: true,
            data: JSON.stringify(oDeptReqPolicy),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                var oDeptReqPolicy = jQuery.parseJSON(data);
                if (oDeptReqPolicy.DepartmentRequirementPolicyID > 0) {
                    alert("Data Saved sucessfully");
                    $("#winDepartmentSetUp").icsWindow('close');
                    window.location.href = _sBaseAddress + '/DepartmentRequirementPolicy/ViewDepartmentSetup?menuid=' + _nMenuID;                    
                } else {
                    alert(oDeptReqPolicy.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });
}

function RefreshControls(oClosedDays)
{
    var sWeekDays = "";
    for (var i = 0; i < oClosedDays.length; i++) {
        sWeekDays += oClosedDays[i].WeekDay + ',';
    }
    if (sWeekDays.length > 0) {
        sWeekDays = sWeekDays.substring(0, sWeekDays.length - 1);
    }
    $("#txtDepartment").val(_oDeptReqPolicy.DepartmentName);
    $("#txtDepartmentClose").val(sWeekDays);
    $("#txtDescription").val(_oDeptReqPolicy.Description);
    $('#txtHeadCount').val(_oDeptReqPolicy.HeadCount);
    $('#txtBudget').val(_oDeptReqPolicy.Budget);
}

function ValidateInput() {

    if (_oDeptReqPolicy == null || parseInt(_oDeptReqPolicy.LocationID) <= 0) {
        alert('Invalid Location!')
        return false;
    }

    if (_oDeptReqPolicy == null || parseInt(_oDeptReqPolicy.DepartmentID)<=0)
    {
        alert('Please select Department!')
        return false;
    }

    var oDeptReqDesignations = $("#tblDeptReqDesignations").datagrid("getRows");
    if (oDeptReqDesignations == null || oDeptReqDesignations.length <= 0) {
        alert('Please add an Designations!')
        return false;
    }

    if (_oShifts==null || _oShifts.length <= 0) {
        alert('Please add an shift!')
        return false;
    }    
    return true;
}

function RefreshObject() {

    var sClosedDays = $('#txtDepartmentClose').val();
    var aDepartmentCloseDays = [];
    if (sClosedDays != null) {
        var aClosedDays = sClosedDays.split(',');
        for (var i = 0; i < aClosedDays.length; i++) {
            var oDepartmentCloseDay = {
                WeekDay: aClosedDays[i]
            };
            aDepartmentCloseDays.push(oDepartmentCloseDay);
        }
    }
    
    for (var i = 0; i < _oShifts.length; i++)
    {
        _oShifts[i].Sequence = i + 1;
    }

    var oTempDesignations = $("#tblDeptReqDesignations").datagrid("getRows");
    for (var i = 0; i < oTempDesignations.length; i++) {
        oTempDesignations[i].Sequence = i + 1;
    }


    var oDeptReqPolicy = {
        DepartmentRequirementPolicyID: parseInt(_oDeptReqPolicy.DepartmentRequirementPolicyID),
        LocationID: parseInt(_oDeptReqPolicy.LocationID),
        BusinessUnitID: parseInt(_oDeptReqPolicy.BusinessUnitID),
        DepartmentID: parseInt(_oDeptReqPolicy.DepartmentID),
        LocationName : _oDeptReqPolicy.LocationName,
        Department: $("#txtDepartment").val(),
        Description: $("#txtDescription").val(),
        DepartmentCloseDays: aDepartmentCloseDays,
        TempDesignations: oTempDesignations,
        HeadCount: parseInt($('#txtHeadCount').val()),
        Budget:$('#txtBudget').val(),
        Shifts: _oShifts
    };
    return oDeptReqPolicy;
}

function DefineDeptReqDesignationDataGrid() {    
    var tblColums = [];
    var oColumn = null;
    oColumn = { field: "Designation", title: "Designation", width: "150px" };
    tblColums.push(oColumn);
    if (_oShifts.length > 0) {
        for (var i = 0; i < _oShifts.length; i++) {
            oColumn = {
                field: 'Column' + (i + 1),
                title: _oShifts[i].ShiftWithDuration,
                width: "170",
                align: "right",
                editor: { type: 'numberbox', options: { precision: 0 } }
            };
            tblColums.push(oColumn);
        }
    }
    $('#tblDeptReqDesignations').datagrid({
        columns: [tblColums]
    });
}

function DesignationExists(oTDesignation) {
    var oDeptReqDesignations = $("#tblDeptReqDesignations").datagrid("getRows");    
    for (var i = 0; i < oDeptReqDesignations.length; i++) {
        if (parseInt(oDeptReqDesignations[i].DesignationID) == parseInt(oTDesignation.id))
        {
            return true;
        }
    }
    return false;
}

function RefreshDepartmentList(oDepartmentSetUp)
{
    console.log(oDepartmentSetUp);
    data = [oDepartmentSetUp];
    data = { "total": "" + data.length + "", "rows": data };
    $('#tblDepartmentSetUp').treegrid('loadData', data);
}

function RefreshDeptReqDesignationsList(oDeptReqDesignations) {
    data = oDeptReqDesignations;
    data = { "total": "" + data.length + "", "rows": data };
    $('#tblDeptReqDesignations').datagrid('loadData', data);
}

function GetWeekDays() {
    var objdays = [];
    var objday1 = { WeekDay: "Saturday" };
    var objday2 = { WeekDay: "Sunday" };
    var objday3 = { WeekDay: "Monday" };
    var objday4 = { WeekDay: "Tuesday" };
    var objday5 = { WeekDay: "Wednesday" };
    var objday6 = { WeekDay: "Thurseday" };
    var objday7 = { WeekDay: "Friday" };

    objdays.push(objday1);
    objdays.push(objday2);
    objdays.push(objday3);
    objdays.push(objday4);
    objdays.push(objday5);
    objdays.push(objday6);
    objdays.push(objday7);
    return objdays;
}

function CheckSelectedDays()
{
    var sClosedDays = $('#txtDepartmentClose').val();
    if (sClosedDays != null)
    {
        var aClosedDays = sClosedDays.split(',');
        for (var i = 0; i < aClosedDays.length; i++)
        {
            if (aClosedDays[i] == "Saturday") {
                $('#tblDay').datagrid('checkRow', 0);
            }
            else if (aClosedDays[i]== "Sunday") {
                $('#tblDay').datagrid('checkRow', 1);
            }
            else if (aClosedDays[i] == "Monday") {
                $('#tblDay').datagrid('checkRow', 2);
            }
            else if (aClosedDays[i] == "Tuesday") {
                $('#tblDay').datagrid('checkRow', 3);
            }
            else if (aClosedDays[i] == "Wednesday") {
                $('#tblDay').datagrid('checkRow', 4);
            }
            else if (aClosedDays[i] == "Thurseday") {
                $('#tblDay').datagrid('checkRow', 5);
            }
            else if (aClosedDays[i] == "Friday") {
                $('#tblDay').datagrid('checkRow', 6);
            }
        }
    }
}

function AlreadySelected(nKey) {
    for (var j = 0; j < _oTDesignations.length; j++) {
        if (_oTDesignations[j].id == nKey) {
            return true;
        }
    }
    return false;
}

function RefreshCboShift()
{
    var oHRMShift = {
        HRMShiftID:0
    };
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/HRMShift/GetsShifts",
        traditional: true,
        data: JSON.stringify(oHRMShift),
        contentType: "application/json; charset=utf-8",
        success: function (data) {            
            var oHRMShifts = jQuery.parseJSON(data);
            var listShifts = "<option value='" + 0 + "'>" + "--Select Shift--" + "</option>";
            if (oHRMShifts.length > 0) {
                for (var i = 0; i < oHRMShifts.length; i++) {
                    listShifts += "<option value='" + oHRMShifts[i].ShiftID + "'>" + oHRMShifts[i].ShiftWithDuration + "</option>";
                }
            }
            $("#cboShift").html(listShifts);
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function onClickRow(index) {    
    if (editIndex != index) {
        if (endEditing()) {
            $('#tblDeptReqDesignations').datagrid('selectRow', index).datagrid('beginEdit', index);
            editIndex = index;
        } else {
            $('#tblDeptReqDesignations').datagrid('selectRow', editIndex);
        }
    }
}

var editIndex = undefined;
function endEditing() {
    if (editIndex == undefined) { return true; }
    if ($('#tblDeptReqDesignations').datagrid('validateRow', editIndex)) {
        $('#tblDeptReqDesignations').datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}