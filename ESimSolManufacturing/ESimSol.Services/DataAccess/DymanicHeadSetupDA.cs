using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DymanicHeadSetupDA
    {
        public DymanicHeadSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DymanicHeadSetup oDHSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DymanicHeadSetup]"
                                    + "%n, %s, %n, %n, %n, %s, %b, %n, %n",
                                    oDHSetup.DymanicHeadSetupID, oDHSetup.Name, oDHSetup.ReferenceTypeInt, oDHSetup.MappingTypeInt, oDHSetup.MappingID, oDHSetup.Note, oDHSetup.Activity, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DymanicHeadSetup oDHSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DymanicHeadSetup]"
                                    + "%n, %s, %n, %n, %n, %s, %b, %n, %n",
                                    oDHSetup.DymanicHeadSetupID, oDHSetup.Name, oDHSetup.ReferenceTypeInt, oDHSetup.MappingTypeInt, oDHSetup.MappingID, oDHSetup.Note, oDHSetup.Activity, nUserId, (int)eEnumDBOperation);
        }
        public static void ActivateDymanicHeadSetup(TransactionContext tc, DymanicHeadSetup oDymanicHeadSetup)
        {
            tc.ExecuteNonQuery("Update DymanicHeadSetup Set Activity=Activity^1 Where DymanicHeadSetupID=%n", oDymanicHeadSetup.DymanicHeadSetupID);

        }
        public static void Process(TransactionContext tc, DymanicHeadSetup oDHSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_Process_DynamicHead]" + "%n", nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nid)
        {
            return tc.ExecuteReader("SELECT * FROM View_DymanicHeadSetup WHERE DymanicHeadSetupID=%n", nid);
        }
        public static IDataReader GetByRef(TransactionContext tc, EnumReferenceType eEnumReferenceType)
        {
            return tc.ExecuteReader("SELECT * FROM View_DymanicHeadSetup AS DHS WHERE DHS.ReferenceType=%n", (int)eEnumReferenceType);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DymanicHeadSetup ");
        }
        public static IDataReader Gets(TransactionContext tc, bool bActivity)
        {
            return tc.ExecuteReader("SELECT * FROM View_DymanicHeadSetup Where Activity=%b", bActivity);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
