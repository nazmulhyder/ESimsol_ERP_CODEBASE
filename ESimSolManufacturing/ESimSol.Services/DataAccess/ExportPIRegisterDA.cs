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
    public class ExportPIRegisterDA
    {
        public ExportPIRegisterDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nReportLayout, int nDueOptions)
        {
            return tc.ExecuteReader("EXEC [SP_ExportPIRegister]" + "%s,%n,%n", sSQL, nReportLayout, nDueOptions);
        }      
        #endregion
    }
}
