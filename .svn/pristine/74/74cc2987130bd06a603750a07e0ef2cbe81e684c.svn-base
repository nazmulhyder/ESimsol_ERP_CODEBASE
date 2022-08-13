

function InitializeExportPIReportsEvents() {
    DynamicRefreshList(_oExportPIReports, "tblExportPIReports");

    $("#cboPIType").icsLoadCombo({
        List: _oPIPaymentTypes,
        OptionValue: "id",
        DisplayText: "Value"
    });

    //$("#cboTextileUnits").icsLoadCombo({
    //    List: _oTextileUnits,
    //    OptionValue: "id",
    //    DisplayText: "Value"
    //});

    $("#txtSearchByExportPI").keyup(function (e) {
        if (e.keyCode == 13) {
            var oExportPI = {
                TextileUnitInInt: parseInt($("#cboTextileUnits").val()),
                PINo : $("#txtSearchByExportPI").val()
            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _sBaseAddress + "/ExportPI/GetsBySearchKey",
                traditional: true,
                data: JSON.stringify(oExportPI),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var oExportPIs = jQuery.parseJSON(data);
                    if (oExportPIs != null) {
                        if (oExportPIs.length > 0) {
                            DynamicRefreshList(oExportPIs, "tblExportPIs");
                        }
                        else {
                            DynamicRefreshList([], "tblExportPIs");
                        }
                    } else {
                        DynamicRefreshList([], "tblExportPIs");
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }
        else {
            var nTextileUnitInInt = parseInt($("#cboTextileUnits").val());
            var txtSearchBy = $("#txtSearchByExportPI").val();
            var oCurrentLists = $("#tblExportPIs").datagrid("getRows");
            var sTempSearchString = "";
            var oSearchedLists = [];
            if (e.keyCode == 8) {
                oCurrentLists = _oExportPIs;
            }
            for (var i = 0; i < oCurrentLists.length; i++) {
                sTempSearchString = oCurrentLists[i].PINo;
                var n = sTempSearchString.toUpperCase().indexOf(txtSearchBy.toUpperCase());
                if (n != -1) {
                    if (parseInt(nTextileUnitInInt) > 0) {
                        if (oCurrentLists[i].TextileUnitInInt == parseInt(nTextileUnitInInt)) {
                            oSearchedLists.push(oCurrentLists[i]);
                        }
                    }
                    else {
                        oSearchedLists.push(oCurrentLists[i]);
                    }
                }
            }
            DynamicRefreshList(oSearchedLists, "tblExportPIs");
        }
    });

}
