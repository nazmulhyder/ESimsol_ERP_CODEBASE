var _bIsAddLeaveHead = false;
var _oLeaveHead = { LeaveHeadID: 0 };

function InitializeLeaveHeadsEvents() {
    
    DynamicRefreshList(_oLeaveHeads, "tblLeaveHeads");
    $("#btnNewLeaveHead").click(function () {
        debugger
        $("#lblHeaderName").html("New Leave Head");
        //$("#txtDescription").val("");
        $("#winLeaveHead").icsWindow("open", "New Leave Head");
        EnableFields();
        ResetAllFields("winLeaveHead");
        _oLeaveHead.LeaveHeadID = 0;
        _bIsAddLeaveHead = true;
    });

    $("#btnClose").click(function () {
        $("#winLeaveHead").icsWindow("close");
    });

    $("#btnSave").click(function () {
        debugger
        if (!ValidateInput()) return;
        var oLeaveHead = RefreshObject();
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oLeaveHead,
            ObjectId: oLeaveHead.LeaveHeadID,
            ControllerName: "LeaveHead",
            ActionName: "Save",
            TableId: "",
            IsWinClose: false
        };
        oLeaveHead.LHRules = $('#tblLHRules').datagrid('getRows');

        $.icsSave(obj, function (response) {
            if (response.status && response.obj.LeaveHeadID > 0) {

                if (_bIsAddLeaveHead) {
                    var oLeaveHeads = $('#tblLeaveHeads').datagrid('getRows');
                    var nIndex = oLeaveHeads.length;
                    $('#tblLeaveHeads').datagrid('appendRow', response.obj);
                    $('#tblLeaveHeads').datagrid('selectRow', nIndex);
                }
                else {
                    var oLeaveHead = $('#tblLeaveHeads').datagrid('getSelected');
                    var SelectedRowIndex = $('#tblLeaveHeads').datagrid('getRowIndex', oLeaveHead);
                    $('#tblLeaveHeads').datagrid('updateRow', { index: SelectedRowIndex, row: response.obj });
                }
                $("#winLeaveHead").icsWindow("close");
            }
        });

        ResetAllFields("winLeaveHead");
    });

    $("#btnEdit").click(function () {
        debugger
        _bIsAddLeaveHead = false;
        $("#lblHeaderName").text("Edit Leave Head");
        var oLeaveHead = $('#tblLeaveHeads').datagrid('getSelected');
        if (oLeaveHead == null || oLeaveHead.LeaveHeadID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oLeaveHead,
            ControllerName: "LeaveHead",
            ActionName: "getLeaveHead",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj.LeaveHeadID > 0) {
                _oLeaveHead = response.obj;
                debugger
                //alert(response.obj.LeaveHeadID);
                $("#txtName").val(_oLeaveHead.Name);
                $("#txtNameInBangla").val(_oLeaveHead.NameInBangla);
                $("#txtShortName").val(_oLeaveHead.ShortName);
                $('#chkLWP').attr('checked', _oLeaveHead.IsLWP);
                $("#txtCode").val(_oLeaveHead.Code);
                $("#txtDescription").val(_oLeaveHead.Description);
                $("#txtTotalDay").val(_oLeaveHead.TotalDay);
                document.getElementById("cboRequiredFor").selectedIndex = _oLeaveHead.RequiredFor;
                $("#winLeaveHead").icsWindow("open", "Edit Leave Head");
                DynamicRefreshList(_oLeaveHead.LHRules, "tblLHRules");
            }
        });
        
        EnableFields();
    });

    $("#btnView").click(function () {
        debugger
        _bIsAddLeaveHead = false;
        $("#lblHeaderName").text("View Leave Head");
        var oLeaveHead = $('#tblLeaveHeads').datagrid('getSelected');
        if (oLeaveHead == null || oLeaveHead.LeaveHeadID <= 0) {
            alert("Please select a item from list!");
            return;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oLeaveHead,
            ControllerName: "LeaveHead",
            ActionName: "getLeaveHead",
            IsWinClose: false
        };

        $.icsDataGet(obj, function (response) {
            if (response.status && response.obj.LeaveHeadID > 0) {
                _oLeaveHead = response.obj;
                $("#txtName").val(_oLeaveHead.Name);
                $("#txtNameInBangla").val(_oLeaveHead.NameInBangla);
                $("#txtShortName").val(_oLeaveHead.ShortName);
                $('#chkLWP').attr('checked', _oLeaveHead.IsLWP);
                $("#txtCode").val(_oLeaveHead.Code);
                $("#txtDescription").val(_oLeaveHead.Description);
                $("#txtTotalDay").val(_oLeaveHead.TotalDay);
                document.getElementById("cboRequiredFor").selectedIndex = _oLeaveHead.RequiredFor;

                $("#txtName").prop("disabled", true);
                $("#txtNameInBangla").prop("disabled", true);
                $("#txtShortName").prop("disabled", true);
                $("#txtCode").prop("disabled", true);
                $("#txtDescription").prop("disabled", true);
                $("#txtTotalDay").prop("disabled", true);
                $("#cboRequiredFor").prop("disabled", true);
                $("#chkLWP").prop("disabled", true);
                $("#btnSave").hide();
            }
        });
        $("#winLeaveHead").icsWindow("open", "View Leave Head");
    });

    $("#btnDelete").click(function () {
        debugger
        var oHRR = $('#tblLeaveHeads').datagrid('getSelected');
        if (oHRR == null || oHRR.LeaveHeadID <= 0) {
            alert("Please select a item from list!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndex = $('#tblLeaveHeads').datagrid('getRowIndex', oHRR);

        if (oHRR.LeaveHeadID > 0) {
            var obj = {
                BaseAddress: _sBaseAddress,
                Object: oHRR,
                ControllerName: "LeaveHead",
                ActionName: "Delete",
                TableId: "tblLeaveHeads",
                IsWinClose: false
            };
            $.icsDelete(obj);
        }
    });

    $("#btnActivity").click(function () {
        var oLeaveHead = $('#tblLeaveHeads').datagrid('getSelected');
        if (oLeaveHead == null || oLeaveHead.LeaveHeadID <= 0) {
            alert("Please select a item from list!");
            return false;
        }
        if (oLeaveHead.IsActive == true) {
            if (!confirm("Confirm to In-Active?")) return;
            oLeaveHead.IsActive = false;
        }
        else {
            if (!confirm("Confirm to Active?")) return;
            oLeaveHead.IsActive = true;
        }
        var SelectedRowIndex = $('#tblLeaveHeads').datagrid('getRowIndex', oLeaveHead);
        if (oLeaveHead.LeaveHeadID > 0) {
            debugger;
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/LeaveHead/ChangeActiveStatus",
                traditional: true,
                data: JSON.stringify(oLeaveHead),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var oLeaveHead = jQuery.parseJSON(data);
                    debugger;
                    if (oLeaveHead != null) {
                        if (oLeaveHead.LeaveHeadID > 0) {
                            alert("Data Save Succesfully!!");
                            $('#tblLeaveHeads').datagrid('updateRow', { index: SelectedRowIndex, row: oLeaveHead });
                        }
                        else {
                            alert(oLeaveHead.ErrorMessage);
                        }
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
    });



    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------- Start Leave Module ---------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    $("#btnAddLHRule").click(function () {
        debugger;
        _oLHRule = [];
        DynamicRefreshList([], "tblLeaveHeadRuleDetails");

        
        $("#winLeaveHeadRule").icsWindow("open", "Add Leave Head Rule");
        sessionStorage.setItem("RuleOperation", "AddRule");

        ResetLHRuleWindow();
    });
    $("#btnEditLHRule").click(function () {
        debugger
        var oLHRule = $('#tblLHRules').datagrid('getSelected');
        if (oLHRule == null) {
            alert("Please select a item from list!");
            return;
        }
        _oLHRule = oLHRule;
        sessionStorage.setItem("RuleOperation", "EditRule");
        var SelectedRowIndexLHRule = $('#tblLHRules').datagrid('getRowIndex', oLHRule);
        sessionStorage.setItem("SelectedRowIndexLHRule", SelectedRowIndexLHRule);

        $("#winLeaveHeadRule").icsWindow("open", "Edit Leave Head Rule");
        RefreshLHRuleWindow();
    });
    $("#btnDeleteLHRule").click(function () {
        var oLHRule = $('#tblLHRules').datagrid('getSelected');
        if (oLHRule == null) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedDeleteRowIndex = $('#tblLHRules').datagrid('getRowIndex', oLHRule);
        $('#tblLHRules').datagrid('deleteRow', SelectedDeleteRowIndex);
        alert("Delete sucessfully");
    });
    $("#btnDeleteLHRuleDetail").click(function () {
        var oLHRuleDetail = $('#tblLeaveHeadRuleDetails').datagrid('getSelected');
        if (oLHRuleDetail == null) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return;
        var SelectedRowIndexLHRuleDetail = $('#tblLeaveHeadRuleDetails').datagrid('getRowIndex', oLHRuleDetail);
        $('#tblLeaveHeadRuleDetails').datagrid('deleteRow', SelectedRowIndexLHRuleDetail);
        alert("Delete sucessfully");
        $('#txtLHRuleTypeDescription').val(SetLHRuleText());
        ResetSequence();
    });
    function ResetSequence()
    {
        var oList = $('#tblLeaveHeadRuleDetails').datagrid('getRows');
        for(var i=0; i<oList.length; i++)
        {
            oList[i].Sequence = i + 1;
        }
        DynamicRefreshList(oList, "tblLeaveHeadRuleDetails");
    }
    function RefreshLHRuleWindow()
    {
        $("#cboLHRuleType").icsLoadCombo({ List: _oLHRuleTypes, OptionValue: "id", DisplayText: "Value", });
        $("#cboLHRuleDetailValueType").icsLoadCombo({ List: _oLHRuleValueTypes, OptionValue: "id", DisplayText: "Value", });
        $('#cboLHRuleType').val(_oLHRule.LHRuleTypeInt);
        $('#txtRuleTypeRemarks').val(_oLHRule.Remarks);
        DynamicRefreshList(_oLHRule.LHRuleDetails, "tblLeaveHeadRuleDetails");
        $('#txtLHRuleTypeDescription').val(SetLHRuleText());
    }
    $("#btnCloseLHRuleDetail").click(function () {
        debugger;
        $("#winLeaveHeadRule").icsWindow("close");
    });
    $("#btnAddLHRuleDetail").click(function () {

        if ($('#cboLHRuleDetailValueType').val() == 0)
        {
            alert("Enter The Rule Value Type");
            $('#cboLHRuleDetailValueType').focus();
            return;
        }
        if ($('#txtLHRuleDetailValue').val() == "") {
            alert("Enter The Rule Value");
            $('#txtLHRuleDetailValue').focus();
            return;
        }
        debugger;
        var oLHRuleDetail = {
            LHRuleDetailID: 0,
            LHRuleID: 0,
            LHRuleValueTypeInt: $('#cboLHRuleDetailValueType').val(),
            LHRuleValueTypeSt: $('#cboLHRuleDetailValueType option:selected').text(),
            LHRuleValue: $('#txtLHRuleDetailValue').val(),
            Sequence: $("#tblLeaveHeadRuleDetails").datagrid('getRows').length + 1,
        }
        $('#tblLeaveHeadRuleDetails').datagrid('appendRow', oLHRuleDetail);
        $('#tblLeaveHeadRuleDetails').datagrid('selectRow', $('#tblLeaveHeadRuleDetails').datagrid('getRows').length - 1);
        ResetLHRuleDetailWindow();
        $('#txtLHRuleTypeDescription').val(SetLHRuleText());
    });
    function ResetLHRuleDetailWindow()
    {
        $('#cboLHRuleDetailValueType').val(0);
        $('#txtLHRuleDetailValue').val("");
    }
    function ResetLHRuleWindow() {
        $("#cboLHRuleType").icsLoadCombo({ List: _oLHRuleTypes, OptionValue: "id", DisplayText: "Value", });
        $("#cboLHRuleDetailValueType").icsLoadCombo({ List: _oLHRuleValueTypes, OptionValue: "id", DisplayText: "Value", });
        $('#cboLHRuleType').val(0);
        $('#txtRuleTypeRemarks').val("");
        $('#txtLHRuleTypeDescription').val("");
    }
    $("#btnSaveLHRuleDetail").click(function () {

        debugger;
        if ($('#cboLHRuleType').val() == 0) {
            alert("Enter The Rule Type");
            $('#cboLHRuleType').focus();
            return;
        }
        if ($('#txtLHRuleDescription').val() == "") {
            alert("No Rule Description");
            return;
        }
        var oLHRule = {
            LHRuleID: _oLHRule.LHRuleID,
            LeaveHeadID: _oLHRule.LeaveHeadID,
            LHRuleTypeInt: $('#cboLHRuleType').val(),
            LHRuleTypeSt: $('#cboLHRuleType option:selected').text(),
            Remarks: $('#txtRuleTypeRemarks').val(),
            LHRuleTypeDescription: $('#txtLHRuleTypeDescription').val(),
            LHRuleDetails: $("#tblLeaveHeadRuleDetails").datagrid('getRows')
        }
        if (sessionStorage.getItem("RuleOperation") == "EditRule")
        {
            var nIndex = sessionStorage.getItem("SelectedRowIndexLHRule");
            $('#tblLHRules').datagrid('updateRow', { index: nIndex, row: oLHRule });
        }
        if (sessionStorage.getItem("RuleOperation") == "AddRule") {
            $('#tblLHRules').datagrid('appendRow', oLHRule);
            var oList = $('#tblLHRules').datagrid('getRows');
            $('#tblLHRules').datagrid('selectRow', oList.length-1);
        }
        ResetLHRuleDetailWindow();
        ResetLHRuleWindow();
        $('#btnCloseLHRuleDetail').click();
    });
    //-------------------------------------------------------- END Leave Module ---------------------------------------------------------
}






function ValidateInput() {

    if ($("#txtName").val() == null || $("#txtName").val() == "") {
        alert("Please enter Name!");
        $('#txtName').focus();
        return false;
    }

    if ($("#txtDescription").val() == null || $("#txtDescription").val() == "") {
        alert("Please enter Description!");
        $('#txtDescription').focus();
        return false;
    }

    //if ($("#txtTotalDay").val() == null || $("#txtTotalDay").val() == "") {
    //    alert("Please enter Total Day!");
    //    $('#txtTotalDay').focus();
    //    return false;
    //}
    return true;
}

function RefreshObject() {
    var oLeaveHead = {
        LeaveHeadID: _oLeaveHead.LeaveHeadID,
        IsActive: true,
        Code: $("#txtCode").val(),
        Name: $("#txtName").val(),
        NameInBangla: $("#txtNameInBangla").val(),
        ShortName: $("#txtShortName").val(),
        IsLWP: $('#chkLWP').is(':checked'),
        IsHRApproval: 0,
        Description: $("#txtDescription").val(),
        RequiredFor: cboRequiredFor.options[cboRequiredFor.selectedIndex].value,
        TotalDay: $("#txtTotalDay").val(),
        
    };
    return oLeaveHead;
}

function EnableFields() {
    $("#txtName").prop("disabled", false);
    $("#txtNameInBangla").prop("disabled", false);
    $("#txtShortName").prop("disabled", false);
    $("#chkLWP").prop("disabled", false);
    $("#txtCode").prop("disabled", false);
    $("#txtDescription").prop("disabled", false);
    $("#txtTotalDay").prop("disabled", false);
    $("#cboRequiredFor").prop("disabled", false);
    $("#btnSave").show();
}

function Refresh() {
    DynamicRefreshList(_oLeaveHeads, "tblLeaveHeads");
}

function ResetAllFields(S)
{
    $("#txtCode").val("");
    $("#txtName").val("");
    $("#txtNameInBangla").val("");
    $("#txtShortName").val("");
    $('#chkLWP').attr('checked', false);
    $("#txtDescription").val("");
    $("#txtTotalDay").val("");
}

function SetSequence(n) {
    debugger;
    var oLHRuleDetail = $('#tblLeaveHeadRuleDetails').datagrid('getSelected');
    if (oLHRuleDetail == null) {
        alert("Please select an item from list."); return;
    }

    var nNextIndex = -1;
    var nRowIndex = $("#tblLeaveHeadRuleDetails").datagrid("getRowIndex", oLHRuleDetail);
    var oLHRuleDetails = $('#tblLeaveHeadRuleDetails').datagrid('getRows');

    var oLHRuleDetail_Next = {}

    if (n == 1)
        nNextIndex = nRowIndex - 1;
    else
        nNextIndex = nRowIndex + 1;
    oLHRuleDetail_Next = oLHRuleDetails[nNextIndex];

    if (oLHRuleDetail_Next == null) return;

    var nSequence = oLHRuleDetail.Sequence;
    oLHRuleDetail.Sequence = oLHRuleDetail_Next.Sequence;
    oLHRuleDetail_Next.Sequence = nSequence;

    oLHRuleDetails[nNextIndex] = oLHRuleDetail;
    oLHRuleDetails[nRowIndex] = oLHRuleDetail_Next;
    DynamicRefreshList(oLHRuleDetails, 'tblLeaveHeadRuleDetails');

    $("#tblLeaveHeadRuleDetails").datagrid("selectRow", nNextIndex);
    $('#txtLHRuleTypeDescription').val(SetLHRuleText());
}

function SetLHRuleText()
{
    debugger;
    var sText = "";
    var oList = $("#tblLeaveHeadRuleDetails").datagrid('getRows');
    for(var i=0; i<oList.length; i++)
    {
        sText = sText + " " + oList[i].LHRuleValue;
    }
    return sText;
}



