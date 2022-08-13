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
	public class FARegisterSummeryDA 
	{
	

		#region Get & Exist Function

        public static IDataReader Gets(string BUIDs, DateTime StartDate, DateTime EndDate, int ProductCategoryID, int ReportLayout, TransactionContext tc)
		{
            return tc.ExecuteReader("EXEC [SP_FARegisterSummery]"
                                    + "%s,%d,%d,%n,%n",
                                    BUIDs, StartDate, EndDate,ProductCategoryID, ReportLayout);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
