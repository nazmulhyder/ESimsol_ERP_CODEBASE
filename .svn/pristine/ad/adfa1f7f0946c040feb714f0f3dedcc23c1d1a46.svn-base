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
    public class AttendanceCalendarSessionService : MarshalByRefObject, IAttendanceCalendarSessionService
    {
        #region Private functions and declaration
        private AttendanceCalendarSession MapObject(NullHandler oReader)
        {
            AttendanceCalendarSession oAttendanceCalendarSession = new AttendanceCalendarSession();
            oAttendanceCalendarSession.ACSID = oReader.GetInt32("ACSID");
            oAttendanceCalendarSession.AttendanceCalendarID = oReader.GetInt32("AttendanceCalendarID");
            oAttendanceCalendarSession.Session = oReader.GetString("Session");
            oAttendanceCalendarSession.StartDate = oReader.GetDateTime("StartDate");
            oAttendanceCalendarSession.EndDate = oReader.GetDateTime("EndDate");
            oAttendanceCalendarSession.Description = oReader.GetString("Description");
            oAttendanceCalendarSession.IsActive = oReader.GetBoolean("IsActive");
            return oAttendanceCalendarSession;
        }

        private AttendanceCalendarSession CreateObject(NullHandler oReader)
        {
            AttendanceCalendarSession oAttendanceCalendarSession = MapObject(oReader);
            return oAttendanceCalendarSession;
        }

        private List<AttendanceCalendarSession> CreateObjects(IDataReader oReader)
        {
            List<AttendanceCalendarSession> oAttendanceCalendarSession = new List<AttendanceCalendarSession>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceCalendarSession oItem = CreateObject(oHandler);
                oAttendanceCalendarSession.Add(oItem);
            }
            return oAttendanceCalendarSession;
        }

        #endregion

        #region Interface implementation
        public AttendanceCalendarSessionService() { }

        public AttendanceCalendarSession IUD(AttendanceCalendarSession oAttendanceCalendarSession, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceCalendarSession oNewAttendanceCalendarSession = new AttendanceCalendarSession();
            try
            {
                tc = TransactionContext.Begin(true);
                List<AttendanceCalendarSessionHoliday> oAttendanceCalendarSessionHolidays = new List<AttendanceCalendarSessionHoliday>();
                AttendanceCalendarSessionHoliday oAttendanceCalendarSessionHoliday = new AttendanceCalendarSessionHoliday();
                oAttendanceCalendarSessionHolidays = oAttendanceCalendarSession.AttendanceCalendarSessionHolidays;
                IDataReader reader;
                reader = AttendanceCalendarSessionDA.IUD(tc, oAttendanceCalendarSession, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oNewAttendanceCalendarSession = CreateObject(oReader);
                }
                reader.Close();

                if (nDBOperation != 3)
                {
                    #region AttendanceCalendarSessionHoliday Part
                    foreach (AttendanceCalendarSessionHoliday oItem in oAttendanceCalendarSessionHolidays)
                    {
                        IDataReader readerDetail;
                        oItem.ACSID = oNewAttendanceCalendarSession.ACSID;
                        if (oItem.ACSHID <= 0)
                        {
                            readerDetail = AttendanceCalendarSessionHolidayDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                            NullHandler oReaderDetail = new NullHandler(readerDetail);
                            readerDetail.Close();
                        }
                        if (oItem.ACSHID > 0)
                        {
                            readerDetail = AttendanceCalendarSessionHolidayDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Update);
                            NullHandler oReaderDetail = new NullHandler(readerDetail);
                            readerDetail.Close();
                        }
                    }
                    #endregion
                }

                tc.End();

                if(nDBOperation == (int)EnumDBOperation.Delete){
                    oNewAttendanceCalendarSession = new AttendanceCalendarSession();
                    oNewAttendanceCalendarSession.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewAttendanceCalendarSession.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oNewAttendanceCalendarSession;
        }

        public AttendanceCalendarSession Get(int id, Int64 nUserId)
        {
            AttendanceCalendarSession aAttendanceCalendarSession = new AttendanceCalendarSession();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceCalendarSessionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aAttendanceCalendarSession = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AttendanceCalendarSession", e);
                #endregion
            }

            return aAttendanceCalendarSession;
        }

        public List<AttendanceCalendarSession> Gets(int nAttendanceCalendarID,Int64 nUserID)
        {
            List<AttendanceCalendarSession> oAttendanceCalendarSession = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceCalendarSessionDA.Gets(tc, nAttendanceCalendarID);
                oAttendanceCalendarSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceCalendarSession", e);
                #endregion
            }
            return oAttendanceCalendarSession;
        }
        public List<AttendanceCalendarSession> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceCalendarSession> oAttendanceCalendarSession = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceCalendarSessionDA.Gets(tc, sSQL);
                oAttendanceCalendarSession = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceCalendarSession", e);
                #endregion
            }
            return oAttendanceCalendarSession;
        }

        public List<AttendanceCalendarSession> ChangeActiveStatus(AttendanceCalendarSession oACS, Int64 nUserId)
        {
            List<AttendanceCalendarSession> oAttendanceCalendarSessions = new List<AttendanceCalendarSession>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = AttendanceCalendarSessionDA.ChangeActiveStatus(tc, oACS, nUserId);
                oAttendanceCalendarSessions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oACS = new AttendanceCalendarSession();
                oACS.ErrorMessage = e.Message.Split('!')[0];
                oAttendanceCalendarSessions.Add(oACS);
                #endregion
            }
            return oAttendanceCalendarSessions;
        }
        #endregion
    }
}
