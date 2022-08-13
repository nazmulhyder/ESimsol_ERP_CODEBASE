using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class ImportPaymentDA
    {
        public ImportPaymentDA() { }

        #region Insert Function        
        public static IDataReader InsertUpdate(TransactionContext tc, ImportPayment oImportPayment, EnumDBOperation eDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPayment]"
                                    + "%n, %n, %n, %d, %n, %n, %n, %n, %n, %s, %n, %d, %d, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n",
                                    oImportPayment.ImportPaymentID,  oImportPayment.ImportInvoiceID, oImportPayment.LiabilityTypeInt, oImportPayment.PaymentDate, oImportPayment.MarginAccountID, oImportPayment.MarginCurrencyID, oImportPayment.LCMarginAmount, oImportPayment.MarginCCRate, oImportPayment.LCMarginAmountBC, oImportPayment.LiabilityNo, oImportPayment.InterestRate, oImportPayment.DateOfOpening, oImportPayment.DateOfMaturity, oImportPayment.BankAccountID, oImportPayment.CurrencyID, oImportPayment.Amount, oImportPayment.CCRate, oImportPayment.AmountBC, oImportPayment.Remarks, oImportPayment.MarginSettledRate, oImportPayment.LiabilitySettledRate, oImportPayment.ForExGainLossInt, oImportPayment.ForExCurrencyID, oImportPayment.ForExAmount, oImportPayment.ForExCCRate, oImportPayment.ForExAmountBC, oImportPayment.ChargeAmount, oImportPayment.ChargeAmountBC, oImportPayment.ApprovedBy,  nUserID, (int)eDBOperation);
        }
        public static void Delete(TransactionContext tc, ImportPayment oImportPayment, EnumDBOperation eDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPayment]"
                                    + "%n, %n, %n, %d, %n, %n, %n, %n, %n, %s, %n, %d, %d, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n",
                                    oImportPayment.ImportPaymentID, oImportPayment.ImportInvoiceID, oImportPayment.LiabilityTypeInt, oImportPayment.PaymentDate, oImportPayment.MarginAccountID, oImportPayment.MarginCurrencyID, oImportPayment.LCMarginAmount, oImportPayment.MarginCCRate, oImportPayment.LCMarginAmountBC, oImportPayment.LiabilityNo, oImportPayment.InterestRate, oImportPayment.DateOfOpening, oImportPayment.DateOfMaturity, oImportPayment.BankAccountID, oImportPayment.CurrencyID, oImportPayment.Amount, oImportPayment.CCRate, oImportPayment.AmountBC, oImportPayment.Remarks, oImportPayment.MarginSettledRate, oImportPayment.LiabilitySettledRate, oImportPayment.ForExGainLossInt, oImportPayment.ForExCurrencyID, oImportPayment.ForExAmount, oImportPayment.ForExCCRate, oImportPayment.ForExAmountBC, oImportPayment.ChargeAmount, oImportPayment.ChargeAmountBC, oImportPayment.ApprovedBy, nUserID, (int)eDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {

            return tc.ExecuteReader("SELECT * FROM View_ImportPayment WHERE ImportPaymentID=%n", nID);
        }
        public static IDataReader GetBy(TransactionContext tc, int nImportInvoiceID)
        {

            return tc.ExecuteReader("SELECT * FROM View_ImportPayment WHERE ImportInvoiceID=%n", nImportInvoiceID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPayment WHERE LiabilityType in (2,3) ");
        }       
        public static IDataReader Gets(TransactionContext tc, string nSql)
        {
            return tc.ExecuteReader(nSql);
        }
        #endregion

    }
}
