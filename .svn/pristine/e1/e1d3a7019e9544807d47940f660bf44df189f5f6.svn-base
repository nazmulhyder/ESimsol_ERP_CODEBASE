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
	public class ServiceInvoiceDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, ServiceInvoiceDetail oServiceInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_ServiceInvoiceDetail]"
                                    + "%n,%n,%n,%n, %s,%d, %n,%n,%n, %s, %n, %n,%n,%s",
                                       oServiceInvoiceDetail.ServiceInvoiceDetailID, oServiceInvoiceDetail.ServiceInvoiceID, oServiceInvoiceDetail.VehiclePartsID, oServiceInvoiceDetail.MUnitID,
                                       oServiceInvoiceDetail.PartsNo, oServiceInvoiceDetail.WorkDateSt, oServiceInvoiceDetail.Qty, oServiceInvoiceDetail.UnitPrice, oServiceInvoiceDetail.Amount, oServiceInvoiceDetail.Remarks, oServiceInvoiceDetail.WorkChargeTypeInt, nUserID, (int)eEnumDBOperation, sIDs);
		}

        public static void Delete(TransactionContext tc, ServiceInvoiceDetail oServiceInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ServiceInvoiceDetail]"
                                   + "%n,%n,%n,%n, %s,%d, %n,%n,%n, %s, %n, %n,%n,%s",
                                       oServiceInvoiceDetail.ServiceInvoiceDetailID, oServiceInvoiceDetail.ServiceInvoiceID, oServiceInvoiceDetail.VehiclePartsID, oServiceInvoiceDetail.MUnitID,
                                       oServiceInvoiceDetail.PartsNo, oServiceInvoiceDetail.WorkDateSt, oServiceInvoiceDetail.Qty, oServiceInvoiceDetail.UnitPrice, oServiceInvoiceDetail.Amount, oServiceInvoiceDetail.Remarks, oServiceInvoiceDetail.WorkChargeTypeInt, nUserID, (int)eEnumDBOperation, sIDs);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_ServiceInvoiceDetail WHERE ServiceInvoiceDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc, int id)
		{
            return tc.ExecuteReader("SELECT * FROM View_ServiceInvoiceDetail WHERE ServiceInvoiceID = %n", id);
		}
        public static IDataReader GetsLog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ServiceInvoiceDetailLog WHERE ServiceInvoiceLogID = %n", id);
        }
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
