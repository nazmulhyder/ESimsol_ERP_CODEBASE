using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportFormatDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportFormat oImportFormat, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportFormat]" + "%n, %n, %s, %b, %s, %s,%n, %n ",
                                                            oImportFormat.ImportFormatID,
                                                            oImportFormat.ImportFormatType,
                                                            oImportFormat.AttatchmentName,
                                                            oImportFormat.AttatchFile,
                                                            oImportFormat.FileType,
                                                            oImportFormat.Remarks,
                                                            (int)eEnumDBOperation,
                                                            nUserID);
        }

        public static void Delete(TransactionContext tc, ImportFormat oImportFormat, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportFormat]" + "%n, %n, %s, %b, %s, %s,%n, %n ",
                                                            oImportFormat.ImportFormatID,
                                                            oImportFormat.ImportFormatType,
                                                            oImportFormat.AttatchmentName,
                                                            oImportFormat.AttatchFile,
                                                            oImportFormat.FileType,
                                                            oImportFormat.Remarks,
                                                            (int)eEnumDBOperation,
                                                            nUserID);
        }
        #endregion

        #region Update Photo
        public static void UpdatePhoto(TransactionContext tc, ImportFormat oImportFormat)
        {

            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oImportFormat.AttatchFile;

            string sSQL = SQLParser.MakeSQL("UPDATE ImportFormat SET AttatchFile=%q"
                + " WHERE ImportFormatID=%n", "@Photopic", oImportFormat.ImportFormatID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportFormat WHERE ImportFormatID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportFormat");
        }
        public static IDataReader GetByType(TransactionContext tc, EnumImportFormatType eIFT)
        {
            return tc.ExecuteReader("SELECT * FROM ImportFormat WHERE ImportFormatType=%n", (int)eIFT);
        }
        public static IDataReader GetWithAttachFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM ImportFormat WHERE ImportFormatID=%n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
