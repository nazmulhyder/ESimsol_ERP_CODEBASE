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
    #region EmployeeDoc

    public class EmployeeDoc : BusinessObject
    {
        public EmployeeDoc()
        {
            EmployeeDocID = 0;
            EmployeeID = 0;
            DocType = EnumEmployeeDoc.General;
            DocTypeID = 0;
            AttachmentFile = null;
            FileType = "";
            Description = "";
            IssueDate = DateTime.Now;
            ExpireDate = DateTime.Now;
            ErrorMessage = "";
            EmployeeDocs = new List<EmployeeDoc>();
        }

        #region Properties
        public int EmployeeDocID { get; set; }
        public int EmployeeID { get; set; }
        public EnumEmployeeDoc DocType { get; set; }
        public int DocTypeInt { get; set; }
        public int DocTypeID { get; set; }
        public byte[] AttachmentFile { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ErrorMessage { get; set; }
        public List<EmployeeDoc> EmployeeDocs { get; set; }
        #endregion

        #region Derived Property

        public string IssueDateInString { get { return IssueDate.ToString("dd MMM yyyy"); } }
        public string ExpireDateInString { get { return ExpireDate.ToString("dd MMM yyyy"); } }

        #endregion

        #region Functions

        public EmployeeDoc Get(int id, long nUserID)
        {
            return EmployeeDoc.Service.Get(id, nUserID);
        }
        public static List<EmployeeDoc> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeDoc.Service.Gets(nEmployeeID, nUserID);
        }
        public static List<EmployeeDoc> Gets(string sSQL, long nUserID)
        {
            return EmployeeDoc.Service.Gets(sSQL, nUserID);
        }
        public EmployeeDoc Save(long nUserID)
        {
            return EmployeeDoc.Service.Save(this, nUserID);
        }
        public string Delete(string sIDs, long nUserID)
        {
            return EmployeeDoc.Service.Delete(sIDs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeDocService Service
        {
            get { return (IEmployeeDocService)Services.Factory.CreateService(typeof(IEmployeeDocService)); }
        }

        #endregion
    }
    #endregion


    #region IEmployeeDoc interface
    public interface IEmployeeDocService
    {
        EmployeeDoc Get(int id, Int64 nUserID);
        List<EmployeeDoc> Gets(int nEmployeeID, Int64 nUserID);
        List<EmployeeDoc> Gets(string sSQl, Int64 nUserID);
        string Delete(string sIDs, Int64 nUserID);
        EmployeeDoc Save(EmployeeDoc oEmployeeDoc, Int64 nUserID);
    }
    #endregion
}
