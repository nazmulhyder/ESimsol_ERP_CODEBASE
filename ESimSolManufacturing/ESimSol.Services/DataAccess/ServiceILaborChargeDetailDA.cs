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
	public class ServiceILaborChargeDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, ServiceILaborChargeDetail oServiceILaborChargeDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_ServiceILaborChargeDetail]"
                                    + "%n,%n,%n,%n, %n,%n,%n,	%s,%n,%n,%s",
                                       oServiceILaborChargeDetail.ServiceILaborChargeDetailID, oServiceILaborChargeDetail.ServiceInvoiceID, oServiceILaborChargeDetail.ServiceWorkID, oServiceILaborChargeDetail.ServiceILaborChargeTypeInt,
                                       oServiceILaborChargeDetail.WorkingHour, oServiceILaborChargeDetail.WorkingCost, oServiceILaborChargeDetail.ChargeAmount, oServiceILaborChargeDetail.ChargeDescription, nUserID, (int)eEnumDBOperation, sIDs);
		}

        public static void Delete(TransactionContext tc, ServiceILaborChargeDetail oServiceILaborChargeDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ServiceILaborChargeDetail]"
                                       + "%n,%n,%n,%n, %n,%n,%n,	%s,%n,%n,%s",
                                       oServiceILaborChargeDetail.ServiceILaborChargeDetailID, oServiceILaborChargeDetail.ServiceInvoiceID, oServiceILaborChargeDetail.ServiceWorkID, oServiceILaborChargeDetail.ServiceILaborChargeTypeInt,
                                       oServiceILaborChargeDetail.WorkingHour, oServiceILaborChargeDetail.WorkingCost, oServiceILaborChargeDetail.ChargeAmount, oServiceILaborChargeDetail.ChargeDescription, nUserID, (int)eEnumDBOperation, sIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_ServiceILaborChargeDetail WHERE ServiceILaborChargeDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc, int id)
		{
            return tc.ExecuteReader("SELECT * FROM View_ServiceILaborChargeDetail WHERE ServiceInvoiceID = %n", id);
		}
        public static IDataReader GetsLog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ServiceILaborChargeDetailLog WHERE ServiceInvoiceLogID = %n", id);
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
