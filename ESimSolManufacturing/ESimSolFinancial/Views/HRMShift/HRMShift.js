var _bIsAddHRMShift = false;
var _oHRMShift = { ShiftID: 0 };

function InitializeHRMShiftsEvents() {
    DynamicRefreshList(_oHRMShifts, "tblHRMShifts");
    $("#btnNewHRMShift").click(function () {
        debugger
        $("#lblHeaderName").html("New HRM Shift");
        //$("#txtDescription").val("");
        //$("#winHRMShift").icsWindow("open", "New HRM Shift");
        ResetAllFields("winHRMShift");
        _oHRMShift.ShiftID = 0;
        _bIsAddHRMShift = true;
        var oShifts = $('#tblHRMShifts').datagrid('getRows');
        sessionStorage.setItem("Shifts", JSON.stringify(oShifts));
        sessionStorage.setItem("SelectedRowIndex", -1);
        sessionStorage.setItem("ShiftHeader", "HRM Shift");
        window.location.href = _sBaseAddress + "/HRMShift/View_HRMShift_V1?id=0";
        //$("#btnSave_ShiftBreakSchedule").hide("");
        //$("#btnSave_ShiftOTSlab").hide("");
    });

    $("#btnClose").click(function () {
        $("#winHRMShift").icsWindow("close");
    });


    $("#btnSave").click(function () {
        debugger        
        if (!ValidateInput()) return;
        var oHRMShift = RefreshObject();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oHRMShift,
            ObjectId: oHRMShift.ShiftID,
            ControllerName: "HRMShift",
            ActionName: "Save",
            TableId: "",
            IsWinClose: false
        };

        $.icsSave(obj, function (response) {
            if (response.status && response.obj.ShiftID > 0) {
                debugger
                _oHRMShift.ShiftID = response.obj.ShiftID;
                if (_bIsAddHRMShift) {
                    var oHRMShifts = $('#tblHRMShifts').datagrid('getRows');
                    var nIndex = oHRMShifts.length;
                    $('#tblHRMShifts').datagrid('appendRow', response.obj);
                    $('#tblHRMShifts').datagrid('selectRow', nIndex);
                }
                else {
                    var oHRMShift = $('#tblHRMShifts').datagrid('getSelected');
                    var SelectedRowIndex = $('#tblHRMShifts').datagrid('getRowIndex', oHRMShift);
                    $('#tblHRMShifts').datagrid('updateRow', { index: SelectedRowIndex, row: response.obj });
                }
                //$("#winHRMShift").icsWindow("close");
                $("#btnSave_ShiftBreakSchedule").show();
                $("#btnSave_ShiftOTSlab").show();
                ResetAllFields("winHRMShift");
            }
        });

        
    });

    $("#btnEdit").click(function () {
        debugger;
        DynamicRefreshList([], "tblShiftBreakSchedule");
        _bIsAddHRMShift = false;
        $("#lblHeaderName").text("Edit HRM Shift");
        var oHRMShift = $('#tblHRMShifts').datagrid('getSelected');
        if (oHRMShift == null || oHRMShift.ShiftID <= 0) {
            alert("Please select a item from list!");
            return;
        }

        //var obj = {
        //    BaseAddress: _sBaseAddress,
        //    Object: oHRMShift,
        //    ControllerName: "HRMShift",
        //    ActionName: "getHRMShift",
        //    IsWinClose: false
        //};

        //$.icsDataGet(obj, function (response) {
        //    if (response.status && response.obj.ShiftID > 0) {
        //        _oHRMShift = response.obj;
        //        LoadShiftBrSchedule();
        //        LoadShiftOTSlab();
        //        LoadShiftBrName();
        //        $("#txtName").val(_oHRMShift.Name);
        //        $("#txtCode").val(_oHRMShift.Code);
        //        $('#tpReportTime').timespinner('setValue', _oHRMShift.ReportTimeInString);
        //        $('#tpStartTime').timespinner('setValue', _oHRMShift.StartTimeInString);
        //        $('#tpEndTime').timespinner('setValue', _oHRMShift.EndTimeInString);
        //        $('#tpTolerenceTime').timespinner('setValue', _oHRMShift.ToleranceTimeInString);
        //        $("#txtTotalWorkingTime").val(_oHRMShift.TotalWorkingTimeInString);
        //        $('#tpDayEndTime').timespinner('setValue', _oHRMShift.DayEndTimeInString);
        //        $('#tpDayStartTime').timespinner('setValue', _oHRMShift.DayStartTimeInString);
                
        //        $("#chkIsOT").attr("checked", _oHRMShift.IsOT);
        //        if (_oHRMShift.IsOT) {
        //            $('#tpOTStart').timespinner({ disabled: false });
        //            $('#tpOTEnd').timespinner({ disabled: false });
        //            $("#chkOTOnActual").prop('disabled', false);
        //            $("#chkOTOnActual").attr('checked', _oHRMShift.IsOTOnActual);
        //            $("#txtOTCalculateAfterInMinute").prop('disabled', false);
        //            $("#txtOTCalculateAfterInMinute").val(_oHRMShift.OTCalculateAfterInMinute);
        //        }
        //        else {
        //            $('#tpOTStart').timespinner({ disabled: true });
        //            $('#tpOTEnd').timespinner({ disabled: true });
        //            $("#chkOTOnActual").prop('disabled', true);
        //            $("#txtOTCalculateAfterInMinute").prop('disabled', true);
        //        }
        //        $('#tpOTStart').timespinner('setValue', _oHRMShift.OTStartTimeInString);
        //        $('#tpOTEnd').timespinner('setValue', _oHRMShift.OTEndTimeInString);
        //    }
        //});
        //$("#winHRMShift").icsWindow("open", "Edit HRM Shift");


        var oShifts = $('#tblHRMShifts').datagrid('getRows');
        sessionStorage.setItem("Shifts", JSON.stringify(oShifts));
        sessionStorage.setItem("SelectedRowIndex", -1);
        sessionStorage.setItem("ShiftHeader", "HRM Shift");
        window.location.href = _sBaseAddress + "/HRMShift/View_HRMShift_V1?id=" + oHRMShift.ShiftID;
    });


    $("#btnCopy").click(function () {

        var oHRS = $('#tblHRMShifts').datagrid('getSelected');
        if (oHRS == null || oHRS.ShiftID <= 0) {
            alert("Please select a item from list!");
            return;
        }

        if (!confirm("Confirm to Copy?")) return;

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/HRMShift/Copy",
            traditional: true,
            data: JSON.stringify(oHRS),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                var oShift = jQuery.parseJSON(data);
                if (oShift.ErrorMessage == "" || oShift.ErrorMessage == null) {

                    alert("Shift Save Succesfully!!");
                    if (oShift.ShiftID > 0) {
                        var oShifts = $('#tblHRMShifts').datagrid('getRows');
                        var nIndex = oShifts.length;

                        $('#tblHRMShifts').datagrid('appendRow', oShift);
                        $('#tblHRMShifts').datagrid('selectRow', nIndex);
                    }

                }
                else {
                    alert(oShift.ErrorMessage);

                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });



    });


    $("#btnDelete").click(function () {
        debugger
        var oHRR = $('#tblHRMShifts').datagrid('getSelected');
        if (oHRR == null || oHRR.ShiftID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblHRMShifts').datagrid('getRowIndex', oHRR);

        if (oHRR.ShiftID > 0) {
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oHRR,
                ControllerName: "HRMShift",
                ActionName: "Delete",
                TableId: "tblHRMShifts",
                IsWinClose: false
            };
            $.icsDelete(obj);
        }
    });

    $("#btnActivity").click(function () {
        var oHRMShift = $('#tblHRMShifts').datagrid('getSelected');
        if (oHRMShift == null || oHRMShift.ShiftID <= 0) {
            alert("Please select a item from list!");
            return false;
        }
        if (oHRMShift.IsActive == true) {
            if (!confirm("Confirm to In-Active?")) return;
            oHRMShift.IsActive = false;
        }
        else {
            if (!confirm("Confirm to Active?")) return;
            oHRMShift.IsActive = true;
        }
        var SelectedRowIndex = $('#tblHRMShifts').datagrid('getRowIndex', oHRMShift);
        if (oHRMShift.ShiftID > 0) {
            debugger;
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/HRMShift/ActiveInActive",
                traditional: true,
                data: JSON.stringify(oHRMShift),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var oHRMShift = jQuery.parseJSON(data);
                    debugger;
                    if (oHRMShift != null) {
                        if (oHRMShift.ShiftID > 0) {
                            alert("Data Save Succesfully!!");
                            $('#tblHRMShifts').datagrid('updateRow', { index: SelectedRowIndex, row: oHRMShift });
                        }
                        else {
                            alert(oHRMShift.ErrorMessage);
                        }
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
    });

    $('#txtSearchbyShiftCode').keyup(function (e) {
        debugger
        var keyCode = e.keyCode || e.which;
        //if (keyCode == 13) {
        var txtSearchbyCode =$("#txtSearchbyShiftCode").val();
            txtSearchbyCode = txtSearchbyCode;
            //var bFlag=false;
            var sTempName = "";
            var oSearchedData = [];
            var rows = $('#tblHRMShifts').datagrid('getRows');
            for (i = 0; i < rows.length; ++i) {
                sTempName = rows[i]['Code'];
                if (txtSearchbyCode.toUpperCase() == sTempName.toUpperCase()) {
                    oSearchedData.push(rows[i]);
                }
            }

            $('#tblHRMShifts').empty();
            data = oSearchedData;
            if (data.length == 0) {
                Refresh();
            }
            else {
                var data = { "total": "" + data.length + "", "rows": data };
                $('#tblHRMShifts').datagrid('loadData', data);
            }
        //}
    });

    $('#chkIsOT').click(function () {
        if (document.getElementById("chkIsOT").checked == true) {
            $('#tpOTStart').timespinner({ disabled: false });
            $('#tpOTEnd').timespinner({ disabled: false });
            $("#chkOTOnActual").prop('disabled', false);
            $("#txtOTCalculateAfterInMinute").prop('disabled', false);
        }
        else {
            $('#tpOTStart').timespinner({ disabled: true });
            $('#tpOTEnd').timespinner({ disabled: true });
            $("#chkOTOnActual").prop('disabled', true);
            $("#txtOTCalculateAfterInMinute").prop('disabled', true);
            $("#chkOTOnActual").attr('checked', false);
        }
        $('#tpOTStart').timespinner('setValue', '00:00');
        $('#tpOTEnd').timespinner('setValue', '00:00');
        $("#txtOTCalculateAfterInMinute").val(0);
    });

    $("#btnSave_ShiftBreakSchedule").click(function () {
        if (!ValidateInput_ShiftBrSchedule()) return;
        var oSBSchedule = RefreshObject_ShiftBrSchedule();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oSBSchedule,
            ObjectId: oSBSchedule.ShiftBScID,
            ControllerName: "HRMShift",
            ActionName: "ShiftBreakSchedule_IU",
            TableId: "tblShiftBreakSchedule",
            IsWinClose: false
        };
        $.icsSave(obj);
    });

    $('#btnDelete_ShiftBreakSchedule').click(function (e) {
        var oShiftBreakSchedule = $('#tblShiftBreakSchedule').datagrid('getSelected');
        if (oShiftBreakSchedule == null || oShiftBreakSchedule.ShiftBScID <= 0) {
            alert("Please select an item from list!");
            return;
        }

        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblShiftBreakSchedule').datagrid('getRowIndex', oShiftBreakSchedule);
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oShiftBreakSchedule,
            ControllerName: "HRMShift",
            ActionName: "ShiftBreakSchedule_Delete",
            TableId: "tblShiftBreakSchedule",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $('#btnActivity_ShiftBreakSchedule').click(function (e) {
        var oShiftBreakSchedule = $('#tblShiftBreakSchedule').datagrid('getSelected');
        if (oShiftBreakSchedule == null || oShiftBreakSchedule.ShiftBScID <= 0) {
            alert("Please select an item from list!");
            return;
        }

        var SelectedRowIndex = $('#tblShiftBreakSchedule').datagrid('getRowIndex', oShiftBreakSchedule);
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/HRMShift/ShiftBreakSchedule_Activity",
            traditional: true,
            data: JSON.stringify(oShiftBreakSchedule),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oShiftBreakSchedule = jQuery.parseJSON(data);
                if (oShiftBreakSchedule.ShiftBScID > 0) {
                    alert("Saved Successfully!");
                    $('#tblShiftBreakSchedule').datagrid('updateRow', { index: SelectedRowIndex, row: oShiftBreakSchedule });
                }
                else {
                    alert(oShiftBreakSchedule.ErrorMessage);
                }
            }
        });
    });

    // Shift OT Slab Start
    $("#btnSave_ShiftOTSlab").click(function () {
        if (!ValidateInput_ShiftOTSlab()) return;
        var oShiftOTSlab = RefreshObject_ShiftOTSlab();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oShiftOTSlab,
            ObjectId: oShiftOTSlab.ShiftOTSlabID,
            ControllerName: "HRMShift",
            ActionName: "ShiftOTSlab_IU",
            TableId: "tblShiftOTSlab",
            IsWinClose: false
        };
        
        $.icsSave(obj, function (response) {
            if (response.status && response.obj.ShiftOTSlabID > 0) {
                $("#txtMinInMin").val("");
                $("#txtMaxInMin").val("");
                $("#txtAchieveInMin").val("");
            }
        });
    });

    $('#btnDelete_ShiftOTSlab').click(function (e) {
        var oShiftOTSlab = $('#tblShiftOTSlab').datagrid('getSelected');
        if (oShiftOTSlab == null || oShiftOTSlab.ShiftBScID <= 0) {
            alert("Please select an item from list!");
            return;
        }

        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblShiftOTSlab').datagrid('getRowIndex', oShiftOTSlab);
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oShiftOTSlab,
            ControllerName: "HRMShift",
            ActionName: "ShiftOTSlab_Delete",
            TableId: "tblShiftOTSlab",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $('#btnActivity_ShiftOTSlab').click(function (e) {
        var oShiftOTSlab = $('#tblShiftOTSlab').datagrid('getSelected');
        if (oShiftOTSlab == null || oShiftOTSlab.ShiftOTSlabID <= 0) {
            alert("Please select an item from list!");
            return;
        }

        var SelectedRowIndex = $('#tblShiftOTSlab').datagrid('getRowIndex', oShiftOTSlab);
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/HRMShift/ShiftOTSlab_Activity",
            traditional: true,
            data: JSON.stringify(oShiftOTSlab),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oShiftOTSlab = jQuery.parseJSON(data);
                if (oShiftOTSlab.ShiftOTSlabID > 0) {
                    alert("Saved Successfully!");
                    $('#tblShiftOTSlab').datagrid('updateRow', { index: SelectedRowIndex, row: oShiftOTSlab });
                }
                else {
                    alert(oShiftOTSlab.ErrorMessage);
                }
            }
        });
    });

    // Shift OT Slab End


    $("#btnInActive").click(function () {
        var oHRMShift = $('#tblHRMShifts').datagrid('getSelected');
        if (oHRMShift == null || oHRMShift.ShiftID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (oHRMShift.IsActive == false) {
            alert("Already Inactive!");
            return;
        }

        var oShift = [];

        for (var i = 0; i < _oHRMShifts.length; i++)
        {
            if(oHRMShift.ShiftID!=_oHRMShifts[i].ShiftID)
            {
                oShift.push(_oHRMShifts[i]);
            }
        }


        $("#cboTransferTo").icsLoadCombo({
            List: oShift,
            OptionValue: "ShiftID",
            DisplayText: "Name"
        });

        $("#winTransferToPicker").icsWindow("open");
    });

    $("#btnInactiveShiftClose").click(function () {
        $("#winTransferToPicker").icsWindow("close");
    });

    $("#btnInactiveShiftOK").click(function () {
        var nTransferToShift = $("#cboTransferTo").val();
        if (nTransferToShift <= 0) { alert("Please select a shift!"); return;}

        var oHRMShift = $('#tblHRMShifts').datagrid('getSelected');
        SelectedRowIndex = $('#tblHRMShifts').datagrid('getRowIndex', oHRMShift);

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/HRMShift/HRMShiftInActive",
                traditional: true,
                data: JSON.stringify({ nShiftID: oHRMShift.ShiftID, ntRShiftID: nTransferToShift }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var oHRMShift = jQuery.parseJSON(data);
                    debugger;
                    if (oHRMShift != null) {
                        if (oHRMShift.ShiftID > 0) {
                            alert("Data Save Succesfully!!");
                            $('#tblHRMShifts').datagrid('updateRow', { index: SelectedRowIndex, row: oHRMShift });
                            $("#winTransferToPicker").icsWindow("close");
                        }
                        else {
                            alert(oHRMShift.ErrorMessage);
                            $("#winTransferToPicker").icsWindow("close");
                        }
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
    });

    $("#btnActive").click(function () {
        var oHRMShift = $('#tblHRMShifts').datagrid('getSelected');
        if (oHRMShift == null || oHRMShift.ShiftID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (oHRMShift.IsActive == true) {
            alert("Already Active!");
            return;
        }
        var SelectedRowIndex = $('#tblHRMShifts').datagrid('getRowIndex', oHRMShift);
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/HRMShift/HRMShiftActive",
            traditional: true,
            data: JSON.stringify({ nShiftID: oHRMShift.ShiftID }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                var oHRMShift = jQuery.parseJSON(data);
                debugger;
                if (oHRMShift != null) {
                    if (oHRMShift.ShiftID > 0) {
                        alert("Data Save Succesfully!!");
                        $('#tblHRMShifts').datagrid('updateRow', { index: SelectedRowIndex, row: oHRMShift });
                        $("#winTransferToPicker").icsWindow("close");
                    }
                    else {
                        alert(oHRMShift.ErrorMessage);
                        $("#winTransferToPicker").icsWindow("close");
                    }
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });
}

function ValidateInput() {
    if ($("#txtName").val() == null || $("#txtName").val() == "") {
        alert("Please enter Name!");
        $('#txtName').focus();
        return false;
    }
    if ($('#tpStartTime').timespinner('getValue') == "") {
        alert("Please enter start time!");
        $('#tpStartTime').focus();
        return false;
    }
    if ($('#tpEndTime').timespinner('getValue') == "") {
        alert("Please enter end time!");
        $('#tpEndTime').focus();
        return false;
    }
    return true;
}

function RefreshObject() {
    debugger;
    var TWT = 0;
    var endTime = $('#tpEndTime').timespinner('getValue');
    var startTime = $('#tpStartTime').timespinner('getValue');
    var eTime = endTime.split(':');
    var hEndTime = parseFloat(eTime[0]);
    var mEndTime = parseFloat(eTime[1]);
    var sTime = startTime.split(':');
    var hStartTime = parseFloat(sTime[0]);
    var mStartTime = parseFloat(sTime[1]);
    if (hEndTime < hStartTime) {
        TWT = (((hEndTime + 12) * 60) + mEndTime) - (((hStartTime - 12) * 60) + mStartTime);
    }
    else {
        TWT = (((hEndTime) * 60) + mEndTime) - (((hStartTime) * 60) + mStartTime);
    }
    var oHRMShift = {
        ShiftID: _oHRMShift.ShiftID,
        Code: $("#txtCode").val(),
        Name: $("#txtName").val(),
        ReportTime: $('#tpReportTime').timespinner('getValue'),
        StartTime: $('#tpStartTime').timespinner('getValue'),
        EndTime: $('#tpEndTime').timespinner('getValue'),
        TotalWorkingTime: TWT,
        ToleranceTime: $('#tpTolerenceTime').timespinner('getValue'),
        DayEndTime: $('#tpDayEndTime').timespinner('getValue'),
        DayStartTime: $('#tpDayStartTime').timespinner('getValue'),
        IsOT: $('#chkIsOT').is(":checked"),
        OTStartTime: $('#tpOTStart').timespinner('getValue'),
        OTEndTime: $('#tpOTEnd').timespinner('getValue'),
        IsOTOnActual: $('#chkOTOnActual').is(":checked"),
        OTCalculateAfterInMinute: $("#txtOTCalculateAfterInMinute").val()
    };
    return oHRMShift;    
}

function Refresh() {
    DynamicRefreshList(_oHRMShifts, "tblHRMShifts");
}
function ResetAllFields(S)
{
    $("#txtName").val("");
    $("#txtCode").val("");
    $('#tpDayStartTime').timespinner('setValue', '00:00');
    $('#tpDayEndTime').timespinner('setValue', '23:59');

    $('#tpStartTime').timespinner('setValue', '00:00');
    $('#tpEndTime').timespinner('setValue', '00:00');
    $('#tpReportTime').timespinner('setValue', '00:00');
    $('#tpTolerenceTime').timespinner('setValue', '00:00');
    $("#txtTotalWorkingTime").val("");

    $("#chkIsOT").prop('checked', false);
    $('#tpOTStart').timespinner({ disabled: true });
    $('#tpOTEnd').timespinner({ disabled: true });
    $("#chkOTOnActual").prop('disabled', true);
    $("#chkOTOnActual").attr('checked', false);
    $("#txtOTCalculateAfterInMinute").prop('disabled', true);
    $('#tpOTStart').timespinner('setValue', '00:00');
    $('#tpOTEnd').timespinner('setValue', '00:00');

    //$("#btnSave_ShiftBreakSchedule").hide();
    //$("#btnDelete_ShiftBreakSchedule").hide("");
    LoadShiftBrName();
    DynamicRefreshList([], "tblShiftBreakSchedule");

    //$("#btnSave_ShiftOTSlab").hide();
    //$("#btnDelete_ShiftOTSlab").hide("");
    $("#txtMinInMin").val("");
    $("#txtMaxInMin").val("");
    $("#txtAchieveInMin").val("");
    DynamicRefreshList([], "tblShiftOTSlab");

}

function LoadShiftBrName()
{
    var oShiftBreakName = { ShiftBNID: 0 }
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oShiftBreakName,
        ControllerName: "HRMShift",
        ActionName: "getsShiftBreakName",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            $("#cboShiftBreakName").icsLoadCombo({
                List: response.objs,
                OptionValue: "ShiftBNID",
                DisplayText: "Name",
                InitialValue: "Br. Name"
            });
        }
    });
    $('#tpStartBSchedule').timespinner('setValue', '00:00');
    $('#tpEndBSchedule').timespinner('setValue', '00:00');
}

function ValidateInput_ShiftBrSchedule() {
    if (_oHRMShift.ShiftID<=0)
    {
        alert("Please save a shift first!");
        return false;
    }
    if ($("#cboShiftBreakName").val()<= 0) {
        alert("Please select a Shift Break Name!");
        $('#cboShiftBreakName').focus();
        return false;
    }
    if ($('#tpStartBSchedule').timespinner('getValue') == "") {
        alert("Please enter start time!");
        $('#tpStartBSchedule').focus();
        return false;
    }
    if ($('#tpEndBSchedule').timespinner('getValue') == "") {
        alert("Please enter end time!");
        $('#tpEndBSchedule').focus();
        return false;
    }
    return true;
}

function RefreshObject_ShiftBrSchedule() {
    var oHRMShift = {
        ShiftBScID :0,
        ShiftID :_oHRMShift.ShiftID,
        ShiftBNID : $("#cboShiftBreakName").val(),
        StartTime: $('#tpStartBSchedule').timespinner('getValue'),
        EndTime: $('#tpEndBSchedule').timespinner('getValue')
    };
    
    return oHRMShift;
}

function LoadShiftBrSchedule() {
    var oShiftBreakName = { ShiftID: _oHRMShift.ShiftID };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oShiftBreakName,
        ControllerName: "HRMShift",
        ActionName: "getsShiftBreakSchedule",
        IsWinClose: false
    };
    debugger
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            DynamicRefreshList(response.objs, "tblShiftBreakSchedule");
        }
    });
}

