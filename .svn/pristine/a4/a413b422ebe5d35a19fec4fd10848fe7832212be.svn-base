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
    public class AttendanceCalendarService : MarshalByRefObject, IAttendanceCalendarService
    {
        #region Private functions and declaration
        private AttendanceCalendar MapObject(NullHandler oReader)
        {
            AttendanceCalendar oAttendanceCalendar = new AttendanceCalendar();
            oAttendanceCalendar.AttendanceCalendarID = oReader.GetInt32("AttendanceCalendarID");
            oAttendanceCalendar.Name = oReader.GetString("Name");
            oAttendanceCalendar.Code = oReader.GetInt32("Code");
            oAttendanceCalendar.Description = oReader.GetString("Description");
            oAttendanceCalendar.IsActive = oReader.GetBoolean("IsActive");
            return oAttendanceCalendar;
        }

        private AttendanceCalendar CreateObject(NullHandler oReader)
        {
            AttendanceCalendar oAttendanceCalendar = MapObject(oReader);
            return oAttendanceCalendar;
        }

        private List<AttendanceCalendar> CreateObjects(IDataReader oReader)
        {
            List<AttendanceCalendar> oAttendanceCalendar = new List<AttendanceCalendar>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceCalendar oItem = CreateObject(oHandler);
                oAttendanceCalendar.Add(oItem);
            }
            return oAttendanceCalendar;
        }

        #endregion

        #region Interface implementation
        public AttendanceCalendarService() { }

        public AttendanceCalendar IUD(AttendanceCalendar oAttendanceCalendar, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
            
                reader = AttendanceCalendarDA.IUD(tc, oAttendanceCalendar, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceCalendar = new AttendanceCalendar();
                    oAttendanceCalendar = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oAttendanceCalendar = new AttendanceCalendar();
                    oAttendanceCalendar.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save AttendanceCalendar. Because of " + e.Message, e);
                #endregion
            }
            return oAttendanceCalendar;
        }

        public AttendanceCalendar Get(int id, Int64 nUserId)
        {
            AttendanceCalendar aAttendanceCalendar = new AttendanceCalendar();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceCalendarDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aAttendanceCalendar = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AttendanceCalendar", e);
                #endregion
            }

            return aAttendanceCalendar;
        }

        public List<AttendanceCalendar> Gets(Int64 nUserID)
        {
            List<AttendanceCalendar> oAttendanceCalendar = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceCalendarDA.Gets(tc);
                oAttendanceCalendar = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceCalendar", e);
                #endregion
            }

            return oAttendanceCalendar;
        }

        public List<AttendanceCalendar> Gets(string sSQL,Int64 nUserID)
        {
            List<AttendanceCalendar> oAttendanceCalendar = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceCalendarDA.Gets(tc, sSQL);
                oAttendanceCalendar = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceCalendar", e);
                #endregion
            }

            return oAttendanceCalendar;
        }
        #endregion
    }
}