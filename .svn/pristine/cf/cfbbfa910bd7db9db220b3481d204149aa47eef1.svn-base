using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductCategoryPropertyValueDA
    {
        public ProductCategoryPropertyValueDA() { }

        #region Insert Update Delete Function
        public static void Insert(TransactionContext tc, ProductCategoryPropertyValue oPCPV)
        {
            tc.ExecuteNonQuery("INSERT INTO [ProductCategoryPropertyValue]([PCPID],[PropertyValueID],[Note]) VALUES(%n,%n,%s)", oPCPV.PCPID, oPCPV.PropertyValueID, oPCPV.Note);
        }
        public static void Delete(TransactionContext tc, int nPCPVID)
        {
            tc.ExecuteNonQuery("DELETE FROM ProductCategoryPropertyValue WHERE PCPVID=%n", nPCPVID);
        }
        public static void DeleteByPCPID(TransactionContext tc, int nPCPID)
        {
            tc.ExecuteNonQuery("DELETE FROM ProductCategoryPropertyValue WHERE PCPID=%n", nPCPID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ProductCategoryPropertyValue WHERE PCPVID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nPCPID)
        {
            return tc.ExecuteReader("SELECT * FROM ProductCategoryPropertyValue WHERE PCPID=%n", nPCPID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
