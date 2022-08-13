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
	public class GUPReportSetUpDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, GUPReportSetUp oGUPReportSetUp, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_GUPReportSetUp]"
									+"%n,%n,%n,%n,%n",
									oGUPReportSetUp.GUPReportSetUpID,oGUPReportSetUp.ProductionStepID,oGUPReportSetUp.Sequence,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, GUPReportSetUp oGUPReportSetUp, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_GUPReportSetUp]"
									+"%n,%n,%n,%n,%n",
									oGUPReportSetUp.GUPReportSetUpID,oGUPReportSetUp.ProductionStepID,oGUPReportSetUp.Sequence,nUserID, (int)eEnumDBOperation);
		}
        public static IDataReader UpDown(TransactionContext tc, GUPReportSetUp oGUPReportSetUp, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_GUPReportSetUpUpDown]" + "%n, %n", oGUPReportSetUp.GUPReportSetUpID, oGUPReportSetUp.IsUp);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_GUPReportSetUp WHERE GUPReportSetUpID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_GUPReportSetUp Order By Sequence");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
