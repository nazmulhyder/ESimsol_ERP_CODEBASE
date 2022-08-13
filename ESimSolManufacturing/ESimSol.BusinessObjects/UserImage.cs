using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region UserImage
    public class UserImage : BusinessObject
    {
        public UserImage()
        {
            UserImageID = 0;
            UserID = 0;
            ImageType = EnumUserImageType.None;
            ImageFile = null;
            ErrorMessage = "";

        }

        #region Properties
        public int UserImageID { get; set; }
        public int UserID { get; set; }
        public EnumUserImageType ImageType { get; set; }
        public byte[] ImageFile { get; set; }

        #endregion

        #region Derive Property

        public string ErrorMessage { get; set; }
        public List<UserImage> UserImages { get; set; }
        public Company Company { get; set; }

        public int ImageTypeInt { get; set; }
        public string ImageTypeString { get { return ImageType.ToString();} }


        #endregion

    #endregion


        #region Functions

        public UserImage Get(int nId, long nUserID)
        {
            return UserImage.Service.Get(nId, nUserID);
        }
        public UserImage GetbyUser(int nUserImageID, int ImageType, long nUserID)
        {
            return UserImage.Service.GetbyUser(nUserImageID, ImageType, nUserID);
        }
        public static List<UserImage> Gets(int nUserID_Im, long nUserID)
        {
            return UserImage.Service.Gets(nUserID_Im, nUserID);
        }
        public static List<UserImage> Gets(string sSQL, long nUserID)
        {
            return UserImage.Service.Gets(sSQL, nUserID);
        }
        public UserImage IUD(int nDBOperation, long nUserID)
        {
            return UserImage.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion
        #region ServiceFactory
     
        internal static IUserImageService Service
        {
            get { return (IUserImageService)Services.Factory.CreateService(typeof(IUserImageService)); }
        }
        #endregion
    }

    #region IUserImage interface
    public interface IUserImageService
    {
        UserImage Get(int id, Int64 nUserID);
        UserImage GetbyUser(int nUserImageID, int ImageType, Int64 nUserID);
        List<UserImage> Gets(int nUserID_Im, Int64 nUserID);
        List<UserImage> Gets(string sSQL, Int64 nUserID);
        UserImage IUD(UserImage oUserImage, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
