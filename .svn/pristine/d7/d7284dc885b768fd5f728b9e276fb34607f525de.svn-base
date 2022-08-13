var _oCompareOperators = [];
var _oApprovedByUsers = [];
var _oReceivedByUsers = [];
var _oExecutionStatuses = [];

function InitializeAdvSearchSUPEEvents() {
    $(".lblLoadingMessage").hide();
    $('#tblSUPEAdvSearch').datagrid({ selectOnCheck: false, checkOnSelect: true });
    $("#cboSearchByExecutionDateAdvSearch").icsLoadCombo({
        List: _oCompareOperators,
        OptionValue: "Value",
        DisplayText: "Text"
    });
    $("#btnCloseAdvSearch").click(function() {
        $("#winAdvSearchSUPE").icsWindow("close");
    });

    $("#btnResetAdvSearch").click(function () {
        ResetAdvSearchWindow();
        DateActionsExecutionDateAdvSearch();
    });

    $("#btnSearchAdvSearch").click(function () {
        var checkDate = CheckFromAndToDateValidation("cboSearchByExecutionDateAdvSearch", "txtFromExecutionDateAdvSearch", "txtToExecutionDateAdvSearch");
        if (!checkDate) {
            alert("To Date must be greater then From Date");
            return false;
        }
        

        var oSUPE = { ErrorMessage: "" }
        oSUPE.ErrorMessage = RefreshAdvSearchArguments();

        $(".lblLoadingMessage").show();

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SUProductionExecution/Gets",
            traditional: true,
            data: JSON.stringify(oSUPE),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSUPEs = jQuery.parseJSON(data);
                $(".lblLoadingMessage").hide();
                if (oSUPEs != null) {
                    if (oSUPEs.length > 0) {
                        DynamicRefreshList(oSUPEs, "tblSUPEAdvSearch");
                    } else {
                        alert("Sorry, No data found.");
                        DynamicRefreshList([], "tblSUPEAdvSearch");
                    }

                } else {
                    alert("Sorry, No data found.");
                    DynamicRefreshList([], "tblSUPEAdvSearch");
                }

            }
        });
    });

    $("#btnOkAdvSearch").click(function() {
        var oSUPEs = $("#tblSUPEAdvSearch").datagrid('getChecked');
        if (oSUPEs.length > 0) {
            $("#winAdvSearchSUPE").icsWindow("close");
            DynamicRefreshList(oSUPEs, "tblSUPEs");
            _oDBSUPEs = oSUPEs;
        }
        else {
            alert("Please select minimum one item from list.");
            return false;
        }
    });

    DateActionsExecutionDateAdvSearch();
    RefreshcboSearchByExecutionStatusAdvSearch(_oExecutionStatuses);
    RefreshcboSearchByReceivedByNameAdvSearch(_oReceivedByUsers);
    RefreshcboSearchByApprovedByNameAdvSearch(_oApprovedByUsers);
    RefreshcboSearchByReceivedStoreAdvSearch(_oReceivedStores);

}
function RefreshAdvSearchArguments() {
    var arg = 'Arguments;';
    var sExecutionNo = $.trim($("#txtSearchByExecutionNoAdvSearch").val());
    if (sExecutionNo != null) {
        arg = arg + sExecutionNo + '~';
    }

    var nExecutionStatus = $.trim($("#cboSearchByExecutionStatusAdvSearch").val());
    if (nExecutionStatus != null) {
        arg = arg + nExecutionStatus + '~';
    }
    
    var nReceivedBy = $.trim($("#cboSearchByReceivedByNameAdvSearch").val());
    if (nReceivedBy != null) {
        arg = arg + nReceivedBy + '~';
    }
    var nApprovedBy = $.trim($("#cboSearchByApprovedByNameAdvSearch").val());
    if (nApprovedBy != null) {
        arg = arg + nApprovedBy + '~';
    }
    var nReceivedStoreID = $.trim($("#cboSearchByReceivedStoreAdvSearch").val());
    if (nReceivedStoreID != null) {
        arg = arg + nReceivedStoreID + '~';
    }
    var nExecutionDate = $.trim($("#cboSearchByExecutionDateAdvSearch").val());
    if (nExecutionDate != null) {
        arg = arg + nExecutionDate + '~';
    }
    var sFromExecutionDate = $.trim($("#txtFromExecutionDateAdvSearch").datebox('getValue'));
    if (sFromExecutionDate != null) {
        arg = arg + sFromExecutionDate + '~';
    }
    var sToExecutionDate = $.trim($("#txtToExecutionDateAdvSearch").datebox('getValue'));
    if (sToExecutionDate != null) {
        arg = arg + sToExecutionDate + '~';
    }
    return arg;
}

