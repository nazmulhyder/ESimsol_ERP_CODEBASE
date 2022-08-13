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
    public class PurchaseInvoiceLCReportService : MarshalByRefObject, IPurchaseInvoiceLCReportService
    {
        #region Private functions and declaration
        private PurchaseInvoiceLCReport MapObject(NullHandler oReader)
        {
            PurchaseInvoiceLCReport oPurchaseInvoiceLCReport = new PurchaseInvoiceLCReport();
            oPurchaseInvoiceLCReport.PurchaseInvoiceID = oReader.GetInt32("PurchaseInvoiceLCID");
            oPurchaseInvoiceLCReport.PurchaseInvoiceNo = oReader.GetString("PurchaseInvoiceLCNo");
            oPurchaseInvoiceLCReport.SupplierID = oReader.GetInt32("SupplierID");
            oPurchaseInvoiceLCReport.SupplierName = oReader.GetString("SupplierName");
            oPurchaseInvoiceLCReport.InvoiceType = (EnumImportPIType)oReader.GetInt16("InvoiceType");
            oPurchaseInvoiceLCReport.CurrencyName = oReader.GetString("CurrencyName");
            oPurchaseInvoiceLCReport.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPurchaseInvoiceLCReport.MUnit = oReader.GetString("MUnit");
            oPurchaseInvoiceLCReport.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oPurchaseInvoiceLCReport.DateofPayment = oReader.GetDateTime("DateofPayment");
            oPurchaseInvoiceLCReport.CurrentStatus = (EnumPurchaseInvoiceEvent)oReader.GetInt16("CurrentStatus");            
            oPurchaseInvoiceLCReport.ProductID = oReader.GetInt32("ProductID");
            oPurchaseInvoiceLCReport.ProductName = oReader.GetString("ProductName");
            oPurchaseInvoiceLCReport.ProductCode = oReader.GetString("ProductCode");
            oPurchaseInvoiceLCReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oPurchaseInvoiceLCReport.Quantity = oReader.GetDouble("Quantity");
            oPurchaseInvoiceLCReport.ReceiveQty = oReader.GetDouble("ReceiveQty");
            oPurchaseInvoiceLCReport.LCNO = oReader.GetString("LCNO");
            oPurchaseInvoiceLCReport.DateOfLC = oReader.GetDateTime("DateOfLC");
            oPurchaseInvoiceLCReport.NegotiateBankBranchID = oReader.GetInt32("NegotiateBankBranchID");
            oPurchaseInvoiceLCReport.BankName = oReader.GetString("BankName");            
            oPurchaseInvoiceLCReport.LiabilityNo = oReader.GetString("LiabilityNo");
            oPurchaseInvoiceLCReport.DateOfStockIn = oReader.GetDateTime("DateOfStockIn");
            

            return oPurchaseInvoiceLCReport;

        }

        private PurchaseInvoiceLCReport CreateObject(NullHandler oReader)
        {
            PurchaseInvoiceLCReport oPurchaseInvoiceLCReport = MapObject(oReader);
            return oPurchaseInvoiceLCReport;
        }

        private List<PurchaseInvoiceLCReport> CreateObjects(IDataReader oReader)
        {
            List<PurchaseInvoiceLCReport> oPurchaseInvoiceLCReport = new List<PurchaseInvoiceLCReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseInvoiceLCReport oItem = CreateObject(oHandler);
                oPurchaseInvoiceLCReport.Add(oItem);
            }
            return oPurchaseInvoiceLCReport;
        }

        #endregion

        #region Interface implementation
        public PurchaseInvoiceLCReportService() { }


        public List<PurchaseInvoiceLCReport> Gets(string sSQL, Int64 nUserId)
        {
            List<PurchaseInvoiceLCReport> oPurchaseInvoiceLCReports = new List<PurchaseInvoiceLCReport>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                //reader = PurchaseInvoiceLCReportDA.Gets(tc, sSQL);
                oPurchaseInvoiceLCReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get View_PurchaseInvoiceLCReport", e);
                //#endregion
                oPurchaseInvoiceLCReports = new List<PurchaseInvoiceLCReport>();
                PurchaseInvoiceLCReport oPurchaseInvoiceLCReport = new PurchaseInvoiceLCReport();
                oPurchaseInvoiceLCReport.ErrorMessage = e.Message;
                oPurchaseInvoiceLCReports.Add(oPurchaseInvoiceLCReport);
            }
            return oPurchaseInvoiceLCReports;
        }

        #endregion

    }
}
