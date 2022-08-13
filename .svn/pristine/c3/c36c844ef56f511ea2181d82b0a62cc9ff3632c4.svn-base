
var _nProductIdWarp = 0;
var _nProductIdWeft = 0;
var _IsWarp = true;

var editIndexFabricPatternWarp = undefined;
var editIndexFabricPatternWeft = undefined;
var _nEndsCountWarp = 0;
var _nEndsCountWeft = 0;
var _nValueWarp = 0;
var _nValueWeft = 0;
var _sColorWarp = 0;
var _sColorWeft = 0;
var _nLabDipDetailID = 0;

function InitializeFabricPattern() {

    $("#txtFPRepeatWarp,#txtFPRepeatWeft").val("");
    $("#txtReed").numberbox({min:0,precision:0});
    $("#txtPick").numberbox({ min: 0, precision: 0 });
    $("#txtDent").numberbox({ min: 0, precision: 2 });
    $("#txtGSM").numberbox({min:0,precision:2});
    $("#txtWarp").numberbox({ min: 0, precision: 0 });
    $("#txtWeft").numberbox({ min: 0, precision: 0 });

    $("#cboWeave").icsLoadCombo({
        List: _oFabricWeaves,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

    /*----------------- Product Search by text   ---------------------*/

    $("#txtSearchByProductNameWarp").keydown(function (e) {
        if (e.keyCode == 13) // Enter Press
        {
            _IsWarp = true;
            ProductSearch($("#txtSearchByProductNameWarp").val());
        }
    });

    $("#txtSearchByProductNameWeft").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            _IsWarp = false;
            ProductSearch($("#txtSearchByProductNameWeft").val());
        }
    });


    /*---------------------------------------------------------------*/
    $("#btnSaveFabricPattern").click(function () {
        endEditingFabricPatternWeft();
        endEditingFabricPatternWarp();
        if (!ValidateInputFabricPattern()) return;
        var oFabricPattern = RefreshObjectFabricPattern();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabricPattern,
            ObjectId: oFabricPattern.FPID,
            ControllerName: "Fabric",
            ActionName: "SaveFP",
            TableId: "tblFabricPatterns",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                
                if (response.obj.FPID > 0) {
                    InactiveOtherPatterns(response.obj);
                }
            }
        });
    });

    function InactiveOtherPatterns(oFP)
    {
        
        var oFPs = $("#tblFabricPatterns").datagrid("getRows");
        var nLength = oFPs.length;
        for (var i = 0; i < nLength; i++) {
            if (oFPs[i].FPID != oFP.FPID && oFPs[i].IsActive == true) {
                oFPs[i].IsActive = false;
                oFPs[i].Status = "Inactive";
                $("#tblFabricPatterns").datagrid("updateRow", { index: i, row: oFPs[i] });
            }
        }
    }

    $("#btnAddFPWarp").click(function () {
        _IsWarp = true;
        SaveFPDetail();
    });

    $("#txtEndsCountWarp").keydown(function (e) {
        if (e.keyCode === 13) {
            if ($.trim($("#txtEndsCountWarp").val()).length == 0 || parseInt($("#txtEndsCountWarp").val()) == 0) {
                $("#txtEndsCountWarp").addClass("errorFieldBorder");
                $("#txtEndsCountWarp").focus();
                alert("Ends cannot be zero.");
                return false;
            } else {
                $("#txtEndsCountWarp").removeClass("errorFieldBorder");
            }

            _IsWarp = true;
            SaveFPDetail();
        }
    });

    $("#btnAddFPWeft").click(function () {
        _IsWarp = false;
        SaveFPDetail();
    });

    $("#txtEndsCountWeft").keydown(function (e) {
        if (e.keyCode === 13) {
            if ($.trim($("#txtEndsCountWeft").val()).length == 0 || parseInt($("#txtEndsCountWeft").val()) == 0) {
                $("#txtEndsCountWeft").addClass("errorFieldBorder");
                $("#txtEndsCountWeft").focus();
                alert("Ends cannot be zero.");
                return false;
            } else {
                $("#txtEndsCountWeft").removeClass("errorFieldBorder");
            }
            _IsWarp = false;
            SaveFPDetail();
        }
    });

    $("#btnRemoveFPWarp").click(function () {
        FPDetailDeleteOperation("tblFabricPatternWarp");
    });

    $("#btnRemoveFPWeft").click(function () {
        FPDetailDeleteOperation("tblFabricPatternWeft");
    });

    $("#btnCloseFabricPatter").click(function () {
        ResetControll();
        $("#winFabricPattern").icsWindow("close");
    });

    $("#btnPrintFabricPattern").click(function () {
        var nts = ((new Date()).getTime()) / 1000;
        window.open(_sBaseAddress + '/Fabric/PrintFabricPattern?nFPID=' + _oFabricPattern.FPID + "&bIsDeisplyPattern=" + true +"&nBUID="+_nBUID+"&nts=" + nts, "_blank");

    });

    $("#btnRepeatFPWarp").click(function () {
        RepeatWarp();
    });

    $("#txtFPRepeatWarp").keydown(function (e) {
        if (e.keyCode === 13)
        {
            RepeatWarp();
        }
    });

    $("#btnRepeatFPWeft").click(function () {
        RepeatWeft();
    });

    $("#txtFPRepeatWeft").keydown(function (e) {
        if (e.keyCode === 13) {
            RepeatWeft();
        }
    });

    $("#btnCopyWarp").click(function () {
        var oWarps = $("#tblFabricPatternWarp").datagrid("getChecked");
        if (oWarps == null || oWarps.length == 0)
        {
            alert("Check item(s) from list.");
            return false;
        }

        if (_oFabricPattern == null || _oFabricPattern.FPID == 0)
        {
            alert("Invalid Fabric Pattern.");
            return false;
        }
     
        var nLength = oWarps.length;
        for (var i = 0; i < nLength; i++) {
            if (oWarps[i].GroupNo > 0) {
                CheckASet(oWarps[i], "tblFabricPatternWarp");
            }
        }

        var oFPDetails = $("#tblFabricPatternWarp").datagrid("getChecked");
        var sFPDetailIds = "";
        $.map(oFPDetails, function (c) {
            sFPDetailIds = c.FPDID + "," + sFPDetailIds;
        });
        sFPDetailIds = sFPDetailIds.substring(0, sFPDetailIds.length - 1);

        var oFPD = {
            FPID : _oFabricPattern.FPID,
            FPDetailIds: sFPDetailIds
        };

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFPD,
            ObjectId: oFPD.FPDID,
            ControllerName: "Fabric",
            ActionName: "CopyFromWarp",
            TableId: "",
            IsWinClose: false,
            Message: ""
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if ($.trim(response.obj.ErrorMessage) == "") {
                    if (response.obj.FPID > 0) {
                        DynamicRefreshList(response.obj.FabricPatternDetails, "tblFabricPatternWeft");
                    }
                }
            }
        });
    });

    $("#btnUndoRepeatFPWarp").click(function () {
        UndoRepeatedFPD("tblFabricPatternWarp");
    });

    $("#btnUndoRepeatFPWeft").click(function () {
        UndoRepeatedFPD("tblFabricPatternWeft");
    });


    $("#txtColorWarp").keydown(function (e) {
        if (e.keyCode == 13) // Enter Press
        {
            _IsWarp = true;
            GetsWaftWarp($("#txtColorWarp").val());
        }
    });

    $("#txtColorWeft").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            _IsWarp = false;
            GetsWaftWarp($("#txtColorWeft").val());
        }
    });

    $("#btnRefreshWarp").click(function () {
        
        endEditingFabricPatternWarp();
        RefreshSequence(true);
    });
    $("#btnRefreshWeft").click(function () {
        endEditingFabricPatternWeft();
        RefreshSequence(false);
    });
    $('#btnMakeTwistedWarp').click(function (e) {
        debugger;
        var bIsWarp = true;
        MakeTwistedAdd("tblFabricPatternWarp", bIsWarp);
    });
    $('#btnMakeTwistedWeft').click(function (e) {
        debugger;
        var bIsWarp = false;
        MakeTwistedAdd("tblFabricPatternWeft", bIsWarp);
    });

    $('#btnRemoveTwistedWarp').click(function (e) {
        debugger;

        var bIsWarp = true;
        RemoveTwiste("tblFabricPatternWarp", bIsWarp);
    });
    $('#btnRemoveTwistedWeft').click(function (e) {
        debugger;

        var bIsWarp = false;
        RemoveTwiste("tblFabricPatternWeft", bIsWarp);
    });
}

