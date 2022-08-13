using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region ImageComment
    
    public class ImageComment : BusinessObject
    {
        public ImageComment()
        {
            ImageCommentID = 0;
            TechnicalSheetID = 0;
            Comments = "";            
            ErrorMessage = "";
        }

        #region Properties
         
        public int ImageCommentID { get; set; }
         
public int TechnicalSheetID { get; set; }
         
public string Comments { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
       
        #endregion

        #region Functions

        public static List<ImageComment> Gets(long nUserID)
        {
            return ImageComment.Service.Gets( nUserID);
        }

        public static List<ImageComment> Gets(int nTchnicalSheetID, long nUserID)
        {
            return ImageComment.Service.Gets(nTchnicalSheetID, nUserID);
        }
        
        public ImageComment Get(int id, long nUserID)
        {
            return ImageComment.Service.Get(id, nUserID);
        }
        public static List<ImageComment> Save(TechnicalSheet oTechnicalSheet, long nUserID)
        {
            return ImageComment.Service.Save(oTechnicalSheet, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ImageComment.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImageCommentService Service
        {
            get { return (IImageCommentService)Services.Factory.CreateService(typeof(IImageCommentService)); }
        }
        #endregion
    }
    #endregion

    #region IImageComment interface
     
    public interface IImageCommentService
    {
         
        ImageComment Get(int id, Int64 nUserID);
         
        List<ImageComment> Gets(Int64 nUserID);
         
        List<ImageComment> Gets(int nTechnicalSheetID, Int64 nUserID);        
         
        string Delete(int id, Int64 nUserID);
         
        List<ImageComment> Save(TechnicalSheet oTechnicalSheet, Int64 nUserID);
    }
    #endregion
}
