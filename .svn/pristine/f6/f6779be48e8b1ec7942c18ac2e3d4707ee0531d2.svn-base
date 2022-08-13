
var _oContractor = [];
var _oBankBranch = [];
var _oMasterLC = [];
var _oSelectedExportPIs = [];
var _oExportLCAmendmentRequests=[]

function InitializeExportLCEvents() {
    DynamicRefreshList([], "tblExportPILCMappings");
    LoadComboboxexExportLC();

    $("#btnSaveExportLC").click(function (e) {
        
        if (!ValidateInputExportLC(false)) return;

        if ($("#cboLCTerm").val() <= 0) {
            $('#tabExportLCTabsExportLC').tabs('select', 1);
            alert("Please select LC Term!"); $('#cboLCTerm').focus();
            $("#cboLCTerm").addClass("errorFieldBorder");
            return false;
        } else { $("#cboLCTerm").removeClass("errorFieldBorder"); }
        
        var oExportLC = RefreshObjectExportLC();
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportLC/SaveExportLC",
            traditional: true,
            data: JSON.stringify(oExportLC),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                var oELC = jQuery.parseJSON(data);
                if (oELC.ErrorMessage == "" || oELC.ErrorMessage == null) {
                    _oExportLC = oELC;
                    alert("Data Save Succesfully!!");
                    $('#tabExportLCTabsExportLC').tabs('select', 1);
                    //if (_oPurchaseOrder.PurchaseOrderDetails != null) {
                    //    DynamicRefreshList(_oPurchaseOrder.PurchaseOrderDetails, 'tblPODetail');
                    //}

                    var oExportLCs = $("#tblExportLCs").datagrid("getRows");
                    var nFlag = 0;
                    for (var i = 0; i < oExportLCs.length; i++) {
                        if (oExportLCs[i].ExportLCID == _oExportLC.ExportLCID) {
                            $('#tblExportLCs').datagrid('updateRow', { index: i, row: _oExportLC });
                            nFlag = 1;
                            break;
                        }
                    }
                    if (nFlag == 0) {
                        var nIndex = oExportLCs.length;
                        $("#tblExportLCs").datagrid("appendRow", _oExportLC);
                        $("#tblExportLCs").datagrid("selectRow", nIndex);
                    }

                }
                else {
                    alert(oELC.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
        //var obj =
        //{
        //    BaseAddress: _sBaseAddress,
        //    Object: oExportLC,
        //    ObjectId: oExportLC.ExportLCID,
        //    ControllerName: "ExportLC",
        //    ActionName: "SaveExportLC",
        //    TableId: "tblExportLCs",
        //    IsWinClose: true
        //};
        //$.icsSave(obj);
    });

    $("#btnSaveExportLCTnC").click(function (e) {
        if (!ValidateInputExportLC(false)) return;

        if (_oExportLC.ExportLCID <= 0 || _oExportLC == null)
        {
            alert("Please, First save LC from  previous tab!");
            return ;
        }

        if ($("#cboLCTerm").val() <= 0) {
            $('#tabExportLCTabsExportLC').tabs('select', 1);
            alert("Please select LC Term!"); $('#cboLCTerm').focus();
            $("#cboLCTerm").addClass("errorFieldBorder");
            return ;
        }
        else { $("#cboLCTerm").removeClass("errorFieldBorder"); }

        var oExportLC = RefreshObjectExportLC_TnC();
        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportLC,
                ObjectId: oExportLC.ExportLCID,
                ControllerName: "ExportLC",
                ActionName: "SaveExportLCTnC",
                TableId: "tblExportLCs",
                IsWinClose: true
            };
        $.icsSave(obj);
    });
        
    $("#btnCloseExportLC").click(function () {
        $("#winExportBills").icsWindow("close");
    });

    $("#btnCloseExportBill").click(function () {
        $("#winExportLC").icsWindow("close");
    });

    $("#btnCloseMasterLC").click(function () {
        $("#winAddMasterLC").icsWindow("close");
    });

    $("#btnCloseExportLCAmendmentRequests").click(function () {
        $("#winExportLCAmendmentRequests").icsWindow("close");
    });

    $("#btnCloseExportLCAmendmentClauses").click(function () {
        $("#winExportLCAmendmentRequest").icsWindow("close");
    });

    $("#btnCloseExportLCTnCC").click(function () {
        $("#winExportLC").icsWindow("close");
    });

  
    $("#btnCloseExportBillDetails").click(function () {
        $("#winExportBillDetails").icsWindow("close");
    });

    $("#txtContractorName").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            PickContractor_LC();
        }
        else if (e.keyCode === 08) {
            $("#txtContractorName").removeClass("fontColorOfPickItem");
            _oExportLC.ApplicantID = 0;
        }
    });

    $("#btnClrContractor_ELC").click(function () {
        $("#txtContractorName").removeClass("fontColorOfPickItem");
        $("#txtContractorName").val("");
        _oExportLC.ApplicantID = 0;
        $("#txtVat").val("");
        $("#txtReg").val("");
        $("#btnAddNewVatReg").hide();
    });

    $("#btnPickContractor_ELC").click(function () {
        PickContractor_LC();
    });


    



    $("#txtDeliverTo").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            PickDeliveryTo_LC();
        }
        else if (e.keyCode === 08) {
            $("#txtDeliverTo").removeClass("fontColorOfPickItem");
            _oExportLC.DeliveryToID = 0;
        }
    });

    $("#btnClrDeliveryTo_ELC").click(function () {
        $("#txtDeliverTo").removeClass("fontColorOfPickItem");
        $("#txtDeliverTo").val("");
        _oExportLC.DeliveryToID = 0;
    });

    $("#btnPickDeliveryTo_ELC").click(function () {
        PickDeliveryTo_LC();
    });



    $("#txtSearchByContractorNamePicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oContractor,
            TableId: "tblContractorsPicker"
        };
        $('#txtSearchByContractorNamePicker').icsSearchByText(obj);

    });

    $("#btnCloseContractorPicker_ELC").click(function () {
        $("#winContractorPicker_ELC").icsWindow("close");
        _oContractor = [];
        $("#winContractorPicker_ELC input").val("");
        DynamicRefreshList([], "tblContractorsPicker");
    });

    $("#winContractorPicker_ELC").keydown(function (e) {
       
        if (e.keyCode == 38 || e.keyCode == 40 || e.keyCode == 32) {
           
            $('#tblContractorsPicker').icsUpDownEvent({ Event: e, IsChecked: true });
        }
    });

    $("#txtSearchByBankBranch_Issue").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            PickBankBranch_ELC();
        }
        else if (e.keyCode === 08) {
            $("#txtSearchByBankBranch_Issue").removeClass("fontColorOfPickItem");
            _oExportLC.BBranchID_Issue = 0;
        }
    });

    $("#btnClrBankBranch_Issue").click(function () {
        $("#txtSearchByBankBranch_Issue").removeClass("fontColorOfPickItem");
        $("#txtSearchByBankBranch_Issue").val("");
        _oExportLC.BBranchID_Issue = 0;
    });

    $("#btnPickBankBranch_Issue").click(function () {
        PickBankBranch_ELC();
    });

    $("#txtSearchByContractorNamePicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oBankBranch,
            TableId: "tblBankBranchsPicker"
        };
        $('#txtSearchByContractorNamePicker').icsSearchByText(obj);
    });

    $("#btnCloseBankBranchPicker_ELC").click(function () {
        $("#winBankBranchPicker_ELC").icsWindow("close");
        _oBankBranch = [];
        $("#winBankBranchPicker_ELC input").val("");
        DynamicRefreshList([], "tblBankBranchsPicker");
    });

    $("#winBankBranchPicker_ELC").keydown(function (e) {
        if (e.keyCode == 38 || e.keyCode == 40 || e.keyCode == 32) {
           
            $('#tblBankBranchsPicker').icsUpDownEvent({ Event: e, IsChecked: true });
        }
    });

    $("#txtPINo").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            PickExportPI_ELC();
        }
        else if (e.keyCode === 08) {
            $("#txtPINo").removeClass("fontColorOfPickItem");
            //_oExportLC.BBranchID_Issue = 0;
        }
    });

    $("#btnAddPILCMapping").click(function () {
        PickExportPI_ELC();
    });

    $("#btnCloseExportPIPicker_ELC").click(function () {
        $("#winExportPIPicker_ELC").icsWindow("close");
        _oSelectedExportPIs = [];
        $("#winExportPIPicker_ELC input").val("");
        DynamicRefreshList([], "tblExportPILCMappings");
    });

    $("#winExportPIPicker_ELC").keydown(function (e) {
        if (e.keyCode == 38 || e.keyCode == 40 || e.keyCode == 32) {
           
            $('#tblExportPIPicker').icsUpDownEvent({ Event: e, IsChecked: true });
        }
    });

    $("#txtMasterLCSearch").keydown(function (e) {
        if (e.keyCode === 13) // Enter Press
        {
            PickMasterLC_ELC();
        }
        else if (e.keyCode === 08) {
            $("#txtMasterLCSearch").removeClass("fontColorOfPickItem");
         
        }
    });
  
    $("#btnPickMasterLC").click(function () {
        if (_oExportLC.ExportLCID <= 0)
        {
            alert("Please first confirm L/C Save");
            return;
        }
        PickMasterLC_ELC();
    });

    $("#btnSaveMasterLC").click(function () {
        if (!ValidationMasterLC()) return false;
        var oMasterLC = {
            MasterLCNo: $("#txtMasterLCNo").val(),
            LastDofShipment: $('#txtLastDofShipment').datebox('getValue'),
            ExpireDate: $('#txtExpireDate_MasterLC').datebox('getValue'),
            MasterLCDate: $('#txtMasterLCDate').datebox('getValue'),
            Country: $("#txtCountry").val(),
            ProductDesc: $("#txtProductDesc").val(),
            MasterLCType : ($("#chkMaskerLC").is(':checked') == true ? 1 : 2),
            Remark: $("#txtRemark").val()
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/MasterLC/SaveMasterLC",
            traditional: true,
            data: JSON.stringify(oMasterLC),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oMasterLC = jQuery.parseJSON(data);
                if (parseInt(oMasterLC.MasterLCID) > 0) {
                    var oMasterLCs = $("#tblMasterLCPicker").datagrid("getRows");
                    var nIndex = oMasterLCs.length;
                    $("#tblMasterLCPicker").datagrid("appendRow", oMasterLC);
                    $("#tblMasterLCPicker").datagrid("selectRow", nIndex);
                    $("#winAddMasterLC").icsWindow("close");
                }
                else {
                    alert(oMasterLC.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });
 
    $("#txtSearchByMasterLCNoPicker").keydown(function (e) {
        var obj = {
            Event: e,
            SearchProperty: "Name",
            GlobalObjectList: _oMasterLC,
            TableId: "tblMasterLCPicker"
        };
        $('#txtSearchByMasterLCNoPicker').icsSearchByText(obj);
    });

    $("#btnCloseMasterLCPicker_ELC").click(function () {
        $("#winMasterLC_ELC").icsWindow("close");
        _oMasterLC = [];
        $("#winMasterLC_ELC input").val("");
        DynamicRefreshList([], "tblMasterLCPicker");
    });

    $("#txtNegoDays").keydown(function (e) {
        var dShipmentDate = $('#txtShipmentDate').datebox('getValue');
        if (dShipmentDate == null || dShipmentDate == "")
        {
            alert("Please give Shipment Date.");
            $("#txtNegoDays").val("");
            return false;
        }
    });

    $("#txtNegoDays").focusout(function () {
        if ($.trim($("#txtNegoDays").val()) != "")
        {
            var nTotalDays = parseInt($("#txtNegoDays").val());
            CheckDisburseDate(nTotalDays, _oExportLC.ExportLCID);
        }
    });

    $("#btnOkExportLCAmendmentClauses").click(function () {
        AddExportLCAmendmentClauses();
    });

    $("#winExportLCAmendmentRequest").on("keydown", function (e) {
        var oContractor = $('#tblExportLCAmendmentRequest').datagrid('getSelected');
        var nIndex = $('#tblExportLCAmendmentRequest').datagrid('getRowIndex', oContractor);
        if (e.which === 38)//up arrow=38
        {
            if (nIndex <= 0) {
                $('#tblExportLCAmendmentRequest').datagrid('selectRow', 0);
            }
            else {
                $('#tblExportLCAmendmentRequest').datagrid('selectRow', nIndex - 1);
            }
            $('#txtNewAmendmentCaluse').blur();
        }
        if (e.which === 40)//down arrow=40
        {
            var oCurrentList = $('#tblExportLCAmendmentRequest').datagrid('getRows');
            if (nIndex >= oCurrentList.length - 1) {
                $('#tblExportLCAmendmentRequest').datagrid('selectRow', oCurrentList.length - 1);
            }
            else {
                $('#tblExportLCAmendmentRequest').datagrid('selectRow', nIndex + 1);
            }
            $('#txtNewAmendmentCaluse').blur();
        }
        if (e.which === 13)//enter=13
        {
            //AddExportLCAmendmentClauses();
        }
    });

    $("#btnAddAmendmentCaluse").click(function () {
        AddAmendmentCaluse();
    });

    $("#txtNewAmendmentCaluse").keydown(function (e) {
        if (e.keyCode === 13)
        {
            
            AddAmendmentCaluse();
        }
    });

    $("#btnSaveExportLCStatusUpdate").click(function (e) {
        var oSelectedObject = $("#tblExportLCs").datagrid("getSelected");
        var nRowIndex = $("#tblExportLCs").datagrid("getRowIndex", oSelectedObject);
        var oExportLC = {
            ExportLCID:_oExportLC.ExportLCID,
            SLNo: parseInt($("#cboLCStatus").val()),
            Remark: $("#txtNoteUpdateStatus").val()
        };
        var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportLC,
                ObjectId: oExportLC.ExportLCID,
                ControllerName: "ExportLC",
                ActionName: "Save_ExportLCStatusUpdate",
                TableId: "tblExportLCs",
                IsWinClose: true
            };
        $.icsSave(obj, function (response) {
            if (response.status) {
                OperationPerformsExportLC(nRowIndex, response.obj);
            }
        });
    });

    $("#btnRemovePILCMapping").click(function () {
        var oExportPILCMapping = $("#tblExportPILCMappings").datagrid("getSelected");
        if (oExportPILCMapping == null || oExportPILCMapping.ExportPILCMappingID <= 0)
        {
            alert("Please select an item from list.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;

        var SelectedRowIndex = $('#tblExportPILCMappings').datagrid('getRowIndex', oExportPILCMapping);
        if (oExportPILCMapping == null || oExportPILCMapping.ExportPILCMappingID <= 0) {
            $('#tblExportPILCMappings').datagrid('deleteRow', SelectedRowIndex);
        }
        else {
            
            var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportPILCMapping,
                ObjectId: oExportPILCMapping.ExportPILCMappingID,
                ControllerName: "ExportLC",
                ActionName: "DeletePILCMapping",
                TableId: "tblExportPILCMappings",
                IsWinClose: false
            };
            $.icsDelete(obj, function (response) {
                RefreshTotalSummery_PI();
                CalculateTotalEPIDetail();
                
            });
        }
    });

    $("#btnShowDOPILCMapping").click(function () {
        
        var oExportPI = $("#tblExportPILCMappings").datagrid("getSelected");
        if (oExportPI == null || oExportPI.ExportPIID <= 0)
        {
            alert("Please select an item list");
            return false;
        }

        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oExportPI,
            ControllerName: "ExportPI",
            ActionName: "GetDOs",
            IsWinClose: false
        };
        $.icsMaxDataGets(obj, function (response) {
            
            if (response.status && response.objs.length > 0) {
                if (response.objs.length > 0) {
                    if ($.trim(response.objs[0].ErrorMessage) == "") {
                        var tblColums = []; var oColumn = { field: "Note", title: "DO No", width: 200, align: "left" }; tblColums.push(oColumn);
                        oColumn = { field: "IssueDateInString", title: "Date", width: 100, align: "center" }; tblColums.push(oColumn);
                        oColumn = { field: "QtySt", title: "DO Qty", width: 80, align: "right" }; tblColums.push(oColumn);
                        oColumn = { field: "OverdueRateSt", title: "Challan Qty", width: 80, align: "right" }; tblColums.push(oColumn);
                        var oPickerParam = {
                            winid: 'winDOs',
                            winclass: 'clsDOs',
                            winwidth: 510,
                            winheight: 460,
                            tableid: 'tblDOs',
                            tablecolumns: tblColums,
                            datalist: response.objs,
                            multiplereturn: false,
                            searchingbyfieldName: 'Note',
                            windowTittle: 'DO List',
                            btnOkShow: false
                        };
                        $.icsPicker(oPickerParam);
                    }
                    else {
                        alert(response.objs[0].ErrorMessage);
                    }
                }
                else {
                    alert(response.objs[0].ErrorMessage);
                }
            }
            else {
                alert("Sorry, No Applicant Found.");
            }
        });
    });

    $("#btnDeleteMasterLCMapping").click(function () {
        var oMasterLCMapping = $("#tblMasterLCs").datagrid("getSelected");
        if (oMasterLCMapping == null || oMasterLCMapping.MasterLCMappingID <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        if (!confirm("Confirm to Delete?")) return false;

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oMasterLCMapping,
            ObjectId: oMasterLCMapping.MasterLCMappingID,
            ControllerName: "MasterLC",
            ActionName: "DeleteMasterLCMapping",
            TableId: "tblMasterLCs",
            IsWinClose: false
        };
        $.icsDelete(obj);
    });

    $("#btnAddNewVatReg").click(function () {
        if (_oExportLC.ApplicantID <= 0) {
            alert("Invalid Contractor.");
            return false;
        }
        $("#txtVatPicker").val("");
        $("#txtRegPicker").val("");
        $("#lblBuyerNamePicker").text($("#txtContractorName").val());
        $("#winVatReg").icsWindow("open", "Add Vat & Registration");
    });

    $("#btnCloseVatReg").click(function(){
        $("#lblBuyerNamePicker").text("");
        $("#winVatReg").icsWindow("close");
    });

    $("#btnSaveVatReg").click(function () {

        if ($.trim($("#txtRegPicker").val()) == "") {
            alert("Please give Registration");
            $("#txtRegPicker").focus();
            $("#txtRegPicker").addClass("errorFieldBorder");
            return false;
        } else {
            $("#txtRegPicker").removeClass("errorFieldBorder");
        }

        if ($.trim($("#txtVatPicker").val()) == "") {
            alert("Please give Vat");
            $("#txtVatPicker").focus();
            $("#txtVatPicker").addClass("errorFieldBorder");
            return false;
        } else {
            $("#txtVatPicker").removeClass("errorFieldBorder");
        }

        var oContractor = {
            ContractorID: _oExportLC.ApplicantID,
            VAT: $.trim($("#txtVatPicker").val()),
            TIN: $.trim($("#txtRegPicker").val())
        };

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oContractor,
            ObjectId: oContractor.ContractorID,
            ControllerName: "Contractor",
            ActionName: "UpdateVatAndReg",
            TableId: "",
            IsWinClose: true
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                if (response.obj != null && response.obj.ContractorID > 0) {
                    debugger;
                    alert(response.obj.VAT);
                    $("#txtVat").val(response.obj.VAT);
                    $("#txtReg").val(response.obj.TIN);
                    AddVatRegBtnHideShow();
                }
            }
        });

    });


    $("#chkMaskerLC1").change(function () {
        if (this.checked) {
            $(".lblLCType").text("Master");
            $('#chkExportLC1').prop('checked', false);
            $("#txtMasterLCNo1").attr("placeholder", "Type Master LC No");

        } else {
            $(".lblLCType").text("");
            $('#chkExportLC1').prop('checked', true);
            $("#txtMasterLCNo1").attr("placeholder", "Type LC No");
        }
    });

    $("#chkExportLC1").change(function () {
        if (this.checked) {
            $(".lblLCType").text("");
            $('#chkMaskerLC1').prop('checked', false);
            $("#txtMasterLCNo1").attr("placeholder", "Type LC No");
        } else {
            $(".lblLCType").text("Master");
            $('#chkMaskerLC1').prop('checked', true);
            $("#txtMasterLCNo1").attr("placeholder", "Type Master LC No");
        }
    });

    $("#btnAddMasterLC1").click(function () {
        //Imrez
            if (!ValidationMasterLC1()) return false;

            var oMasterLC = {
                MasterLCNo: $("#txtMasterLCNo1").val(),
                MasterLCDate: $('#txtMasterLCDate1').datebox('getValue'),
                MasterLCType: ($("#chkMaskerLC").is(':checked') == true ? 1 : 2),
            };
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/MasterLC/SaveMasterLC",
                traditional: true,
                data: JSON.stringify(oMasterLC),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var oMasterLC = jQuery.parseJSON(data);
                    if (parseInt(oMasterLC.MasterLCID) > 0) {
                        var oMasterLCMapping = {
                            ExportLCID: _oExportLC.ExportLCID,
                            ContractorID: _oExportLC.ApplicantID,
                            MasterLCID: oMasterLC.MasterLCID
                        };
                        $.ajax({
                            type: "POST",
                            dataType: "json",
                            url: _sBaseAddress + "/MasterLC/SaveMasterLCMapping",
                            traditional: true,
                            data: JSON.stringify(oMasterLCMapping),
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                var oMasterLCMapping = jQuery.parseJSON(data);
                                if (parseInt(oMasterLCMapping.MasterLCMappingID) > 0) {
                                    var oMasterLCMappings = $("#tblMasterLCs").datagrid("getRows");
                                    var nIndex = oMasterLCMappings.length;
                                    $("#tblMasterLCs").datagrid("appendRow", oMasterLCMapping);
                                    $("#tblMasterLCs").datagrid("selectRow", nIndex);

                                    //Ratin
                                    $('#chkExportLC1').prop('checked', true);
                                    $('#chkMaskerLC1').prop('checked', false);
                                    $("#txtMasterLCDate1").datebox({ disabled: false });
                                    $("#txtMasterLCDate1").datebox("setValue", icsdateformat(new Date()));
                                    $("#txtMasterLCNo1").attr("placeholder", "Type LC No");
                                    $("#txtMasterLCNo1").val("");
                                }
                                else {
                                    alert(oMasterLCMapping.ErrorMessage);
                                }
                            },
                            error: function (xhr, status, error) {
                                alert(error);
                            }
                        });
                    }
                    else {
                        alert(oMasterLC.ErrorMessage);
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });

    });
}



