using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LB_LocationDA
    {
        public LB_LocationDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LB_Location oLB_Location, short nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LB_Location] "
                                    + "%n, %s, %s, %s, %b, %D, %n, %D, %n, %n",
                                    oLB_Location.LB_LocationID, oLB_Location.LB_IPV4, oLB_Location.LB_KnownName, oLB_Location.LB_LocationNote, oLB_Location.LB_Is_Classified, oLB_Location.LB_ClassificationDate, oLB_Location.LB_ClasifiedBy, oLB_Location.LB_FirstHitDate, oLB_Location.LB_FirstHitBy, nDBOperation);
        }        
        #endregion

        #region Gets & Queries
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM LB_Location Where LB_LocationID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static bool HasLocation(TransactionContext tc, string sLB_IPV4)
        {
            var obj = tc.ExecuteScalar("Select ISNULL(Count(*),0) from LB_Location Where LB_Is_Classified=1 And LB_IPV4=%s", sLB_IPV4);
           return (Convert.ToInt32(obj)>0);
        }

        #endregion
    }

    public class LB_UserLocationMapDA
    {
        public LB_UserLocationMapDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LB_UserLocationMap oLBULM, short nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LB_UserLocationMap] " + "%n, %n, %n, %D, %n, %n", oLBULM.LB_UserLocationMapID, oLBULM.LB_UserID, oLBULM.LB_LB_LocationID, oLBULM.LB_ExpireDateTime, nUserID, nDBOperation);
        }

        public static void Delete(TransactionContext tc, string sSQL)
        {
             tc.ExecuteNonQuery(sSQL);
        }
        public static void DeleteRow(TransactionContext tc, LB_UserLocationMap oLBULM, short nDBOperation, int nUserID)
        {
             tc.ExecuteNonQuery("EXEC [SP_IUD_LB_UserLocationMap] " + "%n, %n, %n, %D, %n, %n", oLBULM.LB_UserLocationMapID, oLBULM.LB_UserID, oLBULM.LB_LB_LocationID, DateTime.Now, nUserID, nDBOperation);
        }
        #endregion

        #region Gets & Queries
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LB_UserLocationMap Where LB_UserLocationMapID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static bool HasLocationBind(TransactionContext tc, string sLB_IPV4, long nUserID)
        {
            var obj = tc.ExecuteScalar("SELECT ISNULL(Count(*),0) FROM LB_UserLocationMap ULM WHERE ULM.LB_UserID=" + nUserID + " AND ULM.LB_ExpireDateTime>=GETDATE() AND ULM.LB_LB_LocationID IN (SELECT LB_Location.LB_LocationID FROM LB_Location WHERE LB_Location.LB_IPV4 ='" + sLB_IPV4 + "')");
            return (Convert.ToInt32(obj) > 0);
        }
        #endregion
    }
}