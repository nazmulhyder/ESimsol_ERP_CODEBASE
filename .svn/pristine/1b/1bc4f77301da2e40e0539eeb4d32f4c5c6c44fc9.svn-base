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

    public class AttendanceSchemeDayOffService : MarshalByRefObject, IAttendanceSchemeDayOffService
    {
        #region Private functions and declaration
        private AttendanceSchemeDayOff MapObject(NullHandler oReader)
        {
            AttendanceSchemeDayOff oAttendanceSchemeDayOff = new AttendanceSchemeDayOff();
            oAttendanceSchemeDayOff.AttendanceSchemeDayOffID = oReader.GetInt32("AttendanceSchemeDayOffID");
            oAttendanceSchemeDayOff.AttendanceSchemeID = oReader.GetInt32("AttendanceSchemeID");
            oAttendanceSchemeDayOff.WeekDay = oReader.GetString("WeekDay");
            //oAttendanceSchemeDayOff.IsAlternate = oReader.GetBoolean("IsAlternate");
            oAttendanceSchemeDayOff.DayOffType = oReader.GetInt16("DayOffType");
            oAttendanceSchemeDayOff.InTime = oReader.GetDateTime("InTime");
            oAttendanceSchemeDayOff.OutTime = oReader.GetDateTime("OutTime");
            oAttendanceSchemeDayOff.IsAlternateFromFirstWeek = oReader.GetBoolean("IsAlternateFromFirstWeek");
            oAttendanceSchemeDayOff.NoOfRandomDayOff = oReader.GetInt32("NoOfRandomDayOff");

            return oAttendanceSchemeDayOff;
        }

        private AttendanceSchemeDayOff CreateObject(NullHandler oReader)
        {
            AttendanceSchemeDayOff oAttendanceSchemeDayOff = MapObject(oReader);
            return oAttendanceSchemeDayOff;
        }

        private List<AttendanceSchemeDayOff> CreateObjects(IDataReader oReader)
        {
            List<AttendanceSchemeDayOff> oAttendanceSchemeDayOff = new List<AttendanceSchemeDayOff>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceSchemeDayOff oItem = CreateObject(oHandler);
                oAttendanceSchemeDayOff.Add(oItem);
            }
            return oAttendanceSchemeDayOff;
        }

        #endregion

        #region Interface implementation
        public AttendanceSchemeDayOffService() { }

        public AttendanceSchemeDayOff Get(int id, Int64 nUserId)
        {
            AttendanceSchemeDayOff aAttendanceSchemeDayOff = new AttendanceSchemeDayOff();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceSchemeDayOffDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aAttendanceSchemeDayOff = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AttendanceSchemeDayOff", e);
                #endregion
            }

            return aAttendanceSchemeDayOff;
        }
        public List<AttendanceSchemeDayOff> Gets(int nAttendanceSchemeID, Int64 nUserID)
        {
            List<AttendanceSchemeDayOff> oAttendanceSchemeDayOff = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSchemeDayOffDA.Gets(nAttendanceSchemeID, tc);
                oAttendanceSchemeDayOff = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceSchemeDayOff", e);
                #endregion
            }

            return oAttendanceSchemeDayOff;
        }
        public List<AttendanceSchemeDayOff> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceSchemeDayOff> oAttendanceSchemeDayOff = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSchemeDayOffDA.Gets(tc, sSQL);
                oAttendanceSchemeDayOff = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceSchemeDayOff", e);
                #endregion
            }

            return oAttendanceSchemeDayOff;
        }
        public AttendanceSchemeDayOff IUD(AttendanceSchemeDayOff oAttendanceSchemeDayOff, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceSchemeDayOff oNewCCCD = new AttendanceSchemeDayOff();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceSchemeDayOffDA.IUD(tc, oAttendanceSchemeDayOff, nUserID, nDBOperation);

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
