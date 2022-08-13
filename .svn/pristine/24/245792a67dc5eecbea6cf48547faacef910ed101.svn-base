using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;


namespace ESimSol.Services.DataAccess
{
    public class PFDistributionDA
    {
        public PFDistributionDA() { }
        public static IDataReader Distribute(TransactionContext tc, int EnumDBOp, int nPFID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_PFDistribute]"
                                    + "%n,%n, %n", EnumDBOp, nPFID, nUserID);
        }

        public static IDataReader Distributes(TransactionContext tc, int EnumDBOp, int nPFID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_PFDistribute]"
                                    + "%n,%n, %n",EnumDBOp, nPFID, nUserID);
        }
        public static void Rollback(TransactionContext tc, int EnumDBOp, int nPFID, Int64 nUserID)
        {
           tc.ExecuteNonQuery("EXEC [SP_Process_PFDistribute]"
                                    + "%n,%n, %n",EnumDBOp, nPFID, nUserID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
    }
}
