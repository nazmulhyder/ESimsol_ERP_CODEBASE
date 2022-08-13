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
    #region VehicleModelImage
    
    public class VehicleModelImage : BusinessObject
    {
        public VehicleModelImage()
        {
            VehicleModelImageID = 0;
            VehicleModelID = 0;
            ImageTitle = "";
            LargeImage = null;
            VehicleModelThumbnailID = 0;
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            ImageTypeInInt = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int VehicleModelImageID { get; set; }
         
        public int VehicleModelID { get; set; }
         
        public string ImageTitle { get; set; }
         
        public byte[] LargeImage { get; set; }
         
        public int VehicleModelThumbnailID { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public  EnumImageType ImageType { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<VehicleModelImage> VehicleModelImages { get; set; }
        public List<VehicleModelThumbnail> VehicleModelThumbnails { get; set; }
        public List<EnumObject> ImageTypeObjs { get; set; }
        public Image TSImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        public System.Drawing.Image BackImage { get; set; }
         
        public int ImageTypeInInt { get; set; }
        #endregion

        #region Functions

        public static List<VehicleModelImage> Gets(int nVehicleModelID, long nUserID)
        {
            return VehicleModelImage.Service.Gets(nVehicleModelID, nUserID);
        }

        public VehicleModelImage Get(int id, long nUserID)
        {
            return VehicleModelImage.Service.Get(id, nUserID);
        }

        public VehicleModelImage GetFrontImage(int nVehicleModelID, long nUserID)
        {
            return VehicleModelImage.Service.GetFrontImage(nVehicleModelID, nUserID);
        }
        public VehicleModelImage GetBackImage(int nVehicleModelID, long nUserID)
        {
            return VehicleModelImage.Service.GetBackImage(nVehicleModelID, nUserID);
        }
        public VehicleModelImage GetImageByType(int nVehicleModelID, int nImageType, long nUserID)
        {
            return VehicleModelImage.Service.GetImageByType(nVehicleModelID, nImageType, nUserID);
        }
        
        public VehicleModelImage Save(long nUserID)
        {
            return VehicleModelImage.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return VehicleModelImage.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVehicleModelImageService Service
        {
            get { return (IVehicleModelImageService)Services.Factory.CreateService(typeof(IVehicleModelImageService)); }
        }
        #endregion
    }
    #endregion

    #region IVehicleModelImage interface
     
    public interface IVehicleModelImageService
    {
         
        VehicleModelImage Get(int id, Int64 nUserID);

        VehicleModelImage GetFrontImage(int nVehicleModelID, Int64 nUserID);
        VehicleModelImage GetImageByType(int nVehicleModelID, int nType, Int64 nUserID);
        VehicleModelImage GetBackImage(int nVehicleModelID, Int64 nUserID);
         
        List<VehicleModelImage> Gets(int nVehicleModelID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        VehicleModelImage Save(VehicleModelImage oVehicleModelImage, Int64 nUserID);
    }
    #endregion
}
