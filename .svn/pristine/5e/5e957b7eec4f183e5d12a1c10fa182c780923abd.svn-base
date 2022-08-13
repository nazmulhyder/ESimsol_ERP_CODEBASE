var _oBankForwardingList = [];
function LoadExportBillsEvents()
{
    BtnPrintFormatEB();
    WinPrintFormatCheckBoxControlEB();
    DynamicRefreshList(_oExportBills, "tblExportBills");

    
    
    $("#btnAddSendToParty").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBillSendToParty").icsWindow('open', "Export Bill (Send To Party)");
     //   RefreshExportBillLayout("btnAddSendToParty");
        
        GetExportBillInformation(oExportBill);
    });
   
    $("#btnAddRecedFromParty").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBillRecdFromParty").icsWindow('open', "Export Bill (Receive From Party)");
     ///   RefreshExportBillLayout("btnAddRecedFromParty");
        
        GetExportBillInformation_ReceiveFromParty(oExportBill);
    });

    $("#btnAddSendToBank").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBillSendToBank").icsWindow('open', "Export Bill (Send to Bank)");
        ///   RefreshExportBillLayout("btnAddRecedFromParty");
        
        GetExportBillInformation_SendToBank(oExportBill);
    });
    $("#btnAddRecedfromBank").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBillReceiveFromBank").icsWindow('open', "Export Bill (Receive From Bank)");
        ///   RefreshExportBillLayout("btnAddRecedFromParty");
        
        GetExportBillInformation_RecdFromBank(oExportBill);
    });
    $("#btnAddMaturity").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBillMaturityReceive").icsWindow('open', "Export Bill (Maturity Receive)");
        ///   RefreshExportBillLayout("btnAddRecedFromParty");
        
        GetExportBillInformation_MaturityReceive(oExportBill);
    });
    $("#btnAddBillRelization").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBillRelization").icsWindow('open', "Export Bill (Payment Receive)");
        ///   RefreshExportBillLayout("btnAddRecedFromParty");
        
        GetExportBillInformation_BillRelization(oExportBill);
    });
    $("#btnAddBankFDDRecd").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBillBBankFDDReceive").icsWindow('open', "Export Bill (Bank FDD Receive)");
        ///   RefreshExportBillLayout("btnAddRecedFromParty");
        
        GetExportBillInformation_BFDDReceive(oExportBill);
    });

    $("#btnAddEncashment").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportBillEncashmen").icsWindow('open', "Export Bill (Encashment Receive)");
        ///   RefreshExportBillLayout("btnAddRecedFromParty");
        
        GetExportBillInformation_EncashmentReceive(oExportBill);
    });
    // End ExportDocForwading
    
    $("#btnExportBillHistory").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportDBillHistory").icsWindow('open', "Export Bill History");        
        Gets_ExportBillHistory(oExportBill);
    });
    
    $("#btnPrintDoc").click(function () {
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || oExportBill.ExportBillID <= 0) { alert("Please Select an item from list!"); return; }
        GetExportDocs(oExportBill.ExportLCID);
        BtnPrintFormatEB();
        $("#rdoInLbs").prop("checked", true);
        $("#rdoInA4").prop("checked", true);
        $("#winPrintFormat").icsWindow("open", "Print Format");
    });

    $("#btnPrintList").click(function () {
        var oExportBills = $('#tblExportBills').datagrid('getRows');
        if (oExportBills.length <= 0) {
            alert("Sorry, there is no data to Print");
        }
        var ids = ICS_PropertyConcatation(oExportBills, 'ExportBillID');
        var tsv = ((new Date()).getTime()) / 1000;
        var buid= parseInt(sessionStorage.getItem('BUID'));
        window.open(_sBaseAddress + "/ExportBill/PrintList?ids=" + ids + "&buid=" + buid + "&ts=" + tsv);
    });

    $("#btnExportToXL").click(function () {
        var oExportBills = $('#tblExportBills').datagrid('getRows');
        if (oExportBills.length <= 0) {
            alert("Sorry, there is no data to Print");
        }
        var ids = ICS_PropertyConcatation(oExportBills, 'ExportBillID');
        var tsv = ((new Date()).getTime()) / 1000;
        var buid = parseInt(sessionStorage.getItem('BUID'));
        window.open(_sBaseAddress + "/ExportBill/ExportToXL?ids=" + ids + "&buid=" + buid + "&ts=" + tsv);
    });
    
    $('#txtSearchbyLDBCNo').keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {
            var oExportBill =
            {
                ExportBillID: 0,
                BUID: _nBUID,
                LDBCNo: document.getElementById('txtSearchbyLDBCNo').value
            };
            Gets_byLDBCNo(oExportBill);
        }
        else if (code === 8) {
            DynamicRefreshList(_oExportBills, "tblExportBills");
        }
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
            var oExportBill =
            {
                ExportBillID: 0,
                BUID: _nBUID,
                ExportLCNo: $.trim($("#txtSearchbyLCNo").val())
            };
            Gets_byLCNo(oExportBill);
        }
        else if (code === 8) {
            DynamicRefreshList(_oExportBills, "tblExportBills");
        }
    });

    $('#txtSearchbyBillNo').keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)//Enter key-13
        {

            if ($.trim($("#txtSearchbyBillNo").val()) == "") {
                alert("Please give Bill No.");
                return false;
            }
            var oExportBill = {
                ExportBillID: 0,
                BUID: _nBUID,
                ExportBillNo: $.trim($("#txtSearchbyBillNo").val())
            };
            var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportBill,
                ControllerName: "ExportBill",
                ActionName: "GetbyBillNo",
                IsWinClose: false
            };

            $.icsMaxDataGets(obj, function (response) {
                if (response.status && response.objs.length > 0)
                {
                    DynamicRefreshList(response.objs, "tblExportBills");
                }
                else
                {
                    alert("No List Found");
                    DynamicRefreshList([], "tblExportBills");
                }
            });
        }
        else if (code === 8)
        {
            DynamicRefreshList(_oExportBills, "tblExportBills");
        }
    });

    $("#btnAdvSearchExportBill").click(function () {
        $("#winAdvSearchExportBill").icsWindow("open", "Advance Search");
        DynamicRefreshList([], "tblExportBillAdvSearch");
        //DateActions();
        //ResetAdvSearch();
        //UnselectAllRowsOfATable();
    });

    $("#btnTemplateExportDocForwarding").click(function () {
        if (_oBankForwardingList.length > 0)
        {
            for (var i = 0; i < _oBankForwardingList.length; i++) {
                if (_oBankForwardingList[i].Name_Print.toUpperCase() == "PACKING & WEIGHT LIST") {
                    _oBankForwardingList[i].Copies = 6;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "COMMERCIAL INVOICE") {
                    _oBankForwardingList[i].Copies = 8;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "BILL OF EXCHANGE") {
                    _oBankForwardingList[i].Copies = 2;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "BENEFICIARY CERTIFICATE") {
                    _oBankForwardingList[i].Copies = 2;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "CERTIFICATE OF ORIGIN") {
                    _oBankForwardingList[i].Copies = 6;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "TRUCK RECEIPT") {
                    _oBankForwardingList[i].Copies = 4;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "DELIVERY CHALLAN") {
                    _oBankForwardingList[i].Copies = 4;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "TO WHOM IT MAY CONCERN") {
                    _oBankForwardingList[i].Copies = 2;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "MUSOK-11") {
                    _oBankForwardingList[i].Copies = 0;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "BACK TO BACK L/C(ORIGINAL)") {
                    _oBankForwardingList[i].Copies = 1;
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "BTMA CERTIFICATE") {
                    _oBankForwardingList[i].Copies = 0;
                }
            }
            DynamicRefreshList(_oBankForwardingList, "tblExportDocForwarding");

            for (var i = 0; i < _oBankForwardingList.length; i++)
            {
                if (_oBankForwardingList[i].Name_Print.toUpperCase() == "PACKING & WEIGHT LIST") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "COMMERCIAL INVOICE") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "BILL OF EXCHANGE") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "BENEFICIARY CERTIFICATE") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "CERTIFICATE OF ORIGIN") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "TRUCK RECEIPT") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "DELIVERY CHALLAN") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "TO WHOM IT MAY CONCERN") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "MUSOK-11") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "BACK TO BACK L/C(ORIGINAL)") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
                else if (_oBankForwardingList[i].Name_Print.toUpperCase() == "BTMA CERTIFICATE") {
                    $("#tblExportDocForwarding").datagrid("checkRow", i);
                }
            }
        }
    });

    $("#btnCloseExportLC").click(function () {
        $("#winExportBills").icsWindow("close");
    });

    $("#btnCloseExportLCTnCC").click(function () {
        $("#winExportLC").icsWindow("close");
    });

    $("#btnViewExportLCInBill").click(function () {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        $(".amendmentDateFields").hide();
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        if (oExportBill == null || parseInt(oExportBill.ExportLCID) <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportLC").icsWindow('open', "View Export LC");
        RefreshExportLCLayoutLC("btnViewExportLC");
        var oExportLC = {
            ExportLCID: oExportBill.ExportLCID
        }
        GetExportLCInformationLC(oExportLC);
        SetAmendmentDateLC("btnViewExportLC");
    });

}

