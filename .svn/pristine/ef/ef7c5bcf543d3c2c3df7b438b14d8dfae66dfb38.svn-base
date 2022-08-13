var msservice = angular.module('ms.service', []); //'ui.grid.cellNav'

msservice.service('msModal', function ($uibModal) {
    this.call = function (modalProperty) {
        return $uibModal.open({
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: (modalProperty.size == undefined) ? 'sm' : modalProperty.size,
            templateUrl: (modalProperty.url == undefined) ? "" : modalProperty.url,
            controller: (modalProperty.modalcontroller == undefined) ? '' : modalProperty.modalcontroller,
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            resolve: {
                obj: function () {
                    return {
                        result: (modalProperty.objs == undefined) ? [] : modalProperty.objs,
                        multiSelect: (modalProperty.multiSelect == undefined) ? false : modalProperty.multiSelect,
                        columnDefs: (modalProperty.columns == undefined) ? [] : modalProperty.columns,
                        //write by Mahabub
                        enableHorizontalScrollbar: (modalProperty.enableHorizontalScrollbar == undefined) ? false : modalProperty.enableHorizontalScrollbar,
                        showVerticalScroll: (modalProperty.showVerticalScroll == undefined) ? false : modalProperty.showVerticalScroll,
                        pickertitle: (modalProperty.pickertitle == undefined) ? null : modalProperty.pickertitle,
                        objectFieldName: (modalProperty.objectFieldName == undefined) ? null : modalProperty.objectFieldName,
                        objectID: (modalProperty.objectID == undefined) ? 0 : modalProperty.objectID,
                        requestactionType: (modalProperty.requestactionType == undefined) ? null : modalProperty.requestactionType,
                        controllerName: (modalProperty.controllerName == undefined) ? null : modalProperty.controllerName,
                        controlleractionName: (modalProperty.controlleractionName == undefined) ? null : modalProperty.controlleractionName,
                        //write by Muneef
                        searchingbyfieldName: (modalProperty.searchingbyfieldName == undefined) ? null : modalProperty.searchingbyfieldName
                    };
                }
            }
        });
    }

    this.Instance = function (modalProperty) {

        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: (modalProperty.size == undefined) ? 'sm' : modalProperty.size,
            templateUrl: (modalProperty.url == undefined) ? sessionStorage.getItem('BaseAddress')+ '/Home/Modal' : modalProperty.url,
            controller: (modalProperty.modalcontroller == undefined) ? '' : modalProperty.modalcontroller,
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
            searcingPropertyName: (modalProperty.searcingPropertyName == undefined) ? '' : modalProperty.searcingPropertyName,
            searchbyfieldName: (modalProperty.searchbyfieldName == undefined) ? null : modalProperty.searchbyfieldName,
            //write by Muneef
            okButtonLabel: (modalProperty.okButtonLabel == undefined) ? 'Ok' : modalProperty.okButtonLabel
        }

        modalInstance.controller = function ($http, $filter, $scope, $document, $uibModalInstance, uiGridConstants) {
            $scope.modalOptionsGrid = {
                enableFullRowSelection: true,
                enableHighlighting: true,
                enableSorting: true,
                enableColumnResizing: true,
                //enableRowSelection: true,
                //enableRowHeaderSelection: false,
                showColumnFooter:modalProperty.showColumnFooter,
                enableSelectAll: modalProperty.multiSelect,
                multiSelect: modalProperty.multiSelect,
                enableHorizontalScrollbar: (modalProperty.enableHorizontalScrollbar ? uiGridConstants.scrollbars.ALWAYS : uiGridConstants.scrollbars.NEVER),
                //enableVerticalScrollbar : false,
                columnDefs: (modalProperty.columns.length > 0) ? modalProperty.columns : [],
                data: modalProperty.objs,
                onRegisterApi: function (gridApi) {
                    $scope.modalGridApi = gridApi;
                    $scope.modalGridApi.grid.modifyRows(modalProperty.objs);
                    $scope.selectRows(modalProperty.objs, modalProperty.rows, modalProperty.selection);

                    if (modalProperty.summation != undefined || modalProperty.summation != null){
                        gridApi.selection.on.rowSelectionChanged($scope, function (row)
                        {
                            if (Array.isArray(modalProperty.summation.field))
                                $scope.SetSummationOfArray(modalProperty.summation);
                            else
                                $scope.SetSummation(modalProperty.summation);
                        });
                    }
                }
            };


            //searcingPropertyName
            var sSearchFiled = (modalProperty.searcingPropertyName == undefined ? modalProperty.searchingbyfieldName : modalProperty.searcingPropertyName);
            $scope.SearchPickerKeyDown = function (e) {
                debugger;
                var code = (e.keyCode ? e.keyCode : e.which);
                //var sName = $.trim($scope.txtSearchfieldName);
                var txtSearchBy = $.trim($scope.txtSearchfieldName);
                var oSearchedLists = [];
                var sTempName = "";
                var oCurrentList = $scope.modalOptionsGrid.data;//   $("#" + defaultSettings.TableId).datagrid("getRows");

                if (e.which === 8) {
                    oCurrentList = modalProperty.objs;
                }

                if (sSearchFiled != undefined)
                {
                    for (var i = 0; i < oCurrentList.length; i++)
                    {
                        sTempName = oCurrentList[i][sSearchFiled];
                        var n = sTempName.toUpperCase().indexOf(txtSearchBy.toUpperCase());
                        if (n != -1) {
                            oSearchedLists.push(oCurrentList[i]);
                        }
                    }
                }
                //DynamicRefreshList(oSearchedLists, defaultSettings.TableId)
                $scope.modalOptionsGrid.data = oSearchedLists;
            };


            $scope.modalTitle = modalProperty.title;
           
            $scope.okButtonLabel = modalInstance.okButtonLabel;


            $scope.splitByUppercase = function (value) {
                return " by "+value.match(/[A-Z][a-z]+|[0-9]+/g).join(" ");
            }

            $scope.PlaceHolderName = modalProperty.searchbyfieldName == null ? 'Search' + (sSearchFiled == undefined ? '' : $scope.splitByUppercase(sSearchFiled)) : modalProperty.searchbyfieldName;


            $scope.selectRows = function (data, rows, selection) {
                if (rows != undefined && data.length > 0 && rows.length > 0) {
                    for (var i = 0; i < rows.length; i++) {
                        var select = {}; select[selection] = rows[i][selection];
                        var obj = $filter('filter')(data, select)[0];
                        if (obj != undefined)
                            $scope.modalGridApi.selection.selectRow(obj);
                    }
                }
            };
            $scope.SetSummation = function (sumObj) {
                var result = 0;
                var oItems = $scope.modalGridApi.selection.getSelectedRows();
                for(var i = 0; i < oItems.length; i++) {
                    result +=  oItems[i][sumObj.field];
                }
                result = parseFloat(result).toFixed(2);
                debugger;
                if (sumObj.label.indexOf("@" + sumObj.field) != -1) {
                    $scope.summationLabel = sumObj.label;
                    $scope.summationLabel = $scope.summationLabel.replace("@" + sumObj.field, (result == 0 ? "-" : result));
                }else {
                    $scope.summationLabel = sumObj.label;
                    $scope.summationValue = (result == 0 ? "-" : result);
                }
            };
            $scope.SetSummationOfArray = function (sumObj) {
                $scope.summationLabel = sumObj.label;
                var oItems = $scope.modalGridApi.selection.getSelectedRows();
                for (var j = 0; j < sumObj.field.length; j++)
                {
                    var result = 0;
                    for (var i = 0; i < oItems.length; i++) {
                        result+= oItems[i][sumObj.field[j]];
                    }
                    result = parseFloat(result).toFixed(2);

                    if (sumObj.label.indexOf("@" + sumObj.field[j]) != -1) {
                        $scope.summationLabel = $scope.summationLabel.replace("@" + sumObj.field[j], (result == 0 ? "-" : result));
                    } else {
                        $scope.summationValue = (result == 0 ? "-" : result)+" ";
                    }
                }
            };

            $scope.modalOptionsOk = function () {
                var oItems = $scope.modalGridApi.selection.getSelectedRows();
                if (oItems == null || oItems.length <= 0) {
                    alert("No selected item found.");
                    return false;
                }
                if (modalProperty.multiSelect) {
                    $uibModalInstance.close(oItems);
                }
                else {
                    $uibModalInstance.close(oItems[0]);
                }
            };

            $scope.modalOptionsCancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            $(document).keyup(function (e)
            {
                var oItems = $scope.modalGridApi.selection.getSelectedRows();
                if (oItems.length > 0 && e.which === 13)
                {
                    $scope.modalOptionsOk();
                }
                else if(e.which === 38 || e.which === 40)//UP or Down
                {
                    if (oItems.length > 0)
                    {
                        var Index = $scope.modalOptionsGrid.data.indexOf(oItems[0])
                        //$scope.modalGridApi.selection.unSelectRow(oItems);
                        if (oItems.length < 2)
                        {
                            if (e.which === 38 && Index > 0) {
                                $scope.modalGridApi.selection.clearSelectedRows();
                                $scope.modalGridApi.core.refresh();
                                $scope.modalGridApi.selection.selectRow($scope.modalOptionsGrid.data[--Index]);
                            }
                            else if (e.which === 40 && Index < $scope.modalOptionsGrid.data.length - 1) {
                                $scope.modalGridApi.selection.clearSelectedRows();
                                $scope.modalGridApi.core.refresh();
                                $scope.modalGridApi.selection.selectRow($scope.modalOptionsGrid.data[++Index]);
                            }
                        }
                    }
                    else
                        $scope.modalGridApi.selection.selectRow($scope.modalOptionsGrid.data[0]);
                }
            });
        }
        return $uibModal.open(modalInstance);
    }

    this.Message = function (modalProperty) {
        var modalInstance = {
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            size: (modalProperty.size == undefined) ? 'sm' : modalProperty.size,
            templateUrl: (modalProperty.url == undefined) ? _sBaseAddress + '/Home/ModalMessage' : modalProperty.url,
            controller: (modalProperty.modalcontroller == undefined) ? '' : modalProperty.modalcontroller,
            controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller,
        }

        var defaultProperty = {
            headeTitle: 'Proceed?',
            bodyText: 'Make sure, are you want to do it?',
            sucessText: ' Ok',
            cancelText: ' Cancel',
            feedbackType: true,
            autoClose: false,
        }

        modalInstance.controller = function ($http, $scope, $uibModalInstance, $timeout) {

            var modalMessage = {};
            angular.extend(modalMessage, defaultProperty, modalProperty);


            $scope.modalMessage = modalMessage;
            if ($scope.modalMessage.feedbackType) {
                $scope.modalMessage.autoClose = false;
                $scope.modalMessage.feedbackType = false; // show the button
            }
            else {
                $scope.modalMessage.feedbackType = true;
            }


            $timeout(function () {
                if (modalMessage.autoClose)
                    $uibModalInstance.close(true);
            }, 1000);

            $scope.modalMessageOk = function () {
                $uibModalInstance.close(true);
            };

            $scope.modalMessageCancel = function () {
                $uibModalInstance.close(false);
            };
        }
        return $uibModal.open(modalInstance);
    }
});

