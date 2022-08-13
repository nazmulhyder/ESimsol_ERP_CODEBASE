var _oEmployeesPicker = [];

function InitializeBaseEmployeePickerByNameEvents() {
    $("#txtSearchByBaseEmployeeNamePicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oEmployeesPicker,
            TableId: "tblBaseEmployeePickerByName"
        };
        $('#txtSearchByBaseEmployeeNamePicker').icsSearchByText(obj);
    });

    $("#btnBaseCloseEmployeePickerByName").click(function () {
        $("#winBaseEmployeePickerByName").icsWindow("close");
        _oEmployeesPicker = [];
        $("#winBaseEmployeePickerByName input").val("");
        DynamicRefreshList([], "tblBaseEmployeePickerByName");
    });
}

function IntitializeEmployeeSearchByTextPicker() {
    DynamicRefreshList([], "tblBaseEmployeePickerByName");
}