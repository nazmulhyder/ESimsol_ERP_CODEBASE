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
    #region ExportDocForwarding
    [DataContract]
    public class ExportDocForwarding : BusinessObject
    {
        public ExportDocForwarding()
        {
            ExportDocForwardingID = 0;
       
            ReferenceID = 0;
            Name_Print = "";
            Copies = 1;
            Name_Doc = "";
            ExportDocID =0;
            Selected = false;
            RefType = EnumMasterLCType.ExportLC;
            ErrorMessage = "";
            ExportDocForwardings = new List<ExportDocForwarding>();
            DocumentType = EnumDocumentType.None;
        }

        #region Properties
        public int ExportDocForwardingID { get; set; }
        public int ReferenceID { get; set; }
        public string Name_Print { get; set; }
        public string Name_Doc { get; set; }
        public int Copies { get; set; }
        public int ExportDocID { get; set; }
        public bool Selected { get; set; }
        public EnumMasterLCType RefType { get; set; }
        public int RefTypeInInt { get; set; }
        public EnumDocumentType DocumentType { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public List<ExportDocForwarding> ExportDocForwardings { get; set; }
        #endregion

        #region Functions
     
        public ExportDocForwarding Get(int nId, Int64 nUserID)
        {
            return ExportDocForwarding.Service.Get(nId, nUserID);
        }
  
        public static List<ExportDocForwarding> Gets(int nReferenceID,int nRefType, int nUserID)
        {
            return ExportDocForwarding.Service.Gets(nReferenceID,nRefType, nUserID);
        }
     
        public ExportDocForwarding Save(Int64 nUserID)
        {
            return ExportDocForwarding.Service.Save(this, nUserID);
        }
        public string Delete(int nExportBillID, Int64 nUserID)
        {
            return ExportDocForwarding.Service.Delete(nExportBillID, nUserID);
        }
        public ExportDocForwarding DeleteBYExportBillID(int nExportBillID, Int64 nUserID)
        {
            return ExportDocForwarding.Service.DeleteBYExportBillID(nExportBillID, nUserID);
        }
        #endregion

        #region ServiceFactory

     
        internal static IExportDocForwardingService Service
        {
            get { return (IExportDocForwardingService)Services.Factory.CreateService(typeof(IExportDocForwardingService)); }
        }
        #endregion
    }
    #endregion

    #region IExportDocForwarding interface
    
    public interface IExportDocForwardingService
    {
        ExportDocForwarding Get(int id, Int64 nUserID);
      
        List<ExportDocForwarding> Gets(int nReferenceID, int nRefType, Int64 nUserID);
        string Delete(int nExportBillID, Int64 nUserID);
        ExportDocForwarding Save(ExportDocForwarding oExportDocForwarding, Int64 nUserID);
        ExportDocForwarding DeleteBYExportBillID(int nExportBillID, Int64 nUserID);
        
    }
    #endregion
}
