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

    #region VehicleOrderThumbnail
    
    public class VehicleOrderThumbnail : BusinessObject
    {
        public VehicleOrderThumbnail()
        {
            VehicleOrderThumbnailID = 0;
            VehicleOrderID = 0;
            ImageTitle = "";
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            VehicleOrderImageID = 0;
        }
        #region Properties
         
        public int VehicleOrderThumbnailID { get; set; }
         
        public int VehicleOrderID { get; set; }   
         
        public string ImageTitle { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public int VehicleOrderImageID { get; set; }
         
        public EnumImageType ImageType { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Property
        public Image ThumImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        #endregion
        #region Functions
        public static List<VehicleOrderThumbnail> Gets_Report(int id, long nUserID)
        {
            return VehicleOrderThumbnail.Service.Gets_Report(id, nUserID);
        }
        public static List<VehicleOrderThumbnail> Gets(int nVehicleOrderID, long nUserID)
        {
            return VehicleOrderThumbnail.Service.Gets(nVehicleOrderID, nUserID);
        }

        public VehicleOrderThumbnail Get(int id, long nUserID)
        {           
            return VehicleOrderThumbnail.Service.Get(id, nUserID);
        }

        public static List<VehicleOrderThumbnail> Gets(string sSQL, long nUserID)
        {
            return VehicleOrderThumbnail.Service.Gets(sSQL, nUserID);
        }

        public VehicleOrderThumbnail GetFrontImage(int nVehicleOrderID, long nUserID)
        {
            return VehicleOrderThumbnail.Service.GetFrontImage(nVehicleOrderID, nUserID);
        }

        public VehicleOrderThumbnail GetMeasurementSpecImage(int nVehicleOrderID, long nUserID)
        {
            return VehicleOrderThumbnail.Service.GetMeasurementSpecImage(nVehicleOrderID, nUserID);
        }
        public VehicleOrderThumbnail Save(long nUserID)
        {
            return VehicleOrderThumbnail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return VehicleOrderThumbnail.Service.Delete(id, nUserID);
        }

        #endregion
        #region ServiceFactory

        internal static IVehicleOrderThumbnailService Service
        {
            get { return (IVehicleOrderThumbnailService)Services.Factory.CreateService(typeof(IVehicleOrderThumbnailService)); }
        }

        #endregion
    }
    #endregion
        
    #region IVehicleOrderThumbnail interface
     
    public interface IVehicleOrderThumbnailService
    {
         
        VehicleOrderThumbnail Get(int id, Int64 nUserID);
         
        List<VehicleOrderThumbnail> Gets_Report(int id, Int64 nUserID);
         
        VehicleOrderThumbnail GetFrontImage(int nVehicleOrderID, Int64 nUserID);
         
        VehicleOrderThumbnail GetMeasurementSpecImage(int nVehicleOrderID, Int64 nUserID);
         
        List<VehicleOrderThumbnail> Gets(int nVehicleOrderID, Int64 nUserID);
         
        List<VehicleOrderThumbnail> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        VehicleOrderThumbnail Save(VehicleOrderThumbnail oVehicleOrderThumbnail, Int64 nUserID);
    }
    #endregion
}
