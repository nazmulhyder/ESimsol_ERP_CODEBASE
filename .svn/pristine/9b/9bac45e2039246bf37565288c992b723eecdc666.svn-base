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
    public class TechnicalSheetImageDA
    {
        public TechnicalSheetImageDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, TechnicalSheetImage oTechnicalSheetImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oTechnicalSheetImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO TechnicalSheetImage(TechnicalSheetImageID,TechnicalSheetID,ImageType,ImageTitle,LargeImage,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %D)",
                oTechnicalSheetImage.TechnicalSheetImageID, oTechnicalSheetImage.TechnicalSheetID,  (int)oTechnicalSheetImage.ImageType, oTechnicalSheetImage.ImageTitle, "@pic", nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, TechnicalSheetImage oTechnicalSheetImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oTechnicalSheetImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("UPDATE TechnicalSheetImage SET TechnicalSheetID=%n,  ImageType= %n,ImageTitle=%s, LargeImage=%q, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE TechnicalSheetImageID=%n", oTechnicalSheetImage.TechnicalSheetID, (int)oTechnicalSheetImage.ImageType, oTechnicalSheetImage.ImageTitle, "@pic", nUserID, DateTime.Now, oTechnicalSheetImage.TechnicalSheetImageID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nTechnicalSheetID)
        {
            tc.ExecuteNonQuery("UPDATE TechnicalSheetImage SET ImageType=%n WHERE TechnicalSheetID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nTechnicalSheetID, (int)EnumImageType.FrontImage);
        }

        public static void ResetBackPage(TransactionContext tc, int nTechnicalSheetID)
        {
            tc.ExecuteNonQuery("UPDATE TechnicalSheetImage SET ImageType=%n WHERE TechnicalSheetID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nTechnicalSheetID, (int)EnumImageType.BackImage);
        }

        public static void ResetMeasurementSpecPage(TransactionContext tc, int nTechnicalSheetID)
        {
            tc.ExecuteNonQuery("UPDATE TechnicalSheetImage SET ImageType=%n WHERE TechnicalSheetID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nTechnicalSheetID, (int)EnumImageType.MeasurmentSpecImage);
        }

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM TechnicalSheetImage WHERE TechnicalSheetImageID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("TechnicalSheetImage", "TechnicalSheetImageID");
        }
        #endregion
        #endregion

        #region Insert Update Delete Function New System
        //public static IDataReader InsertUpdate(TransactionContext tc, TechnicalSheetImage oTechnicalSheetImage, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        //{
        //    SqlParameter picparameter = new SqlParameter();
        //    picparameter.SqlDbType = SqlDbType.Image;
        //    picparameter.ParameterName = "pic";
        //    picparameter.Value = oTechnicalSheetImage.LargeImage;

        //    return tc.ExecuteReader("EXEC [SP_IUD_TechnicalSheetImage]"
        //                            + "%n, %n, %b, %s, %q, %n, %n",
        //                            oTechnicalSheetImage.TechnicalSheetImageID, oTechnicalSheetImage.TechnicalSheetID, oTechnicalSheetImage.IsCoverPage, oTechnicalSheetImage.ImageTitle, "@pic", nUserID, (int)eEnumDBOperation);
        //}

        //public static void Delete(TransactionContext tc, TechnicalSheetImage oTechnicalSheetImage, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        //{
        //    SqlParameter picparameter = new SqlParameter();
        //    picparameter.SqlDbType = SqlDbType.Image;
        //    picparameter.ParameterName = "pic";
        //    picparameter.Value = oTechnicalSheetImage.LargeImage;

        //    tc.ExecuteNonQuery("EXEC [SP_IUD_TechnicalSheetImage]"
        //                            + "%n, %n, %b, %s, %q, %n, %n",
        //                            oTechnicalSheetImage.TechnicalSheetImageID, oTechnicalSheetImage.TechnicalSheetID, oTechnicalSheetImage.IsCoverPage, oTechnicalSheetImage.ImageTitle, "@pic", nUserID, (int)eEnumDBOperation);
        //}
        #endregion


        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nTechnicalSheetImageID)
        {
            return tc.ExecuteReader("SELECT * FROM TechnicalSheetImage WHERE TechnicalSheetImageID=%n ", nTechnicalSheetImageID);
        }


        public static IDataReader Gets(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetImage WHERE TechnicalSheetID=%n", nTechnicalSheetID);
        }

        public static IDataReader GetFrontImage(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM TechnicalSheetImage WHERE TechnicalSheetID=%n AND ImageType=%n", nTechnicalSheetID, (int)EnumImageType.FrontImage);
        }
        public static IDataReader GetBackImage(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM TechnicalSheetImage WHERE TechnicalSheetID=%n AND ImageType=%n", nTechnicalSheetID, (int)EnumImageType.BackImage);
        }
        #endregion
    }
}
