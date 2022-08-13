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
    #region COSImage
    
    public class COSImage : BusinessObject
    {
        public COSImage()
        {
            COSImageID = 0;
            OperationType =  EnumOperationType.None;
            ImageTitle = "";
            LargeImage = null;
            COSVFormat =  EnumClientOperationValueFormat.None;
            COSVFormatInInt = 0;
            OperationTypeInInt = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int COSImageID { get; set; }

        public EnumOperationType OperationType { get; set; }
        public int OperationTypeInInt { get; set; }
         
        public string ImageTitle { get; set; }
         
        public byte[] LargeImage { get; set; }

        public EnumClientOperationValueFormat COSVFormat { get; set; }
        public int COSVFormatInInt { get; set; }
        
     
         
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<COSImage> COSImages { get; set; }
        public List<EnumObject> OperationTypes { get; set; }
        public List<EnumObject> COSVFormats { get; set; }
        public Image TSImage { get; set; }

        public string OperationTypeInString
        {
            get
            {
                return this.OperationType.ToString();
            }
        }
        public string COSVFormatInString
        {
            get
            {
                return this.COSVFormat.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<COSImage> Gets(long nUserID)
        {
            return COSImage.Service.Gets( nUserID);
        }

        public COSImage Get(int id, long nUserID)
        {
            return COSImage.Service.Get(id, nUserID);
        }

        public COSImage GetByOperationAndCOSFormat(int nOperationType, int nCOSFormat, long nUserID)
        {
            return COSImage.Service.GetByOperationAndCOSFormat(nOperationType, nCOSFormat, nUserID);
        } 
        public COSImage Save(long nUserID)
        {
            return COSImage.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return COSImage.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICOSImageService Service
        {
            get { return (ICOSImageService)Services.Factory.CreateService(typeof(ICOSImageService)); }
        }
        #endregion
    }
    #endregion

    #region ICOSImage interface
     
    public interface ICOSImageService
    {
         
        COSImage Get(int id, Int64 nUserID);
        COSImage GetByOperationAndCOSFormat(int nOperationType, int nCOSFormat, Int64 nUserID);
         
     
        List<COSImage> Gets( Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        COSImage Save(COSImage oCOSImage, Int64 nUserID);
    }
    #endregion
}
