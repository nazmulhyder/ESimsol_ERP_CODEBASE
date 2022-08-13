using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class ExportPIReportService : MarshalByRefObject, IExportPIReportService
    {
        #region Private functions and declaration
        private ExportPIReport MapObject(NullHandler oReader)
        {
            ExportPIReport oPIReport = new ExportPIReport();

            oPIReport.ExportPIID = oReader.GetInt32("ExportPIID");
            //oPIReport.PICode = oReader.GetString("PICode");
            oPIReport.PINo = oReader.GetString("PINo");
            //oPIReport.PIYear = oReader.GetString("PIYear");
            oPIReport.IssueDate = oReader.GetDateTime("IssueDate");
            oPIReport.ContractorID = oReader.GetInt32("ContractorID");
            oPIReport.ContractorName = oReader.GetString("ContractorName");
            oPIReport.BuyerID = oReader.GetInt32("BuyerID");
            oPIReport.RateUnit = oReader.GetInt32("RateUnit");
            oPIReport.BuyerName = oReader.GetString("BuyerName");
            oPIReport.MKTPName = oReader.GetString("MKTPName");
            oPIReport.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oPIReport.LCNo = oReader.GetString("ExportLCNo");
            oPIReport.Currency = oReader.GetString("Currency");
            oPIReport.ProductName = oReader.GetString("ProductName");
            oPIReport.BankName = oReader.GetString("BankName");
            oPIReport.Qty = oReader.GetDouble("Qty");
            oPIReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oPIReport.AdjQty = oReader.GetDouble("AdjQty");
            oPIReport.AdjRate = oReader.GetDouble("AdjRate");
            oPIReport.DocCharge = oReader.GetDouble("DocCharge");
            oPIReport.AdjValue = oReader.GetDouble("AdjValue");
            oPIReport.CRate = oReader.GetDouble("CRate");
            oPIReport.CRateTwo = oReader.GetDouble("CRateTwo");
            oPIReport.QtyCom = oReader.GetDouble("QtyCom");
            oPIReport.MUName = oReader.GetString("MUName");
            oPIReport.CPersonName = oReader.GetString("CPersonName");
            oPIReport.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
            oPIReport.LCNo = oReader.GetString("ExportLCNo");
            oPIReport.CurrentStatus_LC = oReader.GetInt32("CurrentStatus_LC");
            //oExportPI.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oPIReport.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oPIReport.AmendmentNo = oReader.GetInt32("AmendmentNo");
            oPIReport.AmendmentRequired = oReader.GetBoolean("AmendmentRequired");
            oPIReport.Amount_Accep = oReader.GetDouble("Amount_Accep");
            oPIReport.Amount_Maturity = oReader.GetDouble("Amount_Maturity");
            oPIReport.FabricNo = oReader.GetString("FabricNo");
            oPIReport.FileNo = oReader.GetString("FileNo");
            oPIReport.Construction = oReader.GetString("Construction");
            oPIReport.ColorName = oReader.GetString("ColorName");
            oPIReport.ReferenceCaption = oReader.GetString("ReferenceCaption");
            oPIReport.ProductDescription = oReader.GetString("ProductDescription");
            oPIReport.SizeName = oReader.GetString("SizeName");
            oPIReport.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oPIReport.ModelReferenceName = oReader.GetString("ModelReferenceName");
            //oPIReport.Measurement = oReader.GetString("Measurement");
            oPIReport.IssueDate = oReader.GetDateTime("IssueDate");
            //oPIReport.OrderSheetDetailID = oReader.GetInt32("OrderSheetDetailID");
            oPIReport.IsApplySizer = oReader.GetBoolean("IsApplySizer");
            oPIReport.ColorID = oReader.GetInt32("ColorID");
            oPIReport.BUID = oReader.GetInt32("BUID");
            if (oPIReport.AmendmentNo>0)
            {
                oPIReport.LCNo = oPIReport.LCNo + " A-" + oPIReport.AmendmentNo;
            }

            return oPIReport;
        }

        private ExportPIReport CreateObject(NullHandler oReader)
        {
            ExportPIReport oPIReport = new ExportPIReport();
            oPIReport = MapObject(oReader);
            return oPIReport;
        }

        private List<ExportPIReport> CreateObjects(IDataReader oReader)
        {
            List<ExportPIReport> oPIReport = new List<ExportPIReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPIReport oItem = CreateObject(oHandler);
                oPIReport.Add(oItem);
            }
            return oPIReport;
        }

        #endregion

        #region Interface implementation
        public ExportPIReportService() { }

        public List<ExportPIReport> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportPIReport> oPIReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIReportDA.Gets(tc, sSQL);
                oPIReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oPIReport;
        }
       

        #endregion
    }
}