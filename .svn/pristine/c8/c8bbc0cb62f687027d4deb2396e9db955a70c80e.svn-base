using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ChequeBookDA
    {
        public ChequeBookDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ChequeBook oChequeBook, EnumDBOperation eEnumDBOperation,int nCurrentUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ChequeBook]" + "%n, %n, %s, %s, %n, %s, %b, %n, %D, %s, %n, %n",
                                   oChequeBook.ChequeBookID, oChequeBook.BankAccountID, oChequeBook.BookCodePartOne, oChequeBook.BookCodePartTwo, oChequeBook.PageCount, oChequeBook.FirstChequeNo, oChequeBook.IsActive, oChequeBook.ActivteBy, oChequeBook.ActivateTime, oChequeBook.Note, (int)eEnumDBOperation, nCurrentUserID);
        }

        public static void Delete(TransactionContext tc, ChequeBook oChequeBook, EnumDBOperation eEnumDBOperation, int nCurrentUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ChequeBook]" + "%n, %n, %s, %s, %n, %s, %b, %n, %D, %s, %n, %n",
                                   oChequeBook.ChequeBookID, oChequeBook.BankAccountID, oChequeBook.BookCodePartOne, oChequeBook.BookCodePartTwo, oChequeBook.PageCount, oChequeBook.FirstChequeNo, oChequeBook.IsActive, oChequeBook.ActivteBy, oChequeBook.ActivateTime, oChequeBook.Note, (int)eEnumDBOperation, nCurrentUserID);
        }
        public static void ChequeBookActiveInActive(TransactionContext tc, ChequeBook oChequeBook)
        {
            tc.ExecuteNonQuery("UPDATE ChequeBook SET  IsActive=%b, ActivteBy=%n,  ActivateTime=%D FROM ChequeBook WHERE ChequeBookID=%n", oChequeBook.IsActive, oChequeBook.ActivteBy, oChequeBook.ActivateTime, oChequeBook.ChequeBookID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeBook WHERE ChequeBookID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeBook ORDER BY BankName, AccountNo, BookCodePartTwo ASC");
        }
        public static IDataReader Gets(TransactionContext tc, bool bIsActive)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeBook WHERE IsActive=%b ORDER BY ChequeBookID DESC, BankName, AccountNo, BookCodePartTwo ASC", bIsActive);
        }
        public static IDataReader GetsByAccountNo(TransactionContext tc, string sAccountNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeBook WHERE AccountNo LIKE '%" + sAccountNo + "%' ORDER BY AccountNo ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
