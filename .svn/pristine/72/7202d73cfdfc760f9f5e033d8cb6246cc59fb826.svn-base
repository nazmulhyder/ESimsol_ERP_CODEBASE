
var _oExportTR = [];
var _nExportTRID = 0;
var _oExportDocTnC = 0;
var _oBankForwardingList = [];
function LoadExportBillEvents() {

    $("#cboEBillParticular").icsLoadCombo({
        List: _oExportBillParticulars,
        OptionValue: "ExportBillParticularID",
        DisplayText: "Name"
    });

    $("#cboBankAccount_EN").icsLoadCombo({
        List: _oBankAccounts,
        OptionValue: "BankAccountID",
        DisplayText: "AccountNameandNo"
    });

    $("#cboCurrency_EncashmentAccount").icsLoadCombo({
        List: _oCurrencys,
        OptionValue: "CurrencyID",
        DisplayText: "CurrencyName"
    });

    if (_oExportBills.length > 0) {
        $('#tblExportBills').datagrid({ onSelect: function (rowIndex, rowData) { RowSelect(rowIndex, rowData); } });
    }

    $("#btnSaveSendToParty").click(function (e) {
        var oExportBill = RefreshObject_SendToParty();

        var oSelectedObject = $("#tblExportBills").datagrid("getSelected");
        var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oSelectedObject);

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveBOEinCustomerHand",
            TableId: "tblExportBills",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.ExportBillID > 0) {
                    RowSelect(nRowIndex, response.obj);
                }
            }
        });
    });

    $("#btnSaveRecdFromParty").click(function (e) {

        var oExportBill = RefreshObject_ReceiveFromParty();

        var oSelectedObject = $("#tblExportBills").datagrid("getSelected");
        var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oSelectedObject);

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveExportBillReceiveFromParty",
            TableId: "tblExportBills",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.ExportBillID > 0) {
                    RowSelect(nRowIndex, response.obj);
                }
            }
        });
    });

    $("#btnSaveSendToBank").click(function (e) {

        var oExportBill = RefreshObject_SendToBank();

        var oSelectedObject = $("#tblExportBills").datagrid("getSelected");
        var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oSelectedObject);

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveExportBillSendToBank",
            TableId: "tblExportBills",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.ExportBillID > 0) {
                    RowSelect(nRowIndex, response.obj);
                }
            }
        });
    });
    $("#btnSaveReceiveFromBank").click(function (e) {

        var oExportBill = RefreshObject_ReceiveFromBank();

        var oSelectedObject = $("#tblExportBills").datagrid("getSelected");
        var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oSelectedObject);

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveExportBillReceiveFromBank",
            TableId: "tblExportBills",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.ExportBillID > 0) {
                    RowSelect(nRowIndex, response.obj);
                }
            }
        });
    });
    $("#btnSaveMaturityReceive").click(function (e) {

        var oExportBill = RefreshObject_MaturityReceive();

        var oSelectedObject = $("#tblExportBills").datagrid("getSelected");
        var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oSelectedObject);

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveMaturityReceived",
            TableId: "tblExportBills",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.ExportBillID > 0) {
                    RowSelect(nRowIndex, response.obj);
                }
            }
        });
    });

    $("#btnSaveBillRelization").click(function (e) {
        var oExportBill = RefreshObject_BillRelization();

        var oSelectedObject = $("#tblExportBills").datagrid("getSelected");
        var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oSelectedObject);

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveBillRealized",
            TableId: "tblExportBills",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.ExportBillID > 0) {
                    RowSelect(nRowIndex, response.obj);
                }
            }
        });
    });

    $("#btnSaveBBankFDDReceive").click(function (e) {
        var oExportBill = RefreshObject_BBankFDDReceive();

        var oSelectedObject = $("#tblExportBills").datagrid("getSelected");
        var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oSelectedObject);

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveBBankFDDReceived",
            TableId: "tblExportBills",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.ExportBillID > 0) {
                    RowSelect(nRowIndex, response.obj);
                }
            }
        });
    });

    $("#btnSaveBillEncashment").click(function (e) {

        var oExportBill = RefreshObject_BillEncashment();

        var oSelectedObject = $("#tblExportBills").datagrid("getSelected");
        var nRowIndex = $("#tblExportBills").datagrid("getRowIndex", oSelectedObject);

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportBill,
            ObjectId: oExportBill.ExportBillID,
            ControllerName: "ExportBill",
            ActionName: "SaveEncashmentRecived",
            TableId: "tblExportBills",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj.ExportBillID > 0) {
                    RowSelect(nRowIndex, response.obj);
                }
            }
        });
    });

    $("#btnSaveExportDocForwarding").click(function (e) {

        // if (!ValidateInputExportBill()) return;
        endEditing();
        var oExportDocForwarding = {
                    ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
                    ExportDocForwardingID:0,
                    ExportDocForwardings:  $('#tblExportDocForwarding').datagrid('getChecked')
                };

        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportDocForwarding,
                ObjectId: oExportDocForwarding.ExportDocForwardingID,
                ControllerName: "ExportDocForwarding",
                ActionName: "SaveExportDocForwarding",
                TableId: "tblExportDocForwarding",
                IsWinClose: true
            };
        $.icsSave(obj);
    });
    $("#btnDeleteExportDocForwarding").click(function () {
        var oExportDocForwarding = $("#tblExportDocForwarding").datagrid("getSelected");
        var SelectedRowIndex = $('#tblExportDocForwarding').datagrid('getRowIndex', oExportDocForwarding);
        if (!confirm("Confirm to Delete?")) return false;
        if (oExportDocForwarding == null || oExportDocForwarding.ExportDocForwardingID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportDocForwarding,
            ControllerName: "ExportDocForwarding",
            ActionName: "DeleteExportDocForwarding",
            TableId: "tblExportDocForwarding",
            IsWinClose: false
        };
        $.icsDelete(obj);
        $('#tblExportDocForwarding').datagrid('deleteRow', SelectedRowIndex);
    });

  

    $("#btnCloseBillEncashment").click(function () {
        $("#winExportBillEncashmen").icsWindow("close");
        $("#winExportBillEncashmen input").val("");
        $("#winExportBillEncashmen select").val(0);
    });
  
    $("#btnCloseSendToParty").click(function () {
        $("#winExportBillSendToParty").icsWindow("close");
        $("#winExportBillSendToParty input").val("");
        $("#winExportBillSendToParty select").val(0);
    });
    $("#btnCloseRecdFromParty").click(function () {
        $("#winExportBillSendToParty").icsWindow("close");
        $("#winExportBillSendToParty input").val("");
        $("#winExportBillSendToParty select").val(0);
    });
    $("#btnCloseSendToBank").click(function () {
        $("#winExportBillSendToParty").icsWindow("close");
        $("#winExportBillSendToParty input").val("");
        $("#winExportBillSendToParty select").val(0);
    });
    $("#btnCloseReceiveFromBank").click(function () {
        $("#winExportBillSendToParty").icsWindow("close");
        $("#winExportBillSendToParty input").val("");
        $("#winExportBillSendToParty select").val(0);
    });
    $("#btnCloseMaturityReceive").click(function () {
        $("#winExportBillSendToParty").icsWindow("close");
        $("#winExportBillSendToParty input").val("");
        $("#winExportBillSendToParty select").val(0);
    });
    $("#btnCloseBillRelization").click(function () {
        $("#winExportBillRelization").icsWindow("close");
        $("#winExportBillRelization input").val("");
        $("#winExportBillRelization select").val(0);
    });
    $("#btnCloseBBankFDDReceive").click(function () {
        $("#winExportBillBBankFDDReceive").icsWindow("close");
        $("#winExportBillBBankFDDReceive input").val("");
        $("#winExportBillBBankFDDReceive select").val(0);
    });
    $("#btnCloseExportDocForwarding").click(function () {
        $("#winExportDocForwarding").icsWindow("close");
        $("#winExportDocForwarding input").val("");
        $("#winExportDocForwarding select").val(0);
        _oBankForwardingList = [];
    });
   

   // Start Product, Terms & Condition picker
    $("#btnAddExportBillParticular").click(function (e) {
       
        

        var sEBillParticular = "";
        var nEBillParticular = $("#cboEBillParticular option:selected").val();
        if (nEBillParticular > 0) {
            sEBillParticular = $("#cboEBillParticular option:selected").text();
        }
        if (nEBillParticular == null || parseInt(nEBillParticular) <= 0) {
            alert("Please select a Particular from list!");
            return;
        }

        var oExportBillRealizeds = [];
        oExportBillRealizeds = $('#tblExportBillRealizeds').datagrid('getRows');
        var oExportBillRealized = {
            ParticularName: sEBillParticular,
            ExportBillParticularID: nEBillParticular,
            ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
            Amount: parseFloat($("#txtAmountEBillParticular").val())
        };
        oExportBillRealizeds.push(oExportBillRealized);
        DynamicRefreshList(oExportBillRealizeds, "tblExportBillRealizeds");
    
        $("#txtAmountEBillParticular").val("");
       
    });


    $("#btnDeleteExportBillParticular").click(function () {
        var oExportBillRealized = $("#tblExportBillRealizeds").datagrid("getSelected");
        var SelectedRowIndex = $('#tblExportBillRealizeds').datagrid('getRowIndex', oExportBillRealized);
        if (!confirm("Confirm to Delete?")) return false;
        if (oExportBillRealized == null || oExportBillRealized.ExportBillRealizedID <= 0) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportBillRealized,
            ControllerName: "ExportBill",
            ActionName: "Delete_ExportBillRealized",
            TableId: "tblExportBillRealizeds",
            IsWinClose: false
        };
        $.icsDelete(obj);
        $('#tblExportBillRealizeds').datagrid('deleteRow', SelectedRowIndex);
    });
 
    $("#btnAddEncashmentAccount").click(function (e) {

        

        var sBankAccountName = "";
        var nBankAccountID_EN = $("#cboBankAccount_EN option:selected").val();
        if (nBankAccountID_EN > 0) {
            sBankAccountName = $("#cboBankAccount_EN option:selected").text();
        }
        if (nBankAccountID_EN == null || parseInt(nBankAccountID_EN) <= 0) {
            alert("Please select a Bank Account Name from list!");
            return;
        }

        var nCurrencyID = $("#cboCurrency_EncashmentAccount option:selected").val();

        if (nCurrencyID == null || parseInt(nCurrencyID) <= 0) {
            alert("Please select a Currency Name from list!");
            return;
        }

        var oExportBillEncashments = [];
        oExportBillEncashments = $('#tblExportBillEncashments').datagrid('getRows');
        var oExportBillEncashment = {
            BankAccount: sBankAccountName,
            BankAccountID: nBankAccountID_EN,
            CurrencyID: nCurrencyID,
            CCRate: parseFloat($("#txtCCRate_EncashmentAccount").val()),
            ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
            Amount: parseFloat($("#txtAmount_EncashmentAccount").val())
        };
        oExportBillEncashments.push(oExportBillEncashment);
        DynamicRefreshList(oExportBillEncashments, "tblExportBillEncashments");

        $("#txtCCRate_EncashmentAccount").val("");
        $("#txtAmount_EncashmentAccount").val("");

    });

    /// ExportDoc TnC
  
    ///Pick ExportTR
    $("#winExportTR").on("keydown", function (e) {
        var oExportTR = $('#tblExportTRs').datagrid('getSelected');
        var nIndex = $('#tblExportTRs').datagrid('getRowIndex', oExportTR);
        if (e.which == 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblExportTRs').datagrid('selectRow', 0);
            }
            else {
                $('#tblExportTRs').datagrid('selectRow', nIndex - 1);
            }
        }
        if (e.which == 40)//down arrow=40
        {
            var oExportTRList = $('#tblExportTRs').datagrid('getRows');
            if (nIndex >= oExportTRList.length - 1) {
                $('#tblExportTRs').datagrid('selectRow', oExportTRList.length - 1);
            }
            else {
                $('#tblExportTRs').datagrid('selectRow', nIndex + 1);
            }
        }
        if (e.which == 13)//enter=13
        {
            var oExportTR = $("#tblExportTRs").icsGetSelectedItem();
            if (oExportTR != null && oExportTR.ExportTRID > 0) {
                _nExportTRID = oExportTR.ExportTRID;

            }
        }
    });


   

    
    //// END Export DOC TR
}


