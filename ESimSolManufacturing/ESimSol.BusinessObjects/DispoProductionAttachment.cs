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
    #region DispoProductionAttachment

    public class DispoProductionAttachment : BusinessObject
    {

        #region Constructor
        public DispoProductionAttachment()
        {
            DispoProductionAttachmentID = 0;
            DispoProductionCommentID = 0;
            File = null;
            FileName = "";
            FileType = "";
            IsFile = false;
            IsRemoveFile = false;
            SubFileName = "";
            Param = "";
            ErrorMessage = "";

            DispoProductionAttachments = new List<DispoProductionAttachment>();
        }
        #endregion

        #region Properties

        public int DispoProductionAttachmentID { get; set; }
        public int DispoProductionCommentID { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool IsFile { get; set; }
        public bool IsRemoveFile { get; set; }


        public string Param { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public System.Drawing.Image Image { get; set; }
        public bool Selected { get; set; }
        public List<DispoProductionAttachment> DispoProductionAttachments { get; set; }
        public Company Company { get; set; }
        public string SubFileName { get; set; }
        #endregion

        #region Functions
        public static List<DispoProductionAttachment> Gets(long nUserID)
        {
            return DispoProductionAttachment.Service.Gets(nUserID);
        }
        //public static List<DispoProductionAttachment> GetsByDispoProductionAttachment(string sDispoProductionAttachment, long nUserID)
        //{
        //    return DispoProductionAttachment.Service.GetsByDispoProductionAttachment(sDispoProductionAttachment, nUserID);
        //}
        public DispoProductionAttachment Get(int id, long nUserID)
        {
            return DispoProductionAttachment.Service.Get(id, nUserID);
        }
        public DispoProductionAttachment Save(long nUserID)
        {
            return DispoProductionAttachment.Service.Save(this, nUserID);
        }

        public static List<DispoProductionAttachment> Gets(string sSQL, long nUserID)
        {
            return DispoProductionAttachment.Service.Gets(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return DispoProductionAttachment.Service.Delete(id, nUserID);
        }
        #endregion




        #region ServiceFactory
        internal static IDispoProductionAttachmentService Service
        {
            get { return (IDispoProductionAttachmentService)Services.Factory.CreateService(typeof(IDispoProductionAttachmentService)); }
        }

        #endregion
    }
    #endregion

    #region IDispoProductionAttachment interface

    public interface IDispoProductionAttachmentService
    {
        DispoProductionAttachment Get(int id, Int64 nUserID);
        List<DispoProductionAttachment> Gets(Int64 nUserID);
        List<DispoProductionAttachment> Gets(string sSQL, Int64 nUserID);
        //List<DispoProductionAttachment> GetsByDispoProductionAttachment(string sDispoProductionAttachment, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        DispoProductionAttachment Save(DispoProductionAttachment oDispoProductionAttachment, Int64 nUserID);

    }
    #endregion
}
