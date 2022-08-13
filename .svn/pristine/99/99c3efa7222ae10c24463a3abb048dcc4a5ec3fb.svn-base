using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{

    public class BusinessUnitWiseAccountHeadDA
    {
        public BusinessUnitWiseAccountHeadDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead, EnumDBOperation eEnumDBOperation, Int64 nUserId, string BusinessUnitWiseAccountHeadIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BusinessUnitWiseAccountHead]"
                                    + "%n, %n, %n, %n, %n, %s"
                                    , oBusinessUnitWiseAccountHead.BusinessUnitWiseAccountHeadID, oBusinessUnitWiseAccountHead.BusinessUnitID, oBusinessUnitWiseAccountHead.AccountHeadID, nUserId, (int)eEnumDBOperation, BusinessUnitWiseAccountHeadIDs);
        }

        public static void Delete(TransactionContext tc, BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead, EnumDBOperation eEnumDBOperation, Int64 nUserId, string BusinessUnitWiseAccountHeadIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BusinessUnitWiseAccountHead]"
                                    + "%n, %n, %n, %n, %n, %s"
                                    , oBusinessUnitWiseAccountHead.BusinessUnitWiseAccountHeadID, oBusinessUnitWiseAccountHead.BusinessUnitID, oBusinessUnitWiseAccountHead.AccountHeadID, nUserId, (int)eEnumDBOperation, BusinessUnitWiseAccountHeadIDs);
        }

        public static IDataReader IUDFromCOA(TransactionContext tc, BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead, string sBusinessUnitIDs, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountHeadWiseBusinessUnit]" + "%n, %s, %n",
                                    oBusinessUnitWiseAccountHead.AccountHeadID, sBusinessUnitIDs, nUserId);
        }
        public static void CopyBasicChartOfAccount(TransactionContext tc, int nCompanyID, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_Copy_BasicChartOfAccount]"
                                    + "%n, %n ", nCompanyID, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessUnitWiseAccountHead WHERE BusinessUnitWiseAccountHeadID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessUnitWiseAccountHead");
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessUnitWiseAccountHead where BusinessUnitID=%n ", nBUID);
        }
        public static IDataReader GetsByCOA(TransactionContext tc, int nAHID)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessUnitWiseAccountHead where AccountHeadID=%n ", nAHID);
        }

        public static IDataReader GetsLeftSelectedBusinessUnitWiseAccountHead(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
 
}
