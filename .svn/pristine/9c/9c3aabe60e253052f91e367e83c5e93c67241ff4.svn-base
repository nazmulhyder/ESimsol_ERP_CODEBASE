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
    [Serializable]
    public class ImportPIReferenceService : MarshalByRefObject, IImportPIReferenceService
    {
        #region Private functions and declaration
        private ImportPIReference MapObject(NullHandler oReader)
        {
            ImportPIReference oImportPIReference = new ImportPIReference();
            oImportPIReference.ImportPIReferenceID = oReader.GetInt32("ImportPIReferenceID");
            oImportPIReference.ImportPIReferenceLogID = oReader.GetInt32("ImportPIReferenceLogID");
            oImportPIReference.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportPIReference.ImportPILogID = oReader.GetInt32("ImportPILogID");
            oImportPIReference.ReferenceID = oReader.GetInt32("ReferenceID");
            oImportPIReference.BUID = oReader.GetInt32("BUID");
            oImportPIReference.ImportPINo = oReader.GetString("ImportPINo");
            oImportPIReference.ReferenceNo = oReader.GetString("ReferenceNo");
            oImportPIReference.BillNo = oReader.GetString("BillNo");
            oImportPIReference.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oImportPIReference.Amount = oReader.GetDouble("Amount");
            oImportPIReference.YetToReferenceAmount = oReader.GetDouble("YetToReferenceAmount");
            oImportPIReference.ReferenceAmount = oReader.GetDouble("ReferenceAmount");
            oImportPIReference.ConvertionRate = oReader.GetDouble("ConvertionRate");
            oImportPIReference.AmountInBaseCurrency = oReader.GetDouble("AmountInBaseCurrency");
            oImportPIReference.CurrencyID = oReader.GetInt32("CurrencyID");
            
            return oImportPIReference;
        }

        private ImportPIReference CreateObject(NullHandler oReader)
        {
            ImportPIReference oPCD = new ImportPIReference();
            oPCD=MapObject( oReader);
            return oPCD;
        }

        private List<ImportPIReference> CreateObjects(IDataReader oReader)
        {
            List<ImportPIReference> oPCDs = new List<ImportPIReference>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPIReference oItem = CreateObject(oHandler);
                oPCDs.Add(oItem);
            }
            return oPCDs;
        }
        #endregion

        #region Interface implementation

        public ImportPIReference Get(int nImportInvoiceDetailID, Int64 nUserId)
        {
            ImportPIReference oPCD = new ImportPIReference();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPIReferenceDA.Get(tc, nImportInvoiceDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPCD = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPIReference", e);
                #endregion
            }

            return oPCD;
        }

        public List<ImportPIReference> Gets(Int64 nUserId)
        {
            List<ImportPIReference> oPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIReferenceDA.Gets(tc);
                oPCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoices", e);
                #endregion
            }
            return oPCDs;
        }
        public List<ImportPIReference> Gets(int nImportPIID, Int64 nUserId)
        {
            List<ImportPIReference> oPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIReferenceDA.Gets(tc, nImportPIID);
                oPCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Proforma Invoices", e);
                #endregion
            }
            return oPCDs;
        }

       
        public List<ImportPIReference> GetsByImportPIID(int nImportPIId, Int64 nUserId)
        {
            List<ImportPIReference> oImportPIReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIReferenceDA.GetsByImportPIID(tc, nImportPIId);
                oImportPIReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIReference", e);
                #endregion
            }

            return oImportPIReference;
        }

        public List<ImportPIReference> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportPIReference> oImportPIReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIReferenceDA.Gets(tc, sSQL);
                oImportPIReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIReference", e);
                #endregion
            }

            return oImportPIReference;
        }
        public String Delete(ImportPIReference oImportPIReference, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportPIReferenceDA.Delete(tc, oImportPIReference, EnumDBOperation.Delete, nUserID,"");
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }
        #endregion
    }
}
