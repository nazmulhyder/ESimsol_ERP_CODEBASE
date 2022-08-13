function InitializeExportDocSetupEvents() {
    var isPikerOpen = false;

    debugger;
    //ExportDocSetup
    $("#cboDocumentType").icsLoadCombo({
        List: _oDocumentTypes,
        OptionValue: "Value",
        DisplayText: "Text"
    });
    $("#cboExportLCType").icsLoadCombo({
        List: _oExportLCTypes,
        OptionValue: "id",
        DisplayText: "Value"
    });
    $("#cboReportOn").icsLoadCombo({
        List: _oPrintNo,
        OptionValue: "id",
        DisplayText: "Value"
    });
    $("#cboProductPrintType").icsLoadCombo({
        List: _oPrintNo,
        OptionValue: "id",
        DisplayText: "Value"
    });
 
    $("#cboGoodsDesViewType").icsLoadCombo({
        List: _oGoodsDesViewType,
        OptionValue: "id",
        DisplayText: "Value"
    });

    $("#btnSaveExportDocSetup").click(function () {
        debugger;
        if (!ValidateInputExportDocSetup()) return;
        var oExportDocSetup = RefreshObjectExportDocSetup();

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportDocSetup,
            ObjectId: oExportDocSetup.ExportDocSetupID,
            ControllerName: "ExportDocSetup",
            ActionName: "Save",
            TableId: "tblExportDocSetups",
            IsWinClose: true
        };
        $.icsSave(obj);
    });

    $("#btnCopyDocSetup").click(function () {
        if (!ValidateInputExportDocSetup()) return;
        var oExportDocSetup = RefreshObjectExportDocSetup();
        oExportDocSetup.ExportDocSetupID = 0;
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportDocSetup,
            ObjectId: oExportDocSetup.ExportDocSetupID,
            ControllerName: "ExportDocSetup",
            ActionName: "SaveCopy",
            TableId: "tblExportDocSetups",
            IsWinClose: true
        };
        $.icsSave(obj);
    });

    

    $("#btnCloseExportDocSetup").click(function () {
        $("#winExportDocSetup").icsWindow('close');
        $(".wininputfieldstyle input").val("");
        //$("#chkIsOwnExportDocSetup").prop("checked", false);
    });
}


