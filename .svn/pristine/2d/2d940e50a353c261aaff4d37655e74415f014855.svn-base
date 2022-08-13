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
    public class ShiftBreakScheduleService : MarshalByRefObject, IShiftBreakScheduleService
    {
        #region Private functions and declaration
        private ShiftBreakSchedule MapObject(NullHandler oReader)
        {
            ShiftBreakSchedule oShiftBreakSchedule = new ShiftBreakSchedule();

            oShiftBreakSchedule.ShiftBScID = oReader.GetInt32("ShiftBScID");
            oShiftBreakSchedule.ShiftID = oReader.GetInt32("ShiftID");
            oShiftBreakSchedule.ShiftBNID = oReader.GetInt32("ShiftBNID");
            oShiftBreakSchedule.StartTime = oReader.GetDateTime("StartTime");
            oShiftBreakSchedule.EndTime = oReader.GetDateTime("EndTime");
            oShiftBreakSchedule.IsActive = oReader.GetBoolean("IsActive");
            oShiftBreakSchedule.ShiftBreakName = oReader.GetString("ShiftBreakName");
            return oShiftBreakSchedule;

        }

        private ShiftBreakSchedule CreateObject(NullHandler oReader)
        {
            ShiftBreakSchedule oShiftBreakSchedule = MapObject(oReader);
            return oShiftBreakSchedule;
        }

        private List<ShiftBreakSchedule> CreateObjects(IDataReader oReader)
        {
            List<ShiftBreakSchedule> oShiftBreakSchedules = new List<ShiftBreakSchedule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShiftBreakSchedule oItem = CreateObject(oHandler);
                oShiftBreakSchedules.Add(oItem);
            }
            return oShiftBreakSchedules;
        }

        #endregion

        #region Interface implementation
        public ShiftBreakScheduleService() { }
        public ShiftBreakSchedule IUD(ShiftBreakSchedule oShiftBreakSchedule, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ShiftBreakScheduleDA.IUD(tc, oShiftBreakSchedule, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBreakSchedule = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oShiftBreakSchedule.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oShiftBreakSchedule.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oShiftBreakSchedule;
        }

        public ShiftBreakSchedule Get(int nShiftBScID, Int64 nUserId)
        {
            ShiftBreakSchedule oShiftBreakSchedule = new ShiftBreakSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftBreakScheduleDA.Get(nShiftBScID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBreakSchedule = CreateObject(oReader);
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

                oShiftBreakSchedule.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftBreakSchedule;
        }

        public ShiftBreakSchedule Get(string sSQL, Int64 nUserId)
        {
            ShiftBreakSchedule oShiftBreakSchedule = new ShiftBreakSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftBreakScheduleDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBreakSchedule = CreateObject(oReader);
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

                oShiftBreakSchedule.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftBreakSchedule;
        }

        public List<ShiftBreakSchedule> Gets(Int64 nUserID)
        {
            List<ShiftBreakSchedule> oShiftBreakSchedule = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShiftBreakScheduleDA.Gets(tc);
                oShiftBreakSchedule = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShiftBreakSchedule", e);
                #endregion
            }
            return oShiftBreakSchedule;
        }

        public List<ShiftBreakSchedule> Gets(string sSQL, Int64 nUserID)
        {
            List<ShiftBreakSchedule> oShiftBreakSchedule = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShiftBreakScheduleDA.Gets(sSQL, tc);
                oShiftBreakSchedule = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShiftBreakSchedule", e);
                #endregion
            }
            return oShiftBreakSchedule;
        }

        #endregion

        #region Activity
        public ShiftBreakSchedule Activite(int nShiftBScID, Int64 nUserId)
        {
            ShiftBreakSchedule oShiftBreakSchedule = new ShiftBreakSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftBreakScheduleDA.Activity(nShiftBScID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBreakSchedule = CreateObject(oReader);
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
                oShiftBreakSchedule.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftBreakSchedule;
        }


        #endregion

    }
}
