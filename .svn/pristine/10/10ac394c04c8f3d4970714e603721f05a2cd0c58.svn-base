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
    public class VehicleModelThumbnailDA
    {
        public VehicleModelThumbnailDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, VehicleModelThumbnail oVehicleModelThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleModelThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO VehicleModelThumbnail(VehicleModelThumbnailID,VehicleModelID,ImageType,ImageTitle,ThumbnailImage,VehicleModelImageID,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %n, %D)",
                oVehicleModelThumbnail.VehicleModelThumbnailID, oVehicleModelThumbnail.VehicleModelID, (int)oVehicleModelThumbnail.ImageType, oVehicleModelThumbnail.ImageTitle, "@pic", oVehicleModelThumbnail.VehicleModelImageID, nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, VehicleModelThumbnail oVehicleModelThumbnail, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleModelThumbnail.ThumbnailImage;

            string sSQL = SQLParser.MakeSQL("UPDATE VehicleModelThumbnail SET VehicleModelID=%n, ImageType=%n,  ImageTitle=%s, ThumbnailImage=%q, VehicleModelImageID=%n, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE VehicleModelThumbnailID=%n", oVehicleModelThumbnail.VehicleModelID, (int)oVehicleModelThumbnail.ImageType,  oVehicleModelThumbnail.ImageTitle, "@pic", oVehicleModelThumbnail.VehicleModelImageID, nUserID, DateTime.Now, oVehicleModelThumbnail.VehicleModelThumbnailID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

        public static void ResetFrontPage(TransactionContext tc, int nVehicleModelID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleModelThumbnail SET ImageType=%n WHERE VehicleModelID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nVehicleModelID, (int)EnumImageType.FrontImage);
        }
        public static void ResetBackPage(TransactionContext tc, int nVehicleModelID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleModelThumbnail SET ImageType=%n WHERE VehicleModelID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nVehicleModelID, (int)EnumImageType.BackImage);
        }
        public static void ResetMeasurementSpecPage(TransactionContext tc, int nVehicleModelID)
        {
            tc.ExecuteNonQuery("UPDATE VehicleModelThumbnail SET ImageType=%n WHERE VehicleModelID=%n AND ImageType = %n", (int)EnumImageType.NormalImage, nVehicleModelID, (int)EnumImageType.MeasurmentSpecImage);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM VehicleModelThumbnail WHERE VehicleModelThumbnailID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("VehicleModelThumbnail", "VehicleModelThumbnailID");
        }
        #endregion
        #endregion

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VehicleModelThumbnail oVehicleModelThumbnail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oVehicleModelThumbnail.ThumbnailImage;

            return tc.ExecuteReader("EXEC [SP_IUD_VehicleModelThumbnail]"
                                    + "%n, %n, %n, %s, %q, %n, %n, %n",
                                    oVehicleModelThumbnail.VehicleModelThumbnailID, oVehicleModelThumbnail.VehicleModelID, (int)oVehicleModelThumbnail.ImageType, oVehicleModelThumbnail.ImageTitle, "@pic", oVehicleModelThumbnail.VehicleModelImageID, nUserID, (int)eEnumDBOperation);
        }

        //public static void Delete(TransactionContext tc, VehicleModelThumbnail oVehicleModelThumbnail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        //{
        //    SqlParameter picparameter = new SqlParameter();
        //    picparameter.SqlDbType = SqlDbType.Image;
        //    picparameter.ParameterName = "pic";
        //    picparameter.Value = oVehicleModelThumbnail.ThumbnailImage;

        //    tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleModelThumbnail]"
        //                            + "%n, %n, %b, %s, %q, %n, %n, %n",
        //                            oVehicleModelThumbnail.VehicleModelThumbnailID, oVehicleModelThumbnail.VehicleModelID, oVehicleModelThumbnail.IsCoverPage, oVehicleModelThumbnail.ImageTitle, "@pic", oVehicleModelThumbnail.VehicleModelImageID, nUserID, (int)eEnumDBOperation);
        //}
        #endregion


        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {

            return tc.ExecuteReader("SELECT * FROM VehicleModelThumbnail WHERE VehicleModelID=%n AND IsCoverPage=%b", id, true);

           // return tc.ExecuteReader("SELECT * FROM View_VehicleModel WHERE VehicleModelID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int nVehicleModelThumbnailID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleModelThumbnail WHERE VehicleModelThumbnailID=%n ", nVehicleModelThumbnailID);
        }

        
        public static IDataReader Gets(TransactionContext tc, int nVehicleModelID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VehicleModelThumbnail WHERE VehicleModelID=%n", nVehicleModelID);
        }


        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetFrontImage(TransactionContext tc, int nVehicleModelID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleModelThumbnail WHERE VehicleModelID=%n AND ImageType=%n", nVehicleModelID, (int)EnumImageType.FrontImage);
        }

        public static IDataReader GetMeasurementSpecImage(TransactionContext tc, int nVehicleModelID)
        {
            return tc.ExecuteReader("SELECT * FROM VehicleModelThumbnail WHERE VehicleModelID=%n AND ImageType =%n", nVehicleModelID, (int)EnumImageType.MeasurmentSpecImage);
        }
        

        #endregion
    }
}
