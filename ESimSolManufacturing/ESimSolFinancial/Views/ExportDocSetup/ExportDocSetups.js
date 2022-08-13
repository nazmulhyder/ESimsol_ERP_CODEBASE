function InitializeExportDocSetupsEvents()
{
    $("#btnAddExportDocSetup").click(function () {
        $("#winExportDocSetup").icsWindow("open", "Add ExportDocSetup");
        $("#winExportDocSetup input").not("input[type='button']").val("");
        _oExportDocSetup = null;
        RefreshExportDocSetupLayout("btnAddExportDocSetup"); //button id as parameter
    });

    $("#btnEditExportDocSetup").click(function () {
        var oExportDocSetup = $("#tblExportDocSetups").datagrid("getSelected");
        debugger;
        if (oExportDocSetup == null || oExportDocSetup.ExportDocSetupID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportDocSetup").icsWindow('open', "Edit ExportDocSetup");
        _oExportDocSetup = oExportDocSetup;
        RefreshExportDocSetupLayout("btnEditExportDocSetup");
        RefreshExportDocSetupControl(oExportDocSetup);
    });

    $("#btnViewExportDocSetup").click(function () {
        var oExportDocSetup = $("#tblExportDocSetups").datagrid("getSelected");
        if (oExportDocSetup == null || oExportDocSetup.ExportDocSetupID <= 0) { alert("Please select an item from list!"); return; }
        $("#winExportDocSetup").icsWindow('open', "View ExportDocSetup");
        RefreshExportDocSetupLayout("btnViewExportDocSetup");
        RefreshExportDocSetupControl(oExportDocSetup);
    });

    $("#btnDeleteExportDocSetup").click(function () {
        var oExportDocSetup = $("#tblExportDocSetups").datagrid("getSelected");
        if (!confirm("Confirm to Delete?")) return false;
        if (oExportDocSetup == null || oExportDocSetup.ExportDocSetupID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportDocSetup,
            ControllerName: "ExportDocSetup",
            ActionName: "Delete",
            TableId: "tblExportDocSetups",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $("#btnActivate").click(function () {
        debugger;
        var oExportDocSetup = $("#tblExportDocSetups").datagrid("getSelected");
        if (!confirm("Confirm to Active?")) return false;
        if (oExportDocSetup == null || oExportDocSetup.ExportDocSetupID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportDocSetup,
            ObjectId: oExportDocSetup.ExportDocSetupID,
            ControllerName: "ExportDocSetup",
            ActionName: "ActivateExportDocSetup",
            TableId: "tblExportDocSetups",
            IsWinClose: false
        };
        $.icsSave(obj);
        var SelectedRowIndex = $('#tblExportDocSetups').datagrid('getRowIndex', oExportDocSetup);
        $('#tblExportDocSetups').datagrid('updateRow', { index: SelectedRowIndex, row: oExportDocSetup });
    });
    
    DynamicRefreshList(_oExportDocSetups, "tblExportDocSetups");
}

function RefreshExportDocSetupLayout(buttonId) {
    if (buttonId === "btnViewExportDocSetup") {
        $("#winExportDocSetup input").prop("disabled", true);
        $("#winExportDocSetup select").prop("disabled", true);
        $("#btnSaveExportDocSetup").hide();
    }
    else {
        $("#winExportDocSetup input").not("#txtExportDocSetupCode").prop("disabled", false);
        $("#winExportDocSetup select").prop("disabled", false);
        $("#btnSaveExportDocSetup").show();
    }
    //$("#txtNumebr").icsNumberField({ precision: 2 });
}

function RefreshExportDocSetupControl(oExportDocSetup) {
  
    debugger;
    $("#txtDocName").val(oExportDocSetup.DocName);
    $("#txtDocHeader").val(oExportDocSetup.DocHeader);
    $("#txtBeneficiary").val(oExportDocSetup.Beneficiary);
    $("#txtBillNo").val(oExportDocSetup.BillNo);
    $("#txtNoAndDateOfDoc").val(oExportDocSetup.NoAndDateOfDoc);
    $("#cboDocumentType").val(oExportDocSetup.DocumentTypeInt);
    $("#cboExportLCType").val(oExportDocSetup.ExportLCType);
    $("#cboReportOn").val(oExportDocSetup.PrintOn);
    $("#cboProductPrintType").val(oExportDocSetup.ProductPrintType);
    $("#txtProformaInvoiceNoAndDate").val(oExportDocSetup.ProformaInvoiceNoAndDate);
    $("#txtAccountOf").val(oExportDocSetup.AccountOf);
    $("#txtDocumentaryCreditNoDate").val(oExportDocSetup.DocumentaryCreditNoDate);
    $("#txtAgainstExportLC").val(oExportDocSetup.AgainstExportLC);
    $("#txtPortofLoading").val(oExportDocSetup.PortofLoading);
    $("#txtFinalDestination").val(oExportDocSetup.FinalDestination);
    $("#txtIssuingBank").val(oExportDocSetup.IssuingBank);
    $("#txtNegotiatingBank").val(oExportDocSetup.NegotiatingBank);
    $("#txtCountryofOrigin").val(oExportDocSetup.CountryofOrigin);
    $("#txtTermsofPayment").val(oExportDocSetup.TermsofPayment);
    $("#txtAccount").val(oExportDocSetup.Account);
    $("#txtNotifyParty").val(oExportDocSetup.NotifyParty);
    $("#txtRemarks").val(oExportDocSetup.Remarks);
    $("#txtAmountInWord").val(oExportDocSetup.AmountInWord);
    $("#txtWecertifythat").val(oExportDocSetup.Wecertifythat);
    $("#txtCertification").val(oExportDocSetup.Certification);
    $("#txtDeliveryTo").val(oExportDocSetup.DeliveryTo);
    $("#txtChallanNo").val(oExportDocSetup.ChallanNo);
    $("#txtIRC").val(oExportDocSetup.IRC);
    $("#txtGarmentsQty").val(oExportDocSetup.GarmentsQty);
    $("#txtHSCode").val(oExportDocSetup.HSCode);
    $("#txtAreaCode").val(oExportDocSetup.AreaCode);
    $("#txtSpecialNote").val(oExportDocSetup.SpecialNote);
    $("#txtRemark").val(oExportDocSetup.Remark);
    $("#chkIsPrintVat").prop("checked", oExportDocSetup.IsVat);
    $("#chkIsPrintRegistration").prop("checked", oExportDocSetup.IsRegistration);
    $("#chkIsPrintHeader").prop("checked", oExportDocSetup.IsPrintHeader);
    $("#chkIsPrintOriginal").prop("checked", oExportDocSetup.IsPrintOriginal);
    $("#chkIsPrintGrossNetWeight").prop("checked", oExportDocSetup.IsPrintGrossNetWeight);
    $("#chkIsPrintDeliveryBy").prop("checked", oExportDocSetup.IsPrintDeliveryBy);
    $("#chkIsPrintTerm").prop("checked", oExportDocSetup.IsPrintTerm);
    $("#chkIsPrintQty").prop("checked", oExportDocSetup.IsPrintQty);
    $("#chkIsPrintUnitPrice").prop("checked", oExportDocSetup.IsPrintUnitPrice);
    $("#chkIsPrintValue").prop("checked", oExportDocSetup.IsPrintValue);
    $("#chkIsPrintWeight").prop("checked", oExportDocSetup.IsPrintWeight);
    $("#chkIsPrintFrieghtPrepaid").prop("checked", oExportDocSetup.IsPrintFrieghtPrepaid);
    $("#chkIsPrintInvoiceDate").prop("checked", oExportDocSetup.IsPrintInvoiceDate);
    $("#chkForCaptionInDubleLine").prop("checked", oExportDocSetup.ForCaptionInDubleLine);
    $("#chkIsShowAmendmentNo").prop("checked", oExportDocSetup.IsShowAmendmentNo);
    $("#txtClauseOne").val(oExportDocSetup.ClauseOne);
    $("#txtClauseTwo").val(oExportDocSetup.ClauseTwo);
    $("#txtClauseThree").val(oExportDocSetup.ClauseThree);
    $("#txtClauseFour").val(oExportDocSetup.ClauseFour);
    $("#txtCarrier").val(oExportDocSetup.Carrier);
    $("#txtAuthorisedSignature").val(oExportDocSetup.AuthorisedSignature);
    $("#txtReceiverSignature").val(oExportDocSetup.ReceiverSignature);
    $("#txtFor").val(oExportDocSetup.For);
    $("#txtMUnitName").val(oExportDocSetup.MUnitName);
    $("#txtNetWeightName").val(oExportDocSetup.NetWeightName);
    $("#txtGrossWeightName").val(oExportDocSetup.GrossWeightName);
    $("#txtBag_Name").val(oExportDocSetup.Bag_Name);
    $("#txtCountryofOriginName").val(oExportDocSetup.CountryofOriginName);
    $("#txtSellingOnAbout").val(oExportDocSetup.SellingOnAbout);
    $("#txtPortofLoadingName").val(oExportDocSetup.PortofLoadingName);
    $("#txtFinalDestinationName").val(oExportDocSetup.FinalDestinationName);
    $("#txtTruckNo_Print").val(oExportDocSetup.TruckNo_Print);
    $("#txtDriver_Print").val(oExportDocSetup.Driver_Print);
    $("#txtTO").val(oExportDocSetup.TO);
    $("#txtShippingMark").val(oExportDocSetup.ShippingMark);
    $("#txtReceiverCluse").val(oExportDocSetup.ReceiverCluse);
    $("#txtCarrierName").val(oExportDocSetup.CarrierName);
    $("#txtDescriptionOfGoods").val(oExportDocSetup.DescriptionOfGoods);
    $("#txtMarkSAndNos").val(oExportDocSetup.MarkSAndNos);
    $("#txtQtyInOne").val(oExportDocSetup.QtyInOne);
    $("#txtQtyInTwo").val(oExportDocSetup.QtyInTwo);
    $("#txtValueName").val(oExportDocSetup.ValueName);
    $("#txtUPName").val(oExportDocSetup.UPName);
    $("#txtNoOfBag").val(oExportDocSetup.NoOfBag);
    $("#txtCTPApplicant").val(oExportDocSetup.CTPApplicant);
    $("#txtGRPNoDate").val(oExportDocSetup.GRPNoDate);
    $("#txtASPERPI").val(oExportDocSetup.ASPERPI);
    $("#txtTextWithGoodsCol").val(oExportDocSetup.TextWithGoodsCol),
    $("#txtTextWithGoodsRow").val(oExportDocSetup.TextWithGoodsRow),
    $("#cboGoodsDesViewType").val(parseInt(oExportDocSetup.GoodsDesViewType));
    $("#txtToTheOrderOf").val(oExportDocSetup.ToTheOrderOf);
    $("#cboOrderOfBankType").val(parseInt(oExportDocSetup.OrderOfBankTypeInInt));
    $("#txtTermsOfShipment").val(oExportDocSetup.TermsOfShipment);

    $('#txtFontSize_Normal').val(oExportDocSetup.FontSize_Normal),
    $('#txtFontSize_ULine').val(oExportDocSetup.FontSize_ULine),
    $('#txtFontSize_Bold').val(oExportDocSetup.FontSize_Bold),
    $('#txtWeightPBag').val(oExportDocSetup.WeightPBag),
    $('#txtGrossWeightPTage').val(oExportDocSetup.GrossWeightPTage),
    $('#txtBagCount').val(oExportDocSetup.BagCount)
   


    debugger;
}

