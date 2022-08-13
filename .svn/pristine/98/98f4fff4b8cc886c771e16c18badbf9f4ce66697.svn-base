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
    public class FabricPlanOrderDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricPlanOrder oFabricPlanOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPlanOrder]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%s,%s,%n,%n",
                                   oFabricPlanOrder.FabricPlanOrderID, oFabricPlanOrder.RefID, (int)oFabricPlanOrder.RefType, oFabricPlanOrder.ColumnCount, oFabricPlanOrder.Weave, oFabricPlanOrder.Reed, oFabricPlanOrder.Pick, oFabricPlanOrder.GSM, oFabricPlanOrder.Dent, oFabricPlanOrder.Warp, oFabricPlanOrder.Weft, oFabricPlanOrder.RepeatSize, oFabricPlanOrder.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricPlanOrder oFabricPlanOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricPlanOrder]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%s,%s,%n,%n",
                                   oFabricPlanOrder.FabricPlanOrderID, oFabricPlanOrder.RefID, (int)oFabricPlanOrder.RefType, oFabricPlanOrder.ColumnCount, oFabricPlanOrder.Weave, oFabricPlanOrder.Reed, oFabricPlanOrder.Pick, oFabricPlanOrder.GSM, oFabricPlanOrder.Dent, oFabricPlanOrder.Warp, oFabricPlanOrder.Weft, oFabricPlanOrder.RepeatSize, oFabricPlanOrder.Note, nUserID, (int)eEnumDBOperation);
        }
      

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPlanOrder WHERE FabricPlanOrderID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFabricPlanOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPlanOrder where FabricPlanOrderID=" + nFabricPlanOrderID);
        }
        public static IDataReader Gets(TransactionContext tc, int nRefID, int nRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPlanOrder where RefID=%n AND RefType = %n ", nRefID, nRefType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader CopyFabricPlan(TransactionContext tc, FabricPlanOrder oFP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPlanCopy]"
                                   + "%n, %n, %n,%n,%n,%n,%n",
                                   oFP.FabricPlanOrderIDFrom, oFP.FabricPlanOrderID, oFP.RefType, oFP.RefID, oFP.WarpWeftType, nUserID, (int)eEnumDBOperation);
        }

        #endregion
    }

}