function GetExportDocs(nExportLCID)
{
    var oExportBill = { ExportLCID: nExportLCID };
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "GetExportDocs",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            DynamicRefreshList(response.objs, "tblExportDocSetup");
        }
        else {
            alert("No List Found");
            DynamicRefreshList([], "tblExportDocSetup");
        }
    });
}


function SetAmendmentDateLC(btnId) {
    if (btnId == "btnAddAmendmentRecd") {
        $("#txtAmendmentDate").datebox({ disabled: false });
        $("#txtAmendmentDate").datebox("setValue", icsdateformat(new Date()));
    }
    else {
        $("#txtAmendmentDate").datebox({ disabled: false });
        $("#txtAmendmentDate").datebox("setValue", "");
    }
}

function RefreshExportLCLayoutLC(buttonId) {
    $("#txtVat").val("");
    $("#txtReg").val("");
    $("#winExportLC").find("input, select").prop("disabled", true);
    $("#btnSaveExportLC,#btnSaveExportLCTnC").hide();


    $("#txtSearchByContractor_ELC,#txtSearchByBankBranch_Issue").removeClass("fontColorOfPickItem");
    $("#txtFileNo,#txtLCStatusInString,#txtAmount").prop("disabled", true);
    $("#cboBenificiary,#cboBenificiary01").val(1);
    $(".allTimeDisbaled").prop("disabled", true);
    $("#txtNegoDate").prop("disabled", true);
    $("#txtNegoDate").val("");

}

