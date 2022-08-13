var _oSUPEs = [];
var _oDBSUPEs = [];
var _otblColumns = [];

function InitializeSUPEsEvents() {
    $('#txtExeNoSUPE').keyup(function (e) {
        if (e.keyCode == 13) {
            var oSUPE = { ErrorMessage:'Arguments;'+ $.trim($('#txtExeNoSUPE').val())+'~~~~~~~~' }
            $.icsDataGets({
                BaseAddress: _sBaseAddress,
                Object: oSUPE,
                ControllerName: "SUProductionExecution",
                ActionName: "Gets",
                IsWinClose: false
            }, function (response) {
                if (response.status) {
                    if (response.objs.length > 0) {
                        DynamicRefreshList(response.objs, "tblSUPEs");
                    }
                    else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSUPEs");
                    }
                }
            });
        }
        else {
            var obj =
            {
                Event: e,
                SearchProperty: "ExecutionNo",
                GlobalObjectList: _oDBSUPEs,
                TableId: "tblSUPEs"
            };
            $('#txtExeNoSUPE').icsSearchByText(obj);
        }
    });

    $("#btnAdvSearchSUPE").click(function () {
        $("#winAdvSearchSUPE").icsWindow("open", "Productin Execution Advance Search");
        $("#txtSearchByExecutionNoAdvSearch").focus();
        DynamicRefreshList([], "tblSUPEAdvSearch");
        DynamicResetAdvSearchWindow("winAdvSearchSUPE");
        DynamicDateActions("cboSearchByExecutionDateAdvSearch", "txtFromExecutionDateAdvSearch", "txtToExecutionDateAdvSearch");
    });

    $("#btnNewSUPE").click(function () {
        _oSUPE = { SUProductionExecutionID: 0 };
        ResetAllFields("winSUPE");
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUProductionExecution/Get",
            traditional: true,
            data: JSON.stringify(_oSUPE),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oSUPE = { ErrorMessage: "" };
                _oSUPE = jQuery.parseJSON(data);
                RefreshControlSUPE(_oSUPE);
                $("#winSUPE").icsWindow("open", "New Production Execution");
                RefreshLayoutSUPE("btnNewSUPE"); //button id as parameter
                $("#btnReceiveConfirmSUPE").hide();
                _bEditable = true;//to control cell edit of details
                $("#cboFinishStoresSUPE").focus();
                $("#cboFinishStoresSUPE").addClass("ComboFocusBorderClass");
            }
        });
    });

    $("#cboFinishStoresSUPE").focusout(function (e) {
        $("#cboFinishStoresSUPE").removeClass("ComboFocusBorderClass");
    });
     
    $("#btnEditSUPE").click(function () {
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || oSUPE.SUProductionExecutionID <= 0) { alert("Please select an item from list!"); return false; }
        if (parseInt(oSUPE.ExecutionStatusInInt) != 1) { alert("Please select an Initialized item!"); return false; }
        ResetAllFields("winSUPE");
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUProductionExecution/Get",
            traditional: true,
            data: JSON.stringify(oSUPE),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oSUPE = { ErrorMessage: "" };
                oSUPE = jQuery.parseJSON(data);
                RefreshControlSUPE(oSUPE);
                $("#winSUPE").icsWindow("open", "Edit Production Execution");
                RefreshLayoutSUPE("btnEditSUPE"); //button id as parameter
                $("#btnReceiveConfirmSUPE").hide();
                _bEditable = true;//to control cell edit of details
                $("#txtPickProductSUPED").focus();
                $("#cboFinishStoresSUPE").removeClass("ComboFocusBorderClass");
            }
        });
    });

    $('#btnDeleteSUPE').click(function () {
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || oSUPE.SUProductionExecutionID <= 0) { alert("Please select an item from list!"); return false; }
        if (parseInt(oSUPE.ExecutionStatusInInt) != 1) { alert("Please select an Initialized item!"); return false; }
        if (!confirm("Confirm to Delete?")) return false;
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oSUPE,
            ObjectId: oSUPE.SUProductionExecutionID,
            ControllerName: "SUProductionExecution",
            ActionName: "Delete",
            TableId: "tblSUPEs",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $("#btnViewSUPE").click(function () {
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || oSUPE.SUProductionExecutionID <= 0) { alert("Please select an item from list!"); return; }
        _oSUPE = oSUPE;
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUProductionExecution/Get",
            traditional: true,
            data: JSON.stringify(oSUPE),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _oSUPE = { ErrorMessage: "" };
                _oSUPE = jQuery.parseJSON(data);
                RefreshControlSUPE(_oSUPE);
                $("#winSUPE").icsWindow("open", "View Production Execution");
                RefreshLayoutSUPE("btnViewSUPE"); //button id as parameter
                _bEditable = false;//to control cell edit of details
                $("#btnCloseSUPE").focus();
            }
        });
    });

    $("#btnReqforApproveSUPE").click(function () {
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || parseInt(oSUPE.SUProductionExecutionID) <= 0) { alert("Please select an item from list!"); return false; }
        if (parseInt(oSUPE.ExecutionStatusInInt) != 1) { alert("Please select an Initialized item!"); return false; }
        if (!confirm("Confirm to Submit Request?")) return false;
        var nRowIndex = $('#tblSUPEs').datagrid('getRowIndex', oSUPE);
        var sSuccessMessage = "Successfully Submitted Request for Approval";
        oSUPE.ExecutionStatus = 2;
        UpdateSUPEStatus(oSUPE, sSuccessMessage, nRowIndex);
    });
    $("#btnApproveSUPE").click(function () {
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || parseInt(oSUPE.SUProductionExecutionID) <= 0) { alert("Please select an item from list!"); return false; }
        if (parseInt(oSUPE.ExecutionStatusInInt) != 2) { alert("Please select a Requested for Approval item!"); return false; }
        if (!confirm("Confirm to Approve?")) return false;
        var nRowIndex = $('#tblSUPEs').datagrid('getRowIndex', oSUPE);
        var sSuccessMessage = "Successfully Approved";
        oSUPE.ExecutionStatus = 3;
        UpdateSUPEStatus(oSUPE, sSuccessMessage, nRowIndex);

    });
    $("#btnCancelSUPE").click(function () {
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || parseInt(oSUPE.SUProductionExecutionID) <= 0) { alert("Please select an item from list!"); return false; }
        if (parseInt(oSUPE.ExecutionStatusInInt) == 4) { alert("ProductionExecution Already Received. can not Cancel!"); return false; }
        if (!confirm("Confirm to Cancel?")) return false;
        var nRowIndex = $('#tblSUPEs').datagrid('getRowIndex', oSUPE);
        var sSuccessMessage = "Successfully Canceled";
        oSUPE.ExecutionStatus = 5;
        UpdateSUPEStatus(oSUPE, sSuccessMessage, nRowIndex);

    });
    $("#btnReceiveSUPE").click(function () {
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || parseInt(oSUPE.SUProductionExecutionID) <= 0) { alert("Please select an item from list!"); return false; }
        if (parseInt(oSUPE.ExecutionStatusInInt) != 3) { alert("Please select an Approved item!"); return false; }
        ResetAllFields("winSUPE");
        $.ajax({
            type: "POST",
            url: _sBaseAddress + "/SUProductionExecution/Get",
            traditional: true,
            data: JSON.stringify(oSUPE),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                 
                oSUPE = { ErrorMessage: "" };
                oSUPE = jQuery.parseJSON(data);
                RefreshControlSUPE(oSUPE);
                $("#winSUPE").icsWindow("open", "Receive Finished Goods");
                RefreshLayoutSUPE("btnReceiveSUPE"); //button id as parameter
                $("#btnSaveSUPE").hide();
                _bEditable = false;//to control cell edit of details
                $("#txtRemarks").focus();
            }
        });

    });

    //$("#btnPreviewSUPE").click(function () {
    //    var oSUPE = $("#tblSUPEs").datagrid("getSelected");
    //    if (oSUPE == null || oSUPE.SUProductionExecutionID <= 0) { alert("Please select an item from list!"); return; }
    //    _oSUPE = oSUPE;
    //    window.open(_sBaseAddress + '/SUProductionExecution/PrintSUProductionExecutionPreview?nID=' + _oSUPE.SUProductionExecutionID, "_blank");
    //});

    $("#btnPreviewSUPE").click(function () {
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || oSUPE.SUProductionExecutionID <= 0) { alert("Please select an item from list!"); return; }
        BtnPrintBtnClick(2);
        return false;
    });

    QtyFromatPopUpMenuLoad(2);
    $("#btnOkPrintFormat2").click(function () {
        $("#winPrintFormat2").icsWindow("close");
        var bPrintFormat = true;
        if ($("#chkInLBS2").is(':checked')) {
            bPrintFormat = false;
        }
        var oSUPE = $("#tblSUPEs").datagrid("getSelected");
        if (oSUPE == null || oSUPE.SUProductionExecutionID <= 0) { alert("Please select an item from list!"); return; }
        _oSUPE = oSUPE;
        window.open(_sBaseAddress + '/SUProductionExecution/PrintSUProductionExecutionPreview?nID=' + _oSUPE.SUProductionExecutionID + "&bPrintFormat=" + bPrintFormat, "_blank");
    });




    $("#btnPrintList").click(function () {
        BtnPrintBtnClick(1);
        return false;
    });

    QtyFromatPopUpMenuLoad(1);
    $("#btnOkPrintFormat1").click(function () {
        $("#winPrintFormat1").icsWindow("close");
        var bPrintFormat = true;
        if ($("#chkInLBS1").is(':checked')) {
            bPrintFormat = false;
        }

        var oSUPEs = $('#tblSUPEs').datagrid('getRows');
        if (oSUPEs.length == 0) {
            alert("No List Found.");
            return false;
        }
        var sIDs = "";
        for (var i = 0; i < oSUPEs.length; i++) {
            sIDs = oSUPEs[i].SUProductionExecutionID + "," + sIDs;
            //oSUPEs[i].IsPrintInKg = bPrintFormat;
        }
        IDs = sIDs.substring(0, sIDs.length - 1);
        $("#txtCollectionPrintText").val(bPrintFormat + "~" + IDs);
    });


    GenerateTableColumnsSUPE();
    DynamicRefreshList(_oDBSUPEs, 'tblSUPEs');
    for (var i = 0; i < _oDBSUPEs.length;i++)
    {
        _oDBSUPEs[i].ExecutionDate = _oDBSUPEs[i].ExecutionDateInString;
    }
    $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUPEs));
    RefreshLayoutSUPE("load");
}

