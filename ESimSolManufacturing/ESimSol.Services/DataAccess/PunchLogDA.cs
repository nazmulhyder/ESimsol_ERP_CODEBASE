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
    public class PunchLogDA
    {
        public PunchLogDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, int nEmployeeId, DateTime dtPunchTime, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PunchLog] %n,%D", nEmployeeId, dtPunchTime.ToString("dd MMM yyyy HH:mm"));

        }

        public static IDataReader UploadXL(TransactionContext tc, PunchLog oPunchLogXL, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_PunchLog] %s,%s",
                   oPunchLogXL.CardNo,
                   oPunchLogXL.PunchDateTime_ST
                   );
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nPunchLogID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PunchLog WHERE PunchLogID=%n", nPunchLogID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PunchLog");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static bool IsAttFreeze(string sSQL, TransactionContext tc)
        {
            object obj = tc.ExecuteScalar(sSQL);
            if (obj == null)
            {
                return false;
            }
            else
            {
                int n = Convert.ToInt32(obj);
                if (n > 0) return true;
                else
                    return false;
            }
        }
        public static IDataReader Delete(int nPunchLogID, TransactionContext tc)
        {
            return tc.ExecuteReader("DELETE FROM PunchLog  WHERE PunchLogID=" + nPunchLogID);
        }
        #endregion
    }
}
