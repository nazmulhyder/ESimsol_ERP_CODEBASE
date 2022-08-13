using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region ProductionExecutionPlanDetailBreakdown
    
    public class ProductionExecutionPlanDetailBreakdown : BusinessObject
    {
        public ProductionExecutionPlanDetailBreakdown()
        {
            ProductionExecutionPlanDetailBreakdownID = 0;
            ProductionExecutionPlanDetailID = 0;
            WorkingDate = DateTime.Now;
            RegularTime = 0;
            OverTime = 0;
            DailyProduction = 0;
            EfficencyInParcent = 0;
            PLineConfigureID = 0;
            RecapNo = "";
            StyleNo = "";
            BuyerName = "";
            OrderRecapID = 0;
            RecapQty = 0;
            ExecutionQty = 0;
            ProductionExecutionPlanID = 0;
            UnitSymbol = "";
            PlanDate = DateTime.Today;
            BrekDownIDs = "";
            BUID = 0;
            ProductionUnitID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ProductionExecutionPlanDetailBreakdownID { get; set; }
        public int ProductionExecutionPlanDetailID { get; set; }
        public int ProductionExecutionPlanID { get; set; }
        public DateTime WorkingDate { get; set; }
        public double RegularTime { get; set; }
         public double OverTime { get; set; }
        public double DailyProduction { get; set; }
        public double EfficencyInParcent { get; set; }
        public int  PLineConfigureID { get; set; }
		public string  RecapNo { get; set; } 
		public string  StyleNo { get; set; }
        public string  BuyerName { get; set; }
        public int OrderRecapID { get; set; }
        public double RecapQty { get; set; }
        public double ExecutionQty { get; set; }
        public string UnitSymbol { get; set; }
        public int  BUID { get; set; }
        public int ProductionUnitID { get; set; }
         public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         public DateTime PlanDate { get; set; }
         public string BrekDownIDs { get; set; }
        public string WorkingDateInString
        {
            get
            {
                return this.WorkingDate.ToString("dd MMM yyyy")+"("+this.WorkingDate.ToString("dddd") +")";
            }
        }
      

        #endregion

        #region Functions

        public static List<ProductionExecutionPlanDetailBreakdown> Gets(int ProductionExecutionPlanDetailID, long nUserID)
        {
            return ProductionExecutionPlanDetailBreakdown.Service.Gets(ProductionExecutionPlanDetailID, nUserID);
        }
        public static List<ProductionExecutionPlanDetailBreakdown> Gets(string sSQL, long nUserID)
        {
            return ProductionExecutionPlanDetailBreakdown.Service.Gets(sSQL, nUserID);
        }

        public static List<ProductionExecutionPlanDetailBreakdown> GetsByPEPPlanID(int nProductionExecutionPlanID, long nUserID)
        {
            return ProductionExecutionPlanDetailBreakdown.Service.GetsByPEPPlanID(nProductionExecutionPlanID, nUserID);
        }
        
        public ProductionExecutionPlanDetailBreakdown Get(int ProductionExecutionPlanDetailBreakdownID, long nUserID)
        {
            return ProductionExecutionPlanDetailBreakdown.Service.Get(ProductionExecutionPlanDetailBreakdownID, nUserID);
        }

        public ProductionExecutionPlanDetailBreakdown Save(long nUserID)
        {
            return ProductionExecutionPlanDetailBreakdown.Service.Save(this, nUserID);
        }
        public string Paste(long nUserID)
        {
            return ProductionExecutionPlanDetailBreakdown.Service.Paste(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IProductionExecutionPlanDetailBreakdownService Service
        {
            get { return (IProductionExecutionPlanDetailBreakdownService)Services.Factory.CreateService(typeof(IProductionExecutionPlanDetailBreakdownService)); }
        }
        #endregion
    }
    #endregion

    #region IProductionExecutionPlanDetailBreakdown interface
     
    public interface IProductionExecutionPlanDetailBreakdownService
    {
         
        ProductionExecutionPlanDetailBreakdown Get(int ProductionExecutionPlanDetailBreakdownID, Int64 nUserID);
         
        List<ProductionExecutionPlanDetailBreakdown> Gets(int ProductionExecutionPlanDetailID, Int64 nUserID);
         
        List<ProductionExecutionPlanDetailBreakdown> GetsByPEPPlanID(int nProductionExecutionPlanID, Int64 nUserID);
        
         
        List<ProductionExecutionPlanDetailBreakdown> Gets(string sSQL, Int64 nUserID);
         
        ProductionExecutionPlanDetailBreakdown Save(ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown, Int64 nUserID);
        string Paste(ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown, Int64 nUserID);

    }
    #endregion
    
}
