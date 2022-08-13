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
    public class SizeCategoryDA
    {
        public SizeCategoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SizeCategory oSizeCategory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SizeCategory]" + "%n, %s, %n, %s, %n, %n",
                                    oSizeCategory.SizeCategoryID, oSizeCategory.SizeCategoryName, oSizeCategory.Sequence, oSizeCategory.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SizeCategory oSizeCategory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SizeCategory]" + "%n, %s, %n, %s, %n, %n",
                                    oSizeCategory.SizeCategoryID, oSizeCategory.SizeCategoryName, oSizeCategory.Sequence, oSizeCategory.Note, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader ResetSequence(TransactionContext tc, SizeCategory oSizeCategory, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE SizeCategory SET Sequence=%n, DBUserID=%n, DBServerDateTime=%D WHERE SizeCategoryID=%n", oSizeCategory.Sequence, nUserID, DateTime.Now, oSizeCategory.SizeCategoryID);
            return tc.ExecuteReader("SELECT * FROM SizeCategory WHERE SizeCategoryID=%n", oSizeCategory.SizeCategoryID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM SizeCategory WHERE SizeCategoryID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM SizeCategory ORDER BY Sequence ASC");
        }
        public static IDataReader GetsBySizeCategory(TransactionContext tc, string sSizeCategory)
        {
            return tc.ExecuteReader("SELECT * FROM SizeCategory WHERE SizeCategoryName LIKE ('%" + sSizeCategory + "%') ORDER BY Sequence ASC");
        }        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsSizeForQC(TransactionContext tc, int nSaleOrderID)
        {
                                 // SELECT * FROM SizeCategory WHERE SizeCategoryID IN (SELECT DISTINCT SizeID FROM SaleOrderDetail WHERE SaleOrderID=45) ORDER BY SizeCategoryID
            return tc.ExecuteReader("SELECT * FROM SizeCategory WHERE SizeCategoryID IN(SELECT DISTINCT SizeID FROM SaleOrderDetail WHERE SaleOrderID=%n) ORDER BY SizeCategoryID", nSaleOrderID);
        }
        #endregion
    }
}
