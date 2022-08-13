var _oDepartment = { DepartmentID: 0, ParentID: 0 };
var _oEmployees = [];
var _bIsAddDepartment = false;
var _sRemovedids = '';
function InitializeDepartmentsEvents() {
    RefreshDepartmentList(_oTDepartment);

    $("#btnNewDepartment").click(function () {
        var oDepartment = $('#tblDepartments').datagrid('getSelected');
        if (oDepartment == null || oDepartment.id <= 0) {
            alert("Please select a item from list!");
            return;
        }
        var SelectedRowIndex = $('#tblDepartments').datagrid('getRowIndex', oDepartment);
        $("#winDepartment").icsWindow("open", "New Department");
        $("#txtDepartmentName").focus();
        _oDepartment.ParentID = oDepartment.id;
        _oDepartment.DepartmentID = 0;
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oDepartment,
            ControllerName: "Department",
            ActionName: "getchildren",
            IsWinClose: false
        };
        $.icsDataGets(obj, function (response) {
            debugger
            if (response.status && response.objs.length > 0) {
                debugger
                DynamicRefreshList(response.objs, "tblDepartment");
            }
        });
        ResetAllFields("winDepartment");
        _bIsAddDepartment = true;
    });

    $("#AddDepartment").click(function () {
        if (!ValidateInput()) return;
        var oDepartment = RefreshObject();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oDepartment,
            ObjectId: oDepartment.DepartmentID,
            ControllerName: "Department",
            ActionName: "Save",
            TableId: "tblDepartment",
            IsWinClose: false
        };
        $.icsSave(obj);
        $("#txtCode").val("");
        $("#txtDepartmentName").val("");
        $("#txtDepartmentNameInBangla").val("");
        $("#txtDescription").val("");
        $("#txtDepartmentName").focus();
    });

    $("#RemoveDepartment").click(function () {
        var oDepartment = $('#tblDepartment').datagrid('getSelected');
        if (oDepartment == null || oDepartment.DepartmentID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (oDepartment.ParentID == 0) {
            alert("Root Department is not deletable item");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        //var SelectedRowIndex = $('#tblDepartment').datagrid('getRowIndex', oDepartment);

        if (oDepartment.DepartmentID > 0) {

            debugger
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oDepartment,
                ControllerName: "Department",
                ActionName: "Delete",
                TableId: "tblDepartment",
                IsWinClose: false
            };
            $.icsDelete(obj);
            _sRemovedids = _sRemovedids + oDepartment.DepartmentID + ',';
        }
    });

    $("#btnClose").click(function () {
        debugger
        if (_sRemovedids.length > 0) {
            _sRemovedids = _sRemovedids.substring(0, _sRemovedids.length - 1);
        }
        oDepartments = $('#tblDepartment').datagrid('getRows');
        if (oDepartments != null) {
            if (_sRemovedids != "") {
                var RemovedIds = _sRemovedids.split(",");
                if (RemovedIds.length > 0) {
                    for (i = 0; i < RemovedIds.length; i++) {
                        $('#tblDepartments').treegrid('remove', RemovedIds[i]);
                    }
                }
            }
            var newDepartments = [];
            newDepartments=oDepartments;
           debugger
            if (newDepartments != null) {
                if (newDepartments.length > 0) {
                  
                    for ( var i = 0; i < newDepartments.length; i++) {
                        if (IsExists(_oDepartment.ParentID, newDepartments[i].DepartmentID)==false) {
                            console.log(newDepartments[i]);
                            var oTDepartment = {
                                id: newDepartments[i].DepartmentID,
                                text: newDepartments[i].Name,
                                state: '',
                                attributes: '',
                                parentid: newDepartments[i].ParentID,
                                code: newDepartments[i].Code,
                                sequence: newDepartments[i].Sequence,
                                requiredPerson: newDepartments[i].RequiredPerson,
                                isActive: newDepartments[i].IsActive,
                                Description: newDepartments[i].Description
                            };
                           
                            $('#tblDepartments').treegrid('append', { parent: _oDepartment.ParentID, data: [oTDepartment] });
                        }
                    }
                    $('#tblDepartments').treegrid('select', _oDepartment.ParentID);
                    $('#tblDepartments').treegrid('expand', _oDepartment.ParentID);
                }
            }
        }
        $("#winDepartment").icsWindow('close');
    });

    $("#btnEditDepartment").click(function () {
        debugger
        var oDep= $('#tblDepartments').datagrid('getSelected');
        if (oDep == null || oDep.id <= 0)
        {
            alert("Please select a item from list!");
            return;
        }
        if(oDep.parentid==0)
        {
            alert("You can not edit root Department");
            return;
        }
        var oDepartment = { DepartmentID: oDep.id }
       
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oDepartment,
            ControllerName: "Department",
            ActionName: "getDepartment",
            IsWinClose: false
        };
        
        $.icsDataGet(obj, function (response) {
           
            if (response.status && response.obj.DepartmentID > 0) {
                debugger
                _oDepartment = response.obj;
                $("#txtEditCode").val(_oDepartment.Code);
                $("#txtEditDepartmentName").val(_oDepartment.Name);
                $("#txtEditDepartmentNameInBangla").val(_oDepartment.NameInBangla);
                $("#txtEditDescription").val(_oDepartment.Description);
            }
        });
        $("#winEditDepartment").icsWindow('open');

    });

    $("#btnEdit").click(function () {
        if ($('#txtEditDepartmentName').val() == null || $('#txtEditDepartmentName').val() == "") {
            alert("Please enter Department name!");
            $('#txtDepartmentName').focus();
            return;
        }
        
        var oDepartment = {
            DepartmentID: _oDepartment.DepartmentID,
            Code: _oDepartment.Code,
            Name: $('#txtEditDepartmentName').val(),
            NameInBangla: $('#txtEditDepartmentNameInBangla').val(),
            Description: $('#txtEditDescription').val(),
            ParentID: _oDepartment.ParentID,
            Sequence: 0,
            RequiredPerson: 1,
            IsActive: true
        };
       
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oDepartment,
            ObjectId: oDepartment.DepartmentID,
            ControllerName: "Department",
            ActionName: "Save",
            TableId: "",
            IsWinClose: true
        };
    
        $.icsSave(obj, function (response) {
            if (response.status && response.obj.DepartmentID > 0) {
                var oDepartment = response.obj;
                if(oDepartment.DepartmentID>0)
                {
                    var oTDepartment={
                        id : oDepartment.DepartmentID,
                        text : oDepartment.Name,
                        state : '',
                        attributes : '',
                        parentid : oDepartment.ParentID,
                        sequence:oDepartment.Sequence,
                        requiredPerson : oDepartment.RequiredPerson,
                        code : oDepartment.Code,
                        isActive:oDepartment.IsActive,
                        Description : oDepartment.Description
                    };
                    $('#tblDepartments').treegrid('update',{	id: oDepartment.DepartmentID, row: oTDepartment });
                }
            }
        });
    });

    $("#RemoveDepartmentFromCollection").click(function () {
        var oDep = $('#tblDepartments').datagrid('getSelected');
        if (oDep == null || oDep.id <= 0) {
            alert("Please select a item from list!");
            return;
        }
      
        if (oDep.parentid == 0) {
            alert("Root Department is not deletable item");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblDepartments').datagrid('getRowIndex', oDep);
        var oDepartment = { DepartmentID: oDep.id, ParentID: oDep.parentid }
        if (oDepartment.DepartmentID > 0) {

            debugger
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oDepartment,
                ControllerName: "Department",
                ActionName: "Delete",
                TableId: "",
                IsWinClose: false
            };
            //$.icsDelete(obj);
            //$('#tblDepartments').treegrid('remove', oDepartment.DepartmentID);
            $.icsDelete(obj, function (response) {
                if (response.status && response.Message == 'deleted') {
                    debugger;
                    $('#tblDepartments').treegrid('remove', oDepartment.DepartmentID);

                    $('#tblDepartments').treegrid('select', oDepartment.ParentID);;
                    var oParent = $('#tblDepartments').datagrid('getSelected');
                    if (oParent != null) {
                        var oChildrens = [];
                        for (var i = 0; i < oParent.children.length; i++) {

                            if (oParent.children[i].DepartmentID != oDepartment.DepartmentID) {
                                oChildrens.push(oParent.children[i]);
                            }
                        }
                        oParent.children = oChildrens;
                        $('#tblDepartments').treegrid('update', { id: oParent.DepartmentID, row: oParent });
                    }
                }
            })
        }
    });

    $('#txtSearchByCode').keypress(function (e) {
        debugger;
        var c = String.fromCharCode(e.which);
        var txtSearchByCode = $("#txtSearchByCode").val();
        txtSearchByCode = txtSearchByCode + c;

        var bFlag = false;
        var sAccountHeadCode = "";
        var rows = $('#tblDepartments').treegrid('getChildren', 1);
        for (i = 0; i < rows.length; ++i) {
            sAccountHeadCode = rows[i]['code'].substring(0, txtSearchByCode.length);
            if (txtSearchByCode.toUpperCase() == sAccountHeadCode.toUpperCase()) {
                var id = rows[i]['id'];
                $('#tblDepartments').treegrid('select', id);
                break;
            }
        }
    });

    $('#txtSearchByName').keypress(function (e) {
        //debugger;
        var c = String.fromCharCode(e.which);
        var txtSearchByName =  $("#txtSearchByName").val();
        txtSearchByName = txtSearchByName + c;

        var bFlag = false;
        var sAccountHeadName = "";
        var rows = $('#tblDepartments').treegrid('getChildren', 1);
        for (i = 0; i < rows.length; ++i) {
            sAccountHeadName = rows[i]['text'].substring(0, txtSearchByName.length);
            if (txtSearchByName.toUpperCase() == sAccountHeadName.toUpperCase()) {
                var id = rows[i]['id'];
                $('#tblDepartments').treegrid('select', id);
                break;
            }
        }
    });

    $("#btnEditDepartmentClose").click(function () {
        $("#winEditDepartment").icsWindow('close');
    });
}

