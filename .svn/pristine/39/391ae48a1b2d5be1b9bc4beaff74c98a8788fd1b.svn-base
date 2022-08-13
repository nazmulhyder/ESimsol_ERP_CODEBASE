using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class DUDyeingStepDA
    {
        public DUDyeingStepDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUDyeingStep oDUDyeingStep, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_DUDyeingStep]"
                                    + "%n,%n, %s, %s, %s, %n, %n",
                                    oDUDyeingStep.DUDyeingStepID, oDUDyeingStep.DyeingStepType, oDUDyeingStep.Name, oDUDyeingStep.ShortName, oDUDyeingStep.Note,  nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUDyeingStep oDUDyeingStep, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_DUDyeingStep]"
                                    + "%n,%n, %s, %s, %s, %n, %n",
                                    oDUDyeingStep.DUDyeingStepID, oDUDyeingStep.DyeingStepType, oDUDyeingStep.Name, oDUDyeingStep.ShortName, oDUDyeingStep.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DUDyeingStep WHERE DUDyeingStepID=%n", nID);
        }
     
        public static IDataReader GetBy(TransactionContext tc, int nDyeingStepType)
        {
            return tc.ExecuteReader("SELECT * FROM DUDyeingStep WHERE DyeingStepType=%n", nDyeingStepType);
        }
     
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DUDyeingStep");
        }
        public static IDataReader GetsByOrderSetup(TransactionContext tc, string sDUOrderSetupID)
        {
            return tc.ExecuteReader("SELECT * FROM DUDyeingStep where DyeingStepType in (Select DyeingStepType from DUStepWiseSetup where DUOrderSetupID in (%q))", sDUOrderSetupID);
            
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, DUDyeingStep oDUDyeingStep)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DUDyeingStep Set Activity=~Activity WHERE DUDyeingStepID=%n", oDUDyeingStep.DUDyeingStepID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM DUDyeingStep WHERE DUDyeingStepID=%n", oDUDyeingStep.DUDyeingStepID);

        }
    
     
    
        #endregion
    }
}