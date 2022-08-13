using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class ExportPITandCClauseDA
    {
        public ExportPITandCClauseDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPITandCClause oExportPITandCClause, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportPITandCClauseID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPITandCClause]" + "%n, %n, %s, %n,%n, %s, %n, %n", 
                oExportPITandCClause.ExportPITandCClauseID, oExportPITandCClause.ExportPIID, oExportPITandCClause.TermsAndCondition, (int)oExportPITandCClause.DocFor,oExportPITandCClause.ExportTnCCaptionID, sExportPITandCClauseID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ExportPITandCClause oExportPITandCClause, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportPITandCClauseID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPITandCClause]" + "%n, %n, %s, %n,%n, %s, %n, %n",
                oExportPITandCClause.ExportPITandCClauseID, oExportPITandCClause.ExportPIID, oExportPITandCClause.TermsAndCondition, (int)oExportPITandCClause.DocFor,oExportPITandCClause.ExportTnCCaptionID, sExportPITandCClauseID, nUserID, (int)eEnumDBOperation);
        }
        public static void DeleteALL(TransactionContext tc, ExportPITandCClause oExportPITandCClause)
        {
            tc.ExecuteNonQuery("Delete from ExportPITandCClause where ExportPIID>0 and ExportPIID=%n", oExportPITandCClause.ExportPIID);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPITandCClause WHERE ExportPITandCClauseID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPITandCClause WHERE ExportPIID = %n", nExportPIID);
        }

        public static IDataReader GetsPILog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPITandCClauseLog WHERE ExportPILogID = %n", id);
        }
        //GetsPILog

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }


}