msservice.service('msGridControl', function ($uibModal) {


});

msservice.service('userSession', function () {

    this.SelectedIndex = function () {
        return "SelectedRowIndex";
    }

    this.clear = function () {

        var baseAddress = sessionStorage.getItem('BaseAddress');
        var nBUID = sessionStorage.getItem('BUID');
        var bIsSuperuser = sessionStorage.getItem('IsSuperUser');

        sessionStorage.clear();
        sessionStorage.setItem('BaseAddress', baseAddress);
        sessionStorage.setItem('BUID', nBUID);
        sessionStorage.setItem('IsSuperUser', bIsSuperuser);
    };

    this.setData = function (dataAs, obj) {
        var objs = sessionStorage.getItem(dataAs);
        var nIndex = parseInt(sessionStorage.getItem("SelectedRowIndex"));

        if (objs != null) {
            objs = jQuery.parseJSON(objs);
        }
        else {
            objs = [];
        }

        if (obj != null) {
            if (nIndex != -1) {
                objs[nIndex] = obj;
            }
            else {
                sessionStorage.setItem("SelectedRowIndex", objs.length);
                objs.push(obj);
            }
        }
        sessionStorage.setItem(dataAs, JSON.stringify(objs));
    };

    this.getData = function (dataAs) {
        var objs = sessionStorage.getItem(dataAs);
        if (objs != null && objs.length > 0) {
            objs = jQuery.parseJSON(objs);
        }
        else {
            objs = [];
        }
        return objs;

    };

    this.getRowIndex = function () {
        var index = sessionStorage.getItem("SelectedRowIndex");
        if (index == undefined || index == null)
            index = -1;
        else
            index = parseInt(index);

        return index;
    };

    this.previousPage = function () {
        window.location.href = sessionStorage.getItem("BackLink");
    };

    this.viewOption = function () {
        var viewType = sessionStorage.getItem("Operation");
        viewType = (viewType == null || viewType == '') ? '' : angular.lowercase(viewType);
        if (viewType == 'view')
            return true;
        else
            return false;

    };
});

