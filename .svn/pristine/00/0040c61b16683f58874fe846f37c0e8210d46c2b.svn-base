using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ChangesEquitySetupDA
    {
        public ChangesEquitySetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ChangesEquitySetup oChangesEquitySetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ChangesEquitySetup]"
                                    + "%n, %n, %s, %n, %n",
                                    oChangesEquitySetup.ChangesEquitySetupID, oChangesEquitySetup.EquityCategoryInt, oChangesEquitySetup.Remarks,nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ChangesEquitySetup oChangesEquitySetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ChangesEquitySetup]"
                                    + "%n, %n, %s, %n, %n",
                                    oChangesEquitySetup.ChangesEquitySetupID, oChangesEquitySetup.EquityCategoryInt, oChangesEquitySetup.Remarks, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ChangesEquitySetup WHERE ChangesEquitySetupID=%n", nID);
        }


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ChangesEquitySetup");
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
