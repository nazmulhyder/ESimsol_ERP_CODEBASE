function InitializeExportLCsEvents() {
    $('#tblExportLCs').datagrid({
        onSelect: function (rowIndex, rowData)
        {
            OperationPerformsExportLC(rowIndex, rowData);
        }
    });
    var oELC = {
        ExportLCID:1,
        CurrentStatusInInt : 1
    };
    OperationPerformsExportLC(0, oELC);

    DynamicRefreshList(_oExportLCs, "tblExportLCs");


    $("#btnAddExportLC").click(function () {
        $('#chkExportLC1').prop('checked', true);
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        $(".amendmentDateFields").hide();
        $("#winExportLC").icsWindow('open', "Add ExportLC");
        _oExportLC = NewExportLCObj();
        RefreshExportLCLayout("btnAddExportLC");
        RefreshExportLCControl(_oExportLC);
        $("#txtContractorName,#txtDeliverTo,#txtExportLCNo,#txtNegoDays,#txtSearchByBankBranch_Issue,#txtAmount,#txtLCStatusInString,#txtNote,#txtHSCode,#txtAreaCode").val("");
        $("#cboBankBranch_Advice,#cboBankBranch_Nego").val(0);
        $("#txtOpeningDate,#txtReceiveDate,#txtShipmentDate,#txtExpireDate").datebox("setValue", "");
        SetAmendmentDate("btnAddExportLC");
    });

    $("#btnEditExportLC").click(function () {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        $(".amendmentDateFields").hide();
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }
        if (oExportLC.CurrentStatus >= 2) {
            alert("Sorry, Already Approved.");
            return false;
        }
        $("#winExportLC").icsWindow('open', "Edit ExportLC");
        RefreshExportLCLayout("btnEditExportLC");
        GetExportLCInformation(oExportLC);
        SetAmendmentDate("btnEditExportLC");
    });

    $("#btnViewExportLC").click(function () {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        $(".amendmentDateFields").hide();
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportLC").icsWindow('open', "View ExportLC");
        RefreshExportLCLayout("btnViewExportLC");
        GetExportLCInformation(oExportLC);
        SetAmendmentDate("btnViewExportLC");
    });

    $("#btnDeleteExportLC").click(function () {
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || oExportLC.ExportLCID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (oExportLC.CurrentStatus >= 2) {
            alert("Sorry, Already Approved.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;

        
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportLC,
            ObjectId: oExportLC.ExportLCID,
            ControllerName: "ExportLC",
            ActionName: "DeleteExportLC",
            TableId: "tblExportLCs",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $('#txtSearchbyLCNo').keypress(function (e) {

        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            if ($.trim($("#txtSearchbyLCNo").val()) == "")
            {
                alert("Please give LC No.");
                return false;
            }

            var oExportLC =
            {
                ExportLCID: 0,
                BUID:_nBUID,
                ExportLCNo: $.trim($("#txtSearchbyLCNo").val())
            };
            Gets_byLCNo(oExportLC);
        }

    });

    $("#btnAddBills").click(function () {
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }
        //if (oExportLC.CurrentStatus != 2) { alert("Please select an approve L/C!"); return; }
       
        DynamicRefreshList([], "tblExportBills");
        $("#winExportBills").icsWindow('open', "Add Export Bill");
        RefreshExportLCLayout("btnAddBills");
        FieldDisableEnabledELC("btnAddBills");
        GetExportLCInformation_ExportBill(oExportLC);
    });

    $("#btnGetOrginalCopy").click(function () {
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }
        
        if (!confirm("Confirm to Get orginal Copy?")) return false;
        var nRowIndex = $('#tblExportLCs').datagrid('getRowIndex', oExportLC);
        var sSuccessMessage = "Successfully Change Get Orginal copy";
        
        UpdatePIGetOrginalCopy(oExportLC, sSuccessMessage, nRowIndex);
    });

    $("#btnApproveLC").click(function () {
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        
        if (oExportLC != null && oExportLC.CurrentStatus == 2)
        {
            alert("Sorry Already Approved.");
            return false;
        }
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }

        if (!confirm("Confirm to Approved?")) return false;
        var nRowIndex = $('#tblExportLCs').datagrid('getRowIndex', oExportLC);
        var sSuccessMessage = "Successfully Approved";
        oExportLC.AmendmentDate = oExportLC.AmendmentDateSt;
        ApprovedLC(oExportLC, sSuccessMessage, nRowIndex);
    });

    $("#btnAddAmendmentRecd").click(function () {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }

        if (oExportLC.Stability == true) {
            var bDecision = confirm("Do you want to change version no. or not?");
            var nRowIndex = $("#tblExportLCs").datagrid("getRowIndex", oExportLC);
            $(".amendmentDateFields").show();
            //Ratin
            SetAmendmentDate("btnAddAmendmentRecd");
            if (bDecision) {
                //if (!ValidateInputExportLC(false)) return;
                ReceiveAmendment(nRowIndex, oExportLC);
            }
            else {
                $(".amendmentDateFields").show();
                var oExportLC = $("#tblExportLCs").datagrid("getSelected");
                $("#winExportLC").icsWindow('open', "Edit ExportLC");
                RefreshExportLCLayout("btnEditExportLC");
                GetExportLCInformation(oExportLC);
                SetAmendmentDate("btnAddAmendmentRecd");
            }

        }
        else {
            $(".amendmentDateFields").show();
            var oExportLC = $("#tblExportLCs").datagrid("getSelected");
            $("#winExportLC").icsWindow('open', "Edit ExportLC");
            RefreshExportLCLayout("btnEditExportLC");
            GetExportLCInformation(oExportLC);
            SetAmendmentDate("btnAddAmendmentRecd");
        }
    });

    $("#btnAdvanceSearchLC").click(function () {
        $("#winAdvSearchExportLC").icsWindow("open", "Advance Search");
        ResetAdvSearch();
        DynamicRefreshList([], "tblExportLCAdvSearch");
    });

    $("#btnAddAmendmentRequest").click(function () {
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }
      
        $("#winExportLCAmendmentRequests").icsWindow('open', "View ExportLC");

        PickExportLCAmendmentRequests(oExportLC);
    });

    $("#btnAddExportLCAmendmentRequest").click(function () {
        _oExportLC = $("#tblExportLCs").datagrid("getSelected");
        var oExportLCAmendmentRequest = NewExportLCAmendmentRequest();
        $("#winExportLCAmendmentRequest").icsWindow('open', "ExportLCAmendmentClause");
        Gets_ExportLCAmendmentClauses(oExportLCAmendmentRequest);
        $("#txtNewAmendmentCaluse").focus();
        $('#tblExportLCAmendmentRequest').datagrid('selectRow', -1);
    });

    $("#btnEditExportLCAmendmentRequest").click(function () {
        _oExportLC = $("#tblExportLCs").datagrid("getSelected");
        var oExportLCAmendmentRequest = $("#tblExportLCAmendmentRequests").datagrid("getSelected");
        
        $("#winExportLCAmendmentRequest").icsWindow('open', "ExportLCAmendmentClause");
        Gets_ExportLCAmendmentClauses(oExportLCAmendmentRequest);
        $("#txtNewAmendmentCaluse").focus();
        $('#tblExportLCAmendmentRequest').datagrid('selectRow', -1);
    });

    $("#btnDeleteExportLCAmendmentRequest").click(function () {
        var oExportLCAmendmentRequest = $("#tblExportLCAmendmentRequests").datagrid("getSelected");
        if (oExportLCAmendmentRequest == null || oExportLCAmendmentRequest.ExportLCAmendmentRequestID <= 0) {
            alert("Please select an item from list!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return false;
       
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportLCAmendmentRequest,
            ObjectId: oExportLCAmendmentRequest.ExportLCAmendmentRequestID,
            ControllerName: "ExportLC",
            ActionName: "Delete_AmendmentRequest",
            TableId: "tblExportLCAmendmentRequests",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $("#btnPrintExportLCAmendmentRequest").click(function () {
        var oExportLCAmendmentRequest = $("#tblExportLCAmendmentRequests").datagrid("getSelected");
        if (oExportLCAmendmentRequest == null || oExportLCAmendmentRequest.ExportLCAmendmentRequestID <= 0) {
            alert("Please select an item from list!");
            return;
        }
        $("#winExportLCAmendmentRequest").icsWindow('close');
        window.open(_sBaseAddress + '/ExportLC/PrintAmendmentRequest?id=' + oExportLCAmendmentRequest.ExportLCAmendmentRequestID, "_blank");
    });
    ///End LCAmendmentRequest
    $("#btnExportLCStatusUpdate").click(function () {
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportLCStatusUpdate").icsWindow('open', "Export L/C Status Change");

        GetExportLCInformation(oExportLC);
    });

    $("#btnCloseExportLCStatusUpdate").click(function () {
        $("#winExportLCStatusUpdate").icsWindow("close");
    });

    $("#btnSearchLC").click(function () {
        var oExportLC = {
            IsRecDateSearch: true,
            LCRecivedDate: $('#txtLCRecDate').datebox('getValue'),
            BUID: _nBUID,
            ExportLCNo: ""
        }
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportLC,
            ControllerName: "ExportLC",
            ActionName: "GetbyLCNo",
            IsWinClose: false
        };
        $.icsDataGets(obj, function (response) {
            if (response.status && response.objs.length > 0) {
                if (response.objs.length > 0) {
                    DynamicRefreshList(response.objs, "tblExportLCs");
                }
                else {
                    DynamicRefreshList([], "tblExportLCs");
                    alert("No List Found.");
                }
            } else {
                DynamicRefreshList([], "tblExportLCs");
                alert("No List Found.");
            }
        });
    });

    $("#btnUpdatePartyInfo").click(function () {
        var tsv = ((new Date()).getTime()) / 1000;
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || parseInt(oExportLC.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }
        sessionStorage.setItem("BackLink", window.location.href);
        window.open(sessionStorage.getItem("BaseAddress") + '/ExportPartyInfo/ViewPartyWiseExportInfo?pid=' + parseInt(oExportLC.ApplicantID) + '&ts=' + tsv, '_blank');
        //window.location.href = sessionStorage.getItem("BaseAddress") + "/ExportPartyInfo/ViewPartyWiseExportInfo?pid=" + parseInt(oExportLC.ApplicantID) + "&ts=" + tsv;
    });
    //// Start Export DOC TR
    $("#btnOkExportTR").click(function () {
        var oExportTR = $("#tblExportTRs").icsGetSelectedItem();
        if (oExportTR != null && oExportTR.ExportTRID > 0) {
            _nExportTRID = oExportTR.ExportTRID;
            RefreshControl_ExportTR(oExportTR);
        }
    });

    $("#btnExportDocTnC").click(function () {
        var oExportLC = $("#tblExportLCs").datagrid("getSelected");
        if (oExportLC == null || oExportLC.ExportLCID <= 0) { alert("Please select an item from list!"); return; }
        _oExportLC = oExportLC;
        $("#winExportDocTnC").icsWindow('open', "Export LC format Setup");
        GetFormatInformation(oExportLC);
    });
    $("#btnCloseExportTR").click(function () {
        $("#winExportTR").icsWindow("close");
    });
    $("#btnCloseExportDocTnC").click(function () {
        $("#winExportDocTnC").icsWindow("close");
        $("#winExportDocTnC input").val("");
        $("#winExportDocTnC select").val(0);
    });
    $("#btnPickExportTruckReceipt").click(function () {
        PickExportTR();
    });

    $("#btnSaveExportDocTnC").click(function (e) {
        // if (!ValidateInputExportBill()) return;
        //ratin
        endEditing();
        var oExportDocTnC = {
            ExportLCID: (_oExportLC != null) ? _oExportLC.ExportLCID : 0,
            ExportDocTnCID: (_oExportDocTnC!= null) ? _oExportDocTnC.ExportDocTnCID:0,
            ExportTRID: _nExportTRID,
            IsPrintGrossNetWeight: $("#chkIsPrintGrossNetWeight").is(':checked'),
            IsPrintOriginal: $("#chkIsPrintOriginal").is(':checked'),
            DeliveryBy: $("#txtDeliveryBy").val(),
            IsDeliveryBy: $("#chkIsDeliveryBy").is(':checked'),
            Term: $("#txtTerm").val(),
            IsTerm: $("#chkIsTerm").is(':checked'),
            MeasurementCarton: $("#txtMeasurementCarton").val(),
            PerCartonWeight: $("#txtPerCartonWeight").val(),
            ExportPartyInfoBills: $('#tblPartyInfo').datagrid('getChecked'),
            ExportDocForwardings: $('#tblExportDocForwarding').datagrid('getChecked')
        };

        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportDocTnC,
                ObjectId: oExportDocTnC.ReferenceID,
                RefTypeInInt:2,//ExportLC
                ControllerName: "ExportDocTnC",
                ActionName: "Save",
                TableId: "tblExportLCs",
                IsWinClose: true
            };
        $.icsSave(obj);
    });

}


function PickExportTR() {
    var oExportTR = {
        ExportTRID: 0,
        BUID: _nBUID
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oExportTR,
        ControllerName: "ExportTR",
        ActionName: "PickExportTRs",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblExportTRs");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ExportTRID > 0) {
                _oExportTR = []; _oExportTR = response.objs;
                DynamicRefreshList(response.objs, "tblExportTRs");
                $("#winExportTR").icsWindow('open', "Export TR Search");
                $("#divExportTR").focus();
                $("#winExportTR input").val("");
            }
            else {
                _oExportTR = [];
                DynamicRefreshList([], "tblExportTRs");
                alert(response.objs[0].ErrorMessage);
                // $("#txtSearchByAccountOfAdvSearch").removeClass("fontColorOfPickItem");
                _nExportTRID = 0;
            }
        }
        else {
            alert("Sorry, No data found, Try again.");
            // $("#txtSearchByAccountOfAdvSearch").removeClass("fontColorOfPickItem");
            _nExportTRID = 0;
        }
    });
}

