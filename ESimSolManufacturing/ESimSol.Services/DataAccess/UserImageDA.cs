using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class UserImageDA
    {
        public UserImageDA() { }

        #region Insert Update Delete Function

        public static string IUD(TransactionContext tc, UserImage oUserImage, Int64 nUserID, int nDBOperation)
        {

            
            string sSQL  = "";
            string sErrorMessage = "";
            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oUserImage.ImageFile;

            object oExistCount = tc.ExecuteScalar("SELECT COUNT(*) FROM UserImage WHERE UserID=%n AND ImageType=%n", oUserImage.UserID,(int)oUserImage.ImageType);
            int m = 0;

            if (oExistCount != null && oExistCount.ToString() != "")
            {
                m = Convert.ToInt32(oExistCount);
            }
          

            if (nDBOperation == 1)
            {
                if (m > 0)
                {
                    sErrorMessage = oUserImage.ImageType.ToString()+" already exist!";
                }
                else if (oUserImage.ImageFile != null && oUserImage.ImageType != EnumUserImageType.None)
                {

                    object obj = tc.ExecuteScalar("SELECT MAX(UserImageID)  FROM UserImage");
                    int n = 0;

                    if (obj != null && obj.ToString() != "")
                    {
                        n = Convert.ToInt32(obj);
                    }
                    oUserImage.UserImageID = n + 1;

                    sSQL = SQLParser.MakeSQL("INSERT INTO UserImage(UserImageID,UserID,ImageType,ImageFile,DBUserID,DBServerDateTime)"
                         + "VALUES(%n,%n,%n,%q,%n,%D)", oUserImage.UserImageID, oUserImage.UserID, (int)oUserImage.ImageType, "@Photopic", nUserID, DateTime.Now);

                    tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
                }
                else
                {
                    if (oUserImage.ImageType == EnumUserImageType.None)
                    {
                        sErrorMessage = "Please enter Image Type!";
                    }
                    else if (oUserImage.ImageFile == null)
                    {
                        sErrorMessage = "Please enter Image!";
                    }
                }
            }
            else if (nDBOperation == 2)
            {
                if (oUserImage.ImageFile != null && oUserImage.ImageType != EnumUserImageType.None)
                {
                    sSQL = SQLParser.MakeSQL("UPDATE UserImage SET ImageType=%n,ImageFile=%q WHERE UserImageID=%n", (int)oUserImage.ImageType, "@Photopic", oUserImage.UserImageID);
                    tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
                }
                else if (oUserImage.ImageFile != null)
                {
                    sSQL = SQLParser.MakeSQL("UPDATE UserImage SET ImageFile=%q WHERE UserImageID=%n", "@Photopic", oUserImage.UserImageID);
                    tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
                }
                else if (oUserImage.ImageType != EnumUserImageType.None)
                {
                    sSQL = SQLParser.MakeSQL("UPDATE UserImage SET ImageType=%n WHERE UserImageID=%n", (int)oUserImage.ImageType, oUserImage.UserImageID);
                    tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
                }
                else if (oUserImage.ImageType == EnumUserImageType.None)
                {
                    sErrorMessage = "Please enter Image Type!";
                }
              
            }
            else if (nDBOperation == 3)
            {
                sSQL = SQLParser.MakeSQL("DELETE FROM UserImage WHERE UserImageID=" + oUserImage.UserImageID);
                tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo); 
            }

            return sErrorMessage;
        }
    
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nUserImageID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM UserImage WHERE UserImageID=%n", nUserImageID);
        }
        public static IDataReader GetbyUser(int nUserImageID,int ImageType, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM UserImage WHERE UserID=%n and ImageType=%n ", nUserImageID, ImageType);
        }
     
        public static IDataReader Gets(int nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM UserImage WHERE UserID =" + nUserID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
