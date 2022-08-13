using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ChangesEquitySetupDetailDA
    {
        public ChangesEquitySetupDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ChangesEquitySetupDetail oChangesEquitySetupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ChangesEquitySetupDetail]"
                                    + "%n, %n, %n, %n, %n, %s",
                                    oChangesEquitySetupDetail.ChangesEquitySetupDetailID, oChangesEquitySetupDetail.ChangesEquitySetupID, oChangesEquitySetupDetail.EffectedAccountID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ChangesEquitySetupDetail oChangesEquitySetupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sChangesEquitySetupDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ChangesEquitySetupDetail]"
                                    + "%n, %n, %n, %n, %n, %s",
                                    oChangesEquitySetupDetail.ChangesEquitySetupDetailID, oChangesEquitySetupDetail.ChangesEquitySetupID, oChangesEquitySetupDetail.EffectedAccountID, nUserID, (int)eEnumDBOperation, sChangesEquitySetupDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChangesEquitySetupDetail WHERE ChangesEquitySetupDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChangesEquitySetupDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nChangesEquitySetupID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChangesEquitySetupDetail AS TT WHERE TT.ChangesEquitySetupID=%n", nChangesEquitySetupID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
