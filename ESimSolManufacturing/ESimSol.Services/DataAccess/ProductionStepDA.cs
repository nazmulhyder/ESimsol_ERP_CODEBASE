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
    public class ProductionStepDA
    {
        public ProductionStepDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionStep oProductionStep, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionStep]" + "%n, %s, %n, %s,%s, %n, %n",
                                    oProductionStep.ProductionStepID, oProductionStep.StepName, oProductionStep.ProductionStepTypeInt, oProductionStep.Note, oProductionStep.ShortName, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ProductionStep oProductionStep, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionStep]" + "%n, %s, %n, %s,%s, %n, %n",
                                    oProductionStep.ProductionStepID, oProductionStep.StepName, oProductionStep.ProductionStepTypeInt, oProductionStep.Note, oProductionStep.ShortName, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ProductionStep WHERE ProductionStepID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ProductionStep");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }      
        #endregion
    }
}
