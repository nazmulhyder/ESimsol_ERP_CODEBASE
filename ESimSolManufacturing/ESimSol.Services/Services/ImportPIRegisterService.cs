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
    public class ImportPIRegisterService : MarshalByRefObject, IImportPIRegisterService
    {
        #region Private functions and declaration
        private ImportPIRegister MapObject(NullHandler oReader)
        {
            ImportPIRegister oImportPIRegister = new ImportPIRegister();
            oImportPIRegister.ImportPIDetailID = oReader.GetInt32("ImportPIDetailID");
            oImportPIRegister.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportPIRegister.ProductID = oReader.GetInt32("ProductID");
            oImportPIRegister.MUnitID = oReader.GetInt32("MUnitID");
            oImportPIRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportPIRegister.Qty = oReader.GetDouble("Qty");
            oImportPIRegister.Amount = oReader.GetDouble("Amount");
            oImportPIRegister.ImportPINo = oReader.GetString("ImportPINo");
            oImportPIRegister.BUID = oReader.GetInt32("BUID");
            oImportPIRegister.SLNo = oReader.GetString("SLNo");
            oImportPIRegister.IssueDate = oReader.GetDateTime("IssueDate");
            oImportPIRegister.ImportPIStatus = (EnumImportPIState)oReader.GetInt32("ImportPIStatus");
            oImportPIRegister.SupplierID = oReader.GetInt32("SupplierID");
            oImportPIRegister.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oImportPIRegister.ConcernPersonID = oReader.GetInt32("ConcernPersonID");
            oImportPIRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportPIRegister.TotalValue = oReader.GetDouble("TotalValue");
            oImportPIRegister.AdviseBBID = oReader.GetInt32("AdviseBBID");
            oImportPIRegister.LCTermID = oReader.GetInt32("LCTermID");
            oImportPIRegister.ImportPIType = (EnumImportPIType)oReader.GetInt32("ImportPIType");
            oImportPIRegister.PaymentInstructionType = (EnumPaymentInstruction)oReader.GetInt32("PaymentInstructionType");
            oImportPIRegister.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oImportPIRegister.DateOfApproved = oReader.GetDateTime("DateOfApproved");
            oImportPIRegister.VersionNumber = oReader.GetInt32("VersionNumber");
            oImportPIRegister.ValidityDate = oReader.GetDateTime("ValidityDate");
            oImportPIRegister.DeliveryClause = oReader.GetString("DeliveryClause");
            oImportPIRegister.PaymentClause = oReader.GetString("PaymentClause");
            oImportPIRegister.ShipmentBy = (EnumShipmentBy)oReader.GetInt32("ShipmentBy");
            oImportPIRegister.Remarks = oReader.GetString("Remarks");
            oImportPIRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oImportPIRegister.ProductCode = oReader.GetString("ProductCode");
            oImportPIRegister.ProductName = oReader.GetString("ProductName");
            oImportPIRegister.MUName = oReader.GetString("MUName");
            oImportPIRegister.MUSymbol = oReader.GetString("MUSymbol");
            oImportPIRegister.SupplierName = oReader.GetString("SupplierName");
            oImportPIRegister.SCPName = oReader.GetString("SCPName");
            oImportPIRegister.CPName = oReader.GetString("CPName");
            oImportPIRegister.CurrencyName = oReader.GetString("CurrencyName");
            oImportPIRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oImportPIRegister.BankName = oReader.GetString("BankName");
            oImportPIRegister.BranchName = oReader.GetString("BranchName");
            oImportPIRegister.LCTermName = oReader.GetString("LCTermName");
            oImportPIRegister.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportPIRegister.RateUnit = oReader.GetInt32("RateUnit");
            //PI Register Two
            oImportPIRegister.ImportPIDate = oReader.GetDateTime("ImportPIDate");
            oImportPIRegister.PIQty = oReader.GetDouble("PIQty");
            oImportPIRegister.PIAmount = oReader.GetDouble("PIAmount");
            oImportPIRegister.SalesContractNo = oReader.GetString("SalesContractNo");
            oImportPIRegister.SalesContractDate = oReader.GetDateTime("SalesContractDate");
            oImportPIRegister.LCID = oReader.GetInt32("LCID");
            oImportPIRegister.LCNo = oReader.GetString("LCNo");
            oImportPIRegister.LCDate = oReader.GetDateTime("LCDate");
            oImportPIRegister.BankBranchID = oReader.GetInt32("BankBranchID");
            oImportPIRegister.LCAmount = oReader.GetDouble("LCAmount");
            oImportPIRegister.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oImportPIRegister.InvoiceValue = oReader.GetDouble("InvoiceValue");
            oImportPIRegister.ContractorName = oReader.GetString("ContractorName");
            oImportPIRegister.ContractorID = oReader.GetInt32("ContractorID");
            oImportPIRegister.GRNQty = oReader.GetDouble("GRNQty");
            oImportPIRegister.MUnitName = oReader.GetString("MUnitName");

            return oImportPIRegister;
        }

        private ImportPIRegister CreateObject(NullHandler oReader)
        {
            ImportPIRegister oImportPIRegister = new ImportPIRegister();
            oImportPIRegister = MapObject(oReader);
            return oImportPIRegister;
        }

        private List<ImportPIRegister> CreateObjects(IDataReader oReader)
        {
            List<ImportPIRegister> oImportPIRegister = new List<ImportPIRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPIRegister oItem = CreateObject(oHandler);
                oImportPIRegister.Add(oItem);
            }
            return oImportPIRegister;
        }

        #endregion

        #region Interface implementation
        public ImportPIRegisterService() { }        
        public List<ImportPIRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportPIRegister> oImportPIRegister = new List<ImportPIRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportPIRegisterDA.Gets(tc, sSQL);
                oImportPIRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIRegister", e);
                #endregion
            }

            return oImportPIRegister;
        }

        public List<ImportPIRegister> GetsForPIRegTwo(string sSQL, Int64 nUserID)
        {
            List<ImportPIRegister> oImportPIRegister = new List<ImportPIRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportPIRegisterDA.GetsForPIRegTwo(tc, sSQL);
                oImportPIRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIRegister", e);
                #endregion
            }

            return oImportPIRegister;
        }

        #endregion
    }
}
