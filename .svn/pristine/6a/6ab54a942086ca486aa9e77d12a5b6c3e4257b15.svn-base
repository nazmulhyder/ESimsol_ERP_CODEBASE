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
	public class OrderSheetDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, OrderSheet oOrderSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_OrderSheet]"
                                    + "%n,%s,%n,%d,%n,%n,%n,%n,%n,%s,%n,%s,%n,%n,%n,%n,%n,%d,%n,%n,%n,%n",
                                    oOrderSheet.OrderSheetID, oOrderSheet.PONo, (int)oOrderSheet.OrderSheetStatus, oOrderSheet.OrderDate, oOrderSheet.BUID, (int)oOrderSheet.OrderSheetType, oOrderSheet.ContractorID, oOrderSheet.ContactPersonnelID, oOrderSheet.Priority, oOrderSheet.Note, oOrderSheet.MKTEmpID, oOrderSheet.PartyPONo, oOrderSheet.PaymentType, oOrderSheet.DeliveryTo, oOrderSheet.DeliveryContactPerson, oOrderSheet.CurrencyID, oOrderSheet.BuyerID, oOrderSheet.ExpectedDeliveryDate,oOrderSheet.RateUnit, (int)oOrderSheet.ProductNature,  nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, OrderSheet oOrderSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_OrderSheet]"
                                    + "%n,%s,%n,%d,%n,%n,%n,%n,%n,%s,%n,%s,%n,%n,%n,%n,%n,%d,%n,%n,%n,%n",
                                    oOrderSheet.OrderSheetID, oOrderSheet.PONo, (int)oOrderSheet.OrderSheetStatus, oOrderSheet.OrderDate, oOrderSheet.BUID, (int)oOrderSheet.OrderSheetType, oOrderSheet.ContractorID, oOrderSheet.ContactPersonnelID, oOrderSheet.Priority, oOrderSheet.Note, oOrderSheet.MKTEmpID, oOrderSheet.PartyPONo, oOrderSheet.PaymentType, oOrderSheet.DeliveryTo, oOrderSheet.DeliveryContactPerson, oOrderSheet.CurrencyID, oOrderSheet.BuyerID, oOrderSheet.ExpectedDeliveryDate, oOrderSheet.RateUnit, (int)oOrderSheet.ProductNature, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader ChangeStatus(TransactionContext tc, OrderSheet oOrderSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_OrderSheetOperation]"
                                    + "%n,%n,%n,%s,%n,%n,%n",
                                    0, oOrderSheet.OrderSheetID, (int)oOrderSheet.OrderSheetStatus, oOrderSheet.Note, (int)oOrderSheet.OrderSheetActionType, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader AcceptRevise(TransactionContext tc, OrderSheet oOrderSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptOrderSheetRevise]"
                                   + "%n, %s, %n, %d, %n, %n, %n, %n, %n, %s, %n, %s, %n, %n, %n, %n, %n, %d, %n, %n, %n, %b",
                                   oOrderSheet.OrderSheetID, oOrderSheet.PONo, (int)oOrderSheet.OrderSheetStatus, oOrderSheet.OrderDate, oOrderSheet.BUID, (int)oOrderSheet.OrderSheetType, oOrderSheet.ContractorID, oOrderSheet.ContactPersonnelID, oOrderSheet.Priority, oOrderSheet.Note, oOrderSheet.MKTEmpID, oOrderSheet.PartyPONo, oOrderSheet.PaymentType, oOrderSheet.DeliveryTo, oOrderSheet.DeliveryContactPerson, oOrderSheet.CurrencyID, oOrderSheet.BuyerID, oOrderSheet.ExpectedDeliveryDate, oOrderSheet.RateUnit, (int)oOrderSheet.ProductNature, nUserID, oOrderSheet.IsNewVersion);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_OrderSheet WHERE OrderSheetID=%n", nID);
		}

        public static IDataReader GetByLog(TransactionContext tc, long nLogID)
		{
			return tc.ExecuteReader("SELECT * FROM View_OrderSheetLog WHERE OrderSheetLogID=%n", nLogID);
		}
        
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_OrderSheet ");
		}
        public static IDataReader BUWiseWithProductNatureGets(int nBUID,int nProductNature, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_OrderSheet Where BUID = %n AND ISNULL(ProductNature,0) = %n", nBUID, nProductNature);
		} 

		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
