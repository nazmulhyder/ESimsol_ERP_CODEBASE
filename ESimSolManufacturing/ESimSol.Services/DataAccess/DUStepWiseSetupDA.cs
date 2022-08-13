using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class DUStepWiseSetupDA
    {
        public DUStepWiseSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUStepWiseSetup oDUStepWiseSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_DUStepWiseSetup]"
                                    + "%n,%n,%n, %s, %n,%n",
                                    oDUStepWiseSetup.DUStepWiseSetupID, oDUStepWiseSetup.DUOrderSetupID, oDUStepWiseSetup.DyeingStepType, oDUStepWiseSetup.Note,  nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUStepWiseSetup oDUStepWiseSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_DUStepWiseSetup]"
                                    + "%n,%n,%n, %s, %n,%n",
                                    oDUStepWiseSetup.DUStepWiseSetupID, oDUStepWiseSetup.DUOrderSetupID, oDUStepWiseSetup.DyeingStepType, oDUStepWiseSetup.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DUStepWiseSetup WHERE DUStepWiseSetupID=%n", nID);
        }
     
        public static IDataReader GetBy(TransactionContext tc, int nDyeingStepType)
        {
            return tc.ExecuteReader("SELECT * FROM DUStepWiseSetup WHERE DyeingStepType=%n", nDyeingStepType);
        }
     
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DUStepWiseSetup");
        }
      
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}