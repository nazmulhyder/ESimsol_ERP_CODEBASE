using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNExecutionOrderStatusDA
    {
        public FNExecutionOrderStatusDA() { }

        #region Insert Update Delete Function
        public static IDataReader Gets(TransactionContext tc, string sFNExONo ,string sFNEOIDs)
        {
            return tc.ExecuteReader("EXEC [SP_FNExecutionOrderStatus]" + "%s,%s",sFNEOIDs,sFNExONo);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_FNExecutionOrderStatus]'" + sSQL + "'");
        }
     
        #endregion


    }
}
