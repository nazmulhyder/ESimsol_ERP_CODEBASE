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

    #region SalesQuotationThumbnail
    
    public class SalesQuotationThumbnail : BusinessObject
    {
        public SalesQuotationThumbnail()
        {
            SalesQuotationThumbnailID = 0;
            SalesQuotationID = 0;
            ImageTitle = "";
            ThumbnailImage = null;
            ImageType = EnumImageType.Select_Image_Type;
            ImageTypeInInt = 0;
            SalesQuotationImageID = 0;
        }
        #region Properties
         
        public int SalesQuotationThumbnailID { get; set; }
         
        public int SalesQuotationID { get; set; }   
         
        public string ImageTitle { get; set; }
         
        public byte[] ThumbnailImage { get; set; }
         
        public int SalesQuotationImageID { get; set; }
         
        public EnumImageType ImageType { get; set; }
        public int ImageTypeInInt { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Property
        public Image ThumImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        #endregion
        #region Functions
        public static List<SalesQuotationThumbnail> Gets_Report(int id, long nUserID)
        {
            return SalesQuotationThumbnail.Service.Gets_Report(id, nUserID);
        }
        public static List<SalesQuotationThumbnail> Gets(int nSalesQuotationID, long nUserID)
        {
            return SalesQuotationThumbnail.Service.Gets(nSalesQuotationID, nUserID);
        }

        public SalesQuotationThumbnail Get(int id, long nUserID)
        {           
            return SalesQuotationThumbnail.Service.Get(id, nUserID);
        }

        public static List<SalesQuotationThumbnail> Gets(string sSQL, long nUserID)
        {
            return SalesQuotationThumbnail.Service.Gets(sSQL, nUserID);
        }

        public SalesQuotationThumbnail GetFrontImage(int nSalesQuotationID, long nUserID)
        {
            return SalesQuotationThumbnail.Service.GetFrontImage(nSalesQuotationID, nUserID);
        }

        public SalesQuotationThumbnail GetMeasurementSpecImage(int nSalesQuotationID, long nUserID)
        {
            return SalesQuotationThumbnail.Service.GetMeasurementSpecImage(nSalesQuotationID, nUserID);
        }
        public SalesQuotationThumbnail Save(long nUserID)
        {
            return SalesQuotationThumbnail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return SalesQuotationThumbnail.Service.Delete(id, nUserID);
        }

        #endregion
        #region ServiceFactory

        internal static ISalesQuotationThumbnailService Service
        {
            get { return (ISalesQuotationThumbnailService)Services.Factory.CreateService(typeof(ISalesQuotationThumbnailService)); }
        }

        #endregion
    }
    #endregion
        
    #region ISalesQuotationThumbnail interface
     
    public interface ISalesQuotationThumbnailService
    {
         
        SalesQuotationThumbnail Get(int id, Int64 nUserID);
         
        List<SalesQuotationThumbnail> Gets_Report(int id, Int64 nUserID);
         
        SalesQuotationThumbnail GetFrontImage(int nSalesQuotationID, Int64 nUserID);
         
        SalesQuotationThumbnail GetMeasurementSpecImage(int nSalesQuotationID, Int64 nUserID);
         
        List<SalesQuotationThumbnail> Gets(int nSalesQuotationID, Int64 nUserID);
         
        List<SalesQuotationThumbnail> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        SalesQuotationThumbnail Save(SalesQuotationThumbnail oSalesQuotationThumbnail, Int64 nUserID);
    }
    #endregion
}