function CheckDisburseDate(nTotalDays, nExportLCID)
{
    if (nExportLCID > 0) {
        var oExportLC = {
            ExportLCID: nExportLCID
        };

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportLC,
            ControllerName: "ExportLC",
            ActionName: "CheckSUDeliveryChallanDateForLC",
            IsWinClose: false
        };
        $.icsDataGet(obj, function (response) {
            
            if (response.status && response.obj != null) {
                if (response.obj.SUDeliveryChallanID > 0) {
                    SetNegoDate(nTotalDays, response.obj.ChallanDateSt);
                }
                else {
                    SetNegoDate(nTotalDays, $('#txtShipmentDate').datebox('getValue'));
                }
            }
            else {
                SetNegoDate(nTotalDays, $('#txtShipmentDate').datebox('getValue'));
            }
        });
    }
    else
    {
        SetNegoDate(nTotalDays, $('#txtShipmentDate').datebox('getValue'));
    }
   
}

function CheckDisburseDate01(nTotalDays, nExportLCID) {
    if (nExportLCID > 0) {
        var oExportLC = {
            ExportLCID: nExportLCID
        };

        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oExportLC,
            ControllerName: "ExportLC",
            ActionName: "CheckSUDeliveryChallanDateForLC",
            IsWinClose: false
        };
        $.icsDataGet(obj, function (response) {
            
            if (response.status && response.obj != null) {
                if (response.obj.SUDeliveryChallanID > 0) {
                    SetNegoDate01(nTotalDays, response.obj.ChallanDateSt);
                }
                else {
                    SetNegoDate01(nTotalDays, $('#txtShipmentDate01').datebox('getValue'));
                }
            }
            else {
                SetNegoDate01(nTotalDays, $('#txtShipmentDate01').datebox('getValue'));
            }
        });
    }
    else {
        SetNegoDate01(nTotalDays, $('#txtShipmentDate01').datebox('getValue'));
    }

}

