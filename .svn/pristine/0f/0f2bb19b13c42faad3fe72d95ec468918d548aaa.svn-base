using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Drawing;

namespace ESimSol.BusinessObjects
{
    #region KommFileImage
    
    public class KommFileImage : BusinessObject
    {
        public KommFileImage()
        {
            KommFileImageID = 0;
            KommFileID = 0;
            ImageTitle = "";
            LargeImage = null;
            KommFileThumbnailID = 0;
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            ImageTypeInInt = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int KommFileImageID { get; set; }
         
        public int KommFileID { get; set; }
         
        public string ImageTitle { get; set; }
         
        public byte[] LargeImage { get; set; }
         
        public int KommFileThumbnailID { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public  EnumImageType ImageType { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<KommFileImage> KommFileImages { get; set; }
        public List<KommFileThumbnail> KommFileThumbnails { get; set; }
        public List<EnumObject> ImageTypeObjs { get; set; }
        public Image TSImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        public System.Drawing.Image BackImage { get; set; }
         
        public int ImageTypeInInt { get; set; }
        #endregion

        #region Functions

        public static List<KommFileImage> Gets(int nKommFileID, long nUserID)
        {
            return KommFileImage.Service.Gets(nKommFileID, nUserID);
        }

        public KommFileImage Get(int id, long nUserID)
        {
            return KommFileImage.Service.Get(id, nUserID);
        }

        public KommFileImage GetFrontImage(int nKommFileID, long nUserID)
        {
            return KommFileImage.Service.GetFrontImage(nKommFileID, nUserID);
        }
        public KommFileImage GetBackImage(int nKommFileID, long nUserID)
        {
            return KommFileImage.Service.GetBackImage(nKommFileID, nUserID);
        }
        public KommFileImage GetImageByType(int nKommFileID, int nImageType, long nUserID)
        {
            return KommFileImage.Service.GetImageByType(nKommFileID, nImageType, nUserID);
        }
        
        public KommFileImage Save(long nUserID)
        {
            return KommFileImage.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return KommFileImage.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IKommFileImageService Service
        {
            get { return (IKommFileImageService)Services.Factory.CreateService(typeof(IKommFileImageService)); }
        }
        #endregion
    }
    #endregion

    #region IKommFileImage interface
     
    public interface IKommFileImageService
    {
         
        KommFileImage Get(int id, Int64 nUserID);

        KommFileImage GetFrontImage(int nKommFileID, Int64 nUserID);
        KommFileImage GetImageByType(int nKommFileID, int nType, Int64 nUserID);
        KommFileImage GetBackImage(int nKommFileID, Int64 nUserID);
         
        List<KommFileImage> Gets(int nKommFileID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        KommFileImage Save(KommFileImage oKommFileImage, Int64 nUserID);
    }
    #endregion
}