function PrintList() {
    var oShifts = [];
    oShifts = $('#tblHRMShifts').datagrid('getChecked');
    if (oShifts.length <= 0) { alert("please select at least one item"); return; }
    var sShiftIDs = "";
    for (var i = 0; i < oShifts.length; i++) {
        sShiftIDs += oShifts[i].ShiftID + ",";
    }
    sShiftIDs = sShiftIDs.substring(0, sShiftIDs.length - 1);

    var tsv = ((new Date()).getTime()) / 1000;
    window.open(_sBaseAddress + "/HRMShift/PrintShift?sShiftIDs=" + sShiftIDs + "&ts=" + tsv, "_blank");
}

// Shift OT Slab Start
function ValidateInput_ShiftOTSlab() {
    debugger
    if (_oHRMShift.ShiftID <= 0) {
        alert("Please save a shift first!");
        return false;
    }
 
    if ($.trim($("#txtMinInMin").val()) <= 0) {
        alert("Please select a min value!");
        $('#txtMinInMin').focus();
        return false;
    }
    if ($.trim($("#txtMaxInMin").val()) <= 0) {
        alert("Please select a max value!");
        $('#txtMaxInMin').focus();
        return false;
    }
    if ($.trim($("#txtAchieveInMin").val()) <= 0) {
        alert("Please select a achive value!");
        $('#txtAchieveInMin').focus();
        return false;
    }
    return true;
}

function RefreshObject_ShiftOTSlab() {
    var oShiftOTSlab = {
        ShiftOTSlabID: 0,
        ShiftID: _oHRMShift.ShiftID,
        MinOTInMin: $.trim($("#txtMinInMin").val()),
        MaxOTInMin: $.trim($("#txtMaxInMin").val()),
        AchieveOTInMin:$.trim($("#txtAchieveInMin").val())
    };

    return oShiftOTSlab;
}

function LoadShiftOTSlab() {
    var oShiftOTSlab = { ShiftID: _oHRMShift.ShiftID };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oShiftOTSlab,
        ControllerName: "HRMShift",
        ActionName: "getsShiftOTSlab",
        IsWinClose: false
    };
    debugger
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            DynamicRefreshList(response.objs, "tblShiftOTSlab");
        }
    });
    $("#txtMinInMin").val("");
    $("#txtMaxInMin").val("");
    $("#txtAchieveInMin").val("");
}

// Shift OT Slab End