function SetNegoDate(nTotalDays, dDate) {
    $("#txtNegoDate").prop("disabled", true);
    if (nTotalDays > 0) {
        //var dShipmentDate = $('#txtShipmentDate').datebox('getValue');
        var oDate = new Date(dDate);
        oDate = oDate.setDate(oDate.getDate() + nTotalDays);
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var sDate = new Date(oDate).getDate() + " " + monthNames[new Date(oDate).getMonth()] + " " + new Date(oDate).getFullYear();
        $('#txtNegoDate').val(sDate);
    }
    else {
        $('#txtNegoDate').val("");
    }

    if ($("#txtNegoDate").val() == "NaN undefined NaN")
    {
        $('#txtNegoDate').val("");
    }
}

function SetNegoDate01(nTotalDays, dDate) {
    $("#txtNegoDate01").prop("disabled", true);
    if (nTotalDays > 0) {
        //var dShipmentDate = $('#txtShipmentDate01').datebox('getValue');
        var oDate = new Date(dDate);
        oDate = oDate.setDate(oDate.getDate() + nTotalDays);
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var sDate = new Date(oDate).getDate() + " " + monthNames[new Date(oDate).getMonth()] + " " + new Date(oDate).getFullYear();
        $('#txtNegoDate01').val(sDate);
    }
    else {
        $('#txtNegoDate01').val("");
    }

    if ($("#txtNegoDate01").val() == "NaN undefined NaN") {
        $('#txtNegoDate01').val("");
    }
}

