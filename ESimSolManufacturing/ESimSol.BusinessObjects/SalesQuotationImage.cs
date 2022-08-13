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
    #region SalesQuotationImage
    
    public class SalesQuotationImage : BusinessObject
    {
        public SalesQuotationImage()
        {
            SalesQuotationImageID = 0;
            SalesQuotationID = 0;
            ImageTitle = "";
            LargeImage = null;
            SalesQuotationThumbnailID = 0;
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            ImageTypeInInt = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int SalesQuotationImageID { get; set; }
         
        public int SalesQuotationID { get; set; }
         
        public string ImageTitle { get; set; }
         
        public byte[] LargeImage { get; set; }
         
        public int SalesQuotationThumbnailID { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public  EnumImageType ImageType { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<SalesQuotationImage> SalesQuotationImages { get; set; }
        public List<SalesQuotationThumbnail> SalesQuotationThumbnails { get; set; }
        public List<EnumObject> ImageTypeObjs { get; set; }
        public Image TSImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        public System.Drawing.Image BackImage { get; set; }
         
        public int ImageTypeInInt { get; set; }
        #endregion

        #region Functions

        public static List<SalesQuotationImage> Gets(int nSalesQuotationID, long nUserID)
        {
            return SalesQuotationImage.Service.Gets(nSalesQuotationID, nUserID);
        }

        public SalesQuotationImage Get(int id, long nUserID)
        {
            return SalesQuotationImage.Service.Get(id, nUserID);
        }

        public SalesQuotationImage GetFrontImage(int nSalesQuotationID, long nUserID)
        {
            return SalesQuotationImage.Service.GetFrontImage(nSalesQuotationID, nUserID);
        }
        public SalesQuotationImage GetBackImage(int nSalesQuotationID, long nUserID)
        {
            return SalesQuotationImage.Service.GetBackImage(nSalesQuotationID, nUserID);
        }
        public SalesQuotationImage GetImageByType(int nSalesQuotationID,int nType, long nUserID)
        {
            return SalesQuotationImage.Service.GetImageByType(nSalesQuotationID, nType, nUserID);
        }
        public SalesQuotationImage GetLogImageByType(int nSalesQuotationID, int nType, long nUserID)
        {
            return SalesQuotationImage.Service.GetLogImageByType(nSalesQuotationID, nType, nUserID);
        }
        
        public SalesQuotationImage Save(long nUserID)
        {
            return SalesQuotationImage.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return SalesQuotationImage.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISalesQuotationImageService Service
        {
            get { return (ISalesQuotationImageService)Services.Factory.CreateService(typeof(ISalesQuotationImageService)); }
        }
        #endregion
    }
    #endregion

    #region ISalesQuotationImage interface
     
    public interface ISalesQuotationImageService
    {
        SalesQuotationImage Get(int id, Int64 nUserID);
        SalesQuotationImage GetFrontImage(int nSalesQuotationID, Int64 nUserID);
        SalesQuotationImage GetBackImage(int nSalesQuotationID, Int64 nUserID);
        SalesQuotationImage GetImageByType(int nSalesQuotationID, int nType, Int64 nUserID);
        SalesQuotationImage GetLogImageByType(int nSalesQuotationID, int nType, Int64 nUserID);
        List<SalesQuotationImage> Gets(int nSalesQuotationID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        SalesQuotationImage Save(SalesQuotationImage oSalesQuotationImage, Int64 nUserID);
    }
    #endregion
}
