var _oCostCenters = [];
var _oCostCenter = {};
var _bMultipleItemReturn = false;

function InitializeCostCenterEvents() {
    
    

   
    

    

    $("#btnCloseCostCenter").click(function () {
       

        $("#winCostCenter").icsWindow('close');
       
    });
}




function RefreshControlCostCenter() {
    debugger;
    var oCostCenter = {
            CCID: 0

        }
        $.ajax({
            type: "POST",
            dataType: "json",
            url:_sBaseAddress+"/CostCenter/GetCostCenter",
            data: JSON.stringify({oCostCenter:oCostCenter, bSearchWithUserPermission: true}),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                //debugger;
                var oCostCenters = jQuery.parseJSON(data);
                if (oCostCenters != null) {
                    $('#TreeCostCenter').tree({
                        data: [oCostCenters]
                    });
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
}



