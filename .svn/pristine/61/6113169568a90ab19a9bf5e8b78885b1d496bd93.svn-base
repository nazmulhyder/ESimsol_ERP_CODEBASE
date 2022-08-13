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
    public class VoucherReferenceService : MarshalByRefObject, IVoucherReferenceService
    {
        #region Private functions and declaration
        private VoucherReference MapObject(NullHandler oReader)
        {
            VoucherReference oVoucherReference = new VoucherReference();
            oVoucherReference.VoucherReferenceID = oReader.GetInt32("VoucherReferenceID");
            oVoucherReference.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            //oVoucherReference.CostCenterName = oReader.GetString("CostCenterName");
            oVoucherReference.Amount = oReader.GetDouble("Amount");
            oVoucherReference.Description = oReader.GetString("Description");
            oVoucherReference.TransactionDate = oReader.GetDateTime("TransactionDate");
            oVoucherReference.CurrencyID = oReader.GetInt32("CurrencyID");
            oVoucherReference.CurrencyConversionRate = oReader.GetDouble("CurrencyConversionRate");
            oVoucherReference.CurrencySymbol = oReader.GetString("CurrencySymbol");
            return oVoucherReference;
        }

        private VoucherReference CreateObject(NullHandler oReader)
        {
            VoucherReference oVoucherReference = new VoucherReference();
            oVoucherReference = MapObject(oReader);
            return oVoucherReference;
        }

        private List<VoucherReference> CreateObjects(IDataReader oReader)
        {
            List<VoucherReference> oVoucherReference = new List<VoucherReference>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherReference oItem = CreateObject(oHandler);
                oVoucherReference.Add(oItem);
            }
            return oVoucherReference;
        }

        #endregion

        #region Interface implementation
        public VoucherReferenceService() { }

        public VoucherReference Save(VoucherReference oVoucherReference, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVoucherReference.VoucherReferenceID <= 0)
                {
                    reader = VoucherReferenceDA.InsertUpdate(tc, oVoucherReference, EnumDBOperation.Insert);
                }
                else
                {
                    reader = VoucherReferenceDA.InsertUpdate(tc, oVoucherReference, EnumDBOperation.Update);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherReference = new VoucherReference();
                    oVoucherReference = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VoucherReference. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherReference;
        }
        public bool Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherReference oVoucherReference = new VoucherReference();
                oVoucherReference.VoucherReferenceID = id;
                VoucherReferenceDA.Delete(tc, oVoucherReference, EnumDBOperation.Delete);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete VoucherReference. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public VoucherReference Get(int id, int nUserID)
        {
            VoucherReference oAccountHead = new VoucherReference();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherReferenceDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VoucherReference", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<VoucherReference> Gets(int nUserID)
        {
            List<VoucherReference> oVoucherReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherReferenceDA.Gets(tc);
                oVoucherReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherReference", e);
                #endregion
            }

            return oVoucherReference;
        }

        public List<VoucherReference> Gets(int nVoucherDetailID, int nUserID)
        {
            List<VoucherReference> oVoucherReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherReferenceDA.Gets(tc, nVoucherDetailID);
                oVoucherReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherReference", e);
                #endregion
            }

            return oVoucherReference;
        }
        public List<VoucherReference> GetsBy(int nVoucherlID, int nUserID)
        {
            List<VoucherReference> oVoucherReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherReferenceDA.GetsBy(tc, nVoucherlID);
                oVoucherReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherReference", e);
                #endregion
            }

            return oVoucherReference;
        }
        public List<VoucherReference> Gets(string sSQL, int nUserId)
        {
            List<VoucherReference> oVoucherReference  = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherReferenceDA.Gets(tc, sSQL);
                oVoucherReference  = CreateObjects(reader);
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

            return oVoucherReference ;
        }

        #endregion
    }   


}
