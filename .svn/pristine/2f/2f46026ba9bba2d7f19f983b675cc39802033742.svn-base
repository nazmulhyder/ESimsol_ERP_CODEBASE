using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class FGQCDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FGQC oFGQC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FGQC]"
                                    + "%n, %s, %d, %n, %n, %s, %n, %n",
                                    oFGQC.FGQCID, oFGQC.FGQCNo, oFGQC.FGQCDate, oFGQC.BUID, oFGQC.ApprovedBy, oFGQC.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FGQC oFGQC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FGQC]"
                                    + "%n, %s, %d, %n, %n, %s, %n, %n",
                                    oFGQC.FGQCID, oFGQC.FGQCNo, oFGQC.FGQCDate, oFGQC.BUID, oFGQC.ApprovedBy, oFGQC.Remarks, nUserID, (int)eEnumDBOperation);
        }
        public static void Approved(TransactionContext tc, FGQC oFGQC, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE FGQC SET ApprovedBy = %n WHERE FGQCID = %n", nUserID, oFGQC.FGQCID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FGQC WHERE FGQCID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FGQC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}