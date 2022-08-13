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
    public class ProductionRecipeDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ProductionRecipe oProductionRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionRecipe]"
                                   + "%n, %n, %n, %n,%n, %n, %s, %n,%n,%n,%s",
                                   oProductionRecipe.ProductionRecipeID, oProductionRecipe.ProductionSheetID, oProductionRecipe.ProductID, oProductionRecipe.QtyInPercent, oProductionRecipe.MUnitID, oProductionRecipe.RequiredQty, oProductionRecipe.Remarks, (int)oProductionRecipe.QtyType, (int)eEnumDBOperation, nUserID,  sDetailIDs);
        }

        public static void Delete(TransactionContext tc, ProductionRecipe oProductionRecipe, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionRecipe]"
                                   + "%n, %n, %n, %n,%n, %n, %s, %n,%n,%n,%s",
                                   oProductionRecipe.ProductionRecipeID, oProductionRecipe.ProductionSheetID, oProductionRecipe.ProductID, oProductionRecipe.QtyInPercent, oProductionRecipe.MUnitID, oProductionRecipe.RequiredQty, oProductionRecipe.Remarks, (int)oProductionRecipe.QtyType, (int)eEnumDBOperation, nUserID, sDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader GetsByWU(TransactionContext tc, int nPSID, int nWUID, int nRMRequisitionID)
        {
            return tc.ExecuteReader("EXEC[SP_GetProductionRecipe] %n,%n,%n", nPSID, nWUID, nRMRequisitionID);
        }
        
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionRecipe WHERE ProductionRecipeID=%n", nID);
        }
        public static IDataReader Gets(int nORSID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionRecipe WHERE ProductionSheetID = %n ORDER BY ProductionRecipeID", nORSID);
        }

        public static IDataReader GetsByLog(int nORSLogID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionRecipeLog WHERE ProductionSheetLogID = %n", nORSLogID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
  
}
