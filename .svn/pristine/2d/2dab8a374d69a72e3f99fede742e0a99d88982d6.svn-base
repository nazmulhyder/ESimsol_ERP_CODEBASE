using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SourcingConfigHeadDA
    {
        public SourcingConfigHeadDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SourcingConfigHead oSourcingConfigHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SourcingConfigHead]" + "%n, %n, %s, %s, %n, %n",
                                    oSourcingConfigHead.SourcingConfigHeadID, oSourcingConfigHead.SourcingConfigHeadType, oSourcingConfigHead.SourcingConfigHeadName, oSourcingConfigHead.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SourcingConfigHead oSourcingConfigHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SourcingConfigHead]" + "%n, %n, %s, %s, %n, %n",
                                    oSourcingConfigHead.SourcingConfigHeadID, oSourcingConfigHead.SourcingConfigHeadType, oSourcingConfigHead.SourcingConfigHeadName, oSourcingConfigHead.Remarks, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM SourcingConfigHead ");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
