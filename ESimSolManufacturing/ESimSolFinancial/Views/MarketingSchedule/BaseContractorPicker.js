var _oContractorsPicker = [];

function InitializeBaseContractorPickerEvents()
{
    
    $("#txtSearchByBaseContractorNamePicker").keydown(function (e) {
        
            var obj = {
                Event: e,
                SearchProperty: "Name",
                GlobalObjectList: _oContractorsPicker,
                TableId: "tblBaseContractorsPicker"
            };
            $('#txtSearchByBaseContractorNamePicker').icsSearchByText(obj);
        
    });
    
    $("#btnBaseCloseContractorPicker").click(function () {
        $("#winBaseContractorPicker").icsWindow("close");
        _oContractorsPicker = [];
        $("#winBaseContractorPicker input").val("");
        DynamicRefreshList([], "tblBaseContractorsPicker");
    });
}

function IntitializeContractorSearchByTextPicker()
{
    DynamicRefreshList([], "tblBaseContractorsPicker");
}