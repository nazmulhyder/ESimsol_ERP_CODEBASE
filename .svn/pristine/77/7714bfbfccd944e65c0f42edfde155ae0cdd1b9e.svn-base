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
    public class LeaveLedgerReportService : MarshalByRefObject, ILeaveLedgerReportService
    {
        #region Private functions and declaration
        private LeaveLedgerReport MapObject(NullHandler oReader)
        {
            LeaveLedgerReport oLeaveLedgerReport = new LeaveLedgerReport();

            oLeaveLedgerReport.EmpLeaveLedgerID = oReader.GetInt32("EmpLeaveLedgerID");
            oLeaveLedgerReport.LeaveDuration = oReader.GetInt32("LeaveDuration");
            oLeaveLedgerReport.EmployeeID = oReader.GetInt32("EmployeeID");
            oLeaveLedgerReport.EmployeeName = oReader.GetString("EmployeeName");
            oLeaveLedgerReport.LocationName = oReader.GetString("LocationName");
            oLeaveLedgerReport.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oLeaveLedgerReport.BUName = oReader.GetString("BUName");
            oLeaveLedgerReport.EmployeeCode = oReader.GetString("EmployeeCode");
            oLeaveLedgerReport.DepartmentName = oReader.GetString("DepartmentName");
            oLeaveLedgerReport.DesignationName = oReader.GetString("DesignationName");
            oLeaveLedgerReport.JoiningDate = oReader.GetDateTime("JoiningDate");
            oLeaveLedgerReport.ApplicationDate = oReader.GetDateTime("ApplicationDate");
            oLeaveLedgerReport.StartDate = oReader.GetDateTime("StartDate");
            oLeaveLedgerReport.EndDate = oReader.GetDateTime("EndDate");
            oLeaveLedgerReport.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oLeaveLedgerReport.LeaveType = (EnumLeaveType)oReader.GetInt32("LeaveType");
            oLeaveLedgerReport.EnjoyedLeaveSalaryMonth = oReader.GetInt32("EnjoyedLeaveSalaryMonth");
            oLeaveLedgerReport.LeaveShortName = oReader.GetString("LeaveShortName");
            oLeaveLedgerReport.LeaveName = oReader.GetString("LeaveName");
            oLeaveLedgerReport.TotalLeave = oReader.GetDouble("TotalLeave");
            oLeaveLedgerReport.Full_Enjoyed = oReader.GetDouble("Full_Enjoyed");
            oLeaveLedgerReport.Half_Enjoyed = oReader.GetDouble("Half_Enjoyed");
            oLeaveLedgerReport.Short_Enjoyed = oReader.GetDouble("Short_Enjoyed");
            oLeaveLedgerReport.Full_Balance = oReader.GetDouble("Full_Balance");
            oLeaveLedgerReport.Half_Balance = oReader.GetDouble("Half_Balance");
            oLeaveLedgerReport.Short_Balance = oReader.GetDouble("Short_Balance");
            //oLeaveLedgerReport.Enjoyed = oReader.GetDouble("Enjoyed");

            return oLeaveLedgerReport;
        }

        private LeaveLedgerReport CreateObject(NullHandler oReader)
        {
            LeaveLedgerReport oLeaveLedgerReport = MapObject(oReader);
            return oLeaveLedgerReport;
        }

        private List<LeaveLedgerReport> CreateObjects(IDataReader oReader)
        {
            List<LeaveLedgerReport> oLeaveLedgerReport = new List<LeaveLedgerReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LeaveLedgerReport oItem = CreateObject(oHandler);
                oLeaveLedgerReport.Add(oItem);
            }
            return oLeaveLedgerReport;
        }

        #endregion

        #region Interface implementation
        public LeaveLedgerReportService() { }
        public List<LeaveLedgerReport> Gets(string sEmployeeIDs, string sDepartmentIds, string sDesignationIds, int ACSID, int nLeaveHeadID, double nBalanceAmount, int nBalanceType, bool bReportingPerson, DateTime dtFrom, DateTime dtTo, bool bDate, bool IsActive, bool IsInActive, string sBUnit, string sLocationID, Int64 nUserID)
        {
            List<LeaveLedgerReport> oLeaveLedgerReport = new List<LeaveLedgerReport>();
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LeaveLedgerReportDA.Gets(sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID, nUserID, tc);
                oLeaveLedgerReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LeaveLedgerReport", e);
                #endregion
            }
            return oLeaveLedgerReport;
        }

        public List<LeaveLedgerReport> GetLeaveWithEnjoyBalance(string sBUIDs, string sLocIDs, string sDeptIDs, string sDesgIDs, string sEmployeeIDs, DateTime sStartDate, DateTime sEndDate, int nApplicationNature, int nLeaveHeadId, int nLeaveType, int nLeaveStatus, int nIsPaid, int nIsUnPaid, Int64 nUserID)
        {
            List<LeaveLedgerReport> oELLs = new List<LeaveLedgerReport>();
            LeaveLedgerReport oELL = new LeaveLedgerReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveLedgerReportDA.GetLeaveWithEnjoyBalance(tc, sBUIDs, sLocIDs, sDeptIDs, sDesgIDs, sEmployeeIDs, sStartDate, sEndDate,
                nApplicationNature, nLeaveHeadId, nLeaveType, nLeaveStatus, nIsPaid, nIsUnPaid, nUserID);
                oELLs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELL.ErrorMessage = ex.Message;
                oELLs.Add(oELL);
                #endregion
            }

            return oELLs;
        }
        public List<LeaveLedgerReport> GetsComp(string sEmployeeIDs, string sDepartmentIds, string sDesignationIds, int ACSID, int nLeaveHeadID, double nBalanceAmount, int nBalanceType, bool bReportingPerson, DateTime dtFrom, DateTime dtTo, bool bDate, bool IsActive, bool IsInActive, string sBUnit, string sLocationID, Int64 nUserID)
        {
            List<LeaveLedgerReport> oLeaveLedgerReport = new List<LeaveLedgerReport>();
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LeaveLedgerReportDA.GetsComp(sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID, nUserID, tc);
                oLeaveLedgerReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LeaveLedgerReport", e);
                #endregion
            }
            return oLeaveLedgerReport;
        }
        #endregion
    }
}
