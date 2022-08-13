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
	public class PackingListDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, PackingListDetail oPackingListDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID,string sIDS)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_PackingListDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%s",
                                    oPackingListDetail.PackingListDetailID, oPackingListDetail.PackingListID, oPackingListDetail.ColorID, oPackingListDetail.SizeID, oPackingListDetail.Qty, nUserID, (int)eEnumDBOperation, sIDS);
		 }

        public static void Delete(TransactionContext tc, PackingListDetail oPackingListDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDS)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_PackingListDetail]"
                                        + "%n,%n,%n,%n,%n,%n,%n,%s",
                                        oPackingListDetail.PackingListDetailID, oPackingListDetail.PackingListID, oPackingListDetail.ColorID, oPackingListDetail.SizeID, oPackingListDetail.Qty,nUserID, (int)eEnumDBOperation,  sIDS);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PackingListDetail WHERE PackingListDetailID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int nPackingID)
		{
            return tc.ExecuteReader("SELECT * FROM View_PackingListDetail WHERE PackingListID = " + nPackingID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
