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
    public class RosterPlanEmployeeV2Service : MarshalByRefObject, IRosterPlanEmployeeV2Service
    {
        private RosterPlanEmployeeV2 MapObject(NullHandler oReader)
        {
            RosterPlanEmployeeV2 oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
            oRosterPlanEmployeeV2.RPEID = oReader.GetInt32("RPEID");
            oRosterPlanEmployeeV2.EmployeeID = oReader.GetInt32("EmployeeID");
            oRosterPlanEmployeeV2.ShiftID = oReader.GetInt32("ShiftID");
            oRosterPlanEmployeeV2.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oRosterPlanEmployeeV2.LocationID = oReader.GetInt32("LocationID");
            oRosterPlanEmployeeV2.DepartmentID = oReader.GetInt32("DepartmentID");
            oRosterPlanEmployeeV2.MaxOTInMin = oReader.GetInt32("MaxOTInMin");
            oRosterPlanEmployeeV2.Remarks = oReader.GetString("Remarks");
            oRosterPlanEmployeeV2.IsDayOff = oReader.GetBoolean("IsDayOff");
            oRosterPlanEmployeeV2.IsPIMSRoaster = oReader.GetBoolean("IsPIMSRoster");
            oRosterPlanEmployeeV2.IsHoliday = oReader.GetBoolean("IsHoliday");
            oRosterPlanEmployeeV2.InTime = oReader.GetDateTime("InTime");
            oRosterPlanEmployeeV2.OutTime = oReader.GetDateTime("OutTime");
            oRosterPlanEmployeeV2.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oRosterPlanEmployeeV2.ShiftStartTime = oReader.GetDateTime("ShiftStartTime");
            oRosterPlanEmployeeV2.ShiftEndTime = oReader.GetDateTime("ShiftEndTime");
            oRosterPlanEmployeeV2.EmployeeCode = oReader.GetString("EmployeeCode");
            oRosterPlanEmployeeV2.EmployeeName = oReader.GetString("EmployeeName");
            oRosterPlanEmployeeV2.ShiftName = oReader.GetString("ShiftName");
            oRosterPlanEmployeeV2.BUName = oReader.GetString("BUName");
            oRosterPlanEmployeeV2.LocationName = oReader.GetString("LocationName");
            oRosterPlanEmployeeV2.Department = oReader.GetString("Department");
            oRosterPlanEmployeeV2.UserName = oReader.GetString("UserName");
            oRosterPlanEmployeeV2.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            return oRosterPlanEmployeeV2;
        }

        private RosterPlanEmployeeV2 CreateObject(NullHandler oReader)
        {
            RosterPlanEmployeeV2 oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
            oRosterPlanEmployeeV2 = MapObject(oReader);
            return oRosterPlanEmployeeV2;
        }

        private List<RosterPlanEmployeeV2> CreateObjects(IDataReader oReader)
        {
            List<RosterPlanEmployeeV2> oRosterPlanEmployeeV2 = new List<RosterPlanEmployeeV2>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RosterPlanEmployeeV2 oItem = CreateObject(oHandler);
                oRosterPlanEmployeeV2.Add(oItem);
            }
            return oRosterPlanEmployeeV2;
        }


        public List<RosterPlanEmployeeV2> Gets(string sSQL, Int64 nUserID)
        {
            List<RosterPlanEmployeeV2> oRosterPlanEmployeeV2 = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterPlanEmployeeV2DA.Gets(sSQL, tc);
                oRosterPlanEmployeeV2 = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oRosterPlanEmployeeV2;
        }


        public RosterPlanEmployeeV2 GetTotalCount(string ssql, long nUserId)
        {
            RosterPlanEmployeeV2 oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
            int nTotal = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                nTotal = RosterPlanEmployeeV2DA.GetTotalCount(tc, ssql, nUserId);
                oRosterPlanEmployeeV2.TotalAttendanceCount = nTotal;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
                oRosterPlanEmployeeV2.ErrorMessage = e.Message;
                #endregion
            }
            return oRosterPlanEmployeeV2;
        }

        public RosterPlanEmployeeV2 UpdateRosterPlanEmployee(RosterPlanEmployeeV2 oRosterPlanEmployeeV2, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RosterPlanEmployeeV2DA.UpdateRosterPlanEmployee(tc, oRosterPlanEmployeeV2, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRosterPlanEmployeeV2 = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }
            return oRosterPlanEmployeeV2;
        }

        public List<RosterPlanEmployeeV2> CommitRosterPlanEmployee(RosterPlanEmployeeV2 oRosterPlanEmployeeV2, Int64 nUserID)
        {
            List<RosterPlanEmployeeV2> oRosterPlanEmployeeV2s = new List<RosterPlanEmployeeV2>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = RosterPlanEmployeeV2DA.CommitRosterPlanEmployee(tc, oRosterPlanEmployeeV2, nUserID);
                oRosterPlanEmployeeV2s = CreateObjects(reader);
                reader.Close();
                

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oRosterPlanEmployeeV2s;
        }
    }
}
