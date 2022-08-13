var _bIsAddHoliday = false;
var _oHoliday = { HolidayID: 0 };

function InitializeHolidaysEvents() {

    DynamicRefreshList(_oHolidays, "tblHolidays");
    EnableFields();
    $("#btnNewHoliday").click(function () {
        debugger
        $("#lblHeaderName").html("New Holiday");
        //$("#txtDescription").val("");
        $("#winHoliday").icsWindow("open", "New Holiday");
        EnableFields();
        ResetAllFields("winHoliday");
        _oHoliday.HolidayID = 0;
        _bIsAddHoliday = true;
    });

    $("#btnClose").click(function () {
        $("#winHoliday").icsWindow("close");
    });

    $("#btnSave").click(function () {
        debugger
        if (!ValidateInput()) return;
        var oHoliday = RefreshObject();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oHoliday,
            ObjectId: oHoliday.HolidayID,
            ControllerName: "Holiday",
            ActionName: "Save",
            TableId: "",
            IsWinClose: false
        };

        $.icsSave(obj, function (response) {
            if (response.status && response.obj.HolidayID > 0) {

                if (_bIsAddHoliday) {
                    var oHolidays = $('#tblHolidays').datagrid('getRows');
                    var nIndex = oHolidays.length;
                    $('#tblHolidays').datagrid('appendRow', response.obj);
                    $('#tblHolidays').datagrid('selectRow', nIndex);
                }
                else {
                    var oHoliday = $('#tblHolidays').datagrid('getSelected');
                    var SelectedRowIndex = $('#tblHolidays').datagrid('getRowIndex', oHoliday);
                    $('#tblHolidays').datagrid('updateRow', { index: SelectedRowIndex, row: response.obj });
                }
                $("#winHoliday").icsWindow("close");
            }
        });

        ResetAllFields("winHoliday");
    });

    $("#btnEdit").click(function () {
        debugger
        _bIsAddHoliday = false;
        $("#lblHeaderName").text("Edit Holiday");
        var oHoliday = $('#tblHolidays').datagrid('getSelected');
        if (oHoliday == null || oHoliday.HolidayID <= 0) {
            alert("Please select a item from list!");
            return;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oHoliday,
            ControllerName: "Holiday",
            ActionName: "GetHoliday",
            IsWinClose: false
        };
     
        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj.HolidayID > 0) {
                _oHoliday = response.obj;
                $('#txtDescription').val(oHoliday.Description);
                $('#txtCode').val(oHoliday.Code);
                document.getElementById("cboHolidayTypeOfHoliday").selectedIndex = oHoliday.TypeOfHoliday;
                //$("#chkIsActive").prop('checked', oHoliday.IsActive);
                //document.getElementById("chkIsActive").checked = oHoliday.IsActive;
                $('#cboDay').val(oHoliday.DayOfMonth.split('-')[0]);
                document.getElementById("cboMonth").options[document.getElementById("cboMonth").value].text = oHoliday.DayOfMonth.split('-')[1];
            }
        });
        $("#winHoliday").icsWindow("open", "Edit Holiday");
        EnableFields();
    });

    $("#btnView").click(function () {
        debugger
        _bIsAddHoliday = false;
        $("#lblHeaderName").text("View Holiday");
        var oHoliday = $('#tblHolidays').datagrid('getSelected');
        if (oHoliday == null || oHoliday.HolidayID <= 0) {
            alert("Please select a item from list!");
            return;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oHoliday,
            ControllerName: "Holiday",
            ActionName: "GetHoliday",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj.HolidayID > 0) {
                _oHoliday = response.obj;
                $('#txtDescription').val(oHoliday.Description);
                $('#txtCode').val(oHoliday.Code);
                document.getElementById("cboHolidayTypeOfHoliday").selectedIndex = oHoliday.TypeOfHoliday;
                //$("#chkIsActive").prop('checked', oHoliday.IsActive);
                $('#cboDay').val(oHoliday.DayOfMonth.split('-')[0]);
                document.getElementById("cboMonth").options[document.getElementById("cboMonth").value].text = oHoliday.DayOfMonth.split('-')[1];

                $("#txtDescription").prop("disabled", true);
                $("#txtCode").prop("disabled", true);
                $("#cboHolidayTypeOfHoliday").prop("disabled", true);
                $("#cboDay").prop("disabled", true);
                $("#cboMonth").prop("disabled", true);
                //$("#chkIsActive").prop("disabled", true);

            }
        });
        $("#winHoliday").icsWindow("open", "View Holiday");
    });

    $("#btnDelete").click(function () {
        debugger
        var oHD = $('#tblHolidays').datagrid('getSelected');
        if (oHD == null || oHD.HolidayID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblHolidays').datagrid('getRowIndex', oHD);

        if (oHD.HolidayID > 0) {
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oHD,
                ControllerName: "Holiday",
                ActionName: "Delete",
                TableId: "tblHolidays",
                IsWinClose: false
            };
            $.icsDelete(obj);
        }
    });

    $("#btnActivity").click(function () {
        var oHoliday = $('#tblHolidays').datagrid('getSelected');
        if (oHoliday == null || oHoliday.HolidayID <= 0) {
            alert("Please select a item from list!");
            return false;
        }
        if (oHoliday.IsActive == true) {
            if (!confirm("Confirm to In-Active?")) return;
            oHoliday.IsActive = false;
        }
        else {
            if (!confirm("Confirm to Active?")) return;
            oHoliday.IsActive = true;
        }
        var SelectedRowIndex = $('#tblHolidays').datagrid('getRowIndex', oHoliday);
        if (oHoliday.HolidayID > 0) {
            debugger;
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/Holiday/ChangeActiveStatus",
                traditional: true,
                data: JSON.stringify(oHoliday),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var oHoliday = jQuery.parseJSON(data);
                    debugger;
                    if (oHoliday != null) {
                        if (oHoliday.HolidayID > 0) {
                            alert("Data Save Succesfully!!");
                            $('#tblHolidays').datagrid('updateRow', { index: SelectedRowIndex, row: oHoliday });
                        }
                        else {
                            alert(oHoliday.ErrorMessage);
                        }
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
    });

    $('#txtSearchByName').keypress(function (e) {
        //debugger;
        var c = String.fromCharCode(e.which);
        var txtSearchByName = $("#txtSearchByName").val();
        txtSearchByName = txtSearchByName + c;

        var bFlag = false;
        var sTempName = "";
        var rows = $('#tblHolidays').datagrid('getRows');
        for (i = 0; i < rows.length; ++i) {
            sTempName = rows[i]['Description'].substring(0, txtSearchByName.length);
            if (txtSearchByName.toUpperCase() == sTempName.toUpperCase()) {
                bFlag = true;
                break;
            }
        }
        if (bFlag) {
            $('#tblHolidays').datagrid('selectRow', i);
        }
    });
}

function ValidateInput() {
    debugger;
    var sTypeOfHoliday = document.getElementById("cboHolidayTypeOfHoliday");
    var nTypeOfHoliday = sTypeOfHoliday.options[sTypeOfHoliday.selectedIndex].innerHTML;
    if ($("#txtDescription").val() == null || $("#txtDescription").val() == "") {
        alert("Please enter a description!");
        $('#txtDescription').focus();
        return false;
    }
    if (nTypeOfHoliday == "None") {
        alert("Please Enter the TypeOfHoliday Name");
        document.getElementById("cboHolidayTypeOfHoliday").style.borderColor = 'red';
        document.getElementById("cboHolidayTypeOfHoliday").focus();
        return false;
    }
    return true;
}

function RefreshObject() {
    debugger;
    //var IsActive = false;
    //if ($("#chkIsActive").is(':checked') == true) {
    //    IsActive = true;
    //}
    var oHoliday = {
        HolidayID: _oHoliday.HolidayID,
        IsActive: true,
        Code: $("#txtCode").val(),
        Description: $("#txtDescription").val(),
        TypeOfHolidayInt: document.getElementById("cboHolidayTypeOfHoliday").selectedIndex,
        DayOfMonth: GenerateStartTimeInString()
    };

    return oHoliday;
}

function EnableFields() {
    $("#txtDescription").prop("disabled", false);
    $("#txtCode").prop("disabled", false);
    $("#cboHolidayTypeOfHoliday").prop("disabled", false);
    $("#cboDay").prop("disabled", false);
    $("#cboMonth").prop("disabled", false);

}

//function Refresh() {
//    DynamicRefreshList(_oHolidays, "tblHolidays");
//}