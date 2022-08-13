using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportCnfDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportCnf oImportCnf, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportCnf]"
                                    + "%n,%s,%D, %n,%n,%s,%n,%n",
                                    oImportCnf.ImportCnfID, oImportCnf.FileNo, oImportCnf.SendDate, oImportCnf.ContractorID, oImportCnf.ImportInvoiceID, oImportCnf.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ImportCnf oImportCnf, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportCnf]"
                                    + "%n,%s,%D, %n,%n,%s,%n,%n",
                                       oImportCnf.ImportCnfID, oImportCnf.FileNo, oImportCnf.SendDate, oImportCnf.ContractorID, oImportCnf.ImportInvoiceID, oImportCnf.Note, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportCnf");
        }
        public static IDataReader GetBy(TransactionContext tc, int nImportInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportCnf where ImportInvoiceID=%n", nImportInvoiceID);
        }
        public static IDataReader Get(TransactionContext tc, long nImportCnfID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportCnf WHERE ImportCnfID=%n", nImportCnfID);
        }
        #endregion
    }
}
