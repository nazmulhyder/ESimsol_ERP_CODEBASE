using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUBatchCost
    public class DUBatchCost : BusinessObject
    {
        #region  Constructor
        public DUBatchCost()
        {
            //RS COST
            RouteSheetID = 0;
            RSNo = "";
            OrderNo = "";
            FactoryName = "";
            Buyer = "";
            Qty_Dyes = 0;
            Qty_Chemical = 0;
            Qty_Yarn = 0;
            YarnCount = "";
            WUnit = "";
            RSDate = DateTime.MinValue;

            //ORDER 
            MachineID =0;
			MachineName =""; 
			LocationID =0;
			LocationName =""; 
			WorkingUnitID =0;
			WUName =""; 
			ShadeName =""; 
			ShadePertage =0;
			MLoadTime = DateTime.MinValue;
            MUnLoadTime = DateTime.MinValue;
			Qty_Yarn =0;
			Qty_Dyes =0;
			Qty_Chemical =0;
			Value_Dyes =0;
			Value_Chemical =0;
			Value_Yarn =0;
			LotNo =""; 
			LotID_Yarn =0;
            //DETAIL FROM RS COST
            DyeingOrderDetailID = 0;
            DyeingOrderID = 0;
            ContractorID = 0;
            ExportPIID = 0;
            BuyerID = 0;
            OrderType = 0;
            ContractorName = "";
            ColorName = "";
            CurrencySymbol = "";
            //RS DETAIL COST
            RouteSheetDetailID = 0;
            ProductID = 0;
            ProductName = "";
            Qty = 0.0;
            Value = 0.0;
            TotalQtyLotID = 0;
            TotalQty = 0.0;
            ReturnQty = 0.0;
            TotalRate = 0.0;
            UnitPrice = 0.0;
            MUnitID = 0;
            MUnit = "";
            PINo = "";
            CurrencyID = 0;
            InvoiceDetailID = 0;
            InvoiceTypeID = 0;
            IsInHouse = false;
            IsReDyeing = false;
            RSState = EnumRSState.None;
            BUID = 0;
            RecycleQty = 0;
            //RecycleValue = 0;
        }
        #endregion

        #region Properties   
        public int RouteSheetID { get; set; }
        public string RSNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime RSDate { get; set; }
        public string QCDate { get; set; }
        public string FactoryName { get; set; }
        public string Location { get; set; }
        public string Buyer { get; set; }
        public double Qty_Dyes { get; set; }
        public double Qty_Chemical { get; set; }
        public double Qty_Yarn { get; set; }
        public double Value_Dyes { get; set; }
        public double Value_Chemical { get; set; }
        public double Value_Yarn { get; set; }
        public string LotNo { get; set; }
        public string OperationUnitName { get; set; }
        public string WUnit { get; set; }

        public int MachineID { get; set; }
        public int NumberOfAddition { get; set; }
        public string MachineName { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int WorkingUnitID { get; set; }
        public string WUName { get; set; }
        public string ShadeName { get; set; }
        public double ShadePertage { get; set; }
        public DateTime MLoadTime { get; set; }
        public DateTime MUnLoadTime { get; set; }
        public int LotID_Yarn { get; set; }

        public string CurrencySymbol { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int DyeingOrderID { get; set; }
        public int ContractorID { get; set; }
        public int ExportPIID { get; set; }
        public int BuyerID { get; set; }
        public string ContractorName { get; set; }
        public string ColorName { get; set; }
        public int RouteSheetDetailID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double Value { get; set; }
        public int TotalQtyLotID { get; set; }
        public double TotalQty { get; set; }
        public double ReturnQty { get; set; }
        public int InvoiceTypeID { get; set; }
        public int InvoiceDetailID { get; set; }
        public int CurrencyID { get; set; }
        public int MUnitID { get; set; }
        public double TotalRate { get; set; }
        public string Params { get; set; }
        public EnumRSState RSState { get; set; }
        public double UnitPrice { get; set; }
        public string ProductCategoryName { get; set; }
        public string PINo { get; set; }
        public double AdditionalQty { get; set; }
        public double AdditionalRate { get; set; }
        public double MCapacity { get; set; }
        public double PShort { get; set; }
        public double QtyDC { get; set; }
        public double RecycleQty { get; set; }
        public double WastageQty { get; set; }
        public double Qty_Finishid { get; set; }
        public double ValueRecycle { get; set; }
        public bool IsInHouse { get; set; }
        public bool IsReDyeing { get; set; }
        #endregion

        #region Property for Search
        public int BUID { get; set; }
        public int OrderType { get; set; }
        public int ReportType { get; set; }
        public int DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        #endregion

        #region Property for RS Wise Cost
        public string ProductName { get; set; }
        public string YarnCount { get; set; }
     
        public string MUnit { get; set; }
        public string OperationUnitNameY { get; set; }
        public string LocationNameY { get; set; }
        #endregion
       
        #region Derived Properties
         public string RSNoID
         {
             get
             {
                 return this.RSNo + "~" + this.RouteSheetID;
             }
         }
         public string RSDateSt
         {
             get
             {
                 if (this.RSDate == DateTime.MinValue)
                     return "-";
                 else return RSDate.ToString("dd MMM yyyy");
             }
         }
         public string MLoadTimeSt
         {
             get
             {
                 if (this.MLoadTime == DateTime.MinValue)
                     return "-";
                 else return MLoadTime.ToString("dd MMM yyyy hh:MM tt");
             }
         }
         public string MUnLoadTimeSt
         {
             get
             {
                 if (this.MUnLoadTime == DateTime.MinValue)
                     return "-";
                 else return MUnLoadTime.ToString("dd MMM yyyy hh:MM tt");
             }
         }
         public string  DyeingTimeDuration
         {
             get
             {
                 if (this.MLoadTime == DateTime.MinValue || this.MUnLoadTime == DateTime.MinValue || this.MUnLoadTime <= this.MLoadTime)
                     return "-";
                 else
                 {
                     TimeSpan TS = MUnLoadTime - MLoadTime;
                     int hour = TS.Hours + (TS.Days*24);
                     int mins = TS.Minutes;
                     int secs = TS.Seconds;
                     return (hour.ToString("00") + ":" + mins.ToString("00") + ":" + secs.ToString("00"));
                 }
             }
         }
         private double nDyeing_Cost = 0;
         public double Dyeing_Cost
         {
             get
             {
                 if (this.IsInHouse == false)
                 {
                     nDyeing_Cost = this.Value_Dyes + this.Value_Chemical;
                 }
                 else
                 {
                     //if (this.IsReDyeing == true)
                     //{
                     //    nDyeing_Cost = this.Value_Dyes + this.Value_Chemical;
                     //}
                     //else
                     //{
                         nDyeing_Cost = this.Value_Yarn + this.Value_Dyes + this.Value_Chemical;
                     //}
                 }
                 return nDyeing_Cost;
             }
         }
         public double Finish_Cost
         {
             get
             {
                 if (this.IsInHouse == false)
                 {
                     nDyeing_Cost = this.Value_Dyes + this.Value_Chemical;
                 }
                 else
                 {
                     //if (this.IsReDyeing == true)
                     //{
                     //    nDyeing_Cost = this.Value_Dyes + this.Value_Chemical;
                     //}
                     //else
                     //{
                     nDyeing_Cost = this.Value_Yarn + this.Value_Dyes + this.Value_Chemical - this.ValueRecycle;
                     //}
                 }
                 return nDyeing_Cost;
             }
         }
         public double Chem_CostPerKG
         {
             get
             {
                 return (this.Qty_Yarn > 0 ? this.Value_Chemical / this.Qty_Yarn : 0);
             }
         }
         public double Dyes_CostPerKG
         {
             get
             {
                 return (this.Qty_Yarn > 0 ? this.Value_Dyes / this.Qty_Yarn : 0);
             }
         }
         private double nDyeing_CostPerKg = 0;
         public double Dyeing_CostPerKg
         {
             get
             {
                 /// if Out Party Yarn and ReDyeing Than Calalute only Dyes and Chemical
                 if (this.IsInHouse == false)
                 {
                     nDyeing_CostPerKg = ((this.Qty_Yarn) > 0) ? (this.Value_Dyes + this.Value_Chemical) / this.Qty_Yarn : 0;
                 }
                 else
                 {
                     //if (this.IsReDyeing == true)
                     //{
                     //    nDyeing_CostPerKg = ((this.Qty_Yarn) > 0) ? ( this.Value_Dyes + this.Value_Chemical) / this.Qty_Yarn : 0;
                     //}
                     //else
                     //{
                         //nDyeing_CostPerKg = (this.Qty_Yarn > 0 ? this.Value_Yarn / this.Qty_Yarn : 0) + (this.Qty_Chemical > 0 ? this.Value_Chemical / this.Qty_Chemical : 0) + (Qty_Dyes > 0 ? this.Value_Dyes / this.Qty_Dyes : 0);
                         nDyeing_CostPerKg = ((this.Qty_Yarn) > 0) ? (this.Value_Yarn + this.Value_Dyes + this.Value_Chemical) / this.Qty_Yarn : 0;
                     //}
                 }
                 return nDyeing_CostPerKg;

                 
             }
         }

         public string IsReDyeingST
         {
             get
             {
                 if (this.IsReDyeing) return "Re Dyeing";
                 else return "Fresh Dyeing";
             }
         }
         public string IsInHouseSt
         {
             get
             {
                 if (this.IsInHouse) return "In House";
                 else return "Out House";
             }
         }
         public string RSStateSt
         {
             get
             {
                 return ((EnumRSState)this.RSState).ToString();
             }
         }
        #endregion

        #region Functions
        public static List<DUBatchCost> Gets(DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType, long nUserID)
        {
            return DUBatchCost.Service.Gets(dStartDate, dEndDate, sSQL, nRouteSheetID, nReportType, nUserID);
        }
        public static List<DUBatchCost> GetsDetail(string sSQL, int nRouteSheetID, long nUserID)
        {
            return DUBatchCost.Service.GetsDetail(sSQL, nRouteSheetID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUBatchCostService Service
        {
            get { return (IDUBatchCostService)Services.Factory.CreateService(typeof(IDUBatchCostService)); }
        }
      
        #endregion
    }
    #endregion

    
    #region IDUBatchCost interface
   
    public interface IDUBatchCostService
    {
        List<DUBatchCost> Gets(DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType, Int64 nUserID);
        List<DUBatchCost> GetsDetail(string sSQL, int nRouteSheetID, Int64 nUserID);
    }
    
    #endregion


    public class abc : xyz
    {
        public int print()
        {
            return 1;
        }
    }

    public interface xyz 
    {
        int print();
    }

}