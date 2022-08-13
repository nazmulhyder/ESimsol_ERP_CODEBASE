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

    public class MasterLCTermsAndConditionDA
    {
        public MasterLCTermsAndConditionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MasterLCTermsAndCondition oMasterLCTermsAndCondition, EnumDBOperation eEnumDBOperation, string sMasterLCTermsAndConditionIDs, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MasterLCTermsAndCondition]"
                                    + "%n,%n,%s,%n,%n,%s", oMasterLCTermsAndCondition.MasterLCTermsAndConditionID, oMasterLCTermsAndCondition.MasterLCID, oMasterLCTermsAndCondition.TermsAndCondition, nUserID, (int)eEnumDBOperation, "");
        }
        public static void Delete(TransactionContext tc, MasterLCTermsAndCondition oMasterLCTermsAndCondition, EnumDBOperation eEnumDBOperation, string sMasterLCTermsAndConditionIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MasterLCTermsAndCondition]"
                                    + "%n,%n,%s,%n,%n,%s", oMasterLCTermsAndCondition.MasterLCTermsAndConditionID, oMasterLCTermsAndCondition.MasterLCID, oMasterLCTermsAndCondition.TermsAndCondition, nUserID, (int)eEnumDBOperation, sMasterLCTermsAndConditionIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCTermsAndCondition WHERE MasterLCTermsAndConditionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCTermsAndCondition WHERE MasterLCID = %n", id);
        }

        public static IDataReader GetsMasterLCLog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCTermsAndConditionLog WHERE MasterLCLogID = %n", id);
        }
        //GetsPILog

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
    
}
