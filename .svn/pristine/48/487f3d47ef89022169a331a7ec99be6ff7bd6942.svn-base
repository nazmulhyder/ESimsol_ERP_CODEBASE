using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ExportTRDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportTR oExportTR, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportTR]"
                                    + "%n, %s, %d, %s, %s, %s, %b,%n, %n, %n ",
                                    oExportTR.ExportTRID, oExportTR.TruckReceiptNo, oExportTR.TruckReceiptDate, oExportTR.Carrier, oExportTR.TruckNo, oExportTR.DriverName, oExportTR.Activity, oExportTR.BUID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ExportTR oExportTR, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportTR]"
                                    + "%n, %s, %d, %s, %s, %s, %b,%n, %n, %n ",
                                    oExportTR.ExportTRID, oExportTR.TruckReceiptNo, oExportTR.TruckReceiptDate, oExportTR.Carrier, oExportTR.TruckNo, oExportTR.DriverName, oExportTR.Activity, oExportTR.BUID, (int)eEnumDBOperation, nUserID);
        }
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportTR WHERE ExportTRID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ExportTR");
        }
        public static IDataReader Gets(TransactionContext tc,bool bActivity, int nBUID )
        {
            return tc.ExecuteReader("SELECT * FROM ExportTR WHERE Activity = %b AND BUID = %n", bActivity, nBUID);
        }

        public static IDataReader BUWiseGets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportTR WHERE  BUID = %n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
