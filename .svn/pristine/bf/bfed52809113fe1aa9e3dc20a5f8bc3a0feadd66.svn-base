using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    class ExportUPDetailDA
    {
        public ExportUPDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, ExportUPDetail oEUPD, int nDBOperation, Int64 nUserID, string sExportUPDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportUPDetail] %n, %n, %n, %d, %n, %s, %n, %n, %n, %s", oEUPD.ExportUPDetailID, oEUPD.ExportUPID, oEUPD.ExportUDID, oEUPD.DateofUDReceive, oEUPD.ContractPersonalID, oEUPD.Note, oEUPD.Amount, nUserID, nDBOperation, sExportUPDetailIDs);

        }
        public static void Delete(TransactionContext tc, ExportUPDetail oEUPD, int nDBOperation, Int64 nUserID, string sExportUPDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportUPDetail] %n, %n, %n, %d, %n, %s, %n, %n, %n, %s", oEUPD.ExportUPDetailID, oEUPD.ExportUPID, oEUPD.ExportUDID, oEUPD.DateofUDReceive, oEUPD.ContractPersonalID, oEUPD.Note, oEUPD.Amount, nUserID, nDBOperation, sExportUPDetailIDs);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nExportUPDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportUPDetail WHERE ExportUPDetailID=%n", nExportUPDetailID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
