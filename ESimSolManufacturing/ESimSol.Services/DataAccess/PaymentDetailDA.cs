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
	public class PaymentDetailDA 
	{
		#region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PaymentDetail oPaymentDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_PaymentDetail]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n,%n,%n,%s",
                                    oPaymentDetail.PaymentDetailID, oPaymentDetail.PaymentID, oPaymentDetail.ReferenceTypeInInt, oPaymentDetail.ReferenceID, oPaymentDetail.PaymentAmount,oPaymentDetail.Note, oPaymentDetail.DisCount,   oPaymentDetail.AdditionalAmount,  nUserID, (int)eEnumDBOperation,sDetailIDs);
		}
		public static void Delete(TransactionContext tc, PaymentDetail oPaymentDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_PaymentDetail]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n,%n,%n,%s",
                                    oPaymentDetail.PaymentDetailID, oPaymentDetail.PaymentID, oPaymentDetail.ReferenceTypeInInt, oPaymentDetail.ReferenceID, oPaymentDetail.PaymentAmount, oPaymentDetail.Note, oPaymentDetail.DisCount, oPaymentDetail.AdditionalAmount, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PaymentDetail WHERE PaymentDetailID=%n", nID);
		}
        public static IDataReader Gets(int nPaymentID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_PaymentDetail WHERE PaymentID = %n ORDER BY PaymentID", nPaymentID);
		}
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
