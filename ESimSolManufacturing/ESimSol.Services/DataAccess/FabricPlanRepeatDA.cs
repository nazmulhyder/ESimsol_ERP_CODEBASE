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
    public class FabricPlanRepeatDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricPlanRepeat oFabricPlanRepeat, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPlanRepeat]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n",
                                   oFabricPlanRepeat.FabricPlanRepeatID,
                                   oFabricPlanRepeat.FabricPlanOrderID,
                                   (int)oFabricPlanRepeat.WarpWeftType,
                                    oFabricPlanRepeat.RepeatNo,
                                   oFabricPlanRepeat.StartCol,
                                   oFabricPlanRepeat.EndCol,                                 
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricPlanRepeat oFabricPlanRepeat, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricPlanRepeat]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n",
                                   oFabricPlanRepeat.FabricPlanRepeatID,
                                   oFabricPlanRepeat.FabricPlanOrderID,
                                   (int)oFabricPlanRepeat.WarpWeftType,
                                    oFabricPlanRepeat.RepeatNo,
                                   oFabricPlanRepeat.StartCol,
                                   oFabricPlanRepeat.EndCol,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPlanRepeat WHERE FabricPlanRepeatID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPlanRepeat");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