function RefreshControl_ExportTR(oExportTR) {

    $("#txtTruckReceiptNo").val(oExportTR.TruckReceiptNo);
    $("#txtTruckReceiptDate").val(oExportTR.TruckReceiptDateInString);
    $("#txtCarrier").val(oExportTR.Carrier);
    $("#txtTruckNo").val(oExportTR.TruckNo);
    $("#txtDriverName").val(oExportTR.DriverName);

}

function NewExportLCAmendmentRequest() {

    var oExportLCAmendmentRequest = {
        ExportLCAmendmentRequestID: 0,
        ExportLCID: _oExportLC.ExportLCID
    };
    return oExportLCAmendmentRequest;
}

function Gets_ExportLCAmendmentClauses(oExportLCAmendmentRequest) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportLCAmendmentRequest,
        ControllerName: "ExportLC",
        ActionName: "GetsExportLCAmendmentClauses",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshListForMultipleSelection(response.objs, "tblExportLCAmendmentRequest");
                $('#tblExportLCAmendmentRequest').datagrid({ selectOnCheck: false, checkOnSelect: true });
            }
            else { DynamicRefreshListForMultipleSelection([], "tblExportLCAmendmentRequest"); }

        }
    });
}

function PickExportLCAmendmentRequests(oExportLC) {
    
    var ExportLCAmendmentRequest = {
        ExportLCID: oExportLC.ExportLCID
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: ExportLCAmendmentRequest,
        ControllerName: "ExportLC",
        ActionName: "GetsExportLCAmendmentRequests",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblExportLCAmendmentRequests");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ExportLCAmendmentRequestID > 0) { _oExportLCAmendmentRequests = []; _oExportLCAmendmentRequests = response.objs; DynamicRefreshList(response.objs, "tblExportLCAmendmentRequests"); }
            else { _oExportLCAmendmentRequests = []; DynamicRefreshList([], "tblExportLCAmendmentRequests"); alert(response.objs[0].ErrorMessage); }
       
            $("#divExportLCAmendmentRequests").focus();
            $("#winExportLCAmendmentRequests input").not("input[type='button']").val("");
        }
    });
}


