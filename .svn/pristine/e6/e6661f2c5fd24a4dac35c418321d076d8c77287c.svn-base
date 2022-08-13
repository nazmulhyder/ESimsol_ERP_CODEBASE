using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PropertyValueDA
    {
        public PropertyValueDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PropertyValue oPropertyValue, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PropertyValue]"
                                    + "%n, %n, %s, %s, %n, %n",
                                    oPropertyValue.PropertyValueID, (int)oPropertyValue.PropertyType, oPropertyValue.Remarks, oPropertyValue.ValueOfProperty, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, PropertyValue oPropertyValue, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PropertyValue]"
                                    + "%n, %n, %s, %s, %n, %n",
                                    oPropertyValue.PropertyValueID, (int)oPropertyValue.PropertyType, oPropertyValue.Remarks, oPropertyValue.ValueOfProperty, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM PropertyValue WHERE PropertyValueID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PropertyValue Order By PropertyType");
        }
        public static IDataReader GetsByProperty(TransactionContext tc, int nPropertyID)
        {
            return tc.ExecuteReader("SELECT * FROM PropertyValue WHERE PropertyID=%n", nPropertyID);
        }

        public static IDataReader GetsByPropertyValue(TransactionContext tc, string sValueOfProperty, int nPropertytype)
        {
            return tc.ExecuteReader("SELECT * FROM PropertyValue WHERE ValueOfProperty LIKE ('%" + sValueOfProperty + "%')  AND PropertyType= " + nPropertytype + " Order by [ValueOfProperty]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
