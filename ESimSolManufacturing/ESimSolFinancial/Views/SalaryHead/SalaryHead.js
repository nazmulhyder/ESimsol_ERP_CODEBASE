var _bIsAddSalaryHead = false;
var _oSalaryHead = { SalaryHeadID: 0 };

function InitializeSalaryHeadsEvents() {

    DynamicRefreshList(_oSalaryHeads, "tblSalaryHeads");
    $("#btnNewSalaryHead").click(function () {
        debugger
        $("#lblHeaderName").html("New Leave Head");
        //$("#txtDescription").val("");
        $("#winSalaryHead").icsWindow("open", "New Leave Head");
        EnableFields();
        ResetAllFields("winSalaryHead");
        _oSalaryHead.SalaryHeadID = 0;
        _bIsAddSalaryHead = true;
    });

    $("#btnClose").click(function () {
        $("#winSalaryHead").icsWindow("close");
    });

    $("#btnSave").click(function () {
        debugger
        if (!ValidateInput()) return;
        var oSalaryHead = RefreshObject();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSalaryHead,
            ObjectId: oSalaryHead.SalaryHeadID,
            ControllerName: "SalaryHead",
            ActionName: "Save",
            TableId: "",
            IsWinClose: false
        };

        $.icsSave(obj, function (response) {
            if (response.status && response.obj.SalaryHeadID > 0) {

                if (_bIsAddSalaryHead) {
                    var oSalaryHeads = $('#tblSalaryHeads').datagrid('getRows');
                    var nIndex = oSalaryHeads.length;
                    $('#tblSalaryHeads').datagrid('appendRow', response.obj);
                    $('#tblSalaryHeads').datagrid('selectRow', nIndex);
                }
                else {
                    var oSalaryHead = $('#tblSalaryHeads').datagrid('getSelected');
                    var SelectedRowIndex = $('#tblSalaryHeads').datagrid('getRowIndex', oSalaryHead);
                    $('#tblSalaryHeads').datagrid('updateRow', { index: SelectedRowIndex, row: response.obj });
                }
                $("#winSalaryHead").icsWindow("close");
            }
        });

        ResetAllFields("winSalaryHead");
    });

    $("#btnEdit").click(function () {
        debugger
        _bIsAddSalaryHead = false;
        $("#lblHeaderName").text("Edit Leave Head");
        var oSalaryHead = $('#tblSalaryHeads').datagrid('getSelected');
        if (oSalaryHead == null || oSalaryHead.SalaryHeadID <= 0) {
            alert("Please select a item from list!");
            return;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSalaryHead,
            ControllerName: "SalaryHead",
            ActionName: "getSalaryHead",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj.SalaryHeadID > 0) {
                _oSalaryHead = response.obj;
                $("#txtName").val(_oSalaryHead.Name);
                $("#txtNameInBangla").val(_oSalaryHead.NameInBangla);
                $("#txtDescription").val(_oSalaryHead.Description);
                document.getElementById("cboSalaryHeadType").selectedIndex = _oSalaryHead.SalaryHeadType;
                $("#txtSequence").val(_oSalaryHead.Sequence);
                $('#chkProcessDependent').attr('checked', _oSalaryHead.IsProcessDependent);
            }
        });
        $("#winSalaryHead").icsWindow("open", "Edit Leave Head");
        EnableFields();
    });

    $("#btnView").click(function () {
        debugger
        _bIsAddSalaryHead = false;
        $("#lblHeaderName").text("View Leave Head");
        var oSalaryHead = $('#tblSalaryHeads').datagrid('getSelected');
        if (oSalaryHead == null || oSalaryHead.SalaryHeadID <= 0) {
            alert("Please select a item from list!");
            return;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSalaryHead,
            ControllerName: "SalaryHead",
            ActionName: "getSalaryHead",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj.SalaryHeadID > 0) {
                _oSalaryHead = response.obj;
                $("#txtName").val(_oSalaryHead.Name);
                $("#txtNameInBangla").val(_oSalaryHead.NameInBangla);
                $("#txtDescription").val(_oSalaryHead.Description);
                document.getElementById("cboSalaryHeadType").selectedIndex = _oSalaryHead.SalaryHeadType;
                $('#chkProcessDependent').attr('checked', _oSalaryHead.IsProcessDependent);

                $("#txtName").prop("disabled", true);
                $("#txtNameInBangla").prop("disabled", true);
                $("#txtDescription").prop("disabled", true);
                $("#txtSequence").prop("disabled", true);
                $("#cboSalaryHeadType").prop("disabled", true);
                $("#chkProcessDependent").prop("disabled", true);
            }
        });
        $("#winSalaryHead").icsWindow("open", "View Leave Head");
    });

    $("#btnDelete").click(function () {
        debugger
        var oHRR = $('#tblSalaryHeads').datagrid('getSelected');
        if (oHRR == null || oHRR.SalaryHeadID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblSalaryHeads').datagrid('getRowIndex', oHRR);

        if (oHRR.SalaryHeadID > 0) {
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oHRR,
                ControllerName: "SalaryHead",
                ActionName: "Delete",
                TableId: "tblSalaryHeads",
                IsWinClose: false
            };
            $.icsDelete(obj);
        }
    });

    $("#btnActivity").click(function () {
        var oSalaryHead = $('#tblSalaryHeads').datagrid('getSelected');
        if (oSalaryHead == null || oSalaryHead.SalaryHeadID <= 0) {
            alert("Please select a item from list!");
            return false;
        }
        if (oSalaryHead.IsActive == true) {
            if (!confirm("Confirm to In-Active?")) return;
            oSalaryHead.IsActive = false;
        }
        else {
            if (!confirm("Confirm to Active?")) return;
            oSalaryHead.IsActive = true;
        }
        var SelectedRowIndex = $('#tblSalaryHeads').datagrid('getRowIndex', oSalaryHead);
        if (oSalaryHead.SalaryHeadID > 0) {
            debugger;
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/SalaryHead/ChangeActiveStatus",
                traditional: true,
                data: JSON.stringify(oSalaryHead),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var oSalaryHead = jQuery.parseJSON(data);
                    debugger;
                    if (oSalaryHead != null) {
                        if (oSalaryHead.SalaryHeadID > 0) {
                            alert("Data Save Succesfully!!");
                            $('#tblSalaryHeads').datagrid('updateRow', { index: SelectedRowIndex, row: oSalaryHead });
                        }
                        else {
                            alert(oSalaryHead.ErrorMessage);
                        }
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
    });
}

function ValidateInput() {

    if ($("#txtName").val() == null || $("#txtName").val() == "") {
        alert("Please enter Name!");
        $('#txtName').focus();
        return false;
    }

    var sSalaryHeadType = document.getElementById("cboSalaryHeadType");
    var nSalaryHeadType = sSalaryHeadType.options[sSalaryHeadType.selectedIndex].innerHTML;
    if (document.getElementById("txtDescription").value == null || document.getElementById("txtDescription").value == "") {
        alert("Please enter a description!");
        $('#txtDescription').focus();
        return false;
    }
    if (nSalaryHeadType == "None") {
        alert("Please Enter the SalaryHeadType Name");
        document.getElementById("cboSalaryHeadType").style.borderColor = 'red';
        document.getElementById("cboSalaryHeadType").focus();
        return false;
    }

    return true;
}

function RefreshObject() {
    var oSalaryHead = {
        SalaryHeadID: _oSalaryHead.SalaryHeadID,
        IsActive: true,
        Name: $("#txtName").val(),
        NameInBangla: $("#txtNameInBangla").val(),
        Description: $("#txtDescription").val(),
        SalaryHeadTypeInt: document.getElementById("cboSalaryHeadType").selectedIndex,
        IsProcessDependent: $("#chkProcessDependent").is(':checked'),
        Sequence: $("#txtSequence").val()
    };
    return oSalaryHead; 
}

function EnableFields() {
    $("#txtName").prop("disabled", false);
    $("#txtNameInBangla").prop("disabled", false);
    $("#txtDescription").prop("disabled", false);
    $("#txtSequence").prop("disabled", false);
    $("#cboSalaryHeadType").prop("disabled", false);
    $("#chkProcessDependent").prop("disabled", false);
}

function UP() {
    debugger;
    var oSalaryHead = $('#tblSalaryHeads').datagrid('getSelected');

    if (oSalaryHead == null || oSalaryHead.SalaryHeadID <= 0) {
        alert("Please select a item from list!");
        return false;
    }

    var SelectedRowIndex = $('#tblSalaryHeads').datagrid('getRowIndex', oSalaryHead);
    if (oSalaryHead.Sequence <= 1) {
        return;
    }

    var obj = {
        SalaryHeadID: oSalaryHead.SalaryHeadID,
        IsUp: true
    };

    sessionStorage.setItem("upIndex", (SelectedRowIndex - 1));
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SalaryHead/UpDown",
        traditional: true,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            var oSalaryHeads = jQuery.parseJSON(data);
            debugger;
            if (oSalaryHeads.length > 0) {
                var n = parseInt(sessionStorage.getItem("upIndex"));
                DynamicRefreshList(oSalaryHeads, 'tblSalaryHeads');
                $('#tblSalaryHeads').datagrid('selectRow', n);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });

} 
function Down() {
    debugger;
    var oSalaryHead = $('#tblSalaryHeads').datagrid('getSelected');
    var oSalaryHeads = $('#tblSalaryHeads').datagrid('getRows');

    if (oSalaryHead == null || oSalaryHead.SalaryHeadID <= 0) {
        alert("Please select a item from list!");
        return false;
    }

    var SelectedRowIndex = $('#tblSalaryHeads').datagrid('getRowIndex', oSalaryHead);
    if ((SelectedRowIndex + 1) == oSalaryHeads.length) {
        return;
    }

    var obj = {
        SalaryHeadID: oSalaryHead.SalaryHeadID,
        IsUp: false
    };

    sessionStorage.setItem("upIndex", (SelectedRowIndex + 1));
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SalaryHead/UpDown",
        traditional: true,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            var oSalaryHeads = jQuery.parseJSON(data);
            debugger;
            if (oSalaryHeads.length > 0) {
                var n = parseInt(sessionStorage.getItem("upIndex"));
                DynamicRefreshList(oSalaryHeads, 'tblSalaryHeads');
                $('#tblSalaryHeads').datagrid('selectRow', n);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });

}