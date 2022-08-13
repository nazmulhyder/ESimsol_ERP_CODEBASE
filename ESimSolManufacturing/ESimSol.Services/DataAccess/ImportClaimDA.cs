using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportClaimDA
    {
        public ImportClaimDA() { }

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ImportClaim oImportClaim, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportClaim]"
                                    + "%n,%n,%d,%n,  %s, %s,%n ,%n,%n",
                                    oImportClaim.ImportClaimID,
                                    oImportClaim.ImportInvoiceID,
                                    oImportClaim.IssueDate,
                                    oImportClaim.ClaimReasonID,
                                    oImportClaim.Note,
                                    oImportClaim.SettleBy,
                                    oImportClaim.Amount,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static void Delete(TransactionContext tc, ImportClaim oImportClaim, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportClaim]"
                                   + "%n,%n,%d,%n,  %s, %s,%n,%n,%n",
                                    oImportClaim.ImportClaimID,
                                    oImportClaim.ImportInvoiceID,
                                    oImportClaim.IssueDate,
                                    oImportClaim.ClaimReasonID,
                                    oImportClaim.Note,
                                    oImportClaim.SettleBy,
                                    oImportClaim.Amount,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nImportClaimID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportClaim where ImportClaimID=%n", nImportClaimID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportClaim");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        //public static IDataReader Request(TransactionContext tc, ImportClaim oImportClaim)
        //{
        //    string sSQL1 = SQLParser.MakeSQL("UPDATE ImportClaim Set RequestBy=" + oImportClaim.RequestBy + ", RequestDate= '" + oImportClaim.RequestDate + "' WHERE ImportClaimID=%n", oImportClaim.ImportClaimID);
        //    tc.ExecuteNonQuery(sSQL1);
        //    return tc.ExecuteReader("SELECT * FROM View_ImportClaim WHERE ImportClaimID=%n", oImportClaim.ImportClaimID);
        //}

        //public static IDataReader Approve(TransactionContext tc, ImportClaim oImportClaim)
        //{
        //    string sSQL1 = SQLParser.MakeSQL("UPDATE ImportClaim Set ApproveBy=" + oImportClaim.ApproveBy + ", ApproveDate= '" + oImportClaim.ApproveDate + "', SettleBy= " + oImportClaim.SettleBy + " WHERE ImportClaimID=%n", oImportClaim.ImportClaimID);
        //    tc.ExecuteNonQuery(sSQL1);
        //    return tc.ExecuteReader("SELECT * FROM View_ImportClaim WHERE ImportClaimID=%n", oImportClaim.ImportClaimID);
        //}

        public static IDataReader Request(TransactionContext tc, ImportClaim oImportClaim, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportClaim]"
                                    + "%n,%n,%d,%n,  %s, %s,%n,%n,%n",
                                    oImportClaim.ImportClaimID,
                                    oImportClaim.ImportInvoiceID,
                                    oImportClaim.IssueDate,
                                    oImportClaim.ClaimReasonID,
                                    oImportClaim.Note,
                                    oImportClaim.SettleBy,
                                     oImportClaim.Amount,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static IDataReader Approve(TransactionContext tc, ImportClaim oImportClaim, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportClaim]"
                                    + "%n,%n,%d,%n,  %s, %s,%n, %n,%n",
                                    oImportClaim.ImportClaimID,
                                    oImportClaim.ImportInvoiceID,
                                    oImportClaim.IssueDate,
                                    oImportClaim.ClaimReasonID,
                                    oImportClaim.Note,
                                    oImportClaim.SettleBy,
                                     oImportClaim.Amount,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        #endregion
    }

}
