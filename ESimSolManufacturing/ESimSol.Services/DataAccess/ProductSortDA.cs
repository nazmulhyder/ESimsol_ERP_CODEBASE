using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class ProductSortDA
    {
        public ProductSortDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductSort oProductSort, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ProductSort]"
                                    + "%n,%n, %n,%n,%n, %n, %n, %n",
                                    oProductSort.ProductSortID, oProductSort.ProductID, oProductSort.SortType, oProductSort.ProductID_Bulk, oProductSort.Qty_Grace, oProductSort.Value, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ProductSort oProductSort, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ProductSort]"
                                     + "%n,%n, %n,%n,%n, %n, %n, %n",
                                   oProductSort.ProductSortID, oProductSort.ProductID, oProductSort.SortType, oProductSort.ProductID_Bulk, oProductSort.Qty_Grace, oProductSort.Value, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductSort WHERE ProductSortID=%n", nID);
        }
        public static IDataReader GetBy(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductSort WHERE ProductID=%n", nID);
        }
        public static IDataReader GetsBy(TransactionContext tc, long nProductID_Bulk)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductSort WHERE ProductID_Bulk=%n", nProductID_Bulk);
        }
     
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductSort");
        }
        public static IDataReader Gets(TransactionContext tc,int nBUID )
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductSort WHERE BUID=%n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      
    
     
    
        #endregion
    }
}