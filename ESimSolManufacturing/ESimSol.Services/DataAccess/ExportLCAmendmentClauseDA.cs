using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class ExportLCAmendmentClauseDA
    {
        public ExportLCAmendmentClauseDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportLCAmendmentClause oExportLCAmendmentClause, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId, string sExportLCAmendmentClauseID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportLCAmendmentClause]"
                                    + " %n,%n,%s,%n,%n,%s",
                                    oExportLCAmendmentClause.ExportLCAmendmentClauseID,
                                    oExportLCAmendmentClause.ExportLCAmendRequestID,
                                    oExportLCAmendmentClause.Clause,
                                    nUserId,
                                    (int)eEnumDBPurchaseLC,
                                    sExportLCAmendmentClauseID);

        }
        #endregion

   

        #region Delete Function
        public static void Delete(TransactionContext tc, ExportLCAmendmentClause oExportLCAmendmentClause, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId, string sExportLCAmendmentClauseID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportLCAmendmentClause]"
                                    + " %n,%n,%s,%n,%n,%s",
                                    oExportLCAmendmentClause.ExportLCAmendmentClauseID,
                                    oExportLCAmendmentClause.ExportLCAmendRequestID,
                                    oExportLCAmendmentClause.Clause,
                                    nUserId,
                                    (int)eEnumDBPurchaseLC,
                                    sExportLCAmendmentClauseID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ExportLCAmendmentClause", "ExportLCAmendmentClauseID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int nPLCAID)
        {
            return tc.ExecuteReader("SELECT * from ExportLCAmendmentClause where ExportLCAmendRequestID=%n ", nPLCAID);
        }

        #endregion
    }


}
