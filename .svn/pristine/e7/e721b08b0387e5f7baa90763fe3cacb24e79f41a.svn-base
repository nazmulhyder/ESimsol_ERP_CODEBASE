using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    class COAChartOfAccountNameAlternativeDA
    {
        public COAChartOfAccountNameAlternativeDA() { }

        #region Inseret Update Delete
        public static IDataReader InsertUpdate(TransactionContext tc, COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_COA_ChartOfAccountNameAlternative]"
                                                 + "%n,                                                                          %n,                                     %s,                                 %s,                           %n,              %n",
                                    oCOAChartOfAccountNameAlternative.AlternativeAccountHeadID, oCOAChartOfAccountNameAlternative.AccountHeadID, oCOAChartOfAccountNameAlternative.Name, oCOAChartOfAccountNameAlternative.Description, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_COA_ChartOfAccountNameAlternative]"
                                   + "%n,                                         %n,                                     %s,                                 %s,                %n,              %n",
                                    oCOAChartOfAccountNameAlternative.AlternativeAccountHeadID, oCOAChartOfAccountNameAlternative.AccountHeadID, oCOAChartOfAccountNameAlternative.Name, oCOAChartOfAccountNameAlternative.Description, nUserID, (int)eEnumDBOperation);
        }

        #endregion


        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("select * from COA_ChartOfAccountNameAlternative");
        }

        public static IDataReader GetsByAccountHeadID(TransactionContext tc, int nAccountHeadID)
        {
            return tc.ExecuteReader("SELECT * FROM COA_ChartOfAccountNameAlternative WHERE AccountHeadID=%n", nAccountHeadID);
        }
        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("select * from View_COAChartOfAccountNameAlternative where COAChartOfAccountNameAlternativeID =%n", id);
        }
        public static IDataReader GetsbyParentID(TransactionContext tc, int nParentID, Int64 nUserId)
        {
            return tc.ExecuteReader("select * from COA_ChartOfAccountNameAlternative where AccountHeadID = %n", nParentID);
        }

        public static IDataReader SearchbyAlternativeName(TransactionContext tc, string Search, int ParentID, Int64 nUserId)
        {
            
            Search = "%" + Search + "%";
            return tc.ExecuteReader("select * from COA_ChartOfAccountNameAlternative where AccountHeadID =%n and Name like %s", ParentID, Search);
            //return tc.ExecuteReader("SELECT * FROM Product WHERE ProductBaseID=%n AND ProductName like %s", nID, sProductName);
           
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
