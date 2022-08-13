using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class BusinessSessionDA
    {
        public BusinessSessionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BusinessSession oBusinessSession, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BusinessSession]" + "%n, %s, %b, %s, %n, %n",
                                    oBusinessSession.BusinessSessionID, oBusinessSession.SessionName, oBusinessSession.IsActive, oBusinessSession.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, BusinessSession oBusinessSession, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BusinessSession]" + "%n, %s, %b, %s, %n, %n",
                                    oBusinessSession.BusinessSessionID, oBusinessSession.SessionName, oBusinessSession.IsActive, oBusinessSession.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessSession WHERE BusinessSessionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessSession");
        }

        public static IDataReader Gets(TransactionContext tc, bool bIsActive)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessSession WHERE IsActive = %b", bIsActive);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
