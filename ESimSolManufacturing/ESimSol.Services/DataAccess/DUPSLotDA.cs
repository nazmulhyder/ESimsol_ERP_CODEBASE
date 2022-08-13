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
	public class DUPSLotDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, DUPSLot oDUPSLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_DUPSLot]"
                                    + "%n,%n,%n,%n,    %n,%n,%n,%n",
                                    oDUPSLot.DUPSLotID, oDUPSLot.DUPScheduleID, oDUPSLot.DUPScheduleDetailID, oDUPSLot.LotID, oDUPSLot.Qty, oDUPSLot.DODID, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, DUPSLot oDUPSLot, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_DUPSLot]"
                                    + "%n,%n,%n,%n,    %n,%n,%n,%n",
                                    oDUPSLot.DUPSLotID, oDUPSLot.DUPScheduleID, oDUPSLot.DUPScheduleDetailID, oDUPSLot.LotID, oDUPSLot.Qty, oDUPSLot.DODID, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_DUPSLot WHERE DUPSLotID=%n", nID);
		}
        public static IDataReader GetsBy(TransactionContext tc, int nDUPScheduleID)
		{
            return tc.ExecuteReader("SELECT * FROM View_DUPSLot where DUPScheduleID=%n", nDUPScheduleID);
		}
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUPSLot");
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
