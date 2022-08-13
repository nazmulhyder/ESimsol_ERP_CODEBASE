
(function ($) {
   
    var oActiveWindows = [];
    $.fn.icsCurrencyBox = function (action, value, dpoint) {
        $(this).css("text-align", "right");
        $(this).focus(function () {
            $(this).val(icsRemoveComma($(this).val(),dpoint));
            //$(this).css("text-align", "left");
            $(this).select();
        });

        $(this).focusout(function () {
            $(this).val(icsFormatPrice($(this).val(), null, dpoint));
        });

        $(this).keydown(function (e) {
            if (e.which != 109 &&  e.which != 9 && e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57) && (e.which < 96 || e.which > 105) && e.which != 110 && e.which != 190) {
                return false;
            }
            else {

                if ($(this).val() != '0.00' && $(this).val().toString().split('.').length > 1) {
                    if (e.which == 110 || e.which == 190) {
                        return false;
                    }
                }
            }
        });
    };

    $.fn.icsWindow = function (action, title) {
        /*
            When Use : It can close a window by pressing ESC button.

            Advantages : 1. Can close window by pressing on ESC button.
                         2. Can set window title.

            Operation Steps : 

                1.  First add below line under $(document).ready function of main view page
                    $(document).keydown(function (e) { if (e.keyCode === 27) { $('div').icsWindow("close")}});

                2.  Declaration : $("#winId").icsWindow("open","title");  //Open a window
                                  $("#winId").icsWindow("close");         //Close a window

        */

        if (!$.trim(action.length)) { alert("icsWindow parameter value not found. Use 'open' or 'close'"); return }
        if (action.toLowerCase() != "open" && action.toLowerCase() != "close") { alert("icsWindow parameter value must be 'open' or 'close'."); return; }
         
        if (action.toLowerCase() == "open") {
            var oActiveWindow =
            {
                WinId: $(this).attr("id"),
                Index: (oActiveWindows.length) + 1
            };
            oActiveWindows.push(oActiveWindow);
            $(this).window("open");
            $(this).window("setTitle", title);
        }
        else if (action.toLowerCase() == "close") {
            IcsWindowClose(oActiveWindows);
        }
    };


    $.fn.icsNumberField = function (options) {

        // debugger;
        /*
             When Use : Make an input field number box.

             Advantages : 1. min : Can avoid floating number. (Example : if min = 0 then this input field will avoid number under 0)
                          2. precision : will decide how many digits will be after decimal point(dot)
                          3. If "precision" is not declear then this input field will take only int number. (Avoid float number)

             Lackings : 1. Can't take negative value

             Operation Steps : 
 
                    1.  Declaration : $("#myNumberBox").icsNumberField({
                                        min: anyNumberValue,
                                        max: anyNumberValue,
                                        precision: anyNumberValue
                                    });
                        N.B : Here we will use only the properties that we need.
                        If we don't need any property then the declaration will be : $("#myNumberBox").icsNumberField();
         */

        options = $.extend({
            min: null,
            max: null,
            precision: 0
        }, options);
        var nValue = 0;

        $(this).keypress(function (event) {
            //debugger;
            if (event.which === 0 || event.which === 8 || event.which === 9 || event.which === 37 || event.which === 39 || (event.which === 46 && options.precision > 0 && $(this).val().indexOf('.') == -1)) {
                return true;
            }
            else if ((event.which < 48) || event.which > 57) {
                event.preventDefault();
            }
            else {
                nValue = parseFloat((($(this).val() == null || $(this).val() == "") ? "" : $(this).val()) + event.key);
                //if (options.min != null && options.max == null && parseFloat(nValue) < parseFloat(options.min)) { event.preventDefault();}
                if ($(this).val().indexOf('.') != -1 && parseInt($(this).val().toString().split(".")[1].length) >= parseInt(options.precision)) { event.preventDefault(); }
                else if (options.min != null && options.max == null && parseFloat(nValue) < parseFloat(options.min)) { event.preventDefault(); }
                else if (options.min == null && options.max != null && parseFloat(nValue) > parseFloat(options.max)) { event.preventDefault(); }
            }
            $(this).focusout(function () { ($(this).val() == null || $(this).val() == "") ? $(this).val("") : $(this).val($(this).val()); });
        });

    };

    $.fn.icsLoadCombo = function (options) {
        /*
          When Use : Load an HTML Combo box.

          Operation Steps : 

                 1.  Declaration : $("#myComboId").icsLoadCombo({
                                    List: objCollectionList,
                                    OptionValue: "objectId",
                                    DisplayText: "objectName"
                                 });
       */
        //debugger;
        var defaultSettings = $.extend({
            List: [],
            OptionValue: "",
            DisplayText: "",
            InitialValue:"Default"
        }, options);
       
        var listItems = "";

        if (defaultSettings.List == null)
            defaultSettings.List = [];

        var Items = defaultSettings.List;
        //debugger;

        if (defaultSettings.InitialValue == "Default")
        {
            if (ICS_IsExist(Items, defaultSettings.OptionValue, '-1')) {
                listItems += "<option value=-1>" + "--Select One--" + "</option>";
            } else {
                listItems += "<option value=0>" + "--Select One--" + "</option>";
            }            
            for (var i = 0; i < Items.length; i++)
            {
                if (Items[i][defaultSettings.OptionValue] != 0)
                {
                    listItems += "<option value='" + Items[i][defaultSettings.OptionValue] + "'>" + Items[i][defaultSettings.DisplayText] + "</option>";
                }
            }
        }
        else if (defaultSettings.InitialValue == "Fixed")
        {
            for (var i = 0; i < Items.length; i++)
            {
              listItems += "<option value='" + Items[i][defaultSettings.OptionValue] + "'>" + Items[i][defaultSettings.DisplayText] + "</option>"; 
            }
        }
        else {//Customize
            //debugger;
            listItems = "";
            if (Items.length > 0) {
                if (ICS_IsExist(Items, defaultSettings.OptionValue, '-1')) {
                    listItems += "<option value=-1>" + defaultSettings.InitialValue + "</option>";
                } else {
                    listItems += "<option value=0>" + defaultSettings.InitialValue + "</option>";
                }
            }
            for (var i = 0; i < Items.length; i++) {
                if (Items[i][defaultSettings.OptionValue] > 0) {
                    listItems += "<option value='" + Items[i][defaultSettings.OptionValue] + "'>" + Items[i][defaultSettings.DisplayText] + "</option>";
                }
            }
        }
        $(this).html(listItems);
    };

    $.fn.icsUpDownEvent = function (options) {
        //Up down key control in a collection table
        var defaultSettings = $.extend({
            Event: "",
            IsChecked:false
        }, options);

        if (!$.trim(defaultSettings.Event).length) {
            alert("Please give function Event as property.");
            return false;
        }

        if (!defaultSettings.IsChecked)
        {
            var oSelectedObject = $(this).datagrid("getSelected");
            var nIndex = $(this).datagrid("getRowIndex", oSelectedObject);
            if (defaultSettings.Event.which === 38)//up arrow=38
            {
                if (nIndex <= 0) {
                    $(this).datagrid("selectRow", 0);
                }
                else {
                    $(this).datagrid("selectRow", nIndex - 1);
                }
            }
            else if (defaultSettings.Event.which === 40)//down arrow=40
            {
                var oCurrentList = $(this).datagrid("getRows");
                if (nIndex >= oCurrentList.length - 1) {
                    $(this).datagrid("selectRow", oCurrentList.length - 1);
                }
                else {
                    $(this).datagrid("selectRow", nIndex + 1);
                }
            }
        }
        else {
            var nIndex = parseInt($(this).datagrid("getChecked").length);
            if (defaultSettings.Event.which == 38) {

                if (nIndex <=0 ) {
                    $(this).datagrid("uncheckRow", 0);
                    //$(this).datagrid("unselectRow", 0);
                }
                else if (nIndex > 0) {
                    $(this).datagrid("uncheckRow", nIndex-1);
                   // $(this).datagrid("unselectRow", nIndex - 1);
                   // if (nIndex >= 2) { $(this).datagrid("selectRow", nIndex - 2); } 
                }

            }
            else if (defaultSettings.Event.which == 40) {
                if(nIndex<=0){
                    $(this).datagrid("checkRow", 0);
                    //$(this).datagrid("selectRow", 0);
                }
                else if (parseInt($(this).datagrid("getRows").length) > nIndex) {
                    $(this).datagrid("checkRow", nIndex);
                    //$(this).datagrid("selectRow", nIndex);
                }
            }

        }

    };

    $.fn.icsSearchByText = function (options) {
        //Write text in a input text field and sort list from a collection by string match.
        //Example : In bank menu : this plaugin use in "Search by Bank code" or "Search by Bank Name" input text fields
        var defaultSettings = $.extend({
            Event: "",
            SearchProperty: "",
            GlobalObjectList: "",
            TableId: ""
        }, options);

        var txtSearchBy = $(this).val();
        var oSearchedLists = [];
        var sTempName = "";
        var oCurrentList = $("#" + defaultSettings.TableId).datagrid("getRows");
       
        if (defaultSettings.Event.which === 8) {
            oCurrentList = defaultSettings.GlobalObjectList;
        }
        for (var i = 0; i < oCurrentList.length; i++) {
            sTempName = oCurrentList[i][defaultSettings.SearchProperty];
            var n = sTempName.toUpperCase().indexOf(txtSearchBy.toUpperCase());
            if (n != -1) {
                oSearchedLists.push(oCurrentList[i]);
            }
        }
        DynamicRefreshList(oSearchedLists, defaultSettings.TableId);
    };

    $.icsCheckValidation = function (options, callback) {
        var defaultSettings = $.extend({
            FieldID : "",
            AlertMessage: "Please enter some value.",
            ShowAlert: false
        }, options);
        if (!$.trim($(this).val()).length) {
            alert(defaultSettings.AlertMessage);
            $("#" + FieldID).val("");
            $("#" + FieldID).focus();
            $("#" + FieldID).addClass("errorFieldBorder");
            if (callback != null) { return callback({ status: false, obj: obj }); }
        } else {
            $("#" + FieldID).removeClass("errorFieldBorder");
            if (callback != null) { return callback({ status: true, obj: obj }); }
        }
    };

    /*---------------Auto Complete------------------*/
    $.fn.icsAutoComplete = function (options, callback) {
        //debugger;
        /*
            BaseAddress: "", //the base address from session// optional if baseaddress is set in layout page
            ControllerName: "", //the controller to use
            ActionName: "", //the action in the controller// needs to be post type
            Object: "", //the object to pass to the action.{ParamName:''}
            PropertyName: "", //name of the property to show in the list and in the textbox
            ParamName: "", //name of the porperty of the Object that will hold entered text to use as searching parameter
            PreParam: "", //string to add infront of the parameter to make use of query handling functions// optional
            PostParam:"", //string to add at the end of the parameter to make use of query handling functions// optional
            CharCount: 2, //number of characters after database is accessed// optional
            Columns:[] // if more than one column is needed. bigger textbox means wider list[{field:nameofproperty,width:widthinpercentaage},{field:'EmployeeName',width:'60%'}]// optional

            callback //only called on Enter KeyDown Event// contains an Object with the selected Object as an parameter called: obj
        */
        CheckDuplicateIds(this[0]);
        var defaultSettings = $.extend({
            BaseAddress: "",
            ControllerName: "",
            ActionName: "",
            Object: "",
            PropertyName: "",
            ParamName: "",
            PreParam: "",
            PostParam: "",
            CharCount: 2,
            Columns: []
        }, options);
        $(this).unbind('keydown');
        $(this).unbind('keyup');
        $(this).unbind('focusout');
        $(this).prop('autocomplete', 'off');
        $(this).keydown(function (e) {
            //debugger;
            //if ($(this).data('IsBusy')) { return false; }
            if (e.which != 9 && e.which != 13 && !(e.which >= 16 && e.which <= 20) && e.which != 27 && !(e.which >= 33 && e.which <= 40) && e.which != 45 && !(e.which >= 91 && e.which <= 93) && !(e.which >= 106 && e.which <= 123) && !(e.which >= 144 && e.which <= 145)) {
                $(this).removeClass('fontColorOfPickItem');
                $(this).data('obj', null);
            }

            if (e.which === 9 || e.which === 27)//tab=9,escape=27
            {
                if ($("#divICSTestboxDataSearch").length) {
                    IcsWindowClose(oActiveWindows);
                    $('#divICSTestboxDataSearch').remove();
                    $(this).data('autocomplete', 'close');
                }
            }
            if (!$("#divICSTestboxDataSearch").length) { return; }
            var selected = $('#tblSuggestions').datagrid('getSelected');
            var nIndex = $('#tblSuggestions').datagrid('getRowIndex', selected);

            if (e.which === 38)//up arrow=38
            {
                if (nIndex <= 0) {
                    $('#tblSuggestions').datagrid('selectRow', 0);
                }
                else {
                    $('#tblSuggestions').datagrid('selectRow', nIndex - 1);
                }
            }
            if (e.which === 40)//down arrow=40
            {
                var oCurrentList = $('#tblSuggestions').datagrid('getRows');
                if (nIndex >= oCurrentList.length - 1) {
                    $('#tblSuggestions').datagrid('selectRow', oCurrentList.length - 1);
                }
                else {
                    $('#tblSuggestions').datagrid('selectRow', nIndex + 1);
                }
            }
            if (e.which === 13)//enter=13
            {
                if ($(this).data('obj') != null) {
                    if ($("#divICSTestboxDataSearch").length) {
                        IcsWindowClose(oActiveWindows);
                        $('#divICSTestboxDataSearch').remove();
                        $(this).data('autocomplete', 'close');
                    }
                    if (callback != null) { return callback({ obj: $(this).data('obj') }); }
                }
            }
        }).keyup(function (e) {
            //debugger;
            /*
            skipped KeyCodes
            tab	9,enter	13,shift	16,ctrl	17,alt	18,pause/break	19,caps lock	20,escape	27,page up	33,page down	34,end	35,home	36,left arrow	37,up arrow	38,right arrow	39,down arrow	40,
            insert	45,left window key	91,right window key	92,select key	93,multiply	106,add	107,subtract	109,decimal point	110,divide	111,f1	112,f2	113,f3	114,f4	115,f5	116,
            f6	117,f7	118,f8	119,f9	120,f10	121,f11	122,f12	123,num lock	144,scroll lock	145
            */
            if (e.which === 9 || e.which === 13 || e.which >= 16 && e.which <= 20 || e.which === 27 || e.which >= 33 && e.which <= 40 || e.which === 45 || e.which >= 91 && e.which <= 93 || e.which >= 106 && e.which <= 123 || e.which >= 144 && e.which <= 145) { return false; }
            var TextBox = $(this);
            var sEnteredTex = $.trim($(this).val());
            if (sEnteredTex === null || sEnteredTex === "") {
                $(TextBox).data('SearchText', sEnteredTex);
                if ($("#divICSTestboxDataSearch").length) {
                    IcsWindowClose(oActiveWindows);
                    $('#divICSTestboxDataSearch').remove();
                    $(this).data('autocomplete', 'close');
                }
                return false;
            }
            else {
                if (sEnteredTex.length >= defaultSettings.CharCount) {
                    defaultSettings.Object[defaultSettings.ParamName] = defaultSettings.PreParam;
                    defaultSettings.Object[defaultSettings.ParamName] = defaultSettings.Object[defaultSettings.ParamName] + sEnteredTex + defaultSettings.PostParam;
                    if ($(TextBox).data('DataList') && $(TextBox).data('SearchText') && sEnteredTex.toUpperCase().indexOf($(TextBox).data('SearchText').toUpperCase()) === 0) {
                        var objs = IcsAutoCompleteSearchByText({ SearchProperty: defaultSettings.PropertyName, GlobalObjectList: $(TextBox).data('DataList'), Columns: defaultSettings.Columns, TextBox: TextBox });
                        ICSAutoCompleteWindowGenerate(TextBox, defaultSettings, objs, callback);
                    }
                    else {
                        //$(TextBox).data('IsBusy', true);
                        $.icsDataGets({
                            BaseAddress: defaultSettings.BaseAddress,
                            Object: defaultSettings.Object,
                            ControllerName: defaultSettings.ControllerName,
                            ActionName: defaultSettings.ActionName,
                            IsWinClose: false
                        }, function (response) {
                            //$(TextBox).data('IsBusy', false);
                            if (response.status && response.objs.length) {
                                var objs = [];
                                for (var i = 0; i < response.objs.length; i++) {
                                    if (response.objs[i][defaultSettings.PropertyName] != null && response.objs[i][defaultSettings.PropertyName].length) {
                                        objs.push(response.objs[i]);
                                    }
                                }
                                $(TextBox).data('DataList', objs);
                                ICSAutoCompleteWindowGenerate(TextBox, defaultSettings, objs, callback);
                            }
                        });
                    }
                    if (sEnteredTex.length === defaultSettings.CharCount && $(TextBox).data('SearchText') !== sEnteredTex) {
                        $(TextBox).data('SearchText', sEnteredTex);
                    }
                }
            }
        }).focusout(function (e) {
            if ($(this).data('obj') != null) {
                if ($("#divICSTestboxDataSearch").length) {
                    IcsWindowClose(oActiveWindows);
                    $('#divICSTestboxDataSearch').remove();
                    $(this).data('autocomplete', 'close');
                }
                if (callback != null) { return callback({ obj: $(this).data('obj') }); }
            }
        });




    };


    /*--------------- Data Grid Related-------------*/

    $.fn.icsDataGridMultiSelect = function (bValue)
    {
        if (!bValue) { $(this).datagrid('hideColumn', 'Selected'); $(this).datagrid({ selectOnCheck: true, checkOnSelect: true }); }
        else { $(this).datagrid({ selectOnCheck: false, checkOnSelect: false }); }
    };

    $.fn.icsGetSelectedItem = function () {
        /*
         Pourpose: To get selected row from datagrid and it's return always obj when data found.
         How to Use: $('#tabelId').icsGetSelectedItem();
       */
        var obj = $(this).datagrid("getSelected");
        if (obj == null) { alert("Please select an item!"); return obj; }
        IcsWindowClose(oActiveWindows);
        return obj;
    };

    $.fn.icsGetCheckedItem = function () {
        /*
         Pourpose: To get cheecked rows from datagrid and it's return always list when data found.
         How to Use: $('#tabelId').icsGetCheckedItem();
       */
        var objs = $(this).datagrid("getChecked");
        if (objs == null || objs.length == 0) { alert("No checked item found."); return objs; }
        IcsWindowClose(oActiveWindows);
        return objs;
    };


    /*--------------- Data Get -------------*/

    $.icsSave = function (options, callback) {
        var defaultSettings = $.extend({
            BaseAddress: "",
            Object: "",
            ObjectId: "",
            ControllerName: "",
            ActionName: "",
            TableId: "",
            IsWinClose: "",
            Message: "Save Successfully."
        }, options);

        var isEdit = defaultSettings.ObjectId === 0 ? false : true;
        var isErrorFound = false;
        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                //debugger;
                var obj = jQuery.parseJSON(data);
                if (obj != null) {
                    if (!$.trim(obj.ErrorMessage).length) {
                        if (defaultSettings.Message != "") { alert(defaultSettings.Message); }
                        if (!isEdit) {
                            if ($.trim(defaultSettings.TableId).length) {
                                var oCollections = $("#" + defaultSettings.TableId).datagrid("getRows");
                                var nIndex = oCollections.length;
                                $("#" + defaultSettings.TableId).datagrid("appendRow", obj);
                                $("#" + defaultSettings.TableId).datagrid("selectRow", nIndex);
                            }
                        }
                        else {
                            if ($.trim(defaultSettings.TableId).length) {
                                var oSelectedObject = $("#" + defaultSettings.TableId).datagrid("getSelected");
                                var nRowIndexBank = $("#" + defaultSettings.TableId).datagrid("getRowIndex", oSelectedObject);
                                $("#" + defaultSettings.TableId).datagrid("updateRow", { index: nRowIndexBank, row: obj });
                            }
                        }
                    }
                    else { alert(obj.ErrorMessage); isErrorFound = true; }
                }
                else { alert("Operation Unsuccessful."); }
                if (defaultSettings.IsWinClose && !isErrorFound) { IcsWindowClose(oActiveWindows); };
                if (callback != null) { return callback({ status: true, obj: obj }); }
            },
            error: function (xhr, status, error) {
                if (callback != null) { return callback({ status: false, obj: obj }); }
            }
        });
    };

    $.icsDelete = function (options, callback) {
        var defaultSettings = $.extend({
            BaseAddress: "",
            Object: "",
            ControllerName: "",
            ActionName: "",
            TableId: "",
            ShowMessage:true,
            IsWinClose: ""
        }, options);

        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var sMessage = jQuery.parseJSON(data);
                if (sMessage != null) {
                    if (sMessage.toLowerCase() == "deleted") {
                     
                        if (defaultSettings.ShowMessage)
                            alert("Delete successfully.");
                        if ($.trim(defaultSettings.TableId).length) {
                            var oSelectedObject = $("#" + defaultSettings.TableId).datagrid("getSelected");
                            var nRowIndexBank = $("#" + defaultSettings.TableId).datagrid("getRowIndex", oSelectedObject);
                            $("#" + defaultSettings.TableId).datagrid("deleteRow", nRowIndexBank);
                        }

                    }
                    else {
                        alert(sMessage);
                    }
                }
                else { alert("Operation Unsuccessful."); }
                 
                if (defaultSettings.IsWinClose) { IcsWindowClose(oActiveWindows); };
                if (callback != null) { return callback({ status: true, Message: sMessage.toLowerCase() }); }
            },
            error: function (xhr, status, error) {
                alert(error);
                if (callback != null) { return callback({ status: false, Message: "" }); }
            }
        });
    };

    $.icsDataGet = function (options, callback) {
        var defaultSettings = $.extend({
            BaseAddress: "",
            Object: "",
            ControllerName: "",
            ActionName: "",
            IsWinClose: ""
        }, options);
         
        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var obj = jQuery.parseJSON(data);
                if (defaultSettings.IsWinClose) { IcsWindowClose(oActiveWindows); };
                return callback({ status: true, obj: obj });
            },
            error: function (xhr, status, error) {
                return callback({ status: false, obj: null });
            }
        });
    };

    $.icsDataGets = function (options, callback) {
        var defaultSettings = $.extend({
            BaseAddress: "",
            Object: "",
            ControllerName: "",
            ActionName: "",
            IsWinClose: ""
        }, options);
        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var objs = [];
                if (typeof (data) === "string")
                {
                    objs = jQuery.parseJSON(data);
                }
                else {
                    objs = data;
                }
                if (defaultSettings.IsWinClose) { IcsWindowClose(oActiveWindows); };
                return callback({ status: true, objs: objs });
            },
            error: function (xhr, status, error) {
                
                alert(error);
                return callback({ status: false, obj: null });
            }
        });
    };

    $.icsMaxDataGets = function (options, callback) {
        //Can return unlimited items in a list from controller to view.

        /*N.B : 
            Just remove below code from controller's JsonResult

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUSoftWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);


            Add below code in controller's JsonResult

            var jsonResult = Json(_oDUSoftWindings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        */

        var defaultSettings = $.extend({
            BaseAddress: "",
            Object: "",
            ControllerName: "",
            ActionName: "",
            IsWinClose: ""
        }, options);

        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var objs = data;
                if (defaultSettings.IsWinClose) { IcsWindowClose(oActiveWindows); };
                return callback({ status: true, objs: objs });
            },
            error: function (xhr, status, error) {
                alert(error);
                return callback({ status: false, obj: null });
            }
        });
    };


    // -------Will be  Delete --------------------
    $.fn.icsDBSearchByText = function (options) {
        /*Can be Replaced By $.icsDataGets*/
        var defaultSettings = $.extend({
            BaseAddress: "",
            Object: "",
            ControllerName: "",
            ActionName: "",
            TableId: ""
        }, options);
      
        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var oCollections = jQuery.parseJSON(data);
                if (oCollections.length > 0) {
                    DynamicRefreshList(oCollections, defaultSettings.TableId);
                }
                else {
                    DynamicRefreshList([], defaultSettings.TableId);
                }
            },
            error: function (xhr, status, error) {
                alert(error + " - Please check BaseAddress, Object, ControllerName, ActionName and TableId");
            }
        });
    };

    $.fn.icsLoadDataBySingleId = function (options) {
        /*Can be Replaced By $.icsDataGets*/
        //Select an item from collection page and click a button
        //Example : select a bank from a bank menu and click bank branch button
        var defaultSettings = $.extend({
            BaseAddress: "",
            Object: "",
            ControllerName: "",
            ActionName: "",
            WindowId: ""
        }, options);

      
        var tableId = this;
        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(options.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var objCollections = jQuery.parseJSON(data);
                $("#" + defaultSettings.WindowId).window("open");
                if (objCollections.length > 0) {
                    objCollections = { "total": "" + objCollections.length + "", "rows": objCollections };
                    $(tableId).datagrid("loadData", objCollections);
                }

            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    };

    $.fn.icsPickerSearchByText = function (options) {

        var defaultSettings = $.extend({
            BaseAddress: "",
            Object: "",
            ControllerName: "",
            ActionName: "",
            WindowId: "",
            TableId: ""
        }, options);

       
        var textId = this;
        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var objCollections = jQuery.parseJSON(data);
                if (objCollections.length > 0) {
                    var oCollections = objCollections;
                    oCollections = { "total": "" + oCollections.length + "", "rows": oCollections };
                    $("#" + defaultSettings.WindowId).window("open");
                    $("#" + defaultSettings.TableId).datagrid("loadData", oCollections);
                }
                else {
                    alert("No Data Found.");
                    $(textId).val("");
                    return false;
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    };

    $.fn.icsPickSelectedItem = function (options) {
        /*Its a part of plugin $.fn.icsPickerSearchByText.
        When use $.fn.icsPickerSearchByText, then a collection window will open.
        Then select an item from that window and press enter. In this case this plugin can use.*/

        var defaultSettings = $.extend({
            WindowId: ""
        }, options);

      
        var oSelectedObj = $(this).datagrid("getSelected");
        if (oSelectedObj == null) {
            alert("Please select an item!");
            return false;
        }
        $("#" + defaultSettings.WindowId).window("close");
        return oSelectedObj;
    };
    // ---------------------------

    $.icsDataManipulation = function (options, callback) {

        var defaultSettings = $.extend({
            Operation: "",
            BaseAddress: "",
            Object: "",
            ObjectId: "",
            ControllerName: "",
            ActionName: "",
            TableId: "",
            IsWinClose: ""
        }, options);

        if (!$.trim(defaultSettings.Operation).length) { alert("Please give 'Operation' name. Use 'Save' or 'Delete' or 'Get' or 'Gets' "); return }
        if (defaultSettings.Operation.toLowerCase() != "save" && defaultSettings.Operation.toLowerCase() != "delete" && defaultSettings.Operation.toLowerCase() != "get" && defaultSettings.Operation.toLowerCase() != "gets") { alert("'Operation' name must be 'Save' or 'Delete' or 'Get' or 'Gets' "); return; };


        $.ajax({
            type: "POST",
            dataType: "json",
            url: defaultSettings.BaseAddress + "/" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName,
            traditional: true,
            data: JSON.stringify(defaultSettings.Object),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var obj = jQuery.parseJSON(data);
                 
                if (obj != null) {
                     
                    if (defaultSettings.Operation.toLowerCase() == "save") {
                        var isEdit = defaultSettings.ObjectId === 0 ? false : true;
                        if (!$.trim(obj.ErrorMessage).length) {
                            if (!isEdit) {
                                alert("Data Save successful.");
                                var oCollections = $("#" + defaultSettings.TableId).datagrid("getRows");
                                var nIndex = oCollections.length;
                                $("#" + defaultSettings.TableId).datagrid("appendRow", obj);
                                $("#" + defaultSettings.TableId).datagrid("selectRow", nIndex);
                            }
                            else {
                                alert("Data Edit successful.");
                                var oSelectedObject = $("#" + defaultSettings.TableId).datagrid("getSelected");
                                var nRowIndexBank = $("#" + defaultSettings.TableId).datagrid("getRowIndex", oSelectedObject);
                                $("#" + defaultSettings.TableId).datagrid("updateRow", { index: nRowIndexBank, row: obj });
                            }

                        }
                        else { alert(obj.ErrorMessage); }
                    }

                    else if (defaultSettings.Operation.toLowerCase() == "delete") {

                        if (obj.ErrorMessage.toLowerCase() == "delete") {
                            alert("Delete successfully.");
                            var oSelectedObject = $("#" + defaultSettings.TableId).datagrid("getSelected");
                            var nRowIndexBank = $("#" + defaultSettings.TableId).datagrid("getRowIndex", oSelectedObject);
                            $("#" + defaultSettings.TableId).datagrid("deleteRow", nRowIndexBank);
                        }
                    }

                    else if ((defaultSettings.Operation.toLowerCase() == "get" || defaultSettings.Operation.toLowerCase() == "gets") && callback == "undefined") {
                        if (defaultSettings.Operation.toLowerCase() == "get") { DynamicRefreshList([obj], defaultSettings.TableId); }
                        else if (defaultSettings.Operation.toLowerCase() == "gets") { DynamicRefreshList(obj, defaultSettings.TableId); }
                    }

                }
                else { alert("Operation Unsuccessful."); }
                 
                if (defaultSettings.IsWinClose) { IcsWindowClose(oActiveWindows); };
                if (callback != null) {
                    if (defaultSettings.Operation.toLowerCase() == "save" || defaultSettings.Operation.toLowerCase() == "delete" || defaultSettings.Operation.toLowerCase() == "get") { callback({ status: true, obj: obj }); }
                    else if (defaultSettings.Operation.toLowerCase() == "gets") { callback({ status: true, objs: obj }); }
                }


            },
            error: function (xhr, status, error) {
                alert(error + " - Please check BaseAddress, Object, ControllerName, ActionName and TableId");
                if (callback != null) {

                    callback({ status: false, obj: null });
                }
            }
        });
    };

    //ics picker
    $.icsPicker = function (options) {
        //debugger;
        var defaultSettings = $.extend({
            winid: "winwin",
            winclass: "winclass",
            winwidth: 600,
            winheight: 400,
            tableid: "tbldefault",
            tableHeight: 0,
            btnOkShow: true, // Ok Button display or not
            btnOkText : "Ok",
            tablecolumns: [],
            datalist: [],
            multiplereturn: false,
            allChecked : false,
            searchingbyfieldName: '',
            windowTittle: "Data List",
            searchOption: true, //Search Text box display or not
            placeholder : "Search By Name"
        }, options);

        $("#" + defaultSettings.winid).remove();
        if (defaultSettings.winwidth <= 180) { defaultSettings.winwidth = 400; }
        if (defaultSettings.winheight <= 120) { defaultSettings.winheight = 120; }
        var bodyTag = $('.menuMainCollectionTable').closest('body');

        // window  Picker win
        var winwindowPicker = '<div  id="' + defaultSettings.winid + '" class="' + defaultSettings.winclass + '"  title="' + defaultSettings.windowTittle + '" tabindex="-1"></div>';
        $(bodyTag).append(winwindowPicker);//apend window picker win in parent win
        $("#" + defaultSettings.winid).window({ modal: true, closed: true, collapsible: false, minimizable: false, maximizable: false, closable: false, width: defaultSettings.winwidth, height: defaultSettings.winheight, modal: true });

        var tablwidth = (parseInt(defaultSettings.winwidth) - 20) + 'px';
        var tableheight = (parseInt(defaultSettings.tableHeight) == 0 ? (parseInt(defaultSettings.winheight) - 100) : parseInt(defaultSettings.tableHeight)) + 'px';

        var dTempSearchwin = '<div id= "toolbarPicker" > <input id="txtSearch" style="width:' + tablwidth + '"  type= "text" placeholder= "'+defaultSettings.placeholder+'"/></div>';
        var sTable = '<table id= "' + defaultSettings.tableid + '" style="width:' + tablwidth + ';height:' + tableheight + ';margin:0;vertical-align:top;" singleselect="true" fitcolumns="false"  rownumbers="true" pagination="false" autorowheight="false" > </table>';

        $("#" + defaultSettings.winid).append(dTempSearchwin); //apend tool bar  win of table
        $("#" + defaultSettings.winid).append(sTable);//Append Table

        //Actions field Set
        var fieldSet = '<fieldset class="actionfieldsetstyle"><legend>Actions : </legend><a id= "btnOk" href="javascript:void(0)">' + defaultSettings.btnOkText + '</a>&nbsp;&nbsp;';
        fieldSet += '<a id="btnICSClose" href="javascript:void(0)">Close</a></fieldset>';
        $("#" + defaultSettings.winid).append(fieldSet);


        if (defaultSettings.btnOkText == "Ok") {
            $('#btnOk').linkbutton({ iconCls: 'icon-ok', plain: true });
        }
        else if (defaultSettings.btnOkText == "Print")
        {
            $('#btnOk').linkbutton({ iconCls: 'icon-print', plain: true });
        }

        $('#btnICSClose').linkbutton({ iconCls: 'icon-cancel', plain: true });

        //add default column
        var odefaultcolumn = { field: "Selected", checkbox: 'true' };
        var otablecolumns = [];
        otablecolumns.push(odefaultcolumn);
        for (var i = 0; i < defaultSettings.tablecolumns.length; i++) {
            otablecolumns.push(defaultSettings.tablecolumns[i]);
        }

        $("#" + defaultSettings.tableid).datagrid({ columns: [otablecolumns], toolbar: '#toolbarPicker' });//Column Initaialize

        $("#" + defaultSettings.winid).icsWindow('open', defaultSettings.windowTittle);
        $("#" + defaultSettings.tableid).datagrid({ selectOnCheck: false, checkOnSelect: false });
        if (!defaultSettings.multiplereturn) {
            $('#' + defaultSettings.tableid).datagrid('hideColumn', 'Selected');
        }

        if (!defaultSettings.btnOkShow) { $("#" + defaultSettings.winid).find("#btnOk").hide(); }
        if (!defaultSettings.searchOption) { $("#" + defaultSettings.winid).find("#toolbarPicker").find("#txtSearch").hide(); }

        $("#" + defaultSettings.winid).focus();
       
        SearchByPickerTextField(defaultSettings.winid, 'txtSearch', defaultSettings.tableid, defaultSettings.datalist, defaultSettings.searchingbyfieldName);
        DynamicRefreshList(defaultSettings.datalist, defaultSettings.tableid);//Load in grid
        if (defaultSettings.datalist.length > 0)
        {
            $('#' + defaultSettings.tableid).datagrid('selectRow',0);
        }
        if (defaultSettings.multiplereturn && defaultSettings.allChecked)
        {
            $("#" + defaultSettings.tableid).datagrid("checkAll");
        }


        //Refresh key Up ,Down and Enter

        $(document).find('#' + defaultSettings.winid).keydown(function (e) {
            
            var oTempobj = $('#' + defaultSettings.tableid).datagrid('getSelected');
            var nIndex = $('#' + defaultSettings.tableid).datagrid('getRowIndex', oTempobj);
            if (e.which === 38)//up arrow=38
            {
                if (nIndex <= 0) {
                    $('#' + defaultSettings.tableid).datagrid('selectRow', 0);
                }
                else {
                    $('#' + defaultSettings.tableid).datagrid('selectRow', nIndex - 1);
                }
            }
            else if (e.which === 40)//down arrow=40
            {
                var oCurrentList = $('#' + defaultSettings.tableid).datagrid('getRows');
                if (nIndex >= oCurrentList.length - 1) {
                    $('#' + defaultSettings.tableid).datagrid('selectRow', oCurrentList.length - 1);
                }
                else {
                    $('#' + defaultSettings.tableid).datagrid('selectRow', nIndex + 1);
                }

            }
            //else if (e.which === 27)//escape=27
            //{
            //    $("#" + defaultSettings.winid).icsWindow("close");
            //    $("#" + defaultSettings.winid + " input").val("");
            //    $("#" + defaultSettings.winid).remove();
            //    DynamicRefreshList([], defaultSettings.tableid);
            //}
            else if (e.which === 32)//32=space
            {
                var isChecked = $("#" + defaultSettings.winid).find(".datagrid-row-selected").find("input[type=checkbox]").is(':checked');
                if (isChecked) {
                    $("#" + defaultSettings.winid).find("tbody").find(".datagrid-row-selected").closest('tr').removeClass("datagrid-row-checked");
                    $("#" + defaultSettings.winid).find(".datagrid-row-selected").find("input[type=checkbox]").prop('checked', false);
                } else {
                    $("#" + defaultSettings.winid).find("tbody").find(".datagrid-row-selected").closest('tr').addClass("datagrid-row-checked");
                    $("#" + defaultSettings.winid).find(".datagrid-row-selected").find("input[type=checkbox]").prop('checked', true);
                }
            }
        });

        //Close Window
        $("#" + defaultSettings.winid).find("#btnICSClose").click(function () {
            $("#" + defaultSettings.winid).icsWindow("close");
            $("#" + defaultSettings.winid + " input").val("");
            $("#" + defaultSettings.winid).remove();
            DynamicRefreshList([], defaultSettings.tableid);
        });
    };
    
    //ics ProgressBar (muneef)
    $.icsProgressBar = function (isStart) {
        if (isStart)
        {
            ProgressBarShow();
            setInterval(UpdatePickerProgress, 250);
        }
        else
        {
            $('#icsprogressbar').progressbar('setValue', 100);
            ProgressBarHide();
        }
    };

    ///EASY UI DATA GRID FOOTER  (muneef)
    $.icsMakeFooterColumn = function (sTable, sFieldNames) {
        //debugger;
        var FooterField = [];
        var obj = new Object();
        obj[sFieldNames[0]] = " Total: ";
        for (var j = 1; j < sFieldNames.length; j++) {
            var data = $('#' + sTable).datagrid('getRows').select(sFieldNames[j]);
            obj[sFieldNames[j]] = parseFloat(parseFloat($.icsGridSum(data)).toFixed(2));
        }

        var oAllColumns = $('#' + sTable).datagrid('getColumnFields');
        for (var i = 0; i < oAllColumns.length; i++)
        {
            var bFlag = true;
            for (var j = 0; j < sFieldNames.length; j++)
            {
                if(sFieldNames[j] == oAllColumns[i])
                {
                    bFlag = false;
                }
            }
            if (bFlag)
            {
                obj[oAllColumns[i]] = "";
            }
        }

        FooterField.push(obj);
        $('#' + sTable).datagrid('reloadFooter', FooterField);
    }
    $.icsGridSum=function (data) {
        var sum = 0;
        for (i = 0; i < data.length; i++) {
            if (data[i] == "" || data[i] == undefined) data[i] = 0;
            if (isNaN(data[i]))
                data[i] = icsRemoveComma(data[i]);
            sum += parseFloat(data[i]);
        }
        return sum;
    }

    ///EASY UI DATA GRID FOOTER  (Saurove)
    $.icsUpdatedMakeFooterColumn = function (sTable, sFieldNames, nPointAfterDecimal) {
        debugger;
        var FooterField = [];
        var obj = new Object();
        obj[sFieldNames[0]] = " Total: ";
        for (var j = 1; j < sFieldNames.length; j++) {
            var data = $('#' + sTable).datagrid('getRows').select(sFieldNames[j]);
            obj[sFieldNames[j]] = parseFloat($.icsUpdatedGridSum(data)).toFixed(nPointAfterDecimal);
        }

        var oAllColumns = $('#' + sTable).datagrid('getColumnFields');
        for (var i = 0; i < oAllColumns.length; i++) {
            var bFlag = true;
            for (var j = 0; j < sFieldNames.length; j++) {
                if (sFieldNames[j] == oAllColumns[i]) {
                    bFlag = false;
                }
            }
            if (bFlag) {
                obj[oAllColumns[i]] = "";
            }
        }

        FooterField.push(obj);
        $('#' + sTable).datagrid('reloadFooter', FooterField);
    }
    $.icsUpdatedGridSum = function (data) {
        var sum = 0;
        for (i = 0; i < data.length; i++) {
            if (data[i] == "" || data[i] == undefined) data[i] = 0;
            if (isNaN(data[i]))
                data[i] = icsUpdatedRemoveComma(data[i]);
            sum += parseFloat(data[i]);
        }
        return sum;
    }
    function icsUpdatedRemoveComma(userInput, dpoint) {
        //debugger;
        var amountInString = "";
        if (userInput === null || userInput === "") {
            amountInString = "0.00";
        }
        else if (userInput[0] == '(' && userInput[userInput.length - 1] == ')') {
            userInput = userInput.substring(1, userInput.length - 1);
            isMinus = true;
            userInput = (userInput * -1);
            if (userInput === null || userInput === "")
                userInput = 0;
            if (isNaN(userInput))
                userInput = 0;
            amountInString = userInput;
        }
        else {
            amountInString = "";
            for (var i = 0; i < userInput.length; i++) {
                var char = userInput.charAt(i);
                var charForCheck = char;
                char = char.match(/\d+/g);
                if (char != null) {
                    amountInString = amountInString + userInput.charAt(i);
                    count = 1;
                }
                else if (charForCheck == ",") {
                    continue;
                }
                else if (charForCheck == "-") {
                    amountInString = amountInString + userInput.charAt(i);
                }
                else if (charForCheck == ".") {
                    amountInString = amountInString + userInput.charAt(i);
                }
            }

        }
        var ntoFixed = 6;
        if (dpoint != undefined && dpoint != null) {
            ntoFixed = parseInt(dpoint);
        }
        return parseFloat(amountInString).toFixed(ntoFixed);
    }

    //ics dynamic picker (muneef)
    $.icsDynamicPicker = function (options) {

        //Progress Bar (Start)
        ProgressBarShow();
        setInterval(UpdatePickerProgress, 250);

        var defaultSettings = $.extend({
            paramObj: {},
            pkID:'',
            winid: "winwin",
            winclass: "winclass",
            winwidth: 600,
            winheight: 400,
            tableid: "tbldefault",
            tableHeight: 0,
            btnOkShow: true, // Ok Button display or not
            btnOkText: "Ok",
            tablecolumns: [],
            datalist: [],
            multiplereturn: false,
            allChecked: false,
            searchingbyfieldName: '',
            windowTittle: "Data List",
            callBack:null,
            searchOption: true, //Search Text box display or not
            placeholder: "Search By Name"
        }, options);

        $.icsDataGets(options.paramObj, function (response) {

            //Progress Bar (Ends/Hide)
            $('#icsprogressbar').progressbar('setValue', 100);
            ProgressBarHide(); 

            if (response.status && response.objs.length > 0) {
                if (response.objs[0][options.pkID] > 0)
                {
                    defaultSettings.datalist = response.objs; 
                    /*===========PICKER CODES START===================================*/

                    $("#" + defaultSettings.winid).remove();
                    if (defaultSettings.winwidth <= 180) { defaultSettings.winwidth = 400; }
                    if (defaultSettings.winheight <= 120) { defaultSettings.winheight = 120; }
                    var bodyTag = $('.menuMainCollectionTable').closest('body');

                    // window  Picker win
                    var winwindowPicker = '<div  id="' + defaultSettings.winid + '" class="' + defaultSettings.winclass + '"  title="' + defaultSettings.windowTittle + '" tabindex="-1"></div>';
                    $(bodyTag).append(winwindowPicker);//apend window picker win in parent win
                    $("#" + defaultSettings.winid).window({ modal: true, closed: true, collapsible: false, minimizable: false, maximizable: false, closable: false, width: defaultSettings.winwidth, height: defaultSettings.winheight, modal: true });

                    var tablwidth = (parseInt(defaultSettings.winwidth) - 20) + 'px';
                    var tableheight = (parseInt(defaultSettings.tableHeight) == 0 ? (parseInt(defaultSettings.winheight) - 100) : parseInt(defaultSettings.tableHeight)) + 'px';

                    var dTempSearchwin = '<div id= "toolbarPicker" > <input id= "txtSearch" style="width:' + tablwidth + '"  type= "text" placeholder= "' + defaultSettings.placeholder + '"/></div>';
                    var sTable = '<table id= "' + defaultSettings.tableid + '" style="width:' + tablwidth + ';height:' + tableheight + ';margin:0;vertical-align:top;" singleselect="true" fitcolumns="false"  rownumbers="true" pagination="false" autorowheight="false" > </table>';

                    $("#" + defaultSettings.winid).append(dTempSearchwin); //apend tool bar  win of table
                    $("#" + defaultSettings.winid).append(sTable);//Append Table

                    //Actions field Set
                    var fieldSet = '<fieldset class="actionfieldsetstyle"><legend>Actions : </legend><a id= "btnOk" href="javascript:void(0)">' + defaultSettings.btnOkText + '</a>&nbsp;&nbsp;';
                    fieldSet += '<a id="btnICSClose" href="javascript:void(0)">Close</a></fieldset>';
                    $("#" + defaultSettings.winid).append(fieldSet);


                    if (defaultSettings.btnOkText == "Ok") {
                        $('#btnOk').linkbutton({ iconCls: 'icon-ok', plain: true });
                    }
                    else if (defaultSettings.btnOkText == "Print") {
                        $('#btnOk').linkbutton({ iconCls: 'icon-print', plain: true });
                    }

                    $('#btnICSClose').linkbutton({ iconCls: 'icon-cancel', plain: true });

                    //add default column
                    var odefaultcolumn = { field: "Selected", checkbox: 'true' };
                    var otablecolumns = [];
                    otablecolumns.push(odefaultcolumn);
                    for (var i = 0; i < defaultSettings.tablecolumns.length; i++) {
                        otablecolumns.push(defaultSettings.tablecolumns[i]);
                    }

                    $("#" + defaultSettings.tableid).datagrid({ columns: [otablecolumns], toolbar: '#toolbarPicker' });//Column Initaialize

                    $("#" + defaultSettings.winid).icsWindow('open', defaultSettings.windowTittle);
                    $("#" + defaultSettings.tableid).datagrid({ selectOnCheck: false, checkOnSelect: false });
                    if (!defaultSettings.multiplereturn) {
                        $('#' + defaultSettings.tableid).datagrid('hideColumn', 'Selected');
                    }

                    if (!defaultSettings.btnOkShow) { $("#" + defaultSettings.winid).find("#btnOk").hide(); }
                    if (!defaultSettings.searchOption) { $("#" + defaultSettings.winid).find("#toolbarPicker").find("#txtSearch").hide(); }

                    $("#" + defaultSettings.winid).focus();

                    SearchByPickerTextField(defaultSettings.winid, 'txtSearch', defaultSettings.tableid, defaultSettings.datalist, defaultSettings.searchingbyfieldName);
                    DynamicRefreshList(defaultSettings.datalist, defaultSettings.tableid);//Load in grid
                    if (defaultSettings.datalist.length > 0) {
                        $('#' + defaultSettings.tableid).datagrid('selectRow', 0);
                    }
                    if (defaultSettings.multiplereturn && defaultSettings.allChecked) {
                        $("#" + defaultSettings.tableid).datagrid("checkAll");
                    }
                    //Refresh key Up ,Down and Enter

                    $(document).find('#' + defaultSettings.winid).keydown(function (e) {

                        var oTempobj = $('#' + defaultSettings.tableid).datagrid('getSelected');
                        var nIndex = $('#' + defaultSettings.tableid).datagrid('getRowIndex', oTempobj);
                        if (e.which === 38)//up arrow=38
                        {
                            if (nIndex <= 0) {
                                $('#' + defaultSettings.tableid).datagrid('selectRow', 0);
                            }
                            else {
                                $('#' + defaultSettings.tableid).datagrid('selectRow', nIndex - 1);
                            }
                        }
                        else if (e.which === 40)//down arrow=40
                        {
                            var oCurrentList = $('#' + defaultSettings.tableid).datagrid('getRows');
                            if (nIndex >= oCurrentList.length - 1) {
                                $('#' + defaultSettings.tableid).datagrid('selectRow', oCurrentList.length - 1);
                            }
                            else {
                                $('#' + defaultSettings.tableid).datagrid('selectRow', nIndex + 1);
                            }

                        }
                        else if (e.which === 32)//32=space
                        {
                            var isChecked = $("#" + defaultSettings.winid).find(".datagrid-row-selected").find("input[type=checkbox]").is(':checked');
                            if (isChecked) {
                                $("#" + defaultSettings.winid).find("tbody").find(".datagrid-row-selected").closest('tr').removeClass("datagrid-row-checked");
                                $("#" + defaultSettings.winid).find(".datagrid-row-selected").find("input[type=checkbox]").prop('checked', false);
                            } else {
                                $("#" + defaultSettings.winid).find("tbody").find(".datagrid-row-selected").closest('tr').addClass("datagrid-row-checked");
                                $("#" + defaultSettings.winid).find(".datagrid-row-selected").find("input[type=checkbox]").prop('checked', true);
                            }
                        }
                    });

                    //Close Window
                    $("#" + defaultSettings.winid).find("#btnICSClose").click(function () {
                        $("#" + defaultSettings.winid).icsWindow("close");
                        $("#" + defaultSettings.winid + " input").val("");
                        $("#" + defaultSettings.winid).remove();
                        DynamicRefreshList([], defaultSettings.tableid);
                    });
                    /*===========PICKER CODES END===================================*/

                    /*===============BTN OK=================================*/
                    $("#" + options.winid).find("#btnOk").click(function () {
                        //debugger;
                        //for Single Select
                        SetPickerValueAssign(options);
                    });
                    $(document).find('.' + options.winclass).keydown(function (e) {
                        if (e.which == 13)//enter=13
                        {
                            SetPickerValueAssign(options);
                        }
                    });
                    /*==================BTN OK ENDS==============================*/
                }
                else { alert(response.objs[0].ErrorMessage); }
            }
            else {
                alert("Data Not Found.");
                return;
            }
        });
        
        function SetPickerValueAssign(oPickerobj) {
            var oResult;
            if (oPickerobj.multiplereturn) {
                oResult = $('#' + oPickerobj.tableid).datagrid('getChecked');
            }
            else {
                oResult = $('#' + oPickerobj.tableid).datagrid('getSelected');
            }
            oPickerobj.callBack(oResult);
            $("#" + oPickerobj.winid).icsWindow("close");
            $("#" + oPickerobj.winid).remove();
        }
    };

    //ics tree
    $.icsTree = function (options) {
        //debugger;
        var defaultSettings = $.extend({
            paramObj: {},
            pkID: '',
            winid: "winwin",
            winclass: "winclass",
            winwidth: 600,
            winheight: 400,
            tableid: "tbldefault",
            tableHeight: 0,
            btnOkShow: true, // Ok Button display or not
            btnOkText: "Ok",
            tablecolumns: [],
            datalist: [],
            multiplereturn: false,
            allChecked: false,
            searchingbyfieldName: '',
            windowTittle: "Data List",
            callBack: null,
            searchOption: true, //Search Text box display or not
            placeholder: "Search By Name"
        }, options);

       
        /*===========PICKER CODES START===================================*/

        $("#" + defaultSettings.winid).remove();
        if (defaultSettings.winwidth <= 180) { defaultSettings.winwidth = 400; }
        if (defaultSettings.winheight <= 120) { defaultSettings.winheight = 120; }
        var bodyTag = $('.menuMainCollectionTable').closest('body');

        // window  Picker win
        var winwindowPicker = '<div  id="' + defaultSettings.winid + '" class="' + defaultSettings.winclass + '"  title="' + defaultSettings.windowTittle + '" tabindex="-1"></div>';
        $(bodyTag).append(winwindowPicker);//apend window picker win in parent win
        $("#" + defaultSettings.winid).window({ modal: true, closed: true, collapsible: false, minimizable: false, maximizable: false, closable: false, width: defaultSettings.winwidth, height: defaultSettings.winheight, modal: true });

        var tablwidth = (parseInt(defaultSettings.winwidth) - 20) + 'px';
        var tableheight = (parseInt(defaultSettings.tableHeight) == 0 ? (parseInt(defaultSettings.winheight) - 100) : parseInt(defaultSettings.tableHeight)) + 'px';

        //var dTempSearchwin = '<div id= "toolbarPicker" > <input id= "txtSearch" style="width:' + tablwidth + '"  type= "text" placeholder= "' + defaultSettings.placeholder + '"/></div>';
        var sTable = '<table id= "' + defaultSettings.tableid + '" style="width:' + tablwidth + ';height:' + tableheight + ';margin:0;vertical-align:top;" singleselect="true" fitcolumns="false"  rownumbers="true" pagination="false" autorowheight="false" > </table>';

        //$("#" + defaultSettings.winid).append(dTempSearchwin); //apend tool bar  win of table
        $("#" + defaultSettings.winid).append(sTable);//Append Table

        //Actions field Set
        var fieldSet = '<fieldset class="actionfieldsetstyle"><legend>Actions : </legend><a id= "btnOk" href="javascript:void(0)">' + defaultSettings.btnOkText + '</a>&nbsp;&nbsp;';
        fieldSet += '<a id="btnICSClose" href="javascript:void(0)">Close</a></fieldset>';
        $("#" + defaultSettings.winid).append(fieldSet);


        if (defaultSettings.btnOkText == "Ok") {
            $('#btnOk').linkbutton({ iconCls: 'icon-ok', plain: true });
        }
        else if (defaultSettings.btnOkText == "Print") {
            $('#btnOk').linkbutton({ iconCls: 'icon-print', plain: true });
        }

        $('#btnICSClose').linkbutton({ iconCls: 'icon-cancel', plain: true });

        //add default column
        var odefaultcolumn = { field: "Selected", checkbox: 'true' };
        var otablecolumns = [];
        otablecolumns.push(odefaultcolumn);
        for (var i = 0; i < defaultSettings.tablecolumns.length; i++) {
            otablecolumns.push(defaultSettings.tablecolumns[i]);
        }

        $("#" + defaultSettings.tableid).treegrid({ columns: [otablecolumns], toolbar: '#toolbarPicker' });//Column Initaialize

        $("#" + defaultSettings.winid).icsWindow('open', defaultSettings.windowTittle);
        $("#" + defaultSettings.tableid).treegrid({ selectOnCheck: false, checkOnSelect: false });
        if (!defaultSettings.multiplereturn) {
            $('#' + defaultSettings.tableid).treegrid('hideColumn', 'Selected');
        }

        if (!defaultSettings.btnOkShow) { $("#" + defaultSettings.winid).find("#btnOk").hide(); }
        if (!defaultSettings.searchOption) { $("#" + defaultSettings.winid).find("#toolbarPicker").find("#txtSearch").hide(); }

        $("#" + defaultSettings.winid).focus();

        SearchByPickerTextField(defaultSettings.winid, 'txtSearch', defaultSettings.tableid, defaultSettings.datalist, defaultSettings.searchingbyfieldName);
        // DynamicRefreshList(defaultSettings.datalist, defaultSettings.tableid);//Load in grid

        $("#" + defaultSettings.tableid).treegrid({
            idField: options.pkID,
            treeField: 'text'
        });

        $("#" + defaultSettings.tableid).treegrid('loadData', [defaultSettings.datalist]);

        if (defaultSettings.datalist.length > 0) {
            $('#' + defaultSettings.tableid).treegrid('selectRow', 0);
        }
        if (defaultSettings.multiplereturn && defaultSettings.allChecked) {
            $("#" + defaultSettings.tableid).treegrid("checkAll");
        }
        //Refresh key Up ,Down and Enter

        $(document).find('#' + defaultSettings.winid).keydown(function (e) 
        {
            var oTempobj = $('#' + defaultSettings.tableid).treegrid('getSelected');
            var nIndex = $('#' + defaultSettings.tableid).treegrid('getRowIndex', oTempobj);
            if (e.which === 38)//up arrow=38
            {
                if (nIndex <= 0) {
                    $('#' + defaultSettings.tableid).treegrid('selectRow', 0);
                }
                else {
                    $('#' + defaultSettings.tableid).treegrid('selectRow', nIndex - 1);
                }
            }
            else if (e.which === 40)//down arrow=40
            {
                var oCurrentList = $('#' + defaultSettings.tableid).treegrid('getRows');
                if (nIndex >= oCurrentList.length - 1) {
                    $('#' + defaultSettings.tableid).treegrid('selectRow', oCurrentList.length - 1);
                }
                else {
                    $('#' + defaultSettings.tableid).treegrid('selectRow', nIndex + 1);
                }

            }
            else if (e.which === 32)//32=space
            {
                var isChecked = $("#" + defaultSettings.winid).find(".treegrid-row-selected").find("input[type=checkbox]").is(':checked');
                if (isChecked) {
                    $("#" + defaultSettings.winid).find("tbody").find(".treegrid-row-selected").closest('tr').removeClass("treegrid-row-checked");
                    $("#" + defaultSettings.winid).find(".treegrid-row-selected").find("input[type=checkbox]").prop('checked', false);
                } else {
                    $("#" + defaultSettings.winid).find("tbody").find(".treegrid-row-selected").closest('tr').addClass("treegrid-row-checked");
                    $("#" + defaultSettings.winid).find(".treegrid-row-selected").find("input[type=checkbox]").prop('checked', true);
                }
            }
        });

        //Close Window
        $("#" + defaultSettings.winid).find("#btnICSClose").click(function () {
            $("#" + defaultSettings.winid).icsWindow("close");
            $("#" + defaultSettings.winid + " input").val("");
            $("#" + defaultSettings.winid).remove();
            DynamicRefreshList([], defaultSettings.tableid);
        });
        /*===========PICKER CODES END===================================*/



        /*===============BTN OK=================================*/
        $("#" + options.winid).find("#btnOk").click(function () 
        {
            //debugger;
            //for Single Select
            SetPickerValueAssign(options);
        });
        $(document).find('.' + options.winclass).keydown(function (e) {
            if (e.which == 13)//enter=13
            {
                SetPickerValueAssign(options);
            }
        });
        /*==================BTN OK ENDS==============================*/
        
        function SetPickerValueAssign(oPickerobj) {
            var oResult;
            if (oPickerobj.multiplereturn) {
                oResult = $('#' + oPickerobj.tableid).treegrid('getChecked');
            }
            else {
                oResult = $('#' + oPickerobj.tableid).treegrid('getSelected');
                oResult.ParentNode = $('#' + oPickerobj.tableid).treegrid('getParent', oResult.id);
            }
            oPickerobj.callBack(oResult);
            $("#" + oPickerobj.winid).icsWindow("close");
            $("#" + oPickerobj.winid).remove();
        }
    };

    //New Plugins
    $.icsOpenPickerInNewPage = function (options, callback) {
        
        /*
        Declaration in Controller : 

        Example : 
        public ActionResult View_Payment(string sPaymentID, string sBtnID, string sMsg) //1. string formate (s + Properties.PrimaryKeyName) = sPaymentID  2. Only This three parameters allow 
        {
            int nPaymentID = Convert.ToInt32(sPaymentID);
            if (nPaymentID > 0)
            {
                _oPayment = _oPayment.Get(nPaymentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oPayment.PaymentPIs = PaymentPI.GetsByPaymentID(nPaymentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.PaymentMethods = PaymentMethodObj.Gets();
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oPayment.BtnIDHtml = sBtnID;
            return View(_oPayment);
        }
        */

        var Properties = $.extend({
            BaseAddress: "",
            TableId: "",
            OpenedPageTitle: "",
            Object: "",
            ObjectId: "",   //***If we need we can send more property values by this property separat by "~" symbol (Ex - PaymentID + "~" + PaymentNo ) . But in controller first paramter name must be like sPaymentID
            PrimaryKeyName : "",
            ControllerName: "",
            ActionName: "",
            BtnID:"" //Clicked button ID
        }, options);
        //var sObjectId = window.btoa(Properties.ObjectId);  //Encode Conversion

        
        if (Properties.ObjectId <= 0) {
            if ($.trim(Properties.TableId) != "") {
                var oSingleObjects = $('#' + Properties.TableId).datagrid('getRows');
                sessionStorage.setItem("SingleObjects", JSON.stringify(oSingleObjects));
                sessionStorage.setItem("SelectedRowIndex", -1);
            }
            sessionStorage.setItem("ObjectHeader", Properties.OpenedPageTitle);
            window.location.href = Properties.BaseAddress + "/" + Properties.ControllerName + "/" + Properties.ActionName + "?s" + Properties.PrimaryKeyName + "=0&sBtnID=" + Properties.BtnID + "&sMsg=N/A";
        }
        else {
            if ($.trim(Properties.TableId) != "") {
                var oSingleObject = $('#' + Properties.TableId).datagrid('getSelected');
                if (oSingleObject == null || oSingleObject[Properties.ObjectId] <= 0) {
                    alert("Please select an item from list!");
                    if (callback != null) { return callback({ status: false, obj: obj }); }
                }
                var SelectedRowIndex = $('#' + Properties.TableId).datagrid('getRowIndex', Properties.Object);
                var oSingleObjects = $('#' + Properties.tableId).datagrid('getRows');
                sessionStorage.setItem("oSingleObjects", JSON.stringify(oSingleObjects));
                sessionStorage.setItem("SelectedRowIndex", SelectedRowIndex);
            }
            sessionStorage.setItem("ObjectHeader", Properties.OpenedPageTitle);
            window.location.href = Properties.BaseAddress + "/" + Properties.ControllerName + "/" + Properties.ActionName + "?s" + Properties.PrimaryKeyName + "=" + Properties.ObjectId + "&sBtnID=" + Properties.BtnID + "&sMsg=N/A";
        }
        if (callback != null) { return callback({ status: true, obj: obj }); }


        /*
          Back btn (In this view) formate :

          $("#btnBack").click(function () {
              window.location.href = _sBaseAddress + "/FabricExecutionOrder/ViewFabricExecutionOrders?menuid=" + _nMenuid;
          });
        */

    };

    $.fn.icsCheckValidate = function (options, callback) {
        //Incomplete for easyui fields. (Pending)
        /*
        How to declare :

         var obj = {
            FieldDependencys: [
                    { FieldId: 'txtSearchByContractor', DependentVariable: _nBuyerId}
            ]
        };

        $("#winDUSoftWinding").icsCheckValidate(obj,function (response) {
            if (!response.status) {
                var sErrorField = response.fieldId;
                alert(MakeValidateAlertMessage(sErrorField));
                return false;
            }
        });
        
        */

        var defaultSettings = $.extend({
            FieldDependencys: []
        }, options);

        var oCurrentField = null;
        var sFieldId = "";
        var sFieldClass = "";
        var sFieldType = "";
        var sFieldValue = "";
        var isDisable = false;
        var sTagName = "";

        var FieldDependencyIdsList = [];
        var RequiredFieldIds=[];
        RequiredFieldIds.push("required");
        var FieldDependencyIds = defaultSettings.FieldDependencys;

        for (var i = 0; i < FieldDependencyIds.length; i++) {
            FieldDependencyIdsList.push(FieldDependencyIds[i].FieldId);
        }

        var allInputFields = $(this).find("input, select");
        for (var i = 0, max = allInputFields.length; i < max; i++) {
            oCurrentField = allInputFields[i];
            sFieldId = $(oCurrentField).attr('id');
            sFieldClass = $(oCurrentField).attr('class');
            isDisabled = $("#" + sFieldId).prop('disabled');
            sTagName = $("#" + sFieldId).prop('tagName').toLowerCase();
            sFieldValue = $.trim($(oCurrentField).val());
            var nIsRequiredField = (RequiredFieldIds.length > 0 ? RequiredFieldIds.indexOf(sFieldClass) : 0); //Only required class fields will check for validation
            if (nIsRequiredField > -1) {
                var nIsDependencyField = FieldDependencyIdsList.indexOf(sFieldId);
                if (nIsDependencyField > -1) {
                    var sDependentVariableVal = FieldDependencyIds[nIsDependencyField].DependentVariable;
                    if ($.trim(sDependentVariableVal) == "" || parseInt(sDependentVariableVal) == 0) {
                        if (!isDisabled) {
                            $("#" + sFieldId).removeClass("errorFieldBorder");
                            if (sTagName == "input") {
                                sFieldType = $(oCurrentField).attr('type');
                                if ($.trim(sFieldValue) == '') {
                                    return callback({ status: false, fieldId: sFieldId });
                                }
                            }
                            else if (sTagName == "select") {
                                if ($.trim(sFieldValue) == '' || parseInt(sFieldValue) == 0) {
                                    return callback({ status: false, fieldId: sFieldId });
                                }
                            }
                        }
                    }
                }
                else {
                    if (!isDisabled) {
                        $("#" + sFieldId).removeClass("errorFieldBorder");
                        if (sTagName == "input") {
                            sFieldType = $(oCurrentField).attr('type');
                            if ($.trim(sFieldValue) == '') {
                                return callback({ status: false, fieldId: sFieldId });
                            }
                        }
                        else if (sTagName == "select") {
                            if ($.trim(sFieldValue) == '' || parseInt(sFieldValue) == 0) {
                                return callback({ status: false, fieldId: sFieldId });
                            }
                        }
                    }
                }
            }
        }
        return callback({ status: true, fieldId: sFieldId });
    };

    $.fn.icsAlert = function (options, callback) {
        var defaultSettings = $.extend({
            AlertId: "For Confirmaion",
            AlertType: "warning",   //For Types 1.info  2.success 3.warning & 4.error
            Message: "Confirm?",
            TotalBtn: 2,
            NameOfBtns: "Yes ~ No" //must separate by ~
        }, options);

        var nTotalBtn = defaultSettings.NameOfBtns.split("~").length;
        var sAlertId = (defaultSettings.AlertId.replace(/\s/g, "").length == 0 ? "icsAlert" : defaultSettings.AlertId.replace(/\s/g, ""));
        var className = sAlertId + "Warning icsAlertWarning";
        if (defaultSettings.AlertType == "info") { className = sAlertId + "Info icsAlertInfo"; }
        else if (defaultSettings.AlertType == "success") { className = sAlertId + "Success icsAlertSuccess"; }
        else if (defaultSettings.AlertType == "error") { className = sAlertId + "Error icsAlertError"; }

        var thisId = $(this).attr("id");

        $("#" + thisId).closest(".MainBody").parent().attr('id', 'AlertParent');
        $("#" + thisId).closest(".MainBody").parent().find("#" + sAlertId).remove();


        var sTitle = (defaultSettings.AlertType == "info" ? "information" : defaultSettings.AlertType);


        $("#" + thisId).closest(".MainBody").parent().append("<div id='" + sAlertId + "' class='icsAlert " + className + "' style='text-align: center;padding:10px;'><h3 style='font-weight: bold;font-size: 23px;'>" + sTitle.toUpperCase() + "</h3><p>" + defaultSettings.Message + "</p></div>");   //#icsAlert style in main.css
        $("#" + sAlertId).focus();

        for (var i = 0; i < nTotalBtn; i++) {
            if (i == 0) {
                $("#" + sAlertId).append("<div id='" + sAlertId + "BtnDiv' class='alertBtn'></div>");
            }
            var value = defaultSettings.NameOfBtns.split("~")[i];
            $("#" + sAlertId + "BtnDiv").append("<input id='" + sAlertId + "" + value.replace(/\s/g, "") + "' class='" + sAlertId + "IcsAlertBtn' style='margin-right:20px;margin-bottom:10px;' type='button' value='" + value + "'/>");
        }
        $(".icsAlert").css("margin-top", ($(window).height() / 4) + "px");
        $(".icsAlert").css("margin-bottom", ($(window).height() / 2.5) + "px");


        var parent = $("#" + thisId).closest(".MainBody").parent();// $("#MainBody").attr("id");
        $("#" + thisId).closest(".MainBody").parent().find('input:disabled, select:disabled').addClass('alertDisabledFields');
        $("#" + thisId).closest(".MainBody").parent().find("*").not("#" + sAlertId + ", .alertBtn input").attr("disabled", true);

        //$("#" + thisId).closest(".MainBody").parent().find("*").not("#" + sAlertId + ", .alertBtn input").not(".easyui-datagrid").attr("disabled", true);

        $("#" + sAlertId).find("." + sAlertId + "IcsAlertBtn").click(function () {
            $("#AlertParent").find("*").not(".alertDisabledFields").attr("disabled", false);
            $(parent).find('.alertDisabledFields').removeClass('alertDisabledFields');
            $("#" + sAlertId).remove();
            //alert($(this).val());
            return callback({ status: true, btnText: $(this).val() });
        });

       
    };




}(jQuery));




