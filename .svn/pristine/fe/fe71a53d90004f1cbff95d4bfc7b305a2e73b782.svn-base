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
	public class ServiceInvoiceDA 
	{
		#region Insert Update Delete Function
		public static IDataReader InsertUpdate(TransactionContext tc, ServiceInvoice oServiceInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
		    return tc.ExecuteReader("EXEC [SP_IUD_ServiceInvoice]"
                                      + "%n,%n,%n, %n,%n, %n,%n,%n,%n, %d,%s, %n,%n,%n,%n, %n,%n,%n, %s, %s,%n,%n,%n,%n,%n,%n",
                                    oServiceInvoice.ServiceInvoiceID, oServiceInvoice.InvoiceTypeInt,   oServiceInvoice.ServiceInvoiceTypeInt, oServiceInvoice.VehicleRegistrationID, oServiceInvoice.ServiceOrderID,
                                    oServiceInvoice.WorkOrderByID, oServiceInvoice.CustomerID, oServiceInvoice.ContactPersonID, oServiceInvoice.InvoiceStatusInt, oServiceInvoice.ServiceInvoiceDateSt,
                                    oServiceInvoice.KilometerReading, oServiceInvoice.DiscountAmount_Parts, oServiceInvoice.NetAmount_Parts, oServiceInvoice.NetAmount_Payble, oServiceInvoice.LaborCharge_Total, oServiceInvoice.LaborCharge_Discount,
                                    oServiceInvoice.LaborCharge_Vat, oServiceInvoice.LaborCharge_Net, oServiceInvoice.CustomerRemarks, oServiceInvoice.OfficeRemarks, oServiceInvoice.CurrencyID, oServiceInvoice.PartsCharge_Vat, oServiceInvoice.PartsCharge_Net,  oServiceInvoice.ServiceScheduleID,   nUserID, (int)eEnumDBOperation);
		}
		public static void Delete(TransactionContext tc, ServiceInvoice oServiceInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_ServiceInvoice]"
                                      + "%n,%n,%n, %n,%n, %n,%n,%n,%n, %d,%s, %n,%n,%n,%n, %n,%n,%n, %s, %s,%n,%n,%n,%n,%n,%n",
                                    oServiceInvoice.ServiceInvoiceID, oServiceInvoice.InvoiceTypeInt, oServiceInvoice.ServiceInvoiceTypeInt, oServiceInvoice.VehicleRegistrationID, oServiceInvoice.ServiceOrderID,
                                    oServiceInvoice.WorkOrderByID, oServiceInvoice.CustomerID, oServiceInvoice.ContactPersonID, oServiceInvoice.InvoiceStatusInt, oServiceInvoice.ServiceInvoiceDateSt,
                                    oServiceInvoice.KilometerReading, oServiceInvoice.DiscountAmount_Parts, oServiceInvoice.NetAmount_Parts, oServiceInvoice.NetAmount_Payble, oServiceInvoice.LaborCharge_Total, oServiceInvoice.LaborCharge_Discount,
                                    oServiceInvoice.LaborCharge_Vat, oServiceInvoice.LaborCharge_Net, oServiceInvoice.CustomerRemarks, oServiceInvoice.OfficeRemarks, oServiceInvoice.CurrencyID, oServiceInvoice.PartsCharge_Vat, oServiceInvoice.PartsCharge_Net, oServiceInvoice.ServiceScheduleID, nUserID, (int)eEnumDBOperation);
        }
        public static void Approve(TransactionContext tc, ServiceInvoice oServiceInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ServiceInvoice]"
                                      + "%n,%n,%n, %n,%n, %n,%n,%n,%n, %d,%s, %n,%n,%n,%n, %n,%n,%n, %s, %s,%n,%n,%n,%n,%n,%n",
                                    oServiceInvoice.ServiceInvoiceID, oServiceInvoice.InvoiceTypeInt, oServiceInvoice.ServiceInvoiceTypeInt, oServiceInvoice.VehicleRegistrationID, oServiceInvoice.ServiceOrderID,
                                    oServiceInvoice.WorkOrderByID, oServiceInvoice.CustomerID, oServiceInvoice.ContactPersonID, oServiceInvoice.InvoiceStatusInt, oServiceInvoice.ServiceInvoiceDateSt,
                                    oServiceInvoice.KilometerReading, oServiceInvoice.DiscountAmount_Parts, oServiceInvoice.NetAmount_Parts, oServiceInvoice.NetAmount_Payble, oServiceInvoice.LaborCharge_Total, oServiceInvoice.LaborCharge_Discount,
                                    oServiceInvoice.LaborCharge_Vat, oServiceInvoice.LaborCharge_Net, oServiceInvoice.CustomerRemarks, oServiceInvoice.OfficeRemarks, oServiceInvoice.CurrencyID, oServiceInvoice.PartsCharge_Vat, oServiceInvoice.PartsCharge_Net, oServiceInvoice.ServiceScheduleID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Revise(TransactionContext tc, ServiceInvoice oServiceInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptServiceInvoicRevise]"
                                      + "%n,%n, %n,%n,%n, %n,%n,%n,%n, %d,%s, %n,%n,%n,%n, %n,%n,%n, %s, %s,%n,%n,%n,%n,%n",
                                    oServiceInvoice.ServiceInvoiceID, oServiceInvoice.InvoiceTypeInt,  oServiceInvoice.ServiceInvoiceTypeInt, oServiceInvoice.VehicleRegistrationID, oServiceInvoice.ServiceOrderID,
                                    oServiceInvoice.WorkOrderByID, oServiceInvoice.CustomerID, oServiceInvoice.ContactPersonID, oServiceInvoice.InvoiceStatusInt, oServiceInvoice.ServiceInvoiceDateSt,
                                    oServiceInvoice.KilometerReading, oServiceInvoice.DiscountAmount_Parts, oServiceInvoice.NetAmount_Parts, oServiceInvoice.NetAmount_Payble, oServiceInvoice.LaborCharge_Total, oServiceInvoice.LaborCharge_Discount,
                                    oServiceInvoice.LaborCharge_Vat, oServiceInvoice.LaborCharge_Net, oServiceInvoice.CustomerRemarks, oServiceInvoice.OfficeRemarks, oServiceInvoice.CurrencyID, oServiceInvoice.PartsCharge_Vat, oServiceInvoice.PartsCharge_Net, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_ServiceInvoice WHERE ServiceInvoiceID=%n", nID);
		}
        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ServiceInvoiceLog WHERE ServiceInvoiceLogID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ServiceInvoice");
        }
        public static IDataReader GetsServiceInvoiceLog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ServiceInvoiceLog WHERE ServiceInvoiceID=%n", id);
        }
		#endregion
	}

}