function RefreshObjectExportDocSetup() {
    var nExportDocSetupId = (_oExportDocSetup == null ? 0 : _oExportDocSetup.ExportDocSetupID);
    var oExportDocSetup = {
        ExportDocSetupID: nExportDocSetupId,
        CompanyID: 1,
        DocumentTypeInt: parseInt($("#cboDocumentType").val()),
        ExportLCType: parseInt($("#cboExportLCType").val()),
        PrintOn: parseInt($("#cboReportOn").val()),
        ProductPrintType: parseInt($("#cboProductPrintType").val()),
        DocName: $.trim($("#txtDocName").val()),
        DocHeader: $.trim($("#txtDocHeader").val()),
        Beneficiary: $.trim($("#txtBeneficiary").val()),
        BillNo: $.trim($("#txtBillNo").val()),
        NoAndDateOfDoc: $.trim($("#txtNoAndDateOfDoc").val()),
        ProformaInvoiceNoAndDate: $.trim($("#txtProformaInvoiceNoAndDate").val()),
        AccountOf: $.trim($("#txtAccountOf").val()),
        DocumentaryCreditNoDate: $.trim($("#txtDocumentaryCreditNoDate").val()),
        AgainstExportLC: $.trim($("#txtAgainstExportLC").val()),
        PortofLoading: $.trim($("#txtPortofLoading").val()),
        FinalDestination: $.trim($("#txtFinalDestination").val()),
        IssuingBank: $.trim($("#txtIssuingBank").val()),
        NegotiatingBank: $.trim($("#txtNegotiatingBank").val()),
        CountryofOrigin: $.trim($("#txtCountryofOrigin").val()),
        TermsofPayment: $.trim($("#txtTermsofPayment").val()),
        Account: $.trim($("#txtAccount").val()),
        NotifyParty: $.trim($("#txtNotifyParty").val()),
        Remarks: $.trim($("#txtRemarks").val()),
        AmountInWord: $.trim($("#txtAmountInWord").val()),
        Wecertifythat: $.trim($("#txtWecertifythat").val()),
        Certification: $.trim($("#txtCertification").val()),
        DeliveryTo: $.trim($("#txtDeliveryTo").val()),

        IRC: $.trim($("#txtIRC").val()),
        GarmentsQty: $.trim($("#txtGarmentsQty").val()),
        HSCode: $.trim($("#txtHSCode").val()),
        AreaCode: $.trim($("#txtAreaCode").val()),
        SpecialNote: $.trim($("#txtSpecialNote").val()),
        Remark: $.trim($("#txtRemark").val()),
        ChallanNo: $.trim($("#txtChallanNo").val()),
        
        IsVat: $("#chkIsPrintVat").is(":checked"),
        IsRegistration: $("#chkIsPrintRegistration").is(":checked"),

        IsPrintHeader: $("#chkIsPrintHeader").is(":checked"),
        IsPrintOriginal: $("#chkIsPrintOriginal").is(":checked"),
        IsPrintGrossNetWeight: $("#chkIsPrintGrossNetWeight").is(":checked"),
        IsPrintDeliveryBy: $("#chkIsPrintDeliveryBy").is(":checked"),
        IsPrintTerm: $("#chkIsPrintTerm").is(":checked"),
        IsPrintQty: $("#chkIsPrintQty").is(":checked"),
        IsPrintUnitPrice: $("#chkIsPrintUnitPrice").is(":checked"),
        IsPrintValue: $("#chkIsPrintValue").is(":checked"),
        IsPrintWeight: $("#chkIsPrintWeight").is(":checked"),
        IsPrintFrieghtPrepaid: $("#chkIsPrintFrieghtPrepaid").is(":checked"),
        IsPrintInvoiceDate: $("#chkIsPrintInvoiceDate").is(":checked"),
        ForCaptionInDubleLine: $("#chkForCaptionInDubleLine").is(":checked"),
        IsShowAmendmentNo: $("#chkIsShowAmendmentNo").is(":checked"),
        BUID:_nBUID,
        ClauseOne: $.trim($("#txtClauseOne").val()),
        ClauseTwo: $.trim($("#txtClauseTwo").val()),
        ClauseThree: $.trim($("#txtClauseThree").val()),
        ClauseFour: $.trim($("#txtClauseFour").val()),
        Carrier: $.trim($("#txtCarrier").val()),

        AuthorisedSignature: $.trim($("#txtAuthorisedSignature").val()),
        ReceiverSignature: $.trim($("#txtReceiverSignature").val()),
        For: $.trim($("#txtFor").val()),
        MUnitName: $.trim($("#txtMUnitName").val()),
        NetWeightName: $.trim($("#txtNetWeightName").val()),
        GrossWeightName: $.trim($("#txtGrossWeightName").val()),
        Bag_Name: $.trim($("#txtBag_Name").val()),
        CountryofOriginName : $.trim($("#txtCountryofOriginName").val()),
        SellingOnAbout: $.trim($("#txtSellingOnAbout").val()),
        PortofLoadingName: $.trim($("#txtPortofLoadingName").val()),
        FinalDestinationName: $.trim($("#txtFinalDestinationName").val()),
        TruckNo_Print: $.trim($("#txtTruckNo_Print").val()),
        Driver_Print: $.trim($("#txtDriver_Print").val()),
        TO: $.trim($("#txtTO").val()),
        ShippingMark: $.trim($("#txtShippingMark").val()),
        ReceiverCluse: $.trim($("#txtReceiverCluse").val()),
        CarrierName: $.trim($("#txtCarrierName").val()),
        DescriptionOfGoods: $.trim($("#txtDescriptionOfGoods").val()),
        MarkSAndNos: $.trim($("#txtMarkSAndNos").val()),
        QtyInOne: $.trim($("#txtQtyInOne").val()),
        QtyInTwo: $.trim($("#txtQtyInTwo").val()),
        ValueName: $.trim($("#txtValueName").val()),
        UPName: $.trim($("#txtUPName").val()),
        CTPApplicant: $.trim($("#txtCTPApplicant").val()),
        GRPNoDate: $.trim($("#txtGRPNoDate").val()),
        ASPERPI: $.trim($("#txtASPERPI").val()),
        TextWithGoodsCol: $.trim($("#txtTextWithGoodsCol").val()),
        TextWithGoodsRow: $.trim($("#txtTextWithGoodsRow").val()),
        NoOfBag: $.trim($("#txtNoOfBag").val()),
        GoodsDesViewType: parseInt($("#cboGoodsDesViewType").val()),
        ToTheOrderOf : $.trim($("#txtToTheOrderOf").val()),
        OrderOfBankTypeInInt : $('#cboOrderOfBankType').val(),
        TermsOfShipment: $.trim($("#txtTermsOfShipment").val()),
        FontSize_Normal: parseFloat($('#txtFontSize_Normal').val()),
        FontSize_ULine: parseFloat($('#txtFontSize_ULine').val()),
        FontSize_Bold: parseFloat($('#txtFontSize_Bold').val()),
        WeightPBag: parseFloat($('#txtWeightPBag').val()),
        GrossWeightPTage: parseFloat($('#txtGrossWeightPTage').val()),
        BagCount: parseFloat($('#txtBagCount').val())

    };
    return oExportDocSetup;
}

function ValidateInputExportDocSetup(e) {

    if (!$.trim($("#txtDocName").val()).length) {
        alert("Please enter ExportDocSetup name!");
        $('#txtDocName').val("");
        $('#txtDocName').focus();
        $('#txtDocName').css("border-color", "#c00");
        return false;
    } else {
        $('#txtDocName').css("border-color", "");
    }

    var nDocumentType = $("#cboDocumentType").val();
    if (parseInt(nDocumentType) === 0) {
        alert("Please select Doc Type!");
        $('#cboDocumentType').val("");
        $('#cboDocumentType').focus();
        $('#cboDocumentType').css("border-color", "#c00");
        return false;
    } else {
        $('#cboDocumentType').css("border-color", "");
    }
    var nDocumentType = $("#cboExportLCType").val();
    if (parseInt(nDocumentType) === 0) {
        alert("Please select L/C Type!");
        $('#cboExportLCType').val("");
        $('#cboExportLCType').focus();
        $('#cboExportLCType').css("border-color", "#c00");
        return false;
    } else {
        $('#cboExportLCType').css("border-color", "");
    }

    return true;
}

function LoadImage(nCompanyId) {
    var oCompany = {
        CompanyID: nCompanyId
    };
    $.ajax({
        type: "POST",
        url: _sBaseAddress + "/Company/LoadCompanyLogo",
        traditional: true,
        data: JSON.stringify(oCompany),
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            debugger;

            $("#imgCompanyLogo").attr("src", data);
        }
    });
}
