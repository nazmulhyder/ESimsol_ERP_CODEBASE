using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{
    [Serializable]
    public class ImportPackDetailService : MarshalByRefObject, IImportPackDetailService
    {
        #region Private functions and declaration
        private ImportPackDetail MapObject(NullHandler oReader)
        {
            ImportPackDetail oImportPackDetail = new ImportPackDetail();
            oImportPackDetail.ImportPackDetailID= oReader.GetInt32("ImportPackDetailID");
            oImportPackDetail.ImportPackID = oReader.GetInt32("ImportPackID");
            oImportPackDetail.ProductID = oReader.GetInt32("ProductID");
            oImportPackDetail.ImportInvoiceDetailID = oReader.GetInt32("ImportInvoiceDetailID");
            oImportPackDetail.NumberOfPack = oReader.GetDouble("NumberOfPack");
            oImportPackDetail.QtyPerPack = oReader.GetDouble("QtyPerPack");
            oImportPackDetail.Qty = oReader.GetDouble("Qty");
            oImportPackDetail.MUnitID = oReader.GetInt32("MUnitID");
            oImportPackDetail.ProductCode = oReader.GetString("ProductCode");
            oImportPackDetail.ProductName = oReader.GetString("ProductName");
            //oImportPackDetail.MUSymbol = oReader.GetString("MUnitSymbol");
            oImportPackDetail.MUName = oReader.GetString("MUName");
            oImportPackDetail.LotNo = oReader.GetString("LotNo");
            oImportPackDetail.Remarks = oReader.GetString("Remarks");
            oImportPackDetail.IsSerialNoApply = oReader.GetBoolean("IsSerialNoApply");
            oImportPackDetail.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oImportPackDetail.LCLandingCost= oReader.GetDouble("LCLandingCost");
            oImportPackDetail.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportPackDetail.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oImportPackDetail.Brand = oReader.GetString("Brand");
            oImportPackDetail.Origin = oReader.GetString("Origin");
            oImportPackDetail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oImportPackDetail.StyleNo = oReader.GetString("StyleNo");
           // oImportPackDetail.Shade = oReader.GetString("Shade");
            oImportPackDetail.MURate = oReader.GetDouble("MURate");
            
            return oImportPackDetail;
        }

        private ImportPackDetail CreateObject(NullHandler oReader)
        {
            ImportPackDetail oImportPackDetail = new ImportPackDetail();
            oImportPackDetail=MapObject(oReader);
            return oImportPackDetail;
        }

        private List<ImportPackDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportPackDetail> oImportPackDetails = new List<ImportPackDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPackDetail oItem = CreateObject(oHandler);
                oImportPackDetails.Add(oItem);
            }
            return oImportPackDetails;
        }
        #endregion

        #region Interface implementation
        public ImportPackDetailService() { }

        #region New Version
        public ImportPackDetail Get(int nImportPackDetailID, Int64 nUserId)
        {
            ImportPackDetail oImportPackDetail = new ImportPackDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPackDetailDA.Get(tc, nImportPackDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPackDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Import Invoice Product", e);
                #endregion
            }

            return oImportPackDetail;
        }
        public string Delete(ImportPackDetail oImportPackDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportPackDetailDA.Delete(tc, oImportPackDetail, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public List<ImportPackDetail> Gets(int nImportPackID, Int64 nUserId)
        {
            List<ImportPackDetail> oImportPackDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPackDetailDA.Gets(nImportPackID, tc);
                oImportPackDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Import Invoice Products", e);
                #endregion
            }
            return oImportPackDetails;
        }
        public List<ImportPackDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportPackDetail> oImportPackDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPackDetailDA.Gets(tc, sSQL);
                oImportPackDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPackDetail", e);
                #endregion
            }

            return oImportPackDetail;
        }

        public List<ImportPackDetail> GetsByInvioce(int nImportInvoiceID, Int64 nUserId)
        {
            List<ImportPackDetail> oImportPackDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPackDetailDA.GetsByInvioce(nImportInvoiceID, tc);
                oImportPackDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Import Invoice Products", e);
                #endregion
            }
            return oImportPackDetails;
        }

        #endregion

      
    

        #endregion
    }
}
