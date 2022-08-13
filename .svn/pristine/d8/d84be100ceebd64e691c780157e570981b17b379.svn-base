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
    
    public class StyleBudgetDA
    {
        public StyleBudgetDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, StyleBudget oStyleBudget, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_StyleBudget]" + "%n,%n,%s,%n, %s,%n, %n,%n,%n,%d,%d,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oStyleBudget.StyleBudgetID, oStyleBudget.BUID, oStyleBudget.BudgetTitle, oStyleBudget.CostSheetID, oStyleBudget.FileNo, (int)oStyleBudget.StyleBudgetType, oStyleBudget.TechnicalSheetID, oStyleBudget.MerchandiserID, oStyleBudget.StatusInInt, oStyleBudget.CostingDate, oStyleBudget.ShipmentDate, oStyleBudget.ApproxQty, oStyleBudget.UnitID, oStyleBudget.WeightPerDozen, 
                                    oStyleBudget.WeightUnitID, oStyleBudget.WastageInPercent, oStyleBudget.GG, oStyleBudget.FabricDescription, oStyleBudget.StyleDescription, oStyleBudget.CurrencyID, oStyleBudget.ProcessLoss, oStyleBudget.FabricWeightPerDozen, 
                                    oStyleBudget.FabricUnitPrice, oStyleBudget.FabricCostPerDozen, oStyleBudget.AccessoriesCostPerDozen, oStyleBudget.ProductionCostPerDozen, oStyleBudget.BuyingCommission, oStyleBudget.BankingCost,  oStyleBudget.FOBPricePerPcs, 
                                    oStyleBudget.OfferPricePerPcs, oStyleBudget.CMCost,  oStyleBudget.ConfirmPricePerPcs, oStyleBudget.PrintPricePerPcs, oStyleBudget.EmbrodaryPricePerPcs, oStyleBudget.TestPricePerPcs, oStyleBudget.OthersPricePerPcs, oStyleBudget.CourierPricePerPcs, oStyleBudget.OthersCaption, oStyleBudget.CourierCaption,
                                    oStyleBudget.FabricCostPerPcs, oStyleBudget.AccessoriesCostPerPcs,oStyleBudget.CMCostPerPcs,oStyleBudget.FOBPricePerDozen,oStyleBudget.OfferPricePerDozen,oStyleBudget.ConfirmPricePerDozen,oStyleBudget.PrintPricePerDozen,oStyleBudget.EmbrodaryPricePerDozen,oStyleBudget.TestPricePerDozen,oStyleBudget.OthersPricePerDozen,
                                    oStyleBudget.CourierPricePerDozen,  oStyleBudget.RefObjectID,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, StyleBudget oStyleBudget, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_StyleBudget]" + "%n,%n,%s,%n, %s,%n, %n,%n,%n,%d,%d,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oStyleBudget.StyleBudgetID, oStyleBudget.BUID, oStyleBudget.BudgetTitle, oStyleBudget.CostSheetID, oStyleBudget.FileNo, (int)oStyleBudget.StyleBudgetType, oStyleBudget.TechnicalSheetID, oStyleBudget.MerchandiserID, oStyleBudget.StatusInInt, oStyleBudget.CostingDate, oStyleBudget.ShipmentDate, oStyleBudget.ApproxQty, oStyleBudget.UnitID, oStyleBudget.WeightPerDozen,
                                    oStyleBudget.WeightUnitID, oStyleBudget.WastageInPercent, oStyleBudget.GG, oStyleBudget.FabricDescription, oStyleBudget.StyleDescription, oStyleBudget.CurrencyID, oStyleBudget.ProcessLoss, oStyleBudget.FabricWeightPerDozen,
                                    oStyleBudget.FabricUnitPrice, oStyleBudget.FabricCostPerDozen, oStyleBudget.AccessoriesCostPerDozen, oStyleBudget.ProductionCostPerDozen, oStyleBudget.BuyingCommission, oStyleBudget.BankingCost, oStyleBudget.FOBPricePerPcs,
                                    oStyleBudget.OfferPricePerPcs, oStyleBudget.CMCost, oStyleBudget.ConfirmPricePerPcs, oStyleBudget.PrintPricePerPcs, oStyleBudget.EmbrodaryPricePerPcs, oStyleBudget.TestPricePerPcs, oStyleBudget.OthersPricePerPcs, oStyleBudget.CourierPricePerPcs, oStyleBudget.OthersCaption, oStyleBudget.CourierCaption,
                                    oStyleBudget.FabricCostPerPcs, oStyleBudget.AccessoriesCostPerPcs, oStyleBudget.CMCostPerPcs, oStyleBudget.FOBPricePerDozen, oStyleBudget.OfferPricePerDozen, oStyleBudget.ConfirmPricePerDozen, oStyleBudget.PrintPricePerDozen, oStyleBudget.EmbrodaryPricePerDozen, oStyleBudget.TestPricePerDozen, oStyleBudget.OthersPricePerDozen,
                                    oStyleBudget.CourierPricePerDozen, oStyleBudget.RefObjectID, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader ChangeStatus(TransactionContext tc, StyleBudget oStyleBudget, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_StyleBudgetOperation]" + " %n, %n, %s, %n, %n, %n",
                                    oStyleBudget.StyleBudgetID, (int)oStyleBudget.StyleBudgetStatus, oStyleBudget.sNote, (int)oStyleBudget.StyleBudgetActionType, nUserID, (int)eEnumDBOperation);
        }
      
        #region Accept Cost Sheet Revise
        public static IDataReader AcceptStyleBudgetRevise(TransactionContext tc, StyleBudget oStyleBudget, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_AcceptStyleBudgetRevise]" + "%n,%s,%s,%n,%n,%n,%n,%n,%d,%d,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oStyleBudget.StyleBudgetID, oStyleBudget.FileNo, oStyleBudget.BudgetTitle, oStyleBudget.CostSheetID, (int)oStyleBudget.StyleBudgetType, oStyleBudget.TechnicalSheetID, oStyleBudget.MerchandiserID, oStyleBudget.StatusInInt, oStyleBudget.CostingDate, oStyleBudget.ShipmentDate, oStyleBudget.ApproxQty, 
                                    oStyleBudget.UnitID, oStyleBudget.WeightPerDozen, oStyleBudget.WeightUnitID, oStyleBudget.WastageInPercent, oStyleBudget.GG, oStyleBudget.FabricDescription, oStyleBudget.StyleDescription, oStyleBudget.CurrencyID, oStyleBudget.ProcessLoss, 
                                    oStyleBudget.FabricWeightPerDozen, oStyleBudget.FabricUnitPrice, oStyleBudget.FabricCostPerDozen, oStyleBudget.AccessoriesCostPerDozen, oStyleBudget.ProductionCostPerDozen, oStyleBudget.BuyingCommission, oStyleBudget.BankingCost, oStyleBudget.FOBPricePerPcs, 
                                    oStyleBudget.OfferPricePerPcs, oStyleBudget.CMCost, oStyleBudget.ConfirmPricePerPcs,  oStyleBudget.PrintPricePerPcs, oStyleBudget.EmbrodaryPricePerPcs, oStyleBudget.TestPricePerPcs, oStyleBudget.OthersPricePerPcs, oStyleBudget.CourierPricePerPcs, oStyleBudget.OthersCaption, oStyleBudget.CourierCaption,
                                     oStyleBudget.FabricCostPerPcs, oStyleBudget.AccessoriesCostPerPcs,oStyleBudget.CMCostPerPcs,oStyleBudget.FOBPricePerDozen,oStyleBudget.OfferPricePerDozen,oStyleBudget.ConfirmPricePerDozen,oStyleBudget.PrintPricePerDozen,oStyleBudget.EmbrodaryPricePerDozen,oStyleBudget.TestPricePerDozen,oStyleBudget.OthersPricePerDozen,
                                    oStyleBudget.CourierPricePerDozen, oStyleBudget.RefObjectID,  nUserID);
        }

        #endregion

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_StyleBudget WHERE StyleBudgetID=%n", nID);
        }

        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_StyleBudgetLog WHERE StyleBudgetLogID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_StyleBudget");
        }

        public static IDataReader GetsStyleBudgetLog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_StyleBudgetLog WHERE StyleBudgetID=%n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
   
}
