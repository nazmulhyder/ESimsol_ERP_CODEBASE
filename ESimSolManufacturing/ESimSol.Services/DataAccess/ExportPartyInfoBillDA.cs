using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportPartyInfoBillDA
    {
        public ExportPartyInfoBillDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPartyInfoBill oExportPartyInfoBill, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPartyInfoBill]" + "%n, %n, %n,%n,%n, %n, %n, %s",
                                    oExportPartyInfoBill.ExportPartyInfoBillID, oExportPartyInfoBill.ReferenceID, oExportPartyInfoBill.ExportPartyInfoID,oExportPartyInfoBill.ExportPartyInfoDetailID, (int)oExportPartyInfoBill.RefType, (int)eEnumDBOperation, nUserID, "");
        }

        public static void Delete(TransactionContext tc, ExportPartyInfoBill oExportPartyInfoBill, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportPartyInfoBillIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPartyInfoBill]" + "%n, %n, %n,%n, %n, %n, %n, %s",
                                    oExportPartyInfoBill.ExportPartyInfoBillID, oExportPartyInfoBill.ReferenceID, oExportPartyInfoBill.ExportPartyInfoID, oExportPartyInfoBill.ExportPartyInfoDetailID, (int)oExportPartyInfoBill.RefType, (int)eEnumDBOperation, nUserID, sExportPartyInfoBillIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoBill WHERE ExportPartyInfoBillID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoBill");
        }
        public static IDataReader GetsByExportLC(TransactionContext tc, int nReferenceID, int nRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoBill WHERE ReferenceID=%n AND RefType=%n", nReferenceID,nRefType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