function AddExportLCAmendmentClauses() {
    //var oExportLCAmendmentClauses = $("#tblExportLCAmendmentRequest").icsGetCheckedItem();
    var obj = $("#tblExportLCAmendmentRequest").datagrid("getChecked");
    
    if (obj == null || obj.length<=0)
    {
        alert("Please select an item from list.");
        return false;
    }
    var oExportLC = $("#tblExportLCs").datagrid("getSelected");
    var nRowIndex = $("#tblExportLCs").datagrid("getRowIndex", oExportLC);

  

    var oExportLCAmendmentClauses = [];
    //oExportLCAmendmentClauses.push(obj);
    for (var i = 0; i < obj.length; i++) {
        oExportLCAmendmentClauses.push(obj[i]);
    }
    var oExportLCAmendmentRequest = NewExportLCAmendmentRequest();
    oExportLCAmendmentRequest.ExportLCAmendmentClauses = oExportLCAmendmentClauses;
    if (oExportLCAmendmentClauses[0].ExportLCAmendRequestID > 0) {
        oExportLCAmendmentRequest.ExportLCAmendmentRequestID = oExportLCAmendmentClauses[0].ExportLCAmendRequestID;
    }
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oExportLCAmendmentRequest,
        ObjectId: oExportLCAmendmentRequest.ExportLCAmendmentRequestID,
        ControllerName: "ExportLC",
        ActionName: "Save_AmendmentRequest",
        TableId: "tblExportLCAmendmentRequests",
        IsWinClose: true
    };
    $.icsSave(obj, function (response) {
        if (response.status && response.obj != null) {
            if (response.obj != null && response.obj.ExportLCAmendmentRequestID > 0) {
                $("#tblExportLCs").datagrid("updateRow", { index: nRowIndex, row: response.obj.ExportLC });
                OperationPerformsExportLC(nRowIndex, response.obj.ExportLC);
            }
        }
    });
}

function ValidationMasterLC()
{
    if ($.trim($("#txtMasterLCNo").val()) == "") {
        alert("Please give LC No");
        $("#txtMasterLCNo").focus();
        $("#txtMasterLCNo").addClass("errorFieldBorder");
        return false;
    } else {
        $("#txtMasterLCNo").removeClass("errorFieldBorder");
    }
    return true;
}

function ValidationMasterLC1() {
    if (_oExportLC.ExportLCID <= 0 || _oExportLC==null ) {
        alert("Please, First Save L/C, from previous tab ");
       
        return false;
    }
    if ($.trim($("#txtMasterLCNo1").val()) == "") {
        alert("Please Entry Master/Export LV No");
        $("#txtMasterLCNo1").focus();
        $("#txtMasterLCNo1").addClass("errorFieldBorder");
        return false;
    } else {
        $("#txtMasterLCNo1").removeClass("errorFieldBorder");
    }
    return true;
}

function AddAmendmentCaluse() {
    if ($.trim($("#txtNewAmendmentCaluse").val()) == "") {
        alert("Please write some Clause");
        $("#txtNewAmendmentCaluse").addClass("errorFieldBorder");
        $("#txtNewAmendmentCaluse").focus();
        return false;
    } else {
        $("#txtNewAmendmentCaluse").removeClass("errorFieldBorder");
    }
    var oExportLCAmendmentClause = {
        ExportLCAmendmentClauseID: 0,
        Clause: $("#txtNewAmendmentCaluse").val(),
        ExportLCAmendRequestID: 0
    };
    $('#tblExportLCAmendmentRequest').datagrid('appendRow', oExportLCAmendmentClause);
    $("#txtNewAmendmentCaluse").val("");
}

function LoadComboboxexExportLC() {
    

    $("#cboBankBranch_Advice, #cboBankBranch_Nego").icsLoadCombo({
        List: _oBankBranchs,
        OptionValue: "BankBranchID",
        DisplayText: "BankBranchName"
    });

    $("#cboCurrency").icsLoadCombo({
        List: _oCurrencys,
        OptionValue: "CurrencyID",
        DisplayText: "Symbol"
    });

    $("#cboLCTerm").icsLoadCombo({
        List: _oLCTerms,
        OptionValue: "LCTermID",
        DisplayText: "NameDaysInString"
    });

    $("#cboBenificiary, #cboBenificiary01").icsLoadCombo({
        List: _oBusinessUnits,
        OptionValue: "BusinessUnitID",
        DisplayText: "Name"
    });

    $("#cboLCStatus").icsLoadCombo({
        List: _oLCStatus,
        OptionValue: "id",
        DisplayText: "Value"
    });
    $("#cboPaymentInstruction").icsLoadCombo({
        List: _oPaymentInstructions,
        OptionValue: "id",
        DisplayText: "Value"
    });
}

function NewExportLCAmendmentRequest() {
    var oExportLCAmendmentRequest = {
        ExportLCAmendmentRequestID: 0,
        ExportLCID: _oExportLC.ExportLCID,
        ExportLCAmendmentClauses:[]
    };
    return oExportLCAmendmentRequest;
}

