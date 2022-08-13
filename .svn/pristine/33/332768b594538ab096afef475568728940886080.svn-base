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
    public class AttendanceDailyV2Service : MarshalByRefObject, IAttendanceDailyV2Service
    {
        private AttendanceDailyV2 MapObject(NullHandler oReader)
        {
            AttendanceDailyV2 oAttendanceDailyV2 = new AttendanceDailyV2();
            oAttendanceDailyV2.MOCAID = oReader.GetInt32("MOCAID");
            oAttendanceDailyV2.AttendanceID = oReader.GetInt32("AttendanceID");
            oAttendanceDailyV2.EmployeeID = oReader.GetInt32("EmployeeID");
            oAttendanceDailyV2.MOCID = oReader.GetInt32("MOCID");
            oAttendanceDailyV2.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oAttendanceDailyV2.InTime = oReader.GetDateTime("InTime");
            oAttendanceDailyV2.OutTime = oReader.GetDateTime("OutTime");
            oAttendanceDailyV2.OverTimeInMin = oReader.GetInt32("OverTimeInMinute");
            oAttendanceDailyV2.IsDayOff = oReader.GetBoolean("IsDayOff");
            oAttendanceDailyV2.IsHoliday = oReader.GetBoolean("IsHoliday");
            oAttendanceDailyV2.IsLeave = oReader.GetBoolean("IsLeave");
            oAttendanceDailyV2.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oAttendanceDailyV2.TotalWorkingHourInMinute = oReader.GetInt32("TotalWorkingHourInMinute");
            oAttendanceDailyV2.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oAttendanceDailyV2.LocationID = oReader.GetInt32("LocationID");
            oAttendanceDailyV2.DepartmentID = oReader.GetInt32("DepartmentID");
            oAttendanceDailyV2.DesignationID = oReader.GetInt32("DesignationID");
            oAttendanceDailyV2.ShiftID = oReader.GetInt32("ShiftID");
            oAttendanceDailyV2.IsManual = oReader.GetBoolean("IsManual");
            oAttendanceDailyV2.IsOSD = oReader.GetBoolean("IsOSD");
            oAttendanceDailyV2.BUName = oReader.GetString("BUName");
            oAttendanceDailyV2.BUAddress = oReader.GetString("BUAddress");
            oAttendanceDailyV2.LocationName = oReader.GetString("LocationName");
            oAttendanceDailyV2.DepartmentName = oReader.GetString("Department");
            oAttendanceDailyV2.DesignationName = oReader.GetString("Designation");
            oAttendanceDailyV2.ShiftName = oReader.GetString("HRM_Shift");
            oAttendanceDailyV2.LeaveName = oReader.GetString("LeaveName");
            oAttendanceDailyV2.EmployeeCode = oReader.GetString("Code");
            oAttendanceDailyV2.EmployeeName = oReader.GetString("EmployeeName");
            oAttendanceDailyV2.JoiningDate = oReader.GetDateTime("JoiningDate");
            oAttendanceDailyV2.EarlyDepartureMinute = oReader.GetInt32("EarlyDepartureMinute");
            oAttendanceDailyV2.LateArrivalMinute = oReader.GetInt32("LateArrivalMinute");
            oAttendanceDailyV2.IsUnPaid = oReader.GetBoolean("IsUnPaid");
            oAttendanceDailyV2.ShiftStartTime = oReader.GetDateTime("ShiftStartTime");
            oAttendanceDailyV2.ShiftEndTime = oReader.GetDateTime("ShiftEndTime");
            oAttendanceDailyV2.IsManualOT = oReader.GetBoolean("IsManualOT");
            oAttendanceDailyV2.Remark = oReader.GetString("Remark");
            oAttendanceDailyV2.TimeCardName = oReader.GetString("TimeCardName");

            if (oAttendanceDailyV2.ShiftStartTime > oAttendanceDailyV2.ShiftEndTime)
            {
                oAttendanceDailyV2.ShiftEndTime = oAttendanceDailyV2.ShiftEndTime.AddDays(1);
            }
            return oAttendanceDailyV2;
        }

        private AttendanceDailyV2 CreateObject(NullHandler oReader)
        {
            AttendanceDailyV2 oAttendanceDailyV2 = new AttendanceDailyV2();
            oAttendanceDailyV2 = MapObject(oReader);
            return oAttendanceDailyV2;
        }

        private List<AttendanceDailyV2> CreateObjects(IDataReader oReader)
        {
            List<AttendanceDailyV2> oAttendanceDailyV2 = new List<AttendanceDailyV2>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceDailyV2 oItem = CreateObject(oHandler);
                oAttendanceDailyV2.Add(oItem);
            }
            return oAttendanceDailyV2;
        }
        public List<AttendanceDailyV2> CompGets(string sSQL, Int64 nUserID)
        {
            List<AttendanceDailyV2> oAttendanceDailyV2 = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyV2DA.CompGets(sSQL, tc);
                oAttendanceDailyV2 = CreateObjects(reader);
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
            return oAttendanceDailyV2;
        }

        public List<AttendanceDailyV2> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceDailyV2> oAttendanceDailyV2 = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyV2DA.Gets(sSQL, tc);
                oAttendanceDailyV2 = CreateObjects(reader);
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
            return oAttendanceDailyV2;
        }

        public AttendanceDailyV2 AttendanceDaily_Manual_Single(AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyV2DA.AttendanceDaily_Manual_Single(tc, oAttendanceDailyV2, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDailyV2 = CreateObject(oReader);
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
            return oAttendanceDailyV2;
        }

        public AttendanceDailyV2 AttendanceDaily_Manual_Single_Comp(AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyV2DA.AttendanceDaily_Manual_Single_Comp(tc, oAttendanceDailyV2, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDailyV2 = CreateObject(oReader);
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
            return oAttendanceDailyV2;
        }

        public string AssignLeave(AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AttendanceDailyV2DA.AssignLeave(tc, oAttendanceDailyV2, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data Commit successfully";
        }

        public AttendanceDailyV2 GetTotalCount(string ssql, long nUserId)
        {
            AttendanceDailyV2 oAttendanceDailyV2 = new AttendanceDailyV2();
            int nTotal = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                nTotal = AttendanceDailyV2DA.GetTotalCount(tc, ssql, nUserId);
                oAttendanceDailyV2.TotalAttendanceCount = nTotal;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDailyV2 = new AttendanceDailyV2();
                oAttendanceDailyV2.ErrorMessage = e.Message;
                #endregion
            }
            return oAttendanceDailyV2;
        }
    }
}
