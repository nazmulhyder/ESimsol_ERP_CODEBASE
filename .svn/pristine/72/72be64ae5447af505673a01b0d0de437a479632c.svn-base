using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class ImportProductDA
    {
        public ImportProductDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportProduct oImportProduct, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ImportProduct]"
                                    + "%n,%n, %n, %s, %s,%s,%n, %n",
                                    oImportProduct.ImportProductID, oImportProduct.BUID, oImportProduct.ProductTypeInt, oImportProduct.FileName, oImportProduct.Name, oImportProduct.PrintName,    nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ImportProduct oImportProduct, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ImportProduct]"
                                    + "%n,%n, %n, %s, %s,%s,%n, %n",
                                       oImportProduct.ImportProductID, oImportProduct.BUID, oImportProduct.ProductTypeInt, oImportProduct.FileName, oImportProduct.Name, oImportProduct.PrintName, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportProduct WHERE ImportProductID=%n", nID);
        }

        public static IDataReader GetByBU(TransactionContext tc, long nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportProduct WHERE BUID=%n", nBUID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportProduct");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, ImportProduct oImportProduct)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ImportProduct Set Activity=~Activity WHERE ImportProductID=%n", oImportProduct.ImportProductID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ImportProduct WHERE ImportProductID=%n", oImportProduct.ImportProductID);

        }
    
     
    
        #endregion
    }
}