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
	public class PackingListDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, PackingList oPackingList, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_PackingList]"
									+"%n,%n,%n,%n,%n,%n,%s,%n,%n,%s,%n,%s,%n,%n",
                                    oPackingList.PackingListID, oPackingList.CIDID, oPackingList.UnitInPack, oPackingList.PackInCarton, oPackingList.QtyInCarton, oPackingList.CartonQty, oPackingList.CartonNo, oPackingList.GrossWeight, oPackingList.NetWeight, oPackingList.CTNMeasurement, oPackingList.TotalVolume, oPackingList.Note, nUserID,(int)eEnumDBOperation);
		 }

		public static void Delete(TransactionContext tc, PackingList oPackingList, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_PackingList]"
                                        + "%n,%n,%n,%n,%n,%n,%s,%n,%n,%s,%n,%s,%n,%n",
                                        oPackingList.PackingListID, oPackingList.CIDID, oPackingList.UnitInPack, oPackingList.PackInCarton, oPackingList.QtyInCarton, oPackingList.CartonQty, oPackingList.CartonNo, oPackingList.GrossWeight, oPackingList.NetWeight, oPackingList.CTNMeasurement, oPackingList.TotalVolume, oPackingList.Note,nUserID, (int)eEnumDBOperation);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PackingList WHERE PackingListID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM PackingList");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
