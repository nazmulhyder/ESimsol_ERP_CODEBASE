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
    #region CandidateTraining

    public class CandidateTraining : BusinessObject
    {
        public CandidateTraining()
        {
            CTID = 0;
            CandidateID = 0;
            CourseName = "";
            Specification = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Duration = 0;
            PassingDate = DateTime.Now;
            Result = "";
            Institution = "";
            CertifyBodyVendor = "";
            Country = "";
            LastUpdatedDate = DateTime.Now;
            ErrorMessage = "";

        }

        #region Properties
        public int CTID { get; set; }
        public int CandidateID { get; set; }
        public string CourseName { get; set; }
        public string Specification { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public DateTime PassingDate { get; set; }
        public string Result { get; set; }
        public string Institution { get; set; }
        public string CertifyBodyVendor { get; set; }
        public string Country { get; set; }
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
        public string PassingDateInString
        {
            get { return PassingDate.ToString("dd MMM yyyy"); }
        }


        #endregion

        #region Functions

        public static CandidateTraining Get(int id, long nUserID)
        {
            return CandidateTraining.Service.Get(id, nUserID);
        }
        public static CandidateTraining Get(string sSQL, long nUserID)
        {
            return CandidateTraining.Service.Get(sSQL, nUserID);
        }
        public static List<CandidateTraining> Gets(int nCID, long nUserID)
        {
            return CandidateTraining.Service.Gets(nCID, nUserID);
        }
        public static List<CandidateTraining> Gets(string sSQL, long nUserID)
        {
            return CandidateTraining.Service.Gets(sSQL, nUserID);
        }
        public CandidateTraining IUD(int nDBOperation, long nUserID)
        {
            return CandidateTraining.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICandidateTrainingService Service
        {
            get { return (ICandidateTrainingService)Services.Factory.CreateService(typeof(ICandidateTrainingService)); }
        }

        #endregion
    }
    #endregion

    #region ICandidateTraining interface

    public interface ICandidateTrainingService
    {
        CandidateTraining Get(int id, Int64 nUserID);
        CandidateTraining Get(string sSQL, Int64 nUserID);
        List<CandidateTraining> Gets(int nCID, Int64 nUserID);
        List<CandidateTraining> Gets(string sSQL, Int64 nUserID);
        CandidateTraining IUD(CandidateTraining oCandidateTraining, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
