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
	public class PTUUnit2DA 
	{
		#region Insert Update Delete Function

        public static IDataReader UpdateGrace(TransactionContext tc, PTUUnit2 oPTUUnit2, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_UpdtaGrace]"
                                    + "%n,%n,%n",
									oPTUUnit2.PTUUnit2ID,oPTUUnit2.GraceQty,  nUserID);
		}

	

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PTUUnit2 WHERE PTUUnit2ID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int nExportSCID, int nBUID)
		{
            return tc.ExecuteReader("SELECT * FROM View_PTUUnit2 AS HH WHERE HH.ExportSCID=%n AND HH.BUID=%n ORDER BY PTUUnit2ID ASC", nExportSCID, nBUID);
		}
        public static IDataReader WaitFoSubcontractReceivePTU(TransactionContext tc,  int nBUID,  int Productnature)
		{
            return tc.ExecuteReader("SELECT * FROM View_PTUUnit2 WHERE ISNULL(SubContractReadStockQty,0) >0 AND BUID = %n AND ProductNature = %n ORDER BY PTUUnit2ID ASC", nBUID, Productnature);
		} 
        
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
