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
    public class VPTransactionService : MarshalByRefObject, IVPTransactionService
    {
        #region Private functions and declaration
        private VPTransaction MapObject(NullHandler oReader)
        {
            VPTransaction oVPTransaction = new VPTransaction();
            oVPTransaction.VPTransactionID = oReader.GetInt32("VPTransactionID");           
            oVPTransaction.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oVPTransaction.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVPTransaction.VoucherID = oReader.GetInt32("VoucherID");
            oVPTransaction.Qty = oReader.GetDouble("Qty");
            oVPTransaction.IsDr = oReader.GetBoolean("IsDr");
            oVPTransaction.UnitPrice = oReader.GetDouble("UnitPrice");
            oVPTransaction.Amount = oReader.GetDouble("Amount");
            oVPTransaction.CurrencyID = oReader.GetInt32("CurrencyID");
            oVPTransaction.ConversionRate = oReader.GetDouble("ConversionRate");
            oVPTransaction.TransactionDate = oReader.GetDateTime("TransactionDate");
            oVPTransaction.ProductID = oReader.GetInt32("ProductID");
            oVPTransaction.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oVPTransaction.ProductName = oReader.GetString("ProductName");
            oVPTransaction.ProductCode = oReader.GetString("ProductCode");
            oVPTransaction.WorkingUnitName = oReader.GetString("OperationUnitName");
            oVPTransaction.MUnitID = oReader.GetInt32("MUnitID");
            oVPTransaction.MUnitName = oReader.GetString("MUnitName"); 
            oVPTransaction.Description = oReader.GetString("Description");
            oVPTransaction.CurrencySymbol = oReader.GetString("CurrencySymbol");
            return oVPTransaction;
        }
   
      
        private VPTransaction CreateObject(NullHandler oReader)
        {
            VPTransaction oVPTransaction = new VPTransaction();
            oVPTransaction = MapObject(oReader);
            return oVPTransaction;
        }

        private List<VPTransaction> CreateObjects(IDataReader oReader)
        {
            List<VPTransaction> oVPTransaction = new List<VPTransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VPTransaction oItem = CreateObject(oHandler);
                oVPTransaction.Add(oItem);
            }
            return oVPTransaction;
        }
    

        #endregion

        #region Interface implementation
        public VPTransactionService() { }

        public VPTransaction Save(VPTransaction oVPTransaction, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVPTransaction.VPTransactionID <= 0)
                {
                    reader = VPTransactionDA.InsertUpdate(tc, oVPTransaction, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VPTransactionDA.InsertUpdate(tc, oVPTransaction, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVPTransaction = new VPTransaction();
                    oVPTransaction = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VPTransaction. Because of " + e.Message, e);
                #endregion
            }
            return oVPTransaction;
        }
        public bool Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VPTransaction oVPTransaction = new VPTransaction();
                oVPTransaction.VPTransactionID = id;
                VPTransactionDA.Delete(tc, oVPTransaction, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete VPTransaction. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public VPTransaction Get(int id, int nUserID)
        {
            VPTransaction oAccountHead = new VPTransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VPTransactionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VPTransaction", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<VPTransaction> Gets(int nUserID)
        {
            List<VPTransaction> oVPTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VPTransactionDA.Gets(tc);
                oVPTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VPTransaction", e);
                #endregion
            }

            return oVPTransaction;
        }

        public List<VPTransaction> Gets(int nVoucherDetailID, int nUserID)
        {
            List<VPTransaction> oVPTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VPTransactionDA.Gets(tc, nVoucherDetailID);
                oVPTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VPTransaction", e);
                #endregion
            }

            return oVPTransaction;
        }
        public List<VPTransaction> GetsBy(int nVoucherID, int nUserID)
        {
            List<VPTransaction> oVPTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VPTransactionDA.GetsBy(tc, nVoucherID);
                oVPTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VPTransaction", e);
                #endregion
            }

            return oVPTransaction;
        }

        public List<VPTransaction> Gets(string sSQL, int nUserId)
        {
            List<VPTransaction> oVPTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VPTransactionDA.Gets(tc, sSQL);
                oVPTransaction = CreateObjects(reader);
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

            return oVPTransaction;
        }
       


        #endregion
    }   


}
