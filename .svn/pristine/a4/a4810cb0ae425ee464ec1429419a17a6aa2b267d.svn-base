debugger;
function RefreshMUnitList(oMUnits) {
    var data = oMUnits;
    data = { "total": "" + data.length + "", "rows": data };
    $("#tblMeasurementUnits").datagrid("loadData", data);
}

function LoadMeasurementUnitsEvents() {


    $("#btnAddMUnit").click(function () {
        debugger;
        
        $("#winMeasurementUnit").icsWindow({ Property: 'open' });

        $("#winMeasurementUnit").window("setTitle", "Add Information");
        $("#winMeasurementUnit .bodyfieldsetstyle").find("legend").text("Add Measurement Unit");
        $(".wininputfieldstyle input").val("");
        $("#radioBtnIsRoundNo").prop("checked", true);
        $("#radioBtnIsRoundYes").prop("checked", false);
        _oMUnit = null;
        

        RefreshMeasurementUnitLayout("btnAddMUnit"); //button id as parameter
    });

    $("#btnEditMUnit").click(function () {
        var oMUnit = $("#tblMeasurementUnits").datagrid("getSelected");
        if (oMUnit == null || oMUnit.MeasurementUnitID <= 0) {
            alert("Please select an item from list!");
            return;
        }
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MeasurementUnit/Get",
            traditional: true,
            data: JSON.stringify(oMUnit),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oMUnit = jQuery.parseJSON(data);
                if (oMUnit.MeasurementUnitID > 0) {
                    _oMUnit = oMUnit;
                    RefreshMeasurementUnitLayout("btnEditMUnit");
                    $("#winMeasurementUnit").window("setTitle", "Edit Information");
                    $("#winMeasurementUnit .bodyfieldsetstyle").find("legend").text("Edit MeasurementUnit");
                    RefreshMUnitControl(oMUnit);
                    $("#winMeasurementUnit").window("open");
                }
            }
        });
    });

    $("#btnViewMUnit").click(function () {
        var oMUnit = $("#tblMeasurementUnits").datagrid("getSelected");
        if (oMUnit == null || oMUnit.MeasurementUnitID <= 0) {
            alert("Please select an item from list!");
            return;
        }
        RefreshMeasurementUnitLayout("btnViewMUnit");
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/MeasurementUnit/Get",
            traditional: true,
            data: JSON.stringify(oMUnit),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oMUnit = jQuery.parseJSON(data);
                if (oMUnit.MeasurementUnitID > 0) {
                    $("#winMeasurementUnit").window("setTitle", "View Information");
                    $("#winMeasurementUnit .bodyfieldsetstyle").find("legend").text("View MeasurementUnit");
                    RefreshMUnitControl(oMUnit);
                    $("#winMeasurementUnit").window("open");
                }
            }
        });
    });

    $("#btnDeleteMUnit").click(function () {
        var oMUnit = $("#tblMeasurementUnits").datagrid("getSelected");
        if (!confirm("Confirm to Delete?")) return false;
        if (oMUnit == null || oMUnit.MeasurementUnitID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }

        $("#tblMeasurementUnits").icsDelete({
            BaseAddress: _sBaseAddress,
            Object: oMUnit,
            ControllerName: "MeasurementUnit",
            ActionName: "Delete"
        });


    });

}

function RefreshMeasurementUnitLayout(buttonId) {
    if (buttonId === "btnViewMUnit") {
        $("#winMeasurementUnit input").prop("disabled", true);
        $("#btnSaveMeasurementUnit").hide();
    }
    else
    {
        $("#winMeasurementUnit input").prop("disabled", false);
        $("#btnSaveMeasurementUnit").show();
    }
    //$(".disabled input").prop("disabled", true);
    //$(".number").icsNumberField();
}
