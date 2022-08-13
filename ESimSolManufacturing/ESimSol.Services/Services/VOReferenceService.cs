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
    public class VOReferenceService : MarshalByRefObject, IVOReferenceService
    {
        #region Private functions and declaration
        private VOReference MapObject(NullHandler oReader)
        {
            VOReference oVOReference = new VOReference();
            oVOReference.VOReferenceID = oReader.GetInt32("VOReferenceID");
            oVOReference.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oVOReference.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVOReference.VoucherID = oReader.GetInt32("VoucherID");
            oVOReference.OrderID = oReader.GetInt32("OrderID");
            oVOReference.TransactionDate = oReader.GetDateTime("TransactionDate");
            oVOReference.Remarks = oReader.GetString("Remarks");
            oVOReference.IsDebit = oReader.GetBoolean("IsDebit");
            oVOReference.CurrencyID = oReader.GetInt32("CurrencyID");
            oVOReference.ConversionRate = oReader.GetDouble("ConversionRate");
            oVOReference.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
            oVOReference.Amount = oReader.GetDouble("Amount");
            oVOReference.CCTID = oReader.GetInt32("CCTID");
            oVOReference.RefNo = oReader.GetString("RefNo");
            oVOReference.OrderNo = oReader.GetString("OrderNo");
            oVOReference.OrderDate = oReader.GetDateTime("OrderDate");
            oVOReference.SubledgerID = oReader.GetInt32("SubledgerID");
            oVOReference.SubledgerName = oReader.GetString("SubledgerName");
            oVOReference.VoucherNo = oReader.GetString("VoucherNo");
            oVOReference.CurrencyName = oReader.GetString("CurrencyName");
            oVOReference.Symbol = oReader.GetString("Symbol");
            oVOReference.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oVOReference.AccountHeadName = oReader.GetString("AccountHeadName");
            oVOReference.ComponentID = oReader.GetInt32("ComponentID");
            return oVOReference;
        }

        private VOReference CreateObject(NullHandler oReader)
        {
            VOReference oVOReference = new VOReference();
            oVOReference = MapObject(oReader);
            return oVOReference;
        }

        private List<VOReference> CreateObjects(IDataReader oReader)
        {
            List<VOReference> oVOReference = new List<VOReference>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VOReference oItem = CreateObject(oHandler);
                oVOReference.Add(oItem);
            }
            return oVOReference;
        }

        #endregion

        #region Interface implementation
        public VOReferenceService() { }

        public VOReference Save(VOReference oVOReference, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVOReference.VOReferenceID <= 0)
                {
                    reader = VOReferenceDA.InsertUpdate(tc, oVOReference, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VOReferenceDA.InsertUpdate(tc, oVOReference, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVOReference = new VOReference();
                    oVOReference = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VOReference. Because of " + e.Message, e);
                #endregion
            }
            return oVOReference;
        }
        public bool Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VOReference oVOReference = new VOReference();
                oVOReference.VOReferenceID = id;
                VOReferenceDA.Delete(tc, oVOReference, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete VOReference. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public VOReference Get(int id, int nUserID)
        {
            VOReference oVOReference = new VOReference();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VOReferenceDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVOReference = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VOReference", e);
                #endregion
            }

            return oVOReference;
        }

        public List<VOReference> Gets(int nUserID)
        {
            List<VOReference> oVOReferences = new List<VOReference>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VOReferenceDA.Gets(tc);
                oVOReferences = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VOReference", e);
                #endregion
            }

            return oVOReferences;
        }

        public List<VOReference> Gets(int nVoucherDetailID, int nUserID)
        {
            List<VOReference> oVOReferences = new List<VOReference>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VOReferenceDA.Gets(tc, nVoucherDetailID);
                oVOReferences = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VOReference", e);
                #endregion
            }

            return oVOReferences;
        }
        public List<VOReference> GetsByOrder(int nVOrderID, int nUserID)
        {
            List<VOReference> oVOReferences = new List<VOReference>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VOReferenceDA.GetsByOrder(tc, nVOrderID);
                oVOReferences = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VOReference", e);
                #endregion
            }

            return oVOReferences;
        }
        public List<VOReference> GetsBy(int nVoucherlID, int nUserID)
        {
            List<VOReference> oVOReferences = new List<VOReference>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VOReferenceDA.GetsBy(tc, nVoucherlID);
                oVOReferences = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VOReference", e);
                #endregion
            }

            return oVOReferences;
        }
        public List<VOReference> Gets(string sSQL, int nUserId)
        {
            List<VOReference> oVOReferences = new List<VOReference>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VOReferenceDA.Gets(tc, sSQL);
                oVOReferences  = CreateObjects(reader);
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

            return oVOReferences ;
        }

        #endregion
    }   


}
