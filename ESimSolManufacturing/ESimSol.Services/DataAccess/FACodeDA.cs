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
	public class FACodeDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FACode oFACode, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FACode]"
									+"%n,%n,%n,%s,      %n,%n,%s,%n,%n",
                                    oFACode.FACodeID, oFACode.ProductID, oFACode.CodingPartType, oFACode.CodingPartValue, oFACode.ValueLength, oFACode.Sequence, oFACode.Remarks, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FACode oFACode, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FACode]"
                                    + "%n,%n,%n,%s,      %n,%n,%s,%n,%n",
                                    oFACode.FACodeID, oFACode.ProductID, oFACode.CodingPartType, oFACode.CodingPartValue, oFACode.ValueLength, oFACode.Sequence, oFACode.Remarks, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_FACode WHERE FACodeID=%n ORDER BY Sequence", nID);
		}
        internal static IDataReader GetsByProduct(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_FACode WHERE ISNULL(ProductID,0)=%n ORDER BY Sequence", id);
        }
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FACode");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 
		#endregion

    }

}
