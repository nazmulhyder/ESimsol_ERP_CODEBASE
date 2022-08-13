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
    #region TechnicalSheetImage
    
    public class TechnicalSheetImage : BusinessObject
    {
        public TechnicalSheetImage()
        {
            TechnicalSheetImageID = 0;
            TechnicalSheetID = 0;
            ImageTitle = "";
            LargeImage = null;
            TechnicalSheetThumbnailID = 0;
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            ImageTypeInInt = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int TechnicalSheetImageID { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public string ImageTitle { get; set; }
         
        public byte[] LargeImage { get; set; }
         
        public int TechnicalSheetThumbnailID { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public  EnumImageType ImageType { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<TechnicalSheetImage> TechnicalSheetImages { get; set; }
        public List<TechnicalSheetThumbnail> TechnicalSheetThumbnails { get; set; }
        public List<EnumObject> ImageTypeObjs { get; set; }
        public Image TSImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        public System.Drawing.Image BackImage { get; set; }
         
        public int ImageTypeInInt { get; set; }
        #endregion

        #region Functions

        public static List<TechnicalSheetImage> Gets(int nTechnicalSheetID, long nUserID)
        {
            return TechnicalSheetImage.Service.Gets(nTechnicalSheetID, nUserID);
        }

        public TechnicalSheetImage Get(int id, long nUserID)
        {
            return TechnicalSheetImage.Service.Get(id, nUserID);
        }

        public TechnicalSheetImage GetFrontImage(int nTechnicalSheetID, long nUserID)
        {
            return TechnicalSheetImage.Service.GetFrontImage(nTechnicalSheetID, nUserID);
        }
        public TechnicalSheetImage GetBackImage(int nTechnicalSheetID, long nUserID)
        {
            return TechnicalSheetImage.Service.GetBackImage(nTechnicalSheetID, nUserID);
        }
        
        public TechnicalSheetImage Save(long nUserID)
        {
            return TechnicalSheetImage.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return TechnicalSheetImage.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ITechnicalSheetImageService Service
        {
            get { return (ITechnicalSheetImageService)Services.Factory.CreateService(typeof(ITechnicalSheetImageService)); }
        }
        #endregion
    }
    #endregion

    #region ITechnicalSheetImage interface
     
    public interface ITechnicalSheetImageService
    {
         
        TechnicalSheetImage Get(int id, Int64 nUserID);
         
        TechnicalSheetImage GetFrontImage(int nTechnicalSheetID, Int64 nUserID);
         
        TechnicalSheetImage GetBackImage(int nTechnicalSheetID, Int64 nUserID);
         
        List<TechnicalSheetImage> Gets(int nTechnicalSheetID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        TechnicalSheetImage Save(TechnicalSheetImage oTechnicalSheetImage, Int64 nUserID);
    }
    #endregion
}
