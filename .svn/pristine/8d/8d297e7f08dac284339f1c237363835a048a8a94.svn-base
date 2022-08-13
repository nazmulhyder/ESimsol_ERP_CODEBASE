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

    #region VehicleModelThumbnail
    
    public class VehicleModelThumbnail : BusinessObject
    {
        public VehicleModelThumbnail()
        {
            VehicleModelThumbnailID = 0;
            VehicleModelID = 0;
            ImageTitle = "";
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            ImageTypeInInt = 0;
            VehicleModelImageID = 0;
        }

        #region Properties
         
        public int VehicleModelThumbnailID { get; set; }
         
        public int VehicleModelID { get; set; }   
         
        public string ImageTitle { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public int VehicleModelImageID { get; set; }
         
        public EnumImageType ImageType { get; set; }
        public int ImageTypeInInt { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public Image ThumImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        #endregion

        #region Functions
        public static List<VehicleModelThumbnail> Gets_Report(int id, long nUserID)
        {
            return VehicleModelThumbnail.Service.Gets_Report(id, nUserID);
        }
        public static List<VehicleModelThumbnail> Gets(int nVehicleModelID, long nUserID)
        {
            return VehicleModelThumbnail.Service.Gets(nVehicleModelID, nUserID);
        }

        public VehicleModelThumbnail Get(int id, long nUserID)
        {           
            return VehicleModelThumbnail.Service.Get(id, nUserID);
        }

        public static List<VehicleModelThumbnail> Gets(string sSQL, long nUserID)
        {
            return VehicleModelThumbnail.Service.Gets(sSQL, nUserID);
        }

        public VehicleModelThumbnail GetFrontImage(int nVehicleModelID, long nUserID)
        {
            return VehicleModelThumbnail.Service.GetFrontImage(nVehicleModelID, nUserID);
        }

        public VehicleModelThumbnail GetMeasurementSpecImage(int nVehicleModelID, long nUserID)
        {
            return VehicleModelThumbnail.Service.GetMeasurementSpecImage(nVehicleModelID, nUserID);
        }
        public VehicleModelThumbnail Save(long nUserID)
        {
            return VehicleModelThumbnail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return VehicleModelThumbnail.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IVehicleModelThumbnailService Service
        {
            get { return (IVehicleModelThumbnailService)Services.Factory.CreateService(typeof(IVehicleModelThumbnailService)); }
        }

        #endregion
    }
    #endregion
        
    #region IVehicleModelThumbnail interface
     
    public interface IVehicleModelThumbnailService
    {
         
        VehicleModelThumbnail Get(int id, Int64 nUserID);
         
        List<VehicleModelThumbnail> Gets_Report(int id, Int64 nUserID);
         
        VehicleModelThumbnail GetFrontImage(int nVehicleModelID, Int64 nUserID);
         
        VehicleModelThumbnail GetMeasurementSpecImage(int nVehicleModelID, Int64 nUserID);
         
        List<VehicleModelThumbnail> Gets(int nVehicleModelID, Int64 nUserID);
         
        List<VehicleModelThumbnail> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        VehicleModelThumbnail Save(VehicleModelThumbnail oVehicleModelThumbnail, Int64 nUserID);
    }
    #endregion
}