function PickContractor_LC() {
    
    var oContractor = {
        Params: '2,3' + '~' + $.trim($("#txtContractorName").val()) + "~3~"+_nBUID // 3 = PI Issue Status 
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByPIStatus",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblContractorsPicker");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                var tblColums = []; var oColumn = { field: "Name", title: "Name", width: 200, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ContractorTypeInString", title: "Type", width: 120, align: "left" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winContractors',
                    winclass: 'clsContractors',
                    winwidth: 400,
                    winheight: 460,
                    tableid: 'tblContractors',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Applicant List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);
            }
            else {
                alert(response.objs[0].ErrorMessage);
            }
        }
        else {
            alert("Sorry, No Applicant Found.");
        }
    });
}

function PickDeliveryTo_LC()
{

    var oContractor = {
        Params: '2,3' + '~' + $.trim($("#txtDeliverTo").val())+'~'+_nBUID
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByNameType",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblContractorsPicker");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                var tblColums = []; var oColumn = { field: "Name", title: "Name", width: 200, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ContractorTypeInString", title: "Type", width: 120, align: "left" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winDeliveryTos',
                    winclass: 'clsDeliveryTos',
                    winwidth: 400,
                    winheight: 460,
                    tableid: 'tblDeliveryTos',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Delivery To List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);
            }
            else {
                alert(response.objs[0].ErrorMessage);
            }
        }
        else {
            alert("Sorry, No Applicant Found.");
        }
    });
}

function IntializePickerbutton(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        SetPickerValueAssign(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which === 13) {
            SetPickerValueAssign(oPickerobj);
        }
    });
}

function AddVatRegBtnHideShow()
{
    var sVal = $("#txtVat").val();
    var sRegistration = $("#txtReg").val();

    if ($.trim(sVal) == "" && $.trim(sRegistration) == "") {
        $("#btnAddNewVatReg").show();
    } else {
        $("#btnAddNewVatReg").hide();
    }
}

