using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class LCTermDA
    {
        public LCTermDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LCTerm oLCTerm, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LCTerm]"
                                    + "%n, %s, %s, %n, %n, %n",
                                    oLCTerm.LCTermID, oLCTerm.Name, oLCTerm.Description, oLCTerm.Days, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, LCTerm oLCTerm, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LCTerm]"
                                    + "%n, %s, %s, %n, %n, %n",
                                    oLCTerm.LCTermID, oLCTerm.Name, oLCTerm.Description, oLCTerm.Days, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM LCTerm WHERE LCTermID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM LCTerm");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}