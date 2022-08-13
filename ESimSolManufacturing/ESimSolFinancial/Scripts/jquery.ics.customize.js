//start date formater 
function icsdateformat(date) {    
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var y = date.getFullYear();
    var m = date.getMonth();
    var d = icsCustomStringFormat(date.getDate(), '00');
    //return m+'/'+d+'/'+y;
    var result = d + ' ' + mthNames[m] + ' ' + y;
    return result;
}

//return date from a string date format like "01 Oct 2014"
function icsdateparser(s) {
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    if (!s) return new Date();
    var ss = (s.split(' '));
    var d = parseInt(ss[0], 10);
    var m = stringarrayindex(mthNames, ss[1]) + 1;
    var y = parseInt(ss[2], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}
//GET FORMAT (09 OCT 2019) OR (WED OCT 09 2019) AND RETURN (WED OCT 09 2019)
//ALI AKRAM 09 OCT 2019
//Business Object (this.ServiceDate.DayOfWeek.ToString().Substring(0, 3) + " " + this.ServiceDate.ToString("MMM dd yyyy");
function icsdateformatWithDay(date) {
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var y = date.getFullYear();
    var m = date.getMonth();
    var d = icsCustomStringFormat(date.getDate(), '00');
    return date.toDateString();
}
function icsdateparserWithDay(s) {
    debugger;
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    if (!s) return new Date();
    var ss = (s.split(' '));
    var d = 0;
    var m = 0;
    var y = 0;
    if (ss.length == 4) {
        d = parseInt(ss[2], 10);
        m = stringarrayindex(mthNames, ss[1]) + 1;
        y = parseInt(ss[3], 10);
    }
    if (ss.length == 3)
    {
        d = parseInt(ss[0], 10);
        m = stringarrayindex(mthNames, ss[1]) + 1;
        y = parseInt(ss[2], 10);
    }
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}

function icsdatetimeformat(date) {
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var y = date.getFullYear();
    var m = date.getMonth();
    var d = icsCustomStringFormat(date.getDate(), '00');
    var hours = date.getHours();
    var ampm = (hours >= 12) ? 'PM' : 'AM';
    hours = icsCustomStringFormat(((hours >= 12) ? ((hours % 12 == 0) ? 12 : hours % 12) : (hours == 0) ? 12 : hours), '00');
    var minutes = icsCustomStringFormat((date.getMinutes()), '00');


    //return m+'/'+d+'/'+y;
    var result = d + ' ' + mthNames[m] + ' ' + y + ' ' + hours + ':' + minutes + ' ' + ampm;
    return result;
}

//return date from a string date format like "01 Oct 2014 12:34 PM"
function icsdatetimeparser(s) {
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    if (!s) return new Date();
    var ss = (s.split(' '));
    var d = parseInt(ss[0], 10);
    var m = stringarrayindex(mthNames, ss[1]) + 1;
    var y = parseInt(ss[2], 10);
    var hourmin = ss[3].split(':');

    var hours = parseInt(hourmin[0], 10);
    var minutes = parseInt(hourmin[1], 10);
    var ampm = ss[4];
    hours = ampm == 'PM' ? (hours == 12 ? 12 : hours + 12) : (hours == 12 ? 0 : hours);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d) && !isNaN(hours) && !isNaN(minutes)) {
        return new Date(y, m - 1, d, hours, minutes);
    } else {
        return new Date();
    }
}

function icsmonthformat(date) {
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var y = date.getFullYear();
    var m = date.getMonth();
    var d = date.getDate();
    //return m+'/'+d+'/'+y;
    var result = mthNames[m] + ' ' + y;
    return result
}