function GetExportLCInformationLC(oExportLC) {
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

function RefreshExportLCControl(oExportLC) {
    DynamicRefreshList(oExportLC.ExportPILCMappings, "tblExportPILCMappings");
    DynamicRefreshList(oExportLC.MasterLCMappings, "tblMasterLCs");

    $("#txtAmount").prop("disabled", true);
    $("#txtLCStatusInString").prop("disabled", true);
    
    if (oExportLC.LiborRate == true) {
        $('#chkLIBORRateYes').prop('checked', true);
        $('#chkLIBORRateNo').prop('checked', false);
    } else {
        $('#chkLIBORRateYes').prop('checked', false);
        $('#chkLIBORRateNo').prop('checked', true);
    }
    if (oExportLC.BBankFDD == true) {
        $('#chkBankFDDYes').prop('checked', true);
        $('#chkBankFDDNo').prop('checked', false);
    } else {
        $('#chkBankFDDYes').prop('checked', false);
        $('#chkBankFDDNo').prop('checked', true);
    }
    if (oExportLC.PartialShipmentAllowed == true) {
        $('#chkPartialShipmentAllow').prop('checked', true);
        $('#chkPartialShipmentNotAllow').prop('checked', false);
    } else {
        $('#chkPartialShipmentAllow').prop('checked', false);
        $('#chkPartialShipmentNotAllow').prop('checked', true);
    }
    if (oExportLC.TransShipmentAllowed == true) {
        $('#chkTransshipmentAllow').prop('checked', true);
        $('#chkTransshipmentNotAllow').prop('checked', false);
    } else {
        $('#chkTransshipmentAllow').prop('checked', false);
        $('#chkTransshipmentNotAllow').prop('checked', true);
    }
    if (oExportLC.IsForeignBank == true) {
        $('#chkIsBFDD_Foreign').prop('checked', true);
        $('#chkIsBFDD_Local').prop('checked', false);
    } else {
        $('#chkIsBFDD_Foreign').prop('checked', false);
        $('#chkIsBFDD_Local').prop('checked', true);
    }


    $("#txtFileNo").val(oExportLC.FileNo);
    $("#txtExportLCNo").val(oExportLC.ExportLCNo);
    $("#txtNegoDays").val(oExportLC.NegoDays);
    $("#txtAmount").val(oExportLC.AmountSt);
    $("#lblTotalValue_LC").text(oExportLC.AmountSt);
    $("#txtNote").val(oExportLC.Remark);
    $("#txtHSCode").val(oExportLC.HSCode);
    $("#txtAreaCode").val(oExportLC.AreaCode);
    $("#txtLCStatusInString").val(oExportLC.CurrentStatusInST);
    $("#txtLCStatus_Update").val(oExportLC.CurrentStatusInST);
    if (oExportLC.OpeningDateST != "-") {
        $('#txtOpeningDate').datebox('setValue', oExportLC.OpeningDateST);
    }
    if (oExportLC.LCRecivedDateST != "-") {
        $('#txtReceiveDate').datebox('setValue', oExportLC.LCRecivedDateST);
    }
    if (oExportLC.ShipmentDateST != "-") {
        $('#txtShipmentDate').datebox('setValue', oExportLC.ShipmentDateST);
    }
    if (oExportLC.ExpiryDateST != "-") {
        $('#txtExpireDate').datebox('setValue', oExportLC.ExpiryDateST);
    }
    $("#cboBankBranch_Advice").val(oExportLC.BBranchID_Advice);
    $("#cboBankBranch_Nego").val(oExportLC.BBranchID_Nego);
    $("#cboTextileUnitExportLC").val(oExportLC.TextileUnitInInt);
    $("#cboCurrency").val(oExportLC.CurrencyID);
    $("#cboLCTerm").val(oExportLC.LCTramsID);
    $("#cboBenificiary").val(1);// for one company , if multi company please change it
    $('#txtOverDueRate').numberbox('setValue', oExportLC.OverDueRate);
    $('#txtDiscrepancyCharge').numberbox('setValue', oExportLC.DCharge);
    $("#txtShipmentFrom").val(oExportLC.ShipmentFrom);
    $("#txtGarmentsQty").val(oExportLC.GarmentsQty);
    $("#txtIRC").val(oExportLC.FECircular);
    $("#txtFrightPrepaid").val(oExportLC.FrightPrepaid);
    if (oExportLC.ExportLCID > 0) {
        $("#txtSearchByBankBranch_Issue").val(oExportLC.BankName_Issue + "[" + oExportLC.BBranchName_Issue + "]");
        $("#txtSearchByContractor_ELC").val(oExportLC.ApplicantName);
        $("#txtSearchByContractor_ELC").addClass("fontColorOfPickItem");
    }
    else
    {
        $("#txtSearchByContractor_ELC").removeClass("fontColorOfPickItem");
        $("#txtSearchByBankBranch_Issue,#txtSearchByContractor_ELC").val();
    }

    $("#txtAmendmentDate").datebox('setValue', oExportLC.AmendmentDateSt);
    CalculateTotalEPIDetail();

    GetContractorInfoELCLC(oExportLC.ApplicantID);

    CheckDisburseDateLC(oExportLC.NegoDays, oExportLC.ExportLCID);
    //SetNegoDate(oExportLC.NegoDays, $('#txtShipmentDate').datebox('getValue'));
    TextileUnitOffNoLC();
}

function GetContractorInfoELCLC(nApplicantID) {
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
                $("#txtVat").val(response.obj.Vat);
                $("#txtReg").val(response.obj.Registration);
                AddVatRegBtnHideShowLC();
            }
        }
    });
}

function AddVatRegBtnHideShowLC() {
    var sVal = $("#txtVat").val();
    var sRegistration = $("#txtReg").val();

    if ($.trim(sVal) == "" && $.trim(sRegistration) == "") {
        $("#btnAddNewVatReg").show();
    } else {
        $("#btnAddNewVatReg").hide();
    }
}

function CheckDisburseDateLC(nTotalDays, nExportLCID) {
    if (nExportLCID > 0) {
        var oExportLC = {
            ExportLCID: nExportLCID
        };

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportLC,
            ControllerName: "ExportLC",
            ActionName: "CheckSUDeliveryChallanDateForLC",
            IsWinClose: false
        };
        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.SUDeliveryChallanID > 0) {
                    SetNegoDate(nTotalDays, response.obj.ChallanDateSt);
                }
                else {
                    SetNegoDate(nTotalDays, $('#txtShipmentDate').datebox('getValue'));
                }
            }
            else {
                SetNegoDate(nTotalDays, $('#txtShipmentDate').datebox('getValue'));
            }
        });
    }
    else {
        SetNegoDate(nTotalDays, $('#txtShipmentDate').datebox('getValue'));
    }

}

function TextileUnitOffNoLC() {
    
    var oRows = $("#tblExportPILCMappings").datagrid("getRows");
    if (oRows.length > 0) {
        $("#cboTextileUnitExportLC").prop("disabled", true);
    } else {
        $("#cboTextileUnitExportLC").prop("disabled", false);
    }
}

function BtnPrintFormatEB() {
    $('#chkPrintNormal').prop('checked', true);
    $('#chkPrintTitleInPAD').prop('checked', false);
    $('#chkPrintTitleInImg').prop('checked', false);
}

function GetSelectedFormat()
{
    var nPrintType = 0;
    if ($("#chkPrintNormal").is(':checked')) {
        nPrintType = 1;
    }else if ($("#chkPrintTitleInPAD").is(':checked')) {
        nPrintType = 2;
    }else if ($("#chkPrintTitleInImg").is(':checked')) {
        nPrintType = 3;
    }
    return nPrintType;
}

