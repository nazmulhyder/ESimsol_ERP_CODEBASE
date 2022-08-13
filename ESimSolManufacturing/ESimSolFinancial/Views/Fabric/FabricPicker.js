var _oFabricsPicker = [];

function InitializeFabricPickerByNameEvents() {
    $("#txtSearchByFabricNamePicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "FabricNo",
            GlobalObjectList: _oFabricsPicker,
            TableId: "tblFabricPickerByName"
        };
        $('#txtSearchByFabricNamePicker').icsSearchByText(obj);
    });

    $("#btnCloseFabricPickerByName").click(function () {
        $("#winFabricPickerByName").icsWindow("close");
        _oFabricsPicker = [];
        $("#winFabricPickerByName input").val("");
        DynamicRefreshList([], "tblFabricPickerByName");
    });
}

function IntitializeFabricSearchByTextPicker() {
    DynamicRefreshList([], "tblFabricPickerByName");
}