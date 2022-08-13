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
    public class TechnicalSheetThumbnailDA
    {
        public TechnicalSheetThumbnailDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, TechnicalSheetThumbnail oTechnicalSheetThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oTechnicalSheetThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO TechnicalSheetThumbnail(TechnicalSheetThumbnailID,TechnicalSheetID,ImageType,ImageTitle,ThumbnailImage,TechnicalSheetImageID,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %n, %D)",
                oTechnicalSheetThumbnail.TechnicalSheetThumbnailID, oTechnicalSheetThumbnail.TechnicalSheetID, (int)oTechnicalSheetThumbnail.ImageType, oTechnicalSheetThumbnail.ImageTitle, "@pic", oTechnicalSheetThumbnail.TechnicalSheetImageID, nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, TechnicalSheetThumbnail oTechnicalSheetThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oTechnicalSheetThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("UPDATE TechnicalSheetThumbnail SET TechnicalSheetID=%n, ImageType=%n,  ImageTitle=%s, ThumbnailImage=%q, TechnicalSheetImageID=%n, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE TechnicalSheetThumbnailID=%n", oTechnicalSheetThumbnail.TechnicalSheetID, (int)oTechnicalSheetThumbnail.ImageType,  oTechnicalSheetThumbnail.ImageTitle, "@pic", oTechnicalSheetThumbnail.TechnicalSheetImageID, nUserID, DateTime.Now, oTechnicalSheetThumbnail.TechnicalSheetThumbnailID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nTechnicalSheetID)
        {
            tc.ExecuteNonQuery("UPDATE TechnicalSheetThumbnail SET ImageType=%n WHERE TechnicalSheetID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nTechnicalSheetID, (int)EnumImageType.FrontImage);
        }
        public static void ResetBackPage(TransactionContext tc, int nTechnicalSheetID)
        {
            tc.ExecuteNonQuery("UPDATE TechnicalSheetThumbnail SET ImageType=%n WHERE TechnicalSheetID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nTechnicalSheetID, (int)EnumImageType.BackImage);
        }
        public static void ResetMeasurementSpecPage(TransactionContext tc, int nTechnicalSheetID)
        {
            tc.ExecuteNonQuery("UPDATE TechnicalSheetThumbnail SET ImageType=%n WHERE TechnicalSheetID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nTechnicalSheetID, (int)EnumImageType.MeasurmentSpecImage);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM TechnicalSheetThumbnail WHERE TechnicalSheetThumbnailID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("TechnicalSheetThumbnail", "TechnicalSheetThumbnailID");
        }
        #endregion
        #endregion

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TechnicalSheetThumbnail oTechnicalSheetThumbnail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oTechnicalSheetThumbnail.ThumbnailImage;

            return tc.ExecuteReader("EXEC [SP_IUD_TechnicalSheetThumbnail]"
                                    + "%n, %n, %n, %s, %q, %n, %n, %n",
                                    oTechnicalSheetThumbnail.TechnicalSheetThumbnailID, oTechnicalSheetThumbnail.TechnicalSheetID, (int)oTechnicalSheetThumbnail.ImageType, oTechnicalSheetThumbnail.ImageTitle, "@pic", oTechnicalSheetThumbnail.TechnicalSheetImageID, nUserID, (int)eEnumDBOperation);
        }

        //public static void Delete(TransactionContext tc, TechnicalSheetThumbnail oTechnicalSheetThumbnail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        //{
        //    SqlParameter picparameter = new SqlParameter();
        //    picparameter.SqlDbType = SqlDbType.Image;
        //    picparameter.ParameterName = "pic";
        //    picparameter.Value = oTechnicalSheetThumbnail.ThumbnailImage;

        //    tc.ExecuteNonQuery("EXEC [SP_IUD_TechnicalSheetThumbnail]"
        //                            + "%n, %n, %b, %s, %q, %n, %n, %n",
        //                            oTechnicalSheetThumbnail.TechnicalSheetThumbnailID, oTechnicalSheetThumbnail.TechnicalSheetID, oTechnicalSheetThumbnail.IsCoverPage, oTechnicalSheetThumbnail.ImageTitle, "@pic", oTechnicalSheetThumbnail.TechnicalSheetImageID, nUserID, (int)eEnumDBOperation);
        //}
        #endregion


        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {

            return tc.ExecuteReader("SELECT * FROM TechnicalSheetThumbnail WHERE TechnicalSheetID=%n AND IsCoverPage=%b", id, true);

           // return tc.ExecuteReader("SELECT * FROM View_TechnicalSheet WHERE TechnicalSheetID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int nTechnicalSheetThumbnailID)
        {
            return tc.ExecuteReader("SELECT * FROM TechnicalSheetThumbnail WHERE TechnicalSheetThumbnailID=%n ", nTechnicalSheetThumbnailID);
        }

        
        public static IDataReader Gets(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetThumbnail WHERE TechnicalSheetID=%n", nTechnicalSheetID);
        }


        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetFrontImage(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM TechnicalSheetThumbnail WHERE TechnicalSheetID=%n AND ImageType=%n", nTechnicalSheetID, (int)EnumImageType.FrontImage);
        }

        public static IDataReader GetMeasurementSpecImage(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM TechnicalSheetThumbnail WHERE TechnicalSheetID=%n AND ImageType =%n", nTechnicalSheetID, (int)EnumImageType.MeasurmentSpecImage);
        }
        

        #endregion
    }
}
