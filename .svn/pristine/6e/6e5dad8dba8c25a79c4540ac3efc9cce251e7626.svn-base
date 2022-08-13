using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportBankForwardingDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportBankForwarding oExportBankForwarding, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBankForwarding]"
                                    + "%n, %n, %s, %n,%n, %n, %n,%s",
                                    oExportBankForwarding.ExportBankForwardingID,  oExportBankForwarding.ExportBillID, oExportBankForwarding.Name_Print, oExportBankForwarding.Copies, oExportBankForwarding.Copies_Original,  nUserID, (int)eEnumDBOperation,"");
        }

        public static void Delete(TransactionContext tc, ExportBankForwarding oExportBankForwarding, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportBankForwarding]"
                                    + "%n, %n, %s, %n,%n, %n, %n,%s",
                                    oExportBankForwarding.ExportBankForwardingID, oExportBankForwarding.ExportBillID, oExportBankForwarding.Name_Print, oExportBankForwarding.Copies, oExportBankForwarding.Copies_Original, nUserID, (int)eEnumDBOperation, sIDs);
        }

        public static void DeleteBYExportBillID(TransactionContext tc, ExportBankForwarding oExportBankForwarding)
        {
            tc.ExecuteNonQuery("DELETE FROM ExportBankForwarding WHERE ExportBillID=%n", oExportBankForwarding.ExportBillID);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBankForwarding WHERE ExportBankForwardingID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nExportBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBankForwarding WHERE ExportBillID=%n ", nExportBillID);
        }
 
        #endregion
    }
}
