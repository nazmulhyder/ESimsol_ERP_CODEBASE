using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SalesCommissionDA
    {
        public SalesCommissionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SalesCommission oSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalesCommission]" + "%n, %n, %n, %n,%n, %n, %b, %n, %n,%n, %n, %n",
                                    oSC.SalesCommissionID, oSC.ExportPIID, oSC.ContactPersonnelID,oSC.ContractorID, oSC.Percentage, oSC.CommissionAmount, oSC.Activity, oSC.Status, oSC.Percentage_Maturity, oSC.CommissionOn,nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, SalesCommission oSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SalesCommission]" + "%n, %n, %n, %n,%n, %n, %b, %n, %n,%n, %n, %n",
                                     oSC.SalesCommissionID, oSC.ExportPIID, oSC.ContactPersonnelID, oSC.ContractorID, oSC.Percentage, oSC.CommissionAmount, oSC.Activity, oSC.Status, oSC.Percentage_Maturity, oSC.CommissionOn, nUserID, (int)eEnumDBOperation);
        }

        public static int GetStatus(TransactionContext tc, int nExportPIID)
        {
            object obj = tc.ExecuteScalar("SELECT top(1)[status]  FROM [SalesCommission] where ExportPIID=%n", nExportPIID);
            return Convert.ToInt32(obj);
        }
      
      
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesCommission WHERE SalesCommissionID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc,int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesCommission where ExportPIID=%n", nExportPIID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

     
   

     
       
        #endregion
    }
}
