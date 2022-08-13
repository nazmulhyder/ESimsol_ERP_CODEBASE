function InitializeFabricStickerEvents() {
    debugger;

    $("#cboFabricDesignFabricSticker").icsLoadCombo({
        List: _oFabricDesigns,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

    $("#cboFabricWeaveFabricSticker").icsLoadCombo({
        List: _oFabricWeaves,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

    $("#btnSaveFabricSticker").click(function () {
        if (!ValidateInputFabricSticker()) return;
        var oFabricSticker = RefreshObjectFabricSticker();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabricSticker,
            ObjectId: oFabricSticker.FabricStickerID,
            ControllerName: "Fabric",
            ActionName: "SaveFabricSticker",
            TableId: "tblFabricStickers",
            IsWinClose: true
        };
        $.icsSave(obj);
    });

    $("#btnCloseFabricSticker").click(function () {
        $("#winFabricSticker").icsWindow("close");
    });

    $('#txtFabricMillNameFabricSticker').keyup(function (e) {
        
        if (e.keyCode == 65) // a/A
        {
            $('#txtFabricMillNameFabricSticker').val("Akij Textile Mills Ltd.");
        }
    })
}

function RefreshObjectFabricSticker() {
    var nPrice = 0;
    if ((!$.trim($("#txtPriceFabricSticker").val()).length) || parseInt($("#txtPriceFabricSticker").val()) == 0) {
        nPrice = 0;
    }
    else {
        nPrice = parseFloat($("#txtPriceFabricSticker").val());
    }
    var nFabricStickerID = (_oFabricSticker == null ? 0 : _oFabricSticker.FabricStickerID);
    var oFabricSticker = {
        FabricStickerID: nFabricStickerID,
        Title: $.trim($("#txtTitleFabricSticker").val()),
        FabricMillName: $.trim($("#txtFabricMillNameFabricSticker").val()),
        FabricArticleNo: $.trim($("#txtFabricArticleNoFabricSticker").val()),
        Composition: parseInt($("#cboCompositionFabricSticker").val()),
        FabricDesignID: parseInt($("#cboFabricDesignFabricSticker").val()),
        FabricWeave: parseInt($("#cboFabricWeaveFabricSticker").val()),
        Construction: $.trim($("#txtConstructionFabricSticker").val()),
        Width: $.trim($("#txtWidthFabricSticker").val()),
        Weight: $.trim($("#txtWeightFabricSticker").val()),
        FinishType: parseInt($("#cboFinishTypeFabricSticker").val()),
        StickerDate: $('#txtDateFabricSticker').datebox('getValue'),
        Price: parseFloat(nPrice),
        PrintCount: parseInt($("#txtPrintCountFabricSticker").val()),
        Email: $("#txtEmailFabricSticker").val(),
        Phone: $("#txtPhoneFabricSticker").val()
    };
    return oFabricSticker;
}

function ValidateInputFabricSticker() {
    if ((!$.trim($("#txtPrintCountFabricSticker").val()).length) || parseInt($("#txtPrintCountFabricSticker").val()) == 0) {
        alert("Please give how many times this sticker will print.");
        $('#txtPrintCountFabricSticker').focus();
        $('#txtPrintCountFabricSticker').addClass("errorFieldBorder");
        return false;
    } else {
        $('#txtPrintCountFabricSticker').removeClass("errorFieldBorder");
    }
    return true;
}