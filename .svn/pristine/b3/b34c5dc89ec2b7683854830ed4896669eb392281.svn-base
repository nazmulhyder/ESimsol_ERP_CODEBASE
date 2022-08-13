using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherReportDA
    {
        public VoucherReportDA() { }

        #region Insert Update Delete Function
      

    
        #endregion

        #region Get & Exist Function
     

    
        public static IDataReader Gets_Report(TransactionContext tc, DateTime dStartDate, DateTime dEndDate,int nVoucherTypeID, int nReportType )
        {
            return tc.ExecuteReader("EXEC [SP_RPT_VoucherReport]" + "%d, %d, %n, %n", dStartDate, dEndDate, nVoucherTypeID, nReportType);
        }
        #endregion
    }  
}