msservice.service('icsMethod', function () {
    /*Write by: Md. Mahabub Alam; Description:Use for multiple Date criteria;Date:25 Jan 2017 */
    this.IsDateBoxdisabled = function (nIndex) {
        //Select-One- = 0, EqualTo = 1,NotEqualTo = 2,GreaterThen = 3,SmallerThen = 4,Between = 5,NotBetween = 6
        switch (nIndex) {
            case 0:
            case 1:
            case 2: return true;
            case 3:
            case 4:
            case 5:
            case 6: return false;
            default: return false;
        }

    }

    //Property Concation Write by : Md. Mahabub Alam
    this.ICS_PropertyConcatation = function (oList, sProperty) {
        var sIDs = "";
        if (oList.length > 0) {
            for (var i = 0; i < oList.length; i++) {
                var oTempField = oList[i];
                sIDs += oTempField[sProperty] + ",";
            }
            return sIDs.substring(0, sIDs.length - 1);
        }
        return sIDs;
    }

    //ICs Is Exist List, 
    this.ICS_IsExist = function (oList, PropertyName, PropertyValue)//developed by Mahabub
    {
        for (var i = 0; i < oList.length; i++) {
            var oTempList = oList[i];
            if (oTempList[PropertyName] == PropertyValue) {
                return true;
            }
        }
        return false;
    }

    //ICs Is Exist List,for 2 Property
    this.ICS_IsExistForTwoProperty = function (oList, PropertyName1, PropertyValue1, PropertyName2, PropertyValue2)//developed by Mahabub
    {
        for (var i = 0; i < oList.length; i++) {
            var oTempList = oList[i];
            if (oTempList[PropertyName1] == PropertyValue1 && oTempList[PropertyName2] == PropertyValue2) {
                return true;
            }
        }
        return false;
    }

    //ICs Is Exist List, for 3 Property
    this.ICS_IsExistForThreeProperty = function (oList, PropertyName1, PropertyValue1, PropertyName2, PropertyValue2, PropertyName3, PropertyValue3)//developed by Mahabub
    {
        for (var i = 0; i < oList.length; i++) {
            var oTempList = oList[i];
            if (oTempList[PropertyName1] == PropertyValue1 && oTempList[PropertyName2] == PropertyValue2 && oTempList[PropertyName3] == PropertyValue3) {
                return true;
            }
        }
        return false;
    }

    //ICS_DynamiObjectMake Write by : Md. Mahabub Alam
    this.ICS_DynamiObjectMake = function (sId, sField)
    {
        var obj = {};
        obj[sId] = 0;
        obj[sField] = "--Select One--";
        return obj;
    }
});

