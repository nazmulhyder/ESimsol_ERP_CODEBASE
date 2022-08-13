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
    public class EmployeeDocument : BusinessObject
    {
        public EmployeeDocument()
        {
            EDID = 0;
            EmployeeID = 0;
            FileName = "";
            DocFile = null;
            DocFileType = "";
            ErrorMessage = "";
        }

        #region Properties
        public int EDID { get; set; }
        public int EmployeeID { get; set; }
        public string Description { get; set; }
        public byte[] DocFile { get; set; }
        public string DocFileType { get; set; }
        public string FileName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property

        #endregion

        #region Functions
        public EmployeeDocument Save(long nUserID)
        {
            return EmployeeDocument.Service.Save(this, nUserID);
        }
        public static List<EmployeeDocument> Gets(long nUserID)
        {
            return EmployeeDocument.Service.Gets(nUserID);
        }
        public static EmployeeDocument GetWithAttachFile(int nId, long nUserID)
        {
            return EmployeeDocument.Service.GetWithAttachFile(nId, nUserID);
        }
        public EmployeeDocument Get(int nId, long nUserID)
        {
            return EmployeeDocument.Service.Get(nId, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return EmployeeDocument.Service.Delete(nId, nUserID);
        }
        public static List<EmployeeDocument> Gets(string SsQL, long nUserID)
        {
            return EmployeeDocument.Service.Gets(SsQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeDocumentService Service
        {
            get { return (IEmployeeDocumentService)Services.Factory.CreateService(typeof(IEmployeeDocumentService)); }
        }
        #endregion
    }
    #endregion

    #region IBank interface

    public interface IEmployeeDocumentService
    {
        EmployeeDocument Save(EmployeeDocument oEmployeeDocument, long nUserID);
        List<EmployeeDocument> Gets(long nUserID);
        EmployeeDocument GetWithAttachFile(int id, long nUserID);
        EmployeeDocument Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        List<EmployeeDocument> Gets(string SsQL, long nUserID);
    }
    #endregion
}