///progressbar with signalr
function ProgressBarHide() {
    $("#icsprogressbar").progressbar({ value: 0 });
    $("#icsprogressbarParent").hide();
}

function ProgressBarShow() {
    $("#icsprogressbar").progressbar({ value: 0 });
    $("#icsprogressbarParent").show();
}

function UpdatePickerProgress() {
    var value = $('#icsprogressbar').progressbar('getValue');
    if (value < 90) {
        value += Math.floor(Math.random() * 10);
        $('#icsprogressbar').progressbar('setValue', value);
    }
}

function UpdateProgressBar(sMSG, nCount) {
    $('#icsprogressbar').progressbar({ text: sMSG + ' {value}%' })
    $('#icsprogressbar').progressbar('setValue', nCount ? parseInt(nCount) ? parseInt(nCount) : 0 : 0);
}

function InitializeProgressBar() {
    $.connection.hub.logging = true;
    var progressNotifier = $.connection.progressHub;

    progressNotifier.client.broadcastMessage = function (message, count) {
        $("#icsprogressbarParent").show();
        UpdateProgressBar(message, count);        
    };

    $.connection.hub.start().done(function () {
        console.log('connection started!');
    });

    $.connection.hub.disconnected(function () {
        $.connection.hub.start().done(function () {
            console.log('connection started!');
        });
    });
}

