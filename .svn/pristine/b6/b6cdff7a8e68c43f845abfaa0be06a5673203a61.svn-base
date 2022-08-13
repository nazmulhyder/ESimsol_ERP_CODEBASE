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
    public class RSShiftService : MarshalByRefObject, IRSShiftService
    {
        #region Private functions and declaration
        private RSShift MapObject(NullHandler oReader)
        {
            RSShift oRSShift = new RSShift();
            oRSShift.RSShiftID = oReader.GetInt32("RSShiftID");
            oRSShift.Name = oReader.GetString("Name");
            oRSShift.Note = oReader.GetString("Note");
            oRSShift.Activity = oReader.GetBoolean("Activity");
            oRSShift.StartDateTime = oReader.GetDateTime("StartDateTime");
            oRSShift.EndDateTime = oReader.GetDateTime("EndDateTime");
            oRSShift.ModuleType = (EnumModuleName)oReader.GetInt32("ModuleType");
            oRSShift.ModuleTypeInt = oReader.GetInt32("ModuleType");
            return oRSShift;
        }

        private RSShift CreateObject(NullHandler oReader)
        {
            RSShift oRSShift = new RSShift();
            oRSShift = MapObject(oReader);
            return oRSShift;
        }

        private List<RSShift> CreateObjects(IDataReader oReader)
        {
            List<RSShift> oRSShifts = new List<RSShift>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RSShift oItem = CreateObject(oHandler);
                oRSShifts.Add(oItem);
            }
            return oRSShifts;
        }

        #endregion

        #region Interface implementation
        public RSShiftService() { }


        public RSShift Save(RSShift oRSShift, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region RSShift
                IDataReader reader;
                if (oRSShift.RSShiftID <= 0)
                {
                    reader = RSShiftDA.InsertUpdate(tc, oRSShift, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = RSShiftDA.InsertUpdate(tc, oRSShift, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSShift = new RSShift();
                    oRSShift = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRSShift = new RSShift();
                oRSShift.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRSShift;
        }
        public RSShift ToggleActivity(RSShift oRSShift, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RSShiftDA.ToggleActivity(tc, oRSShift);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSShift = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oRSShift = new RSShift();
                oRSShift.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRSShift;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            
            string sMessage = "Delete sucessfully";
            try
            {
                tc = TransactionContext.Begin(true);
                RSShift oRSShift = new RSShift();
                oRSShift.RSShiftID = id;
                RSShiftDA.Delete(tc, oRSShift, EnumDBOperation.Delete, nUserId);
               
                tc.End();
            }
            catch (Exception e)
            {
                sMessage = "Delete operation could not complete";
            }

            return sMessage;
        }

        public RSShift Get(int id, Int64 nUserId)
        {
            RSShift oRSShift = new RSShift();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RSShiftDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSShift = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oRSShift;
        }
        public List<RSShift> GetsActive(Int64 nUserId)
        {
            List<RSShift> oRSShifts = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RSShiftDA.GetsActive(tc);
                oRSShifts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RS Shift", e);
                #endregion
            }

            return oRSShifts;
        }
        public List<RSShift> GetsByModule(int nBUID, string sModuleIDs, Int64 nUserId)
        {
            List<RSShift> oRSShifts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSShiftDA.GetsByModule(tc, nBUID, sModuleIDs);
                oRSShifts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oRSShifts;
        }

        public List<RSShift> Gets(Int64 nUserId)
        {
            List<RSShift> oRSShifts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSShiftDA.Gets(tc);
                oRSShifts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oRSShifts;
        }

        public List<RSShift> Gets(string sql, Int64 nUserId)
        {
            List<RSShift> oRSShifts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSShiftDA.Gets(tc, sql);
                oRSShifts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oRSShifts;
        }

        #endregion
    }
}