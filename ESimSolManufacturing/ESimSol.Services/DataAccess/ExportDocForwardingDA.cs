using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportDocForwardingDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportDocForwarding oExportDocForwarding, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportDocForwarding]"
                                    + "%n, %n, %n, %s, %n,%n, %n, %n,%s",
                                    oExportDocForwarding.ExportDocForwardingID, oExportDocForwarding.ReferenceID, oExportDocForwarding.ExportDocID, oExportDocForwarding.Name_Print, oExportDocForwarding.Copies, (int)oExportDocForwarding.RefType,  nUserID, (int)eEnumDBOperation,"");
        }

        public static void Delete(TransactionContext tc, ExportDocForwarding oExportDocForwarding, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportDocForwarding]"
                                    + "%n, %n, %n, %s, %n,%n, %n, %n,%s",
                                    oExportDocForwarding.ExportDocForwardingID, oExportDocForwarding.ReferenceID, oExportDocForwarding.ExportDocID, oExportDocForwarding.Name_Print, oExportDocForwarding.Copies, (int)oExportDocForwarding.RefType, nUserID, (int)eEnumDBOperation, sIDs);
        }

        public static void DeleteBYExportBillID(TransactionContext tc, ExportDocForwarding oExportDocForwarding)
        {
            tc.ExecuteNonQuery("DELETE FROM ExportDocForwarding WHERE ExportBillID=%n", oExportDocForwarding.ReferenceID);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocForwarding WHERE ExportDocForwardingID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nReferenceID, int nRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocForwarding WHERE ReferenceID=%n AND RefType =%n ORDER BY Sequence ASC", nReferenceID, nRefType);
        }
 
        #endregion
    }
}
