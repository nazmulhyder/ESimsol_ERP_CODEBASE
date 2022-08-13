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
	public class PAMDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, PAM oPAM, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_PAM]"
									+"%n,%s,%n,%s,%s,%n,%n",
									oPAM.PAMID,oPAM.PAMNo,oPAM.StyleID,oPAM.ForwardWeek,oPAM.Remarks,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, PAM oPAM, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_PAM]"
                                    + "%n,%s,%n,%s,%s,%n,%n",
									oPAM.PAMID,oPAM.PAMNo,oPAM.StyleID,oPAM.ForwardWeek,oPAM.Remarks,nUserID, (int)eEnumDBOperation);
		}

        public static IDataReader AccepRevise(TransactionContext tc, PAM oPAM, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_PAMAccepRevise]"
                                   + "%n,%s,%n,%s,%s,%n",
                                   oPAM.PAMID, oPAM.PAMNo, oPAM.StyleID,   oPAM.ForwardWeek,  oPAM.Remarks, nUserID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PAM WHERE PAMID=%n", nID);
		}
		public static IDataReader Gets(int nStyleID, TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM View_PAM WHERE StyleID=%n",nStyleID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static int GetPAMID(TransactionContext tc, int nStyleID, string sForwardWeek)
        {
            int nPAMID = 0;
            object oValue = tc.ExecuteScalar("SELECT TOP 1 HH.PAMID FROM PAM AS HH WHERE HH.ForwardWeek = %s AND HH.StyleID = %n", sForwardWeek, nStyleID);
            if (oValue != null)
            {
                nPAMID = Convert.ToInt32(oValue);
            }
            return nPAMID;
        }        
		#endregion
	}

}
