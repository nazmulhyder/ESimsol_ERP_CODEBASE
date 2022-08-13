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
    public class KommFileImageDA
    {
        public KommFileImageDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, KommFileImage oKommFileImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oKommFileImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO KommFileImage(KommFileImageID,KommFileID,ImageType,ImageTitle,LargeImage,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %D)",
                oKommFileImage.KommFileImageID, oKommFileImage.KommFileID,  (int)oKommFileImage.ImageType, oKommFileImage.ImageTitle, "@pic", nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, KommFileImage oKommFileImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oKommFileImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("UPDATE KommFileImage SET KommFileID=%n,  ImageType= %n,ImageTitle=%s, LargeImage=%q, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE KommFileImageID=%n", oKommFileImage.KommFileID, (int)oKommFileImage.ImageType, oKommFileImage.ImageTitle, "@pic", nUserID, DateTime.Now, oKommFileImage.KommFileImageID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nKommFileID)
        {
            tc.ExecuteNonQuery("UPDATE KommFileImage SET ImageType=%n WHERE KommFileID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nKommFileID, (int)EnumImageType.FrontImage);
        }

        public static void ResetBackPage(TransactionContext tc, int nKommFileID)
        {
            tc.ExecuteNonQuery("UPDATE KommFileImage SET ImageType=%n WHERE KommFileID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nKommFileID, (int)EnumImageType.BackImage);
        }

        public static void ResetMeasurementSpecPage(TransactionContext tc, int nKommFileID)
        {
            tc.ExecuteNonQuery("UPDATE KommFileImage SET ImageType=%n WHERE KommFileID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nKommFileID, (int)EnumImageType.MeasurmentSpecImage);
        }

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM KommFileImage WHERE KommFileImageID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("KommFileImage", "KommFileImageID");
        }
        #endregion
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nKommFileImageID)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileImage WHERE KommFileImageID=%n ", nKommFileImageID);
        }

        public static IDataReader Gets(TransactionContext tc, int nKommFileID)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileImage WHERE KommFileID=%n", nKommFileID);
        }

        public static IDataReader GetFrontImage(TransactionContext tc, int nKommFileID)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileImage WHERE KommFileID=%n AND ImageType=%n", nKommFileID, (int)EnumImageType.FrontImage);
        }
        public static IDataReader GetBackImage(TransactionContext tc, int nKommFileID)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileImage WHERE KommFileID=%n AND ImageType=%n", nKommFileID, (int)EnumImageType.BackImage);
        }
        public static IDataReader GetImageByType(TransactionContext tc, int nKommFileID, int nImageType)
        {
            return tc.ExecuteReader("SELECT * FROM KommFileImage WHERE KommFileID=%n AND ImageType=%n", nKommFileID, nImageType);
        }
        #endregion
    }
}
