using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{

    public class VoucherBillDA
    {
        public VoucherBillDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VoucherBill oVoucherBill, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VoucherBill]" 
                                    + " %n, %n, %n, %n, %s, %d, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %d, %s, %n, %n",
                                    oVoucherBill.VoucherBillID, oVoucherBill.AccountHeadID, oVoucherBill.SubLedgerID, oVoucherBill.BUID, oVoucherBill.BillNo, oVoucherBill.BillDate, oVoucherBill.DueDate, oVoucherBill.CreditDays, oVoucherBill.Amount, oVoucherBill.IsActive, oVoucherBill.CurrencyID, oVoucherBill.CurrencyRate, oVoucherBill.CurrencyAmount, oVoucherBill.ReferenceTypeInInt, oVoucherBill.ReferenceObjID, oVoucherBill.OpeningBillAmount, oVoucherBill.OpeningBillDate, oVoucherBill.Remarks, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, VoucherBill oVoucherBill, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VoucherBill]"
                                    + " %n, %n, %n, %n, %s, %d, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %d, %s, %n, %n",
                                    oVoucherBill.VoucherBillID, oVoucherBill.AccountHeadID, oVoucherBill.SubLedgerID, oVoucherBill.BUID, oVoucherBill.BillNo, oVoucherBill.BillDate, oVoucherBill.DueDate, oVoucherBill.CreditDays, oVoucherBill.Amount, oVoucherBill.IsActive, oVoucherBill.CurrencyID, oVoucherBill.CurrencyRate, oVoucherBill.CurrencyAmount, oVoucherBill.ReferenceTypeInInt, oVoucherBill.ReferenceObjID, oVoucherBill.OpeningBillAmount, oVoucherBill.OpeningBillDate, oVoucherBill.Remarks, nUserId, (int)eEnumDBOperation);
        }
        public static void HoldUnHold(TransactionContext tc, VoucherBill oVoucherBill)
        {
            tc.ExecuteNonQuery("UPDATE VoucherBill SET IsHoldBill=%b WHERE VoucherBillID=%n", oVoucherBill.IsHoldBill, oVoucherBill.VoucherBillID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBill WHERE VoucherBillID = %n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nAccountHeadID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBill WHERE AccountHeadID=%n", nAccountHeadID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBill WHERE RemainningBalance > 0");
        }
        public static IDataReader GetsReceivableOrPayableBill(TransactionContext tc, int nComponentType)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBill AS VB WHERE VB.RemainningBalance>0 AND VB.AccountHeadID IN (SELECT COA.AccountHeadID FROM View_ChartsOfAccount AS COA WHERE COA.ComponentID=%n)", nComponentType);
        }
        #endregion
    }  
   
}
