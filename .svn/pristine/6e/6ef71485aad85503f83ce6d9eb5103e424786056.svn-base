//var _oIsAdd = false;
//var _nProductID = 0;
//var _nCheckReceiveOrSubmitSave = 0;
//var _nContractorID = 0;
var _oFNLabDipDetails = [];
var _oFNLabDipDetail = null;
function InitializeFNLabdipDetailEvents() {
    $("#btnUpdateLabDip").hide();

   
    $('#cboShadeCount').val(3);

    $("#btnCloseLD").click(function () {
        $("#winFNLabdip").icsWindow('close');
        $(".wininputfieldstyle input").val("");
        //$("#chkIsOwnFabric").prop("checked", false);
    });

    $("#btnAddLabDip").click(function () {
        SaveLabDipDetail(false);
    });
    $("#btnUpdateLabDip").click(function () {
        SaveLabDipDetail(true);
    });
   
    $('#btnEditLabDip').click(function (e) {
        debugger;

            var oFNLabDipDetail = $("#tblLabDipDetail").datagrid("getSelected");
            if (oFNLabDipDetail == null || oFNLabDipDetail.FNLabDipDetailID <= 0) { alert("Please select an item from list!"); return false; }
            if (oFNLabDipDetail.ReceiveBY != 0) {
                alert("Already Received!");
                return;
            }
            if (oFNLabDipDetail.SubmitBy != 0) {
                alert("Already Submited!");
                return;
            }
            var obj =
            {
                BaseAddress: _sBaseAddress,
                Object: oFNLabDipDetail,
                ControllerName: "FNLabDipDetail",
                ActionName: "GetFNLabDipDetail",
                IsWinClose: false
            };

            $.icsDataGet(obj, function (response)
            {

                if (response.status && response.obj != null) {
                    if (response.obj.FNLabDipDetailID > 0) {
                        _oFNLabDipDetail = response.obj;
                       
                        $("#btnUpdateLabDip").show();
                        $("#btnEditLabDip").hide();
                       
                        $('#cboShadeCount').val(response.obj.ShadeCount);
                        //$('#cboCombo').val(response.obj.Combo);
                        //$('#cboKnitPly').val(response.obj.KnitPlyYarn);
                        $('#txtColorNameLD').val(response.obj.ColorName);
                        $('#txtPantonNo').val(response.obj.PantonNo);
                        $('#txtRGB').val(response.obj.RGB);
                        $('#txtNoteLD').val(response.obj.Note);
                        
                        //$('#lblEditUpdate').text('Update');

                    }
                    else { alert(response.obj.ErrorMessage); }
                }
            });
      
    });

    $("#btnRemoveLabDip").click(function () {

        var oFNLabDipDetail = $("#tblLabDipDetail").datagrid("getSelected");
        if (oFNLabDipDetail == null || oFNLabDipDetail.FNLabDipDetailID <= 0) { alert("Please select an item from list!"); return false; }
        if (oFNLabDipDetail.ReceiveBY != 0) {
            alert("Already Received!");
            return;
        }
        if (oFNLabDipDetail.SubmitBy != 0) {
            alert("Already Submited!");
            return;
        }
        if (!confirm("Confirm to Delete?")) return false;
        var nIndex = $("#tblLabDipDetail").datagrid("getRowIndex", oFNLabDipDetail);
        var obj =
        {
            BaseAddress: _sBaseAddress,
            Object: oFNLabDipDetail,
            ControllerName: "FNLabDipDetail",
            ActionName: "DeleteDetail",
            TableId: "tblLabDipDetail",
            IsWinClose: false,
            Message: ''
        };
        $.icsDelete(obj, function (response) {
            if (response.status && response.Message == 'deleted')
            {
                var oLabdipdetails = $('#tblLabDipDetail').datagrid('getRows');
                DynamicRefreshList(oLabdipdetails, 'tblLabDipDetail');
            }
        });



    });
    
    $("#btnIssued").click(function () {

        var oFNLabDipDetail = $("#tblLabDipDetail").datagrid("getSelected");
        if (oFNLabDipDetail == null || oFNLabDipDetail.FNLabDipDetailID <= 0) { alert("Please select an item from list!"); return false; }
        if (oFNLabDipDetail.ReceiveBY != 0) {
            alert("Already Received!");
            return;
        }
      
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFNLabDipDetail,
            ObjectId: oFNLabDipDetail.FNLabDipDetailID,
            ControllerName: "FNLabDipDetail",
            ActionName: "Issued",
            TableId: ((oFNLabDipDetail != null) ? "tblLabDipDetail" : ""),
            IsWinClose: false,
            Message: "Issued Successfully."
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                debugger;

            }
        })
    });
    $("#btnReLabDip").click(function ()
    {
        
        var oFNLabDipDetail = $("#tblLabDipDetail").datagrid("getSelected");
        if (oFNLabDipDetail == null || oFNLabDipDetail.FNLabDipDetailID <= 0) { alert("Please select an item from list!"); return false; }
        //if (oFNLabDipDetail.ReceiveBY != 0) {
        //    alert("Already Received!");
        //    return;
        //}
        
        oFNLabDipDetail.ReferenceLDID = oFNLabDipDetail.FNLabDipDetailID;
        oFNLabDipDetail.FNLabDipDetailID = 0;
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFNLabDipDetail,
            ObjectId: oFNLabDipDetail.FNLabDipDetailID,
            ControllerName: "FNLabDipDetail",
            ActionName: "SaveDetail",
            TableId: ((oFNLabDipDetail != null) ? "tblLabDipDetail" : ""),
            IsWinClose: false,
            Message: "No Create Successfully."
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                debugger;
                
            }
        })
    });
    $("#btnSaveLDNo").click(function () {
        var oFNLabDipDetail = $("#tblLabDipDetail").datagrid("getSelected");
        if (oFNLabDipDetail == null || oFNLabDipDetail.FNLabDipDetailID <= 0) { alert("Please select an item from list!"); return false; }
        if (oFNLabDipDetail.ReceiveBY != 0) {
            alert("Already Received!");
            return;
        }
        if (oFNLabDipDetail.SubmitBy != 0) {
            alert("Already Submited!");
            return;
        }
        oFNLabDipDetail.LDNo = $.trim($('#txtLDNo').val());
        oFNLabDipDetail.ReceiveBY=0;
        var obj = {
            BaseAddress: _sBaseAddress,
            Object: oFNLabDipDetail,
            ObjectId: oFNLabDipDetail.FNLabDipDetailID,
            ControllerName: "FNLabDipDetail",
            ActionName: "Save_LDNo",
            TableId: ((oFNLabDipDetail != null) ? "tblLabDipDetail" : ""),
            IsWinClose: false,
            Message: "No Create Successfully."
        };
        $.icsSave(obj, function (response) {
            if (response.status && response.obj != null) {
                debugger;

            }
        })
    });
    $("#btnAssignShade").click(function () {
        GetsShade();
    });
}
function RefreshObjectDetail() {

    var oFNLabDipDetail = {
        FNLabDipDetailID: (_oFNLabDipDetail != null && _oFNLabDipDetail.FNLabDipDetailID > 0) ? _oFNLabDipDetail.FNLabDipDetailID : 0,
        FabricID: _oFabric.FabricID,
        ColorSet: 0,
        ShadeCount: $('#cboShadeCount').val(),
        //Combo: $('#cboCombo').val(),
        ColorName: $.trim($('#txtColorNameLD').val()),
        Note: $.trim($('#txtNoteLD').val()),
        PantonNo: $.trim($('#txtPantonNo').val()),
        RGB: $.trim($('#txtRGB').val()),
        ReferenceLDID:0
    }
    return oFNLabDipDetail;
}

