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
	public class PAMDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, PAMDetail oPAMDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDS)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_PAMDetail]"
									+"%n,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n,%s",
									oPAMDetail.PAMDetailID,oPAMDetail.PAMID,oPAMDetail.ColorID,oPAMDetail.MinQuantity,oPAMDetail.Quantity,oPAMDetail.MaxQuantity,oPAMDetail.ConfirmWeek,oPAMDetail.DesignationWeek,oPAMDetail.WearHouseWeek,nUserID, (int)eEnumDBOperation, sDetailIDS);
		}

        public static void Delete(TransactionContext tc, PAMDetail oPAMDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDS)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_PAMDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n,%s",
                                    oPAMDetail.PAMDetailID, oPAMDetail.PAMID, oPAMDetail.ColorID, oPAMDetail.MinQuantity, oPAMDetail.Quantity, oPAMDetail.MaxQuantity, oPAMDetail.ConfirmWeek, oPAMDetail.DesignationWeek, oPAMDetail.WearHouseWeek, nUserID, (int)eEnumDBOperation, sDetailIDS);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PAMDetail WHERE PAMDetailID=%n", nID);
		}
		public static IDataReader Gets(int nPAMID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_PAMDetail WHERE PAMID =%n", nPAMID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