function RefreshcboSearchByExecutionStatusAdvSearch(oExecutionStatuses) {
    $('#cboSearchByExecutionStatusAdvSearch').empty();
    var listItems;
    for (var i = 0; i < oExecutionStatuses.length; i++) {
        listItems += '<option value="' + oExecutionStatuses[i].id + '">' + oExecutionStatuses[i].Value + '</option>';
    }
    $('#cboSearchByExecutionStatusAdvSearch').html(listItems);
    $('#cboSearchByExecutionStatusAdvSearch').val(0);
}

function RefreshcboSearchByReceivedByNameAdvSearch(oReceivedByUsers) {
    $('#cboSearchByReceivedByNameAdvSearch').empty();
    var listItems;
    listItems += '<option value="' + 0 + '">--Select User--</option>';
    for (var i = 0; i < oReceivedByUsers.length; i++) {
        listItems += '<option value="' + oReceivedByUsers[i].UserID + '">' + oReceivedByUsers[i].UserName + '</option>';
    }
    $('#cboSearchByReceivedByNameAdvSearch').html(listItems);
    $('#cboSearchByReceivedByNameAdvSearch').val(0);
}

function RefreshcboSearchByApprovedByNameAdvSearch(oApprovedByUsers) {
    $('#cboSearchByApprovedByNameAdvSearch').empty();
    var listItems;
    listItems += '<option value="' + 0 + '">--Select User--</option>';
    for (var i = 0; i < oApprovedByUsers.length; i++) {
        listItems += '<option value="' + oApprovedByUsers[i].UserID + '">' + oApprovedByUsers[i].UserName + '</option>';
    }
    $('#cboSearchByApprovedByNameAdvSearch').html(listItems);
    $('#cboSearchByApprovedByNameAdvSearch').val(0);
}

function RefreshcboSearchByReceivedStoreAdvSearch(oReceiveStores) {
    $('#cboSearchByReceivedStoreAdvSearch').empty();
    var listItems;
    listItems += '<option value="' + 0 + '">--Select Store--</option>';
    for (var i = 0; i < oReceiveStores.length; i++) {
        listItems += '<option value="' + oReceiveStores[i].WorkingUnitID + '">' + oReceiveStores[i].OperationUnitName + '</option>';
    }
    $('#cboSearchByReceivedStoreAdvSearch').html(listItems);
    $('#cboSearchByReceivedStoreAdvSearch').val(_oSUPE.WorkingUnitID);
}


function CheckFromAndToDateValidation(OperationComboId, FromDateId, ToDateId) {
    $("#" + OperationComboId).parent().parent().parent().find("select").removeClass("errorFieldBorder");
    var nCboVal = $("#" + OperationComboId).val();
    if (parseInt(nCboVal) == 5 || parseInt(nCboVal) == 6) {
        var fromDate = $("#" + FromDateId).datebox("getValue");
        var toDate = $("#" + ToDateId).datebox("getValue");
        if (fromDate > toDate) {
            $("#" + ToDateId).focus();
            $("#" + OperationComboId).addClass("errorFieldBorder");
            return false;
        } else {
            $("#" + OperationComboId).removeClass("errorFieldBorder");
            return true;
        }
    } else {
        return true;
    }
}

function ResetAdvSearchWindow() {
    $("#winAdvSearchSUPE input").not("input[type='button']").val("");
    $("#winAdvSearchSUPE input").removeClass("fontColorOfPickItem");
    $("#winAdvSearchSUPE select").val(0);
    DateActionsExecutionDateAdvSearch();
    $("#txtFromExecutionDateAdvSearch,#txtToExecutionDateAdvSearch").datebox({ disabled: false });
    $("#txtFromExecutionDateAdvSearch,#txtToExecutionDateAdvSearch").datebox("setValue", icsdateformat(new Date()));
    DynamicRefreshList([], "tblSUPEAdvSearch");
}

function DateActionsExecutionDateAdvSearch() {
    DynamicDateActions("cboSearchByExecutionDateAdvSearch", "txtFromExecutionDateAdvSearch", "txtToExecutionDateAdvSearch");
}



