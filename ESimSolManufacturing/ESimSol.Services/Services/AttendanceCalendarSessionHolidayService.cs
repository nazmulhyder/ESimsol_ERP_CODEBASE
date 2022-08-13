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
    public class AttendanceCalendarSessionHolidayService : MarshalByRefObject, IAttendanceCalendarSessionHolidayService
    {
        #region Private functions and declaration
        private AttendanceCalendarSessionHoliday MapObject(NullHandler oReader)
        {
            AttendanceCalendarSessionHoliday oAttendanceCalendarSessionHoliday = new AttendanceCalendarSessionHoliday();
            oAttendanceCalendarSessionHoliday.ACSHID = oReader.GetInt32("ACSHID");
            oAttendanceCalendarSessionHoliday.ACSID = oReader.GetInt32("ACSID");
            oAttendanceCalendarSessionHoliday.HolidayID = oReader.GetInt32("HolidayID");
            oAttendanceCalendarSessionHoliday.StartDate = oReader.GetDateTime("StartDate");
            oAttendanceCalendarSessionHoliday.EndDate = oReader.GetDateTime("EndDate");
            oAttendanceCalendarSessionHoliday.IsActive = oReader.GetBoolean("IsActive");
            oAttendanceCalendarSessionHoliday.HolidayDescription = oReader.GetString("HolidayDescription");
            

            return oAttendanceCalendarSessionHoliday;
        }

        private AttendanceCalendarSessionHoliday CreateObject(NullHandler oReader)
        {
            AttendanceCalendarSessionHoliday oAttendanceCalendarSessionHoliday = MapObject(oReader);
            return oAttendanceCalendarSessionHoliday;
        }

        private List<AttendanceCalendarSessionHoliday> CreateObjects(IDataReader oReader)
        {
            List<AttendanceCalendarSessionHoliday> oAttendanceCalendarSessionHoliday = new List<AttendanceCalendarSessionHoliday>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceCalendarSessionHoliday oItem = CreateObject(oHandler);
                oAttendanceCalendarSessionHoliday.Add(oItem);
            }
            return oAttendanceCalendarSessionHoliday;
        }

        #endregion

        #region Interface implementation
        public AttendanceCalendarSessionHolidayService() { }

        public AttendanceCalendarSessionHoliday Get(int id, Int64 nUserId)
        {
            AttendanceCalendarSessionHoliday aAttendanceCalendarSessionHoliday = new AttendanceCalendarSessionHoliday();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceCalendarSessionHolidayDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aAttendanceCalendarSessionHoliday = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RosterPlan", e);
                #endregion
            }

            return aAttendanceCalendarSessionHoliday;
        }

        public List<AttendanceCalendarSessionHoliday> Gets(int nACSID, Int64 nUserID)
        {
            List<AttendanceCalendarSessionHoliday> oAttendanceCalendarSessionHoliday = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceCalendarSessionHolidayDA.Gets(tc, nACSID);
                oAttendanceCalendarSessionHoliday = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceCalendarSessionHoliday", e);
                #endregion
            }

            return oAttendanceCalendarSessionHoliday;
        }
        public List<AttendanceCalendarSessionHoliday> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceCalendarSessionHoliday> oAttendanceCalendarSessionHoliday = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceCalendarSessionHolidayDA.Gets(tc, sSQL);
                oAttendanceCalendarSessionHoliday = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceCalendarSessionHoliday", e);
                #endregion
            }

            return oAttendanceCalendarSessionHoliday;
        }
        public AttendanceCalendarSessionHoliday IUD(AttendanceCalendarSessionHoliday oAttendanceCalendarSessionHoliday, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceCalendarSessionHoliday oNewAttendanceCalendarSessionHoliday = new AttendanceCalendarSessionHoliday();
            try
            {
                tc = TransactionContext.Begin(true);

                //AttendanceCalendarSessionHoliday oAttendanceCalendarSessionHoliday = new AttendanceCalendarSessionHoliday();
                //oAttendanceCalendarSessionHolidays = oRosterPlan.AttendanceCalendarSessionHolidays;
                IDataReader reader;
                reader = AttendanceCalendarSessionHolidayDA.IUD(tc, oAttendanceCalendarSessionHoliday, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oNewAttendanceCalendarSessionHoliday = CreateObject(oReader);
                }
                reader.Close();


                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oNewAttendanceCalendarSessionHoliday = new AttendanceCalendarSessionHoliday();
                    oNewAttendanceCalendarSessionHoliday.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewAttendanceCalendarSessionHoliday.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oNewAttendanceCalendarSessionHoliday;
        }
        #endregion
    }
}
