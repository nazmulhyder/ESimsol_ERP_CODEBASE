using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region CompanyDoc

    public class CompanyDoc : BusinessObject
    {
        public CompanyDoc()
        {
            CompanyDocID = 0;
            CompanyID = 0;
            Description = "";
            IssueDate = DateTime.Now;
            ExpireDate = DateTime.Now;
            DocName = "";
            AttachmentFile = null;
            FileType = "";
            IsActive = true;
            ErrorMessage = "";
            CompanyDocs = new List<CompanyDoc>();
        }

        #region Properties
        public int CompanyDocID { get; set; }
        public int CompanyID { get; set; }
        public string Description { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string DocName { get; set; }
        public byte[] AttachmentFile { get; set; }
        public string FileType { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public List<CompanyDoc> CompanyDocs { get; set; }
        #endregion

        #region Derived Property
        public string IssueDateInString { get { return IssueDate.ToString("dd MMM yyyy"); } }
        public string ExpireDateInString { get { return ExpireDate.ToString("dd MMM yyyy"); } }

        public string FileStatus
        {
            get
            {
                if (this.IsActive) { return "Active"; }
                else { return "Inactive"; }
            }
        }

        #endregion

        #region Functions
        public CompanyDoc Get(int id, long nUserID)
        {
            return CompanyDoc.Service.Get(id, nUserID);
        }
        public static List<CompanyDoc> Gets(int nCompanyID, long nUserID)
        {
            return CompanyDoc.Service.Gets(nCompanyID, nUserID);
        }
        public static List<CompanyDoc> Gets(string sSQL, long nUserID)
        {
            return CompanyDoc.Service.Gets(sSQL, nUserID);
        }
        public CompanyDoc Save(long nUserID)
        {
            return CompanyDoc.Service.Save(this, nUserID);
        }
        public string Delete(string sIDs, long nUserID)
        {
            return CompanyDoc.Service.Delete(sIDs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICompanyDocService Service
        {
            get { return (ICompanyDocService)Services.Factory.CreateService(typeof(ICompanyDocService)); }
        }

        #endregion
    }
    #endregion

    #region ICompanyDoc interface
    public interface ICompanyDocService
    {
        CompanyDoc Get(int id, Int64 nUserID);
        List<CompanyDoc> Gets(int nCompanyID, Int64 nUserID);
        List<CompanyDoc> Gets(string sSQl, Int64 nUserID);
        string Delete(string sIDs, Int64 nUserID);
        CompanyDoc Save(CompanyDoc oCompanyDoc, Int64 nUserID);
    }
    #endregion
}
