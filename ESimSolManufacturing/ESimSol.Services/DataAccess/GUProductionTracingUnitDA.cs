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
    public class GUProductionTracingUnitDA
    {
        public GUProductionTracingUnitDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GUProductionTracingUnit oGUProductionTracingUnit, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GUProductionTracingUnit]" + "%n, %s, %s, %n, %n", nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, GUProductionTracingUnit oGUProductionTracingUnit, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GUProductionTracingUnit]" + "%n, %s, %s, %n, %n", nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader CommitProductionExecution(TransactionContext tc, PTUTransection oPTUTransection, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitProductionExecution]" + "%n, %n, %n, %n, %n, %n, %d, %s,%n,%n,%n, %b,%n",
                                   oPTUTransection.PTUTransectionID, oPTUTransection.GUProductionTracingUnitID, oPTUTransection.ProductionStepID, oPTUTransection.PLineConfigureID, oPTUTransection.MeasurementUnitID, oPTUTransection.Quantity, oPTUTransection.OperationDate, oPTUTransection.Note, oPTUTransection.ActualWorkingHour, oPTUTransection.UseOperator, oPTUTransection.UseHelper, oPTUTransection.IsUsesValueUpdate,  nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionTracingUnit WHERE GUProductionTracingUnitID=%n", nID);
        }

        public static IDataReader GetsByGUProductionOrder(TransactionContext tc, int nGUProductionOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionTracingUnit WHERE GUProductionOrderID=%n", nGUProductionOrderID);
        }

        public static IDataReader GetsPTU(TransactionContext tc, int nGUProductionOrderID, int nProductionStepID)
        {
            return tc.ExecuteReader("EXEC [SP_GetPTU]" + "%n, %n", nGUProductionOrderID, nProductionStepID);
        }
        public static IDataReader GetsPTU(TransactionContext tc, int nGUProductionOrderID)
        {
            return tc.ExecuteReader("EXEC [SP_GetPTU]" + "%n", nGUProductionOrderID);
        }

        public static IDataReader GetsPTUSet(TransactionContext tc, int nPTUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionTracingUnit WHERE GUProductionOrderID=(SELECT GUProductionOrderID FROM GUProductionTracingUnit WHERE GUProductionTracingUnitID=%n)", nPTUID);
        }

        public static IDataReader GetPTUViewDetails(TransactionContext tc, DateTime dOperationDate, int nExcutionFatoryId, int nGUProductionOrderID)
        {
            return tc.ExecuteReader("SELECT * from  View_GUProductionTracingUnit WHERE GUProductionTracingUnitID IN(SELECT GUProductionTracingUnitID FROM PTUTransaction where OperationDate='" + dOperationDate + "'and ExecutionFactoryID= " + nExcutionFatoryId + " and GUProductionTracingUnitID IN( Select GUProductionTracingUnitID From GUProductionTracingUnit where GUProductionOrderID=" + nGUProductionOrderID + "))");
        }

        public static IDataReader GetsByOrderRecap(TransactionContext tc, int nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionTracingUnit AS PTU WHERE PTU.SaleOrderID =%n ORDER BY GUProductionTracingUnitID", nOrderRecapID);
        }

        public static IDataReader Gets_byPOIDs(TransactionContext tc, string sPOIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionTracingUnit WHERE GUProductionOrderID IN (" + sPOIDs + ") Order By GUProductionTracingUnitID");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