// ADDED BY : MUNEEF TIMU, 2017
msservice.service('advanceSearch', function ($uibModal) {
  
    /*
    ------------------------------------------------------------------------
    =============SAMPLE FUNCTION FOR ADVANCE SEARCH===(MAT)=================
    ------------------------------------------------------------------------
    
            $scope.AdvanceSearch=function()
            {
                ============================================================== ELEMENT LIST START ==============================================================
                var oElementList = [
                                    
                                    # --1-- # INPUT TYPE: "TEXT" (InputType: is not case sensitive)
                                    { DisplayName: "Challan No",  BOField: "ChallanNo",      InputType: 'text' },
                                    
                                    # --2-- # INPUT TYPE: "DATE"
                                            **** [1] 'CompareOperators' Has To Be Defined For Date Type (In Modal Object) ****
    
                                    { DisplayName: "Challan Date",BOField: "ChallanDate",    InputType: 'date' },
    
                                    # --3-- # INPUT TYPE: "PICKER"
                                            **** [1] 'PickerObject' Has To Be Assigned ***
    
                                    { DisplayName: "Contractor",  BOField: "ContractorID",   InputType: 'picker', PickerObject:paramObj_Contractor },
    
                                    ==================================PickerObject==================================
                                    var paramObj_Contractor={
                                        obj:{Params: '2,3' + '~' +'@@ContractorID'+'~'+sessionStorage.getItem('BUID')},
                                        objName:'Params',
    
                                        ****** a) '@@ContractorID' Will Be Replace With 'BOField' ID-Values ******
                                        ****** b) 'objName' Says Where To Replace ******
    
                                        url:_sBaseAddress+'/Contractor/ContractorSearchByNameType',
                                        title:'Contractor List',
                                        multiSelect:true,
                                        columns:[{ field: 'Name', name: 'Contractor Name' },{ field: 'Address', name: 'Contractor Address' }]
                                    }
                                    =================================================================================
    
                                     # --4-- # INPUT TYPE: "BOOL"
                                             **** [1] It Has To Be A Array Of 'DisplayName' & 'BOField' (div class will be defined by array.length)
    
                                    { DisplayName: ["Yet To Forward (HO)","Yet To Forward (Buyer)"],  BOField: ["YetToHO","YetToBuyer"],   InputType: 'bool'},
    
                                    # --5-- # INPUT TYPE: "BOOL"
                                            **** [1] It Has To Be A Array Of 'DisplayName' & 'BOField' (div class will be defined by array.length)
    
                                    { DisplayName: "SomeType",    BOField: "SomeType",       InputType: 'select', OptionList:[{id:2,Value:"KisuNai"}]} 
                                    
                                            *** a) OPTIONAL FOR 'select'
                                                        OptionValue:{id:'SomeId',Value:'SomeValue'},    (default:  OptionValues are id & Value **EnumType)
                                                        DefaultOption:'----SELECT SOMETHING----'        (default:  DefaultOption Is '----SELECT ONE----')

                                    # --6-- #INPUT TYPE: "HIDDEN"
                                            { BOField:     "BUID",      Value:nBUID, InputType: 'hidden'}
                                    }];
                ============================================================== ELEMENT LIST END ==============================================================
    
    
    
                ============================================================== MODAL OBJECT START ==============================================================
                var modalObj={
                    size:'md',
                    title:"Advance Search",
                    url:_sBaseAddress+'/Home/AdvanceSearch',
                    modalController:'ContractorModalCtrl',appController:'LabdipChallanCtrl',
                    
                    //** For Date Type Input
                    CompareOperators:oCompareOperators,
    
                    HtmlElements:oElementList,
    
                    isAdvanceSearch:true, 
                    //** a) if isAdvanceSearch TRUE: 'urlAdvanceSearch' should be define
                    //** b) if isAdvanceSearch FALSE: it will return a object with input values
    
    
                    //** a) 'AdvSearch': Is a Post Type Method Which can return 'Serialize' or 'MaxLength' JsonResult
                    //** b) 'AdvSearch': It's Parameter Is a Obejct
                    //** c) 'Object'   : [i]   if it's Property matches With 'BOField', they will contain the value
                                         [ii]  and also the Property 'Params' will contain the values as a string (like: ChallanNo+'~'+LabDipNo+'~'+Date)
                                         [iii] if the values are needed as a string, 'Params' Has to be a property of the Object
                                         
                    urlAdvanceSearch:_sBaseAddress + '/LabdipChallan/AdvSearch',
                }
                ============================================================== MODAL OBJECT END ==============================================================
    
    
                =================================================== INITIALIZE THE Modal Instance & GET THE RESULT ===========================================
                var modalInstance=advanceSearch.Instance(modalObj);
                modalInstance.result.then(function (result) 
                {
                    //** 1) if isAdvanceSearch TRUE: it will return the JsonResult
                     $scope.gridOptions.data=result;
    
                    //** 2) if isAdvanceSearch FALSE: it will return a object with input values
                }, 
                function () 
                {
                    $log.info('Adv Modal dismissed at: ' + new Date());
                });
                =================================================== Modal Instance & ADV SEARCH END ===========================================
    
            };
    */


    this.Instance = function (modalProperty) {

    var modalInstance = {
        ariaLabelledBy: 'modal-title',
        ariaDescribedBy: 'modal-body',
        //scope: scope,
        size: (modalProperty.size == undefined) ? 'sm' : modalProperty.size,
        templateUrl: (modalProperty.url == undefined) ? sessionStorage.getItem('BaseAddress') + '/Home/AdvanceSearch' : modalProperty.url,
        controller: (modalProperty.modalcontroller == undefined) ? '' : modalProperty.modalcontroller,
        controllerAs: (modalProperty.appcontroller == undefined) ? '' : modalProperty.appcontroller
    }

    modalInstance.controller = function ($http,$log, $scope, $sce, $document, $uibModalInstance, $compile, uiGridConstants, msModal) {
        //================ DATE ======================
        $(document).on('mousemove', '.modal-body', function () {
            $('.date-container').datepicker({
                format: "dd M yyyy",
                calendarWeeks: true,
                autoclose: true,
                todayHighlight: true
            });
        });

        //================ DECLARE ======================
        $scope.elements = [];
        $scope.SearchObject = {};
        $scope.lblLoadingMessage = true; $scope.Validation = false;
        $scope.btnHide_Ok=$scope.btnHide_Reset = $scope.btnHide_Search =true;
        //================ INIT ======================
        var oElementList = modalProperty.HtmlElements;
        $scope.modalTitle = modalProperty.title;
        $scope.isAdvanceSearch = (modalProperty.isAdvanceSearch == undefined ? false : modalProperty.isAdvanceSearch);
        $scope.allFieldDisabled = (modalProperty.isAdvanceSearch == undefined ? false : modalProperty.allFieldDisabled);
        $scope.urlAdvanceSearch= modalProperty.urlAdvanceSearch;
        $scope.CompareOperators = (modalProperty.CompareOperators == undefined) ? [] : modalProperty.CompareOperators;
        $scope.matData = (modalProperty.data == undefined) ? {} : modalProperty.data;
        debugger;
        if ($scope.isAdvanceSearch)
            $scope.btnHide_Reset = $scope.btnHide_Search = false;
        else if (!$scope.allFieldDisabled) $scope.btnHide_Ok = false;

        //================ INITIALIZATION HTML-Element (BIND-HTML) ======================
        for (var i = 0; i < oElementList.length; i++) {
            var trustedHtml = $sce.trustAsHtml(AddElement(oElementList[i], i));
            $scope.elements.push(trustedHtml);
        }

        //================ RESET ======================
        $scope.Reset=function()
        {
            for (var i = 0; i < oElementList.length; i++)
            {
                var oElement = oElementList[i];
                switch (oElement.InputType.toUpperCase())
                {
                    case 'HIDDEN':
                        $scope.SearchObject[oElement.BOField] = oElement.Value; break;
                    case 'TEXT':
                        $scope.SearchObject[oElement.BOField] = ""; break;
                    case 'PICKER':
                        $scope.SearchObject[oElement.BOField] = "";
                        $scope.SearchObject['txt' + oElement.BOField] = ""; break;
                    case 'DATE':
                        $scope.SearchObject['cbo' + oElement.BOField] = $scope.CompareOperators[0].id;
                        $scope.SearchObject[oElement.BOField + 'EndDisabled'] = true;
                        $scope.SearchObject[oElement.BOField + 'Start'] = $scope.SearchObject[oElement.BOField + 'End'] = icsdateformat(new Date());
                        break;
                    case 'BOOL': 
                        for (var j = 0; j < oElement.DisplayName.length; j++) {
                             $scope.SearchObject[oElement.BOField[j]]=false;
                        }
                        break;
                    case 'SELECT':
                        $scope.SearchObject[oElement.BOField] = 0; break;                }
            }
        }
       
        //================ ADD HTML-Element =======================
        function AddElement(oElement, index)
        {
            var oResult =   '<div class="row col-md-12">' +
                            '<div class="col-md-3 text-right"><label class="control-label">'+oElement.DisplayName+': </label></div>' +
                            '<div class="col-md-9 text-left">';
            switch(oElement.InputType.toUpperCase())
            {
                case 'HIDDEN':
                    $scope.SearchObject[oElement.BOField] = oElement.Value; return '<span></span>';
                case 'TEXT': oResult = oResult + '<input type="text" class="form-control" ng-model="SearchObject.' + oElement.BOField + '" placeHolder="Type ' + oElement.DisplayName + ' .." ng-disabled="allFieldDisabled"></div> ';
                             $scope.SearchObject[oElement.BOField] = ($scope.matData[oElement.BOField] == undefined ? "" : $scope.matData[oElement.BOField]);
                             break;

                case 'DATE': oResult = oResult  + '<div class="col-md-4 text-left"> <select class="form-control" ng-model="SearchObject.cbo' + oElement.BOField + '" ng-change="' + oElement.BOField + 'Change()" ng-options="item.id as item.Value for item in CompareOperators"></select></div>'
                                                + '<div class="col-md-8 text-left">'
                                                + '<div class="col-md-12"> <div class="input-group date date-container" style="width:43%;float:left">'
                                                + '<input type="text" class="form-control" ng-model="SearchObject.' + oElement.BOField + 'Start"><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span></div>'
                                                + '<div style="width:14%;float:inherit;"><span class="input-group-addon" style="height:26px;"><span class="label label-primary">To</span></span></div>'
                                                + '<div class="input-group date date-container" style="width:43%;float:left">'
                                                + '<input type="text" class="form-control" ng-model="SearchObject.' + oElement.BOField + 'End" ng-disabled="' + oElement.BOField + 'EndDisabled"><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>'
                                                + '</div> </div></div></div> ';
                                                $scope.SearchObject['cbo' + oElement.BOField] = $scope.CompareOperators[0].id;
                                                $scope.SearchObject[oElement.BOField + 'EndDisabled'] = true;
                                                $scope.SearchObject[oElement.BOField + 'Start'] = $scope.SearchObject[oElement.BOField + 'End'] = icsdateformat(new Date());
                                                break;

                case 'PICKER': oResult = oResult + '<div class="input-group">'
                                                    + '<input class="form-control" ng-model="$parent.SearchObject.txt' + oElement.BOField + '" placeholder="Type ' + oElement.DisplayName + ' & Press Enter" ng-disabled="disabled" ng-keydown="SearchKeyDown($event,' + index + ')"  ng-disabled="allFieldDisabled" />'
                                                    + '<span class="input-group-btn">'
                                                    + '<button type="button" class="btn btn-sm btn-primary" aria-label="Left Align" ng-click="PickPicker(' + index + ')"  ng-disabled="allFieldDisabled"> <span class="glyphicon glyphicon-search" aria-hidden="true"></span></button>'
                                                    + '</span></div></div> ';
                               $scope.SearchObject[oElement.BOField] = ($scope.matData[oElement.BOField] == undefined ? "" : $scope.matData[oElement.BOField]);
                               break;
                case 'BOOL':
                               oResult = '<div class="row col-md-12">' 
                               var colMd=12/oElement.DisplayName.length ;  
                               for (var i = 0; i < oElement.DisplayName.length; i++) 
                               {
                                   oResult = oResult + '<div class="col-md-' + colMd + ' text-right">' + '<input type="checkbox" ng-model="SearchObject.' + oElement.BOField[i] + '"  ng-disabled="allFieldDisabled">'
                                       + '<label class="control-label" style="padding-left:2px">' + oElement.DisplayName[i] + ' </label></div>';
                                   $scope.SearchObject[oElement.BOField[i]] = ($scope.matData[oElement.BOField] == undefined ? false : $scope.matData[oElement.BOField][i]);
                               }
                               break;

                case 'SELECT': oResult = oResult + '<select class="form-control" ng-model="SearchObject.' + oElement.BOField
                               + '" ng-options="item.' + (oElement.OptionValue == undefined ? "id" : oElement.OptionValue.id)
                               + ' as item.' + (oElement.OptionValue == undefined ? "Value" : oElement.OptionValue.Value) + ' for item in SearchObject.' + oElement.BOField + 's"  ng-disabled="allFieldDisabled">'
                               + '<option value="">' + (oElement.DefaultOption == undefined ? "----Select One----" : oElement.DefaultOption) + '</option></select></div>'
                               $scope.SearchObject[oElement.BOField] = ($scope.matData[oElement.BOField] == undefined ? 0 : $scope.matData[oElement.BOField]);
                               $scope.SearchObject[oElement.BOField + "s"] = oElement.OptionList;
                                   break; 


            }

            return oResult+'</div>';
        }

        //================ PICKER CODE START ======================
        $scope.SearchKeyDown= function (keyEvent,index) {
            if (keyEvent.which == 13) {
                $scope.PickPicker(index);
            }
            else if (keyEvent.which == 8) {
                $scope.SearchObject[oElementList[index].BOField] = "";
            }
        };
        $scope.PickPicker = function (index)
        {
            //alert("Hello Me mE :D ???"); return;
            $scope.SetPickerValue = function (result, index)
            {    
                debugger;
                if (result.length > 0) {
                    var oIds = result[0][$scope.opickerField]; //console.log(oIds);
                    for (var i = 1; i < result.length; i++) {
                        oIds += "," + result[i][$scope.opickerField];
                    }
                    $scope.SearchObject[oElementList[index].BOField] = oIds;

                    if(result.length>1)
                        $scope.SearchObject['txt' + oElementList[index].BOField] = result.length + " item(s) Selected";
                    else
                        $scope.SearchObject['txt' + oElementList[index].BOField] = result[0][oElementList[index].PickerObject.columns[0].field];
                } else
                {
                    $scope.SearchObject[oElementList[index].BOField] = result[$scope.opickerField];
                    $scope.SearchObject['txt' + oElementList[index].BOField] = result[oElementList[index].PickerObject.columns[0].field];
                }
            }

            var oPickerObject = oElementList[index].PickerObject;
            var oObjName = oPickerObject.objName;
            var txtField = $scope.SearchObject['txt' + oElementList[index].BOField];

             //=============SAMPLE DEFINATION FOR PICKER OBJECT==============================================================*/
            //    var paramObj_Contractor = {
           //    obj: { Params: '2,3' + '~' + '@@ContractorName' + '~' + sessionStorage.getItem('BUID') },
          //    objName: 'Params',
         //    url: _sBaseAddress + '/Contractor/ContractorSearchByNameType',
        //    title: 'Contractor List',
       //    multiSelect: false,
      //    columns: [{ field: 'Name', name: 'Contractor Name' }, { field: 'Address', name: 'Contractor Address' }]
     //}

            var oParam =
                {
                    urlObj: Object.assign({}, oPickerObject.obj),
                    urlObjData: oPickerObject.obj[oObjName],
                    url: oPickerObject.url,
                    title: oPickerObject.title,
                    multiSelect: oPickerObject.multiSelect,
                    columns: oPickerObject.columns,
                    callBack: $scope.SetPickerValue,
                    elementIndex:index
                }
            debugger;

            $scope.opickerField = (oPickerObject.objField == undefined ? oElementList[index].BOField : oPickerObject.objField);

            if (oPickerObject.searchObj!=undefined)
            for (var i = 0; i < oPickerObject.searchObj.length; i++)
            {
                if (oPickerObject.searchObj[i].field != undefined) { oParam.urlObj[oPickerObject.searchObj[i].field] = $scope.SearchObject[oPickerObject.searchObj[i].BOField]; }
            }

            oParam.urlObj[oObjName] = oParam.urlObjData.replace("@" + $scope.opickerField, (txtField == undefined ? "" : txtField));
            $scope.GetsPickerValue(oParam);
        };
        //================= DYNAMIC PICKER ========================
        $scope.GetsPickerValue = function (paramObj) {
            $scope.lblLoadingMessage = false;
            var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } };
            $http.post(paramObj.url, $.param(paramObj.urlObj), config).then(
                    function (response) {
                        $scope.lblLoadingMessage = true; debugger;
                        if (response.data == null || response.data.length <= 0) {
                            alert("No Data Found!!"); return;
                        }
                        var _results = (typeof (response.data[0]) === "object" ? response.data : JSON.parse(response.data));

                        if (_results == null || _results.length <= 0) { alert("No Data Found!!"); return; }

                        if (_results[0].ErrorMessage != null && _results[0].ErrorMessage != "") { alert(_results[0].ErrorMessage); return; }

                        var modalObj =
                        {
                            size: 'md', title: paramObj.title, url: _sBaseAddress + '/Home/Modal',
                            modalController: 'advModalCtrl', appController: paramObj.appController,
                            objs: _results, multiSelect: paramObj.multiSelect, columns: paramObj.columns
                        }

                        var modalInstance = msModal.Instance(modalObj);
                        modalInstance.result.then(function (result)
                        { paramObj.callBack(result, paramObj.elementIndex); }, function ()
                        { $log.info('Modal dismissed at: ' + new Date()); });
                    },
                    function (response)
                    { $scope.lblLoadingMessage = true; alert(response.statusText); }
            );
        };
        //================== PICKER END ===========================

        //=============== MAKE OBJECT-From-Element ================
        function MakeObject()
        {
            var refObj = new Object; 
            for (var i = 0; i < oElementList.length; i++)
            {
                refObj[oElementList[i].BOField] = GetFiledData(oElementList[i]);
            }
            return refObj;
        }
        //=============== GET DATE -From-Element ==================
        function GetFiledData(oElement)
        {
            switch (oElement.InputType.toUpperCase())
            {
                case 'TEXT':
                case 'PICKER':
                                if ($scope.SearchObject[oElement.BOField] != "")    $scope.Validation = true;   return $scope.SearchObject[oElement.BOField];
                                break;
                case 'DATE':
                                if ($scope.SearchObject['cbo' + oElement.BOField] > 0) $scope.Validation = true;
                                return $scope.SearchObject['cbo' + oElement.BOField] + '~' + $scope.SearchObject[oElement.BOField + 'Start'] + '~' + $scope.SearchObject[oElement.BOField + 'End']
                                break;
                case 'BOOL':    var result = "";
                                for (var i = 0; i < oElement.DisplayName.length; i++)
                                {
                                    if ($scope.SearchObject[oElement.BOField[i]]) $scope.Validation = true;
                                    if (i == 0)
                                        result = $scope.SearchObject[oElement.BOField[i]];
                                    else
                                        result =result+ '~' + $scope.SearchObject[oElement.BOField[i]]
                                }
                                return result;
                                break;
                case 'SELECT':
                                if ($scope.SearchObject[oElement.BOField] > 0) { $scope.Validation = true; return $scope.SearchObject[oElement.BOField]; }
                                else return 0;
                                break;
                case 'HIDDEN':
                    $scope.SearchObject[oElement.BOField] = oElement.Value; return $scope.SearchObject[oElement.BOField];  break;
            }
            return '';
        }

        //=============== BUTTON--MODAL ===========================
        $scope.modalOptionsSearch = function () {
            $scope.Validation = false;
            var result = MakeObject();  //console.log(result);
            if (!$scope.Validation && $scope.isAdvanceSearch)
            {
                alert("Please Select At Least One Searching Criteria!!"); return;
            }
            $scope.lblLoadingMessage = false;
            if ($scope.isAdvanceSearch)
                $scope.GetsSearchData(result);
            else
                $uibModalInstance.close(result);
        };
        $scope.modalOptionsReset = function ()
        {
            $scope.Reset();
        }
        $scope.modalOptionsCancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
       
        //================= SEARCH--DATA ==========================
        $scope.GetsSearchData = function (searchObj) {
            var oParams = "";
            for (var i = 0; i < oElementList.length; i++)
            {
                oParams += searchObj[oElementList[i].BOField]+'~'
            }
            searchObj.Params = oParams; searchObj.ErrorMessage = oParams;
            sessionStorage.setItem("AdvSearchString", oParams); sessionStorage.setItem("AdvSearchObject", JSON.stringify(searchObj));
            var config = { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } }; 
            $http.post($scope.urlAdvanceSearch, $.param(searchObj), config).then(
                    function (response) {
                        $scope.lblLoadingMessage = true; debugger;
                        if (response.data == null || response.data.length <= 0) {alert("No Data Found!!"); return;}
                  
                        var _results = (typeof (response.data[0]) === "object" ? response.data : JSON.parse(response.data));

                        if (_results == null || _results.length <= 0) { alert("No Data Found!!"); return; }
                        if (_results[0].ErrorMessage != null && _results[0].ErrorMessage != "") { alert(_results[0].ErrorMessage); return; }

                        $uibModalInstance.close(_results);
                    },
                    function (response)
                    { alert(response.statusText); $scope.lblLoadingMessage = true; }
            );
        }
    }
    return $uibModal.open(modalInstance);
    }

});

