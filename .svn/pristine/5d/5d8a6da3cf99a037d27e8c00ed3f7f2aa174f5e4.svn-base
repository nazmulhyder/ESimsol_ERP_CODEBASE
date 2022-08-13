using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricProcessDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricProcess oFabricProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricProcess]"
                                    + "%n, %n, %s, %b, %n, %n",
                                    oFabricProcess.FabricProcessID, oFabricProcess.ProcessType, oFabricProcess.Name, oFabricProcess.IsYarnDyed, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricProcess oFabricProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricProcess]"
                                    + "%n, %n, %s, %b, %n, %n",
                                    oFabricProcess.FabricProcessID, oFabricProcess.ProcessType, oFabricProcess.Name, oFabricProcess.IsYarnDyed, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader GetsByFabricNameType(TransactionContext tc, string sName, string eFabricType)
        {
            string sSQL = "SELECT * FROM FabricProcess Where FabricProcessID>0";
            if (eFabricType.Length > 0)
            {
                sSQL = sSQL + " And ProcessType  in(" + eFabricType + ")";
            }
            if (sName.Trim() != "")
            {
                sSQL = sSQL + " And Name LIKE ('%" + sName.Trim() + "%')";
            }
            sSQL = sSQL + " Order by [Name]";

            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricProcess order by ProcessType,Name");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nFabricProcessID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricProcess WHERE FabricProcessID=%n", nFabricProcessID);
        }
        #endregion
    }
}
