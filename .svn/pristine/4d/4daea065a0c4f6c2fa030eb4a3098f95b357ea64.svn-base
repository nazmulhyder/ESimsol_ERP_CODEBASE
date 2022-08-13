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
	public class OrderStepGroupDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, OrderStepGroupDetail oOrderStepGroupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_OrderStepGroupDetail]"
                                    + "%n,%n,%n,%n,%n,%n, %s",
                                    oOrderStepGroupDetail.OrderStepGroupDetailID, oOrderStepGroupDetail.OrderStepGroupID, oOrderStepGroupDetail.OrderStepID, oOrderStepGroupDetail.Sequence, nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

        public static void Delete(TransactionContext tc, OrderStepGroupDetail oOrderStepGroupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_OrderStepGroupDetail]"
                                    + "%n,%n,%n,%n,%n,%n, %s",
                                    oOrderStepGroupDetail.OrderStepGroupDetailID, oOrderStepGroupDetail.OrderStepGroupID, oOrderStepGroupDetail.OrderStepID, oOrderStepGroupDetail.Sequence, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

        public static void UpDown(TransactionContext tc, OrderStepGroupDetail oOrderStepGroupDetail, bool bIsREfresh)
        {
            tc.ExecuteNonQuery("EXEC[SP_OrderStepGroup_UPDown]" + "%n,%n,%b,%b", oOrderStepGroupDetail.OrderStepGroupID, oOrderStepGroupDetail.OrderStepGroupDetailID, bIsREfresh, oOrderStepGroupDetail.bIsUp);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_OrderStepGroupDetail WHERE OrderStepGroupDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc, int id)
		{
            return tc.ExecuteReader("SELECT * FROM View_OrderStepGroupDetail Where OrderStepGroupID = " + id + " Order By Sequence");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
