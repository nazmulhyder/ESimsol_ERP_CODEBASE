using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportTnCCaptionDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportTnCCaption oExportTnCCaption, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportTnCCaption]"
                                    + "%n, %s,%b, %n,%n, %n",
                                    oExportTnCCaption.ExportTnCCaptionID, oExportTnCCaption.Name, oExportTnCCaption.Activity, oExportTnCCaption.Sequence, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ExportTnCCaption oExportTnCCaption, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportTnCCaption]"
                                    + "%n, %s,%b, %n,%n, %n",
                                      oExportTnCCaption.ExportTnCCaptionID, oExportTnCCaption.Name, oExportTnCCaption.Activity, oExportTnCCaption.Sequence, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ExportTnCCaption");
        }
        public static IDataReader GetsActivity(TransactionContext tc, bool bActivity)
        {
            return tc.ExecuteReader("SELECT * FROM ExportTnCCaption where Activity=%b", bActivity);
        }
        public static IDataReader Get(TransactionContext tc, long nExportTnCCaptionID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportTnCCaption WHERE ExportTnCCaptionID=%n", nExportTnCCaptionID);
        }
        #endregion
    }
}
