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
	public class POSpecDA 
	{
		#region Insert Update Delete Function

        public static void Delete(TransactionContext tc, string sSQL)
        {
            tc.ExecuteNonQuery(sSQL);
        }

        public static IDataReader InsertUpdate(TransactionContext tc, POSpec oPOSpec, int nDBOperation, int nUserID, string sSpecIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_POSpec]"
                                     + "%n,%n,%n,%s,%n,%n,%s",
                                     oPOSpec.POSpecID, oPOSpec.SpecHeadID, oPOSpec.PODetailID, oPOSpec.PODescription, nUserID, nDBOperation, sSpecIDs);
        }


        public static void Delete(TransactionContext tc, POSpec oPOSpec, int nDBOperation, int nUserID, string sSpecIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_POSpec]"
                                    + "%n,%n,%n,%s,%n,%n,%s",
                                    oPOSpec.POSpecID, oPOSpec.SpecHeadID, oPOSpec.PODetailID, oPOSpec.PODescription, nUserID, nDBOperation, sSpecIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_POSpec WHERE POSpecID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM POSpec");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
