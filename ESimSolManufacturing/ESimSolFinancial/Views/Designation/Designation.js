/*var _oDesignation = { DesignationID: 0, ParentID: 0 };
var _oEmployees = [];
var _bIsAddDesignation = false;
var _sRemovedids = '';
var _nDepartmentRequirementPolicyID = 0;*/
function InitializeDesignationsEvents() {
    RefreshDesignationList(_oTDesignation);

    $("#btnNewDesignation").click(function () {
        var oDesignation = $('#tblDesignations').datagrid('getSelected');
        if (oDesignation == null || oDesignation.id <= 0) {
            alert("Please select a item from list!");
            return;
        }
        var SelectedRowIndex = $('#tblDesignations').datagrid('getRowIndex', oDesignation);
        $("#winDesignation").icsWindow("open", "New Designation");
        $("#txtDesignationName").focus();
        _oDesignation.ParentID = oDesignation.id;
        _oDesignation.DesignationID = 0;
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oDesignation,
            ControllerName: "Designation",
            ActionName: "getchildren",
            IsWinClose: false
        };
        DynamicRefreshList([], "tblDesignation");

        ResetAllFields("winDesignation");
        _bIsAddDesignation = true;
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/Designation/getchildren",
            traditional: true,
            data: JSON.stringify(oDesignation),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                var oRows = jQuery.parseJSON(data);
                if (oRows.length > 0) {
                    $('#tblDesignation').datagrid('loadData', oRows);                    
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });

        
    });

    $("#btnPickResponsibility").click(function () {
        debugger;

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

    $("#btnEditPickResponsibility").click(function () {
        debugger;

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
        var oResponsibility = $("#tblHRResponsibility").datagrid("getSelected");
        $('#txtResponsibility').data("HRRID", oResponsibility.HRRID);
        $('#txtResponsibility').data("DescriptionInBangla", oResponsibility.DescriptionInBangla);
        $('#txtResponsibility').val(oResponsibility.Description);

        $('#txtEditResponsibility').data("HRRID", oResponsibility.HRRID);
        $('#txtEditResponsibility').data("DescriptionInBangla", oResponsibility.DescriptionInBangla);
        $('#txtEditResponsibility').val(oResponsibility.Description);

        $("#winHRResponsibility").icsWindow('close');
    });

    $("#btnCloseHRResponsibility").click(function () {
        $("#winHRResponsibility").icsWindow('close');
    });

    $("#btnClearResponsibility").click(function () {
        $('#txtResponsibility').val('');
    });

    $("#btnEditClearResponsibility").click(function () {
        $('#txtEditResponsibility').val('');
    });

    $("#AddDesignation").click(function () {
        debugger;
        if (!ValidateInput()) return;
        var oDesignation = RefreshObject();
        
        $.ajax({
            type: "POST",
            dataType: "json",
            url: sessionStorage.getItem('BaseAddress') + "/Designation/Save",
            traditional: true,
            data: JSON.stringify(oDesignation),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oDesignation = jQuery.parseJSON(data);
                if (oDesignation.ErrorMessage == "" || oDesignation.ErrorMessage == null) {
                    alert("Data Saved sucessfully");
                    $('#tblDesignation').datagrid('appendRow', oDesignation);
                    
                }
                else {
                    alert(oDesignation.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }

        });

        //$.icsSave(obj);

        $("#txtCode").val("");
        $("#txtDesignationName").val("");
        $("#txtDesignationNameInBangla").val("");
        $("#txtResponsibility").val("");
        $("#txtDescription").val("");
        $("#txtDesignationName").focus();

    });

    $("#RemoveDesignation").click(function () {
        var oDesignation = $('#tblDesignation').datagrid('getSelected');
        if (oDesignation == null || oDesignation.DesignationID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (oDesignation.ParentID == 0) {
            alert("Root Designation is not deletable item");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblDesignation').datagrid('getRowIndex', oDesignation);

        if (oDesignation.DesignationID > 0) {

            debugger
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oDesignation,
                ControllerName: "Designation",
                ActionName: "Delete",
                TableId: "tblDesignation",
                IsWinClose: false
            };
            $.icsDelete(obj);
            _sRemovedids = _sRemovedids + oDesignation.DesignationID + ',';
        }
    });

    $("#btnClose").click(function () {
        debugger
        if (_sRemovedids.length > 0) {
            _sRemovedids = _sRemovedids.substring(0, _sRemovedids.length - 1);
        }
        oDesignations = $('#tblDesignation').datagrid('getRows');

        if (oDesignations != null) {
            if (_sRemovedids != "") {
                var RemovedIds = _sRemovedids.split(",");
                if (RemovedIds.length > 0) {
                    for (i = 0; i < RemovedIds.length; i++) {
                        $('#tblDesignations').treegrid('remove', RemovedIds[i]);
                    }
                }
            }
            var newDesignations = [];
            newDesignations = oDesignations;
            debugger
            if (newDesignations != null) {
                if (newDesignations.length > 0) {
                    for (i = 0; i < newDesignations.length; i++) {
                      
                        if (!IsdExists(_oDesignation.ParentID, newDesignations[i].DesignationID)) {
                            var oTDesignation = {
                                id: newDesignations[i].DesignationID,
                                text: newDesignations[i].Name,
                                state: '',
                                attributes: '',
                                parentid: newDesignations[i].ParentID,
                                code: newDesignations[i].Code,
                                Responsibility: newDesignations[i].Responsibility,
                                sequence: newDesignations[i].Sequence,
                                requiredPerson: newDesignations[i].RequiredPerson,
                                isActive: newDesignations[i].IsActive,
                                Description: newDesignations[i].Description
                            };
                            $('#tblDesignations').treegrid('append', { parent: _oDesignation.ParentID, data: [oTDesignation] });
                        }
                    }
                    $('#tblDesignations').treegrid('select', _oDesignation.ParentID);
                    $('#tblDesignations').treegrid('expand', _oDesignation.ParentID);
                }
            }
        }
        $("#winDesignation").icsWindow('close');
    });

    $("#btnEditDesignation").click(function () {
        debugger
        var oDep = $('#tblDesignations').datagrid('getSelected');
        if (oDep == null || oDep.id <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (oDep.parentid == 0) {
            alert("You can not edit root Designation");
            return;
        }
        var oDesignation = { DesignationID: oDep.id }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oDesignation,
            ControllerName: "Designation",
            ActionName: "getDesignation",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {

            if (response.status && response.obj.DesignationID > 0) {
                debugger
                _oDesignation = response.obj;
                $("#txtEditCode").val(_oDesignation.Code);
                $("#txtEditDesignationName").val(_oDesignation.Name);
                $("#txtEditDesignationNameInBangla").val(_oDesignation.NameInBangla);
                $("#txtEditDescription").val(_oDesignation.Description);
                $("#txtEditResponsibility").val(_oDesignation.Responsibility);
                if (_oDesignation.EmployeeTypeID > 0) {
                    $("#cboEmployeeType").val(_oDesignation.EmployeeTypeID);
                }
                else {
                    $("#cboEmployeeType").icsLoadCombo({
                        List: _oEmployeeTypes,
                        OptionValue: "EmployeeTypeID",
                        DisplayText: "Name",
                        InitialValue: "--Select Employee Type--"
                    });
                }
            }
        });
        $("#winEditDesignation").icsWindow('open');

    });

    $("#chkEmployeeType").change(function (e) {
        debugger;
        if ($("#chkEmployeeType").is(':checked')) {
            $("#cboEmployeeType").icsLoadCombo({
                List: _oEmployeeTypes,
                OptionValue: "EmployeeTypeID",
                DisplayText: "Name",
                InitialValue: "--Select Employee Type--"
            });
            if (_oDesignation.EmployeeTypeID > 0) {
                $("#cboEmployeeType").val(_oDesignation.EmployeeTypeID);
            }
            $('#cboEmployeeType').show();
        }
        else {
            if (_oDesignation.EmployeeTypeID > 0) {
                $("#cboEmployeeType").val(_oDesignation.EmployeeTypeID);
            }
            else {
                $("#cboEmployeeType").icsLoadCombo({
                    List: _oEmployeeTypes,
                    OptionValue: "EmployeeTypeID",
                    DisplayText: "Name",
                    InitialValue: "--Select Employee Type--"
                });
            }
            $('#cboEmployeeType').hide();
        }
    });

    $("#btnEdit").click(function () {
        if ($('#txtEditDesignationName').val() == null || $('#txtEditDesignationName').val() == "") {
            alert("Please enter Designation name!");
            $('#txtEditDesignationName').focus();
            return;
        }

        var oDesignation = {
            DesignationID: _oDesignation.DesignationID,
            Code: _oDesignation.Code,
            Name: $('#txtEditDesignationName').val(),
            NameInBangla: $('#txtEditDesignationNameInBangla').val(),
            Description: $('#txtEditDescription').val(),
            HRResponsibilityID: parseInt($('#txtEditResponsibility').data("HRRID")),
            Responsibility: $('#txtEditResponsibility').val(),
            ResponsibilityInBangla: $('#txtEditResponsibility').data("DescriptionInBangla"),
            ParentID: _oDesignation.ParentID,
            Sequence: 0,
            RequiredPerson: 1,
            EmployeeTypeID: $('#cboEmployeeType').val(),
            IsActive: true
        };

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oDesignation,
            ObjectId: oDesignation.DesignationID,
            ControllerName: "Designation",
            ActionName: "Save",
            TableId: "",
            IsWinClose: true
        };

        $.icsSave(obj, function (response) {
            if (response.status && response.obj.DesignationID > 0) {
                var oDesignation = response.obj;
                if (oDesignation.DesignationID > 0) {
                    var oTDesignation = {
                        id: oDesignation.DesignationID,
                        text: oDesignation.Name,
                        state: '',
                        attributes: '',
                        parentid: oDesignation.ParentID,
                        sequence: oDesignation.Sequence,
                        Responsibility: oDesignation.Responsibility,
                        requiredPerson: oDesignation.RequiredPerson,
                        code: oDesignation.Code,
                        isActive: oDesignation.IsActive,
                        Description: oDesignation.Description
                    };
                    $('#tblDesignations').treegrid('update', { id: oDesignation.DesignationID, row: oTDesignation });
                }
            }
        });
    });

    $("#RemoveDesignationFromCollection").click(function () {
        debugger;
        var oDep = $('#tblDesignations').datagrid('getSelected');
        if (oDep == null || oDep.id <= 0) {
            alert("Please select a item from list!");
            return;
        }

        if (oDep.parentid == 0) {
            alert("Root Designation is not deletable item");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblDesignation').datagrid('getRowIndex', oDep);
        var oDesignation = { DesignationID: oDep.id }
        if (oDesignation.DesignationID > 0) {

            debugger;
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oDesignation,
                ControllerName: "Designation",
                ActionName: "Delete",
                TableId: "",
                IsWinClose: false
            };
            //$.icsDelete(obj);
            //$('#tblDesignations').treegrid('remove', oDesignation.DesignationID);

            $.icsDelete(obj, function (response) {
                debugger;
                if (response.status && response.Message == 'deleted') {

                    $('#tblDesignations').treegrid('remove', oDesignation.DesignationID);
                    $('#tblDesignations').treegrid('select', oDesignation.ParentID);;
                    var oParent = $('#tblDesignations').datagrid('getSelected');
                    if (oParent != null) {
                        var oChildrens = [];
                        for (var i = 0; i < oParent.children.length; i++) {

                            if (oParent.children[i].DesignationID != oDesignation.DesignationID) {
                                oChildrens.push(oParent.children[i]);
                            }
                        }
                        oParent.children = oChildrens;
                        $('#tblDesignations').treegrid('update', { id: oParent.DesignationID, row: oParent });
                    }
                    location.reload();
                }
            })
            
        }
    });

    $('#txtSearchByCode').keypress(function (e) {
        debugger;
        var c = String.fromCharCode(e.which);
        var txtSearchByCode = document.getElementById('txtSearchByCode').value;
        txtSearchByCode = txtSearchByCode + c;

        var bFlag = false;
        var sAccountHeadCode = "";
        var rows = $('#tblDesignations').treegrid('getChildren', 1);
        for (i = 0; i < rows.length; ++i) {
            sAccountHeadCode = rows[i]['code'].substring(0, txtSearchByCode.length);
            if (txtSearchByCode.toUpperCase() == sAccountHeadCode.toUpperCase()) {
                var id = rows[i]['id'];
                $('#tblDesignations').treegrid('select', id);
                break;
            }
        }
    });

    $('#txtSearchByName').keypress(function (e) {
        debugger;
        var c = String.fromCharCode(e.which);
        var txtSearchByName = document.getElementById('txtSearchByName').value;
        txtSearchByName = txtSearchByName + c;

        var bFlag = false;
        var sAccountHeadName = "";
        var rows = $('#tblDesignations').treegrid('getChildren', 1);
        for (i = 0; i < rows.length; ++i) {
            sAccountHeadName = rows[i]['text'].substring(0, txtSearchByName.length);
            if (txtSearchByName.toUpperCase() == sAccountHeadName.toUpperCase()) {
                var id = rows[i]['id'];
                $('#tblDesignations').treegrid('select', id);
                break;
            }
        }
    });

    $("#btnEditDesignationClose").click(function () {
        $("#winEditDesignation").icsWindow('close');
    });
}

function IsdExists(nparentid, id) {
    var oChildNodes = $('#tblDesignations').treegrid('getChildren', nparentid);
    for (i = 0; i < oChildNodes.length; i++) {
        if (oChildNodes[i].id == id) {
            return true;
        }
    }
    return false;
}

function ValidateInput() {
    if ($('#txtDesignationName').val() == null || $('#txtDesignationName').val() == "") {
        alert("Please enter Designation name!");
        $('#txtDesignationName').focus();
        return false;
    }
    return true;
}

function RefreshObject() {
    debugger;
    var oDesignation = {
        DesignationID: _oDesignation.DesignationID,
        Code: _oDesignation.Code,
        Name: $('#txtDesignationName').val(),
        NameInBangla: $('#txtDesignationNameInBangla').val(),
        HRResponsibilityID: parseInt($('#txtResponsibility').data("HRRID")),
        Responsibility: $('#txtResponsibility').val(), 
        ResponsibilityInBangla: $('#txtResponsibility').data("DescriptionInBangla"),
        Description: $('#txtDescription').val(),
        ParentID: _oDesignation.ParentID,
        Sequence: 0,//$('#txtSequence').val(),
        RequiredPerson: 1,//$('#txtRequiredPerson').val(), 
        IsActive: true
    };
    return oDesignation;
}

function RefreshDesignationList(oTDesignation) {
    data = [oTDesignation];
    data = { "total": "" + data.length + "", "rows": data };
    $('#tblDesignations').treegrid('loadData', data);
}

