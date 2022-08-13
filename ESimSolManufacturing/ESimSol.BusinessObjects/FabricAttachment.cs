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
    #region FabricAttachment
    public class FabricAttachment : BusinessObject
    {
        public FabricAttachment()
        {
            FabricAttachmentID = 0;
            FabricID = 0;
            AttatchmentName = "";
            AttatchFile = null;
            FileType = "";
            Remarks = "";
            SwatchType = EnumSwatchType.None;
            SwatchTypeInInt = (int)EnumSwatchType.None;
            FabricAttachments = new List<FabricAttachment>();

            ErrorMessage = "";
        }

        #region Properties
        public int FabricAttachmentID { get; set; }
        public int FabricID { get; set; }
        public string AttatchmentName { get; set; }
        public byte[] AttatchFile { get; set; }
        public string FileType { get; set; }
        public string Remarks { get; set; }
        public EnumSwatchType SwatchType { get; set; }
        public int SwatchTypeInInt { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property
        public List<FabricAttachment> FabricAttachments { get; set; }

        public string SwatchTypeinString
        {
            get
            {
                return EnumObject.jGet(this.SwatchType);
            }
        }

        public string AttatchFileinString
        {
            get
            {
                return FabricAttachmentID.ToString();
            }
        }
        #endregion

        #region Functions
        public FabricAttachment Save(long nUserID)
        {
            return FabricAttachment.Service.Save(this, nUserID);
        }
        public static List<FabricAttachment> Gets(long nUserID)
        {
            return FabricAttachment.Service.Gets(nUserID);
        }
        public static FabricAttachment GetWithAttachFile(int nId, long nUserID)
        {
            return FabricAttachment.Service.GetWithAttachFile(nId, nUserID);
        }
        public FabricAttachment Get(int nId, long nUserID)
        {
            return FabricAttachment.Service.Get(nId, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricAttachment.Service.Delete(nId, nUserID);
        }
        public static List<FabricAttachment> GetsAttachmentByFabric(int nFabricId, long nUserID)
        {
            return FabricAttachment.Service.GetsAttachmentByFabric(nFabricId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricAttachmentService Service
        {
            get { return (IFabricAttachmentService)Services.Factory.CreateService(typeof(IFabricAttachmentService)); }
        }
        #endregion
    }
    #endregion

    #region IBank interface

    public interface IFabricAttachmentService
    {
        FabricAttachment Save(FabricAttachment oFabricAttachment, long nUserID);
        List<FabricAttachment> Gets(long nUserID);
        FabricAttachment GetWithAttachFile(int id, long nUserID);
        FabricAttachment Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        List<FabricAttachment> GetsAttachmentByFabric(int nFabricId, long nUserID);
    }
    #endregion
}