function Gets_byLCNo(oExportLC) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportLC,
        ControllerName: "ExportLC",
        ActionName: "GetbyLCNo",
        IsWinClose: false
    };
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblExportLCs");
            }
            else {
                DynamicRefreshList([], "tblExportLCs");
            }
        } else {
            DynamicRefreshList([], "tblExportLCs");
        }
    });
}

function GetExportLCInformation(oExportLC) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportLC,
        ControllerName: "ExportLC",
        ActionName: "GetExportLC",
        IsWinClose: false
    };
    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.ExportLCID > 0) { RefreshExportLCControl(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else { alert("No information found."); }
    });
}

function GetExportLCInformation_ExportBill(oExportLC) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportLC,
        ControllerName: "ExportLC",
        ActionName: "GetExportLC_ExportBill",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            
            if (response.obj.ExportLCID > 0) { RefreshExportLCControl_ExportBill(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else { alert("No information found."); }
    });
}

function RefreshExportLCLayout(buttonId) {
    if (buttonId == "btnViewExportLC") {
        $("#txtVat").val("");
        $("#txtReg").val("");
        $("#winExportLC").find("input, select").prop("disabled", true);
        $("#btnSaveExportLC,#btnSaveExportLCTnC").hide();

    }
    else if (buttonId == "btnAddBills") {
        $("#winExportLC").find("input, select").prop("disabled", false);
        $("#tblExportBills_Info").prop("disabled", true);        
    }    
    else {
        $("#txtVat").val("");
        $("#txtReg").val("");
        $("#winExportLC input").prop("disabled", false);
        $("#winExportLC select").prop("disabled", false);
        $("#btnSaveExportLC,#btnSaveExportLCTnC").show();
        //$("#txtFileNo").prop("disabled", true);
        $("#txtAmount").prop("disabled", true);
        $("#txtLCStatusInString").prop("disabled", true);        
    }

    //if (buttonId == "btnAddAmendmentRecd") {
    //    $("#btnAmendmentReceiveSaveExportLC").show();
    //    $("#btnSaveExportLC").hide();
    //} else {
    //    $("#btnAmendmentReceiveSaveExportLC").hide();
    //    $("#btnSaveExportLC").show();
    //}


    $("#txtContractorName,#txtDeliverTo,#txtSearchByBankBranch_Issue").removeClass("fontColorOfPickItem");
    $("#txtLCStatusInString,#txtAmount").prop("disabled", true);
    $("#cboBenificiary,#cboBenificiary01").val(_nBUID);
    $(".allTimeDisbaled").prop("disabled", true);
    $("#txtNegoDate").prop("disabled", true);
    $("#txtNegoDate").val("");
    
}

