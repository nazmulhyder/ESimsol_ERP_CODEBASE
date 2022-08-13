using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductCategoryPropertyDA
    {
        public ProductCategoryPropertyDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, ProductCategoryProperty oPCPI, int nEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductCategoryProperty]"
                                    + "%n,%n,%n,%b,%s,%n",
                                    oPCPI.PCPID, oPCPI.ProductCategoryID, oPCPI.PropertyID, oPCPI.IsMandatory, oPCPI.Note, nEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductCategoryProperty WHERE PCPID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nPCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductCategoryProperty WHERE ProductCategoryID=%n", nPCID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
