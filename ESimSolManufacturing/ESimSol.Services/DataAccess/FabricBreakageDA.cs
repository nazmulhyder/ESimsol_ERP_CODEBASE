using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{

    public class FabricBreakageDA
    {
        public FabricBreakageDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricBreakage oFabricBreakage, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBreakage]"
                                    + "%n,  %s,  %n,%n,%n",
                                    oFabricBreakage.FBreakageID,  oFabricBreakage.Name, (int)oFabricBreakage.WeavingProcess, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricBreakage oFabricBreakage, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBreakage]"
                                    + "%n,  %s,  %n,%n,%n",
                                    oFabricBreakage.FBreakageID, oFabricBreakage.Name, (int)oFabricBreakage.WeavingProcess, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricBreakage WHERE FBreakageID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricBreakage");
        }
        public static IDataReader Gets(int eProcess, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricBreakage WHERE WeavingProcess = %n ", eProcess);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
  
}
