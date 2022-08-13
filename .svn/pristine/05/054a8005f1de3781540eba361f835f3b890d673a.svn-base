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
    public class ImportInvoiceDetailService : MarshalByRefObject, IImportInvoiceDetailService
    {
        #region Private functions and declaration
        private ImportInvoiceDetail MapObject(NullHandler oReader)
        {
            ImportInvoiceDetail oImportInvoiceDetail = new ImportInvoiceDetail();
            oImportInvoiceDetail.ImportInvoiceDetailID= oReader.GetInt32("ImportInvoiceDetailID");
            oImportInvoiceDetail.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportInvoiceDetail.ProductID = oReader.GetInt32("ProductID");
            oImportInvoiceDetail.ImportPIDetailID = oReader.GetInt32("ImportPIDetailID");
            oImportInvoiceDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportInvoiceDetail.Qty = oReader.GetDouble("Qty");
            oImportInvoiceDetail.ReceiveQty = oReader.GetDouble("ReceiveQty");
            oImportInvoiceDetail.MUnitID = oReader.GetInt32("MUnitID");
            oImportInvoiceDetail.ProductCode = oReader.GetString("ProductCode");
            oImportInvoiceDetail.ProductName = oReader.GetString("ProductName");
            oImportInvoiceDetail.IsSerialNoApply = oReader.GetBoolean("IsSerialNoApply");
            oImportInvoiceDetail.MUSymbol = oReader.GetString("MUnit");
            oImportInvoiceDetail.MUName = oReader.GetString("MUnit");
            oImportInvoiceDetail.Currency = oReader.GetString("Currency");
            oImportInvoiceDetail.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oImportInvoiceDetail.LCLandingCost = oReader.GetDouble("LCLandingCost");
            oImportInvoiceDetail.Amount = oReader.GetDouble("Amount");
            oImportInvoiceDetail.RateUnit = oReader.GetInt32("RateUnit");
            oImportInvoiceDetail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oImportInvoiceDetail.StyleNo = oReader.GetString("StyleNo");
            return oImportInvoiceDetail;
        }

        private ImportInvoiceDetail CreateObject(NullHandler oReader)
        {
            ImportInvoiceDetail oImportInvoiceDetail = new ImportInvoiceDetail();
            oImportInvoiceDetail=MapObject(oReader);
            return oImportInvoiceDetail;
        }

        private List<ImportInvoiceDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportInvoiceDetail oItem = CreateObject(oHandler);
                oImportInvoiceDetails.Add(oItem);
            }
            return oImportInvoiceDetails;
        }
        #endregion

        #region Interface implementation
        public ImportInvoiceDetailService() { }

        #region New Version
        public ImportInvoiceDetail Get(int nImportInvoiceDetailID, Int64 nUserId)
        {
            ImportInvoiceDetail oImportInvoiceDetail = new ImportInvoiceDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvoiceDetailDA.Get(tc, nImportInvoiceDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoiceDetail = CreateObject(oReader);
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

            return oImportInvoiceDetail;
        }

        public List<ImportInvoiceDetail> Gets(int nImportInvoiceID, Int64 nUserId)
        {
            List<ImportInvoiceDetail> oImportInvoiceDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceDetailDA.Gets(nImportInvoiceID, tc);
                oImportInvoiceDetails = CreateObjects(reader);
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
            return oImportInvoiceDetails;
        }

        public List<ImportInvoiceDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportInvoiceDetail> oImportInvoiceDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceDetailDA.Gets(tc, sSQL);
                oImportInvoiceDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvoiceDetail", e);
                #endregion
            }

            return oImportInvoiceDetail;
        }

   

        #endregion

        public string Delete(ImportInvoiceDetail oImportInvoiceDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportInvoiceDetailDA.Delete(tc, oImportInvoiceDetail, EnumDBOperation.Delete, nUserID,"");
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
        public ImportInvoiceDetail Save(ImportInvoiceDetail oImportInvoiceDetail, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oImportInvoiceDetail.ImportInvoiceDetailID <= 0)
                {

                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseRequisition", EnumRoleOperationType.Add);
                    reader = ImportInvoiceDetailDA.InsertUpdate(tc, oImportInvoiceDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseRequisition", EnumRoleOperationType.Edit);
                    reader = ImportInvoiceDetailDA.InsertUpdate(tc, oImportInvoiceDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoiceDetail = new ImportInvoiceDetail();
                    oImportInvoiceDetail = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportInvoiceDetail = new ImportInvoiceDetail();
                oImportInvoiceDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportInvoiceDetail;
        }

        #endregion
    }
}