function StopSignalr() {
    $.connection.hub.stop();
}
///progressbar with signalr

//IcsAutoComplete Related Function
function CheckMultipleIds() {
    $('[id]').each(function () {
        var ids = $('[id="' + this.id + '"]');
        if (ids.length > 1 && ids[0] == this) {
            console.warn('Multiple IDs #' + this.id);
        }
    });
}
function CheckDuplicateIds(elem) {
    $('[id]').each(function () {
        var ids = $('[id="' + this.id + '"]');
        if (ids.length > 1 && ids[0] == elem) {
            console.warn('Duplicate IDs #' + this.id);
        }
    });
}
function IcsAutoCompleteSearchByText(options) {
    //Write text in a input text field and sort list from a collection by string match.
    //Example : In bank menu : this plaugin use in "Search by Bank code" or "Search by Bank Name" input text fields
    var defaultSettings = $.extend({
        SearchProperty: "",
        GlobalObjectList: "",
        Columns: [],
        TextBox: ""
    }, options);

    var txtSearchBy = $(defaultSettings.TextBox).val();
    var oSearchedLists = [];
    var sTempName = "", sCombined = '';
    oCurrentList = defaultSettings.GlobalObjectList;

    for (var i = 0; i < oCurrentList.length; i++) {
        if (defaultSettings.Columns && defaultSettings.Columns.length > 0) {
            sCombined = '';
            for (var j = 0; j < defaultSettings.Columns.length ; j++) {
                sCombined = sCombined + oCurrentList[i][defaultSettings.Columns[j].field] + ' ';
            }
            sTempName = sCombined;
        }
        else {
            sTempName = oCurrentList[i][defaultSettings.SearchProperty];
        }
        var n = sTempName.toUpperCase().indexOf(txtSearchBy.toUpperCase());
        if (n != -1) {
            oSearchedLists.push(oCurrentList[i]);
        }
    }
    return oSearchedLists;
}
function IcsAutoCompleteRowSelect(rowIndex, rowData, TextBox, PropertyName, callback) {
    $(TextBox).val(rowData[PropertyName]);
    $(TextBox).data('obj', rowData);
    $(TextBox).addClass('fontColorOfPickItem');

}
function IcsAutoCompleteRowClick(rowIndex, rowData, TextBox, callback) {
    if ($("#divICSTestboxDataSearch").length) {
        $('#divICSTestboxDataSearch').icsWindow('close');
        $('#divICSTestboxDataSearch').remove();
        $(this).data('autocomplete', 'close');
    }
    $(TextBox).focus();
    if (callback != null) { return callback({ obj: rowData }); }
}
function ICSAutoCompleteWindowGenerate(TextBox, defaultSettings, objs, callback) {
    //debugger;
    if ($("#divICSTestboxDataSearch").length) {
        $('#divICSTestboxDataSearch').icsWindow('close');
        $('#divICSTestboxDataSearch').remove();
        $(this).data('autocomplete', 'close');
    }

    var columns = []
    if (defaultSettings.Columns && defaultSettings.Columns.length > 0) {
        for (var i = 0; i < defaultSettings.Columns.length; i++) {
            columns.push({ field: defaultSettings.Columns[i].field, width: defaultSettings.Columns[i].width ? defaultSettings.Columns[i].width : 100 / defaultSettings.Columns.length + '%' });
        }
    }
    else {
        columns.push({ field: defaultSettings.PropertyName, width: '100%' });
    }
    var offset = $(TextBox).offset();
    var divParent = $(TextBox).closest('div');
    var width = $(TextBox).width() + 5;
    //debugger;
    $(divParent).append($('<div id="divICSTestboxDataSearch"></div>'));
    $('#divICSTestboxDataSearch').append($('<table id="tblSuggestions" style="width:100%;max-height:130px;" singleselect="true" fitcolumns="true" rownumbers="false" pagination="false" autorowheight="false" showHeader="false" ></table>'));
    //$('#divICSTestboxDataSearch').append($('<div id="toolbarSuggestions" > <</div>'));
    $('#tblSuggestions').datagrid({ columns: [columns] });
    $('#tblSuggestions').datagrid({
        onSelect: function (rowIndex, rowData) { IcsAutoCompleteRowSelect(rowIndex, rowData, TextBox, defaultSettings.PropertyName, callback); },
        onClickRow: function (rowIndex, rowData) { IcsAutoCompleteRowClick(rowIndex, rowData, TextBox, callback); }
    });
    $('#divICSTestboxDataSearch').window({
        title: '',
        border: false,
        noheader: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        closable: false,
        draggable: false,
        resizable: false,
        shadow: false,
        inline: false,
        left: offset.left,
        top: offset.top + 21,
        width: width,
        closed: true,
        modal: false
    });
    if (objs != null && objs.length > 0) {
        DynamicRefreshList(objs, 'tblSuggestions');//Load in grid
        $('#divICSTestboxDataSearch').icsWindow('open');
        $(TextBox).data('autocomplete', 'open');
        //$(TextBox).data('DataList', objs);
    }
}
//End IcsAutoComplete Related Function

