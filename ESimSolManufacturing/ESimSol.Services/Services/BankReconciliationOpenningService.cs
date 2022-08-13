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
    public class BankReconciliationOpenningService : MarshalByRefObject, IBankReconciliationOpenningService
    {
        #region Private functions and declaration
        private BankReconciliationOpenning MapObject(NullHandler oReader)
        {
            BankReconciliationOpenning oBankReconciliationOpenning = new BankReconciliationOpenning();
            oBankReconciliationOpenning.BankReconciliationOpenningID = oReader.GetInt32("BankReconciliationOpenningID");
            oBankReconciliationOpenning.AccountingSessionID = oReader.GetInt32("AccountingSessionID");
            oBankReconciliationOpenning.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oBankReconciliationOpenning.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oBankReconciliationOpenning.SubledgerID = oReader.GetInt32("SubledgerID");
            oBankReconciliationOpenning.IsDr = oReader.GetBoolean("IsDr");            
            oBankReconciliationOpenning.CurrencyID = oReader.GetInt32("CurrencyID");
            oBankReconciliationOpenning.ConversionRate = oReader.GetDouble("ConversionRate");
            oBankReconciliationOpenning.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
            oBankReconciliationOpenning.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oBankReconciliationOpenning.AccountCode = oReader.GetString("AccountCode");
            oBankReconciliationOpenning.AccountHeadName = oReader.GetString("AccountHeadName");
            oBankReconciliationOpenning.SessionName = oReader.GetString("SessionName");            
            oBankReconciliationOpenning.BUName = oReader.GetString("BUName");
            oBankReconciliationOpenning.BUCode = oReader.GetString("BUCode");
            oBankReconciliationOpenning.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oBankReconciliationOpenning.CurrencyName = oReader.GetString("CurrencyName");            
            if (oBankReconciliationOpenning.IsDr)
            {
                oBankReconciliationOpenning.DrAmount = oReader.GetDouble("AmountInCurrency");
                oBankReconciliationOpenning.CrAmount = 0.0;
                oBankReconciliationOpenning.BCDrAmount = oReader.GetDouble("OpenningBalance");
                oBankReconciliationOpenning.BCCrAmount = 0.0;
            }
            else
            {
                oBankReconciliationOpenning.DrAmount = 0.0;
                oBankReconciliationOpenning.CrAmount = oReader.GetDouble("AmountInCurrency");
                oBankReconciliationOpenning.BCDrAmount = 0.0;
                oBankReconciliationOpenning.BCCrAmount = oReader.GetDouble("OpenningBalance");
            }
            return oBankReconciliationOpenning;
        }

        private BankReconciliationOpenning CreateObject(NullHandler oReader)
        {
            BankReconciliationOpenning oBankReconciliationOpenning = new BankReconciliationOpenning();
            oBankReconciliationOpenning = MapObject(oReader);
            return oBankReconciliationOpenning;
        }

        private List<BankReconciliationOpenning> CreateObjects(IDataReader oReader)
        {
            List<BankReconciliationOpenning> oBankReconciliationOpenning = new List<BankReconciliationOpenning>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BankReconciliationOpenning oItem = CreateObject(oHandler);
                oBankReconciliationOpenning.Add(oItem);
            }
            return oBankReconciliationOpenning;
        }

        #endregion

        #region Interface implementation
        public BankReconciliationOpenningService() { }

        public BankReconciliationOpenning Save(BankReconciliationOpenning oBankReconciliationOpenning, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBankReconciliationOpenning.BankReconciliationOpenningID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.BankReconciliationOpenning, EnumRoleOperationType.Add);
                    reader = BankReconciliationOpenningDA.InsertUpdate(tc, oBankReconciliationOpenning, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.BankReconciliationOpenning, EnumRoleOperationType.Edit);
                    reader = BankReconciliationOpenningDA.InsertUpdate(tc, oBankReconciliationOpenning, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankReconciliationOpenning = new BankReconciliationOpenning();
                    oBankReconciliationOpenning = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save BankReconciliationOpenning. Because of " + e.Message, e);
                #endregion
            }
            return oBankReconciliationOpenning;
        }
        public string BRBalanceTranfer(BankReconciliationOpenning oBankReconciliationOpenning, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BankReconciliationOpenningDA.BRBalanceTranfer(tc, oBankReconciliationOpenning, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save BankReconciliationOpenning. Because of " + e.Message, e);
                #endregion
            }
            return Global.SuccessMessage;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BankReconciliationOpenning oBankReconciliationOpenning = new BankReconciliationOpenning();
                oBankReconciliationOpenning.BankReconciliationOpenningID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AccountOpenning, EnumRoleOperationType.Delete);
                BankReconciliationOpenningDA.Delete(tc, oBankReconciliationOpenning, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BankReconciliationOpenning. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public BankReconciliationOpenning Get(int id, int nCompanyID, int nUserId)
        {
            BankReconciliationOpenning oAccountHead = new BankReconciliationOpenning();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BankReconciliationOpenningDA.Get(tc, id, nCompanyID);
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
                throw new ServiceException("Failed to Get BankReconciliationOpenning", e);
                #endregion
            }

            return oAccountHead;
        }

        public BankReconciliationOpenning GetsByAccountAndSubledgerAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID)
        {
            BankReconciliationOpenning oAccountHead = new BankReconciliationOpenning();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BankReconciliationOpenningDA.GetsByAccountAndSubledgerAndSession(tc, nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID);
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
                throw new ServiceException("Failed to Get BankReconciliationOpenning", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BankReconciliationOpenning> Gets(int nCompanyID, int nUserID)
        {
            List<BankReconciliationOpenning> oBankReconciliationOpenning = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankReconciliationOpenningDA.Gets(tc, nCompanyID);
                oBankReconciliationOpenning = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankReconciliationOpenning", e);
                #endregion
            }

            return oBankReconciliationOpenning;
        }

        public List<BankReconciliationOpenning> GetsSubLedgerWiseBills(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID, int nUserID)
        {
            List<BankReconciliationOpenning> oBankReconciliationOpenning = new List<BankReconciliationOpenning>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankReconciliationOpenningDA.GetsSubLedgerWiseBills(tc, nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID);
                oBankReconciliationOpenning = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankReconciliationOpenning", e);
                #endregion
            }

            return oBankReconciliationOpenning;
        }

        public List<BankReconciliationOpenning> GetsByAccountAndSession(int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nUserID)
        {
            List<BankReconciliationOpenning> oBankReconciliationOpenning = new List<BankReconciliationOpenning>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankReconciliationOpenningDA.GetsByAccountAndSession(tc, nAccountHeadID, nAccountingSessionID, nBusinessUnitID);
                oBankReconciliationOpenning = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankReconciliationOpenning", e);
                #endregion
            }

            return oBankReconciliationOpenning;
        }
        public List<BankReconciliationOpenning> Gets(string sSQL,int nCompanyID, int nUserID)
        {
            List<BankReconciliationOpenning> oBankReconciliationOpenning = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from BankReconciliationOpenning where BankReconciliationOpenningID IN (1,2,80,272,347,370,60,45) AND CompanyID = " + nCompanyID + " )";
                }
                reader = BankReconciliationOpenningDA.Gets(tc, sSQL);
                oBankReconciliationOpenning = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankReconciliationOpenning", e);
                #endregion
            }

            return oBankReconciliationOpenning;
        }
        #endregion
    }
}
