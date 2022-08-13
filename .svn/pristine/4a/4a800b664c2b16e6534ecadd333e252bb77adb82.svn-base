using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ExpenditureHeadMappingDA
    {

        #region 

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ExpenditureHeadMapping oExpenditureHeadMapping, EnumDBOperation eEnumDBExpenditureHeadMapping, Int64 nUserId, string sIIDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExpenditureHeadMapping]"
                                    + "%n,%n,%n,%n,%n,%n,%s",
                                     oExpenditureHeadMapping.ExpenditureHeadMappingID,
                                     oExpenditureHeadMapping.ExpenditureHeadID,
                                     oExpenditureHeadMapping.OperationTypeInt,
                                     oExpenditureHeadMapping.DrCrType,
                                     nUserId,
                                     (int)eEnumDBExpenditureHeadMapping,
                                     sIIDetailIDs
                                     );
        }

        public static void Delete(TransactionContext tc, ExpenditureHeadMapping oExpenditureHeadMapping, EnumDBOperation eEnumDBExpenditureHeadMapping, Int64 nUserId, string sIIDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExpenditureHeadMapping]"
                                    + "%n,%n,%n,%n,%n,%n,%s",
                                    oExpenditureHeadMapping.ExpenditureHeadMappingID,
                                     oExpenditureHeadMapping.ExpenditureHeadID,
                                     oExpenditureHeadMapping.DrCrType,
                                     oExpenditureHeadMapping.OperationTypeInt,
                                     nUserId,
                                     (int)eEnumDBExpenditureHeadMapping,
                                     sIIDetailIDs
                                     );
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExpenditureHeadMapping WHERE ExpenditureHeadMappingID=%n", nID);
        }
        public static IDataReader Gets(int nExpenditureHeadID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ExpenditureHeadMapping where ExpenditureHeadID=%n", nExpenditureHeadID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

        #endregion
    }
}
