using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class IntegrationSetupDA
    {
        public IntegrationSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, IntegrationSetup oIntegrationSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_IntegrationSetup]" + "%n, %s, %n, %s, %s, %s, %n, %n, %n, %n, %n",
                                    oIntegrationSetup.IntegrationSetupID, oIntegrationSetup.SetupNo, oIntegrationSetup.VoucherSetupInt, oIntegrationSetup.DataCollectionSQL, oIntegrationSetup.KeyColumn, oIntegrationSetup.Note, oIntegrationSetup.Sequence, oIntegrationSetup.SetupType, oIntegrationSetup.BUID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, IntegrationSetup oIntegrationSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_IntegrationSetup]" + "%n, %s, %n, %s, %s, %s, %n, %n, %n, %n, %n",
                                    oIntegrationSetup.IntegrationSetupID, oIntegrationSetup.SetupNo, oIntegrationSetup.VoucherSetupInt, oIntegrationSetup.DataCollectionSQL, oIntegrationSetup.KeyColumn, oIntegrationSetup.Note, oIntegrationSetup.Sequence, oIntegrationSetup.SetupType, oIntegrationSetup.BUID, nUserID, (int)eEnumDBOperation);
        }
        public static void UpdateSequence(TransactionContext tc, IntegrationSetup oIntegrationSetup)
        {
            tc.ExecuteNonQuery("UPDATE IntegrationSetup SET  Sequence=%n WHERE IntegrationSetupID=%n", oIntegrationSetup.Sequence, oIntegrationSetup.IntegrationSetupID);            
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_IntegrationSetup WHERE IntegrationSetupID=%n", nID);
        }
        public static IDataReader GetByVoucherSetup(TransactionContext tc, EnumVoucherSetup eEnumVoucherSetup)
        {
            return tc.ExecuteReader("SELECT * FROM View_IntegrationSetup WHERE VoucherSetup=%n", (int)eEnumVoucherSetup);
        }
       
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_IntegrationSetup AS HH ORDER BY HH.BUID, HH.Sequence ASC");
        }   
    
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsBySetupType(TransactionContext tc, EnumSetupType eEnumSetupType)
        {
            return tc.ExecuteReader("SELECT * FROM View_IntegrationSetup WHERE SetupType = %n ORDER BY Sequence", (int)eEnumSetupType);
        }
        public static IDataReader GetsByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_IntegrationSetup WHERE BUID = %n ORDER BY Sequence ASC", nBUID);
        }
        #endregion
    }
}
