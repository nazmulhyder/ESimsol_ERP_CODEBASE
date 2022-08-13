using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ImportRegisterService : MarshalByRefObject, IImportRegisterService
    {
        #region Private functions and declaration
        private ImportRegister MapObject(NullHandler oReader)
        {
            ImportRegister oImportRegister = new ImportRegister();
            oImportRegister.ImportPIDetailID = oReader.GetInt32("ImportPIDetailID");
            oImportRegister.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportRegister.ProductID = oReader.GetInt32("ProductID");
            oImportRegister.ProductCode = oReader.GetString("ProductCode");
            oImportRegister.ProductName = oReader.GetString("ProductName");
            oImportRegister.MUnitID = oReader.GetInt32("MUnitID");
            oImportRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportRegister.PIQty = oReader.GetDouble("PIQty");
            oImportRegister.PIAmount = oReader.GetDouble("PIAmount");
            oImportRegister.ImportPINo = oReader.GetString("ImportPINo");
            oImportRegister.BUID = oReader.GetInt32("BUID");
            oImportRegister.ImportPIDate = oReader.GetDateTime("PIDate");
            oImportRegister.ImportPIStatus = (EnumImportPIState)oReader.GetInt16("ImportPIStatus");
            oImportRegister.SupplierID = oReader.GetInt32("SupplierID");
            oImportRegister.ProductType = (EnumProductNature)oReader.GetInt16("ProductType");
            oImportRegister.PartyName = oReader.GetString("PartyName");
            oImportRegister.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportRegister.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportRegister.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oImportRegister.SalesContractNo = oReader.GetString("SalesContractNo");
            oImportRegister.SalesContractDate = oReader.GetDateTime("SalesContractDate");
            oImportRegister.BankAccountID = oReader.GetInt32("BankAccountID");
            oImportRegister.BankBranchID_Nego = oReader.GetInt32("BankBranchID_Nego");
            oImportRegister.LCAmount = oReader.GetDouble("LCAmount");
            oImportRegister.IssueBankName = oReader.GetString("IssueBankName");
            oImportRegister.IssueBankSName = oReader.GetString("IssueBankSName");
            oImportRegister.NegoBankName = oReader.GetString("NegoBankName");
            oImportRegister.NegoBankSName = oReader.GetString("NegoBankSName");
            oImportRegister.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportRegister.ImportInvoiceDate = oReader.GetDateTime("ImportInvoiceDate");
            oImportRegister.InvoiceValue = oReader.GetDouble("InvoiceValue");
            oImportRegister.InvoiceDueValue = oReader.GetDouble("InvoiceDueValue");
            oImportRegister.BillofLoadingNo = oReader.GetString("BillofLoadingNo");
            oImportRegister.BillofLoadingDate = oReader.GetDateTime("BillofLoadingDate");
            oImportRegister.BillOfEntrtNo = oReader.GetString("BillOfEntrtNo");
            oImportRegister.BillofEntrtDate = oReader.GetDateTime("BillofEntrtDate");
            oImportRegister.GoodsRcvQty = oReader.GetDouble("GoodsRcvQty");
            oImportRegister.GoodsRcvDate = oReader.GetDateTime("GoodsRcvDate");
            oImportRegister.AcceptanceDate = oReader.GetDateTime("AcceptanceDate");
            oImportRegister.MaturityValue = oReader.GetDouble("MaturityValue");
            oImportRegister.MaturityDate = oReader.GetDateTime("MaturityDate");
            oImportRegister.PaymentDate = oReader.GetDateTime("PaymentDate");

            return oImportRegister;
        }

        private ImportRegister CreateObject(NullHandler oReader)
        {
            ImportRegister oImportRegister = new ImportRegister();
            oImportRegister = MapObject(oReader);
            return oImportRegister;
        }

        private List<ImportRegister> CreateObjects(IDataReader oReader)
        {
            List<ImportRegister> oImportRegister = new List<ImportRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportRegister oItem = CreateObject(oHandler);
                oImportRegister.Add(oItem);
            }
            return oImportRegister;
        }

        #endregion

        #region Interface implementation
        public ImportRegisterService() { }
        public List<ImportRegister> Gets(string sSQL, long nUserID)
        {
            List<ImportRegister> oImportRegisters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportRegisterDA.Gets(tc,sSQL);
                oImportRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportRegister", e);
                #endregion
            }
            return oImportRegisters;
        }

        #endregion
    }   
}
