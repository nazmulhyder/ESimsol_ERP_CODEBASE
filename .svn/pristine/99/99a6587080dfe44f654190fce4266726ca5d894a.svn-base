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
	public class BTMADA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, BTMA oBTMA, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_BTMA]"
									+"%n,%n,%n,%s,%s,%n,%n,%s,%s,%s,%D,%n,%s,%D,%s,%D,%s,%s,%D,%s,%D,%n,%D,%n,%n,%n",
                                    oBTMA.BTMAID, oBTMA.ExportLCID, oBTMA.ExportBillID, oBTMA.BankName, oBTMA.BranchName, oBTMA.Amount, oBTMA.SupplierID, oBTMA.SupplierName, oBTMA.MasterLCNos, oBTMA.MasterLCDates, oBTMA.InvoiceDate, oBTMA.ImportLCID, oBTMA.ImportLCNo, oBTMA.ImportLCDate, oBTMA.GarmentsQty, oBTMA.DeliveryDate, oBTMA.DeliveryChallanNo, oBTMA.MushakNo, oBTMA.MushakDate, oBTMA.GatePassNo, oBTMA.GatePassDate, oBTMA.PrintBy, oBTMA.PrintDate, oBTMA.Amount_ImportLC, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, BTMA oBTMA, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_BTMA]"
                                    + "%n,%n,%n,%s,%s,%n,%n,%s,%s,%s,%D,%n,%s,%D,%s,%D,%s,%s,%D,%s,%D,%n,%D,%n,%n,%n",
                                    oBTMA.BTMAID, oBTMA.ExportLCID, oBTMA.ExportBillID, oBTMA.BankName, oBTMA.BranchName, oBTMA.Amount, oBTMA.SupplierID, oBTMA.SupplierName, oBTMA.MasterLCNos, oBTMA.MasterLCDates, oBTMA.InvoiceDate, oBTMA.ImportLCID, oBTMA.ImportLCNo, oBTMA.ImportLCDate, oBTMA.GarmentsQty, oBTMA.DeliveryDate, oBTMA.DeliveryChallanNo, oBTMA.MushakNo, oBTMA.MushakDate, oBTMA.GatePassNo, oBTMA.GatePassDate, oBTMA.PrintBy, oBTMA.PrintDate, oBTMA.Amount_ImportLC, nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_BTMA WHERE BTMAID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_BTMA");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion

        public static IDataReader Update_PrintBy(TransactionContext tc, BTMA oBTMA, Int64 nUserID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update BTMA Set PrintBy=" + nUserID
                                            + ",  PrintDate='" + DateTime.Now.ToString("dd MMM yyyy")
                                            + "', CertificateNo=" + oBTMA.CertificateNo
                                            + " WHERE BTMAID=%n", oBTMA.BTMAID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_BTMA WHERE BTMAID=%n", oBTMA.BTMAID);

        }

        internal static IDataReader Get_MaxCNo(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT MAX(ISNULL(CertificateNo,0)) AS CertificateNo FROM BTMA");
        }
    }

}
