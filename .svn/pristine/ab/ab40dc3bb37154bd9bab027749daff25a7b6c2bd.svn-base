using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class CompanyDocumentDA
    {
        #region Insert Function
        public static void Insert(TransactionContext tc, CompanyDocument oCompanyDocument, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oCompanyDocument.DocFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO CompanyDocument(CDID,CompanyID,Description,FileName,DocFile,FileType,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n, %s, %s, %q, %s, %n, %D)",
            oCompanyDocument.CDID, oCompanyDocument.CompanyID, oCompanyDocument.Description, oCompanyDocument.FileName, "@file", oCompanyDocument.FileType, nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, CompanyDocument oCompanyDocument, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oCompanyDocument.DocFile;

            string sSQL = SQLParser.MakeSQL("UPDATE CompanyDocument SET CompanyID=%n,Description=%s,FileName=%s,DocFile=%q,FileType=%s,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE CompanyDocumentID=%n", oCompanyDocument.CompanyID, oCompanyDocument.Description, oCompanyDocument.FileName, "@file", oCompanyDocument.FileType, nUserID, DateTime.Now, oCompanyDocument.CDID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM CompanyDocument WHERE CDID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("CompanyDocument", "CDID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CompanyDocument");
        }

        public static IDataReader GetWithAttachFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM CompanyDocument WHERE CDID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM CompanyDocument WHERE CDID=%n", id);
        }

        public static IDataReader Gets(string sSql, TransactionContext tc)
        {
            return tc.ExecuteReader(sSql);
        }
        #endregion
    }
}
