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
	public class RawMaterialSourcingDA 
	{

		#region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int OrderRecapID)
		{
            return tc.ExecuteReader("Exec [SP_RawMaterialSourcing] %n", OrderRecapID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
