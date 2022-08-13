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
    public class AccountsBookSetupService : MarshalByRefObject, IAccountsBookSetupService
    {
        #region Private functions and declaration
        private AccountsBookSetup MapObject(NullHandler oReader)
        {
            AccountsBookSetup oAccountsBookSetup = new AccountsBookSetup();
            oAccountsBookSetup.AccountsBookSetupID = oReader.GetInt32("AccountsBookSetupID");
            oAccountsBookSetup.AccountsBookSetupName = oReader.GetString("AccountsBookSetupName");
            oAccountsBookSetup.MappingType = (EnumACMappingType)oReader.GetInt32("MappingType");
            oAccountsBookSetup.MappingTypeInt = oReader.GetInt32("MappingType");
            oAccountsBookSetup.Note = oReader.GetString("Note");
            return oAccountsBookSetup;
        }
        private AccountsBookSetup CreateObject(NullHandler oReader)
        {
            AccountsBookSetup oAccountsBookSetup = new AccountsBookSetup();
            oAccountsBookSetup = MapObject(oReader);
            return oAccountsBookSetup;
        }
        private List<AccountsBookSetup> CreateObjects(IDataReader oReader)
        {
            List<AccountsBookSetup> oAccountsBookSetup = new List<AccountsBookSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountsBookSetup oItem = CreateObject(oHandler);
                oAccountsBookSetup.Add(oItem);
            }
            return oAccountsBookSetup;
        }
        #endregion

        #region Interface implementation
        public AccountsBookSetup Save(AccountsBookSetup oAccountsBookSetup, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<AccountsBookSetupDetail> oAccountsBookSetupDetails = new List<AccountsBookSetupDetail>();
                oAccountsBookSetupDetails = oAccountsBookSetup.AccountsBookSetupDetails;
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oAccountsBookSetup.AccountsBookSetupID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AccountsBookSetup, EnumRoleOperationType.Add);
                    reader = AccountsBookSetupDA.InsertUpdate(tc, oAccountsBookSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AccountsBookSetup, EnumRoleOperationType.Edit);
                    reader = AccountsBookSetupDA.InsertUpdate(tc, oAccountsBookSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountsBookSetup = new AccountsBookSetup();
                    oAccountsBookSetup = CreateObject(oReader);
                }
                reader.Close();
                if (oAccountsBookSetupDetails != null)
                {
                    #region Delete All AccountsBookSetupDetail for this AccountsBookSetup
                    IDataReader readerDetail;
                    oAccountsBookSetupDetails[0].AccountsBookSetupID = oAccountsBookSetup.AccountsBookSetupID;
                    readerDetail = AccountsBookSetupDetailDA.InsertUpdate(tc, oAccountsBookSetupDetails[0], EnumDBOperation.Delete,  nUserID);
                    readerDetail.Close();
                    #endregion

                    #region Insert AccountsBookSetupDetail
                    foreach (AccountsBookSetupDetail oItem in oAccountsBookSetupDetails)
                    {
                        IDataReader readerDetails;
                        oItem.AccountsBookSetupID = oAccountsBookSetup.AccountsBookSetupID;
                        oItem.AccountsBookSetupDetailID = 0; // Altime Insert
                        readerDetails = AccountsBookSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        readerDetails.Close();
                    }
                    #endregion
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAccountsBookSetup = new AccountsBookSetup();
                oAccountsBookSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oAccountsBookSetup;
        }

        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountsBookSetup oAccountsBookSetup = new AccountsBookSetup();
                oAccountsBookSetup.AccountsBookSetupID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AccountsBookSetup, EnumRoleOperationType.Delete);
                AccountsBookSetupDA.Delete(tc, oAccountsBookSetup, EnumDBOperation.Delete, nUserId);
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

        public AccountsBookSetup Get(int id, int nUserId)
        {
            AccountsBookSetup oAccountHead = new AccountsBookSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountsBookSetupDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Accounts Book Setup", e);
                #endregion
            }
            return oAccountHead;
        }
        public List<AccountsBookSetup> Gets(int nUserID)
        {
            List<AccountsBookSetup> oAccountsBookSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountsBookSetupDA.Gets(tc);
                oAccountsBookSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountsBookSetup", e);
                #endregion
            }
            return oAccountsBookSetup;
        }
        public List<AccountsBookSetup> Gets(string sSQL, int nUserID)
        {
            List<AccountsBookSetup> oAccountsBookSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from AccountsBookSetup where AccountsBookSetupID in (1,2,80,272,347,370,60,45)";
                }
                reader = AccountsBookSetupDA.Gets(tc, sSQL);
                oAccountsBookSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountsBookSetup", e);
                #endregion
            }

            return oAccountsBookSetup;
        }
        #endregion
    }
}
