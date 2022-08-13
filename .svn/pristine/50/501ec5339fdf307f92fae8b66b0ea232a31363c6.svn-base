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
    public class MonthlyAttendanceReportService : MarshalByRefObject, IMonthlyAttendanceReportService
    {
        #region Private functions and declaration
        private MonthlyAttendanceReport MapObject(NullHandler oReader)
        {
            MonthlyAttendanceReport oMonthlyAttendanceReport = new MonthlyAttendanceReport();
            oMonthlyAttendanceReport.EmployeeID = oReader.GetInt32("EmployeeID");
            oMonthlyAttendanceReport.EmployeeName = oReader.GetString("EmployeeName");
            oMonthlyAttendanceReport.EmployeeCode = oReader.GetString("EmployeeCode");
            oMonthlyAttendanceReport.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oMonthlyAttendanceReport.LocationName = oReader.GetString("LocationName");
            oMonthlyAttendanceReport.DepartmentName = oReader.GetString("DepartmentName");
            oMonthlyAttendanceReport.DesignationName = oReader.GetString("DesignationName");
            oMonthlyAttendanceReport.Education = oReader.GetString("Education");
            oMonthlyAttendanceReport.JoiningDate = oReader.GetDateTime("JoiningDate");
            oMonthlyAttendanceReport.TotalWorkingDay = oReader.GetInt32("TotalWorkingDay");
            oMonthlyAttendanceReport.PresentDay = oReader.GetInt32("PresentDay");
            oMonthlyAttendanceReport.AbsentDay = oReader.GetInt32("AbsentDay");
            oMonthlyAttendanceReport.DayOFF = oReader.GetInt32("DayOFF");
            oMonthlyAttendanceReport.HoliDay = oReader.GetInt32("HoliDay");
            oMonthlyAttendanceReport.Leave = oReader.GetInt32("Leave");
            oMonthlyAttendanceReport.LeaveHalfShort = oReader.GetInt32("LeaveHalfShort");
            oMonthlyAttendanceReport.NoWork = oReader.GetInt32("NoWork");
            //oMonthlyAttendanceReport.OvertimeInhour = oReader.GetDouble("OvertimeInhour");
            oMonthlyAttendanceReport.IsActive = oReader.GetBoolean("IsActive");

            oMonthlyAttendanceReport.EarlyOutDays = oReader.GetInt32("EarlyOutDays");
            oMonthlyAttendanceReport.EarlyOutMins = oReader.GetInt32("EarlyOutMins");
            oMonthlyAttendanceReport.LateDays = oReader.GetInt32("LateDays");
            oMonthlyAttendanceReport.LateMins = oReader.GetInt32("LateMins");
            oMonthlyAttendanceReport.ExcessMins = oReader.GetInt32("ExcessMins");
            oMonthlyAttendanceReport.DisAction = oReader.GetInt32("DisAction");
            oMonthlyAttendanceReport.NOTMin = oReader.GetInt32("NOTMin");
            oMonthlyAttendanceReport.HOTMin = oReader.GetInt32("HOTMin");

            oMonthlyAttendanceReport.BUName = oReader.GetString("BUName");
            oMonthlyAttendanceReport.RemarkWithCount = oReader.GetString("RemarkWithCount");
            oMonthlyAttendanceReport.RegularOT_Hr = oReader.GetDouble("RegularOT_Hr");
            oMonthlyAttendanceReport.ExtraOT_Hr = oReader.GetDouble("ExtraOT_Hr");
            oMonthlyAttendanceReport.OT_Hr = oReader.GetDouble("OT_Hr");
            oMonthlyAttendanceReport.OT_Rate = oReader.GetDouble("OTRate");
            oMonthlyAttendanceReport.OT_Amount = oReader.GetDouble("OT_Amount");
            oMonthlyAttendanceReport.PaymentDay = oReader.GetInt32("PaymentDay");
            oMonthlyAttendanceReport.GrossSalary = oReader.GetDouble("GrossSalary");
            oMonthlyAttendanceReport.PerDaySalary = oReader.GetDouble("PerDaySalary");
            oMonthlyAttendanceReport.PerDaySalaryBasic = oReader.GetDouble("PerDaySalaryBasic");
            oMonthlyAttendanceReport.SearchingDay = oReader.GetInt32("SearchingDay");
            oMonthlyAttendanceReport.GrossSalaryBasedOnPresent = oReader.GetDouble("GrossSalaryBasedOnPresent");
            oMonthlyAttendanceReport.TotalAmount = oReader.GetDouble("TotalAmount");

            return oMonthlyAttendanceReport;
        }

        private MonthlyAttendanceReport CreateObject(NullHandler oReader)
        {
            MonthlyAttendanceReport oMonthlyAttendanceReport = MapObject(oReader);
            return oMonthlyAttendanceReport;
        }

        private List<MonthlyAttendanceReport> CreateObjects(IDataReader oReader)
        {
            List<MonthlyAttendanceReport> oMonthlyAttendanceReport = new List<MonthlyAttendanceReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MonthlyAttendanceReport oItem = CreateObject(oHandler);
                oMonthlyAttendanceReport.Add(oItem);
            }
            return oMonthlyAttendanceReport;
        }

        #endregion

        #region Interface implementation
        public MonthlyAttendanceReportService() { }

        public List<MonthlyAttendanceReport> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID)
        {
            List<MonthlyAttendanceReport> oMonthlyAttendanceReport = new List<MonthlyAttendanceReport>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MonthlyAttendanceReportDA.Gets(sEmployeeIDs,sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo,sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks,nUserID, tc);
                oMonthlyAttendanceReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MonthlyAttendanceReport", e);
                #endregion
            }
            return oMonthlyAttendanceReport;
        }

        public List<MonthlyAttendanceReport> Gets_F3(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, Int64 nUserID)
        {
            List<MonthlyAttendanceReport> oMonthlyAttendanceReports = new List<MonthlyAttendanceReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MonthlyAttendanceReportDA.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, 0, 0,"", "", nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    MonthlyAttendanceReport oItem = new MonthlyAttendanceReport();
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.BusinessUnitID = oreader.GetInt32("BusinessUnitID");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.Education = oreader.GetString("Education");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oItem.TotalWorkingDay = oreader.GetInt32("TotalWorkingDay");
                    oItem.PresentDay = oreader.GetInt32("PresentDay");
                    oItem.AbsentDay = oreader.GetInt32("AbsentDay");
                    oItem.DayOFF = oreader.GetInt32("DayOFF");
                    oItem.HoliDay = oreader.GetInt32("HoliDay");
                    oItem.Leave = oreader.GetInt32("Leave");
                    oItem.LeaveHalfShort = oreader.GetInt32("LeaveHalfShort");
                    oItem.NoWork = oreader.GetInt32("NoWork");
                    oItem.IsActive = oreader.GetBoolean("IsActive");
                    oItem.RemarkWithCount = oreader.GetString("RemarkWithCount");

                    oItem.EarlyOutDays = oreader.GetInt32("EarlyOutDays");
                    oItem.EarlyOutMins = oreader.GetInt32("EarlyOutMins");
                    oItem.LateDays = oreader.GetInt32("LateDays");
                    oItem.LateMins = oreader.GetInt32("LateMins");
                    oItem.ExcessMins = oreader.GetInt32("ExcessMins");
                    oItem.DisAction = oreader.GetInt32("DisAction");

                    oItem.ConfirmationDate = oreader.GetDateTime("ConfirmationDate");
                    oItem.LastWorkingDate = oreader.GetDateTime("LastWorkingDate");
                    oItem.EmployeeType = oreader.GetString("EmployeeType");
                    oItem.EmployeeCategory = (EnumEmployeeCategory)oreader.GetInt32("EmployeeCategory");
                    oItem.ReportingPerson = oreader.GetString("ReportingPerson");
                    oItem.NOTMin = oreader.GetInt32("NOTMin");
                    oItem.HOTMin = oreader.GetInt32("HOTMin");
                    oItem.NightAllDay = oreader.GetInt32("NightAllDay");

                    oMonthlyAttendanceReports.Add(oItem);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MonthlyAttendanceReport", e);
                #endregion
            }
            return oMonthlyAttendanceReports;
        }

        public List<MonthlyAttendanceReport> Gets_F3_Comp(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, Int64 nUserID)
        {
            List<MonthlyAttendanceReport> oMonthlyAttendanceReports = new List<MonthlyAttendanceReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MonthlyAttendanceReportDA.Gets_F3_Comp(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    MonthlyAttendanceReport oItem = new MonthlyAttendanceReport();
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.BusinessUnitID = oreader.GetInt32("BusinessUnitID");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.Education = oreader.GetString("Education");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oItem.TotalWorkingDay = oreader.GetInt32("TotalWorkingDay");
                    oItem.PresentDay = oreader.GetInt32("PresentDay");
                    oItem.AbsentDay = oreader.GetInt32("AbsentDay");
                    oItem.DayOFF = oreader.GetInt32("DayOFF");
                    oItem.HoliDay = oreader.GetInt32("HoliDay");
                    oItem.Leave = oreader.GetInt32("Leave");
                    oItem.LeaveHalfShort = oreader.GetInt32("LeaveHalfShort");
                    oItem.NoWork = oreader.GetInt32("NoWork");
                    oItem.IsActive = oreader.GetBoolean("IsActive");

                    oItem.RemarkWithCount = oreader.GetString("RemarkWithCount");
                    oItem.EarlyOutDays = oreader.GetInt32("EarlyOutDays");
                    oItem.EarlyOutMins = oreader.GetInt32("EarlyOutMins");
                    oItem.LateDays = oreader.GetInt32("LateDays");
                    oItem.LateMins = oreader.GetInt32("LateMins");
                    oItem.ExcessMins = oreader.GetInt32("ExcessMins");
                    oItem.DisAction = oreader.GetInt32("DisAction");

                    oItem.ConfirmationDate = oreader.GetDateTime("ConfirmationDate");
                    oItem.LastWorkingDate = oreader.GetDateTime("LastWorkingDate");
                    oItem.EmployeeType = oreader.GetString("EmployeeType");
                    oItem.EmployeeCategory = (EnumEmployeeCategory)oreader.GetInt32("EmployeeCategory");
                    oItem.ReportingPerson = oreader.GetString("ReportingPerson");
                    oItem.NOTMin = oreader.GetInt32("NOTMin");
                    oItem.HOTMin = oreader.GetInt32("HOTMin");
                    oItem.NightAllDay = oreader.GetInt32("NightAllDay");

                    oMonthlyAttendanceReports.Add(oItem);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MonthlyAttendanceReport", e);
                #endregion
            }
            return oMonthlyAttendanceReports;
        }

        #endregion
    }
}
