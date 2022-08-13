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
    public class FabricSizingPlanDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricSizingPlanDetail oFabricSizingPlanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFSPDIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSizingPlanDetail]"
                                   + "%n,%n,%n,%n,  %n,%n,%n",
                                   oFabricSizingPlanDetail.FSPDID, oFabricSizingPlanDetail.FabricSizingPlanID, oFabricSizingPlanDetail.FabricMachineTypeID, oFabricSizingPlanDetail.SizingBeamNo, oFabricSizingPlanDetail.Qty, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricSizingPlanDetail oFabricSizingPlanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSizingPlanDetail]"
                                   + "%n,%n,%n,%n,  %n,%n,%n",
                                   oFabricSizingPlanDetail.FSPDID, oFabricSizingPlanDetail.FabricSizingPlanID, oFabricSizingPlanDetail.FabricMachineTypeID, oFabricSizingPlanDetail.SizingBeamNo, oFabricSizingPlanDetail.Qty, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSizingPlanDetail WHERE FSPDID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFabricSizingPlanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSizingPlanDetail WHERE FabricSizingPlanID =%n", nFabricSizingPlanID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
