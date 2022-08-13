using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Net.Mail;

namespace ESimSol.Services.DataAccess
{
    public class HumanInteractionAgentDA
    {
        public HumanInteractionAgentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, HumanInteractionAgent oHumanInteractionAgent, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HumanInteractionAgent]"
                                    + "%n, %n, %s, %n, %n, %n, %s",
                                  oHumanInteractionAgent.HIAID, oHumanInteractionAgent.MessageBodyText, nUserId, (int)eEnumDBOperation, "");
        }

        public static void UpdateRead(TransactionContext tc, HumanInteractionAgent oHIA, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE HumanInteractionAgent SET  IsRead=%n, ReadDatetime=%q WHERE HIAID=%n", true, Global.DBDateTime, oHIA.HIAID);
        }

        public static void Delete(TransactionContext tc, HumanInteractionAgent oHumanInteractionAgent, EnumDBOperation eEnumDBOperation, Int64 nUserId, string sHumanInteractionAgentIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HumanInteractionAgent]"
                                    + "%n, %n, %s, %n, %n, %n, %s",
                                     oHumanInteractionAgent.HIAID, nUserId, (int)eEnumDBOperation, sHumanInteractionAgentIDs);
        }



        #endregion

        #region Get & Exist Function

        public static int GetHIA_NotificationCount(TransactionContext tc, Int64 nUserID)
        {
            object obj = tc.ExecuteScalar("SELECT COUNT(*) FROM HumanInteractionAgent WHERE IsRead=0 AND UserID=%n", nUserID);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }
        public static IDataReader Get(TransactionContext tc, long nid)
        {
            return tc.ExecuteReader("SELECT * FROM View_HumanInteractionAgent WHERE HIAID=%n", nid);
        }


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_HumanInteractionAgent");
        }

        public static IDataReader GetsBy(TransactionContext tc, Int64 nUserID, bool bIsAll)
        {
            if (bIsAll)
            {
                return tc.ExecuteReader("SELECT * FROM View_HumanInteractionAgent where UserID=%n order by HIAID DESC", nUserID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_HumanInteractionAgent where UserID=%n AND IsRead=0 ORDER BY HIAID DESC", nUserID);
            }
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
