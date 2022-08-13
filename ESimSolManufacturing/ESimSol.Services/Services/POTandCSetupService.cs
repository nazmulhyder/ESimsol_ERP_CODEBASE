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
    public class POTandCSetupService : MarshalByRefObject, IPOTandCSetupService
    {
        #region Private functions and declaration
        private POTandCSetup MapObject(NullHandler oReader)
        {
            POTandCSetup oPOTandCSetup = new POTandCSetup();
            oPOTandCSetup.POTandCSetupID = oReader.GetInt32("POTandCSetupID");
            oPOTandCSetup.Clause = oReader.GetString("Clause");
            oPOTandCSetup.ClauseType = oReader.GetInt32("ClauseType");
            oPOTandCSetup.Activity = oReader.GetBoolean("Activity");
            oPOTandCSetup.Note = oReader.GetString("Note");
            oPOTandCSetup.BUID = oReader.GetInt32("BUID");
            

            return oPOTandCSetup;
        }

        private POTandCSetup CreateObject(NullHandler oReader)
        {
            POTandCSetup oPOTandCSetup = new POTandCSetup();
            oPOTandCSetup = MapObject(oReader);
            return oPOTandCSetup;
        }

        private List<POTandCSetup> CreateObjects(IDataReader oReader)
        {
            List<POTandCSetup> oPOTandCSetups = new List<POTandCSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                POTandCSetup oItem = CreateObject(oHandler);
                oPOTandCSetups.Add(oItem);
            }
            return oPOTandCSetups;
        }

        #endregion

        #region Interface implementation
        public POTandCSetupService() { }

        public POTandCSetup Save(POTandCSetup oPOTandCSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPOTandCSetup.POTandCSetupID <= 0)
                {
                    reader = POTandCSetupDA.InsertUpdate(tc, oPOTandCSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = POTandCSetupDA.InsertUpdate(tc, oPOTandCSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPOTandCSetup = new POTandCSetup();
                    oPOTandCSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPOTandCSetup.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return oPOTandCSetup;
        }
        public string Delete(POTandCSetup oPOTandCSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {


                tc = TransactionContext.Begin(true);
            
               
                POTandCSetupDA.Delete(tc, oPOTandCSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public POTandCSetup Get(int id, Int64 nUserId)
        {
            POTandCSetup oPOTandCSetup = new POTandCSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = POTandCSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPOTandCSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oPOTandCSetup;
        }

        public List<POTandCSetup> Gets(Int64 nUserId)
        {
            List<POTandCSetup> oPOTandCSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POTandCSetupDA.Gets(tc);
                oPOTandCSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oPOTandCSetups;
        }
        public List<POTandCSetup> Gets(string sClauseType,Int64 nUserId)
        {
            List<POTandCSetup> oPOTandCSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POTandCSetupDA.Gets(tc, sClauseType);
                oPOTandCSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oPOTandCSetups;
        }

        public List<POTandCSetup> Gets(bool bActivity,Int64 nUserId)
        {
            List<POTandCSetup> oPOTandCSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POTandCSetupDA.Gets(tc, bActivity);
                oPOTandCSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oPOTandCSetups;
        }

        public List<POTandCSetup> GetsByBU(int nBUID, Int64 nUserId)
        {
            List<POTandCSetup> oPOTandCSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POTandCSetupDA.GetsByBU(tc, nBUID);
                oPOTandCSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oPOTandCSetups;
        }
        public string ActivatePOTandCSetup(POTandCSetup oPOTandCSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {


                tc = TransactionContext.Begin(true);
                POTandCSetupDA.ActivatePOTandCSetup(tc, oPOTandCSetup);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Activate PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return "Activate sucessfully";
        }
        #endregion
    }
}
