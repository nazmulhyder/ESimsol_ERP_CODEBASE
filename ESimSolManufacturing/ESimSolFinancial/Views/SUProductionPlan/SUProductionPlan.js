
var _oMachines = [];

function InitializeSUProductionPlanEvents() {
    
    

    $("#btnSaveSUProductionPlan").click(function () {
        if (!ValidateInputSUProductionPlan()) return;
        var oSUProductionPlan = RefreshObjectSUProductionPlan();
        var nRowIndex = $("#tblSUProductionPlans").datagrid("getRowIndex", oSUProductionPlan);
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSUProductionPlan,
            ObjectId: oSUProductionPlan.SUProductionPlanID,
            ControllerName: "SUProductionPlan",
            ActionName: "Save",
            TableId: "tblSUProductionPlans",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if(response.status)
            {
                _oDBSUProductionPlans = [];
                _oDBSUProductionPlans = $("#tblSUProductionPlans").datagrid('getRows');
                var oSUProductionPlan = response.obj;
                $("#tblSUProductionPlans").datagrid("updateRow", { index: nRowIndex, row: oSUProductionPlan });

                $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUProductionPlans));

                $("#txtProductSUProductionPlan").val('');
                $("#cboMachineQty").empty();
                $("#txtCapacityPerDay").val('');
                $("#txtAvailableMachineQty").val('');
                $("#txtRemarks").val('');
                $('#cboMeasurementUnitTypes').empty();
                $("#txtAvailableMachineQty").val('');
                _nAvailableMachines = 0;
                _oProduct = {};
                _oMachines = [];
            }
        });
    
    });
    $("#btnCloseSUProductionPlan").click(function () {
        $("#txtProductSUProductionPlan").val('');
        $("#cboMachineQty").empty();
        $("#txtCapacityPerDay").val('');
        $("#txtAvailableMachineQty").val('');
        $("#txtRemarks").val('');
        $('#cboMeasurementUnitTypes').empty();
        $("#txtAvailableMachineQty").val('');
        _nAvailableMachines = 0;
        _oProduct = {};
        _oMachines = [];
        

        $("#winSUProductionPlan").icsWindow('close');
       
    });


    $("#txtProductSUProductionPlan").keydown(function (e) {
       // 
        if (e.keyCode === 13) // Enter Press
        {
            PickProductSUProductionPlan();
        }
        else if (e.keyCode === 08 || e.keyCode === 46) {
            $("#txtProductSUProductionPlan").removeClass("fontColorOfPickItem");
            _oProduct = {};
            $('#cboMeasurementUnitTypes').empty();
        }
    });
    $("#btnPicProductSUProductionPlan").click(function () {
        PickProductSUProductionPlan();
    });
    $("#btnClrProductSUProductionPlan").click(function () {
        $("#txtProductSUProductionPlan").removeClass("fontColorOfPickItem");
        $("#txtProductSUProductionPlan").val("");
        $("#txtAvailableMachineQty").val('');
        _nAvailableMachines = 0;
        _oProduct = {};
        $('#cboMeasurementUnitTypes').empty();
    });
    $("#winProductPickerByName").on("keydown", function (e) {
        var oProduct = $('#tblProductPicker').datagrid('getSelected');
        var nIndex = $('#tblProductPicker').datagrid('getRowIndex', oProduct);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblProductPicker').datagrid('selectRow', 0);
            }
            else {
                $('#tblProductPicker').datagrid('selectRow', nIndex - 1);
            }
            
        }
        if (e.which == 40)//down arrow=40
        {
            var oCurrentList = $('#tblProductPicker').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblProductPicker').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblProductPicker').datagrid('selectRow', nIndex + 1);
            }
            
        }
        if (e.which == 13)//enter=13
        {
            
            GetAvailableMachineCount();
        }
    });
    $("#btnOkProductPickerByName").click(function () {
        
        GetAvailableMachineCount();
    });

    $('#txtCapacityPerDay').icsNumberField({
        min: 0,
        max: null,
        precision: 0
    });
}

