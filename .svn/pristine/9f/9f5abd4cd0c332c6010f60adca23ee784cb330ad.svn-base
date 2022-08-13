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
    public class VoucherBillTransactionService : MarshalByRefObject, IVoucherBillTransactionService
    {
        #region Private functions and declaration
        private VoucherBillTransaction MapObject(NullHandler oReader)
        {
            VoucherBillTransaction oVoucherBillTransaction = new VoucherBillTransaction();
            oVoucherBillTransaction.VoucherBillTransactionID = oReader.GetInt32("VoucherBillTransactionID");
            oVoucherBillTransaction.VoucherBillID = oReader.GetInt32("VoucherBillID");
            oVoucherBillTransaction.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oVoucherBillTransaction.Amount = oReader.GetDouble("Amount");
            oVoucherBillTransaction.IsDr = oReader.GetBoolean("IsDr");
            oVoucherBillTransaction.CurrencyID = oReader.GetInt32("CurrencyID");
            oVoucherBillTransaction.ConversionRate = oReader.GetDouble("ConversionRate");
            oVoucherBillTransaction.TransactionDate = oReader.GetDateTime("TransactionDate");
            oVoucherBillTransaction.TrType = (EnumVoucherBillTrType)oReader.GetInt32("TrType");            
            oVoucherBillTransaction.BillNo = oReader.GetString("BillNo"); 
            oVoucherBillTransaction.BillAmount = oReader.GetDouble("BillAmount");
            oVoucherBillTransaction.BillDate = oReader.GetDateTime("BillDate");
            oVoucherBillTransaction.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oVoucherBillTransaction.AccountHeadName = oReader.GetString("AccountHeadName");
            oVoucherBillTransaction.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVoucherBillTransaction.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oVoucherBillTransaction.BaseCurrencyID = oReader.GetInt32("BaseCurrencyID");
            oVoucherBillTransaction.BaseCurrencySymbol = oReader.GetString("BaseCurrencySymbol");
            oVoucherBillTransaction.VoucherNo = oReader.GetString("VoucherNo");
            oVoucherBillTransaction.VoucherID = oReader.GetInt32("VoucherID");            
            oVoucherBillTransaction.CCTID = oReader.GetInt32("CCTID");
            oVoucherBillTransaction.CCID = oReader.GetInt32("CCID");
            oVoucherBillTransaction.CostCenterCode = oReader.GetString("CostCenterCode");
            oVoucherBillTransaction.CostCenterName = oReader.GetString("CostCenterName");
            
            return oVoucherBillTransaction;
        }
   
      
        private VoucherBillTransaction CreateObject(NullHandler oReader)
        {
            VoucherBillTransaction oVoucherBillTransaction = new VoucherBillTransaction();
            oVoucherBillTransaction = MapObject(oReader);
            return oVoucherBillTransaction;
        }

        private List<VoucherBillTransaction> CreateObjects(IDataReader oReader)
        {
            List<VoucherBillTransaction> oVoucherBillTransaction = new List<VoucherBillTransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherBillTransaction oItem = CreateObject(oHandler);
                oVoucherBillTransaction.Add(oItem);
            }
            return oVoucherBillTransaction;
        }
    

        #endregion

        #region Interface implementation
        public VoucherBillTransactionService() { }

        public VoucherBillTransaction Save(VoucherBillTransaction oVoucherBillTransaction, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVoucherBillTransaction.VoucherBillTransactionID <= 0)
                {
                    reader = VoucherBillTransactionDA.InsertUpdate(tc, oVoucherBillTransaction, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VoucherBillTransactionDA.InsertUpdate(tc, oVoucherBillTransaction, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherBillTransaction = new VoucherBillTransaction();
                    oVoucherBillTransaction = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VoucherBillTransaction. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherBillTransaction;
        }
        public bool Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherBillTransaction oVoucherBillTransaction = new VoucherBillTransaction();
                oVoucherBillTransaction.VoucherBillTransactionID = id;
                VoucherBillTransactionDA.Delete(tc, oVoucherBillTransaction, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete VoucherBillTransaction. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public VoucherBillTransaction Get(int id, int nUserID)
        {
            VoucherBillTransaction oAccountHead = new VoucherBillTransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherBillTransactionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VoucherBillTransaction", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<VoucherBillTransaction> Gets(int nUserID)
        {
            List<VoucherBillTransaction> oVoucherBillTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBillTransactionDA.Gets(tc);
                oVoucherBillTransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBillTransaction", e);
                #endregion
            }

            return oVoucherBillTransactions;
        }

        public List<VoucherBillTransaction> Gets(int nVoucherDetailID, int nUserID)
        {
            List<VoucherBillTransaction> oVoucherBillTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBillTransactionDA.Gets(tc, nVoucherDetailID);
                oVoucherBillTransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBillTransaction", e);
                #endregion
            }

            return oVoucherBillTransactions;
        }
        public List<VoucherBillTransaction> GetsBy(int nVoucherID, int nUserID)
        {
            List<VoucherBillTransaction> oVoucherBillTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBillTransactionDA.GetsBy(tc, nVoucherID);
                oVoucherBillTransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBillTransaction", e);
                #endregion
            }

            return oVoucherBillTransactions;
        }

        public List<VoucherBillTransaction> Gets(string sSQL, int nUserId)
        {
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBillTransactionDA.Gets(tc, sSQL);
                oVoucherBillTransactions = CreateObjects(reader);
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

            return oVoucherBillTransactions;
        }
        #endregion
    }   
}
