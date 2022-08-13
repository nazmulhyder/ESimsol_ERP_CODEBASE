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
	public class ProductionOrderDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, ProductionOrder oProductionOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_ProductionOrder]"
                                    + "%n,%s,%n,%n,%d,%n,%n,%n,%s,%n,%n,%n,%d,%n,%n, %n,%n",
                                    oProductionOrder.ProductionOrderID, oProductionOrder.PONo, oProductionOrder.ExportSCID,  (int)oProductionOrder.ProductionOrderStatus, oProductionOrder.OrderDate, oProductionOrder.BUID, oProductionOrder.ContractorID, oProductionOrder.ContactPersonnelID, oProductionOrder.Note,  oProductionOrder.DeliveryTo, oProductionOrder.DeliveryContactPerson,  oProductionOrder.BuyerID, oProductionOrder.ExpectedDeliveryDate, oProductionOrder.MKTEmpID, (int)oProductionOrder.ProductNature,  nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, ProductionOrder oProductionOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionOrder]"
                                    + "%n,%s,%n,%n,%d,%n,%n,%n,%s,%n,%n,%n,%d,%n,%n, %n,%n",
                                    oProductionOrder.ProductionOrderID, oProductionOrder.PONo, oProductionOrder.ExportSCID, (int)oProductionOrder.ProductionOrderStatus, oProductionOrder.OrderDate, oProductionOrder.BUID, oProductionOrder.ContractorID, oProductionOrder.ContactPersonnelID, oProductionOrder.Note, oProductionOrder.DeliveryTo, oProductionOrder.DeliveryContactPerson, oProductionOrder.BuyerID, oProductionOrder.ExpectedDeliveryDate, oProductionOrder.MKTEmpID, (int)oProductionOrder.ProductNature, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader ChangeStatus(TransactionContext tc, ProductionOrder oProductionOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ProductionOrderOperation]"
                                   + "%n,%n,%n,%s,%n,%n,%n",
                                   0,oProductionOrder.ProductionOrderID, (int)oProductionOrder.ProductionOrderStatus, oProductionOrder.Note, (int)oProductionOrder.ProductionOrderActionType, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader AcceptRevise(TransactionContext tc, ProductionOrder oProductionOrder,  Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptProductionOrderRevise]"
                                   + "%n, %s, %n, %n, %d, %n, %n, %n, %s, %n, %n, %n, %d, %n, %n, %n, %b",
                                   oProductionOrder.ProductionOrderID, oProductionOrder.PONo, oProductionOrder.ExportSCID, (int)oProductionOrder.ProductionOrderStatus, oProductionOrder.OrderDate, oProductionOrder.BUID, oProductionOrder.ContractorID, oProductionOrder.ContactPersonnelID, oProductionOrder.Note, oProductionOrder.DeliveryTo, oProductionOrder.DeliveryContactPerson, oProductionOrder.BuyerID, oProductionOrder.ExpectedDeliveryDate, oProductionOrder.MKTEmpID, (int)oProductionOrder.ProductNature, nUserID, oProductionOrder.IsNewVersion);
        }

        public static void SendToProduction(TransactionContext tc, ProductionOrder oProductionOrder, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_SendToProduction]"
                                    + "%n,%n",oProductionOrder.ProductionOrderID,nUserID);
        }
        public static IDataReader DevelopmentRecapSendToProducton(TransactionContext tc, int nProductionOrderID, Int64 nUserID)//use for buying house
        {
            return tc.ExecuteReader("EXEC [Sp_DevelopmentRecapSendToProduction]" + "%n, %n", nProductionOrderID, nUserID);
        }

     
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_ProductionOrder WHERE ProductionOrderID=%n", nID);
		}
        public static IDataReader GetByLog(TransactionContext tc, long nLogID)
		{
			return tc.ExecuteReader("SELECT * FROM View_ProductionOrderLog WHERE ProductionOrderLogID=%n", nLogID);
		}
        
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_ProductionOrder ");
		}
        public static IDataReader BUWithProductNatureWiseGets(int nBUID, int ProductNature, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_ProductionOrder Where BUID = %n AND ISNULL(ProductNature,0) = %n", nBUID,ProductNature);
		} 

		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
