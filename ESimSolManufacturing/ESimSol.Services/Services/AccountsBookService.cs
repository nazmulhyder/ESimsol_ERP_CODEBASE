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
    public class AccountsBookService : MarshalByRefObject, IAccountsBookService
    {
        #region Private functions and declaration
        private AccountsBook MapObject(NullHandler oReader)
        {
            AccountsBook oAccountsBook = new AccountsBook();
            oAccountsBook.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oAccountsBook.AccountCode = oReader.GetString("AccountCode");
            oAccountsBook.AccountHeadName = oReader.GetString("AccountHeadName");
            oAccountsBook.AccountType = (EnumAccountType)oReader.GetInt32("AccountType");
            oAccountsBook.AccountTypeInInt = oReader.GetInt32("AccountType");
            oAccountsBook.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            oAccountsBook.ComponentTypeInInt = oReader.GetInt32("ComponentType");
            oAccountsBook.MappingType = (EnumACMappingType)oReader.GetInt32("MappingType");
            oAccountsBook.MappingTypeInt = oReader.GetInt32("MappingType");
            oAccountsBook.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oAccountsBook.DebitAmount = oReader.GetDouble("DebitAmount");
            oAccountsBook.CreditAmount = oReader.GetDouble("CreditAmount");
            oAccountsBook.ClosingBalance = oReader.GetDouble("ClosingBalance");
            oAccountsBook.ParentHeadID = oReader.GetInt32("ParentHeadID");
            oAccountsBook.ParentHeadName = oReader.GetString("ParentHeadName");

            return oAccountsBook;
        }
        private AccountsBook CreateObject(NullHandler oReader)
        {
            AccountsBook oAccountsBook = new AccountsBook();
            oAccountsBook = MapObject(oReader);
            return oAccountsBook;
        }
        private List<AccountsBook> CreateObjects(IDataReader oReader)
        {
            List<AccountsBook> oAccountsBook = new List<AccountsBook>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountsBook oItem = CreateObject(oHandler);
                oAccountsBook.Add(oItem);
            }
            return oAccountsBook;
        }
        #endregion

        #region Interface implementation
        public List<AccountsBook> Gets(int nAccountsBookSetupID, DateTime dStartDate, DateTime dEndDate, string BUIDs, bool bApprovedOnly, int nUserID)
        {
            List<AccountsBook> oAccountsBook = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountsBookDA.Gets(tc, nAccountsBookSetupID, dStartDate, dEndDate, BUIDs, bApprovedOnly);
                oAccountsBook = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Accounts Book", e);
                #endregion
            }

            return oAccountsBook;
        }
        #endregion
    }
}
