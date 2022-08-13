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
	public class FADepreciationDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FADepreciation oFADepreciation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FADepreciation]"
                                    + "%n,%n,%d,%n,%n,%b",
									oFADepreciation.FADepreciationID,oFADepreciation.BUID,oFADepreciation.DepreciationDate,nUserID, (int)eEnumDBOperation,true);
		}

		public static void Delete(TransactionContext tc, FADepreciation oFADepreciation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_FADepreciation]"
                                    + "%n,%n,%d,%n,%n,%b",
                                    oFADepreciation.FADepreciationID, oFADepreciation.BUID, oFADepreciation.DepreciationDate, nUserID, (int)eEnumDBOperation, true);
        }
        
        public static IDataReader Approval(TransactionContext tc, FADepreciation oFADepreciation, Int64 nUserID)
        {
            if(oFADepreciation.bIsApproved)
            {
                tc.ExecuteNonQuery("Update FADepreciation SET ApprovedBy = " + nUserID + " WHERE FADepreciationID = " + oFADepreciation.FADepreciationID);
            }
            else
            {
                tc.ExecuteNonQuery("Update FADepreciation SET ApprovedBy = 0  WHERE FADepreciationID = " + oFADepreciation.FADepreciationID);
            }
            return tc.ExecuteReader("SELECT * FROM View_FADepreciation WHERE FADepreciationID=%n", oFADepreciation.FADepreciationID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FADepreciation WHERE FADepreciationID=%n", nID);
		}
		
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
