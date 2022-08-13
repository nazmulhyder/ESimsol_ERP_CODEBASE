var _oIsAdd = false;

var _nCheckReceiveOrSubmitSave = 0;
var _nContractorID = 0;
var _bValidation = false;
function InitializeFabricEvents() {
    debugger;
    LoadComboboxesFabric();
    $('.regionFabricStage').hide();
    $('.numberField, .numberFormat').icsNumberField({ min: 0, precision: 0 });
    $("#txtCopyNo").val(1);
    $("#btnCopyFabric").hide();
    $("#txtDateFabric").datebox("setValue", icsdateformat(new Date()));
    $("#txtFromDateAdvSearch,#txtToDateAdvSearch,#txtReceiveDateFabric,#txtSubDateFabric").datebox({ disabled: false });
    $("#txtFromDateAdvSearch,#txtToDateAdvSearch,#txtReceiveDateFabric,#txtSubDateFabric").datebox("setValue", icsdateformat(new Date()));


    $("#txtFabricNoFabric").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $("#btnSaveFabric,#btnCopyFabric").click(function () {
        debugger;
        var nCount = ($("#txtCopyNo").val() == undefined) ? 1 : $("#txtCopyNo").val();
        if (!ValidateInputFabric()) return;
        $("#btnSaveFabric,#btnCopyFabric").hide();
        if (nCount <= 1) {
            debugger;
            var oFabric = RefreshObjectFabric();
            //oFabric.FabricSeekingDates = RefreshObject_FabricSeekingDates();
            if (_bValidation) return;
            $("#progressbar").progressbar({ value: 0 });
            $("#progressbarParent").show();
            var intervalID = setInterval(updateProgress, 250);

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/Fabric/Save",
                traditional: true,
                data: JSON.stringify(oFabric),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var oFabric = jQuery.parseJSON(data);
                    clearInterval(intervalID);
                    $("#progressbarParent").hide();
                    if (oFabric.FabricID > 0) {

                        if ($("#btnSaveFabric").data('FabricSC') == null || !$("#btnSaveFabric").data('FabricSC')) {
                            if (_oIsAdd == true) {
                                var oFabrics = $('#tblFabrics').datagrid('getRows');
                                var nIndex = oFabrics.length;
                                $("#tblFabrics").datagrid("appendRow", oFabric);
                                $('#tblFabrics').datagrid('selectRow', nIndex);
                                _oIsAdd = false;
                            }
                            else {
                                $('#tblFabrics').datagrid('updateRow', { index: _nSelectedRowIndex, row: oFabric });
                                $('#tblFabrics').datagrid('selectRow', _nSelectedRowIndex);
                            }
                        }
                        else {
                            alert("Save Successfully.")
                        }
                       
                        $("#btnSaveFabric").show();
                        $("#winFabric").icsWindow('close');
                        $(".wininputfieldstyle input").val("");

                    }
                    else {
                        alert(oFabric.ErrorMessage);
                        $("#btnSaveFabric").show();
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });

            //var obj = {
            //    BaseAddress: _sBaseAddress,
            //    Object: oFabric,
            //    ObjectId: oFabric.FabricID,
            //    ControllerName: "Fabric",
            //    ActionName: "Save",
            //    TableId: "tblFabrics",
            //    IsWinClose: true,
            //    Message: ($(this).attr("id") == "btnSaveFabric" ? "Succesfully saved" : "Succesfully copied")
            //};
            //$.icsSave(obj);
            //clearInterval(intervalID);
            //$("#progressbarParent").hide();
            //$("#btnSaveFabric,#btnCopyFabric").show();
        }
        else {
            var oFabrics = [];
            for (var i = 0; i < nCount; i++) {
                // if (!ValidateInputFabric()) return;
                var oFabric = RefreshObjectFabric();
                oFabric.FabricID = 0;
                oFabric.FabricNo = "";
                //oFabric.FabricSeekingDates = RefreshObject_FabricSeekingDates();
                if (_bValidation) return;
                oFabrics.push(oFabric);
            }
            //$("#progressbar").progressbar({ value: 0 });
            //$("#progressbarParent").show();
            //var intervalID = setInterval(updateProgress, 250);

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/Fabric/SaveAll",
                traditional: true,
                data: JSON.stringify(oFabrics),
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    var oFabrics = jQuery.parseJSON(data);
                    var oFabricsTwo = $('#tblFabrics').datagrid('getRows');
                    var nIndex = oFabricsTwo.length;
                    nIndex = nIndex - 1;
                    //clearInterval(intervalID);
                    //$("#progressbarParent").hide();
                    if (oFabrics[0].ErrorMessage == '' || oFabrics[0].ErrorMessage == null) {

                        for (i = 0; i < oFabrics.length; ++i) {
                            debugger;
                            nIndex = nIndex + 1;
                            //oFabrics[i].IsOparate=-1;
                            $("#tblFabrics").datagrid("appendRow", oFabrics[i]);
                            $('#tblFabrics').datagrid('selectRow', nIndex);
                        }
                        alert("Data Save Successfully");

                    }
                    else {
                        alert(oFabrics[0].ErrorMessage);
                    }


                    //var oFabric = jQuery.parseJSON(data);
                    //clearInterval(intervalID);
                    //$("#progressbarParent").hide();
                    //if (oFabric.ErrorMessage == "" || oFabric.ErrorMessage == null)
                    //{
                    //    $("#tblFabrics").datagrid("appendRow", oFabric);
                    //    var nIndex=oFabrics.length;
                    //    alert("aaa");
                    //    var oFabrics= $('#tblFabrics').datagrid('getRows');
                    //    $('#tblFabrics').datagrid('selectRow', nIndex);
                    //}
                    //else {
                    //    alert(oFabric.ErrorMessage);
                    //}
                    $("#btnSaveFabric,#btnCopyFabric").show();
                    $("#winFabric").icsWindow('close');
                    $(".wininputfieldstyle input").val("");
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });


        }
    });

    $("#btnCloseFabric").click(function () {
        $("#winFabric").icsWindow('close');
        $(".wininputfieldstyle input").val("");
        $("#chkIsOwnFabric").prop("checked", false);
    });

    $("#btnReFabricSubmissionFabric").click(function () {
        var oFabric = $("#tblFabrics").datagrid("getSelected");
        if (oFabric == null || oFabric.FabricID <= 0) { alert("Please select an item from list!"); return; }
        $("#winFabric").icsWindow('open', "Re Fabric Submission");
        _oFabric = null;
        RefreshFabricLayout("btnAddFabric");
        $("#btnSaveFabric,#txtCopyNo,#lblGiveInfo").hide();
        $("#txtHLNo").prop("disabled", true);
        $("#btnReFabricSubmissionSave").show();
        $('#cboMktPersonsFabric').focus();
        GetFabricInformation(oFabric);
        UnselectAllRowsOfATable();
    });

    $("#btnReFabricSubmissionSave").click(function () {

        debugger;
        if (!ValidateInputFabric()) return;
       
        var oFabric = RefreshObjectFabric();
        oFabric.ApprovedBy = 0;
        oFabric.IssueDate = icsdateformat(new Date());
        oFabric.SeekingSubmissionDate = icsdateformat(new Date());
        oFabric.SubmissionDate = icsdateformat(new Date());
        oFabric.ReceiveDate = icsdateformat(new Date());
        oFabric.FabricSeekingDates = RefreshObject_FabricSeekingDates();
        debugger;
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/Fabric/ReFabricSubmission",
            traditional: true,
            data: JSON.stringify(oFabric),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oFabric = jQuery.parseJSON(data);
                if (oFabric.ErrorMessage == "") {
                    if (oFabric.FabricID > 0) {
                        alert("Re Febric Submit Successful.");
                        $("#winFabric").icsWindow("close");
                        var oFabrics = $("#tblFabrics").datagrid("getRows");
                        var nIndex = oFabrics.length;
                        $("#tblFabrics").datagrid("appendRow", oFabric);
                        $("#tblFabrics").datagrid("selectRow", nIndex);
                    }
                }
                else {
                    alert(oFabric.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    });

    $("#btnResetContractor").click(function () {
        $("#txtContractorName").val("");
        _nContractorID = 0;
    });

    $("#btnContractor").click(function () {

        var sContractorName = $.trim($("#txtContractorName").val());
        /// 2 Buyer
        var oContractor = {
            Params: '2' + '~' + sContractorName,
        };
        GetContractors(oContractor);
    });
    $("#txtContractorName").keydown(function (e) {
        var nkeyCode = e.keyCode || e.which;
        if (nkeyCode == 13) {
            var sContractorName = $.trim($("#txtContractorName").val());

            if ($('#txtContractorName').val() == null || $('#txtContractorName').val() == "") {
                alert("Please Type Name or part Name and Press Enter.");
                $('#txtContractorName').focus();
                $("#txtContractorName").addClass("errorFieldBorder");
                return;
            }

            var oContractor = {
                Params: '2' + '~' + sContractorName,
            };
            GetContractors(oContractor);
        }
        else if (nkeyCode == 8) {
            $("#txtContractorName").val("");
            _nContractorID = 0;
            $("#cboCPIssueTo").empty();
        }
    });
}

function LoadComboboxesFabric() {

    debugger;
    //PickProductInFabric();
//    _oFabricOrderSetups
    $("#cboProductFabric").icsLoadCombo({
        List: _oProducts,
        OptionValue: "ProductID",
        DisplayText: "ProductName"
    });
    $("#cboFabricOrderType").icsLoadCombo({
        List: _oFabricOrderSetups,
        OptionValue: "FabricOrderType",
        DisplayText: "OrderName"
    });
    $("#cboFinishTypeFabric").icsLoadCombo({
        List: _oFinishTypes,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });
    $("#cboMktPersonsFabric").icsLoadCombo({
        List: _oMktPersons,
        OptionValue: "MarketingAccountID",
        DisplayText: "Name_Group"
    });

    $("#cboPriorityLevelFabric").icsLoadCombo({
        List: _oPriorityLevels,
        OptionValue: "Value",
        DisplayText: "Text"
    });

    //$("#cboSwatchTypeSubmit").icsLoadCombo({
    //    List: _oSwatchTypes,
    //    OptionValue: "id",
    //    DisplayText: "Value"
    //});

    $("#cboProcessTypeFabric").icsLoadCombo({
        List: _oProcessTypes,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

    $("#cboWeaveFabric").icsLoadCombo({
        List: _oFabricWeaves,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

    $("#cboFabricDesign").icsLoadCombo({
        List: _oFabricDesigns,
        OptionValue: "FabricProcessID",
        DisplayText: "Name"
    });

    $("#cboPrimaryLightSource,#cboSecondaryLightSource").icsLoadCombo({
        List: _oLightSources,
        OptionValue: "LightSourceID",
        DisplayText: "Descriptions"
    });
}

function RefreshCon_BuyerRequest(oFabric) {
    debugger;
    var oFabricSeekingDates = [];
    if (oFabric != null) {
        oFabricSeekingDates = oFabric.FabricSeekingDates;
    }

    $("#chkIsHandLoom").prop("checked", false);
    $("#chkIsSample").prop("checked", false);
    $("#chkIsBulk").prop("checked", false);
    $("#chkIsAnalysis").prop("checked", false);
    $("#chkIsCAD").prop("checked", false);
    $("#chkIsColor").prop("checked", false);
    $("#chkIsLabdip").prop("checked", false);
    $("#chkIsYarnSkein").prop("checked", false);
    $("#txtHandLoomDate").datebox("setValue", icsdateformat(new Date()));
    $("#txtSampleDate").datebox("setValue", icsdateformat(new Date()));
    $("#txtBulkDate").datebox("setValue", icsdateformat(new Date()));
    $("#txtAnalysis").datebox("setValue", icsdateformat(new Date()));
    $("#txtCADDate").datebox("setValue", icsdateformat(new Date()));
    $("#txtColorDate").datebox("setValue", icsdateformat(new Date()));
    $("#txtLabdipDate").datebox("setValue", icsdateformat(new Date()));
    $("#txtYarnSkein").datebox("setValue", icsdateformat(new Date()));
    $('.numberField').val('');
    for (var i = 0; i < oFabricSeekingDates.length; i++) {

        if (oFabricSeekingDates[i].FabricRequestTypeInt == 1) {
            $("#chkIsHandLoom").prop("checked", true);
            $('#txtHandLoomDate').datebox('setValue', oFabricSeekingDates[i].SeekingDateST);
            $('#txtHandLommSet').val(oFabricSeekingDates[i].NoOfSets);
        }

        if (oFabricSeekingDates[i].FabricRequestTypeInt == 2) {
            $("#chkIsSample").prop("checked", true);
            $('#txtSampleDate').datebox('setValue', oFabricSeekingDates[i].SeekingDateST);
            $('#txtSampleSet').val(oFabricSeekingDates[i].NoOfSets);
        }
        if (oFabricSeekingDates[i].FabricRequestTypeInt == 3) {
            $("#chkIsBulk").prop("checked", true);
            $('#txtBulkDate').datebox('setValue', oFabricSeekingDates[i].SeekingDateST);
            $('#txtBulkSet').val(oFabricSeekingDates[i].NoOfSets);
        }
        if (oFabricSeekingDates[i].FabricRequestTypeInt == 4) {
            $("#chkIsAnalysis").prop("checked", true);
            $('#txtAnalysis').datebox('setValue', oFabricSeekingDates[i].SeekingDateST);
            $('#txtAnalysisSet').val(oFabricSeekingDates[i].NoOfSets);
        }


        if (oFabricSeekingDates[i].FabricRequestTypeInt == 5) {
            $("#chkIsCAD").prop("checked", true);
            $('#txtCADDate').datebox('setValue', oFabricSeekingDates[i].SeekingDateST);
            $('#txtCadSet').val(oFabricSeekingDates[i].NoOfSets);
        }
        if (oFabricSeekingDates[i].FabricRequestTypeInt == 6) {
            $("#chkIsColor").prop("checked", true);
            $('#txtColorDate').datebox('setValue', oFabricSeekingDates[i].SeekingDateST);
            $('#txtColorSet').val(oFabricSeekingDates[i].NoOfSets);
        }
        if (oFabricSeekingDates[i].FabricRequestTypeInt == 7) {
            $("#chkIsLabdip").prop("checked", true);
            $('#txtLabdipDate').datebox('setValue', oFabricSeekingDates[i].SeekingDateST);
            $('#txtLabdipSet').val(oFabricSeekingDates[i].NoOfSets);
        }
        if (oFabricSeekingDates[i].FabricRequestTypeInt == 8) {
            $("#chkIsYarnSkein").prop("checked", true);
            $('#txtYarnSkein').datebox('setValue', oFabricSeekingDates[i].SeekingDateST);
            $('#txtYarnSkeinSet').val(oFabricSeekingDates[i].NoOfSets);
        }

    }

}

function RefreshObject_FabricSeekingDates() {
    debugger;
    _bValidation = false;
    var oFabricSeekingDates = [];
    var dIssueDate = $('#txtDateFabric').datebox('getValue');

    var bIstrue = false;
    bIstrue = $("#chkIsHandLoom").is(":checked");

    if (bIstrue == true) {
        if (new Date($('#txtHandLoomDate').datebox('getValue')) == "Invalid Date") {
            alert("Please give Hand Loom Seeking Date!");
            _bValidation = true;
            return false;
        }
        var dSeekingDateFabric = $('#txtHandLoomDate').datebox('getValue');
        if (new Date(dSeekingDateFabric) < new Date(dIssueDate)) {
            alert("Hand Loom Date Date must greater than issue date.");
            _bValidation = true;
            return false;
        }
        var oFabricSeekingDate = { FabricRequestTypeInt: 1, SeekingDate: $('#txtHandLoomDate').datebox('getValue'), NoOfSets: $('#txtHandLommSet').val() };
        oFabricSeekingDates.push(oFabricSeekingDate);
    }
    bIstrue = $("#chkIsSample").is(":checked")
    if (bIstrue == true) {
        if (new Date($('#txtSampleDate').datebox('getValue')) == "Invalid Date") {
            alert("Please give Sample Seeking Date!");
            _bValidation = true;
            return false;
        }
        var dSeekingDateFabric = $('#txtSampleDate').datebox('getValue');
        if (new Date(dSeekingDateFabric) < new Date(dIssueDate)) {
            alert("Hand Sample Date Date must greater than issue date.");
            _bValidation = true;
            return false;
        }
        var oFabricSeekingDate = { FabricRequestTypeInt: 2, SeekingDate: $('#txtSampleDate').datebox('getValue'), NoOfSets: $('#txtSampleSet').val() };
        oFabricSeekingDates.push(oFabricSeekingDate);
    }

    bIstrue = $("#chkIsBulk").is(":checked")
    if (bIstrue == true) {
        if (new Date($('#txtBulkDate').datebox('getValue')) == "Invalid Date") {
            alert("Please give Bulk Seeking Date!");
            _bValidation = true;
            return false;
        }
        var dSeekingDateFabric = $('#txtBulkDate').datebox('getValue');
        if (new Date(dSeekingDateFabric) < new Date(dIssueDate)) {
            alert("Hand Bulk  Seeking Date must greater than issue date.");
            _bValidation = true;
            return false;
        }
        var oFabricSeekingDate = { FabricRequestTypeInt: 3, SeekingDate: $('#txtBulkDate').datebox('getValue'), NoOfSets: $('#txtBulkSet').val() };
        oFabricSeekingDates.push(oFabricSeekingDate);
    }

    bIstrue = $("#chkIsAnalysis").is(":checked")
    if (bIstrue == true) {
        if (new Date($('#txtAnalysis').datebox('getValue')) == "Invalid Date") {
            alert("Please give Analysis seeking Date!");
            _bValidation = true;
            return false;
        }
        var dSeekingDateFabric = $('#txtAnalysis').datebox('getValue');
        if (new Date(dSeekingDateFabric) < new Date(dIssueDate)) {
            alert("Hand Analysis Seeking Date must greater than issue date.");
            _bValidation = true;
            return false;
        }
        var oFabricSeekingDate = { FabricRequestTypeInt: 4, SeekingDate: $('#txtAnalysis').datebox('getValue'), NoOfSets: $('#txtAnalysisSet').val() };
        oFabricSeekingDates.push(oFabricSeekingDate);
    }
    bIstrue = $("#chkIsCAD").is(":checked")
    if (bIstrue == true) {
        if (new Date($('#txtCADDate').datebox('getValue')) == "Invalid Date") {
            alert("Please give CAD seeking Date!");
            _bValidation = true;
            return false;
        }
        var dSeekingDateFabric = $('#txtCADDate').datebox('getValue');
        if (new Date(dSeekingDateFabric) < new Date(dIssueDate)) {
            alert("CAD Seeking Date must greater than issue date.");
            _bValidation = true;
            return false;
        }
        var oFabricSeekingDate = { FabricRequestTypeInt: 5, SeekingDate: $('#txtCADDate').datebox('getValue'), NoOfSets: $('#txtCadSet').val() };
        oFabricSeekingDates.push(oFabricSeekingDate);
    }
    bIstrue = $("#chkIsColor").is(":checked")
    if (bIstrue == true) {
        if (new Date($('#txtColorDate').datebox('getValue')) == "Invalid Date") {
            alert("Please give Color seeking Date!");
            _bValidation = true;
            return false;
        }
        var dSeekingDateFabric = $('#txtColorDate').datebox('getValue');
        if (new Date(dSeekingDateFabric) < new Date(dIssueDate)) {
            alert("Color Seeking Date must greater than issue date.");
            _bValidation = true;
            return false;
        }
        var oFabricSeekingDate = { FabricRequestTypeInt: 6, SeekingDate: $('#txtColorDate').datebox('getValue'), NoOfSets: $('#txtColorSet').val() };
        oFabricSeekingDates.push(oFabricSeekingDate);
    }
    bIstrue = $("#chkIsLabdip").is(":checked")
    if (bIstrue == true) {
        if (new Date($('#txtLabdipDate').datebox('getValue')) == "Invalid Date") {
            alert("Please give Labdip seeking Date!");
            _bValidation = true;
            return false;
        }
        var dSeekingDateFabric = $('#txtLabdipDate').datebox('getValue');
        if (new Date(dSeekingDateFabric) < new Date(dIssueDate)) {
            alert("Lab dip Seeking Date must greater than issue date.");
            _bValidation = true;
            return false;
        }
        var oFabricSeekingDate = { FabricRequestTypeInt: 7, SeekingDate: $('#txtLabdipDate').datebox('getValue'), NoOfSets: $('#txtLabdipSet').val() };
        oFabricSeekingDates.push(oFabricSeekingDate);
    }
    bIstrue = $("#chkIsYarnSkein").is(":checked")
    if (bIstrue == true) {
        if (new Date($('#txtYarnSkein').datebox('getValue')) == "Invalid Date") {
            alert("Please give YarnSkein seeking Date!");
            _bValidation = true;
            return false;
        }
        var dSeekingDateFabric = $('#txtYarnSkein').datebox('getValue');
        if (new Date(dSeekingDateFabric) < new Date(dIssueDate)) {
            alert("YarnSkein Date must greater than issue date.");
            _bValidation = true;
            return false;
        }
        var oFabricSeekingDate = { FabricRequestTypeInt: 8, SeekingDate: $('#txtYarnSkein').datebox('getValue'), NoOfSets: $('#txtYarnSkeinSet').val() };
        oFabricSeekingDates.push(oFabricSeekingDate);
    }
    return oFabricSeekingDates;
}

function GetFabricInformation(oFabric) {
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oFabric,
        ControllerName: "Fabric",
        ActionName: "Get",
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

function RefreshFabricLayout(buttonId) {

    //txtActualConstruction
    $("#txtCopyNo").val(1);
    $("#trFabricHideShowFields,#btnShowHistory").hide();
    if (buttonId === "btnViewFabric") {

        $("#winFabric input").prop("disabled", true);
        $("#winFabric select").prop("disabled", true);
        $('.tdInput input, .tdInput select').css("border-color", "");
        $("#btnSaveFabric,#btnReceiveFabricInFabric,#btnReFabricSubmissionSave, #btnSubmitFabricInFabric, #txtCopyNo,#lblGiveInfo").hide();

    }
    else if (buttonId == "btnCopy") {
        $("#btnCopyFabric").show();
        $("#btnSaveFabric, #btnSubmitFabricInFabric").hide();
        $("#btnReFabricSubmissionSave,#btnReceiveFabricInFabric").hide();
    }
    else if (buttonId == "btnSubmitFabricInFabricPattern") {
        $("#trFabricHideShowFields").show();
        $("#divSubDateHideShow").show();
        $('#winFabric input,select,textarea').prop("disabled", true);
        $('#txtHLNo').prop("disabled", true);


        $("#txtSubDateFabric").datebox({ disabled: false });
        $("#txtSubDateFabric").datebox("setValue", icsdateformat(new Date()));

        $('#btnSubmitFabricInFabric').show();
        $("#btnSaveFabric,#btnReceiveFabricInFabric,#btnReFabricSubmissionSave,#txtCopyNo,#lblGiveInfo,#btnContractor, #btnResetContractor").hide();
    }
    else {
        $("#winFabric input").not("#txtFabricNoFabric").prop("disabled", false);
        $("#winFabric select").prop("disabled", false);
        $("#btnSaveFabric").show();
        $("#btnCopyFabric,#btnReceiveFabricInFabric,#btnSubmitFabricInFabric").hide();
    }

    //$("#txtDateFabric").datebox({ disabled: true });
    $("#txtDateFabric").datebox("setValue", icsdateformat(new Date()));
    
}

function RefreshFabricControl(oFabric) {
    debugger;
    _oFabric = oFabric;
    //if (oFabric.FabricNo.length > 0)
    //{
    //    oFabric.FabricNo = oFabric.FabricNo.substring(oFabric.FabricNo.length, 1);
    //}

    if (oFabric.FabricID>0)
        $('#btnShowHistory').show();
    //else
    //    $('#btnShowHistory').hide();

    $("#txtFabricNoFabric").val(oFabric.FabricNum);
    $("#txtBuyerReferenceFabric").val(oFabric.BuyerReference);
    //  $("#txtProduct").val(oFabric.ProductName);
    //_nProductId = oFabric.ProductID;
    $("#txtConstructionFabric").val(oFabric.Construction);
    $("#txtConstructionFabricPI").val(oFabric.ConstructionPI);
    $("#txtFabricWidthFabric").val(oFabric.FabricWidth);
    $("#cboFinishTypeFabric").val(oFabric.FinishType);
    $("#txtContractorName").val(oFabric.BuyerName);
    _nContractorID = oFabric.BuyerID;
    $("#cboMktPersonsFabric").val(oFabric.MKTPersonID);
    $("#txtStyleNoFabric").val(oFabric.StyleNo);
    $("#txtColorFabric").val(oFabric.ColorInfo);
    $("#chkWashFabric").prop("checked", oFabric.IsWash);
    $("#chkFinishFabric").prop("checked", oFabric.IsFinish);
    $("#chkDyeingFabric").prop("checked", oFabric.IsDyeing);
    $("#chkPrintFabric").prop("checked", oFabric.IsPrint);
    $("#txtNoteFabric").val(oFabric.Remarks);
    $("#txtDateFabric").datebox("setValue", oFabric.IssueDateInString);
    //$("#txtSeekingDateFabric").datebox("setValue", oFabric.SeekingSubmissionDateInString);
    $("#txtSubDateFabric").datebox("setValue", oFabric.SubmissionDateInString);
    $("#cboPriorityLevelFabric").val(oFabric.PriorityLevelInInt);
    $('#txtHLNo').val(oFabric.HandLoomNo);
    //$('#txtActualConstruction').val(oFabric.ActualConstruction);
    $('#cboPrimaryLightSource').val(oFabric.PrimaryLightSourceID);
    $('#cboSecondaryLightSource').val(oFabric.SecondaryLightSourceID);
    $('#txtEndUse').val(oFabric.EndUse);
    $('#txtNoOfFrame').val(oFabric.NoOfFrame);
    $('#txtWeftColor').val(oFabric.WeftColor);
    $("#txtWeightDec").val(parseFloat(oFabric.WeightDec));
    
    $("#txtReceiveDateFabric").datebox("setValue", oFabric.ReceiveDateInString);

    $("#cboProcessTypeFabric").val(oFabric.ProcessType);
    $("#cboFabricDesign").val(oFabric.FabricDesignID);
    $("#cboWeaveFabric").val(oFabric.FabricWeave);
    $("#cboFabricOrderType").val(oFabric.FabricOrderTypeInt);
    
    if (_nContractorID > 0) {
        $("#txtContractorName").addClass("fontColorOfPickItem");
    }
    else {
        $("#txtContractorName").removeClass("fontColorOfPickItem");
    }
    $("#cboProductFabric").val(oFabric.ProductID);
    debugger;
    //RefreshCon_BuyerRequest(oFabric);
}

function RefreshObjectFabric() {
    debugger;
   
    var oFabric = {
        FabricID: (_oFabric == null ? 0 : _oFabric.FabricID),
        FabricNo: $.trim($("#txtFabricNoFabric").val()),
        BuyerReference: $.trim($("#txtBuyerReferenceFabric").val()),
        ProductID: parseInt($("#cboProductFabric").val()),     //Composition
        Construction: $.trim($("#txtConstructionFabric").val()),
        ConstructionPI: $.trim($("#txtConstructionFabricPI").val()),
        FabricWidth: $.trim($("#txtFabricWidthFabric").val()),
        FinishType: parseInt($("#cboFinishTypeFabric").val()),
        IssueDate: $('#txtDateFabric').datebox('getValue'),
        BuyerID: parseInt(_nContractorID),
        MKTPersonID: parseInt($("#cboMktPersonsFabric").val()),
        StyleNo: $.trim($("#txtStyleNoFabric").val()),
        ColorInfo: $.trim($("#txtColorFabric").val()),
        IsWash: $("#chkWashFabric").is(':checked'),
        IsFinish: $("#chkFinishFabric").is(':checked'),
        IsDyeing: $("#chkDyeingFabric").is(':checked'),
        IsPrint: $("#chkPrintFabric").is(':checked'),
        Remarks: $.trim($("#txtNoteFabric").val()),
        PriorityLevelInInt: parseInt($("#cboPriorityLevelFabric").val()),
        SeekingSubmissionDate: $('#txtSeekingDate').datebox('getValue'),
        ProcessType: parseInt($("#cboProcessTypeFabric").val()),
        FabricWeave: parseInt($("#cboWeaveFabric").val()),
        FabricDesignID: parseFloat($("#cboFabricDesign").val()),
        FabricSeekingDates: [],
        nCheckReceiveOrSubmitSave: _nCheckReceiveOrSubmitSave,
        PrimaryLightSourceID: $('#cboPrimaryLightSource').val(),
        SecondaryLightSourceID: $('#cboSecondaryLightSource').val(),
        EndUse: $.trim($('#txtEndUse').val()),
        NoOfFrame: $('#txtNoOfFrame').val(),
        WeftColor: $('#txtWeftColor').val(),
        FabricOrderTypeInt: parseInt($("#cboFabricOrderType").val()),
        WeightDec: parseFloat($("#txtWeightDec").val())
       
    };
    console.log(oFabric)
    return oFabric;
}

function ValidateInputFabric() {

    //if (parseInt($("#cboProcessTypeFabric").val()) == 0) {
    //    alert("Please select Process Type!");
    //    $('#cboProcessTypeFabric').focus();
    //    $('#cboProcessTypeFabric').addClass("errorFieldBorder");
    //    return false;
    //} else {
    //    $('#cboProcessTypeFabric').removeClass("errorFieldBorder");
    //}

    //if (parseInt($("#cboFabricDesign").val()) == 0) {
    //    alert("Please select Fabric Design!");
    //    $('#cboFabricDesign').focus();
    //    $('#cboFabricDesign').addClass("errorFieldBorder");
    //    return false;
    //} else {
    //    $('#cboFabricDesign').removeClass("errorFieldBorder");
    //}

    if (parseInt($("#cboFabricOrderType").val()) == 0) {
        alert("Please select Order Type!");
        $('#cboFabricOrderType').focus();
        $('#cboFabricOrderType').addClass("errorFieldBorder");
        return false;
    } else {
        $('#cboFabricOrderType').removeClass("errorFieldBorder");
    }

    if (parseInt($("#cboProductFabric").val()) == 0 || $("#cboProductFabric").val()==null) {
        alert("Please selectComposition !");
        $('#cboProductFabric').focus();
        $('#cboProductFabric').addClass("errorFieldBorder");
        return false;
    } else {
        $('#cboProductFabric').removeClass("errorFieldBorder");
    }


    if (parseInt(_nContractorID) == 0 || _nContractorID == null) {
        alert("Please select Buyer!");
        $('#txtContractorName').val("");
        $('#txtContractorName').focus();
        $('#txtContractorName').addClass("errorFieldBorder");
        return false;
    } else {
        $('#txtContractorName').removeClass("errorFieldBorder");
    }


    if (parseInt($("#cboMktPersonsFabric").val()) == 0) {
        alert("Please select Mkt Person!");
        $('#cboMktPersonsFabric').focus();
        $('#cboMktPersonsFabric').addClass("errorFieldBorder");
        return false;
    } else {
        $('#cboMktPersonsFabric').removeClass("errorFieldBorder");
    }

    //var dSubDateFabric = $('#txtSubDateFabric').datebox('getValue');
    //if (dSubDateFabric == "") {
    //    alert("Please select Sub Date!");
    //    return false;
    //}
    return true;
}

function PickProductInFabric() {

    debugger

    var sProductName = "";//$("#txtProductFabric").val();

    var oProduct = { BUID: sessionStorage.getItem("BUID"), ProductName: "" };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oProduct,
        ControllerName: "Fabric",
        ActionName: "GetProducts",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {
        var listItems = "";
        if (response.status && response.objs.length > 0)
        {
            debugger;
          
            if (response.objs[0].ProductID > 0) {

                for (var i = 0; i < response.objs.length; i++) {
                    if (response.objs[i].ProductID != 0) {
                        listItems += "<option value='" + response.objs[i].ProductID + "'>" + response.objs[i].ProductName + "</option>";
                    }
                }
                $("#cboProductFabric").html(listItems);
            }
            else {
                $("#cboProductFabric").empty();
               
            }
        }
        else {
            $("#cboProductFabric").empty();
            listItems += "<option value='" + _oFabric.ProductID + "'>" + _oFabric.ProductName + "</option>";
            $("#cboProductFabric").html(listItems);
            $("#cboProductFabric").val(_oFabric.ProductID);
        }
    });
}

function GetContractors(oContractor) {

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oContractor,
        ControllerName: "Contractor",
        ActionName: "ContractorSearchByNameType",
        IsWinClose: false
    };

    $("#progressbar").progressbar({ value: 0 });
    $("#progressbarParent").show();
    var intervalID = setInterval(updateProgress, 250);
    $.icsDataGets(obj, function (response) {
        clearInterval(intervalID);
        $("#progressbarParent").hide();
        if (response.status && response.objs.length > 0) {
            if (response.objs[0].ContractorID > 0) {
                debugger;
                var tblColums = [];
                var oColumn = { field: "ContractorID", title: "Code", width: 50, align: "center" }; tblColums.push(oColumn);
                oColumn = { field: "Name", title: "Name", width: 190, align: "left" }; tblColums.push(oColumn);
                oColumn = { field: "Phone", title: "Phone", width: 140, align: "left" }; tblColums.push(oColumn);

                var oPickerParam = {
                    winid: 'winContractorPicker',
                    winclass: 'clsContractorPicker',
                    winwidth: 460,
                    winheight: 460,
                    tableid: 'tblContractorPicker',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'Name',
                    windowTittle: 'Contractor List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbutton(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
        else {
            alert("No contractor found.");
        }
    });


}