function ReceiveAmendment(nRowIndex1, oExportLC) {

    oExportLC.AmendmentDate = oExportLC.AmendmentDateSt;
    oExportLC.LCRecivedDate = oExportLC.LCRecivedDateST;
    oExportLC.OpeningDate = oExportLC.OpeningDateST;
    oExportLC.ShipmentDate = oExportLC.ShipmentDateST;
    oExportLC.ExpiryDate = oExportLC.ExpiryDateST;

    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportLC,
        ObjectId: oExportLC.ExportLCID,
        ControllerName: "ExportLC",
        ActionName: "SaveAmendment",
        TableId: "tblExportLCs",
        IsWinClose: false,
        Message: ""
    };
    $.icsSave(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj != null && response.obj.ExportLCID > 0) {
                if ($.trim(response.obj.ErrorMessage) == "")
                {
                    if (response.obj.ExportLCID > 0) {
                        $("#tblExportLCs").datagrid("updateRow", { index: nRowIndex1, row: response.obj });
                        OperationPerformsExportLC(nRowIndex1, response.obj);
                        $("#winExportLC").icsWindow('open', "Amendment Receive");
                        RefreshExportLCLayout("btnAddAmendmentRecd");
                        GetExportLCInformation(oExportLC);
                    }
                }
            }
        }
    });
}