function SetPickerValueAssign(oPickerobj) {
    var oreturnObj = null, oreturnObjs = [];
    if (oPickerobj.multiplereturn) {
        oreturnObjs = $('#' + oPickerobj.tableid).datagrid('getChecked');
    } else {
        oreturnObj = $('#' + oPickerobj.tableid).datagrid('getSelected');
    } 

    if (oPickerobj.winid == 'winContractors') {
        if (oreturnObj != null && oreturnObj.ContractorID > 0) {
                var oContractor = oreturnObj;
                $('#txtContractorName').val(oContractor.Name);
                $("#txtVat").val(oContractor.VAT);
                $("#txtReg").val(oContractor.TIN);
                $("#txtContractorName").addClass("fontColorOfPickItem");
                $("#txtContractorName").focus();
                _oExportLC.ApplicantID = oContractor.ContractorID;
                AddVatRegBtnHideShow();
        } else {
            alert("Please select an Applicant.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winDeliveryTos') {
        if (oreturnObj != null && oreturnObj.ContractorID > 0) {
            var oContractor = oreturnObj;
            $('#txtDeliverTo').val(oContractor.Name);
            $("#txtDeliverTo").addClass("fontColorOfPickItem");
            $("#txtDeliverTo").focus();
            _oExportLC.DeliveryToID = oContractor.ContractorID;
        } else {
            alert("Please select an Applicant.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winBankBranch') {
        if (oreturnObj != null && oreturnObj.BankBranchID > 0) {
            var oBankBranch = oreturnObj;
            $('#txtSearchByBankBranch_Issue').val(oBankBranch.BankBranchName);
            $("#txtSearchByBankBranch_Issue").addClass("fontColorOfPickItem");
            $("#txtSearchByBankBranch_Issue").focus();
            _oExportLC.BBranchID_Issue = oBankBranch.BankBranchID;
        } else {
            alert("Please select a Bank Branch.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winMasterLC_ELC') {

        if (_oExportLC.ExportLCID <= 0 || _oExportLC.ExportLCID==null)
        {
            alert("Please, First save L/C");
            return;
        }
        if (oreturnObj != null && oreturnObj.MasterLCID > 0) {
            var oMasterLC = oreturnObj;
           
            var oMasterLCMapping = {
                ExportLCID: _oExportLC.ExportLCID,
                ContractorID: _oExportLC.ApplicantID,
                MasterLCID: oMasterLC.MasterLCID
            };
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/MasterLC/SaveMasterLCMapping",
                traditional: true,
                data: JSON.stringify(oMasterLCMapping),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    
                    var oMasterLCMapping = jQuery.parseJSON(data);
                    if (parseInt(oMasterLCMapping.MasterLCMappingID) > 0) {
                        var oMasterLCMappings = $("#tblMasterLCs").datagrid("getRows");
                        var nIndex = oMasterLCMappings.length;
                        $("#tblMasterLCs").datagrid("appendRow", oMasterLCMapping);
                        $("#tblMasterLCs").datagrid("selectRow", nIndex);
                    }
                    else {
                        alert(oMasterLCMapping.ErrorMessage);
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        } else {
            alert("Please select a Master LC.");
            return false;
        }
    }
    else if (oPickerobj.winid == 'winExportPI') {
        if (oreturnObj != null && oreturnObj.ExportPIID > 0) {

            var oExportPI = {
                ExportPIID: parseInt(oreturnObj.ExportPIID)
            };
            var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oExportPI,
                ControllerName: "ExportPI",
                ActionName: "GetExportPI",
                IsWinClose: false
            };
            $.icsDataGet(obj, function (response) {
                //Ratin
                if (response.status && response.obj != null) {
                    var oEPI = response.obj;
                    if (oEPI.ExportPIID > 0)
                    {
                        if (_oExportLC.ApplicantID == 0) {
                            _oExportLC.ApplicantID = oEPI.ContractorID;
                            if (_oExportLC.ApplicantID > 0) {
                                $("#txtContractorName").addClass("fontColorOfPickItem");
                                $("#txtContractorName").val(oEPI.ContractorName);
                                $("#cboLCTerm").val(oEPI.LCTermID);
                                _oExportLC.DeliveryToID = oEPI.DeliveryToID;
                                $("#txtDeliverTo").val(oEPI.DeliveryToName);
                            } else {
                                $("#txtContractorName").removeClass("fontColorOfPickItem");
                                $("#txtContractorName").val("");
                            }
                        }

                        if (parseInt($("#cboCurrency").val()) == 0) {
                            $("#cboCurrency").val(oEPI.CurrencyID);
                        }

                        if (parseInt($("#cboBankBranch_Advice").val()) == 0) {
                            $("#cboBankBranch_Advice").val(oEPI.BankBranchID);
                        }
                    }
                    PickPIExportLC(oreturnObj);
                }
            });
          
        }
        else {
            alert("Please select an item.");
            return false;
        }
    }
    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
}

function AppendPIwithLC(oTempPILCMapping)
{

    var oPILCMappings = $("#tblExportPILCMappings").datagrid("getRows");
    var nFlag = 0;
    for (var i = 0; i < oPILCMappings.length; i++) {
        if (oPILCMappings[i].ExportPIID == oTempPILCMapping.ExportPIID) {
            $('#tblExportPILCMappings').datagrid('updateRow', { index: i, row: oTempPILCMapping });
            nFlag = 1;
            break;
        }
    }
    if (nFlag == 0) {
        var nIndex = oPILCMappings.length;
        $("#tblExportPILCMappings").datagrid("appendRow", oTempPILCMapping);
        $("#tblExportPILCMappings").datagrid("selectRow", nIndex);
    }
}
function PickPIExportLC(oreturnObj)
{
    
    var oTempPILCMapping = oreturnObj;
    var oExportLC = RefreshObjectExportLC();
    if (_oExportLC == null || parseInt(_oExportLC.ExportLCID) <= 0) {

        if (oExportLC.ApplicantID <= 0) {
            oExportLC.ApplicantID = oTempPILCMapping.ContractorID;
        }
        AppendPIwithLC(oTempPILCMapping);
       
    }
    else {
        oTempPILCMapping.ExportLCID = _oExportLC.ExportLCID;
        oTempPILCMapping.ExportLC = _oExportLC.ExportLC;

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/ExportLC/Save_ExportPILCMapping",
            traditional: true,
            data: JSON.stringify(oTempPILCMapping),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oPILCMapping = jQuery.parseJSON(data);
                if (parseInt(oPILCMapping.ExportPILCMappingID) > 0) {
                    
                    AppendPIwithLC(oPILCMapping);
                  
                    if (oPILCMapping.ExportLC != null) {
                        if (oPILCMapping.ExportLC.ExportLCID > 0) {
                            _oExportLC = oPILCMapping.ExportLC;
                            var oExportLCs = $("#tblExportLCs").datagrid("getRows");
                            var nIndex = oExportLCs.length;
                            $("#tblExportLCs").datagrid("appendRow", oPILCMapping.ExportLC);
                            $("#tblExportLCs").datagrid("selectRow", nIndex);
                        }
                    }
                    RefreshTotalSummery_PI();
                    SetAllFieldsValueAccordingToExportPI(oPILCMapping.ExportPIID);
                }
                else {
                    alert(oPILCMapping.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    }
}

function PickBankBranch_ELC() {
    var oBankBranch = {
        BUID: _nBUID,
        DeptIDs: '5',//EnumOperationalDept : Export_Party=5
        BankName: $.trim($("#txtSearchByBankBranch_Issue").val())
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oBankBranch,
        ControllerName: "BankBranch",
        ActionName: "GetsBankBranchSearchByBankName",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblBankBranchsPicker");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].BankBranchID > 0) {
                var tblColums = []; var oColumn = { field: "BankName", title: "Bank Name", width: 200, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "BranchName", title: "Branch Name", width: 280, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Address", title: "Address", width: 280, align: "left" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winBankBranch',
                    winclass: 'clsBankBranch',
                    winwidth: 700,
                    winheight: 460,
                    tableid: 'tblBankBranch',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'BankName',
                    windowTittle: 'Bank Branches'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);
            }
            else {
                alert(response.objs[0].ErrorMessage);
            }
        }
        else {
            alert("Sorry, No Bank Found.");
        }
    });
}

function PickExportPI_ELC() {
    //if (!ValidateInputExportLC(true)) return;

    var oExportPI = {
        PINo: $.trim($("#txtPINo").val()),
        ContractorID: _oExportLC.ApplicantID,
        LCID: _oExportLC.ExportLCID,
        BUID: _nBUID
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oExportPI,
        ControllerName: "ExportLC",
        ActionName: "GetsExportPI",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblExportPIPicker");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ExportPIID > 0) {
                var tblColums = []; var oColumn = { field: "PINo_Full", title: "PI No", width: 190, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "ContractorName", title: "Party", width: 200, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "BankName", title: "Bank", width: 120, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "LCTermsName", title: "LC Terms", width: 120, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "AmountSt", title: "Amount", width: 100, align: "right" }; tblColums.push(oColumn);
                var oPickerParam = {
                    winid: 'winExportPI',
                    winclass: 'clsExportPI',
                    winwidth: 800,
                    winheight: 460,
                    tableid: 'tblExportPI',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'PIFullNo',
                    windowTittle: 'PI(s) Search'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);
            }
            else {
                alert(response.objs[0].ErrorMessage);
            }
        }
        else {
            alert("Sorry, No PI Found.");
        }
    });
}

function PickMasterLC_ELC() {
    if (_oExportLC.ExportLCID <= 0 || _oExportLC.ExportLCID == null) {
        alert("Please, First save L/C");
        return;
    }

    var MasterLC = {
        MasterLCNo: $.trim($("#txtMasterLCSearch").val()),
        ContractorID: _oExportLC.ApplicantID
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: MasterLC,
        ControllerName: "MasterLC",
        ActionName: "GetsMasterLC",
        IsWinClose: false
    };
    DynamicRefreshList([], "tblMasterLCPicker");
    $.icsDataGets(obj, function (response) {
        if (response.status && response.objs.length > 0) {
            _oMasterLC = [];
            if (response.objs[0].MasterLCID > 0) {
                _oMasterLC = response.objs;
            }
            var tblColums = [];
            var oColumn = { field: "MasterLCNo", title: "LC No", width: 120, align: "left" }; tblColums.push(oColumn);
            oColumn = { field: "MasterLCDateSt", title: "Date", width: 120, align: "center" }; tblColums.push(oColumn);
            oColumn = { field: "MasterLCTypeSt", title: "Type", width: 120, align: "left" }; tblColums.push(oColumn);
            var oPickerParam = {
                winid: 'winMasterLC_ELC',
                winclass: 'clsMasterLC_ELC',
                winwidth: 380,
                winheight: 460,
                tableid: 'tblMasterLCPicker',
                tablecolumns: tblColums,
                datalist: _oMasterLC,
                multiplereturn: false,
                searchingbyfieldName: 'MasterLCNo',
                windowTittle: 'Master LC Search'
            };
            $.icsPicker(oPickerParam);
            $("#winMasterLC_ELC").find("#txtSearch").width(118);
            $("#winMasterLC_ELC").find("#txtSearch").parent().append('<input id="btnAddMasterLC" type="button" style="width:100px" value="Add"/>');
            $("#winMasterLC_ELC").find("#txtSearch").parent().append('<input id="btnDeleteMasterLC" style="width:100px" type="button" value="Delete"/>');

            $("#winMasterLC_ELC #btnAddMasterLC").click(function () {
                $('#chkMaskerLC').prop('checked', true);
                $('#chkExportLC').prop('checked', false);
                $("#txtMasterLCNo,#txtCountry,#txtProductDesc,#txtRemark").val("");
                $("#winAddMasterLC").find(".easyui-datebox").datebox("setValue", ""); //icsdateformat(new Date())
                $("#winAddMasterLC").icsWindow('open', "Add LC");
                _oMasterLC = null;
            });

            $("#winMasterLC_ELC #btnDeleteMasterLC").click(function () {
                var oMasterLC = $("#tblMasterLCPicker").datagrid("getSelected");
                if (oMasterLC == null || oMasterLC.MasterLCID <= 0) {
                    alert("Please select an item from list.");
                    return false;
                }
                if (!confirm("Confirm to Delete?")) return false;

                var obj =
                {
                    BaseAddress: _sBaseAddress,
                    Object: oMasterLC,
                    ObjectId: oMasterLC.MasterLCID,
                    ControllerName: "MasterLC",
                    ActionName: "DeleteMasterLC",
                    TableId: "tblMasterLCPicker",
                    IsWinClose: false
                };
                $.icsDelete(obj);
            });
            IntializePickerbutton(oPickerParam);
        }
    });

    $("#chkMaskerLC").change(function () {
        if (this.checked) {
            $(".lblLCType").text("Master");
            $('#chkExportLC').prop('checked', false);
        } else {
            $(".lblLCType").text("");
            $('#chkExportLC').prop('checked', true);
        }
    });

    $("#chkExportLC").change(function () {
        if (this.checked) {
            $(".lblLCType").text("");
            $('#chkMaskerLC').prop('checked', false);
        } else {
            $(".lblLCType").text("Master");
            $('#chkMaskerLC').prop('checked', true);
        }
    });

}

function IsExists(oPILCMapping) {
    var oPILCMappings = $('#tblExportPILCMappings').datagrid('getRows');
    for (var i = 0; i < oPILCMappings.length; i++) {
        if (parseInt(oPILCMappings[i].ExportPIID) == parseInt(oPILCMapping.ExportPIID)) {
            return true;
        }
    }
    return false;
}

function IsExists_ELC(oExportLC) {
    var oExportLCs = $('#tblExportPILCMappings').datagrid('getRows');
    for (var i = 0; i < oExportLCs.length; i++) {
        if (parseInt(oExportLCs[i].ExportLCID) == parseInt(oExportLC.ExportLCID)) {
            return true;
        }
    }
    return false;
}

function CalculateTotalEPIDetail()
{
    
    var nTotalAmount = 0;
    var oExportPIDetails = $("#tblExportPILCMappings").datagrid("getRows");
    for (var i = 0; i < oExportPIDetails.length; i++) {
        nTotalAmount = parseFloat(nTotalAmount) + parseFloat(oExportPIDetails[i].Amount);
    }
    var sCurrency = $("#cboCurrency option:selected").text();
    $("#lblCurrency").text((sCurrency == "--Select One--" ? "" : sCurrency) + " ");
    $("#lblTotalValue_LC").text(formatPrice(nTotalAmount, 2));
    $("#txtAmount").val((sCurrency == "--Select One--" ? "" : sCurrency) + " " + formatPrice(nTotalAmount));
}

function RefreshObjectExportLC() {

    var nTotalAmount = 0;
    var oExportPIDetails = $("#tblExportPILCMappings").datagrid("getRows");
    for (var i = 0; i < oExportPIDetails.length; i++)
    {
        nTotalAmount = parseFloat(nTotalAmount) + parseFloat(oExportPIDetails[i].Amount);
    }
    $("#txtAmount").val($("#cboCurrency option:selected").text() + " " + nTotalAmount);
    $("#lblTotalValue_LC").text(nTotalAmount);

    var bIsLIBORRate =false; if( $('#cboLiborRate').val()==1){ bIsLIBORRate=true;}
    var bIsBBankFDD = false; if ($('#cboBankFDD').val() == 1) { bIsBBankFDD = true; }
    var oExportLC = {
        ExportLCID: (_oExportLC != null) ? _oExportLC.ExportLCID : 0,
        ExportLCNo: $("#txtExportLCNo").val(),
        NegoDays : parseInt($("#txtNegoDays").val()),
        FileNo: $.trim($("#txtFileNo").val()),
        OpeningDate: $('#txtOpeningDate').datebox('getValue'),
        BBranchID_Advice: parseInt($("#cboBankBranch_Advice").val()),
        BBranchID_Nego: parseInt($("#cboBankBranch_Nego").val()),
        BBranchID_Issue: _oExportLC.BBranchID_Issue,
        ApplicantID: _oExportLC.ApplicantID,
        DeliveryToID: _oExportLC.DeliveryToID,
        CurrencyID: parseInt($("#cboCurrency").val()),
        ShipmentDate: $('#txtShipmentDate').datebox('getValue'),
        ExpiryDate: $('#txtExpireDate').datebox('getValue'),
        LCRecivedDate: $('#txtReceiveDate').datebox('getValue'),
        Remark: $("#txtNote").val(),
        HSCode: $.trim($("#txtHSCode").val()),
        AreaCode: $.trim($("#txtAreaCode").val()),
        LiborRate: $("#chkLIBORRateYes").is(":checked"),
        BBankFDD: $("#chkBankFDDYes").is(":checked"),
        OverDueRate: $('#txtOverDueRate').numberbox('getValue'),
        IRC: $("#txtIRC").val(),
        ERC: $("#txtERC").val(),
        FrightPrepaid: $("#txtFrightPrepaid").val(),
        DCharge: $('#txtDiscrepancyCharge').numberbox('getValue'),
        LCTramsID: parseInt($("#cboLCTerm").val()),
        IsForeignBank: $("#chkIsBFDD_Foreign").is(":checked"),
        TransShipmentAllowed: $("#chkTransshipmentAllow").is(":checked"),
        Amount: parseFloat(nTotalAmount),//_oExportLC.Amount,
        PartialShipmentAllowed: $("#chkPartialShipmentAllow").is(":checked"),
        ShipmentFrom: $("#txtShipmentFrom").val(),
        ExportPILCMappings: $('#tblExportPILCMappings').datagrid('getRows'),
        GarmentsQty: $("#txtGarmentsQty").val(),
        AmendmentDate: $('#txtAmendmentDate').datebox('getValue'),
        BUID: _nBUID,
        BankBranchID_Forwarding: parseInt($("#cboBankBranch_ForwardingBank").val()),
        PaymentInstruction: $("#cboPaymentInstruction").val()
    };
    return oExportLC;
}


function RefreshObjectExportLC_TnC() {
    var nTotalAmount = 0;
    var oExportPIDetails = $("#tblExportPILCMappings").datagrid("getRows");
    for (var i = 0; i < oExportPIDetails.length; i++) {
        nTotalAmount = parseFloat(nTotalAmount) + parseFloat(oExportPIDetails[i].Amount);
    }
    $("#txtAmount").val($("#cboCurrency option:selected").text() + " " + nTotalAmount);
    $("#lblTotalValue_LC").text(nTotalAmount);

    var oExportLC = {
        ExportLCID: (_oExportLC != null) ? _oExportLC.ExportLCID : 0,
        ExportLCNo: $("#txtExportLCNo").val(),
        NegoDays: parseInt($("#txtNegoDays").val()),
        FileNo: _oExportLC.FileNo,
        OpeningDate: $('#txtOpeningDate').datebox('getValue'),
        BBranchID_Advice: parseInt($("#cboBankBranch_Advice").val()),
        BBranchID_Nego: parseInt($("#cboBankBranch_Nego").val()),
        BBranchID_Issue: _oExportLC.BBranchID_Issue,
        ApplicantID: _oExportLC.ApplicantID,
        DeliveryToID : _oExportLC.DeliveryToID,
        CurrencyID: parseInt($("#cboCurrency").val()),
        ShipmentDate: $('#txtShipmentDate').datebox('getValue'),
        ExpiryDate: $('#txtExpireDate').datebox('getValue'),
        LCRecivedDate: $('#txtReceiveDate').datebox('getValue'),
        Remark: $("#txtNote").val(),
        HSCode: $.trim($("#txtHSCode").val()),
        AreaCode: $.trim($("#txtAreaCode").val()),
        LiborRate: $("#chkLIBORRateYes").is(":checked"),
        BBankFDD: $("#chkBankFDDYes").is(":checked"),
        OverDueRate: $('#txtOverDueRate').numberbox('getValue'),
        IRC: $("#txtIRC").val(),
        ERC: $("#txtERC").val(),
        FrightPrepaid: $("#txtFrightPrepaid").val(),
        DCharge: $('#txtDiscrepancyCharge').numberbox('getValue'),
        LCTramsID: parseInt($("#cboLCTerm").val()),
        IsForeignBank: $("#chkIsBFDD_Foreign").is(":checked"),
        TransShipmentAllowed: $("#chkTransshipmentAllow").is(":checked"),
        Amount: parseFloat(nTotalAmount),//_oExportLC.Amount,
        PartialShipmentAllowed: $("#chkPartialShipmentAllow").is(":checked"),
        ShipmentFrom: $("#txtShipmentFrom").val(),
        GarmentsQty: $("#txtGarmentsQty").val(),
        AmendmentDate: $('#txtAmendmentDate').datebox('getValue'),
        BUID: _nBUID,
        BankBranchID_Forwarding: parseInt($("#cboBankBranch_ForwardingBank").val()),
        PaymentInstruction: $("#cboPaymentInstruction").val()
    };
    return oExportLC;
}

function ValidateInputExportLC(bIsDetailAdd) {

 

    if (!$.trim($('#txtExportLCNo').val()).length) {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        alert("Please enter L/C No.");
        $('#txtExportLCNo').focus();
        $("#txtExportLCNo").addClass("errorFieldBorder");
        return false;
    } else { $('#txtExportLCNo').removeClass("errorFieldBorder"); }
    

    //if (_oExportLC.ApplicantID <= 0) {
    //    $('#tabExportLCTabsExportLC').tabs('select', 0);
    //    alert("Please select Applicant Name!");
    //    $('#txtContractorName').focus();
    //    $("#txtContractorName").addClass("errorFieldBorder");
    //    return false;
    //} else { $("#txtContractorName").removeClass("errorFieldBorder"); }


    if ($('#txtOpeningDate').datebox('getValue') == null || $('#txtOpeningDate').datebox('getValue') == "") {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        alert("Please give LC Open Date!"); $('#txtOpeningDate').focus();
        return false;
    } 

    if ($('#txtReceiveDate').datebox('getValue') == null || $('#txtReceiveDate').datebox('getValue') == "") {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        alert("Please give Receive Date!"); $('#txtReceiveDate').focus();
        return false;
    }

    if ($('#txtShipmentDate').datebox('getValue') == null || $('#txtShipmentDate').datebox('getValue') == "") {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        alert("Please give Shipment Date!"); $('#txtShipmentDate').focus();
        return false;
    }

    if ($('#txtExpireDate').datebox('getValue') == null || $('#txtExpireDate').datebox('getValue') == "") {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        alert("Please give Expire Date!"); $('#txtExpireDate').focus();
        return false;
    }

    if ($("#cboBenificiary").val() <= 0) {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        alert("Please select Benificiary!"); $('#cboCurrency').focus();
        $("#cboBenificiary").addClass("errorFieldBorder");
        return false;
    } else { $("#cboBenificiary").removeClass("errorFieldBorder"); }

    if (_oExportLC.BBranchID_Issue <= 0) {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        alert("Please select Issue Bank!");
        $('#txtSearchByBankBranch_Issue').focus();
        $("#txtSearchByBankBranch_Issue").addClass("errorFieldBorder");
        return false;
    } else { $("#txtSearchByBankBranch_Issue").removeClass("errorFieldBorder"); }

    //if ($("#cboBankBranch_Advice").val() <= 0) {
    //    $('#tabExportLCTabsExportLC').tabs('select', 0);
    //    alert("Please select Advice Bank Name!"); $('#cboBankBranch_Advice').focus();
    //    $("#cboBankBranch_Advice").addClass("errorFieldBorder");
    //    return false;
    //} else {
    //    $("#cboBankBranch_Advice").removeClass("errorFieldBorder");
    //}

    //if ($("#cboCurrency").val() <= 0) {
    //    $('#tabExportLCTabsExportLC').tabs('select', 0);
    //    alert("Please select Currency!"); $('#cboCurrency').focus();
    //    $("#cboCurrency").addClass("errorFieldBorder");
    //    return false;
    //} else { $("#cboCurrency").removeClass("errorFieldBorder"); }

    if ($("#cboBankBranch_Nego").val() <= 0) {
        $('#tabExportLCTabsExportLC').tabs('select', 0);
        alert("Please select Negoatiation Bank Name!"); $('#cboBankBranch_Nego').focus();
        $("#cboBankBranch_Nego").addClass("errorFieldBorder");
        return false;
    } else {
        $("#cboBankBranch_Nego").removeClass("errorFieldBorder");
    }

    //if ($('#txtOpeningDate').datebox('getValue') == null || $('#txtOpeningDate').datebox('getValue') == "") {
    //    $('#tabExportLCTabsExportLC').tabs('select', 0);
    //    alert("Please select Issue Date!"); $('#txtOpeningDate').focus();
    //    $('#txtOpeningDate').css("border", "1px solid #c00");
    //    $("#txtOpeningDate").addClass("errorFieldBorder");
    //    return false;
    //} else {
    //    $("#txtOpeningDate").removeClass("errorFieldBorder");
    //}

    //if ($('#txtReceiveDate').datebox('getValue') == null || $('#txtReceiveDate').datebox('getValue') == "") {
    //    $('#tabExportLCTabsExportLC').tabs('select', 0);
    //    alert("Please select Receive Date!"); $('#txtReceiveDate').focus();
    //    $("#txtReceiveDate").addClass("errorFieldBorder");
    //    return false;
    //} else { $("#txtReceiveDate").removeClass("errorFieldBorder"); }

   

   
   

    //if (bIsDetailAdd == false) {
    //    if ($('#txtLCOpenDate').datebox('getValue') == null || $('#txtLCOpenDate').datebox('getValue') == "") {
    //        $('#tabExportLCTabs').tabs('select', 1);
    //        alert("Please select LC open Date!"); $('#txtLCOpenDate').focus();
    //        $('#txtLCOpenDate').css("border", "1px solid #c00");
    //        return false;
    //    } else { $('#txtLCOpenDate').css("border", ""); }

    //    if ($('#txtDeliveryDate').datebox('getValue') == null || $('#txtDeliveryDate').datebox('getValue') == "") {
    //        $('#tabExportLCTabs').tabs('select', 1);
    //        alert("Please select Delivery Date!"); $('#txtDeliveryDate').focus();
    //        $('#txtDeliveryDate').css("border", "1px solid #c00");
    //        return false;
    //    } else { $('#txtDeliveryDate').css("border", ""); }

    //    if ($("#cboLCTerm").val() <= 0) {
    //        $('#tabExportLCTabs').tabs('select', 1);
    //        alert("Please select LC Term!"); $('#cboLCTerm').focus();
    //        $('#cboLCTerm').css("border", "1px solid #c00");
    //        return false;
    //    } else { $('#cboLCTerm').css("border", ""); }
    //}
    
    return true;
}

function IsLIBORateYes() {
    document.getElementById("chkLIBORRateYes").checked = true;
    document.getElementById("chkLIBORRateNo").checked = false;
}

function IsLIBORateNo() {
    document.getElementById("chkLIBORRateYes").checked = false;
    document.getElementById("chkLIBORRateNo").checked = true;
}

function IsTransshipmentAllow() {
    document.getElementById("chkTransshipmentAllow").checked = true;
    document.getElementById("chkTransshipmentNotAllow").checked = false;
}

function IsTransshipmentNotAllow() {
    document.getElementById("chkTransshipmentAllow").checked = false;
    document.getElementById("chkTransshipmentNotAllow").checked = true;
}

function IsPShipmentAllow() {
    document.getElementById("chkPartialShipmentAllow").checked = true;
    document.getElementById("chkPartialShipmentNotAllow").checked = false;
}

function IsPShipmentNotAllow() {
    document.getElementById("chkPartialShipmentAllow").checked = false;
    document.getElementById("chkPartialShipmentNotAllow").checked = true;
}

function IsBFDD_Local() {
    document.getElementById("chkIsBFDD_Local").checked = true;
    document.getElementById("chkIsBFDD_Foreign").checked = false;
}

function IsDFDD_Foreign() {
    document.getElementById("chkIsBFDD_Local").checked = false;
    document.getElementById("chkIsBFDD_Foreign").checked = true;
}

function IsBankFDDYes() {
    document.getElementById("chkBankFDDYes").checked = true;
    document.getElementById("chkBankFDDNo").checked = false;
}

function IsBankFDDNo() {
    document.getElementById("chkBankFDDYes").checked = false;
    document.getElementById("chkBankFDDNo").checked = true;
}

function RefreshTotalSummery_PI() {
    var nTotalQty = 0;
    var nTotalValue = 0;
    var sCurrency = "";
    var oExportPILCMappings = $('#tblExportPILCMappings').datagrid('getRows');
    for (var i = 0; i < oExportPILCMappings.length; i++) {
        nTotalQty = 0;
        nTotalValue = nTotalValue + parseFloat(oExportPILCMappings[i].Amount);
        sCurrency = oExportPILCMappings[i].Currency;
    }
  
    document.getElementById("lblTotalValue_LC").innerHTML = sCurrency + " " + formatPrice(nTotalValue);
    $("#txtAmount").val(sCurrency + " " + formatPrice(nTotalValue));
}

