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
    public class MeasurementSpecAttachmentDA
    {


        #region Insert Function

        public static void Insert(TransactionContext tc, MeasurementSpecAttachment oMeasurementSpecAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oMeasurementSpecAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO MeasurementSpecAttachment(MeasurementSpecAttachmentID,TechnicalSheetID,AttatchmentName, IsMeasurmentSpecAttachment, AttatchFile,  FileType,Remarks,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n, %s, %b, %q, %s, %s, %n, %D)",
            oMeasurementSpecAttachment.MeasurementSpecAttachmentID, oMeasurementSpecAttachment.TechnicalSheetID, oMeasurementSpecAttachment.AttatchmentName, oMeasurementSpecAttachment.IsMeasurmentSpecAttachment,  "@file", oMeasurementSpecAttachment.FileType, oMeasurementSpecAttachment.Remarks, nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, MeasurementSpecAttachment oMeasurementSpecAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oMeasurementSpecAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("UPDATE MeasurementSpecAttachment SET TechnicalSheetID=%n,AttatchmentName=%s,IsMeasurmentSpecAttachment = %b, AttatchFile=%q,FileType%s,Remarks=%s,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE MeasurementSpecAttachmentID=%n", oMeasurementSpecAttachment.TechnicalSheetID, oMeasurementSpecAttachment.AttatchmentName, oMeasurementSpecAttachment.IsMeasurmentSpecAttachment,  "@file", oMeasurementSpecAttachment.FileType, oMeasurementSpecAttachment.Remarks, nUserID, DateTime.Now, oMeasurementSpecAttachment.MeasurementSpecAttachmentID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM MeasurementSpecAttachment WHERE MeasurementSpecAttachmentID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("MeasurementSpecAttachment", "MeasurementSpecAttachmentID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int id, bool bIsMeasurmentSpecAttachment)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpecAttachment WHERE TechnicalSheetID=%n and IsMeasurmentSpecAttachment = %b", id,bIsMeasurmentSpecAttachment);
        }

        public static IDataReader GetWithAttachFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM MeasurementSpecAttachment WHERE MeasurementSpecAttachmentID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeasurementSpecAttachment WHERE MeasurementSpecAttachmentID=%n", id);
        }
        #endregion
    }
}
