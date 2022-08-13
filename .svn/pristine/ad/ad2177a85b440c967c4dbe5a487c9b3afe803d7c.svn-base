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
    public class ImportLCReportService : MarshalByRefObject, IImportLCReportService
    {
        #region Private functions and declaration
        private ImportLCReport MapObject(NullHandler oReader)
        {
            ImportLCReport oImportLCReport = new ImportLCReport();

            oImportLCReport.PPCID = oReader.GetInt32("PPCID");
            oImportLCReport.ImportLCNO = oReader.GetString("ImportLCNO");
            oImportLCReport.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oImportLCReport.SupplierName = oReader.GetString("SupplierName");
            oImportLCReport.ProductName = oReader.GetString("ProductName");
            oImportLCReport.ProductCode = oReader.GetString("ProductCode");
            oImportLCReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportLCReport.Quantity = oReader.GetDouble("Quantity");
            oImportLCReport.NegotiateBankName = oReader.GetString("NegotiateBankName");
            oImportLCReport.LCPaymentType = (EnumLCPaymentType)oReader.GetInt16("LCPaymentType");
            oImportLCReport.LCAmount = oReader.GetDouble("Amount");
            oImportLCReport.LCCoverNoteNo = oReader.GetString("LCCoverNoteNo");
            oImportLCReport.ExpireDate = oReader.GetDateTime("ExpireDate");
            oImportLCReport.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oImportLCReport.LCCurrentStatus = (EnumLCCurrentStatus)oReader.GetInt16("LCCurrentStatus");
            oImportLCReport.LCANo = oReader.GetString("LCANo");
            oImportLCReport.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oImportLCReport.MUnit = oReader.GetString("MUnit");

            return oImportLCReport;

        }

        private ImportLCReport CreateObject(NullHandler oReader)
        {
            ImportLCReport oImportLCReport = MapObject(oReader);
            return oImportLCReport;
        }

        private List<ImportLCReport> CreateObjects(IDataReader oReader)
        {
            List<ImportLCReport> oImportLCReport = new List<ImportLCReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCReport oItem = CreateObject(oHandler);
                oImportLCReport.Add(oItem);
            }
            return oImportLCReport;
        }

        #endregion

        #region Interface implementation
        public ImportLCReportService() { }


        public List<ImportLCReport> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportLCReport> oImportLCReports = new List<ImportLCReport>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCReportDA.Gets(tc, sSQL);
                oImportLCReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get View_ImportLCReport", e);
                //#endregion
                oImportLCReports = new List<ImportLCReport>();
                ImportLCReport oImportLCReport = new ImportLCReport();
                oImportLCReport.ErrorMessage = e.Message;
                oImportLCReports.Add(oImportLCReport);
            }
            return oImportLCReports;
        }

        #endregion

    }
}
