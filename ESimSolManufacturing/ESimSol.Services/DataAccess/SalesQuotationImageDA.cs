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
    public class SalesQuotationImageDA
    {
        public SalesQuotationImageDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, SalesQuotationImage oSalesQuotationImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oSalesQuotationImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO SalesQuotationImage(SalesQuotationImageID,SalesQuotationID,ImageType,ImageTitle,LargeImage,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %D)",
                oSalesQuotationImage.SalesQuotationImageID, oSalesQuotationImage.SalesQuotationID,  (int)oSalesQuotationImage.ImageType, oSalesQuotationImage.ImageTitle, "@pic", nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, SalesQuotationImage oSalesQuotationImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oSalesQuotationImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("UPDATE SalesQuotationImage SET SalesQuotationID=%n,  ImageType= %n,ImageTitle=%s, LargeImage=%q, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE SalesQuotationImageID=%n", oSalesQuotationImage.SalesQuotationID, (int)oSalesQuotationImage.ImageType, oSalesQuotationImage.ImageTitle, "@pic", nUserID, DateTime.Now, oSalesQuotationImage.SalesQuotationImageID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nSalesQuotationID)
        {
            tc.ExecuteNonQuery("UPDATE SalesQuotationImage SET ImageType=%n WHERE SalesQuotationID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nSalesQuotationID, (int)EnumImageType.FrontImage);
        }

        public static void ResetBackPage(TransactionContext tc, int nSalesQuotationID)
        {
            tc.ExecuteNonQuery("UPDATE SalesQuotationImage SET ImageType=%n WHERE SalesQuotationID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nSalesQuotationID, (int)EnumImageType.BackImage);
        }

        public static void ResetMeasurementSpecPage(TransactionContext tc, int nSalesQuotationID)
        {
            tc.ExecuteNonQuery("UPDATE SalesQuotationImage SET ImageType=%n WHERE SalesQuotationID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nSalesQuotationID, (int)EnumImageType.MeasurmentSpecImage);
        }

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM SalesQuotationImage WHERE SalesQuotationImageID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("SalesQuotationImage", "SalesQuotationImageID");
        }
        #endregion
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nSalesQuotationImageID)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationImage WHERE SalesQuotationImageID=%n ", nSalesQuotationImageID);
        }

        public static IDataReader Gets(TransactionContext tc, int nSalesQuotationID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesQuotationImage WHERE SalesQuotationID=%n", nSalesQuotationID);
        }

        public static IDataReader GetFrontImage(TransactionContext tc, int nSalesQuotationID)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationImage WHERE SalesQuotationID=%n AND ImageType=%n", nSalesQuotationID, (int)EnumImageType.FrontImage);
        }
        public static IDataReader GetBackImage(TransactionContext tc, int nSalesQuotationID)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationImage WHERE SalesQuotationID=%n AND ImageType=%n", nSalesQuotationID, (int)EnumImageType.BackImage);
        }
        public static IDataReader GetImageByType(TransactionContext tc, int nSalesQuotationID, int nImageType)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationImage WHERE SalesQuotationID=%n AND ImageType=%n", nSalesQuotationID, nImageType);
        }
        public static IDataReader GetLogImageByType(TransactionContext tc, int nSalesQuotationID, int nImageType)
        {
            return tc.ExecuteReader("SELECT * FROM SalesQuotationImageLog WHERE SalesQuotationID=%n AND ImageType=%n", nSalesQuotationID, nImageType);
        }
        #endregion
    }
}
