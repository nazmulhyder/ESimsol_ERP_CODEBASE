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
	public class PreTreatmentStockDA 
	{
	

		#region Get & Exist Function
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}

        public static IDataReader GetsStock(TransactionContext tc, int ProductID, int TreatmentProcess)
        {
            return tc.ExecuteReader("EXEC [SP_Get_FNRequisitionProduct]" + "%n,%n",  ProductID, TreatmentProcess);
        } 

		#endregion
	}

}
