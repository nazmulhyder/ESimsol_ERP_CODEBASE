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
    public class ImportInvoiceRegisterService : MarshalByRefObject, IImportInvoiceRegisterService
    {
        #region Private functions and declaration
        private ImportInvoiceRegister MapObject(NullHandler oReader)
        {
            ImportInvoiceRegister oImportInvoiceRegister = new ImportInvoiceRegister();
            oImportInvoiceRegister.ImportInvoiceDetailID = oReader.GetInt32("ImportInvoiceDetailID");
            oImportInvoiceRegister.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportInvoiceRegister.ProductID = oReader.GetInt32("ProductID");
            oImportInvoiceRegister.MUnitID = oReader.GetInt32("MUnitID");
            oImportInvoiceRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportInvoiceRegister.Qty = oReader.GetDouble("Qty");
            oImportInvoiceRegister.Amount = oReader.GetDouble("Amount");
            oImportInvoiceRegister.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportInvoiceRegister.BUID = oReader.GetInt32("BUID");
            oImportInvoiceRegister.ABPNo = oReader.GetString("ABPNo");
            oImportInvoiceRegister.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oImportInvoiceRegister.InvoiceStatus = (EnumInvoiceEvent)oReader.GetInt32("InvoiceStatus");
            oImportInvoiceRegister.ContractorID = oReader.GetInt32("ContractorID");
            oImportInvoiceRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportInvoiceRegister.InvoiceAmount = oReader.GetDouble("InvoiceAmount");
            oImportInvoiceRegister.LCTermID = oReader.GetInt32("LCTermID");
            oImportInvoiceRegister.InvoiceType = (EnumImportPIType)oReader.GetInt32("InvoiceType");
            oImportInvoiceRegister.PaymentInstructionType = (EnumPaymentInstruction)oReader.GetInt32("PaymentInstructionType");
            oImportInvoiceRegister.AcceptedBy = oReader.GetInt32("AcceptedBy");
            oImportInvoiceRegister.DateofAcceptance = oReader.GetDateTime("DateofAcceptance");
            oImportInvoiceRegister.VersionNumber = oReader.GetInt32("VersionNumber");
            oImportInvoiceRegister.DateofApplication = oReader.GetDateTime("DateofApplication");
            oImportInvoiceRegister.DateofAcceptance = oReader.GetDateTime("DateofAcceptance");
            oImportInvoiceRegister.DateofBankInfo = oReader.GetDateTime("DateofBankInfo");
            oImportInvoiceRegister.DateofMaturity = oReader.GetDateTime("DateofMaturity");
            oImportInvoiceRegister.DateOfTakeOutDoc = oReader.GetDateTime("DateOfTakeOutDoc");
            oImportInvoiceRegister.DateofNegotiation = oReader.GetDateTime("DateofNegotiation");
            oImportInvoiceRegister.DateofPayment = oReader.GetDateTime("DateofPayment");
            oImportInvoiceRegister.DateofReceive = oReader.GetDateTime("DateofReceive");
            oImportInvoiceRegister.Remarks = oReader.GetString("Remarks");
            oImportInvoiceRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oImportInvoiceRegister.ProductCode = oReader.GetString("ProductCode");
            oImportInvoiceRegister.ProductName = oReader.GetString("ProductName");
            oImportInvoiceRegister.MUName = oReader.GetString("MUName");
            oImportInvoiceRegister.MUSymbol = oReader.GetString("MUSymbol");
            oImportInvoiceRegister.SupplierName = oReader.GetString("SupplierName");
            oImportInvoiceRegister.FileNo = oReader.GetString("FileNo");
            oImportInvoiceRegister.BLNo = oReader.GetString("BLNo");
            oImportInvoiceRegister.BLDate = oReader.GetDateTime("BLDate");
            oImportInvoiceRegister.CurrencyName = oReader.GetString("CurrencyName");
            oImportInvoiceRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oImportInvoiceRegister.CRate_Acceptance = oReader.GetDouble("CRate_Acceptance");
            oImportInvoiceRegister.LCTermName = oReader.GetString("LCTermName");
            oImportInvoiceRegister.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportInvoiceRegister.BankName_Nego = oReader.GetString("BankName_Nego");
            oImportInvoiceRegister.BBRanchName_Nego = oReader.GetString("BBRanchName_Nego");
            oImportInvoiceRegister.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oImportInvoiceRegister.RateUnit = oReader.GetInt32("RateUnit");
            oImportInvoiceRegister.MasterLCNos = oReader.GetString("MasterLCNos");
            oImportInvoiceRegister.BillofEntryDate = oReader.GetDateTime("BillofEntryDate");
            oImportInvoiceRegister.BillofEntryNo = oReader.GetString("BillofEntryNo");
            return oImportInvoiceRegister;
        }

        private ImportInvoiceRegister CreateObject(NullHandler oReader)
        {
            ImportInvoiceRegister oImportInvoiceRegister = new ImportInvoiceRegister();
            oImportInvoiceRegister = MapObject(oReader);
            return oImportInvoiceRegister;
        }

        private List<ImportInvoiceRegister> CreateObjects(IDataReader oReader)
        {
            List<ImportInvoiceRegister> oImportInvoiceRegister = new List<ImportInvoiceRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportInvoiceRegister oItem = CreateObject(oHandler);
                oImportInvoiceRegister.Add(oItem);
            }
            return oImportInvoiceRegister;
        }

        #endregion

        #region Interface implementation
        public ImportInvoiceRegisterService() { }        
        public List<ImportInvoiceRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportInvoiceRegister> oImportInvoiceRegister = new List<ImportInvoiceRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportInvoiceRegisterDA.Gets(tc, sSQL);
                oImportInvoiceRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvoiceRegister", e);
                #endregion
            }

            return oImportInvoiceRegister;
        }
        #endregion
    }
}
