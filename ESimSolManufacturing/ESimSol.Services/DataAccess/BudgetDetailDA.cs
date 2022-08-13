using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BudgetDetailDA
    {
        public BudgetDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BudgetDetail oBudgetDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BudgetDetail]"
                                    + "%n, %n, %n, %n, %s, %n, %n, %s",
                                    oBudgetDetail.BudgetDetailID, oBudgetDetail.BudgetID, oBudgetDetail.AccountHeadID,
                                    oBudgetDetail.BudgetAmount, oBudgetDetail.Remarks, (int)eEnumDBOperation, nUserID, sIDs);
        }
        public static void Delete(TransactionContext tc, BudgetDetail oBudgetDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BudgetDetail]"
                                    + "%n, %n, %n, %n, %s, %n, %n, %s",
                                    oBudgetDetail.BudgetDetailID, oBudgetDetail.BudgetID, oBudgetDetail.AccountHeadID,
                                    oBudgetDetail.BudgetAmount, oBudgetDetail.Remarks, (int)eEnumDBOperation, nUserID, sIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_BudgetDetail
        }
        public static IDataReader GetsByBID(TransactionContext tc, int BID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BudgetDetail WHERE BudgetID=" + BID);
        }
        #endregion

    }  
}

