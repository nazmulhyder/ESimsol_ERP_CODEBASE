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
    public class PurchaseQuotationAttachmentDA
    {
      

        #region Insert Function

        public static void Insert(TransactionContext tc, PurchaseQuotationAttachment oPurchaseQuotationAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oPurchaseQuotationAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO PurchaseQuotationAttachment(PurchaseQuotationAttachmentID,PurchaseQuotationServiceID,AttatchmentName,AttatchFile,FileType,Remarks,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n, %s, %q, %s, %s, %n, %D)",
            oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID, oPurchaseQuotationAttachment.PurchaseQuotationID, oPurchaseQuotationAttachment.AttatchmentName, "@file",  oPurchaseQuotationAttachment.FileType, oPurchaseQuotationAttachment.Remarks,  nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, PurchaseQuotationAttachment oPurchaseQuotationAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oPurchaseQuotationAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("UPDATE PurchaseQuotationAttachment SET PurchaseQuotationServiceID=%n,AttatchmentName=%s,AttatchFile=%q,FileType%s,Remarks=%s,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE PurchaseQuotationAttachmentID=%n", oPurchaseQuotationAttachment.PurchaseQuotationID, oPurchaseQuotationAttachment.AttatchmentName, "@file", oPurchaseQuotationAttachment.FileType, oPurchaseQuotationAttachment.Remarks, nUserID, DateTime.Now, oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        public static void ResetApproved(TransactionContext tc, PurchaseQuotationAttachment oPurchaseQuotationAttachment, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE PurchaseQuotationAttachment  SET IsApproved=0 WHERE PurchaseQuotationServiceID=%n", oPurchaseQuotationAttachment.PurchaseQuotationID);
        }

        //public static void Approved(TransactionContext tc, PurchaseQuotationAttachment oPurchaseQuotationAttachment, Int64 nUserID)
        //{
        //    tc.ExecuteNonQuery("UPDATE PurchaseQuotationAttachment  SET IsApproved=1, DBUserID=%n,DBServerDateTime=%D WHERE PurchaseQuotationAttachmentID=%n",nUserID,   DateTime.Now, oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID);
        //}

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM PurchaseQuotationAttachment WHERE PurchaseQuotationAttachmentID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("PurchaseQuotationAttachment", "PurchaseQuotationAttachmentID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotationAttachment WHERE PurchaseQuotationServiceID=%n", id);
        }

        public static IDataReader GetWithAttachFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM PurchaseQuotationAttachment WHERE PurchaseQuotationAttachmentID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseQuotationAttachment WHERE PurchaseQuotationAttachmentID=%n", id);
        }
        #endregion
    }
}
