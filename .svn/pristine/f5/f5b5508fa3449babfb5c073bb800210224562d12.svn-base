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
    #region BarCodeComment
    
    public class BarCodeComment : BusinessObject
    {
        public BarCodeComment()
        {
            BarCodeCommentID = 0;
            OrderRecapID = 0;
            Comments = "";
            BarCodeCommentLogID = 0;
            OrderRecapLogID = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int BarCodeCommentID { get; set; }
         
        public int OrderRecapID { get; set; }
         
        public string Comments { get; set; }
        public int BarCodeCommentLogID { get; set; }
        public int OrderRecapLogID { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
       
        #endregion

        #region Functions

        public static List<BarCodeComment> Gets(long nUserID)
        {
            return BarCodeComment.Service.Gets( nUserID);
        }

        public static List<BarCodeComment> Gets(int nOrderRecapID, long nUserID)
        {
            return BarCodeComment.Service.Gets(nOrderRecapID, nUserID);
        }

        public static List<BarCodeComment> GetsForLog(int nOrderRecapLogID, long nUserID)
        {
            return BarCodeComment.Service.Gets(nOrderRecapLogID, nUserID);
        }
        
        public BarCodeComment Get(int id, long nUserID)
        {
            return BarCodeComment.Service.Get(id, nUserID);
        }
        public static List<BarCodeComment> Save(OrderRecap oOrderRecap, long nUserID)
        {
            return BarCodeComment.Service.Save(oOrderRecap, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return BarCodeComment.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBarCodeCommentService Service
        {
            get { return (IBarCodeCommentService)Services.Factory.CreateService(typeof(IBarCodeCommentService)); }
        }
        #endregion
    }
    #endregion

    #region IBarCodeComment interface
     
    public interface IBarCodeCommentService
    {
         
        BarCodeComment Get(int id, Int64 nUserID);
         
        List<BarCodeComment> Gets(Int64 nUserID);
         
        List<BarCodeComment> Gets(int nOrderRecapID, Int64 nUserID);
        List<BarCodeComment> GetsForLog(int nOrderRecapLogID, Int64 nUserID);        

        string Delete(int id, Int64 nUserID);

        List<BarCodeComment> Save(OrderRecap oOrderRecap, Int64 nUserID);
    }
    #endregion
}
