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
	public class NOASpecDA 
	{
		#region Insert Update Delete Function

        public static void Delete(TransactionContext tc, string sSQL)
        {
            tc.ExecuteNonQuery(sSQL);
        }

        public static IDataReader InsertUpdate(TransactionContext tc, NOASpec oNOASpec, int nDBOperation, int nUserID, string sSpecIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_NOASpec]"
                                     + "%n,%n,%n,%s,%n,%n,%s",
                                     oNOASpec.NOASpecID, oNOASpec.SpecHeadID, oNOASpec.NOADetailID, oNOASpec.NOADescription, nUserID, nDBOperation, sSpecIDs);
        }


        public static void Delete(TransactionContext tc, NOASpec oNOASpec, int nDBOperation, int nUserID, string sSpecIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_NOASpec]"
                                    + "%n,%n,%n,%s,%n,%n,%s",
                                    oNOASpec.NOASpecID, oNOASpec.SpecHeadID, oNOASpec.NOADetailID, oNOASpec.NOADescription, nUserID, nDBOperation, sSpecIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_NOASpec WHERE NOASpecID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM NOASpec");
		}
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByLog(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        } 

		#endregion
	}

}
