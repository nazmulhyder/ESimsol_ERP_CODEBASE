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
    public class EmployeeDocDA
    {
        public EmployeeDocDA() { }

        #region Insert Update Delete Function
        public static void InsertUpdate(TransactionContext tc, EmployeeDoc oEmployeeDoc, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oEmployeeDoc.AttachmentFile;
            string sSQL = "";
            if (eEnumDBOperation == EnumDBOperation.Update)
            {
                sSQL = SQLParser.MakeSQL("UPDATE EmployeeDoc SET EmployeeID=%n,DocType=%n,DocTypeID=%n,AttachmentFile=%q,FileType=%s,Description=%s,IssueDate=%d,ExpireDate=%d,DBUserID=%n,DBServerDateTime=%D"
                    + " WHERE EmployeeDocID=%n", oEmployeeDoc.EmployeeID, oEmployeeDoc.DocType, oEmployeeDoc.DocTypeID, "@file", oEmployeeDoc.FileType, oEmployeeDoc.Description, oEmployeeDoc.IssueDate, oEmployeeDoc.ExpireDate, nUserId, DateTime.Now, oEmployeeDoc.EmployeeDocID);

            }
            else
            {


                sSQL = SQLParser.MakeSQL("INSERT INTO EmployeeDoc(EmployeeDocID,EmployeeID,DocType,DocTypeID,AttachmentFile,FileType,Description,IssueDate,ExpireDate,DBUserID,DBServerDateTime)"
                                  + " VALUES(%n, %n, %n, %n, %q, %s, %s, %d, %d, %n,%D)",
                                   oEmployeeDoc.EmployeeDocID, oEmployeeDoc.EmployeeID, oEmployeeDoc.DocType, oEmployeeDoc.DocTypeID, "@file", oEmployeeDoc.FileType, oEmployeeDoc.Description, oEmployeeDoc.IssueDate, oEmployeeDoc.ExpireDate, nUserId, DateTime.Now);

            }

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        public static void Delete(TransactionContext tc, string sIDs, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Delete from EmployeeDoc Where EmployeeDocID in (" + sIDs + ")");
        }

        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("EmployeeDoc", "EmployeeDocID");
        }
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeDoc WHERE EmployeeDocID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeDoc Where EmployeeID=%n", nEmployeeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
