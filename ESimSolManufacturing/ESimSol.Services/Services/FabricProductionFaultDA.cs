using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricProductionFaultDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricProductionFault oFabricProductionFault, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricProductionFault]"
                                    + "%n, %n, %s, %b, %n, %n",
                                    oFabricProductionFault.FPFID, oFabricProductionFault.FabricFaultType, oFabricProductionFault.Name, oFabricProductionFault.IsActive, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricProductionFault oFabricProductionFault, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricProductionFault]"
                                    + "%n, %n, %s, %b, %n, %n",
                                    oFabricProductionFault.FPFID, oFabricProductionFault.FabricFaultType, oFabricProductionFault.Name, oFabricProductionFault.IsActive, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricProductionFault");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nFPFID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricProductionFault WHERE FPFID=%n", nFPFID);
        }
        public static void ActiveOrInactive(TransactionContext tc, int nFPFID, bool bIsActive)
        {
            bIsActive = (bIsActive == true ? false : true);
            tc.ExecuteNonQuery("UPDATE FabricProductionFault SET IsActive=%b WHERE FPFID=%n", bIsActive, nFPFID);
        }
        #endregion
    }
}
