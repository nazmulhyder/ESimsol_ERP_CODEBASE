using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
	public class AttachDocumentDA 
	{
		#region Insert Update Delete Function
        #region Insert Function

        public static void Insert(TransactionContext tc, AttachDocument oAttachDocument, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oAttachDocument.AttachFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO AttachDocument(AttachDocumentID,RefID,RefType, FileName,AttachFile,FileType,Remarks,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n,%n, %s, %q, %s, %s, %n, %D)",
            oAttachDocument.AttachDocumentID, oAttachDocument.RefID, (int)oAttachDocument.RefType, oAttachDocument.FileName, "@file", oAttachDocument.FileType, oAttachDocument.Remarks, nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, AttachDocument oAttachDocument, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oAttachDocument.AttachFile;
            string sSQL = SQLParser.MakeSQL("UPDATE AttachDocument SET RefID=%n, FileName=%s,AttachFile=%q,FileType=%s,Remarks=%s,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE AttachDocumentID=%n", oAttachDocument.RefID, oAttachDocument.FileName, "@file", oAttachDocument.FileType, oAttachDocument.Remarks, nUserID, DateTime.Now, oAttachDocument.AttachDocumentID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }


        #endregion

		#endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("AttachDocument", "AttachDocumentID");
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM AttachDocument WHERE AttachDocumentID=%n", nID);
        }
        #endregion

		#region Get & Exist Function
        public static IDataReader GetUserSignature(TransactionContext tc, int nSignatureUserID)
		{
            return tc.ExecuteReader("SELECT * FROM AttachDocument WHERE AttachDocumentID = (SELECT TOP 1  HH.AttachDocumentID FROM AttachDocument AS HH WHERE HH.RefType=%n AND HH.RefID=%n)", (int)EnumAttachRefType.UserSignature, nSignatureUserID);
		}
        public static IDataReader GetWithAttachFile(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM AttachDocument WHERE AttachDocumentID=%n", nID);
        }
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM AttachDocument");
		}
        public static IDataReader GetsDataSet(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(int id, int nRefType, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttachDocumentWithOutFile  WHERE RefID = %n AND RefType = %n", id, nRefType);
        }
        public static IDataReader Gets_WithAttachFile(int nRefID, int nRefType, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM AttachDocument  WHERE RefID = %n AND RefType = %n", nRefID, nRefType);
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
