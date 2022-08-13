using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricBatchQCFaultDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricBatchQCFault oFabricBatchQCFault, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchQCFault]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %d, %s",
                                    oFabricBatchQCFault.FBQCFaultID,
                                    oFabricBatchQCFault.FBQCDetailID,
                                    oFabricBatchQCFault.FPFID,
                                    oFabricBatchQCFault.FaultPoint,
                                    oFabricBatchQCFault.NoOfFault,
                                    (int)eEnumDBOperation, 
                                    nUserID,
                                    oFabricBatchQCFault.FaultDate,
                                    oFabricBatchQCFault.Remarks);
        }

        public static void Delete(TransactionContext tc, FabricBatchQCFault oFabricBatchQCFault, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchQCFault]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %d, %s",
                                    oFabricBatchQCFault.FBQCFaultID,
                                    oFabricBatchQCFault.FBQCDetailID,
                                    oFabricBatchQCFault.FPFID,
                                    oFabricBatchQCFault.FaultPoint,
                                    oFabricBatchQCFault.NoOfFault,
                                    (int)eEnumDBOperation,
                                    nUserID
                                    , oFabricBatchQCFault.FaultDate
                                    , oFabricBatchQCFault.Remarks);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQCFault");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, long nFabricBatchQCFaultID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQCFault WHERE FBQCFaultID=%n", nFabricBatchQCFaultID);
        }
        #endregion
    }
}
