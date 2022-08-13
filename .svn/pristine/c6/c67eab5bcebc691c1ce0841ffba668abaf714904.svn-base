using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ContainingProductDA
    {
        public ContainingProductDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ContainingProduct oContainingProduct, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ContainingProduct]" + "%n, %n, %n, %s, %n, %n",
                                    oContainingProduct.ContainingProductID, oContainingProduct.WorkingUnitID, oContainingProduct.ProductCategoryID, oContainingProduct.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ContainingProduct oContainingProduct, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ContainingProduct]" + "%n, %n, %n, %s, %n, %n",
                                    oContainingProduct.ContainingProductID, oContainingProduct.WorkingUnitID, oContainingProduct.ProductCategoryID, oContainingProduct.Remarks, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContainingProduct WHERE ContainingProductID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContainingProduct");
        }
        public static IDataReader GetsByWU(TransactionContext tc, int nWUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContainingProduct WHERE WorkingUnitID=%n", nWUID);
        }   
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
