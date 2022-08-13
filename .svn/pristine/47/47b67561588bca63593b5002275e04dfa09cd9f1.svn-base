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
    public class AttendanceRatioReportService : MarshalByRefObject, IAttendanceRatioReportService
    {
        #region Private functions and declaration
        private AttendanceRatioReport MapObject(NullHandler oReader)
        {
            AttendanceRatioReport oAttendanceRatioReport = new AttendanceRatioReport();
            oAttendanceRatioReport.DepartmentID = oReader.GetInt32("DepartmentID");
            oAttendanceRatioReport.Permanent = oReader.GetInt32("Permanent");
            oAttendanceRatioReport.Probationary = oReader.GetInt32("Probationary");
            oAttendanceRatioReport.Contractual = oReader.GetInt32("Contractual");
            oAttendanceRatioReport.DepartmentName = oReader.GetString("DepartmentName");
            oAttendanceRatioReport.ShiftID = oReader.GetInt32("ShiftID");
            oAttendanceRatioReport.ShiftName = oReader.GetString("ShiftName");
            oAttendanceRatioReport.LocationName = oReader.GetString("LocationName");
            oAttendanceRatioReport.Designation = oReader.GetString("Designation");
            oAttendanceRatioReport.Gender = oReader.GetString("Gender");

            oAttendanceRatioReport.EmpTotal = oReader.GetInt32("EmpTotal");
            oAttendanceRatioReport.TotalPresent = oReader.GetInt32("TotalPresent");
            oAttendanceRatioReport.OTPresent = oReader.GetInt32("OTPresent");
            oAttendanceRatioReport.AbsentLeave = oReader.GetInt32("AbsentLeave");
            oAttendanceRatioReport.New = oReader.GetInt32("New");
            oAttendanceRatioReport.Lefty = oReader.GetInt32("Lefty");

            return oAttendanceRatioReport;
        }

        private AttendanceRatioReport CreateObject(NullHandler oReader)
        {
            AttendanceRatioReport oAttendanceRatioReport = MapObject(oReader);
            return oAttendanceRatioReport;
        }

        private List<AttendanceRatioReport> CreateObjects(IDataReader oReader)
        {
            List<AttendanceRatioReport> oAttendanceRatioReport = new List<AttendanceRatioReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceRatioReport oItem = CreateObject(oHandler);
                oAttendanceRatioReport.Add(oItem);
            }
            return oAttendanceRatioReport;
        }

        #endregion

        #region Interface implementation
        public AttendanceRatioReportService() { }

        public List<AttendanceRatioReport> Gets(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, Int64 nUserID)
        {
            List<AttendanceRatioReport> oAttendanceRatioReport = new List<AttendanceRatioReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceRatioReportDA.Gets(dDate, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, sShiftIDs, nUserID, tc);
                oAttendanceRatioReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceRatioReport", e);
                #endregion
            }
            return oAttendanceRatioReport;
        }
        public List<AttendanceRatioReport> GetsComp(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, Int64 nUserID)
        {
            List<AttendanceRatioReport> oAttendanceRatioReport = new List<AttendanceRatioReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceRatioReportDA.GetsComp(dDate, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, sShiftIDs, nUserID, tc);
                oAttendanceRatioReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceRatioReport", e);
                #endregion
            }
            return oAttendanceRatioReport;
        }
        #endregion
    }
}
