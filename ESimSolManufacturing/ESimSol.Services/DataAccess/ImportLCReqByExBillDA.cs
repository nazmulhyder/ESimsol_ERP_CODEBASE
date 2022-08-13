using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class ImportLCReqByExBillDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ImportLCReqByExBill oImportLCReqByExBill, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCReqByExBill]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%d,%n,%n",
                                   oImportLCReqByExBill.ImportLCReqByExBillID, oImportLCReqByExBill.ImportLCID, oImportLCReqByExBill.AmendmentNo, oImportLCReqByExBill.BankAccountID, oImportLCReqByExBill.Amount, oImportLCReqByExBill.MarginLien, oImportLCReqByExBill.MarginCash, oImportLCReqByExBill.MarginSCF, oImportLCReqByExBill.MarginLienP, oImportLCReqByExBill.MarginCashP, oImportLCReqByExBill.MarginSCFP, oImportLCReqByExBill.Enclosed, oImportLCReqByExBill.Note, oImportLCReqByExBill.IssueDate, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ImportLCReqByExBill oImportLCReqByExBill, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLCReqByExBill]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%d,%n,%n",
                                    oImportLCReqByExBill.ImportLCReqByExBillID, oImportLCReqByExBill.ImportLCID, oImportLCReqByExBill.AmendmentNo, oImportLCReqByExBill.BankAccountID, oImportLCReqByExBill.Amount, oImportLCReqByExBill.MarginLien, oImportLCReqByExBill.MarginCash, oImportLCReqByExBill.MarginSCF, oImportLCReqByExBill.MarginLienP, oImportLCReqByExBill.MarginCashP, oImportLCReqByExBill.MarginSCFP, oImportLCReqByExBill.Enclosed, oImportLCReqByExBill.Note, oImportLCReqByExBill.IssueDate, nUserID, (int)eEnumDBOperation);
        }

        #endregion



        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLCReqByExBill WHERE ImportLCReqByExBillID=%n", nID);
        }
        public static IDataReader GetByLC(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT TOP 1 * FROM View_ImportLCReqByExBill WHERE ImportLCID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLCReqByExBill ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
