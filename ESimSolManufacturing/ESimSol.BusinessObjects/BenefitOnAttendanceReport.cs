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
    #region BenefitOnAttendanceReport

    public class BenefitOnAttendanceReport : BusinessObject
    {
        public BenefitOnAttendanceReport()
        {
            BOAName = "";
            EmployeeID = 0;
            EmployeeCode = "";
            EmployeeName = "";
            TotalDay = 0;
            ErrorMessage = "";
            JoiningDate = DateTime.Now;
            DesignationName = "";
            DepartmentName = "";

            Benefit = "";
            Amount = 0;
            IsActive = false;
        }

        #region Properties
        public string BOAName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime JoiningDate { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int TotalDay { get; set; }
        public string ErrorMessage { get; set; }
        public string Benefit { get; set; }
        public double Amount { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region Derived Property
        public List<BenefitOnAttendanceReport> BORs { get; set; }
        public Company Company { get; set; }
        public List<CellRowSpan> CellRowSpans { get; set; }
        public string WorkingStatusInST { get { return (this.IsActive) ? "Continued" : "Discontinued"; } }
        #endregion

        #region Functions
        public static List<BenefitOnAttendanceReport> Gets(DateTime StartDate, DateTime EndDate, string BOAIDs, string sEmployeeIDs, string sLocationID, string sDepartmentIDs, string sBusinessUnitIDs, double nStartSalaryRange, double nEndSalaryRange, long nUserID)
        {
            return BenefitOnAttendanceReport.Service.Gets(StartDate, EndDate, BOAIDs, sEmployeeIDs,sLocationID, sDepartmentIDs,sBusinessUnitIDs, nStartSalaryRange, nEndSalaryRange, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBenefitOnAttendanceReportService Service
        {
            get { return (IBenefitOnAttendanceReportService)Services.Factory.CreateService(typeof(IBenefitOnAttendanceReportService)); }
        }

        #endregion
    }
    #endregion

    #region IBenefitOnAttendanceReport interface

    public interface IBenefitOnAttendanceReportService
    {
        List<BenefitOnAttendanceReport> Gets(DateTime StartDate, DateTime EndDate, string BOAIDs, string sEmployeeIDs, string sLocationID, string sDepartmentIDs, string sBusinessUnitIDs, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID);
    }

    #endregion
}
