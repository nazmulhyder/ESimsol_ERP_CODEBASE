using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportTermsAndConditionDA
    {
        public ExportTermsAndConditionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportTermsAndCondition oExportTermsAndCondition, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportTermsAndCondition]"
                                    + "%n, %n, %s,%s, %n,%b,%n,%n, %n, %n",
                                     oExportTermsAndCondition.ExportTermsAndConditionID, oExportTermsAndCondition.ClauseType, oExportTermsAndCondition.Clause, oExportTermsAndCondition.Note, oExportTermsAndCondition.BUID,oExportTermsAndCondition.Activity, (int)oExportTermsAndCondition.DocFor,oExportTermsAndCondition.ExportTnCCaptionID, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ExportTermsAndCondition oExportTermsAndCondition, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportTermsAndCondition]"
                                    + "%n, %n, %s,%s, %n,%b,%n,%n, %n, %n",
                                     oExportTermsAndCondition.ExportTermsAndConditionID, oExportTermsAndCondition.ClauseType, oExportTermsAndCondition.Clause, oExportTermsAndCondition.Note, oExportTermsAndCondition.BUID, oExportTermsAndCondition.Activity, (int)oExportTermsAndCondition.DocFor,oExportTermsAndCondition.ExportTnCCaptionID, nUserId, (int)eEnumDBOperation);
        }

        public static void ActivatePITandCSetup(TransactionContext tc, ExportTermsAndCondition oExportTermsAndCondition)
        {
            tc.ExecuteNonQuery("Update ExportTermsAndCondition Set Activity=Activity^1 Where ExportTermsAndConditionID=%n", oExportTermsAndCondition.ExportTermsAndConditionID);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nid)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportTermsAndCondition WHERE ExportTermsAndConditionID=%n", nid);
        }


        public static IDataReader GetsByTypeAndBU(string sDocFors, string BUs, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportTermsAndCondition WHERE Activity = 1 AND DocFor IN (" + sDocFors + ") AND BUID IN (" + BUs + ") order by SLNo");
        }

        public static IDataReader Gets(TransactionContext tc, bool bActivity, int nBUID)
        {
            string sql = "SELECT * FROM View_ExportTermsAndCondition Where BUID =%n AND Activity=%b order by SLNo";
            return tc.ExecuteReader(sql,nBUID, bActivity);
        }

        public static IDataReader BUWiseGets(TransactionContext tc, int nBUID)
        {
            string sql = "SELECT * FROM View_ExportTermsAndCondition Where BUID =%n  order by SLNo";
            return tc.ExecuteReader(sql,nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void UpdateSequence(TransactionContext tc, ExportTermsAndCondition oExportTermsAndCondition)
        {
            tc.ExecuteNonQuery("Update ExportTermsAndCondition SET SLNo = %n WHERE ExportTermsAndConditionID = %n", oExportTermsAndCondition.SLNo, oExportTermsAndCondition.ExportTermsAndConditionID);
        }
        #endregion
    }
}