function RefreshObject_SendToParty() {

    var oExportBill = {
        ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
        SendToParty: new Date($('#txtSendToParty').datebox('getValue')),
        RecdFromParty: new Date($('#txtSendToParty').datebox('getValue')),
        SendToBankDate: new Date($('#txtSendToParty').datebox('getValue')),
        RecedFromBankDate: new Date($('#txtSendToParty').datebox('getValue')),
        LDBCDate: new Date($('#txtSendToParty').datebox('getValue')),
        BankFDDRecDate: new Date($('#txtSendToParty').datebox('getValue')),
        NoteCarry: document.getElementById('txtNote').value,
        
    };
    return oExportBill;
}

function RefreshObject_ReceiveFromParty() {

    var oExportBill = {
        ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
        RecdFromParty: new Date($('#txtRecdFromParty').datebox('getValue')),
        SendToParty: new Date($('#txtRecdFromParty').datebox('getValue')),
        SendToBankDate: new Date($('#txtRecdFromParty').datebox('getValue')),
        RecedFromBankDate: new Date($('#txtRecdFromParty').datebox('getValue')),
        LDBCDate: new Date($('#txtRecdFromParty').datebox('getValue')),
        BankFDDRecDate: new Date($('#txtRecdFromParty').datebox('getValue')),
        NoteCarry: document.getElementById('txtNote_RecdFromParty').value
    };
    return oExportBill;
}

