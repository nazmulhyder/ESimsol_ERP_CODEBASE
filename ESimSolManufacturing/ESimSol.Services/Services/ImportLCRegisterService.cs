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
    public class ImportLCRegisterService : MarshalByRefObject, IImportLCRegisterService
    {
        #region Private functions and declaration
        private ImportLCRegister MapObject(NullHandler oReader)
        {
            ImportLCRegister oImportLCRegister = new ImportLCRegister();
            oImportLCRegister.ImportLCDetailID = oReader.GetInt32("ImportLCDetailID");
            oImportLCRegister.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportLCRegister.ProductID = oReader.GetInt32("ProductID");
            oImportLCRegister.MUnitID = oReader.GetInt32("MUnitID");
            oImportLCRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportLCRegister.Qty = oReader.GetDouble("Qty");
            oImportLCRegister.InvoiceValue = oReader.GetDouble("InvoiceValue");
            oImportLCRegister.Amount = oReader.GetDouble("Amount");
            oImportLCRegister.AmountWithRate = oReader.GetDouble("AmountWithRate");
            oImportLCRegister.CCRate = oReader.GetDouble("CCRate");
            oImportLCRegister.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportLCRegister.BUID = oReader.GetInt32("BUID");
            oImportLCRegister.SLNo = oReader.GetString("SLNo");
            oImportLCRegister.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oImportLCRegister.LCCurrentStatus = (EnumLCCurrentStatus)oReader.GetInt32("ImportLCStatus");
            oImportLCRegister.SupplierID = oReader.GetInt32("SupplierID");
            oImportLCRegister.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oImportLCRegister.ConcernPersonID = oReader.GetInt32("ConcernPersonID");
            oImportLCRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportLCRegister.TotalValue = oReader.GetDouble("TotalValue");
            oImportLCRegister.AdviseBBID = oReader.GetInt32("AdviseBBID");
            oImportLCRegister.LCTermID = oReader.GetInt32("LCTermID");
            oImportLCRegister.PaymentInstructionType = (EnumPaymentInstruction)oReader.GetInt32("PaymentInstructionType");
            oImportLCRegister.ReceivedBy = oReader.GetInt32("ApprovedBy");
            oImportLCRegister.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oImportLCRegister.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oImportLCRegister.VersionNo = oReader.GetInt32("VersionNo");
            oImportLCRegister.ExpireDate = oReader.GetDateTime("ExpireDate");
            oImportLCRegister.DeliveryClause = oReader.GetString("DeliveryClause");
            oImportLCRegister.PaymentClause = oReader.GetString("PaymentClause");
            oImportLCRegister.ShipmentBy = (EnumShipmentBy)oReader.GetInt32("ShipmentBy");
            oImportLCRegister.LCAppType = (EnumLCAppType)oReader.GetInt32("LCAppType");
            oImportLCRegister.ImportPIType = (EnumImportPIType)oReader.GetInt32("ImportPIType");
            oImportLCRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oImportLCRegister.ProductCode = oReader.GetString("ProductCode");
            oImportLCRegister.ProductName = oReader.GetString("ProductName");
            oImportLCRegister.MUName = oReader.GetString("MUName");
            oImportLCRegister.MUSymbol = oReader.GetString("MUSymbol");
            oImportLCRegister.SupplierName = oReader.GetString("SupplierName");
            oImportLCRegister.SCPName = oReader.GetString("SCPName");
            oImportLCRegister.CPName = oReader.GetString("CPName");
            oImportLCRegister.CurrencyName = oReader.GetString("CurrencyName");
            oImportLCRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oImportLCRegister.BankName = oReader.GetString("BankName");
            oImportLCRegister.BankShortName = oReader.GetString("BankShortName");
            oImportLCRegister.ExportLCNo = oReader.GetString("ExportLCNo");
            oImportLCRegister.BranchName = oReader.GetString("BranchName");
            oImportLCRegister.LCTermName = oReader.GetString("LCTermName");
            oImportLCRegister.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportLCRegister.FileNo = oReader.GetString("FileNo");
            oImportLCRegister.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportLCRegister.ImportPINo = oReader.GetString("ImportPINo");
            oImportLCRegister.PIDate = oReader.GetDateTime("PIDate");
            oImportLCRegister.PIValue = oReader.GetDouble("PIValue");
            oImportLCRegister.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oImportLCRegister.AmmendmentAmount = oReader.GetDouble("AmmendmentAmount");
            oImportLCRegister.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            //oImportLCRegister.YetToInvoiceQty = oReader.GetDouble("YetToInvoiceQty");
            //oImportLCRegister.InvoiceAmount = oReader.GetDouble("InvoiceAmount");
            //oImportLCRegister.YetToInvoiceAmount = oReader.GetDouble("YetToInvoiceAmount");
            return oImportLCRegister;
        }

        private ImportLCRegister CreateObject(NullHandler oReader)
        {
            ImportLCRegister oImportLCRegister = new ImportLCRegister();
            oImportLCRegister = MapObject(oReader);
            return oImportLCRegister;
        }

        private List<ImportLCRegister> CreateObjects(IDataReader oReader)
        {
            List<ImportLCRegister> oImportLCRegister = new List<ImportLCRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCRegister oItem = CreateObject(oHandler);
                oImportLCRegister.Add(oItem);
            }
            return oImportLCRegister;
        }

        #endregion

        #region Interface implementation
        public ImportLCRegisterService() { }        
        public List<ImportLCRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportLCRegister> oImportLCRegister = new List<ImportLCRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportLCRegisterDA.Gets(tc, sSQL);
                oImportLCRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLCRegister", e);
                #endregion
            }

            return oImportLCRegister;
        }
        #endregion
    }
}
