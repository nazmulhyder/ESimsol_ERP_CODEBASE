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
    public class FabricLoomPlanDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricLoomPlanDetail oFabricLoomPlanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricLoomPlanDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricLoomPlanDetail]"
                                   + "%n,%n,%n,%n,%n,%s",
                                   oFabricLoomPlanDetail.FLPDID, oFabricLoomPlanDetail.FLPID, oFabricLoomPlanDetail.FBPBeamID, nUserID, (int)eEnumDBOperation, sFabricLoomPlanDetailIDs);
        }

        public static void Delete(TransactionContext tc, FabricLoomPlanDetail oFabricLoomPlanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricLoomPlanDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricLoomPlanDetail]"
                                   + "%n,%n,%n,%n,%n,%s",
                                   oFabricLoomPlanDetail.FLPDID, oFabricLoomPlanDetail.FLPID, oFabricLoomPlanDetail.FBPBeamID, nUserID, (int)eEnumDBOperation, sFabricLoomPlanDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricLoomPlanDetail WHERE FLPDID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFabricLoomPlanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricLoomPlanDetail WHERE FLPID =%n", nFabricLoomPlanID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void DeleteByID(TransactionContext tc, FabricLoomPlanDetail oFabricLoomPlanDetail)
        {
            tc.ExecuteNonQuery("DELETE FROM FabricLoomPlanDetail WHERE FLPDID=" + oFabricLoomPlanDetail.FLPDID);
        }

        #endregion
    }

}
