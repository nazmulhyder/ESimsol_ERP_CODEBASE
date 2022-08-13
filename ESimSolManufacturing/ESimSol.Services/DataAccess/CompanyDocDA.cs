using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;
using System.Data.SqlClient;


namespace ESimSol.Services.DataAccess
{
    public class CompanyDocDA
    {
        public CompanyDocDA() { }

        #region Insert Update Delete Function
        public static void InsertUpdate(TransactionContext tc, CompanyDoc oCompanyDoc, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oCompanyDoc.AttachmentFile;
            string sSQL = "";
            if(eEnumDBOperation==EnumDBOperation.Update)
            {
                sSQL = SQLParser.MakeSQL("UPDATE CompanyDoc SET CompanyID=%n, Description=%s, IssueDate=%d, ExpireDate=%d,  AttachmentFile=%q, FileType=%s,IsActive=%b DBUserID=%n, DBServerDateTime=%D"
                    + " WHERE CompanyDocID=%n", oCompanyDoc.CompanyID, oCompanyDoc.Description, oCompanyDoc.IssueDate, oCompanyDoc.ExpireDate, "@file", oCompanyDoc.FileType, oCompanyDoc.IsActive, nUserId, DateTime.Now, oCompanyDoc.CompanyDocID);

            }
            else
            {
                sSQL = SQLParser.MakeSQL("INSERT INTO CompanyDoc(CompanyDocID,CompanyID,Description,IssueDate,ExpireDate,AttachmentFile,FileType,IsActive,DBUserID,DBServerDateTime)"
                                  + " VALUES(%n, %n, %s, %d, %d, %q, %s, %b, %n, %D)",
                       oCompanyDoc.CompanyDocID, oCompanyDoc.CompanyID, oCompanyDoc.Description, oCompanyDoc.IssueDate, oCompanyDoc.ExpireDate, "@file", oCompanyDoc.FileType, oCompanyDoc.IsActive, nUserId, DateTime.Now);

            }
  
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        public static void Delete(TransactionContext tc, string sIDs, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Delete from CompanyDoc Where CompanyDocID in ("+sIDs+")");
        }

        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("CompanyDoc", "CompanyDocID");
        }
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM CompanyDoc WHERE CompanyDocID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM CompanyDoc Where CompanyID=%n", nCompanyID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
