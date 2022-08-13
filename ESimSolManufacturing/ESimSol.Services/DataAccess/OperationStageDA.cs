using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
   public class OperationStageDA
    {
       public OperationStageDA() { }

        #region Insert Update Delete Function
       public static IDataReader IUD(TransactionContext tc, OperationStage oOperationStage, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_OperationStage]  %n ,%s ,%n ,%n ,%n",
                                               oOperationStage.OperationStageID,oOperationStage.Name,oOperationStage.OperationStageEnum, nUserID,nDBOperation  );

        }
        #endregion

        #region Get & Exist Function
       public static IDataReader Get(TransactionContext tc, int nOperationStageID)
        {
            return tc.ExecuteReader("SELECT * FROM  OperationStage WHERE OperationStageID=%n", nOperationStageID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM  OperationStage");
        }


        #endregion
    }
}
