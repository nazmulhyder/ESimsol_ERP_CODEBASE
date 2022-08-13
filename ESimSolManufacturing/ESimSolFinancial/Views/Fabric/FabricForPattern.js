var _oLabDipOrder = null;
var _oLabDipOrderDetail = null;
var _oLabDipOrders = [];
var _nCheckReceiveOrSubmitSave = 0;

function InitializeFabricForPattern() {

    LoadComboboxsFFP();

    $('#btnFabricPattern').click(function (e) {
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }
        if (oFabric.ReceiveDateSt == "-") {
            alert("Sorry, Without receive this fabric's pattern cannot create.");
            return false;
        }
        window.location.href = _sBaseAddress + '/Fabric/ViewFabricPatterns?menuid=' + _nmenuid + "&nFabricID=" + oFabric.FabricID;
    });

    $('#btnRequestForLabdip').click(function (e) {
        //ResetControll();
        _oFabric = $("#tblFabrics").datagrid("getSelected");
        if (_oFabric == null || _oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }
        var nSelectedRowIndex = $('#tblFabrics').datagrid('getRowIndex', _oFabric);
        //sessionStorage.clear();
        sessionStorage.setItem("Operation", "New");
        sessionStorage.setItem("SelectedRowIndex", nSelectedRowIndex);
        sessionStorage.setItem("FabricHeader", "Labdip");
        sessionStorage.setItem("Fabrics", JSON.stringify($('#tblFabrics').datagrid('getRows')));
        sessionStorage.setItem("BackLink", window.location.href);
        window.location.href = _sBaseAddress + "/LabDip/ViewLabdipFromFabric?nId=" + _oFabric.FabricID + "&buid=" + _nBUID;

      
    });

    $('#btnPrintDeskLoom').click(function (e) {
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }

        var nts = ((new Date()).getTime()) / 1000;
        window.open(_sBaseAddress + '/Fabric/PrintDeskLoom?nFabricID=' + oFabric.FabricID + "&nts=" + nts, "_blank");
    });

    $("#btnEditFabricInFabricPattern").click(function () {
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }
        _nSelectedRowIndex = $('#tblFabrics').datagrid('getRowIndex', oFabric);

        $("#winFabric").icsWindow('open', "Edit Fabric");
        _oFabric = oFabric;
        
        FieldHideShowControlFP("btnEditFabricInFabricPattern");
        GetFabricInformation(oFabric);
        UnselectAllRowsOfATable();
       
        $("#cboProductFabric").focus();
        
    });

    $("#btnReceiveFabricInFabricPattern").click(function () {
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }
        if (oFabric.ReceiveDateSt != "-") {
            alert("Already Received.");
            return false;
        }
        if (oFabric.ApprovedBy == 0) {
            alert("Sorry, Only approved fabric can receive.");
            return false;
        }

        $("#winFabric").icsWindow('open', "Fabric Request");
        _oFabric = oFabric;
        FieldHideShowControlFP("btnReceiveFabricInFabricPattern");
        GetForReceiveDate(oFabric);
        _nCheckReceiveOrSubmitSave = 1; //1 = Receive Date
        UnselectAllRowsOfATable();
       
    });

    $("#btnReceiveFabricInFabric").click(function () {
        if (!ValidateInputFabric()) return;

        var dIssueDate = $('#txtDateFabric').datebox('getValue');
        var dReceiveDate = $('#txtReceiveDateFabric').datebox('getValue');

        if (new Date(dReceiveDate) < new Date(dIssueDate)) {
            alert("Receive Date must greater than Issue Date.");
            return false;
        }
        var oFb = RefreshObjectFabric();

        if ($.trim($('#txtHLNo').val()) == '')
        {
            alert("Hand loom no required.");
            return false;
        }
        var oFabric = {
            FabricID: oFb.FabricID,
            ReceiveDate: $('#txtReceiveDateFabric').datebox('getValue'),
            HandLoomNo: $.trim($('#txtHLNo').val())
        };
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabric,
            ObjectId: oFabric.FabricID,
            ControllerName: "Fabric",
            ActionName: "SaveReceiveDate",
            TableId: "tblFabrics",
            IsWinClose: true
        };
        $.icsSave(obj);
    });

    $("#btnSubmitFabricInFabricPattern").click(function () {
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }

        if (oFabric.SubmissionDateSt != "-") {
            alert("Already Submited.");
            return false;
        }

        if (oFabric.ApprovedBy == 0) {
            alert("Sorry, Only approved fabric can receive.");
            return false;
        }
        if (oFabric.ReceiveDateSt == "-") {
            alert("Sorry, Only Received fabric can Submit.");
            return false;
        }

        $("#winFabric").icsWindow('open', "Fabric Request");
        _oFabric = oFabric;
        FieldHideShowControlFP("btnSubmitFabricInFabricPattern");
        _nCheckReceiveOrSubmitSave = 2; //2 = Submit Date
        GetForSubmitDate(oFabric);
        UnselectAllRowsOfATable();
    });

    $("#btnSubmitFabricInFabric").click(function () {
        if (!ValidateInputFabric()) return;

        var dIssueDate = $('#txtDateFabric').datebox('getValue');
        var dSubDate = $('#txtSubDateFabric').datebox('getValue');

        if (new Date(dSubDate) < new Date(dIssueDate)) {
            alert("Submission Date must greater than Issue Date.");
            return false;
        }

        var oFb = RefreshObjectFabric();
        var oFabric = {
            FabricID: oFb.FabricID,
            SubmissionDate: $('#txtSubDateFabric').datebox('getValue')
        };
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFabric,
            ObjectId: oFabric.FabricID,
            ControllerName: "Fabric",
            ActionName: "SaveSubmissionDate",
            TableId: "tblFabrics",
            IsWinClose: true
        };
        $.icsSave(obj);
    });

    $("#txtSearchByBuyerNameFP").keydown(function (e) {
        if (e.keyCode === 13) {
            var oFabric = {
                BuyerName: $.trim($("#txtSearchByBuyerNameFP").val())
            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/Fabric/SearchByBuyerNameOnlyApprovedFabric",
                traditional: true,
                data: JSON.stringify(oFabric),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var oFabrics = jQuery.parseJSON(data);
                    if (oFabrics != null) {
                        if (oFabrics.length > 0) {
                            DynamicRefreshList(oFabrics, "tblFabrics");
                        }
                        else {
                            alert("No data found.");
                        }
                    }
                    else {
                        alert("No data found.");
                    }
                },
            });
        }
    });

    if (_nFabricID > 0) {
        for (var i = 0; i < _oFabrics.length; i++) {
            if (_oFabrics[i].FabricID == _nFabricID) {
                $('#tblFabrics').datagrid('selectRow', i);
                break;
            }
        }
    }
}

