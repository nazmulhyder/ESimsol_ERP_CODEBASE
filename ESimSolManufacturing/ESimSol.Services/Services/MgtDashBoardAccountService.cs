using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class MgtDashBoardAccountService : MarshalByRefObject, IMgtDashBoardAccountService
    {
        #region Private functions and declaration

        private MgtDashBoardAccount MapObject(NullHandler oReader)
        {
            MgtDashBoardAccount oMgtDashBoardAccount = new MgtDashBoardAccount();
            oMgtDashBoardAccount.BUID = oReader.GetInt32("BUID");
            oMgtDashBoardAccount.ReportDate = oReader.GetDateTime("ReportDate");
            oMgtDashBoardAccount.CashBalance = oReader.GetDouble("CashBalance");
            oMgtDashBoardAccount.BankBalance = oReader.GetDouble("BankBalance");
            oMgtDashBoardAccount.ForeignBankBalance = oReader.GetDouble("ForeignBankBalance");
            oMgtDashBoardAccount.Receivable = oReader.GetDouble("Receivable");
            oMgtDashBoardAccount.Payable = oReader.GetDouble("Payable");
            oMgtDashBoardAccount.BankLoan = oReader.GetDouble("BankLoan");

            oMgtDashBoardAccount.CashCurrencyID = oReader.GetInt32("CashCurrencyID");
            oMgtDashBoardAccount.CashCSymbol = oReader.GetString("CashCSymbol");

            oMgtDashBoardAccount.BankCurrencyID = oReader.GetInt32("BankCurrencyID");
            oMgtDashBoardAccount.BankCSymbol = oReader.GetString("BankCSymbol");

            oMgtDashBoardAccount.FBankCurrencyID = oReader.GetInt32("FBankCurrencyID");
            oMgtDashBoardAccount.FBankCSymbol = oReader.GetString("FBankCSymbol");

            oMgtDashBoardAccount.RcvCurrencyID = oReader.GetInt32("RcvCurrencyID");
            oMgtDashBoardAccount.RcvCSymbol = oReader.GetString("RcvCSymbol");

            oMgtDashBoardAccount.PayableCurrencyID = oReader.GetInt32("PayableCurrencyID");
            oMgtDashBoardAccount.PayableCSymbol = oReader.GetString("PayableCSymbol");

            oMgtDashBoardAccount.BLoanCurrencyID = oReader.GetInt32("BLoanCurrencyID");
            oMgtDashBoardAccount.BLoanCSymbol = oReader.GetString("BLoanCSymbol"); 


            return oMgtDashBoardAccount;
        }

        private MgtDashBoardAccount CreateObject(NullHandler oReader)
        {
            MgtDashBoardAccount oMgtDashBoardAccount = new MgtDashBoardAccount();
            oMgtDashBoardAccount = MapObject(oReader);
            return oMgtDashBoardAccount;
        }

        private List<MgtDashBoardAccount> CreateObjects(IDataReader oReader)
        {
            List<MgtDashBoardAccount> oMgtDashBoardAccount = new List<MgtDashBoardAccount>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MgtDashBoardAccount oItem = CreateObject(oHandler);
                oMgtDashBoardAccount.Add(oItem);
            }
            return oMgtDashBoardAccount;
        }

        #endregion

        #region Interface implementation        
        public List<MgtDashBoardAccount> Gets(MgtDashBoardAccount oMgtDashBoardAccount, Int64 nUserID)
        {
            List<MgtDashBoardAccount> oMgtDashBoardAccounts = new List<MgtDashBoardAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MgtDashBoardAccountDA.Gets(tc, oMgtDashBoardAccount);
                oMgtDashBoardAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MgtDashBoardAccount", e);
                #endregion
            }
            return oMgtDashBoardAccounts;
        }

        #endregion
    }

}
