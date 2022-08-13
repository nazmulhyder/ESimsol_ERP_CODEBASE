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
    public class FabricRequisitionDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricRequisitionDetail oFabricRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricRequisitionDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricRequisitionDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%s",
                                   oFabricRequisitionDetail.FabricRequisitionDetailID, oFabricRequisitionDetail.FabricRequisitionID, oFabricRequisitionDetail.FEOSID, oFabricRequisitionDetail.FSCDID, oFabricRequisitionDetail.ReqQty, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, FabricRequisitionDetail oFabricRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricRequisitionDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricRequisitionDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%s",
                                   oFabricRequisitionDetail.FabricRequisitionDetailID, oFabricRequisitionDetail.FabricRequisitionID, oFabricRequisitionDetail.FEOSID, oFabricRequisitionDetail.FSCDID, oFabricRequisitionDetail.ReqQty, nUserID, (int)eEnumDBOperation, sFabricRequisitionDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricRequisitionDetail WHERE FabricRequisitionDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricRequisitionDetail");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricRequisitionDetail WHERE FabricRequisitionID=%n", nID);
        }

        #endregion
    }

}
