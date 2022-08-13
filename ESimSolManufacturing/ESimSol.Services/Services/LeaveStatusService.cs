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
    public class LeaveStatusService : MarshalByRefObject, ILeaveStatusService
    {
        private LeaveStatus MapObject(NullHandler oReader)
        {
            LeaveStatus oLeaveStatus = new LeaveStatus();
            oLeaveStatus.EmployeeID = oReader.GetInt32("EmployeeID");
            oLeaveStatus.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oLeaveStatus.LeaveDays = oReader.GetInt32("LeaveDays");
            oLeaveStatus.LeaveHeadName = oReader.GetString("LeaveHeadName");
            oLeaveStatus.LeaveHeadShortName = oReader.GetString("LeaveHeadShortName");
            return oLeaveStatus;
        }
        private LeaveStatus CreateObject(NullHandler oReader)
        {
            LeaveStatus oLeaveStatus = MapObject(oReader);
            return oLeaveStatus;
        }

        private List<LeaveStatus> CreateObjects(IDataReader oReader)
        {
            List<LeaveStatus> oLeaveStatus = new List<LeaveStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LeaveStatus oItem = CreateObject(oHandler);
                oLeaveStatus.Add(oItem);
            }
            return oLeaveStatus;
        }

        public List<LeaveStatus> Gets(string sSQL, DateTime SalaryStartDate, DateTime SalaryEndDate, long nUserID)
        {
            List<LeaveStatus> oLeaveStatus = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveStatusDA.Gets(sSQL,SalaryStartDate,SalaryEndDate, tc);
                oLeaveStatus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Leave Status", e);
                #endregion
            }
            return oLeaveStatus;
        }
        public List<LeaveStatus> CompGets(string sSQL, int nMOCID, DateTime SalaryStartDate, DateTime SalaryEndDate, long nUserID)
        {
            List<LeaveStatus> oLeaveStatus = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveStatusDA.CompGets(sSQL, nMOCID, SalaryStartDate, SalaryEndDate, tc);
                oLeaveStatus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Leave Status", e);
                #endregion
            }
            return oLeaveStatus;
        }
    }
}
