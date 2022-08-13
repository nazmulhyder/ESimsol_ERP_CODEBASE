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
    public class AttendanceSchemeLeaveService : MarshalByRefObject, IAttendanceSchemeLeaveService
    {
        #region Private functions and declaration
        private AttendanceSchemeLeave MapObject(NullHandler oReader)
        {
            AttendanceSchemeLeave oAttendanceSchemeLeave = new AttendanceSchemeLeave();
            oAttendanceSchemeLeave.AttendanceSchemeLeaveID = oReader.GetInt32("AttendanceSchemeLeaveID");
            oAttendanceSchemeLeave.AttendanceSchemeID = oReader.GetInt32("AttendanceSchemeID");
            oAttendanceSchemeLeave.LeaveID = oReader.GetInt32("LeaveID");
            oAttendanceSchemeLeave.TotalDay = oReader.GetInt32("TotalDay");
            oAttendanceSchemeLeave.DeferredDay = oReader.GetInt32("DeferredDay");
            oAttendanceSchemeLeave.ActivationAfter = (EnumRecruitmentEvent)oReader.GetInt32("ActivationAfter");
            oAttendanceSchemeLeave.IsLeaveOnPresence = oReader.GetBoolean("IsLeaveOnPresence");
            oAttendanceSchemeLeave.IsComp = oReader.GetBoolean("IsComp");
            oAttendanceSchemeLeave.PresencePerLeave = oReader.GetInt32("PresencePerLeave");
            oAttendanceSchemeLeave.IsCarryForward = oReader.GetBoolean("IsCarryForward");
            oAttendanceSchemeLeave.MaxCarryDays = oReader.GetInt32("MaxCarryDays");
            oAttendanceSchemeLeave.LeaveName = oReader.GetString("LeaveName");
            return oAttendanceSchemeLeave;
        }

        private AttendanceSchemeLeave CreateObject(NullHandler oReader)
        {
            AttendanceSchemeLeave oAttendanceSchemeLeave = MapObject(oReader);
            return oAttendanceSchemeLeave;
        }

        private List<AttendanceSchemeLeave> CreateObjects(IDataReader oReader)
        {
            List<AttendanceSchemeLeave> oAttendanceSchemeLeave = new List<AttendanceSchemeLeave>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceSchemeLeave oItem = CreateObject(oHandler);
                oAttendanceSchemeLeave.Add(oItem);
            }
            return oAttendanceSchemeLeave;
        }

        #endregion

        #region Interface implementation
        public AttendanceSchemeLeaveService() { }

        public AttendanceSchemeLeave Get(int id, Int64 nUserId)
        {
            AttendanceSchemeLeave aAttendanceSchemeLeave = new AttendanceSchemeLeave();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceSchemeLeaveDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aAttendanceSchemeLeave = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AttendanceSchemeLeave", e);
                #endregion
            }

            return aAttendanceSchemeLeave;
        }
        public List<AttendanceSchemeLeave> Gets(int nAttendanceSchemeID, Int64 nUserID)
        {
            List<AttendanceSchemeLeave> oAttendanceSchemeLeave = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSchemeLeaveDA.Gets(nAttendanceSchemeID,tc);
                oAttendanceSchemeLeave = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceSchemeLeave", e);
                #endregion
            }

            return oAttendanceSchemeLeave;
        }
        public List<AttendanceSchemeLeave> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceSchemeLeave> oAttendanceSchemeLeave = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSchemeLeaveDA.Gets(tc, sSQL);
                oAttendanceSchemeLeave = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceSchemeLeave", e);
                #endregion
            }

            return oAttendanceSchemeLeave;
        }
        public AttendanceSchemeLeave IUD(AttendanceSchemeLeave oAttendanceSchemeLeave, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceSchemeLeave oNewCCCD = new AttendanceSchemeLeave();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceSchemeLeaveDA.IUD(tc, oAttendanceSchemeLeave, nUserID, nDBOperation);

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
