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
    public class AccountOpenningService : MarshalByRefObject, IAccountOpenningService
    {
        #region Private functions and declaration
        private AccountOpenning MapObject(NullHandler oReader)
        {
            AccountOpenning oAccountOpenning = new AccountOpenning();
            oAccountOpenning.AccountOpenningID = oReader.GetInt32("AccountOpenningID");
            oAccountOpenning.AccountingSessionID = oReader.GetInt32("AccountingSessionID");
            oAccountOpenning.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oAccountOpenning.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oAccountOpenning.IsDebit = oReader.GetBoolean("IsDebit");
            oAccountOpenning.CurrencyID = oReader.GetInt32("CurrencyID");
            oAccountOpenning.AmountInCurrency = oReader.GetDouble("AmountInCurrency");            
            oAccountOpenning.ConversionRate = oReader.GetDouble("ConversionRate");
            oAccountOpenning.OpenningBalance = oReader.GetDouble("OpenningBalance");            
            oAccountOpenning.BUCode = oReader.GetString("BUCode");
            oAccountOpenning.BUName = oReader.GetString("BUName");
            oAccountOpenning.AccountCode = oReader.GetString("AccountCode");
            oAccountOpenning.AccountHeadName = oReader.GetString("AccountHeadName");
            oAccountOpenning.SessionName = oReader.GetString("SessionName");
            oAccountOpenning.CName = oReader.GetString("CName");
            oAccountOpenning.CSymbol = oReader.GetString("CSymbol");
            oAccountOpenning.ComponentID = oReader.GetInt32("ComponentID");
            oAccountOpenning.ComponentType = oReader.GetString("ComponentType");
            if (oAccountOpenning.IsDebit)
            {
                oAccountOpenning.DrAmount = oReader.GetDouble("AmountInCurrency");
                oAccountOpenning.CrAmount = 0.00;
                oAccountOpenning.BCDrAmount = oReader.GetDouble("OpenningBalance");
                oAccountOpenning.BCCrAmount = 0.00;
            }
            else
            {
                oAccountOpenning.DrAmount = 0.00;
                oAccountOpenning.CrAmount = oReader.GetDouble("AmountInCurrency");
                oAccountOpenning.BCDrAmount = 0.00;
                oAccountOpenning.BCCrAmount = oReader.GetDouble("OpenningBalance"); 
            }
            return oAccountOpenning;
        }

        private AccountOpenning CreateObject(NullHandler oReader)
        {
            AccountOpenning oAccountOpenning = new AccountOpenning();
            oAccountOpenning = MapObject(oReader);
            return oAccountOpenning;
        }

        private List<AccountOpenning> CreateObjects(IDataReader oReader)
        {
            List<AccountOpenning> oAccountOpenning = new List<AccountOpenning>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountOpenning oItem = CreateObject(oHandler);
                oAccountOpenning.Add(oItem);
            }
            return oAccountOpenning;
        }

        #endregion

        #region Interface implementation
        public AccountOpenningService() { }

        public AccountOpenning SetOpenningBalance(AccountOpenning oAccountOpenning, int nUserId)
        {            
            //string sAccountOpenningBreakdownIDs = "";
            List<AccountOpenningBreakdown> oAccountOpenningBreakdowns = new List<AccountOpenningBreakdown>();
            oAccountOpenningBreakdowns = oAccountOpenning.AccountOpenningBreakdowns;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader readerAccountOpenning = null;
                readerAccountOpenning = AccountOpenningDA.SetOpenningBalance(tc, oAccountOpenning, nUserId);
                NullHandler oReaderAccountOpenning = new NullHandler(readerAccountOpenning);
                if (readerAccountOpenning.Read())
                {
                    oAccountOpenning = new AccountOpenning();
                    oAccountOpenning = CreateObject(oReaderAccountOpenning);                    
                }
                readerAccountOpenning.Close();

                #region AccountOpenningBreakdowns
                if (oAccountOpenningBreakdowns != null)
                {
                    if (oAccountOpenningBreakdowns.Count > 0)
                    {
                        foreach (AccountOpenningBreakdown oAccountOpenningBreakdown in oAccountOpenningBreakdowns)
                        {
                            IDataReader readerAccountOpenningBreakdown = null;
                            oAccountOpenningBreakdown.AccountingSessionID = oAccountOpenning.AccountingSessionID;
                            //oAccountOpenningBreakdown.CurrencyID = oAccountOpenning.CurrencyID;
                            //oAccountOpenningBreakdown.ConversionRate = oAccountOpenning.ConversionRate;                            

                            readerAccountOpenningBreakdown = AccountOpenningBreakdownDA.InsertUpdate(tc, oAccountOpenningBreakdown, EnumDBOperation.Insert, nUserId);
                            NullHandler oReaderAccountOpenningBreakdown = new NullHandler(readerAccountOpenningBreakdown);                            
                            readerAccountOpenningBreakdown.Close();
                        }
                    }
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAccountOpenning = new AccountOpenning();
                oAccountOpenning.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oAccountOpenning;
        }

        public AccountOpenning SetOpenningBalanceSubledger(AccountOpenning oAccountOpenning, int nUserId)
        {
            //string sAccountOpenningBreakdownIDs = "";
            List<AccountOpenningBreakdown> oAccountOpenningBreakdowns = new List<AccountOpenningBreakdown>();
            oAccountOpenningBreakdowns = oAccountOpenning.AccountOpenningBreakdowns;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader readerAccountOpenning = null;
                readerAccountOpenning = AccountOpenningDA.SetOpenningBalanceSubledger(tc, oAccountOpenning, nUserId);
                NullHandler oReaderAccountOpenning = new NullHandler(readerAccountOpenning);
                if (readerAccountOpenning.Read())
                {
                    oAccountOpenning = new AccountOpenning();
                    oAccountOpenning = CreateObject(oReaderAccountOpenning);
                }
                readerAccountOpenning.Close();

                #region AccountOpenningBreakdowns
                if (oAccountOpenningBreakdowns != null)
                {
                    if (oAccountOpenningBreakdowns.Count > 0)
                    {
                        foreach (AccountOpenningBreakdown oAccountOpenningBreakdown in oAccountOpenningBreakdowns)
                        {
                            IDataReader readerAccountOpenningBreakdown = null;
                            oAccountOpenningBreakdown.AccountingSessionID = oAccountOpenning.AccountingSessionID;
                            //oAccountOpenningBreakdown.CurrencyID = oAccountOpenning.CurrencyID;
                            //oAccountOpenningBreakdown.ConversionRate = oAccountOpenning.ConversionRate;                            

                            readerAccountOpenningBreakdown = AccountOpenningBreakdownDA.InsertUpdate(tc, oAccountOpenningBreakdown, EnumDBOperation.Insert, nUserId);
                            NullHandler oReaderAccountOpenningBreakdown = new NullHandler(readerAccountOpenningBreakdown);
                            readerAccountOpenningBreakdown.Close();
                        }
                    }
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAccountOpenning = new AccountOpenning();
                oAccountOpenning.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oAccountOpenning;
        }

        public AccountOpenning Get(int id, int nCompanyID, int nUserId)
        {
            AccountOpenning oAccountHead = new AccountOpenning();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountOpenningDA.Get(tc, id, nCompanyID);
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
                throw new ServiceException("Failed to Get AccountOpenning", e);
                #endregion
            }

            return oAccountHead;
        }

        public AccountOpenning Get(int nAccountHeadID, int nSessionID, int nBusinessUnitID, int nUserID)
        {
            AccountOpenning oAccountHead = new AccountOpenning();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountOpenningDA.Get(tc, nAccountHeadID, nSessionID, nBusinessUnitID);
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
                throw new ServiceException("Failed to Get AccountOpenning", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<AccountOpenning> GetsByAccountAndSession(int nAccountHeadID, int nSessionID, int nUserID)
        {
            List<AccountOpenning> oAccountOpenning = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountOpenningDA.GetsByAccountAndSession(tc, nAccountHeadID, nSessionID, nUserID);
                oAccountOpenning = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountOpenning", e);
                #endregion
            }

            return oAccountOpenning;
        }

        public List<AccountOpenning> Gets(int nSessionID, int nCompanyID, int nUserID)
        {
            List<AccountOpenning> oAccountOpenning = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountOpenningDA.Gets(tc, nSessionID, nCompanyID);
                oAccountOpenning = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountOpenning", e);
                #endregion
            }

            return oAccountOpenning;
        }

        public AccountOpenning Save(AccountOpenning oAccountOpenning, int nUserId)
        {
            return oAccountOpenning;
        }
        #endregion
    }

}
