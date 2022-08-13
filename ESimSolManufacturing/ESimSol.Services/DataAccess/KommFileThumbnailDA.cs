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
    public class KommFileThumbnailDA
    {
        public KommFileThumbnailDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, KommFileThumbnail oKommFileThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oKommFileThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO KommFileThumbnail(KommFileThumbnailID,KommFileID,ImageType,ImageTitle,ThumbnailImage,KommFileImageID,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %n, %D)",
                oKommFileThumbnail.KommFileThumbnailID, oKommFileThumbnail.KommFileID, (int)oKommFileThumbnail.ImageType, oKommFileThumbnail.ImageTitle, "@pic", oKommFileThumbnail.KommFileImageID, nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, KommFileThumbnail oKommFileThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oKommFileThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("UPDATE KommFileThumbnail SET KommFileID=%n, ImageType=%n,  ImageTitle=%s, ThumbnailImage=%q, KommFileImageID=%n, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE KommFileThumbnailID=%n", oKommFileThumbnail.KommFileID, (int)oKommFileThumbnail.ImageType,  oKommFileThumbnail.ImageTitle, "@pic", oKommFileThumbnail.KommFileImageID, nUserID, DateTime.Now, oKommFileThumbnail.KommFileThumbnailID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nKommFileID)
        {
            tc.ExecuteNonQuery("UPDATE KommFileThumbnail SET ImageType=%n WHERE KommFileID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nKommFileID, (int)EnumImageType.FrontImage);
        }
        public static void ResetBackPage(TransactionContext tc, int nKommFileID)
        {
            tc.ExecuteNonQuery("UPDATE KommFileThumbnail SET ImageType=%n WHERE KommFileID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nKommFileID, (int)EnumImageType.BackImage);
        }
        public static void ResetMeasurementSpecPage(TransactionContext tc, int nKommFileID)
        {
            tc.ExecuteNonQuery("UPDATE KommFileThumbnail SET ImageType=%n WHERE KommFileID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nKommFileID, (int)EnumImageType.MeasurmentSpecImage);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM KommFileThumbnail WHERE KommFileThumbnailID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("KommFileThumbnail", "KommFileThumbnailID");
        }
        #endregion
        #endregion

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, KommFileThumbnail oKommFileThumbnail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oKommFileThumbnail.ThumbnailImage;

            return tc.ExecuteReader("EXEC [SP_IUD_KommFileThumbnail]"
                                    + "%n, %n, %n, %s, %q, %n, %n, %n",
                                    oKommFileThumbnail.KommFileThumbnailID, oKommFileThumbnail.KommFileID, (int)oKommFileThumbnail.ImageType, oKommFileThumbnail.ImageTitle, "@pic", oKommFileThumbnail.KommFileImageID, nUserID, (int)eEnumDBOperation);
        }

        //public static void Delete(TransactionContext tc, KommFileThumbnail oKommFileThumbnail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        //{
        //    SqlParameter picparameter = new SqlParameter();
        //    picparameter.SqlDbType = SqlDbType.Image;
        //    picparameter.ParameterName = "pic";
        //    picparameter.Value = oKommFileThumbnail.ThumbnailImage;

        //    tc.ExecuteNonQuery("EXEC [SP_IUD_KommFileThumbnail]"
        //                            + "%n, %n, %b, %s, %q, %n, %n, %n",
        //                            oKommFileThumbnail.KommFileThumbnailID, oKommFileThumbnail.KommFileID, oKommFileThumbnail.IsCoverPage, oKommFileThumbnail.ImageTitle, "@pic", oKommFileThumbnail.KommFileImageID, nUserID, (int)eEnumDBOperation);
        //}
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {

            return tc.ExecuteReader("SELECT * FROM KommFileThumbnail WHERE KommFileID=%n AND IsCoverPage=%b", id, true);

           // return tc.ExecuteReader("SELECT * FROM KommFile WHERE KommFileID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int nKommFileThumbnailID)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileThumbnail WHERE KommFileThumbnailID=%n ", nKommFileThumbnailID);
        }

        
        public static IDataReader Gets(TransactionContext tc, int nKommFileID)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileThumbnail WHERE KommFileID=%n", nKommFileID);
        }


        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetFrontImage(TransactionContext tc, int nKommFileID)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileThumbnail WHERE KommFileID=%n AND ImageType=%n", nKommFileID, (int)EnumImageType.FrontImage);
        }

        public static IDataReader GetMeasurementSpecImage(TransactionContext tc, int nKommFileID)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileThumbnail WHERE KommFileID=%n AND ImageType =%n", nKommFileID, (int)EnumImageType.MeasurmentSpecImage);
        }
        

        #endregion
    }
}
