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
    public class ImportPIDetailService : MarshalByRefObject, IImportPIDetailService
    {
        #region Private functions and declaration
        private ImportPIDetail MapObject(NullHandler oReader)
        {
            ImportPIDetail oImportPIDetail = new ImportPIDetail();
            oImportPIDetail.ImportPIDetailID = oReader.GetInt32("ImportPIDetailID");
            oImportPIDetail.ImportPIDetailLogID = oReader.GetInt32("ImportPIDetailLogID");
            oImportPIDetail.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportPIDetail.ImportPILogID = oReader.GetInt32("ImportPILogID");
            oImportPIDetail.ProductID = oReader.GetInt32("ProductID");
            oImportPIDetail.MUnitID = oReader.GetInt32("MUnitID");
            oImportPIDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportPIDetail.FreightRate = oReader.GetDouble("FreightRate");
            oImportPIDetail.Qty = oReader.GetDouble("Qty");
            oImportPIDetail.Note = oReader.GetString("Note");
            oImportPIDetail.ProductCode = oReader.GetString("ProductCode");
            oImportPIDetail.ProductName = oReader.GetString("ProductName");
            oImportPIDetail.MUName = oReader.GetString("MUName");
            oImportPIDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oImportPIDetail.SmallUnitValue = oReader.GetDouble("SmallUnitValue");
            oImportPIDetail.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oImportPIDetail.Amount = oReader.GetDouble("Amount");
            oImportPIDetail.ProductUnitType = (EnumUniteType)oReader.GetInt32("ProductUnitType");
            oImportPIDetail.ProductUnitTypeInInt = oReader.GetInt32("ProductUnitType");
            oImportPIDetail.RateUnit = oReader.GetInt32("RateUnit");
            oImportPIDetail.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oImportPIDetail.CRate = oReader.GetDouble("CRate");
            oImportPIDetail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oImportPIDetail.StyleNo = oReader.GetString("StyleNo");
            oImportPIDetail.BuyerName = oReader.GetString("BuyerName");
            oImportPIDetail.Shade = oReader.GetString("Shade");
            
            return oImportPIDetail;
        }

        private ImportPIDetail CreateObject(NullHandler oReader)
        {
            ImportPIDetail oPCD = new ImportPIDetail();
            oPCD=MapObject( oReader);
            return oPCD;
        }

        private List<ImportPIDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportPIDetail> oPCDs = new List<ImportPIDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPIDetail oItem = CreateObject(oHandler);
                oPCDs.Add(oItem);
            }
            return oPCDs;
        }
        #endregion

        #region Interface implementation

        public ImportPIDetail Get(int nImportInvoiceDetailID, Int64 nUserId)
        {
            ImportPIDetail oPCD = new ImportPIDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPIDetailDA.Get(tc, nImportInvoiceDetailID);
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
                throw new ServiceException("Failed to Get ImportPIDetail", e);
                #endregion
            }

            return oPCD;
        }

        public List<ImportPIDetail> Gets(Int64 nUserId)
        {
            List<ImportPIDetail> oPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDetailDA.Gets(tc);
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
        public List<ImportPIDetail> Gets(int nImportPIID, Int64 nUserId)
        {
            List<ImportPIDetail> oPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDetailDA.Gets(tc, nImportPIID);
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

       
        public List<ImportPIDetail> GetsByImportPIID(int nImportPIId, Int64 nUserId)
        {
            List<ImportPIDetail> oImportPIDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDetailDA.GetsByImportPIID(tc, nImportPIId);
                oImportPIDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIDetail", e);
                #endregion
            }

            return oImportPIDetail;
        }

        public List<ImportPIDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportPIDetail> oImportPIDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDetailDA.Gets(tc, sSQL);
                oImportPIDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIDetail", e);
                #endregion
            }

            return oImportPIDetail;
        }
        public String Delete(ImportPIDetail oImportPIDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportPIDetailDA.Delete(tc, oImportPIDetail, EnumDBOperation.Delete, nUserID,"");
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
