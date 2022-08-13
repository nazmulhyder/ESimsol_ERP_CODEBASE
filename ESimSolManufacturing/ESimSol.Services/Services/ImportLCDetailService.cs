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
    public class ImportLCDetailService : MarshalByRefObject, IImportLCDetailService
    {
        #region Private functions and declaration
        private ImportLCDetail MapObject( NullHandler oReader)
        {
            ImportLCDetail oImportLCDetail = new ImportLCDetail();
            oImportLCDetail.ImportLCDetailID = oReader.GetInt32("ImportLCDetailID");
            oImportLCDetail.ImportLCDetailLogID = oReader.GetInt32("ImportLCDetailLogID");
            oImportLCDetail.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportLCDetail.ImportPIType = (EnumImportPIType)oReader.GetInt16("ImportPIType");
            oImportLCDetail.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportLCDetail.Amount = oReader.GetDouble("Amount");
            oImportLCDetail.Qty = oReader.GetDouble("Qty");
            oImportLCDetail.ImportPINo = oReader.GetString("ImportPINo");
            oImportLCDetail.SupplierID = oReader.GetInt32("SupplierID");
            oImportLCDetail.IssueDate = oReader.GetDateTime("IssueDate");
            oImportLCDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportLCDetail.BankName = oReader.GetString("BankName");
            oImportLCDetail.ProductTypeName = oReader.GetString("ProductTypeName");
            oImportLCDetail.AskingDeliveryDate = oReader.GetDateTime("AskingDeliveryDate");
            oImportLCDetail.IsTransShipmentAllow = oReader.GetBoolean("IsTransShipmentAllow");
            oImportLCDetail.IsPartShipmentAllow = oReader.GetBoolean("IsPartShipmentAllow");
            oImportLCDetail.DateOfApproved = oReader.GetDateTime("DateOfApproved");
            oImportLCDetail.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oImportLCDetail.SupplierName = oReader.GetString("SupplierName");
            oImportLCDetail.Currency = oReader.GetString("Currency");
            oImportLCDetail.LCTermsName = oReader.GetString("LCTermsName");
            oImportLCDetail.PaymentInstruction = (EnumPaymentInstruction)oReader.GetInt16("PaymentInstructionType");
            oImportLCDetail.ProductType = (EnumProductNature)oReader.GetInt32("ProductType");
            return oImportLCDetail;
        }

        private ImportLCDetail CreateObject(NullHandler oReader)
        {
            ImportLCDetail oPPC = new ImportLCDetail();
            oPPC=MapObject(oReader);
            return oPPC;
        }

        private List<ImportLCDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportLCDetail> oPPCs = new List<ImportLCDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCDetail oItem = CreateObject(oHandler);
                oPPCs.Add(oItem);
            }
            return oPPCs;
        }
        #endregion

        #region Interface implementation
        public ImportLCDetailService() { }
        public ImportLCDetail GetByPurhcaseLCID(int PurhcaseLCID, Int64 nUserId)
        {
            ImportLCDetail oPPC = new ImportLCDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLCDetailDA.GetByPurhcaseLCID(tc, PurhcaseLCID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPPC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchasePaymentContract", e);
                #endregion
            }

            return oPPC;
        }


        public ImportLCDetail Get(int id, Int64 nUserId)
        {
            ImportLCDetail oPPC = new ImportLCDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLCDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPPC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchasePaymentContract", e);
                #endregion
            }

            return oPPC;
        }
        public List<ImportLCDetail> Gets(int nImportLCID, Int64 nUserId)
        {
            List<ImportLCDetail> oPPCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDetailDA.Gets(tc, nImportLCID);
                oPPCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchasePaymentContracts", e);
                #endregion
            }

            return oPPCs;
        }
        public List<ImportLCDetail> GetsLog(int nImportLCLogID, Int64 nUserId)
        {
            List<ImportLCDetail> oPPCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDetailDA.GetsLog(tc, nImportLCLogID);
                oPPCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchasePaymentContracts", e);
                #endregion
            }

            return oPPCs;
        }
        public List<ImportLCDetail> GetsByInvoice(int nInvoiceID, Int64 nUserId)
        {
            List<ImportLCDetail> oPPCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDetailDA.GetsByInvoice(tc, nInvoiceID);
                oPPCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchasePaymentContracts", e);
                #endregion
            }

            return oPPCs;
        }

        public String Delete(ImportLCDetail oPurchasePaymentContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportLCDetailDA.Delete(tc, oPurchasePaymentContract, EnumDBOperation.Delete, nUserID,"");
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