function LoadComboboxsFFP()
{
    debugger;
    $("#cboMKTPersonLDO").icsLoadCombo({
        List: _oMktPersons,
        OptionValue: "EmployeeID",
        DisplayText: "Name"
    });

}

function GetForSubmitDate(oFabric) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oFabric,
        ControllerName: "Fabric",
        ActionName: "GetForSubmitDate",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.FabricID > 0) {
                RefreshFabricControl(response.obj);
            }
            else {
                alert(response.obj.ErrorMessage);
            }
        }
        else { alert("No information found."); }
    });
}

function GetForReceiveDate(oFabric) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oFabric,
        ControllerName: "Fabric",
        ActionName: "GetForReceiveDate",
        IsWinClose: false
    };

    $.icsDataGet(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj.FabricID > 0) {
                RefreshFabricControl(response.obj);
            }
            else {
                alert(response.obj.ErrorMessage);
            }
        }
        else { alert("No information found."); }
    });
}

//function FabricPikerDisableEnable() {
//    $("#txtFabricWidthFabric,#cboFinishTypeFabric,#cboPriorityLevelFabric,#cboMktPersonsFabric,#txtColorFabric,#txtBuyerReferenceFabric").prop("disabled", true);
//    $("#cboProcessTypeFabric,#cboWeaveFabric").prop("disabled", true);
//    $("#txtSearchByBuyerNameFabric,#btnClrBuyerFabric,#btnPickBuyerFabric").prop("disabled", true);
//    $("#txtDateFabric,#txtSeekingDateFabric").datebox({ disabled: true });
//}

function FieldHideShowControlFP(sButtonId) {
    debugger;
    $('#txtContractorName').css({ 'width': '98%' });
    $('#btnContractor, #btnResetContractor').hide();

    if (sButtonId == "btnEditFabricInFabricPattern") {
        $("#trFabricHideShowFields").hide();
        $('#winFabric input,select,textarea').prop("disabled", true);
        $('#txtHLNo').prop("disabled", true);
        $(" #cboProductFabric,#txtConstructionFabric ,#txtConstructionFabricPI,#cboFabricDesign,#txtStyleNoFabric,#txtNoteFabric,#cboPrimaryLightSource ,#cboSecondaryLightSource,#txtEndUse").prop("disabled", false);

        $("#btnSaveFabric").show();
        $("#lblGiveInfo,#btnReceiveFabricInFabric,#btnSubmitFabricInFabric").hide();
    }
    else if (sButtonId == "btnReceiveFabricInFabricPattern") {
        $("#trFabricHideShowFields").show();
        $("#divSubDateHideShow").hide();
        $('#winFabric input,select,textarea').prop("disabled", true);
        $('#txtHLNo').prop("disabled", false);
       
        $("#txtReceiveDateFabric").datebox({ disabled: false });
        $("#txtReceiveDateFabric").datebox("setValue", icsdateformat(new Date()));
       
        $("#btnReceiveFabricInFabric").show();
        $("#lblGiveInfo,#btnSaveFabric,#btnSubmitFabricInFabric").hide();
    }
    else if (sButtonId == "btnSubmitFabricInFabricPattern") {
        $("#trFabricHideShowFields").show();
        $("#divSubDateHideShow").show();
        $('#winFabric input,select,textarea').prop("disabled", true);
        $('#txtHLNo').prop("disabled", true);


        $("#txtSubDateFabric").datebox({ disabled: false });
        $("#txtSubDateFabric").datebox("setValue", icsdateformat(new Date()));
        
        $('#btnSubmitFabricInFabric').show();
        $("#btnSaveFabric,#btnReceiveFabricInFabric,#lblGiveInfo,#btnContractor, #btnResetContractor").hide();
    }
}