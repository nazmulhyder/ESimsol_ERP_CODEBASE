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
    public class BuyerConcernDA
    {
        public BuyerConcernDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BuyerConcern oBuyerConcern, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BuyerConcern]"
                                    + "%n, %n, %s, %s, %s, %s, %n, %n",
                                    oBuyerConcern.BuyerConcernID, oBuyerConcern.BuyerID, oBuyerConcern.ConcernName, oBuyerConcern.ConcernEmail, oBuyerConcern.ConcernAddress, oBuyerConcern.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, BuyerConcern oBuyerConcern, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BuyerConcern]"
                                    + "%n, %n, %s, %s, %s, %s, %n, %n",
                                    oBuyerConcern.BuyerConcernID, oBuyerConcern.BuyerID, oBuyerConcern.ConcernName, oBuyerConcern.ConcernEmail, oBuyerConcern.ConcernAddress, oBuyerConcern.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM BuyerConcern WHERE BuyerConcernID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BuyerConcern");
        }
        public static IDataReader GetsByContractor(TransactionContext tc, int nBuyerID)
        {
            return tc.ExecuteReader("SELECT * FROM BuyerConcern WHERE BuyerID=%n", nBuyerID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
