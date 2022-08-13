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
	public class ReportConfigureDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, ReportConfigure oReportConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_ReportConfigure]"
									+"%n,%n,%s,%s,%n,%n",
									oReportConfigure.ReportConfigureID,oReportConfigure.UserID,oReportConfigure.FieldNames,oReportConfigure.CaptionNames,(int)oReportConfigure.ReportType, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, ReportConfigure oReportConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_ReportConfigure]"
                                    + "%n,%n,%s,%s,%n,%n",
                                    oReportConfigure.ReportConfigureID, oReportConfigure.UserID, oReportConfigure.FieldNames, oReportConfigure.CaptionNames, (int)oReportConfigure.ReportType, (int)eEnumDBOperation);
        }
		#endregion

		#region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nReportType, long nUserID)
		{
            return tc.ExecuteReader("SELECT top 1 * FROM ReportConfigure WHERE UserID=%n  AND ReportType = %n", nUserID, nReportType);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM ReportConfigure");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
