var _oEmployeesPicker = [];

function InitializeEmployeePickerByNameEvents() {
    $("#txtSearchByEmployeeNamePicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oEmployeesPicker,
            TableId: "tblEmployeePickerByName"
        };
        $('#txtSearchByEmployeeNamePicker').icsSearchByText(obj);
    });

    $("#btnCloseEmployeePickerByName").click(function () {
        $("#winEmployeePickerByName").icsWindow("close");
        _oEmployeesPicker = [];
        $("#winEmployeePickerByName input").val("");
        DynamicRefreshList([], "tblEmployeePickerByName");
    });
}

function IntitializeEmployeeSearchByTextPicker() {
    DynamicRefreshList([], "tblEmployeePickerByName");
}