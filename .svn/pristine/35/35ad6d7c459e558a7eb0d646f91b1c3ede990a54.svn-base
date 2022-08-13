using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductionSheetDA
    {
        public ProductionSheetDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionSheet oProductionSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionSheet]"
                                    + "%n, %s, %n, %n, %d,%n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n",
                                    oProductionSheet.ProductionSheetID, oProductionSheet.SheetNo, (int)oProductionSheet.SheetStatus, oProductionSheet.PTUUnit2ID, oProductionSheet.IssueDate,  oProductionSheet.ProductID, oProductionSheet.BUID, oProductionSheet.Note, oProductionSheet.Quantity, oProductionSheet.FGWeight, oProductionSheet.NaliWeight, oProductionSheet.WeightFor, oProductionSheet.FGWeightUnitID, oProductionSheet.RecipeID, oProductionSheet.ModelReferenceID, oProductionSheet.MachineID, oProductionSheet.PerCartonFGQty, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ProductionSheet oProductionSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionSheet]"
                                    + "%n, %s, %n, %n, %d,%n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n",
                                    oProductionSheet.ProductionSheetID, oProductionSheet.SheetNo, (int)oProductionSheet.SheetStatus, oProductionSheet.PTUUnit2ID, oProductionSheet.IssueDate, oProductionSheet.ProductID, oProductionSheet.BUID, oProductionSheet.Note, oProductionSheet.Quantity, oProductionSheet.FGWeight, oProductionSheet.NaliWeight, oProductionSheet.WeightFor, oProductionSheet.FGWeightUnitID, oProductionSheet.RecipeID, oProductionSheet.ModelReferenceID, oProductionSheet.MachineID, oProductionSheet.PerCartonFGQty, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionSheet WHERE ProductionSheetID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionSheet");
        }

        public static IDataReader BUWiseGets(int nBUID, int nProductNature, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionSheet WHERE SheetStatus=0 AND BUID = " + nBUID + " AND ISNULL(ProductNature,0) = " + nProductNature);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_ProductionSheet
        }
        #endregion
    }  
}
