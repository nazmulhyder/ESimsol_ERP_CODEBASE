using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductPropertyInformationDA
    {
        public ProductPropertyInformationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductPropertyInformation oProductPropertyInformation, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductPropertyInformation]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oProductPropertyInformation.ProductPropertyInfoID, oProductPropertyInformation.ProductID, oProductPropertyInformation.BUID, oProductPropertyInformation.PropertyValueID, (int)eEnumDBOperation, nUserId);
        }

        public static void Delete(TransactionContext tc, ProductPropertyInformation oProductPropertyInformation, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductPropertyInformation]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oProductPropertyInformation.ProductPropertyInfoID, oProductPropertyInformation.ProductID, oProductPropertyInformation.BUID, oProductPropertyInformation.PropertyValueID, (int)eEnumDBOperation, nUserId);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductPropertyInformation WHERE ProductPropertyID=%n ", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductPropertyInformation ");
        }
        public static IDataReader Gets(TransactionContext tc, int nProductID, int BUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductPropertyInformation WHERE ProductID=%n AND BUID = %n", nProductID, BUID);
        }
     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}