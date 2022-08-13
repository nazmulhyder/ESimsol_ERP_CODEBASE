using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LCContactDA
    {
        public LCContactDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LCContact oLCContact, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LCContact]" + "%n, %n,  %d,%n,%n,   %s, %n, %n",
                                    oLCContact.LCContactID, oLCContact.BUID, oLCContact.BalanceDate, oLCContact.LCInHand, oLCContact.ContactInHand, oLCContact.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, LCContact oLCContact, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LCContact]" + "%n, %n,  %d,%n,%n,   %s, %n, %n",
                                    oLCContact.LCContactID, oLCContact.BUID, oLCContact.BalanceDate, oLCContact.LCInHand, oLCContact.ContactInHand, oLCContact.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static IDataReader GetsLcContacts(TransactionContext tc, LCContact oLCContact, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_LCContact]" + " %d", oLCContact.BalanceDate);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCContact WHERE LCContactID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LCContact");
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
