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
    public class SalesReportDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SalesReport oSalesReport, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalesReport]"
                                   + "%n,%s,%s,%s,%s,%s,%n,%n,%n,%n,%n,%s,%s,%s,%s,%s,%n,%n",
                                   oSalesReport.SalesReportID,
                                   oSalesReport.Name,
                                   oSalesReport.PrintName,
                                   oSalesReport.Query,
                                   oSalesReport.IDs,
                                   oSalesReport.GrpByQ,
                                   oSalesReport.ReportType,
                                   oSalesReport.BUID,
                                   oSalesReport.Activity,
                                   oSalesReport.ParentID,
                                   oSalesReport.IsDouble,
                                   oSalesReport.AllocationHeader,
                                   oSalesReport.DispoTargetQuery,
                                   oSalesReport.QueryLayerTwo,
                                   oSalesReport.QueryLayerThree,
                                   oSalesReport.Note,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SalesReport oSalesReport, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SalesReport]"
                                   + "%n,%s,%s,%s,%s,%s,%n,%n,%n,%n,%n,%s,%s,%s,%s,%s,%n,%n",
                                   oSalesReport.SalesReportID,
                                   oSalesReport.Name,
                                   oSalesReport.PrintName,
                                   oSalesReport.Query,
                                   oSalesReport.IDs,
                                   oSalesReport.GrpByQ,
                                   oSalesReport.ReportType,
                                   oSalesReport.BUID,
                                   oSalesReport.Activity,
                                   oSalesReport.ParentID,
                                   oSalesReport.IsDouble,
                                   oSalesReport.AllocationHeader,
                                   oSalesReport.DispoTargetQuery,
                                   oSalesReport.QueryLayerTwo,
                                   oSalesReport.QueryLayerThree,
                                   oSalesReport.Note,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesReport WHERE SalesReportID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesReport");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader BUWiseGets(TransactionContext tc, int BUID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportPIPrintSetup WHERE BUID = %n", BUID);
        }
        public static IDataReader GetByParent(TransactionContext tc, long ParentID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesReport WHERE ParentID=%n", ParentID);
        }
        #endregion
    }
}
