using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class BrandDA
    {
        public BrandDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Brand oBrand, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Brand]"
                                    + "%n, %s, %s,  %s, %s, %s, %s, %s, %n,  %n",
                                    oBrand.BrandID, oBrand.BrandName, oBrand.BrandCode, oBrand.FaxNo, oBrand.WebAddress, oBrand.EmailAddress, oBrand.ShortName, oBrand.Remarks, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Brand oBrand, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Brand]"
                                    + "%n, %s, %s,  %s, %s, %s, %s, %s, %n,  %n",
                                    oBrand.BrandID, oBrand.BrandName, oBrand.BrandCode, oBrand.FaxNo, oBrand.WebAddress, oBrand.EmailAddress, oBrand.ShortName, oBrand.Remarks, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM Brand WHERE BrandID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Brand");
        }

       
        public static IDataReader GetsByName(TransactionContext tc, string sName)
        {

            return tc.ExecuteReader("SELECT * FROM Brand WHERE BrandName LIKE ('%" + sName + "%')   Order by [BrandName]");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
