using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PropertyDA
    {
        public PropertyDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Property oProperty, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Property]"
                                    + "%n, %s, %s, %b, %n, %n, %n ",
                                    oProperty.PropertyID, oProperty.PropertyName, oProperty.Note, oProperty.IsMandatory, oProperty.ProductCategoryID, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Property oProperty, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Property]"
                                    + "%n, %s, %s, %b, %n, %n, %n ",
                                    oProperty.PropertyID, oProperty.PropertyName, oProperty.Note, oProperty.IsMandatory, oProperty.ProductCategoryID, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * from  View_Property WHERE PropertyID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * from  View_Property");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
