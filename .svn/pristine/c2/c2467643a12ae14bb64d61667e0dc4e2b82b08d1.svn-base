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

    public class PQTermsAndConditionDA
    {
        public PQTermsAndConditionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PQTermsAndCondition oPQTermsAndCondition, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPQTermsAndConditionIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PQTermsAndCondition]"
                                    + "%n,%n,%s,%n,%n,%s", oPQTermsAndCondition.PQTermsAndConditionID, oPQTermsAndCondition.PurchaseQuotationID, oPQTermsAndCondition.TermsAndCondition, nUserID, (int)eEnumDBOperation, "");
        }
        public static void Delete(TransactionContext tc, PQTermsAndCondition oPQTermsAndCondition, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPQTermsAndConditionIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PQTermsAndCondition]"
                                    + "%n,%n,%s,%n,%n,%s", oPQTermsAndCondition.PQTermsAndConditionID, oPQTermsAndCondition.PurchaseQuotationID, oPQTermsAndCondition.TermsAndCondition, nUserID, (int)eEnumDBOperation, sPQTermsAndConditionIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM PQTermsAndCondition WHERE PQTermsAndConditionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM PQTermsAndCondition WHERE PurchaseQuotationID = %n", id);
        }

        public static IDataReader GetsByLog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM PQTermsAndConditionLog WHERE PurchaseQuotationLogID =%n", id);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
    
}

