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
    public class FabricPlanDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricPlanDetail oFabricPlanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPlanDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n",
                                   oFabricPlanDetail.FabricPlanDetailID, oFabricPlanDetail.FabricPlanID, oFabricPlanDetail.ColNo, oFabricPlanDetail.EndsCount, oFabricPlanDetail.SLNo, oFabricPlanDetail.RepeatNo, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricPlanDetail oFabricPlanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricPlanDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n",
                                    oFabricPlanDetail.FabricPlanDetailID, oFabricPlanDetail.FabricPlanID, oFabricPlanDetail.ColNo, oFabricPlanDetail.EndsCount, oFabricPlanDetail.SLNo, oFabricPlanDetail.RepeatNo, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader SaveAll(TransactionContext tc, FabricPlanDetail oFabricPlanDetail,  EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPlanDetail]  %n,%n, %n,%n,%n,%n, %n,%n", oFabricPlanDetail.FabricPlanDetailID, oFabricPlanDetail.FabricPlanID, oFabricPlanDetail.ColNo, oFabricPlanDetail.EndsCount, oFabricPlanDetail.SLNo, oFabricPlanDetail.RepeatNo, nUserID, (int)eEnumDBOperation);
        }
        public static void UpdateEnds(TransactionContext tc, int nFabricPlanID)
        {
            tc.ExecuteNonQuery("Update FabricPlanDetail set EndsCount=0 from FabricPlanDetail as TT where FabricPlanID in (Select FabricPlanID from FabricPlan where FabricPlanOrderID in (Select FabricPlanOrderID from FabricPlan where FabricPlanID=%n))", nFabricPlanID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPlanDetail WHERE FabricPlanDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc,int nFabricPlanID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricPlanDetail where FabricPlanID=" + nFabricPlanID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
