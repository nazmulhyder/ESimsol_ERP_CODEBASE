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
	public class LotSpecDA 
	{
		#region Insert Update Delete Function


        public static void Delete(TransactionContext tc, string sSQL)
        {
            tc.ExecuteNonQuery(sSQL);
        }

        public static IDataReader IUD(TransactionContext tc, LotSpec oLotSpec, short nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_POSpec]"
                                     + "%n,%n,%n,%s,%n,%n",
                                     oLotSpec.LotSpecID, oLotSpec.SpecHeadID, oLotSpec.LotID, oLotSpec.SpecDescription, nUserID, nDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_LotSpec WHERE LotSpecID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM LotSpec");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
