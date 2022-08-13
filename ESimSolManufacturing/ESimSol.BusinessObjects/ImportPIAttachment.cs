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

    #region ImportPIAttachment
    
    public class ImportPIAttachment : BusinessObject
    {
        public ImportPIAttachment()
        {
            ImportPIAttachmentID = 0;
            ImportPIID = 0;
            AttatchmentName = "";
            AttatchFile = null;
            FileType = "";
            Remarks = "";
            ErrorMessage = "";
            ImportPIAttachments = new List<ImportPIAttachment>();
        }

        #region Properties
         
        public int ImportPIAttachmentID { get; set; }
         
        public int ImportPIID { get; set; }
         
        public string AttatchmentName { get; set; }
         
        public byte[] AttatchFile { get; set; }
        
         
        public string FileType { get; set; }
         
        public string Remarks { get; set; }
        
         
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property
         
        public List<ImportPIAttachment> ImportPIAttachments { get; set; }

        public string AttatchFileinString
        {
            get
            {
                return ImportPIAttachmentID.ToString();
            }
        }
        #endregion

        #region Functions
        public ImportPIAttachment Save(long nUserID)
        {
            
            return ImportPIAttachment.Service.Save(this, nUserID);
        }

           public static List<ImportPIAttachment> Gets(int id, long nUserID) //ImportPId
        {
            
            return ImportPIAttachment.Service.Gets(id, nUserID);
        }

        public static ImportPIAttachment GetWithAttachFile(int id, long nUserID) //ImportPId
        {
            return ImportPIAttachment.Service.GetWithAttachFile(id, nUserID);
        }

        public static ImportPIAttachment Get(int id, long nUserID) //ImportPId
        {
            return ImportPIAttachment.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ImportPIAttachment.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IImportPIAttachmentService Service
        {
            get { return (IImportPIAttachmentService)Services.Factory.CreateService(typeof(IImportPIAttachmentService)); }
        }
        #endregion
    }
    #endregion


    #region IImportPIAttachmentService interface
     
    public interface IImportPIAttachmentService
    {

         
        List<ImportPIAttachment> Gets(int id, Int64 nUserID);   //ImportPID
         
        ImportPIAttachment Get(int id, Int64 nUserID);   //ImportPIAttachmentID
         
        ImportPIAttachment GetWithAttachFile(int id, Int64 nUserID);   //ImportPIAttachmentID
         
        ImportPIAttachment Save(ImportPIAttachment oImportPIAttachment, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
    }
    #endregion
    
   
}