//return date from a string date format like "Oct 2014"
function icsmonthparser(s) {
    debugger;
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    if (!s) return new Date();
    var ss = (s.split(' '));
    var d = new Date().getDate(); //parseInt(ss[0], 10);
    var m = stringarrayindex(mthNames, ss[0]) + 1;
    var y = parseInt(ss[1], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}
 
function icsyearformat(date) {
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var y = date.getFullYear();
    var m = date.getMonth();
    var d = icsCustomStringFormat(date.getDate(), '00');
    //return m+'/'+d+'/'+y;
    var result = y;
    return result;
}

//return date from a string date format like "01 Oct 2014"
function icsyearparser(s) {
    var mthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    if (!s) return new Date();
    //var ss = (s.split(' '));
    var d = new Date().getDate();
    var m = new Date().getMonth();
    var y = parseInt(s, 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}
//end date formater


//write by Mahabub
function Ics_WeekFind(d)
{
    //debugger;
    var day = d.getDay();
    if (day == 0) day = 7;
    d.setDate(d.getDate() + (4 - day));
    var year = d.getFullYear();
    var ZBDoCY = Math.floor((d.getTime() - new Date(year, 0, 1, -6)) / 86400000);
    return 1 + Math.floor(ZBDoCY / 7);
}


//start string custom format
//nInteger=integer value; sFormat=format in string like- '0000'
function icsCustomStringFormat(nInteger, sFormat) {
    var sInteger = nInteger.toString();
    var lenDIff;
    (sFormat.length > sInteger.length) ? lenDIff = sFormat.length - sInteger.length : lenDIff = 0;
    return sFormat.substring(0, lenDIff) + sInteger;
}
//end string custom format

//start common function
function stringarrayindex(aArray, sItem) {
    if (aArray == null) return -1;
    for (var i = 0; i < aArray.length; i++) {
        if (aArray[i] == sItem) {
            return i;
        }
    }
    return -1;
}
//end common function


//start price format 
function formatPriceWithZeroDecimal(val, row) {
    if (val == null) {
        val = 0.00;
    }
    val = parseFloat(val);
    var test = val.toFixed(0);
    var tests = addComma(test);
    return tests;
}

function formatPricewithoutdecimal(val, row) {
    if (val == null) {
        val = 0.00;
    }
    val = parseFloat(val);
    var test = val.toFixed(0);
    var tests = addComma(test);
    return tests;
}

function formatPrice(val, row) {
    if (val == null) {
        val = 0.00;
    }
    if (val == '')
    {
        return '';
    }
    val = parseFloat(val);
    var test = val.toFixed(2);
    var tests = addComma(test);
    return tests;
}

function formatPriceFor3digit(val, row) {
    if (val == null) {
        val = 0.00;
    }
    val = parseFloat(val);
    var test = val.toFixed(3);
    var tests = addComma(test);
    return tests;
}

function formatPrice4digit(val, row) {
    if (val == null) {
        val = 0.00;
    }
    val = parseFloat(val);
    var test = val.toFixed(4);
    var tests = addComma(test);
    return tests;
}

function formatPrice5digit(val, row) {
    if (val == null) {
        val = 0.00;
    }
    val = parseFloat(val);
    var test = val.toFixed(5);
    var tests = addComma(test);
    return tests;
}
function formatPrice6digit(val, row) {
    if (val == null) {
        val = 0.00;
    }
    val = parseFloat(val);
    var test = val.toFixed(6);
    var tests = addComma(test);
    return tests;
}

function formatPrice8digit(val, row) {
    if (val == null) {
        val = 0.00;
    }
    val = parseFloat(val);
    var test = val.toFixed(8);
    var tests = addComma(test);
    return tests;
}

function formatPriceWithoutZero(val, row) {
    if (val == null) {
        return "";
    }
    val = parseFloat(val);
    if (val == 0)
    {
        return "";
    }

    var test = val.toFixed(2);
    var tests = addComma(test);
    return tests;
}

function addComma(nStr) {
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
//end price format 


////start conversion

///region GetKG
/// <summary>
/// Convert LBS into KG
/// </summary>
/// <param name="nKG"></param>
/// /// <param name="nDigit"></param>
/// <returns>Retrun KG value of LBS</returns>
function GetKG(nLBS, nDigit) {
    if (nDigit > 8) nDigit = 8;
    nLBS = nLBS * 0.45359237001003542909395360718511;
    // nLBS = nLBS * (1 /2.2046226218);
    return nLBS.toFixed(nDigit);
}
//endregion GetKG

//start GetLBS
/// <summary>
/// Convert KG into LBS
/// </summary>
/// <param name="nKG"></param>
/// /// <param name="nDigit"></param>
/// <returns>Retrun LBS value of KG</returns>
function GetLBS(nKG, nDigit) {
    if (nDigit > 8) nDigit = 8;
    nKG = nKG * 2.2046226218;
    // nKG = nKG *2.2046244201837775;
    return nKG.toFixed(nDigit);
}
//end GetLBS

///region GetMeter
/// <summary>
/// Convert Yard into Meter
/// </summary>
/// <param name="nYard"></param>
/// <param name="nDigit"></param>
/// <returns>Returns Meter value of Yard</returns>
function GetMeter(nYard, nDigit) {
    if (nDigit > 8) nDigit = 8;
    var nLengthInMeter = parseFloat(nYard) / parseFloat(1.0936132983);
    if (isNaN(nLengthInMeter)) nLengthInMeter = 0;
    else if (nLengthInMeter == "" || nLengthInMeter == null) nLengthInMeter = 0;
    return nLengthInMeter.toFixed(nDigit);
}
////endregion GetMeter

////region GetYard
/// <summary>
/// Convert Meter into Yard
/// </summary>
/// <param name="nMeter"></param>
/// <param name="nDigit"></param>
/// <returns>Returns Yard value of Meter</returns>
function GetYard(nMeter, nDigit) {
    if (nDigit > 8) nDigit = 8;
    var nLengthInYard = parseFloat(nMeter) * parseFloat(1.0936132983);
    if (isNaN(nLengthInYard)) nLengthInYard = 0;
    else if (nLengthInYard == "" || nLengthInYard == null) nLengthInYard = 0;
    return nLengthInYard.toFixed(nDigit);
}
////endregion GetYard

/////end conversion


//ICs Is Exist List, PropertyName, PropertyValue
function ICS_IsExist(oList, PropertyName, PropertyValue)//developed by Mahabub
{
   // debugger;
    for (var i = 0; i < oList.length; i++) {
        var oTempList = oList[i];
        if (oTempList[PropertyName] == PropertyValue) {
            return true;
        }
    }
    return false;
}


//ICs Is Exist List, PropertyName, PropertyValue
function ICS_IsExistForTwoProperty(oList, PropertyName1, PropertyValue1, PropertyName2, PropertyValue2)//developed by Mahabub
{
    //debugger;
    for (var i = 0; i < oList.length; i++) {
        var oTempList = oList[i];
        if (oTempList[PropertyName1] == PropertyValue1 && oTempList[PropertyName2] == PropertyValue2)
        {
            return true;
        }
    }
    return false;
}

//ICS_IsExistForArray for Array 
function ICS_IsExistInArray(nValue, aArray)
{
    var status = false;
    for (var i = 0; i < aArray.length; i++)
    {
        var nTempValue = aArray[i];
        if (nTempValue==nValue)
        {
            status = true;
            break;
        }
    }
    return status;
}

//Property Concation Write by : Md. Mahabub Alam
function ICS_PropertyConcatation(oList, sProperty) {
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

//Object find from List Write by : Md. Mahabub Alam
function ICS_FindObject(oList, sPropertyName, PropertyValue) {
    for (var i = 0; i < oList.length; i++) {
        var oTempList = oList[i];
        if (oTempList[sPropertyName] == PropertyValue) {
            return oTempList;
        }
    }
    return null;
}

function ICS_FindObjects(oList, sPropertyName, PropertyValue)
{
    var oNewList = [];
    for (var i = 0; i < oList.length; i++)
    {
        var oTempList = oList[i];
        if (oTempList[sPropertyName] == PropertyValue)
        {
            oNewList.push(oTempList);
        }
    }
    return oNewList;
}

function ICS_RemoveObject(oList, sPropertyName, PropertyValue)
{
    var oNewList = [];
    for (var i = 0; i < oList.length; i++)
    {
        var oTempList = oList[i];
        if (oTempList[sPropertyName]!= PropertyValue)
        {
            oNewList.push(oTempList);
        }
    }
    return oNewList;
}

function ICS_FindObjectforDoubleProperty(oList, sPropertyName, PropertyValue,sPropertyName2, PropertyValue2) {
    for (var i = 0; i < oList.length; i++) {
        var oTempList = oList[i];
        if (oTempList[sPropertyName] == PropertyValue && oTempList[sPropertyName2] == PropertyValue2) {
            return oTempList;
        }
    }
    return null;
}
//Object find and remove from List Write by : Md. Mahabub Alam
function ICS_POP(oList, sPropertyName, PropertyValue) {
    for (var i = 0; i < oList.length; i++) {
        var oTempList = oList[i];
        if (oTempList[sPropertyName] == PropertyValue) {
            oList.splice(i, 1);
        }
    }
    return oList;
}

//ICS_TotalCalculation Write by : Md. Mahabub Alam
function ICS_TotalCalculation(oList, sPropertyName) {
    //debugger;
    var nTotalValue = 0;
    if (oList.length > 0) {
        for (var i = 0; i < oList.length; i++)
        {
            var oTempField = oList[i];
            nTotalValue+= parseFloat(oTempField[sPropertyName]);
        }
    }
    return nTotalValue;
}
//ICS_ListSum Write by : Ali Akram Oct 17, 2019
function ICS_ListSum(oList, sPropertyName) {
    //debugger;
    var nTotalValue = 0.0;
    if (oList.length > 0) {
        for (var i = 0; i < oList.length; i++) {
            var oTempField = oList[i];
            nTotalValue += parseFloat(oTempField[sPropertyName]);
        }
    }
    return nTotalValue;
}

// Permission Checker
function PermissionChecker(sOperationType, sModuleName, oAURoles) {
    debugger;
    if (JSON.parse(sessionStorage.getItem('IsSuperUser')) === true) //check SuperUser
    {
        return true;
    }
    else {
        for (var i = 0; i < oAURoles.length; i++) {
            if (oAURoles[i].OperationTypeST === sOperationType && oAURoles[i].ModuleNameST === sModuleName) {
                return true;
            }
        }
        return false;
    }
}

///start convert number value into Taka string
function TakaWords(inputValue) {
    if (!inputValue || inputValue === '' || inputValue === 0) { return ''; }
    else { return InWords(inputValue, "Taka", "Paisa"); }
}

function InWords(inputValue, beforeDecimal, afterDecimal) {
    //debugger;
    var commaCount = 0, digitCount = 0;
    var sign = "", takaWords = "", numStr = "", taka = "", paisa = "", pow = "";
    var pows = ["Crore", "Thousand", "Lakh"];

    if (inputValue < 0) {
        sign = "Minus";
        inputValue = Math.abs(inputValue);
    }

    numStr = inputValue.toFixed(2);
    paisa = HundredWords(parseInt(Right(numStr, 2)));

    if (paisa != "") {
        paisa = paisa.substring(0, 1).toUpperCase() + paisa.substring(1);
        paisa = afterDecimal + " " + paisa;
    }

    numStr = Left(numStr, numStr.length - 3);
    taka = HundredWords(parseInt(Right(numStr, 3)));

    if (numStr.length <= 3) {
        numStr = "";
    }
    else {
        numStr = Left(numStr, numStr.length - 3);
    }

    commaCount = 1;
    if (numStr != "") {
        do {
            if (commaCount % 3 == 0) {
                digitCount = 3;
            }
            else {
                digitCount = 2;
            }

            pow = HundredWords(parseInt(Right(numStr, digitCount)));
            if (pow != "") {
                if ((inputValue).toString().length > 10) {
                    //pow = pow + " " + pows[commaCount % 3] + " crore ";//By Abdullah
                    pow = pow + " " + pows[commaCount % 3];
                }
                else {
                    pow = pow + " " + pows[commaCount % 3];
                }
            }
            if (taka != "") {
                if (pow != "") {
                    pow = pow + " ";
                }
            }

            taka = pow + taka;
            if (numStr.length <= digitCount) {
                numStr = "";
            }
            else {
                numStr = Left(numStr, numStr.length - digitCount);
            }
            commaCount = commaCount + 1;

        }
        while (numStr != "");
    }

    if (taka != "") {
        taka = taka.substring(0, 1).toUpperCase() + taka.substring(1);
        taka = beforeDecimal + " " + taka;
    }
    takaWords = taka;

    if (takaWords != "") {
        if (paisa != "") {
            takaWords = takaWords + " And ";
        }
    }
    takaWords = takaWords + paisa;

    if (takaWords == "") {
        takaWords = beforeDecimal + " Zero";
    }
    takaWords = sign + takaWords + " Only";
    return takaWords;
}

function HundredWords(inputValue) {
    var hundredWords = "", numStr = "", pos1 = "", pos2 = "", pos3 = "";
    var digits = ["", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine"];
    var teens = ["Ten", "Eleven", "Twelve", "Thirteen", "Forteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"];
    var tens = ["", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"];

    numStr = Right(icsCustomStringFormat(inputValue, "000"), 3);
    if (Left(numStr, 1) != "0") {
        pos1 = digits[parseInt(Left(numStr, 1))] + " Hundred";
    }
    else {
        pos1 = "";
    }

    numStr = Right(numStr, 2);
    if (Left(numStr, 1) == "1") {
        pos2 = teens[parseInt(Right(numStr, 1))].toString();
        pos3 = "";
    }
    else {
        pos2 = tens[parseInt(Left(numStr, 1))].toString();
        pos3 = digits[parseInt(Right(numStr, 1))].toString();
    }
    hundredWords = pos1;
    if (hundredWords != "") {
        if (pos2 != "") {
            hundredWords = hundredWords + " ";
        }
    }
    hundredWords = hundredWords + pos2;

    if (hundredWords != "") {
        if (pos3 != "") {
            hundredWords = hundredWords + " ";
        }
    }
    hundredWords = hundredWords + pos3;

    return hundredWords;
}

function Right(intputString, length) {
    var retStr = "";
    if (length < intputString.length && length > 0) {
        retStr = intputString.substring((intputString.length - length), intputString.length);
    }
    else {
        retStr = intputString;
    }
    return retStr;
}

function Left(intputString, length) {
    var retStr = "";
    if (length < intputString.length) {
        retStr = intputString.substring(0, length);
    }
    else {
        retStr = intputString;
    }
    return retStr;
}


///end convert number value into Taka string