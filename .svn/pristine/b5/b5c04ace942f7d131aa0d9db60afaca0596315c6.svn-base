using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class BenefitOnAttendanceReportService : MarshalByRefObject, IBenefitOnAttendanceReportService
    {
        #region Private functions and declaration
        private BenefitOnAttendanceReport MapObject(NullHandler oReader)
        {
            BenefitOnAttendanceReport oBenefitOnAttendanceReport = new BenefitOnAttendanceReport();
            oBenefitOnAttendanceReport.BOAName = oReader.GetString("BOAName");
            oBenefitOnAttendanceReport.EmployeeID = oReader.GetInt32("EmployeeID");
            oBenefitOnAttendanceReport.EmployeeCode = oReader.GetString("EmployeeCode");
            oBenefitOnAttendanceReport.EmployeeName = oReader.GetString("EmployeeName");
            oBenefitOnAttendanceReport.DesignationName = oReader.GetString("DesignationName");
            oBenefitOnAttendanceReport.DepartmentName = oReader.GetString("DepartmentName");
            oBenefitOnAttendanceReport.JoiningDate = oReader.GetDateTime("JoiningDate");
            oBenefitOnAttendanceReport.TotalDay = oReader.GetInt32("TotalDay");

            oBenefitOnAttendanceReport.Benefit = oReader.GetString("Benefit");
            oBenefitOnAttendanceReport.Amount = oReader.GetInt32("Amount");
            oBenefitOnAttendanceReport.IsActive = oReader.GetBoolean("IsActive");

            return oBenefitOnAttendanceReport;
        }

        private BenefitOnAttendanceReport CreateObject(NullHandler oReader)
        {
            BenefitOnAttendanceReport oBenefitOnAttendanceReport = MapObject(oReader);
            return oBenefitOnAttendanceReport;
        }

        private List<BenefitOnAttendanceReport> CreateObjects(IDataReader oReader)
        {
            List<BenefitOnAttendanceReport> oBenefitOnAttendanceReports = new List<BenefitOnAttendanceReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BenefitOnAttendanceReport oItem = CreateObject(oHandler);
                oBenefitOnAttendanceReports.Add(oItem);
            }
            return oBenefitOnAttendanceReports;
        }
        #endregion

        #region Interface implementation
        public BenefitOnAttendanceReportService() { }
        public List<BenefitOnAttendanceReport> Gets(DateTime StartDate, DateTime EndDate, string BOAIDs, string sEmployeeIDs, string  sLocationID, string sDepartmentIDs, string sBusinessUnitIDs, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID)
        {
            List<BenefitOnAttendanceReport> oBenefitOnAttendanceReports = new List<BenefitOnAttendanceReport>();
            BenefitOnAttendanceReport oBenefitOnAttendanceReport = new BenefitOnAttendanceReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BenefitOnAttendanceReportDA.Gets(StartDate, EndDate, BOAIDs, sEmployeeIDs,sLocationID, sDepartmentIDs,sBusinessUnitIDs, nStartSalaryRange, nEndSalaryRange, nUserID, tc);
                oBenefitOnAttendanceReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oBenefitOnAttendanceReports = new List<BenefitOnAttendanceReport>();
                oBenefitOnAttendanceReport = new BenefitOnAttendanceReport();
                oBenefitOnAttendanceReports.Add(oBenefitOnAttendanceReport);
                #endregion
            }
            return oBenefitOnAttendanceReports;
        }
        #endregion
    }
}
