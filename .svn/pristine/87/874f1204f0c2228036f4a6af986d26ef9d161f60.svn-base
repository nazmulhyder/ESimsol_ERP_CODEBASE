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
    public class VehicleOrderImageDA
    {
        public VehicleOrderImageDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, VehicleOrderImage oVehicleOrderImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleOrderImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO VehicleOrderImage(VehicleOrderImageID,VehicleOrderID,ImageType,ImageTitle,LargeImage,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %D)",
                oVehicleOrderImage.VehicleOrderImageID, oVehicleOrderImage.VehicleOrderID,  (int)oVehicleOrderImage.ImageType, oVehicleOrderImage.ImageTitle, "@pic", nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, VehicleOrderImage oVehicleOrderImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleOrderImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("UPDATE VehicleOrderImage SET VehicleOrderID=%n,  ImageType= %n,ImageTitle=%s, LargeImage=%q, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE VehicleOrderImageID=%n", oVehicleOrderImage.VehicleOrderID, (int)oVehicleOrderImage.ImageType, oVehicleOrderImage.ImageTitle, "@pic", nUserID, DateTime.Now, oVehicleOrderImage.VehicleOrderImageID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nVehicleOrderID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleOrderImage SET ImageType=%n WHERE VehicleOrderID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nVehicleOrderID, (int)EnumImageType.FrontImage);
        }

        public static void ResetBackPage(TransactionContext tc, int nVehicleOrderID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleOrderImage SET ImageType=%n WHERE VehicleOrderID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nVehicleOrderID, (int)EnumImageType.BackImage);
        }

        public static void ResetMeasurementSpecPage(TransactionContext tc, int nVehicleOrderID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleOrderImage SET ImageType=%n WHERE VehicleOrderID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nVehicleOrderID, (int)EnumImageType.MeasurmentSpecImage);
        }

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM VehicleOrderImage WHERE VehicleOrderImageID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("VehicleOrderImage", "VehicleOrderImageID");
        }
        #endregion
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nVehicleOrderImageID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleOrderImage WHERE VehicleOrderImageID=%n ", nVehicleOrderImageID);
        }

        public static IDataReader Gets(TransactionContext tc, int nVehicleOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleOrderImage WHERE VehicleOrderID=%n", nVehicleOrderID);
        }

        public static IDataReader GetFrontImage(TransactionContext tc, int nVehicleOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleOrderImage WHERE VehicleOrderID=%n AND ImageType=%n", nVehicleOrderID, (int)EnumImageType.FrontImage);
        }

        public static IDataReader GetImageByType(TransactionContext tc, int nVehicleOrderID, int nImageType)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleOrderImage WHERE VehicleOrderID=%n AND ImageType=%n", nVehicleOrderID, nImageType);
        }

        public static IDataReader GetBackImage(TransactionContext tc, int nVehicleOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleOrderImage WHERE VehicleOrderID=%n AND ImageType=%n", nVehicleOrderID, (int)EnumImageType.BackImage);
        }
        #endregion
    }
}