function RefreshExportLCControl(oExportLC) {
    _oExportLC = oExportLC;
    DynamicRefreshList(_oExportLC.ExportPILCMappings, "tblExportPILCMappings");
    DynamicRefreshList(_oExportLC.MasterLCMappings, "tblMasterLCs");

    $("#txtAmount").prop("disabled", true);
    $("#txtLCStatusInString").prop("disabled", true);
    
    if (_oExportLC.LiborRate == true) {
        $('#chkLIBORRateYes').prop('checked', true);
        $('#chkLIBORRateNo').prop('checked', false);
    } else {
        $('#chkLIBORRateYes').prop('checked', false);
        $('#chkLIBORRateNo').prop('checked', true);
    }
    if (_oExportLC.BBankFDD == true) {
        $('#chkBankFDDYes').prop('checked', true);
        $('#chkBankFDDNo').prop('checked', false);
    } else {
        $('#chkBankFDDYes').prop('checked', false);
        $('#chkBankFDDNo').prop('checked', true);
    }
    if (_oExportLC.PartialShipmentAllowed == true) {
        $('#chkPartialShipmentAllow').prop('checked', true);
        $('#chkPartialShipmentNotAllow').prop('checked', false);
    } else {
        $('#chkPartialShipmentAllow').prop('checked', false);
        $('#chkPartialShipmentNotAllow').prop('checked', true);
    }
    if (_oExportLC.TransShipmentAllowed == true) {
        $('#chkTransshipmentAllow').prop('checked', true);
        $('#chkTransshipmentNotAllow').prop('checked', false);
    } else {
        $('#chkTransshipmentAllow').prop('checked', false);
        $('#chkTransshipmentNotAllow').prop('checked', true);
    }
    if (_oExportLC.IsForeignBank == true) {
        $('#chkIsBFDD_Foreign').prop('checked', true);
        $('#chkIsBFDD_Local').prop('checked', false);
    } else {
        $('#chkIsBFDD_Foreign').prop('checked', false);
        $('#chkIsBFDD_Local').prop('checked', true);
    }

    $("#txtFileNo").val(_oExportLC.FileNo);
    $("#txtExportLCNo").val(_oExportLC.ExportLCNo);
    $("#txtNegoDays").val(_oExportLC.NegoDays);
    $("#txtAmount").val(_oExportLC.AmountSt);
    $("#lblTotalValue_LC").text(_oExportLC.AmountSt);
    $("#txtNote").val(_oExportLC.Remark);
    $("#txtHSCode").val(_oExportLC.HSCode);
    $("#txtAreaCode").val(_oExportLC.AreaCode);
    $("#txtLCStatusInString").val(_oExportLC.CurrentStatusInST);
    $("#txtLCStatus_Update").val(_oExportLC.CurrentStatusInST);
    debugger;
    if (_oExportLC.OpeningDateST != "-") {
        $('#txtOpeningDate').datebox('setValue', _oExportLC.OpeningDateST);
    }
    if (_oExportLC.LCRecivedDateST != "-") {
        $('#txtReceiveDate').datebox('setValue', _oExportLC.LCRecivedDateST);
    }
    if (_oExportLC.ShipmentDateST != "-") {
        $('#txtShipmentDate').datebox('setValue', _oExportLC.ShipmentDateST);
    }
    if (_oExportLC.ExpiryDateST != "-") {
        $('#txtExpireDate').datebox('setValue', _oExportLC.ExpiryDateST);
    }
    $("#cboBankBranch_Advice").val(_oExportLC.BBranchID_Advice);
    $("#cboBankBranch_Nego").val(_oExportLC.BBranchID_Nego);
    $("#cboCurrency").val(_oExportLC.CurrencyID);
    $("#cboLCTerm").val(_oExportLC.LCTramsID);
    $("#cboBenificiary").val(1); // for one company , if multi company please change it
    $('#txtOverDueRate').numberbox('setValue', _oExportLC.OverDueRate);
    $('#txtDiscrepancyCharge').numberbox('setValue', _oExportLC.DCharge);
    $("#txtShipmentFrom").val(_oExportLC.ShipmentFrom);
    $("#txtGarmentsQty").val(_oExportLC.GarmentsQty);
    $("#txtIRC").val(_oExportLC.IRC);
    $("#txtERC").val(_oExportLC.ERC);
    $("#cboPaymentInstruction").val(_oExportLC.PaymentInstruction);
    $("#txtFrightPrepaid").val(_oExportLC.FrightPrepaid);
    if (_oExportLC.ExportLCID > 0) {
        $("#txtSearchByBankBranch_Issue").val(_oExportLC.BankName_Issue + "[" + _oExportLC.BBranchName_Issue + "]");
        $("#txtContractorName").val(_oExportLC.ApplicantName);
        $("#txtContractorName").addClass("fontColorOfPickItem");
    }
    else
    {
        $("#txtContractorName").removeClass("fontColorOfPickItem");
        
        $("#txtSearchByBankBranch_Issue,#txtContractorName,#txtDeliverTo").val();
    }

    if (_oExportLC.DeliveryToID > 0) {
        $("#txtDeliverTo").val(_oExportLC.DeliveryToName);
        $("#txtDeliverTo").addClass("fontColorOfPickItem");
    } else {
        $("#txtDeliverTo").removeClass("fontColorOfPickItem");
        $("#txtDeliverTo").val("");
    }


    $("#txtAmendmentDate").datebox('setValue', _oExportLC.AmendmentDateSt);
    CalculateTotalEPIDetail();
    $("#cboBenificiary").val(1);

    GetContractorInfoELC(oExportLC.ApplicantID);

    CheckDisburseDate(_oExportLC.NegoDays, _oExportLC.ExportLCID);

}


