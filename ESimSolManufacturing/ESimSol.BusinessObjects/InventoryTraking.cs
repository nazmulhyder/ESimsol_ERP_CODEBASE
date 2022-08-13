using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region InventoryTraking
    public class InventoryTraking : BusinessObject
    {
        #region  Constructor
        public InventoryTraking()
        {
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            ProductID = 0;
            OpeningQty = 0;
            InQty = 0;
            OutQty = 0;
            ClosingQty = 0;
            OpeningValue = 0;
            InValue = 0;
            OutValue = 0;
            ClosingValue = 0;
            ValueType = 0;
            TriggerParentType = 0;
            LotID = 0;
            InOutType = 0;
            WorkingUnitID = 0;
            CurrencyID = 0;
            UnitPrice = 0;
            MUnitID = 0;
            BUName = "";
            LocName = "";
            OPUName = "";
            StyleNo = "";
            Param = "";
            TransactionDate = DateTime.Now;
            EntryDateTime = DateTime.Now;
            Balance = 0;
            ParentID = 0;
            ColorID = 0;
            ColorName = "";
            Remarks = "";
            ParentType = EnumTriggerParentsType.None;
            RefType = EnumGRNType.None;
            RefObjectID = 0;
            RefNo = "";
            ContractorName = "";
            Origin = "";
            OpeningAmount = 0;
            InAmount = 0;
            OutAmount = 0;
            ClosingAmount = 0;
            IsTransectionOnly = false;
            Measurement = "";
            FinishDia = "";
            MCDia = "";
            GSM = "";
            URL = "";
            ErrorMessage = "";

            InGRN = 0;
            InAdj = 0;
            InRS = 0;
            InTr = 0;
            InRet = 0;
          
            OutAdj = 0;
            OutRS = 0;
            OutTr = 0;
            OutRet = 0;
            OutCon = 0;
            InTrSW = 0;
            OutDC = 0;
            InWastage = 0;
            InRecycle = 0;
            InWIP = 0;
            OutWastage  = 0;
            OutRecycle  = 0;
            OutWIP = 0;
            PCategoryName = "";
        }
        #endregion

        #region Properties

        public int ProductID { get; set; }
        public int LotID { get; set; }
        public int ParentID { get; set; }
        public double OpeningQty { get; set; }
        public double InQty { get; set; }
        public double OutQty { get; set; }
        public double ClosingQty { get; set; }
        public double OpeningValue { get; set; }
        public double ClosingAmount { get; set; }
        public double OutAmount { get; set; }
        public double InValue { get; set; }
        public double InAmount { get; set; }
        public double OpeningAmount { get; set; }
        public double OutValue { get; set; }
        public double ClosingValue { get; set; }
        public double InGRN { get; set; }
        public double InAdj { get; set; }
        public double InRS { get; set; }
        public double InTr { get; set; }
        public double InTrSW { get; set; }
        public double InRet { get; set; }
     
        public double OutAdj { get; set; }
        public double OutRS { get; set; }
        public double OutTr { get; set; }
        public double OutRet { get; set; }
        public double OutCon { get; set; }
        public double OutTrSW { get; set; }
        public double OutDC { get; set; }
        public double InWastage  { get; set; }
        public double InRecycle  { get; set; }
        public double InWIP  { get; set; }
        public double OutWastage  { get; set; }
	    public double OutRecycle  { get; set; }
	    public double OutWIP  { get; set; }
        
        public int MUnitID { get; set; }
        public int CurrencyID { get; set; }
        public string Currency { get; set; }
        public int InOutType { get; set; }
        public int WorkingUnitID { get; set; }
        public int ValueType { get; set; }
        public int RefObjectID { get; set; }
        public int TriggerParentType { get; set; }
        public int PCategoryID { get; set; }
        public int BUID { get; set; }
        //use in Item MOvement
        public string BUName { get; set; }
        public string LocName { get; set; }
        public string Origin { get; set; }
        public string ContractorName { get; set; }
        public string OPUName { get; set; }
        public string StyleNo { get; set; }
        public double UnitPrice { get; set; }
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public string Param { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string FinishDia { get; set; }
        public string MCDia { get; set; }
        public string GSM { get; set; }
        public string URL { get; set; }
        public string Measurement { get; set; }
        public bool IsTransectionOnly { get; set; }
        #endregion

        #region Derived Properties
        public string PCategoryName { get; set; }
        public string ErrorMessage { get; set; }
        public string WUName { get; set; }
        public string LotNo { get; set; }
        public string LogNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MUnit { get; set; }
        public int DateType { get; set; }
        public double Balance { get; set; }
        public EnumTriggerParentsType ParentType { get; set; }
        public EnumGRNType RefType { get; set; }
        public string ParentTypeSt { get; set; }
        public string RefTypeSt { get { return EnumObject.jGet(this.RefType); } }
        public string RefTypeStTemp { get {
            if (this.RefType == EnumGRNType.ImportInvoice)
                return "ImportLC";
            else 
                return EnumObject.jGet(this.RefType); 
        } }
        public string ParentTypeEnumSt { get { return EnumObject.jGet(this.ParentType); } }
        public string RefNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDateSt
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy");
            }
        }
        public string TimeSt
        {
            get
            {
                return this.StartDate.ToString("HH:mm");

            }
        }
        public string StartDatetimeSt
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string EndDateSt
        {
            get
            {
                return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDatetimeSt
        {
            get
            {
                return this.EndDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string WorkingUnitName
        {
            get
            {
                if (this.OPUName == "")
                {
                    return this.LocName;
                }
                else
                {
                    return this.BUName + "-" + this.LocName + "[" + this.OPUName + "]";
                }
            }
        }

        public string TransactionDateSt
        {
            get
            {
                if (this.TransactionDate == DateTime.MinValue)
                {
                    return "Opening";
                }
                else
                {
                    return this.TransactionDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string EntryDateTimeSt
        {
            get
            {
                if (this.EntryDateTime == DateTime.MinValue)
                {
                    return "Opening";
                }
                else
                {
                    return this.EntryDateTime.ToString("dd MMM yyyy hh:mm ss tt");
                }
            }
        }

        public string InQtySt
        {
            get
            {
                return Global.MillionFormat(this.InQty);
            }
        }

        public string OutQtySt
        {
            get
            {
                return Global.MillionFormat(this.OutQty);
            }
        }


        public string UnitPriceSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.UnitPrice);
            }
        }
        #endregion


        #region Functions
        public static List<InventoryTraking> Gets_WU(DateTime dStartDate, DateTime dEndDate, int nBUID, int nTParentType, int nValueType, long nUserID)
        {
            return InventoryTraking.Service.Gets_WU(dStartDate, dEndDate, nBUID, nTParentType, nValueType, nUserID);
        }
        public static List<InventoryTraking> Gets_Product(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, long nUserID)
        {
            return InventoryTraking.Service.Gets_Product(dStartDate, dEndDate, nBUID, nWorkingUnitID, nTParentType, nValueType, nMUnitID, nCurrencyID, nUserID);
        }
        public static List<InventoryTraking> Gets_ProductDyeing(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, long nUserID)
        {
            return InventoryTraking.Service.Gets_ProductDyeing(dStartDate, dEndDate, nBUID, nWorkingUnitID, nTParentType, nValueType, nMUnitID, nCurrencyID, nUserID);
        }
        public static List<InventoryTraking> Gets_Qty_Value(DateTime dStartDate, DateTime dEndDate, int nBUID, string nWorkingUnitIDs, int nCurrencyID, int nProductCategoryID, long nUserID)
        {
            return InventoryTraking.Service.Gets_Qty_Value(dStartDate, dEndDate, nBUID, nWorkingUnitIDs, nCurrencyID, nProductCategoryID, nUserID);
        }
        public static List<InventoryTraking> Gets_ITransactions(DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut, Int64 nUserID)
        {
            return InventoryTraking.Service.Gets_ITransactions(dStartDate, dEndDate, sSQL, nReportLayOut, nUserID);
        }
        public static List<InventoryTraking> Gets_Lot(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nProductID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, long nUserID)
        {
            return InventoryTraking.Service.Gets_Lot(dStartDate, dEndDate, nBUID, nWorkingUnitID, nProductID, nTParentType, nValueType, nMUnitID, nCurrencyID, nUserID);
        }
        public static List<InventoryTraking> Gets_ForItemMovement(DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut, long nUserID)
        {
            return InventoryTraking.Service.Gets_ForItemMovement(dStartDate, dEndDate, sSQL, nReportLayOut, nUserID);
        }
        public static List<InventoryTraking> Gets_ForInventorySummary(DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut, long nUserID)
        {
            return InventoryTraking.Service.Gets_ForInventorySummary(dStartDate, dEndDate, sSQL, nReportLayOut, nUserID);
        }
        public static List<InventoryTraking> Gets_ForItemMovementDetails(DateTime dStartDate, DateTime dEndDate, string sSQL, string sSQL_FindLot, long nUserID)
        {
            return InventoryTraking.Service.Gets_ForItemMovementDetails(dStartDate, dEndDate, sSQL, sSQL_FindLot, nUserID);
        }
        public static List<InventoryTraking> Gets_ProductWise(string sProductIDs, int nBUID, DateTime dStartDate, DateTime dEndDate, long nUserID)
        {
            return InventoryTraking.Service.Gets_ProductWise(sProductIDs, nBUID, dStartDate, dEndDate, nUserID);
        }
        #endregion
        #region [NonDB Functions]

        #endregion
        #region ServiceFactory

        internal static IInventoryTrakingService Service
        {
            get { return (IInventoryTrakingService)Services.Factory.CreateService(typeof(IInventoryTrakingService)); }
        }
        #endregion
    }
    #endregion

    #region IInventoryTraking interface
    public interface IInventoryTrakingService
    {

        List<InventoryTraking> Gets_WU(DateTime dStartDate, DateTime dEndDate, int nBUID, int nTParentType, int nValueType, Int64 nUserID);
        List<InventoryTraking> Gets_Product(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, Int64 nUserID);
        List<InventoryTraking> Gets_ProductDyeing(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, Int64 nUserID);
        List<InventoryTraking> Gets_Lot(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nProductID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, Int64 nUserID);
        List<InventoryTraking> Gets_Qty_Value(DateTime dStartDate, DateTime dEndDate, int nBUID, string nWorkingUnitIDs, int nCurrencyID, int nProductCategoryID, Int64 nUserID);
        List<InventoryTraking> Gets_ITransactions(DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut, Int64 nUserID);
        List<InventoryTraking> Gets_ForItemMovement(DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut, long nUserID);
        List<InventoryTraking> Gets_ForInventorySummary(DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut, long nUserID);
        List<InventoryTraking> Gets_ForItemMovementDetails(DateTime dStartDate, DateTime dEndDate, string sSQL, string sSQL_FindLot, long nUserID);
        List<InventoryTraking> Gets_ProductWise(string sProductIDs, int nBUID, DateTime dStartDate,DateTime dEndDate, long nUserID);
    }
    #endregion
}