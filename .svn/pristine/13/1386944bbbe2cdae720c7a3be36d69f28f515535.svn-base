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
	public class WorkOrderDetailDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, WorkOrderDetail oWorkOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            return tc.ExecuteReader("EXEC [SP_IUD_WorkOrderDetail]"
                                   + "%n, %n, %n, %n, %n,%n, %n, %s, %s, %n, %n, %n,%s,%s, %s,%s,%s, %n, %n, %s",
                                   oWorkOrderDetail.WorkOrderDetailID, oWorkOrderDetail.WorkOrderID, oWorkOrderDetail.ProductID, oWorkOrderDetail.StyleID, oWorkOrderDetail.ColorID, oWorkOrderDetail.SizeID, oWorkOrderDetail.OrderRecapID, oWorkOrderDetail.Measurement, oWorkOrderDetail.ProductDescription, oWorkOrderDetail.UnitID, oWorkOrderDetail.Qty, oWorkOrderDetail.UnitPrice, oWorkOrderDetail.MCDia, oWorkOrderDetail.FinishDia, oWorkOrderDetail.GSM, oWorkOrderDetail.Remarks, oWorkOrderDetail.Stretch_Length,  nUserID, (int)eEnumDBOperation, sDetailIDs);
        }


		public static void Delete(TransactionContext tc, WorkOrderDetail oWorkOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_WorkOrderDetail]"
                                   + "%n, %n, %n, %n, %n,%n, %n, %s, %s, %n, %n, %n,%s,%s, %s,%s,%s, %n, %n, %s",
                                   oWorkOrderDetail.WorkOrderDetailID, oWorkOrderDetail.WorkOrderID, oWorkOrderDetail.ProductID, oWorkOrderDetail.StyleID, oWorkOrderDetail.ColorID, oWorkOrderDetail.SizeID, oWorkOrderDetail.OrderRecapID, oWorkOrderDetail.Measurement, oWorkOrderDetail.ProductDescription, oWorkOrderDetail.UnitID, oWorkOrderDetail.Qty, oWorkOrderDetail.UnitPrice, oWorkOrderDetail.MCDia, oWorkOrderDetail.FinishDia, oWorkOrderDetail.GSM, oWorkOrderDetail.Remarks, oWorkOrderDetail.Stretch_Length, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }
        
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_WorkOrderDetail WHERE WorkOrderDetailID=%n", nID);
		}
		public static IDataReader Gets(int nORSID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_WorkOrderDetail WHERE WorkOrderID = %n ORDER BY WorkOrderDetailID", nORSID);
		}

        public static IDataReader GetsByLog(int nORSLogID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_WorkOrderDetailLog WHERE WorkOrderLogID = %n",nORSLogID);
		} 
        
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
