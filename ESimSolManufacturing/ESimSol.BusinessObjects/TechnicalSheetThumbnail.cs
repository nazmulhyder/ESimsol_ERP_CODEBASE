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

    #region TechnicalSheetThumbnail
    
    public class TechnicalSheetThumbnail : BusinessObject
    {
        public TechnicalSheetThumbnail()
        {
            TechnicalSheetThumbnailID = 0;
            TechnicalSheetID = 0;
            ImageTitle = "";
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            TechnicalSheetImageID = 0;
        }

        #region Properties
         
        public int TechnicalSheetThumbnailID { get; set; }
         
        public int TechnicalSheetID { get; set; }   
         
        public string ImageTitle { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public int TechnicalSheetImageID { get; set; }
         
        public EnumImageType ImageType { get; set; }
        

         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public Image ThumImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        #endregion

        #region Functions
        public static List<TechnicalSheetThumbnail> Gets_Report(int id, long nUserID)
        {
            return TechnicalSheetThumbnail.Service.Gets_Report(id, nUserID);
        }
        public static List<TechnicalSheetThumbnail> Gets(int nTechnicalSheetID, long nUserID)
        {
            return TechnicalSheetThumbnail.Service.Gets(nTechnicalSheetID, nUserID);
        }

        public TechnicalSheetThumbnail Get(int id, long nUserID)
        {           
            return TechnicalSheetThumbnail.Service.Get(id, nUserID);
        }

        public static List<TechnicalSheetThumbnail> Gets(string sSQL, long nUserID)
        {
            return TechnicalSheetThumbnail.Service.Gets(sSQL, nUserID);
        }

        public TechnicalSheetThumbnail GetFrontImage(int nTechnicalSheetID, long nUserID)
        {
            return TechnicalSheetThumbnail.Service.GetFrontImage(nTechnicalSheetID, nUserID);
        }

        public TechnicalSheetThumbnail GetMeasurementSpecImage(int nTechnicalSheetID, long nUserID)
        {
            return TechnicalSheetThumbnail.Service.GetMeasurementSpecImage(nTechnicalSheetID, nUserID);
        }
        public TechnicalSheetThumbnail Save(long nUserID)
        {
            return TechnicalSheetThumbnail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return TechnicalSheetThumbnail.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static ITechnicalSheetThumbnailService Service
        {
            get { return (ITechnicalSheetThumbnailService)Services.Factory.CreateService(typeof(ITechnicalSheetThumbnailService)); }
        }

        #endregion
    }
    #endregion
        
    #region ITechnicalSheetThumbnail interface
     
    public interface ITechnicalSheetThumbnailService
    {
         
        TechnicalSheetThumbnail Get(int id, Int64 nUserID);
         
        List<TechnicalSheetThumbnail> Gets_Report(int id, Int64 nUserID);
         
        TechnicalSheetThumbnail GetFrontImage(int nTechnicalSheetID, Int64 nUserID);
         
        TechnicalSheetThumbnail GetMeasurementSpecImage(int nTechnicalSheetID, Int64 nUserID);
         
        List<TechnicalSheetThumbnail> Gets(int nTechnicalSheetID, Int64 nUserID);
         
        List<TechnicalSheetThumbnail> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        TechnicalSheetThumbnail Save(TechnicalSheetThumbnail oTechnicalSheetThumbnail, Int64 nUserID);
    }
    #endregion
}