function GetContractorInfoELC(nApplicantID)
{
    var oContractor = {
        ContractorID: nApplicantID,
    };
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "Get",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.ContractorID > 0) {
                $("#txtVat").val(response.obj.VAT);
                $("#txtReg").val(response.obj.TIN);
                AddVatRegBtnHideShow();
            }
        }
    });
}

function RefreshExportLCControl_ExportBill(oExportLC) {
    
    debugger;
    _oExportLC = oExportLC;
    
    if (_oExportLC.ExportBills.length > 0) {
        DynamicRefreshList(_oExportLC.ExportBills, "tblExportBills");
    }
    //RefreshTotalSummery();
    $("#txtAmount").prop("disabled", true);
    $("#txtLCStatusInString").prop("disabled", true);

 

    $("#txtFileNo01").val(_oExportLC.FileNo);
    $("#txtExportLCNo01").val(_oExportLC.ExportLCNo);
    $("#txtAmount01").val(_oExportLC.AmountSt);
    $("#txtNote01").val(_oExportLC.Remark);
    $("#txtLCStatusInString01").val(_oExportLC.CurrentStatusInST);


    $('#txtOpeningDate01').datebox('setValue', _oExportLC.OpeningDateST);
    $('#txtReceiveDate01').datebox('setValue', _oExportLC.LCRecivedDateST);
    $('#txtShipmentDate01').datebox('setValue', _oExportLC.ShipmentDateST);
    $('#txtExpireDate01').datebox('setValue', _oExportLC.ExpiryDateST);
    $("#txtBankBranch_Advice01").val(_oExportLC.BankName_Advice);
    $("#txtBankBranch_Nego01").val(_oExportLC.BankName_Nego);
    $("#txtCurrency01").val(_oExportLC.CurrencyName);
    $("#txtNegoDays01").val(_oExportLC.NegoDays);
    
    $("#cboBenificiary01").val(1); // for one company , if multi company please change it

    SetNegoDate01(_oExportLC.NegoDays, $('#txtShipmentDate01').datebox('getValue'));
    
    if (_oExportLC.ExportLCID > 0) {
        document.getElementById('txtSearchByBankBranch_Issue01').value = _oExportLC.BankName_Issue + "[" + _oExportLC.BBranchName_Issue + "]";
        document.getElementById('txtContractorName01').value = _oExportLC.ApplicantName;
        var txtApplicantName = document.getElementById("txtContractorName01");
        txtApplicantName.style.color = "blue";
        txtApplicantName.style.fontWeight = "bold";
    }
    RefreshTotalSummery_EBilll();
    debugger;
    SetRemainingBillAmountLC(oExportLC);
    CheckDisburseDate01(_oExportLC.NegoDays, _oExportLC.ExportLCID);
}

