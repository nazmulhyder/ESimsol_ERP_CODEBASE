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
    public class VehicleModelImageDA
    {
        public VehicleModelImageDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, VehicleModelImage oVehicleModelImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleModelImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO VehicleModelImage(VehicleModelImageID,VehicleModelID,ImageType,ImageTitle,LargeImage,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %D)",
                oVehicleModelImage.VehicleModelImageID, oVehicleModelImage.VehicleModelID,  (int)oVehicleModelImage.ImageType, oVehicleModelImage.ImageTitle, "@pic", nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, VehicleModelImage oVehicleModelImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleModelImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("UPDATE VehicleModelImage SET VehicleModelID=%n,  ImageType= %n,ImageTitle=%s, LargeImage=%q, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE VehicleModelImageID=%n", oVehicleModelImage.VehicleModelID, (int)oVehicleModelImage.ImageType, oVehicleModelImage.ImageTitle, "@pic", nUserID, DateTime.Now, oVehicleModelImage.VehicleModelImageID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nVehicleModelID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleModelImage SET ImageType=%n WHERE VehicleModelID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nVehicleModelID, (int)EnumImageType.FrontImage);
        }

        public static void ResetBackPage(TransactionContext tc, int nVehicleModelID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleModelImage SET ImageType=%n WHERE VehicleModelID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nVehicleModelID, (int)EnumImageType.BackImage);
        }

        public static void ResetMeasurementSpecPage(TransactionContext tc, int nVehicleModelID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleModelImage SET ImageType=%n WHERE VehicleModelID=%n AND ImageType =%n", (int)EnumImageType.NormalImage, nVehicleModelID, (int)EnumImageType.MeasurmentSpecImage);
        }

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM VehicleModelImage WHERE VehicleModelImageID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("VehicleModelImage", "VehicleModelImageID");
        }
        #endregion
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nVehicleModelImageID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleModelImage WHERE VehicleModelImageID=%n ", nVehicleModelImageID);
        }

        public static IDataReader Gets(TransactionContext tc, int nVehicleModelID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleModelImage WHERE VehicleModelID=%n", nVehicleModelID);
        }

        public static IDataReader GetFrontImage(TransactionContext tc, int nVehicleModelID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleModelImage WHERE VehicleModelID=%n AND ImageType=%n", nVehicleModelID, (int)EnumImageType.FrontImage);
        }
        public static IDataReader GetBackImage(TransactionContext tc, int nVehicleModelID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleModelImage WHERE VehicleModelID=%n AND ImageType=%n", nVehicleModelID, (int)EnumImageType.BackImage);
        }
        public static IDataReader GetImageByType(TransactionContext tc, int nVehicleModelID, int nImageType)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleModelImage WHERE VehicleModelID=%n AND ImageType=%n", nVehicleModelID, nImageType);
        }
        #endregion
    }
}
