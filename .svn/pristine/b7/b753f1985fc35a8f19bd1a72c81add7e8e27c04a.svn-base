
function InitializeFabricPatterns() {
   
    $('#btnEditFabricPattern').hide();
    $("#btnDeleteFabricPattern").hide();
    $("#btnApproveFabricPattern").hide();
    //$("#btnActivityFabricPattern").hide();
    $('#btnActivityFabricPattern').find("label").text("Active");

    $('#tblFabricPatterns').datagrid({ onSelect: function (rowIndex, rowData) { OperationPerforms(rowIndex, rowData); } });
    DynamicRefreshList(_oFabricPatterns, "tblFabricPatterns");

    SetFabricInfo();

    $('#btnBackToFabric').click(function (e) {
        if (sessionStorage.getItem("BackLink") != "")
            window.location.href = sessionStorage.getItem("BackLink");
        else
            window.location.href = _sBaseAddress + '/Fabric/ViewFabricForPattern?menuid=' + _nmenuid + "&nFabricId=" + _oFabric.FabricID;
    });

    $("#btnAddFabricPattern").click(function () {
        ResetControll();
        $("#winFabricPattern").icsWindow('open', "Add Fabric Pattern");
        //$("#txtWarp").numberbox("setValue", 20);
        //$("#txtWeft").numberbox("setValue", 20);
        _oFabricPattern = null;
        SetFabriPatternInfo();
        RefreshFabricLayout("btnAddFabricPattern");
        CalculateTotalWarpOrWeft([], "Warp");
        CalculateTotalWarpOrWeft([], "Weft");

        $(".copy").show();
        $("#txtFabricId").css("width","84px");
    });

    $('#btnEditFabricPattern').click(function (e) {
        debugger;
        var oFabricPattern = $("#tblFabricPatterns").datagrid("getSelected");
        if (oFabricPattern == null || oFabricPattern.FPID <= 0) { alert("Please select an item from list!"); return; }
        ResetControll();
        if (oFabricPattern.ApproveBy > 0) { alert('Unable to edit. Already Approved.'); return; }
        $("#winFabricPattern").icsWindow('open', "Edit Fabric Pattern");
        RefreshFabricLayout("btnEditFabricPattern");
        GetFabricPattern(oFabricPattern);

        $(".copy").hide();
        $("#txtFabricId").css("width", "80%");
    });

    $('#btnViewFabricPattern').click(function (e) {
        var oFabricPattern = $("#tblFabricPatterns").datagrid("getSelected");
        if (oFabricPattern == null || oFabricPattern.FPID <= 0) { alert("Please select an item from list!"); return; }
        ResetControll();
        $("#winFabricPattern").icsWindow('open', "View Fabric Pattern");
        RefreshFabricLayout("btnViewFabric");
        GetFabricPattern(oFabricPattern);

        $(".copy").hide();
        $("#txtFabricId").css("width", "80%");
    });

    $("#btnDeleteFabricPattern").click(function () {

        var oFabricPattern = $("#tblFabricPatterns").datagrid("getSelected");
        if (oFabricPattern == null || oFabricPattern.FPID <= 0) { alert("Please select an item from list!"); return; }
        if (oFabricPattern.ApproveBy > 0) { alert('Unable to Delete. Already Approved.'); return; }
        if (!confirm("Confirm to Delete?")) return;
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oFabricPattern,
            ControllerName: "Fabric",
            ActionName: "DeleteFP",
            TableId: "tblFabricPatterns",
            IsWinClose: false
        };
        $.icsDelete(obj);


    });

    $("#btnApproveFabricPattern").click(function (e) {

        var oFabricPattern = $('#tblFabricPatterns').datagrid('getSelected');
        if (oFabricPattern == null || oFabricPattern.FPID <= 0) { alert("Please select a item from list!"); return; }

        if (oFabricPattern.ApproveBy > 0) { alert('Already Approved.'); return; }
        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oFabricPattern,
                ObjectId: oFabricPattern.FPID,
                ControllerName: "Fabric",
                ActionName: "ApproveFP",
                TableId: "tblFabricPatterns",
                IsWinClose: false,
                Message: "Approved Successfully."
            };
        $.icsSave(obj, function (response) {
            
            if (response.status && response.obj != null) {

                if (response.obj.FPID > 0) {

                    if (response.obj.ApproveBy>0) {
                        $('#btnEditFabricPattern').hide();
                        $("#btnDeleteFabricPattern").hide();
                        $("#btnApproveFabricPattern").hide();
                        //$("#btnActivityFabricPattern").show();
                        $('#btnActivityFabricPattern').find("label").text("Active");
                    }
                }
            }
        });

    });
    $("#btnCopyPattern").click(function (e) {

        var oFabricPattern = $('#tblFabricPatterns').datagrid('getSelected');
        if (oFabricPattern == null || oFabricPattern.FPID <= 0) { alert("Please select a item from list!"); return; }
      
     
        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oFabricPattern,
                ObjectId: oFabricPattern.FPID,
                ControllerName: "Fabric",
                ActionName: "CopyFP",
                TableId: "tblFabricPatterns",
                IsWinClose: true,
                Message: "Copy Successfully."
            };
        $.icsSave(obj, function (response) {

            if (response.status && response.obj != null) {

                if (response.obj.FPID > 0) {
                    debugger;
                    $("#tblFabricPatterns").datagrid("appendRow", response.obj);
                    //InactiveOtherPatternsTwo(response.obj);
                    var oFPs = $("#tblFabricPatterns").datagrid("getRows");
                    var nIndex = oFPs.length;
                    $("#tblFabricPatterns").datagrid("selectRow", nIndex);
                }
            }
        });

    });

    $("#btnActivityFabricPattern").click(function (e) {
        var oFabricPattern = $('#tblFabricPatterns').datagrid('getSelected');
        if (oFabricPattern == null || oFabricPattern.FPID <= 0) { alert("Please select a item from list!"); return; }
        var bIsShowConfirmMessage = true;
        var oFabricPatterns = $('#tblFabricPatterns').datagrid('getRows');
        for (var i = 0; i < oFabricPatterns.length; i++) {
            if (oFabricPatterns[i].IsActive && (oFabricPattern.FPID != oFabricPatterns[i].FPID)) {
                //var bConfirm = confirm("First inactive pattern no: " + oFabricPatterns[i].PatternNo);
                var bConfirm = confirm("Already has an active pattern (" + oFabricPatterns[i].PatternNo + "). Do you want to forcely active your selected pattern?");
                if (!bConfirm) {
                    return false;
                }
                else {
                    bIsShowConfirmMessage = false;
                    break;
                }
            }
        }
        if (bIsShowConfirmMessage)
        {
            if (!confirm("Are you sure to change activity.")) return false;
        }
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oFabricPattern,
            ObjectId: oFabricPattern.FPID,
            ControllerName: "Fabric",
            ActionName: "ChangeActivityFP",
            TableId: "tblFabricPatterns",
            IsWinClose: false,
            Message: (oFabricPattern.IsActive) ? "Inctive Successfully." : "Active Successfully."
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                debugger;
                if (response.obj.FPID > 0) {
                    if (response.obj.IsActive) {
                        $('#btnActivityFabricPattern').find("label").text("Inactive");
                        if (!bIsShowConfirmMessage) {
                            InactiveOtherPatterns1(response.obj);
                        }
                    }
                    else {
                        $('#btnActivityFabricPattern').find("label").text("Active");
                    }
                }
            }
        });

    });

    function InactiveOtherPatterns1(oFP) {
        debugger;
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

    
    $("#btnPrintFP").click(function () {
        var oFabricPattern = $('#tblFabricPatterns').datagrid('getSelected');
        if (oFabricPattern == null || oFabricPattern.FPID <= 0) { alert("Please select an item from list!"); return; }
        var nts = ((new Date()).getTime()) / 1000;
        window.open(_sBaseAddress + '/Fabric/PrintFabricPattern?nFPID=' + oFabricPattern.FPID + "&bIsDeisplyPattern=" + true + "&nBUID=" + _nBUID + "&nts=" + nts, "_blank");
        window.open(_sBaseAddress + '/Fabric/DeskLoomSample?nFPID=' + oFabricPattern.FPID + "&bIsDeisplyPattern=" + false + "&nBUID=" + _nBUID + "&nts=" + nts, "_blank");
    });

    $("#btnSaveByLabdip").click(function () {
        debugger;
        endEditingFabricPatternWeft();
        endEditingFabricPatternWarp();
        var oFabricPattern = {
            FPID:  0,
            FabricID: _oFabric.FabricID,
            FSCDetailID: _nFSCDetailID,
            Weave: '',
            Reed: 0,
            Pick: 0,
            Dent: 0,
            GSM: 0,
            Warp: 0,
            Weft:0,
            Note:"",
            Ratio:'',
            RepeatSize: ''
        };

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabricPattern,
            ObjectId: oFabricPattern.FPID,
            ControllerName: "Fabric",
            ActionName: "SaveByLabdip",
            TableId: "tblFabricPatterns",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {

                if (response.obj.FPID > 0) {
                    debugger;
                    InactiveOtherPatternsTwo(response.obj);
                }
            }
        });
    });

    $("#btnSaveSequence").click(function () {
        debugger;
        endEditingFabricPatternWeft();
        endEditingFabricPatternWarp();
        if (_oFabricPattern.FPID <= 0)
        {
            alert("Data Note found");
            return;
        }
        var oFPDWarp = [];
        var oFPDWeft = [];
        var oFPDWarp = $("#tblFabricPatternWarp").datagrid("getRows");
        var oFPDWeft = $("#tblFabricPatternWeft").datagrid("getRows");
        for (var i = 0; i < oFPDWeft.length; i++) {
            oFPDWarp.push(oFPDWeft[i]);
        }
        if (oFPDWarp.length > 0) {
            for (var i = 0; i < oFPDWarp.length; i++) {
                oFPDWarp[i].SLNo = i + 1;
            }
        }
        var oFabricPattern = {
            FPID: _oFabricPattern.FPID,
            FabricID: _oFabric.FabricID,
            FSCDetailID: _nFSCDetailID,
            PatternNo: _oFabricPattern.PatternNo,
            Status: _oFabricPattern.Status,
            ApproveByName: _oFabricPattern.ApproveByName,
            Weave: '',
            Reed: 0,
            Pick: 0,
            Dent: 0,
            GSM: 0,
            Warp: _oFabricPattern.Warp,
            Weft: _oFabricPattern.Weft,
            Note: "",
            Ratio: '',
            FabricPatternDetails: oFPDWarp,
            RepeatSize: ''
        };

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabricPattern,
            ObjectId: oFabricPattern.FPID,
            ControllerName: "Fabric",
            ActionName: "SaveSequence",
            TableId: "tblFabricPatterns",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {

                if (response.obj.FPID > 0) {
                    debugger;
                    InactiveOtherPatterns1(response.obj);
                }
            }
        });
    });
}
function InactiveOtherPatternsTwo(oFP) {

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
function OperationPerforms(rowIndex, rowData) {
    if (rowData != null && rowData.FPID > 0) {
        if (rowData.ApproveBy > 0) {
            $('#btnEditFabricPattern').hide();
            $("#btnDeleteFabricPattern").hide();
            $("#btnApproveFabricPattern").hide();
            //$("#btnActivityFabricPattern").show();

            //if (rowData.IsActive) {
            //    $('#btnActivityFabricPattern').find("label").text("Inactive");
            //}
            //else {
            //    $('#btnActivityFabricPattern').find("label").text("Active");
            //}
        }
        else {
            $('#btnEditFabricPattern').show();
            $("#btnDeleteFabricPattern").show();
            $("#btnApproveFabricPattern").show();
            //$("#btnActivityFabricPattern").hide();
            $('#btnActivityFabricPattern').find("label").text("Active");
        }

        if (rowData.IsActive) {
            $('#btnActivityFabricPattern').find("label").text("Inactive");
        }
        else {
            $('#btnActivityFabricPattern').find("label").text("Active");
        }
    }
}
function RefreshFabricLayout(buttonId) {
    if (buttonId === "btnViewFabric") {
        $("#winFabricPattern input").prop("disabled", true);
        $("#winFabricPattern select").prop("disabled", true);
        
        $("#toolbarFabricPatternWarp, #toolbarFabricPatternWeft").hide();
        $("#btnSaveFabricPattern").hide();
        $('#btnPrintFabricPattern').show();
        $("#txtFPRepeatWarp,#txtFPRepeatWeft").prop("disabled", true);
        $("#btnReceiveFabricInFabric,#btnSubmitFabricInFabric,#btnSaveFabric").hide();

        $("#btnAddFPWarp,#btnRemoveFPWarp,#btnRepeatFPWarp,#btnAddFPWeft,#btnRemoveFPWeft,#btnRepeatFPWeft").hide();

    }
    else {
        $("#winFabricPattern input").not("#txtPatternNo,#txtFabricId,#txtConstruction,#txtBuyer").prop("disabled", false);
        $("#winFabricPattern select").prop("disabled", false);
       
        $("#toolbarFabricPatternWarp, #toolbarFabricPatternWeft").show();
        $('#btnSaveFabricPattern').show();
        $('#btnPrintFabricPattern').show();
        $("#txtFPRepeatWarp,#txtFPRepeatWeft").prop("disabled", false);
        $("#btnAddFPWarp,#btnRemoveFPWarp,#btnRepeatFPWarp,#btnAddFPWeft,#btnRemoveFPWeft,#btnRepeatFPWeft").show();
    }
    $(".disabled input").prop("disabled", true);
    $("#txtFPRepeatWarp,#txtFPRepeatWeft").val("");
    $("#txtFPRepeatWarp,#txtFPRepeatWeft").removeClass("errorFieldBorder");
    $("#lblTotalEndsWarp,#lblTotalEndsWeft").text(0);
}
function SetFabricInfo()
{
    $("#lblFabricId").html(_oFabric.FabricNo);
    $("#lblConstruction").html(_oFabric.Construction);
    $("#lblBuyer").html(_oFabric.BuyerName);
    $("#lblFabricId, #lblConstruction, #lblBuyer").css("margin-left", "5px");
    $('#tblFabricPatterns').find('.panel-title').html("asd");
}
function GetFabricPattern(oLabDipOrder) {

    debugger;
   var obj =
   {
       BaseAddress: _sBaseAddress,
       Object: oLabDipOrder,
       ControllerName: "Fabric",
       ActionName: "GetFP",
       IsWinClose: false
   };

    $.icsDataGet(obj, function (response) {
        
        if (response.status && response.obj != null) {
            if (response.obj.FPID > 0) {
                RefreshControlFabricPattern(response.obj);
                var oFPDetails = response.obj.FPDetails;
                var oFPDetailsWeft = response.obj.FabricPatternDetails;
                if (oFPDetails.length > 0 && oFPDetails[0].FPDID > 0) {
                    _oCellRowSpans = oFPDetails[0].CellRowSpans;
                    _oCellRowSpansWeft = oFPDetails[0].CellRowSpansWeft;
                }
                else {
                    _oCellRowSpans = [];
                    _oCellRowSpansWeft = [];
                   
                }
                if (oFPDetailsWeft.length > 0 && oFPDetailsWeft[0].FPDID > 0) {
                  
                    _oCellRowSpansWeft = oFPDetailsWeft[0].CellRowSpansWeft;
                }
                else {
                 
                    _oCellRowSpansWeft = [];

                }
                if (oFPDetails.length > 0) {

                    var oFPDetailsWarp = [];
                    oFPDetailsWarp = oFPDetails;
                    //for (var i = 0; i < oFPDetails.length; i++) {
                    //    if (oFPDetails[i].IsWarp) { oFPDetailsWarp.push(oFPDetails[i]); }
                    //    else { oFPDetailsWeft.push(oFPDetails[i]); }
                    //}
                    DynamicRefreshListForMultipleSelection(oFPDetailsWarp, "tblFabricPatternWarp");
                    DynamicRefreshListForMultipleSelection(oFPDetailsWeft, "tblFabricPatternWeft");
                    CalculateTotalWarpOrWeft(oFPDetailsWarp, "Warp");
                    CalculateTotalWarpOrWeft(oFPDetailsWeft, "Weft");
                }
            }
            else { alert(response.obj.ErrorMessage); }
        }
        else { alert("No information found."); }
    });
}
function CalculateTotalWarpOrWeft(oFPDetails, sWarpOrWeft) {
    var nTotaEnds = 0;
    var nTwistedGroup = 0;
    for (var i = 0; i < oFPDetails.length; i++) {
        if (oFPDetails[i].TwistedGroup > 0)
        {
            if (nTwistedGroup != oFPDetails[i].TwistedGroup) {
                nTotaEnds += oFPDetails[i].EndsCount;
            }
        }
        else {
            nTotaEnds += oFPDetails[i].EndsCount;
        }
        nTwistedGroup = oFPDetails[i].TwistedGroup;
    }
    if (isNaN(nTotaEnds)) {
        nTotaEnds = 0;
    }
    if (sWarpOrWeft == "Warp") {
        $("#lblTotalEndsWarp").text(nTotaEnds);
    }
    else if (sWarpOrWeft == "Weft") {
        $("#lblTotalEndsWeft").text(nTotaEnds);
    }
}

