using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;


namespace ESimSol.Services.DataAccess
{
    public class MailAssignedPersonDA
    {
        public MailAssignedPersonDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, MailAssignedPerson oMailAssignedPerson, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MailAssignedPerson]"
                                    + "%n,%n,%s,%b,%n", oMailAssignedPerson.MAPID, oMailAssignedPerson.MSID, oMailAssignedPerson.MailTo, oMailAssignedPerson.IsCCMail, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nMAPID)
        {
            return tc.ExecuteReader("SELECT * FROM MailAssignedPerson WHERE MAPID=%n", nMAPID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
