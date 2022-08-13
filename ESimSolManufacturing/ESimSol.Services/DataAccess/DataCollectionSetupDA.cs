using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DataCollectionSetupDA
    {
        public DataCollectionSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DataCollectionSetup oDataCollectionSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sDataCollectionSetupIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DataCollectionSetup]" + "%n, %n, %n, %n, %n, %s, %s, %s, %s, %n, %n, %s", 
                                    oDataCollectionSetup.DataCollectionSetupID, oDataCollectionSetup.DataReferenceTypeInInt, oDataCollectionSetup.DataReferenceID, oDataCollectionSetup.DataSetupTypeInInt, oDataCollectionSetup.DataGenerateTypeInInt, oDataCollectionSetup.QueryForValue, oDataCollectionSetup.ReferenceValueFields, oDataCollectionSetup.FixedText, oDataCollectionSetup.Note, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, DataCollectionSetup oDataCollectionSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sDataCollectionSetupIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DataCollectionSetup]" + "%n, %n, %n, %n, %n, %s, %s, %s, %s, %n, %n, %s",
                                    oDataCollectionSetup.DataCollectionSetupID, oDataCollectionSetup.DataReferenceTypeInInt, oDataCollectionSetup.DataReferenceID, oDataCollectionSetup.DataSetupTypeInInt, oDataCollectionSetup.DataGenerateTypeInInt, oDataCollectionSetup.QueryForValue, oDataCollectionSetup.ReferenceValueFields, oDataCollectionSetup.FixedText, oDataCollectionSetup.Note, nUserID, (int)eEnumDBOperation, sDataCollectionSetupIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DataCollectionSetup WHERE DataCollectionSetupID=%n", nID);
        }

        public static IDataReader Gets( TransactionContext tc, int nDataReferenceID, int nDataReferenceType)
        {
            return tc.ExecuteReader("SELECT * FROM DataCollectionSetup where DataReferenceID =%n AND DataReferenceType=%n", nDataReferenceID, nDataReferenceType);
        }
        public static IDataReader GetsByIntegrationSetup(TransactionContext tc, int nIntegrationSetupID, int nDataReferenceType)
        {
            if (nDataReferenceType == (int)EnumDataReferenceType.IntegrationDetail)
            {
                return tc.ExecuteReader("SELECT * FROM DataCollectionSetup WHERE DataReferenceType = %n AND DataReferenceID IN (SELECT IntegrationSetupDetailID FROM IntegrationSetupDetail WHERE IntegrationSetupID = %n)", nDataReferenceType ,nIntegrationSetupID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM DataCollectionSetup WHERE DataReferenceType = %n AND DataReferenceID IN (SELECT DebitCreditSetupID FROM DebitCreditSetup WHERE IntegrationSetupDetailID IN (SELECT IntegrationSetupDetailID FROM IntegrationSetupDetail WHERE IntegrationSetupID = %n)) ",  nDataReferenceType , nIntegrationSetupID);
            }
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
