using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FabricAttachmentDA
    {
        #region Insert Function

        public static void Insert(TransactionContext tc, FabricAttachment oFabricAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oFabricAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO FabricAttachment(FabricAttachmentID,FabricID,AttatchmentName,AttatchFile,FileType,Remarks,SwatchType,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n, %s, %q, %s, %s, %n, %n, %D)",
            oFabricAttachment.FabricAttachmentID, oFabricAttachment.FabricID, oFabricAttachment.AttatchmentName, "@file", oFabricAttachment.FileType, oFabricAttachment.Remarks, (int)oFabricAttachment.SwatchType, nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, FabricAttachment oFabricAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oFabricAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("UPDATE FabricAttachment SET FabricID=%n,AttatchmentName=%s,AttatchFile=%q,FileType%s,Remarks=%s,SwatchType=%n,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE FabricAttachmentID=%n", oFabricAttachment.FabricID, oFabricAttachment.AttatchmentName, "@file", oFabricAttachment.FileType, oFabricAttachment.Remarks,(int)oFabricAttachment.SwatchType, nUserID, DateTime.Now, oFabricAttachment.FabricAttachmentID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }
      
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM FabricAttachment WHERE FabricAttachmentID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("FabricAttachment", "FabricAttachmentID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricAttachment");
        }

        public static IDataReader GetWithAttachFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM FabricAttachment WHERE FabricAttachmentID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricAttachment WHERE FabricAttachmentID=%n", id);
        }

        public static IDataReader GetsAttachmentByFabric(TransactionContext tc, int nFabricId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricAttachment WHERE FabricID=%n", nFabricId);
        }
        #endregion
    }
}
