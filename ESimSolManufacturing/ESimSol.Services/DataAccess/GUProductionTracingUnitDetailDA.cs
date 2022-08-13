using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class GUProductionTracingUnitDetailDA
    {
        public GUProductionTracingUnitDetailDA() { }

        #region GUProductionTracingUnitDetail

        public static void UpdateExecutionQty(TransactionContext tc, int id, double nExecutionQty )
        {
            tc.ExecuteNonQuery("Update GUProductionTracingUnitDetail SET ExecutionQty = %n WHERE GUProductionTracingUnitDetailID = %n", nExecutionQty, id);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM  View_GUProductionTracingUnitDetail");
        }

        public static IDataReader Gets(int PUID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM  View_GUProductionTracingUnitDetail Where GUProductionTracingUnitID =" + PUID);
        }
        public static IDataReader Gets(String sSql, TransactionContext tc)
        {
            return tc.ExecuteReader(sSql);
        }

        public static IDataReader Get(TransactionContext tc, int nGUProductionTracingUnitDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM  View_GUProductionTracingUnitDetail WHERE GUProductionTracingUnitDetailID = %n", nGUProductionTracingUnitDetailID);
        }
        public static IDataReader GetBySequence(TransactionContext tc, int nGUProductionTracingUnitID, int nSequence)
         {
             return tc.ExecuteReader("SELECT top 1 * FROM  View_GUProductionTracingUnitDetail WHERE GUProductionTracingUnitID = %n AND Sequence = %n", nGUProductionTracingUnitID, nSequence);
         }
        public static IDataReader GetsByOrderRecap(TransactionContext tc, int nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionTracingUnitDetail AS PTUD WHERE  PTUD.GUProductionTracingUnitID IN(SELECT PTU.GUProductionTracingUnitID FROM GUProductionTracingUnit AS PTU WHERE PTU.OrderRecapID =%n) ORDER BY GUProductionTracingUnitDetailID", nOrderRecapID);
        }

        public static IDataReader Gets_byPOIDs(TransactionContext tc, string sPOIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionTracingUnitDetail AS PTUD WHERE  PTUD.GUProductionTracingUnitID IN(SELECT PTU.GUProductionTracingUnitID FROM GUProductionTracingUnit AS PTU WHERE PTU.GUProductionOrderID IN(" + sPOIDs + "))  Order By GUProductionTracingUnitDetailID");
        }
        
        #endregion
    }
}
