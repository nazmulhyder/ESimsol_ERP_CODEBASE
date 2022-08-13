using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FabricOrderSetupDA
    {
        public FabricOrderSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricOrderSetup oFabricOrderSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricOrderSetup]"
                                    + "%n,%n,%s,%s,%n, %s,%s,%s,%s,%n, %n,%n,%n, %b,%b, %s,%s,%b,%b, %n,%n",
                                    oFabricOrderSetup.FabricOrderSetupID, oFabricOrderSetup.FabricOrderType, oFabricOrderSetup.OrderName, oFabricOrderSetup.ShortName, oFabricOrderSetup.PrintNo, oFabricOrderSetup.POPrintName, oFabricOrderSetup.CodeNo, oFabricOrderSetup.CodeName, oFabricOrderSetup.ComboNo, oFabricOrderSetup.CurrencyID, oFabricOrderSetup.MUnitID, oFabricOrderSetup.MUnitID_Alt, oFabricOrderSetup.BUID, oFabricOrderSetup.IsApplyPO, oFabricOrderSetup.Activity, oFabricOrderSetup.CodeNo_Lab, oFabricOrderSetup.CodeName_Lab, oFabricOrderSetup.IsRateApply,oFabricOrderSetup.IsLocal, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, FabricOrderSetup oFabricOrderSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricOrderSetup]"
                                   + "%n,%n,%s,%s,%n, %s,%s,%s,%s,%n, %n,%n,%n, %b,%b, %s,%s,%b,%b, %n,%n",
                                    oFabricOrderSetup.FabricOrderSetupID, oFabricOrderSetup.FabricOrderType, oFabricOrderSetup.OrderName, oFabricOrderSetup.ShortName, oFabricOrderSetup.PrintNo, oFabricOrderSetup.POPrintName, oFabricOrderSetup.CodeNo, oFabricOrderSetup.CodeName, oFabricOrderSetup.ComboNo, oFabricOrderSetup.CurrencyID, oFabricOrderSetup.MUnitID, oFabricOrderSetup.MUnitID_Alt, oFabricOrderSetup.BUID, oFabricOrderSetup.IsApplyPO, oFabricOrderSetup.Activity, oFabricOrderSetup.CodeNo_Lab, oFabricOrderSetup.CodeName_Lab, oFabricOrderSetup.IsRateApply,oFabricOrderSetup.IsLocal, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricOrderSetup WHERE FabricOrderSetupID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nOrderType)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricOrderSetup WHERE FabricOrderType=%n", nOrderType);
        }
        public static IDataReader GetByOrderTypes(TransactionContext tc,int buid, bool bIsPO)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricOrderSetup WHERE Activity=1 and BUID=%n and IsApplyPO=%b  order by ComboNo", buid, bIsPO);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricOrderSetup Order by ComboNo");
        }
        public static IDataReader GetsActive(TransactionContext tc, int nBUID)
        {
            string sSQL = "SELECT * FROM View_FabricOrderSetup WHERE Activity=1";
            if (nBUID > 0) { sSQL = sSQL + " and BUID="+nBUID;}
            sSQL = sSQL + " order by FabricOrderType";
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, FabricOrderSetup oFabricOrderSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricOrderSetup Set Activity=~Activity WHERE FabricOrderSetupID=%n", oFabricOrderSetup.FabricOrderSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricOrderSetup WHERE FabricOrderSetupID=%n", oFabricOrderSetup.FabricOrderSetupID);

        }
        #endregion
    }
}