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
    public class HolidayService : MarshalByRefObject, IHolidayService
    {
        #region Private functions and declaration
        private Holiday MapObject(NullHandler oReader)
        {
            Holiday oHoliday = new Holiday();
            oHoliday.HolidayID = oReader.GetInt32("HolidayID");
            oHoliday.Code = oReader.GetInt32("Code");
            oHoliday.Description = oReader.GetString("Description");
            oHoliday.DayOfMonth = oReader.GetString("DayOfMonth");
            oHoliday.TypeOfHoliday = (EnumHolidayType)oReader.GetInt16("TypeOfHoliday");
            oHoliday.IsActive = oReader.GetBoolean("IsActive");
            return oHoliday;
        }

        private Holiday CreateObject(NullHandler oReader)
        {
            Holiday oHoliday = MapObject(oReader);
            return oHoliday;
        }

        private List<Holiday> CreateObjects(IDataReader oReader)
        {
            List<Holiday> oHoliday = new List<Holiday>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Holiday oItem = CreateObject(oHandler);
                oHoliday.Add(oItem);
            }
            return oHoliday;
        }

        #endregion

        #region Interface implementation
        public HolidayService() { }

        public Holiday Save(Holiday oHoliday, Int64 nUserID)
       {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oHoliday.HolidayID <= 0)
                {
                    reader = HolidayDA.InsertUpdate(tc, oHoliday, EnumDBOperation.Insert, nUserID);
                }
                else
                {

                    reader = HolidayDA.InsertUpdate(tc, oHoliday, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHoliday = new Holiday();
                    oHoliday = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save Holiday. Because of " + e.Message, e);
                #endregion
            }
            return oHoliday;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Holiday oHoliday = new Holiday();
                oHoliday.HolidayID = id;
                HolidayDA.Delete(tc, oHoliday, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Holiday. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public Holiday Get(int id, Int64 nUserId)
        {
            Holiday aHoliday = new Holiday();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HolidayDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aHoliday = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Holiday", e);
                #endregion
            }

            return aHoliday;
        }

        public List<Holiday> Gets(Int64 nUserID)
        {
            List<Holiday> oHoliday = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HolidayDA.Gets(tc);
                oHoliday = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Holiday", e);
                #endregion
            }

            return oHoliday;
        }

        public List<Holiday> Gets(string sSql,Int64 nUserID)
        {
            List<Holiday> oHolidays = new List<Holiday>();
            Holiday oHoliday = new Holiday();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HolidayDA.Gets(tc, sSql);
                oHolidays = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oHoliday.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                oHolidays.Add(oHoliday);
                #endregion
            }

            return oHolidays;
        }

        public string ChangeActiveStatus(Holiday oHoliday, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                HolidayDA.ChangeActiveStatus(tc, oHoliday);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Update sucessfully";
        }
        #endregion
    }
}
