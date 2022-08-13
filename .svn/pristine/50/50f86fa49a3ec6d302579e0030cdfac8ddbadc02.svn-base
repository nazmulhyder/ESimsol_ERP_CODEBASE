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
    public class FnOrderExecuteDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FnOrderExecute oFnOrderExecute, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FnOrderExecute]"
                                   + "%n,%n,%n,%n,%d,%n,%n,%s,%n,%n,%n,%n,%s,%n,%n",
                                   oFnOrderExecute.FnOrderExecuteID, oFnOrderExecute.FSCDID, oFnOrderExecute.FNLabdipDetailID, oFnOrderExecute.ShadeID, oFnOrderExecute.IssueDate, oFnOrderExecute.FinishWidth, oFnOrderExecute.NoOfFrame, oFnOrderExecute.DyeType, oFnOrderExecute.Qty, oFnOrderExecute.Percentage, oFnOrderExecute.GreigeWidth, oFnOrderExecute.GL, oFnOrderExecute.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FnOrderExecute oFnOrderExecute, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FnOrderExecute]"
                                    + "%n,%n,%n,%n,%d,%n,%n,%s,%n,%n,%n,%n,%s,%n,%n",
                                    oFnOrderExecute.FnOrderExecuteID, oFnOrderExecute.FSCDID, oFnOrderExecute.FNLabdipDetailID, oFnOrderExecute.ShadeID, oFnOrderExecute.IssueDate, oFnOrderExecute.FinishWidth, oFnOrderExecute.NoOfFrame, oFnOrderExecute.DyeType, oFnOrderExecute.Qty, oFnOrderExecute.Percentage, oFnOrderExecute.GreigeWidth, oFnOrderExecute.GL, oFnOrderExecute.Note, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FnOrderExecute WHERE FnOrderExecuteID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FnOrderExecute");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
