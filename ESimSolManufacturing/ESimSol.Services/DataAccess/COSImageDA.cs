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
    public class COSImageDA
    {
        public COSImageDA() { }

        #region Old System
        #region Insert Function
        public static void Insert(TransactionContext tc, COSImage oCOSImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oCOSImage.LargeImage;

            string sSQL = SQLParser.MakeSQL("INSERT INTO COSImage(COSImageID,OperationType,COSVFormat,ImageTitle,LargeImage,DBUserID,DBServerDateTime)"
                + " VALUES(%n, %n, %n, %s, %q, %n, %D)",
                oCOSImage.COSImageID,(int)oCOSImage.OperationType,  (int)oCOSImage.COSVFormat, oCOSImage.ImageTitle, "@pic", nUserID, DateTime.Now);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, COSImage oCOSImage, Int64 nUserID)
        {
            SqlParameter picparameter = new SqlParameter();
            picparameter.SqlDbType = SqlDbType.Image;
            picparameter.ParameterName = "pic";
            picparameter.Value = oCOSImage.LargeImage;
            string sSQL = "";
            if(oCOSImage.LargeImage==null)
            {
                sSQL = SQLParser.MakeSQL("UPDATE COSImage SET OperationType=%n,  COSVFormat= %n,ImageTitle=%s, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE COSImageID=%n", (int)oCOSImage.OperationType, (int)oCOSImage.COSVFormat, oCOSImage.ImageTitle, nUserID, DateTime.Now, oCOSImage.COSImageID);
            }
            else
            {
               sSQL = SQLParser.MakeSQL("UPDATE COSImage SET OperationType=%n,  COSVFormat= %n,ImageTitle=%s, LargeImage=%q, DBUserID=%n, DBServerDateTime=%D"
                + " WHERE COSImageID=%n", (int)oCOSImage.OperationType, (int)oCOSImage.COSVFormat, oCOSImage.ImageTitle, "@pic", nUserID, DateTime.Now, oCOSImage.COSImageID);
            }
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, picparameter);
        }

       

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM COSImage WHERE COSImageID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("COSImage", "COSImageID");
        }
        #endregion
        #endregion

      


        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nCOSImageID)
        {
            return tc.ExecuteReader("SELECT * FROM COSImage WHERE COSImageID=%n ", nCOSImageID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_COSWithoutImage ");
        }
        public static IDataReader GetsByOperationTypeANDCOSFormat(TransactionContext tc, int nOPerationType, int nCOSFormat)
        {
            return tc.ExecuteReader("SELECT top 1 * FROM COSImage WHERE OperationType =%n AND COSVFormat = %n", nOPerationType, nCOSFormat);
        }
        #endregion
    }
}
