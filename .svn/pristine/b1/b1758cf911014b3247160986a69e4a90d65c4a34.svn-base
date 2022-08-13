using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class EmployeeDocumentDA
    {
        #region Insert Function
        public static void Insert(TransactionContext tc, EmployeeDocument oEmployeeDocument, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oEmployeeDocument.DocFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO EmployeeDocument(EDID,EmployeeID,FileName,DocFile,DocFileType,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n, %s, %q, %s, %n, %D)",
            oEmployeeDocument.EDID, oEmployeeDocument.EmployeeID, oEmployeeDocument.FileName, "@file", oEmployeeDocument.DocFileType, nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, EmployeeDocument oEmployeeDocument, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oEmployeeDocument.DocFile;

            string sSQL = SQLParser.MakeSQL("UPDATE EmployeeDocument SET EmployeeID=%n,FileName=%s,DocFile=%q,DocFileType=%s,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE EDID=%n", oEmployeeDocument.EmployeeID, oEmployeeDocument.FileName, "@file", oEmployeeDocument.DocFileType, nUserID, DateTime.Now, oEmployeeDocument.EDID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM EmployeeDocument WHERE EDID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("EmployeeDocument", "EDID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeDocument");
        }

        public static IDataReader GetWithAttachFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeDocument WHERE EDID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeDocument WHERE EDID=%n", id);
        }

        public static IDataReader Gets(string sSql, TransactionContext tc)
        {
            return tc.ExecuteReader(sSql);
        }
        #endregion
    }
}