function RefreshObject_SendToBank() {

    var oExportBill = {
        ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
        SendToBankDate: new Date($('#txtSendToBank').datebox('getValue')),
        RecdFromParty: new Date($('#txtSendToBank').datebox('getValue')),
        SendToParty: new Date($('#txtSendToBank').datebox('getValue')),
        RecedFromBankDate: new Date($('#txtSendToBank').datebox('getValue')),
        LDBCDate: new Date($('#txtSendToBank').datebox('getValue')),
        BankFDDRecDate: new Date($('#txtSendToBank').datebox('getValue')),
        NoteCarry: document.getElementById('txtNote_SendToBank').value
    };
    return oExportBill;
}

function RefreshObject_ReceiveFromBank() {

    var oExportBill = {
        ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
        RecedFromBankDate: new Date($('#txtRecedFromBankDate').datebox('getValue')),
        SendToBankDate: new Date($('#txtRecedFromBankDate').datebox('getValue')),
        RecdFromParty: new Date($('#txtRecedFromBankDate').datebox('getValue')),
        SendToParty: new Date($('#txtRecedFromBankDate').datebox('getValue')),
        LDBCDate: new Date($('#txtRecedFromBankDate').datebox('getValue')),
        BankFDDRecDate: new Date($('#txtRecedFromBankDate').datebox('getValue')),
        LDBCNo: document.getElementById('txtLDBCNo').value,
        NoteCarry: document.getElementById('txtNote_ReceieFromBank').value
    };
    return oExportBill;
}

