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

    public class StyleBudgetDetailDA
    {
        public StyleBudgetDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, StyleBudgetDetail oStyleBudgetDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sStyleBudgetDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_StyleBudgetDetail]"
                                    + "%n,%n,%n,%n, %s,%s,%n, %s,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%s", oStyleBudgetDetail.StyleBudgetDetailID, oStyleBudgetDetail.StyleBudgetID, oStyleBudgetDetail.MaterialTypeInInt, oStyleBudgetDetail.MaterialID, oStyleBudgetDetail.Description, oStyleBudgetDetail.Width, oStyleBudgetDetail.Consumption, oStyleBudgetDetail.Ply, oStyleBudgetDetail.MaterialMarketPrice, oStyleBudgetDetail.UsePercentage, oStyleBudgetDetail.EstimatedCost, oStyleBudgetDetail.WastagePercentPerMaterial, oStyleBudgetDetail.Sequence, oStyleBudgetDetail.UnitID, oStyleBudgetDetail.KnittingCost, oStyleBudgetDetail.DyeingCost, oStyleBudgetDetail.LycraCost, oStyleBudgetDetail.AOPCost, oStyleBudgetDetail.WashCost, oStyleBudgetDetail.YarnDyeingCost, oStyleBudgetDetail.SuedeCost, oStyleBudgetDetail.FinishingCost, oStyleBudgetDetail.BrushingCost, oStyleBudgetDetail.RateUnit,  nUserID, (int)eEnumDBOperation, sStyleBudgetDetailIDs);
        }

        public static void Delete(TransactionContext tc, StyleBudgetDetail oStyleBudgetDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sStyleBudgetDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_StyleBudgetDetail]"
                                    + "%n,%n,%n,%n, %s,%s,%n, %s,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%s", oStyleBudgetDetail.StyleBudgetDetailID, oStyleBudgetDetail.StyleBudgetID, oStyleBudgetDetail.MaterialTypeInInt, oStyleBudgetDetail.MaterialID, oStyleBudgetDetail.Description, oStyleBudgetDetail.Width, oStyleBudgetDetail.Consumption, oStyleBudgetDetail.Ply, oStyleBudgetDetail.MaterialMarketPrice, oStyleBudgetDetail.UsePercentage, oStyleBudgetDetail.EstimatedCost, oStyleBudgetDetail.WastagePercentPerMaterial, oStyleBudgetDetail.Sequence, oStyleBudgetDetail.UnitID, oStyleBudgetDetail.KnittingCost, oStyleBudgetDetail.DyeingCost, oStyleBudgetDetail.LycraCost, oStyleBudgetDetail.AOPCost, oStyleBudgetDetail.WashCost, oStyleBudgetDetail.YarnDyeingCost, oStyleBudgetDetail.SuedeCost, oStyleBudgetDetail.FinishingCost, oStyleBudgetDetail.BrushingCost, oStyleBudgetDetail.RateUnit, nUserID, (int)eEnumDBOperation, sStyleBudgetDetailIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_StyleBudgetDetail WHERE StyleBudgetDetailID=%n", nID);
        }
        public static IDataReader Gets(int nStyleBudgetID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_StyleBudgetDetail where StyleBudgetID =%n Order by MaterialType, Sequence", nStyleBudgetID);
        }

        public static IDataReader GetActualSheet(int nSaleOrderID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC[SP_ActualStyleBudget]"+ "%n", nSaleOrderID);
        }
        public static IDataReader GetsStyleBudgetLog(int nStyleBudgetLogID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_StyleBudgetLogDetail where StyleBudgetLogID =%n", nStyleBudgetLogID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
  
}
