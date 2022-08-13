using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{


    public class CostCenterTransactionDA
    {
        public CostCenterTransactionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CostCenterTransaction oCostCenterTransaction, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CostCenterTransaction]" + "%n,%n,%n,%n,%n,%n,%n,%s,%d,%n,%d,%b,%n,%n",
                                    oCostCenterTransaction.CCTID,
                                    oCostCenterTransaction.CCID,
                                    oCostCenterTransaction.AccountHeadID,
                                    oCostCenterTransaction.VoucherDetailID,
                                    oCostCenterTransaction.Amount,
                                    oCostCenterTransaction.CurrencyID,
                                    oCostCenterTransaction.CurrencyConversionRate,
                                    oCostCenterTransaction.Description,
                                    oCostCenterTransaction.TransactionDate,
                                    oCostCenterTransaction.LastUpdateBY,
                                    oCostCenterTransaction.LastUpdateDate,
                                    oCostCenterTransaction.IsDr,
                                    nUserId,
                                    (int)eEnumDBOperation);
        }

        public static void Update(TransactionContext tc, CostCenterTransaction oCostCenterTransaction, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CostCenterTransaction]" + "%n,%n,%n,%n,%n,%n,%n,%s,%d,%n,%d,%b,%n,%n",
                                    oCostCenterTransaction.CCTID,
                                    oCostCenterTransaction.CCID,
                                    oCostCenterTransaction.AccountHeadID,
                                    oCostCenterTransaction.VoucherDetailID,
                                    oCostCenterTransaction.Amount,
                                    oCostCenterTransaction.CurrencyID,
                                    oCostCenterTransaction.CurrencyConversionRate,
                                    oCostCenterTransaction.Description,
                                    oCostCenterTransaction.TransactionDate,
                                    oCostCenterTransaction.LastUpdateBY,
                                    oCostCenterTransaction.LastUpdateDate,
                                    oCostCenterTransaction.IsDr,
                                    nUserId,
                                    (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, CostCenterTransaction oCostCenterTransaction, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CostCenterTransaction]" + "%n,%n,%n,%n,%n,%n,%n,%s,%d,%n,%d,%b,%n,%n",
                                    oCostCenterTransaction.CCTID,
                                    oCostCenterTransaction.CCID,
                                    oCostCenterTransaction.AccountHeadID,
                                    oCostCenterTransaction.VoucherDetailID,
                                    oCostCenterTransaction.Amount,
                                    oCostCenterTransaction.CurrencyID,
                                    oCostCenterTransaction.CurrencyConversionRate,
                                    oCostCenterTransaction.Description,
                                    oCostCenterTransaction.TransactionDate,
                                    oCostCenterTransaction.LastUpdateBY,
                                    oCostCenterTransaction.LastUpdateDate,
                                    oCostCenterTransaction.IsDr,
                                    nUserId,
                                    (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostCenterTransaction WHERE CCID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostCenterTransaction");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int nVoucherDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostCenterTransaction WHERE VoucherDetailID=%n", nVoucherDetailID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nVoucherID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostCenterTransaction WHERE VoucherDetailID in (Select VoucherDetail.VoucherDetailID from VoucherDetail where VoucherID=%n) ORDER BY VoucherDetailID, CCID", nVoucherID);
        }
        public static IDataReader GetsbyCC(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader( sSQL);
        }

        public static IDataReader GetsByAcccountHead(TransactionContext tc, int nAccountHeadID)
        {
            return tc.ExecuteReader("SELECT   CostCenter.Name as CostCenterName ,CostCenter.CCID ,0 as Amount ,'' as AccountHeadName ,''as VoucherNo ,0 as VoucherDetailID"
+" , '' as [Description]  ,GETDATE () as TransactionDate ,"
+ "((Select isnull(SUM(Amount),0) From CostCenterTransaction where CCID=CostCenter.CCID  and IsDr=1)-(Select isnull(SUM(Amount),0) From CostCenterTransaction where " + "CCID=CostCenter.CCID  and IsDr=0)) as CurrentBalance,(Select top(1)IsDr from CostCenterTransaction  where CCID=CostCenter.CCID  ) as IsDr FROM CostCenter WHERE CCGID = 5 and IsLastLayer=1 and CCID in (Select CCID from COA_ChartOfAccountCostCenter where AccountHeadID=" + nAccountHeadID.ToString() + ")");
        }
        public static IDataReader GetCurrentBalance(TransactionContext tc, int nCCID, int nCurrencyID, bool bIsApproved)
        {

            return tc.ExecuteReader("EXEC [dbo].[SP_CostCenterCurrentBalance] %n, %n, %b", nCCID, nCurrencyID, bIsApproved);
        }
        #endregion
    }  

   
}