//ICS Picker for all module
function SearchByPickerTextField(winId, txtfieldid, tableid, oDataList, searchingbyfieldName) {
    $("#" + winId).find("#" + txtfieldid).keyup(function (e) {
       
        var obj = {
            Event: e,
            SearchProperty: searchingbyfieldName,
            GlobalObjectList: oDataList,
            TableId: tableid
        };
        $("#" + txtfieldid).icsSearchByText(obj);
    });
}

function IcsWindowClose(oActiveWindows) {
    var sWinId = (oActiveWindows.length > 0) ? oActiveWindows[oActiveWindows.length - 1].WinId : '';
   
    if (sWinId != '') {
        oActiveWindows.pop();
        $("#" + sWinId + "").window("close");
    }
}

function DynamicRefreshList(objCollections, tableId) {
    var data = objCollections;
    data = { "total": "" + data.length + "", "rows": data };
    $("#" + tableId).datagrid("loadData", data);
}

function DynamicRefreshListForMultipleSelection(objCollections, tableId) {
    //<th data-options="field:'Selected',checkbox:true"></th>
    $('#' + tableId).datagrid({ selectOnCheck: false, checkOnSelect: false });
    var data = { "total": "" + objCollections.length + "", "rows": objCollections };
    $('#' + tableId).datagrid('loadData', data);
 
}

