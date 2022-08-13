using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportPIReferenceDA
    {
        public ImportPIReferenceDA() { }
        #region New Version By Mohammad Shahjada Sagor
        public static IDataReader InsertUpdate(TransactionContext tc, ImportPIReference oImportPIReference, EnumDBOperation eEnumDBImportPIReference, Int64 nUserId, string sIPDIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPIReference]"
                                    + "%n, %n,%n,%n,%n,%n,%n,%n,%s",
                                   oImportPIReference.ImportPIReferenceID, oImportPIReference.ImportPIID, oImportPIReference.ReferenceID, oImportPIReference.Amount, oImportPIReference.ConvertionRate, oImportPIReference.AmountInBaseCurrency,   nUserId, (int)eEnumDBImportPIReference, sIPDIDs);
        }

        public static void Delete(TransactionContext tc, ImportPIReference oImportPIReference, EnumDBOperation eEnumDBImportPIReference, Int64 nUserId, string sIPDIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPIReference]"
                                    + "%n, %n,%n,%n,%n,%n,%n,%n,%s",
                                   oImportPIReference.ImportPIReferenceID, oImportPIReference.ImportPIID, oImportPIReference.ReferenceID, oImportPIReference.Amount, oImportPIReference.ConvertionRate, oImportPIReference.AmountInBaseCurrency, nUserId, (int)eEnumDBImportPIReference, sIPDIDs);
        }

       

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIReference WHERE ImportPIReferenceID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIReference");
        }
        public static IDataReader Gets(TransactionContext tc, int nImportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIReference WHERE ImportPIID=%n", nImportPIID);
        }
         
        public static IDataReader GetsByImportPIID(TransactionContext tc, int nImportPIId)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIReference WHERE ImportPIID = %n", nImportPIId);
        }     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        


        #endregion
    }
}