function UndoRepeatedFPD(sTableID)
{
    var oFPD = $("#" + sTableID).datagrid("getSelected");
    if (oFPD == null || oFPD.FPDID == 0)
    {
        alert("Select an item from list.");
        return false;
    }

    if (oFPD.GroupNo == 0 || oFPD.SetNo == 0)
    {
        alert("No repeated item found.");
        return false;
    }
    var nRowIndex = $("#" + sTableID).datagrid("getRowIndex", oFPD);
    $("#" + sTableID).datagrid("checkRow", nRowIndex);

    CheckASet(oFPD, sTableID);
    
    var oNewList = GetRepeatedListWithoutSelectedSet(oFPD, sTableID);
    var sFPDetailIds = "";
    $.map(oNewList, function (obj) {
        sFPDetailIds = obj.FPDID + "," + sFPDetailIds;
    });

    if ($.trim(sFPDetailIds) == "")
    {
        alert("No repeated item found.");
        return false;
    }
    if (!confirm("Make sure checked item(s) repeated part will remove. Confirm Remove?")) return false;

    sFPDetailIds = sFPDetailIds.substring(0, sFPDetailIds.length - 1);

    var oFPDetail = {
        FPDetailIds: sFPDetailIds
    };

    DeleteRepeatedItems(oFPDetail, sTableID, oFPD.GroupNo);
}

