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
    #region CandidateApplication

    public class CandidateApplication : BusinessObject
    {
        public CandidateApplication()
        {
            CAppID = 0;
            CandidateID = 0;
            CircularID = 0;
            ExpectedSalary = 0;
            IsActive = true;
            IsSelected = false;
            InterviewDate = DateTime.Now;
            Note = "";
            ErrorMessage = "";
            CandidateName = "";
            NoOfPosition = 0;
            CircularStartDate = DateTime.Now;
            CircularEndDate = DateTime.Now;
            ApplicationDate = DateTime.Now;
            DepartmentName = "";
            DesignationName = "";

        }

        #region Properties
        public int CAppID { get; set; }
        public Int64 CandidateID { get; set; }
        public int CircularID { get; set; }
        public double ExpectedSalary { get; set; }
        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
        public DateTime InterviewDate { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public string SelectionStatus { get { if (IsSelected)return "Selected"; else return "--"; } }
        public string InterviewDateInString
        {
            get { return InterviewDate.ToString("dd MMM yyyy"); }
        }

        public string CandidateName { get; set; }

        public int NoOfPosition { get; set; }

        public DateTime CircularStartDate { get; set; }

        public DateTime CircularEndDate { get; set; }

        public string DepartmentName { get; set; }

        public string DesignationName { get; set; }

        public DateTime ApplicationDate { get; set; }
        public string CircularStartDateInString
        {
            get { return CircularStartDate.ToString("dd MMM yyyy"); }
        }
        public string CircularEndDateInString
        {
            get { return CircularEndDate.ToString("dd MMM yyyy"); }
        }
        public string ApplicationDateInString
        {
            get { return ApplicationDate.ToString("dd MMM yyyy"); }
        }
        #endregion

        #region Functions
        public static CandidateApplication Get(int id, long nUserID)
        {
            return CandidateApplication.Service.Get(id, nUserID);
        }
        public static CandidateApplication Get(string sSQL, long nUserID)
        {
            return CandidateApplication.Service.Get(sSQL, nUserID);
        }
        public static List<CandidateApplication> Gets(long nUserID)
        {
            return CandidateApplication.Service.Gets(nUserID);
        }
        public static List<CandidateApplication> Gets(string sSQL, long nUserID)
        {
            return CandidateApplication.Service.Gets(sSQL, nUserID);
        }
        public CandidateApplication IUD(int nDBOperation, long nUserID)
        {
            return CandidateApplication.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICandidateApplicationService Service
        {
            get { return (ICandidateApplicationService)Services.Factory.CreateService(typeof(ICandidateApplicationService)); }
        }

        #endregion
    }
    #endregion

    #region ICandidateApplication interface

    public interface ICandidateApplicationService
    {
        CandidateApplication Get(int id, Int64 nUserID);
        CandidateApplication Get(string sSQL, Int64 nUserID);
        List<CandidateApplication> Gets(Int64 nUserID);
        List<CandidateApplication> Gets(string sSQL, Int64 nUserID);
        CandidateApplication IUD(CandidateApplication oCandidateApplication, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
