function InitializeFabricStickersEvents() {
    DynamicRefreshList(_oFabricStickers, "tblFabricStickers");
    LoadComboboxesFabricSticker();
    $(".number").val(0);

    $("#btnAddFabricSticker").click(function () {
        $("#winFabricSticker").icsWindow("open", "Add Fabric Sticker");
        $("#winFabricSticker input").val("");
        $("#winFabricSticker select").val(0);
        $("#winFabricSticker .number").val(0);
        _oFabricSticker = null;
        RefreshFabricStickerLayout("btnAddFabricSticker");
    });

    $("#btnEditFabricSticker").click(function () {
        var oFabricSticker = $("#tblFabricStickers").datagrid("getSelected");
        if (oFabricSticker == null || oFabricSticker.FabricStickerID <= 0) { alert("Please select an item from list!"); return; }
        $("#winFabricSticker").icsWindow("open", "Edit Fabric Sticker");
        _oFabricSticker = oFabricSticker;
        RefreshFabricStickerLayout("btnEditFabricSticker");
        GetFabricStickerInformation(oFabricSticker);
    });

    $("#btnViewFabricSticker").click(function () {
        var oFabricSticker = $("#tblFabricStickers").datagrid("getSelected");
        if (oFabricSticker == null || oFabricSticker.FabricStickerID <= 0) { alert("Please select an item from list!"); return; }
        $("#winFabricSticker").icsWindow("open", "Edit Fabric Sticker");
        _oFabricSticker = oFabricSticker;
        RefreshFabricStickerLayout("btnViewFabricSticker");
        GetFabricStickerInformation(oFabricSticker);
    });

    $("#btnDeleteFabricSticker").click(function () {
        var oFabricSticker = $("#tblFabricStickers").datagrid("getSelected");
        if (!confirm("Confirm to Delete?")) return false;
        if (oFabricSticker == null || oFabricSticker.FabricStickerID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabricSticker,
            ControllerName: "Fabric",
            ActionName: "DeleteFabricSticker",
            TableId: "tblFabricStickers",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });
}

function RefreshFabricStickerLayout(buttonId) {
    if (buttonId === "btnViewFabricSticker") {
        $("#winFabricSticker input").prop("disabled", true);
        $("#winFabricSticker select").prop("disabled", true);
    }
    else {
        $("#winFabricSticker input").prop("disabled", false);
        $("#winFabricSticker select").prop("disabled", false);
    }
}

function GetFabricStickerInformation(oFabricSticker) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oFabricSticker,
        ControllerName: "Fabric",
        ActionName: "GetFabricSticker",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.FabricStickerID > 0) {
                RefreshFabricStickerControl(response.obj);
            }
            else {
                alert(response.obj.ErrorMessage);
            }
        }
        else { alert("No information found."); }
    });
}


function RefreshFabricStickerControl(oFabricSticker) {
    $("#txtTitleFabricSticker").val(oFabricSticker.Title);
    $("#txtFabricMillNameFabricSticker").val(oFabricSticker.FabricMillName);
    $("#txtFabricArticleNoFabricSticker").val(oFabricSticker.FabricArticleNo);
    $("#cboCompositionFabricSticker").val(oFabricSticker.Composition);
    $("#cboFabricDesignFabricSticker").val(oFabricSticker.FabricDesignID);
    $("#cboFabricWeaveFabricSticker").val(oFabricSticker.FabricWeave);
    $("#txtConstructionFabricSticker").val(oFabricSticker.Construction);
    $("#txtWidthFabricSticker").val(oFabricSticker.Width);
    $("#txtWeightFabricSticker").val(oFabricSticker.Weight);
    $("#cboFinishTypeFabricSticker").val(oFabricSticker.FinishType);
    $("#txtDateFabricSticker").val(oFabricSticker.StickerDateSt);
    $("#txtPriceFabricSticker").val(oFabricSticker.Price);
    $("#txtPrintCountFabricSticker").val(oFabricSticker.PrintCount);
    $("#txtEmailFabricSticker").val(oFabricSticker.Email);
    $("#txtPhoneFabricSticker").val(oFabricSticker.Phone);
}

function LoadComboboxesFabricSticker() {
    $("#cboFinishTypeFabricSticker").icsLoadCombo({
        List: _oFinishTypes,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });
    PickProductInFabricSticker();
}

function PickProductInFabricSticker() {
    var sDBObjectName = "Fabric";
    var nTriggerParentsType = 109; //_ViewProduct
    var nOperationalEvent = 704; // _View
    var nInOutType = 100;  // None
    var nDirections = 0;
    var nStoreID = 0;
    var nMapStoreID = 0;

    var sProductName = "";//$("#txtProductFabric").val();
    var oProduct = {
        Params: $.trim(sProductName) + '~' + sDBObjectName + '~' + nTriggerParentsType + '~' + nOperationalEvent + '~' + nInOutType + '~' + nDirections + '~' + nStoreID + '~' + nMapStoreID
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oProduct,
        ControllerName: "Product",
        ActionName: "ATMLNewSearchByProductName",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ProductID > 0) {
                _oProductsPicker = response.objs;
                var listItems = "";
                for (var i = 0; i < response.objs.length; i++) {
                    if (response.objs[i].ProductID != 0) {
                        listItems += "<option value='" + response.objs[i].ProductID + "'>" + response.objs[i].ProductName + "</option>";
                    }
                }
                $("#cboCompositionFabricSticker").html(listItems);
            }
            else {
                $("#cboCompositionFabricSticker").empty();
            }
        }
        else {
            $("#cboCompositionFabricSticker").empty();
        }
    });
}

function isValidFabricSticker() {
    debugger;
    var oFabricStickers = [];
    var oFabricSticker = $("#tblFabricStickers").datagrid("getSelected");
    if (oFabricSticker == null || oFabricSticker.FabricStickerID <= 0) {
        alert("Please select an item from list.");
        return false;
    }
    oFabricSticker.StickerDate = oFabricSticker.StickerDateSt;
    oFabricStickers.push(oFabricSticker);
    $("#txtFabricStickerCollectionList").val(JSON.stringify(oFabricStickers));
    $("#txtFabricStickerCollectionListNew").val(JSON.stringify(oFabricStickers));
}