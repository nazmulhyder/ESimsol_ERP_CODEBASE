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
    public class BuyerWiseBrandDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BuyerWiseBrand oBuyerWiseBrand, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BuyerWiseBrand]"
                                    + "%n,%n,%n,%n,%n,%b,%s",
                                    oBuyerWiseBrand.BuyerWiseBrandID, oBuyerWiseBrand.BrandID, oBuyerWiseBrand.BuyerID, nUserID, (int)eEnumDBOperation, false, "");
        }

        public static void Delete(TransactionContext tc, BuyerWiseBrand oBuyerWiseBrand, EnumDBOperation eEnumDBOperation, Int64 nUserID, bool IsUserBased, string ids)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BuyerWiseBrand]"
                                    + "%n,%n,%n,%n,%n,%b,%s",
                                    oBuyerWiseBrand.BuyerWiseBrandID, oBuyerWiseBrand.BrandID, oBuyerWiseBrand.BuyerID,  nUserID, (int)eEnumDBOperation, IsUserBased, ids);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BuyerWiseBrand WHERE BuyerWiseBrandID=%n", nID);
        }

        public static IDataReader GetsByBrand(TransactionContext tc , int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_BuyerWiseBrand Where BrandID = " + id);
        }

        public static IDataReader GetsByBuyer(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_BuyerWiseBrand Where BuyerID = " + id);
        }

   

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }  


    
}
