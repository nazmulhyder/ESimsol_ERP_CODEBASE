using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
	#region FASchedule  
	public class FASchedule : BusinessObject
	{	
		public FASchedule()
		{
			FAScheduleID = 0;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
			FARegisterID = 0;
            MonthCount = 0;
            DepreciationMethod = 0;
            OpeningBookValue = 0.0;
            DepreciationRate = 0.0;
            DepreciationValue = 0.0;
            AdditionValue = 0.0;
            ClosingBookValue = 0.0;
            AccumulatedDepValue = 0.0;
            ErrorMessage = "";
		}

		#region Property
        public int FAScheduleID { get; set; }
        public int FARegisterID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MonthCount { get; set; }
        public double OpeningBookValue { get; set; }
        public int DepreciationMethod { get; set; }
        public double DepreciationRate { get; set; }
        public double DepreciationValue { get; set; }
        public double AdditionValue { get; set; }
        public double AccumulatedDepValue { get; set; }
        public double ClosingBookValue { get; set; }
        public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string OpeningBookValueSt { get { return Global.MillionFormatRound(this.OpeningBookValue, 2); } }
        public string DepreciationRateSt { get { return Global.MillionFormatRound(this.DepreciationRate, 2); } }
        public string DepreciationValueSt { get { return Global.MillionFormatRound(this.DepreciationValue, 2); } }
        public string AdditionValueSt { get { return Global.MillionFormatRound(this.AdditionValue, 2); } }
        public string AccumulatedDepValueSt { get { return Global.MillionFormatRound(this.AccumulatedDepValue, 2); } }
        public string ClosingBookValueSt { get { return Global.MillionFormatRound(this.ClosingBookValue, 2); } }
		#endregion 

		#region Functions 
		public static List<FASchedule> Gets(long nUserID)
		{
			return FASchedule.Service.Gets(nUserID);
		}
		public static List<FASchedule> Gets(string sSQL, long nUserID)
		{
			return FASchedule.Service.Gets(sSQL,nUserID);
		}
        public static List<FASchedule> Gets(int nFARID, long nUserID)
		{
            return FASchedule.Service.Gets(nFARID, nUserID);
		}
        public static List<FASchedule> GetsLogScheduleBy(int nFARLogID, long nUserID)
		{
            return FASchedule.Service.GetsLogScheduleBy(nFARLogID, nUserID);
		}
        public static List<FASchedule> SaveFASchedules(int nFARID, long nUserID)
        {
            return FASchedule.Service.SaveFASchedules(nFARID, nUserID);
        }
       
        
		#endregion

		#region ServiceFactory
		internal static IFAScheduleService Service
		{
			get { return (IFAScheduleService)Services.Factory.CreateService(typeof(IFAScheduleService)); }
		}
		#endregion
    }
	#endregion

	#region IFASchedule interface
	public interface IFAScheduleService 
	{
		List<FASchedule> Gets(Int64 nUserID);
        List<FASchedule> Gets(int nFARID, Int64 nUserID);
        List<FASchedule> GetsLogScheduleBy(int nFARLogID, Int64 nUserID);
        List<FASchedule> Gets(string sSQL, Int64 nUserID);
        //List<FAScheduleReport> Gets(double DateYear, Int64 nUserID);
        List<FASchedule> SaveFASchedules(int nFARID, Int64 nUserID);
	}
	#endregion

    #region FAScheduleReport
    public class FAScheduleReport : BusinessObject
    {
        public FAScheduleReport()
        {
            FARegisterID = 0;
            PurchaseDate = DateTime.MinValue;
            ProductID = 0;
            ProductName = "";
            ProductCategoryID = 0;
            ProductCategoryName = "";
            Amount_Cost = 0.0; 
			SalvageValue = 0.0; 
			DEPPercentage = 0.0; 
			OpenningCost = 0.0; 
			DepreciationperYear = 0.0; 
			TotalAccumulatedCost = 0.0; 
			ClosingCost = 0.0; 
			UsefulLifetime = 0.0; 

            ErrorMessage = "";
        }

        #region Property
        public int FARegisterID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public double Amount_Cost { get; set; }
        public double SalvageValue { get; set; }
        public double DEPPercentage { get; set; }
        public double OpenningCost { get; set; }
        public double DepreciationperYear { get; set; }
        public double TotalAccumulatedCost { get; set; }
        public double ClosingCost { get; set; }
        public double UsefulLifetime { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string PurchaseDateSt { get { return this.PurchaseDate.ToString("dd MMM yyyy"); } }
        #endregion

        #region Functions
        
        #endregion

        
    }
    #endregion

}
