using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class MasterLCMappingDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MasterLCMapping oMasterLCMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MasterLCMapping]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oMasterLCMapping.MasterLCMappingID, oMasterLCMapping.ExportLCID, oMasterLCMapping.MasterLCID, oMasterLCMapping.ContractorID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MasterLCMapping oMasterLCMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MasterLCMapping]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oMasterLCMapping.MasterLCMappingID, oMasterLCMapping.ExportLCID, oMasterLCMapping.MasterLCID, oMasterLCMapping.ContractorID, nUserID,  (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCMapping WHERE MasterLCMappingID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCMapping");
        }
        public static IDataReader Gets(TransactionContext tc, int nELCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCMapping WHERE ExportLCID=%n", nELCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
