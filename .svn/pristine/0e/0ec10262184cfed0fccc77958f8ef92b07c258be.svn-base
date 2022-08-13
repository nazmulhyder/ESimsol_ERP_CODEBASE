using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportPIDetailDA
    {
        public ExportPIDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPIDetail oEPIDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID,string sIDs )
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPIDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %s,%s,%s, %n, %n, %n, %n,%n,%s,%n,%n,%n,%n,%n, %n, %n, %n, %n,%s,%s,%n,%b,%s,%s,%n,%n,%n,%n, %n, %n,%s",
                                    oEPIDetail.ExportPIDetailID, oEPIDetail.ExportPIID, oEPIDetail.FabricID, oEPIDetail.ProductID, oEPIDetail.Qty, oEPIDetail.MUnitID, oEPIDetail.UnitPrice, oEPIDetail.Amount, oEPIDetail.StyleNo, oEPIDetail.BuyerReference, oEPIDetail.ColorInfo, oEPIDetail.AdjQty, oEPIDetail.AdjRate, oEPIDetail.DocCharge, oEPIDetail.CRate, oEPIDetail.ColorID, oEPIDetail.ProductDescription, oEPIDetail.ModelReferenceID, oEPIDetail.PolyMUnitID, oEPIDetail.OrderSheetDetailID, oEPIDetail.ColorQty, oEPIDetail.ExportQualityID, oEPIDetail.FabricWeave, oEPIDetail.FinishType, oEPIDetail.ProcessType, oEPIDetail.FabricDesignID, oEPIDetail.FabricWidth, oEPIDetail.Construction, oEPIDetail.RecipeID, oEPIDetail.IsDeduct, oEPIDetail.Shrinkage, oEPIDetail.Weight, oEPIDetail.DyeingType, (int)oEPIDetail.SaleType, oEPIDetail.PackingType, (int)oEPIDetail.ShadeType, (int)eEnumDBOperation, nUserID, sIDs);
        }
        public static void Delete(TransactionContext tc, ExportPIDetail oEPIDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPIDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %s,%s,%s, %n, %n, %n, %n,%n,%s,%n,%n,%n,%n,%n, %n, %n, %n, %n,%s,%s,%n,%b,%s,%s,%n,%n,%n,%n, %n, %n,%s",
                                    oEPIDetail.ExportPIDetailID, oEPIDetail.ExportPIID, oEPIDetail.FabricID, oEPIDetail.ProductID, oEPIDetail.Qty, oEPIDetail.MUnitID, oEPIDetail.UnitPrice, oEPIDetail.Amount, oEPIDetail.StyleNo, oEPIDetail.BuyerReference, oEPIDetail.ColorInfo, oEPIDetail.AdjQty, oEPIDetail.AdjRate, oEPIDetail.DocCharge, oEPIDetail.CRate, oEPIDetail.ColorID, oEPIDetail.ProductDescription, oEPIDetail.ModelReferenceID, oEPIDetail.PolyMUnitID, oEPIDetail.OrderSheetDetailID, oEPIDetail.ColorQty, oEPIDetail.ExportQualityID, oEPIDetail.FabricWeave, oEPIDetail.FinishType, oEPIDetail.ProcessType, oEPIDetail.FabricDesignID, oEPIDetail.FabricWidth, oEPIDetail.Construction, oEPIDetail.RecipeID, oEPIDetail.IsDeduct, oEPIDetail.Shrinkage, oEPIDetail.Weight, oEPIDetail.DyeingType, (int)oEPIDetail.SaleType, oEPIDetail.PackingType, (int)oEPIDetail.ShadeType, (int)eEnumDBOperation, nUserID, sIDs);
        }

        //public static void DeleteRevise(TransactionContext tc, ExportPIDetail oExportPIDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportPIDetailIDs)
        //{
        //    tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPIDetailRevise]" + "%n, %n, %n, %n, %n, %n, %n, %n, %s,%s,%s,%s,%s,%s, %n, %n",
        //                           oExportPIDetail.ExportPIDetailID, oExportPIDetail.ExportPIID, oExportPIDetail.FabricID, oExportPIDetail.ProductID, oExportPIDetail.Qty, oExportPIDetail.MUnitID, oExportPIDetail.UnitPrice, oExportPIDetail.Amount, oExportPIDetail.Description, oExportPIDetail.StyleNo, oExportPIDetail.StyleNo, oExportPIDetail.BuyerReference, oExportPIDetail.ColorInfo, sExportPIDetailIDs,(int)eEnumDBOperation, nUserID);
        //}
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIDetail WHERE ExportPIDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIDetail WHERE ExportPIID =%n order by ExportPIDetailID ", nExportPIID);
        }

      
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIDetail");
        }

        public static IDataReader GetsByPI(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIDetail WHERE ExportPIID=%n order by ProductID", nPIID);
        }
        public static IDataReader GetsByPIAndSortByOrderSheet(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPIDetail WHERE ExportPIID=%n ORDER BY OrderSheetDetailID  ASC", nPIID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportPIDetailLog] WHERE ExportPIID=%n", nPIID);
        }
        public static IDataReader GetsLogDetail(TransactionContext tc, int nPILogID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportPIDetailLog] WHERE ExportPILogID=%n  Order BY ExportPIDetailLogID", nPILogID);
        }

        //public static IDataReader GetsByLC(TransactionContext tc, int nExportLCID)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_ExportPIDetails_LC WHERE PIID in (Select ExportPIID from [ExportPI] where LCID =%n ) ", nExportLCID);
        //}
        public static IDataReader UpdateQuality(TransactionContext tc, ExportPIDetail oExportPIDetail)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportPIDetail Set ExportQualityID=%n WHERE ExportPIDetailID=%n", oExportPIDetail.ExportQualityID, oExportPIDetail.ExportPIDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportPIDetail WHERE ExportPIDetailID=%n", oExportPIDetail.ExportPIDetailID);
        }
        public static IDataReader UpdateCRate(TransactionContext tc, ExportPIDetail oExportPIDetail)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportPIDetail Set CRate=%n ,CRateTwo=%n ,QtyCom =%n WHERE ExportPIDetailID=%n", oExportPIDetail.CRate, oExportPIDetail.CRateTwo, oExportPIDetail.QtyCom, oExportPIDetail.ExportPIDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportPIDetail WHERE ExportPIDetailID=%n", oExportPIDetail.ExportPIDetailID);
        }
        #endregion
    }
}
