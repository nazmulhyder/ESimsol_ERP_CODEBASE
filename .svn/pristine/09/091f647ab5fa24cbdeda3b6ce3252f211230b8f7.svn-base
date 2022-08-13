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
    #region CandidateExperience

    public class CandidateExperience : BusinessObject
    {
        public CandidateExperience()
        {
            CExpID = 0;
            CandidateID = 0;
            Organization = "";
            OrganizationType = "";
            Address = "";
            Department = "";
            Designation = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            AreaOfExperience = "";
            MajorResponsibility = "";
            LastUpdatedDate = DateTime.Now;
            ErrorMessage = "";

        }

        #region Properties
        public int CExpID { get; set; }
        public int CandidateID { get; set; }
        public string Organization { get; set; }
        public string OrganizationType { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AreaOfExperience { get; set; }
        public string MajorResponsibility { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string LastUpdatedDateInString
        {
            get { return LastUpdatedDate.ToString("dd MMM yyyy"); }
        }
        public string StartDateInString
        {
            get { return StartDate.ToString("dd MMM yyyy"); }
        }
        public string EndDateInString
        {
            get { return EndDate.ToString("dd MMM yyyy"); }
        }

        #endregion

        #region Functions
        public static CandidateExperience Get(int id, long nUserID)
        {
            return CandidateExperience.Service.Get(id, nUserID);
        }
        public static CandidateExperience Get(string sSQL, long nUserID)
        {
            return CandidateExperience.Service.Get(sSQL, nUserID);
        }
        public static List<CandidateExperience> Gets(int nCID, long nUserID)
        {
            return CandidateExperience.Service.Gets(nCID, nUserID);
        }
        public static List<CandidateExperience> Gets(string sSQL, long nUserID)
        {
            return CandidateExperience.Service.Gets(sSQL, nUserID);
        }
        public CandidateExperience IUD(int nDBOperation, long nUserID)
        {
            return CandidateExperience.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICandidateExperienceService Service
        {
            get { return (ICandidateExperienceService)Services.Factory.CreateService(typeof(ICandidateExperienceService)); }
        }

        #endregion
    }
    #endregion

    #region ICandidateExperience interface

    public interface ICandidateExperienceService
    {
        CandidateExperience Get(int id, Int64 nUserID);
        CandidateExperience Get(string sSQL, Int64 nUserID);
        List<CandidateExperience> Gets(int nCID, Int64 nUserID);
        List<CandidateExperience> Gets(string sSQL, Int64 nUserID);
        CandidateExperience IUD(CandidateExperience oCandidateExperience, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