function RefreshTotalSummery_EBilll() {
    
    var nTotalQty = 0;
    var nTotalValue = 0;
    var sCurrency = "";
    var sMUnit = "";
    var oExportBills = $('#tblExportBills').datagrid('getRows');
    for (var i = 0; i < oExportBills.length; i++) {
      
        nTotalValue = nTotalValue + parseFloat(oExportBills[i].Amount);
        sCurrency = oExportBills[i].Currency;
        
    }

    //document.getElementById("lblTotalQty_Bill").innerHTML = formatPrice(nTotalQty) + "" + sMUnit;
    document.getElementById("lblTotalValue_Bill").innerHTML = sCurrency + "" + formatPrice(nTotalValue);
}

function NewExportLCObj()
{
    
    var oExportLC = {
        ExportLCID           :0,
        ExportLCNo           :"",
        FileNo               :"",
        OpeningDate          : new Date(),
        BBranchID_Advice     :0,
        BBranchID_Nego       :0,
        BBranchID_Issue      :0,
        ApplicantID          :0,
        CurrencyID           :0,
        ShipmentDate        : new Date(),
        ExpiryDate          : new Date(),
        LCRecivedDate       : new Date(),
        LiborRate   :false,
        BBankFDD    :false,
        OverDueRate :0,
        FECircular  :"",
        FrightPrepaid:"",
        LCTramsID    :0,
        DCharge      :0,
        IsForeignBank   :false,
        TransShipmentAllowed :false,
        Amount: 0,
        PartialShipmentAllowed:false,
        ShipmentFrom: "",
        ExportPILCMappings: [],
        MasterLCMappings:[],
        Remark: "",
        OpeningDateST: icsdateformat(new Date()),
        LCRecivedDateST: icsdateformat(new Date()),
        ShipmentDateST: icsdateformat(new Date()),
        ExpiryDateST: icsdateformat(new Date()),
        AmountSt:'',
        CurrentStatusInST:""
    };
    return oExportLC;
}

