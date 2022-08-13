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
    public class MonthlyAttendance_LeaveService : MarshalByRefObject, IMonthlyAttendance_LeaveService
    {
        #region Private functions and declaration
        private MonthlyAttendance_Leave MapObject(NullHandler oReader)
        {
            MonthlyAttendance_Leave oMonthlyAttendance_Leave = new MonthlyAttendance_Leave();
            oMonthlyAttendance_Leave.EmployeeID = oReader.GetInt32("EmployeeID");
            oMonthlyAttendance_Leave.LeaveID = oReader.GetInt32("LeaveID");
            oMonthlyAttendance_Leave.LeaveName = oReader.GetString("LeaveName");
            oMonthlyAttendance_Leave.Enjoyed = oReader.GetInt32("Enjoyed");

            return oMonthlyAttendance_Leave;
        }

        private MonthlyAttendance_Leave CreateObject(NullHandler oReader)
        {
            MonthlyAttendance_Leave oMonthlyAttendance_Leave = MapObject(oReader);
            return oMonthlyAttendance_Leave;
        }

        private List<MonthlyAttendance_Leave> CreateObjects(IDataReader oReader)
        {
            List<MonthlyAttendance_Leave> oMonthlyAttendance_Leave = new List<MonthlyAttendance_Leave>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MonthlyAttendance_Leave oItem = CreateObject(oHandler);
                oMonthlyAttendance_Leave.Add(oItem);
            }
            return oMonthlyAttendance_Leave;
        }

        #endregion

        #region Interface implementation
        public MonthlyAttendance_LeaveService() { }
        public List<MonthlyAttendance_Leave> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, Int64 nUserID)
        {
            List<MonthlyAttendance_Leave> oMonthlyAttendance_Leave = new List<MonthlyAttendance_Leave>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MonthlyAttendance_LeaveDA.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID, tc);
                oMonthlyAttendance_Leave = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MonthlyAttendance_Leave", e);
                #endregion
            }
            return oMonthlyAttendance_Leave;
        }
        public List<MonthlyAttendance_Leave> Gets_Comp(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, Int64 nUserID)
        {
            List<MonthlyAttendance_Leave> oMonthlyAttendance_Leave = new List<MonthlyAttendance_Leave>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MonthlyAttendance_LeaveDA.Gets_Comp(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, nUserID, tc);
                oMonthlyAttendance_Leave = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MonthlyAttendance_Leave", e);
                #endregion
            }
            return oMonthlyAttendance_Leave;
        }
        #endregion
    }
}
