using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FDOrderSetupDA
    {
        public FDOrderSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FDOrderSetup oFDOrderSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_FDOrderSetup]"
                                    + "%n,%n, %s,%s,%s, %s, %s,%s, %n,%n,%n, %n,  %n, %n,  %n, %n, %n , %n",
                                    oFDOrderSetup.FDOrderSetupID, oFDOrderSetup.FDOTypeInt, oFDOrderSetup.ShortName, oFDOrderSetup.NoCode, oFDOrderSetup.DONoCode, oFDOrderSetup.FDOName, oFDOrderSetup.PrintName, oFDOrderSetup.NoteFixed, oFDOrderSetup.PrintNo, oFDOrderSetup.Activity, oFDOrderSetup.MUnitID, oFDOrderSetup.MUnitID_Alt, oFDOrderSetup.BUID, oFDOrderSetup.CurrencyID, oFDOrderSetup.ComboNo, oFDOrderSetup.PrintFormat, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, FDOrderSetup oFDOrderSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_FDOrderSetup]"
                                         + "%n,%n, %s,%s,%s, %s, %s,%s, %n,%n,%n, %n,  %n, %n,  %n, %n, %n, %n",
                                    oFDOrderSetup.FDOrderSetupID, oFDOrderSetup.FDOTypeInt, oFDOrderSetup.ShortName, oFDOrderSetup.NoCode, oFDOrderSetup.DONoCode, oFDOrderSetup.FDOName, oFDOrderSetup.PrintName, oFDOrderSetup.NoteFixed, oFDOrderSetup.PrintNo, oFDOrderSetup.Activity, oFDOrderSetup.MUnitID, oFDOrderSetup.MUnitID_Alt, oFDOrderSetup.BUID, oFDOrderSetup.CurrencyID, oFDOrderSetup.ComboNo, oFDOrderSetup.PrintFormat, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FDOrderSetup WHERE FDOrderSetupID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nOrderType)
        {
            return tc.ExecuteReader("SELECT * FROM View_FDOrderSetup WHERE FDOType=%n and Activity=1", nOrderType);
        }
        public static IDataReader GetByOrderTypes(TransactionContext tc,int nBUID, bool bIsInHouse, string sOrderType)
        {
            return tc.ExecuteReader("SELECT * FROM View_FDOrderSetup WHERE Activity=1 and BUID=%n and IsInHouse=%b and OrderType in (%q) order by ComboNo",nBUID, bIsInHouse, sOrderType);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FDOrderSetup order by ComboNo");
        }
        public static IDataReader GetsActive(TransactionContext tc, int nBUID)
        {
            string sSQL = "SELECT * FROM View_FDOrderSetup WHERE Activity=1";
            if (nBUID > 0) { sSQL = sSQL + " and BUID="+nBUID;}
            sSQL=sSQL+" order by ComboNo";
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
        public static IDataReader Activate(TransactionContext tc, FDOrderSetup oFDOrderSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FDOrderSetup Set Activity=~Activity WHERE FDOrderSetupID=%n", oFDOrderSetup.FDOrderSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FDOrderSetup WHERE FDOrderSetupID=%n", oFDOrderSetup.FDOrderSetupID);

        }
        #endregion
    }
}