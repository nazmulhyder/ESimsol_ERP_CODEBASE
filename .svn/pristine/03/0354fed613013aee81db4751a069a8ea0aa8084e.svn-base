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
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Drawing;

namespace ESimSol.BusinessObjects
{
    #region FNBatchCost
    public class FNBatchCost : BusinessObject
    {
        #region  Constructor
        public FNBatchCost()
        {
            //RS COST
            FNExOID 	= 0;
            ProDate = DateTime.MinValue;
            FNBatchID =0;
            Qty_Batch = 0.0;
            Qty_Production = 0.0;
            Value = 0.0;
            CurrencyID=0;  
            Currency=""; 
            ID =0;
            Name ="";

            //Detail\
            FabricID =0;
            SCNo="";
            DispoNo="";
            Construction="";
            ContractorName="";
            BuyerName="";
            Color="";
            OrderDate = DateTime.MinValue;
            ContractorID =0;
            Qty_Order =0.0;
            OrderType =0;
            PIID  =0;
            PINo ="";
            BuyerID =0;
            UnitPricePI = 0.0;
			Qty =0.0;
			UnitPrice =0.0;
			LotID =0;
			MUnitID  =0;
			MUnit ="";
			CurrencyID  =0;
			ProductID  =0;
			ProductName ="";
			ProductCategoryName ="";
			MachineName ="";
			MachineID =0;
            ReportType = 0;
            IsProduction = false;
            FNPBatchID = 0;
            FNCode = "";
        }
        #endregion

        #region
        public int FNExOID { get; set; }
        public int ID { get; set; }
        public DateTime ProDate { get; set; }
        public int FNBatchID { get; set; }
        public double Qty_Batch { get; set; }
        public double Qty_Production { get; set; }
        public double Value { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public int CurrencyID { get; set; }
        #endregion

        #region Property for Search
        public int DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Property for Details
        public int FabricID { get; set; }        
        public string SCNo { get; set; }
        public string DispoNo { get; set; }
        public string FNBatchNo { get; set; }
        public string Construction { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string Color { get; set; }
        public DateTime OrderDate { get; set; }
        public int ContractorID { get; set; }
        public double Qty_Order { get; set; }
        public int OrderType { get; set; }
        public int PIID { get; set; }
        public string PINo { get; set; }
        public string FNCode { get; set; }
        public int BuyerID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public int LotID { get; set; }
        public int MUnitID { get; set; }
        public int ProductID { get; set; }
        public int MachineID { get; set; }
        public string MUnit { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string MachineName { get; set; }
        public string FNProcess { get; set; }
        public bool IsProduction { get; set; }
        public EnumFNTreatment FNTreatment { get; set; }
        public double UnitPricePI { get; set; }
        public int FNPBatchID { get; set; }
        public int ReportType { get; set; }
        public string MktName { get; set; }
        public int MktAccountID { get; set; }
        #endregion
       
        #region Derived Properties
        public string ProDateSt
         {
             get
             {
                 if (this.ProDate == DateTime.MinValue)
                     return "-";
                 else return ProDate.ToString("dd MMM yyyy");
             }
         }
        public double Amount
        {
            get
            {
                return Qty * UnitPrice;
            }
        }
        public string FNTreatmentSt
        {
            get
            {
                return EnumObject.jGet(this.FNTreatment);
            }
        }
        public string FNTreatment_Process
        {
            get
            {
                return this.FNProcess+ "[" + EnumObject.jGet(this.FNTreatment) + "]"  ;
            }
        }
        public string IsProductionSt
        {
            get
            {
                if (IsProduction == true) return "Reproduction";
                return "Fresh";
            }
        }
        #endregion

        #region Functions
        public static List<FNBatchCost> Gets(DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType, long nUserID)
        {
            return FNBatchCost.Service.Gets(dStartDate, dEndDate, sSQL, nRouteSheetID, nReportType, nUserID);
        }
        public static List<FNBatchCost> GetsDetail(string sSQL, long nUserID)
        {
            return FNBatchCost.Service.GetsDetail(sSQL, nUserID);
        }
     
        public static List<FNBatchCost> GetsSQL(string sSQL, long nUserID)
        {
            return FNBatchCost.Service.GetsSQL(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
         internal static IFNBatchCostService Service
         {
             get { return (IFNBatchCostService)Services.Factory.CreateService(typeof(IFNBatchCostService)); }
         }
      
        #endregion


    }
    #endregion

    #region IFNBatchCost interface
   
    public interface IFNBatchCostService
    {
        List<FNBatchCost> Gets(DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType, Int64 nUserID);
        List<FNBatchCost> GetsDetail(string sSQL, Int64 nUserID);
        List<FNBatchCost> GetsSQL(string sSQL, Int64 nUserID);
    }
    
    #endregion
}