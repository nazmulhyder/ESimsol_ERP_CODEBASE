using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class ReceivedChequeAttachmentDA
    {
      

        #region Insert Function

        public static void Insert(TransactionContext tc, ReceivedChequeAttachment oReceivedChequeAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oReceivedChequeAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO ReceivedChequeAttachment(ReceivedChequeAttachmentID,ReceivedChequeID,AttatchmentName,AttatchFile,FileType,Remarks,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n, %s, %q, %s, %s, %n, %D)",
            oReceivedChequeAttachment.ReceivedChequeAttachmentID, oReceivedChequeAttachment.ReceivedChequeID, oReceivedChequeAttachment.AttatchmentName, "@file",  oReceivedChequeAttachment.FileType, oReceivedChequeAttachment.Remarks,  nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, ReceivedChequeAttachment oReceivedChequeAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oReceivedChequeAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("UPDATE ReceivedChequeAttachment SET ReceivedChequeID=%n,AttatchmentName=%s,AttatchFile=%q,FileType%s,Remarks=%s,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE ReceivedChequeAttachmentID=%n", oReceivedChequeAttachment.ReceivedChequeID, oReceivedChequeAttachment.AttatchmentName, "@file", oReceivedChequeAttachment.FileType, oReceivedChequeAttachment.Remarks, nUserID, DateTime.Now, oReceivedChequeAttachment.ReceivedChequeAttachmentID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        public static void ResetApproved(TransactionContext tc, ReceivedChequeAttachment oReceivedChequeAttachment, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE ReceivedChequeAttachment  SET IsApproved=0 WHERE ReceivedChequeID=%n", oReceivedChequeAttachment.ReceivedChequeID);
        }

        //public static void Approved(TransactionContext tc, ReceivedChequeAttachment oReceivedChequeAttachment, Int64 nUserID)
        //{
        //    tc.ExecuteNonQuery("UPDATE ReceivedChequeAttachment  SET IsApproved=1, DBUserID=%n,DBServerDateTime=%D WHERE ReceivedChequeAttachmentID=%n",nUserID,   DateTime.Now, oReceivedChequeAttachment.ReceivedChequeAttachmentID);
        //}

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM ReceivedChequeAttachment WHERE ReceivedChequeAttachmentID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ReceivedChequeAttachment", "ReceivedChequeAttachmentID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReceivedChequeAttachment WHERE ReceivedChequeID=%n", id);
        }

        public static IDataReader GetWithAttachFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM ReceivedChequeAttachment WHERE ReceivedChequeAttachmentID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReceivedChequeAttachment WHERE ReceivedChequeAttachmentID=%n", id);
        }
        #endregion
    }
}
