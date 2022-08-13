using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SalesCommissionPayableDA
    {
        public SalesCommissionPayableDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SalesCommissionPayable oSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalesCommissionPayable]" + "%n, %n, %n, %n, %n, %b, %n, %n, %n, %n",
                                    oSC.SalesCommissionPayableID, oSC.ExportPIID, oSC.ContactPersonnelID, oSC.Percentage, oSC.CommissionAmount, oSC.Status, oSC.Percentage,nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, SalesCommissionPayable oSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SalesCommissionPayable]" + "%n, %n, %n, %n, %n, %b, %n, %n, %n, %n",
                                      oSC.SalesCommissionPayableID, oSC.ExportPIID, oSC.ContactPersonnelID, oSC.Percentage, oSC.CommissionAmount, oSC.Status, oSC.Percentage, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader PayableApproval(TransactionContext tc, SalesCommissionPayable oSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalesCommission_Payable_Approval]" + "%n ,%n,%n, %n, %n ,%n ,%n,%n ,%n,%n",
                                    oSC.ExportBillID, oSC.SalesCommissionPayableID, oSC.ContactPersonnelID, oSC.Status_Payable, oSC.CurrencyID_BC, oSC.CRate, oSC.AdjOverdueAmount, oSC.AdjPayable, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader VoucherEffect(TransactionContext tc, SalesCommissionPayable oSalesCommissionPayable)
        {
            string sSQL1 = "";
            if (oSalesCommissionPayable.ExportBillID > 0)
            { sSQL1 = SQLParser.MakeSQL("Update ExportBill Set IsWillVoucherEffect=~ISNULL(IsWillVoucherEffect ,0) WHERE ExportBillID=%n", oSalesCommissionPayable.ExportBillID); }
        

            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT TOP 1 * FROM View_SalesCommissionPayable WHERE ExportBillID=%n", oSalesCommissionPayable.ExportBillID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesCommissionPayable WHERE SalesCommissionPayableID=%n", nID);
        }
     

        public static IDataReader Gets(TransactionContext tc,int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesCommissionPayable where ExportPIID=%n", nExportPIID);
        }

     

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


     
   

     
       
        #endregion
    }
}
