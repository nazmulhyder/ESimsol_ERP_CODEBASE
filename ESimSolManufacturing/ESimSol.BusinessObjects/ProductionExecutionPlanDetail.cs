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

    #region ProductionExecutionPlanDetail
    
    public class ProductionExecutionPlanDetail : BusinessObject
    {
        public ProductionExecutionPlanDetail()
        {
            ProductionExecutionPlanDetailID = 0;
            ProductionExecutionPlanID = 0;
            FactoryName ="";
            StartDate=DateTime.Now;
            EndDate = DateTime.Now;
            WorkingDays =0;
            MachineQty = 0;
            ProductionPerHour = 0;
            AvgDailyProduction = 0;
            TotalProduction = 0;
            PLineConfigureID = 0;
            ErrorMessage = "";
            LineNo = "";
            ProductionUnitID = 0;
            PUShortName = "";
            Params = "";
            Dynamics = new List<dynamic>();
            PLineConfigures = new List<PLineConfigure>();
        }

        #region Properties
        public int ProductionExecutionPlanDetailID { get; set; }
        public int ProductionExecutionPlanID { get; set; }
        public int PLineConfigureID { get; set; }
        public string FactoryName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double WorkingDays { get; set; }
        public double MachineQty { get; set; }
        public double ProductionPerHour { get; set; }
        public double AvgDailyProduction { get; set; }
        public double TotalProduction { get; set; }
        public string LineNo { get; set; }
        public int ProductionUnitID { get; set; }
        public string PUShortName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Params { get; set; }
        public int BUID { get; set; }
        public List<dynamic> Dynamics { get; set; }
        public List<PLineConfigure> PLineConfigures { get; set; }
       public List<ProductionExecutionPlanDetailBreakdown> ProductionExecutionPlanDetailBreakdowns { get; set; }
     
       public string StartDateInString
       {
           get
           {
               return this.StartDate.ToString("dd MMM yyyy");
           }
       }

       public string EndDateInString
       {
           get
           {
               return this.EndDate.ToString("dd MMM yyyy");
           }
       }
        #endregion

        #region Functions

        public static List<ProductionExecutionPlanDetail> Gets(int CourierServiceID, long nUserID)
        {
            return ProductionExecutionPlanDetail.Service.Gets(CourierServiceID, nUserID);
        }
        public static List<ProductionExecutionPlanDetail> Gets(string sSQL, long nUserID)
        {
            return ProductionExecutionPlanDetail.Service.Gets(sSQL, nUserID);
        }
        public ProductionExecutionPlanDetail Get(int ProductionExecutionPlanDetailID, long nUserID)
        {
            return ProductionExecutionPlanDetail.Service.Get(ProductionExecutionPlanDetailID, nUserID);
        }

        public ProductionExecutionPlanDetail Save(long nUserID)
        {
            return ProductionExecutionPlanDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IProductionExecutionPlanDetailService Service
        {
            get { return (IProductionExecutionPlanDetailService)Services.Factory.CreateService(typeof(IProductionExecutionPlanDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IProductionExecutionPlanDetail interface
     
    public interface IProductionExecutionPlanDetailService
    {
         
        ProductionExecutionPlanDetail Get(int ProductionExecutionPlanDetailID, Int64 nUserID);
         
        List<ProductionExecutionPlanDetail> Gets(int CourierServiceID, Int64 nUserID);
         
        List<ProductionExecutionPlanDetail> Gets(string sSQL, Int64 nUserID);
         
        ProductionExecutionPlanDetail Save(ProductionExecutionPlanDetail oProductionExecutionPlanDetail, Int64 nUserID);


    }
    #endregion
   
}