function DynamicTreeList(oTObject, tableId) {
    $("#" + tableId).tree({ data: [oTObject] });
}

function RefreshTreeList(oTObject, tableId) {
    data = [oTObject];
    data = { "total": "" + data.length + "", "rows": data };
    $("#" + tableId).treegrid('loadData', data);
}

function CustomTypeConversion() {
    /* Must call this function from ready function */
    $(".number").icsNumberField().css("text-align", "right");
}

function PressSpaceAndCheckUncheckedPickerItems(sWinid)
{
    var isChecked = $("#" + sWinid).find(".datagrid-row-selected").find("input[type=checkbox]").is(':checked');
    if (isChecked) {
        $("#" + sWinid).find("tbody").find(".datagrid-row-selected").closest('tr').removeClass("datagrid-row-checked");
        $("#" + sWinid).find(".datagrid-row-selected").find("input[type=checkbox]").prop('checked', false);
    } else {
        $("#" + sWinid).find("tbody").find(".datagrid-row-selected").closest('tr').addClass("datagrid-row-checked");
        $("#" + sWinid).find(".datagrid-row-selected").find("input[type=checkbox]").prop('checked', true);
    }
}


function ResetAllFields(windowId) {
    $("#" + windowId).find("input").not("input[type='button']").val("");
    $("#" + windowId).find("select").val(0);
    $("#" + windowId).find("input[type='checkbox']").prop("checked", false);
    $("#" + windowId).find(".easyui-datebox").datebox({ disabled: false });
    $("#" + windowId).find(".easyui-datebox").datebox("setValue", icsdateformat(new Date()));

    $("#" + windowId).find("input").removeClass("errorFieldBorder");
    $("#" + windowId).find("select").removeClass("errorFieldBorder");
}

