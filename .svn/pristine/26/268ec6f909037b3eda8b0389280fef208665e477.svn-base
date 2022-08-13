using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class FinancialPositionSetupDA
    {
        public FinancialPositionSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FinancialPositionSetup oFinancialPositionSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FinancialPositionSetup]"
                                    + "%n, %n, %n, %n, %n",
                                    oFinancialPositionSetup.FinancialPositionSetupID, oFinancialPositionSetup.AccountHeadID, oFinancialPositionSetup.Sequence, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FinancialPositionSetup oFinancialPositionSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FinancialPositionSetup]"
                                    + "%n, %n, %n, %n, %n",
                                    oFinancialPositionSetup.FinancialPositionSetupID, oFinancialPositionSetup.AccountHeadID, oFinancialPositionSetup.Sequence, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FinancialPositionSetup WHERE FinancialPositionSetupID=%n", nID);
        }

        public static IDataReader Gets( TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FinancialPositionSetup  Order by ComponentID, Sequence");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use FinancialPositionSetup
        }


        #endregion
    }  
    
    
    
   
}
