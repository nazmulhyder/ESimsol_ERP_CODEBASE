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
    public class AccountsBookSetupDetailService : MarshalByRefObject, IAccountsBookSetupDetailService
    {
        #region Private functions and declaration
        private AccountsBookSetupDetail MapObject(NullHandler oReader)
        {
            AccountsBookSetupDetail oAccountsBookSetupDetail = new AccountsBookSetupDetail();
            oAccountsBookSetupDetail.AccountsBookSetupDetailID = oReader.GetInt32("AccountsBookSetupDetailID");
            oAccountsBookSetupDetail.AccountsBookSetupID = oReader.GetInt32("AccountsBookSetupID");
            oAccountsBookSetupDetail.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oAccountsBookSetupDetail.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            oAccountsBookSetupDetail.ComponentTypeInt = oReader.GetInt32("ComponentType");
            oAccountsBookSetupDetail.AccountHeadName = oReader.GetString("AccountHeadName");
            oAccountsBookSetupDetail.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oAccountsBookSetupDetail.CategoryName = oReader.GetString("CategoryName");
            return oAccountsBookSetupDetail;
        }
        private AccountsBookSetupDetail CreateObject(NullHandler oReader)
        {
            AccountsBookSetupDetail oAccountsBookSetupDetail = new AccountsBookSetupDetail();
            oAccountsBookSetupDetail = MapObject(oReader);
            return oAccountsBookSetupDetail;
        }
        private List<AccountsBookSetupDetail> CreateObjects(IDataReader oReader)
        {
            List<AccountsBookSetupDetail> oAccountsBookSetupDetail = new List<AccountsBookSetupDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountsBookSetupDetail oItem = CreateObject(oHandler);
                oAccountsBookSetupDetail.Add(oItem);
            }
            return oAccountsBookSetupDetail;
        }
        #endregion

        #region Interface implementation
        public AccountsBookSetupDetail Save(AccountsBookSetupDetail oAccountsBookSetupDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<AccountsBookSetupDetail> oAccountsBookSetupDetails = new List<AccountsBookSetupDetail>();
                tc = TransactionContext.Begin(true);
                if (oAccountsBookSetupDetails != null)
                {
                    foreach (AccountsBookSetupDetail oItem in oAccountsBookSetupDetails)
                    {
                        IDataReader reader;
                        if (oItem.AccountsBookSetupDetailID <= 0)
                        {
                            reader = AccountsBookSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = AccountsBookSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oAccountsBookSetupDetail = new AccountsBookSetupDetail();
                            oAccountsBookSetupDetail = CreateObject(oReader);
                        }
                        reader.Close();
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save Accounts Book Setup. Because of " + e.Message, e);
                #endregion
            }
            return oAccountsBookSetupDetail;
        }

        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountsBookSetupDetail oAccountsBookSetupDetail = new AccountsBookSetupDetail();
                oAccountsBookSetupDetail.AccountsBookSetupDetailID = id;
                AccountsBookSetupDetailDA.Delete(tc, oAccountsBookSetupDetail, EnumDBOperation.Delete, nUserId);
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

        public AccountsBookSetupDetail Get(int id, int nUserId)
        {
            AccountsBookSetupDetail oAccountHead = new AccountsBookSetupDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountsBookSetupDetailDA.Get(tc, id);
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
        public List<AccountsBookSetupDetail> Gets(int id, int nUserID)
        {
            List<AccountsBookSetupDetail> oAccountsBookSetupDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountsBookSetupDetailDA.Gets(tc, id);
                oAccountsBookSetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountsBookSetupDetail", e);
                #endregion
            }
            return oAccountsBookSetupDetail;
        }
        public List<AccountsBookSetupDetail> GetsSQL(string sSQL, int nUserID)
        {
            List<AccountsBookSetupDetail> oAccountsBookSetupDetail = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from AccountsBookSetupDetail where AccountsBookSetupDetailID in (1,2,80,272,347,370,60,45)";
                }
                reader = AccountsBookSetupDetailDA.GetsSQL(tc, sSQL);
                oAccountsBookSetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountsBookSetupDetail", e);
                #endregion
            }

            return oAccountsBookSetupDetail;
        }
        #endregion
    }
}
