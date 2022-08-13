using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{

    public class CostSheetDetailDA
    {
        public CostSheetDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CostSheetDetail oCostSheetDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sCostSheetDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_CostSheetDetail]"
                                    + "%n,%n,%n,%n, %s,%s,%n, %s,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%s", oCostSheetDetail.CostSheetDetailID, oCostSheetDetail.CostSheetID, oCostSheetDetail.MaterialTypeInInt, oCostSheetDetail.MaterialID, oCostSheetDetail.Description, oCostSheetDetail.Width, oCostSheetDetail.Consumption, oCostSheetDetail.Ply, oCostSheetDetail.MaterialMarketPrice, oCostSheetDetail.UsePercentage, oCostSheetDetail.EstimatedCost, oCostSheetDetail.WastagePercentPerMaterial, oCostSheetDetail.Sequence, oCostSheetDetail.UnitID, oCostSheetDetail.KnittingCost, oCostSheetDetail.DyeingCost, oCostSheetDetail.LycraCost, oCostSheetDetail.AOPCost, oCostSheetDetail.WashCost, oCostSheetDetail.YarnDyeingCost, oCostSheetDetail.SuedeCost, oCostSheetDetail.FinishingCost, oCostSheetDetail.BrushingCost, oCostSheetDetail.RateUnit,  nUserID, (int)eEnumDBOperation, sCostSheetDetailIDs);
        }

        public static void Delete(TransactionContext tc, CostSheetDetail oCostSheetDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sCostSheetDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_CostSheetDetail]"
                                    + "%n,%n,%n,%n, %s,%s,%n, %s,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%s", oCostSheetDetail.CostSheetDetailID, oCostSheetDetail.CostSheetID, oCostSheetDetail.MaterialTypeInInt, oCostSheetDetail.MaterialID, oCostSheetDetail.Description, oCostSheetDetail.Width, oCostSheetDetail.Consumption, oCostSheetDetail.Ply, oCostSheetDetail.MaterialMarketPrice, oCostSheetDetail.UsePercentage, oCostSheetDetail.EstimatedCost, oCostSheetDetail.WastagePercentPerMaterial, oCostSheetDetail.Sequence, oCostSheetDetail.UnitID, oCostSheetDetail.KnittingCost, oCostSheetDetail.DyeingCost, oCostSheetDetail.LycraCost, oCostSheetDetail.AOPCost, oCostSheetDetail.WashCost, oCostSheetDetail.YarnDyeingCost, oCostSheetDetail.SuedeCost, oCostSheetDetail.FinishingCost, oCostSheetDetail.BrushingCost, oCostSheetDetail.RateUnit, nUserID, (int)eEnumDBOperation, sCostSheetDetailIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetDetail WHERE CostSheetDetailID=%n", nID);
        }
        public static IDataReader Gets(int nCostSheetID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetDetail where CostSheetID =%n Order by MaterialType, Sequence", nCostSheetID);
        }

        public static IDataReader GetActualSheet(int nSaleOrderID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC[SP_ActualCostSheet]"+ "%n", nSaleOrderID);
        }
        public static IDataReader GetsCostSheetLog(int nCostSheetLogID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetLogDetail where CostSheetLogID =%n", nCostSheetLogID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
  
}
