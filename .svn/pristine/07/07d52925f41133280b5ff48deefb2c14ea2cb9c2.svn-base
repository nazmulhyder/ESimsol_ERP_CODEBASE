using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    class ProductionScheduleDetailDA
    {
        
        public ProductionScheduleDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ProductionScheduleDetail oProductionScheduleDetail, EnumDBOperation eEnumDBProductionScheduleDetail, Int64 nUserId, string sProductionScheduleDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionScheduleDetail]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oProductionScheduleDetail.ProductionScheduleDetailID, oProductionScheduleDetail.ProductionScheduleID, oProductionScheduleDetail.ProductionTracingUnitID, oProductionScheduleDetail.DODID, oProductionScheduleDetail.ProductionQty, oProductionScheduleDetail.Remarks, nUserId, (int)eEnumDBProductionScheduleDetail, sProductionScheduleDetailIDs);
        }


        public static void Delete(TransactionContext tc, ProductionScheduleDetail oProductionScheduleDetail, EnumDBOperation eEnumDBProductionScheduleDetail, Int64 nUserId, string sProductionScheduleDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionScheduleDetail]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oProductionScheduleDetail.ProductionScheduleDetailID, oProductionScheduleDetail.ProductionScheduleID, oProductionScheduleDetail.ProductionTracingUnitID, oProductionScheduleDetail.DODID, oProductionScheduleDetail.ProductionQty, oProductionScheduleDetail.Remarks, nUserId, (int)eEnumDBProductionScheduleDetail, sProductionScheduleDetailIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc,int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionScheduleDetail where ProductionScheduleID=%n", id);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionScheduleDetail ");
        }

        public static IDataReader Gets(TransactionContext tc,string sPSIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionScheduleDetail where ProductionScheduleID in (%q) ", sPSIDs);
        }
        public static IDataReader GetsSqL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
