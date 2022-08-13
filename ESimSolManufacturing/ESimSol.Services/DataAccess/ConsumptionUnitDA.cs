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

    public class ConsumptionUnitDA
    {
        public ConsumptionUnitDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ConsumptionUnit oConsumptionUnit, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ConsumptionUnit]"
                                    + "%n,%s, %s, %n, %n,%n",
                                    oConsumptionUnit.ConsumptionUnitID, oConsumptionUnit.Name, oConsumptionUnit.Note, oConsumptionUnit.ParentConsumptionUnitID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ConsumptionUnit oConsumptionUnit, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ConsumptionUnit]"
                                    + "%n,%s, %s, %n, %n,%n",
                                    oConsumptionUnit.ConsumptionUnitID, oConsumptionUnit.Name, oConsumptionUnit.Note, oConsumptionUnit.ParentConsumptionUnitID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ConsumptionUnit WHERE ConsumptionUnitID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ConsumptionUnit Order By CUSequence");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }



        public static IDataReader ChangeGroup(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }



        public static void UpdateSequence(TransactionContext tc, ConsumptionUnit oConsumptionUnit)
        {
            tc.ExecuteNonQuery("Update ConsumptionUnit SET CUSequence = %n WHERE ConsumptionUnitID = %n", oConsumptionUnit.CUSequence, oConsumptionUnit.ConsumptionUnitID);
        }



        #endregion
    }  
}