function RefreshObject_MaturityReceive() {

    var oExportBill = {
        ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
        AcceptanceDate: new Date($('#txtAcceptanceDate').datebox('getValue')),
        MaturityDate: new Date($('#txtMaturityDate').datebox('getValue')),
        MaturityReceivedDate: new Date($('#txtMaturityReceivedDate').datebox('getValue')),
        NoteCarry: document.getElementById('txtNote_MaturityRecd').value
    };
    return oExportBill;
}

function RefreshObject_BillRelization() {

    var oExportBill = {
        ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
        RelizationDate: new Date($('#txtRelizationDate').datebox('getValue')),
        ExportBillRealizeds : $('#tblExportBillRealizeds').datagrid('getRows'),
        NoteCarry: document.getElementById('txtNote_Relization').value
    };
    return oExportBill;
}

function RefreshObject_BBankFDDReceive() {

    var oExportBill = {
        ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
        BankFDDRecDate: new Date($('#txtBankFDDRecDate').datebox('getValue')),
        RecedFromBankDate: new Date($('#txtBankFDDRecDate').datebox('getValue')),
        SendToBankDate: new Date($('#txtBankFDDRecDate').datebox('getValue')),
        RecdFromParty: new Date($('#txtBankFDDRecDate').datebox('getValue')),
        SendToParty: new Date($('#txtBankFDDRecDate').datebox('getValue')),
        LDBCDate: new Date($('#txtBankFDDRecDate').datebox('getValue')),
        NoteCarry: document.getElementById('txtNote_BFDDReceive').value
    };
    return oExportBill;
}

