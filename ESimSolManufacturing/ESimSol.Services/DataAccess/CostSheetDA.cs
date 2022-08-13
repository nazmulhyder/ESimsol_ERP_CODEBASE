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
    
    public class CostSheetDA
    {
        public CostSheetDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CostSheet oCostSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_CostSheet]" + "%n,%n,%s,%n, %n,%n,%n,%d,%d,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oCostSheet.CostSheetID, oCostSheet.BUID, oCostSheet.FileNo, (int)oCostSheet.CostSheetType, oCostSheet.TechnicalSheetID, oCostSheet.MerchandiserID, oCostSheet.StatusInInt, oCostSheet.CostingDate, oCostSheet.ShipmentDate, oCostSheet.ApproxQty, oCostSheet.UnitID, oCostSheet.WeightPerDozen, 
                                    oCostSheet.WeightUnitID, oCostSheet.WastageInPercent, oCostSheet.GG, oCostSheet.FabricDescription, oCostSheet.StyleDescription, oCostSheet.CurrencyID, oCostSheet.ProcessLoss, oCostSheet.FabricWeightPerDozen, 
                                    oCostSheet.FabricUnitPrice, oCostSheet.FabricCostPerDozen, oCostSheet.AccessoriesCostPerDozen, oCostSheet.ProductionCostPerDozen, oCostSheet.BuyingCommission, oCostSheet.BankingCost,  oCostSheet.FOBPricePerPcs, 
                                    oCostSheet.OfferPricePerPcs, oCostSheet.CMCost,  oCostSheet.ConfirmPricePerPcs, oCostSheet.PrintPricePerPcs, oCostSheet.EmbrodaryPricePerPcs, oCostSheet.TestPricePerPcs, oCostSheet.OthersPricePerPcs, oCostSheet.CourierPricePerPcs, oCostSheet.OthersCaption, oCostSheet.CourierCaption,
                                    oCostSheet.FabricCostPerPcs, oCostSheet.AccessoriesCostPerPcs,oCostSheet.CMCostPerPcs,oCostSheet.FOBPricePerDozen,oCostSheet.OfferPricePerDozen,oCostSheet.ConfirmPricePerDozen,oCostSheet.PrintPricePerDozen,oCostSheet.EmbrodaryPricePerDozen,oCostSheet.TestPricePerDozen,oCostSheet.OthersPricePerDozen,
                                    oCostSheet.CourierPricePerDozen,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, CostSheet oCostSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_CostSheet]" + "%n,%n,%s,%n, %n,%n,%n,%d,%d,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oCostSheet.CostSheetID, oCostSheet.BUID, oCostSheet.FileNo, (int)oCostSheet.CostSheetType, oCostSheet.TechnicalSheetID, oCostSheet.MerchandiserID, oCostSheet.StatusInInt, oCostSheet.CostingDate, oCostSheet.ShipmentDate, oCostSheet.ApproxQty, oCostSheet.UnitID, oCostSheet.WeightPerDozen,
                                    oCostSheet.WeightUnitID, oCostSheet.WastageInPercent, oCostSheet.GG, oCostSheet.FabricDescription, oCostSheet.StyleDescription, oCostSheet.CurrencyID, oCostSheet.ProcessLoss, oCostSheet.FabricWeightPerDozen,
                                    oCostSheet.FabricUnitPrice, oCostSheet.FabricCostPerDozen, oCostSheet.AccessoriesCostPerDozen, oCostSheet.ProductionCostPerDozen, oCostSheet.BuyingCommission, oCostSheet.BankingCost, oCostSheet.FOBPricePerPcs,
                                    oCostSheet.OfferPricePerPcs, oCostSheet.CMCost, oCostSheet.ConfirmPricePerPcs, oCostSheet.PrintPricePerPcs, oCostSheet.EmbrodaryPricePerPcs, oCostSheet.TestPricePerPcs, oCostSheet.OthersPricePerPcs, oCostSheet.CourierPricePerPcs, oCostSheet.OthersCaption, oCostSheet.CourierCaption,
                                    oCostSheet.FabricCostPerPcs, oCostSheet.AccessoriesCostPerPcs, oCostSheet.CMCostPerPcs, oCostSheet.FOBPricePerDozen, oCostSheet.OfferPricePerDozen, oCostSheet.ConfirmPricePerDozen, oCostSheet.PrintPricePerDozen, oCostSheet.EmbrodaryPricePerDozen, oCostSheet.TestPricePerDozen, oCostSheet.OthersPricePerDozen,
                                    oCostSheet.CourierPricePerDozen, nUserID, (int)eEnumDBOperation);
        }


        public static IDataReader ChangeStatus(TransactionContext tc, CostSheet oCostSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_CostSheetOperation]" + " %n, %n, %s, %n, %n, %n",
                                    oCostSheet.CostSheetID, (int)oCostSheet.CostSheetStatus, oCostSheet.sNote, (int)oCostSheet.CostSheetActionType, nUserID, (int)eEnumDBOperation);
        }

        #region Accept Cost Sheet Revise
        public static IDataReader AcceptCostSheetRevise(TransactionContext tc, CostSheet oCostSheet, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_AcceptCostSheetRevise]" + "%n,%s,%n,%n,%n,%n,%d,%d,%n,%n,%n,%n,%n,%s,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oCostSheet.CostSheetID, oCostSheet.FileNo, (int)oCostSheet.CostSheetType, oCostSheet.TechnicalSheetID, oCostSheet.MerchandiserID, oCostSheet.StatusInInt, oCostSheet.CostingDate, oCostSheet.ShipmentDate, oCostSheet.ApproxQty, 
                                    oCostSheet.UnitID, oCostSheet.WeightPerDozen, oCostSheet.WeightUnitID, oCostSheet.WastageInPercent, oCostSheet.GG, oCostSheet.FabricDescription, oCostSheet.StyleDescription, oCostSheet.CurrencyID, oCostSheet.ProcessLoss, 
                                    oCostSheet.FabricWeightPerDozen, oCostSheet.FabricUnitPrice, oCostSheet.FabricCostPerDozen, oCostSheet.AccessoriesCostPerDozen, oCostSheet.ProductionCostPerDozen, oCostSheet.BuyingCommission, oCostSheet.BankingCost, oCostSheet.FOBPricePerPcs, 
                                    oCostSheet.OfferPricePerPcs, oCostSheet.CMCost, oCostSheet.ConfirmPricePerPcs, oCostSheet.PrintPricePerPcs, oCostSheet.EmbrodaryPricePerPcs, oCostSheet.TestPricePerPcs, oCostSheet.OthersPricePerPcs, oCostSheet.CourierPricePerPcs, oCostSheet.OthersCaption, oCostSheet.CourierCaption,
                                     oCostSheet.FabricCostPerPcs, oCostSheet.AccessoriesCostPerPcs,oCostSheet.CMCostPerPcs,oCostSheet.FOBPricePerDozen,oCostSheet.OfferPricePerDozen,oCostSheet.ConfirmPricePerDozen,oCostSheet.PrintPricePerDozen,oCostSheet.EmbrodaryPricePerDozen,oCostSheet.TestPricePerDozen,oCostSheet.OthersPricePerDozen,
                                    oCostSheet.CourierPricePerDozen,   nUserID);
        }

        #endregion

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheet WHERE CostSheetID=%n", nID);
        }

        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetLog WHERE CostSheetLogID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheet");
        }

        public static IDataReader GetsCostSheetLog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostSheetLog WHERE CostSheetID=%n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
   
}