// ADDED BY : MUNEEF TIMU, 2017
msservice.directive('compileTemplate', function ($compile, $parse) {
    return {
        link: function (scope, element, attr)
        {
            var parsed = $parse(attr.ngBindHtml);
            function getStringValue()
            {
                return (parsed(scope) || '').toString();
            }
            // Recompile if the template changes
            scope.$watch(getStringValue, function ()
            {
                $compile(element, null, -9999)(scope);  // The -9999 makes it skip directives so that we do not recompile ourselves
            });
        }
    }
});

// ADDED BY : MUNEEF TIMU, 2018
msservice.directive('icsTab', function ($timeout) {
    return {
        restrict: 'A',
        require: '^ngModel',    
        link: function (scope, element, attrs, ngModel)
        {
            //console.log("From icsTab SEPT: ", scope, attrs);
            if (parseInt(attrs.icsTab) > 0)
            {
                var tabclass = "ics-tab-" + parseInt(attrs.icsTab);
                element.addClass(tabclass);

                if (parseInt(attrs.icsTab) == 1)
                    $timeout(function () { angular.element(document.querySelector('.' + tabclass)).focus(); }, 0);

                element.on('keypress', function (event)
                {
                    scope.$apply(function () {
                        scope.ngModel = element.val();
                        scope.$eval(attrs.cstInput, { 'answer': scope.ngModel });
                    });

                    if (event.which === 13)
                    {
                        var tabindex = parseInt(attrs.icsTab) + parseInt(1);
                        var tabclass = "ics-tab-" + tabindex;
                        $timeout(function () { angular.element(document.querySelector('.' + tabclass)).focus(); }, 0);
                        //angular.element(document.querySelector('.ics-tab-' + parseInt(attrs.icsTab))).blur();
                        return;
                    }
                });
            }
        }
    };
})

