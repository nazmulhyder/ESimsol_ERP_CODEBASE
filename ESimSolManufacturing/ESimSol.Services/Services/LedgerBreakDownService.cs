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
    public class LedgerBreakDownService : MarshalByRefObject, ILedgerBreakDownService
    {
        #region Private functions and declaration
        private LedgerBreakDown MapObject(NullHandler oReader)
        {
            LedgerBreakDown oLedgerBreakDown = new LedgerBreakDown();
            oLedgerBreakDown.LedgerBreakDownID = oReader.GetInt32("LedgerBreakDownID");
            oLedgerBreakDown.ReferenceID = oReader.GetInt32("ReferenceID");
            oLedgerBreakDown.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oLedgerBreakDown.AccountCode = oReader.GetString("AccountCode");
            oLedgerBreakDown.AccountHeadName = oReader.GetString("AccountHeadName");
            oLedgerBreakDown.IsEffectedAccounts = oReader.GetBoolean("IsEffectedAccounts");
            return oLedgerBreakDown;
        }
        private LedgerBreakDown CreateObject(NullHandler oReader)
        {
            LedgerBreakDown oLedgerBreakDown = new LedgerBreakDown();
            oLedgerBreakDown = MapObject(oReader);
            return oLedgerBreakDown;
        }

        private List<LedgerBreakDown> CreateObjects(IDataReader oReader)
        {
            List<LedgerBreakDown> oLedgerBreakDowns = new List<LedgerBreakDown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LedgerBreakDown oItem = CreateObject(oHandler);
                oLedgerBreakDowns.Add(oItem);
            }
            return oLedgerBreakDowns;
        }
        #endregion

        #region Interface implementation
        public LedgerBreakDown Save(LedgerBreakDown oLedgerBreakDown, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLedgerBreakDown.LedgerBreakDownID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LedgerBreakDown, EnumRoleOperationType.Add);
                    reader = LedgerBreakDownDA.InsertUpdate(tc, oLedgerBreakDown, EnumDBOperation.Insert, nUserID,"", false);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LedgerBreakDown, EnumRoleOperationType.Edit);
                    reader = LedgerBreakDownDA.InsertUpdate(tc, oLedgerBreakDown, EnumDBOperation.Update, nUserID,"",false);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLedgerBreakDown = new LedgerBreakDown();
                    oLedgerBreakDown = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save LedgerBreakDown. Because of " + e.Message, e);
                #endregion
            }
            return oLedgerBreakDown;
        }
        public string Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LedgerBreakDown oLedgerBreakDown = new LedgerBreakDown();
                oLedgerBreakDown.LedgerBreakDownID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.LedgerBreakDown, EnumRoleOperationType.Delete);
                LedgerBreakDownDA.Delete(tc, oLedgerBreakDown, EnumDBOperation.Delete, nUserID,"", false);
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
        public LedgerBreakDown Get(int nID, int nUserID)
        {
            LedgerBreakDown oLedgerBreakDown = new LedgerBreakDown();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LedgerBreakDownDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLedgerBreakDown = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LedgerBreakDown", e);
                #endregion
            }

            return oLedgerBreakDown;
        }

        public List<LedgerBreakDown> Gets(int ReferenceID, bool bIsEffectedAccounts, int nUserID)
        {
            List<LedgerBreakDown> oLedgerBreakDowns = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LedgerBreakDownDA.Gets(tc, ReferenceID, bIsEffectedAccounts);
                oLedgerBreakDowns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LedgerBreakDowns", e);
                #endregion
            }
            return oLedgerBreakDowns;
        }

        public List<LedgerBreakDown> Gets(int nStatementSetupID, int nUserID)
        {
            List<LedgerBreakDown> oLedgerBreakDowns = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LedgerBreakDownDA.Gets(tc, nStatementSetupID);
                oLedgerBreakDowns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LedgerBreakDowns", e);
                #endregion
            }
            return oLedgerBreakDowns;
        }
        #endregion
    }
   
}
