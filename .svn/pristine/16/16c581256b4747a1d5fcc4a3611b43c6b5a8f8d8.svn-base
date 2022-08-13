using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class CostSetupDA
    {
        public CostSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CostSetup oCostSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CostSetup]"
                                    + "%n, %n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n",
                                    oCostSetup.CostSetupID, oCostSetup.CustomsDuty, oCostSetup.RegulatoryDuty, oCostSetup.SupplementaryDuty, oCostSetup.ValueAddedTxt, oCostSetup.AdvanceIncomeTax, oCostSetup.AdvanceTradeVat, oCostSetup.ATVDeductedProfit, oCostSetup.CustomClearingAndInsuranceFee, oCostSetup.MarginRate, oCostSetup.CurrencyRate, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, CostSetup oCostSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CostSetup]"
                                    + "%n, %n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n",
                                    oCostSetup.CostSetupID, oCostSetup.CustomsDuty, oCostSetup.RegulatoryDuty, oCostSetup.SupplementaryDuty, oCostSetup.ValueAddedTxt, oCostSetup.AdvanceIncomeTax, oCostSetup.AdvanceTradeVat, oCostSetup.ATVDeductedProfit, oCostSetup.CustomClearingAndInsuranceFee, oCostSetup.MarginRate, oCostSetup.CurrencyRate, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM CostSetup WHERE CostSetupID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CostSetup");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_CostSetup
        }
        #endregion
    }
}
