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
    public class KnittingOrderTermsAndConditionDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingOrderTermsAndConditionIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingOrderTermsAndCondition]"
                                   + "%n,%n,%n,%s,%n,%n,%s",
                                   oKnittingOrderTermsAndCondition.KnittingOrderTermsAndConditionID, oKnittingOrderTermsAndCondition.KnittingOrderID, oKnittingOrderTermsAndCondition.ClauseType, oKnittingOrderTermsAndCondition.TermsAndCondition, nUserID, (int)eEnumDBOperation, sKnittingOrderTermsAndConditionIDs);
        }

        public static void Delete(TransactionContext tc, KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingOrderTermsAndConditionIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingOrderTermsAndCondition]"
                                   + "%n,%n,%n,%s,%n,%n,%s",
                                   oKnittingOrderTermsAndCondition.KnittingOrderTermsAndConditionID, oKnittingOrderTermsAndCondition.KnittingOrderID, oKnittingOrderTermsAndCondition.ClauseType, oKnittingOrderTermsAndCondition.TermsAndCondition, nUserID, (int)eEnumDBOperation, sKnittingOrderTermsAndConditionIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrderTermsAndCondition WHERE KnittingOrderTermsAndConditionID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrderTermsAndCondition");
        }
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrderTermsAndCondition WHERE KnittingOrderID = %n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