function SaveLabDipDetail(bIsUpdate) {
    debugger;

    //if (!Validation()) return false;
    if (!ValidateInputLabdip()) return false;
    var oFNLabDipDetail = RefreshObjectDetail();
    oFNLabDipDetail.FNLabDipDetailID = (bIsUpdate) ? oFNLabDipDetail.FNLabDipDetailID : 0;
  

    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oFNLabDipDetail,
        ObjectId: oFNLabDipDetail.FNLabDipDetailID,
        ControllerName: "FNLabDipDetail",
        ActionName: "SaveDetail",
        TableId: ((oFNLabDipDetail != null) ? "tblLabDipDetail" : ""),
        IsWinClose: false,
        Message: (oFNLabDipDetail.FNLabDipDetailID> 0) ? "Update Successfully." : "Save Successfully."
    };
    $.icsSave(obj, function (response) {
        if (response.status && response.obj != null) {
            debugger;
            if (response.obj.FNLabDipDetailID > 0) {
                //if (response.obj.FabricID > 0)
                //{
                //    $('#tblLabDipDetail').datagrid('appendRow', response.obj);
                //}
                $('#txtColorNameLD').val("");
                $('#txtPantonNo').val("");
                $('#txtRGB').val("");
                $('#txtNoteLD').val("");
                $('#txtColorNameLD').focus();
                $("#btnUpdateLabDip").hide();
                $("#btnEditLabDip").show();
            }
        }
    });
}

function RefreshCon_LabDip(oFabric) {
    debugger;
    $('#txtFabricNo_LD').val(oFabric.FabricNo);
    $('#txtComposition_LD').val(oFabric.ProductName);
    $('#txtConstruction_LD').val(oFabric.Construction);

    DynamicRefreshList(_oFNLabDipDetails, 'tblLabDipDetail');
    GetsFNLabDipDetails(oFabric);
}

