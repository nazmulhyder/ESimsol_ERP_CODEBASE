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
    public class CostCenterTransactionService : MarshalByRefObject, ICostCenterTransactionService
    {
        #region Private functions and declaration
        private CostCenterTransaction MapObject(NullHandler oReader)
        {
            CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
            oCostCenterTransaction.CCTID = oReader.GetInt32("CCTID");
            oCostCenterTransaction.CCID = oReader.GetInt32("CCID");
            oCostCenterTransaction.CostCenterName = oReader.GetString("CostCenterName");
            oCostCenterTransaction.CostCenterCode = oReader.GetString("CostCenterCode");
            oCostCenterTransaction.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oCostCenterTransaction.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oCostCenterTransaction.VoucherID = oReader.GetInt32("VoucherID");        
            oCostCenterTransaction.IsDr = oReader.GetBoolean("IsDr");
            oCostCenterTransaction.Amount = oReader.GetDouble("Amount");
            oCostCenterTransaction.CurrencyID = oReader.GetInt32("CurrencyID");
            oCostCenterTransaction.CurrencyConversionRate = oReader.GetDouble("CurrencyConversionRate");
            oCostCenterTransaction.Description = oReader.GetString("Description");
            oCostCenterTransaction.TransactionDate = oReader.GetDateTime("TransactionDate");
            oCostCenterTransaction.LastUpdateBY = oReader.GetInt32("LastUpdateBY");
            oCostCenterTransaction.LastUpdateDate = oReader.GetDateTime("LastUpdateDate");
            oCostCenterTransaction.CategoryName = oReader.GetString("CategoryName");
            oCostCenterTransaction.CCCategoryID = oReader.GetInt32("CCCategoryID");
            oCostCenterTransaction.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oCostCenterTransaction.IsBillRefApply = oReader.GetBoolean("IsBillRefApply");
            oCostCenterTransaction.IsOrderRefApply = oReader.GetBoolean("IsOrderRefApply");
            oCostCenterTransaction.IsChequeApply = oReader.GetBoolean("IsChequeApply");
            oCostCenterTransaction.AmountBC = oReader.GetDouble("AmountBC");
            return oCostCenterTransaction;
        }
        private CostCenterTransaction MapObject_Report(NullHandler oReader)
        {
            CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
            //oCostCenterTransaction.CCTID = oReader.GetInt32("CCTID");
            oCostCenterTransaction.CCID = oReader.GetInt32("CCID");
            oCostCenterTransaction.CostCenterName = oReader.GetString("CostCenterName");
            oCostCenterTransaction.AccountHeadName = oReader.GetString("AccountHeadName");
            oCostCenterTransaction.VoucherNo = oReader.GetString("VoucherNo");
            //oCostCenterTransaction.DR_CR = oReader.GetString("IsDr");
            oCostCenterTransaction.IsDr = oReader.GetBoolean("IsDr");
            oCostCenterTransaction.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oCostCenterTransaction.Amount = oReader.GetDouble("Amount");
            oCostCenterTransaction.Description = oReader.GetString("Description");
            oCostCenterTransaction.TransactionDate = oReader.GetDateTime("TransactionDate");
            oCostCenterTransaction.CurrentBalance = oReader.GetDouble("CurrentBalance");
           

            return oCostCenterTransaction;
        }
        private CostCenterTransaction CreateObject_report(NullHandler oReader)
        {
            CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
            oCostCenterTransaction = MapObject_Report(oReader);
            return oCostCenterTransaction;
        }
        private CostCenterTransaction CreateObject(NullHandler oReader)
        {
            CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
            oCostCenterTransaction = MapObject(oReader);
            return oCostCenterTransaction;
        }

        private List<CostCenterTransaction> CreateObjects(IDataReader oReader)
        {
            List<CostCenterTransaction> oCostCenterTransaction = new List<CostCenterTransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostCenterTransaction oItem = CreateObject(oHandler);
                oCostCenterTransaction.Add(oItem);
            }
            return oCostCenterTransaction;
        }
        private List<CostCenterTransaction> CreateObjects_report(IDataReader oReader)
        {
            List<CostCenterTransaction> oCostCenterTransaction = new List<CostCenterTransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostCenterTransaction oItem = CreateObject_report(oHandler);
                oCostCenterTransaction.Add(oItem);
            }
            return oCostCenterTransaction;
        }

        #endregion

        #region Interface implementation
        public CostCenterTransactionService() { }

        public CostCenterTransaction Save(CostCenterTransaction oCostCenterTransaction, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCostCenterTransaction.CCTID <= 0)
                {
                    reader = CostCenterTransactionDA.InsertUpdate(tc, oCostCenterTransaction, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = CostCenterTransactionDA.InsertUpdate(tc, oCostCenterTransaction, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostCenterTransaction = new CostCenterTransaction();
                    oCostCenterTransaction = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save CostCenterTransaction. Because of " + e.Message, e);
                #endregion
            }
            return oCostCenterTransaction;
        }
        public bool Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
                oCostCenterTransaction.CCTID = id;
                CostCenterTransactionDA.Delete(tc, oCostCenterTransaction, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete CostCenterTransaction. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public CostCenterTransaction Get(int id, int nUserID)
        {
            CostCenterTransaction oAccountHead = new CostCenterTransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CostCenterTransactionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get CostCenterTransaction", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CostCenterTransaction> Gets(int nUserID)
        {
            List<CostCenterTransaction> oCostCenterTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterTransactionDA.Gets(tc);
                oCostCenterTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterTransaction", e);
                #endregion
            }

            return oCostCenterTransaction;
        }

        public List<CostCenterTransaction> Gets(int nVoucherDetailID, int nUserID)
        {
            List<CostCenterTransaction> oCostCenterTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterTransactionDA.Gets(tc, nVoucherDetailID);
                oCostCenterTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterTransaction", e);
                #endregion
            }

            return oCostCenterTransaction;
        }
        public List<CostCenterTransaction> GetsBy(int nVoucherID, int nUserID)
        {
            List<CostCenterTransaction> oCostCenterTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterTransactionDA.GetsBy(tc, nVoucherID);
                oCostCenterTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterTransaction", e);
                #endregion
            }

            return oCostCenterTransaction;
        }

        public List<CostCenterTransaction> GetsbyCC(string sSQL, int nUserID)
        {
            List<CostCenterTransaction> oCostCenterTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterTransactionDA.GetsbyCC(tc, sSQL);
                oCostCenterTransaction = CreateObjects_report(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterTransaction", e);
                #endregion
            }

            return oCostCenterTransaction;
        }

        public double GetCurrentBalance(int nCCID, int nCurrencyID, bool bIsApproved, int nUserId)
        {
            double nCurrentBalance = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CostCenterTransactionDA.GetCurrentBalance(tc, nCCID,nCurrencyID, bIsApproved);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    nCurrentBalance = oReader.GetDouble("CurrentBalance");
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return nCurrentBalance;
        }

        public List<CostCenterTransaction> GetsByAcccountHead(int nAHead, int nUserID)
        {
            List<CostCenterTransaction> oCostCenterTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterTransactionDA.GetsByAcccountHead(tc, nAHead);
                oCostCenterTransaction = CreateObjects_report(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterTransaction", e);
                #endregion
            }

            return oCostCenterTransaction;
        }
        public List<CostCenterTransaction> Gets(string sSQL, int nUserID)
        {
            List<CostCenterTransaction> oCostCenterTransaction = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterTransactionDA.Gets(tc, sSQL);
                oCostCenterTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterTransaction", e);
                #endregion
            }

            return oCostCenterTransaction;
        }


        #endregion
    }   


}
