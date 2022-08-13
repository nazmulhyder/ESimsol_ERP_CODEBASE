using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricTransferPackingListDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricTransferPackingListDetail oFabricTransferPackingListDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricTransferPackingListDetail]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oFabricTransferPackingListDetail.FTPLDetailID, oFabricTransferPackingListDetail.FTPListID, oFabricTransferPackingListDetail.LotID, oFabricTransferPackingListDetail.Qty, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, FabricTransferPackingListDetail oFabricTransferPackingListDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricTransferPackingListDetail]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oFabricTransferPackingListDetail.FTPLDetailID, oFabricTransferPackingListDetail.FTPListID, oFabricTransferPackingListDetail.LotID, oFabricTransferPackingListDetail.Qty, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricTransferPackingListDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nFTPListID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricTransferPackingListDetail WHERE FTPListID=%n", nFTPListID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nFTPLDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricTransferPackingListDetail WHERE FTPLDetailID=%n", nFTPLDetailID);
        }
        #endregion
    }
}
