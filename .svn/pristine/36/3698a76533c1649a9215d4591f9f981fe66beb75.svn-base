using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    #region PlanAnalysis
    
    public class PlanAnalysis : BusinessObject
    {
        public PlanAnalysis() 
        {
            TempID = 0;
            PlanDate=DateTime.Now;
			PlanQty =0;
			TotalPlanQty =0;
			ExecutionStepQty =0;
            TotalExecutionStepQty = 0;
            TechnicalSheetID = 0;
            MaxExecutionDate = DateTime.Now;
            TotalQtyOfMaxExecutionDate = 0;
            GapInPercent = 0;
            ShortQty = 0;
            ApproximateFinishDate = DateTime.Now;
            RequiredPerDayTotalQty = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int TempID { get; set; }
         
        public int PlanQty { get; set; }
         
        public int TotalPlanQty { get; set; }
         
        public int ExecutionStepQty { get; set; }
         
        public int TotalExecutionStepQty { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public double GapInPercent { get; set; }
         
        public DateTime PlanDate { get; set; }
         
        public double ShortQty { get; set; }
         
        public DateTime MaxExecutionDate { get; set; }
         
        public DateTime ApproximateFinishDate {get;set;}
         
        public double RequiredPerDayTotalQty {get;set;}
         
        public double TotalQtyOfMaxExecutionDate { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<PlanAnalysis> PlanAnalysisList{ get; set; }
        public string PlanDateInString
        {
            get
            {
                return this.PlanDate.ToString("dd MMM yyyy");
            }
        }
        public string PlanDateInShortString
        {
            get
            {
                return this.PlanDate.ToString("dd MMM");
            }
        }

        public string MaxExecutionDateInString
        {
           get
            {
                return this.MaxExecutionDate.ToString("dd MMM yyyy");
            }
        }
        public string MaxExecutionDateInShortString
        {
            get
            {
                return this.MaxExecutionDate.ToString("dd MMM");
            }
        }
        public string ApproximateFinishDateInString
        {
            get
            {
                return this.ApproximateFinishDate.ToString("dd MMM yyyy");
            }
        }
        public Company Company { get; set; }

        #endregion


        #region Functions

        public static List<PlanAnalysis> Gets(int stepID, int GUProductionOrderID, long nUserID) 
        {
            return PlanAnalysis.Service.Gets(stepID,GUProductionOrderID, nUserID);
        }
        #endregion

        #region ServiceFactory

 
        internal static IPlanAnalysisService Service
        {
            get { return (IPlanAnalysisService)Services.Factory.CreateService(typeof(IPlanAnalysisService)); }
        }
        #endregion
    }
    #endregion


    #region IPlanAnalysis interface
     
    public interface IPlanAnalysisService
    {
         
        List<PlanAnalysis> Gets(int stepID, int GUProductionOrderID, Int64 nUserID);
    }

    #endregion

    
   
}
