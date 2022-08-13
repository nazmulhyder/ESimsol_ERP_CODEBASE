using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class CIStatementSetupDA
    {
        public CIStatementSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CIStatementSetup oCIStatementSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CIStatementSetup]"
                                    + "%n, %n, %n, %s, %n, %n",
                                    oCIStatementSetup.CIStatementSetupID, oCIStatementSetup.CIHeadType, oCIStatementSetup.AccountHeadID, oCIStatementSetup.DisplayCaption, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, CIStatementSetup oCIStatementSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CIStatementSetup]"
                                    + "%n, %n, %n, %s, %n, %n",
                                    oCIStatementSetup.CIStatementSetupID, oCIStatementSetup.CIHeadType, oCIStatementSetup.AccountHeadID, oCIStatementSetup.DisplayCaption, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CIStatementSetup WHERE CIStatementSetupID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CIStatementSetup Order by CIHeadType");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use CIStatementSetup
        }

      
        #endregion
    }  
    
   
}
