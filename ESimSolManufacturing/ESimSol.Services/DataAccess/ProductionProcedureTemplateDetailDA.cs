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
    public class ProductionProcedureTemplateDetailDA
    {
        public ProductionProcedureTemplateDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionProcedureTemplateDetail oProductionProcedureTemplateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionProcedureTemplateDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                    oProductionProcedureTemplateDetail.ProductionProcedureTemplateDetailID, oProductionProcedureTemplateDetail.ProductionProcedureTemplateID,  oProductionProcedureTemplateDetail.ProductionStepID, oProductionProcedureTemplateDetail.Sequence, oProductionProcedureTemplateDetail.Remarks, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ProductionProcedureTemplateDetail oProductionProcedureTemplateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sProductionProcedureTemplateDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionProcedureTemplateDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                    oProductionProcedureTemplateDetail.ProductionProcedureTemplateDetailID, oProductionProcedureTemplateDetail.ProductionProcedureTemplateID, oProductionProcedureTemplateDetail.ProductionStepID, oProductionProcedureTemplateDetail.Sequence, oProductionProcedureTemplateDetail.Remarks, nUserID, (int)eEnumDBOperation, sProductionProcedureTemplateDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionProcedureTemplateDetail WHERE ProductionProcedureTemplateDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionProcedureTemplateDetail ORDER BY ProductionProcedureTemplateID, Sequence");
        }

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionProcedureTemplateDetail WHERE ProductionProcedureTemplateID=%n ORDER BY Sequence", id);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
