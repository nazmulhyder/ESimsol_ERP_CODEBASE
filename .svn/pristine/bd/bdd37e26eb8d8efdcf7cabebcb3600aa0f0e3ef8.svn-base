using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ImportInvChallanDetailDA
    {

        #region 

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ImportInvChallanDetail oImportInvChallanDetail, EnumDBOperation eEnumDBImportInvChallanDetail, Int64 nUserId, string sIIDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportInvChallanDetail]"
                                    + "%n,%n,%n,%n,%s,%n,%n,%n,%s,%n,%n,%s",
                                     oImportInvChallanDetail.ImportInvChallanDetailID,
                                     oImportInvChallanDetail.ImportInvChallanID,
                                     oImportInvChallanDetail.ImportInvoiceID,
                                     oImportInvChallanDetail.ImportPackDetailID,
                                     oImportInvChallanDetail.LotNo,
                                     oImportInvChallanDetail.NumberOfPack,
                                     oImportInvChallanDetail.QtyPerPack,
                                     oImportInvChallanDetail.Qty,
                                     oImportInvChallanDetail.Note,
                                     nUserId,
                                     (int)eEnumDBImportInvChallanDetail,
                                     sIIDetailIDs
                                     );
        }

        public static void Delete(TransactionContext tc, ImportInvChallanDetail oImportInvChallanDetail, EnumDBOperation eEnumDBImportInvChallanDetail, Int64 nUserId, string sIIDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportInvChallanDetail]"
                                    + "%n,%n,%n,%n,%s,%n,%n,%n,%s,%n,%n,%s",
                                        oImportInvChallanDetail.ImportInvChallanDetailID,
                                     oImportInvChallanDetail.ImportInvChallanID,
                                     oImportInvChallanDetail.ImportInvoiceID,
                                     oImportInvChallanDetail.ImportPackDetailID,
                                     oImportInvChallanDetail.LotNo,
                                     oImportInvChallanDetail.NumberOfPack,
                                     oImportInvChallanDetail.QtyPerPack,
                                     oImportInvChallanDetail.Qty,
                                     oImportInvChallanDetail.Note,
                                     nUserId,
                                     (int)eEnumDBImportInvChallanDetail,
                                     sIIDetailIDs
                                     );
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvChallanDetail WHERE ImportInvChallanDetailID=%n", nID);
        }
        public static IDataReader Gets(int nImportInvChallanID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportInvChallanDetail where ImportInvChallanID=%n order by LotNo", nImportInvChallanID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

        #endregion
    }
}
