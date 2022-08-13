using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportLCCodeDA
    {
        public ImportLCCodeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportLCCode oImportLCCode, EnumDBOperation eEnumDBImportLCCode, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCCode]" + "%n, %s, %s, %s, %n, %n",
                                    oImportLCCode.ImportLCCodeID, oImportLCCode.LCCode, oImportLCCode.LCNature, oImportLCCode.Remarks, nUserID, (int)eEnumDBImportLCCode);
        }

        public static void Delete(TransactionContext tc, ImportLCCode oImportLCCode, EnumDBOperation eEnumDBImportLCCode, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLCCode]" + "%n, %s, %s, %s, %n, %n",
                                    oImportLCCode.ImportLCCodeID, oImportLCCode.LCCode, oImportLCCode.LCNature, oImportLCCode.Remarks, nUserID, (int)eEnumDBImportLCCode);
        }

        public static IDataReader GetsProductionPlan(TransactionContext tc, ImportLCCode oImportLCCode)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCCode]" + "%n, %s, %s, %s",
                                   oImportLCCode.ImportLCCodeID, oImportLCCode.LCCode, oImportLCCode.LCNature, oImportLCCode.Remarks);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLCCode ORDER BY ImportLCCodeID ASC");
        }


        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLCCode WHERE ImportLCCodeID=%n", nID);
        }
        #endregion       
    }
}
