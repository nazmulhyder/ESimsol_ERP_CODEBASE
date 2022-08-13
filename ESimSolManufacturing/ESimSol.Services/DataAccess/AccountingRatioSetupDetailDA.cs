using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountingRatioSetupDetailDA
    {
        public AccountingRatioSetupDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountingRatioSetupDetail oAccountingRatioSetupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountingRatioSetupDetail]"
                                    + "%n, %n, %n, %b, %n,%n, %n, %s",
                                    oAccountingRatioSetupDetail.AccountingRatioSetupDetailID, oAccountingRatioSetupDetail.AccountingRatioSetupID, oAccountingRatioSetupDetail.SubGroupID, oAccountingRatioSetupDetail.IsDivisible, (int)oAccountingRatioSetupDetail.RatioComponent, (int)eEnumDBOperation, nUserID, "");
        }

        public static void Delete(TransactionContext tc, AccountingRatioSetupDetail oAccountingRatioSetupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sAccountingRatioSetupDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AccountingRatioSetupDetail]"
                                    + "%n, %n, %n, %b, %n,%n, %n, %s",
                                    oAccountingRatioSetupDetail.AccountingRatioSetupDetailID, oAccountingRatioSetupDetail.AccountingRatioSetupID, oAccountingRatioSetupDetail.SubGroupID, oAccountingRatioSetupDetail.IsDivisible, (int)oAccountingRatioSetupDetail.RatioComponent, (int)eEnumDBOperation, nUserID, sAccountingRatioSetupDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingRatioSetupDetail WHERE AccountingRatioSetupDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingRatioSetupDetail");
        }
        public static IDataReader GetsForARS(TransactionContext tc, int nARSID, bool bIsDivisible)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingRatioSetupDetail AS ARSD WHERE ARSD.AccountingRatioSetupID=%n AND ARSD.IsDivisible=%b", nARSID, bIsDivisible);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_AccountingRatioSetupDetail
        }
      

       

       
        #endregion
    }  
}
