using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class ImportSetupDA
    {
        public ImportSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportSetup oImportSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ImportSetup]"
                                    + "%n,%n, %b, %b, %b,%n, %s,%b,%b,%n,%n, %n, %n,%s,%D, %n, %n",
                                    oImportSetup.ImportSetupID, oImportSetup.BUID, oImportSetup.IsApplyPO, oImportSetup.IsApplyTT, oImportSetup.IsApplyMasterLC, oImportSetup.CurrencyID,    oImportSetup.Note,  oImportSetup.IsFreightRate,oImportSetup.IsApplyRateOn,oImportSetup.FileTypeInt ,oImportSetup.ShipmentDay,oImportSetup.ExpireDay,oImportSetup.DaysCalculateOnInt ,oImportSetup.CoverNoteNumber, oImportSetup.CoverNoteDate,  nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ImportSetup oImportSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ImportSetup]"
                                    + "%n,%n, %b, %b, %b,%n, %s,%b,%b,%n,%n, %n, %n,%s,%D, %n, %n",
                                     oImportSetup.ImportSetupID, oImportSetup.BUID, oImportSetup.IsApplyPO, oImportSetup.IsApplyTT, oImportSetup.IsApplyMasterLC, oImportSetup.CurrencyID, oImportSetup.Note, oImportSetup.IsFreightRate, oImportSetup.IsApplyRateOn, oImportSetup.FileTypeInt, oImportSetup.ShipmentDay, oImportSetup.ExpireDay, oImportSetup.DaysCalculateOnInt, oImportSetup.CoverNoteNumber, oImportSetup.CoverNoteDate, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportSetup WHERE ImportSetupID=%n", nID);
        }

        public static IDataReader GetByBU(TransactionContext tc, long nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportSetup WHERE BUID=%n and Activity=1", nBUID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportSetup");
        }
        public static IDataReader Gets(TransactionContext tc,int nBUID )
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportSetup WHERE BUID=%n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, ImportSetup oImportSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ImportSetup Set Activity=~Activity WHERE ImportSetupID=%n", oImportSetup.ImportSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ImportSetup WHERE ImportSetupID=%n", oImportSetup.ImportSetupID);

        }
    
     
    
        #endregion
    }
}