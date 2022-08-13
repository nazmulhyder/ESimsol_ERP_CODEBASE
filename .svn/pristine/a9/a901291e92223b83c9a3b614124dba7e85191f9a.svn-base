using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
   public  class EmployeeBatchDA
    {
        #region Insert Update Delete Function

       public static IDataReader InsertUpdate(TransactionContext tc, EmployeeBatch oEmployeeBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeBatch]"
                                   + "%n,%s,%s,%s,%d,%s,%n,%n",
                                   oEmployeeBatch.EmployeeBatchID, oEmployeeBatch.BatchNo, oEmployeeBatch.BatchName, oEmployeeBatch.CauseOfCreation, oEmployeeBatch.CreateDate, oEmployeeBatch.Remarks, nUserID, (int)eEnumDBOperation);
        }

       public static void Delete(TransactionContext tc, EmployeeBatch oEmployeeBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeBatch]"
                                    + "%n,%s,%s,%s,%d,%s,%n,%n",
                                  oEmployeeBatch.EmployeeBatchID, oEmployeeBatch.BatchNo, oEmployeeBatch.BatchName, oEmployeeBatch.CauseOfCreation, oEmployeeBatch.CreateDate, oEmployeeBatch.Remarks, nUserID, (int)eEnumDBOperation);
        }


        #endregion
       public static void Approve(TransactionContext tc, EmployeeBatch oEmployeeBatch, Int64 nUserId)
       {
           tc.ExecuteNonQuery("UPDATE EmployeeBatch SET ApprovedBy = %n  WHERE EmployeeBatchID = %n", nUserId, oEmployeeBatch.EmployeeBatchID);
       }
       public static void UndoApprove(TransactionContext tc, EmployeeBatch oEmployeeBatch, Int64 nUserId)
       {
           tc.ExecuteNonQuery("UPDATE EmployeeBatch SET ApprovedBy = 0  WHERE EmployeeBatchID = %n", oEmployeeBatch.EmployeeBatchID);
       }
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeBatch WHERE EmployeeBatchID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