function GetAvailableMachineCount() {
    var oProduct = $("#tblProductPicker").icsGetSelectedItem();
    if (oProduct != null && oProduct.ProductID > 0) {
        $("#txtProductSUProductionPlan").val(oProduct.ShortName);
        $('#txtProductSUProductionPlan').css("border", "");
        $("#txtProductSUProductionPlan").addClass("fontColorOfPickItem");
        _oProduct = oProduct;
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUProductionPlan/GetAvaibaleMachineCount",
            traditional: true,
            data: JSON.stringify(_oProduct),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                
                _nAvailableMachines = parseInt(data);
                $("#cboMachineQty").empty();
                if (_nAvailableMachines > -1) {
                    $("#txtAvailableMachineQty").val('Available : ' + _nAvailableMachines);

                    for (var i = 0; i < _nAvailableMachines; i++) {
                        _oMachines.push({ id: i + 1, value: i + 1 });
                    }
                    $("#cboMachineQty").icsLoadCombo({
                        List: _oMachines,
                        OptionValue: "id",
                        DisplayText: "value",
                        InitialValue: "Deafult"
                    });

                }
                else {
                    $("#txtAvailableMachineQty").val('Available : 0');
                    $("#cboMachineQty").icsLoadCombo({
                        List: _oMachines,
                        OptionValue: "id",
                        DisplayText: "value",
                        InitialValue: "Deafult"
                    });
                }
                $('#cboMeasurementUnitTypes').empty();
                var listItems;
                for (var i = 0; i < _oMUs.length; i++) {
                    if (_oMUs[i].UnitType == _oProduct.MeasurementUnitType) {
                        listItems += "<option value='" + _oMUs[i].MeasurementUnitID + "'>" + _oMUs[i].Symbol + "</option>";
                    }
                }
                $('#cboMeasurementUnitTypes').html(listItems);
                $('#cboMeasurementUnitTypes').val(_oProduct.MeasurementUnitID);
            }
        });
    }
}
function PickProductSUProductionPlan() {
    
    if ($.trim($("#txtProductSUProductionPlan").val()).length === 0) {
        alert("Please enter some text.");
        return false;
    }
    var sDBObjectName = "SUProductionPlan";
    var nTriggerParentsType = 109;
    var nOperationalEvent = 704;
    var nInOutType = 100;
    var nDirections = 0;
    var nStoreID = 0;
    var nMapStoreID = 0;

    var sProductName = $("#txtProductSUProductionPlan").val();
    var oProduct = {
        Params: $.trim(sProductName) + '~' + sDBObjectName + '~' + nTriggerParentsType + '~' + nOperationalEvent + '~' + nInOutType + '~' + nDirections + '~' + nStoreID + '~' + nMapStoreID
    };
    IntitializeProductSearchingPicker();
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oProduct,
        ControllerName: "Product",
        ActionName: "ATMLNewSearchByProductName",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ProductID > 0) {
                _oProductsPicker = response.objs;
                DynamicRefreshList(response.objs, "tblProductPicker");
                $("#winProductPickerByName").icsWindow('open', "Product Search");
                $("#divProductPickerByName").focus();
                $("#winProductPickerByName input").val("");
            }
            else {
                _oProductsPicker = [];
                DynamicRefreshList([], "tblProductPicker");
                alert(response.objs[0].Errormessage);
                $("#txtProductSUProductionPlan").removeClass("fontColorOfPickItem");
                _nProductId = 0;
            }
        }
        else {
            alert("Sorry, No data found, Try again.");
            $("#txtProductSUProductionPlan").removeClass("fontColorOfPickItem");
            _nProductId = 0;
        }
    });
}

