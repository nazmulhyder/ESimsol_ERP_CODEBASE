using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    class ExportUPDA
    {
        public ExportUPDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, ExportUP oExportUP, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportUP] %n,%n,%s,%d,%n,%n,%n,%n", oExportUP.ExportUPID, oExportUP.BUID, oExportUP.UPNo, oExportUP.ExportUPDate, (short)oExportUP.UPStatus, oExportUP.ExportUPSetupID, nUserID, nDBOperation);

        }
        public static void Delete(TransactionContext tc, ExportUP oExportUP, int nDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportUP] %n,%n,%s,%d,%n,%n,%n,%n", oExportUP.ExportUPID, oExportUP.BUID, oExportUP.UPNo, oExportUP.ExportUPDate, (short)oExportUP.UPStatus, oExportUP.ExportUPSetupID, nUserID, nDBOperation);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nExportUPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportUP WHERE ExportUPID=%n", nExportUPID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}