function RefreshObject_BillEncashment() {

    var oExportBill = {
        ExportBillID: (_oExportBill != null) ? _oExportBill.ExportBillID : 0,
        EncashmentDate: new Date($('#txtEncashmentDate').datebox('getValue')),
        ExportBillEncashments: $('#tblExportBillEncashments').datagrid('getRows'),
        NoteCarry: document.getElementById('txtNote_Encashment').value
    };
    return oExportBill;
}




function RowSelect(rowIndex, rowData) {

    //  BOEinHand = 0,       //BOEinCustomerHand = 1,       //AcceptedBill = 2,//ReceivedFromBuyer = 2,       //Nego_Transit = 3,       //NegotiatedBill = 4,
    //B_AcceptedBill = 5,       //FDDinHand = 6,       //Discounted = 7,       //Encashment = 8,       //BillClosed = 9,       //BillRealized = 10,       //BillCancel = 11,
    //R_ForDiscounted = 12,

    document.getElementById("btnAddSendToParty").style.display = 'none';
    document.getElementById("btnAddRecedFromParty").style.display = 'none';
    document.getElementById("btnAddSendToBank").style.display = 'none';
    document.getElementById("btnAddRecedfromBank").style.display = 'none';

    document.getElementById("btnAddLDBP").style.display = 'none';
    document.getElementById("btnAddMaturity").style.display = 'none';
    document.getElementById("btnAddBillRelization").style.display = 'none';
    document.getElementById("btnAddBankFDDRecd").style.display = 'none';
    document.getElementById("btnAddEncashment").style.display = 'none';

    if (rowData.StateInt <= 0) {
        document.getElementById("btnAddSendToParty").style.display = '';

    }
    else if (rowData.StateInt == 1) {
        document.getElementById("btnAddSendToParty").style.display = '';
        document.getElementById("btnAddRecedFromParty").style.display = '';

    }
    else if (rowData.StateInt == 2) {
        document.getElementById("btnAddRecedFromParty").style.display = '';
        document.getElementById("btnAddSendToBank").style.display = '';

    }
    else if (rowData.StateInt == 3) {
        document.getElementById("btnAddSendToBank").style.display = '';
        document.getElementById("btnAddRecedfromBank").style.display = '';
    }
    else if (rowData.StateInt == 4) {
        document.getElementById("btnAddRecedfromBank").style.display = '';
        document.getElementById("btnAddMaturity").style.display = '';


    }
    else if (rowData.StateInt == 5) {
        document.getElementById("btnAddMaturity").style.display = '';
        document.getElementById("btnAddBillRelization").style.display = '';


    }
    else if (rowData.StateInt == 12) {
        document.getElementById("btnAddLDBP").style.display = '';
    }
    else if (rowData.StateInt == 7) {
        document.getElementById("btnAddLDBP").style.display = '';
        document.getElementById("btnAddBillRelization").style.display = '';
    }
    else if (rowData.StateInt == 10) {
        document.getElementById("btnAddBillRelization").style.display = '';
        document.getElementById("btnAddBankFDDRecd").style.display = '';
    }
    else if (rowData.StateInt == 6) {

        document.getElementById("btnAddBankFDDRecd").style.display = '';
        document.getElementById("btnAddEncashment").style.display = '';
    }
    else if (rowData.StateInt == 8) {

        document.getElementById("btnAddEncashment").style.display = '';
    }
    else {
        document.getElementById("btnAddSendToParty").style.display = 'none';
        document.getElementById("btnAddRecedFromParty").style.display = 'none';
        document.getElementById("btnAddSendToBank").style.display = 'none';
        document.getElementById("btnAddRecedfromBank").style.display = 'none';

        document.getElementById("btnAddLDBP").style.display = 'none';
        document.getElementById("btnAddMaturity").style.display = 'none';
        document.getElementById("btnAddBillRelization").style.display = 'none';
        document.getElementById("btnAddBankFDDRecd").style.display = 'none';
        document.getElementById("btnAddEncashment").style.display = 'none';
    }
}


function SetCCRate() {
    _nBaseCurrencyID = $("#cboCurrency_EncashmentAccount").val();
    if (parseInt(_nBaseCurrencyID) == 1) {
        $("#txtCCRate_EncashmentAccount").val(1);
        $("#txtCCRate_EncashmentAccount").attr("disabled", true);
    }
    else {
        $("#txtCCRate_EncashmentAccount").val("");
        $("#txtCCRate_EncashmentAccount").attr("disabled", false);
    }
}


