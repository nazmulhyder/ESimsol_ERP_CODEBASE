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
    public class KnitDyeingProgramAttachmentDA 
	{
		#region Insert Update Delete Function
        #region Insert Function

        public static void Insert(TransactionContext tc, KnitDyeingProgramAttachment oKnitDyeingProgramAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oKnitDyeingProgramAttachment.AttachFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO KnitDyeingProgramAttachment(KnitDyeingProgramAttachmentID,KnitDyeingProgramID, FileName,AttachFile,FileType,Remarks,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n, %s, %q, %s, %s, %n, %D)",
            oKnitDyeingProgramAttachment.KnitDyeingProgramAttachmentID, oKnitDyeingProgramAttachment.KnitDyeingProgramID, oKnitDyeingProgramAttachment.FileName, "@file", oKnitDyeingProgramAttachment.FileType, oKnitDyeingProgramAttachment.Remarks, nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, KnitDyeingProgramAttachment oKnitDyeingProgramAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oKnitDyeingProgramAttachment.AttachFile;
            string sSQL = SQLParser.MakeSQL("UPDATE KnitDyeingProgramAttachment SET KnitDyeingProgramID=%n, FileName=%s,AttachFile=%q,FileType=%s,Remarks=%s,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE KnitDyeingProgramAttachmentID=%n", oKnitDyeingProgramAttachment.KnitDyeingProgramID, oKnitDyeingProgramAttachment.FileName, "@file", oKnitDyeingProgramAttachment.FileType, oKnitDyeingProgramAttachment.Remarks, nUserID, DateTime.Now, oKnitDyeingProgramAttachment.KnitDyeingProgramAttachmentID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }


        #endregion

		#endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("KnitDyeingProgramAttachment", "KnitDyeingProgramAttachmentID");
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM KnitDyeingProgramAttachment WHERE KnitDyeingProgramAttachmentID=%n", nID);
        }
        #endregion

		#region Get & Exist Function
        public static IDataReader Gets(int id,  TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingProgramAttachmentWithOutFile  WHERE KnitDyeingProgramID = %n", id);
        }

        public static IDataReader GetWithAttachFile(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM KnitDyeingProgramAttachment WHERE KnitDyeingProgramAttachmentID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        } 
		#endregion
	}

}
