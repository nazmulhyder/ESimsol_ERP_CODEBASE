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

    #region KommFileThumbnail
    
    public class KommFileThumbnail : BusinessObject
    {
        public KommFileThumbnail()
        {
            KommFileThumbnailID = 0;
            KommFileID = 0;
            ImageTitle = "";
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            KommFileImageID = 0;
        }

        #region Properties
         
        public int KommFileThumbnailID { get; set; }
         
        public int KommFileID { get; set; }   
         
        public string ImageTitle { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public int KommFileImageID { get; set; }
         
        public EnumImageType ImageType { get; set; }
        public int ImageTypeInInt { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public Image ThumImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        #endregion

        #region Functions
        public static List<KommFileThumbnail> Gets_Report(int id, long nUserID)
        {
            return KommFileThumbnail.Service.Gets_Report(id, nUserID);
        }
        public static List<KommFileThumbnail> Gets(int nKommFileID, long nUserID)
        {
            return KommFileThumbnail.Service.Gets(nKommFileID, nUserID);
        }

        public KommFileThumbnail Get(int id, long nUserID)
        {           
            return KommFileThumbnail.Service.Get(id, nUserID);
        }

        public static List<KommFileThumbnail> Gets(string sSQL, long nUserID)
        {
            return KommFileThumbnail.Service.Gets(sSQL, nUserID);
        }

        public KommFileThumbnail GetFrontImage(int nKommFileID, long nUserID)
        {
            return KommFileThumbnail.Service.GetFrontImage(nKommFileID, nUserID);
        }

        public KommFileThumbnail GetMeasurementSpecImage(int nKommFileID, long nUserID)
        {
            return KommFileThumbnail.Service.GetMeasurementSpecImage(nKommFileID, nUserID);
        }
        public KommFileThumbnail Save(long nUserID)
        {
            return KommFileThumbnail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KommFileThumbnail.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IKommFileThumbnailService Service
        {
            get { return (IKommFileThumbnailService)Services.Factory.CreateService(typeof(IKommFileThumbnailService)); }
        }

        #endregion
    }
    #endregion
        
    #region IKommFileThumbnail interface
     
    public interface IKommFileThumbnailService
    {
         
        KommFileThumbnail Get(int id, Int64 nUserID);
         
        List<KommFileThumbnail> Gets_Report(int id, Int64 nUserID);
         
        KommFileThumbnail GetFrontImage(int nKommFileID, Int64 nUserID);
         
        KommFileThumbnail GetMeasurementSpecImage(int nKommFileID, Int64 nUserID);
         
        List<KommFileThumbnail> Gets(int nKommFileID, Int64 nUserID);
         
        List<KommFileThumbnail> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        KommFileThumbnail Save(KommFileThumbnail oKommFileThumbnail, Int64 nUserID);
    }
    #endregion
}
