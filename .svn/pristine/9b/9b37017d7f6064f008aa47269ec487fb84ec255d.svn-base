using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class HolidayCalendarService : MarshalByRefObject, IHolidayCalendarService
    {
        private HolidayCalendar MapObject(NullHandler oReader)
        {
            HolidayCalendar oHolidayCalendar = new HolidayCalendar();
            oHolidayCalendar.HolidayCalendarID = oReader.GetInt32("HolidayCalendarID");
            oHolidayCalendar.CalendarNo = oReader.GetString("CalendarNo");
            oHolidayCalendar.CalendarName = oReader.GetString("CalendarName");
            oHolidayCalendar.StartDate = oReader.GetDateTime("StartDate");
            oHolidayCalendar.EndDate = oReader.GetDateTime("EndDate");
            oHolidayCalendar.Remarks = oReader.GetString("Remarks");
            oHolidayCalendar.TotalHolidays = oReader.GetInt32("TotalHolidays");
            return oHolidayCalendar;
        }
        private HolidayCalendar CreateObject(NullHandler oReader)
        {
            HolidayCalendar oHolidayCalendar = new HolidayCalendar();
            oHolidayCalendar = MapObject(oReader);
            return oHolidayCalendar;
        }

        private List<HolidayCalendar> CreateObjects(IDataReader oReader)
        {
            List<HolidayCalendar> oHolidayCalendar = new List<HolidayCalendar>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HolidayCalendar oItem = CreateObject(oHandler);
                oHolidayCalendar.Add(oItem);
            }
            return oHolidayCalendar;
        }

        public HolidayCalendar Save(HolidayCalendar oHolidayCalendar, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oHolidayCalendar.HolidayCalendarID <= 0)
                {
                    reader = HolidayCalendarDA.InsertUpdate(tc, oHolidayCalendar, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = HolidayCalendarDA.InsertUpdate(tc, oHolidayCalendar, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHolidayCalendar = new HolidayCalendar();
                    oHolidayCalendar = CreateObject(oReader);
                }
                reader.Close();

                #region Get Holiday Calendar
                reader = HolidayCalendarDA.Get(tc, oHolidayCalendar.HolidayCalendarID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHolidayCalendar = new HolidayCalendar();
                    oHolidayCalendar = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oHolidayCalendar = new HolidayCalendar();
                    oHolidayCalendar.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oHolidayCalendar;
        }

        public HolidayCalendar Get(int id, Int64 nUserId)
        {
            HolidayCalendar oHolidayCalendar = new HolidayCalendar();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HolidayCalendarDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHolidayCalendar = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Holiday Calendar", e);
                #endregion
            }

            return oHolidayCalendar;
        }
        public List<HolidayCalendar> Gets(string sSQL, Int64 nUserID)
        {
            List<HolidayCalendar> oHolidayCalendar = new List<HolidayCalendar>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = HolidayCalendarDA.Gets(tc, sSQL);
                oHolidayCalendar = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get HolidayCalendar", e);
                #endregion
            }
            return oHolidayCalendar;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                HolidayCalendar oHolidayCalendar = new HolidayCalendar();
                oHolidayCalendar.HolidayCalendarID = id;
                HolidayCalendarDA.Delete(tc, oHolidayCalendar, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

    }
}