function WinPrintFormatCheckBoxControlEB() {
    $("#btnClosePrintFormat").click(function () {
        $("#winPrintFormat").icsWindow("close");
    });

    $("#btnOkPrintFormat").click(function () {
        debugger;
        if (!($("#chkPrintTitleInImg").is(':checked')) && !($("#chkPrintTitleInPAD").is(':checked')) && !($("#chkPrintNormal").is(':checked')))
        {
            alert("Please select one format.");
            return false;
        }
        var nPrintType = GetSelectedFormat();
        if (nPrintType == 0)
        {
            alert("Please select one format. ");
            return false;
        }
        var nUnitType = parseInt($('input[name=Unit]:checked').val());
        var nPageSize = parseInt($('input[name=PageSize]:checked').val());
        var oExportBill = $("#tblExportBills").datagrid("getSelected");
        var oExportDocs = $('#tblExportDocSetup').datagrid('getChecked');
        if (oExportDocs == null || oExportDocs.length <= 0) { alert("Please select at least one Export Document Setup."); return; }
        $("#winPrintFormat").icsWindow("close");
        for (var i = 0; i<oExportDocs.length; i++)
        {
            window.open(_sBaseAddress + '/ExportBill/PrintExportDoc?id=' + oExportBill.ExportBillID + "&nDocType=" + oExportDocs[i].ExportDocSetupID + "&nPrintType=" + nPrintType + "&nUnitType=" + nUnitType + "&nPageSize=" + nPageSize, "_blank");
        } 
    });

    $("#chkPrintTitleInImg").change(function () {
        if (this.checked) {
            $('#chkPrintTitleInImg').prop('checked', true);
            $('#chkPrintTitleInPAD').prop('checked', false);
            $('#chkPrintNormal').prop('checked', false);
        } else {
            $('#chkPrintTitleInPAD').prop('checked', false);
            $('#chkPrintTitleInImg').prop('checked', false);
            $('#chkPrintNormal').prop('checked', false);
        }
    });

    $("#chkPrintTitleInPAD").change(function () {
        if (this.checked) {
            $('#chkPrintTitleInPAD').prop('checked', true);
            $('#chkPrintTitleInImg').prop('checked', false);
            $('#chkPrintNormal').prop('checked', false);
        } else {
            $('#chkPrintTitleInPAD').prop('checked', false);
            $('#chkPrintTitleInImg').prop('checked', false);
            $('#chkPrintNormal').prop('checked', false);
        }
    });

    $("#chkPrintNormal").change(function () {
        if (this.checked) {
            $('#chkPrintNormal').prop('checked', true);
            $('#chkPrintTitleInPAD').prop('checked', false);
            $('#chkPrintTitleInImg').prop('checked', false);
        } else {
            $('#chkPrintTitleInPAD').prop('checked', false);
            $('#chkPrintTitleInImg').prop('checked', false);
            $('#chkPrintNormal').prop('checked', false);
        }
    });

}

function Gets_byLDBCNo(oExportBill) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "GetbyLDBCNo",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0)
        {
            DynamicRefreshList(response.objs, "tblExportBills");
        }
        else {
            alert("No List Found");
            DynamicRefreshList([], "tblExportBills");
        }
    });
}

function Gets_byLCNo(oExportBill) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "GetbyLCNo",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0)
        {
            DynamicRefreshList(response.objs, "tblExportBills");
        }
        else {
            alert("No List Found");
            DynamicRefreshList([], "tblExportBills");
        }
    });
}



function Gets_ExportDocForwarding_Reload(oExportBill) {
    _oBankForwardingList = [];
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportDocForwarding",
        ActionName: "GetbyExportBill_Reload",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {

        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                _oBankForwardingList = response.objs;
                DynamicRefreshList(response.objs, "tblExportDocForwarding");
                $('#tblExportDocForwarding').datagrid({ selectOnCheck: false, checkOnSelect: true });
            }
            else { DynamicRefreshList([], "tblExportDocForwarding"); _oBankForwardingList = [];}

        }
    });
}

function Gets_ExportBillHistory(oExportBill) {
    
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "GetExportBillHistorys",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        
        if (response.status && response.objs.length > 0) {
            if (response.objs.length > 0) {
                DynamicRefreshList(response.objs, "tblExportBillHistorys");
                
            }
            else { DynamicRefreshList([], "tblExportBillHistorys"); }

        }
    });
}

