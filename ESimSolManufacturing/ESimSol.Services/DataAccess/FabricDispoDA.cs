using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricDispoDA
    {
        public FabricDispoDA() { }
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricDispo oFabricDispo, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricDispo]"
                                    + "%n, %s, %n, %n, %b,%b, %n, %n, %n",
                                    oFabricDispo.FabricDispoID, oFabricDispo.Code, oFabricDispo.FabricOrderType, oFabricDispo.BusinessUnitType, oFabricDispo.IsReProduction, oFabricDispo.IsYD, oFabricDispo.CodeLength, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, FabricDispo oFabricDispo, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricDispo]"
                                    + "%n, %s, %n, %n, %b,%b, %n, %n, %n",
                                    oFabricDispo.FabricDispoID, oFabricDispo.Code, oFabricDispo.FabricOrderType, oFabricDispo.BusinessUnitType, oFabricDispo.IsReProduction,oFabricDispo.IsYD, oFabricDispo.CodeLength, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricDispo WHERE FabricDispoID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricDispo");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}