function ResetAllTables() {
    for (var i = 0; i < arguments.length; i++) {
        var data = [];
        data = { "total": "" + data.length + "", "rows": data };
        $("#" + arguments[i]).datagrid("loadData", data);
    }
}


var _sTableId = "";
function WindowIndexController(sTableId) {
    $("#" + sTableId).datagrid("selectRow", 0);
    _sTableId = sTableId;
}

function UnselectAllRowsOfATable() {
    var sPreciousTableId = _sTableId;
    //$("#" + sPreciousTableId).datagrid("unselectAll");
}


function DynamicDateActions(ComboId, FromDateId, ToDateId) {
    var nDateOptionVal = $("#" + ComboId).val();
    if (parseInt(nDateOptionVal) == 0) {
        $("#" + FromDateId).datebox({ disabled: true });
        $("#" + FromDateId).datebox("setValue", icsdateformat(new Date()));
        $("#" + ToDateId).datebox({ disabled: true });
        $("#" + ToDateId).datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) >= 1 && parseInt(nDateOptionVal) <= 4) {
        $("#" + FromDateId).datebox({ disabled: false });
        $("#" + FromDateId).datebox("setValue", icsdateformat(new Date()));
        $("#" + ToDateId).datebox({ disabled: true });
        $("#" + ToDateId).datebox("setValue", icsdateformat(new Date()));
    }
    else if (parseInt(nDateOptionVal) == 5 || parseInt(nDateOptionVal) == 6) {
        $("#" + FromDateId).datebox({ disabled: false });
        $("#" + FromDateId).datebox("setValue", icsdateformat(new Date()));
        $("#" + ToDateId).datebox({ disabled: false });
        $("#" + ToDateId).datebox("setValue", icsdateformat(new Date()));
    }
}

