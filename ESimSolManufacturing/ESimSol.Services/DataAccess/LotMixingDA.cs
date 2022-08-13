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
	public class LotMixingDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, LotMixing oLotMixing, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_LotMixing]"
									+"%n,%s,%n,%d,%n,%d,%s,%n,%n,%n,%n",
									oLotMixing.LotMixingID,oLotMixing.SLNo,oLotMixing.BUID,oLotMixing.CreateDate,oLotMixing.ApproveByID,oLotMixing.ApproveDate,oLotMixing.Remarks,oLotMixing.Percentage, oLotMixing.WorkingUnitID, nUserID, (int)eEnumDBOperation);
		}
        public static void Delete(TransactionContext tc, LotMixing oLotMixing, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LotMixing]"
                                    + "%n,%s,%n,%d,%n,%d,%s,%n,%n,%n,%n",
                                    oLotMixing.LotMixingID, oLotMixing.SLNo, oLotMixing.BUID, oLotMixing.CreateDate, oLotMixing.ApproveByID, oLotMixing.ApproveDate, oLotMixing.Remarks, oLotMixing.Percentage, oLotMixing.WorkingUnitID, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_LotMixing WHERE LotMixingID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_LotMixing");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
