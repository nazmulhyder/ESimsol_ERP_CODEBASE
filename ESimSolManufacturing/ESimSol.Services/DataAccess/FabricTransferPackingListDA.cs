using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricTransferPackingListDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricTransferPackingList oFabricTransferPackingList, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricTransferPackingList]"
                                    + "%n, %n, %n, %s, %n, %D, %n, %n",
                                    oFabricTransferPackingList.FTPListID, oFabricTransferPackingList.FTNID, oFabricTransferPackingList.FEOID, oFabricTransferPackingList.Note, oFabricTransferPackingList.StoreID, oFabricTransferPackingList.PackingListDate, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricTransferPackingList oFabricTransferPackingList, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricTransferPackingList]"
                                    + "%n, %n, %n, %s, %n, %D, %n, %n",
                                    oFabricTransferPackingList.FTPListID, oFabricTransferPackingList.FTNID, oFabricTransferPackingList.FEOID, oFabricTransferPackingList.Note, oFabricTransferPackingList.StoreID, oFabricTransferPackingList.PackingListDate, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricTransferPackingList WHERE FTNID = 0 ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nFTPListID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricTransferPackingList WHERE FTPListID=%n", nFTPListID);
        }
        public static void Untag(TransactionContext tc, int nFTPListID)
        {
            tc.ExecuteNonQuery("UPDATE FabricTransferPackingList SET FTNID=0 WHERE FTPListID=%n", nFTPListID);
        }
        #endregion
    }
}
