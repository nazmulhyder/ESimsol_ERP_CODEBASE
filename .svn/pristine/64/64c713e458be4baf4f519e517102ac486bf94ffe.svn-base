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
    public class GratuitySchemeDA
    {
        public GratuitySchemeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, GratuityScheme oGratuityScheme, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GratuityScheme] %n,%s,%s,%n,%n",
                   oGratuityScheme.GSID, oGratuityScheme.Name,
                   oGratuityScheme.Description, nUserID, nDBOperation);
        }

        public static IDataReader Activity(GratuityScheme oGratuityScheme, Int64 nUserId, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE GratuityScheme SET InactiveDate=%d WHERE GSID=%n;SELECT * FROM View_GratuityScheme WHERE GSID=%n", DateTime.Now, oGratuityScheme.GSID, oGratuityScheme.GSID);

        }

        public static IDataReader Approve(GratuityScheme oGratuityScheme, Int64 nUserId, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE GratuityScheme SET ApproveBy=%n,ApproveByDate=%d WHERE GSID=%n;SELECT * FROM View_GratuityScheme WHERE GSID=%n", nUserId, DateTime.Now, oGratuityScheme.GSID, oGratuityScheme.GSID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nGSID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GratuityScheme WHERE GSID=%n", nGSID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GratuityScheme");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