function DynamicValueActions(ComboId, FromValueId, ToValueId) {
    var nValueOptionVal = $("#" + ComboId).val();
    if (parseInt(nValueOptionVal) == 0) {
        $("#" + FromValueId).prop('disabled', true);
        $("#" + FromValueId).val(0);
        $("#" + ToValueId).prop('disabled', true);
        $("#" + ToValueId).val(0);
    }
    else if (parseInt(nValueOptionVal) >= 1 && parseInt(nValueOptionVal) <= 4) {
        $("#" + FromValueId).prop('disabled', false);
        $("#" + FromValueId).val(0);
        $("#" + ToValueId).prop('disabled', true);
        $("#" + ToValueId).val(0);
    }
    else if (parseInt(nValueOptionVal) == 5 || parseInt(nValueOptionVal) == 6) {
        $("#" + FromValueId).prop('disabled', false);
        $("#" + FromValueId).val(0);
        $("#" + ToValueId).prop('disabled', false);
        $("#" + ToValueId).val(0);
    }
}


function DynamicResetAdvSearchWindow(WindowId) {
    $("#" + WindowId).find("input").not(".oneCboAndTwoDateBoxs input").not("input[type='button']").val("");
    $("#" + WindowId).find("input").removeClass("fontColorOfPickItem");
    $("#" + WindowId).find("select").val(0);
}


function RemoveItemFromList(removeBtnId, collectionTableId, sPrimaryKey) {
    /*
        Only remove item from list not from Database table
    */
    //$("#" + removeBtnId).html("abc");
    $("#" + removeBtnId).click(function () {
        var obj = $("#" + collectionTableId).datagrid("getSelected");
        if (obj == null || obj.sPrimaryKey <= 0) {
            alert("Please select an item from list.");
            return false;
        }
        var nRowIndex = $("#" + collectionTableId).datagrid("getRowIndex", obj);
        $("#" + collectionTableId).datagrid("deleteRow", nRowIndex);
    });
}


window.onload = function () {
    $(".PrintForm").parent().find("select, input, a, div").css({ 'float': 'left', 'margin-right': '4px' });


};