function DeleteRepeatedItems(oFPDetail, sTableID, nGroupNo) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/Fabric/DeleteFPDetail",
        traditional: true,
        data: JSON.stringify(oFPDetail),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var sMessage = jQuery.parseJSON(data);
            if (sMessage != null) {
                if (sMessage.toLowerCase() == "deleted") {
                    alert("Delete successfully.");

                    if ($.trim(sTableID) != "") {
                        var oTempFPDs = $("#" + sTableID).datagrid("getRows");
                        var splitFPDs = oFPDetail.FPDetailIds.split(',');

                        var oFPDs = [];
                        $.map(oTempFPDs, function (obj) {
                            if (obj.GroupNo == nGroupNo)
                            {
                                oFPDs.push(obj);
                            }
                        });

                        for (var j = oFPDs.length - 1; j >= 0; j--)
                        {
                            for (var i = splitFPDs.length - 1; i >= 0; i--) {
                                if (parseInt(oFPDs[j].FPDID) == parseInt(splitFPDs[i])) {
                                    var nRowIndex = $("#" + sTableID).datagrid("getRowIndex", oFPDs[j]);
                                    $("#" + sTableID).datagrid("deleteRow", nRowIndex);
                                    break;
                                }
                            }
                        }

                        $("#" + sTableID).datagrid("uncheckAll");
                        TotalEndsCalculation(sTableID);
                    }
                    else {
                        alert("Invalid table name.");
                        return false;
                    }
                }
                else {
                    alert(sMessage);
                    if (_IsWarp) {
                        editIndexFabricPatternWarp = undefined;
                        endEditingFabricPatternWarp();
                    } else {
                        editIndexFabricPatternWeft = undefined;
                        endEditingFabricPatternWeft();
                    }
                }
            }
            else {
                alert("Operation Unsuccessful.");
                if (_IsWarp) {
                    editIndexFabricPatternWarp = undefined;
                    endEditingFabricPatternWarp();
                } else {
                    editIndexFabricPatternWeft = undefined;
                    endEditingFabricPatternWeft();
                }
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function GetRepeatedListWithoutSelectedSet(oFPD, sTableID)
{
    var oFPDetails = $("#" + sTableID).datagrid("getRows");
    var oCheckedFPDs = $("#" + sTableID).datagrid("getChecked");
    var nGroupNo = parseFloat(oFPD.GroupNo);
    var oNewList = [];
    var bIsInList = false;

    //Get Repeated items without selected set
    if (nGroupNo > 0) {
        for (var i = 0; i < oFPDetails.length; i++) {
            bIsInList = false;
            if (oFPDetails[i].GroupNo == nGroupNo) {
                for (var j = 0; j < oCheckedFPDs.length; j++) {
                    if (oFPDetails[i].FPDID == oCheckedFPDs[j].FPDID) {
                        bIsInList = true;
                        break;
                    }
                }
                if (!bIsInList) {
                    oNewList.push(oFPDetails[i]);
                }
            }
        }
    }
    return oNewList;
}

function FPDetailDeleteOperation(sTableID) {
    var oFPDs = $("#" + sTableID).datagrid("getChecked");
    if (oFPDs == null || oFPDs.length == 0) {
        alert("Check item(s) from list!");
        return false;
    }

    var nLength = oFPDs.length;
    for (var i = 0; i < nLength; i++) {
        if (oFPDs[i].GroupNo > 0) {
            CheckASet(oFPDs[i], sTableID);
        }
    }

    if (!confirm("Make sure checked item(s) will delete. Confirm Delete?")) return false;

    var oFPDetails = $("#" + sTableID).datagrid("getChecked");
    var sFPDetailIds = "";
    $.map(oFPDetails, function (c) {
        sFPDetailIds = c.FPDID + "," + sFPDetailIds;
    });
    sFPDetailIds = sFPDetailIds.substring(0, sFPDetailIds.length - 1);

    var oFPDetail = {
        FPDetailIds: sFPDetailIds
    };

    DeleteFPDetail(oFPDetail, sTableID);
}

function CheckASet(oWarp, sTableID)
{
    var nRowIndex = $("#" + sTableID).datagrid("getRowIndex", oWarp);
    var oTempWarps = $("#" + sTableID).datagrid("getRows");

    //Check Upper items of this item
    if (oWarp.SetNo > 1)
    {
        for (var i = nRowIndex - 1; i >= 0; i--) {

            $("#" + sTableID).datagrid("checkRow", i);
            if (oTempWarps[i].SetNo == 1)
            {
                break;
            }
        }
    }
   
    //Check lower items under this item
    for (var i = nRowIndex + 1; i < oTempWarps.length; i++) {
        if (oTempWarps[i].SetNo > oWarp.SetNo) {
            $("#" + sTableID).datagrid("checkRow", i);
            continue;
        }
        else {
            break;
        }
    }
}

function RepeatWeft() {
    var nRepearNo = $("#txtFPRepeatWeft").val();
    if ($.trim(nRepearNo).length == 0 || parseFloat(nRepearNo) == 0) {
        alert("Please give repeat number.");
        $("#txtFPRepeatWeft").focus();
        $("#txtFPRepeatWeft").val("");
        $("#txtFPRepeatWeft").addClass("errorFieldBorder");
        return false;
    } else {
        $("#txtFPRepeatWeft").removeClass("errorFieldBorder");
    }

    var oRepeatObjs = $("#tblFabricPatternWeft").datagrid("getChecked");
    if (oRepeatObjs.length == 0) {
        alert("Please select minimum one item from below list.");
        return false;
    }

    var sFPIDs = "";
    for (var i = 0; i < oRepeatObjs.length; i++) {
        sFPIDs = sFPIDs + "," + oRepeatObjs[i].FPDID;
    }
    sFPIDs = sFPIDs.replace(',', ''); //remove first comma
    var nTotalItems = sFPIDs.split(",").length;
    var oFabricPatternDetail = {
        RepeatNo: parseInt(nRepearNo),
        Params: sFPIDs,
        FPID: oRepeatObjs[0].FPID,
        Count: parseInt(nTotalItems)
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/Fabric/PatternRepeat",
        traditional: true,
        data: JSON.stringify(oFabricPatternDetail),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var oFabricPatternDetail = jQuery.parseJSON(data);
            if (oFabricPatternDetail.ErrorMessage == "") {
                if (oFabricPatternDetail.FabricPatternDetails.length > 0) {
                    var WeftList = [];
                    var oFabricPatternDetails = oFabricPatternDetail.FabricPatternDetails;
                    for (var i = 0; i < oFabricPatternDetails.length; i++) {
                        if (oFabricPatternDetails[i].IsWarp == 0) {
                            WeftList.push(oFabricPatternDetails[i]);
                        }
                    }
                    DynamicRefreshListForMultipleSelection(WeftList, "tblFabricPatternWeft");
                    CalculateTotalWarpOrWeft(WeftList, "Weft");
                    editIndexFabricPatternWeft();
                    $("#txtFPRepeatWeft").val("");
                }
            }
            else {
                alert(oFabricPatternDetail.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function RepeatWarp() {
    var nRepearNo = $("#txtFPRepeatWarp").val();
    if ($.trim(nRepearNo).length == 0 || parseFloat(nRepearNo) == 0) {
        alert("Please give repeat number.");
        $("#txtFPRepeatWarp").focus();
        $("#txtFPRepeatWarp").val("");
        $("#txtFPRepeatWarp").addClass("errorFieldBorder");
        return false;
    } else {
        $("#txtFPRepeatWarp").removeClass("errorFieldBorder");
    }

    var oRepeatObjs = $("#tblFabricPatternWarp").datagrid("getChecked");
    if (oRepeatObjs.length == 0) {
        alert("Please select minimum one item from below list.");
        return false;
    }

    var sFPIDs = "";
    for (var i = 0; i < oRepeatObjs.length; i++) {
        sFPIDs = sFPIDs + "," + oRepeatObjs[i].FPDID;
    }
    sFPIDs = sFPIDs.replace(',', ''); //remove first comma
    var nTotalItems = sFPIDs.split(",").length;
    var oFabricPatternDetail = {
        RepeatNo: parseInt(nRepearNo),
        Params: sFPIDs,
        FPID: oRepeatObjs[0].FPID,
        Count: parseInt(nTotalItems)
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/Fabric/PatternRepeat",
        traditional: true,
        data: JSON.stringify(oFabricPatternDetail),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var oFabricPatternDetail = jQuery.parseJSON(data);
            if (oFabricPatternDetail.ErrorMessage == "") {
                if (oFabricPatternDetail.FabricPatternDetails.length > 0) {
                    var WarpList = [];
                    var oFabricPatternDetails = oFabricPatternDetail.FabricPatternDetails;
                    for (var i = 0; i < oFabricPatternDetails.length; i++) {
                        if (oFabricPatternDetails[i].IsWarp == 1) {
                            WarpList.push(oFabricPatternDetails[i]);
                        }
                    }
                    DynamicRefreshList(WarpList, "tblFabricPatternWarp");
                    CalculateTotalWarpOrWeft(WarpList, "Warp");
                    editIndexFabricPatternWarp = undefined;
                    endEditingFabricPatternWarp();
                    $("#txtFPRepeatWarp").val("");
                }
            }
            else {
                alert(oFabricPatternDetail.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function SetFabriPatternInfo(){
    $("#txtFabricId").val(_oFabric.FabricNo);
    $("#txtConstruction").val(_oFabric.Construction);
    $("#txtBuyer").val(_oFabric.BuyerName);
    $("#cboWeave").val(_oFabric.FabricWeave);
}


function ProductSearch(sProductName)
{

    
    DynamicRefreshList([], "tblProductPickerByName");
  
    var oProduct = {
        BUID: _nBUID,
        ModuleNameInInt:201, //FabricPattern
        ProductUsagesInInt:1, //Regular
        ProductName: sProductName,
    };

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oProduct,
        ControllerName: "Product",
        ActionName: "GetProductsByBUModuleWithProductUse",
        IsWinClose: false
    };


    $.icsDataGets(obj, function (response) {

        if (response.status && response.objs.length > 0) {

            if (response.objs[0].ProductID > 0) {
                var tblColums = [];
                var oColumn = { field: "ProductCode", title: "Code", width: 120, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ProductName", title: "ProductName", width: 300, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ProductCategoryName", title: "Type", width: 80, align: "left" }; tblColums.push(oColumn);

                var oPickerParam = {
                    winid: 'winProductPicker',
                    winclass: 'clsProductPicker',
                    winwidth: 600,
                    winheight: 480,
                    tableid: 'tblProductPicker',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'ProductName',
                    windowTittle: 'Product List'
                };
                $.icsPicker(oPickerParam);
                IntializeProductPickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
        else { alert("No Product Found."); }
    });
}

function GetsWaftWarp(sColorName) {

    debugger;
    var oFabricPattern = {
        FabricID: _oFabric.FabricID,
        Note: sColorName // here Note property use for color name string
    };
    var txtId = $(this).attr("id");
    oFabricPattern.ProductID = (txtId == "txtColorWarp" ? _nProductIdWarp : _nProductIdWeft);
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oFabricPattern,
        ControllerName: "Fabric",
        ActionName: "GetFPColors",
        IsWinClose: false
    };


    $.icsDataGets(obj, function (response) {

        if (response.status && response.objs.length > 0) {

            if (response.objs[0].ProductID > 0) {
                var tblColums = [];
                var oColumn = { field: "ColorName", title: "Code", width: 120, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ColorName", title: "ColorName", width: 300, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ColorNo", title: "LD No", width: 80, align: "left" }; tblColums.push(oColumn);

                var oPickerParam = {
                    winid: 'winFPColorPicker',
                    winclass: 'clsFPColorPicker',
                    winwidth: 450,
                    winheight: 400,
                    tableid: 'tblFPColorPicker',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'ColorName',
                    windowTittle: 'Color List'
                };
                $.icsPicker(oPickerParam);
                IntializeProductPickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
        else { alert("No Product Found."); }
    });
}

function IntializePickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        debugger;
        if (oPickerobj.winid == 'winProductPicker') {
            var obj = $('#' + oPickerobj.tableid).datagrid('getSelected');
            var elementId = (_IsWarp) ? 'txtSearchByProductNameWarp' : 'txtSearchByProductNameWeft';
            var elementIdFocus = (_IsWarp) ? 'txtColorWarp' : 'txtColorWeft';
            if (obj != null && obj.ProductID > 0) {
                $('#' + elementId).val(obj.ProductName);
                $("#" + elementIdFocus).focus();
                if (_IsWarp)
                    _nProductIdWarp = obj.ProductID;
                else
                    _nProductIdWeft = obj.ProductID;
            }
            else {
                if (_IsWarp)
                    _nProductIdWarp = 0;
                else
                    _nProductIdWeft = 0;
            }
        }

        else if (oPickerobj.winid == 'winFPColor') {
            var obj = $('#' + oPickerobj.tableid).datagrid('getSelected');
            var elementId = ($('#tblFPColors').data('IsWarp')) ? 'txtColorWarp' : 'txtColorWeft';
            var elementIdFocus = ($('#tblFPColors').data('IsWarp')) ? 'txtEndsCountWarp' : 'txtEndsCountWeft';

            if (obj != null && obj.LabDipDetailID > 0) {
                $('#' + elementId).val(obj.ColorName);
                $('#' + elementId).addClass("fontColorOfPickItem");
            }
            else {
                $('#' + elementId).val("");
                $('#' + elementId).removeClass("fontColorOfPickItem");
            }
            $("#" + elementIdFocus).focus();
        }
        $("#" + oPickerobj.winid).icsWindow("close");
        $("#" + oPickerobj.winid).remove();

    });
}
function IntializeProductPickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        ProductSelect(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which == 13)//enter=13
        {
            ProductSelect(oPickerobj);
        }
    });
}
function ProductSelect(oPickerobj) {
    if (oPickerobj.winid == 'winProductPicker') {
        var obj = $('#' + oPickerobj.tableid).datagrid('getSelected');
        var elementId = (_IsWarp) ? 'txtSearchByProductNameWarp' : 'txtSearchByProductNameWeft';
        var elementIdFocus = (_IsWarp) ? 'txtColorWarp' : 'txtColorWeft';
        if (obj != null && obj.ProductID > 0)
        {
            $("#" + oPickerobj.winid).icsWindow("close");
            $("#" + oPickerobj.winid).remove();
            $('#' + elementId).val(obj.ProductName);
            $("#" + elementIdFocus).focus();
            if (_IsWarp)
                _nProductIdWarp = obj.ProductID;
            else
                _nProductIdWeft = obj.ProductID;
        }
        else {
            if (_IsWarp)
                _nProductIdWarp = 0;
            else
                _nProductIdWeft = 0;
        }
    }
    else if (oPickerobj.winid == 'winFPColorPicker') {
        debugger;
   
        var obj = $('#' + oPickerobj.tableid).datagrid('getSelected');
        var elementId = (_IsWarp) ? 'txtColorWarp' : 'txtColorWeft';
        var elementIdFocus = (_IsWarp)  ? 'txtEndsCountWarp' : 'txtEndsCountWeft';

        if (obj != null && obj.LabDipDetailID > 0) {
            $("#" + oPickerobj.winid).icsWindow("close");
            $("#" + oPickerobj.winid).remove();
            $('#' + elementId).val(obj.ColorName);
            $('#' + elementId).addClass("fontColorOfPickItem");
            _nLabDipDetailID=obj.LabDipDetailID;
            $("#" + elementIdFocus).focus();
        }
        else {
            $('#' + elementId).val("");
            $('#' + elementId).removeClass("fontColorOfPickItem");
        }
       
    }
}

function RefreshControlFabricPattern(oFabricPattern)
{
    _oFabricPattern = oFabricPattern;
    $('#txtPatternNo').val(oFabricPattern.PatternNo);
    $("#txtFabricId").val(oFabricPattern.FabricNo);
    $("#txtConstruction").val(oFabricPattern.Construction);
    $("#txtBuyer").val(oFabricPattern.BuyerName);
    $("#cboWeave").val(oFabricPattern.Weave);
    $("#txtReed").numberbox("setValue",oFabricPattern.Reed);
    $("#txtPick").numberbox("setValue", oFabricPattern.Pick);
    $("#txtDent").numberbox("setValue", oFabricPattern.Dent);
    $("#txtGSM").numberbox("setValue",oFabricPattern.GSM);
    $("#txtWarp").numberbox("setValue",oFabricPattern.Warp);
    $("#txtWeft").numberbox("setValue", oFabricPattern.Weft);
    $("#txtNote").val(oFabricPattern.Note);
    $("#txtRatio").val(oFabricPattern.Ratio);
    $("#txtRepeatSize").val(oFabricPattern.RepeatSize);
}

function RefreshObjectFabricPattern() {

    var oFabricPattern = {
        FPID: (_oFabricPattern!=null)?_oFabricPattern.FPID:0,
        FabricID: _oFabric.FabricID,
        Weave : $("#cboWeave").val(),
        Reed : $("#txtReed").numberbox("getValue"),
        Pick: $("#txtPick").numberbox("getValue"),
        Dent: $("#txtDent").numberbox("getValue"),
        GSM: $("#txtGSM").numberbox("getValue"),
        Warp: $("#txtWarp").numberbox("getValue"),
        Weft: $("#txtWeft").numberbox("getValue"),
        Note: $("#txtNote").val(),
        Ratio: $("#txtRatio").val(),
        RepeatSize: $("#txtRepeatSize").val()
    };
    return oFabricPattern;
}

function ValidateInputFabricPattern() {

    if (_oFabric.FabricID=0) {
        alert("No fabric found, please back to fabric."); 
        return false;
     } 

    return true;
}

function RefreshObjectFPDetail()
{
    var nFPID = (_oFabricPattern != null) ? _oFabricPattern.FPID : 0;
    var sColorWarp = $("#txtColorWarp").val();
    var sColorWeft = $("#txtColorWeft").val();
    var oFPDetail = {
        FPDID : (_oFPDetail!=null)? _oFPDetail.FPDID:0,
        FPID: nFPID,
        IsWarp : _IsWarp,
        ProductID: (_IsWarp) ? _nProductIdWarp : _nProductIdWeft,
        LabdipDetailID: (_IsWarp) ? _nLabDipDetailID : _nLabDipDetailID,
        ColorName: (_IsWarp) ? sColorWarp.toUpperCase() : sColorWeft.toUpperCase(),
        EndsCount: (_IsWarp) ? parseInt($("#txtEndsCountWarp").val()) : parseInt($("#txtEndsCountWeft").val()),
        FP: (nFPID>0) ? null : RefreshObjectFabricPattern()
    };

    return oFPDetail;
}

function ValidateInputFPDetail() {

    if (_IsWarp) {

        if (_nProductIdWarp <= 0) {
            alert("No product found."); $('#txtSearchByProductNameWarp').focus();
            $("#txtSearchByProductNameWarp").addClass("errorFieldBorder");
            return false;
        } else { $("#txtSearchByProductNameWarp").removeClass("errorFieldBorder"); }

        if ( $.trim($('#txtColorWarp').val()).length == 0) {
            alert("Please color name."); $('#txtColorWarp').focus();
            $("#txtColorWarp").addClass("errorFieldBorder");
            return false;
        } else { $("#txtColorWarp").removeClass("errorFieldBorder"); }

        if (!$.trim($("#txtEndsCountWarp").val() || parseInt($("#txtEndsCountWarp").val()) == 0)) {
            $("#txtEndsCountWarp").focus();
            $("#txtEndsCountWarp").addClass("errorFieldBorder");
        } else {
            $("#txtEndsCountWarp").removeClass("errorFieldBorder");
        }
    }
    else {
        if (_nProductIdWeft <= 0) {
            alert("No product found."); $('#txtSearchByProductNameWeft').focus();
            $('#txtSearchByProductNameWeft').css("border", "1px solid #c00");
            return false;
        } else { $('#txtSearchByProductNameWeft').css("border", ""); }

        if ($.trim($('#txtColorWeft').val()).length == 0) {
            alert("Please color name."); $('#txtColorWeft').focus();
            $('#txtColorWeft').css("border", "1px solid #c00");
            return false;
        } else { $('#txtColorWeft').css("border", ""); }


        if (!$.trim($("#txtEndsCountWeft").val() || parseInt($("#txtEndsCountWeft").val()) == 0)) {
            $("#txtEndsCountWeft").focus();
            $("#txtEndsCountWeft").addClass("errorFieldBorder");
        } else {
            $("#txtEndsCountWeft").removeClass("errorFieldBorder");
        }
    }
    return true;
}

function SaveFPDetail() {

    if (!ValidateInputFPDetail()) return;
    var oFPDetail = RefreshObjectFPDetail();

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oFPDetail,
        ObjectId: oFPDetail.FPDID,
        ControllerName: "Fabric",
        ActionName: "SaveFPDetail",
        TableId: (_IsWarp) ? "tblFabricPatternWarp" : "tblFabricPatternWeft",
        IsWinClose: false,
        Message: ""
    };

    $.icsSave(obj, function (response) {
        if (response.status && response.obj != null) {
            oFPD = response.obj;
            if (oFPD.FPDID > 0) {
                if (_IsWarp) {
                    $("#txtColorWarp").val("");
                    $("#txtEndsCountWarp").val("");
                    $("#txtColorWarp").focus();
                }
                else {
                    $("#txtColorWeft").val("");
                    $("#txtEndsCountWeft").val("");
                    $("#txtColorWeft").focus();
                }

                $(".copy").hide();
                $("#txtFabricId").css("width", "80%");

                var oFPs1 = $("#tblFabricPatterns").datagrid("getRows");
                for (var i = 0; i < oFPs1.length; i++) {
                    oFPs1[i].IsActive = 0;
                    oFPs1[i].Status = "Inactive";
                    $("#tblFabricPatterns").datagrid("updateRow", { index: i, row: oFPs1[i] });
                }

                if (oFPDetail.FPID <= 0) {
                    if (oFPD.FP.FPID > 0) {
                        _oFabricPattern = oFPD.FP;
                        $('#txtPatternNo').val(_oFabricPattern.PatternNo);
                        $('#tblFabricPatterns').datagrid('appendRow', _oFabricPattern);
                        var nIndex = $('#tblFabricPatterns').datagrid('getRows').length;
                        $('#tblFabricPatterns').datagrid('selectRow', nIndex-1);
                        $('#btnSaveFabricPattern').show();
                        $('#btnPrintFabricPattern').show();
                    }
                }
                $("#txtColorWarp").removeClass("fontColorOfPickItem");
                $("#txtColorWeft").removeClass("fontColorOfPickItem");

                
                var oFPs = [];
                if (_IsWarp) {
                    oFPs = $("#tblFabricPatternWarp").datagrid("getRows");
                    CalculateTotalWarpOrWeft(oFPs, "Warp");
                }
                else {
                    oFPs = $("#tblFabricPatternWeft").datagrid("getRows");
                    CalculateTotalWarpOrWeft(oFPs, "Weft");
                }
            }
            else {
                alert(oFPD.ErrorMessage);
            }
        }
    });
}

function DeleteFPDetail(oFPDetail,sTableID)
{
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _sBaseAddress + "/Fabric/DeleteFPDetail",
        traditional: true,
        data: JSON.stringify(oFPDetail),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var sMessage = jQuery.parseJSON(data);
            if (sMessage != null) {
                if (sMessage.toLowerCase() == "deleted") {
                    alert("Delete successfully.");
                    
                    if ($.trim(sTableID) != "") {
                        var oFPDs = $("#" + sTableID).datagrid("getChecked");

                        $.map(oFPDs, function (obj) {
                            var nRowIndex = $("#" + sTableID).datagrid("getRowIndex", obj);
                            $("#" + sTableID).datagrid("deleteRow", nRowIndex);
                        });

                        $("#" + sTableID).datagrid("uncheckAll");
                      
                        TotalEndsCalculation(sTableID);
                    }
                    else {
                        alert("Invalid table name.");
                        return false;
                    }
                }
                else {
                    alert(sMessage);
                    if (_IsWarp) {
                        editIndexFabricPatternWarp = undefined;
                        endEditingFabricPatternWarp();
                    } else {
                        editIndexFabricPatternWeft = undefined;
                        endEditingFabricPatternWeft();
                    }
                }
            }
            else {
                alert("Operation Unsuccessful.");
                if (_IsWarp) {
                    editIndexFabricPatternWarp = undefined;
                    endEditingFabricPatternWarp();
                } else {
                    editIndexFabricPatternWeft = undefined;
                    endEditingFabricPatternWeft();
                }
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function TotalEndsCalculation(sTableID)
{
    var nTotalEnds = 0;
    var oFPDs = $("#" + sTableID).datagrid("getRows");
    $.map(oFPDs, function (obj) {
        nTotalEnds += parseFloat(obj.EndsCount)
    });
    if (isNaN(nTotalEnds)) {
        nTotalEnds = 0;
    }
    var sTotalLblId = (sTableID == "tblFabricPatternWarp" ? "lblTotalEndsWarp" : "lblTotalEndsWeft");
    $("#" + sTotalLblId).text(nTotalEnds);
}


function endEditingFabricPatternWarp() {
    if (editIndexFabricPatternWarp == undefined) {
        return true;
    }
    if ($('#tblFabricPatternWarp').datagrid('validateRow', editIndexFabricPatternWarp)) {
        $('#tblFabricPatternWarp').datagrid('endEdit', editIndexFabricPatternWarp);
        $('#tblFabricPatternWarp').datagrid('selectRow', editIndexFabricPatternWarp);
        _IsWarp = true;
        var oFPDetail = $('#tblFabricPatternWarp').datagrid('getSelected');
        if (parseInt(oFPDetail.EndsCount) <= 0 || (parseInt(oFPDetail.EndsCount) == parseInt(_nEndsCountWarp) && _sColorWarp == oFPDetail.ColorName && _nValueWarp == oFPDetail.Value)) {
            editIndexFabricPatternWarp = undefined;
            return true;
        }
        else {
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oFPDetail,
                ObjectId: oFPDetail.FPDID,
                ControllerName: "Fabric",
                ActionName: "SaveFPDetail",
                TableId: (_IsWarp) ? "tblFabricPatternWarp" : "tblFabricPatternWeft",
                IsWinClose: false,
                Message: ""
            };

            $.icsSave(obj, function (response) {
                var oFPs = $("#tblFabricPatternWarp").datagrid("getRows");
                CalculateTotalWarpOrWeft(oFPs, "Warp");
                editIndexFabricPatternWarp = undefined;
                return true;

            });
        }
    }
    else { return false; }
}

function onClickRowFabricPatternWarp(index) {
    if (editIndexFabricPatternWarp != index) {
        if (endEditingFabricPatternWarp()) {
            $('#tblFabricPatternWarp').datagrid('selectRow', index).datagrid('beginEdit', index);
            var oFPDetail=$('#tblFabricPatternWarp').datagrid('getSelected');
            _nEndsCountWarp = parseInt(oFPDetail.EndsCount);
            _sColorWarp = oFPDetail.ColorName;
            _nValueWarp = oFPDetail.Value;
            editIndexFabricPatternWarp = index;
        }
        else {
            $('#tblFabricPatternWarp').datagrid('selectRow', editIndexFabricPatternWarp);
        }
    }
}



function endEditingFabricPatternWeft() {
    
    if (editIndexFabricPatternWeft == undefined) {
        return true;
    }
    if ($('#tblFabricPatternWeft').datagrid('validateRow', editIndexFabricPatternWeft)) {
        $('#tblFabricPatternWeft').datagrid('endEdit', editIndexFabricPatternWeft);
        $('#tblFabricPatternWeft').datagrid('selectRow', editIndexFabricPatternWeft);
        _IsWarp = false;
        var oFPDetail = $('#tblFabricPatternWeft').datagrid('getSelected');
        if (parseInt(oFPDetail.EndsCount) <= 0 || (parseInt(oFPDetail.EndsCount) == parseInt(_nEndsCountWeft) && _sColorWeft == oFPDetail.ColorName && _nValueWeft == oFPDetail.Value)) {
            editIndexFabricPatternWeft = undefined;
            return true;
        }
        else {
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oFPDetail,
                ObjectId: oFPDetail.FPDID,
                ControllerName: "Fabric",
                ActionName: "SaveFPDetail",
                TableId: (_IsWarp) ? "tblFabricPatternWarp" : "tblFabricPatternWeft",
                IsWinClose: false,
                Message: ""
            };

            $.icsSave(obj, function (response) {
                    var oFPs = $("#tblFabricPatternWeft").datagrid("getRows");
                    CalculateTotalWarpOrWeft(oFPs, "Weft");
                    editIndexFabricPatternWeft = undefined;
                    return true;
            });
        }
    }
    else {
        return false;
    }
}

function onClickRowFabricPatternWeft(index) {
    if (editIndexFabricPatternWeft != index) {
        if (endEditingFabricPatternWeft()) {
            $('#tblFabricPatternWeft').datagrid('selectRow', index).datagrid('beginEdit', index);
            var oFPDetail = $('#tblFabricPatternWeft').datagrid('getSelected');
            _nEndsCountWeft = parseInt(oFPDetail.EndsCount);
            _sColorWeft = oFPDetail.ColorName;
            _nValueWeft = oFPDetail.Value;
            editIndexFabricPatternWarp = index;
            editIndexFabricPatternWeft = index;
        }
        else {
            $('#tblFabricPatternWeft').datagrid('selectRow', editIndexFabricPatternWeft);
        }
    }
}


function ResetControll()
{
    $(".resetfield").val("");
    $("#txtReed").numberbox("setValue", "");
    $("#txtPick").numberbox("setValue", "");
    $("#txtDent").numberbox("setValue", "");
    $("#txtGSM").numberbox("setValue", "");
    $("#txtWarp").numberbox("setValue", "");
    $("#txtWeft").numberbox("setValue", "");

    $("#txtSearchByProductNameWarp").val("");
    $("#txtColorWarp").val("");
    $("#txtEndsCountWarp").val("");

    $("#txtSearchByProductNameWeft").val("");
    $("#txtColorWeft").val("");
    $("#txtEndsCountWeft").val("");
   
    $('#btnSaveFabricPattern').hide();
    $('#btnPrintFabricPattern').hide();
    _nProductIdWarp = 0;
    _nProductIdWeft = 0;
    DynamicRefreshListForMultipleSelection([], "tblFabricPatternWarp");
    DynamicRefreshListForMultipleSelection([], "tblFabricPatternWeft");
}

function onLoadSuccess(data) {
    debugger;
    var nIndex = 0;
    var nSpan = 0;
    for (var i = 0; i < _oCellRowSpans.length; i++) {
        var oMCell = [_oCellRowSpans[i].mergerCell];
        nIndex = oMCell[0][0];
        nSpan = oMCell[0][1];
        if (_oCellRowSpans[i].FieldName == "TwistedGroup") {
            $(this).datagrid('mergeCells', { index: nIndex, field: 'TwistedGroup', rowspan: nSpan });
            $(this).datagrid('mergeCells', { index: nIndex, field: 'EndsCount', rowspan: nSpan });
            $(this).datagrid('mergeCells', { index: nIndex, field: 'TotalEnd', rowspan: nSpan });
        }
    }
}
function onLoadSuccessWeft(data) {
    debugger;
    var nIndex = 0;
    var nSpan = 0;
    for (var i = 0; i < _oCellRowSpansWeft.length; i++) {
        var oMCell = [_oCellRowSpansWeft[i].mergerCell];
        nIndex = oMCell[0][0];
        nSpan = oMCell[0][1];
        if (_oCellRowSpansWeft[i].FieldName == "TwistedGroup") {
            $(this).datagrid('mergeCells', { index: nIndex, field: 'TwistedGroup', rowspan: nSpan });
            $(this).datagrid('mergeCells', { index: nIndex, field: 'EndsCount', rowspan: nSpan });
          
        }
    }
}

function RefreshSequence(IsWarp) {
    var sTableID = "";
    if (IsWarp == true) { sTableID = "tblFabricPatternWarp" } else { sTableID = "tblFabricPatternWeft" }

    var oRSDs = $("#" + sTableID).datagrid('getRows');
    if (oRSDs.length > 0) {
        for (var i = 0; i < oRSDs.length; i++) {
            oRSDs[i].SLNo = i + 1;
        }

        DynamicRefreshList(oRSDs, sTableID);
    }
}


function MakeTwistedAdd(sTableID,bIsWarp) {
  
    var oFabricPatternDetails = $("#" + sTableID).datagrid('getChecked');

    if (oFabricPatternDetails.length <= 0) {
        alert("No items found to be twisted."); return false;
    }

    var sFabricPatternDetailIDs = "";
    for (var i = 0; i < oFabricPatternDetails.length; i++) {
        sFabricPatternDetailIDs = sFabricPatternDetailIDs + oFabricPatternDetails[i].FPDID + ",";
    }
    sFabricPatternDetailIDs = sFabricPatternDetailIDs.substring(0, sFabricPatternDetailIDs.length - 1);
  
    var oFabricPatternDetail = {
        FPID: oFabricPatternDetails[0].FPID,
        TwistedGroup: 0,
        IsWarp:bIsWarp,
        ErrorMessage: sFabricPatternDetailIDs
    }

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oFabricPatternDetail,
        ObjectId: oFabricPatternDetail.FPDID,
        ControllerName: "Fabric",
        ActionName: "MakeTwistedGroup",
        TableId: "",
        IsWinClose: false,
        Message: ""// (oFabricPatternDetail.FabricPatternDetailID>0)?"Update Successfully." : "Save Successfully."
    };
    $.icsSave(obj, function (response) {
        if (response.status && response.obj != null) {
            debugger;
            if (response.obj.FabricPatternDetails.length > 0 && response.obj.FabricPatternDetails[0].FPDID > 0) {
                if (bIsWarp == true) {
                    _oCellRowSpans = response.obj.FabricPatternDetails[0].CellRowSpans;
                }
                else {
                    _oCellRowSpansWeft = response.obj.FabricPatternDetails[0].CellRowSpans;
                }
             //   DynamicRefreshList(response.obj.FabricPatternDetails, 'tblFabricPatternDetail');
                DynamicRefreshList(response.obj.FabricPatternDetails, sTableID);

            }
            else if (response.obj.FabricPatternDetails.length > 0 && response.obj.FabricPatternDetails[0].FPDID <= 0) {
                alert(response.obj.FabricPatternDetails[0].ErrorMessage);
            }
            else {
                alert(response.obj.ErrorMessage);
            }
        }
    });
}

function RemoveTwiste(sTableID, bIsWarp) {
    var oFabricPatternDetails = $("#" + sTableID).datagrid('getChecked');

    if (oFabricPatternDetails.length <= 0) {
        alert("No items found to be twisted."); return false;
    }

    var sFabricPatternDetailIDs = "";
    for (var i = 0; i < oFabricPatternDetails.length; i++) {
        sFabricPatternDetailIDs = sFabricPatternDetailIDs + oFabricPatternDetails[i].FPDID + ",";
    }
    sFabricPatternDetailIDs = sFabricPatternDetailIDs.substring(0, sFabricPatternDetailIDs.length - 1);

    var oFabricPatternDetail = {
        FPID: oFabricPatternDetails[0].FPID,
        TwistedGroup: 0,
        IsWarp: bIsWarp,
        ErrorMessage: sFabricPatternDetailIDs
    }

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oFabricPatternDetail,
        ObjectId: oFabricPatternDetail.FPDID,
        ControllerName: "Fabric",
        ActionName: "RemoveTwistedGroup",
        TableId: "",
        IsWinClose: false,
        Message: ""// (oFabricPatternDetail.FabricPatternDetailID>0)?"Update Successfully." : "Save Successfully."
    };
    $.icsSave(obj, function (response) {
        if (response.status && response.obj != null) {
            debugger;
            if (response.obj.FabricPatternDetails.length > 0 && response.obj.FabricPatternDetails[0].FPDID > 0) {
               
                if (bIsWarp == true) {
                    _oCellRowSpans = response.obj.FabricPatternDetails[0].CellRowSpans;
                }
                else {
                    _oCellRowSpansWeft = response.obj.FabricPatternDetails[0].CellRowSpans;
                }
                DynamicRefreshList(response.obj.FabricPatternDetails, sTableID);
            }
            else {
                alert(response.obj.ErrorMessage);
            }
        }
    });
    
}
