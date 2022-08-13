var _sCCIDs='';

function InitializeSUVehiclesEvents() {

    $('#txtVehicleNoSUVehicles').keyup(function (e) {
        var obj =
        {
            Event: e,
            SearchProperty: "VehicleNo",
            GlobalObjectList: _oDBSUVehicles,
            TableId: "tblSUVehicles"
        };
        $('#txtVehicleNoSUVehicles').icsSearchByText(obj);
    });

    $("#btnNewSUVehicles").click(function () {
        debugger;
        _oSUVehicle = { };
        ResetAllFields("winSUVehicle");


        RefreshControlSUVehicle(_oSUVehicle);
        $("#winSUVehicle").icsWindow("open", "New Vehicle");
        RefreshLayoutSUVehicle("btnNewSUVehicles"); //button id as parameter
    });

    $("#btnEditSUVehicles").click(function () {
        debugger;
        var oSUVehicle = $("#tblSUVehicles").datagrid("getSelected");
        if (oSUVehicle == null || oSUVehicle.SUVehicleID <= 0) { alert("Please select an item from list!"); return; }

        _oSUVehicle = oSUVehicle;
        RefreshControlSUVehicle(_oSUVehicle);
        $("#winSUVehicle").icsWindow('open', "Edit Vehicle");
        RefreshLayoutSUVehicle("btnEditSUVehicles");

    });

    $("#btnViewSUVehicles").click(function () {
        debugger;
        var oSUVehicle = $("#tblSUVehicles").datagrid("getSelected");
        if (oSUVehicle == null || oSUVehicle.SUVehicleID <= 0) { alert("Please select an item from list!"); return; }

        _oSUVehicle = oSUVehicle;
        RefreshControlSUVehicle(_oSUVehicle);
        $("#winSUVehicle").icsWindow('open', "View Vehicle");
        RefreshLayoutSUVehicle("btnViewSUVehicles");

    });

    $("#btnDeleteSUVehicles").click(function () {
        debugger;
        var oSUVehicle = $("#tblSUVehicles").datagrid("getSelected");
        if (oSUVehicle == null || oSUVehicle.SUVehicleID <= 0) { alert("Please select an item from list!"); return; }

        if (!confirm("Confirm to Delete?")) return false;
        
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oSUVehicle,
            ObjectId: oSUVehicle.SUVehicleID,
            ControllerName: "SUVehicle",
            ActionName: "Delete",
            TableId: "tblSUVehicles",
            IsWinClose: false
        };
        $.icsDelete(obj);
        
    });

    

    $("#btnPrintList").click(function () {
        debugger;
        if (!_oDBSUVehicles.length || !$.trim($('#txtCollectionPrintText').val()).length) return false;
        var oDBSUVehicles = $("#tblSUVehicles").datagrid('getRows');
        $('#txtCollectionPrintText').val(JSON.stringify(oDBSUVehicles));
    });
    

    GenerateTableColumnsSUVehicle();
    
    //debugger;
    DynamicRefreshList(_oDBSUVehicles, 'tblSUVehicles');
        
    $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUVehicles));

 
    
    
}


function GenerateTableColumnsSUVehicle() {
    _otblColumns = [];


    
    oColVehicleNo = { field: "VehicleNo", width: "25%", title: "Vehicle No", frozen: true };
    oColDriverName = { field: "DriverName", width: "20%", title: "Driver", frozen: true };
    oColDriverContactNo = { field: "DriverContactNo", width: "20%", title: "Contact No", frozen: true };
    oColRemarks = { field: "Remarks", width: "35%", title: "Remarks", frozen: true };
    //_otblColumns.push(oColumn);


    _otblColumns.push(oColVehicleNo, oColDriverName, oColDriverContactNo, oColRemarks);

    $('#tblSUVehicles').datagrid({ columns: [_otblColumns] });
    
}