function IsExists(nparentid, id) {
    debugger;
    //console.log(nparentid);
    //console.log(id);
    var oChildNodes = $('#tblDepartments').treegrid('getChildren', nparentid);
    //console.log(oChildNodes);
    for (i = 0; i < oChildNodes.length; i++) {
        if (oChildNodes[i].id == id) {
            return true;
        }
    }
    return false;
}

function ValidateInput() {
    if ($('#txtDepartmentName').val() == null || $('#txtDepartmentName').val() == "") {
        alert("Please enter Department name!");
        $('#txtDepartmentName').focus();
        return false;
    }
    return true;
}

function RefreshObject() {
    debugger;
    var oDepartment = {
        DepartmentID: _oDepartment.DepartmentID,
        Code: _oDepartment.Code,
        Name: $('#txtDepartmentName').val(),
        NameInBangla: $('#txtDepartmentNameInBangla').val(),
        Description: $('#txtDescription').val(),
        ParentID: _oDepartment.ParentID,
        Sequence: 0,//$('#txtSequence').val(),
        RequiredPerson: 1,//$('#txtRequiredPerson').val(), 
        IsActive: true
    };
    return oDepartment;
}

function RefreshDepartmentList(oTDepartment) {
    data = [oTDepartment];
    data = { "total": "" + data.length + "", "rows": data };
    $('#tblDepartments').treegrid('loadData', data);
}

