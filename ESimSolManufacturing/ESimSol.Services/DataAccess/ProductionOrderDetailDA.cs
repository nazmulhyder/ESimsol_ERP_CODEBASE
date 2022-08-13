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
	public class ProductionOrderDetailDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ProductionOrderDetail oProductionOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_ProductionOrderDetail]"
                                    + "%n,%n,%n,%n,%n,%s,%s,%n,%n,%s,%n,%n,%s,%n,%s,%s,%n, %n,%s",
                                    oProductionOrderDetail.ProductionOrderDetailID, oProductionOrderDetail.ProductionOrderID, oProductionOrderDetail.ExportSCDetailID, oProductionOrderDetail.ProductID, oProductionOrderDetail.ColorID, oProductionOrderDetail.ProductDescription, oProductionOrderDetail.StyleDescription, oProductionOrderDetail.PolyMUnitID, oProductionOrderDetail.Qty, oProductionOrderDetail.Note, oProductionOrderDetail.ModelReferenceID, oProductionOrderDetail.UnitID, oProductionOrderDetail.BuyerRef, oProductionOrderDetail.ColorQty, oProductionOrderDetail.Model, oProductionOrderDetail.PantonNo, nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

		public static void Delete(TransactionContext tc, ProductionOrderDetail oProductionOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionOrderDetail]"
                                    + "%n,%n,%n,%n,%n,%s,%s,%n,%n,%s,%n,%n,%s,%n,%s,%s,%n, %n,%s",
                                    oProductionOrderDetail.ProductionOrderDetailID, oProductionOrderDetail.ProductionOrderID, oProductionOrderDetail.ExportSCDetailID, oProductionOrderDetail.ProductID, oProductionOrderDetail.ColorID, oProductionOrderDetail.ProductDescription, oProductionOrderDetail.StyleDescription, oProductionOrderDetail.PolyMUnitID, oProductionOrderDetail.Qty, oProductionOrderDetail.Note, oProductionOrderDetail.ModelReferenceID, oProductionOrderDetail.UnitID, oProductionOrderDetail.BuyerRef, oProductionOrderDetail.ColorQty, oProductionOrderDetail.Model, oProductionOrderDetail.PantonNo, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }



		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_ProductionOrderDetail WHERE ProductionOrderDetailID=%n", nID);
		}
		public static IDataReader Gets(int nPOID, TransactionContext tc)

		{
            return tc.ExecuteReader("SELECT * FROM View_ProductionOrderDetail WHERE ProductionOrderID = %n  Order By ProductionOrderDetailID", nPOID);
		}
        public static IDataReader GetsByLog(int nPOLOgID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_ProductionOrderDetailLog WHERE ProductionOrderLogID = %n Order By ProductionOrderDetailLogID", nPOLOgID);
		} 
        
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
