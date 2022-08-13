using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FabricBatchQCDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricBatchQC oFabricBatchQC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchQC]"
                                    + "%n,%n,%n, %D, %D,%n,%n,%n,%n,%n",
                                    oFabricBatchQC.FBQCID, oFabricBatchQC.FBID, oFabricBatchQC.TotalLength, oFabricBatchQC.QCStartDateTime, oFabricBatchQC.QCEndDateTime, oFabricBatchQC.QCInCharge, oFabricBatchQC.GreyWidth, (int)eEnumDBOperation, nUserID, oFabricBatchQC.FabricBatchLoomID);
        }
        public static void Delete(TransactionContext tc, FabricBatchQC oFabricBatchQC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchQC]"
                                    + "%n,%n,%n, %D, %D,%n,%n,%n,%n,%n",
                                    oFabricBatchQC.FBQCID, oFabricBatchQC.FBID, oFabricBatchQC.TotalLength, oFabricBatchQC.QCStartDateTime, oFabricBatchQC.QCEndDateTime, oFabricBatchQC.QCInCharge, oFabricBatchQC.GreyWidth, (int)eEnumDBOperation, nUserID, oFabricBatchQC.FabricBatchLoomID);
        }
      
     
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQC WHERE FBQCID=%n", nID);
        }
        public static IDataReader GetByBatch(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQC WHERE FBID=%n ", nID);
           
        }
        public static IDataReader GetByProduction(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQC WHERE FabricBatchLoomID=%n ", nID);
        }
        
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

     
        #endregion
    }
}
