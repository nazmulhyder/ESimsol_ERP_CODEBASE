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
	public class WorkOrderDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, WorkOrder oWorkOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_WorkOrder]"
                                    + "%n, %n, %s, %s, %d, %d, %n, %n, %n, %s, %n, %d, %n, %n, %n, %n, %n, %n, %n",
                                    oWorkOrder.WorkOrderID, oWorkOrder.BUID, oWorkOrder.FileNo, oWorkOrder.WorkOrderNo, oWorkOrder.WorkOrderDate, oWorkOrder.ExpectedDeliveryDate, oWorkOrder.WorkOrderStatusInt, oWorkOrder.SupplierID, oWorkOrder.ContactPersonID, oWorkOrder.Note, oWorkOrder.MerchandiserID, oWorkOrder.ApproveDate, oWorkOrder.ApproveBy, oWorkOrder.CurrencyID, oWorkOrder.CRate, oWorkOrder.RateUnit, oWorkOrder.ReviseNo, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, WorkOrder oWorkOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_WorkOrder]"
                                    + "%n, %n, %s, %s, %d, %d, %n, %n, %n, %s, %n, %d, %n, %n, %n, %n, %n, %n, %n",
                                    oWorkOrder.WorkOrderID, oWorkOrder.BUID, oWorkOrder.FileNo, oWorkOrder.WorkOrderNo, oWorkOrder.WorkOrderDate, oWorkOrder.ExpectedDeliveryDate, oWorkOrder.WorkOrderStatusInt, oWorkOrder.SupplierID, oWorkOrder.ContactPersonID, oWorkOrder.Note, oWorkOrder.MerchandiserID, oWorkOrder.ApproveDate, oWorkOrder.ApproveBy, oWorkOrder.CurrencyID, oWorkOrder.CRate, oWorkOrder.RateUnit, oWorkOrder.ReviseNo, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader ChangeStatus(TransactionContext tc, WorkOrder oWorkOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_WorkOrderOperation]"
                                    + "%n,%n,%n,%s,%n,%n,%n",
                                    0, oWorkOrder.WorkOrderID, (int)oWorkOrder.WorkOrderStatus, oWorkOrder.Note, (int)oWorkOrder.WorkOrderActionType, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader AcceptRevise(TransactionContext tc, WorkOrder oWorkOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptWorkOrderRevise]"
                                   + "%n, %n, %s, %s, %d, %d, %n, %n, %n, %s, %n, %d, %n, %n, %n, %n, %n, %n, %b",
                                   oWorkOrder.WorkOrderID, oWorkOrder.BUID, oWorkOrder.FileNo, oWorkOrder.WorkOrderNo, oWorkOrder.WorkOrderDate, oWorkOrder.ExpectedDeliveryDate, oWorkOrder.WorkOrderStatusInt, oWorkOrder.SupplierID, oWorkOrder.ContactPersonID, oWorkOrder.Note, oWorkOrder.MerchandiserID, oWorkOrder.ApproveDate, oWorkOrder.ApproveBy,  oWorkOrder.CurrencyID, oWorkOrder.CRate, oWorkOrder.RateUnit, oWorkOrder.ReviseNo, nUserID, oWorkOrder.IsNewVersion);
        }

        public static void BillDone(TransactionContext tc, WorkOrder oWorkOrder)
        {
            tc.ExecuteNonQuery("Update WorkOrder SET WorkOrderStatus = %n WHERE WorkOrderID = %n", oWorkOrder.WorkOrderStatusInt, oWorkOrder.WorkOrderID);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_WorkOrder WHERE WorkOrderID=%n", nID);
		}

        public static IDataReader GetByLog(TransactionContext tc, long nLogID)
		{
			return tc.ExecuteReader("SELECT * FROM View_WorkOrderLog WHERE WorkOrderLogID=%n", nLogID);
		}
        
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_WorkOrder ");
		}        
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
