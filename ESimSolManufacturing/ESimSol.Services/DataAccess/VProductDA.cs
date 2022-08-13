using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class VProductDA
    {
        public VProductDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VProduct oVProduct, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VProduct]" + "%n, %s, %s, %s, %s, %s, %n, %n",
                                    oVProduct.VProductID, oVProduct.ProductCode, oVProduct.ProductName, oVProduct.ShortName, oVProduct.BrandName, oVProduct.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VProduct oVProduct, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VProduct]" + "%n, %s, %s, %s, %s, %s, %n, %n",
                                    oVProduct.VProductID, oVProduct.ProductCode, oVProduct.ProductName, oVProduct.ShortName, oVProduct.BrandName, oVProduct.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VProduct WHERE VProductID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VProduct");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByCodeOrName(TransactionContext tc, VProduct oVProduct)
        {
            return tc.ExecuteReader("SELECT * FROM View_VProduct WHERE NameCode LIKE '%" + oVProduct.NameCode + "%' ORDER BY ProductName");
        }
        #endregion
    }
}