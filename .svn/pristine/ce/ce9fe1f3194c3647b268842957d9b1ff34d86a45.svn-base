using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class VoucherRefReportDA
    {
        public VoucherRefReportDA() { }

        #region Insert Update Delete Function
   
      
        #endregion

        #region Get & Exist Function
      
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
       
        public static IDataReader GetsVoucherBillBreakDown(TransactionContext tc, VoucherRefReport oGL)
        {
            return tc.ExecuteReader("EXEC [SP_VoucherBillBreakDown]" + " %n, %n, %d, %d, %b, %b, %n, %s", oGL.AccountHeadID, oGL.CCID, oGL.StartDate, oGL.EndDate, oGL.IsApproved, oGL.IsAllBill, oGL.CurrencyID, oGL.BusinessUnitIDs);
        }

        public static IDataReader GetsVoucherBillDetails(TransactionContext tc, VoucherRefReport oGL)
        {
            return tc.ExecuteReader("EXEC [SP_VoucherBillLedger]" + " %n, %d, %d, %b, %n, %n", oGL.VoucherBillID, oGL.StartDate, oGL.EndDate, oGL.IsApproved, oGL.CurrencyID, oGL.BusinessUnitIDs);
        }
       
        #endregion
    }  
   
}
