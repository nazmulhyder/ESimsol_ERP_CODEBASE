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
	public class SpecHeadDA 
	{
		#region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, SpecHead oSpecHead, int eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SpecHead]"
                                    + "%n,%s,%b,%n,%n",
                                    oSpecHead.SpecHeadID, oSpecHead.SpecName, oSpecHead.IsActive, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM SpecHead WHERE SpecHeadID=%n", nID);
		}
	
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
