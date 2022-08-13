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
    public class ImportOutStandingDetailService : MarshalByRefObject, IImportOutStandingDetailService
    {
        #region Private functions and declaration
        private ImportOutStandingDetail MapObject(NullHandler oReader)
        {
            ImportOutStandingDetail oImportOutStandingDetail = new ImportOutStandingDetail();   
            oImportOutStandingDetail.LCID = oReader.GetInt32("LCID");
            oImportOutStandingDetail.InvoiceID = oReader.GetInt32("InvoiceID");
            oImportOutStandingDetail.InvoiceType = oReader.GetInt32("InvoiceType");
            oImportOutStandingDetail.ProductID = oReader.GetInt32("ProductID");       
            oImportOutStandingDetail.LCNo = oReader.GetString("LCNo");
            oImportOutStandingDetail.InvoiceNo = oReader.GetString("InvoiceNo");
            oImportOutStandingDetail.ProductName = oReader.GetString("ProductName");
            oImportOutStandingDetail.MUnit = oReader.GetString("MUnit");
            oImportOutStandingDetail.ProductCode = oReader.GetString("ProductCode");
            oImportOutStandingDetail.ContractorID = oReader.GetInt32("ContractorID");
            oImportOutStandingDetail.ContractorName = oReader.GetString("ContractorName");
            oImportOutStandingDetail.Qty = oReader.GetDouble("Qty");
            oImportOutStandingDetail.UnitPrice = oReader.GetDouble("UnitPrice");          
            oImportOutStandingDetail.Qty_Invoice =oReader.GetDouble("Qty_Invoice");
            oImportOutStandingDetail.Qty_PI = oReader.GetDouble("Qty_PI");
            oImportOutStandingDetail.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportOutStandingDetail.ImportPIDetailID = oReader.GetInt32("ImportPIDetailID"); 
            oImportOutStandingDetail.PINo = oReader.GetString("PINo");
            oImportOutStandingDetail.LotNo = oReader.GetString("LotNo");
            oImportOutStandingDetail.WUName = oReader.GetString("WUName");
            oImportOutStandingDetail.ParentType = oReader.GetInt32("ParentType");         
            oImportOutStandingDetail.ParentID = oReader.GetInt32("ParentID");
            oImportOutStandingDetail.WorkingUnitID = oReader.GetInt32("WorkingUnitID"); 
            oImportOutStandingDetail.Qty_TR = Math.Round(oReader.GetDouble("Qty_TR"), 2);
            oImportOutStandingDetail.GRNID = oReader.GetInt32("GRNID");
            oImportOutStandingDetail.GRNDetailID = oReader.GetInt32("GRNDetailID");
            oImportOutStandingDetail.GRNNo = oReader.GetString("GRNNo");
            oImportOutStandingDetail.AgentName = oReader.GetString("AgentName");
            oImportOutStandingDetail.Qty_StockIn = oReader.GetDouble("Qty_StockIn");
            oImportOutStandingDetail.Qty_Short = oReader.GetDouble("Qty_Short");
            oImportOutStandingDetail.ImportInvoiceDetailID = oReader.GetInt32("ImportInvoiceDetailID");
            oImportOutStandingDetail.BLNo = oReader.GetString("BLNo");
            oImportOutStandingDetail.BLDate = oReader.GetDateTime("BLDate");
            oImportOutStandingDetail.FileNo = oReader.GetString("FileNo");
            oImportOutStandingDetail.DocNo = oReader.GetString("DocNo");
            oImportOutStandingDetail.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oImportOutStandingDetail.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oImportOutStandingDetail.ExpireDate = oReader.GetDateTime("ExpireDate");
            oImportOutStandingDetail.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oImportOutStandingDetail.CnFSendDate = oReader.GetDateTime("CnFSendDate");
            oImportOutStandingDetail.InvoiceStatus = (EnumInvoiceEvent)oReader.GetInt32("InvoiceStatus");
            return oImportOutStandingDetail;
        }
        private ImportOutStandingDetail CreateObject(NullHandler oReader)
        {
            ImportOutStandingDetail oImportOutStandingDetail = new ImportOutStandingDetail();
            oImportOutStandingDetail = MapObject(oReader);
            return oImportOutStandingDetail;
        }
        private List<ImportOutStandingDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportOutStandingDetail> oImportOutStandingDetails = new List<ImportOutStandingDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportOutStandingDetail oItem = CreateObject(oHandler);
                oImportOutStandingDetails.Add(oItem);
            }
            return oImportOutStandingDetails;
        }
        #endregion

        #region Interface implementation
        public ImportOutStandingDetailService() { }

        public List<ImportOutStandingDetail> Gets(int nBUID, int nLCPaymentType, int nBankBranchID, int nCurrencyID, DateTime stratDate, DateTime endDate,int nSPRptType,int nDate, long nUserID)
        {
            List<ImportOutStandingDetail> oImportOutStandingDetails = new List<ImportOutStandingDetail>(); 
            ImportOutStandingDetail oImportOutStandingDetail= new ImportOutStandingDetail();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportOutStandingDetailDA.Gets(tc, nBUID, nLCPaymentType, nBankBranchID, nCurrencyID, stratDate, endDate, nSPRptType, nDate);
                oImportOutStandingDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oImportOutStandingDetail.ErrorMessage = ex.Message;
                oImportOutStandingDetails = new List<ImportOutStandingDetail>();
                oImportOutStandingDetails.Add(oImportOutStandingDetail);
                #endregion
            }
            return oImportOutStandingDetails;
        }
       

        #endregion
    }
}
