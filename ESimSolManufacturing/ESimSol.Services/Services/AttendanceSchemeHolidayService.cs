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

    public class AttendanceSchemeHolidayService : MarshalByRefObject, IAttendanceSchemeHolidayService
    {
        #region Private functions and declaration
        private AttendanceSchemeHoliday MapObject(NullHandler oReader)
        {
            AttendanceSchemeHoliday oAttendanceSchemeHoliday = new AttendanceSchemeHoliday();
            oAttendanceSchemeHoliday.AttendanceSchemeHolidayID = oReader.GetInt32("AttendanceSchemeHolidayID");
            oAttendanceSchemeHoliday.AttendanceSchemeID = oReader.GetInt32("AttendanceSchemeID");
            oAttendanceSchemeHoliday.HolidayID = oReader.GetInt32("HolidayID");
            oAttendanceSchemeHoliday.DayQty = oReader.GetInt32("DayQty");
            oAttendanceSchemeHoliday.HoliDayName = oReader.GetString("HoliDay");
            oAttendanceSchemeHoliday.DayOfMonth = oReader.GetString("DayOfMonth");
            return oAttendanceSchemeHoliday;
        }

        private AttendanceSchemeHoliday CreateObject(NullHandler oReader)
        {
            AttendanceSchemeHoliday oAttendanceSchemeHoliday = MapObject(oReader);
            return oAttendanceSchemeHoliday;
        }

        private List<AttendanceSchemeHoliday> CreateObjects(IDataReader oReader)
        {
            List<AttendanceSchemeHoliday> oAttendanceSchemeHoliday = new List<AttendanceSchemeHoliday>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceSchemeHoliday oItem = CreateObject(oHandler);
                oAttendanceSchemeHoliday.Add(oItem);
            }
            return oAttendanceSchemeHoliday;
        }

        #endregion

        #region Interface implementation
        public AttendanceSchemeHolidayService() { }

        public AttendanceSchemeHoliday Get(int id, Int64 nUserId)
        {
            AttendanceSchemeHoliday aAttendanceSchemeHoliday = new AttendanceSchemeHoliday();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceSchemeHolidayDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aAttendanceSchemeHoliday = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AttendanceSchemeHoliday", e);
                #endregion
            }

            return aAttendanceSchemeHoliday;
        }
        public List<AttendanceSchemeHoliday> Gets(int nAttendanceSchemeID, Int64 nUserID)
        {
            List<AttendanceSchemeHoliday> oAttendanceSchemeHoliday = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSchemeHolidayDA.Gets(nAttendanceSchemeID, tc);
                oAttendanceSchemeHoliday = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceSchemeHoliday", e);
                #endregion
            }

            return oAttendanceSchemeHoliday;
        }
        public List<AttendanceSchemeHoliday> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceSchemeHoliday> oAttendanceSchemeHoliday = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSchemeHolidayDA.Gets(tc, sSQL);
                oAttendanceSchemeHoliday = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceSchemeHoliday", e);
                #endregion
            }

            return oAttendanceSchemeHoliday;
        }
        public AttendanceSchemeHoliday IUD(AttendanceSchemeHoliday oAttendanceSchemeHoliday, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceSchemeHoliday oNewCCCD = new AttendanceSchemeHoliday();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceSchemeHolidayDA.IUD(tc, oAttendanceSchemeHoliday, nUserID, nDBOperation);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNewCCCD = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oNewCCCD.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewCCCD.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oNewCCCD;
        }

        #endregion
    }
   
}