// ADDED BY : MUNEEF TIMU, 2018 [UNDER CONSTRUCTION]
msservice.directive('icsGridEvent', function ($timeout) {
    return {
        //restrict: 'A',
        require: '^uiGrid',
        scope: false,
        link: function (scope, element, attrs, uiGridCtrl)
        {
            console.log("  icsGridEvent : ", element);
            var recursiveBind = function (recursiveElement)
            {
                console.log("From recursiveBind : ", recursiveElement[0].childNodes);

                recursiveElement.on('keypress', function (event) {

                    scope.$apply(function () {
                        scope.$eval(attrs.cstInput, { 'answer': scope.ngModel });
                    });
                    console.log("keypress From icsTab SEPT: ", scope, attrs);
                });

                recursiveElement.on('click', function (event) {

                    scope.$apply(function () {
                        scope.$eval(attrs.cstInput, { 'answer': scope.ngModel });
                    });
                    console.log("click From icsTab: ", scope, attrs);
                });

                //recursiveBind(recursiveElement.context);
                
                for (var i = 0; i < recursiveElement.childNodes.length; i++) recursiveBind(recursiveElement.childNodes[i]);

                //while (recursiveElement.nextElementSibling != null)
            }
            recursiveBind(element);
            //recursiveBind(element[1]);
            //recursiveBind(element);
        }
    };
})

