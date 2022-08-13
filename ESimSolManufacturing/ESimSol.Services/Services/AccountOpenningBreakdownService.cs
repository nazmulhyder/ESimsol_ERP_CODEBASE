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
    public class AccountOpenningBreakdownService : MarshalByRefObject, IAccountOpenningBreakdownService
    {
        #region Private functions and declaration
        private AccountOpenningBreakdown MapObject(NullHandler oReader)
        {
            AccountOpenningBreakdown oAccountOpenningBreakdown = new AccountOpenningBreakdown();
            oAccountOpenningBreakdown.AccountOpenningBreakdownID = oReader.GetInt32("AccountOpenningBreakdownID");
            oAccountOpenningBreakdown.AccountingSessionID = oReader.GetInt32("AccountingSessionID");
            oAccountOpenningBreakdown.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oAccountOpenningBreakdown.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oAccountOpenningBreakdown.BreakdownObjID = oReader.GetInt32("BreakdownObjID");
            oAccountOpenningBreakdown.IsDr = oReader.GetBoolean("IsDr");
            oAccountOpenningBreakdown.BreakdownType = (EnumBreakdownType)oReader.GetInt32("BreakdownType");
            oAccountOpenningBreakdown.MUnitID = oReader.GetInt32("MUnitID");
            oAccountOpenningBreakdown.WUnitID = oReader.GetInt32("WUnitID");
            oAccountOpenningBreakdown.UnitPrice = oReader.GetDouble("UnitPrice");
            oAccountOpenningBreakdown.Qty = oReader.GetDouble("Qty");
            oAccountOpenningBreakdown.CurrencyID = oReader.GetInt32("CurrencyID");
            oAccountOpenningBreakdown.ConversionRate = oReader.GetDouble("ConversionRate");
            oAccountOpenningBreakdown.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
            oAccountOpenningBreakdown.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oAccountOpenningBreakdown.AccountCode = oReader.GetString("AccountCode");
            oAccountOpenningBreakdown.AccountHeadName = oReader.GetString("AccountHeadName");
            oAccountOpenningBreakdown.SessionName = oReader.GetString("SessionName");
            oAccountOpenningBreakdown.UnitName = oReader.GetString("UnitName");
            oAccountOpenningBreakdown.Symbol = oReader.GetString("Symbol");
            oAccountOpenningBreakdown.BUName = oReader.GetString("BUName");
            oAccountOpenningBreakdown.BUCode = oReader.GetString("BUCode");
            oAccountOpenningBreakdown.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oAccountOpenningBreakdown.CurrencyName = oReader.GetString("CurrencyName");
            oAccountOpenningBreakdown.BreakdownCode = oReader.GetString("BreakdownCode");
            oAccountOpenningBreakdown.BreakdownName = oReader.GetString("BreakdownName");
            oAccountOpenningBreakdown.WUName = oReader.GetString("WUName");
            oAccountOpenningBreakdown.CCID = oReader.GetInt32("CCID");
            oAccountOpenningBreakdown.CCName = oReader.GetString("CCName");
            oAccountOpenningBreakdown.CCCode = oReader.GetString("CCCode");
            oAccountOpenningBreakdown.IsBTAply = oReader.GetBoolean("IsBTAply");
            if (oAccountOpenningBreakdown.IsDr)
            {
                oAccountOpenningBreakdown.DrAmount = oReader.GetDouble("AmountInCurrency");
                oAccountOpenningBreakdown.CrAmount = 0.0;
                oAccountOpenningBreakdown.BCDrAmount = oReader.GetDouble("OpenningBalance");
                oAccountOpenningBreakdown.BCCrAmount = 0.0;
            }
            else
            {
                oAccountOpenningBreakdown.DrAmount = 0.0;
                oAccountOpenningBreakdown.CrAmount = oReader.GetDouble("AmountInCurrency");
                oAccountOpenningBreakdown.BCDrAmount = 0.0;
                oAccountOpenningBreakdown.BCCrAmount = oReader.GetDouble("OpenningBalance");
            }
            return oAccountOpenningBreakdown;
        }

        private AccountOpenningBreakdown CreateObject(NullHandler oReader)
        {
            AccountOpenningBreakdown oAccountOpenningBreakdown = new AccountOpenningBreakdown();
            oAccountOpenningBreakdown = MapObject(oReader);
            return oAccountOpenningBreakdown;
        }

        private List<AccountOpenningBreakdown> CreateObjects(IDataReader oReader)
        {
            List<AccountOpenningBreakdown> oAccountOpenningBreakdown = new List<AccountOpenningBreakdown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountOpenningBreakdown oItem = CreateObject(oHandler);
                oAccountOpenningBreakdown.Add(oItem);
            }
            return oAccountOpenningBreakdown;
        }

        #endregion

        #region Interface implementation
        public AccountOpenningBreakdownService() { }

        public AccountOpenningBreakdown Save(AccountOpenningBreakdown oAccountOpenningBreakdown, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oAccountOpenningBreakdown.AccountOpenningBreakdownID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AccountOpenning, EnumRoleOperationType.Add);
                    reader = AccountOpenningBreakdownDA.InsertUpdate(tc, oAccountOpenningBreakdown, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AccountOpenning, EnumRoleOperationType.Edit);
                    reader = AccountOpenningBreakdownDA.InsertUpdate(tc, oAccountOpenningBreakdown, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountOpenningBreakdown = new AccountOpenningBreakdown();
                    oAccountOpenningBreakdown = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save AccountOpenningBreakdown. Because of " + e.Message, e);
                #endregion
            }
            return oAccountOpenningBreakdown;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountOpenningBreakdown oAccountOpenningBreakdown = new AccountOpenningBreakdown();
                oAccountOpenningBreakdown.AccountOpenningBreakdownID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AccountOpenning, EnumRoleOperationType.Delete);
                AccountOpenningBreakdownDA.Delete(tc, oAccountOpenningBreakdown, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete AccountOpenningBreakdown. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public AccountOpenningBreakdown Get(int id, int nCompanyID, int nUserId)
        {
            AccountOpenningBreakdown oAccountHead = new AccountOpenningBreakdown();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountOpenningBreakdownDA.Get(tc, id, nCompanyID);
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
                throw new ServiceException("Failed to Get AccountOpenningBreakdown", e);
                #endregion
            }

            return oAccountHead;
        }

        public AccountOpenningBreakdown GetsByAccountAndSubledgerAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID)
        {
            AccountOpenningBreakdown oAccountHead = new AccountOpenningBreakdown();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountOpenningBreakdownDA.GetsByAccountAndSubledgerAndSession(tc, nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID);
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
                throw new ServiceException("Failed to Get AccountOpenningBreakdown", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<AccountOpenningBreakdown> Gets(int nCompanyID, int nUserID)
        {
            List<AccountOpenningBreakdown> oAccountOpenningBreakdown = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountOpenningBreakdownDA.Gets(tc, nCompanyID);
                oAccountOpenningBreakdown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountOpenningBreakdown", e);
                #endregion
            }

            return oAccountOpenningBreakdown;
        }

        public List<AccountOpenningBreakdown> GetsSubLedgerWiseBills(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID)
        {
            List<AccountOpenningBreakdown> oAccountOpenningBreakdown = new List<AccountOpenningBreakdown>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountOpenningBreakdownDA.GetsSubLedgerWiseBills(tc, nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID);
                oAccountOpenningBreakdown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountOpenningBreakdown", e);
                #endregion
            }

            return oAccountOpenningBreakdown;
        }

        public List<AccountOpenningBreakdown> GetsByAccountAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nUserID)
        {
            List<AccountOpenningBreakdown> oAccountOpenningBreakdown = new List<AccountOpenningBreakdown>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountOpenningBreakdownDA.GetsByAccountAndSession(tc, nAccountHeadID, nAccountingSessionID, nBusinessUnitID);
                oAccountOpenningBreakdown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountOpenningBreakdown", e);
                #endregion
            }

            return oAccountOpenningBreakdown;
        }
        public List<AccountOpenningBreakdown> Gets(string sSQL,int nCompanyID, int nUserID)
        {
            List<AccountOpenningBreakdown> oAccountOpenningBreakdown = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from AccountOpenningBreakdown where AccountOpenningBreakdownID IN (1,2,80,272,347,370,60,45) AND CompanyID = " + nCompanyID + " )";
                }
                reader = AccountOpenningBreakdownDA.Gets(tc, sSQL);
                oAccountOpenningBreakdown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountOpenningBreakdown", e);
                #endregion
            }

            return oAccountOpenningBreakdown;
        }
        #endregion
    }
}
