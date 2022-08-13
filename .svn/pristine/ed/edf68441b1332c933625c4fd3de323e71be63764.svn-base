using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportQualityDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportQuality oExportQuality, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportQuality]"
                                    + "%n, %s,%b,%n, %n",
                                    oExportQuality.ExportQualityID, oExportQuality.Name, oExportQuality.Activity,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ExportQuality oExportQuality, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportQuality]"
                                    + "%n, %s,%b,%n, %n",
                                       oExportQuality.ExportQualityID, oExportQuality.Name, oExportQuality.Activity, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ExportQuality");
        }
        public static IDataReader GetsActivity(TransactionContext tc, bool bActivity)
        {
            return tc.ExecuteReader("SELECT * FROM ExportQuality where Activity=%b", bActivity);
        }
        public static IDataReader Get(TransactionContext tc, long nExportQualityID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportQuality WHERE ExportQualityID=%n", nExportQualityID);
        }
        #endregion
    }
}
