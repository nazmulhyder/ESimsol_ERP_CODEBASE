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
	public class KnitDyeingPTULogDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingPTULog oKnitDyeingPTULog, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingPTULog]" +"%n,%n,%n,%n,%n", nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KnitDyeingPTULog oKnitDyeingPTULog, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingPTULog]" + "%n,%n,%n,%n,%n", nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingPTULog WHERE KnitDyeingPTULogID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int nKnitDyeingPTUID, int eRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingPTULog WHERE KnitDyeingPTUID=%n AND KnitDyeingPTURefType = %n", nKnitDyeingPTUID, eRefType);
        }
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingPTULog");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
