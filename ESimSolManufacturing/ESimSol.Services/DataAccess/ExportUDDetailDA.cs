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
    public class ExportUDDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ExportUDDetail oExportUDDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportUDDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportUDDetail]"
                                   + "%n,%n,%n,%n,%n,%s",
                                   oExportUDDetail.ExportUDDetailID, oExportUDDetail.ExportUDID, oExportUDDetail.ExportPIID, nUserID, (int)eEnumDBOperation, sExportUDDetailIDs);
        }

        public static void Delete(TransactionContext tc, ExportUDDetail oExportUDDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportUDDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportUDDetail]"
                                    + "%n,%n,%n,%n,%n,%s",
                                    oExportUDDetail.ExportUDDetailID, oExportUDDetail.ExportUDID, oExportUDDetail.ExportPIID, nUserID, (int)eEnumDBOperation, sExportUDDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportUDDetail WHERE ExportUDDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ExportUDDetail");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(TransactionContext tc, long nExportUDID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportUDDetail WHERE ExportUDID=%n", nExportUDID);
        }

        #endregion
    }

}
