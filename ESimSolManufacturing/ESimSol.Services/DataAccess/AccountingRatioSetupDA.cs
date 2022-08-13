using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountingRatioSetupDA
    {
        public AccountingRatioSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountingRatioSetup oAccountingRatioSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountingRatioSetup]"
                                    + "%n, %s, %n, %s, %s, %s,%n, %n, %n",
                                    oAccountingRatioSetup.AccountingRatioSetupID, oAccountingRatioSetup.Name, (int)oAccountingRatioSetup.RatioFormat,oAccountingRatioSetup.DivisibleName,oAccountingRatioSetup.DividerName,oAccountingRatioSetup.Remarks,(int)oAccountingRatioSetup.RatioSetupType,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, AccountingRatioSetup oAccountingRatioSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AccountingRatioSetup]"
                                    + "%n, %s, %n, %s, %s, %s,%n, %n, %n",
                                    oAccountingRatioSetup.AccountingRatioSetupID, oAccountingRatioSetup.Name, (int)oAccountingRatioSetup.RatioFormat, oAccountingRatioSetup.DivisibleName, oAccountingRatioSetup.DividerName, oAccountingRatioSetup.Remarks, (int)oAccountingRatioSetup.RatioSetupType, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM AccountingRatioSetup WHERE AccountingRatioSetupID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM AccountingRatioSetup");
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

       

       
        #endregion
    }  
}
