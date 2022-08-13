using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class ExportBillPendingDA
    {
        public ExportBillPendingDA() { }

        #region Get & Exist Function       
        public static IDataReader Gets(TransactionContext tc, string sSQL, EnumReportLayout eEnumReportLayout)
        {
            return tc.ExecuteReader("EXEC [SP_ExportBillPending] %n", (int)eEnumReportLayout);
        }      
        #endregion
    }
}
