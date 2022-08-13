
function InitializeFabricsEvents() {
    /// search
    sessionStorage.setItem("ParamsP", "");

    $("#progressbar").progressbar({ value: 0 });

    $("#progressbarParent").hide();

    $('#txtSearchbyFabricNo').keyup(function (e) {
        var obj =
        {
            Event: e,
            SearchProperty: "FabricNo",
            GlobalObjectList: _oFabrics,
            TableId: "tblFabrics"
        };
        $('#txtSearchbyFabricNo').icsSearchByText(obj);
    });

    $("#txtSearchbyFabricNo").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            if ($.trim($("#txtSearchbyFabricNo").val()).length === 0) {
                alert("Please enter some text.");
                return false;
            }
            //Ratin
            var oFabric = {
                FabricNo: $.trim($("#txtSearchbyFabricNo").val())
            };

            //$("#txtSearchbyFabricNo").icsDBSearchByText({
            //    BaseAddress: _sBaseAddress,
            //    Object: oFabric,
            //    ControllerName: "Fabric",
            //    ActionName: "SearchByFabricNo",
            //    TableId: "tblFabrics"
            //});
            $("#progressbar").progressbar({ value: 0 });
            $("#progressbarParent").show();
            var intervalID = setInterval(updateProgress, 250);
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/Fabric/SearchByFabricNo",
                traditional: true,
                data: JSON.stringify(oFabric),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var oFabrics = data;
                    //alert(oFabrics.length + " - " + oFabric.BuyerName);
                    debugger;
                    if (oFabrics != null) {
                        if (oFabrics.length > 0) {
                            DynamicRefreshList(oFabrics, "tblFabrics");
                        }
                        else {
                            alert("Sorry, No data found.");
                            DynamicRefreshList([], "tblFabrics");
                        }

                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblFabrics");
                    }
                    clearInterval(intervalID);
                    $("#progressbarParent").hide();
                }
            });
        }
    });

    $('#txtSearchByBuyerName').keyup(function (e) {
        var obj =
        {
            Event: e,
            SearchProperty: "BuyerName",
            GlobalObjectList: _oFabrics,
            TableId: "tblFabrics"
        };
        $('#txtSearchByBuyerName').icsSearchByText(obj);
    });

    $("#txtSearchByBuyerName").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            if ($.trim($("#txtSearchByBuyerName").val()).length === 0) {
                alert("Please enter some text.");
                return false;
            }
            var oFabric = {
                BuyerName: $.trim($("#txtSearchByBuyerName").val())
            };
            $("#progressbar").progressbar({ value: 0 });
            $("#progressbarParent").show();
            var intervalID = setInterval(updateProgress, 250);
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/Fabric/SearchByBuyerName",
                traditional: true,
                data: JSON.stringify(oFabric),
                contentType: "application/json; charset=utf-8",
                success: function (data)
                {
                    debugger;
                    var oFabrics = data;
                    //alert(oFabrics.length + " - " + oFabric.BuyerName);
                    debugger;
                    if (oFabrics != null) {
                        if (oFabrics.length > 0) {
                            DynamicRefreshList(oFabrics, "tblFabrics");
                        }
                        else {
                            alert("Sorry, No data found.");
                            DynamicRefreshList([], "tblFabrics");
                        }

                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblFabrics");
                    }
                    clearInterval(intervalID);
                    $("#progressbarParent").hide();
                }
            });
        }
    });

    /// Fabric entry

    $("#btnAddFabric").click(function () {
        _oIsAdd = true;
        $("#winFabric").icsWindow("open", "Add Fabric");
        ResetAllFields("winFabric");

        UnselectAllRowsOfATable();
        _oFabric = null;

        RefreshCon_BuyerRequest(_oFabric);
        var nGroupID = 0;
        for (var i = 0; i < _oMktPersons.length; i++) {
            if (_oMktPersons[i].UserID === _nCurrentUserID) {
                debugger;
                nGroupID = _oMktPersons[i].GroupID;
                if (nGroupID > 0) {
                    var nExists = IsMKTHead(nGroupID)
                    $("#cboMktPersonsFabric").val(nExists);
                    break;
                }
            }

        }

        $('.fabricWeftColor').hide();
        $("#txtProduct,#txtContractorName,#txtMktPersonFabric").removeClass("fontColorOfPickItem");
        RefreshFabricLayout("btnAddFabric"); //button id as parameter
        $("#btnSaveFabric").show();
        $("#btnReFabricSubmissionSave").hide();
        $("#txtFabricWidthFabric").val('57"-58"');
        $('#txtNoOfFrame').val(4);
        $("#txtFabricNoFabric").prop("disabled", false);
        $("#txtHLNo,#txtActualConstruction").prop("disabled", true);
        $("#cboMktPersonsFabric").focus();

    });

    $("#btnEditFabric").click(function () {
        _oIsAdd = false;
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }
        _nSelectedRowIndex = $('#tblFabrics').datagrid('getRowIndex', oFabric);
        //if (oFabric.ApprovedBy != 0) {
        //    alert("Already approved.");
        //    return false;
        //}


        $("#winFabric").icsWindow('open', "Edit Fabric");
        _oFabric = oFabric;
        RefreshFabricLayout("btnEditFabric");
        $('#txtHLNo,#txtActualConstruction').prop('disabled', true);
        GetFabricInformation(oFabric);

        UnselectAllRowsOfATable();

        $('.fabricWeftColor').hide();
        $("#btnSaveFabric").show();
        $("#txtFabricNoFabric").prop("disabled", false);
        $("#btnReFabricSubmissionSave").hide();
        $("#cboMktPersonsFabric").focus();

    });

    $("#btnViewFabric").click(function () {
        _oIsAdd = false;
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }

        $("#winFabric").icsWindow('open', "View Fabric");
        $('.fabricWeftColor').hide();
        RefreshFabricLayout("btnViewFabric");
        GetFabricInformation(oFabric);
        UnselectAllRowsOfATable();
        $("#btnReFabricSubmissionSave").hide();
    });

    $("#btnDeleteFabric").click(function () {
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (oFabric.ApprovedBy > 0) {
            alert("Sorry, Alerady Approved.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabric,
            ControllerName: "Fabric",
            ActionName: "Delete",
            TableId: "tblFabrics",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $('#btnApproveFabric').click(function (e) {
        debugger;
        _oIsAdd = false;
        var oFabrics = $('#tblFabrics').datagrid('getChecked');
        if (oFabrics.length <= 0) {
            alert("Please checked at least one item.");
            return;
        }
        if (!confirm("Confirm to Approve?")) return false;
        //if(_nBankAccountID<=0)
        //{
        //    alert("Please Select Bank account");
        //    return;
        //    $('#txtAccountNo').focus()
        //}


        var nSelectedIndex = 0;
        var indexLists = [];
        for (i = 0; i < oFabrics.length; ++i) {
            if (oFabrics[i].ApprovedBy != 0) {
                alert("This is already Approved!!." + "  No:" + oFabrics[i].FabricNo + ".");
                return;
            }

            nSelectedIndex = $('#tblFabrics').datagrid('getRowIndex', oFabrics[i]);
            indexLists.push(nSelectedIndex);
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/Fabric/Approve",
            traditional: true,
            data: JSON.stringify(oFabrics),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oFabrics = jQuery.parseJSON(data);

                if (oFabrics[0].ErrorMessage == '' || oFabrics[0].ErrorMessage == null) {
                    for (i = 0; i < oFabrics.length; ++i) {
                        //oFabrics[i].IsOparate=-1;
                        $('#tblFabrics').datagrid('updateRow', { index: indexLists[i], row: oFabrics[i] });
                        $('#tblFabrics').datagrid('selectRow', indexLists[i]);
                    }
                    alert("Approved  Successfully");

                }
                else {
                    alert(oFabrics[0].ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(xhr + '~' + status + '~' + error);
            }
        });
    });

    $("#btnCopy").click(function () {
        var oFabric = $("#tblFabrics").datagrid('getSelected');
        if (oFabric == null || parseInt(oFabric.FabricID) <= 0) {
            alert("Select an item from list!");
            return;
        }
        if (!confirm("Confirm copy?")) return false;
        oFabric.FabricID = 0;
        oFabric.FabricNo = "";
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabric,
            ObjectId: oFabric.FabricID,
            ControllerName: "Fabric",
            ActionName: "Save",
            TableId: "tblFabrics",
            IsWinClose: true,
            Message: ""
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.FabricID > 0) {
                    _oFabric = response.obj;
                    RefreshFabricLayout("btnCopy");
                    RefreshFabricControl(_oFabric);
                    UnselectAllRowsOfATable();
                    $("#txtBuyerReferenceFabric").focus();
                    $('.fabricWeftColor').hide();
                    $("#winFabric").icsWindow('open', "Copy Fabric");
                }
            }
        });
    });

    //Fabric Attachment

    $('#btnAttachment').click(function (e) {
        var oFabric = $('#tblFabrics').datagrid('getSelected');
        if (oFabric == null || parseInt(oFabric.FabricID) <= 0) {
            alert("Please select a item from list!");
            return;
        }

        //EnumAttchRefType : UserSignature = 4
        window.open(_sBaseAddress + '/Fabric/ViewFabricAttachment?id=' + parseInt(oFabric.FabricID) + '&OperationInfo= Selected Fabrics : ' + oFabric.FabricNo, '_blank');
    });
    //End Fabric Attachment

    //Desk Loom
    $('#btnPrintDeskLoomFabric').click(function (e) {
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }

        var nts = ((new Date()).getTime()) / 1000;
        window.open(_sBaseAddress + '/Fabric/PrintDeskLoom?nFabricID=' + oFabric.FabricID + "&nts=" + nts, "_blank");
    });


    //Fabric Sticker
    $("#btnPrintStickerFabric").click(function () {
        var oFabrics = $("#tblFabrics").datagrid("getChecked");
        if (oFabrics == null || oFabrics.length == 0) {
            alert("Please select minimum one item from list.");
            return false;
        }
        _oStickerPrintList = oFabrics;
        $("#txtStickerHeaderFabric").val("");
        $("#txtStickerHeaderFabric").focus();
        $("#txtStickerHeaderFabric").removeClass("errorFieldBorder");
        $("#winPrintStickerHeader").icsWindow("open", "Set Sticker Header");
        $("#txtStickerHeaderFabric").focus();
    });

    $("#btnOkStickerHeaderFabric").click(function () {
        FabricStickerPrint();
    });

    $("#btnPrintStickerFabric").click(function () {
        var oFabrics = $("#tblFabrics").datagrid("getChecked");
        if (oFabrics == null || oFabrics.length == 0) {
            alert("Please select minimum one item from list.");
            return false;
        }
        _oStickerPrintList = oFabrics;
        $("#txtStickerHeaderFabric").val("");
        $("#txtStickerHeaderFabric").focus();
        $("#txtStickerHeaderFabric").removeClass("errorFieldBorder");
        $("#winPrintStickerHeader").icsWindow("open", "Set Sticker Header");
        $("#txtStickerHeaderFabric").focus();
    });

    $("#btnCloseStickerHeaderFabric").click(function () {
        $("#winPrintStickerHeader").icsWindow("close");
    });

    $("#txtStickerHeaderFabric").keydown(function (e) {
        if (e.keyCode === 13) {
            FabricStickerPrint();
        }
    });

    //Print XL
    $("#btnPrintXL").click(function () {
        var sParams = sessionStorage.getItem("ParamsP");
        window.open(_sBaseAddress + '/Fabric/Print_ReportXL?sTempString=' + sParams, "_blank");
    });

    $("#btnAddFNLabdipDetail").click(function () {
       $("#winFNLabdip").icsWindow("open", "Add Labdip");
       var oFabric = $("#tblFabrics").datagrid("getSelected");
       if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }
       _oFabric = oFabric;
       _nSelectedRowIndex = $('#tblFabrics').datagrid('getRowIndex', oFabric);
      RefreshCon_LabDip(_oFabric);

    });
};

