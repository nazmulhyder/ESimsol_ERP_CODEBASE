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
    #region VehicleOrderImage
    
    public class VehicleOrderImage : BusinessObject
    {
        public VehicleOrderImage()
        {
            VehicleOrderImageID = 0;
            VehicleOrderID = 0;
            ImageTitle = "";
            LargeImage = null;
            VehicleOrderThumbnailID = 0;
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            ImageTypeInInt = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int VehicleOrderImageID { get; set; }
         
        public int VehicleOrderID { get; set; }
         
        public string ImageTitle { get; set; }
         
        public byte[] LargeImage { get; set; }
         
        public int VehicleOrderThumbnailID { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public  EnumImageType ImageType { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<VehicleOrderImage> VehicleOrderImages { get; set; }
        public List<VehicleOrderThumbnail> VehicleOrderThumbnails { get; set; }
        public List<EnumObject> ImageTypeObjs { get; set; }
        public Image TSImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        public System.Drawing.Image BackImage { get; set; }
         
        public int ImageTypeInInt { get; set; }
        #endregion

        #region Functions

        public static List<VehicleOrderImage> Gets(int nVehicleOrderID, long nUserID)
        {
            return VehicleOrderImage.Service.Gets(nVehicleOrderID, nUserID);
        }

        public VehicleOrderImage Get(int id, long nUserID)
        {
            return VehicleOrderImage.Service.Get(id, nUserID);
        }

        public VehicleOrderImage GetFrontImage(int nVehicleOrderID, long nUserID)
        {
            return VehicleOrderImage.Service.GetFrontImage(nVehicleOrderID, nUserID);
        }
        public VehicleOrderImage GetBackImage(int nVehicleOrderID, long nUserID)
        {
            return VehicleOrderImage.Service.GetBackImage(nVehicleOrderID, nUserID);
        }

        public VehicleOrderImage GetImageByType(int nVehicleOrderID, int nEnumImageType,  long nUserID)
        {
            return VehicleOrderImage.Service.GetImageByType(nVehicleOrderID, nEnumImageType, nUserID);
        }
        public VehicleOrderImage Save(long nUserID)
        {
            return VehicleOrderImage.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return VehicleOrderImage.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVehicleOrderImageService Service
        {
            get { return (IVehicleOrderImageService)Services.Factory.CreateService(typeof(IVehicleOrderImageService)); }
        }
        #endregion
    }
    #endregion

    #region IVehicleOrderImage interface
     
    public interface IVehicleOrderImageService
    {
         
        VehicleOrderImage Get(int id, Int64 nUserID);
         
        VehicleOrderImage GetFrontImage(int nVehicleOrderID, Int64 nUserID);
         
        VehicleOrderImage GetBackImage(int nVehicleOrderID, Int64 nUserID);

        VehicleOrderImage GetImageByType(int nVehicleOrderID, int nType, Int64 nUserID);
        
        List<VehicleOrderImage> Gets(int nVehicleOrderID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        VehicleOrderImage Save(VehicleOrderImage oVehicleOrderImage, Int64 nUserID);
    }
    #endregion
}
