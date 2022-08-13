using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services
{
    public class FabricReturnChallanDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricReturnChallanDetail oFabricReturnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {

            return tc.ExecuteReader("EXEC [SP_IUD_FabricReturnChallanDetail]"
                                    + "%n ,%n, %n,%n,%n,%n ,%n,%s,%n,%n",
                                    oFabricReturnChallanDetail.FabricReturnChallanDetailID,
                                    oFabricReturnChallanDetail.FabricReturnChallanID ,
                                    oFabricReturnChallanDetail.FDCDID,
                                    oFabricReturnChallanDetail.ProductID,
                                    oFabricReturnChallanDetail.LotID,
                                    oFabricReturnChallanDetail.MUnitID,
                                    oFabricReturnChallanDetail.Qty,
                                    "",
                                    nUserID,
                                    (int)eEnumDBOperation
                                   );
        }

        public static void Delete(TransactionContext tc, FabricReturnChallanDetail oFabricReturnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string ids)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricReturnChallanDetail]"
                                    + "%n ,%n, %n,%n,%n,%n ,%n, %s, %n,%n",
                                    oFabricReturnChallanDetail.FabricReturnChallanDetailID,
                                    oFabricReturnChallanDetail.FabricReturnChallanID,
                                    oFabricReturnChallanDetail.FDCDID,
                                    oFabricReturnChallanDetail.ProductID,
                                    oFabricReturnChallanDetail.LotID,
                                    oFabricReturnChallanDetail.MUnitID,
                                    oFabricReturnChallanDetail.Qty,
                                    ids,
                                    nUserID,
                                    (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricReturnChallanDetail WHERE FabricReturnChallanDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricReturnChallanDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nFRCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricReturnChallanDetail WHERE FabricReturnChallanDetailID=%n", nFRCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
