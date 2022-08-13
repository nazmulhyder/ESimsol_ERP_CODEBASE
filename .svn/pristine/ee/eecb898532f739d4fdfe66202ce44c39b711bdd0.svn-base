using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
   public  class LoanProductRateDA
    {

       public static IDataReader InsertUpdate(TransactionContext tc, LoanProductRate oLoanProductRate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
       {
           return tc.ExecuteReader("EXEC [SP_IUD_LoanProductRate]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%n,%n",
                                    oLoanProductRate.LoanProductRateID, oLoanProductRate.ProductID,oLoanProductRate.BUID, oLoanProductRate.UnitPrice, oLoanProductRate.CurrencyID, oLoanProductRate.MUnitID, oLoanProductRate.Remarks, nUserID, (int)eEnumDBOperation);
       }
       public static void Delete(TransactionContext tc, LoanProductRate oLoanProductRate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
       {
           tc.ExecuteNonQuery("EXEC [SP_IUD_LoanProductRate]"
                                   + "%n,%n,%n,%n,%n,%n,%s,%n,%n",
                                   oLoanProductRate.LoanProductRateID, oLoanProductRate.ProductID,oLoanProductRate.BUID, oLoanProductRate.UnitPrice, oLoanProductRate.CurrencyID, oLoanProductRate.MUnitID, oLoanProductRate.Remarks, nUserID, (int)eEnumDBOperation);
       }
       public static IDataReader Gets(TransactionContext tc, string sSQL)
       {
           return tc.ExecuteReader(sSQL);
       }
       public static IDataReader Get(TransactionContext tc, long nID)
       {
           return tc.ExecuteReader("SELECT * FROM View_LoanProductRate WHERE LoanProductRateID=%n", nID);
       }
    }
}
