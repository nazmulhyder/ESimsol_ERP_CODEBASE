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
    public class FabricRequisitionRollDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricRequisitionRoll oFabricRequisitionRoll, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricRequisitionRoll]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                   oFabricRequisitionRoll.FabricRequisitionRollID, oFabricRequisitionRoll.FabricRequisitionDetailID, oFabricRequisitionRoll.LotID, oFabricRequisitionRoll.Qty, oFabricRequisitionRoll.FBQCDetailID,oFabricRequisitionRoll.RollNo,oFabricRequisitionRoll.FabricBatchQCLotID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricRequisitionRoll oFabricRequisitionRoll, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricRequisitionRoll]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                   oFabricRequisitionRoll.FabricRequisitionRollID, oFabricRequisitionRoll.FabricRequisitionDetailID, oFabricRequisitionRoll.LotID, oFabricRequisitionRoll.Qty, oFabricRequisitionRoll.FBQCDetailID,oFabricRequisitionRoll.RollNo,oFabricRequisitionRoll.FabricBatchQCLotID, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricRequisitionRoll WHERE FabricRequisitionRollID=%n", nID);
        }
        public static IDataReader GetByDetailID(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricRequisitionRoll WHERE FabricRequisitionDetailID=%n", nID);
        }  
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricRequisitionRoll");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
