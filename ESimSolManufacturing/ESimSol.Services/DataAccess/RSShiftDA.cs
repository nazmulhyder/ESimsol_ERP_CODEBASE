using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class RSShiftDA
    {
        public RSShiftDA() { }

        #region Insert Update Delete Active Function
        public static IDataReader InsertUpdate(TransactionContext tc, RSShift oRSShift, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_RSShift]"
                                    + "%n,%s,%s,%b, %n, %D, %D, %n,%n",
                                    oRSShift.RSShiftID, oRSShift.Name, oRSShift.Note, oRSShift.Activity, oRSShift.ModuleTypeInt, oRSShift.StartDateTimeSt, oRSShift.EndDateTimeSt, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, RSShift oRSShift, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_RSShift]"
                                      + "%n,%s,%s,%b, %n, %D, %D, %n,%n",
                                    oRSShift.RSShiftID, oRSShift.Name, oRSShift.Note, oRSShift.Activity, oRSShift.ModuleTypeInt, oRSShift.StartDateTimeSt, oRSShift.EndDateTimeSt, nUserId, (int)eEnumDBOperation);
        }
        public static IDataReader ToggleActivity(TransactionContext tc, RSShift oRSShift)
        {
            string sSQL1 = SQLParser.MakeSQL("Update RSShift Set Activity=~Activity WHERE RSShiftID=%n", oRSShift.RSShiftID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM RSShift WHERE RSShiftID=%n", oRSShift.RSShiftID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM RSShift WHERE RSShiftID=%n", nID);
        }
        public static IDataReader GetsActive(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM RSShift WHERE Activity=1");
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM RSShift");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByModule(TransactionContext tc, int nBUID, string sModuleIDs)
        {
            return tc.ExecuteReader("SELECT * FROM RSShift WHERE ModuleType IN ( " + sModuleIDs + " ) and Activity=1  ORDER BY Name", nBUID);
        }
        #endregion

    }
}