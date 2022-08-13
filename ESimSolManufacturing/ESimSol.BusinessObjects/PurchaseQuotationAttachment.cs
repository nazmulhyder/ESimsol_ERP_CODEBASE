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

    #region PurchaseQuotationAttachment
    
    public class PurchaseQuotationAttachment : BusinessObject
    {
        public PurchaseQuotationAttachment()
        {
            PurchaseQuotationAttachmentID = 0;
            PurchaseQuotationID = 0;
            AttatchmentName = "";
            AttatchFile = null;
            FileType = "";
            Remarks = "";
            ErrorMessage = "";
            PurchaseQuotationAttachments = new List<PurchaseQuotationAttachment>();
        }

        #region Properties
         
        public int PurchaseQuotationAttachmentID { get; set; }
         
        public int PurchaseQuotationID { get; set; }
         
        public string AttatchmentName { get; set; }
         
        public byte[] AttatchFile { get; set; }
        
         
        public string FileType { get; set; }
         
        public string Remarks { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property
         
        public List<PurchaseQuotationAttachment> PurchaseQuotationAttachments { get; set; }

        public string AttatchFileinString
        {
            get
            {
                return PurchaseQuotationAttachmentID.ToString();
            }
        }
        #endregion

        #region Functions
        public PurchaseQuotationAttachment Save(long nUserID)
        {
            
            return PurchaseQuotationAttachment.Service.Save(this, nUserID);
        }

           public static List<PurchaseQuotationAttachment> Gets(int id, long nUserID) //PurchaseQuotationServiced
        {
            
            return PurchaseQuotationAttachment.Service.Gets(id, nUserID);
        }

        public static PurchaseQuotationAttachment GetWithAttachFile(int id, long nUserID) //PurchaseQuotationServiced
        {
            return PurchaseQuotationAttachment.Service.GetWithAttachFile(id, nUserID);
        }

        public static PurchaseQuotationAttachment Get(int id, long nUserID) //PurchaseQuotationServiced
        {
            return PurchaseQuotationAttachment.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return PurchaseQuotationAttachment.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPurchaseQuotationAttachmentService Service
        {
            get { return (IPurchaseQuotationAttachmentService)Services.Factory.CreateService(typeof(IPurchaseQuotationAttachmentService)); }
        }
        #endregion
    }
    #endregion


    #region IPurchaseQuotationAttachmentService interface
     
    public interface IPurchaseQuotationAttachmentService
    {

         
        List<PurchaseQuotationAttachment> Gets(int id, Int64 nUserID);   //PurchaseQuotationServiceD
         
        PurchaseQuotationAttachment Get(int id, Int64 nUserID);   //PurchaseQuotationAttachmentID
         
        PurchaseQuotationAttachment GetWithAttachFile(int id, Int64 nUserID);   //PurchaseQuotationAttachmentID
         
        PurchaseQuotationAttachment Save(PurchaseQuotationAttachment oPurchaseQuotationAttachment, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
    }
    #endregion
    
   
}
