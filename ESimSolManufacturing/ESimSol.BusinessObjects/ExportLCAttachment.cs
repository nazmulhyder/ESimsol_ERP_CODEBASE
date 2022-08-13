using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class ExportLCAttachment
    {
        public ExportLCAttachment()
        {
            ExportLCAttachmentID = 0;
            ExportLCID = 0;
            AttatchmentName = "";
            AttatchFile = null;
            FileType = "";
            Remarks = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ExportLCAttachmentID { get; set; }
        public int ExportLCID { get; set; }
        public string AttatchmentName { get; set; }
        public byte[] AttatchFile { get; set; }
        public string FileType { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public ExportLCAttachment Save(long nUserID)
        {
            return ExportLCAttachment.Service.Save(this, nUserID);
        }
        public static List<ExportLCAttachment> Gets(long nUserID)
        {
            return ExportLCAttachment.Service.Gets(nUserID);
        }
        public static ExportLCAttachment GetWithAttachFile(int nId, long nUserID)
        {
            return ExportLCAttachment.Service.GetWithAttachFile(nId, nUserID);
        }
        public ExportLCAttachment Get(int nId, long nUserID)
        {
            return ExportLCAttachment.Service.Get(nId, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ExportLCAttachment.Service.Delete(nId, nUserID);
        }
        public static List<ExportLCAttachment> GetsAttachmentById(int nExportLCID, long nUserID)
        {
            return ExportLCAttachment.Service.GetsAttachmentById(nExportLCID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportLCAttachmentService Service
        {
            get { return (IExportLCAttachmentService)Services.Factory.CreateService(typeof(IExportLCAttachmentService)); }
        }
        #endregion
    }

    #region IExportLCAttachment interface

    public interface IExportLCAttachmentService
    {
        ExportLCAttachment Save(ExportLCAttachment oExportLCAttachment, long nUserID);
        List<ExportLCAttachment> Gets(long nUserID);
        ExportLCAttachment GetWithAttachFile(int id, long nUserID);
        ExportLCAttachment Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        List<ExportLCAttachment> GetsAttachmentById(int nExportLCID, long nUserID);
    }
    #endregion
}
