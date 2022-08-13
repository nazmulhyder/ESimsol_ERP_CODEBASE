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
    public class ImportInvChallanDetailService : MarshalByRefObject, IImportInvChallanDetailService
    {
        #region Private functions and declaration
        private ImportInvChallanDetail MapObject(NullHandler oReader)
        {
            ImportInvChallanDetail oImportInvChallanDetail = new ImportInvChallanDetail();
            oImportInvChallanDetail.ImportInvChallanDetailID= oReader.GetInt32("ImportInvChallanDetailID");
            oImportInvChallanDetail.ImportInvChallanID = oReader.GetInt32("ImportInvChallanID");
            oImportInvChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oImportInvChallanDetail.ImportInvoiceDetailID = oReader.GetInt32("ImportInvoiceDetailID");
           
            oImportInvChallanDetail.Qty = oReader.GetDouble("Qty");
            oImportInvChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oImportInvChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oImportInvChallanDetail.ProductName = oReader.GetString("ProductName");
            oImportInvChallanDetail.MUSymbol = oReader.GetString("MUnitSymbol");
            oImportInvChallanDetail.MUName = oReader.GetString("MUName");
            oImportInvChallanDetail.LotNo = oReader.GetString("LotNo");
            oImportInvChallanDetail.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportInvChallanDetail.Note = oReader.GetString("Remarks");
            oImportInvChallanDetail.Qty_GRN = oReader.GetDouble("Qty_GRN");
            oImportInvChallanDetail.Qty_IPL = oReader.GetDouble("Qty_IPL");
            oImportInvChallanDetail.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportInvChallanDetail.ImportPackDetailID = oReader.GetInt32("ImportPackDetailID");
            oImportInvChallanDetail.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oImportInvChallanDetail.NumberOfPack = oReader.GetDouble("NumberOfPack");
            oImportInvChallanDetail.QtyPerPack = oReader.GetDouble("QtyPerPack");
            //oImportInvChallanDetail.QtyPerPack_CD = oReader.GetDouble("QtyPerPack_CD");
            oImportInvChallanDetail.NumberOfPack_CD = oReader.GetDouble("NumberOfPack_CD");
            
            return oImportInvChallanDetail;
        }

        private ImportInvChallanDetail CreateObject(NullHandler oReader)
        {
            ImportInvChallanDetail oImportInvChallanDetail = new ImportInvChallanDetail();
            oImportInvChallanDetail=MapObject(oReader);
            return oImportInvChallanDetail;
        }

        private List<ImportInvChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportInvChallanDetail> oImportInvChallanDetails = new List<ImportInvChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportInvChallanDetail oItem = CreateObject(oHandler);
                oImportInvChallanDetails.Add(oItem);
            }
            return oImportInvChallanDetails;
        }
        #endregion

        #region Interface implementation
        public ImportInvChallanDetailService() { }

        #region New Version
        public ImportInvChallanDetail Get(int nImportInvChallanDetailID, Int64 nUserId)
        {
            ImportInvChallanDetail oImportInvChallanDetail = new ImportInvChallanDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvChallanDetailDA.Get(tc, nImportInvChallanDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvChallanDetail = CreateObject(oReader);
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

            return oImportInvChallanDetail;
        }

        public List<ImportInvChallanDetail> Gets(int nImportInvChallanID, Int64 nUserId)
        {
            List<ImportInvChallanDetail> oImportInvChallanDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvChallanDetailDA.Gets(nImportInvChallanID, tc);
                oImportInvChallanDetails = CreateObjects(reader);
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
            return oImportInvChallanDetails;
        }

        public List<ImportInvChallanDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportInvChallanDetail> oImportInvChallanDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvChallanDetailDA.Gets(tc, sSQL);
                oImportInvChallanDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvChallanDetail", e);
                #endregion
            }

            return oImportInvChallanDetail;
        }

   

        #endregion

        public string Delete(ImportInvChallanDetail oImportInvChallanDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportInvChallanDetailDA.Delete(tc, oImportInvChallanDetail, EnumDBOperation.Delete, nUserID,"");
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
    

        #endregion
    }
}
