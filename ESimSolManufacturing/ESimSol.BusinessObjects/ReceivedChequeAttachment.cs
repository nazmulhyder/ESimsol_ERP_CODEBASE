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

    #region ReceivedChequeAttachment
    
    public class ReceivedChequeAttachment : BusinessObject
    {
        public ReceivedChequeAttachment()
        {
            ReceivedChequeAttachmentID = 0;
            ReceivedChequeID = 0;
            AttatchmentName = "";
            AttatchFile = null;
            FileType = "";
            Remarks = "";
            ErrorMessage = "";
            ReceivedChequeAttachments = new List<ReceivedChequeAttachment>();
        }

        #region Properties
         
        public int ReceivedChequeAttachmentID { get; set; }
         
        public int ReceivedChequeID { get; set; }
         
        public string AttatchmentName { get; set; }
         
        public byte[] AttatchFile { get; set; }
        
         
        public string FileType { get; set; }
         
        public string Remarks { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property
         
        public List<ReceivedChequeAttachment> ReceivedChequeAttachments { get; set; }

        public string AttatchFileinString
        {
            get
            {
                return ReceivedChequeAttachmentID.ToString();
            }
        }
        #endregion

        #region Functions
        public ReceivedChequeAttachment Save(long nUserID)
        {
            
            return ReceivedChequeAttachment.Service.Save(this, nUserID);
        }

           public static List<ReceivedChequeAttachment> Gets(int id, long nUserID) 
        {
            
            return ReceivedChequeAttachment.Service.Gets(id, nUserID);
        }

        public static ReceivedChequeAttachment GetWithAttachFile(int id, long nUserID)
        {
            return ReceivedChequeAttachment.Service.GetWithAttachFile(id, nUserID);
        }

        public static ReceivedChequeAttachment Get(int id, long nUserID)
        {
            return ReceivedChequeAttachment.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ReceivedChequeAttachment.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IReceivedChequeAttachmentService Service
        {
            get { return (IReceivedChequeAttachmentService)Services.Factory.CreateService(typeof(IReceivedChequeAttachmentService)); }
        }
        #endregion
    }
    #endregion


    #region IReceivedChequeAttachmentService interface
     
    public interface IReceivedChequeAttachmentService
    {

         
        List<ReceivedChequeAttachment> Gets(int id, Int64 nUserID);   
         
        ReceivedChequeAttachment Get(int id, Int64 nUserID);   //ReceivedChequeAttachmentID
         
        ReceivedChequeAttachment GetWithAttachFile(int id, Int64 nUserID);   //ReceivedChequeAttachmentID
         
        ReceivedChequeAttachment Save(ReceivedChequeAttachment oReceivedChequeAttachment, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
    }
    #endregion
    
   
}
