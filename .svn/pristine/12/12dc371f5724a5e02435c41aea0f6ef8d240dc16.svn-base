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
	public class FADepreciationDetailDA 
	{


		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FADepreciationDetail WHERE FADepreciationDetailID=%n", nID);
		}
		public static IDataReader Gets(int id, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FADepreciationDetail WHERE FADepreciationID=%n",id);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
