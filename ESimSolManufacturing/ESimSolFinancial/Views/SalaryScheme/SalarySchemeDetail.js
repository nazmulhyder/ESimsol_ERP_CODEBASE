var _oSalarySchemeDetailCalculation = null;
var _oSalarySchemeDetailCalculations = [];
var _oSalarySchemeDetail = null;

function InitializeSalarySchemeDetailEvents() {
 
    //*************************************** Basic Start ********************************************
    $("#txtPercentage_Basic, #txtPercentage_Allowance, #txtTimes, #txtDefferedDay").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && ((e.which < 48 && e.which !=46) || e.which > 57)) {
            return false;
        }
    });

    $("#txtFixedAmount_Basic, #txtFixedAmount_Allowance").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    $("#btnAddGrossCalculation").click(function () {
        if (_nSalarySchemeID <= 0) {
            alert("Please Initialize Salary Scheme!");
            return;
        }
        Reset_SalaryHead();
        sessionStorage.setItem("GrossOrBasic", "Gross");
        $('#btnGross_Basic').hide();
        $("#lblHeaderName_BasicSalaryHead").html("Gross Salary Calculation");
        $("#winBasicSalaryHead").icsWindow("open", "Add Gross Salary Calculation");
        ResetAllFields("winBasicSalaryHead");
    });
    $("#btnAddSalaryHeadCalculation").click(function () {
        if (_nSalarySchemeID <= 0)
        {
            alert("Please Initialize Salary Scheme!");
            return;
        }
        Reset_SalaryHead();
        _oSalarySchemeDetail = $('#tblSalarySchemeDetail_Basic').datagrid('getSelected');
        if (_oSalarySchemeDetail == null || _oSalarySchemeDetail.SalaryHeadID <= 0)
        {
            alert("Please select an item from list!");
            return;
        }
        if (_oSalarySchemeDetail.Calculation != null && _oSalarySchemeDetail.Calculation != "")
        {
            alert("Please remove the present value to add new value!");
            return;
        }
        sessionStorage.setItem("GrossOrBasic", "Basic");
        $('#btnGross_Basic').show();
        $("#lblHeaderName_BasicSalaryHead").html("Salary Head Calculation : " + _oSalarySchemeDetail.SalaryHeadName);
        $("#winBasicSalaryHead").icsWindow("open", "Add SalaryHead Calculation");
        ResetAllFields("winBasicSalaryHead");
    });
    
    $('#btnClear_Basic, #btnClear_Allowance').click(function () {
        if (_oSalarySchemeDetailCalculations.length > 0) {
            var oSalarySchemeDetailCalculations = _oSalarySchemeDetailCalculations;
            _oSalarySchemeDetailCalculations = [];
            for (var i = 0; i < oSalarySchemeDetailCalculations.length - 1; i++) {
                _oSalarySchemeDetailCalculations.push(oSalarySchemeDetailCalculations[i]);
            }
            RefreshEquation();
        }
    });

    $('#btnBracketStart_Basic, #btnBracketStart_Allowance').click(function () {
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 2,
            CalculationOnInt: 0,
            CalculationOnInString: "None",
            FixedValue: 0,
            OperatorInt: 1,
            OperatorInString: "(",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
    });

    $('#btnBracketEnd_Basic, #btnBracketEnd_Allowance').click(function () {
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 2,
            CalculationOnInt: 0,
            CalculationOnInString: "None",
            FixedValue: 0,
            OperatorInt: 2,
            OperatorInString: ")",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
    });

    $('#btnAdd_Basic, #btnAdd_Allowance').click(function () {
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 2,
            CalculationOnInt: 0,
            CalculationOnInString: "None",
            FixedValue: 0,
            OperatorInt: 3,
            OperatorInString: "+",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
    });

    $('#btnSubtract_Basic, #btnSubtract_Allowance').click(function () {
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 2,
            CalculationOnInt: 0,
            CalculationOnInString: "None",
            FixedValue: 0,
            OperatorInt: 4,
            OperatorInString: "-",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
    });

    $('#btnMultiplication_Basic, #btnMultiplication_Allowance').click(function () {
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 2,
            CalculationOnInt: 0,
            CalculationOnInString: "None",
            FixedValue: 0,
            OperatorInt: 5,
            OperatorInString: "*",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
    });

    $('#btnDivision_Basic,#btnDivision_Allowance').click(function () {
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 2,
            CalculationOnInt: 0,
            CalculationOnInString: "None",
            FixedValue: 0,
            OperatorInt: 6,
            OperatorInString: "/",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
    });

    $('#btnGross_Basic,#btnGross_Allowance').click(function () {
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 1,
            CalculationOnInt: 1,
            CalculationOnInString: "Gross",
            FixedValue: 0,
            OperatorInt: 0,
            OperatorInString: "",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
        $("#txtFixedAmount_Basic").val("");
    });

    $('#btnSalaryHead_Basic').click(function () {
        var nSalaryHead = $("#cboSalaryHead_Basic option:selected").val();
        var sSalaryHead = $("#cboSalaryHead_Basic option:selected").text();
        if (parseFloat(nSalaryHead) <= 0) {
            alert('Please select an SalaryHead!');
            return;
        }
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 1,
            CalculationOnInt: 2,
            CalculationOnInString: "SalaryItem",
            FixedValue: 0,
            OperatorInt: 0,
            OperatorInString: "",
            SalaryHeadID: nSalaryHead,
            SalaryHeadName: sSalaryHead,
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
        $("#cboSalaryHead_Basic").val(0);
    });

    $('#btnPercent_Basic').click(function () {
        var sPereentAmount = $("#txtPercentage_Basic").val();
        if (sPereentAmount == null || sPereentAmount == "") {
            alert("Please Enter Percent Amount!");
            return;
        }
        var nPercentAmount = parseFloat(sPereentAmount);
        //if (nPercentAmount <= 0 || nPercentAmount > 100) {
        //    alert("Invalid Percent Range!\n Valid Range (1--100)!");
        //    return;
        //}
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 2,
            CalculationOnInt: 0,
            CalculationOnInString: "None",
            FixedValue: 0,
            OperatorInt: 7,
            OperatorInString: "Percent",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: nPercentAmount
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
        $("#txtPercentage_Basic").val("");
    });

    $('#btnFixedAmount_Basic').click(function () {
        var sFixedAmount = $("#txtFixedAmount_Basic").val();
        if (sFixedAmount == null || sFixedAmount == "") {
            alert("Please Enter Fixed Amount!");
            return;
        }
        var nFixedAmount = parseFloat(sFixedAmount);
        if (nFixedAmount <= 0) {
            alert("Zero or Negative Amount Not Allow!");
            return;
        }
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 1,
            CalculationOnInt: 3,
            CalculationOnInString: "Fixed",
            FixedValue: nFixedAmount.toFixed(2),
            OperatorInt: 0,
            OperatorInString: "",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
        $("#txtFixedAmount_Basic").val("");
    });

    function RefreshEquation() {
        var sEquation = "";
        for (var i = 0; i < _oSalarySchemeDetailCalculations.length; i++) {
            if (parseFloat(_oSalarySchemeDetailCalculations[i].ValueOperatorInt) == 2)//Operator=2
            {
                if (parseFloat(_oSalarySchemeDetailCalculations[i].OperatorInt) != 7) {
                    sEquation = sEquation + _oSalarySchemeDetailCalculations[i].OperatorInString;
                }
                else {
                    sEquation = sEquation + " " + _oSalarySchemeDetailCalculations[i].PercentVelue + " % of";
                }
            }
            else {
                if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 1)//Gross = 1
                {
                    sEquation = sEquation + " " + _oSalarySchemeDetailCalculations[i].CalculationOnInString;
                }
                else if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 2)//SalaryItem = 2
                {
                    sEquation = sEquation + " " + _oSalarySchemeDetailCalculations[i].SalaryHeadName + " ";
                }
                else if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 3)//Fixed = 3
                {
                    sEquation = sEquation + " " + _oSalarySchemeDetailCalculations[i].FixedValue;
                }
            }
        }
        $("#txtEquation_Basic").val(sEquation);
        $("#txtEquation_Allowance").val(sEquation);
    }

    $('#btnSave_BasicSalaryHead').click(function () {
        debugger
        var sGrossOrBasic = sessionStorage.getItem("GrossOrBasic");
        if (sGrossOrBasic == "Basic") {

            if (!ValidateInput_Basic()) { return; }
            var oSalarySchemeDetail = RefreshObject_Basic();
            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/SalaryScheme/SaveDetailAndCalculation_V1",
                traditional: true,
                data: JSON.stringify(oSalarySchemeDetail),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var oSalarySchemeDetail = jQuery.parseJSON(data);
                    if (oSalarySchemeDetail.ErrorMessage == "") {
                        alert("Data Save Successfully.");
                        $("#winBasicSalaryHead").icsWindow("close");
                        var SelectedRowIndex = $('#tblSalarySchemeDetail_Basic').datagrid('getRowIndex', _oSalarySchemeDetail);
                        if (oSalarySchemeDetail != null) {
                            $('#tblSalarySchemeDetail_Basic').datagrid('updateRow', { index: SelectedRowIndex, row: oSalarySchemeDetail });
                        }
                    }
                    else {
                        alert(oSalarySchemeDetail.ErrorMessage);
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
        else {

            var oGrossSalaryCalculation = {
                GSCID: 0,
                SalarySchemeID: _nSalarySchemeID,
                SalarySchemeDetailCalculations: _oSalarySchemeDetailCalculations
            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/SalaryScheme/SaveDetailAndCalculation_V1ForGross",
                traditional: true,
                data: JSON.stringify(oGrossSalaryCalculation),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var oSalarySchemeDetail = jQuery.parseJSON(data);
                    if (oSalarySchemeDetail.ErrorMessage == "") {
                        alert("Data Save Successfully.");
                        $("#winBasicSalaryHead").icsWindow("close");
                        var SelectedRowIndex = $('#tblSalarySchemeDetail_Basic').datagrid('getRowIndex', _oSalarySchemeDetail);
                        $('#txtGrossEquation').val(oSalarySchemeDetail.Calculation);
                    }
                    else {
                        alert(oSalarySchemeDetail.ErrorMessage);
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }

    });

    $('#btnRemoveSCalculation_Basic').click(function (e) {
        debugger;
        var oSalarySchemeDetail = $('#tblSalarySchemeDetail_Basic').datagrid('getSelected');
        if (oSalarySchemeDetail == null) {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (!confirm("Confirm to Remove?")) return;
        var SelectedRowIndex = $('#tblSalarySchemeDetail_Basic').datagrid('getRowIndex', oSalarySchemeDetail);
        var tsv = ((new Date()).getTime()) / 1000;
        if (oSalarySchemeDetail.SalarySchemeDetailID > 0) {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: _sBaseAddress + "/SalaryScheme/SalarySchemeDetail_Delete",
                data: { nId: oSalarySchemeDetail.SalarySchemeDetailID, ts: tsv },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var feedbackmessage = jQuery.parseJSON(data);
                    if (feedbackmessage == "Deleted") {
                        alert("Remove sucessfully ");
                        oSalarySchemeDetail.Calculation = "";
                        $('#tblSalarySchemeDetail_Basic').datagrid('updateRow', { index: SelectedRowIndex, row: oSalarySchemeDetail });
                    }
                    else {
                        alert(feedbackmessage);
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
    });

    $("#btnClose_BasicSalaryHead").click(function () {
        $("#winBasicSalaryHead").icsWindow("close");
    });

    //*************************************** Basic End ********************************************
    //*************************************** Allowance Start **************************************

    $("#btnAddAllowance").click(function () {
        if (_nSalarySchemeID <= 0)
        {
            alert("Please Initialize Salary Scheme!");
            return;
        }
        Reset_SalaryHead();
        $("#lblHeaderName_Allowance").html("Add Allowance");
        $("#winAllowanceSalaryHead").icsWindow("open", "Add Allowance");
        ResetAllFields("winAllowanceSalaryHead");
        LoadAllowanceType();
    });

    $('#btnSalaryHead_Allowance').click(function () {
        var nSalaryHead = $("#cboSalaryHead_Allowance option:selected").val();
        var sSalaryHead = $("#cboSalaryHead_Allowance option:selected").text();
        if (parseFloat(nSalaryHead) <= 0) {
            alert('Please select an SalaryHead!');
            return;
        }
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 1,
            CalculationOnInt: 2,
            CalculationOnInString: "SalaryItem",
            FixedValue: 0,
            OperatorInt: 0,
            OperatorInString: "",
            SalaryHeadID: nSalaryHead,
            SalaryHeadName: sSalaryHead,
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
        $("#cboSalaryHead_Allowance").val(0);
    });

    $('#btnPercent_Allowance').click(function () {
        debugger;
        var sPereentAmount = $("#txtPercentage_Allowance").val();
        if (sPereentAmount == null || sPereentAmount == "") {
            alert("Please Enter Percent Amount!");
            return;
        }
        var nPercentAmount = parseFloat(sPereentAmount);
        if (nPercentAmount <= 0 || nPercentAmount > 100) {
            alert("Invalid Percent Range!\n Valid Range (1--100)!");
            return;
        }
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 2,
            CalculationOnInt: 0,
            CalculationOnInString: "None",
            FixedValue: 0,
            OperatorInt: 7,
            OperatorInString: "Percent",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: nPercentAmount
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
        $("#txtPercentage_Allowance").val("");
    });

    $('#btnFixedAmount_Allowance').click(function () {
        var sFixedAmount = $("#txtFixedAmount_Allowance").val();
        if (sFixedAmount == null || sFixedAmount == "") {
            alert("Please Enter Fixed Amount!");
            return;
        }
        var nFixedAmount = parseFloat(sFixedAmount);
        if (nFixedAmount <= 0) {
            alert("Zero or Negative Amount Not Allow!");
            return;
        }
        var oSSDetailCalculation = {
            SalarySchemeDetailID: 0,
            ValueOperatorInt: 1,
            CalculationOnInt: 3,
            CalculationOnInString: "Fixed",
            FixedValue: nFixedAmount.toFixed(2),
            OperatorInt: 0,
            OperatorInString: "",
            SalaryHeadID: 0,
            SalaryHeadName: "",
            PercentVelue: 0
        };
        _oSalarySchemeDetailCalculations.push(oSSDetailCalculation);
        RefreshEquation();
        $("#txtFixedAmount_Allowance").val("");
    });

    $('#btnSave_Allowance').click(function () {
        if (!ValidateInput_Allowance()) { return; }
        var oSalarySchemeDetail = RefreshObject_Allowance();
        
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _sBaseAddress + "/SalaryScheme/SaveDetailAndCalculation_V1",
            traditional: true,
            data: JSON.stringify(oSalarySchemeDetail),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oSalarySchemeDetail = jQuery.parseJSON(data);
                if (oSalarySchemeDetail.ErrorMessage == "") {
                    alert("Data Save Successfully.");
                    $("#winBasicSalaryHead").icsWindow("close");
                  
                    if (oSalarySchemeDetail != null) {

                        var oSalarySchemeDetails = $('#tblSalarySchemeDetail_Allowance').datagrid('getRows');
                        var nIndex = oSalarySchemeDetails.length;
                        $('#tblSalarySchemeDetail_Allowance').datagrid('appendRow', oSalarySchemeDetail);
                        $('#tblSalarySchemeDetail_Allowance').datagrid('selectRow', nIndex);
                        
                    }
                }
                else {
                    alert(oSalarySchemeDetail.ErrorMessage);
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });

    });

    $('#btnDelete_Allowance').click(function () {
        var oSalarySchemeDetail = $('#tblSalarySchemeDetail_Allowance').datagrid('getSelected');
        if (oSalarySchemeDetail == null)
        {
            alert("Invalid Field!!! please select a valid Field!");
            return false;
        }
        if (!confirm("Confirm to Remove?")) return;
        var SelectedRowIndex = $('#tblSalarySchemeDetail_Allowance').datagrid('getRowIndex', oSalarySchemeDetail);
        var tsv = ((new Date()).getTime()) / 1000;
        if (parseFloat(oSalarySchemeDetail.SalarySchemeDetailID) > 0) {

            $.ajax({
                type: "GET",
                dataType: "json",
                url: _sBaseAddress + "/SalaryScheme/SalarySchemeDetail_Delete",
                data: { nId: oSalarySchemeDetail.SalarySchemeDetailID ,ts: tsv},
                contentType: "application/json; charset=utf-8",
                success: function(data) {

                    var feedbackmessage = jQuery.parseJSON(data);
                    if (feedbackmessage == "Deleted") {
                        alert("Delete sucessfully");
                        $('#tblSalarySchemeDetail_Allowance').datagrid('deleteRow', SelectedRowIndex);

                    } else {
                        alert(feedbackmessage);
                    }
                },
                error: function(xhr, status, error) {
                    alert(error);
                }
            });
        }
        else
        {
            $('#tblSalarySchemeDetail_Allowance').datagrid('deleteRow', SelectedRowIndex);
        }
    });

    $("#btnClose_Allowance").click(function () {
        $("#winAllowanceSalaryHead").icsWindow("close");
    });
//*************************************** Allowance End ********************************************
}

//*************************************** Basic Start ********************************************
function LoadSalaryHeadOfBasicType() {
    $.ajax
    ({
        type: "GET",
        dataType: "json",
        url: _sBaseAddress + "/SalaryHead/LoadAllowance",
        data: { Id: 1 },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            oBasicSalaryHeads = jQuery.parseJSON(data);
            if (oBasicSalaryHeads != null) {
                if (oBasicSalaryHeads.length > 0) {
                    $('#cboSalaryHead_Basic').empty();
                    $('#cboSalaryHead_Allowance').empty();
                    var listItems = "";
                    listItems += "<option value=0>--Salary Head--</option>";
                    for (var i = 0; i < oBasicSalaryHeads.length; i++) {
                        listItems += "<option value='" + parseFloat(oBasicSalaryHeads[i].SalaryHeadID) + "'>" + oBasicSalaryHeads[i].Name + "</option>";
                    }
                    $("#cboSalaryHead_Basic").html(listItems);
                    $("#cboSalaryHead_Allowance").html(listItems);
                }
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function RefreshObject_Basic() {
    var oSalarySchemeDetail = {
        SalarySchemeDetailID: 0,
        SalarySchemeID: _nSalarySchemeID,
        SalaryHeadID: _oSalarySchemeDetail.SalaryHeadID,
        Condition: 0,
        Period: 0,
        Times: 0,
        DeferredDay: 0,
        ActivationAfter: 0,
        SalarySchemeDetailCalculations: _oSalarySchemeDetailCalculations
    };
    return oSalarySchemeDetail;
}

function ValidateInput_Basic() {
    if (parseFloat(_nSalarySchemeID) <= 0) {
        alert("Invalid Salary Scheme!");
        return false;
    }
    if (parseFloat(_oSalarySchemeDetail.SalaryHeadID) <= 0) {
        alert("Invalid Salary Head!");
        return false;
    }
    if (_oSalarySchemeDetailCalculations == null || _oSalarySchemeDetailCalculations.length <= 0) {
        alert("Please Enter Equation!");
        return false;
    }
    var sEquation = ""; var sGrossAmount = "1000000000.00"; var sSalaryHeadAmount = "500.00";
    for (var i = 0; i < _oSalarySchemeDetailCalculations.length; i++) {
        if (parseFloat(_oSalarySchemeDetailCalculations[i].ValueOperatorInt) == 2)//Operator=2
        {
            if (parseFloat(_oSalarySchemeDetailCalculations[i].OperatorInt) != 7) {
                sEquation = sEquation + _oSalarySchemeDetailCalculations[i].OperatorInString;
            }
            else {
                if ((i + 1) >= _oSalarySchemeDetailCalculations.length) {
                    //Must be a head exists
                    alert('Invalid equation!\nPlease check your equation!')
                    return false;
                }
                if (parseFloat(_oSalarySchemeDetailCalculations[i + 1].CalculationOnInt) == 1)//Gross = 1
                {
                    sEquation = sEquation + " (" + sGrossAmount + "*" + _oSalarySchemeDetailCalculations[i].PercentVelue + ")/100";
                }
                else if (parseFloat(_oSalarySchemeDetailCalculations[i + 1].CalculationOnInt) == 2)//SalaryItem = 2
                {
                    sEquation = sEquation + " (" + sSalaryHeadAmount + "*" + _oSalarySchemeDetailCalculations[i].PercentVelue + ")/100";
                }
                else if (parseFloat(_oSalarySchemeDetailCalculations[i + 1].CalculationOnInt) == 3)//Fixed = 3
                {
                    sEquation = sEquation + " (" + _oSalarySchemeDetailCalculations[i + 1].FixedValue + "*" + _oSalarySchemeDetailCalculations[i].PercentVelue + ")/100";
                }
                i = i + 1;
            }
        }
        else {
            if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 1)//Gross = 1
            {
                sEquation = sEquation + " " + sGrossAmount;
            }
            else if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 2)//SalaryItem = 2
            {
                sEquation = sEquation + " " + sSalaryHeadAmount + " ";
            }
            else if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 3)//Fixed = 3
            {
                sEquation = sEquation + " " + _oSalarySchemeDetailCalculations[i].FixedValue;
            }
        }
    }
    try {
        var nTotalAmount = parseFloat(eval(sEquation));
        if (nTotalAmount < 0) {
            alert('Invalid equation!\nYour calculate negative Value!')
            return false;
        }
    }
    catch (err) {
        alert('Invalid equation!\nPlease check your equation!')
        return false;
    }
    return true;
}

function Reset_SalaryHead()
{
    $("#txtEquation_Basic").val("");
    $("#txtEquation_Allowance").val("");
    $("#txtTimes").val("");
    $("#txtDefferedDay").val("");
    document.getElementById("cboPeriod").selectedIndex = 0;
    document.getElementById("cboActivationAfter").selectedIndex = 0;
    document.getElementById("cboCondition").selectedIndex = 0;
    _oSalarySchemeDetailCalculation = null;
    _oSalarySchemeDetail = null;
    _oSalarySchemeDetailCalculations = [];
}
//*************************************** Basic End ********************************************
//*************************************** Allowance Start **************************************
function LoadAllowanceType() {
    $('#cboAllowanceType').empty();
    var listItems = "";
    var i = 0;
    $.ajax
    ({
        type: "GET",
        dataType: "json",
        url: _sBaseAddress + "/SalaryScheme/LoadAllowanceType",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            oAllowanceTypes = jQuery.parseJSON(data);
            if (oAllowanceTypes != null) {
                if (oAllowanceTypes.length > 0) {
                    for (i = 0; i < oAllowanceTypes.length; i++) {
                        listItems += "<option value='" + oAllowanceTypes[i].Id + "'>" + oAllowanceTypes[i].Value + "</option>";
                    }
                }
                $("#cboAllowanceType").html(listItems);
            }
            else {
                alert("data not found");
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function LoadAllowance() {
    var nAllowanceType = $("#cboAllowanceType option:selected").index();
    if (nAllowanceType > 0) {
        nAllowanceType = nAllowanceType + 1;
        $('#cboAllowance').empty();
        var listItems = "";
        var i = 0;
        $.ajax
        ({
            type: "GET",
            dataType: "json",
            url: _sBaseAddress + "/SalaryHead/LoadAllowance",
            data: { Id: nAllowanceType },
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                oAllowances = jQuery.parseJSON(data);
                if (oAllowances != null) {
                    if (oAllowances.length > 0) {
                        for (i = 0; i < oAllowances.length; i++) {
                            listItems += "<option value='" + oAllowances[i].SalaryHeadID + "'>" + oAllowances[i].Name + "</option>";
                        }
                    }
                    $("#cboAllowance").html(listItems);
                }
                else {
                    alert("data not found");
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    }
}

function RefreshObject_Allowance() {
    var oSalarySchemeDetail = {
        SalarySchemeDetailID: 0,
        SalarySchemeID: _nSalarySchemeID,
        SalaryHeadID: parseFloat($("#cboAllowance option:selected").val()),
        ConditionInt: parseFloat($("#cboCondition option:selected").index()),
        PeriodInt: parseFloat($("#cboPeriod option:selected").index()),
        Times: parseFloat($("#txtTimes").val()),
        DeferredDay: parseFloat($("#txtDefferedDay").val()),
        ActivationAfterInt: parseFloat($("#cboActivationAfter option:selected").index()),
        SalaryTypeInt: $('#cboEnumSalaryType').val(),
        SalarySchemeDetailCalculations: _oSalarySchemeDetailCalculations
        
       
    };
   
    return oSalarySchemeDetail;
}

function ValidateInput_Allowance() {
    if (parseFloat(_nSalarySchemeID) <= 0) {
        alert("Invalid Salary Scheme!");
        return false;
    }
    var nAllowanceType = $("#cboAllowanceType option:selected").index();
    if (parseFloat(nAllowanceType) <= 0) {
        alert("Please select Allowance Type!");
        $("#cboAllowanceType").focus();
        return false;
    }
    var nAllowance = $("#cboAllowance option:selected").val();
    if (parseFloat(nAllowance) <= 0) {
        alert("Please select Allowance!");
        $("#cboAllowance").focus();
        return false;
    }
    var sTimes = $("#txtTimes").val();
    if (sTimes == null || sTimes == "") {
        alert("Please Enter Times!");
        $("#txtTimes").focus();
        return false;
    }
    if (parseFloat(sTimes) <= 0) {
        alert("Please Enter Times!");
        $("#txtTimes").focus();
        return false;
    }
    var nPeriod = $("#cboPeriod option:selected").index();
    if (parseFloat(nPeriod) <= 0) {
        alert("Please select Period!");
        $("#cboPeriod").focus();
        return false;
    }
    //var nCondition = $("#cboCondition option:selected").index();        
    //if(parseFloat(nCondition)<=0)
    //{
    //    alert("Please select Condition!");
    //    $("#cboCondition").focus();
    //    return false;
    //}
    var sDefferedDay = $("#txtDefferedDay").val();
    if (sDefferedDay == null || sDefferedDay == "") {
        alert("Please Enter Activation Days!");
        $("#txtDefferedDay").focus();
        return false;
    }
    if (parseFloat(sDefferedDay) <= 0) {
        alert("Please Enter Activation Days!");
        $("#txtDefferedDay").focus();
        return false;
    }
    var nActivationAfter = $("#cboActivationAfter option:selected").index();
    if (parseFloat(nActivationAfter) <= 0) {
        alert("Please select Activation After!");
        $("#cboActivationAfter").focus();
        return false;
    }
    if (_oSalarySchemeDetailCalculations == null || _oSalarySchemeDetailCalculations.length <= 0) {
        alert("Please Enter Equation!");
        return false;
    }
    var sEquation = ""; var sGrossAmount = "1000.00"; var sSalaryHeadAmount = "500.00";
    for (var i = 0; i < _oSalarySchemeDetailCalculations.length; i++) {
        if (parseFloat(_oSalarySchemeDetailCalculations[i].ValueOperatorInt) == 2)//Operator=2
        {
            if (parseFloat(_oSalarySchemeDetailCalculations[i].OperatorInt) != 7) {
                sEquation = sEquation + _oSalarySchemeDetailCalculations[i].OperatorInString;
            }
            else {
                if ((i + 1) >= _oSalarySchemeDetailCalculations.length) {
                    //Must be a head exists
                    alert('Invalid equation!\nPlease check your equation!')
                    return false;
                }
                if (parseFloat(_oSalarySchemeDetailCalculations[i + 1].CalculationOnInt) == 1)//Gross = 1
                {
                    sEquation = sEquation + " (" + sGrossAmount + "*" + _oSalarySchemeDetailCalculations[i].PercentVelue + ")/100";
                }
                else if (parseFloat(_oSalarySchemeDetailCalculations[i + 1].CalculationOnInt) == 2)//SalaryItem = 2
                {
                    sEquation = sEquation + " (" + sSalaryHeadAmount + "*" + _oSalarySchemeDetailCalculations[i].PercentVelue + ")/100";
                }
                else if (parseFloat(_oSalarySchemeDetailCalculations[i + 1].CalculationOnInt) == 3)//Fixed = 3
                {
                    sEquation = sEquation + " (" + _oSalarySchemeDetailCalculations[i + 1].FixedValue + "*" + _oSalarySchemeDetailCalculations[i].PercentVelue + ")/100";
                }
                i = i + 1;
            }
        }
        else {
            if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 1)//Gross = 1
            {
                sEquation = sEquation + " " + sGrossAmount;
            }
            else if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 2)//SalaryItem = 2
            {
                sEquation = sEquation + " " + sSalaryHeadAmount + " ";
            }
            else if (parseFloat(_oSalarySchemeDetailCalculations[i].CalculationOnInt) == 3)//Fixed = 3
            {
                sEquation = sEquation + " " + _oSalarySchemeDetailCalculations[i].FixedValue;
            }
        }
    }
    try {
        var nTotalAmount = parseFloat(eval(sEquation));
        if (nTotalAmount < 0) {
            alert('Invalid equation!\nYour calculate negative Value!')
            return false;
        }
    }
    catch (err) {
        alert('Invalid equation!\nPlease check your equation!')
        return false;
    }
    return true;
}

//*************************************** Allowance End **************************************