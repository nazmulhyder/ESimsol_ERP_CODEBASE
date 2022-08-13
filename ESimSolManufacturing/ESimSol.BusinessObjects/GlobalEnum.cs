using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using ICS.Core.Utility;
using System.Collections.Generic;
using System.Reflection;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region DateObject
    public class DateObject
    {
        #region Date Compare
        public static void CompareDateQuery(ref string sReturn, string sSearchDate, int nDateCriteria, DateTime dStartDate, DateTime dEndDate)
        {
            #region COMPARE DATE

            ConvertToVarchar(ref sSearchDate);  // = "CONVERT(DATE,CONVERT(VARCHAR(12)," + sSearchDate + ",106))";
            string sStartDate = ConvertToVarchar(dStartDate),
                   sEndDate = ConvertToVarchar(dEndDate);

            if (nDateCriteria > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nDateCriteria == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + sSearchDate + " = " + sStartDate + " ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + sSearchDate + " != " + sStartDate + " ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + sSearchDate + " > " + sStartDate + " ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + sSearchDate + " < " + sStartDate + " ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + sSearchDate + " BETWEEN " + sStartDate + " AND " + sEndDate + " ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + sSearchDate + " NOT BETWEEN " + sStartDate + " AND " + sEndDate + " ";
                }
            }
            #endregion
        }
        public static bool CompareDateQuery(ref string sReturn, string sSearchDate, string sSearchStringDate)
        {
            if (!string.IsNullOrEmpty(sSearchStringDate) && sSearchStringDate.Split('~').Length == 3)
            {
                #region COMPARE DATE
                int nDateCriteria = Convert.ToInt32(sSearchStringDate.Split('~')[0]);
                DateTime StartTime = Convert.ToDateTime(sSearchStringDate.Split('~')[1]);
                DateTime EndTime = Convert.ToDateTime(sSearchStringDate.Split('~')[2]);

                ConvertToVarchar(ref sSearchDate);  // = "CONVERT(DATE,CONVERT(VARCHAR(12)," + sSearchDate + ",106))";
                string sStartDate = ConvertToVarchar(StartTime),
                       sEndDate = ConvertToVarchar(EndTime);

                if (nDateCriteria > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (nDateCriteria == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + sSearchDate + " = " + sStartDate + " ";
                    }
                    else if (nDateCriteria == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + sSearchDate + " != " + sStartDate + " ";
                    }
                    else if (nDateCriteria == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + sSearchDate + " > " + sStartDate + " ";
                    }
                    else if (nDateCriteria == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + sSearchDate + " < " + sStartDate + " ";
                    }
                    else if (nDateCriteria == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + sSearchDate + " BETWEEN " + sStartDate + " AND " + sEndDate + " ";
                    }
                    else if (nDateCriteria == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + sSearchDate + " NOT BETWEEN " + sStartDate + " AND " + sEndDate + " ";
                    }
                    return true;
                }
                else 
                    return false;
                #endregion
            }
            else return false;
        }
        public static void ConvertToVarchar(ref string Date) 
        {
            Date = "CONVERT(DATE,CONVERT(VARCHAR(12)," + Date + ",106))";
        }
        public static string ConvertToVarchar(DateTime Date)
        {
            return "CONVERT(DATE,CONVERT(VARCHAR(12),'" + Date.ToString("dd MMM yyyy") + "',106))";
        }
        public static string DateTimeToVarchar(DateTime Date)
        {
            return "CONVERT(DATETIME,CONVERT(VARCHAR(12),'" + Date.ToString("dd MMM yyyy HH:mm:ss") + "',106))";
        }
        #endregion

        #region Get Date
        public static string GetDate(DateTime dDate)
        {
            if (dDate == DateTime.MinValue)
            {
                return "-";
            }
            else
            {
                return dDate.ToString("dd MMM yyyy");
            }
        }
        #endregion
    }
    #endregion

    #region GlobalObject
    public class GlobalObject
    {
        #region Report Header BU to Company
        public static Company BUTOCompany(Company oCompany, BusinessUnit oBusinessUnit)
        {
            oCompany.Name = oBusinessUnit.Name;
            oCompany.Address = oBusinessUnit.Address;
            oCompany.Phone = oBusinessUnit.Phone;
            oCompany.Email = oBusinessUnit.Email;
            oCompany.WebAddress = oBusinessUnit.WebAddress;
            return oCompany;
        }
        #endregion


        #region Qty In Word
        public static string Left(string intputString, int Length)
        {
            string retStr = "";
            if (Length < intputString.Length)
            {
                retStr = intputString.Substring(0, Length);
            }
            else
            {
                retStr = intputString;
            }
            return retStr;
        }
        public static string Right(string intputString, int Length)
        {
            string retStr = "";
            if (Length < intputString.Length && Length > 0)
            {
                retStr = intputString.Substring((intputString.Length - Length), Length);
            }
            else
            {
                retStr = intputString;
            }
            return retStr;
        }
        private static string HundredWords(int inputValue)
        {
            string hundredWords = "", numStr = "", pos1 = "", pos2 = "", pos3 = "";
            string[] digits = new string[10] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] teens = new string[10] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tens = new string[10] { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            numStr = Right(inputValue.ToString("000"), 3);
            if (Left(numStr, 1) != "0")
            {
                pos1 = digits.GetValue(Convert.ToInt32(Left(numStr, 1))) + " Hundred";
            }
            else
            {
                pos1 = "";
            }

            numStr = Right(numStr, 2);
            if (Left(numStr, 1) == "1")
            {
                pos2 = Convert.ToString(teens.GetValue(Convert.ToInt32(Right(numStr, 1))));
                pos3 = "";
            }
            else
            {
                pos2 = Convert.ToString(tens.GetValue(Convert.ToInt32(Left(numStr, 1))));
                pos3 = Convert.ToString(digits.GetValue(Convert.ToInt32(Right(numStr, 1))));
            }
            hundredWords = pos1;
            if (hundredWords != "")
            {
                if (pos2 != "")
                {
                    hundredWords = hundredWords + " ";
                }
            }
            hundredWords = hundredWords + pos2;

            if (hundredWords != "")
            {
                if (pos3 != "")
                {
                    hundredWords = hundredWords + " ";
                }
            }
            hundredWords = hundredWords + pos3;

            return hundredWords;
        }
        private static string HundredWordsAfterPoint(int inputValue)
        {
            string hundredWords = "", numStr = "", pos1 = "", pos2 = "", pos3 = "";
            string[] digits = new string[10] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };

            numStr = Right(inputValue.ToString("000"), 3);
            if (Left(numStr, 1) != "0")
            {
                pos1 = digits.GetValue(Convert.ToInt32(Left(numStr, 1))) + " Hundred";
            }
            else
            {
                pos1 = "";
            }

            numStr = Right(numStr, 2);
            if (numStr.Length == 1)
            {
                pos2 = Convert.ToString(digits.GetValue(Convert.ToInt32(Right(numStr, 1))));
                pos3 = "";
            }
            else
            {
                pos2 = Convert.ToString(digits.GetValue(Convert.ToInt32(Left(numStr, 1))));
                pos3 = Convert.ToString(digits.GetValue(Convert.ToInt32(Right(numStr, 1))));
            }
            hundredWords = pos1;
            if (hundredWords != "")
            {
                if (pos2 != "")
                {
                    hundredWords = hundredWords + " ";
                }
            }
            hundredWords = hundredWords + pos2;

            if (hundredWords != "")
            {
                if (pos3 != "")
                {
                    hundredWords = hundredWords + " ";
                }
            }
            hundredWords = hundredWords + pos3;

            return hundredWords;
        }
        public static string QtyInWords(double inputValue, string sSymbol)
        {
            int commaCount = 0, digitCount = 0;
            string sign = "", inWords = "", numStr = "", completenumber = "", decimalnumber = "", pow = "";
            string[] pows = new string[3] { "Crore", "Thousand", "Lac" };

            if (inputValue < 0)
            {
                sign = "Minus";
                inputValue = Math.Abs(inputValue);
            }

            numStr = inputValue.ToString("0.00");
            decimalnumber = HundredWordsAfterPoint(Convert.ToInt32(Right(numStr, 2)));

            if (decimalnumber != "")
            {
                decimalnumber = decimalnumber.Substring(0, 1).ToUpper() + decimalnumber.Substring(1);                
            }

            numStr = Left(numStr, numStr.Length - 3);
            completenumber = HundredWords(Convert.ToInt32(Right(numStr, 3)));

            if (numStr.Length <= 3)
            {
                numStr = "";
            }
            else
            {
                numStr = Left(numStr, numStr.Length - 3);
            }

            commaCount = 1;
            if (numStr != "")
            {
                do
                {
                    if (commaCount % 3 == 0)
                    {
                        digitCount = 3;
                    }
                    else
                    {
                        digitCount = 2;
                    }

                    pow = HundredWords(Convert.ToInt32(Right(numStr, digitCount)));
                    if (pow != "")
                    {
                        if (Convert.ToString(inputValue).Length > 10)
                        {
                            //pow = pow + " " + pows.GetValue(commaCount % 3) + " crore ";//By Abdullah
                            pow = pow + " " + pows.GetValue(commaCount % 3);
                        }
                        else
                        {
                            pow = pow + " " + pows.GetValue(commaCount % 3);
                        }
                    }
                    if (completenumber != "")
                    {
                        if (pow != "")
                        {
                            pow = pow + " ";
                        }
                    }

                    completenumber = pow + completenumber;
                    if (numStr.Length <= digitCount)
                    {
                        numStr = "";
                    }
                    else
                    {
                        numStr = Left(numStr, numStr.Length - digitCount);
                    }
                    commaCount = commaCount + 1;

                }
                while (numStr != "");
            }

            if (completenumber != "")
            {
                completenumber = completenumber.Substring(0, 1).ToUpper() + completenumber.Substring(1);                
            }
            inWords = completenumber;

            if (inWords != "")
            {
                if (decimalnumber != "")
                {
                    inWords = inWords + " Point ";
                }
            }
            inWords = inWords + decimalnumber;

            if (inWords == "")
            {
                inWords = " Zero";
            }
            inWords = sign + inWords + " " + sSymbol;
            return inWords;
        }
        #endregion

    }
    #endregion

    #region EnumObject
    [Serializable]
    public class EnumObject 
    {
        public EnumObject()
        {
            id = 0;
            Value = "";
            Description = "";
        }
        public int id { get; set; }
        //public string Value { get; private set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public static List<EnumObject> jGets(Type oEnumType)
        {
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            EnumObject oEnumObject = new EnumObject();
            var FI = oEnumType.GetFields();
            Array attributes;

            for (int i = 0; i < FI.Length; i++)
            {
                if (!FI[i].FieldType.IsEquivalentTo(oEnumType)) continue;

                oEnumObject = new EnumObject();
                oEnumObject.id = (int)FI[i].GetValue(i);
                oEnumObject.Value = Global.StringPattern.Replace(FI[i].GetValue(i).ToString(), " ");
                oEnumObject.Description = oEnumObject.Value;

                //if there is any user custome description exist then that will be replaced (as priority)
                attributes = FI[i].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    oEnumObject.Description = ((DescriptionAttribute)attributes.GetValue(0)).Description;
                    oEnumObject.Value = oEnumObject.Description;
                }
                oEnumObjects.Add(oEnumObject);
            }
            return oEnumObjects;
        }
        public static string jGet(object eEnumValue)
        {
            string sReturn = "";
            FieldInfo field = eEnumValue.GetType().GetField(eEnumValue.ToString());
            if (field == null) { return ""; }

            sReturn = Global.StringPattern.Replace(field.GetValue(eEnumValue).ToString(), " ");
            Array attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                sReturn = ((DescriptionAttribute)attributes.GetValue(0)).Description;
            }
            return sReturn;
        }
    }
    #endregion

    #region SalesType
    public class SalesTypeObj
    {
        public SalesTypeObj()
        {
            Id = 0;
            Value = "";
        }
        public int Id { get; set; }
        public string Value { get; set; }
        public static string GetStringSalesType(EnumSalesType eEnumSalesType)
        {
            string sEnumSalesTypeObj = "";
            switch (eEnumSalesType)
            {
                case EnumSalesType.CashSale:
                    sEnumSalesTypeObj = "Cash Sale";
                    break;
                case EnumSalesType.CreditSale:
                    sEnumSalesTypeObj = "Credit Sale";
                    break;
                default:
                    sEnumSalesTypeObj = eEnumSalesType.ToString();
                    break;
            }
            return sEnumSalesTypeObj;
        }

        public static List<SalesTypeObj> Gets()
        {
            List<SalesTypeObj> oSalesTypeObjs = new List<SalesTypeObj>();
            SalesTypeObj oSalesTypeObj = new SalesTypeObj();
            foreach (int oItem in Enum.GetValues(typeof(EnumSalesType)))
            {
                oSalesTypeObj = new SalesTypeObj();
                oSalesTypeObj.Id = oItem;
                oSalesTypeObj.Value = SalesTypeObj.GetStringSalesType((EnumSalesType)oItem);
                oSalesTypeObjs.Add(oSalesTypeObj);
            }
            return oSalesTypeObjs;
        }
    }
    #endregion

    #region FabricBatchStateObj
    public class FabricBatchStateObj
    {
        public FabricBatchStateObj()
        {
            id = 0;
            Value = "";
        }
        public int id { get; set; }
        public string Value { get; set; }
        public static List<FabricBatchStateObj> Gets()
        {
            List<FabricBatchStateObj> oFabricBatchStateObjs = new List<FabricBatchStateObj>();
            FabricBatchStateObj oFabricBatchStateObj = new FabricBatchStateObj();
            foreach (int oItem in Enum.GetValues(typeof(EnumFabricBatchState)))
            {
                oFabricBatchStateObj = new FabricBatchStateObj();
                oFabricBatchStateObj.id = oItem;
                oFabricBatchStateObj.Value = GetEnumFabricBatchStateObjs((EnumFabricBatchState)oItem);
                oFabricBatchStateObjs.Add(oFabricBatchStateObj);
            }
            return oFabricBatchStateObjs;
        }
        public static string GetEnumFabricBatchStateObjs(EnumFabricBatchState eEnumFabricBatchState)
        {
            string sEnumFabricBatchState = "";
            switch (eEnumFabricBatchState)
            {
                case EnumFabricBatchState.Initialize:
                    sEnumFabricBatchState = "Initialize";
                    break;
                case EnumFabricBatchState.InFloor:
                    sEnumFabricBatchState = "In Floor";
                    break;
                case EnumFabricBatchState.Warping:
                    sEnumFabricBatchState = "Warping";
                    break;
                case EnumFabricBatchState.warping_Finish:
                    sEnumFabricBatchState = "Warping Finish";
                    break;
                case EnumFabricBatchState.Sizing:
                    sEnumFabricBatchState = "Sizing";
                    break;
                case EnumFabricBatchState.Sizing_Finish:
                    sEnumFabricBatchState = "Sizing Finish";
                    break;
                case EnumFabricBatchState.DrawingIn:
                    sEnumFabricBatchState = "Drawing In";
                    break;
                case EnumFabricBatchState.DrawingIn_Finish:
                    sEnumFabricBatchState = "Drawing In Finish";
                    break;
                case EnumFabricBatchState.Weaving:
                    sEnumFabricBatchState = "Weaving";
                    break;
                case EnumFabricBatchState.Weaving_Finish:
                    sEnumFabricBatchState = "Weaving Finish";
                    break;
                case EnumFabricBatchState.In_QC:
                    sEnumFabricBatchState = "In QC";
                    break;
                case EnumFabricBatchState.QCDone:
                    sEnumFabricBatchState = "QC Done";
                    break;
                case EnumFabricBatchState.InStore_Partial:
                    sEnumFabricBatchState = "In Delivery Store (Partial)";
                    break;
                case EnumFabricBatchState.InStore:
                    sEnumFabricBatchState = "In Delivery Store";
                    break;

                case EnumFabricBatchState.Finish:
                    sEnumFabricBatchState = "Finish";
                    break;
                default:
                    sEnumFabricBatchState = sEnumFabricBatchState.ToString();
                    break;
            }
            return sEnumFabricBatchState;
        }
    }
    #endregion

    #region Enumeration : EnumWUSubContractStatus
    public enum EnumWUSubContractStatus
    {
        None = 0,
        [Description("Initialize")]
        Initialize = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Request For Revise")]
        ReqForRevise = 3,
        [Description("Cancel")]
        Cancel = 4
    }
    #endregion

    #region Enumeration : EnumWUYarnChallanStatus
    public enum EnumWUYarnChallanStatus
    {
        None = 0,
        [Description("Yet To Challan Start")]
        YetToChallanStart = 1,
        [Description("Partial Challan")]
        PartialChallan = 2,
        [Description("Challan Done")]
        ChallanDone = 3
    }
    #endregion

    #region Enumeration : EnumWUFabricRcvStatus
    public enum EnumWUFabricRcvStatus
    {
        None = 0,
        [Description("Yet To Receive Start")]
        YetToRcvStart = 1,
        [Description("Partial Receive")]
        PartialRcv = 2,
        [Description("Receive Done")]
        RcvDone = 3
    }
    #endregion

    #region Enumeration : EnumWUOrderType
    public enum EnumWUOrderType
    {
        None = 0,
        [Description("Sub-Contract")]
        SubContract = 1
    }
    #endregion

    #region Enumeration : EnumWSCWorkType
    public enum EnumWSCWorkType
    {
        None = 0,
        [Description("Warping")]
        Warping = 1,
        [Description("Sizing")]
        Sizing = 2,
        [Description("Weaving")]
        Weaving = 3,
        [Description("Warping & Sizing")]
        WarpingSizing = 4,
        [Description("Sizing & Weaving")]
        SizingWeaving = 5,
        [Description("Warping, Sizing & Weaving")]
        WarpingSizingWeaving = 6
    }
    #endregion

    #region Enumeration : EnumTransportation
    public enum EnumTransportation
    {
        None = 0,
        [Description("Own Transport")]
        OwnTransport = 1,
        [Description("Arrange By Party")]
        ArrangeByParty = 2
    }
    #endregion

    #region Enummeration : EnumGWDApplyOn
    public enum EnumGWDApplyOn
    {
        None = 0,
        Actual = 1,
        Compliance = 2,
        Both = 3,
    }
    #endregion

    #region Enumeration : EnumCMType
    public enum EnumCMType
    {
        None = 0,
        [Description("CM")]
        CM = 1,
        [Description("Rent + Utility")]
        RentUtility = 2,
        [Description("Overhead")]
        Overhead = 3
    }
    #endregion

    #region Enumeration : MarriedStatus
    public enum EnumMarriedStatus
    {
        None = 0,
        Single = 1,
        Married = 2
    }
    #endregion
  
    #region Enummeration : EnumFGQCRefType
    public enum EnumFGQCRefType
    {
        None = 0,        
        QC = 1,
        RouteSheet = 2
    }
    #endregion

    #region Enummeration : EnumAccountEffectType
    public enum EnumAccountEffectType
    {
        [Description("--Account Effect Type--")]
        None = 0,
        [Description("Bills Payable Effect")]
        Bills_Payable_Effect = 1,//For Import LC Purchase
        [Description("GRN Effect")]
        GRN_Effect = 2, //For Import LC Purchase
        [Description("BTB Liability Effect")]
        BTB_Liability_Effect = 3,
        [Description("BTB Bill Liability Effect")]
        BTB_Bill_Liability_Effect = 4,
        [Description("Cash Sale Bank Effect")]
        Cash_Sale_Bank_Effect = 5,
        [Description("Export Sale Bank Effect")]
        Export_Sale_Bank_Effect = 6,
        [Description("Import Payment Settlement")]
        Import_Payment_Settlement = 7,
        [Description("Raw Material Consumption")]
        Raw_Material_Consumption = 8,
        [Description("GRN Effect(Local)")]
        GRN_Effect_Local = 9,
        [Description("Bills Payable Effect(Local)")]
        Bills_Payable_Effect_Local = 10,
        [Description("Adjustment Effect(Addition)")]
        Adjustment_Effect_Addition = 11,
        [Description("Adjustment Effect(Deduction)")]
        Adjustment_Effect_Deduction = 12,
        [Description("Material Transfer Effect(Issue)")]
        Material_Transfer_Effect_Issue = 13,
        [Description("Material Transfer Effect(Receive)")]
        Material_Transfer_Effect_Receive = 14,
        [Description("Consumable Goods Consumption Effect")]
        Consumable_Goods_Consumption_Effect = 15,
        [Description("Loan Refund Effect")]
        Loan_Refund_Effect = 16
    }
    #endregion

    #region Enummeration : EnumBalanceStatus
    public enum EnumBalanceStatus
    {
        [Description("--Balance Status--")]
        None = 0,
        [Description("Only Opening")]
        OnlyOpening = 1,
        [Description("Only Closing")]
        OnlyClosing = 2,
        [Description("Debit Balance")]
        DebitBalance = 3,
        [Description("Credit Balance")]
        CreditBalance = 4,
        [Description("All Item(Include Zero Balance)")]
        All_Item_Include_Zero_Balance = 5
    }
    #endregion

    #region Enummeration : EnumReconcileDataType
    public enum EnumReconcileDataType 
    { 
        None = 0, 
        ReconcileDone = 1, 
        ReconcilePrepare = 2 
    }
    #endregion

    #region Enummeration : EnumForeignExchangeGainLoss
    public enum EnumForExGainLoss
    {
        None = 0,
        Gain = 1,
        Loss = 2,
        NoEffect = 3
    }
    #endregion

    #region Enummeration : EnumVOrderRefType
    public enum EnumVOrderRefType
    {
        None = 0,
        Manual = 1,
        ExportPI = 2,
        ImportPI = 3,
        ExportLC = 4,
        ImportLC = 5,
        SampleInvoice = 6,
        PurchaseInvoice = 7,
        WorkOrder = 8,
        Product = 9,
        Adjustment = 10, //VOrderRefID id will be AdjustmentRequisitionSlipID
        SubContract = 11, //VOrderRefID id will be SubContractID
        TransferRequisition = 12, //VOrderRefID id will be TransferRequisitionSlipID
        Expenditure = 100
    }
    #endregion

    #region Enummeration : EnumCRStatus
    public enum EnumCRStatus
    {
        Initiallize = 0,
        RequestForApproval = 1,
        Approve = 2,
        Request_Revise = 3,
        StockOut = 4
    }
    #endregion
    #region Enummeration : EnumSPStatus
    public enum EnumSPStatus
    {
        Initiallize = 0,
        [Description("Request For Approval")]
        RequestForApproval = 1,
        [Description("Approve")]
        Approve = 2,
        [Description("In Store")]
        InStore = 3,
        [Description("Partial Disverse")]
        PartialDisverse = 4,
        [Description("Disverse")]
        Disburse = 5,
    }
    #endregion

    #region Enummeration : EnumCRActionType
    public enum EnumCRActionType
    {
        None = 0,
        RequestForApproval = 1,
        Approve = 2,
        UndoApprove = 3,
        Request_revise = 4,
        StockOut = 5,
        UndoRequest = 6
    }
    #endregion

    #region Enummeration : EnumConsumptionType
    public enum EnumConsumptionType
    {
        None = 0,
        GeneralConsumption = 1,
        RawMaterialConsumption = 2
    }
    #endregion

    #region Enummeration : EnumCRRefType
    public enum EnumCRRefType
    {
        None = 0,
        Order = 1,
        Dispo = 2
    }
    #endregion

    #region Enummeration : EnumMKTRef
    public enum EnumMKTRef
    {
        None = 0,
        FabricID = 1,
    }
    #endregion

    #region Enummeration : EnumDevRecapStatus
    public enum EnumDevelopmentStatus
    {
        Initialize = 0,
        RequestForApproved = 1,
        ApprovedDone = 2,
        Inproduction = 3, //Send to Factory
        RawMaterialCollectionDone = 4,
        QCDone = 5,
        ReceivefromFactory = 6,
        SendtoBuyer = 7,
        Feedbackfrombuyer = 8,
        OrderConfirmation = 9,
        Cancel = 10

    }
    #endregion

    #region Enummeration : EnumTSType
    public enum EnumTSType
    {
        Sweater = 0,
        Knit = 1,
        Woven = 2
    }
    #endregion
    #region Enummeration : EnumTimeCardFormat
    public enum EnumTimeCardFormat
    {
        [Description("Time Card F-01")]
        Time_Card_F_01 = 1,
        [Description("Time Card F-02")]
        Time_Card_F_02 = 2,
        [Description("Time Card F-03")]
        Time_Card_F_03 = 3,
        [Description("Time Card F-04")]
        Time_Card_F_04 = 4,
        [Description("Time Card F-05")]
        Time_Card_F_05 = 5,
        [Description("Time Card F-06")]
        Time_Card_F_06 = 6,
        [Description("Time Card F-07")]
        Time_Card_F_07 = 7,
        [Description("Time Card F-08")]
        Time_Card_F_08 = 8
    }
    #endregion

    #region Enummeration : EnumSalarySheetFormat
    public enum EnumSalarySheetFormat
    {
        [Description("Salary Sheet F-01")]
        Salary_Sheet_F_01 = 1,
        [Description("Salary Sheet F-02")]
        Salary_Sheet_F_02 = 2,
        [Description("Salary Sheet F-03")]
        Salary_Sheet_F_03 = 3,
        [Description("Pay Slip F-01")]
        Pay_Slip_F_01 = 4,
        [Description("Pay Slip F-02")]
        Pay_Slip_F_02 = 5,
        [Description("Pay Slip F-03")]
        Pay_Slip_F_03 = 6,
        [Description("BankSheet")]
        BankSheet = 7,
        [Description("CashSheet")]
        CashSheet= 8,
        [Description("Salary Sheet F-04")]
        Salary_Sheet_F_04 =9,
        [Description("Salary Sheet F-05")]
        Salary_Sheet_F_05 = 10
       
    }
    #endregion

    #region Enummeration : EnumLeaveApplicationFormat
    public enum EnumLeaveApplicationFormat
    {
        [Description("Leave Application")]
        Leave_Application = 1,
        [Description("Leave Application XL")]
        Leave_Application_XL = 2,
        [Description("Alternative Duty")]
        Alternative_Duty = 3

    }
    #endregion
    #region Enummeration : EnumStepType
    public enum EnumStepType
    {
        Approval = 0,
        Production = 1
    }
    #endregion

    #region Enummeration : EnumTnAStep
    public enum EnumTnAStep
    {
        None = 0,
        Labdips = 1,
        [Description("Print s/off")]
        Prints_off = 2,
        FittingSample = 3,
        Accessories = 4,
        SampleFabrics=5,
        CollectionSample = 6,
        PreCuttingFile = 7,
        BulkProgram = 8,
        YarnInhoueBy = 9,
        Knitting = 10,
        DyeingBy = 11,
        ForeignAccessoriesInhouseBy = 12,
        PossibleCuttingDate = 13,
        PossibleInputDate = 14,
        [Description("Sewing/days")]
        SewingPerDays = 15,
        FinishingClose = 16,
        PreFinal=17,
        FinalInspection = 18
    }
    #endregion

    #region Enummeration : EnumWindingStatus
    public enum EnumWindingStatus
    {
        None=0,
        Initialize=1, 
        Running=2,
        Completed= 3, 
        Delivered=4     
    }
    #endregion

    #region Enummeration : EnumTwistingStatus
    public enum EnumTwistingStatus
    {
        None = 0,
        Initialize = 1,
        Approve = 2,
        Completed = 3,
        //Delivered = 4
    }
    #endregion

    #region Enummeration : EnumTwistingOrderType
    public enum EnumTwistingOrderType
    {
        None = 0,
        [Description("Open Twisting")]
        OpenTwisting = 1,
        [Description("Twisting Outside")]
        TwistingOutside = 2,
        [Description("Twisting Inhouse")]
        TwistingInhouse = 3
    }
    #endregion

    #region EnumImageType : ImageType
    public enum EnumImageType
    {
        Select_Image_Type = 0,
        FrontImage = 1,
        BackImage = 2,
        MeasurmentSpecImage = 3,
        NormalImage = 4,
        ModelImage=5,
        ExteriorFrontImage = 6,
        ExteriorBackImage = 7,
        InteriorFrontImage = 8,
        InteriorBackImage = 9,
        SideImage = 10,
        TopImage = 11
    }
    #endregion

    #region Enummeration : EnumSourcingConfigHeadType
    public enum EnumSourcingConfigHeadType
    {
        None = 0,
        Wash = 1,
        GSM = 2,
        [Description("Finish DIA")]
        FinishDIA = 3,
        [Description("Machine DIA")]
        MachineDIA = 4,
        [Description("Yarn Type")]
        YarnType = 5,
        [Description("Fabric Type")]
        FabricType = 6,
        [Description("Gray DIA")]
        GrayDIA = 7
    }
    #endregion

    #region EnumEmployeeTimeCard : EnumEmployeeTimeCard
    public enum EnumEmployeeTimeCard
    {
        None = 0,
        [Description("Time Card-F1")]
        Time_Card_F1 = 101,
        [Description("Time Card-F2")]
        Time_Card_F2 = 102,
        [Description("Time Card-F2.1")]
        Time_Card_F2_1 = 103,
        [Description("Time Card-F3")]
        Time_Card_F3 = 104,
        [Description("Time Card-F4")]
        Time_Card_F4 = 105,
        [Description("Time Card-F5")]
        Time_Card_F5 = 106,
        [Description("Time Card-F6")]
        Time_Card_F6 = 107,
        [Description("Time Card-FC7")]
        Time_Card_FC7 = 108,
        [Description("AMG Worker")]
        Time_Card_AMG_Worker = 110,
        [Description("Time Card Worker")]
        Time_Card_Worker = 111,
        [Description("Job Card")]
        Job_Card = 112,
    }
    #endregion

    #region EnumIDCardFormat : EnumIDCardFormat
    public enum EnumIDCardFormat
    {
        None = 0,
        [Description("Protrait")]
        Protrait = 1,
        [Description("Landscape")]
        Landscape = 2,
        [Description("Both Side(Protrait)")]
        Both_Side_Protrait = 3,
        [Description("Both Side(Bangla)")]
        Both_Side_Bangla = 4,
        [Description("ID Card Bangla F1")]
        ID_Card_Bangla_F1 = 5,
        [Description("ID Card Bangla F2")]
        ID_Card_Bangla_F2 = 6,
        [Description("Both Side(Protrait)F2")]
        Both_Side_Protrait_F2 = 7
    }
    #endregion

    #region Enummeration : EnumFNProcess
    public enum EnumFNProcess
    {
        None = 0,
        Singeing = 10,
        Desizing = 20,
        Scouring = 30,
        Bleaching = 40,
        [Description("ReBleach/Mild Bleach")]
        ReBleach_Mild_Bleach = 50,
        Washing = 60,
        Mercerizing = 70,
        Stenter = 80,
        Sanforize = 90,
        Peaching = 100,
        Dyeing = 110,
        Inspection = 120

    }
    #endregion

    #region Enummeration : EnumFNTreatment
    public enum EnumFNTreatment
    {
        None = 0,
        [Description("Pre Treatment")]
        Pre_Treatment = 1,
        Dyeing = 2,
        Finishing = 3,
        Inspection = 4,
        Lab = 5,
        Printing = 6,
        Engraving = 7,
        QAD = 8,
        RnD = 9
       
    }
    #endregion

    #region Enummeration : EnumSalesType
    public enum EnumSalesType
    {
        None = 0,
        CashSale = 1,
        CreditSale = 2
    }
    #endregion

    #region Enummeration : EnumReportLayout
    public enum EnumReportLayout
    {
        None = 0,
        [Description("PI Wise")]
        PIWise = 1,
        [Description("Date Wise")]
        DateWise = 2,
        [Description("Date Wise(Details)")]
        DateWiseDetails = 3,
        [Description("Party Wise")]
        PartyWise = 4,
        [Description("Party Wise(Details)")]
        PartyWiseDetails = 5,
        [Description("Product Wise(Details)")]
        ProductWise = 6,
        [Description("LC Wise")]
        LCWise = 7,
        [Description("Invoice Wise")]
        InvoiceWise = 8,
        [Description("Bank Wise")]
        BankWise = 9,
        [Description("Challan Wise")]
        ChallanWise = 10,
        [Description("Factory Wise")]
        Factorywise = 11,
        [Description("ShipmentDate Wise")]
        ShipmentDateWise = 12,
        [Description("Buying Commission Statement")]
        Buying_Commission_Statement = 13,
        [Description("Product Catagory Wise")]
        ProductCatagoryWise = 14,
        [Description("Merchandiser Wise")]
        MerchandiserWise = 15,
        [Description("Yarn Fabric Wise")]
        YarnFabricWise = 16,
        [Description("Agent Wise")]
        AgentWise = 17,
        [Description("Department Wise")]
        DepartmentWise = 18,
        [Description("MOB Report")]
        MOBReport = 19,
        [Description("Month Wise")]
        Month_Wise = 20,
        [Description("Session Wise")]
        Session_Wise = 21,
        [Description("DO Wise")]
        DO_Wise = 22,
        [Description("Issue Store Wise")]
        Issue_Store_Wise = 23,
        [Description("Receive Store Wise")]
        Receive_Store_Wise = 24,
        [Description("PI Wise(Detail)")]
        PI_Wise_Details = 25,
        [Description("Vehicle Wise")]
        Vehicle_Wise = 26,
        [Description("Export LC Summery")]
        Export_LC_Summery = 27,
        [Description("Export LC PI Summery")]
        Export_LC_PI_Summery = 28,
        [Description("Export LC Details")]
        Export_LC_Details = 29,
        [Description("Style Wise")]
        Style_Wise = 30,
        [Description("Product Summary")]
        ProductSummary = 31,
        [Description("Mother Buyer Wise")]
        MotherBuyerWise = 32,
        [Description("Sales Commission Details")]
        SalesCommissionDetails = 33,
        [Description("Machine Wise")]
        Machine_Wise = 34,
        [Description("Order Wise")]
        Order_Wise = 35,
        [Description("TOP Sheet")]
        TOP_Sheet = 36,
        [Description("Marketing Person Wise")]
        Marteking_Person_Wise= 37,
        [Description("Vechile Model Wise")]
        Vechile_Model_Wise= 38,
        [Description("Order Wise Details")]
        Order_Wise_Details = 39,
        [Description("Style Wise Details")]
        Style_Wise_Details= 40,
        [Description("Consumption Group Summary")]
        Consumption_Group_Summary = 41,
        [Description("Loan Wise(Details)")]
        LoanWise = 42,
        [Description("Order Status Wise")]
        Order_Status_Wise = 43,
        [Description("QC Follow Up")]
        QC_Follow_Up = 44,
        [Description("LC With Amendment")]
        LCWithAmendment = 45,
        [Description("SP Consumption CR")]
        SPConsumptionCR = 46,
        [Description("SP Consumption CR Wise Parts")]
        SPConsumptionCRWiseParts = 47,
        [Description("SP Consumption Parts")]
        SPConsumptionParts = 48,
        [Description("SP Consumption Parts Wise CR")]
        SPConsumptionPartsWiseCR = 49,
        [Description("SP Consumption Transaction")]
        SPConsumptionTransaction = 50,
        [Description("Date Wise(Product Based)")]
        DateWise_ProductBased = 51,
        [Description("Party Wise(Product Based)")]
        PartyWise_ProductBased = 52,
        [Description("Product Wise(Product Based)")]
        ProductWise_ProductBased = 53,
        [Description("Date Wise(Payment Based)")]
        DateWise_PaymentBased = 54,
        [Description("Party Wise(Payment Based)")]
        PartyWise_PaymentBased = 55
    }
    #endregion

    #region Enummeration : EnumStockReportLayout
    public enum EnumStockReportLayout
    {
        ProductWise = 1,
        ProductAndColorWise = 2,
        ProductAndStoreWise = 3,
        ProductAndLotWise = 4
    }
    #endregion
    #region Enummeration : ProductGrade
    public enum EnumProductGrade
    {
        None = 0,
        GradeA = 1,
        GradeB = 2,
        GradeC = 3
    }
    #endregion
    
    #region Enummeration : EnumQtyType
    public enum EnumQtyType
    {
        None = 0,
        Percent = 1,
        Count = 2
    }
    #endregion

    #region Enummeration : EnumQtyType
    public enum EnumKnitDyeingProgramStatus
    {
        None = 0,
        Initilize = 1,
        Approve = 2,
        SendToFactory = 3,
        InProduction = 4,
        ProductionDone = 5
    }
    #endregion

    #region Enummeration : EnumTSType
    public enum EnumServiceScheduleStatus
    {
        None = 0,
        UpComming = 1,
        Pending = 2,
        Done = 3,
        Cancel = 4
    }
    #endregion

    #region Enummeration : EnumFinishYarnChallan
    public enum EnumFinishYarnChallan
    {
        [Description("Yet To Start")]
        Yet_To_Start = 0,
        [Description("Partial Challan")]
        Partial_Challan = 1,
        [Description("Challan Done")]
        Challan_Done = 2
    }
    #endregion

    #region Enummeration : EnumFinishFabricReceive
    public enum EnumFinishFabricReceive
    {
        [Description("Yet To Start")]
        Yet_To_Start = 0,
        [Description("Partial Receive")]
        Partial_Receive = 1,
        [Description("Receive Done")]
        Receive_Done = 2
    }
    #endregion

    #region Enummeration : EnumFabricProductionBatchState
    public enum EnumFabricBatchState
    {
        Initialize = 0,
        InFloor = 1,
        Warping = 2,
        warping_Finish = 3,
        Sizing = 4,
        Sizing_Finish = 5,
        DrawingIn = 6,
        DrawingIn_Finish = 7,
        Weaving = 8,
        Weaving_Finish = 9,
        In_QC = 10,
        QCDone = 11,
        InStore_Partial = 12,
        InStore = 13,
        Finish = 14,
        Cancel = 15,
        LeasingIn = 16,
        LeasingIn_Finish = 17
    }
    #endregion

    #region Enummeration : EnumProductionStatus
    public enum EnumProductionStatus
    {
        Initialize = 0,
        Run = 1,
        RunOut = 2,
        Hold = 3
    }
    #endregion

    #region Enummeration : EnumFabricBatchStatus
    public enum EnumFabricBatchStatus
    {
        Initialize = 0,
        Running = 1,
        Hold = 2,
        Finish = 3,
        
    }
    #endregion

    #region Enummeration : EnumFabricPlanStatus
    public enum EnumFabricPlanStatus
    {
        Initialize = 0,
        Planned = 1,
        Hold = 2,
        Finished = 3,
        Cancel = 4
    }
    #endregion


    #region Enummeration : EnumDateDisplayPart
    public enum EnumDateDisplayPart
    {
        Month = 0,
        Year = 1
    }
    #endregion

    #region Enummeration : EnumFAMethod
    public enum EnumFAMethod
    {
       None = 0,
       [Description("Straight Line Depreciation")]
       Straight_Line_Depreciation=1,
       [Description("Reducing Balance Depreciation")]
       Reducing_Balance_Depreciation=2,
       //[Description("Declining Method")]
       //Declining_Method=3
    }
    #endregion

    #region Enummeration : EnumLHRuleType
    public enum EnumLHRuleType
    {
        None = 0,
        [Description("Leave Limit")]
        Leave_Limit = 1,
        [Description("Minium Days")]
        Minium_Days = 2,
    }
    #endregion

    #region Enummeration : EnumLHRuleValueType
    public enum EnumLHRuleValueType
    {
        None = 0,
        [Description("Text")]
        Text = 1,
        [Description("Number")]
        Number = 2,
        [Description("Leave Days")]
        Leave_Days = 3,
        [Description("Notice Days")]
        NoticeDays = 4,
    }
    #endregion
    
    #region Enummeration : EnumFAEffectOn
    public enum EnumFAEffectOn
    {
        None = 0,
        GRN = 1,
        //[Description("Purchase Invoice")]
        //Purchase_Invoice = 2,

        //[Description("Declining Method")]
        //Declining_Method=3
    }
    #endregion

    #region Enummeration : EnumDEPCalculateOn
    public enum EnumDEPCalculateOn
    {
        None = 0,
        Monthly = 1,
        Quarterly = 2,
        HalfYearly = 3,
        Yearly = 4,
    }
    #endregion

    #region Enummeration : EnumFARegisterOn
    public enum EnumFARegisterOn
    {
        None = 0,
        [Description("Single Qty")]
        Single_Qty = 1,
        [Description("Item Wise")]
        Item_Wise = 2,
        //[Description("Declining Method")]
        //Declining_Method=3
    }
    #endregion

    #region Enummeration : EnumFADeptEffectFrom
    public enum EnumFADeptEffectFrom
    {
        None = 0,
        [Description("GRN Month")]
        GRN_Month = 1,
        [Description("After 1 Month")]
        After_1_Month=2,
        [Description("After 2 Month")]
        After_2_Month=3,
        [Description("After 3 Month")]
        After_3_Month=4,
        //[Description("Declining Method")]
        //Declining_Method=3
    }
    #endregion

    #region Enummeration : EnumDateFormat
    public enum EnumDateFormat
    {
        ShortFormat = 0,
        LongFormat = 1
    }
    #endregion
    #region Enummeration : EnumCustomDateFormat
    public enum EnumCustomDateFormat
    {
        None=0,
        [Description("dd MMM yyyy")]
        dd_MMM_yyyy = 1,
        [Description("MMM yyyy")]
        MMM_yyyy = 2,
        [Description("yyyy")]
        yyyy = 3
    }
    #endregion

    #region Enummeration : Voucher Record Type
    public enum EnumACMappingType
    {
        None = 0,
        Ledger = 1,
        SubLedger = 2
    }
    #endregion

    #region Enummeration : EnumAccountHeadTyupe
    public enum EnumAccountHeadType
    {
        None = 0,
        FixedAccountType = 1,
        DecidedAccountHead = 2,
        ReferenceAccountHead = 3
    }
    #endregion

    #region Enummeration : EnumAccountOperationType
    public enum EnumAccountOperationType
    {
        General = 0,
        BankAccount = 1,
        BankClearing = 2
    }
    #endregion

    #region Enummeration : EnumAccountType
    public enum EnumAccountType
    {
        None = 0,
        Component = 1,
        Segment = 2,
        Group = 3,
        SubGroup = 4,
        Ledger = 5
    }
    #endregion

    #region Enummeration : EnumFAAccountingEffect
    public enum EnumFAAccountingEffect
    {
        None = 0,
        Maintenance = 1,
        Renovation = 2,
        Disposal = 3
    }
    #endregion

    #region Enummeration : EnumFACodingPartType
    public enum EnumFACodingPartType
    {
        None = 0,
        [Description("Business Unit")]
        BusinessUnit = 1,
        Separator = 2,
        FixedText = 3,
        Location = 4,
        Year = 5,
        Numeric = 6,
        ProductShortName = 7
    }
    #endregion

    #region Enummeration : EnumFAAccountHeadType
    public enum EnumFAAccountHeadType
    {
        None = 0,
        [Description("Fixed Asset")]
        Fixed_Asset = 1,
        [Description("Accumulated Depriciation")]
        Accumulated_Depriciation = 2
    }
    #endregion

    #region Enummeration : EnumFNDyeingType
    public enum EnumFNDyeingType
    {
        None = 0,
        [Description("C.P.B Production")]
        CPB = 1,
        [Description("PAD STEAM PRODUCTION")]
        PAD = 2,
        [Description("THERMOSOL PRODUCTION")]
        THARMOSOL = 3

    }
    #endregion

    #region Enummeration : EnumRefType
    public enum EnumAttachRefType
    {
        None = 0,
        ProductionOrder = 1,
        DeliveryOrder = 2,
        DeliveryChallan = 3,
        UserSignature = 4,
        Contractor =5,
        FabricSalesContract = 6, //FabricSalesContract
        ExportPI = 7, //ExportPI
        BusinessUnit_Header = 10, //BusinessUnit
        BusinessUnit_Footer = 11, //BusinessUnit
        ImportPI=12,
        ImportLC = 13,
        ImportInvoice = 14,
        OrderRecapPackingPolicy = 15,
        BarCodeImage = 16,
        CommercialInvoice = 17,
        ReturnChallan = 18,
        Style = 19, //Style /TechnicalSheet
        ServiceOrder = 20,
        FabricDeliveryOrder = 21,
        LotBaseTest=22,
        PurchaseQuotation=23,
        ExportBill = 24,
        SampleInvoice = 25,
        DUDeliveryOrder=26
    }
    #endregion

    #region Enummeration : EnumAccountYearStatus
    public enum EnumAccountYearStatus
    {
        Initialize = 0,
        Running = 1,
        Freeze = 2,
        Closed = 3
    }
    #endregion

    #region Enummeration : EnumAccountYearStatus
    public enum EnumManualVoucherParentType
    {
        None = 0,
        PurchaseInvoice = 1,
        Payment = 2,
        SaleInvoice = 3
    }
    #endregion

    #region Enummeration : EnumBreakdownType
    public enum EnumBreakdownType
    {
        VoucherDetail = 0,
        CostCenter = 1,// BreakdownObjID will be CostCenterID, ExplanationName will be CostCenter Name,ExplanationCode will be CostCenterCode & ExplanationAmount will be User Entered Amount
        BillReference = 2, // BreakdownObjID will be BillID, ExplanationName will be BillNo & ExplanationAmount will be User Entered Amount
        ChequeReference = 3, // ExplanationName will be user entered text & ExplanationAmount will be User Entered Amount
        InventoryReference = 4, //BreakdownObjID will be ProductID, ExplanationName will be Product Name, ExplanationCode will be ProductCode, ExplanationAmount will be User Entered Amount & WorkingUnitID, Qty, UnitPrice  must be confirm   
        SubledgerBill = 5, //Only use in interface not database 
        SubledgerCheque = 6, //Only use in interface not database 
        OrderReference = 7, // BreakdownObjID will be VOrderID, ExplanationName will be OrderNo & ExplanationAmount will be User Entered Amount
        SLOrderReference = 8 //Only use in interface not database 
    }
    #endregion

    #region Enummeration : EnumCFTransactionCategory
    public enum EnumCFTransactionCategory
    {
        None =0,
        [Description("Operating Activities")]
        Operating_Activities = 1,
        [Description("Investing Activities")]
        Investing_Activities = 2,
        [Description("Financing Activities")]
        Financing_Activities = 3,
        [Description("Expenses")]
        Expenses = 4,
        [Description("Changes In CA & CL")]
        Changes_In_CA_AND_CL = 5,
        [Description("Depreciation")]
        Depreciation = 6,
        [Description("Opening Balance")]
        Opening_Balance = 7,
        [Description("Net Increase")]
        Net_Increase = 8,
        [Description("Closing Balance")]
        Closing_Balance = 9
    }
    #endregion

    #region Enummeration : EnumCFTransactionGroup
    public enum EnumCFTransactionGroup
    {
        None =0,
        //Duplicate Entry Not Allow
        //Fixed for Operating_Activities
        [Description("Cash Receipts")]
        Cash_Receipts = 1, // Fixed for Net_TrunOver_of_SCI
        [Description("Cash Paid")]
        Cash_Paid = 2,  // Fixed for COGS_of_SCI_CA_And_CL_Chnages
        [Description("Interest Paid")]
        Interest_Paid = 3, // Fixed for Financila_Cost_of_SCI
        [Description("Income Tax Paid")]
        Income_Tax_paid = 4, // Fixed for Income_Tax_of_SCI

        //Fixed for Investing_Activities
        [Description("Acquisition of Fixed Asset")]
        Acquisition_of_Fixed_Asset = 5, //Fixed_Asset_SFP_Changes_Less_Depreciation. //Choice a Asset Subgroup Head
        [Description("Fixed Doposit")]
        Fixed_Doposit = 6,//Fixed_Deposit_SFP_Changes  // Choice a Asset Subgroup Head
        [Description("Acquisition of Intengible Asstes")]
        Acquisition_of_Intengible_Asstes = 7,//Fixed for Intengible_Asstes_SFP_Changes_Less_Depreciation // Choice a Asset Subgroup Head
        [Description("Capital WIP")]
        Capital_WIP = 8, //Fixed for Investment_SFP_Changes // Choice a Asset Subgroup Head

        //Fixed for Financing_Activities
        [Description("Payment of Lease Loan")]
        Payment_of_Lease_Loan = 9, //Fixed for None_Current_Loan_SFP_Changes // Choice a Libility Subgroup Head
        [Description("Payment of Term Loan")]
        Payment_of_Term_Loan = 10, //Fixed for Current_Loan_SFP_Changes // Choice a Libility Subgroup Head
        [Description("Payment of Dividend")]
        Payment_of_Dividend = 11, //Fixed for Dividend_SFP_Changes // Choice a Libility Subgroup Head

        //Fixed for Expenses
        [Description("Cost of Sales")]
        Cost_of_Sales = 12, // Fixed for COGS_of_SCI
        [Description("Administrative Cost")]
        Administrative_Cost = 13, // Fixed for Administrative_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption
        [Description("Selling Cost")]
        Selling_Cost = 14, // Fixed for Selling_Cost_of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption

        //Fixed for Changes_In_CA_AND_CL
        [Description("Current Asset Chnages")]
        Current_Asset_Chnages = 15, // Fixed for Current_Asset_Chnages_Of_SFP //Choice Multiple Asset Subgroup Head with Different Caption
        [Description("Current Libility Chnages")]
        Current_Libility_Chnages = 16, // Fixed for Current_Libility_Chnages_Of_SFP //Choice Multiple Libility Subgroup Head with Different Caption

        //Fixed for Depreciation      
        [Description("Fixed Asset Depreciation Cost")]
        Fixed_Asset_Depreciation_Cost = 17, // Fixed for Fixed_Asset_Depreciation_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption
        [Description("Intengible Assets Depreciation Cost")]
        Intengible_Assets_Depreciation_Cost = 18, // Fixed for Intengible_Assets_Depreciation_Cost_Of_SCI //Choice Multiple Expenditure Subgroup Head with Same Caption

        //Opening_Balance
        [Description("Opening Balance")]
        Opening_Balance = 19, // Fixed for Opening_Balance_of_FPS // Choice Multiple Asset Subgroup Head with no Caption

        //Fixed for Operating_Activities
        [Description("Other Income")]
        Other_Income = 20 // Fixed for Other_Income_of_SCI
    }
    #endregion

    #region Enummeration : EnumCFDataType
    public enum EnumCFDataType
    {
        None=0,
        [Description("Net Trunover of SCI")]
        Net_Trunover_of_SCI = 1, //Duplicate Not Allow
        [Description("COGS Of Comprehensive Income Current Asset & Current Liability Chnages")]
        COGS_of_SCI_CA_And_CL_Chnages = 2, //Duplicate Not Allow        
        [Description("Financila Cost Of Comprehensive Income")]
        Financila_Cost_of_SCI = 3, //Duplicate Not Allow
        [Description("Income Tax Of Comprehensive Income")]
        Income_Tax_of_SCI = 4,//Duplicate Not Allow
        [Description("Fixed Asset Financial Position Changes Less Depreciation")]
        Fixed_Asset_SFP_Changes_Less_Depreciation = 5,//Duplicate Not Allow
        [Description("Fixed Deposit Financial Position Changes")]
        Fixed_Deposit_SFP_Changes = 6, //Duplicate Not Allow
        [Description("Intengible Asstes Financial Position Changes Less Depreciation")]
        Intengible_Asstes_SFP_Changes_Less_Depreciation = 7,//Duplicate Not Allow
        [Description("Investment Financial Position Changes")]
        Investment_SFP_Changes = 8, // Duplicate Not Allow
        [Description("None Current Loan Financial Position Changes")]
        None_Current_Loan_SFP_Changes = 9,//Duplicate Not Allow
        [Description("Current Loan Financial Position Changes")]
        Current_Loan_SFP_Changes = 10,//Duplicate Not Allow
        [Description("Dividend Financial Position Changes")]
        Dividend_SFP_Changes = 11, //Duplicate Not Allow        
        [Description("COGS Of Comprehensive Income")]
        COGS_of_SCI = 12, //Duplicate Not Allow
        [Description("Administrative Cost Of Comprehensive Income")]
        Administrative_Cost_Of_SCI = 13,//Multiple Entry Allow
        [Description("Selling Cost of Comprehensive Income")]
        Selling_Cost_of_SCI = 14,//Multiple Entry Allow
        [Description("Current Asset Chnages Of Financial Position")]
        Current_Asset_Chnages_Of_SFP = 15, //Multiple Entry Allow
        [Description("Current Libility Chnages Of Financial Position")]
        Current_Libility_Chnages_Of_SFP = 16, //Multiple Entry Allow
        [Description("Fixed Asset Depreciation Cost Of Comprehensive Income")]
        Fixed_Asset_Depreciation_Cost_Of_SCI = 17, //Multiple Entry Allow
        [Description("Intengible Asset Depreciation Cost Of Comprehensive Income")]
        Intengible_Assets_Depreciation_Cost_Of_SCI = 18, //Multiple Entry Allow
        [Description("Opening Balance of Financial Position")]
        Opening_Balance_of_SFP = 19, //Multiple Entry Allow
        [Description("Other Income of Comprehensive Income")]
        Other_Income_of_SCI = 20 // Fixed for Other_Income_of_SCI
    }
    #endregion

    #region Enummeration : EnumCashFlowHeadType
    public enum EnumCashFlowHeadType //Use Only Direct Method
    {
        None = 0,
        [Description("Effected Accounts")]
        Effected_Accounts = 1,
        [Description("Operating Opening Caption")]
        Operating_Opening_Caption = 2,
        [Description("Operating Activities")]
        Operating_Activities = 3,
        [Description("Operating Closing Caption")]
        Operating_Closing_Caption = 4,
        [Description("Investing Opening Caption")]
        Investing_Opening_Caption = 5,
        [Description("Investing Activities")]
        Investing_Activities = 6,
        [Description("Investing Closing Caption")]
        Investing_Closing_Caption = 7,
        [Description("Financing Opening Caption")]
        Financing_Opening_Caption = 8,
        [Description("Financing Activities")]
        Financing_Activities = 9,
        [Description("Financing Closing Caption")]
        Financing_Closing_Caption = 10,
        [Description("Net Cash Flow Caption")]
        Net_Cash_Flow_Caption = 11,
        [Description("Begaining Cash Caption")]
        Begaining_Cash_Caption = 12,
        [Description("Closing Cash Caption")]
        Closing_Cash_Caption = 13
    }
    #endregion

    #region Enummeration : EnumDebitCredit
    public enum EnumDebitCredit
    {
        None = 0,
        Debit = 1,
        Credit = 2
    }
    #endregion

    #region Enummeration : EnumChequeStatus
    public enum EnumChequeStatus
    {
        Initiate = 0,
        Activate = 1,
        Issued = 2,
        Authorized = 3,
        EditMode = 4,
        Sealed = 5,
        DeliverToParty = 6,
        Encash = 7,
        Dishonor = 8,
        Return = 9,
        Cancel = 10
    }
    #endregion

    #region Enummeration : EnumOperationSetting
    public enum EnumChequeType
    {
        None = 0,
        Received = 1,
        Payment = 2,
        Cash = 3,
        Transfer = 4
    }
    #endregion

    #region Enummeration : EnumFAStatus
    public enum EnumFAStatus
    {
        None = 0,
        Initialized = 1,
        Approved = 2,
        ReqForRevice = 3
    }
    #endregion
    
    #region Enummeration : EnumConfig_AddressType
    public enum EnumConfig_AddressType
    {
        District = 1,
        Thana = 2,
        PostOffice = 3,
        Village = 4
    }
    #endregion
    
    #region Enummeration : Comprensive Income Statement Setup
    public enum EnumCISSetup
    {
        None = 0,
        [Description("Gross Turnover")]
        Gross_Turnover = 1,//Income
        [Description("Inventory Head")]
        Inventory_Head = 2, //Assest
        [Description("Purchase Material")]
        Purchase_Material = 3, //Expenditure or Assets
        [Description("Overhead Cost")]
        Overhead_Cost = 4,//Expenditure
        [Description("Working Progress")]
        Working_Process = 5,//Asset
        [Description("Finish Goods")]
        Finish_Goods = 6,//Asset
        [Description("Operating Expenses")]
        Operating_Expenses = 7,//Expenditure
        [Description("Other Income")]
        Other_Income = 8,//Income
        [Description("Income Tax")]
        Income_Tax = 9,//Expenditure
        [Description("Depreciation")]
        Depreciation = 10, //Expenditure
        COGSCaption = 11 //Only for Calculation Not Use in Interface
    }
    #endregion

    #region Enummeration : EnumComponentType
    public enum EnumComponentType
    {
        None = 0,
        Asset = 2,
        Liability = 3,
        OwnersEquity = 4,
        Income = 5,
        Expenditure = 6
    }
    #endregion

    #region Enummeration : EnumDataField
    public enum EnumDataField   //For Voucher Signature
    {
        None = 0,
        AuthorizedByName = 1,
        PreparedByName = 2
    }
    #endregion

    #region Enummeration : EnumDataGenerateType
    public enum EnumDataGenerateType
    {
        None = 0,
        ManualData = 1,
        AutomatedData = 2,
        FixedData = 3
    }
    #endregion

    #region Enummeration : EnumFabricQtyAllowType
    public enum EnumFabricQtyAllowType
    {
        None = 0,
        WarpnWeft = 1,
        Dyeing = 2,
        SolidWhite = 3,
        Grindle = 4,
    }
    #endregion

    #region Enummeration : EnumEquityTransactionType
    public enum EnumEquityTransactionType
    {
        None = 0,
        OpeningBalance = 1,
        OperationalProfit = 2,
        OtherIncome = 3,
        TransactionWithShareholder = 4,
        AdjustmentForDepreciation = 5,
        AdjustmentForDeferredTax = 6,
        ClosingBalance = 7
    }
    #endregion

    #region Enummeration : EnumConfigureValueType
    public enum EnumConfigureValueType
    {
        None = 0,
        BoolValue = 1,
        StringValue = 2
    }
    #endregion

    #region Enummeration : EnumDataSetupTyupe
    public enum EnumDataSetupType
    {
        None = 0,
        VoucherDateSetup = 1,
        CurrencySetup = 2,
        ConversionRateSetup = 3,
        NarrationSetup = 4,
        ReferenceNoteSetup = 5,
        AccountHeadSetup = 6,
        VoucherDetailAmountSetup = 7,
        VoucherDetailNarrationSetup = 8,
        ChequeReferenceAmountSetup = 9,
        ChequeReferenceDescriptinSetup = 10,
        ChequeReferenceDateSetup = 11,
        CostCenterSetup = 12,
        CostCenterAmountSetup = 13,
        CostCenterDescriptionSetup = 14,
        CostCenterDateSetup = 15,
        VoucherBillSetup = 16,
        VoucherBillAmountSetup = 17,
        VoucherBillDescriptionSetup = 18,
        VoucherBillDateSetup = 19,
        InventoryWorkingUnitSetup = 20,
        InventoryProductSetup = 21,
        InventoryQtySetup = 22,
        InventoryUnitSetup = 23,
        InventoryUnitPriceSetup = 24,
        InventoryDescriptionSetup = 25,
        InventoryDateSetup = 26,
        CostCenterCategorySetup = 27,
        BillDateSetup = 28,
        BillDueDateSetup = 29,
        AccountNameSetup = 30,
        ChequeSetup = 31,
        OrderSetup = 32,
        OrderAmountSetup = 33,
        OrderRemarkSetup = 34,
        OrderDateSetup = 35,
        BusinessUnitSetup = 36,
        OrderNoSetup = 37
    }
    #endregion

    #region Enummeration : EnumDataReferenceType
    public enum EnumDataReferenceType
    {
        None = 0,
        IntegrationDetail = 1,  //DataReferenceID will be IntegrationSetupDetailID
        DebitCreditSetup = 2 // DataReferenceID will be DebitCreditSetupID
    }
    #endregion

    #region Enummeration : EnumConfigureType
    public enum EnumConfigureType
    {
        None = 0,
        #region From 1 to 10 -> General Ledger
        [Description("Account Head Wise Narration")]
        GLAccHeadWiseNarration = 1,
        [Description("Voucher Narration")]
        GLVoucherNarration = 2,
        [Description("Cost Center/Subledger Ref")]
        GLCC = 3,
        [Description("Bill Ref")]
        GLBT = 4,
        [Description("Inventory Ref")]
        GLIR = 5,
        [Description("Text Ref")]
        GLVC = 6,
        [Description("Order Ref")]
        GLOR = 7,
        #endregion

        #region From 11 to 20 -> General Journal
        [Description("Account Head Wise Narration")]
        GJAccHeadWiseNarration = 11,
        [Description("Voucher Narration")]
        GJVoucherNarration = 12,
        [Description("Cost Center/Subledger Ref")]
        GJCC = 13,
        [Description("Bill Ref")]
        GJBT = 14,
        [Description("Inventory Ref")]
        GJIR = 15,
        [Description("Text Ref")]
        GJVC = 16,
        [Description("Order Ref")]
        GJOR = 17
        #endregion
    }
    #endregion

    #region Enummeration : EnumDisplayMode
    public enum EnumDisplayMode
    {
        None = 0,
        TransactionView = 1,
        DateView = 2,
        WeeklyView = 3,
        MonthlyView = 4
    }
    #endregion

    #region Enummeration : EnumDueType
    public enum EnumDueType
    {
        AllDueBill = 1,
        OverDueBill = 2

    }
    #endregion

    #region Enummeration : EnumFNMachineType
    public enum EnumFNMachineType
    {
        None = 0,
        Machine = 1,
        Batcher = 2,
        Trolly = 3
    }
    #endregion

    #region Enummeration : EnumPTUUnit2Ref
    public enum EnumPTUUnit2Ref
    {
        Production_Order_Detail = 1,
        ProductionSheet = 2,
        Delivery_Order= 3,//ref id will be Delivery Order DetailID
        Delivery_Challan = 4,
        Grace = 5,
        QC=6, //Actual finish
        Reject = 7,
        PTU_Disburse = 8,//Send
        PTU_Receive = 9,
        Receive_In_Ready_Stock = 10,//Avilable stock to Ready Stock 
        Receive_In_Avilabe_Stock = 11,//Receive in avilable from ready stock
        Return=12, //Return Challan detail
        Subcontract_Issue=13, //Sub Contract Send
        Subcontract_Receive=14, //Sub Contract Receive
        Adjustment = 15 //Adjustment 
    }
    #endregion

    #region Enummeration : EnumProductionSheetStatus
    public enum EnumProductionSheetStatus
    {
        Initialize = 0,
        Approved = 1,
        Production_In_Progress = 2,
        Production_Done = 3
    }
    #endregion

    #region Enummeration : EnumRawMaterialStatus
    public enum EnumRawMaterialStatus
    {
        YetToRawMaterialOut = 0,
        RawMaterial_Out_In_Process = 1,
        Consumption_Done = 2
    }
    #endregion
       
    #region Enummeration : EnumQCStatus
    public enum EnumQCStatus
    {
        [Description("Yet To QC")]
        Yet_To_QC = 0,
        [Description("In QC")]
        In_QC = 1,
        [Description("QC Done")]
        QC_Done = 2,
        [Description("QC Failed")]
        QC_Failed = 3,
        [Description("Reproduction")]
        Reproduction = 4
    }
    #endregion
    //
    #region Enummeration : EnumKnitDyeingPTURefType
    public enum EnumKnitDyeingPTURefType
    {
        None =0,
        [Description("Knit Dyeing Program Issue")]
        KnitDyeing_Program_Issue = 1,//Ref Obj ID will be KnitDyeingProgramDetailID
        [Description("Yarn Booking")]
        Yarn_Booking = 2,//Ref Obj ID will be KnitDyeingYarnBookingID
        [Description("Knitting Issue")]
        Knitting_Issue = 3,//Ref Obj ID will be KnittingYarnChallanDetailID
        [Description("Gray Fabric Received")]
        Gray_Fabric_Received = 4,//Ref Obj ID will be KnittingFabricReceiveDetailID      
        [Description("Knitting Process Loss")]
        Knitting_Process_Loss = 5,//Ref Obj ID will be KnittingFabricReceiveDetailID
        [Description("Gray Fabric Issue For Dyeing")]
        Gray_Fabric_Issue_For_Dyeing = 6,//Ref Obj ID will be KnitDyeingBatchGrayChallanID
    }
    #endregion

    #region Enummeration : EnumFinancialReportType
    public enum EnumFinancialReportType
    {
        None = 0,
        GeneralLedger = 1,
        TrialBalance = 2,
        BalanceSheet = 3,
        IncomeStatement = 4
    }
    #endregion

    #region Enummeration : EnumNumberMethod
    public enum EnumNumberMethod
    {
        None = 0,
        Manual = 1,
        Automatic = 2
    }
    #endregion

    #region Enummeration : EnumRatioFormat
    public enum EnumRatioFormat
    {
        None = 0,
        Ratio = 1,
        Percentage = 2
    }
    #endregion

    #region Enummeration : EnumReceivedChequeStatus
    public enum EnumReceivedChequeStatus
    {
        Initiate = 0,
        //Activate = 1,
        //Issued = 2,
        Authorized = 1,
        //EditMode = 4,
        //Sealed = 5,
        ReceivedFromParty = 2,
        Encash = 3,
        Dishonor = 4,
        Return = 5,
        Cancel = 6
    }
    #endregion

    #region Enummeration : EnumSessionType
    public enum EnumSessionType
    {
        None = 0,
        YearEnd = 1,
        HalfYearEnd = 2,
        QuarterEnd = 3,
        MonthEnd = 4,
        WeekEnd = 5,
        DayEnd = 6
    }
    #endregion
    #region Enummeration : EnumBudgetType
    public enum EnumBudgetType
    {
        None = 0,
        Yearly = 1,        
        Monthly = 2        
    }
    #endregion
    #region Enummeration : EnumBudgetStatus
    public enum EnumBudgetStatus
    {
        None = 0,
        Initialized = 1,
        ReqForApprove = 2,
        Approved = 3,
        ReqForRevise = 4,
    }
    #endregion

    #region Enummeration : EnumTaxEffect
    public enum EnumTaxEffect
    {
        No = 0,
        Yes = 1
    }
    #endregion

    #region Enummeration : Transaction Type
    public enum EnumTransactionType
    {
        None = 0,
        Cheque = 1,
        Payorder = 2,
        Cash = 3,
        BankTransfer = 4

        //None = 0,
        //Initialized = 1,
        //Interest = 2,
        //LTRAdj = 3,
        //Payment = 4,
        //Charge = 5,
        //Paid = 6,
    }
    #endregion

    #region Enumeration : VoucherBatchStatus
    public enum EnumVoucherBatchStatus
    {
        None = 0, BatchOpen = 1, BatchClose = 2, RequestForApprove = 3, ApprovalInProgress = 4, Approved = 5
    }
    #endregion

    #region Enummeration : EnumVoucherBillTrType
    public enum EnumVoucherBillTrType
    {
        None = 0,
        NewRef = 1,
        AgstRef = 2,
        Advance = 3,
        OnAccount = 4
    }
    #endregion

    #region Enummeration : EnumVoucherCategory
    public enum EnumVoucherCategory
    {
        None = 0,
        Contra = 1,
        CreditNote = 2,
        DebitNote = 3,
        DeliveryNote = 4,
        Journal = 5,
        Memorandum = 6,
        Payment = 7,
        Purchase = 8,
        PurchaseOrder = 9,
        Receipt = 10,
        ReceiptNote = 11,
        RejectionsIn = 12,
        ReversingJournal = 13,
        Sales = 14,
        SalesOrder = 15,
        StockJournal = 16
    }
    #endregion

    #region Enummeration : EnumRestartPeriod
    public enum EnumRestartPeriod
    {
        None = 0,
        Monthly = 1,
        Yearly = 2
    }
    #endregion

    #region Enummeration : EnumDyesChemicalViewType
    public enum EnumDyesChemicalViewType
    {
        None = 0,
        [Description("Dyes/Chemical")]
        DyesChemical = 1,
        Lot = 2,
        [Description("Dyes/Chemical and Lot")]
        DyesChemical_and_Lot = 3
    }
    #endregion

    #region Enummeration : EnumVoucherCodeType
    public enum EnumVoucherCodeType
    {
        None = 0,
        Text = 1,
        DateVariation = 2,
        Numeric = 3,
        Separator = 4
    }
    #endregion

    #region Enummeration : EnumVoucherExplanationType
    public enum EnumVoucherExplanationType
    {
        VoucherDetail = 0, //Voucher detail object here Explanation data not need
        CostCenter = 1,// ExplanationID will be CostCenterID, ExplanationName will be CostCenter Name,ExplanationCode will be CostCenterCode & ExplanationAmount will be User Entered Amount
        BillReference = 2, // ExplanationID will be BillID, ExplanationName will be BillNo & ExplanationAmount will be User Entered Amount
        VoucherReference = 3, // ExplanationName will be user entered text & ExplanationAmount will be User Entered Amount
        InventoryReference = 4 //ExplanationID will be ProductID, ExplanationName will be Product Name, ExplanationCode will be ProductCode, ExplanationAmount will be User Entered Amount & WorkingUnitID, Qty, UnitPrice  must be confirm
    }
    #endregion

    #region Enummeration : EnumVoucherOperationType
    public enum EnumVoucherOperationType
    {
        None = 0,
        FreshVoucher = 1,
        ProfitLossAccountVoucher = 2,
        DividendVoucher = 3,
        RetainedEarningVoucher = 4
    }
    #endregion

    #region Enummeration : Voucher Record Type
    public enum EnumVoucherRecordType //Only Use for Voucher Print
    {
        None = 0,
        BusinessUnit = 1,
        Ledger = 2,
        CostCenter = 3
    }
    #endregion

    #region Enummeration : EnumEquityCategory
    public enum EnumEquityCategory
    {
        None = 0,
        Share_Capital = 1,
        Share_Premium = 2,
        Excess_of_Issue_Price_Over_Face_Value_of_GDRs = 3,
        Capital_Reserve_on_Merger = 4,
        Revaluation_Surplus = 5,
        Fair_Value_Gain_on_Investment = 6,
        Retained_Earnings = 7
    }
    #endregion

    #region Enummeration : EnumSetupType
    public enum EnumSetupType
    {
        None = 0,
        Import = 1,
        Export = 2
    }
    #endregion

    #region Enummeration : EnumVoucherSetup
    public enum EnumVoucherSetup
    {
        None = 0,
        [Description("Payment Cheque Issue")]
        Payment_Cheque_Issue = 1,
        [Description("Bank Reconciliation")]
        Bank_Reconciliation = 2,
        [Description("Export Sales Voucher(Delivery)")]
        Export_Sales_Voucher_Delivery = 3,
        [Description("Export Sales Voucher(Invoice)")]
        Export_Sales_Voucher_Invoice = 4,
        [Description("Import Purchase Voucher Invoice")]
        Import_Purchase_Voucher_Invoice = 5,
        [Description("Import Invoice Inventory Voucher(GRN-InventoryItem)")]
        Import_Invoice_Inventory_Voucher_GRN_InventoryItem = 6,
        [Description("Goods In Transit")]
        GoodsInTransit = 7,
        [Description("Import Payment Settlement By Loan")]
        Import_Payment_Settlement_By_Loan = 8,
        [Description("PF Subs/Cont")]
        PF_Subs_Cont = 9,
        [Description("PF Loan Deduction")]
        PF_Loan_Deduction = 10,
        [Description("Import Payment Settlement By A/C TRSFR")]
        Import_Payment_Settlement_By_AccountTransfer = 11,
        [Description("Export Bill Encashment")]
        Export_Bill_Encashment = 12,
        [Description("Cash/Sample Sales Voucher Delivery")]
        Sample_Sales_Voucher_Delivery = 13,
        [Description("Cash/Sample Sales Voucher Invoice")]
        Sample_Sales_Voucher_Invoice = 14,
        [Description("Cash/Sample Payment Encashment Cash")]
        Sample_Payment_Encashment_Cash = 15,
        [Description("Cash/Sample Payment Encashment Bank")]
        Sample_Payment_Encashment_Bank = 16,
        [Description("Cash/Sample Invoice Adjustment Voucher")]
        Sample_Invoice_Adjustment_Voucher = 17,
        [Description("Local Purchase Voucher")]
        Local_Purchase_Voucher = 18,
        [Description("Local Invoice Inventory Voucher(GRN)")]
        Local_Invoice_Inventory_Voucher_GRN = 19,
        [Description("Commission Payable Voucher")]
        Commission_Payable_Voucher = 20,
        [Description("Commission Adjustment With Sample")]
        Commission_Adjustment_With_Sample = 21,
        [Description("Commission Payment Settlement")]
        Commission_Payment_Settlement = 22,
        [Description("WorkOrder Inventory Voucher(GRN)")]
        WorkOrder_Inventory_Voucher_GRN = 24,
        [Description("ImportPI Inventory Voucher(GRN)")]
        ImportPI_Inventory_Voucher_GRN = 25,
        [Description("B2B LC Open Voucher")]
        B2B_LC_Open_Voucher = 26,
        [Description("COGS Voucher")]
        COGS_Voucher = 27,
        [Description("Sample Adjustment Note Voucher")]
        Sample_Adjustment_Note = 28,
        [Description("Sample Adjustment Voucher")]
        Sample_Adjustment_Voucher = 29,
        [Description("Tally Voucher With Bill")]
        Tally_Voucher_With_Bill = 30,
        [Description("Manual Delivery")]
        Manual_Delivery = 31,
        [Description("Local Purchase Voucher With WO")]
        Local_Purchase_Voucher_WO = 32,
        [Description("B2B LC Open Voucher With Bill")]
        B2B_LC_Open_Voucher_With_Bill = 33,
        [Description("Import Invoice Inventory Voucher(GRN-FixedAsset & Consumable)")]
        Import_Invoice_Inventory_Voucher_GRN_FixedAsset_Consumable = 34,
        [Description("Finish Goods QC Voucher")]
        Finish_Goods_QC_Voucher = 35,
        [Description("Raw Material Consumption Voucher")]
        Raw_Material_Consumption_Voucher = 37,
        [Description("Purchase Order Inventory Voucher(GRN)")]
        Purchase_Order_Inventory_Voucher_GRN = 38,
        [Description("Export Sales Voucher(Return Challan)")]
        Export_Sales_Voucher_ReturnChallan = 39,
        [Description("Cash/Sample Sales Voucher(Return Challan)")]
        Sample_Sales_Voucher_ReturnChallan = 40,
        [Description("Commission Payable Maturity Receive")]
        Commission_Payable_Maturity_Receive = 42,
        [Description("Import LC Landed Cost Voucher")]
        Import_LC_Landed_Cost_Voucher = 43,
        [Description("Inventory Adjustment Voucher(Addition)")]
        Inventory_Adjustment_Voucher_Addition = 44,
        [Description("Inventory Adjustment Voucher(Deduction)")]
        Inventory_Adjustment_Voucher_Deduction = 45,
        [Description("Consumable Goods Consumption Voucher")]
        Consumable_Goods_Consumption_Voucher = 46,
        [Description("Sub Contact Voucher(Issue)")]
        Sub_Contact_Voucher_Issue = 47,
        [Description("Sub Contact Voucher(Receive)")]
        Sub_Contact_Voucher_Receive = 48,
        [Description("External Material Transfer Voucher(Issue)")]
        External_Material_Transfer_Voucher_Issue = 49,
        [Description("External Material Transfer Voucher(Receive)")]
        External_Material_Transfer_Voucher_Receive = 50,
        [Description("Payroll Voucher(Factory)")]
        Payroll_Voucher_Factory = 51,
        [Description("Commission Payable Payment Receive")]
        Commission_Payable_Payment_Receive = 52,
        [Description("Commission Paid Cash")]
        Commission_Paid_Cash = 53,
        [Description("Commission Paid Bank")]
        Commission_Paid_Bank = 54,
        [Description("Commission Paid By Sample")]
        Commission_Paid_By_Sample = 55,
        [Description("Local Purchase Voucher(FixedAsset)")]
        Local_Purchase_Voucher_FixedAsset = 56,
        [Description("Local Invoice Inventory Voucher(GRN-FixedAsset)")]
        Local_Invoice_Inventory_Voucher_GRN_FixedAsset = 57,
        [Description("Payroll Voucher(HO)")]
        Payroll_Voucher_HO = 58,
        [Description("Import LC Landed Cost Voucher(Advance)")]
        Import_LC_Landed_Cost_Voucher_Advance = 59,
        [Description("Service Invoice Voucher")]
        Service_Invoice_Voucher = 60,
        [Description("Spare Parts Consumption Voucher")]
        Spare_Parts_Consumption_Voucher = 61,
        [Description("Cash/Sample Sales Voucher Claim Delivery")]
        Sample_Sales_Voucher_DU_Claim_Delivery = 62,
        [Description("Export Sales Voucher Claim Delivery")]
        Export_Sales_Voucher_DU_Claim_Delivery = 63,
        [Description("Bonus Voucher(Factory)")]
        Bonus_Voucher_Factory = 64,
        [Description("Vehicle Return Challan Voucher")]
        Vehicle_Return_Challan_Voucher = 65,
        [Description("Loan Payable Voucher")]
        Loan_Payable_Voucher = 66,
        [Description("Loan Settlement Voucher")]
        Loan_Settlement_Voucher = 67,
        [Description("Fixed Asset Depreciation Voucher")]
        Fixed_Asset_Depreciation_Voucher = 68,
        [Description("Commission Payable Non LC")]
        Commission_NonLC_Payable_Receive = 69,
        [Description("Commission( Non LC) Paid Cash")]
        Commission_NonLC_Paid_Cash = 70,
        [Description("Commission( Non LC) Paid Bank")]
        Commission_NonLC_Paid_Bank = 71,
        [Description("Loan Interest Voucher")]
        Loan_Interest_Voucher = 72,
        [Description("Garments Export Voucher")]
        Garments_Export_Voucher = 73,
        [Description("Garments Bill Submission Voucher")]
        Garments_Bill_Submission_Voucher = 74,
        [Description("Garments Bill Purchase Voucher")]
        Garments_Bill_Purchase_Voucher = 75,
        [Description("Garments Bill Encashment Voucher")]
        Garments_Bill_Encashment_Voucher = 76,
        [Description("Trading Sales Delivery Challan Voucher")]
        Trading_Sales_Delivery_Challan_Voucher = 77,
        [Description("Trading Sales Invoice Voucher")]
        Trading_Sales_Invoice_Voucher = 78,
        [Description("Trading Sales Cash Collection Voucher")]
        Trading_Sales_Cash_Collection_Voucher = 79,
        [Description("Trading Sales Bank Collection Voucher")]
        Trading_Sales_Bank_Collection_Voucher = 80,
        [Description("Commission Payable LC Full")]
        Commission_Payable_LC = 81,
    }
    #endregion

    #region Enummeration : EnumVoucherBillReferenceType
    public enum EnumVoucherBillReferenceType
    {
        None = 0,
        [Description("Export Invoice")]
        Export_Invoice = 1,
        [Description("Local Invoice")]//Local Sale Invoice
        Local_Invoice = 2,
        [Description("Import Invoice")]
        Import_Invoice = 3,
        [Description("Local Purchase Invoice")]
        Local_Purchase_Invoice = 4,
        [Description("Term Loan")]
        Term_Loan = 5, //for Import Payment
        [Description("Sales Commission Payable")]
        SalesCommissionPayable = 6, //It Use For Maturity Received
        [Description("Service Invoice")]
        ServiceInvoice = 7, //ReferenceObjID will be  ServiceInvoiceID
        [Description("Sub Contract")]
        SubContract = 8, //ReferenceObjID id will be SubContractID
        [Description("Transfer Requisition")]
        TransferRequisition = 9, //ReferenceObjID id will be TransferRequisitionSlipID
        [Description("Vehicle Return Challan")]
        Vehicle_Return_Challan = 10,
        [Description("CommercialInvoice")]
        CommercialInvoice = 11, //ReferenceObjID id will be CommercialInvoiceID
        [Description("Trading Sale Invoice")]
        TradingSaleInvoice = 12, //ReferenceObjID id will be TradingSaleInvoiceID        
    }
    #endregion

    #region Enummeration : EnumReferenceType
    public enum EnumReferenceType
    {
        None = 0,
        Customer = 1,
        Vendor = 2,
        BankBranch = 3,
        GenerealBankAccount = 4, 
        Vendor_Foreign = 5,
        Employee = 6,
        EDF_PAD_LTR = 7, //Import Payment ReferenceID Will be ImportPaymentID
        ForeignBankAccount = 8, //Use in ACCost Center ReferenceObjectID will be Bank AccoutnID       
        EDF_PAD_PC_LTR_UPass_LongTerm_Loan = 9, //EDF/PAD/PC/LTR/UPass/LongTerm Loan ReferenceID Will BE LoanID
          
        LC = 20, // Import LC ReferenceObjectID will be LCID
        Invoice = 21, // Import Commercial Invoice ReferenceObjectID will be InvoiceID,
        BL = 22,//Import BL ReferenceID Will be BLID            
        LTR = 23,//Import Payment ReferenceID Will be ImportPaymentID (LTR)
        PAD = 24,//Import Payment ReferenceID Will be ImportPaymentID (PAD)
        LDBC = 25,//Export LDBC Reference ID will be LDBCID(LDBC)
        LDBP = 26, //Export LDBC Reference ID will be LDBCID(LDBP)
        DeliveryChallan = 27, //Export LDBC Reference ID will be LDBCID(LDBP)

        CategoryLC = 50,//Bank Wise Cost Center Category Reference ID Will be BankID(CategoryLTR)
        CategoryInvoice = 51,//Bank Wise Cost Center Category Reference ID Will be BankID(CategoryPAD)
        CategoryBL = 52,//Bank Wise Cost Center Category Reference ID Will be BankID(CategoryPAD)
        CategoryLTR = 53,//Bank Wise Cost Center Category Reference ID Will be BankID(CategoryLTR)
        CategoryPAD = 54,//Bank Wise Cost Center Category Reference ID Will be BankID(CategoryPAD)
        CategoryLDBC = 55,//Bank Wise Cost Center Category Reference ID Will be BankID(CategoryLDBC)
        CategoryLDBP = 536, //Bank Wise Cost Center Category Reference ID Will be BankID(CategoryLDBP)
        CategoryDeliveryChallan = 57, //Deliv
        ConcernPerson_Buyer = 58, //Buyer  Concern Person_Buyer
        SalaryHead = 59, //Use in ACCostCenter ReferenceObjectID will be SalaryHeadID
    }
    #endregion

    #region Enummeration : EnumLCReportLevel
    public enum EnumLCReportLevel
    {
        LCLevel = 0,
        [Description("===LC version level===")]
        LCVersionLevel = 1,
        PILevel = 2,
        ProductLevel = 3
    }
    #endregion

    #region EnumProductionStepType : ProductionStepType
    public enum EnumProductionStepType
    {
        Regular = 0,
        Molding = 1,
        Blowing=2,
        Cutting = 3,
        Printing = 4,
        Sewing = 5,
        QCPass = 6
    }
    #endregion

    #region EnumDocFor : DocFor
    public enum EnumDocFor
    {
       None =0,
        Common = 1,
        PI = 2,//Export PI
        SalesContract = 3,//Sales Contract
        Bill=4,//Bill
        MasterLC=5//Master LC

    }
    #endregion

    #region Enumeration : EnumFinishType
    public enum EnumFinishType
    {
        //added by fahim0abir on 21/06/15 for sales and marketing module
        None = 0,
        Regular = 1,
        RegularSoft = 2,
        SilkySoft = 3,
        Mercerize = 4,
        OneSideBrush = 5,
        BothSideBrush = 6,
        Peach = 7,
        NonPeach = 8,
        PaperTouch = 9,
        DiamondPeach = 10,
        CarbonPeach = 11,
        CarbonBrush = 12,
        TwoSideLightBrush = 13,
        WrinkleFree = 14,
        OneSideMicroPeach = 15,
        PFDPeach = 16,
        EasyCare = 17,
        PeachSideBrush = 18,
        MicroSandPeach = 19,
        Crispy = 20,
        MildBrush = 21,
        HeavyBrushed = 22,
        LightPeach = 23,
        FaceSidePeachAndBackSideBrush = 24,
        HardFinish = 25,
        MicroPeach = 26,
        CarbonBrushBothSide = 27
    }
    #endregion

    #region Enumeration : EnumAccountHolderType
    public enum EnumAccountHolderType
    {
        // If EnumAccountHolderType is Contractor User Table EmployeeID Will be ContactpersonnelID 
        // If EnumAccountHolderType is EWYDL User Table EmployeeID Will be EmployeeID
        None = 0, Own = 1, Contractor = 2
    }
    #endregion

    #region Enumeration : EnumFinancialUserType
    public enum EnumFinancialUserType
    {        
        None = 0,
        [Description("Group Accounts")]
        GroupAccounts = 1,
        [Description("Individual Accounts")]
        IndividualAccounts = 2,
        [Description("Admin User")]
        Admin_User = 3,
        [Description("Normal User")]
        Normal_User = 4
    }
    #endregion

    #region Enummeration : EnumOrderSheetStatus
    public enum EnumOrderSheetStatus
    {
        Intialize = 0,
        Request_For_Approval = 1,
        Approved = 2,
        InProduction = 3,
        Production_Done = 4,
        Request_For_Revise = 5,
        Cancel = 6
    }
    #endregion

    #region Enummeration : EnumTradingSaleInvoiceStatus
    public enum EnumTradingSaleInvoiceStatus
    {
        Intialize = 0,
        Wait_For_Approval = 1,
        Approved = 2,
        Request_For_Revise = 3,
        Cancel = 4
    }
    #endregion
    #region Enummeration : EnumWorkOrderStatus
    public enum EnumWorkOrderStatus
    {
        Intialize = 0,
        Request_For_Approval = 1,
        Approved = 2,
        InReceived = 3,
        Received_Done = 4,
        Request_For_Revise = 5,
        Cancel = 6,
        Bill_Done = 7
    }
    #endregion

    #region Enummeration : EnumProductionOrderStatus
    public enum EnumProductionOrderStatus
    {
        Intialize = 0,
        Request_For_Approval = 1,
        Approved = 2,
        Req_For_Dir_App = 3,
        DIR_Approved = 4,
        InProduction = 5,
        Production_In_Progress = 6,
        Production_Done = 7,
        Request_For_Revise = 8,
        Cancel = 9
    }
    #endregion

    #region Enummeration :EnumOrderSheetActionType
    public enum EnumOrderSheetActionType
    {
        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        InProduction = 5,
        Production_Done = 6,
        Request_For_Revise = 7,
        Accept_Revise = 8,
        Cancel = 9
    }
    #endregion

    #region Enummeration :EnumWorkOrderActionType
    public enum EnumWorkOrderActionType
    {
        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        InReceived = 5,
        Received_Done = 6,
        Request_For_Revise = 7,
        Accept_Revise = 8,
        Cancel = 9
    }
    #endregion

    #region Enummeration :ProductionOrderActionType
    public enum EnumProductionOrderActionType
    {
        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        Req_For_DIR_App = 5,
        Dir_App = 6,
        Unod_DIR_App = 7,
        InProduction = 8,
        Production_Process = 9,
        Production_Done = 10,
        Request_Revise = 11,
        Cancel = 12
    }
    #endregion

    #region Enummeration :EnumPIType
    public enum EnumPIType
    {
        Open = 0,
        [Description("PO")]
        OrderSheet = 1,
        MasterPI = 2,
        SalesContract = 3
    }
    #endregion
    #region Enummeration : RecruitmentEvent
    public enum EnumRecruitmentEvent
    {
        None = 0,
        Joining = 1,
        Confirmation = 2,
        PFMember = 3,
    }
    #endregion

    #region Enummeration : PaymentCycle
    public enum EnumPaymentCycle
    {
        None = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }
    #endregion

    #region Enummeration : EmployeeNature
    public enum EnumEmployeeNature
    {
        None = 0,
        WhiteColor = 1,
        BlueColor = 2
    }
    #endregion

    #region Enummeration : OverTimeON
    public enum EnumOverTimeON
    {
        None = 0,
        Gross = 1,
        Basic = 2
    }
    #endregion

    #region Enummeration : DyeingSolutionType
    public enum EnumDyeingSolutionType
    {
        None = 0,
        Process = 1,
        Solution = 2
    }
    #endregion

    #region Enummeration : SalaryCalculationOn
    public enum EnumSalaryCalculationOn
    {
        None = 0,
        Gross = 1,
        SalaryItem = 2,
        Fixed = 3
    }
    #endregion

    #region Enummeration : EnumBankAccountType
    public enum EnumBankAccountType
    {
        None = 0,
        GeneralAccount  = 1,
        ForeignAccount = 2
   } 
    #endregion

    #region Enummeration : EnumLeaveApplication
    public enum EnumLeaveApplication
    {
        None = 0,
        LeaveApplication = 1,
        LeaveOfAbsence = 2
    }
    #endregion

    #region Enummeration : EnumLeaveType
    public enum EnumLeaveType
    {
        None = 0,
        Full = 1,
        Half = 2,
        Short = 3
    }
    #endregion

    #region Enummeration : EnumLeaveStatus
    public enum EnumLeaveStatus
    {
        None = 0,
        Applied = 1,
        Recommended = 2,
        Approved = 3,
        Canceled = 4
    }
    #endregion

    #region Enummeration : ContractorType
    public enum EnumContractorType
    {
        None = 0,
        Supplier = 1,
        Buyer = 2,
        Factory = 3,
        Bank = 4,
        Agent = 5,
        MotherBuyer = 6,
        InternalContractor = 7,
        Insurance = 8,
        CnFAgent = 9,
    }
    #endregion

    #region Enummeration : ResourceType
    public enum EnumResourcesType
    {
        None = 0,
        Mold = 1,
        Machine = 2,
        Utility = 3

    }
    #endregion

    #region Enummeration : EnumProductType
    public enum EnumProductType
    {
        None = 0,
        InventoryItem = 1,
        FixedAsset = 2,
        Consumable = 3,
        FinishGoods = 4 // it use for define Raw Material & Finish Goods in Transfer Requisition
    }
    #endregion

    #region Enummeration : PaymentInstruction
    public enum EnumPaymentInstruction
    {
        None = 0, BL = 1, Negotiation = 2, Acceptence = 3, Delivery = 4, LCOpen = 5, Shipment = 6, GoodsReceived = 7,
        [Description("Documents Received date at LC Issuing Bank’s Counter")]
        DocumentsReceiveddate = 8
    }
    #endregion

    #region Enummeration : EnumEmployeeDesignationType
    public enum EnumEmployeeDesignationType
    {
        None = 0,
        Admin = 1,
        Management = 2,
        Lab = 3,
        Operational = 4,
        Service = 5,
        Helper = 6,
        Supervisor = 7,
        Incharge = 8,
        MarketPerson = 9,
        WarpingOperator = 15,
        SizeingOperator = 17,
        DrowningOperator = 19,
        LoomOperator = 21,
        Director = 22,
        Merchandiser = 23,
        Commercial = 24,
        ManagingDirector = 25,
        Chairman = 26,
        QC_Person = 27
    }
    #endregion

    #region Enummeration : PriorityLebel
    public enum EnumPriorityLevel
    {
        None = 0, Low = 1, Medium = 2, High = 3
    }
    #endregion

    #region Enummeration : Knit,Ply,Yarn
    public enum EnumKnitPlyYarn
    {
        None = 0, Knit = 1, Ply = 2, Yarn = 3
    }
    #endregion

    #region Enummeration : NumericOrder
    public enum EnumNumericOrder
    {
        Start = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5,
        Sixth = 6,
        Seventh = 7,
        Eighth = 8,
        Ninth = 9,
        Tenth = 10,
        Eleventh = 11,
        Twelveth = 12,
        Thirteenth = 13,
        Fourteenth = 14,
        Fifteenth = 15,
        Sixteenth = 16,
        Seventeenth = 17,
        Eightennth = 18,
        Nineteenth = 19,
        Twentyth = 20,
        TwentyFirst = 21,
        TwentySecond = 22,
        TwentyThird = 23,
        TwentyFourth = 24,
        TwentyFifth = 25,
        TwentySixth = 26,
        TwentySeventh = 27,
        TwentyEighth = 28,
        TwentyNineth = 29,
        Thirty = 30,
        ThirtyFirst = 31,
        ThirtySecond = 32,
        ThirtyThreeth = 33
    }
    #endregion

    #region Enummeration : InOutType
    public enum EnumInOutType
    {
        None = 100,
        Receive = 101, //In
        Disburse = 102// out
    }
    #endregion


    #region Enummeration : TriggerParentsType
    public enum EnumTriggerParentsType
    {
        None = 0,
        [Description("Adjustment")]
        AdjustmentDetail = 101,
        [Description("Delivery Challan")]
        DeliveryChallanDetail = 102,
        [Description("GRN Detail")] // It is used In Lot & Transaction table TriggrParentID Will be GRNDetailID
        GRNDetailDetail = 103,
        [Description("Requisition")] // It is used In Lot & Transaction table TriggrParentID Will be RequisitionDetailID
        RequisitionDetail = 104,
        [Description("Return Challan")]// It is used In Lot & Transaction table TriggrParentID Will be ReturnChallanDetailID (sales return)
        ReturnChallanDetail = 105,
        [Description("Dyeing Card")]// It is used In Transaction table TriggrParentID Will be RouteSheetID
        RouteSheet = 106,//not 0
        [Description("Route Sheet Dye Chemical")] // It is used In Transaction table TriggrParentID Will be RouteSheetDetailID
        RouteSheetDetail = 107,
        [Description("Transfer Requisition")] // It is used In Lot & Transaction table TriggrParentID Will be TransferRequisitionDetaiID
        TransferRequisitionDetail = 108,// not 0        
        [Description("Spinning Production Execution")] // Only insert from spinning finish goods received & IT ParentID will be SUProductionExecutionDetailID
        SUProductionMixture = 113,
        [Description("FabricQCDetail")]
        FabricQCDetail = 114,
        [Description("Production Sheet")] // It is used In Transaction & TriggrParentID Will be ProductionSheetID
        ProductionSheet = 115,
        [Description("Production Recipe")] // It is used In Transaction & TriggrParentID Will be RMRequisitionMaterialID 
        ProductionRecipe = 116,
        [Description("Finish Goods QC")] // It is used In Transaction & TriggrParentID Will be QCID
        FinishGoodsQC = 117,
        [Description("Consumption Requsition")] // It is used In Transaction & TriggrParentID Will be ConsumptionRequsitionDetailID
        ConsumptionRequsition = 118,
        [Description("FNProduction")]
        _FNProduction = 119,
        [Description("FNChallan")]
        _FNChallan = 120,
        [Description("Re-Cycle Process")]
        ReCycleProcess = 121,
        [Description("Service Dyeing")]
        DUProGuideLineDetail = 122,// It is used In Transaction & TriggrParentID Will be DUProGuideLineDetailID
        [Description("Purchase Return")] // It is used In Transaction & TriggrParentID Will be PurchaseReturnDetailID
        PurchaseReturn = 123,
        [Description("Lot Mixing")]
        LotMixing = 124,
        [Description("Style To Style Transfer")]// It is used In Transaction & TriggrParentID Will be S2SLotTransferID
        S2SLotTransfer = 125,
        [Description("FabricProduction")]
        _FabricProduction = 111,
        [Description("Fabric Receive For Finishing")]
        _FabricReceiveForFinishing = 126, // atml it is 117 It is used In Transaction & TriggrParentID Will be FabricRequisitionDetailID (for fabric req. weaving to finishing)
        [Description("FN Requisition Detail")]
        _FNRequisitionDetail = 127,
        [Description("Vehicle Return")]
        VehicleReturnChallan=128,
        [Description("RawMaterial Return")] // Floor/Extra Raw Material Return in  Stock;  It is used In ITransaction & TriggrParentID Will be RMRequisitionMaterialID 
        RawMaterial_Return = 129,
        [Description("GU QC")]
        _GUQC = 130,// use Gurments QC parent id will be GUQCDetail ID
        [Description("Fabric Sales Contract")]
        _FabricSalesContract_GrayReceive = 131,// FNOrderFabricReceive, FNOrderFabricReceiveID
        _FabricSalesContract_GrayIssue = 132,
        _Shipment = 133,
        FabricDeliveryChallan =134,
        _YarnReceiveForFabricExecution = 135,
        FabricExecutionOrderYarnReceive = 136,// parent id will be FEOYRID , it can use for disburse and receive
        KnittingYarnChallan = 137, //Parent ID WIll BE KnittingYarnChallanDetailID
        KnittingFabricReceive = 138, //Parent ID WIll BE KnittingFabricReceiveDetailID
        KnittingYarnReturn = 139, //Parent ID WIll BE KnittingYarnReturnDetailID
        FabricReturnChallan = 140, //Parent ID WIll BE FabricReturnChallanDetailID
        SoftWindingProduction = 141, //Parent ID WIll BE  SoftWindingID
        FNProductionConsumption = 142, //Parent ID WIll BE  FNProductionConsumption its for Finishing Production (Dy-Chemical out)
        [Description("Parts Requisition")] // It is used In Transaction & TriggrParentID Will be PartsRequisitionDetailID
        PartsRequisition = 143,

        [Description("Spare Parts Challan")] // It is used In Transaction & TriggrParentID Will be SparePartsChallanDetailID
        SpareParts_Challan = 144,
        [Description("Delivery Challan")] // It is used In Transaction & TriggrParentID Will be TradingDeliveryChallanDetailID
        Trading_Delivery_Challan = 145,
        [Description("Finished Goods Dumping")] // It is used In Transaction & TriggrParentID Will be Finished Good Dumping For RecycleProcess
        Finished_Goods_Dumping = 146,
        [Description("Sample Request")] // It is used In Transaction & TriggrParentID Will be SampleRequest
        Sample_Request = 147,        
        [Description("Twisting")] // It is used In Transaction & TriggrParentID Will be TwistingDetail
        Twisting = 148,
        [Description("Dyeing Batch Gray Fabric Issue")]
        Dyeing_Batch_Gray_Fabric_Issue= 149, // It is used In Transaction & TriggrParentID Will be KnitDyeingBatchGrayChallanID
        [Description("Transfer Requisition Slip Detail")]
        TransferRequisitionSlipDetail=150,
        [Description("WU Sub Contract Yarn Challan")]
        WUSubContractYarnChallan = 151,
        [Description("WU Sub Contract Fabric Receive")]
        WUSubContractFabricReceive = 152
    }
    #endregion

    #region Enummeration : OrderType
    public enum EnumOrderType
    {
        None = 0,
        LabDipOrder = 1,
        SampleOrder = 2,
        [Description("Bulk Order")]
        BulkOrder = 3,
        [Description("Dyeing Only")]
        DyeingOnly = 4,
        ClaimOrder = 5,
        Sampling = 6, // RnD Order
        SaleOrder = 7, // Without PI Bulk Order
        LoanOrder = 8, // Without PI, Non Dyeing  Order
        ReConing  = 9, // , Non Dyeing  Order
        TwistOrder =10, //, Non Dyeing  Order
        SampleOrder_Two = 11, // For Out or Inside
        SaleOrder_Two = 12, // For Out or Inside
    }
    #endregion

    #region Enummeration : BatchStatus
    public enum EnumBatchStatus
    {
        None = 0, Run = 1, Out = 2
    }
    #endregion
    #region Enummeration : DyeingStepType
    public enum EnumForecastLayout
    {
        None=0,
        [Description("Dyeing Order Issue")]
        Dyeing_Order_Issue = 1,
        [Description("PI Issue LC And DO Not Receive")]
        PI_Issue_LC_And_DO_Not_Receive = 2,
        [Description("L/C Received But DO Not Issue")]
        PI_Issue_LC_Received_But_DO_Not_Issue = 3,
        [Description("Dyeing Order Issue Approval")]
        Dyeing_Order_Issue_TBA = 4,
    }
    public enum EnumDyeingStepType
    {
        None = 0,
        Knitting_CK = 1,
        Knitting_CS = 2,
        Dyeing = 3,
        [Description("Winding")]
        Winding = 4,
        Dryer = 5,
        [Description("Dryer Winding")]
        Dryer_Winding = 6,
        [Description("Twisting")]
        Twisting = 7,
        [Description("Hydro Service")]
        HydroService = 8
    }
    #endregion

    #region Enummeration : OperationUnitType
    public enum EnumOperationUnitType
    {
        None = 0,
        RawStore = 1,
        [Description("Soft Winding")]
        SoftWinding = 2,
        [Description("Twisting")]
        Twisting = 3,
        [Description("Hard Winding")]
        HardWending = 4
    }
    #endregion

    #region Enummeration : CompareOperator
    public enum EnumCompareOperator
    {
        [Description("--Select One--")]
        None = 0,
        [Description("Equal to")]
        EqualTo = 1,
        [Description("Not equal to")]
        NotEqualTo = 2,
        GreaterThan = 3,
        SmallerThan = 4,
        Between = 5,
        NotBetween = 6
    }
    #endregion

    #region Enummeration : EnumKnittingOrderType
    public enum EnumKnittingOrderType
    {
        [Description("--Select Order Type--")]
        None = 0,
        [Description("Sample Development")]
        SampleDevelopment = 1,
        [Description("Sample Order")]
        SampleOrder = 2,
        [Description("Bulk Order")]
        BulkOrder = 3
      
    }
    #endregion
    #region Enummeration : CompareOperatorTwo
    public enum EnumCompareOperatorTwo
    {
        [Description("--Select One--")]
        None = 0,
        [Description("Equal to")]
        EqualTo = 1,
        Between = 5
    }
    #endregion

    #region Enummeration : DOState
    /// <summary>
    /// a Job could have following states in its lifetime. Any one state at any time.
    /// </summary>
    public enum EnumDOState
    {
        None = 0,
        //Initialized = 1,
        //InProduction = 2,
        //Finished = 3,
        //Cancelled = 4

        Initialized = 1,
        Running = 2,
        Booking = 3,
        Hold_Production = 4, //2
        Hold_Delivery = 5, //2
        Delivered = 6, //3
        Closed = 7, //3
        Cancelled = 8,  ///4
                        

    }
    #endregion
    #region Enummeration : DOState
    /// <summary>
    /// a Job could have following states in its lifetime. Any one state at any time.
    /// </summary>
    /// 
    public enum EnumPOState
    {
        None = 0,
        Initialized = 1,
        Running = 2,
        Hold = 3, //2
        Holdmkt = 4, //2
        Delivered = 5, //3
        Closed = 6, //3
        Cancel = 7
    }
    #endregion

    #region Enummeration : PTUState
    /// <summary>
    /// a Job could have following states in its lifetime. Any one state at any time.
    /// </summary>
    public enum EnumPTUState
    {
        None = 0,
        Initialized = 1,
        InProduction = 2,
        Paused = 3,
        Finished = 4,
        Cancelled = 5

    }
    #endregion

    #region Enummeration : ProductionTracingUnit
    public enum EnumProductionTracingUnit
    {
        None = 0,
        OrderIssue = 1, // Parent: DyeingOrderDetail
        ProductionPipeline = 5, //Parent: RouteSheet
        ProductionLossGain = 7, //in QC; Parent:RouteSheet
       // ProductionPipelineFinishGain = 8, // in QC; Parent:RouteSheet
        ProductionPipelineFinishReDyeing = 9, //in QC; Parent:RouteSheet
        ProductionPipelineFinishFreshDyed = 10,// in QC; Parent:RouteSheet
        ProductionFinishIssue = 11, //Finishing Store Receive; Parent: RouteSheet (lot)
       // ProductionFinishCancel = 12, //Finishing Store Receive; Parent: RouteSheet

        DeliveryByChallan = 13, // ChallanDetail (lot)
       // DeliveryCancelByReturnChallan = 14,  // ChallanDetail (lot)
        //SampleOrderIssue = 15, // SampleOrderDetail
        //SampleOrderCancel = 16, // SampleOrderDetail
        PTUDistruibutionGive = 17, // RouteSheet //Source PTU Destination
        PTUDistruibutionTake = 18,  // RouteSheet //Destination PTU

        SourcePTUDistribution = 117, //Source PTU Destination (lot)
        DestinationPTUDistribution = 118, //Destination PTU (lot)

        ManualUpdateDyeingOrder = 21, //User
        ManualUpdateJobOrder = 22,
        ManualUpdateProductionGrace = 23,
        ManualUpdateProductionInProgress = 24,
        ManualUpdateProductionFinished = 25,
        ManualUpdateProductionLossGain = 26,
        ManualUpdateDeliveryByChallan = 27,
        ManualUpdateRate = 28,
        UpdatePTUProductionStatus = 50 //User update PTU Production status; Current value contain previous status
    }
    #endregion

    #region Enummeration : RSStates
    /// <summary>
    /// a Route Sheet could have following states in its lifetime. Only any one state at any time.
    /// </summary>
    public enum EnumRSState
    {
        None = 0,
        Initialized = 1,
        InLab = 2,
        InFloor = 3,
        //Approved = 4, //Raw
         [Description("Yarn Out")]
        YarnOut = 4, //Raw
        DyesChemicalOut = 5,
        LoadedInDyeMachine = 6,//RAw
        UnloadedFromDyeMachine = 7,//RAw
        LoadedInHydro = 8,//Raw
        UnloadedFromHydro = 9,//Raw
        LoadedInDrier = 10,//Raw
        UnLoadedFromDrier = 11,//Raw
        InPackageing = 12,//Dye
         [Description("QC Done")]
        QC_Done = 13,
        InSubFinishingstore_Partially = 14,
        [Description("In HW/Pro.Floor")]
        InHW_Sub_Store = 15,
        InDelivery = 16,
        LotReturn = 17,//dye
        Finished = 18,// Raw
    }
    #endregion

    #region Enummeration : RSSubStates
    public enum EnumRSSubStates
    {
        None = 0,
        OK = 1,
        [Description("Not OK")]
        Not_Ok = 2,
        [Description("Wait For Approval")]
        WaitForApproval = 3,
        [Description("Req. For Cancel")]
        Req_For_Cancel = 4,
        [Description("OK-After Re-Check")]
        OK_After_ReCheck = 5,
        [Description("OK-After Re-Process")]
        OK_After_ReProcess = 6,
    }
    #endregion

    #region Enummeration : EnumShade
    public enum EnumShade
    {
        NA = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        F = 6,
        G = 7,
        H = 8,
        I = 9,
        J = 10,
        K = 11,
        L = 12,
        M = 13,
        N = 14,
        O = 15,
        DTM = 16,
        AVL = 17
    }
    #endregion

    #region Enummeration : EnumLabDipType
    public enum EnumLabDipType
    {
        None = 0,
        Normal = 1,
        DTM = 2,
        [Description("Available")]
        AVL = 3,
        [Description("TBC")]
        TBA = 4
    }
    #endregion

    #region Enummeration : EnumLabDipTwistType
    public enum EnumLDTwistType
    {
        Generale = 0,
        Twisting = 1,
        Injecting = 2
    }
    #endregion

    #region Enummeration : EnumLabDipChallanStatus
    public enum EnumLabDipChallanStatus
    {
        None = 0,
        [Description("Forward To HO")]
        Forward_To_HO = 1,
        [Description("Forward To Buyer")]
        Forward_To_Buyer = 2
    }
    #endregion
    #region Enummeration : EnumExcellColumn
    public enum EnumExcellColumn
    {
        A = 1, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }
    #endregion

    #region Enummeration : EnumPrintFormatType
    public enum EnumPrintFormatType
    {
        None = 0,
        [Description("Normal Format")]
        NormalFormat = 1,
        [Description("Pad Format")]
        PadFormat = 2,
        [Description("Image Format")]
        ImageFormat = 3
    }
    #endregion

    #region Enummeration : EnumLCBillEvent
    public enum EnumLCBillEvent
    {
        [Description("BOE In Hand")]
        BOEinHand = 0,
        [Description("BOE In Customer Hand")]
        BOEInCustomerHand = 1,
        BuyerAcceptedBill = 2,
        NegoTransit = 3,
        NegotiatedBill = 4,
        [Description("Bank Accepted Bill")]
        BankAcceptedBill = 5,
        FDDInHand = 6,
        Discounted = 7,
        Encashment = 8,
        BillClosed = 9,
        BillRealized = 10,
        BillCancel = 11,
        ReqForDiscounted = 12
    }
    #endregion

    #region Enummeration : EnumInvoiceEvent
    public enum EnumInvoiceEvent
    {
        None = 0,
        Initialize = 1,
        [Description("Shipment Done")]
        Shipment_Done = 2,
        DocReceive_By_Bank = 3,
        DocIn_Hand = 4,
        [Description("Doc In CnF")]
        Doc_In_CnF = 5,
        [Description("Position On Outer")]
        Position_Outer = 6,
        [Description("Position On JT")]
        Position_JT = 7,
        [Description("Noting Done")]
        Noting_Done = 8,     //6
        [Description("Assesment Done")]
        Assesment_Done = 9, //7
        [Description("Examin Done")]
        Examin_Done=10, //8
        [Description("DO Received From Shipping Line")]
        DO_Received_From_Shippingline=11,  //9
        [Description("Goods In Transit")]
        Goods_In_Transit=12,    //10
        [Description("Stock In Partial")]
        Stock_In_Partial = 13,    //11 
        [Description("Stock In")]
        Stock_In = 14,    //11 
         [Description("Cancel")]
        Cancel_Invoice = 15  //12

    }
    #endregion

    #region Enummeration : RouteLocation
    public enum EnumRouteLocation
    {
        //RoutePoint
        None = 0,
        ShippingLine = 1,
        PortOfLoading = 2,
        DestinationPort = 3,
        PlaceOfIssue = 4,
        OtherLocation = 5
    }
    #endregion

    #region Enummeration : EnumYarnType
    public enum EnumYarnType
    {
        None = 0,
        FreshDyedYarn = 1,
        Recycle = 2,
        Wastage = 3,
        DyedYarnOne = 4, // For Diffrent Shade
        DyedYarnTwo = 5, // For Diffrent Shade
        DyedYarnThree= 6, // For Diffrent Shade
    }
    #endregion

    #region Enummeration : EnumWYarnType
    public enum EnumWYarnType
    {
        None = 0,
        DyedYarn = 1,
        Gray = 2,
        Spinning = 3,
        LeftOver = 4,
    }
    #endregion

    #region Enummeration : EnumLabdipFormat
    public enum EnumLabdipFormat
    {
        None = 0,
        YarnForm = 1,
        [Description("20-gm")]
        TwentyGM = 2,
        SmallHanger = 3,
        BigHanger = 4,
        [Description("A Four")]
        A4 = 5,
        [Description("2''x2''")]
        TwoTwo = 6
    }
    #endregion

    #region Enummeration : ImportPIState
    public enum EnumImportPIState
    {
        Initialized = 0,
        Accepted = 1,
        Cancel = 2,
        [Description("Request For LC")]
        RequestForLC = 3,
        [Description("LC Confirm")]
        LC_Confirm = 4,
        [Description("Request For Revise")]
        Request_For_Revise = 5,
        [Description("Receive Revise Copy")]
        Receive_Revise = 6
    }
    #endregion

    #region Enummeration : ImportLCChargeType
    public enum EnumLCChargeType
    {
        [Description("Not Applicable")]
        Charge_Free = 0,
        [Description("Applicant")]
        Applicant = 1,
        [Description("Beneficiary")]
        Beneficiary = 2
    }
    #endregion

    #region Enummeration : GrossOrNetWeight
    public enum EnumGrossOrNetWeight
    {
        None = 0,
        GrossWeight = 1,
        NetWeight = 2
    }
    #endregion

    #region Enummeration : ExportLCStatus
    public enum EnumExportLCStatus
    {
        [Description("-")]
        None = 0,
        FreshLC = 1, //New  LC entry
        Approved = 2, // After Approved
        [Description("Request Send For Amendment")]
        RequestForAmendment = 3, // After Send Request
        [Description("Amendment LC")]
        AmendmentReceive = 4, // After Received Amendment Copy
        [Description("Outstanding LC")]
        OutstandingLC = 5,// Bill Create        
        Close = 6,
        Cancel = 7,
         [Description("Partial Cancel")]
        Partial_Cancel = 8
    }
    #endregion

    #region Enummeration : PaymentMethod
    public enum EnumPaymentMethod
    {
        None = 0,
        Cash = 1, Cheque = 2, DemandDraft = 3, PayOrder = 4, BankAdvice = 5, TT = 6, Credit=7
    }
    #endregion

    #region Enummeration : LCCurrentStatus
    public enum EnumLCCurrentStatus
    {
        None = 0,
        [Description("Request For LC")]
        ReqForLC = 1,
        [Description("L/C Open")]
        LC_Open = 2,
        [Description("In Supplier Hand")]
        InSupplierHand = 3,
        [Description("Shipment In Transit")]
        Shipment_InTransit = 4,
        [Description("Partial Cancel")]
        Partial_Cancel = 5,
        LCCancel = 6,
        [Description("Request For Amendment")]
        Req_ForAmendment = 7,
        AmendmentConfirm = 8,
        [Description("Request For Cancel")]
        Req_For_Cancel = 9,
        [Description("Request For Partial Cancel")]
        Req_For_Partial_Cancel = 10,
        Close = 11
    }
    #endregion

    #region Enummeration : LCLCPaymentType
    public enum EnumLCPaymentType
    {
        None = 0,
        [Description("REGULAR")]
        REGULAR = 1,
        [Description("E.D.F.")]
        EDF = 2,
        [Description("OFFSHORE")]
        OFFSHORE = 3,
         [Description("UPAS")]
        UPAS = 4,
        Deferred=5,
        CASH = 6
    }
    #endregion

    #region Enummeration : InvoiceBankStatus
    public enum EnumInvoiceBankStatus
    {
        None = 0,
        Document_In_HandBank = 1,
        WaitFor_Acceptance = 2,
        ABP = 3, //Import Bill Payable
        Payment_Request = 4,
        Payment_Done = 5

    }
    #endregion

    #region Enummeration : PIStatus
    public enum EnumPIStatus
    {
        Initialized = 0,
        RequestForApproved = 1,
        Approved = 2,
        PIIssue = 3,
        BindWithLC = 4,
        RequestForRevise = 5,
        Cancel = 6
    }
    #endregion

    #region Enummeration : EnumSubContractStatus
    public enum EnumSubContractStatus
    {
        Initialized = 0,
        RequestForApproved = 1,
        Approved = 2,
        InProduction = 3,
        ProductionDone = 4,        
        Cancel = 5
    }
    #endregion

    #region Enummeration : EnumTransferStatus
    public enum EnumTransferStatus
    {
        Initialized = 0,
        RequestForApproved = 1,
        Approved = 2,
        Disburse = 3,
        Received = 4,        
        Cancel = 5
    }
    #endregion

    #region Enummeration : EnumPTUType
    public enum EnumPTUType
    {
        None = 0,
        Regular = 1,
        Subcontract = 2
    }
    #endregion

    #region Enummeration : DOStatus
    public enum EnumDOStatus
    {
        Initialized = 0,
        RequestForApproved = 1,
        Approved = 2, 
        Request_For_MD_Approve = 3,
        MD_Approve = 4, 
        Send_To_Factory = 5, 
        Received = 6,
        Challan_Issue = 7,
        Challan_Deliverd = 8,
        Cancel = 9,
        Request_For_Revise = 10
    }
    #endregion
    #region Enummeration : DOStatus
    public enum EnumFabricDOStatus
    {
        Initialized = 0,
        RequestForApproved = 1,
        UndoRequest = 2,
        Approved = 3, //2
        UndoApprove = 4,
        PartiallyDelivered = 5, //3
        Delivered = 6, //4
        RequestForRevise = 7, //5
        Cancel = 8 //6
    }
    #endregion

    #region Enummeration : DO Action 
    public enum EnumDOActionType
    {
        RequestForApproved = 1,
        Undo_Request = 2,
        Approved = 3,
        Undo_Approved = 4,
        Request_For_MD_Approve = 5,
        MD_Approve = 6,
        Send_To_Factory = 7,
        Received = 8,
        Challan_Issue = 9,
        Challan_Deliverd = 10,
        Cancel = 11,
        Request_For_Revise = 12
    }
    #endregion

    #region Enummeration : EnumDOType
    public enum EnumDOType
    {
        None = 0,
        Export = 1,
        [Description("Local PI")]
        Local_PI = 2,
        Advance = 3,
        Compensation = 4,//SpecialInstruction
        //YarnTransfer = 5,
        //[Description("Sample Yarn Requisition")]
        Sample = 6,
        [Description("Special Instruction")]
        SpecialInstruction=7,
        [Description("Local Bill")]
        Local_Bill = 8,
        [Description("PP Sample")]
        PP_Sample = 9,
        [Description("Sample Yardage Request From Stock")]
        SampleYardageRequest = 12
    }
    #endregion

    #region Enummeration : EnumRequisitionType
    public enum EnumRequisitionType
    {
        None = 0,
        InternalTransfer = 1,
        ExternalTransfer = 2,// Sales/Loan Return 
        DeliveryToOtherStoreWithPTU = 3,
        OtherStoreToDeliveryWithPTU = 4
        //LoanReturn = 3
    }
    #endregion

    #region Enummeration : EnumRequisitionType
    public enum EnumTRSRefType
    {
        None = 0,
        Open = 1,
        SubContact = 2,        
        PTUTOAvilable = 3,
        AvilableTOPTU = 4
    }
    #endregion

    #region Enummeration : EnumCarrierType
    public enum EnumRequisitionStatus
    {
        None = 0,
        Initialized = 1,
        Authorized = 2,
        Disbursed = 3,
        Received = 4
    }
    #endregion

    #region Enummeration : EnumChequeRequisitionStatus
    public enum EnumChequeRequisitionStatus
    {
        None = 0,
        Initialized = 1,
        Approved = 2,
        Cancel = 3
    }
    #endregion

    #region Enummeration : EnumDBOperation
    public enum EnumDBOperation
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 3,
        Request = 4,
        Approval = 5,
        Revise = 6, // For Take version of obj
        Cancel = 7,
        Active = 11,
        InActive = 12,
        Start = 13,
        UnApproval = 14,
        Undo = 15,
        Receive = 16,
        Delivered = 17,
        Disburse = 18,
        Settlement = 19,
        Hold = 20,
        RollBack = 21,
        Backup = 22,
        SingleDelete = 23,
        Return = 24,
        Check = 25,
        Upload = 26
    }
    #endregion

    #region Enummeration : EnumMeasurementUnitType
    public enum EnumUniteType
    {
        None = 0,
        Count = 1,
        Weight = 2,
        Length = 3
    }
    #endregion

    #region Enummeration : OperationFunctionality
    public enum EnumOperationFunctionality
    {
        None = 0,

        //For Report View
        _ReportView = 101,

        // Revise

        _ReviseView = 301,

        /*******************Start MTR Related Event ************************/
        //Inventory Operation for RS  --By  Mamun 
        _RSInSubFinishingStore = 601,
        _QCDone = 602,
        _RSInFinishingStore = 603,  // Auto Int.Transfer
        _YarnManage = 604,
        _RSCancelled_Approved = 605,


        //Common edited by Md. Masud Iqbal
        _Add = 701,//insert
        _Edit = 702,
        _Delete = 703,
        _View = 704,
        _Request = 705,
        _Checked = 706,
        _Accept = 707,
        _Approve = 708,
        _Lock = 709,
        _Issue = 710,
        _Cancel = 711,
        _Receive = 712,
        _Disburse = 713,
        _Activity = 714,
        _Recommendation = 718,
        //Admin
        SuperUser = 714,
        Administration = 715,
        ViewAllRouteSheet = 716,
        ShowBuyerInfo = 717,

        /*******************Start Production Related Event ************************/

        //PTU
        PTUViewMktAdmin = 802,
        PTUViewPdAdmin = 803,
        PTUViewProduction = 804,
        PTUViewDelivery = 805,
        PTUViewBuyer = 806,
        PTUAddGrace = 807,
        PTUViewRates = 808,

        //Lab
        LabSupervisor = 809,
        LabSupervisorGlobal = 810,

        //ProductionAdmin
        RSCopy = 811,
        RSAdmin = 812,


        //ProductionFloor
        RSOperation = 813,
        FloorOperationDyeing = 814,


        //ProductionFinishing
        FloorOperationHydro = 815,
        FloorOperationDrier = 816,
        FloorOperationFinishing = 817,
        RSFinishing = 818,

        /*******************End Production Related Event ************************/

        /*******************Start Commercial Related Event ************************/
        //Commercial
        Commercial = 901,
        /*******************End Commercial Related Event ************************/


        LotExchangeByYarnCategory = 212

    }
    #endregion

    #region Enummeration : EnumModuleName
    public enum EnumModuleName
    {
        None = 0,
        Location = 1,
        WorkingUnit = 2,
        Bank = 3,
        AuthorizationRole = 4,
        Shelf = 5,
        BusinessUnit = 6,
        OrderSheet = 7,
        ColorCategory = 8,
        Menu = 9,
        SizeCategory = 10,
        Users = 11,
        ContainingProduct = 12,
        StorePermission = 13,
        ProductPermission = 14,
        DeliveryChallan = 15,
        TransferRequisitionSlip = 16,
        GRN = 17,
        RawMaterialOut = 18,
        FinishGoodsReceived = 19,
        ExportPI = 20,
        ImportPI = 21,
        ProductionOrder = 22,
        ProductionSchedule = 23,
        Recipe = 24,
        ProductionStep = 25,
        ProductionProcedureTemplate = 26,
        ExportPartyInfo = 27,
        PurchaseQuotation = 28,
        PurchaseRequisition = 29,
        NOA = 30,
        PurchaseOrder = 31,
        PaymentTerm = 32,
        PurchaseInvoice = 33,
        ImportLC = 34,
        ImportInvoice = 35,
        ACCostCenter = 36,
        OperationCategorySetup = 37,
        AccountsBookSetup = 38,
        LedgerGroupSetup = 39,
        LedgerBreakDown = 40,
        COA_ChartOfAccountCostCenter = 41,
        ChangesEquitySetup = 42,
        Account_Balance = 42,
        VoucherType = 43,
        AccountingRatioSetup = 44,
        CashFlowSetup = 45,
        AccountingSession = 46,
        VoucherHistory = 47,
        VoucherBatch = 48,
        ReceivedCheque = 49,
        ChequeBook = 50,
        IssueFigure = 51,
        AccountOpenning = 53,
        AccountHeadConfigure = 54,
        ChequeSetup = 55,
        Cheque = 56,
        Voucher = 57,
        BusinessLocation = 58,
        ImportFormat = 59,
        TrailBalance = 60,
        VProduct = 61,
        VoucherBill = 62,
        ChartsOfAccount = 63,
        ProductionSheet = 64,
        DeliveryOrder = 65,
        ProductionExecution = 66,
        QC = 67,
        UnitConversion = 68,
        Product = 69,
        StyleDepartment = 70,
        MaterialType = 71,
        GarmentsClass = 72,
        TechnicalSheet = 73,
        Knitting = 74,
        GarmentsType = 75,
        BuyerConcern = 76,
        Brand = 77,
        BusinessSession = 78,
        Lot = 79,
        Adjustment = 80,
        DiagramIdentification = 81,
        WorkOrder = 82,
        ConsumptionRequisition = 83,
        ConsumptionUnit = 84,
        Subcontract = 85,
        SaleOrder = 86,
        ChequeHistory = 87,
        ReceivedChequeHistory = 88,
        VoucherSignature = 89,
        NegativeLedger = 90,
        BUPermission = 91,
        PropertyValue = 92,
        ClientOperationSetting = 93,
        OrderRecap = 94,
        SampleType = 95,
        OrderRecapComment = 96,
        ReportLayout = 97,
        UserWiseContractorConfigure = 98,
        UserWiseStyleConfigure = 99,
        DevelopmentRecap = 100,
        DevelopmentType = 101,
        MasterLC = 102,
        B2BLCAllocation = 103,
        LCTransfer = 104,
        CommercialInvoice = 105,
        PackingList = 106,
        InspectionCertificate = 107,
        CostSheet = 108,
        CostSheetPackage = 109,
        DeliveryPlan = 110,
        RecycleProcess = 111,
        OrderStep = 112,
        TAPTemplate = 113,
        TAP = 114,
        TAPExecution = 115,
        HIASetup = 116,
        TAPDetail = 117,
        PackageTemplate = 118,
        FabricPO = 120,
        ReturnChallan = 121,
        ProformaInvoice = 122,
        ChequeRequisition = 123,
        BankReconciliation = 124,
        IntegrationSetup = 125,
        VOrder = 126,
        SignatureSetup = 127,
        CashFlowDmSetup = 128,
        Contractor = 129,
        ImportInvoiceChallan = 130,
        ImportPayment = 131,
        BankReconciliationOpenning = 132,
        ModelCategory = 133,
        Feature = 134,
        VehicleModel = 135,
        VehicleOrder = 136,
        KommFile = 137,
        VehicleChassis = 138,
        VehicleEngine = 139,
        VehicleColor = 140,
        RMRequisition = 141,
        CostCalculation = 142,
        SalesQuotation = 143,
        VehicleParts = 144,
        PNWiseAccountHead = 145,
        ConsumptionForecast = 146,
        RMClosingStock = 147,
        SaleInvoice = 148,
        DURequisition = 149,
        ProductCategory = 150,
        AccountEffect = 151,
        DUProGuideLine = 152,
        DUSoftWinding = 153,
        DUHardWinding = 154,
        GUProductionOrder = 155,
        ProductionExecutionPlan = 156,
        QCStep = 157,
        QCTemplate = 158,
        BankAccount = 159,
        ProductionTimeSetup = 160,
        CashFlow = 161,
        ServiceWork = 162,
        ServiceOrder = 163,
        VehicleRegistration = 164,
        VehicleType = 165,
        ServiceInvoice = 166,
        Twisting = 167,
        PartsRequisition = 168,
        PurchaseReturn = 169,
        OrderStepGroup = 170,
        RMConsumption = 171,
        RouteSheet = 172,
        BodyPart = 173,
        LotMixing = 174,
        SampleAdjustment = 175,
        VehicleChallan = 176,
        Job = 177,
        S2SLotTransfer = 178,
        VehicleReturnChallan = 179,
        WYRequisition = 180,
        Hydro = 181,
        Dryer = 182,
        RouteSheetCancel = 183,
        TransferRequisitionSlipDyed = 184, /// its Like 16 but interface different
        YarnRequisition = 185,
        DyeingLoadUnLoad = 186,
        FADepreciation = 187,
        SparePartsRequisition = 188,
        SparePartsChallan = 189,
        CommercialBS = 190,
        CommercialFDBP = 191,
        CommercialEncashment = 192,
        PAM = 193,
        StyleBudget = 194,
        //Fabric related enum : start from 200-249//
        Fabric = 200,
        FabricPattern = 201,
        FabricDeliveryChallan = 202,
        FabricSalesContract = 203,
        FNRequisition = 204,
        FabricBatch = 205,
        FNBatch = 206,
        FabricDeliveryOrder = 207,
        FabricBatchQC = 208,
        FabricSpecification = 209,
        FabricBatch_PRO_Sizing = 210,
        FabricReturnChallan = 211,
        FabricBatchProduction = 212,
        HangerSticker = 213,
        FabricSizingPlan = 214,
        FabricTransferNote = 215,
        FNProduction = 216,
        TradingSaleInvoice = 217,
        TradingDeliveryChallan = 218,
        TradingPayment = 219,
        TradingSaleReturn = 220,
        FabricLoomPlan = 221,
        LoanProductRate = 222,
        SourcingConfigHead = 223,
        KnitDyeingProgram = 224,
        KnitDyeingPTU = 225,
        EmployeeBatch = 226,
        KnitDyeingRecipe = 227,
        ArchiveData = 228,
        ExportFundAllocationHead = 229,
        //Dyeing related enum : start from 250-299//
        DULotDistribution = 250,
        DUDeliveryChallan = 251,
        Labdip = 252,
        FNLabdip = 253,
        FGQC = 254,
        DUPSchedule = 255,
        DyeingOrder = 256,
        LoanInstallment = 257,
        Loan = 258,
        DUReturnChallan = 259,
        DUClaimOrder = 260,
        PolyMeasurement = 261,
        DyeingPeriodConfig = 262,
        EmailConfig = 263,

        //HCM related enum : start from 700
        EmployeeSalaryStructure = 700,
        EmployeeSettlement = 701,
        LeaveApplication = 702,
        FinancialAdjustment = 703,
        Attendance = 704,
        EmployeeSalary = 705,
        ELProcess = 706,
        SalesCommission = 707,
        InventoryTracking = 708,
        SampleRequest = 709,
        BankReconcilationStatement = 710,
        RouteSheetDetail = 711,
        GUQC = 712,
        LeaveLedger = 713,
        Shipment = 714,
        PreInvoice = 715,
        MgtDBACSetup = 716,
        MgtDashBoardAccount = 717,
        KnittingOrder = 718,
        KnittingYarnChallan = 719,
        KnittingFabricReceive = 720,
        KnittingYarnReturn = 721,
        KnittingComposition = 722,
        FnQCTestGroup = 723,
        FNReProRequest = 724,
        DUDeliveryOrder = 725,
        CapitalResourceSpareParts = 726,
        FabricWarpPlan = 727,
        FabricRequisition = 728,
        FabricMachineType = 729,
        LoomExecution = 730,
        FabricClaim = 731,
        IDCardFormat = 732,
        Employee = 733,
        KnitDyeingBatch = 734,
        MktProjectionReport = 735,
        FabricChemicalPlan = 736,
        TimeCard = 737,
        CompensatoryLeave = 738,
        ExportBill = 739,
        WUSubContract = 740,       
        FabricBatchLoom=741,
        FabricAvailableStock=742,
        WUSubContractYarnChallan = 743,
        WUSubContractFabricReceive = 744
    }
    #endregion

    #region Enummeration : EnumReportModule
    public enum EnumReportModule
    {
        None = 0,
        [Description("Voucher Preview")]
        VoucherPreview = 1,
        [Description("Purchase Order Preview")]
        PurchaseOrderPreview = 2,
        [Description("Purchase Requisition Preview")]
        PurchaseRequisitionPreview = 3,
        [Description("Purchase Invoice Preview")]
        PurchaseInvoicePreview = 4,
        [Description("Work Order Preview")]
        WorkOrderPreview = 5,
        [Description("GRN Preview")]
        GRNPreview = 6,
        [Description("Adjustment Requisition Slip Preview")]
        AdjustmentRequisitionSlipPreview = 7,
        [Description("Service Invoice Preview")]
        ServiceInvoicePreview = 8,
        [Description("Purchase Return")]
        PurchaseReturnPreview = 9,
        [Description("Transfer Requisition Slip Preview")]
        TransferRequisitionSlipPreview = 10,
        [Description("Knitting Yarn Challan Preview")]
        KnittingYarnChallanPreview = 11,
        [Description("Knitting Order")]
        KnittingOrder = 12,
        [Description("Knitting Yarn Return")]
        KnittingYarnReturn = 13,
        [Description("GRN Bill Preview")]
        GRNBillPreview = 14,
        [Description("Bank Payment Advice")]
        BankPaymentAdvice = 15,
        [Description("Yarn Requisition Preview")]
        YarnRequisitionPreview = 16,
        [Description("Knit Dyeing Gray Challan")]
        KnitDyeingGrayChallan = 17,
        [Description("Cost Sheet Priview")]
        CostSheetPriview = 18,
        [Description("Greige Inspection Report")]
        GreigeInspectionReport = 19,
        [Description("Sub-Contract Yarn Challan Preview")]
        WUSubContractYarnChallanPreview = 20,
        [Description("Sub-Contract Fabric Receive Preview")]
        WUSubContractFabricReceivePreview = 21
    }
    #endregion

    #region Enummeration : EnumProductUsages
    public enum EnumProductUsages
    {
        None = 0,
        Regular = 1,
        FinishGoods = 2,
        RawMaterial = 3,
        PocketLinkFabric = 4, // it Usages in Technical Sheet
        [Description("Yarn/Fabric")]
        Yarn_Fabric = 5,
        Accessories = 6,
        Dyes = 7,
        Chemical = 8,
        Dyes_Chemical = 9,
        Yarn = 10,
        Fabric = 11,
        SpareParts = 12
    }
    #endregion
    #region Enummeration : EnumDashBoardType
    public enum EnumDashBoardType
    {
        None = 0,
        [Description("Dyeing DashBoard")]
        Dyeing_DashBoard = 1,
        [Description("Garments DashBoard")]
        Garments_DashBoard = 2,
        [Description("Textile DashBoard")]
        Textile_DashBoard = 3,
        [Description("Plastic DashBoard")]
        Plastic_DashBoard = 4,
    }
    #endregion


    #region Enummeration : EnumRecipeType
    public enum EnumRecipeType
    {
        None = 0,
        Production = 1,
        Consumtion = 2
    }
    #endregion

    #region Enumeration : ImportFormatType
    public enum EnumImportFormatType
    {
        None = 0,
        Contractors = 1,
        Area = 2,
        Zone = 3,
        Site = 4,
        ChartsOfAccount = 5,
        VProduct = 6,
        ItemProduct = 7
    }
    #endregion

    #region Enummeration : EnumStoreType
    public enum EnumStoreType
    {
        None = 0,
        IssueStore = 1,
        ReceiveStore = 2
    }
    #endregion

    #region Enummeration : EnumOperationType
    public enum EnumRoleOperationType
    {
        None = 0,
        Add = 1,
        Edit = 2,
        Delete = 3,
        View = 4,
        Preview = 5,
        PrintList = 6,
        RateView = 7,
        Approved = 8,
        AdvSearch = 9,
        Received = 10,
        Cancel = 11,
        ViewCMValue = 12,
        Revise = 13,
        Amendment = 14,
        ORSPrint = 15,
        FollowUp = 16,
        SentToProduction = 17,
        SentTobuyer = 18,
        ReviseHistory = 19,
        Payment = 20,
        OfficialInfo = 21,
        SentToAgent = 22,
        Yarn = 23,
        Accessories = 24,
        XLPrint = 25,
        HRAdminApprove = 26,
        DirectorApprove = 27,
        GMApprove = 28,
        WaitforApproval = 29,
        UndoRequest = 30,
        UpdateInfo = 31,
        RoleAssign = 32,
        UserPermission = 33,
        PMPPermission = 34,
        ActiveInActive = 35,
        Lock = 36,
        UnLock = 37,
        Running = 38,
        Close = 39,
        DynamicHeadRefresh = 40,
        MoveAccountHead = 41,
        DRSSearch = 42,
        DRSPrint = 43,
        ORSSearch = 44,
        ProductionExecution = 45,
        VoucherModify = 46,
        Copy = 47,
        Configure = 48,
        BillTransaction = 49,
        COATemplate = 50,
        Initiate = 51,
        Activate = 52,
        Issued = 53,
        Authorized = 54,
        EditMode = 55,
        DeliverToParty = 56,
        Encash = 57,
        Dishonor = 58,
        Return = 59,
        ExtremeEdit = 60,
        ProductionProcess = 61,//add by Mahabub
        ProductionDone = 62,
        UnApproved = 63,
        Disburse = 64,
        Sealed = 65,
        Rate_Hide = 80,
        Buyer_Hide = 81,
        All_Search = 82,
        ChangeNo = 83,
        Signatory_Approve = 84,
        RequestFor_Approve = 85,
        Settlement = 86,
        Verify = 87,
        Transfer = 88,
        MultipleSummary = 89, // added by akram
        BanglaView = 90,
        BankSubmit = 91,
        FDBPReceived = 92,
        MaturityReceive = 93,
        Realization = 94,
        Purchase = 95,
        Commission = 96,
        BackUP = 97,

        Time_Card_F1 = 101,//Mamiya_ACC
        Time_Card_F2 = 102,//ACC
        Time_Card_F2_1 = 103,//TimeKeeper
        Time_Card_F3 = 104,//Comp
        Time_Card_F4 = 105,
        Time_Card_F5 = 106,//AMG ACC
        Time_Card_FC7 = 107,
        Time_Card_F6 = 108,//AMG Comp
        Time_Card_F6_1 = 109,//AMG Comp (In Date & Out Date) for worker
        //Time_Card_F6_2 = 110,//AMG Comp As per configure
        //Time_Card_F6_3 = 111,//AMG Comp As Per Configure
        Time_Card_AMG_Worker = 112,//AMG Actual worker
        Time_Card_Worker = 113,//AMG Actual and Comp worker
        Job_Card = 114, // Ispahani



        Pay_Slip_Bang = 120,
        Pay_Slip_Eng = 121,
        Pay_Slip_F2 = 122,
        Pay_Slip_F3 = 123,
        Pay_Slip_F4 = 124,
        Pay_slip_F5 = 125,
        Pay_slip_F6 = 126,
        Pay_slip_F6_B = 127,
        Pay_Slip_F4_1 = 128,
        Pay_slip_F5_1 = 129,

        Salary_Sheet_F1 = 130,
        Salary_Sheet_F2 = 131,
        Salary_Sheet_F3 = 132,
        Salary_Sheet_F4 = 133,
        Salary_Sheet_F5 = 134,
        Salary_Sheet_F6 = 135,
        Salary_Sheet_F6_B = 136,

        Bank_Sheet = 140,
        Cash_Sheet = 145,
        OT_Sheet_F1 = 150,
        OT_Sheet_F2 = 155,
        OT_Sheet_F3 = 156,
        Salary_Summary_XL = 160,
        Salary_Summary_F2 = 161,
        Salary_Summery_F3 = 162,
        Salary_Summery_F5 = 165,
        OT_Summary_F1 = 163,
        OT_Summary_F2 = 164,

        Salary_Certificate = 170,

        Att_Summary_F2 = 181,
        Att_Summary_F3 = 182,
        Day_Wise_Salary = 183,



        Final_Settlement_Salary_Sheet_AMG = 200,

        //[Description("Monthly Attendence Report")]
        Monthly_Attendence_Report_ISP = 201,
        //[Description("Quarterly Attendence Report")]
        Quarterly_Attendence_Report_ISP = 202,



        IDCardBothSideProtraitF2=219,
        IDCardProtrait = 220,
        IDCardLandscape = 221,
        IDCardBothSideProtrait = 222,
        IDCardBothSideBangla = 223,
        IDCardBanglaF1 = 224,
        IDCardBanglaF2 = 225,

        [Description("Time Card F-01")]
        Time_Card_F_01 = 226,
        [Description("Time Card F-02")]
        Time_Card_F_02 = 227,
        [Description("Time Card F-03")]
        Time_Card_F_03 = 228,
        [Description("Time Card F-04")]
        Time_Card_F_04 = 229,
        [Description("Time Card F-05")]
        Time_Card_F_05 = 230,
        [Description("Time Card F-06")]
        Time_Card_F_06 = 231,
        [Description("Time Card F-07")]
        Time_Card_F_07 = 232,
        [Description("Time Card F-08")]
        Time_Card_F_08 = 233,


        //Salary SheetV2
        [Description("Salary Sheet F-01")]
        Salary_Sheet_F_01 = 234,
        [Description("Salary Sheet F-02")]
        Salary_Sheet_F_02 = 235,
        [Description("Salary Sheet F-03")]
        Salary_Sheet_F_03 = 236,



        ///Leave Application

        [Description("Leave Application")]
        Leave_Application = 237,
        [Description("Leave Application XL")]
        Leave_Application_XL = 238,
        [Description("Alternative Duty")]
        Alternative_Duty = 239,
        [Description("Grace")]
        Grace = 240,




        //Salary Sheet V2
        [Description("Pay Slip F-01")]
        Pay_Slip_F_01 = 245,
        [Description("Pay Slip F-02")]
        Pay_Slip_F_02 = 246,
        [Description("Pay Slip F-03")]
        Pay_Slip_F_03 = 247,
        [Description("BankSheet")]
        BankSheet = 248,
        [Description("CashSheet")]
        CashSheet = 249,
        [Description("Salary Sheet F-04")]
        Salary_Sheet_F_04 = 250,
        [Description("Salary Sheet F-05")]
        Salary_Sheet_F_05 = 251
    }
    #endregion

    #region Enummeration : WorkingUnitFunctionality
    public enum EnumWorkingUnitFunctionality
    {
        None = 0,
        Inventory = 1,
        Sales = 2,
        Cash = 3
    }
    #endregion

    #region Enummeration : ImportPIType
    public enum EnumImportPIType
    {
        None = 0,
        [Description("Foreign L/C")]
        Foreign = 1,
        [Description("Non L/C")]
        NonLC = 2,
        [Description("TT Foreign")]
        TTForeign = 3,
        [Description("Service Contract")]
        Servise = 4,
        [Description("Fancy Yarn")]
        FancyYarn = 5,
        [Description("Local L/C")]
        Local = 6,
        [Description("TT Local")]
        TTLocal = 7
    }
    #endregion

    #region Enummeration : TradingSaleOrderType
    public enum EnumTradingSaleOrderType
    {
        None = 0,
        Internal_Order = 1,
        External_Order = 2
    }
    #endregion
    #region Enummeration : ImportPIType
    public enum EnumImportPIRefType
    {
        None = 0,
        Bill = 1,
        WO = 2,
        GRN = 3,
        PO = 4
    }
    #endregion

    #region Enummeration : ImportPIType
    public enum EnumImportPIEntryType
    {
        Open_PI = 0,
        Ref_PI = 1
    }
    #endregion

    #region Enummeration : Approval Status
    public enum EnumApprovalStatus
    {
        None = 0,
        WaitingForApproval = 1,
        WaitingForreviseApproval = 2,
        ApprovalDone = 3,
        NotApproved = 4,
        AllPaymentDone = 5,

    }
    #endregion

    #region Enummeration : EnumApplicationType
    public enum EnumApplicationType // this enum for define application type like : desktop application, web application, Android  application etc
    {
        None = 0,
        DesktopApplication = 1,
        WebApplication = 2,
        BuyerAsUser = 3

    }
    #endregion

    #region Enummeration : EnumDocumentType
    public enum EnumDocumentType
    {
        [Description("--Select One--")]
        None = 0,
        [Description("Inspection Certificate")]
        Inspection_Certificate = 1,
        [Description("Certificate of Origin")]
        Certificate_of_Origin = 2,
        [Description("Packing List")]
        Packing_List = 3,
        [Description("Commercial Invoice")]
        Commercial_Invoice = 4,
        [Description("Delivery Challan")]
        Delivery_Challan = 5,
        [Description("Bill Of Exchange")]
        Bill_Of_Exchange = 6,
        [Description("GSP")]
        GSP = 7,
        [Description("AZO DYE STUFF")]
        AZO_DYE_STUFF = 8,
        [Description("Truck Receipt")]
        Truck_Receipt = 9,
        [Description("Commission Memo")]
        Commission_Memo = 10,
        [Description("Weight List")]
        Weight_MeaList = 11,
        [Description("Beneficiary Certificate")]
        Beneficiary_Certificate = 12,
        [Description("Bank Submission")]
        Bank_Submission = 13,
        [Description("Bill of Loading")]
        Bill_of_Loading = 14,
        [Description("Applicant Certificate")]
        Applicant_Certificate = 15,
        [Description("Bank Forwarding")]
        Bank_Forwarding = 16,
        [Description("Packing List Detail")]
        Packing_List_Detail = 17
    }
    #endregion

    #region Enummeration : EnumDocumentPrintType
    public enum EnumDocumentPrintType
    {
        [Description("--Select One--")]
        None = 0,
        [Description("Buyer Letter (Bill)")]
        Buyer_Letter_Bill = 1,
        [Description("BTMA Cash Assistance")]
        BTMA_CASH_ASSISTANCE = 2,
        [Description("BTMA GSP Facility")]
        BTMA_GSP_Facility = 3,
        [Description("SIZING SECTION")]
        SIZING_SECTION = 4,
        [Description("WARPING SECTION")]
        WARPING_SECTION = 5,
        [Description("STICKER PRINT")]
        STICKER_PRINT = 6,
        [Description("CARD PRINT")]
        CARD_PRINT = 7
    }
    #endregion
    #region Enummeration : EnumPCCurrentStatus

    public enum EnumSampleInvoiceStatus
    {
        Initialized = 0,
        WaitingForApprove = 1,
        Approved = 2,
        Canceled = 3,// InTransit = 3,
        RequestForCanceled = 4,// InTransit = 3,
        Settled = 5,
        Canceled_Partily = 6,// InTransit = 3,

    }
    #endregion

    #region Enummeration : EnumPaymentType
    public enum EnumPaymentType // this enum for define Payment type like : (Cash Pay, Account pay) etc
    {
        None = 0,
        Cash = 1,
        Cheque = 2,
        LC = 3,
        TT = 4,
        BankAdvice = 5,
        PayOrder = 6,
        DemandDraft = 7,
        AccountPay=8
    }
    #endregion

    #region Enummeration : EnumPIPaymentType
    public enum EnumPIPaymentType
    {
        None = 0,
        LC = 1,
        NonLC = 2
    }
    #endregion

    #region Enummeration : EnumPaymentType
    public enum EnumPaymentReceiveType // this enum for define Payment type like : (Cash Pay, Account pay) etc
    {
        None = 0,
        Cash = 1,
        Bank = 2
    }
    #endregion

    #region Enummeration : EnumPaymentTypeCommission
    public enum EnumPayment_CommissionType // this enum for define Payment type like : (Cash Pay, Account pay) etc
    {
        None = 0,
        Cash = 1,
        Document = 2,
         [Description("Sample Adjustment")]
        SampleAdjustment = 3,
    }
    #endregion

    #region Enummeration : OrderPaymentType
    public enum EnumOrderPaymentType
    {
        None = 0,
        [Description("Cash or Cheque")]
        CashOrCheque = 1,
        [Description("Adj With PI")]
        AdjWithPI = 2,
         [Description("Adj With L/C")]
        AdjWithNextLC = 3,
         [Description("FoC")]
        FoC = 4,
    }
    #endregion

    #region Enummeration : OrderPaymentType
    public enum EnumOrderPaymentAdjType
    {
        None = 0,
        [Description("By cash")]
        Cash = 1,
        [Description("By cheque")]
        Cheque = 2,
        [Description("Qty Adjustment")]
        AdjQty = 3,
        [Description("Rate Adjustment")]
        AdjRate = 4,
    }
    #endregion

    #region Enummeration : Settlement
    public enum EnumSettlementStatus
    {
        None = 0,
        Initialize = 1,
        WaitingToSettle = 2,
        Settled = 3
    }
    #endregion

    #region Enummeration : EnumHolidayType
    public enum EnumHolidayType
    {
        None = 0,
        Occation = 1,
        PublicHoliday = 2
    }
    #endregion

    #region Enummeration : EnumSalaryHeadType
    public enum EnumSalaryHeadType
    {
        None = 0,
        Basic = 1,
        Addition = 2,
        Deduction = 3,
        Reimbursement = 4
    }
    #endregion

    #region Enummeration : EnumEmployeeWorkigStatus
    public enum EnumEmployeeWorkigStatus
    {
        None = 0,
        InWorkPlace = 1,
        OSD = 2,
        SeasionalOff = 3,
        Suspended = 4,
        Terminated = 5,
        Discontinued = 6
    }
    #endregion

    #region Enummeration : EnumSalaryField
    public enum EnumSalaryField
    {
        None = 0,
        [Description("Employee Code")]
        EmployeeCode = 1,
        [Description("Employee Name")]
        EmployeeName = 2,
        [Description("Parent Department")]
        ParentDepartment = 3,
        [Description("Department")]
        Department = 4,
        [Description("Designation")]
        Designation = 5,
        [Description("Joining Date")]
        JoiningDate = 6,
        [Description("Confirmation Date")]
        ConfirmationDate = 7,
        [Description("Employee Type")]
        EmployeeType = 8,
        [Description("Gender")]
        Gender = 9,
        [Description("Total Days")]
        TotalDays = 10,
        [Description("Present Day")]
        PresentDay = 11,
        [Description("Day Off Hoildays")]
        Day_off_Holidays = 12,
        [Description("Absent Days")]
        AbsentDays = 13,
        [Description("Leave Head")]
        LeaveHead = 14,
        [Description("Leave Days")]
        LeaveDays = 15,
        [Description("Employee Working Days")]
        Employee_Working_Days = 16,
        [Description("Early Out Days")]
        Early_Out_Days = 17,
        [Description("Early Out Mins")]
        Early_Out_Mins = 18,
        [Description("Late Days")]
        LateDays = 19,
        [Description("O T Hours")]
        OTHours = 20,
        [Description("O T Rate")]
        OTRate = 21,
        [Description("O T Allowance")]
        OTAllowance = 22,
        [Description("Last Gross")]
        LastGross= 23,
        [Description("Last Increment")]
        LastIncrement = 24,
        [Description("Increment Effect Date")]
        Increment_Effect_Date = 25,
        [Description("Bank Amount")]
        BankAmount = 26,
        [Description("Cash Amount")]
        CashAmount = 27,
        [Description("Account No")]
        AccountNo = 28,
        [Description("Bank Name")]
        BankName = 29,
        [Description("Late Hrs")]
        LateHrs = 30,
        [Description("Employee Contact No")]
        Employee_ContactNo = 31,
        [Description("Present Salary")]
        PresentSalary = 32,
        [Description("Payment Type")]
        PaymentType = 33,
         [Description("Define OT Hour")]
        DefineOTHour=34,
         [Description("Extra OT Hour")]
         ExtraOTHour = 35,
         [Description("Grade")]
         Grade = 36,
         [Description("Leave Details")]
         LeaveDetail = 37
    }
    #endregion

    #region Enummeration : EnumPageOrientation
    public enum EnumPageOrientation
    {
        None = 0,
        [Description("A4_LandScape")]
        A4_Landscape = 1,
        [Description("Legal_LandScape")]
        Legal_LandScape = 2,
        [Description("Letter_LandScape")]
        Letter_LandScape = 3,
        [Description("Dynamic")]
        Dynamic = 4,
        [Description("Legal_Portrait")]
        Legal_Portrait = 5,
        [Description("A4_Portrait")]
        A4_Portrait = 6
    }
    #endregion

    #region Enummeration : EnumProcessManagementType
    public enum EnumProcessManagementType
    {
        None = 0,
        Attendance = 1,
        Payroll = 2,
    }
    #endregion

    #region Enummeration : EnumProcessType
    public enum EnumProcessType
    {
        None = 0,
        DailyProcess = 1,
        MonthlyProcess = 2,
    }
    #endregion

    #region Enummeration : EnumProcessStatus
    public enum EnumProcessStatus
    {
        Initialize = 0,
        Processed = 1,
        Rollback = 2,
        ReProcessed = 3,
        Freeze = 4,
        UnFreeze = 5,
        Entry = 6
    }
    #endregion

    #region Enummeration : EnumEnumAllowanceType
    public enum EnumEnumAllowanceType
    {
        None = 0,
        Addition = 1,
        Deduction = 2,
    }
    #endregion

    #region Enummeration : AllowanceCondition
    public enum EnumAllowanceCondition
    {
        None = 0,
        MonthlyFullAttendance = 1,
        LWP = 2,
        LeaveAllowance = 3,
        NoWorkAllowance = 4,
        Absent = 5,
        Late = 6,
        EarlyLeaving = 7,
        NewJoining = 8,
        Separation = 9,
        CharityFund = 10,
        EducationFund=999
    }
    #endregion

    #region Enummeration : Period
    public enum EnumPeriod
    {
        None = 0,
        Monthly = 1,
        HalfYearly = 2,
        Yearly = 3
    }
    #endregion

    #region Enummeration : RecruitmentEvent
    public enum EnumOrderSheetType
    {
        Sample = 0,
        OrderSheet = 1
    }
    #endregion

    #region Enummeration : LeaveRequiredFor
    public enum EnumLeaveRequiredFor
    {
        All = 0,
        Male = 1,
        Female = 2,
        Others = 3
    }
    #endregion

    #region Enummeration : EnumProductionProcess
    public enum EnumProductionProcess
    {
        None = 0,
        Knitting = 1,
        Linking = 2,
        Trimming = 3,
        Mending = 4,
        Washing = 5,
        Ironing = 6,
        PolyPacking = 7,
        Cartooning = 8,
        Winding = 9,//WD
        Sweing = 10,//SW
        Production=99

    }

    #endregion

    #region Enummeration : Mail Send Purpose
    public enum EnumMailPurpose
    {
        None = 0,
        NewAccount = 1,
        Update = 2,
        ChangePassword = 3,
        RetrivePassword = 4,

    }

    #endregion

    #region Enummeration : EnumPITerms
    public enum EnumPITerms
    {
        None = 0, PaymentTerms = 1, RequiredTerms = 2, DeliveryTerms = 3, OtherTerms = 4
    }
    #endregion

    #region Enummeration : EnumBusinessUnitType
    public enum EnumBusinessUnitType
    {
        None = 0,
        Dyeing = 1,
        Plastic = 2,
        Integrated = 3,
        Spinning=4,
        Weaving=5,
        Finishing=6,
        Garments = 7,
        Textile = 8,
        Others = 9,
        Vehicle = 10,
    }
    #endregion


    #region Enummeration : EnumDriveType
    public enum EnumDriveType
    {
        None = 0,
        [Description("Front Wheel Drive")]
        Front_Wheel_Drive = 1,
        Quattro = 2
    }
    #endregion

    #region Enummeration : EnumFuelType
    public enum EnumFuelType
    {
        None = 0,
        Petrol = 1,
        Diesel = 2
    }
    #endregion

    #region Enummeration : EnumVehicleRegistrationType
    public enum EnumVehicleRegistrationType
    {
        None = 0,
        [Description("In House Client")]
        Inhouse_Client = 1,
        [Description("Out Client")]
        Out_Client = 2
    }
    #endregion

    #region EnumUserImageType : UserImageType
    public enum EnumUserImageType
    {
        None = 0,
        Photo = 1,
        Signature = 2,
    }
    #endregion

    #region Enummeration : EnumContractorAddressType
    public enum EnumAddressType
    {
        None = 0,
        HeadOffice = 1,
        Factory = 2,
        Foreign = 3,
        Others = 4,
    }
    #endregion

    #region Enummeration : EnumFabricProcessType
    public enum EnumFabricProcessType
    {
        [Description("-")]
        None = 0,
        YarnDyed = 1,
        SolidDyed = 2,
        Grey = 3,
        RFD = 4,
        PFD = 5,
        Print = 6,
        AllOverPrint = 7,
        PigmentPrint = 8,
        HansTuthDesignCanvas = 9
    }
    #endregion

    #region Enummeration : EnumFabricWeave
    public enum EnumFabricWeave
    {
        [Description("-")]
        None = 0,
        Weave = 1,
        Bedford = 2,
        BrokenTwill = 3,
        Cambric = 4,
        Canvas = 5,
        CavalryTwill = 6,
        Chambray = 7,
        DoubleCloth = 8,
        [Description("Fil-a-Fil")]
        FilAFil = 9,
        Dobby = 10,
        SlubCanvas = 11,
        Flannel = 12,
        TwillFlannel = 13,
        TwoToneChambray = 14,
        Gabardine = 15,
        Herringbone = 16,
        Leno = 17,
        Matt = 18,
        Moleskin = 19,
        Ottoman = 20,
        Oxford = 21,
        Plush = 22,
        Poplin = 23,
        Ribstop = 24,
        Sateen = 25,
        Seersucker = 26,
        Sheeting = 27,
        SlubChambray = 28,
        Taffeta = 29,
        Twill = 30,
        FineTwill = 31,
        PanamaCanvas = 32,
        PanamaOxford = 33,
        Pinstripe = 34,
        Voile = 35,
        Plain = 36
    }
    #endregion

    #region EnumShipmentTerms : EnumShipmentTerms
    public enum EnumShipmentTerms
    {
        None = 0,
        FOB = 1,
        EXW = 2,
        FCA = 3,
        FAS = 4,
        FOS = 5,
        CFR = 6,
        CIF = 7,
        CPT = 8,
        CIP = 9,
        DAT = 10,
        DAP = 11,
        DDP = 12,
        CNF = 13
    }
    #endregion

    #region EnumMasterLCType : EnumMasterLCType
    public enum EnumMasterLCType
    {
        None = 0,
        MasterLC = 1,
        ExportLC = 2,
        CommercialDoc= 3
    }
    #endregion
    #region EnumNotifyBy : EnumNotifyBy
    public enum EnumNotifyBy
    {
        None = 0,
        [Description("Party&Bank")]
        Party_Bank = 1,
        [Description("Only Party")]
        Party = 2,
        [Description("Only Bank")]
        Bank = 3,
        [Description("Third Party_Bank")]
        ThirdParty_Bank = 4,
        [Description("Only Third Party")]
        ThirdParty = 5
    }
    #endregion

    #region Enummeration : Mail Type
    public enum MailReportingType
    {
        None = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4
    }
    #endregion

    #region New Enums Fields for TNA

    #endregion

    #region EnumApprovalRequestOperationType
    public enum EnumApprovalRequestOperationType
    {
        None = 0,
        TechnicalSheet = 1,
        DevelopmentRecap = 2,
        SaleOrder = 3,
        ProformaInvoice = 4,
        MasterLC = 5,
        PurchasePI = 6,
        B2BLC = 7,
        WorkOrder = 8,
        B2BLCAllocation = 9,
        CostSheet = 10,
        TAP = 11,
        AdvanceSalesContract = 12,
        SCPI = 13,
        OrderSheet = 14,
        ProductionOrder = 15,
        DeliveryOrder = 16,
        OrderRecap = 17,
        StyleBudget = 18

    }
    #endregion

    #region EnumReviseRequestOperationType
    public enum EnumReviseRequestOperationType
    {
        None = 0,
        OrderSheet = 1,
        WorkOrder = 2,
        OrderRecap = 3,
        ProformaInvoice =4,
        MasterLC = 5
    }
    #endregion

    #region Enummeration : EnumHIASetupType
    public enum EnumHIASetupType
    {
        Manual = 0,
        System = 1
    }
    #endregion

    #region Enummeration : EnumHIASetupType
    public enum EnumTimeEventType
    {
        ON_Date = 0,
        After_Action = 1,
        Before_Action = 2
    }
    #endregion

    #region Enumeration : LocationType
    public enum EnumLocationType
    {
        None = 0, Area = 1, Zone = 2, Site = 3
    }
    #endregion
    
    #region Enumeration : LocationType
    public enum EnumStyleBudgetRecapType
    {
        None = 0, PAM = 1, OrderRecap = 2
    }
    #endregion

    #region Enummeration : EnumDepthOfShade
    public enum EnumDepthOfShade
    {
        None=0,
        Dark = 1,
        ExtraDark = 2,
        Light = 3,
        Medium = 4,
        White = 5,
        Black = 6
    }
    #endregion

    #region Enummeration : EnumPPMObject
    public enum EnumPPMObject
    {
        None = 0,
        Department = 1,
        SalaryScheme = 2,
        SalaryHead = 3,
        EmployeeGroup = 4
    }
    #endregion

    #region Enummeration : EnumITaxRebateType
    public enum EnumITaxRebateType
    {
        None = 0,
        Investment = 1,
        Donation = 2,

    }

    #endregion

    #region Enummeration : EnumTaxArea
    public enum EnumTaxArea
    {
        None = 0,
        CityCorporation = 1,
        DistrictArea = 2,
        OtherArea = 3
    }
    #endregion

    #region Enummeration : EnumEmployeeCardStatus

    public enum EnumEmployeeCardStatus
    {
        None = 0,
        Print = 1,
        Delivered = 2,
        Return = 3,
        Lost = 4,
        Damaged = 5,
    }

    #endregion

    #region Enummeration : EnumTaxPayerType
    public enum EnumTaxPayerType
    {
        None = 0,
        Male = 1,
        Female = 2,
        NonResident = 3
    }
    #endregion

    #region Enummeration : EnumSequenceType
    public enum EnumSequenceType
    {
        None = 0,
        First = 1,
        Next = 2,
        RestOfAmount = 3
    }
    #endregion

    #region Enummeration : EnumCertificateType
    public enum EnumCertificateType
    {
        None = 0,
        COC = 1,
        STCW = 2,

    }

    #endregion

    #region Enummeration : EnumValueOperator

    public enum EnumValueOperator
    {
        None = 0,
        Value = 1,
        Operator = 2

    }

    #endregion

    #region Enummeration : EnumEmployeeLoanStatus
    public enum EnumEmployeeLoanStatus
    {
        None = 0,
        Requested = 1,
        Accepted = 2,
        Approved = 3,
        Cancelled = 4,
        Partial_Disbursed = 5,
        Full_Disbursed = 6,
        Realized = 7,
        Settled = 8
    }
    #endregion

    #region Enummeration : Employee Doc
    public enum EnumEmployeeDoc
    {
        General = 0,
        Education = 1,
        Experience = 2,
        Training = 3,
    }
    #endregion

    #region Enummeration : PayrollCalculationApplyOn
    public enum EnumPayrollApplyOn
    {
        None = 0,
        Gross = 1,
        Basic = 2
    }
    #endregion

    #region Enummeration : EnumCalculationOn
    public enum EnumCalculationOn
    {
        None = 0,
        [Description("Depend On Calculation")]
        Depend_on_Calculation = 1,
        [Description("Basic/As per Month")]
        Basic_As_PerMonth = 2,
        [Description("Gross/As per Month")]
        Gross_As_PerMonth = 3
    }
    #endregion

    #region Enummeration : EnumDeductionCalculationOn
    public enum EnumDeductionCalculationOn
    {
        None=0,
        [Description("Day Basis")]
        Day_Basis = 1,
        [Description("Minute Basis")]
        Minute_Basis = 2
    }
    #endregion

    #region Enummeration : EnumOperator

    public enum EnumOperator
    {
        None = 0,
        BracketStart = 1,
        BracketEnd = 2,
        Addition = 3,
        Subtruction = 4,
        Multiplication = 5,
        Division = 6,
        Percent = 7

    }

    #endregion

    #region Enummeration : Mail Type
    public enum EnumPETType
    {
        None = 0,
        ProfitDeclaration = 6

    }
    #endregion

    #region Enummeration : Data Provider
    public enum EnumDataProvider
    {
        None = 0,
        Access = 1,
        SQL = 2,
        Oracle = 3
    }
    #endregion

    #region Enummeration : Benefit On Attendance
    public enum EnumBenefitOnAttendance
    {
        None = 0,
        DayOff_Holiday_Presence = 1,
        Time_Slot = 2,
        OnlyDayOff_Presence = 3,
        OnlyHoliday_Presence = 4
    }
    #endregion

    #region Enummeration : Employee Grouping
    public enum EnumEmployeeGrouping
    {
        None = 0,
        EmployeeType = 1,
        StaffWorker = 2,
        Block = 3
    }
    #endregion

    #region Enummeration :Enum Month
    public enum EnumMonth
    {
        None = 0,
        Jan = 1,
        Feb = 2,
        Mar = 3,
        Apr = 4,
        May = 5,
        Jun = 6,
        July = 7,
        Aug = 8,
        Sep = 9,
        Oct = 10,
        Nov = 11,
        Dec = 12
    }
    #endregion

    #region Enummeration : EnumCurrentEmploymentStatus
    public enum EnumCurrentEmploymentStatus
    {
        None = 0,
        Initialize = 1,
        WaitingForJoin = 2,
        Onboard = 3,
        Available = 4

    }
    #endregion

    #region Enummeration : EnumVesselType
    public enum EnumVesselType
    {
        None = 0,
        Bulk = 1,
        Container = 2,
        Oil_Tanker = 3,
        Chemical_Tanker = 4,
        Other_Tanker = 5,
        Tug = 6,
        Offshore_Ship = 7

    }

    #endregion

    #region Enummeration : LabdipOrderStatus
    public enum EnumLabdipOrderStatus
    {
        None = 0,
        Initialized = 1,
        WaitForApprove = 2,
        Approve = 3,
        InLab = 4,
        LabdipDone = 5,
        [Description("Release From Lab")]
        WaitingForReceiveFromLab = 6,
        [Description("Labdip In Hand")]
        LabdipInHand = 7,
        [Description("Send To Buyer")]
        LabdipInBuyerHand = 8, ///8
        BuyerApproval = 9, //9
        Cancel = 10//10
    }
    #endregion
    #region Enummeration : FabricLabStatus
    public enum EnumFabricLabStatus
    {
        //None = 0,
        //Initialized = 1,
        //WaitForApprove = 2,
        //Approve = 3,
        //InLab = 4,
        //LabdipDone = 5,
        //[Description("Release From Lab")]
        //WaitingForReceiveFromLab = 6,
        //[Description("Labdip In Hand")]
        //LabdipInHand = 7,
        //[Description("Submitted")]
        //Submitted = 8, ///8
        //Hold = 9, //9
        //Undo = 10, //9
        //Cancel = 15,//10
        //ReLab = 16

        None = 0,
        Initialized = 1,
        WaitForApprove = 2,
        Approve = 3,
        InLab = 4,
        LabdipDone = 5,
        [Description("Release From Lab")]
        WaitingForReceiveFromLab = 6,
        [Description("Labdip In Hand")]
        LabdipInHand = 7,
        [Description("Submitted")]
        Submitted = 8, ///8
        Hold = 9, //9
        DispoIssued = 10, //9
        Cancel = 15,//10
        ReLab = 16, 
        Undo = 17, 
    }
    #endregion

    #region Enumeration : BusinessNature
    public enum EnumBusinessNature
    {
        None = 0, Manufacturing = 1, Service = 2, Trading = 3

    }
    #endregion

    #region Enumeration : LegalFormat
    public enum EnumLegalFormat
    {
        None = 0, PublicLimitedCompany = 1, PrivateLimitedCompany = 2, SelfProprietorship = 3, Partnership = 4
    }
    #endregion

    #region Enummeration : DOControl
    public enum EnumDOControl
    {
        None = 0,
        [Description("After PI Issue")]
        AfterPIIssue = 1,
        [Description("After LC Open")]
        AfterLCOpen = 2,
        AfterBillAcceptance = 3,
        [Description("After UD Receive")]
        AfterUDReceive = 4,
        AfterBillMaturity = 5
    }
    #endregion

    #region Enummeration : EnumPurchaseInvoiceEvent
    public enum EnumPurchaseInvoiceEvent
    {
        None = 0,
        Initialized = 1,
        ShipmentDone = 2,
        AcceptedBill = 3,//ReceivedFromBuyer = 2,
        PaymentDone_Partlaly = 4,
        PaymentDone = 5,
        BillCancel = 6,
        BillClosed = 7
    }
    #endregion

    #region Enummeration : EnumImportInvoiceEvent
    public enum EnumImportInvoiceEvent
    {
        None = 0,
        Initialized = 1,
        ShipmentDone = 2,
        AcceptedBill = 3,//ReceivedFromBuyer = 2,
        PaymentDone_Partlaly = 4,
        PaymentDone = 5,
        BillCancel = 6,
        BillClosed = 7
    }
    #endregion

    #region Enummeration : EnumPackCountBy
    public enum EnumPackCountBy
    {
        Bales = 1,
        Drum = 2,
        Carton = 3,
        Hanks = 4,
        Cone = 5,
        Packet = 6,
        Bag = 7,
        Roll = 8,
        Set = 9,
        Pallet = 10,
        Box = 11
    }
    #endregion

    #region Enummeration : InvoiceBankStatus
    public enum EnumInvoicePaymentStatus
    {
        None = 0,
        //Document_In_HandBank = 1,
        //WaitFor_Acceptance = 2,
        //IBP = 3,
        //Payment_Done = 4,
        Initialize = 1,
        Approved = 10, //Payable Create
        [Description("Send To Reqsition")]
        SendToReq = 11, // Send Payment Requasition
        [Description("Request for Fund Req")]
        ReqForFundReq = 12, // Send Payment Requasition
        ApproveForPayment = 13, // Approve for Payment 
        WaitingForPayment = 14, // Send For Tressery
        [Description("PaymentDone_Partialy")]
        PaymentDonePartialy = 15, // Create payment Account
        PaymentDone = 16,// Create payment Account
        Cancel = 17



    }
    #endregion

    #region Enummeration : EnumInvoiceReferenceType
    public enum EnumInvoiceReferenceType
    {
        None = 0,
        Open = 1,
        PO = 2,
        Import = 3, //Import(LC Purchase) Landing Cost
        Local = 4,
        WO =5// Local (Non LC Purchase) Landing Cost 
    }
    #endregion

    #region Enummeration : EnumPInvoiceType
    public enum EnumPInvoiceType
    {        
        None = 0,
        Standard = 1,
        Advance = 2,
        Bonded = 3
    }
    #endregion

    #region Enummeration : EnumPInvoiceStatus
    public enum EnumPInvoiceStatus
    {
        Initialize = 0,
        Approved = 1,
        GoodsRevInProgress = 2,
        GoodsReceived = 3
    }
    #endregion

    #region Enummeration : Shipment By
    public enum EnumShipmentBy
    {
        None = 0,
        Truck = 1,
        Air = 2,
        Sea = 3,
        Other = 4
    }
    #endregion

    #region Enummeration : EnumLCType
    public enum EnumLCType
    {
        At_Sight = 0,
        Deferred = 1
    }
    #endregion
    #region Enummeration : CurrencyType
    public enum EnumCurrencyType
    {
        None = 0, Local = 1, Foreign = 2
    }
    #endregion
    #region Enummeration : EnumImportPIType
    public enum EnumImportFileType
    {
        Open = 0,
        [Description("Business Unit Wise")]
        B_UnitWise =1,
        [Description("As Per ProductCalegory")]
        AsPerProductCalegory = 2,
      
    }
    #endregion
    #region Enummeration : EnumImportPIType
    public enum EnumImportDateCalBy
    {
        None = 0,
        [Description("Application Req Date")]
        AppReqDate = 1,
        [Description("Cover Note Date")]
        CoverNoteDate = 2,

    }
    #endregion

    #region Enummeration : InvoiceBankStatus
    public enum EnumPCReferenceType
    {
        None = 0,
        Open = 1,
        [Description("OA")]
        OA = 2,
    }
    #endregion
        
    #region Enummeration : EnumGRNType
    public enum EnumGRNType
    {
        None = 0,
        LocalInvoice = 1,  // RefObjectID wiil be PurchaseInvoiceID in GRN Table &  GRN Detail Table RefObjectID wiil be PurchaseInvoiceDetailID
        ImportInvoice = 2, // RefObjectID wiil be ImportInvoiceID in GRN Table &  GRN Detail Table RefObjectID will be ImportackDetailID
        WorkOrder = 3,     // RefObjectID wiil be WorkOrderID in GRN Table &  GRN Detail Table RefObjectID will be WorkOrderDetailID
        //[Description("Service Contract")]
        //Service = 4, // RefObjectID wiil be ImportInvoiceID in GRN Table &  GRN Detail Table RefObjectID will be ImportackDetailID . Only Servics Type Invoice (For Buyer Product Receive)
        [Description("Fancy Yarn")]
        FancyYarn = 5, // RefObjectID wiil be ImportInvoiceID in GRN Table &  GRN Detail Table RefObjectID will be ImportackDetailID . Only Servics Type Invoice (For Buyer Product Receive)
        [Description("ImportPI")]
        ImportPI = 6, // RefObjectID wiil be ImportPIID in GRN Table &  GRN Detail Table RefObjectID will be ImportPIDetailID
        [Description("Floor Return")]
        FloorReturn = 7, // RefObjectID wiil be ConsumptionRequisitionID in GRN Table &  GRN Detail Table RefObjectID will be ConsumptionRequisitionDetailID
        [Description("Purchase Order")]
        PurchaseOrder = 8  // RefObjectID wiil be PurchaseOrderID in GRN Table &  GRN Detail Table RefObjectID wiil be PurchaseOrderDetailID
    }
    #endregion


    #region Enummeration : EnumGRNType
    public enum EnumPurchaseReturnType
    {
        None = 0,
        LocalInvoice = 1,  // RefObjectID wiil be PurchaseInvoiceID in PurchaseReturn Table &  PurchaseReturn Detail Table RefObjectID wiil be PurchaseInvoiceDetailID
        ImportInvoice = 2, // RefObjectID wiil be ImportInvoiceID in PurchaseReturn Table &  PurchaseReturn Detail Table RefObjectID will be ImportackDetailID
        WorkOrder = 3,     // RefObjectID wiil be WorkOrderID in PurchaseReturn Table &  PurchaseReturn Detail Table RefObjectID will be WorkOrderDetailID 
        ImportPI = 4, // RefObjectID wiil be ImportPIID in PurchaseReturn Table &  PurchaseReturn Detail Table RefObjectID will be ImportPIDetailID
        PurchaseOrder = 5  // RefObjectID wiil be PurchaseOrderID in PurchaseReturn Table &  PurchaseReturn Detail Table RefObjectID wiil be PurchaseOrderDetailID
    }
    #endregion

    #region Enummeration : EnumGRNStatus
    public enum EnumGRNStatus
    {
        Initialize = 0,
        Approved = 1,
        GoodReceived = 2
    }
    #endregion

    #region Enummeration : EnumWarpWeft
    public enum EnumWarpWeft
    {
        None = 0,
        [Description("Warp")]
        Warp = 1,
        [Description("Weft")]
        Weft = 2,
        [Description("Warp & Weft")]
        WarpnWeft = 3
    }
    #endregion

    #region Enummeration : EnumProductionType
    public enum EnumProductionType
    {
        None = 0,
        [Description("Full Solution")]
        Full_Solution = 1,
        [Description("Commissioning")]
        Commissioning = 2,
         [Description("Raw Sale")]
        RawSale = 3
    }
    #endregion
    #region Enummeration : EnumRecycleProcessType
    public enum EnumRecycleProcessType
    {
        [Description("Runner Recycle")]
        RunnerRecycle = 1,
        [Description("Goods Return Recycle")]
        GoodsReturnRecycle = 2
    }
    #endregion
    #region Enummeration : EnumArchiveStatus
    public enum EnumArchiveStatus
    {
        [Description("Initialize Value")]
        InitializeValue = 1,
        [Description("Backup Data")]
        BackupData = 2,
        [Description("Approved")]
        Approved = 3
    }
    #endregion

    #region Enummeration : EnumProductNature
    public enum EnumProductNature
    {
        Dyeing = 0,/// Delete it By Yarn
        Hanger = 1,
        Poly = 2,
        Yarn = 3,
        DyesChemical=4,
        Machineries= 5,
        Accessories =6,
        Buying = 7,
        [Description("Gray Yarn")]
        GrayYarn = 8,
        [Description("Dye Yarn")]
        DyeYarn = 9,
        Cone = 10,
        Sizer = 11,
        Dyes = 12,
        Chemical = 13,
    }
    #endregion
    #region Enummeration : EnumCalendarApply
    public enum EnumCalendarApply
    {
        [Description("Full Company")]
        Full_Company=1,
        [Description("DRP Wise")]
        DRP_Wise=2

    }
    #endregion

    #region EnumFNReProRequestStatus : EnumFNReProRequestStatus
    public enum EnumFNReProRequestStatus
    {
        Initialize = 0,
        Approved = 1
    }
    #endregion

    #region Enummeration : EnumMgtDBACType
    public enum EnumMgtDBACType
    {
        None = 0,
        [Description("Cash Balance")]
        Cash_Balance = 1,
        [Description("Bank Balance")]
        Bank_Balance = 2,
        [Description("Foreign Bank Balance")]
        Foreign_Bank_Balance = 3,
        [Description("Receivable")]
        Receivable = 4,
        [Description("Payable")]
        Payable = 5,
        [Description("Bank Loan")]
        Bank_Loan = 6
    }
    #endregion

    #region Enummeration : EnumShipmentMode
    public enum EnumShipmentMode
    {
        None = 0,
        Sea = 1,
        Air = 2,
        [Description("Sea & Air")]
        Sea_And_Air = 3,
        
    }
    #endregion

    #region Enummeration : SampleStates
    /// <summary>
    /// a Sample Order item could have following states in its lifetime. Only any one state at any time.
    /// </summary>
    public enum EnumDyeingOrderState
    {
        None = 0,
        Initialized = 1,
        WatingForApprove = 2, //stop
        [Description("Authorized")]
        ApprovalDone = 3,
         [Description("Req For Labdip")]
        Req_ForLabdip = 4,
         [Description("Lab Dip Done")]
        LabDipDone = 5,
        [Description("Send to Factory")]
        InProduction = 6,
        //[Description("Sample Card Issue")]
        [Description("Export from Factory")]
        ReceiveInHO = 7,
        [Description("Delivered to Buyer")]
        Deliverd = 8,
        Cancelled = 9,

    }
    #endregion

    #region Enummeration : AllowNotAllow
    public enum EnumAllowNotAllow
    {
        NotAllow = 0,
        Allow = 1
    }
    #endregion

    #region Enummeration : LoanTypes
    public enum EnumLiabilityType
    {
        None = 0,
        [Description("A/C TRSFR")]
        Accounttransfer = 1,
        [Description("LTR")]
        LTR = 2,
        EDF = 3,
        [Description("Long-Term Loan")]
        LongTermLoan = 4,
        PAD = 5,
        [Description("PC Loan")]
        PC_Loan = 6,
        [Description("U-Pass")]
        U_Pass = 7
    }
    #endregion

    #region Enummeration : EnumPurchaseRequisitionType
    public enum EnumPurchaseRequisitionType
    {
        None = 0,
        Open = 1,
        BOQ = 2,
    }
    #endregion

    #region Enumeration : EnumQuotationStatus
    public enum EnumQuotationStatus
    {
        Initialize = 0,
        WaitForApproved = 1,
        Approve = 2,
        Pause = 3, 
        RequestRevise=4
    }
    #endregion

    #region Enumeration : EnumSalesStatus
    public enum EnumSalesStatus
    {
        None = 0,
        Open = 1,
        Hold = 2,
        Sold = 3
    }
    #endregion

    #region Enumeration : EnumIsSource
    public enum EnumSource
    {
        Manual = 0,
        PO = 1

    }
    #endregion

    #region Enumeration : EnumIsSource
    public enum EnumProcessProductType
    {
        None = 0,
        RawMaterial = 1,
        DamageProduct = 2

    }
    #endregion

    #region Enummeration : EnumPurchaseInvoiceEvent
    public enum EnumPurchaseRequisitionDetailStatus
    {
        None = 0,
        Initialized = 1,
        AttachWithNOA = 2,
        AttachWithPO = 3,//ReceivedFromBuyer = 2,
        Cancel = 4,
    }
    #endregion

    #region Enummeration : EnumPurchaseInvoiceMode
    public enum EnumInvoicePaymentMode
    {
        None = 0,
        Cash = 1,
        LC = 2, 
        Credit = 3,
        [Description("Credit/Cash")]
        Credit_Cash = 4,
        [Description("Advance Cheque/Cash")]
        Advance_Cash = 5
    }
    #endregion

    #region Enummeration : EnumPurchaseRequisitionStatus
    public enum EnumPurchaseRequisitionStatus
    {
        Initialized = 0,
        Approve = 1,
        Finish = 2,
        Cancel = 3,
        RequestRevise = 4
    }
    #endregion

    #region Enummeration :Enum EnumPunchFormat
    public enum EnumPunchFormat
    {
        DD_MM_YY = 1,
        MM_DD_YY = 2,
        YY_MM_DD = 3,
        WithSeparateTime = 4
    }
    #endregion

    #region Enummeration : EvaluationType
    public enum EnumPOReferenceType
    {
        None = 0,
        Open = 1,
        NOA = 2,
        Requistion = 3
    }
    #endregion 
    #region Enummeration : EnumCostHeadType
    public enum EnumCostHeadType
    {
       None = 0, 
       Add =1,
       Sub = 2
    }
    #endregion
    

    #region Enummeration : EnumAccountHeadNature
    public enum EnumAccountHeadNature
    {
        None = 0,
        [Description("MIT Account")]
        MIT_ACCOUNT = 1,
        [Description("Raw Material Stock Account")]
        RAW_MATERIRAL_STOCK_ACCOUNT = 2,
        [Description("Finish Good Stock Account")]
        FINISH_GOOD_STOCK_ACCOUNT = 3
    }
    #endregion

    #region Enummeration : EnumPOStatus
    public enum EnumPOStatus
    {
        Initialize = 0,
        Approved = 1
    }
    #endregion

    #region Enummeration : POPaymentType
    public enum EnumPOPaymentType
    {
        None = 0,
        LC = 1,
        NonLC = 2,


    }

    #endregion

    #region Enummeration : EnumPaymentTermType
    public enum EnumPaymentTermType
    {
        None = 0,
        Delivery = 1,
        PO = 2,
        Invoice = 3,

    }
    #endregion

    #region Enummeration : EnumPITerms
    public enum EnumPOTerms
    {
        None = 0, ScopeOFWork = 1, DeliveryTime = 2, DeliveryPlace = 3, Payment = 4, Invoice = 5, LDFordelaydelivery = 6, Warranty = 7, General = 8, Special = 9
        //None = 0, DeliveryTime = 1, DeliveryPlace=2, RequiredDoc = 2, Payment = 3, ScopeOFWork = 4, Warranty = 5, General = 6, LDFordelaydelivery = 7
    }
    #endregion

    #region Enummeration : EnumDayApplyType
    public enum EnumDayApplyType
    {
        None = 0,
        After = 1,
        Before = 2,
        With = 3,

    }
    #endregion

    #region Enummeration : EnumDisplayPart
    public enum EnumDisplayPart
    {
        None = 0,
        Day = 1,
        Week = 2,
        Month = 3,
        Year = 4
    }
    #endregion

    #region Enummeration : EnumWoringUnitType
    public enum EnumWoringUnitType
    {
        None = 0,
        General = 1,
        Raw = 2, //this type of store use for Raw Materials
        Salable = 3, // this type of store use for Finish Goods/Delivery  items 
        Available = 4 //this type of store use for Finish Goods/Delivery  Available(not angage with Order) items 
    }
    #endregion

    #region Enummeration : Enum lot Status
    public enum EnumLotStatus
    {
        Open = 0,
        Running = 1,
        Hold = 2
    }
    #endregion

    #region Enummeration : AdjustmentType
    public enum EnumAdjustmentType
    {
        None = 0,
        [Description("Natural Loss Gain")]
        Natural_Loss_Gain = 1,
        [Description("Mistake Adjustment")]
        Mistake_Adjustment = 2,
        [Description("Lot Concatenation")]
        Lot_Concatenation = 3,
        [Description("Lot Adjust To Zero Balance")]
        Lot_AdjustToZeroBalance = 4,
        [Description("Test")]
        Test = 5,
        [Description("Return Stock")]
        Return_Stock = 6,
        [Description("Sample Received")]
        Sample_Receive = 7 ,
        [Description("Excess")]
        Excess = 8,
        [Description("Warping Excess")]
        WarpingExcess = 9,
        [Description("Sizing Excess")]
        SizingExcess = 10,
        [Description("Pre Treatemant Excess")]
        WarpExcess = 11,
        [Description("RnD Excess")]
        RnDExcess = 12,
        [Description("Hangs Excess")]
        HangsExcess = 13,
        [Description("Lab Excess")]
        LabExcess = 14,
        [Description("Dyeing Excess")]
        DyeingExcess = 15,
    }
    #endregion

    #region Enummeration : AdjustmentType
    public enum EnumRefType
    {
        ExportPI=1,
        OrderShit=2,
    }
    #endregion
    //
    #region Enummeration : EnumRecapRefType
    public enum EnumRecapRefType
    {
        None = 0,
        TechnicalSheet = 1,
        OrderRecap = 2,
    }
    #endregion
    
        #region Enummeration : EnumRecapYarnType
    public enum EnumRecapYarnType
    {
        None = 0,
        Fabric = 1,
        ContrastFabric = 2,
        PocketingFabric = 3
    }
    #endregion

    #region Enummeration : AdjustmentType
    public enum EnumClaimOrderType
    {
        None = 0,
        REPLACEMENT = 1,
        ShortClaim = 2,
        PartyClaim = 3,
        ExtraYarn = 4
    }
    #endregion

    #region Enummeration : EnumClaimOperationType
    public enum EnumClaimOperationType
    {
        None = 0,
        Export = 1,
        Import = 2
    }
    #endregion

    #region Enummeration : DeliveryChallanType
    public enum EnumChallanType
    {
        Regular = 1,
        Avaliable = 2,
    }
    #endregion

    #region Enummeration : EnumPlanType
    public enum EnumPlanType
    {
        General = 1,
        Lower = 2,
    }
    #endregion

    #region Enummeration : DeliveryChallanType
    public enum EnumChallanStatus
    {
        Initialized = 0,
        Approved = 1,
        Done=2,
    }
    #endregion

    #region Enummeration : EnumOperationalDept
    public enum EnumOperationalDept
    {
        None = 0,
        Marketing = 1,
        [Description("Import Own")]
        Import_Own= 2,
        [Description("Import Party")]
        Import_Party = 3,
        [Description("Export Own")]
        Export_Own = 4,
        [Description("Export Party")]
        Export_Party = 5,
        Production = 6,
        Accounts = 7
    }
    #endregion

    #region Enummeration : SampleInvoiceType
    public enum EnumSampleInvoiceType
    {
        None = 0, SampleInvoice = 1, Adjstment_Qty = 3, Adjstment_Value = 4, Adjstment_Commission = 5, ReturnAdjustment = 6, DocCharge = 10, SalesContract = 11, Fabric_PO = 12  
    }
    #endregion

    #region Enummeration : PaymentStatus
    public enum EnumPaymentStatus
    {
        Initialize = 0,
        WaitForEncashment = 1,
        Dishonour = 2,
        Encashment = 3,
        Return = 4,
        Canceled = 5
    }
    #endregion
    #region Enummeration : EnumPaymentRefType
    public enum EnumPaymentRefType
    {
        None = 0,
        ReceivedPayment = 1,
        PaidPayment = 2,
        SaleReturnPaid = 3, // It's Used for sale return payment
    }
    #endregion
   
    #region Enummeration : EnumSwatchType
    public enum EnumSwatchType
    {
        None = 0,
        BuyerSwatch = 1,
        RAndDSwatch = 2
    }
    #endregion

    #region Enummeration : EnumFabricProcess
    public enum EnumFabricProcess
    {
        None = 0,
        Process = 1,
        Finish = 2,
        Weave = 3,
        FabricDesign = 4
    }
    #endregion
    
    #region Enummeration : EnumOrderWUType
    public enum EnumFabricRequestType
    {
        None = 0,
         [Description("Hand Loom")]
        HandLoom = 1,
        Sample = 2,
        Bulk = 3,
        Analysis = 4,
        CAD = 5,
        Color = 6,
        Labdip=7,
        YarnSkein = 8,
        SampleFOC = 9,
         [Description("Stock Sale")]
        StockSale = 10,
         [Description("Local Bulk")]
        Local_Bulk=11,
        [Description("Local Sample")]
        Local_Sample=12,
        Buffer = 13,
         [Description("Own PO")]
        OwnPO = 14,

    }
    #endregion

    #region Enummeration : EnumPropertyType
    public enum EnumPropertyType
    {
        None = 0,
        [Description("Size Inch")]
        Size_inch = 1,
        [Description("Size Centimeter")]
        Size_CM = 2,
        Brand = 3,
        [Description("Reference Caption")]
        Reference_Caption = 4,
        [Description("Quantity Per Carton")]
        Qty_PerCarton = 5,
        PP = 6,
        [Description("K-Resin")]
        K_RESIN = 7,
        HIPS = 8,
        GPPS = 9

    }
    #endregion

    #region Enummeration : EnumProductionUnitType
    public enum EnumProductionUnitType
    {
        None = 0,
        [Description("In House Production")]
        InHouseProduction = 1,
        [Description("External Production")]
        ExternalProduction = 2
    }
    #endregion
        
    #region Enummeration : Production Schedule Status
    public enum EnumProductionScheduleStatus
    {
        Hold = 0,
        Publish = 1,
        Running = 2,
        Complete = 3,
        Finish = 4,
        Cancel = 5,
        Urgent = 6
    }
    #region Enummeration : EnumOperationStage
    public enum EnumOperationStage //For ExportMgtChallan report
    {
        None = 0,
        AdvanceDelivery = 1,                         //Pending LC against the Advance delivery.
        //LCInHand_PendingDelivery = 2,                   //Pending Delivery against the non-prepared documents.
        LCInHand_Delivery_Done = 2,    //Pending non-preparation documents against the delivery completed.
        BOINHand = 3,       //Pending prepared documents but non-submission to buyers.
        BOINCUS_Hand = 4,   //Pending party acceptance documents to buyers.
        PendingBankSubmition = 5,    //Pending received party acceptance, but non-submission to bank.
        PendingBankAcceptance = 6,    //Pending bank acceptance documents to buyers bank.
        OverduePayment = 7,             //Pending overdue payment to buyers bank.
        OnlyDuePayment = 8               //Pending Non-due payment to buyers bank.
    }
    #endregion
    #region Enummeration : DeliveryChallanType
    public enum EnumFabricPOStatus
    {
        None = 0,
        Initialized = 1,
        RequestForApprove = 2,
        Approved = 3,
        Received = 4,
        Cancel = 5,
    }
    #endregion

    #endregion
    
    #region Enummeration : EnumAssortmentType
    public enum EnumAssortmentType
    {
        Select_AssortmentType = -1,
        Assort_Color_Assort_Size = 0,
        Solid_Color_Assort_Size = 1,
        Solid_Color_Solid_Size = 2

    }
    #endregion

    #region Enummeration : EnumIncoterms
    public enum EnumIncoterms
    {
        None = 0,
        FOB = 1,
        CNF = 2,
        FCA =3
    }
    #endregion

    #region Enummeration : EnumTransportType
    public enum EnumTransportType
    {
        None = 0,
        Sea = 1,
        Air = 2,
        Road = 3,
        [Description("Sea-Air")]
        Sea_Air = 4
    }
    #endregion

    #region Enummeration : EnumExportSummaryReportName
    public enum EnumExportSummaryReportName
    {
        None = 0,
        [Description("Export LC Receive Report")]
        Export_LC_Receive_Report = 1,
        [Description("Bill Prepare Report")]
        Bill_Prepare_Report = 2,
        [Description("Send To Party Report")]
        Send_To_Party_Report = 3,
        [Description("Received From Party Report")]
        Received_From_Party_Report = 4,
        [Description("Send To Bank Report")]
        Send_To_Bank_Report = 5,
        [Description("Received From Bank Report")]
        Received_From_Bank_Report = 6,
        [Description("LDBC/IBC Report")]
        LDBC_IBC_Date_Report = 7,
        [Description("Acceptance Report")]
        Acceptance_Date_Report = 8,
        [Description("Maturity Report")]
        Maturity_Date_Report = 9,
        [Description("Maturity Received Report")]
        Maturity_Received_Date_Report = 10,
        [Description("Relization Report")]
        Relization_Date_Report = 11,
        [Description("Bank FDD Receive Report")]
        Bank_FDD_Receive_Date_Report = 12,
        [Description("Discounted Report")]
        Discounted_Date_Report = 13,
        [Description("Encashment Report")]
        Encashment_Date_Report = 14,
    }
    #endregion

    #region Enummeration : EnumExportSummaryReportType
    public enum EnumExportSummaryReportType
    {
        None = 0,
         [Description("Regular Statement")]
        RegularStatement=1,
         [Description("CompareStatement")]
        CompareStatement=2
    }
    #endregion

    #region Enummeration : EnumExportSummaryReportLayout
    public enum EnumExportSummaryReportLayout
    {
        None = 0,
        [Description("MKT Person Wise")]
        MKTPersonWise = 1,
        [Description("Buyer Wise")]
        BuyerWise = 2,
        [Description("Bank Wise")]
        BankWise=3
    }
    #endregion

    #region Enummeration : EnumOrderRecapStatus
    public enum EnumOrderRecapStatus
    {
        Initialized = 0,
        RequestForApproval = 1,
        Approved = 2,
        Request_For_Revise = 3
    }
    #endregion

    #region Enummeration : EnumAccountsHeadDefineNature
    public enum EnumAccountsHeadDefineNature
    {
        None = 0,
         [Description("Define AccountHead")]
        DefineAccountHead = 1,
         [Description("Undefine AccountHead")]
        UndefineAccountHead = 2
    }
    #endregion

    #region Enummeration : EnumDepartment
    public enum EnumDepartment
    {
        NA = 0,
        NightShiftHAuthority = -2,
        HigherAuthority = -1,
        Admin = 1,
        Marketing = 2,
        Lab = 3,
        ProductionAdmin = 4,
        ProductionFloor = 5,
        QC = 6,
        Finishing = 7,
        Drier = 8,
        Hydro = 9,
        Delivery = 10,
        ChemicalStore = 11,
        Account = 12,
        Commercial = 13,
        Inventory = 14,

        ReadOnly = 999
    }
    #endregion

    #region Enummeration : EnumActionType
    public enum EnumActionType
    {
        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        Request_For_Revise = 5,
        RequestForProductionPermission = 6,
        UndoRequestForProductionPermission = 7,
        ProductionApproved = 8,
        InProduction = 9,
        ProductionFinished = 10,
        Cancel = 11//It also for NotApprove 

    }
    #endregion

    #region Enummeration : EnumOrderRecapReportFormat
    public enum EnumOrderRecapReportFormat
    {
        Default = 0,
        Formate_1 = 1
    }
    #endregion

    #region Enummeration : EnumAccessoriesType
    public enum EnumClientOperationValueFormat
    {
        [Description("--Select One--")]
        None = 0,
        Default = 1,//order sheet, LCTransfer, Work order normal, piformat, ExportDocSetupFormat, OrderRecapReportFormat, PIOperation
        Format_1 = 2,//order sheet, LCTransfer, ExportDocSetupFormat,Work order normal,piformat, OrderRecapReportFormat
        Format_2 = 3,//order sheet, LCTransfer,Work order normal,piformat
        Format_3 = 4,//order sheet,
        Manual = 5, //use for piformat
        Percent_Base = 6, //Way of LC Tranfer
        FOB_Base =7,  //Way of LC Tranfer
        With_Appllicant_And_DiscountAmount = 8,// PIOperation
        FullValue = 9,//LCTransferType
        PartialValue = 10,//LCTransferType
        Country_Shortname_BuyerShortName_No_Year = 11,//use for piformat ;  
        CountryShortname_BuyerShortName_DAte_Session = 12,//use for piformat ;  
        [Description("Single Currency Voucher")]
        SingleCurrencyVoucher = 13,//use for VoucherFormat;  
        [Description("Multi Currency Voucher")]
        MultiCurrencyVoucher = 14, //use for VoucherFormat;  
        [Description("Buying Format")]
        Buying_Format = 15, //use for Commercial invoice format;  
        [Description("Garments Format")]
        Garments_Format = 16,
        [Description("Stock Report With Qty")]
        StockReportWithQty = 17,//use for Stock Report;  
        [Description("Stock Report With Value")]
        StockReportWithValue = 18, //use for Stock Report;  
        Bijoy = 19,
        Avro = 20,
        TextWise = 21,
        ColumnWise = 22,
        [Description("Manufactureing Format")]
        Manufactureing_Format = 23,
        [Description("Trading Format")]
        Trading_Format = 24,
        English = 25,
        Bangla = 26,
        PIWise = 27,
        WithoutPIWise = 28
    }
    #endregion

    #region Enummeration : SubGender
    public enum EnumSubGender
    {
        None = 0,
        [Description("Boys New Born")]
        BoysNewBorn = 1,
        [Description("Boys Junior")]
        BoysJunior = 2,
        [Description("Girls New Born")]
        GirlsNewBorn = 3,
        [Description("Grirls Junior")]
        GirlsJunior = 4,
        [Description("Mens")]
        Mens = 5,
        [Description("Ladies")]
        Ladies = 6
    }
    #endregion

    #region Enummeration : EnumMonthName
    public enum EnumMonthName
    {
       
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
    #endregion

    #region Enummeration : EnumAccessoriesType
    public enum EnumAccessoriesType
    {
        None = 0,
        Normal = 1,
        Package = 2
    }
    #endregion

    #region Enummeration : EnumReDyeingStatus
    public enum EnumReDyeingStatus
    {
        None = 0,
        [Description("Re-Dyeing")]
        Re_Dyeing = 1,
        [Description("Re-Process")]
        Re_Process = 2
    }
    #endregion

    #region Enummeration : EnumFabricRndShade
    public enum EnumFabricRndShade
    {
        None = 0,
        Reactive = 1,
        Pigment = 2,
        Vat = 3,
        [Description("Reactive+Pigment")]
        Reactive_Pigment = 4
    }
    #endregion

    #region Enummeration : EnumAccessoriesType
    public enum EnumPolyMeasurementType
    {
        None = 0,
        [Description("L x W x T")]
        L_mul_W_mul_T = 1,
        [Description("(L + F) x W x T")]
        L_plus_F_mul_W_mul_T = 2,
        [Description("L x (W + F) x T")]
        L_mul_W_plus_F_mul_T = 3,
        [Description("L x W (G + G) x T")]
        L_mul_W_mul_G_plus_G_mulT = 4,
        [Description("(L + LIP) x W x T")]
        L_plus_LIP_mul_W_mul_T = 5,
        [Description("L + (F + G) x W x T")]
        L_plus_F_plus_G_mul_W_mul_mul_T = 6,
        [Description("L + (LIP + G) x W x T")]
        L_plus_LIP_plus_G_mul_W_mul_T=7,
        [Description("W x T")]
        W_mul_T=8,
        [Description("(L + LIP + G) x W x T")]
        L_plus_LIP_plus_G_mul_W_mul_T_Two =9,
        [Description("L X W")]
        L_Mul_W = 10

    }
    #endregion

    #region Enummeration : EnumMgtDBRefType
    public enum EnumMgtDBRefType
    {
        None = 0,
        [Description("Order Summery")]
        Order_Summery = 1,
        [Description("Top Five Marketing Performance")]
        Top_Five_Marketing_Performance = 2,
        [Description("Highest Produced Product")]
        Highest_Produced_Product = 3,
        [Description("Top Ten Customer")]
        Top_Ten_Customer = 4,
        [Description("Top Five Over Due Customer")]
        Top_Five_Over_Due_Customer = 5,
        [Description("Highest Selling Product")]
        Highest_Selling_Product = 6,
        [Description("ExportPI Issued Vs LCReceived")]
        Export_PI_Issued_Vs_LCReceived = 7,
        [Description("Export Recevable Vs Import Payable")]
        Export_Recevable_Vs_Import_Payable = 8,
        [Description("Stock Summery")]
        Stock_Summery = 9,
        [Description("Attendance Summery")]
        Attendance_Summery = 10
    }
    #endregion

    #region Enummeration : EnumMgtDBRefValueType
    public enum EnumMgtDBRefValueType
    {
        None = 0,
        [Description("SO Qty")]
        SO_Qty = 1,
        [Description("PI Qty")]
        PI_Qty = 2,
        [Description("PO/Bulk Qty")]
        PO_Qty = 3,
        [Description("DO Qty")]
        DO_Qty = 4,
        [Description("Challan Qty")]
        Challan_Qty = 5,
        [Description("Return Qty")]
        Return_Qty = 6,
        [Description("Sample Adj Qty")]
        Sample_Adj_Qty = 7,
        [Description("Claim Qty")]
        Claim_Qty = 8,
        [Description("Import Qty")]
        Import_Qty = 9,
        [Description("Re Dyeing Qty")]
        Re_Dyeing_Qty = 10,
        [Description("Re Cycle Qty")]
        Re_Cycle_Qty = 11
    }
    #endregion

    #region Enummeration : EnumReconcilStatus
    public enum EnumReconcilStatus
    {
        Initialize = 0,
        Approved = 1
    }
    #endregion

    #region Enummeration : EnumImportLetter
    public enum EnumImportLetterType
    {
        None = 0,
         [Description("L/C Opening Request")]
        LCOpeningRequest = 1,
        [Description("L/C Amendment Request")]
        LC_AmendmentRequest = 2,
        [Description("L/C Cancelation")]
        LC_Cancelation = 3,
          [Description("DOC Release")]
        DOC_Release = 4,
        [Description("Invoice Acceptance")]
        Invoice_Acceptance = 5,
        [Description("Invoice Settlement")]
        Invoice_Settlement = 6,
        [Description("CnF Letter")]
        CnfLetter = 7,
        [Description("L/C Partial Cancelation")]
        LC_Partial_Cancelation = 8,
        [Description("Import Claim Report")]
        Import_Claim_Report = 9,
        [Description("Export L/C Incentive")]
        ExportLCIncentive = 20,
        [Description("Export L/C Incentive Bank")]
        ExportLCIncentiveFord = 21,
        [Description("Export Bill Discount")]
        ExportBillDiscount = 22,
    }
    #endregion

    #region Enummeration : EnumImportLetter
    public enum EnumImportLetterIssueTo
    {
        None = 0,
        Bank = 1,
        Supplier = 2,
        IssueBank = 3
       
    }
    #endregion

    #region Enummeration : EumDyedType
    public enum EnumDyedType
    {
        [Description("--Select One--")]
        None = 0,
        [Description("Solid Dyed")]
        SolidDyed = 1,
        [Description("Yarn Dyed")]
        YarnDyed = 2

    }
    #endregion


    #region Enummeration : EumProgramType
    public enum EnumKnitDyeingProgramRefType
    {        
        Open = 0,
        OrderRecap = 1,
        PAM = 2

    }
    #endregion
    #region Enummeration : EumProgramType
    public enum EnumProgramType
    {

        None = 0,
        Sample = 1,
        Bulk= 2

    }
    #endregion
    #region Enummeration : EnumOperationType
    public enum EnumOperationType
    {
        [Description("--Select One--")]
        None = 0,
        [Description("OrderSheet Responsible Person")]
        OrderSheet_Responsible_Person = 1,
        [Description("OrderSheet Shipment Date Count")]
        OrderSheet_ShipmentDate_Count = 2,
        [Description("OrderSheet Preview Format")]
        OrderSheet_PreviewFormat = 3,
        [Description("LC Transfer Preview Format")]
        LCTransfer_PreviewFormat = 4,
        [Description("Work Order Normal Format")]
        WorkOrder_Normal_Format = 5,
        [Description("MD Name")]
        MDName = 6,
        [Description("Chairman Name")]
        ChairmanName = 7,
        [Description("Export PI Format")]
        PIFormat = 8,
        [Description("Way Of LC Transfer")]
        Way_Of_LCTransfer = 9,
        [Description("Export Doc Setup Format")]
        Export_Doc_Setup_Format = 10,
        [Description("OrderRecap Report Format")]
        OrderRecap_Report_Format = 11,
        [Description("Is Print With Padding")]
        IsPrintWithPadding = 12,
        [Description("Commercial Manager")]
        CommercialManager = 13,
        [Description("Export PI Operation")]
        PIOperation = 14,
        [Description("LC Transfer Type")]
        LCTransferType = 15,
        [Description("Voucher Format")]
        VoucherFormat = 16,
        [Description("Logo Print In Voucher")]
        LogoPrintInVoucher = 17,
        [Description("Commercial Invoice Format")]
        CommercialInvoiceFormat = 18,
        [Description("Stock Report Format")]
        StockReportFormat = 19,
        [Description("Is Procurement with Style No")]
        IsProcurementwithStyleNo = 20,
        [Description("Purchase Order Report Format")]
        PurchaseOrderReportFormat = 21,
        [Description("Purchase Requisition Report Format")]
        PurchaseRequisitionReportFormat = 22,
        [Description("Operation With Business Unit")]
        IsOperationWithBusinessUnit = 23,
        [Description("NOA Report Format")]
        NOAReportFormat = 24,
        [Description("Is Style With Product Wise Lot Suggest")]
        IsStyleWithProductWiseLotSuggest = 25,
        [Description("NOA Operation Name")]
        NOAOperationType = 26,
        [Description("Is Product Code Are Manual")]
        IsProductCodeManual = 27,
        [Description("User Inactive with Employee")]
        IsUserInactivewithEmployee = 28,
        [Description("Is Landing CostHead Ledger")]
        IsLandingCostHeadLedger = 29,
        [Description("Bangla Font")]
        BanglaFont = 30,
        [Description("Sub Ledger Report Format")]
        Sub_Ledger_Report_Format = 31,
        [Description("Comprehensive Income Satatement Format")]
        Comprehensive_Income_Satatement_Format = 32,
        [Description("Approve Leave After Save")]
        Approve_Leave_After_Save = 33,
        [Description("Bangla English")]
        BanglaOrEnglish = 34,
        [Description("System Sending Mail Address")]
        SystemSendingmailAddress = 35,
        [Description("Plastic Re-Cycle Cutting Item Price")]
        Plastic_Re_Cycle_Cutting_Item_Price = 36,
        [Description("Commercial Module Using System")]
        Commercial_Module_Using_System = 37,

        [Description("Customer Service Email Body")]
        CustomerServiceEmailBody = 38,

        [Description("Inactive Employee apply on Salary Process")]
        Inactive_Employee_Apply_On_SalaryProcess = 39,

        [Description("Month Cycle")]
        Month_Cycle = 40
    }
    #endregion
    
    #region Enummeration : EnumDataType
    public enum EnumDataType
    {
        None = 0,
        Text = 1,
        Number = 2,
        Date = 3,
        Boolean = 4,
        Enum = 5
    }
    #endregion


    #region Enummeration : EnumApplyOn
    public enum EnumApplyOn
    {
        None = 0,
        Both = 1,
        Actual = 2,
        Compliance = 3
    }
    #endregion

    #region Enummeration : EnumApplyMonthCycle
    public enum EnumApplyMonthCycle
    {
        None = 0,
         [Description("Fixed 30 days")]
        Fixed_30_Days = 1,
         [Description("Depends On Month")]
        Depends_On_Month = 2
    }
    #endregion

    
    #region Enummeration : EnumCSOperationType
    public enum EnumStyleBudgetType
    {
        None = 0,
        PreBudget = 1,
        PostBudget = 2
    }
    #endregion
    
    #region Enummeration : EnumPaymentTerm
    public enum EnumPaymentTerm
    {
        None = 0,
        [Description("LC At Sight")]
        LC_At_Sight = 1,
        [Description("LC Deferred 30 days")]
        LC_Deferred_30_days = 2,
        [Description("LC Deferred 45 Days")]
        LC_Deferred_45_Days = 3,
        [Description("LC Depard 60 Days")]
        LC_Deferred_60_Days = 4,
        [Description("LC Depard 90 Days")]
        LC_Deferred_90_Days = 5,
        [Description("LC Depard 120 Days")]
        LC_Deferred_120_Days = 6,
        [Description("TT_At_Sight")]
        TT_At_Sight = 7,
        [Description("TT Advance 35%, 65% After Shipment")]
        TT_Advance_35Percent_65Percent_After_Shipment = 8,
        [Description("TT Advance 40%, 60% After Shipment")]
        TT_Advance_40Percent_60Percent_After_Shipment = 9,
        [Description("TT Advance 45%, 55% After Shipment")]
        TT_Advance_45Percent_55Percent_After_Shipment = 10,
        [Description("TT Advance 50%, 50% After Shipment")]
        TT_Advance_50Percent_50Percent_After_Shipment = 11,
        [Description("TT Advance 60%, 40% After Shipment")]
        TT_Advance_60Percent_40Percent_After_Shipment = 12,
        [Description("TT Advance 70%, 30% After Shipment")]
        TT_Advance_70Percent_30Percent_After_Shipment = 13,
        [Description("LC_At_Sight")]
        LC_Deferred_15_days = 14
    }
    #endregion

    #region Enummeration : EnumProformaInvoiceActionType
    public enum EnumProformaInvoiceActionType
    {

        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        PI_In_Buyer_Hand = 5,
        Attach_With_LC = 6,
        RequestForRevise = 7,
        Cancel = 8
    }
    #endregion

    #region Enummeration : EnumDeliveryTerm
    public enum EnumDeliveryTerm
    {
        None = 0,
        FOB = 1,
        FCA = 2,
        CNF = 3,
        FCR = 4,
        CIF = 5
    }
    #endregion

    #region Enummeration : EnumDevelopmentRecapActionType
    public enum EnumDevelopmentRecapActionType
    {
        None = 0,
        RequestForApproved = 1,
        UndoRequest = 2,
        ApprovedDone = 3,
        UndoApprove = 4,
        Inproduction = 5, //Send to Factory
        RawMaterialCollectionDone = 6,
        QCDone = 7,
        ReceivefromFactory = 8,
        SendtoBuyer = 9,
        Feedbackfrombuyer = 10,
        OrderConfirmation = 11,
        Cancel = 12

    }
    #endregion

    #region Enummeration : ImportPIType
    public enum EnumLCAppType
    {
        None = 0,
        [Description("L/C")]
        LC = 1,
        [Description("Back To Back")]
        B2BLC = 2,
        [Description("TT")]
        TT = 3
    }
    #endregion

    #region Enummeration : ReportType
    public enum EnumReportType
    {
        [Description("Order Tracking Color View")]
        Order_Tracking_ColorView = 0,
        [Description("Order Tracking PI View")]
        Order_Tracking_PIView = 1,
        [Description("Order Tracking Product View")]
        Order_Tracking_ProductView = 2,
        [Description("Sale Invoice Report")]
        Sale_Invoice_Report = 3,
        [Description("Sales Quotation Report")]
        Sales_Quotation_Report = 4,
        [Description("FN Machine")]
        FNMachine = 5
    }
    #endregion

    #region Enummeration : UploadType
    public enum EnumUploadType
    {
        [Description("Employee Basic Upload")]
        EmployeeBasicUpload = 1
    }
    #endregion

    #region Enummeration : EnumLCStatus
    public enum EnumLCStatus
    {
        None = 0,
        Initilaized = 1,
        Req_For_App = 2,
        Approved = 3,
        Req_for_Ammendment = 4,
        Cancel = 5
    }
    #endregion
    #region Enummeration : EnumPartialShipmentAllow
    public enum EnumPartialShipmentAllow
    {
        None = 0,
        Allow = 1,
        NotAllow = 2

    }
    #endregion

    #region Enummeration : EnumTransferable
    public enum EnumTransferable
    {
        None = 0,
        Allow = 1,
        NotAllow = 2

    }
    #endregion
    #region Enummeration : EnumDefferedFrom
    public enum EnumDefferedFrom
    {
        None = 0,
        Doc_Submit_Date = 1,
        Delivery_Date = 2,
        Bill_of_exchange = 3,
        Date_Of_Acceptance = 4//requirement of umor faruk

    }
    #endregion
    #region Enummeration : EnumMasterLCActionType
    public enum EnumMasterLCActionType
    {
        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        Req_for_Ammendment = 5,
        Cancel = 6
    }
    #endregion

    #region EnumERPOperationType
    public enum EnumERPOperationType
    {
        None = 0,
        ProformaInvoice = 1,
        MasterLC = 2,
        LCTransfer = 3,
        CommercialInvoice = 4,
        SalesContract = 5,
        B2BLCAllocation = 6,
        PurchasePI = 7,
        B2BLC = 8,
        B2B_CommerciaInvoice = 9,
        InvoicePurchase = 10
    }

    #endregion
    #region Enummeration : EnumLCTransferStatus
    public enum EnumLCTransferStatus
    {
        None = 0,
        Initialize = 1,
        Approve = 2,
        Request_for_Revise = 3
    }
    #endregion
    #region Enummeration : EnumLCTranferActionType
    public enum EnumLCTranferActionType
    {

        None = 0,
        Approve = 2,
        Req_for_Revise = 3

    }
    #endregion

    #region Enummeration : LoanType
    public enum EnumLoanType
    {
        None = 0,
        ReducingMethod = 1,
        StateLineMethod = 2
    }
    #endregion

    #region Enummeration :Enum EnumSettleMentType
    public enum EnumSettleMentType
    {
        None = 0,
        Resignation = 1,
        Dismisal = 2,
        Termination = 3,
        Retirement = 4,
        Contract_completion = 5,
        Abs_Cont = 6
    }
    #endregion

    #region Enummeration : EnumESCrearance
    public enum EnumESCrearance
    {
        None = 0,
        Send = 1,
        Accept = 2,
        Reject = 3,
        Hold = 4
    }
    #endregion

    #region Enummeration :Enum EnumDayOffType
    public enum EnumDayOffType
    {
        None = 0,
        Continous = 1,
        Alternate = 2,
        HalfDay = 3,
        Random = 4
    }
    #endregion

    #region Enummeration : EnumSalarySheetFormatProperty

    #region Enummeration : RequestStatus
    public enum EnumRequestStatus
    {
        None = 0,
        Request = 1,
        Approve = 2,
        Cancel = 3,
        Finish = 4
    }
    #endregion
    public enum EnumSalarySheetFormatProperty
    {
        None = 0,
        [Description("EmployeeInformation")]
        EmployeeCode = 100,
        [Description("EmployeeInformation")]
        EmployeeName = 110,
        [Description("EmployeeInformation")]
        ParentDepartment = 120,
        [Description("EmployeeInformation")]
        Department = 130,
        [Description("EmployeeInformation")]
        Designation = 140,
        [Description("EmployeeInformation")]
        JoiningDate = 150,
        [Description("EmployeeInformation")]
        ConfirmationDate = 160,
        [Description("EmployeeInformation")]
        EmployeeType = 170,
        [Description("EmployeeInformation")]
        Gender = 180,
        [Description("EmployeeInformation")]
        EmpGroup = 181,
        [Description("EmployeeInformation")]
        PaymentType = 182,
        [Description("EmployeeInformation")]
        EmployeeContactNo = 190,



        [Description("Att.Detail")]
        TotalDays = 300,
        [Description("Att.Detail")]
        PresentDay = 310,
        [Description("Att.Detail")]
        DayOffHolidays = 320,
        [Description("Att.Detail")]
        AbsentDays = 330,
        [Description("Att.Detail")]
        LeaveHead = 340,
        [Description("Att.Detail")]
        LeaveDays = 350,
        [Description("Att.Detail")]
        EmployeeWorkingDays = 353,
        [Description("Att.Detail")]
        EarlyOutDays = 360,
        [Description("Att.Detail")]
        EarlyOutMins = 370,
        [Description("Att.Detail")]
        LateDays = 380,
        [Description("Att.Detail")]
        LateHrs = 382,
        [Description("Att.Detail")]
        OTHours = 390,
        [Description("Att.Detail")]
        OTRate = 400,
        [Description("Att.Detail")]
        OTAllowance = 410,

        [Description("Increment Detail")]
        LastGross = 600,
        [Description("Increment Detail")]
        LastIncrement = 610,
        [Description("Increment Detail")]
        IncrementEffectDate = 620,

        [Description("Bank Detail")]
        BankAmount = 700,
        [Description("Bank Detail")]
        CashAmount = 710,
        [Description("Bank Detail")]
        AccountNo = 720,
        [Description("Bank Detail")]
        BankName = 730
    }

    #region Enummeration : EnumEmployeeCategory
    public enum EnumEmployeeCategory
    {
        None = 0,
        Probationary = 1,
        Permanent = 2,
        Contractual = 3,
        Seasonal = 4
    }
    #endregion

    #region Enummeration : EnumEmployeeType
    public enum EnumEmployeeType
    {
        None = 0,
        Staff=1,
        Worker=2
    }
    #endregion
    #endregion

    #region Enummeration : EnumInvoiceStatus
    public enum EnumCommercialInvoiceStatus
    {
        Initialized = 0,
        Approve = 1
    }
    #endregion
    #region Enummeration : EnumCommercialBSActionType
    public enum EnumCommercialBSActionType
    {
        Intialized = 0,
        Approved = 1,
        SubmitToBank = 2,
        FDBPRcv = 3,
        MaturityRcv = 4, 
        DocRialization = 5, 
        Encashment = 6,
        BillClosed = 7,
        BillCancel = 8,
        Undo = 9,
        PurchaseApprove = 10,
        EnCashApprove = 11
    }
    #endregion

    #region Enummeration : EnumCommercialBSStatus
    public enum EnumCommercialBSStatus
    {
        Intialized = 0,
        Approved = 1,
        SubmitToBank = 2,
        FDBPRcv = 3,
        MaturityRcv = 4,
        DocRialization = 5,
        Encashment = 6,
        BillClosed = 7,
        BillCancel = 8,
        Undo = 9
    }
    #endregion

    #region EnumCostSheetType
    public enum EnumCostSheetType
    {
        Sweater = 0,
        Knit = 1,
        Woven = 2
    }
    #endregion


    #region Enummeration : EnumCostSheetStaus
    public enum EnumCostSheetStatus
    {
        None = 0,
        Initialized = 1,
        [Description("Request For Approve")]
        Req_For_App = 2,
        Approved = 3,
        [Description("Request For Revise")]
        RequestForRevise = 4,
        Freeze = 5,
        Cancel = 6
    }
    #endregion


    #region Enummeration : EnumStyleBudgetStatus
    public enum EnumStyleBudgetStatus
    {
        None = 0,
        Initialized = 1,
        [Description("Request For Approve")]
        Req_For_App = 2,
        Approved = 3,
        [Description("Request For Revise")]
        RequestForRevise = 4,
        Freeze = 5,
        Cancel = 6
    }
    #endregion

    #region Enumeration : EnumCostSheetMeterialType
    public enum EnumCostSheetMeterialType
    {
        None = 0,
        Yarn = 1,
        Accessories = 2,
        [Description("Production Step")]//Material Id will be ProductionStep ID
        Production_Step = 3
    }
    #endregion
    #region Enummeration : EnumCostSheetActionType
    public enum EnumCostSheetActionType
    {
        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        Request_revise = 5,
        Freeze = 6,
        Cancel = 7
    }
    #endregion
    #region Enummeration : EnumCostSheetActionType
    public enum EnumStyleBudgetActionType
    {
        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        Request_revise = 5,
        Freeze = 6,
        Cancel = 7
    }
    #endregion

    #region Enummeration : EnumExpenditureType
    public enum EnumExpenditureType
    {
        None = 0,
        ExportLC = 1,
        ExportBill = 2,
        ExportBill_Encash = 3,
        ImportLC = 20,
        ImportInvoice = 21
    }
    #endregion

    #region Enummeration : EnumFabricFaultType
    public enum EnumFabricFaultPoint
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
    }
    #endregion

    #region Enummeration : EnumFBQCGrade
    public enum EnumFBQCGrade
    {
        None = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        Reject = 5,
        [Description("Cut Piece")]
        CutPiece = 6
    }

    #endregion

    #region Enummeration : EnumFNBatchStatus
    public enum EnumFNBatchStatus
    {
        None = 0,
        InFloor = 1,
        Singening = 2,
        Washing = 3,
        Mercerizing = 4,
        Dyeing = 5,
        Stenter = 6,
        Sanforize = 7,
        Peaching = 8,
        InQC = 9,
        QcDone = 10,
        PartiallyInDeliveryStore = 11,
        FullyInDeliveryStore = 12,
        Finish = 13
    }
    #endregion

    #region Enummeration : EnumShade
    public enum EnumFNShade
    {
        NA = 0,
        A = 1,
        A1 = 2,
        A2 = 3,
        A3 = 4,
        B = 5,
        B1 = 6,
        B2 = 7,
        B3 = 8,
        C = 9,
        C1 = 10,
        C2 = 11,
        C3 = 12,
        D = 13,
        D1 = 14,
        D2 = 15,
        D3 = 16,
        A4 = 17,
        A5 = 18,
        A6 = 19,
        B4 = 20,
        B5 = 21,
        B6 = 22,
        C4 = 23,
        C5 = 24,
        C6 = 25,
        D4 = 26,
        D5 = 27,
        D6 = 28,
        E = 29,
        F = 30,
        G = 31,
        H = 32,
        I = 33,
        J = 34,
        K = 35,
        L = 36,
        M = 37,
        N = 38,
        O = 39,
        P = 40,
        Q = 41,
        R = 42,
        S = 43,
        T = 44,
        U = 45,
        V = 46,
        W = 47,
        X = 48,
        Y = 49,
        Z = 50

    }
    #endregion

    #region Enummeration : EnumFabricFaultType
    public enum EnumFabricFaultType
    {
        None = 0,
        DyeingFault = 1,
        YarnFault = 2,
        WeaveFault = 3,
        Mechanical = 4,
        Electrical = 5,
        Production = 6,
        Quality = 7,
        Preparatory = 8,
        Pretreatment =9,
        Printing =10,
        Finishing=11 ,
        SpinningFault=12 
    }
    #endregion

    #region EnumMailPurpose
    public enum EnumTAPActionType
    {
        None = 0, Approve = 3, UndoApprove = 4, RequestForRevise = 5
    }
    #endregion

    #region EnumColorType
    public enum EnumColorType
    {
        None = 0,
        ExteriorColor = 1, 
        InteriorColor = 2, 
        Upholstery = 3,
        CommonColor = 4
    }
    #endregion
    #region EnumServiceOrderStatus
    public enum EnumServiceOrderStatus
    {
        Initialize = 0,
        Approved = 1,
        Received= 2,
        Done = 3,
        Canceled = 4
    }
    #endregion
    #region EnumServiceInvoiceStatus
    public enum EnumServiceInvoiceStatus
    {
        None = 0,
        Initialize = 1,
        Approved = 2
    }
    #endregion
      #region EnumServiceInvoiceStatus
    public enum EnumSaleInvoiceStatus
    {
        None = 0,
        Initialize = 1,
        Approved = 2
    }
    #endregion

    #region EnumPreInvoiceStatus
    public enum EnumPreInvoiceStatus
    {
        None = 0,
        Initialize = 1,
        Approved = 2
    }
    #endregion
    
    #region EnumServiceType
    public enum EnumServiceType
    {
        None = 0,
        RegularService = 1,
        ExtraService = 2,
        LaborCharge = 3
    }
    #endregion
    #region EnumServiceILaborChargeType
    public enum EnumServiceILaborChargeType
    {
        None = 0,
        Paying = 1,
        Complementary = 2,
        [Description("Gift & Tips")]
        GiftAndTips = 3,
        Free = 4,
        [Description("Updateing Cost")]
        UpdateingCost = 5,
        [Description("Repairing & Maintenance")]
        RepairingAndMaintenance = 6,
        [Description("Warranty")]
        Warranty  = 7
    }
    #endregion

    #region EnumCutOffType
    public enum EnumCutOffType
    {
        None = 0,
       [Description("1st")]
        First = 1,
        [Description("2nd")]
        Second = 2,
        [Description("3rd")]
        Third = 3
    }
    #endregion

    #region EnumPartsType
    public enum EnumPartsType
    {
        None = 0,
        Trim = 1,
        Wheels = 2
    }
    #endregion

    #region EnumPRequisutionType
    public enum EnumPRequisutionType
    {
        Open = 1,
        [Description ("Service Order")]
        Service_Order = 2
    }
    #endregion

    #region EnumMailPurpose
    public enum EnumTAPStatus
    {
        Initialize = 0,
        Approved = 2,
        Request_for_Revise = 3
    }
    #endregion

    #region Enummeration : EnumRequiredDataType
    public enum EnumRequiredDataType
    {
        Text = 0,
        Number = 1, // 
        Date = 2
    }

    #endregion
    #region Enummeration : EnumCalculationType
    public enum EnumCalculationType
    {
        Days = 0,
        Percentage = 1
    }
    #endregion

    #region Enummeration : EnumLSalesCommissionStatus
    public enum EnumLSalesCommissionStatus
    {
        [Description("Initialized")]
        Initialize = 1,
        [Description("Request For Approve")]
        RequestForApprove = 2,
        [Description("Approve")]
        Approve = 3,
        [Description("Payable")]
        Payable = 4,
        [Description(" Paid Partially")]
        PaidPartially = 5,
        [Description("Paid")]
        Paid = 6,
        [Description("Cancel")]
        Cancel = 7
    }
    #endregion



    #region Enummeration : EnumFeatureType
    public enum EnumFeatureType
    {
         
        None = 0,
        [Description("standard Feature")]
        StandardFeature = 1,
        [Description("Interior Feature")]
        InteriorFeature = 2,
        [Description("Exterior Feature")]
        ExteriorFeature = 3,
        [Description("Safety Feature")]
        SafetyFeature = 4,
        [Description("Country Setting Feature")]
        CountrySettingFeature = 5,
        [Description("Optional Feature")]
        OptionalFeature = 6

    }
    #endregion


    #region Enummeration : OrderStatus
    public enum EnumVOStatus
    {
         
        None = 0,
        Initialize = 1,
        [Description("Receive Komm File")]
        Receive_Komm_File = 2,
        [Description("PI Received")]
        PI_Received = 3,
        [Description("LC Open")]
        LC_Open = 4,
        [Description("Shipment Done")]
        Shipment_Done = 5,
        [Description("Receive In Port")]
        Receive_In_Port = 6,
        [Description("Display In Show Room")]
        Display_In_Show_Room = 7,
        Sold = 8,
       [Description("Deliver Done")]
        Deliver_Done = 9

    }
    #endregion

    #region Enummeration : ServiceOrderType
    public enum EnumServiceOrderType
    {
        None = 0,
        [Description("Regular Service")]
        Regular_Service = 1,
        [Description("On Request Service")]
        On_Request_Scheduled = 2,
        [Description("After Sale Service")]
        After_Sale_Service=3
    }
    #endregion

    #region Enummeration : ServiceInvoiceType
    public enum EnumServiceInvoiceType
    {
        None = 0,
        Open = 1, 
        [Description("Service Order")]
        Service_Order = 2
    }
    #endregion
    #region Enummeration : EnumInvoiceType
    public enum EnumInvoiceType
    {
        ServiceInvoice = 0,
        EstimatedInvoice = 1
    }
    #endregion
    
    #region Enummeration : EnumQuotationType
    public enum EnumQuotationType
    {
        [Description("Stock Item")]
        Stock_Item = 0,
        [Description("New Item")]
        New_Item = 1

    }
    #endregion

    #region Enummeration : EnumVehicleLocation
    public enum EnumVehicleLocation
    {
        [Description("Factory")]
        Factory = 1,
        [Description("In-transit")]
        In_transit = 2,
        [Description("Showroom")]
        Showroom = 3,
        [Description("In Production")]
        In_Production = 4
    }
    #endregion

    #region Enummeration : EnumKommFileStatus
    public enum EnumKommFileStatus
    {
        None = 0,
        [Description("In Production")]
        In_Production = 1,
        [Description("In Transit")]
        In_Transit = 2,
        [Description("In Stock")]
        In_Stock = 3,
        Delivered = 4
    }
    #endregion

    #region Enummeration : EumDyeingType
    public enum EumDyeingType
    {
        [Description("--Select One--")]
        None=0,
        [Description("Hank")]
        Hank = 1,
        [Description("Cone")]
        Cone = 2,
        [Description("Pack")]
        Pack = 3,
        [Description("SpaceDyeing")]
        SpaceDyeing = 4,
        [Description("Stone Wash")]
        StoneWash = 5,
         [Description("Cabinet Dyeing")]
        CabinetDyeing = 6,
         [Description("Piece")]
         Piece =7,

    }
    #endregion


    #region Enummeration : EnumUPStatus
    public enum EnumUPStatus
    {
        WaitingForUD = 0,
        UDReceive = 1,
        Processing = 2,
        Ready = 3,
        Delivered = 4,
        NotNeed = 5,
        Cancel = 6
    }
    #endregion
    #region Enummeration : EnumUDRcvType
    public enum EnumUDRcvType
    {
        [Description("No Receive")]
        No_Receive = 0,  
        [Description("Partily Receive")]
        Partily_Receive = 1,
        [Description("Full Receove")]
        Full_Receove = 2
    }
    #endregion
    #region Enummeration : EnumPriortyLevel
    public enum EnumPriortyLevel
    {
        None = 0,
        Normal = 1,
        Urgent = 2,
        [Description("Top Urgent")]
        TopUrgent = 3
    }
    #endregion
    #region Enummeration : EnumExpenditureHeadType
    public enum EnumExpenditureHeadType
    {
        None = 0,
        Ledger = 1,
        SubLedger = 2
   }
    #endregion

    #region Enummeration : EnumProductionOrderStatus
    public enum EnumGUProductionOrderStatus
    {
        None = 0,
        Initialized = 1,
        Req_For_Approval = 2,
        Approved = 3,
        InProduction = 4,
        ProductionDone = 5,
        InActive = 6,
        Cancel = 7
    }
    #endregion
    #region Enummeration : EnumOPOActionType
    public enum EnumOPOActionType
    {

        None = 0,
        RequestForApproval = 1,
        UndoRequest = 2,
        Approve = 3,
        UndoApprove = 4,
        InProduction = 5,
        ProductionDone = 6,
        InActive = 7,
        Cancel = 8,//It also for NotApprove 
        Active = 9

    }
    #endregion
        
    #region Enummeration : EnumSalaryType
    public enum EnumSalaryType
    {
        Both = 1,   
        Actual = 2,
        Compliance = 3
    }
    #endregion

    #region Enummeration : EnumQCDataType
    public enum EnumQCDataType
    {
        Text = 0,
        Number = 1,
        Date = 2,
        Boolean = 3,
        Production = 4

    }
    #endregion

    #region Enummeration : EnumWeekDays
    public enum EnumWeekDays
    {
        None = 0,
        SuterDay = 1,
        SunDay = 2,
        MonDay = 3,
        TuesDay = 4,
        WednesDay = 5,
        ThursDay =6,
        FriDay = 7
    }
    #endregion
     #region Enummeration : EnumWeekDays
    public enum EnumPlanStatus
    {
        None = 0,
        Initialize = 1,
        Approved = 2,
        Requst_For_Revise = 3
        
    }
    #endregion

    #region Enummeration : EnumShift
    public enum EnumShift
    {
        None = 0,
        Day = 1,
        Night = 2,
        [Description("Sample(R&D)")]
        Sample_RnD = 3,
        ETP = 4,
        A = 5,
        B = 6,
        C = 7
    }
    #endregion

    #region Enummeration : EnumInterestType
    public enum EnumInterestType
    {
        [Description("---Loan Type---")]
        None = 0,
        [Description("Regular")]
        Regular = 1,
        [Description("Over Due")]
        OverDue = 2
    }
    #endregion

    #region Enummeration : EnumFinanceLoanType
    public enum EnumFinanceLoanType
    {
        [Description("---Loan Type---")]
        None = 0,
        [Description("PAD Loan")]
        PADLoan = 1,
        [Description("EDF Loan")]
        EDFLoan = 2,
        [Description("PC Loan")]
        PCLoan  = 3,
        [Description("LTR Loan")]
        LTRLoan = 4,
        [Description("Term Loan")]
        TermLoan = 5,
        [Description("U-Pass")]
        U_Pass = 6
    }
    #endregion

    #region Enummeration : EnumFinanceLoanRefType
    public enum EnumLoanRefType
    {
        [Description("---Loan Ref---")]
        None = 0,
        [Description("Import Invoice")]
        ImportInvoice = 1,
        [Description("Export LC")]
        ExportLC = 2
    }
    #endregion

    #region Enummeration : EnumLoanCompoundType
    public enum EnumLoanCompoundType
    {
        [Description("---Compound Type---")]
        None = 0,
        [Description("Daily")]
        Daily = 1,
        [Description("Monthly ")]
        Monthly = 2,
        [Description("Quarterly")]
        Quarterly = 3,
        [Description("Half Yearly")]
        HalfYearly = 4
    }
    #endregion

    #region Enummeration : EnumCycleType
    public enum EnumCycleType
    {
        [Description("---Cycle Type---")]
        None = 0,
        [Description("Daily")]
        Daily = 1,
        [Description("Monthly ")]
        Monthly = 2,
        [Description("Quarterly")]
        Quarterly = 3
    }
    #endregion

    #region Enummeration : EnumLoanTransfer
    public enum EnumLoanTransfer
    {
        [Description("---Select Loan Transfer---")]
        None = 0,
        [Description("PAD TO EDF")]
        PADTOEDF = 1,
        [Description("Regular To Over Due")]
        Regular_To_OverDue = 2
    }
    #endregion

    #region Enummeration : EnumStatementType
    public enum EnumStatementType
    {
        None = 0,
        ProductionOrder = 1,
        DeliveryOrder = 2,
        Invoice = 3,
        PI = 4,
        LC = 5,

    }
    #endregion

    #region Enummeration : EnumFPReportSetUpType
    public enum EnumFPReportSetUpType
    {
        None = 0,
        [Description("A/C Reciveable")]
        Account_Reciveable = 1,
        [Description("Bill Reciveable")]
        Bill_Reciveable = 2,
        [Description("FC Margin A/C")]
        FC_Margin_Account = 3,
        [Description("FC ERQ A/C")]
        FC_ERQ_Account = 4,
        [Description("FDR A/C")]
        FDR_Account = 5,
        [Description("Bills Payble-BTB")]
        Bills_Payble_BTB = 6,
        [Description("A/C Payble-BTB")]
        Account_Payble_BTB = 7,
        [Description("Short Term-Loan")]
        Short_Term_Loan = 8,
        [Description("Long Term-Loan")]
        Long_Term_Loan = 9
    }
    #endregion

    #region Enummeration : EnumFPReportSubSetup
    public enum EnumFPReportSubSetup
    {
        None = 0,
        [Description("Packing Credit")]
        Packing_Credit = 1,
        [Description("Cash Incentive")]
        Cash_Incentive= 2,
        [Description("LATR/EDF")]
        LATR_OR_EDF = 3,
        [Description("Cash Credit")]
        Cash_Credit = 4,
        [Description("FDBP/LDBP")]
        FDBP_OR_LDBP = 5,
        [Description("LTL-Local")]
        LTL_Local = 6,
        [Description("LTL-OBU")]
        LTL_OBU = 7
     
    }
    #endregion

    #region Enummeration : EnumFuelStatus
    public enum EnumFuelStatus
    {
        None = 0,
        E = 1,
        [Description("1/4")]
        OneByFour = 2,
        [Description("1/2")]
        OneByTwo = 3,
        [Description("3/4")]
        ThreeByFour = 4,
        F = 5
    }
    #endregion
    #region EnumNotifyBy : EnumNotifyBy
    public enum EnumExportGoodsDesViewType
    {
        None = 0,
        [Description("Qty")]
        Qty = 1,
        [Description("Gross Net Weight")]
        Qty_Gross_NetWeight = 2,
        [Description("Qty_Amount")]
        Qty_Amount = 3,
        [Description("Qty_UP_ Amount Bag")]
        Qty_UP_Ampunt_Bag = 4,
         [Description("Qty_UP_Amount")]
        Qty_UP_Ampunt = 5,
       
    }
    #endregion

    #region EnumBankType : EnumBankType
    public enum EnumBankType
    {
        None = 0,
        IssueBank = 1,
        Nego_Bank = 2,
        Forwarding_Bank  = 3,
        Endorse_Bank=4
    }
    #endregion
    //EnumLandingCostType
    #region Enummeration : EnumLandingCostType
    public enum EnumLandingCostType
    {
        Ledger = 0,
        SubLedger = 1
    }
    #endregion

    #region Enummeration : EnumLandingCostType
    public enum EnumLDStatus
    {
        WaitingForIssue = 0,
        Issued = 1,
        InLab = 2,
        Submit = 3,
        Approved = 4,
    }
    #endregion
    #region Enummeration : EnumBeamType
    public enum EnumBeamType
    {
        None = 0,
        UpperBeam = 1,
        LowerBeam = 2
    }
    #endregion
    #region Enummeration : EnumFabricSpeType
    public enum EnumFabricSpeType
    {
        General= 0,
        SeerSucker = 1,
        InjectedSlub = 2,
    }
    #endregion
    #region Enummeration : EnumFabricSpeType
    public enum EnumDispoProType
    {
        None = 0,
        General = 1,
        ExcessFabric = 2,
        ExcessDyeingQty = 3,
        OutSide= 4

    }
    #endregion
    #region Enummeration : WeavingProcess
    public enum EnumWeavingProcess
    {
        Warping = 0, Sizing = 1, Drawing_IN = 2, Loom = 3, Leasing_IN = 4
    }
    #endregion
    #region Enummeration : MachineStatus
    public enum EnumMachineStatus
    {
        Planning = 0, Running = 1, Break = 2, Free = 3, Hold = 4
    }
    #endregion

    #region Enummeration : EnumTextileUnit
    public enum EnumTextileUnit
    {
        None = 0,
        Spinning = 1,
        Weaving = 2,
        Dyeing = 3,
        Finishing = 4
    }
    #endregion

    #region Enummeration : EnumRatioComponent
    public enum EnumRatioComponent
    {
        None = 0,
        Revenue = 1,
        GrossProfit = 2,
        OperatingProfit = 3,
        NetProfit = 4
    }
    #endregion

    #region Enummeration : EnumRatioSetupType
    public enum EnumRatioSetupType
    {
        [Description("Genreal Setup")]
        GenrealSetup = 0,
        [Description("Based On Statement")]
        BasedOnStatement = 1
    }
    #endregion

    #region Enummeration : EnumDeliveryChallan
    public enum EnumDeliveryChallan
    {
        Initialize = 0,
        Approve = 1,
        Disburse = 2,
        Cancel = 3
    }
    #endregion
    #region Enummeration : EnumLoanStatus
    public enum EnumLoanStatus
    {
        None = 0,
        Initialize = 1,
        Approve = 2,
        Receive = 3,
        [Description("Partial Settled")]
        PartialSettled = 4,
        Settled = 5,
        Cancel = 6
    }
    #endregion
    #region Enummeration : EnumSCDetailType
    public enum EnumSCDetailType
    {
        None = 0,//General
        AddCharge = 1,
        DeductCharge = 2,
        ExtraOrder = 3,
        //Cancel = 6
    }
    #endregion
    #region Enummeration : EnumImportClaimSettleType
    public enum EnumImportClaimSettleType
    {
        None = 0,//General
        ByNextInvoiceAdjust = 1,
        ByCashPayment = 2,
    }
    
    #endregion
    #region Enummeration : EnumExportLCType
    public enum EnumExportLCType
    {
        None = 0,
        LC = 1,
        FDD = 2,
        TT = 3
    }
    #endregion

    #region Enummeration : EnumDOPriceType
    public enum EnumDOPriceType
    {
        None = 0,
        General = 1,
        CutPiece = 2,
      
    }
    #endregion

    #region Enummeration : EnumDCEntryType
    public enum EnumDCEntryType
    {
        None = 0,
        [Description("Open")]
        Open = 1,
        [Description("Before Load Dye Machine")]
        BeforeLoadDyeMachine = 2,
        [Description("Before Unload Dye Machine")]
        BeforeUnloadDyeMachine = 3,
        [Description("Before QC Done")]
        BeforeQCDone = 4
    }
    #endregion
    #region Enummeration : FabricPrograme
    public enum EnumFabricPrograme
    {
        None = 0, Knoting = 1, Getting = 2
    }
    #endregion

    #region Enummeration : EnumRSBagType
    public enum EnumRSBagType
    {
        None = 0,
        Cone = 1,
        Bales = 2,
        S_Poly = 3,
        B_Poly = 4
    }
    #endregion

    #region Enummeration : EnumDeliveryValidation
    public enum EnumDeliveryValidation
    {
        ColorQty = 1,
        FullOrderQty = 2,

    }
    #endregion
    #region Enummeration : EnumDyeingRecipeType
    public enum EnumDyeingRecipeType
    {
        General = 0,
        MachineWash= 1,
        FixedQty = 2
      
    }
    #endregion
    #region Enummeration : EnumFabricPlanRefType
    public enum EnumFabricPlanRefType
    {
        [Description("None")]
        None = 0,
        [Description("Fabric")]
        Fabric = 1,
        [Description("Dispo")]
        Dispo = 2,
        [Description("Specification")]
        Specification = 3,
    }
    #endregion

}
