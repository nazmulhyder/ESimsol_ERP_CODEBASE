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

    public class TAPExecutionDA
    {
        public TAPExecutionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TAPExecution oTAPExecution, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TAPExecution]"
                                    + "%n,%n,%n,%s,%n,%n,%n",
                                    oTAPExecution.TAPExecutionID, oTAPExecution.TAPDetailID, oTAPExecution.OrderStepID, oTAPExecution.UpdatedData, nUserID, nUserID, (int)eEnumDBOperation); //here 1st  nUserID used for Update By 
        }

        public static void Delete(TransactionContext tc, TAPExecution oTAPExecution, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TAPExecution]"
                                    + "%n,%n,%n,%s,%n,%n,%n",
                                    oTAPExecution.TAPExecutionID, oTAPExecution.TAPDetailID, oTAPExecution.OrderStepID, oTAPExecution.UpdatedData, nUserID, nUserID, (int)eEnumDBOperation);//here 1st  nUserID used for Update By 
        }
      
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPExecution WHERE TAPExecutionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPExecution ");
        }

        public static IDataReader Gets(TransactionContext tc, int nTAPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPExecution WHERE TAPID = " + nTAPID + " Order By TAPDetailSequence");
        }
        public static IDataReader GetOrderSteps(int nTAPID, bool bIsFollowUp, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Make_OTAPDetail]"
                                     + "%n,%b",nTAPID,bIsFollowUp);

        }

        public static IDataReader Done(TAPExecution oTAPExecution, TransactionContext tc, Int64 nUserID)
        {
            //SP_TAPExecutionDone
            return tc.ExecuteReader("EXEC [SP_TAPExecutionDone]"
                                 + "%n,%n,%n,%n,%n,%n",oTAPExecution.TAPDetailID, oTAPExecution.OrderStepID, oTAPExecution.TAPID, oTAPExecution.RequiredDataTypeInInt, oTAPExecution.TAPExecutionID, nUserID);//here 1st  nUserID used for Update By 

        }
       
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
   

}
