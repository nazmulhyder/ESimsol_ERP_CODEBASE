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
    public class GUProductionProcedureDA
    {
        public GUProductionProcedureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GUProductionProcedure oGUProductionProcedure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GUProductionProcedure]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                    oGUProductionProcedure.GUProductionProcedureID, oGUProductionProcedure.GUProductionOrderID, oGUProductionProcedure.ProductionStepID, oGUProductionProcedure.Sequence, oGUProductionProcedure.Remarks, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, GUProductionProcedure oGUProductionProcedure, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sGUProductionProcedureIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GUProductionProcedure]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                    oGUProductionProcedure.GUProductionProcedureID, oGUProductionProcedure.GUProductionOrderID, oGUProductionProcedure.ProductionStepID, oGUProductionProcedure.Sequence, oGUProductionProcedure.Remarks, nUserID, (int)eEnumDBOperation, sGUProductionProcedureIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionProcedure WHERE GUProductionProcedureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionProcedure");
        }

        public static IDataReader Gets(TransactionContext tc, int nGUProductionOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionProcedure WHERE GUProductionOrderID=%n ORDER BY Sequence", nGUProductionOrderID);
        }

        public static IDataReader GetsbyOrderRecap(TransactionContext tc, int nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionProcedure AS PP WHERE PP.GUProductionOrderID IN (SELECT PO.GUProductionOrderID FROM GUProductionOrder AS PO WHERE PO.SaleOrderID=%n) ORDER BY PP.Sequence", nOrderRecapID);
        }

        public static IDataReader Gets_byPOIDs(TransactionContext tc, string sPOIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionProcedure AS PP WHERE PP.GUProductionOrderID IN (" + sPOIDs + ") ORDER BY PP.Sequence");
        }

        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