function GetExportBillInformation(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "Get",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) { RefreshExportBillControl(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function GetExportBillInformation_ReceiveFromParty(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "Get_ReceiveFromParty",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) { RefreshControl_ReceiveFromParty(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function GetExportBillInformation_SendToBank(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "Get_SendToBank",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) { RefreshControl_SendToBank(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function GetExportBillInformation_RecdFromBank(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "Get_RecdFromBank",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) { RefreshControl_RecdFromBank(response.obj); }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function GetExportBillInformation_MaturityReceive(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "Get_MaturityReceive",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) {
                RefreshControl_MaturityReceive(response.obj);
            }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function GetExportBillInformation_BillRelization(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "Get_BillRelization",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) {
                RefreshControl_BillRelization(response.obj);
            }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function GetExportBillInformation_BFDDReceive(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "Get_FDDinHand",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) {
                RefreshControl_BFDDReceive(response.obj);
            }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function GetExportBillInformation_EncashmentReceive(oExportBill) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportBill,
        ControllerName: "ExportBill",
        ActionName: "Get_EncashmentReceive",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.ExportBillID > 0) {
                RefreshControl_BillEncashment(response.obj);
            }
            else { alert(response.obj.ErrorMessage); }
        }
        else {

            alert("No information found.");
        }
    });
}

function RefreshExportBillLayout(buttonId) {
    if (buttonId === "btnViewExportBill") {
        $("#winExportBill input").prop("disabled", true);
        $("#btnSaveExportBill").hide();
    }
    else {
        $("#winExportBill input").prop("disabled", false);
        $("#btnSaveExportBill").show();
    }
    $(".disabled input").prop("disabled", true);
}

function RefreshExportBillControl(oExportBill)
{
    _oExportBill = oExportBill;

    document.getElementById('lblExportBillNo').innerHTML = _oExportBill.ExportBillNo;

    if (_oExportBill.ExportLC.ExportLCID > 0) {
        var sAmendmentNoAndDate = "";
        if (_oExportBill.ExportLC.VersionNoInInt > 0) {
            var sADate = _oExportBill.ExportLC.AmendmentDateSt;
            if (sADate == "-") {
                sADate = "";
            } else {
                sADate = " (" + sADate + ") ";
            }
            sAmendmentNoAndDate = _oExportBill.ExportLC.VersionNoInInt + sADate;
        }

        document.getElementById('lblAmendmentNoAndDate').innerHTML = sAmendmentNoAndDate;
        document.getElementById('lblLCNo').innerHTML = _oExportBill.ExportLC.ExportLCNo;
        document.getElementById('lblFileNo').innerHTML = _oExportBill.ExportLC.FileNo;
    }

    document.getElementById('lblApplicantName').innerHTML = _oExportBill.ApplicantName;
    document.getElementById('lblExportBillDate').innerHTML = _oExportBill.StartDateSt;
    document.getElementById('lblLCDate').innerHTML = _oExportBill.LCOpeningDatest;

    document.getElementById('lblBankNameAdvice').innerHTML = _oExportBill.BankName_Advice + '[' + _oExportBill.BBranchName_Advice + ']';
    document.getElementById('lblBankNameNego').innerHTML = _oExportBill.BankName_Nego + '[' + _oExportBill.BBranchName_Nego + ']';
    document.getElementById('lblBankNameIssue').innerHTML = _oExportBill.BankName_Issue + '[' + _oExportBill.BBranchName_Issue + ']';

    document.getElementById('lblIAmountSt').innerHTML = _oExportBill.AmountSt;
    document.getElementById('lblIAmountLC').innerHTML = _oExportBill.Amount_LCSt;
    document.getElementById('lblState').innerHTML = _oExportBill.StateSt;
    document.getElementById('lblSendToPartySt').innerHTML = _oExportBill.SendToPartySt;
    document.getElementById('lblSendToBankDateSt').innerHTML = _oExportBill.SendToBankDateSt;
    document.getElementById('lblRecdFromPartyST').innerHTML = _oExportBill.RecdFromPartySt;
    document.getElementById('lblRecedFromBankDateSt').innerHTML = _oExportBill.RecedFromBankDateSt;

    
    $("#txtNote").val(_oExportBill.NoteCarry);

    if (_oExportBill.SendToPartySt == "--" || _oExportBill.SendToPartySt == "")
    {
        $("#txtSendToParty").datebox("setValue", icsdateformat(new Date()));
    }
    else
    {
        $("#txtSendToParty").datebox("setValue", _oExportBill.SendToPartySt);
    }

}

function RefreshControl_ReceiveFromParty(oExportBill) {
    _oExportBill = oExportBill;
    
    document.getElementById('lblExportBillNo1').innerHTML = _oExportBill.ExportBillNo;

    if (_oExportBill.ExportLC.ExportLCID > 0) {
        var sAmendmentNoAndDate = "";
        if (_oExportBill.ExportLC.VersionNoInInt > 0) {
            var sADate = _oExportBill.ExportLC.AmendmentDateSt;
            if (sADate == "-") {
                sADate = "";
            } else {
                sADate = " (" + sADate + ") ";
            }
            sAmendmentNoAndDate = _oExportBill.ExportLC.VersionNoInInt + sADate;
        }

        document.getElementById('lblAmendmentNoAndDate1').innerHTML = sAmendmentNoAndDate;
        document.getElementById('lblLCNo1').innerHTML = _oExportBill.ExportLC.ExportLCNo;
        document.getElementById('lblFileNo1').innerHTML = _oExportBill.ExportLC.FileNo;
    }

    document.getElementById('lblApplicantName1').innerHTML = _oExportBill.ApplicantName;
    document.getElementById('lblExportBillDate1').innerHTML = _oExportBill.StartDateSt;
    document.getElementById('lblLCDate1').innerHTML = _oExportBill.LCOpeningDatest;

    document.getElementById('lblBankNameAdvice1').innerHTML = _oExportBill.BankName_Advice + '[' + _oExportBill.BBranchName_Advice + ']';
    document.getElementById('lblBankNameNego1').innerHTML = _oExportBill.BankName_Nego + '[' + _oExportBill.BBranchName_Nego + ']';
    document.getElementById('lblBankNameIssue1').innerHTML = _oExportBill.BankName_Issue + '[' + _oExportBill.BBranchName_Issue + ']';

    document.getElementById('lblIAmountSt1').innerHTML = _oExportBill.AmountSt;
    document.getElementById('lblIAmountLC1').innerHTML = _oExportBill.Amount_LCSt;
    document.getElementById('lblState1').innerHTML = _oExportBill.StateSt;
    document.getElementById('lblSendToPartySt1').innerHTML = _oExportBill.SendToPartySt;
    document.getElementById('lblSendToBankDateSt1').innerHTML = _oExportBill.SendToBankDateSt;
    document.getElementById('lblRecdFromPartySt1').innerHTML = _oExportBill.RecdFromPartySt;
    document.getElementById('lblRecedFromBankDateSt1').innerHTML = _oExportBill.RecedFromBankDateSt;

  
    $("#txtNote_RecdFromParty").val(_oExportBill.NoteCarry);

    if (_oExportBill.RecdFromPartySt == "--" || _oExportBill.RecdFromPartySt == "") {
        $("#txtRecdFromParty").datebox("setValue", icsdateformat(new Date()));
    }
    else {
        $("#txtRecdFromParty").datebox("setValue", _oExportBill.RecdFromPartySt);
    }

}

function RefreshControl_SendToBank(oExportBill) {
    _oExportBill = oExportBill;
    
    document.getElementById('lblExportBillNo2').innerHTML = _oExportBill.ExportBillNo;

    if (_oExportBill.ExportLC.ExportLCID > 0) {
        var sAmendmentNoAndDate = "";
        if (_oExportBill.ExportLC.VersionNoInInt > 0) {
            var sADate = _oExportBill.ExportLC.AmendmentDateSt;
            if (sADate == "-") {
                sADate = "";
            } else {
                sADate = " (" + sADate + ") ";
            }
            sAmendmentNoAndDate = _oExportBill.ExportLC.VersionNoInInt + sADate;
        }

        document.getElementById('lblAmendmentNoAndDate2').innerHTML = sAmendmentNoAndDate;
        document.getElementById('lblLCNo2').innerHTML = _oExportBill.ExportLC.ExportLCNo;
        document.getElementById('lblFileNo2').innerHTML = _oExportBill.ExportLC.FileNo;
    }

    document.getElementById('lblApplicantName2').innerHTML = _oExportBill.ApplicantName;
    document.getElementById('lblExportBillDate2').innerHTML = _oExportBill.StartDateSt;
    document.getElementById('lblLCDate2').innerHTML = _oExportBill.LCOpeningDatest;

    document.getElementById('lblBankNameAdvice2').innerHTML = _oExportBill.BankName_Advice + '[' + _oExportBill.BBranchName_Advice + ']';
    document.getElementById('lblBankNameNego2').innerHTML = _oExportBill.BankName_Nego + '[' + _oExportBill.BBranchName_Nego + ']';
    document.getElementById('lblBankNameIssue2').innerHTML = _oExportBill.BankName_Issue + '[' + _oExportBill.BBranchName_Issue + ']';

    document.getElementById('lblIAmountSt2').innerHTML = _oExportBill.AmountSt;
    document.getElementById('lblIAmountLC2').innerHTML = _oExportBill.Amount_LCSt;
    document.getElementById('lblState2').innerHTML = _oExportBill.StateSt;
    document.getElementById('lblSendToPartySt2').innerHTML = _oExportBill.SendToPartySt;
    document.getElementById('lblSendToBankDateSt2').innerHTML = _oExportBill.SendToBankDateSt;
    document.getElementById('lblRecdFromPartySt2').innerHTML = _oExportBill.RecdFromPartySt;
    document.getElementById('lblRecedFromBankDateSt2').innerHTML = _oExportBill.RecedFromBankDateSt;


    $("#txtNote_RecdFromParty").val(_oExportBill.NoteCarry);

    if (_oExportBill.SendToBankDateSt == "--" || _oExportBill.SendToBankDateSt == "") {
        $("#txtSendToBank").datebox("setValue", icsdateformat(new Date()));
    }
    else {
        $("#txtSendToBank").datebox("setValue", _oExportBill.SendToBankDateSt);
    }

}

function RefreshControl_RecdFromBank(oExportBill) {
    _oExportBill = oExportBill;
    
    document.getElementById('lblExportBillNo03').innerHTML = _oExportBill.ExportBillNo;


    if (_oExportBill.ExportLC.ExportLCID > 0)
    {
        var sAmendmentNoAndDate = "";
        if (_oExportBill.ExportLC.VersionNoInInt > 0) {
            var sADate = _oExportBill.ExportLC.AmendmentDateSt;
            if (sADate == "-") {
                sADate = "";
            } else {
                sADate = " (" + sADate + ") ";
            }
            sAmendmentNoAndDate = _oExportBill.ExportLC.VersionNoInInt + sADate;
        }

        document.getElementById('lblAmendmentNoAndDate03').innerHTML = sAmendmentNoAndDate;
        document.getElementById('lblLCNo03').innerHTML = _oExportBill.ExportLC.ExportLCNo;
        document.getElementById('lblFileNo03').innerHTML = _oExportBill.ExportLC.FileNo;
    }


    document.getElementById('lblApplicantName03').innerHTML = _oExportBill.ApplicantName;
    document.getElementById('lblExportBillDate03').innerHTML = _oExportBill.StartDateSt;
    document.getElementById('lblLCDate03').innerHTML = _oExportBill.LCOpeningDatest;

    document.getElementById('lblBankNameAdvice03').innerHTML = _oExportBill.BankName_Advice + '[' + _oExportBill.BBranchName_Advice + ']';
    document.getElementById('lblBankNameNego03').innerHTML = _oExportBill.BankName_Nego + '[' + _oExportBill.BBranchName_Nego + ']';
    document.getElementById('lblBankNameIssue03').innerHTML = _oExportBill.BankName_Issue + '[' + _oExportBill.BBranchName_Issue + ']';

    document.getElementById('lblIAmountSt03').innerHTML = _oExportBill.AmountSt;
    document.getElementById('lblIAmountLC03').innerHTML = _oExportBill.Amount_LCSt;
    document.getElementById('lblState03').innerHTML = _oExportBill.StateSt;
    document.getElementById('lblSendToPartySt03').innerHTML = _oExportBill.SendToPartySt;
    document.getElementById('lblSendToBankDateSt03').innerHTML = _oExportBill.SendToBankDateSt;
    document.getElementById('lblRecdFromPartySt03').innerHTML = _oExportBill.RecdFromPartySt;
    document.getElementById('lblRecedFromBankDateSt03').innerHTML = _oExportBill.RecedFromBankDateSt;


    $("#txtNote_ReceieFromBank").val(_oExportBill.NoteCarry);
    $("#txtLDBCNo").val(_oExportBill.LDBCNo);

    if (_oExportBill.LDBCDateSt == "--" || _oExportBill.LDBCDateSt == "") {
        $("#txtRecedFromBankDate").datebox("setValue", icsdateformat(new Date()));
    }
    else {
        $("#txtRecedFromBankDate").datebox("setValue", _oExportBill.LDBCDateSt);
    }



}

function RefreshControl_MaturityReceive(oExportBill) {
    _oExportBill = oExportBill;

   
   
    
    document.getElementById('lblExportBillNo04').innerHTML = _oExportBill.ExportBillNo;

    //Imrez
    document.getElementById('lblLDBCNo04').innerHTML = _oExportBill.LDBCNo;
    document.getElementById('lblLDBCDate04').innerHTML = _oExportBill.LDBCDateSt; //LCTermsName

    if (_oExportBill.ExportLC.ExportLCID > 0) {
        var sAmendmentNoAndDate = "";
        if (_oExportBill.ExportLC.VersionNoInInt > 0) {
            var sADate = _oExportBill.ExportLC.AmendmentDateSt;
            if (sADate == "-") {
                sADate = "";
            } else {
                sADate = " (" + sADate + ") ";
            }
            sAmendmentNoAndDate = _oExportBill.ExportLC.VersionNoInInt + sADate;
        }

        document.getElementById('lblAmendmentNoAndDate04').innerHTML = sAmendmentNoAndDate;
        document.getElementById('lblLCNo04').innerHTML = _oExportBill.ExportLC.ExportLCNo;
        document.getElementById('lblFileNo04').innerHTML = _oExportBill.ExportLC.FileNo;
        document.getElementById('lblLCTerms04').innerHTML = _oExportBill.ExportLC.LCTermsName;
    }

    document.getElementById('lblApplicantName04').innerHTML = _oExportBill.ApplicantName;
    document.getElementById('lblExportBillDate04').innerHTML = _oExportBill.StartDateSt;
    document.getElementById('lblLCDate04').innerHTML = _oExportBill.LCOpeningDatest;

    document.getElementById('lblBankNameAdvice04').innerHTML = _oExportBill.BankName_Advice + '[' + _oExportBill.BBranchName_Advice + ']';
    document.getElementById('lblBankNameNego04').innerHTML = _oExportBill.BankName_Nego + '[' + _oExportBill.BBranchName_Nego + ']';
    document.getElementById('lblBankNameIssue04').innerHTML = _oExportBill.BankName_Issue + '[' + _oExportBill.BBranchName_Issue + ']';

    document.getElementById('lblIAmountSt04').innerHTML = _oExportBill.AmountSt;
    document.getElementById('lblIAmountLC04').innerHTML = _oExportBill.Amount_LCSt;
    document.getElementById('lblState04').innerHTML = _oExportBill.StateSt;
    document.getElementById('lblSendToPartySt04').innerHTML = _oExportBill.SendToPartySt;
    document.getElementById('lblSendToBankDateSt04').innerHTML = _oExportBill.SendToBankDateSt;
    document.getElementById('lblRecdFromPartySt04').innerHTML = _oExportBill.RecdFromPartySt;
    document.getElementById('lblRecedFromBankDateSt04').innerHTML = _oExportBill.RecedFromBankDateSt;

  
   
    if (_oExportBill.AcceptanceDateStr == "" || _oExportBill.AcceptanceDateStr == "--") {
        $('#txtAcceptanceDate').datebox('setValue', icsdateformat(new Date()));
    }
    else {
        $('#txtAcceptanceDate').datebox('setValue', _oExportBill.AcceptanceDateStr);
    }
    if (_oExportBill.MaturityReceivedDateSt == "" || _oExportBill.MaturityReceivedDateSt == "--") {
        $('#txtMaturityReceivedDate').datebox('setValue', icsdateformat(new Date()));
    }
    else {
        $('#txtMaturityReceivedDate').datebox('setValue', _oExportBill.MaturityReceivedDateSt);
    }
    if (_oExportBill.MaturityDateSt == "" || _oExportBill.MaturityDateSt == "--") {
        $('#txtMaturityDate').datebox('setValue', icsdateformat(new Date()));
    }
    else {
        $('#txtMaturityDate').datebox('setValue', _oExportBill.MaturityDateSt);
    }
    document.getElementById('txtNote_MaturityRecd').value = _oExportBill.NoteCarry;

    $("#cboLCTerm").icsLoadCombo({
        List: _oExportBill.LCTermss,
        OptionValue: "LCTermID",
        DisplayText: "NameDaysInString"
    });

    var sTermVal = $("#lblLCTerms04").html();
    
    for (var i = 0; i < _oExportBill.LCTermss.length; i++) {
        if ($.trim(sTermVal.toUpperCase()) == $.trim(_oExportBill.LCTermss[i].NameDaysInString.toUpperCase()))
        {
            $("#cboLCTerm").val(_oExportBill.LCTermss[i].LCTermID);
            break;
        }
    }

}

function RefreshControl_BillRelization(oExportBill) {
    _oExportBill = oExportBill;

    
    document.getElementById('lblExportBillNo05').innerHTML = _oExportBill.ExportBillNo;

    document.getElementById('lblLDBCNo05').innerHTML = _oExportBill.LDBCNo;
    document.getElementById('lblLDBCDate05').innerHTML = _oExportBill.LDBCDateSt; //LCTermsName

    if (_oExportBill.ExportLC.ExportLCID > 0) {
        var sAmendmentNoAndDate = "";
        if (_oExportBill.ExportLC.VersionNoInInt > 0) {
            var sADate = _oExportBill.ExportLC.AmendmentDateSt;
            if (sADate == "-") {
                sADate = "";
            } else {
                sADate = " (" + sADate + ") ";
            }
            sAmendmentNoAndDate = _oExportBill.ExportLC.VersionNoInInt + sADate;
        }

        document.getElementById('lblAmendmentNoAndDate05').innerHTML = sAmendmentNoAndDate;
        document.getElementById('lblLCNo05').innerHTML = _oExportBill.ExportLC.ExportLCNo;
        document.getElementById('lblFileNo05').innerHTML = _oExportBill.ExportLC.FileNo;
        document.getElementById('lblLCTerms05').innerHTML = _oExportBill.ExportLC.LCTermsName;
    }

    document.getElementById('lblApplicantName05').innerHTML = _oExportBill.ApplicantName;
    document.getElementById('lblExportBillDate05').innerHTML = _oExportBill.StartDateSt;
    document.getElementById('lblLCDate05').innerHTML = _oExportBill.LCOpeningDatest;

    document.getElementById('lblBankNameAdvice05').innerHTML = _oExportBill.BankName_Advice + '[' + _oExportBill.BBranchName_Advice + ']';
    document.getElementById('lblBankNameNego05').innerHTML = _oExportBill.BankName_Nego + '[' + _oExportBill.BBranchName_Nego + ']';
    document.getElementById('lblBankNameIssue05').innerHTML = _oExportBill.BankName_Issue + '[' + _oExportBill.BBranchName_Issue + ']';

    document.getElementById('lblIAmountSt05').innerHTML = _oExportBill.AmountSt;
    document.getElementById('lblIAmountLC05').innerHTML = _oExportBill.Amount_LCSt;
    document.getElementById('lblState05').innerHTML = _oExportBill.StateSt;
    document.getElementById('lblSendToPartySt05').innerHTML = _oExportBill.SendToPartySt;
    document.getElementById('lblSendToBankDateSt05').innerHTML = _oExportBill.SendToBankDateSt;
    document.getElementById('lblRecdFromPartySt05').innerHTML = _oExportBill.RecdFromPartySt;
    document.getElementById('lblRecedFromBankDateSt05').innerHTML = _oExportBill.RecedFromBankDateSt;


    if (_oExportBill.RelizationDateSt == "" || _oExportBill.RelizationDateSt == "--") {
        $('#txtRelizationDate').datebox('setValue', icsdateformat(new Date()));
    }
    else {
        $('#txtRelizationDate').datebox('setValue', _oExportBill.RelizationDateSt);
    }
  
    document.getElementById('txtNote_Relization').value = _oExportBill.NoteCarry;
    DynamicRefreshList(_oExportBill.ExportBillRealizeds, "tblExportBillRealizeds");
   
}

function RefreshControl_BFDDReceive(oExportBill) {
    _oExportBill = oExportBill;
    document.getElementById('lblExportBillNo06').innerHTML = _oExportBill.ExportBillNo;
   

    document.getElementById('lblLDBCNo06').innerHTML = _oExportBill.LDBCNo;
    document.getElementById('lblLDBCDate06').innerHTML = _oExportBill.LDBCDateSt; //LCTermsName

    if (_oExportBill.ExportLC.ExportLCID > 0) {
        var sAmendmentNoAndDate = "";
        if (_oExportBill.ExportLC.VersionNoInInt > 0) {
            var sADate = _oExportBill.ExportLC.AmendmentDateSt;
            if (sADate == "-") {
                sADate = "";
            } else {
                sADate = " (" + sADate + ") ";
            }
            sAmendmentNoAndDate = _oExportBill.ExportLC.VersionNoInInt + sADate;
        }

        document.getElementById('lblAmendmentNoAndDate06').innerHTML = sAmendmentNoAndDate;
        document.getElementById('lblLCNo06').innerHTML = _oExportBill.ExportLC.ExportLCNo;
        document.getElementById('lblFileNo06').innerHTML = _oExportBill.ExportLC.FileNo;
        document.getElementById('lblLCTerms06').innerHTML = _oExportBill.ExportLC.LCTermsName;
    }

    document.getElementById('lblApplicantName06').innerHTML = _oExportBill.ApplicantName;
    document.getElementById('lblExportBillDate06').innerHTML = _oExportBill.StartDateSt;
    document.getElementById('lblLCDate06').innerHTML = _oExportBill.LCOpeningDatest;

    document.getElementById('lblBankNameAdvice06').innerHTML = _oExportBill.BankName_Advice + '[' + _oExportBill.BBranchName_Advice + ']';
    document.getElementById('lblBankNameNego06').innerHTML = _oExportBill.BankName_Nego + '[' + _oExportBill.BBranchName_Nego + ']';
    document.getElementById('lblBankNameIssue06').innerHTML = _oExportBill.BankName_Issue + '[' + _oExportBill.BBranchName_Issue + ']';

    document.getElementById('lblIAmountSt06').innerHTML = _oExportBill.AmountSt;
    document.getElementById('lblIAmountLC06').innerHTML = _oExportBill.Amount_LCSt;
    document.getElementById('lblState06').innerHTML = _oExportBill.StateSt;

    document.getElementById('lblLDBCNo06').innerHTML = _oExportBill.LDBCNo;
    document.getElementById('lblLDBCDate06').innerHTML = _oExportBill.LDBCDateSt;
    document.getElementById('lblMaturityReceivedDate06').innerHTML = _oExportBill.MaturityReceivedDateSt;
    document.getElementById('lblMaturityDate06').innerHTML = _oExportBill.MaturityDateSt;


    if (_oExportBill.BankFDDRecDateSt == "" || _oExportBill.BankFDDRecDateSt == "--") {
        $('#txtBankFDDRecDate').datebox('setValue', icsdateformat(new Date()));
    }
    else {
        $('#txtBankFDDRecDate').datebox('setValue', _oExportBill.BankFDDRecDateSt);
    }

    document.getElementById('txtNote_BFDDReceive').value = _oExportBill.NoteCarry;
}

function RefreshControl_BillEncashment(oExportBill) {
    _oExportBill = oExportBill;
    
    document.getElementById('lblExportBillNo07').innerHTML = _oExportBill.ExportBillNo;
   
    document.getElementById('lblLDBCNo07').innerHTML = _oExportBill.LDBCNo;
    document.getElementById('lblLDBCDate07').innerHTML = _oExportBill.LDBCDateSt; //LCTermsName

    if (_oExportBill.ExportLC.ExportLCID > 0) {
        var sAmendmentNoAndDate = "";
        if (_oExportBill.ExportLC.VersionNoInInt > 0) {
            var sADate = _oExportBill.ExportLC.AmendmentDateSt;
            if (sADate == "-") {
                sADate = "";
            } else {
                sADate = " (" + sADate + ") ";
            }
            sAmendmentNoAndDate = _oExportBill.ExportLC.VersionNoInInt + sADate;
        }

        document.getElementById('lblAmendmentNoAndDate07').innerHTML = sAmendmentNoAndDate;
        document.getElementById('lblLCNo07').innerHTML = _oExportBill.ExportLC.ExportLCNo;
        document.getElementById('lblFileNo07').innerHTML = _oExportBill.ExportLC.FileNo;
        document.getElementById('lblLCTerms07').innerHTML = _oExportBill.ExportLC.LCTermsName;
    }

    document.getElementById('lblApplicantName07').innerHTML = _oExportBill.ApplicantName;
    document.getElementById('lblExportBillDate07').innerHTML = _oExportBill.StartDateSt;
    document.getElementById('lblLCDate07').innerHTML = _oExportBill.LCOpeningDatest;

    document.getElementById('lblBankNameAdvice07').innerHTML = _oExportBill.BankName_Advice + '[' + _oExportBill.BBranchName_Advice + ']';
    document.getElementById('lblBankNameNego07').innerHTML = _oExportBill.BankName_Nego + '[' + _oExportBill.BBranchName_Nego + ']';
    document.getElementById('lblBankNameIssue07').innerHTML = _oExportBill.BankName_Issue + '[' + _oExportBill.BBranchName_Issue + ']';

    document.getElementById('lblIAmountSt07').innerHTML = _oExportBill.AmountSt;
    document.getElementById('lblIAmountLC07').innerHTML = _oExportBill.Amount_LCSt;
    document.getElementById('lblState07').innerHTML = _oExportBill.StateSt;
    document.getElementById('lblSendToPartySt07').innerHTML = _oExportBill.SendToPartySt;
    document.getElementById('lblSendToBankDateSt07').innerHTML = _oExportBill.SendToBankDateSt;
    document.getElementById('lblRecdFromPartySt07').innerHTML = _oExportBill.RecdFromPartySt;
    document.getElementById('lblRecedFromBankDateSt07').innerHTML = _oExportBill.RecedFromBankDateSt;


    if (_oExportBill.EncashmentDateSt == "" || _oExportBill.EncashmentDateSt == "--") {
        $('#txtEncashmentDate').datebox('setValue', icsdateformat(new Date()));
    }
    else {
        $('#txtEncashmentDate').datebox('setValue', _oExportBill.EncashmentDateSt);
    }

    document.getElementById('txtNote_Encashment').value = _oExportBill.NoteCarry;
    if (_oExportBill.ExportBillEncashments.length > 0) {
        DynamicRefreshList(_oExportBill.ExportBillEncashments, "tblExportBillEncashments");
    }
    else {
        DynamicRefreshList([], "tblExportBillEncashments");
    }
}