function UpdatePIGetOrginalCopy(oExportLC, sSuccessMessage, nRowIndex) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/ExportLC/UpdateForGetOrginalCopy",
        traditional: true,
        data: JSON.stringify(oExportLC),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            //
            var oExportLC = jQuery.parseJSON(data);
            if (oExportLC.ErrorMessage == "" || oExportLC.ErrorMessage == null) {
                alert(sSuccessMessage);
                $('#tblExportLCs').datagrid('updateRow', { index: nRowIndex, row: oExportLC });
            } else {
                alert(oExportLC.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function ApprovedLC(oExportLC, sSuccessMessage, nRowIndex) {
   var obj =
   {
       BaseAddress: _sBaseAddress,
       Object: oExportLC,
       ObjectId: oExportLC.ExportLCID,
       ControllerName: "ExportLC",
       ActionName: "Approved",
       TableId: "tblExportLCs",
       IsWinClose: false,
       Message: ""
   };
    $.icsSave(obj, function (response) {
        if (response.status && response.obj != null) {
            var oELC = response.obj;
            if (oELC.ErrorMessage == "" || oELC.ErrorMessage == null) {
                alert(sSuccessMessage);
                $('#tblExportLCs').datagrid('updateRow', { index: nRowIndex, row: oELC });
                OperationPerformsExportLC(nRowIndex, oELC);
            } else {
                alert(oELC.ErrorMessage);
            }
        }
    });
}


function FieldDisableEnabledELC(sBtnId) {
    if (sBtnId == "btnAddBills") {
        $(".trAddBill").find("input, select").prop("disabled", true);
        $(".trAddBill .easyui-datebox").datebox({ disabled: true });
    }
    else {
        $(".trAddBill").find("input, select").prop("disabled", false);
        $(".trAddBill .easyui-datebox").datebox({ disabled: false });
    }
    $("#txtFileNo01,#txtLCStatusInString01,#txtAmount01").prop("disabled", true);
}

function OperationPerformsExportLC(rowIndex, oExportLC) {
    $("#btnEditExportLC,#btnDeleteExportLC,#btnApproveLC,#btnAddAmendmentRequest,#btnAddAmendmentRecd,#btnGetOrginalCopy,#btnAddBills,#btnExportLCStatusUpdate").show();
    if (oExportLC != null && oExportLC.ExportLCID > 0) {
        var nCurrentObjStatus = parseInt(oExportLC.CurrentStatusInInt);
        if (nCurrentObjStatus == 0 || nCurrentObjStatus == 1) //None(Initialize) || FreshLC
        {
            $("#btnAddAmendmentRecd,#btnAddBills").hide();
        }
        else if (nCurrentObjStatus == 2) //Approved
        {
            $("#btnEditExportLC,#btnDeleteExportLC,#btnApproveLC").hide();
            $("#btnAddAmendmentRecd").show();
        }
        else if (nCurrentObjStatus == 3) //RequestForAmendment
        {
            $("#btnEditExportLC,#btnDeleteExportLC,#btnApproveLC").hide();
        }
        else if (nCurrentObjStatus == 4) //AmendmentReceive
        {
            $("#btnDeleteExportLC").hide();
        }
        else if (nCurrentObjStatus == 5) //OutstandingLC
        {
            $("#btnEditExportLC,#btnDeleteExportLC,#btnApproveLC").hide();
        }
        else if (nCurrentObjStatus == 6) //LC_Close
        {
            $("#btnEditExportLC,#btnDeleteExportLC,#btnApproveLC,#btnAddBills,#btnAddAmendmentRequest,#btnAddAmendmentRecd").hide();
        }
        else if (nCurrentObjStatus == 7) //LC_Cancel
        {
            $("#btnEditExportLC,#btnDeleteExportLC,#btnApproveLC,#btnAddBills,#btnAddAmendmentRequest,#btnAddAmendmentRecd").hide();
        }
    }
}

function SetAmendmentDate(btnId) {
    if (btnId == "btnAddAmendmentRecd") {
        $("#txtAmendmentDate").datebox({ disabled: false });
        $("#txtAmendmentDate").datebox("setValue", icsdateformat(new Date()));
    }
    else {
        $("#txtAmendmentDate").datebox({ disabled: false });
        $("#txtAmendmentDate").datebox("setValue", "");
    }

}


/// Export Document TnC
function GetFormatInformation(oExportLC) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportLC,
        ControllerName: "ExportDocTnC",
        ActionName: "Get_ExportDocTnCSetup",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {

        if (response.status && response.obj != null) {
            if (response.obj.ExportLCID > 0) { RefreshControl_ExportLCFormatInfo(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function RefreshControl_ExportLCFormatInfo(oExportDocTnC) {
    _oExportLC = oExportDocTnC.ExportLC;
    _oExportDocTnC = oExportDocTnC;
    _nExportTRID = oExportDocTnC.ExportTRID;
    $("#txtTruckReceiptNo").val(oExportDocTnC.TruckReceiptNo);
    $("#txtTruckReceiptDate").val(oExportDocTnC.TruckReceiptDateInString);
    $("#txtCarrier").val(oExportDocTnC.Carrier);
    $("#txtTruckNo").val(oExportDocTnC.TruckNo);
    $("#txtDriverName").val(oExportDocTnC.DriverName);
    $("#txtDeliveryBy").val(oExportDocTnC.DeliveryBy);
    $("#txtTerm").val(oExportDocTnC.Term);
    $("#chkIsPrintGrossNetWeight").prop("checked", oExportDocTnC.IsPrintGrossNetWeight);
    $("#chkIsPrintOriginal").prop("checked", oExportDocTnC.IsPrintOriginal);
    $("#chkIsDeliveryBy").prop("checked", oExportDocTnC.IsDeliveryBy);
    $("#chkIsTerm").prop("checked", oExportDocTnC.IsTerm);
    $("#txtMeasurementCarton").val(oExportDocTnC.MeasurementCarton);
    $("#txtPerCartonWeight").val(oExportDocTnC.PerCartonWeight);
    //forwarding
    data = oExportDocTnC.ExportDocForwardings;
    data = { "total": "" + data.length + "", "rows": data };
    $('#tblExportDocForwarding').datagrid('loadData', data);
    CheckExistingForwardingInfo();
    //party info set
    data = oExportDocTnC.ExportPartyInfoBills;
    data = { "total": "" + data.length + "", "rows": data };
    $('#tblPartyInfo').datagrid('loadData', data);
    CheckExistingPartyInfo();

    //  $('#txtPerCartonWeight').numberbox('setValue', oExportDocTnC.PerCartonWeight);
}
function CheckExistingPartyInfo() {
    var oExportPartyInfoBills = $('#tblPartyInfo').datagrid('getRows');
    if (oExportPartyInfoBills.length > 0) {
        for (var i = 0; i < oExportPartyInfoBills.length; i++) {
            if (Boolean(oExportPartyInfoBills[i].Selected)) {
                $('#tblPartyInfo').datagrid('checkRow', i);
            }
        }
    }
}


function CheckExistingForwardingInfo() {
    var oExportDocForwardings = $('#tblExportDocForwarding').datagrid('getRows');
    if (oExportDocForwardings.length > 0) {
        for (var i = 0; i < oExportDocForwardings.length; i++) {
            if (Boolean(oExportDocForwardings[i].Selected)) {
                $('#tblExportDocForwarding').datagrid('checkRow', i);
            }
        }
    }
}

