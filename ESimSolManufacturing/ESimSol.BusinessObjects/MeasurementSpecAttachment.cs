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

    #region MeasurementSpecAttachment
    
    public class MeasurementSpecAttachment : BusinessObject
    {
        public MeasurementSpecAttachment()
        {
            MeasurementSpecAttachmentID = 0;
            TechnicalSheetID = 0;
            AttatchmentName = "";
            AttatchFile = null;
            IsMeasurmentSpecAttachment = true;
            FileType = "";
            Remarks = "";

            ErrorMessage = "";
            MeasurementSpecAttachments = new List<MeasurementSpecAttachment>();
        }

        #region Properties
         
        public int MeasurementSpecAttachmentID { get; set; }
         
        public int TechnicalSheetID { get; set; }
        public bool IsMeasurmentSpecAttachment { get; set; }
         
        public string AttatchmentName { get; set; }
         
        public byte[] AttatchFile { get; set; }

         
        public string FileType { get; set; }
         
        public string Remarks { get; set; }

         
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property
         
        public List<MeasurementSpecAttachment> MeasurementSpecAttachments { get; set; }

        public string AttatchFileinString
        {
            get
            {
                return MeasurementSpecAttachmentID.ToString();
            }
        }
        #endregion

        #region Functions
        public MeasurementSpecAttachment Save(long nUserID)
        {
            return MeasurementSpecAttachment.Service.Save(this, nUserID);
        }

  
        public static List<MeasurementSpecAttachment> Gets(int id, bool bIsMeasurmentSpecAttachment, long nUserID) //TSID
        {
            return MeasurementSpecAttachment.Service.Gets(id,bIsMeasurmentSpecAttachment, nUserID);
        }

        public static MeasurementSpecAttachment GetWithAttachFile(int id, long nUserID) //MeasurementSpecd
        {
            return MeasurementSpecAttachment.Service.GetWithAttachFile(id, nUserID);
        }

        public static MeasurementSpecAttachment Get(int id, long nUserID) //MeasurementSpecd
        {
            return MeasurementSpecAttachment.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return MeasurementSpecAttachment.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

 
        internal static IMeasurementSpecAttachmentService Service
        {
            get { return (IMeasurementSpecAttachmentService)Services.Factory.CreateService(typeof(IMeasurementSpecAttachmentService)); }
        }

        #endregion
    }
    #endregion


    #region IMeasurementSpecAttachmentService interface
     
    public interface IMeasurementSpecAttachmentService
    {         
        List<MeasurementSpecAttachment> Gets(int id,bool bIsMeasurmentSpecAttachment, Int64 nUserID);   //MeasurementSpecD
         
        MeasurementSpecAttachment Get(int id, Int64 nUserID);   //MeasurementSpecAttachmentID
         
        MeasurementSpecAttachment GetWithAttachFile(int id, Int64 nUserID);   //MeasurementSpecAttachmentID
         
        MeasurementSpecAttachment Save(MeasurementSpecAttachment oMeasurementSpecAttachment, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}
