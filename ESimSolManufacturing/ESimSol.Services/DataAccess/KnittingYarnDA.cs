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
    public class KnittingYarnDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnittingYarn oKnittingYarn, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingYarnIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingYarn]"
                                   + "%n,%n,%n,%s,%n,%n,%s",
                                   oKnittingYarn.KnittingYarnID, oKnittingYarn.KnittingOrderID, oKnittingYarn.YarnID, oKnittingYarn.Remarks, nUserID, (int)eEnumDBOperation, sKnittingYarnIDs);
        }

        public static void Delete(TransactionContext tc, KnittingYarn oKnittingYarn, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingYarnIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingYarn]"
                                    + "%n,%n,%n,%s,%n,%n,%s",
                                    oKnittingYarn.KnittingYarnID, oKnittingYarn.KnittingOrderID, oKnittingYarn.YarnID, oKnittingYarn.Remarks, nUserID, (int)eEnumDBOperation, sKnittingYarnIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingYarn WHERE KnittingYarnID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingYarn");
        }
        public static IDataReader Gets(TransactionContext tc,int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingYarn WHERE KnittingOrderID = %n",id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