function GenerateTableColumnsSUPE() {
    _otblColumns = [];
    var oColExecutionNo = { field: "ExecutionNo", width: "7%", title: "P. Exe. No.", frozen: true };
    var oColExecutionDateInString = { field: "ExecutionDateInString", width: "10%", title: "Exe. Date", frozen: true };
    var oColExecutionStatusInString = { field: "ExecutionStatusInString", width: "13%", title: "Status", frozen: true };
    var oColApprovedByName = { field: "ApprovedByName", width: "15%", title: "Approved By", frozen: true };
    var oColReceivedByName = { field: "ReceivedByName", width: "15%", title: "Received By", frozen: true };
    var oColOperationUnitName = { field: "OperationUnitName", width: "15%", title: "Received Store", frozen: true };
    var oColTotalExecutionQtyInString = { field: "TotalExecutionQtyInString", width: "12%", title: "Received Quantity", align: 'right', frozen: true };
    var oColRemarks = { field: "Remarks", width: "13%", title: "Remarks", frozen: true };
    _otblColumns.push(oColExecutionNo, oColExecutionDateInString, oColExecutionStatusInString, oColApprovedByName, oColReceivedByName, oColOperationUnitName, oColTotalExecutionQtyInString, oColRemarks);
    $('#tblSUPEs').datagrid({ columns: [_otblColumns] });
}

function UpdateSUPEStatus(oSUPE, sSuccessMessage, nRowIndex) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/SUProductionExecution/UpdateSUPEStatus",
        traditional: true,
        data: JSON.stringify(oSUPE),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var oSUPE = jQuery.parseJSON(data);
            if (oSUPE.ErrorMessage == "" || oSUPE.ErrorMessage == null) {
                alert(sSuccessMessage);
                $('#tblSUPEs').datagrid('updateRow', { index: nRowIndex, row: oSUPE });
                _oDBSUPEs = [];
                _oDBSUPEs = $("#tblSUPEs").datagrid('getRows');
                for (var i = 0; i < _oDBSUPEs.length; i++) {
                    _oDBSUPEs[i].ExecutionDate = _oDBSUPEs[i].ExecutionDateInString;
                }
                $('#txtCollectionPrintText').val(JSON.stringify(_oDBSUPEs));
            } else {
                alert(oSUPE.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}









