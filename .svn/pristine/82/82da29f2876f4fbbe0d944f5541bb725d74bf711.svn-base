var _bIsAddHRResponsibility = false;
var _oHRResponsibility = { HRRID: 0 };

function InitializeHRResponsibilitysEvents() {
    DynamicRefreshList(_oHRResponsibilitys, "tblHRResponsibilitys");
    $("#btnNewHRResponsibility").click(function () {
        $("#lblHeaderName").html("New HR Responsibility");
        $("#txtDescription").val("");
        $("#winHRResponsibility").icsWindow("open", "New HR Responsibility");
        ResetAllFields("winHRResponsibility");
        _oHRResponsibility.HRRID = 0;
        _bIsAddHRResponsibility = true;
    });

    $("#btnClose").click(function () {
        $("#winHRResponsibility").icsWindow("close");
    });

    $("#btnSave").click(function () {
        debugger
        if (!ValidateInput()) return;
        var oHRResponsibility = RefreshObject();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oHRResponsibility,
            ObjectId: oHRResponsibility.HRRID,
            ControllerName: "HRResponsibility",
            ActionName: "Save",
            TableId: "",
            IsWinClose: false
        };
        
        $.icsSave(obj, function (response) {
            if (response.status && response.obj.HRRID > 0) {
              
                if (_bIsAddHRResponsibility)
                {
                    var oHRResponsibilitys = $('#tblHRResponsibilitys').datagrid('getRows');
                    var nIndex = oHRResponsibilitys.length;
                    $('#tblHRResponsibilitys').datagrid('appendRow', response.obj);
                    $('#tblHRResponsibilitys').datagrid('selectRow', nIndex);
                }
                else
                {
                    var oHRResponsibility = $('#tblHRResponsibilitys').datagrid('getSelected');
                    var SelectedRowIndex = $('#tblHRResponsibilitys').datagrid('getRowIndex', oHRResponsibility);
                    $('#tblHRResponsibilitys').datagrid('updateRow', { index: SelectedRowIndex, row: response.obj });
                }
                $("#winHRResponsibility").icsWindow("close");
            }
        });

        ResetAllFields("winHRResponsibility");
    });

    $("#btnEdit").click(function () {
        _bIsAddHRResponsibility = false;
        $("#lblHeaderName").text("Edit HR Responsibility");
        var oHRResponsibility = $('#tblHRResponsibilitys').datagrid('getSelected');
        if (oHRResponsibility == null || oHRResponsibility.HRRID <= 0) {
            alert("Please select a item from list!");
            return;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oHRResponsibility,
            ControllerName: "HRResponsibility",
            ActionName: "getHRResponsibility",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj.HRRID > 0) {
                _oHRResponsibility = response.obj;
                $("#txtCode").val(_oHRResponsibility.Code);
                $("#txtDescription").val(_oHRResponsibility.Description);
                $("#txtDescriptionInBangla").val(_oHRResponsibility.DescriptionInBangla);
            }
        });
        $("#winHRResponsibility").icsWindow("open", "Edit HR Responsibility");
    });

    $("#btnDelete").click(function () {
        debugger
        var oHRR = $('#tblHRResponsibilitys').datagrid('getSelected');
        if (oHRR == null || oHRR.HRRID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblHRResponsibilitys').datagrid('getRowIndex', oHRR);
        
        if (oHRR.HRRID > 0) {
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oHRR,
                ControllerName: "HRResponsibility",
                ActionName: "Delete",
                TableId: "tblHRResponsibilitys",
                IsWinClose: false
            };
            $.icsDelete(obj);
        }
    });

    $('#txtSearchbyCode').keypress(function (e) {
        debugger
        var keyCode = e.keyCode || e.which;
        if (keyCode == 13) {
            var txtSearchbyCode = document.getElementById('txtSearchbyCode').value;
            txtSearchbyCode = txtSearchbyCode;
            //var bFlag=false;
            var sTempName = "";
            var oSearchedData = [];
            var rows = $('#tblHRResponsibilitys').datagrid('getRows');
            for (i = 0; i < rows.length; ++i) {
                sTempName = rows[i]['Code'];
                if (txtSearchbyCode.toUpperCase() == sTempName.toUpperCase()) {
                    oSearchedData.push(rows[i]);
                }
            }

            $('#tblHRResponsibilitys').empty();
            data = oSearchedData;
            if (data.length == 0) {
                Refresh();
            }
            else {
                var data = { "total": "" + data.length + "", "rows": data };
                $('#tblHRResponsibilitys').datagrid('loadData', data);
            }
        }
    });
}

function ValidateInput() {
    if ($("#txtDescription").val() == null || $("#txtDescription").val() == "") {
        alert("Please enter Description!");
        $('#txtDescription').focus();
        return false;
    }
    return true;
}

function RefreshObject() {
    
    var oHRResponsibility = {
        HRRID: _oHRResponsibility.HRRID,
        Code: $("#txtCode").val(),
        Description: $("#txtDescription").val(),
        DescriptionInBangla: $("#txtDescriptionInBangla").val()
    };
    return oHRResponsibility;
}

function Refresh() {
    DynamicRefreshList(_oHRResponsibilitys, "tblHRResponsibilitys");
}