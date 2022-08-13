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
	public class OrderStepGroupDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, OrderStepGroup oOrderStepGroup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_OrderStepGroup]"
									+"%n,%s,%s,%n,%n,%n",
									oOrderStepGroup.OrderStepGroupID,oOrderStepGroup.GroupName,oOrderStepGroup.Note, oOrderStepGroup.BUID, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, OrderStepGroup oOrderStepGroup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_OrderStepGroup]"
                                    + "%n,%s,%s,%n,%n,%n",
                                    oOrderStepGroup.OrderStepGroupID, oOrderStepGroup.GroupName, oOrderStepGroup.Note, oOrderStepGroup.BUID, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM OrderStepGroup WHERE OrderStepGroupID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM OrderStepGroup");
		}
        public static IDataReader Gets(TransactionContext tc, int BUID)
		{
            return tc.ExecuteReader("SELECT * FROM OrderStepGroup Where BUID = "+BUID);
		} 
        
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
