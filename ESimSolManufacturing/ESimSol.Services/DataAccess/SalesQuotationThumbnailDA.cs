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
    public class SalesQuotationThumbnailDA
    {
        public SalesQuotationThumbnailDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, SalesQuotationThumbnail oSalesQuotationThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oSalesQuotationThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO SalesQuotationThumbnail(SalesQuotationThumbnailID,SalesQuotationID,ImageType,ImageTitle,ThumbnailImage,SalesQuotationImageID,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %n, %D)",
                oSalesQuotationThumbnail.SalesQuotationThumbnailID, oSalesQuotationThumbnail.SalesQuotationID, (int)oSalesQuotationThumbnail.ImageType, oSalesQuotationThumbnail.ImageTitle, "@pic", oSalesQuotationThumbnail.SalesQuotationImageID, nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, SalesQuotationThumbnail oSalesQuotationThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oSalesQuotationThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("UPDATE SalesQuotationThumbnail SET SalesQuotationID=%n, ImageType=%n,  ImageTitle=%s, ThumbnailImage=%q, SalesQuotationImageID=%n, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE SalesQuotationThumbnailID=%n", oSalesQuotationThumbnail.SalesQuotationID, (int)oSalesQuotationThumbnail.ImageType,  oSalesQuotationThumbnail.ImageTitle, "@pic", oSalesQuotationThumbnail.SalesQuotationImageID, nUserID, DateTime.Now, oSalesQuotationThumbnail.SalesQuotationThumbnailID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nSalesQuotationID)
        {
            tc.ExecuteNonQuery("UPDATE SalesQuotationThumbnail SET ImageType=%n WHERE SalesQuotationID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nSalesQuotationID, (int)EnumImageType.FrontImage);
        }
        public static void ResetBackPage(TransactionContext tc, int nSalesQuotationID)
        {
            tc.ExecuteNonQuery("UPDATE SalesQuotationThumbnail SET ImageType=%n WHERE SalesQuotationID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nSalesQuotationID, (int)EnumImageType.BackImage);
        }
        public static void ResetMeasurementSpecPage(TransactionContext tc, int nSalesQuotationID)
        {
            tc.ExecuteNonQuery("UPDATE SalesQuotationThumbnail SET ImageType=%n WHERE SalesQuotationID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nSalesQuotationID, (int)EnumImageType.MeasurmentSpecImage);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM SalesQuotationThumbnail WHERE SalesQuotationThumbnailID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("SalesQuotationThumbnail", "SalesQuotationThumbnailID");
        }
        #endregion
        #endregion

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SalesQuotationThumbnail oSalesQuotationThumbnail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oSalesQuotationThumbnail.ThumbnailImage;
            return tc.ExecuteReader("EXEC [SP_IUD_SalesQuotationThumbnail]"
                                    + "%n, %n, %n, %s, %q, %n, %n, %n",
                                    oSalesQuotationThumbnail.SalesQuotationThumbnailID, oSalesQuotationThumbnail.SalesQuotationID, (int)oSalesQuotationThumbnail.ImageType, oSalesQuotationThumbnail.ImageTitle, "@pic", oSalesQuotationThumbnail.SalesQuotationImageID, nUserID, (int)eEnumDBOperation);
        }     
        #endregion


        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationThumbnail WHERE SalesQuotationID=%n AND IsCoverPage=%b", id, true);
        }
        public static IDataReader Get(TransactionContext tc, int nSalesQuotationThumbnailID)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationThumbnail WHERE SalesQuotationThumbnailID=%n ", nSalesQuotationThumbnailID);
        }
        public static IDataReader Gets(TransactionContext tc, int nSalesQuotationID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesQuotationThumbnail WHERE SalesQuotationID=%n", nSalesQuotationID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetFrontImage(TransactionContext tc, int nSalesQuotationID)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationThumbnail WHERE SalesQuotationID=%n AND ImageType=%n", nSalesQuotationID, (int)EnumImageType.FrontImage);
        }
        public static IDataReader GetMeasurementSpecImage(TransactionContext tc, int nSalesQuotationID)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationThumbnail WHERE SalesQuotationID=%n AND ImageType =%n", nSalesQuotationID, (int)EnumImageType.MeasurmentSpecImage);
        }
        
        #endregion
    }
}
