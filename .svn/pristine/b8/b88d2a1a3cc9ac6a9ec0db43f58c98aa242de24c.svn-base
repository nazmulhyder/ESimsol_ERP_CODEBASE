using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ChequeDA
    {
        public ChequeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Cheque oCheque, EnumDBOperation eEnumDBOperation, int nCurrentUserID, string sChequeIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Cheque]" + "%n, %n, %n, %n, %s, %d, %n, %n, %n, %s, %s, %n, %n, %s",
                                                            oCheque.ChequeID, oCheque.ChequeBookID, (int)oCheque.ChequeStatus, (int)oCheque.PaymentType, oCheque.ChequeNo, NullHandler.GetNullValue(oCheque.ChequeDate), oCheque.PayTo, oCheque.IssueFigureID, oCheque.Amount, oCheque.VoucherReference, oCheque.Note, (int)eEnumDBOperation, nCurrentUserID, sChequeIDs);
        }

        public static void Delete(TransactionContext tc, Cheque oCheque, EnumDBOperation eEnumDBOperation, int nCurrentUserID, string sChequeIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Cheque]" + "%n, %n, %n, %n, %s, %d, %n, %n, %n, %s, %s, %n, %n, %s",
                                                            oCheque.ChequeID, oCheque.ChequeBookID, (int)oCheque.ChequeStatus, (int)oCheque.PaymentType, oCheque.ChequeNo, NullHandler.GetNullValue(oCheque.ChequeDate), oCheque.PayTo, oCheque.IssueFigureID, oCheque.Amount, oCheque.VoucherReference, oCheque.Note, (int)eEnumDBOperation, nCurrentUserID, sChequeIDs);
        }
        public static IDataReader UpdateChequeStatus(TransactionContext tc, Cheque oCheque, ChequeHistory oChequeHistory, int nCurrentUserID)
        {
            return tc.ExecuteReader("EXEC [SP_UpdateChequeStatus] %n, %n, %n, %n, %s, %s, %n, %n, %d, %n, %s, %s, %n, %n",
                                    oChequeHistory.ChequeHistoryID,
                                    oChequeHistory.ChequeID,
                                    (int)oChequeHistory.PreviousStatus,
                                    (int)oChequeHistory.CurrentStatus,
                                    oChequeHistory.Note,
                                    oChequeHistory.ChangeLog,
                                    oCheque.PayTo,
                                    oCheque.IssueFigureID,
                                    NullHandler.GetNullValue(oCheque.ChequeDate),
                                    oCheque.Amount,
                                    oCheque.VoucherReference,
                                    oCheque.Note,
                                    oCheque.PaymentType,
                                    nCurrentUserID);
        }
        public static void ConfirmRegisterPrint(TransactionContext tc, Cheque oCheque, int nCurrentUserID)
        {
            tc.ExecuteNonQuery("UPDATE Cheque SET RegisterPrint=1 WHERE ChequeID=%n", oCheque.ChequeID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Cheque WHERE ChequeID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Cheque");
        }
        public static IDataReader Gets(TransactionContext tc, int nChequeBookID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Cheque WHERE ChequeBookID=%n ORDER BY ChequeID ASC", nChequeBookID);
        }
        public static IDataReader Gets(TransactionContext tc, int nChequeBookID, int eSealed)
        {
            return tc.ExecuteReader("SELECT * FROM View_Cheque WHERE ChequeBookID=" + nChequeBookID + " AND ChequeStatus<" + eSealed + " ORDER BY ChequeID ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByChequeNo(TransactionContext tc, string sChequeNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_Cheque WHERE ChequeNo LIKE '%" + sChequeNo + "%' ORDER BY ChequeNo ASC");
        }


        #endregion
    }  
}
