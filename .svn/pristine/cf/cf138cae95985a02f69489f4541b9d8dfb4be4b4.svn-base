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
    #region CompanyDocument
    public class CompanyDocument : BusinessObject
    {
        public CompanyDocument()
        {
            CDID = 0;
            CompanyID = 0;
            Description = "";
            DocFile = null;
            FileType = "";
            FileName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int CDID { get; set; }
        public int CompanyID { get; set; }
        public string Description { get; set; }
        public byte[] DocFile { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property

        #endregion

        #region Functions
        public CompanyDocument Save(long nUserID)
        {
            return CompanyDocument.Service.Save(this, nUserID);
        }
        public static List<CompanyDocument> Gets(long nUserID)
        {
            return CompanyDocument.Service.Gets(nUserID);
        }
        public static CompanyDocument GetWithAttachFile(int nId, long nUserID)
        {
            return CompanyDocument.Service.GetWithAttachFile(nId, nUserID);
        }
        public CompanyDocument Get(int nId, long nUserID)
        {
            return CompanyDocument.Service.Get(nId, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return CompanyDocument.Service.Delete(nId, nUserID);
        }
        public static List<CompanyDocument> Gets(string SsQL, long nUserID)
        {
            return CompanyDocument.Service.Gets(SsQL,nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICompanyDocumentService Service
        {
            get { return (ICompanyDocumentService)Services.Factory.CreateService(typeof(ICompanyDocumentService)); }
        }
        #endregion
    }
    #endregion

    #region IBank interface

    public interface ICompanyDocumentService
    {
        CompanyDocument Save(CompanyDocument oCompanyDocument, long nUserID);
        List<CompanyDocument> Gets(long nUserID);
        CompanyDocument GetWithAttachFile(int id, long nUserID);
        CompanyDocument Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        List<CompanyDocument> Gets(string SsQL, long nUserID);
    }
    #endregion
}
