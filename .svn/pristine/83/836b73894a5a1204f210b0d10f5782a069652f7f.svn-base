using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class COA_AccountHeadConfigDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, COA_AccountHeadConfig oCOA_AccountHeadConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_COA_AccountHeadConfig]"
                                    + "%n, %n, %b, %b, %b, %b, %b, %n, %n, %n",
                                    oCOA_AccountHeadConfig.AccountHeadConfigID, oCOA_AccountHeadConfig.AccountHeadID, oCOA_AccountHeadConfig.IsCostCenterApply, oCOA_AccountHeadConfig.IsBillRefApply, oCOA_AccountHeadConfig.IsInventoryApply, oCOA_AccountHeadConfig.IsOrderReferenceApply, oCOA_AccountHeadConfig.IsVoucherReferenceApply, oCOA_AccountHeadConfig.CounterHeadID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, COA_AccountHeadConfig oCOA_AccountHeadConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_COA_AccountHeadConfig]"
                                    + "%n, %n, %b, %b, %b, %b, %b, %n, %n, %n",
                                    oCOA_AccountHeadConfig.AccountHeadConfigID, oCOA_AccountHeadConfig.AccountHeadID, oCOA_AccountHeadConfig.IsCostCenterApply, oCOA_AccountHeadConfig.IsBillRefApply, oCOA_AccountHeadConfig.IsInventoryApply, oCOA_AccountHeadConfig.IsOrderReferenceApply, oCOA_AccountHeadConfig.IsVoucherReferenceApply, oCOA_AccountHeadConfig.CounterHeadID, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nAccountHeadID)
        {
            return tc.ExecuteReader("SELECT * FROM View_COA_AccountHeadConfig WHERE AccountHeadID=%n", nAccountHeadID);
        }
        public static IDataReader Gets(TransactionContext tc, string sChartsOfAccountIDs, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_COA_AccountHeadConfig WHERE AccountHeadID IN (" + sChartsOfAccountIDs + ")");
        }
        public static IDataReader Gets(TransactionContext tc, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_COA_AccountHeadConfig WHERE CompanyID = %n", nCompanyID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)//use View_COA_AccountHeadConfig
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
