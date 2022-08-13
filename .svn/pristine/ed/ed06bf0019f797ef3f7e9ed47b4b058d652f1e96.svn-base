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
    public class ProductionTimeSetupDA
    {
        public ProductionTimeSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionTimeSetup oProductionTimeSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionTimeSetup]" + "%n, %n, %s,%n, %n, %n, %n",
                                    oProductionTimeSetup.ProductionTimeSetupID, oProductionTimeSetup.BUID, oProductionTimeSetup.OffDay, oProductionTimeSetup.RegularTime, oProductionTimeSetup.OverTime, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ProductionTimeSetup oProductionTimeSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionTimeSetup]" + "%n, %n, %s,%n, %n, %n, %n",
                                    oProductionTimeSetup.ProductionTimeSetupID, oProductionTimeSetup.BUID, oProductionTimeSetup.OffDay, oProductionTimeSetup.RegularTime, oProductionTimeSetup.OverTime, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionTimeSetup WHERE ProductionTimeSetupID=%n", nID);
        }

        public static IDataReader GetByBU(TransactionContext tc, long nbuid)
        {
            return tc.ExecuteReader("SELECT top 1 * FROM View_ProductionTimeSetup WHERE BUID=%n", nbuid);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionTimeSetup");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }      
        #endregion
    }
}
