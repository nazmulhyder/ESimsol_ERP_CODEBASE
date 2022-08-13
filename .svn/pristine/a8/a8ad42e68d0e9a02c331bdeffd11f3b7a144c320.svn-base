using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FabricPOSetupDA
    {
        public FabricPOSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricPOSetup oFabricPOSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_FabricPOSetup]"
                                    + "%n, %s,%s,%s,%s, %n,%n, %b,%b,%b,%b, %n,%n",
                                    oFabricPOSetup.FabricPOSetupID,  oFabricPOSetup.NoCode, oFabricPOSetup.FabricCode, oFabricPOSetup.OrderName, oFabricPOSetup.POPrintName, oFabricPOSetup.PrintNo,
                                    oFabricPOSetup.CurrencyID, oFabricPOSetup.Activity, oFabricPOSetup.IsLDApply, oFabricPOSetup.IsNeedCheckBy, oFabricPOSetup.IsNeedExpDelivery, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, FabricPOSetup oFabricPOSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_FabricPOSetup]"
                                   + "%n, %s,%s,%s,%s, %n,%n, %b,%b,%b,%b, %n,%n",
                                    oFabricPOSetup.FabricPOSetupID, oFabricPOSetup.NoCode, oFabricPOSetup.FabricCode, oFabricPOSetup.OrderName, oFabricPOSetup.POPrintName, oFabricPOSetup.PrintNo,
                                    oFabricPOSetup.CurrencyID, oFabricPOSetup.Activity, oFabricPOSetup.IsLDApply, oFabricPOSetup.IsNeedCheckBy, oFabricPOSetup.IsNeedExpDelivery, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPOSetup WHERE FabricPOSetupID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPOSetup");
        }
        public static IDataReader GetsActive(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT Top(1)* FROM View_FabricPOSetup WHERE Activity=1");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, FabricPOSetup oFabricPOSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricPOSetup Set Activity=~Activity WHERE FabricPOSetupID=%n", oFabricPOSetup.FabricPOSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricPOSetup WHERE FabricPOSetupID=%n", oFabricPOSetup.FabricPOSetupID);

        }
    
     
    
        #endregion
    }
}