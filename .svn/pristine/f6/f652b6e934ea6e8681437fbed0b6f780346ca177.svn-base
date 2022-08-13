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
    #region CandidateEducation

    public class CandidateEducation : BusinessObject
    {
        public CandidateEducation()
        {
            CEID = 0;
            CandidateID = 0;
            Degree = "";
            Major = "";
            Session = "";
            PassingYear = DateTime.Now; ;
            BoardUniversity = "";
            Institution = "";
            Result = "";
            Country = "";
            LastUpdatedDate = DateTime.Now;
            ErrorMessage = "";

        }

        #region Properties
        public int CEID { get; set; }
        public int CandidateID { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string Session { get; set; }
        public DateTime PassingYear { get; set; }
        public string BoardUniversity { get; set; }
        public string Institution { get; set; }
        public string Result { get; set; }
        public string Country { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string LastUpdatedDateInString
        {
            get { return LastUpdatedDate.ToString("dd MMM yyyy"); }
        }

        #endregion

        #region Functions

        public static CandidateEducation Get(int id, long nUserID)
        {
            return CandidateEducation.Service.Get(id, nUserID);
        }

        public static CandidateEducation Get(string sSQL, long nUserID)
        {
            return CandidateEducation.Service.Get(sSQL, nUserID);
        }

        public static List<CandidateEducation> Gets(int nCID, long nUserID)
        {
            return CandidateEducation.Service.Gets(nCID, nUserID);
        }

        public static List<CandidateEducation> Gets(string sSQL, long nUserID)
        {
            return CandidateEducation.Service.Gets(sSQL, nUserID);
        }

        public CandidateEducation IUD(int nDBOperation, long nUserID)
        {
            return CandidateEducation.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICandidateEducationService Service
        {
            get { return (ICandidateEducationService)Services.Factory.CreateService(typeof(ICandidateEducationService)); }
        }

        #endregion
    }
    #endregion

    #region ICandidateEducation interface

    public interface ICandidateEducationService
    {
        CandidateEducation Get(int id, Int64 nUserID);
        CandidateEducation Get(string sSQL, Int64 nUserID);
        List<CandidateEducation> Gets(int nCID, Int64 nUserID);
        List<CandidateEducation> Gets(string sSQL, Int64 nUserID);
        CandidateEducation IUD(CandidateEducation oCandidateEducation, int nDBOperation, Int64 nUserID);


    }
    #endregion
}
