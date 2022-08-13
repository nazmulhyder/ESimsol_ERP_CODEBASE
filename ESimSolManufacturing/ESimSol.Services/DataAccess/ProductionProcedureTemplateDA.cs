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
    public class ProductionProcedureTemplateDA
    {
        public ProductionProcedureTemplateDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionProcedureTemplate oProductionProcedureTemplate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionProcedureTemplate]" + "%n, %s, %s, %s, %n, %n",
                                    oProductionProcedureTemplate.ProductionProcedureTemplateID, oProductionProcedureTemplate.TemplateNo, oProductionProcedureTemplate.TemplateName, oProductionProcedureTemplate.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ProductionProcedureTemplate oProductionProcedureTemplate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionProcedureTemplate]" + "%n, %s, %s, %s, %n, %n",
                                    oProductionProcedureTemplate.ProductionProcedureTemplateID, oProductionProcedureTemplate.TemplateNo, oProductionProcedureTemplate.TemplateName, oProductionProcedureTemplate.Remarks, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ProductionProcedureTemplate WHERE ProductionProcedureTemplateID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ProductionProcedureTemplate");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
