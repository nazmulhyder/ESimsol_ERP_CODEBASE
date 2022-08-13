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
    public class ImportLCReqByExBillDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ImportLCReqByExBillDetail oImportLCReqByExBillDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sImportLCReqByExBillDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCReqByExBillDetail]"
                                   + "%n,%n,%n,%n,%n,%s",
                                   oImportLCReqByExBillDetail.ImportLCReqByExBillDetailID, oImportLCReqByExBillDetail.ImportLCReqByExBillID, oImportLCReqByExBillDetail.ExportBillID, nUserID, (int)eEnumDBOperation, sImportLCReqByExBillDetailIDs);
        }

        public static void Delete(TransactionContext tc, ImportLCReqByExBillDetail oImportLCReqByExBillDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sImportLCReqByExBillDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLCReqByExBillDetail]"
                                   + "%n,%n,%n,%n,%n,%s",
                                   oImportLCReqByExBillDetail.ImportLCReqByExBillDetailID, oImportLCReqByExBillDetail.ImportLCReqByExBillID, oImportLCReqByExBillDetail.ExportBillID, nUserID, (int)eEnumDBOperation, sImportLCReqByExBillDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLCReqByExBillDetail WHERE ImportLCReqByExBillDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nImportLCReqByExBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLCReqByExBillDetail WHERE ImportLCReqByExBillID =%n", nImportLCReqByExBillID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
