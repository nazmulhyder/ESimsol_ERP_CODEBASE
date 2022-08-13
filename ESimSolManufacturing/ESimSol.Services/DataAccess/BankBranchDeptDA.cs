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

    public class BankBranchDeptDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, BankBranchDept oBankBranchDept, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BankBranchDept]"
                                   + "%n,%n,%n,%n,%n",oBankBranchDept.BankBranchDeptID, oBankBranchDept.OperationalDeptInInt, oBankBranchDept.BankBranchID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, BankBranchDept oBankBranchDept, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BankBranchDept]"+ "%n,%n,%n,%n,%n",oBankBranchDept.BankBranchDeptID, oBankBranchDept.OperationalDeptInInt, oBankBranchDept.BankBranchID, nUserID, (int)eEnumDBOperation);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankBranchDept WHERE BankBranchDeptID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankBranchDept WHERE BankBranchID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }


   
}
