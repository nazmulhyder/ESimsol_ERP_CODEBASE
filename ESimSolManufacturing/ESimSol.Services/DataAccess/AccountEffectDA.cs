using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountEffectDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountEffect oAccountEffect, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountEffect]"
                                    + "%n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n",
                                    oAccountEffect.AccountEffectID, oAccountEffect.ModuleNameInt, oAccountEffect.ModuleObjID, oAccountEffect.AccountEffectTypeInt, oAccountEffect.DrAccountHeadID, oAccountEffect.CrAccountHeadID, oAccountEffect.Remarks,  oAccountEffect.DebitSubLedgerID, oAccountEffect.CreditSubLedgerID,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, AccountEffect oAccountEffect, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AccountEffect]"
                                    + "%n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n",
                                    oAccountEffect.AccountEffectID, oAccountEffect.ModuleNameInt, oAccountEffect.ModuleObjID, oAccountEffect.AccountEffectTypeInt, oAccountEffect.DrAccountHeadID, oAccountEffect.CrAccountHeadID, oAccountEffect.Remarks, oAccountEffect.DebitSubLedgerID, oAccountEffect.CreditSubLedgerID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountEffect WHERE AccountHeadID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountEffect");
        }
        public static IDataReader Gets(TransactionContext tc, int nModuleObjID, EnumModuleName eModuleName)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountEffect AS HH WHERE HH.ModuleObjID =%n AND HH.ModuleName = %n ORDER BY HH.AccountEffectType ASC", nModuleObjID, (int)eModuleName);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
