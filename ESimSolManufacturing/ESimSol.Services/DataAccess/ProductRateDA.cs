using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductRateDA
    {
        public ProductRateDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductRate oProductRate, EnumDBOperation eDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductRate]"
                                    + "%n, %n, %n, %D, %n, %n, %n",
                                   oProductRate.ProductRateID, oProductRate.ProductID, oProductRate.Rate, oProductRate.ActivationDate, oProductRate.SaleSchemeID, (int)eDBOperation, nUserID);
        }
        public static IDataReader Delete(TransactionContext tc, ProductRate oProductRate, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductRate]"
                                    + "%n, %n, %n, %D, %n, %n, %n",
                                   oProductRate.ProductRateID, oProductRate.ProductID, oProductRate.Rate, oProductRate.ActivationDate, oProductRate.SaleSchemeID, (int)EnumDBOperation.Delete, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductRate WHERE ProductRateID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int ProductID)
        {
            return tc.ExecuteReader("SELECT ProductRate.*, Product.ProductCode, Product.ProductName FROM ProductRate, Product where Product.ProductID=ProductRate.ProductID AND ProductRate.ProductID=%n ORDER BY ActivationDate DESC", ProductID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductRate order by ProductID");
        }
        #endregion
    }
}