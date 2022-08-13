using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;


namespace ESimSol.Services.DataAccess
{
    public class SampleInvoiceApproveDA
    {
        public SampleInvoiceApproveDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate,int nCurrentStatus, int nReportType)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_SampleInvoiceApprove]" + " %d,%d,%n,%n", dStartDate, dEndDate, nCurrentStatus, nReportType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nReportType)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_SampleInvoiceApprove]" + " %s,%n", sSQL, nReportType);
        }
        #endregion
    }  
    
   
}
