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
    public class ProductionProcedureDA
    {
        public ProductionProcedureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionProcedure oProductionProcedure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionProcedure]" + "%n, %n, %n, %n, %s,%s,%s, %n, %n, %s",
                                    oProductionProcedure.ProductionProcedureID, oProductionProcedure.ProductionSheetID, oProductionProcedure.ProductionStepID, oProductionProcedure.Sequence, oProductionProcedure.Remarks, oProductionProcedure.Measurement, oProductionProcedure.ThickNess, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ProductionProcedure oProductionProcedure, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sProductionProcedureIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionProcedure]" + "%n, %n, %n, %n, %s,%s,%s, %n, %n, %s",
                                    oProductionProcedure.ProductionProcedureID, oProductionProcedure.ProductionSheetID, oProductionProcedure.ProductionStepID, oProductionProcedure.Sequence, oProductionProcedure.Remarks, oProductionProcedure.Measurement, oProductionProcedure.ThickNess, nUserID, (int)eEnumDBOperation, sProductionProcedureIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionProcedure WHERE ProductionProcedureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionProcedure");
        }

       

        public static IDataReader Gets(TransactionContext tc, int nProductionSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionProcedure WHERE ProductionSheetID=%n ORDER BY Sequence", nProductionSheetID);
        }

        public static IDataReader GetsbyOrderRecap(TransactionContext tc, int nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionProcedure AS PP WHERE PP.ProductionOrderID IN (SELECT PO.ProductionOrderID FROM ProductionOrder AS PO WHERE PO.SaleOrderID=%n) ORDER BY PP.Sequence", nOrderRecapID);
        }

        public static IDataReader Gets_byPOIDs(TransactionContext tc, string sPOIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionProcedure AS PP WHERE PP.ProductionOrderID IN (" + sPOIDs + ") ORDER BY PP.Sequence");
        }

        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
