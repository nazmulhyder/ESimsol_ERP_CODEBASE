using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherDetailDA
    {
        public VoucherDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherDetail oVoucherDetail, EnumDBOperation eEnumDBOperation, string sVoucherDetailID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VoucherDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %b, %n, %n, %n, %s, %n, %s",
                                    oVoucherDetail.VoucherDetailID, oVoucherDetail.VoucherID, oVoucherDetail.AreaID, oVoucherDetail.ZoneID, oVoucherDetail.SiteID, oVoucherDetail.ProductID, oVoucherDetail.DeptID, oVoucherDetail.AccountHeadID, oVoucherDetail.CostCenterID, oVoucherDetail.CurrencyID, oVoucherDetail.IsDebit, oVoucherDetail.AmountInCurrency, oVoucherDetail.ConversionRate, oVoucherDetail.Amount, oVoucherDetail.Narration, (int)eEnumDBOperation, sVoucherDetailID);
        }
        public static void Delete(TransactionContext tc, VoucherDetail oVoucherDetail, EnumDBOperation eEnumDBOperation, string sVoucherDetailID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VoucherDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %b, %n, %n, %n, %s, %n, %s",
                                    oVoucherDetail.VoucherDetailID, oVoucherDetail.VoucherID, oVoucherDetail.AreaID, oVoucherDetail.ZoneID, oVoucherDetail.SiteID, oVoucherDetail.ProductID, oVoucherDetail.DeptID, oVoucherDetail.AccountHeadID, oVoucherDetail.CostCenterID, oVoucherDetail.CurrencyID, oVoucherDetail.IsDebit, oVoucherDetail.AmountInCurrency, oVoucherDetail.ConversionRate, oVoucherDetail.Amount, oVoucherDetail.Narration, (int)eEnumDBOperation, sVoucherDetailID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nId)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherDetail WHERE VoucherDetailID=%n", nId);
        }

        public static IDataReader GetCurrentBalance(TransactionContext tc, int nAccountHeadID, bool bIsDebit, DateTime dStartDate, DateTime dEndDate)
        {
            if (bIsDebit)
            {
                return tc.ExecuteReader("SELECT (SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE AccountHeadID=" + nAccountHeadID + " AND VD.AuthorizedBy<>0 AND VD.IsDebit=1 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))) "
                                        + "-ISNULL((SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE AccountHeadID=" + nAccountHeadID + " AND VD.AuthorizedBy<>0 AND VD.IsDebit=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))),0) "
                                        + "AS CurrentBalance");
            }
            else
            {
                return tc.ExecuteReader("SELECT (SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE AccountHeadID=" + nAccountHeadID + " AND VD.AuthorizedBy<>0 AND VD.IsDebit=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))) "
                                        + "-ISNULL((SELECT ISNULL(SUM(VD.Amount),0) FROM View_VoucherDetail AS VD WHERE AccountHeadID=" + nAccountHeadID + " AND VD.AuthorizedBy<>0 AND VD.IsDebit=1 AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))),0) "
                                        + "AS CurrentBalance");
            }
            
        }

        public static IDataReader GetProfitLossAccountTransaction(TransactionContext tc, int nBUID, DateTime dStartDate, DateTime dEndDate,int nCompanyID)
        {
            //14 = P/L Appropriation A/C 
            return tc.ExecuteReader("SELECT * FROM View_VoucherDetail AS VD WHERE VD.BUID =%n AND VD.AccountHeadID=14 AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE V.OperationType=2 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)))", nBUID, dStartDate, dEndDate);
        }

        public static IDataReader GetDividendTransaction(TransactionContext tc, int nBUID, DateTime dStartDate, DateTime dEndDate)
        {
            //14 = P/L Appropriation A/C 
            return tc.ExecuteReader("SELECT * FROM View_VoucherDetail WHERE BUID=%n AND AccountHeadID=14 AND VoucherID IN (SELECT VoucherID FROM Voucher WHERE OperationType=3 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)))", nBUID, dStartDate, dEndDate);
        }

        public static IDataReader GetRetaindEarningTransaction(TransactionContext tc, int nBUID, DateTime dStartDate, DateTime dEndDate)
        {
            //14 = P/L Appropriation A/C 
            return tc.ExecuteReader("SELECT * FROM View_VoucherDetail WHERE BUID=%n AND AccountHeadID=14 AND VoucherID IN (SELECT VoucherID FROM Voucher WHERE OperationType=4 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)))", nBUID, dStartDate, dEndDate);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherDetail");
        }
        public static IDataReader Gets(TransactionContext tc, long nVoucherId, int nUserID)
        {
            //Add CompanyID
            //return tc.ExecuteReader("SELECT * FROM View_VoucherDetail AS MM WHERE MM.VoucherID=%n ORDER BY MM.IsDebit DESC", nVoucherId);
            return tc.ExecuteReader("SELECT *, dbo.GetLedgerBalance(MM.AccountHeadID, MM.BUID," + nUserID.ToString() + ") AS LedgerBalance FROM View_VoucherDetail AS MM WHERE MM.VoucherID=%n ORDER BY MM.IsDebit DESC", nVoucherId);
        }
        public static IDataReader GetsForVoucherView(TransactionContext tc, long nVoucherId, int nBUID, int nUserID)
        {
            //Add CompanyID
            return tc.ExecuteReader("SELECT *, dbo.GetLedgerBalance(MM.AccountHeadID, " + nBUID.ToString() + "," + nUserID.ToString() + ") AS LedgerBalance FROM View_VoucherDetail AS MM WHERE MM.VoucherID=%n ORDER BY MM.IsDebit DESC", nVoucherId);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
