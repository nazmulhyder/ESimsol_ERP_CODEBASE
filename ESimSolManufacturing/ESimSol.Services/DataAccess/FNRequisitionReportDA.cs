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
	public class FNRequisitionReportDA 
	{
		#region Get & Exist Function
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
            return tc.ExecuteReader("EXEC [SP_RPT_FNRequisitionDetail] %s,%n", sSQL, 1);
		} 

		#endregion
	}

}
