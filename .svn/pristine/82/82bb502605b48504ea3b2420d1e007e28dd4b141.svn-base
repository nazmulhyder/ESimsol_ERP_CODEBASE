using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportMasterLCDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportMasterLC oImportMasterLC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportMasterLC]"
                                    + "%n,%n,%n, %s,%D,%n, %n",
                                    oImportMasterLC.ImportMasterLCID, oImportMasterLC.ImportLCID, oImportMasterLC.MasterLCID,  oImportMasterLC.MasterLCNo, oImportMasterLC.MasterLCDate, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ImportMasterLC oImportMasterLC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportMasterLC]"
                                    + "%n,%n,%n, %s,%D,%n, %n",
                                        oImportMasterLC.ImportMasterLCID, oImportMasterLC.ImportLCID, oImportMasterLC.MasterLCID, oImportMasterLC.MasterLCNo, oImportMasterLC.MasterLCDate, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportMasterLC");
        }
        public static IDataReader Gets(TransactionContext tc, int nImportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportMasterLC where ImportLCID=%n", nImportLCID);
        }
        public static IDataReader Get(TransactionContext tc, long nImportMasterLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportMasterLC WHERE ImportMasterLCID=%n", nImportMasterLCID);
        }
        #endregion
    }
}
