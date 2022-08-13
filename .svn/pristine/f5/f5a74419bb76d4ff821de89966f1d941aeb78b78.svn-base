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
    public class AccountingActivityService : MarshalByRefObject, IAccountingActivityService
    {
        #region Private functions and declaration
        private AccountingActivity MapObject(NullHandler oReader)
        {
            AccountingActivity oAccountingActivity = new AccountingActivity();
            oAccountingActivity.UserID = oReader.GetInt32("UserID");
            oAccountingActivity.UserName = oReader.GetString("UserName");
            oAccountingActivity.VoucherTypeID = oReader.GetInt32("VoucherTypeID");
            oAccountingActivity.VoucherName = oReader.GetString("VoucherName");
            oAccountingActivity.Added = oReader.GetInt32("Added");
            oAccountingActivity.Edited = oReader.GetInt32("Edited");
            oAccountingActivity.Approved = oReader.GetInt32("Approved");
            oAccountingActivity.Total = oReader.GetInt32("Total");
            
            
            return oAccountingActivity;
        }

        private AccountingActivity CreateObject(NullHandler oReader)
        {
            AccountingActivity oAccountingActivity = new AccountingActivity();
            oAccountingActivity = MapObject(oReader);
            return oAccountingActivity;
        }

        private List<AccountingActivity> CreateObjects(IDataReader oReader)
        {
            List<AccountingActivity> oAccountingActivity = new List<AccountingActivity>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountingActivity oItem = CreateObject(oHandler);
                oAccountingActivity.Add(oItem);
            }
            return oAccountingActivity;
        }

        #endregion

        #region Interface implementation
        public AccountingActivityService() { }

        public List<AccountingActivity> Gets(int nRoleUserID, DateTime dStartDate, DateTime dEnddate, int nUserID)
        {
            List<AccountingActivity> oAccountingActivity = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountingActivityDA.Gets(tc, nRoleUserID, dStartDate, dEnddate); 
                oAccountingActivity = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingActivity", e);
                #endregion
            }

            return oAccountingActivity;
        }

       
       
        #endregion
    }   
}