function RefreshControlSUProductionPlan(oSUProductionPlan) {
    
    $('#txtProductSUProductionPlan').val('');
    $("#txtProductSUProductionPlan").removeClass("fontColorOfPickItem");
    $("#txtProductSUProductionPlan").val(oSUProductionPlan.ProductShortName);
    if (oSUProductionPlan.ProductShortName != null && oSUProductionPlan.ProductShortName != "") {
        $("#txtProductSUProductionPlan").addClass("fontColorOfPickItem");
    }
    
    $("#txtCapacityPerDay").val(oSUProductionPlan.CapacityPerDay);

    $("#txtRemarks").val(oSUProductionPlan.Remarks);
    $("#txtAvailableMachineQty").val('');
    $("#cboMachineQty").empty();

    

    _oProduct = { ProductID: oSUProductionPlan.ProductID, MeasurementUnitType: oSUProductionPlan.MeasurementUnitType };
    if (_oProduct.ProductID != null) {
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUProductionPlan/GetAvaibaleMachineCount",
            traditional: true,
            data: JSON.stringify(_oProduct),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                
                _nAvailableMachines = parseInt(data);
                if (_nAvailableMachines > -1) {

                    $("#txtAvailableMachineQty").val('Available : ' + _nAvailableMachines);

                    for (var i = 0; i < _nAvailableMachines; i++) {
                        _oMachines.push({ id: i + 1, value: i + 1 });
                    }
                    $("#cboMachineQty").icsLoadCombo({
                        List: _oMachines,
                        OptionValue: "id",
                        DisplayText: "value",
                        InitialValue: "Deafult"
                    });
                    $("#cboMachineQty").val(oSUProductionPlan.MachineQty);
                }
                else {
                    $("#txtAvailableMachineQty").val('Available : 0');
                    $("#cboMachineQty").icsLoadCombo({
                        List: _oMachines,
                        OptionValue: "id",
                        DisplayText: "value",
                        InitialValue: "Deafult"
                    });
                }

                $('#cboMeasurementUnitTypes').empty();
                var listItems;
                for (var i = 0; i < _oMUs.length; i++) {
                    if (_oMUs[i].UnitType == _oProduct.MeasurementUnitType) {
                        listItems += "<option value='" + _oMUs[i].MeasurementUnitID + "'>" + _oMUs[i].Symbol + "</option>";
                    }
                }
                $('#cboMeasurementUnitTypes').html(listItems);
                $('#cboMeasurementUnitTypes').val(_oProduct.MeasurementUnitID);
            }
        });
    }

    
}
function RefreshLayoutSUProductionPlan(buttonId) {
    if (buttonId === "btnViewSUProductionPlan") {
        $("#winSUProductionPlan input").prop("disabled", true);
        $("#winSUProductionPlan select").prop("disabled", true);
        $("#btnSaveSUProductionPlan").hide();
    }
    else {
        $("#winSUProductionPlan input").not("#txtAvailableMachineQty").prop("disabled", false);
        $("#winSUProductionPlan select").prop("disabled", false);
        $("#btnSaveSUProductionPlan").show();
    }
}

function RefreshObjectSUProductionPlan() {
    
    var nSUProductionPlanId = (_oSUProductionPlan == null ? 0 : (_oSUProductionPlan.SUProductionPlanID == null) ? 0 : _oSUProductionPlan.SUProductionPlanID);
    var oSUProductionPlan = {
        SUProductionPlanID: nSUProductionPlanId,
        ProductID: _oProduct.ProductID,
        ProductShortName:$.trim($("#txtProductSUProductionPlan").val()),
        MachineQty: $.trim($("#cboMachineQty").val()),
        CapacityPerDay: $.trim($("#txtCapacityPerDay").val()),
        MUnitID: $('#cboMeasurementUnitTypes').val(),
        MeasurementUnitType:_oProduct.MeasurementUnitType,
        Remarks: $.trim($("#txtRemarks").val()),
        IsActive: _oSUProductionPlan.IsActive
    };
    return oSUProductionPlan;
}

function ValidateInputSUProductionPlan() {

    if (_oProduct.ProductID == null || _oProduct.ProductID <= 0)
    {
        alert("Please Select a Product!");
        $('#txtProductSUProductionPlan').val("");
        $('#txtProductSUProductionPlan').focus();
        $('#txtProductSUProductionPlan').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtProductSUProductionPlan').css("border", "");
    }

    if (parseInt($.trim($("#cboMachineQty").val()))<=0) {
        alert("Please Enter Number of Machines Required for this Plan!");
        $('#cboMachineQty').val(0);
        $('#cboMachineQty').focus();
        $('#cboMachineQty').css("border", "1px solid #c00");
        return false;
    } else {
        $('#cboMachineQty').css("border", "");
    }
    var nMachineQty = $.trim($("#cboMachineQty").val());
    if (nMachineQty > _nAvailableMachines)
    {
        alert(nMachineQty+" Machine(s) not Available.\n Please Enter Number of Machines Required for this Plan!");
        $('#cboMachineQty').val(0);
        $('#cboMachineQty').focus();
        $('#cboMachineQty').css("border", "1px solid #c00");
        return false;
    } else {
        $('#cboMachineQty').css("border", "");
    }

    if (!$.trim($("#txtCapacityPerDay").val()).length) {
        alert("Please Enter Production Capacity of this Plan!");
        $('#txtCapacityPerDay').val("");
        $('#txtCapacityPerDay').focus();
        $('#txtCapacityPerDay').css("border", "1px solid #c00");
        return false;
    } else {
        $('#txtCapacityPerDay').css("border", "");
    }
    if (!$.trim($("#cboMeasurementUnitTypes").val()).length) {
        alert("Please Select a Measurement Unit!");
        $('#cboMeasurementUnitTypes').val("");
        $('#cboMeasurementUnitTypes').focus();
        $('#cboMeasurementUnitTypes').css("border", "1px solid #c00");
        return false;
    } else {
        $('#cboMeasurementUnitTypes').css("border", "");
    }

   
    return true;
}


