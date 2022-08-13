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
	public class PISpecDA 
	{
		#region Insert Update Delete Function

        public static void Delete(TransactionContext tc, string sSQL)
        {
            tc.ExecuteNonQuery(sSQL);
        }

        public static IDataReader InsertUpdate(TransactionContext tc, PISpec oPISpec, int nDBOperation, int nUserID, string sSpecIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PISpec]"
                                     + "%n,%n,%n,%s,%n,%n,%s",
                                     oPISpec.PISpecID, oPISpec.SpecHeadID, oPISpec.PIDetailID, oPISpec.PIDescription, nUserID, nDBOperation, sSpecIDs);
        }


        public static void Delete(TransactionContext tc, PISpec oPISpec, int nDBOperation, int nUserID, string sSpecIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PISpec]"
                                    + "%n,%n,%n,%s,%n,%n,%s",
                                    oPISpec.PISpecID, oPISpec.SpecHeadID, oPISpec.PIDetailID, oPISpec.PIDescription, nUserID, nDBOperation, sSpecIDs);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PISpec WHERE PISpecID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM PISpec");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
