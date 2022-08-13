using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class COA_ChartOfAccountCostCenterDA
    {
        public COA_ChartOfAccountCostCenterDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, COA_ChartOfAccountCostCenter oCOA_ChartOfAccountCostCenter, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_COA_ChartOfAccountCostCenter]"
                                    + "%n, %s, %n, %n",
                                    oCOA_ChartOfAccountCostCenter.AccountHeadID, oCOA_ChartOfAccountCostCenter.SelectedCostCenterIDs, nUserId, (int)eEnumDBOperation);
        }

        public void Delete(TransactionContext tc, COA_ChartOfAccountCostCenter oCOA_ChartOfAccountCostCenter, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
             tc.ExecuteNonQuery("EXEC [SP_IUD_COA_ChartOfAccountCostCenter]"
                                    + "%n, %s, %n, %n",
                                     oCOA_ChartOfAccountCostCenter.AccountHeadID, oCOA_ChartOfAccountCostCenter.SelectedCostCenterIDs, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_COA_ChartOfAccountCostCenter WHERE CACCID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_COA_ChartOfAccountCostCenter");
        }

        public static IDataReader GetsByCostCente(TransactionContext tc, int nCCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_COA_ChartOfAccountCostCenter WHERE CCID=%n", nCCID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }

}

  
