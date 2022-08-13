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

    [Serializable]
    public class LedgerGroupSetupService : MarshalByRefObject, ILedgerGroupSetupService
    {
        #region Private functions and declaration
        private LedgerGroupSetup MapObject(NullHandler oReader)
        {
            LedgerGroupSetup oLedgerGroupSetup = new LedgerGroupSetup();
            oLedgerGroupSetup.LedgerGroupSetupID = oReader.GetInt32("LedgerGroupSetupID");
            oLedgerGroupSetup.OCSID = oReader.GetInt32("OCSID");
            oLedgerGroupSetup.LedgerGroupSetupName = oReader.GetString("LedgerGroupSetupName");
            oLedgerGroupSetup.Note = oReader.GetString("Note");
            oLedgerGroupSetup.IsDr = oReader.GetBoolean("IsDr");
            return oLedgerGroupSetup;
        }
        private LedgerGroupSetup CreateObject(NullHandler oReader)
        {
            LedgerGroupSetup oLedgerGroupSetup = new LedgerGroupSetup();
            oLedgerGroupSetup = MapObject(oReader);
            return oLedgerGroupSetup;
        }

        private List<LedgerGroupSetup> CreateObjects(IDataReader oReader)
        {
            List<LedgerGroupSetup> oLedgerGroupSetups = new List<LedgerGroupSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LedgerGroupSetup oItem = CreateObject(oHandler);
                oLedgerGroupSetups.Add(oItem);
            }
            return oLedgerGroupSetups;
        }
        #endregion

        #region Interface implementation
        public LedgerGroupSetup Save(LedgerGroupSetup oLedgerGroupSetup, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLedgerGroupSetup.LedgerGroupSetupID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LedgerGroupSetup, EnumRoleOperationType.Add);
                    reader = LedgerGroupSetupDA.InsertUpdate(tc, oLedgerGroupSetup, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LedgerGroupSetup, EnumRoleOperationType.Edit);
                    reader = LedgerGroupSetupDA.InsertUpdate(tc, oLedgerGroupSetup, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLedgerGroupSetup = new LedgerGroupSetup();
                    oLedgerGroupSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save LedgerGroupSetup. Because of " + e.Message, e);
                #endregion
            }
            return oLedgerGroupSetup;
        }
        public string Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LedgerGroupSetup oLedgerGroupSetup = new LedgerGroupSetup();
                oLedgerGroupSetup.LedgerGroupSetupID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LedgerGroupSetup, EnumRoleOperationType.Delete);
                LedgerGroupSetupDA.Delete(tc, oLedgerGroupSetup, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];

                #endregion
            }
            return "Data delete successfully";
        }
        public LedgerGroupSetup Get(int nID, int nUserID)
        {
            LedgerGroupSetup oLedgerGroupSetup = new LedgerGroupSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LedgerGroupSetupDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLedgerGroupSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LedgerGroupSetup", e);
                #endregion
            }

            return oLedgerGroupSetup;
        }

        public List<LedgerGroupSetup> Gets(int nOCSID, int nUserID)
        {
            List<LedgerGroupSetup> oLedgerGroupSetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LedgerGroupSetupDA.Gets(tc, nOCSID);
                oLedgerGroupSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LedgerGroupSetups", e);
                #endregion
            }
            return oLedgerGroupSetups;
        }

        public List<LedgerGroupSetup> Gets(string sSQL, int nUserID)
        {
            List<LedgerGroupSetup> oLedgerGroupSetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LedgerGroupSetupDA.Gets(tc, sSQL);
                oLedgerGroupSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LedgerGroupSetups", e);
                #endregion
            }
            return oLedgerGroupSetups;
        }
        #endregion
    }


}
