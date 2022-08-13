using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region ApprovalHeadPerson
    public class ApprovalHeadPerson
    {
        public ApprovalHeadPerson()
        {
            ApprovalHeadPersonID = 0;
            ApprovalHeadID = 0;
            UserID = 0;
            Note = "";
            UserName = "";
            EmployeeName = "";
            IsActive = true;
            ErrorMessage = "";
            Params = "";
            oApprovalHeadPersons = new List<ApprovalHeadPerson>();

        }

        #region Properties
        public int ApprovalHeadPersonID { get; set; }
        public int ApprovalHeadID { get; set; }
        public string Note { get; set; }
        public string Params { get; set; }
        public bool IsActive { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public string ErrorMessage { get; set; }
        public List<ApprovalHeadPerson> oApprovalHeadPersons { get; set; }
        #endregion

        #region Functions

        public string ActiveInactive
        {
            get
            {
                return (this.IsActive == true ? "Active":"Inactive");
            }
        }

        public static List<ApprovalHeadPerson> Gets(string sSQL, long nUserID)
        {
            return ApprovalHeadPerson.Service.Gets(sSQL, nUserID);
        }
        public static ApprovalHeadPerson Get(string sSQL, long nUserID)
        {
            return ApprovalHeadPerson.Service.Get(sSQL, nUserID);
        }
        public ApprovalHead IUD(ApprovalHead oApprovalHead, int nDBOperation, long nUserID)
        {
            return ApprovalHeadPerson.Service.IUD(oApprovalHead, nDBOperation, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ApprovalHeadPerson.Service.Delete(this, nUserID);
        }

        public ApprovalHeadPerson Activate(Int64 nUserID)
        {
            return ApprovalHeadPerson.Service.Activate(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IApprovalHeadPersonService Service
        {
            get { return (IApprovalHeadPersonService)Services.Factory.CreateService(typeof(IApprovalHeadPersonService)); }
        }
        #endregion
    }
    #endregion

    #region IApprovalHeadPersonDaily interface

    public interface IApprovalHeadPersonService
    {
        List<ApprovalHeadPerson> Gets(string sSQL, Int64 nUserID);
        ApprovalHeadPerson Get(string sSQL, Int64 nUserID);
        ApprovalHead IUD(ApprovalHead oApprovalHead, int nDBOperation, Int64 nUserID);
        string Delete(ApprovalHeadPerson oApprovalHeadPerson, Int64 nUserID);
        ApprovalHeadPerson Activate(ApprovalHeadPerson oApprovalHeadPerson, Int64 nUserID);
      
    }
    #endregion
}


