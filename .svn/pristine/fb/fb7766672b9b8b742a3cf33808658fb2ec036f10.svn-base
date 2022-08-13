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
    public class VehicleOrderThumbnailDA
    {
        public VehicleOrderThumbnailDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, VehicleOrderThumbnail oVehicleOrderThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleOrderThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO VehicleOrderThumbnail(VehicleOrderThumbnailID,VehicleOrderID,ImageType,ImageTitle,ThumbnailImage,VehicleOrderImageID,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %n, %D)",
                oVehicleOrderThumbnail.VehicleOrderThumbnailID, oVehicleOrderThumbnail.VehicleOrderID, (int)oVehicleOrderThumbnail.ImageType, oVehicleOrderThumbnail.ImageTitle, "@pic", oVehicleOrderThumbnail.VehicleOrderImageID, nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, VehicleOrderThumbnail oVehicleOrderThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleOrderThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("UPDATE VehicleOrderThumbnail SET VehicleOrderID=%n, ImageType=%n,  ImageTitle=%s, ThumbnailImage=%q, VehicleOrderImageID=%n, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE VehicleOrderThumbnailID=%n", oVehicleOrderThumbnail.VehicleOrderID, (int)oVehicleOrderThumbnail.ImageType,  oVehicleOrderThumbnail.ImageTitle, "@pic", oVehicleOrderThumbnail.VehicleOrderImageID, nUserID, DateTime.Now, oVehicleOrderThumbnail.VehicleOrderThumbnailID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nVehicleOrderID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleOrderThumbnail SET ImageType=%n WHERE VehicleOrderID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nVehicleOrderID, (int)EnumImageType.FrontImage);
        }
        public static void ResetBackPage(TransactionContext tc, int nVehicleOrderID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleOrderThumbnail SET ImageType=%n WHERE VehicleOrderID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nVehicleOrderID, (int)EnumImageType.BackImage);
        }
        public static void ResetMeasurementSpecPage(TransactionContext tc, int nVehicleOrderID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleOrderThumbnail SET ImageType=%n WHERE VehicleOrderID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nVehicleOrderID, (int)EnumImageType.MeasurmentSpecImage);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM VehicleOrderThumbnail WHERE VehicleOrderThumbnailID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("VehicleOrderThumbnail", "VehicleOrderThumbnailID");
        }
        #endregion
        #endregion

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleOrderThumbnail oVehicleOrderThumbnail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleOrderThumbnail.ThumbnailImage;
            return tc.ExecuteReader("EXEC [SP_IUD_VehicleOrderThumbnail]"
                                    + "%n, %n, %n, %s, %q, %n, %n, %n",
                                    oVehicleOrderThumbnail.VehicleOrderThumbnailID, oVehicleOrderThumbnail.VehicleOrderID, (int)oVehicleOrderThumbnail.ImageType, oVehicleOrderThumbnail.ImageTitle, "@pic", oVehicleOrderThumbnail.VehicleOrderImageID, nUserID, (int)eEnumDBOperation);
        }     
        #endregion


        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleOrderThumbnail WHERE VehicleOrderID=%n AND IsCoverPage=%b", id, true);
        }
        public static IDataReader Get(TransactionContext tc, int nVehicleOrderThumbnailID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleOrderThumbnail WHERE VehicleOrderThumbnailID=%n ", nVehicleOrderThumbnailID);
        }
        public static IDataReader Gets(TransactionContext tc, int nVehicleOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleOrderThumbnail WHERE VehicleOrderID=%n", nVehicleOrderID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetFrontImage(TransactionContext tc, int nVehicleOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleOrderThumbnail WHERE VehicleOrderID=%n AND ImageType=%n", nVehicleOrderID, (int)EnumImageType.FrontImage);
        }
        public static IDataReader GetMeasurementSpecImage(TransactionContext tc, int nVehicleOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleOrderThumbnail WHERE VehicleOrderID=%n AND ImageType =%n", nVehicleOrderID, (int)EnumImageType.MeasurmentSpecImage);
        }
        
        #endregion
    }
}
