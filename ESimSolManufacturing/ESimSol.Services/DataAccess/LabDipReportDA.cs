using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LabDipReportDA
    {
        public LabDipReportDA() { }

        #region Generation Function

        #endregion

        #region Get & Exist Function
     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabDipReport " + sSQL);
        }
        public static IDataReader Gets_Product(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabdipReport_Product " + sSQL);
        }
        public static IDataReader GetsSql(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader( sSQL);
        }
        #endregion

    }
}