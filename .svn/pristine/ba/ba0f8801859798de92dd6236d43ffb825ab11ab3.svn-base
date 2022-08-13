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
    public class ImportPaymentService : MarshalByRefObject, IImportPaymentService
    {
        #region Private functions and declaration
        private ImportPayment MapObject(NullHandler oReader)
        {
            ImportPayment oImportPayment = new ImportPayment();
            oImportPayment.ImportPaymentID = oReader.GetInt32("ImportPaymentID");
            oImportPayment.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportPayment.LiabilityType = (EnumLiabilityType)oReader.GetInt32("LiabilityType");
            oImportPayment.LiabilityTypeInt = oReader.GetInt32("LiabilityType");
            oImportPayment.PaymentDate = oReader.GetDateTime("PaymentDate");
            oImportPayment.MarginAccountID = oReader.GetInt32("MarginAccountID");
            oImportPayment.MarginCurrencyID = oReader.GetInt32("MarginCurrencyID");
            oImportPayment.LCMarginAmount = oReader.GetDouble("LCMarginAmount");
            oImportPayment.MarginCCRate = oReader.GetDouble("MarginCCRate");
            oImportPayment.LCMarginAmountBC = oReader.GetDouble("LCMarginAmountBC");
            oImportPayment.LiabilityNo = oReader.GetString("LiabilityNo");
            oImportPayment.InterestRate = oReader.GetDouble("InterestRate");
            oImportPayment.DateOfOpening = oReader.GetDateTime("DateOfOpening");
            oImportPayment.DateOfMaturity = oReader.GetDateTime("DateOfMaturity");
            oImportPayment.BankAccountID = oReader.GetInt32("BankAccountID");
            oImportPayment.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportPayment.Amount = oReader.GetDouble("Amount");
            oImportPayment.CCRate = oReader.GetDouble("CCRate");
            oImportPayment.AmountBC = oReader.GetDouble("AmountBC");
            oImportPayment.Remarks = oReader.GetString("Remarks");
            oImportPayment.MarginSettledRate = oReader.GetDouble("MarginSettledRate");
            oImportPayment.LiabilitySettledRate = oReader.GetDouble("LiabilitySettledRate");
            oImportPayment.ForExGainLoss = (EnumForExGainLoss)oReader.GetInt32("ForExGainLoss");
            oImportPayment.ForExGainLossInt = oReader.GetInt32("ForExGainLoss");
            oImportPayment.ForExCurrencyID = oReader.GetInt32("ForExCurrencyID");
            oImportPayment.ForExAmount = oReader.GetDouble("ForExAmount");
            oImportPayment.ForExCCRate = oReader.GetDouble("ForExCCRate");
            oImportPayment.ForExAmountBC = oReader.GetDouble("ForExAmountBC");
            oImportPayment.ChargeAmount = oReader.GetDouble("ChargeAmount");
            oImportPayment.ChargeAmountBC = oReader.GetDouble("ChargeAmountBC");
            oImportPayment.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oImportPayment.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportPayment.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportPayment.InvoiceCurrencyID = oReader.GetInt32("InvoiceCurrencyID");
            oImportPayment.Amount_Invoice = oReader.GetDouble("Amount_Invoice");
            oImportPayment.CRate_Acceptance = oReader.GetDouble("CRate_Acceptance");
            oImportPayment.Amount_LC = oReader.GetDouble("Amount_LC");
            oImportPayment.MarginAccountName = oReader.GetString("MarginAccountName");
            oImportPayment.MarginBankName = oReader.GetString("MarginBankName");
            oImportPayment.MarginAccountNo = oReader.GetString("MarginAccountNo");
            oImportPayment.MarginCSymbol = oReader.GetString("MarginCSymbol");
            oImportPayment.BankAccountName = oReader.GetString("BankAccountName");
            oImportPayment.BankName = oReader.GetString("BankName");
            oImportPayment.BankBranchID = oReader.GetInt32("BankBranchID");
            oImportPayment.BranchName = oReader.GetString("BranchName");
            oImportPayment.AccountNo = oReader.GetString("AccountNo");
            oImportPayment.AccountType = (EnumBankAccountType)oReader.GetInt32("AccountType");
            oImportPayment.CSymbol = oReader.GetString("CSymbol");
            oImportPayment.ForExCSymbol = oReader.GetString("ForExCSymbol");
            oImportPayment.ApprovedByName = oReader.GetString("ApprovedByName");
            oImportPayment.BUID = oReader.GetInt32("BUID");
            return oImportPayment;
        }

        private ImportPayment CreateObject(NullHandler oReader)
        {
            ImportPayment oImportPayment = new ImportPayment();
            oImportPayment = MapObject(oReader);
            return oImportPayment;
        }

        private List<ImportPayment> CreateObjects(IDataReader oReader)
        {
            List<ImportPayment> oImportPayment = new List<ImportPayment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPayment oItem = CreateObject(oHandler);
                oImportPayment.Add(oItem);
            }
            return oImportPayment;
        }

        #endregion

        #region Interface implementation
        public ImportPaymentService() { }
        public ImportPayment Get(int id, Int64 nUserId)
        {
            ImportPayment oImportPayment = new ImportPayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPaymentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPayment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPayment", e);
                #endregion
            }

            return oImportPayment;
        }
        public ImportPayment GetBy(int nImportInvoiceID, Int64 nUserId)
        {
            ImportPayment oImportPayment = new ImportPayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPaymentDA.GetBy(tc, nImportInvoiceID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPayment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPayment", e);
                #endregion
            }

            return oImportPayment;
        }

        public List<ImportPayment> Gets(Int64 nUserID)
        {
            List<ImportPayment> oImportPayment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPaymentDA.Gets(tc);
                oImportPayment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPayment", e);
                #endregion
            }

            return oImportPayment;
        }
        public List<ImportPayment> Gets(string sSQL,Int64 nUserID)
        {
            List<ImportPayment> oImportPayment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPaymentDA.Gets(tc, sSQL);
                oImportPayment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPayment", e);
                #endregion
            }

            return oImportPayment;
        }
        #endregion
    }
   
}
