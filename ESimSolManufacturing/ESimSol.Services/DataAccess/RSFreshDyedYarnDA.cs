using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RSFreshDyedYarnDA
    {
        public RSFreshDyedYarnDA() { }

        #region Generation Function

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nOrderType, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_FreshDyedYarnReceive] '" + dStartDate.ToString("dd MMM yyyy HH:00") + "','" + dEndDate.ToString("dd MMM yyyy HH:00") + "'," + nOrderType + "," + nReportType);
        }
        public static IDataReader Gets(TransactionContext tc,string sSQL, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_FreshDyedYarnReceive] %s, %n",sSQL ,nReportType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, int cboQCdate,DateTime dStartDate, DateTime dEndDate,int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_FreshDyedYarnReceive] %s,%n,%D,%D, %n", sSQL,cboQCdate,dStartDate,dEndDate, nReportType);
        }
        public static IDataReader Gets_Product(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("SELECT * FROM View_RSFreshDyedYarn_Product " + sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_RouteSheetQC]'" + sSQL+"'");
        }
       
        public static IDataReader GetsLoadUnload(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_RSLoadUnLoad]'" + sSQL + "'");
        }
        #endregion
    }
}