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
	public class NOASignatoryDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, NOASignatory oNOASignatory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_NOASignatory]"
									+"%n,%n,%n,%n,%n,%n,%b,%n,%n",
                                    oNOASignatory.NOASignatoryID, oNOASignatory.NOAID, oNOASignatory.ApprovalHeadID,oNOASignatory.SLNo, oNOASignatory.ReviseNo, oNOASignatory.RequestTo, oNOASignatory.IsApprove, nUserID, (int)eEnumDBOperation);
		}
		public static void Delete(TransactionContext tc, NOASignatory oNOASignatory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_NOASignatory]"
									+"%n,%n,%n,%n,%n,%n,%b,%n,%n",
                                    oNOASignatory.NOASignatoryID, oNOASignatory.NOAID, oNOASignatory.ApprovalHeadID, oNOASignatory.SLNo, oNOASignatory.ReviseNo, oNOASignatory.RequestTo, oNOASignatory.IsApprove, nUserID, (int)eEnumDBOperation);
		}
        public static void DeleteAll(TransactionContext tc, int nNOAID, string sNOASignatoryID)
        {
            tc.ExecuteNonQuery("DELETE FROM NOASignatory WHERE isnull(ApproveBy,0)=0 and  NOAID=%n and NOASignatoryID not in (%q)", nNOAID, sNOASignatoryID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_NOASignatory WHERE NOASignatoryID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int nNOAID)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOASignatory where NOAID=%n", nNOAID);
        }
        public static IDataReader GetsByLog(TransactionContext tc, int nNOAID)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOASignatoryLog where NOALogID=%n", nNOAID);
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
