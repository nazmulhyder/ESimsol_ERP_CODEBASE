var _sCCIDs='';

function InitializeSUMachinesEvents() {

    $('#txtMachineNameSUMachine').keyup(function (e) {
        var obj =
        {
            Event: e,
            SearchProperty: "MachineName",
            GlobalObjectList: _oDBSUMachines,
            TableId: "tblSUMachines"
        };
        $('#txtMachineNameSUMachine').icsSearchByText(obj);
    });

    $("#btnNewSUMachine").click(function () {
        debugger;
        if (!ValidateInputSUMachine()) { return false; }
        //Set Data
        var oSUMachine = {
            CCIDs: _sCCIDs,
            Remarks: $.trim($("#txtRemarksSUMachine").val())
        };

        //Insert to db
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUMachine/Save",
            traditional: true,
            data: JSON.stringify(oSUMachine),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                //debugger;
                var oSUMachines = jQuery.parseJSON(data);
                if (oSUMachines.length > 0) {
                    if (oSUMachines[0].ErrorMessage == '') {
                        alert("Data Saved sucessfully.!!");
                        DynamicRefreshList(oSUMachines, 'tblSUMachines');
                    }
                    else {
                        alert(oSUMachines[0].ErrorMessage);
                    }
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }

        });
    });

    $("#btnPickSUMachine").click(function () {
        debugger;
        
        _bMultipleItemReturn = true;
        RefreshControlCostCenter();
        $("#winCostCenterSUMachine").icsWindow('open', "Capital Machines");
        
        
    });
    $("#btnOKCostCenter").click(function () {
        debugger;
        if (_bMultipleItemReturn == true) {
            var oCostCenters = [];
            var oCostCenters = $('#TreeCostCenter').tree('getChecked');
            if (oCostCenters.length <= 0) {
                alert("please select atleast one item.");
                return false;
            }
            _oCostCenters = oCostCenters;
            if (_oCostCenters.length > 0) {
                var sCCNames = "";
                var nCount = 0;
                _sCCIDs = "";
                for (i = 0; i < _oCostCenters.length; ++i) {
                    _sCCIDs = _sCCIDs + _oCostCenters[i].id + ',';
                    sCCNames = sCCNames + _oCostCenters[i].text + ',';
                    nCount++;
                }
                _sCCIDs = _sCCIDs.substring(0, _sCCIDs.length - 1);
                sCCNames = sCCNames.substring(0, sCCNames.length - 1);

                $('#txtMachineNameSUMachine').val(sCCNames + ' Selected');

            }
            else {
                var oCostCenter = $('#TreeCostCenter').tree('getSelected');
                if (oCostCenter == null) {
                    alert("please select a Project.");
                    return false;
                }
                _oCostCenter = oCostCenter;
                $('#txtMachineNameSUMachine').val('1 Machine Selected');
            }
            $("#winCostCenter").icsWindow('close');
        }
    });

    $("#btnDeleteSUMachine").click(function () {
        debugger;
        var oSUMachine = $("#tblSUMachines").datagrid("getSelected");
        if (oSUMachine == null || oSUMachine.SUMachineID <= 0) { alert("Please select an item from list!"); return; }

        if (!confirm("Confirm to Delete?")) return false;
        RowSelect();
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oSUMachine,
            ObjectId: oSUMachine.SUMachineID,
            ControllerName: "SUMachine",
            ActionName: "Delete",
            TableId: "tblSUMachines",
            IsWinClose: false
        };
        $.icsDelete(obj);
        
    });

    $("#btnActiveSUMachine").click(function () {
        debugger;
        var oSUMachine = $("#tblSUMachines").datagrid("getSelected");
        if (oSUMachine == null || oSUMachine.SUMachineID <= 0) { alert("Please select an item from list!"); return; }

        _oSUMachine = oSUMachine;
        var msg = _oSUMachine.IsActive ? "Confirm to In-Active?" : "Confirm to Active?";
        if (!confirm(msg)) return false;
        _oSUMachine.IsActive = !_oSUMachine.IsActive;
        var defaultSettings = {
            BaseAddress: _sBaseAddress,
            Object: _oSUMachine,
            ObjectId: _oSUMachine.SUMachineID,
            ControllerName: "SUMachine",
            ActionName: "ActiveInActive",
            TableId: "tblSUMachines",
            IsWinClose: false
        };
        $.icsSave(defaultSettings, function (response) {

            if (response.status) {
                var oSelectedObject = $("#" + defaultSettings.TableId).datagrid("getSelected");
                var nRowIndex = $("#" + defaultSettings.TableId).datagrid("getRowIndex", oSelectedObject);
                RowSelect(nRowIndex, oSelectedObject);
                
            }
        });

    });

    $("#btnPrintList").click(function () {
        debugger;
        if (!_oDBSUMachines.length || !$.trim($('#txtCollectionPrintText').val()).length) return false;
    });
    

    GenerateTableColumnsSUMachine();
    
    //debugger;
    DynamicRefreshList(_oDBSUMachines, 'tblSUMachines');
        
    $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUMachines));

 
    
    
}
function RowSelect(rowIndex, rowData)
{
    debugger;
    if (rowData == null || rowData.SUMachineID <= 0) { $('#btnActiveSUMachine').linkbutton({ text: 'Active/In-Active', iconCls: 'icon-ok', plain: 'true' }); return false; }

    _oSUMachine = rowData;
    _oSUMachine.IsActive ? $('#btnActiveSUMachine').linkbutton({ text: 'In-Active', iconCls: 'icon-ok', plain: 'true' }) : $('#btnActiveSUMachine').linkbutton({ text: 'Active', iconCls: 'icon-ok', plain: 'true' });
    
}

function GenerateTableColumnsSUMachine() {
    _otblColumns = [];


    
    oColMachineName = { field: "MachineName", width: "35%", title: "Machine Name", frozen: true };
    oColIsActiveInString = { field: "IsActiveInString", width: "10%", title: "Active", frozen: true };
    oColRemarks = { field: "Remarks", width: "35%", title: "Remarks", frozen: true };
    //_otblColumns.push(oColumn);


    _otblColumns.push(oColMachineName,oColIsActiveInString,oColRemarks);

    $('#tblSUMachines').datagrid({ columns: [_otblColumns] });
    $('#tblSUMachines').datagrid({ onSelect: function (rowIndex, rowData) { RowSelect(rowIndex, rowData); } });
}

function ValidateInputSUMachine() {
    if (_oCostCenters.length == null || _oCostCenters.length <= 0 || _oCostCenter == null || _oCostCenter.CCID <= 0) {
        alert("please select atleast one Cost Center.");
        return false;
    }
    return true;
}