var uiKeyUpDown = angular.module('uiKeyUpDown', []);
uiKeyUpDown.directive('uiGridKeyNav', ['$compile', 'gridUtil', uiGridKeyNav]);
function uiGridKeyNav($compile, gridUtil)
{
    console.log('uiGridKeyNav');
    return {

        require: '^uiGrid',
        scope: false,
        link: function ($scope, $elm, $attrs, uiGridCtrl)
        {
            debugger;
            var grid = uiGridCtrl.grid;
            var focuser = $compile('<div class="ui-grid-focuser" role="region" aria-live="assertive" aria-atomic="false" tabindex="0" aria-controls="' + grid.id + '-aria-speakable ' + grid.id + '-grid-container' + '" aria-owns="' + grid.id + '-grid-container' + '"></div>')($scope);

            //element.find('div.list-scrollable')

            $elm.append(focuser);

            $elm.bind('click', function ()
            {
                gridUtil.focus.byElement(focuser[0]);

                console.log('click', gridUtil.focus.byElement(focuser[0]));
                //grid.api.selection.selectRowByVisibleIndex(grid.api.selection.getSelectedRows());
            });

            focuser.bind('keydown', function (e)
            {
                $scope.$apply(function () {
                    var selectedEntities,
                        visibleRows,
                        selectedIndex;

                    console.log('keydown');
                    if (e.keyCode !== 38 && e.keyCode !== 40)
                        return;

                    selectedEntities = grid.api.selection.getSelectedRows();

                    if (selectedEntities.length === 1) {
                        visibleRows = grid.getVisibleRows();

                        selectedIndex = visibleRows.map(function (item) {
                            return item.entity;
                        }).indexOf(selectedEntities[0]);

                        if (selectedIndex < 0)
                            return;

                        if (e.keyCode === 38 && selectedIndex > 0)
                            --selectedIndex;

                        if (e.keyCode === 40 && selectedIndex < grid.options.data.length - 1)
                            ++selectedIndex;

                        grid.api.selection.selectRowByVisibleIndex(selectedIndex);
                        grid.api.core.scrollToIfNecessary(visibleRows[selectedIndex], grid.columns[0]);
                    }
                });
            });
        }
    };
}
