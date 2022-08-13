using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region PerformanceIncentiveEvaluation

    public class PerformanceIncentiveEvaluation : BusinessObject
    {
        public PerformanceIncentiveEvaluation()
        {
            PIEvaluationID = 0;
            PIMemberID = 0;
            Point = 0;
            Amount = 0;
            MonthID = EnumMonth.None;
            Year = 0;
            ApproveBy = 0;
            ApproveByDate = DateTime.Now;
            ErrorMessage = "";
            ApproveByName = "";
            EmployeeName = "";
            EmployeeCode = "";
            
        }

        #region Properties
        public int PIEvaluationID { get; set; }
        public int PIMemberID { get; set; }
        public double Point { get; set; }
        public double Amount { get; set; }
        public EnumMonth MonthID { get; set; }
        public int Year { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<PerformanceIncentiveEvaluation> PerformanceIncentiveEvaluations { get; set; }
        public string EncryptPIMemberID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string ApproveByName { get; set; }
        public string ApproveDateInString
        {
            get
            {
                return ApproveByDate.ToString("dd MMM yyyy");
            }
        }
        public string MonthInString
        {
            get
            {
                return MonthID.ToString();
            }
        }

        #endregion

        #region Functions
        public static PerformanceIncentiveEvaluation Get(int Id, long nUserID)
        {
            return PerformanceIncentiveEvaluation.Service.Get(Id, nUserID);
        }
        public static PerformanceIncentiveEvaluation Get(string sSQL, long nUserID)
        {
            return PerformanceIncentiveEvaluation.Service.Get(sSQL, nUserID);
        }
        public static List<PerformanceIncentiveEvaluation> Gets(long nUserID)
        {
            return PerformanceIncentiveEvaluation.Service.Gets(nUserID);
        }
        public static List<PerformanceIncentiveEvaluation> Gets(string sSQL, long nUserID)
        {
            return PerformanceIncentiveEvaluation.Service.Gets(sSQL, nUserID);
        }
        public PerformanceIncentiveEvaluation IUD(int nDBOperation, long nUserID)
        {
            return PerformanceIncentiveEvaluation.Service.IUD(this, nDBOperation, nUserID);
        }

        public static List<PerformanceIncentiveEvaluation> Approve(string sPIEIDs, long nUserID)
        {
            return PerformanceIncentiveEvaluation.Service.Approve(sPIEIDs, nUserID);
        }

        public static List<PerformanceIncentiveEvaluation> UploadPIEXL(List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluations, long nUserID)
        {
            return PerformanceIncentiveEvaluation.Service.UploadPIEXL(oPerformanceIncentiveEvaluations, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPerformanceIncentiveEvaluationService Service
        {
            get { return (IPerformanceIncentiveEvaluationService)Services.Factory.CreateService(typeof(IPerformanceIncentiveEvaluationService)); }
        }

        #endregion
    }
    #endregion

    #region IPerformanceIncentiveEvaluation interface

    public interface IPerformanceIncentiveEvaluationService
    {
        PerformanceIncentiveEvaluation Get(int id, Int64 nUserID);
        PerformanceIncentiveEvaluation Get(string sSQL, Int64 nUserID);
        List<PerformanceIncentiveEvaluation> Gets(Int64 nUserID);
        List<PerformanceIncentiveEvaluation> Gets(string sSQL, Int64 nUserID);
        PerformanceIncentiveEvaluation IUD(PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation, int nDBOperation, Int64 nUserID);
        List<PerformanceIncentiveEvaluation> Approve(string sPIEIDs, Int64 nUserID);
        List<PerformanceIncentiveEvaluation> UploadPIEXL(List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluations, Int64 nUserID);
    }
    #endregion
}
