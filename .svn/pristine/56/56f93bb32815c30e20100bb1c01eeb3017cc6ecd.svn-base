using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class ImportSummaryRegisterService : MarshalByRefObject, IImportSummaryRegisterService
    {
        #region Private functions and declaration

        private ImportSummaryRegister MapObject(NullHandler oReader)
        {
            ImportSummaryRegister oImportSummaryRegister = new ImportSummaryRegister();
            oImportSummaryRegister.ImportPIDetailID = oReader.GetInt32("ImportPIDetailID");
            oImportSummaryRegister.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportSummaryRegister.ImportPINo = oReader.GetString("ImportPINo");
            oImportSummaryRegister.ImportPIDate = oReader.GetDateTime("ImportPIDate");
            oImportSummaryRegister.ProductName = oReader.GetString("ProductName");
            oImportSummaryRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportSummaryRegister.PIQty = oReader.GetDouble("PIQty");
            oImportSummaryRegister.CRate = oReader.GetDouble("CRate");
            oImportSummaryRegister.PIAmount = oReader.GetDouble("PIAmount");
            oImportSummaryRegister.PartyName = oReader.GetString("PartyName");
            oImportSummaryRegister.SalesContractNo = oReader.GetString("SalesContractNo");
            oImportSummaryRegister.SalesContractDate = oReader.GetDateTime("SalesContractDate");
            oImportSummaryRegister.LCID = oReader.GetInt32("LCID");
            oImportSummaryRegister.LCNo = oReader.GetString("LCNo");
            oImportSummaryRegister.LCDate = oReader.GetDateTime("LCDate");
            oImportSummaryRegister.LCAdviseBankName = oReader.GetString("LCAdviseBankName");
            oImportSummaryRegister.LCAmount = oReader.GetDouble("LCAmount");
            oImportSummaryRegister.InvoiceDetailID = oReader.GetInt32("InvoiceDetailID");
            oImportSummaryRegister.InvoiceID = oReader.GetInt32("InvoiceID");
            oImportSummaryRegister.InvoiceNo = oReader.GetString("InvoiceNo");
            oImportSummaryRegister.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oImportSummaryRegister.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oImportSummaryRegister.InvoiceValue = oReader.GetDouble("InvoiceValue");
            oImportSummaryRegister.InvoiceDue = oReader.GetDouble("InvoiceDue");
            oImportSummaryRegister.AcceptanceDate = oReader.GetDateTime("AcceptanceDate");
            oImportSummaryRegister.MaturityDate = oReader.GetDateTime("MaturityDate");
            oImportSummaryRegister.PaymentDate = oReader.GetDateTime("PaymentDate");
            oImportSummaryRegister.PaymentType = (EnumLiabilityType)oReader.GetInt32("PaymentType");
            oImportSummaryRegister.MaturityValue = oReader.GetDouble("MaturityValue");
            oImportSummaryRegister.GRNDetailID = oReader.GetInt32("GRNDetailID");
            oImportSummaryRegister.RefType = (EnumGRNType)oReader.GetInt32("RefType");
            oImportSummaryRegister.RefObjectID = oReader.GetInt32("RefObjectID");
            oImportSummaryRegister.GRNReceivedQty = oReader.GetDouble("GRNReceivedQty");
            oImportSummaryRegister.GoodReceivedDate = oReader.GetDateTime("GoodReceivedDate");
            return oImportSummaryRegister;
        }

        private ImportSummaryRegister CreateObject(NullHandler oReader)
        {
            ImportSummaryRegister oImportSummaryRegister = new ImportSummaryRegister();
            oImportSummaryRegister = MapObject(oReader);
            return oImportSummaryRegister;
        }

        private List<ImportSummaryRegister> CreateObjects(IDataReader oReader)
        {
            List<ImportSummaryRegister> oImportSummaryRegister = new List<ImportSummaryRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportSummaryRegister oItem = CreateObject(oHandler);
                oImportSummaryRegister.Add(oItem);
            }
            return oImportSummaryRegister;
        }

        #endregion

        #region Interface implementation

        public List<ImportSummaryRegister> Gets(string sGRNDSql, string sInvoiceDSql, string sPIDSql, Int64 nUserID)
        {
            List<ImportSummaryRegister> oImportSummarys = new List<ImportSummaryRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportSummaryRegisterDA.Gets(tc, sGRNDSql, sInvoiceDSql, sPIDSql);
                oImportSummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportSummaryRegister", e);
                #endregion
            }
            return oImportSummarys;
        }

        #endregion
    }

}