function GetsFNLabDipDetails(oFabric)
{
    var oFNLabDipDetail = { FNLabDipDetailID: 0, FabricID: oFabric.FabricID}
    var obj =
    {
        BaseAddress: _sBaseAddress,
        Object: oFNLabDipDetail,
        ControllerName: "FNLabDipDetail",
        ActionName: "GetByFabric",
        IsWinClose: false
    };
    debugger;
    $.icsDataGets(obj, function (response) {

        if (response.status && response.objs != null)
        {
            if (response.objs.length > 0)
            {
                DynamicRefreshList(response.objs, 'tblLabDipDetail');
            }
            
        }
        
    });
}

function ValidateInputLabdip() {

    if ($("#txtColorNameLD").val() == "" || $("#txtColorNameLD").val() == null)
    {
        alert("Please, entry Color Name!");
        $('#txtColorNameLD').focus();
        $('#txtColorNameLD').addClass("errorFieldBorder");
        return false;
    } else {
        $('#txtColorNameLD').removeClass("errorFieldBorder");
    }
    return true;
}
function GetsShade() {
    debugger;
    var oFNLabDipDetail = $('#tblLabDipDetail').datagrid('getSelected');
    var nSelectedIndex = $('#tblLabDipDetail').datagrid('getRowIndex', oFNLabDipDetail);
    if (oFNLabDipDetail == null || oFNLabDipDetail.FNLabDipDetailID <= 0) {
        alert("Please select a item from list!");
        return;
    }
    //if(oFNLabDipDetail.ReceiveBY==0)
    //{
    //    alert("Yet Not Receive!");
    //    return;
    //}
    //if(oFNLabDipDetail.SubmitBy==0)
    //{
    //    alert("Yet Not Submited!");
    //    return;
    //}

    var oFNLabDipDetailTemp = {
        FNLabDipDetailID: oFNLabDipDetail.FNLabDipDetailID
    };
    var obj = {
        BaseAddress: _sBaseAddress,
        Object: oFNLabDipDetailTemp,
        ControllerName: "FNLabDipDetail",
        ActionName: "GetsShade",
        IsWinClose: false
    };

    $.icsDataGets(obj, function (response) {

        if (response.status && response.objs.length > 0) {
            if (response.objs[0].FNLabdipShadeID > 0) {
                debugger;
                var tblColums = [];
                var oColumn = { field: "ShadeStr", title: "Shade", width: 100, align: "left" }; tblColums.push(oColumn);

                var oPickerParam = {
                    winid: 'winShadePicker',
                    winclass: 'clsShadePicker',
                    winwidth: 120,
                    winheight: 300,
                    tableid: 'tblShadePicker',
                    tablecolumns: tblColums,
                    datalist: response.objs,
                    multiplereturn: false,
                    searchingbyfieldName: 'ShadeStr',
                    windowTittle: 'Shade List'
                };
                $.icsPicker(oPickerParam);
                IntializePickerbuttonTwo(oPickerParam);//multiplereturn, winclassName
            }
            else { alert(response.objs[0].ErrorMessage); }
        }
        else {
            alert("No Shade person found.");
        }
    });

}

function IntializePickerbuttonTwo(oPickerobj) {
    $("#" + oPickerobj.winid).find("#btnOk").click(function () {
        SetPickerValueAssignTwo(oPickerobj);
    });
    $(document).find('.' + oPickerobj.winclass).keydown(function (e) {
        if (e.which === 13) {
            SetPickerValueAssignTwo(oPickerobj);
        }
    });
}

function SetPickerValueAssignTwo(oPickerobj) {
    var oreturnObj = null, oreturnObjs = [];
    if (oPickerobj.multiplereturn) {
        oreturnObjs = $('#' + oPickerobj.tableid).datagrid('getChecked');
    } else {
        oreturnObj = $('#' + oPickerobj.tableid).datagrid('getSelected');
    }


    if (oPickerobj.winid == 'winShadePicker') {
        if (oreturnObj != null) {
            var oFNLabDipDetail = $('#tblLabDipDetail').datagrid('getSelected');
            var nSelectedIndex = $('#tblLabDipDetail').datagrid('getRowIndex', oFNLabDipDetail);
            if (oFNLabDipDetail == null || oFNLabDipDetail.FNLabDipDetailID <= 0) {
                alert("Please select a item from list!");
                return;
            }
            oFNLabDipDetail.ShadeID_Ap = oreturnObj.ShadeID;
            oFNLabDipDetail.ShadeStr = oreturnObj.ShadeStr;
            UpdateShade(oFNLabDipDetail);
        }
        else {
            alert("Data not found.");
            return false;
        }
    }

    $("#" + oPickerobj.winid).icsWindow("close");
    $("#" + oPickerobj.winid).remove();
}
function UpdateShade(oFNLabDipDetail) {
    var obj =
      {
          BaseAddress: _sBaseAddress,
          Object: oFNLabDipDetail,
          ObjectId: oFNLabDipDetail.FNLabDipDetailID,
          ControllerName: "FNLabDipDetail",
          ActionName: "UpdateShade",
          TableId: "tblLabDipDetail",
          IsWinClose: true
      };
    $.icsSave(obj, function (response) {
        if (response.status) {
            //RowSelect(nSelectedIndex, response.obj);
        }
    });

}