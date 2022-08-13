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
	public class MerchanidisingReportDA 
	{
		
		#region Get & Exist Function
		public static IDataReader Gets(TransactionContext tc, string sMainSQL, string sTSSQL, string sORSQL, string sCSSQL, string sTAPSQL)
		{
            return tc.ExecuteReader("EXEC [dbo].[SP_GetMerchandisingReport]" + "%s,%s,%s,%s,%s",sMainSQL, sTSSQL, sORSQL, sCSSQL, sTAPSQL );
		} 
		#endregion
	}

}
 