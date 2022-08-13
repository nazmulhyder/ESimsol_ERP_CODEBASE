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
    public class AccountEffectService : MarshalByRefObject, IAccountEffectService
    {
        #region Private functions and declaration
        private AccountEffect MapObject(NullHandler oReader)
        {
            AccountEffect oAccountEffect = new AccountEffect();
            oAccountEffect.AccountEffectID = oReader.GetInt32("AccountEffectID");
            oAccountEffect.ModuleName = (EnumModuleName)oReader.GetInt32("ModuleName");
            oAccountEffect.ModuleNameInt = oReader.GetInt32("ModuleName");
            oAccountEffect.ModuleObjID = oReader.GetInt32("ModuleObjID");
            oAccountEffect.AccountEffectType = (EnumAccountEffectType)oReader.GetInt32("AccountEffectType");
            oAccountEffect.AccountEffectTypeInt = oReader.GetInt32("AccountEffectType");
            oAccountEffect.DrAccountHeadID = oReader.GetInt32("DrAccountHeadID");
            oAccountEffect.CrAccountHeadID = oReader.GetInt32("CrAccountHeadID");
            oAccountEffect.Remarks = oReader.GetString("Remarks");
            oAccountEffect.DrAccountCode = oReader.GetString("DrAccountCode");
            oAccountEffect.DrAccountHeadName = oReader.GetString("DrAccountHeadName");
            oAccountEffect.CrAccountCode = oReader.GetString("CrAccountCode");
            oAccountEffect.CrAccountHeadName = oReader.GetString("CrAccountHeadName");
            oAccountEffect.DebitSubLedgerID = oReader.GetInt32("DebitSubLedgerID");
            oAccountEffect.CreditSubLedgerID = oReader.GetInt32("CreditSubLedgerID");
            oAccountEffect.DebitSubLedgerCode = oReader.GetString("DebitSubLedgerCode");
            oAccountEffect.DebitSubLedgerName = oReader.GetString("DebitSubLedgerName");
            oAccountEffect.CreditSubLedgerCode = oReader.GetString("CreditSubLedgerCode");
            oAccountEffect.CreditSubLedgerName = oReader.GetString("CreditSubLedgerName");
            return oAccountEffect;
        }
        private AccountEffect CreateObject(NullHandler oReader)
        {
            AccountEffect oAccountEffect = new AccountEffect();
            oAccountEffect = MapObject(oReader);
            return oAccountEffect;
        }
        private List<AccountEffect> CreateObjects(IDataReader oReader)
        {
            List<AccountEffect> oAccountEffect = new List<AccountEffect>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountEffect oItem = CreateObject(oHandler);
                oAccountEffect.Add(oItem);
            }
            return oAccountEffect;
        }
        #endregion

        #region Interface implementation
        public AccountEffect Save(AccountEffect oAccountEffect, int nUserID)
        {
            TransactionContext tc = null;
            try
            {                
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oAccountEffect.AccountEffectID <= 0)
                {
                    reader = AccountEffectDA.InsertUpdate(tc, oAccountEffect, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = AccountEffectDA.InsertUpdate(tc, oAccountEffect, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountEffect = new AccountEffect();
                    oAccountEffect = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save AccountEffect. Because of " + e.Message, e);
                #endregion
            }
            return oAccountEffect;
        }

        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountEffect oAccountEffect = new AccountEffect();
                oAccountEffect.AccountEffectID = id;
                AccountEffectDA.Delete(tc, oAccountEffect, EnumDBOperation.Delete, nUserId);
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

        public AccountEffect Get(int id, int nUserId)
        {
            AccountEffect oAccountHead = new AccountEffect();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountEffectDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AccountEffect", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<AccountEffect> Gets(int nModuleObjID, EnumModuleName eModuleName, int nUserID)
        {
            List<AccountEffect> oAccountEffects = new List<AccountEffect>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountEffectDA.Gets(tc, nModuleObjID, eModuleName);
                oAccountEffects = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountEffect", e);
                #endregion
            }
            return oAccountEffects;
        }
        public List<AccountEffect> Gets(int nUserID)
        {
            List<AccountEffect> oAccountEffects = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountEffectDA.Gets(tc);
                oAccountEffects = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountEffect", e);
                #endregion
            }
            return oAccountEffects;
        }
        public List<AccountEffect> Gets(string sSQL, int nUserID)
        {
            List<AccountEffect> oAccountEffects = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountEffectDA.Gets(tc, sSQL);
                oAccountEffects = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountEffect", e);
                #endregion
            }

            return oAccountEffects;
        }
        #endregion
    }
}