//--
function NumberConvertionInWord(number) {
    if ((number < 0) || (number > 999999999)) {
        return "Number is out of range";
    }
    var Gn = Math.floor(number / 10000000);  /* Crore */
    number -= Gn * 10000000;
    var kn = Math.floor(number / 100000);     /* lakhs */
    number -= kn * 100000;
    var Hn = Math.floor(number / 1000);      /* thousand */
    number -= Hn * 1000;
    var Dn = Math.floor(number / 100);       /* Tens (deca) */
    number = number % 100;               /* Ones */
    var tn = Math.floor(number / 10);
    var one = Math.floor(number % 10);
    var res = "";

    if (Gn > 0) {
        res += (NumberConvertionInWord(Gn) + " Crore");
    }
    if (kn > 0) {
        res += (((res == "") ? "" : " ") +
        NumberConvertionInWord(kn) + " Lakhs");
    }
    if (Hn > 0) {
        res += (((res == "") ? "" : " ") +
            NumberConvertionInWord(Hn) + " Thousand");
    }
    if (Dn) {
        res += (((res == "") ? "" : " ") +
            NumberConvertionInWord(Dn) + " hundred");
    }
    var ones = Array("", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eightteen", "Nineteen");
    var tens = Array("", "", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eigthy", "Ninety");
    if (tn > 0 || one > 0) {
        if (!(res == "")) {
            res += " ";
        }
        if (tn < 2) {
            res += ones[tn * 10 + one];
        }
        else {

            res += tens[tn];
            if (one > 0) {
                res += ("-" + ones[one]);
            }
        }
    }
    if (res == "") {
        res = "zero";
    }
    return res;
}

function AmountInWordConversion(number, sCurrency) {
    sCurrency = $.trim(sCurrency);
    var sCurrencyNote='',sPaisa = '';
    if (sCurrency.toLowerCase() == 'taka' || sCurrency.toLowerCase() == 'tk' || sCurrency.toLowerCase() == 'bdt' || sCurrency == '') {
        sCurrencyNote = sCurrency.toUpperCase();
        sPaisa = 'Paisa';
    }
    else if (sCurrency.toLowerCase() == '$' || sCurrency.toLowerCase() == 'dollar' || sCurrency.toLowerCase() == 'usd') {
        sCurrencyNote = sCurrency.toUpperCase();
        sPaisa = 'Cent';
    }
    if (parseInt(number.toString().indexOf('.')) > -1) {
        number = number.toFixed(2);
        var sNumber = number.toString().split(".");
        return NumberConvertionInWord(parseInt(sNumber[0])) + ' ' + sCurrencyNote + ' and ' + NumberConvertionInWord(parseInt(sNumber[1])) + ' ' + sPaisa + ' Only.';
    }
    else {
        return NumberConvertionInWord(parseInt(number)) + ' ' + sCurrencyNote + ' Only.';
    }
}

function MakeValidateAlertMessage(sErrorField) {
    var sMakeAlertMessage = "";
    var sErrorFields = sErrorField.replace(/([a-z])([A-Z])/g, "$1 $2"); //Insert space before capital letters
    sErrorFields = sErrorFields.split(" ");
    for (var i = 1; i < sErrorFields.length; i++) {
        if (sMakeAlertMessage == "") {
            sMakeAlertMessage = sErrorFields[i];
        }
        else {
            sMakeAlertMessage = sErrorFields[i] + " " + sMakeAlertMessage;
        }
    }
    $("#" + sErrorField).focus();
    $("#" + sErrorField).addClass("errorFieldBorder");
    return sMakeAlertMessage + " is required.";
}

function ConvertANumberIntoCustomDigits(nUserInput, nNumberOfDigit) {
    var result = "";
    var nCountLength = String(nUserInput).length;
    if (isNaN(nCountLength)) nCountLength = 0;
    var nNumberOfZero = parseInt(nNumberOfDigit) - parseInt(nCountLength);
    if (isNaN(nNumberOfZero)) nNumberOfZero = 0;
    for (var i = 0; i < nNumberOfZero; i++) {
        result += "0";
    }
    return String(result + String(nUserInput));
}

function BtnPrintBtnClick(nUniqId) {
    $('#chkInKg' + nUniqId).prop('checked', false);
    $('#chkInLBS' + nUniqId).prop('checked', true);
    $("#winPrintFormat" + nUniqId).icsWindow("open", "Print Format");
    $('#chkPrintTitleInText' + nUniqId).prop('checked', true);
    $('#chkPrintTitleInImg' + nUniqId).prop('checked', false);
    $('#chkInYard' + nUniqId).prop('checked', true);
    $('#chkInMeter' + nUniqId).prop('checked', false);
}

function QtyFromatPopUpMenuLoad(nUniqId) {
    $("#btnClosePrintFormat" + nUniqId).click(function () {
        $("#winPrintFormat" + nUniqId).icsWindow("close");
    });

    $("#chkInLBS" + nUniqId).change(function () {
        if (this.checked) {
            $('#chkInLBS' + nUniqId).prop('checked', true);
            $('#chkInKg' + nUniqId).prop('checked', false);
        } else {
            $('#chkInLBS' + nUniqId).prop('checked', false);
            $('#chkInKg' + nUniqId).prop('checked', true);
        }
    });

    $("#chkInKg" + nUniqId).change(function () {
        if (this.checked) {
            $('#chkInLBS' + nUniqId).prop('checked', false);
            $('#chkInKg' + nUniqId).prop('checked', true);
        } else {
            $('#chkInLBS' + nUniqId).prop('checked', true);
            $('#chkInKg' + nUniqId).prop('checked', false);
        }
    });

    $("#chkPrintTitleInImg" + nUniqId).change(function () {
        if (this.checked) {
            $('#chkPrintTitleInImg' + nUniqId).prop('checked', true);
            $('#chkPrintTitleInText' + nUniqId).prop('checked', false);
        } else {
            $('#chkPrintTitleInImg' + nUniqId).prop('checked', false);
            $('#chkPrintTitleInText' + nUniqId).prop('checked', true);
        }
    });

    $("#chkPrintTitleInText" + nUniqId).change(function () {
        if (this.checked) {
            $('#chkPrintTitleInImg' + nUniqId).prop('checked', false);
            $('#chkPrintTitleInText' + nUniqId).prop('checked', true);
        } else {
            $('#chkPrintTitleInImg' + nUniqId).prop('checked', true);
            $('#chkPrintTitleInText' + nUniqId).prop('checked', false);
        }
    });


    $("#chkInYard" + nUniqId).change(function () {
        if (this.checked) {
            $('#chkInMeter' + nUniqId).prop('checked', false);
            $('#chkInYard' + nUniqId).prop('checked', true);
        } else {
            $('#chkInMeter' + nUniqId).prop('checked', true);
            $('#chkInYard' + nUniqId).prop('checked', false);
        }
    });

    $("#chkInMeter" + nUniqId).change(function () {
        if (this.checked) {
            $('#chkInYard' + nUniqId).prop('checked', false);
            $('#chkInMeter' + nUniqId).prop('checked', true);
        } else {
            $('#chkInYard' + nUniqId).prop('checked', true);
            $('#chkInMeter' + nUniqId).prop('checked', false);
        }
    });

}

function LoadBasicEventsFromLayout()
{
    $('.number').keypress(function (event) {
        if (event.keyCode == 9 || event.which == 8 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46) {
            //event.keyCode = 9 = TAB
            //event.which = 8 = BackSpace
            //event.keyCode = 37 = Left key
            //event.keyCode = 39 = Right key
            //event.keyCode = 46 = Delete key
        }
        else if (event.which === 37 || event.which === 39 || event.which === 46) {
            var sVal = $(this).val();
            var nCountDot = sVal.split(".").length;
            if (nCountDot < 2) {
                return true;
            } else {
                event.preventDefault();
            }
        }
        else if ((event.which != 46) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    $(".divNewSearchParent").hide();
    //$(".divNewSearch").append("<a id='btnCloseAdvSearch' href='javascript:void(0)' class='easyui-linkbutton' iconcls='icon-close' plain='true'></a>");
}

function SelectItemFromSession(sSessionFieldName, sPrimaryKeyFeild, sSearchingList, sTableId) //sPrimaryKeyFeild => String Format
{
    /*
        Select or append Item In Collection Page By Session

        This function used when 
        click Save Button of a Single Object Page (like ViewFabricDeliveryOrder)
        and back in collection page (like ViewFabricDeliveryOrders) and select that saved item from list.
    */
    

    /*
    
    1. sSessionFieldName means =>  
    
        sessionStorage.setItem("FDO",JSON.stringify(response.obj));
    
        Here sSessionFieldName = "FDO"

    2. sPrimaryKeyFeild => Primary Key Name. Like "FDOID" //FabricDeliveryOrder module

    3. sSearchingList => Main Collection Menu List

    */

    /*
        How to use :

        Step 1 : After click on Save button and with save is success then add this => sessionStorage.setItem("FDO", JSON.stringify(response.obj));  //FDO = FabricDeliveryOrder

        Step 2 : In Collection Page => SelectItemFromSession("FDO", "FDOID", _oFDOs, "tblFabricDeliveryOrders");
    
    */

    var obj = sessionStorage.getItem(sSessionFieldName);
    if (obj != null)
    {
        
        var bIsInList = false;
        var obj = jQuery.parseJSON(sessionStorage.getItem(sSessionFieldName));

        var nPrimaryKeyFieldValue = sessionStorage.getItem(sPrimaryKeyFeild);
        for (var i = 0; i < sSearchingList.length; i++) {
            if (sSearchingList[i][sPrimaryKeyFeild] == parseInt(obj[sPrimaryKeyFeild])) {
                $('#' + sTableId).datagrid('selectRow', i);
                bIsInList = true;
            }
        }

        if (!bIsInList) {
            var oCollections = $("#" + sTableId).datagrid("getRows");
            var nIndex = oCollections.length;
            $("#" + sTableId).datagrid("appendRow", obj);
            $("#" + sTableId).datagrid("selectRow", nIndex);
        }
    }
}
//--

function icsRemoveComma(userInput, dpoint) {
    //debugger;
    var amountInString = "";
    if (userInput === null || userInput === "") {
        amountInString = "0.00";
    }
    else {
        amountInString = "";
        for (var i = 0; i < userInput.length; i++) {
            var char = userInput.charAt(i);
            var charForCheck = char;
            char = char.match(/\d+/g);
            if (char != null) {
                amountInString = amountInString + userInput.charAt(i);
                count = 1;
            }
            else if (charForCheck == ",") {
                continue;
            }
            else if (charForCheck == "-") {
                amountInString = amountInString + userInput.charAt(i);
            }
            else if (charForCheck == ".") {
                amountInString = amountInString + userInput.charAt(i);
            }
        }
    }
    var ntoFixed = 6;
    if (dpoint != undefined && dpoint != null)
    {
        ntoFixed = parseInt(dpoint);
    }
    return parseFloat(amountInString).toFixed(ntoFixed);
}

function icsFormatPrice(val, row, dpoint) {
    if (val == null) {
        val = 0.00;
    }
    if (typeof dpoint === "undefined" || dpoint === null || isNaN(parseFloat(dpoint))) { dpoint = 2; }
    //val = parseFloat(val);
    val = (val === null || val === undefined || isNaN(parseFloat(val))) ? 0.00 : parseFloat(val)
    var test = val.toFixed(parseInt(dpoint));
    var tests = icsAddComma(test);
    return tests;
}

function icsAddComma(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var process = /(\d+)(\d{3})/;
    while (process.test(x1)) {
        x1 = x1.replace(process, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function OpenWindowWithPost(url, objs) {
    //debugger;
    //keyName, params
    var form = document.createElement('form');
    form.setAttribute('method', 'post');
    form.setAttribute('action', url);
    form.setAttribute('target', "_blank");

    if (objs != null && objs.length > 0) {
        $.each(objs, function (index, obj) {
            if (obj.hasOwnProperty('key') && obj.hasOwnProperty('data')) {
                var input = document.createElement('input');
                input.type = 'hidden';
                input.name = obj['key'];
                input.value = obj['data'];
                form.appendChild(input);
            }
        });
    }

    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
}