function IsMKTHead(nGroupID) {
    for (var i = 0; i < _oMktPersons.length; i++) {
        if (parseInt(_oMktPersons[i].GroupID) === parseInt(nGroupID) && _oMktPersons[i].IsGroupHead === true) {
            return _oMktPersons[i].MarketingAccountID;
        }
    }
    return 0;
}

function FabricStickerPrint() {
    var sStickerHeader = $("#txtStickerHeaderFabric").val(); //A&A
    if (sStickerHeader.length == 0) {
        $("#txtStickerHeaderFabric").val("");
        $("#txtStickerHeaderFabric").focus();
        $("#txtStickerHeaderFabric").addClass("errorFieldBorder");
        alert("Please Give Sticker Header");
        return false;
    } else {
        $("#txtStickerHeaderFabric").removeClass("errorFieldBorder");
    }
    $("#winPrintStickerHeader").icsWindow("close");
    sStickerHeader = sStickerHeader.replace("&", "aaaa");

    var sIds = "";
    for (var i = 0; i < _oStickerPrintList.length; i++) {
        sIds = _oStickerPrintList[i].FabricID + "," + sIds;
    }
    sIds = sIds.substring(0, sIds.length - 1);

    var nts = ((new Date()).getTime()) / 1000;
    window.open(_sBaseAddress + '/Fabric/PrintSticker?sStickerHeader=' + sStickerHeader + "&sParams=" + sIds + "&nts=" + nts, "_blank");
}