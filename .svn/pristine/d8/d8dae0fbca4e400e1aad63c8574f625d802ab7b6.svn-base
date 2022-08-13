using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class Export_LDBPDetailDA
    {
        public Export_LDBPDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Export_LDBPDetail oExport_LDBPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sExport_LDBPDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Export_LDBPDetail]" + "%n, %n, %n, %n, %n, %n, %s",
                                    oExport_LDBPDetail.Export_LDBPDetailID, oExport_LDBPDetail.Export_LDBPID, oExport_LDBPDetail.ExportBillID, oExport_LDBPDetail.BUID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, Export_LDBPDetail oExport_LDBPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sExport_LDBPDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Export_LDBPDetail]" + "%n, %n, %n, %n, %n, %n, %s",
                                    oExport_LDBPDetail.Export_LDBPDetailID, oExport_LDBPDetail.Export_LDBPID, oExport_LDBPDetail.ExportBillID, oExport_LDBPDetail.BUID, nUserID, (int)eEnumDBOperation, sExport_LDBPDetailIDs);
        }
        public static IDataReader InsertUpdate_LDBP(TransactionContext tc, Export_LDBPDetail oExport_LDBPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Export_LDBPDetail_Discount]" + "%n, %n, %n,%s,%n, %D, %n, %n,%n,%n,%n",
                                    oExport_LDBPDetail.Export_LDBPDetailID, oExport_LDBPDetail.Export_LDBPID, oExport_LDBPDetail.ExportBillID, oExport_LDBPDetail.LDBPNo, oExport_LDBPDetail.LDBPAmount, oExport_LDBPDetail.LDBPDate, oExport_LDBPDetail.CurrencyID, oExport_LDBPDetail.CCRate, oExport_LDBPDetail.BankAccountIDRecd, nUserID, (int)eEnumDBOperation);
        }
        public static void UpdateState(TransactionContext tc, int nStatus, int nLCBillID)
        {
            tc.ExecuteNonQuery("UPDATE Export_LDBPDetail SET [Status]=%n WHERE LCBillID=%n", nStatus,  nLCBillID);//1for discount
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Export_LDBPDetail WHERE Export_LDBPDetailID=%n", nID);
        }
        public static IDataReader GetBy(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Export_LDBPDetail WHERE ExportBillID=%n", nID);
        }
        public static IDataReader Gets(int nExport_LDBPID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Export_LDBPDetail where Export_LDBPID =%n", nExport_LDBPID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
