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
    #region Candidate

    public class Candidate : BusinessObject
    {
        public Candidate()
        {
            CandidateID = 0;
            Code = "";
            Name = "";
            FatherName = "";
            MotherName = "";
            DateOfBirth = DateTime.Now;
            Gender = "";
            MaritalStatus = "";
            Nationalism = "";
            NationalID = "";
            Religious = "";
            PresentAddress = "";
            ParmanentAddress = "";
            ContactNo = "";
            AlternateContactNo = "";
            Email = "";
            AlternateEmail = "";
            Objective = "";
            PresentSalary = 0;
            ExpectedSalary = 0;
            CareerSummary = "";
            SpecialQualification = "";
            LastUpdatedDateTime = DateTime.Now;
            Photo = null;
            ErrorMessage = "";

        }

        #region Properties

        public int CandidateID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationalism { get; set; }
        public string NationalID { get; set; }
        public string Religious { get; set; }
        public string PresentAddress { get; set; }
        public string ParmanentAddress { get; set; }
        public string ContactNo { get; set; }
        public string AlternateContactNo { get; set; }
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public string Objective { get; set; }
        public double PresentSalary { get; set; }
        public double ExpectedSalary { get; set; }
        public string CareerSummary { get; set; }
        public string SpecialQualification { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public byte[] Photo { get; set; }
        #endregion

        #region Derived Property

        public List<CandidateEducation> CandidateEducations { get; set; }
        public List<CandidateExperience> CandidateExperiences { get; set; }
        public List<CandidateReference> CandidateReferences { get; set; }
        public List<CandidateTraining> CandidateTrainings { get; set; }
        public string DateOfBirthInString
        {
            get { return DateOfBirth.ToString("dd MMM yyyy"); }
        }
        public string LastUpdatedDateTimeInString
        {
            get { return LastUpdatedDateTime.ToString("dd MMM yyyy"); }
        }

        public CandidateUser CandidateUser { get; set; }
        public System.Drawing.Image CandidatePhoto { get; set; }
        #endregion

        #region Functions

        public static Candidate Get(int id, long nUserID)
        {
            return Candidate.Service.Get(id, nUserID);
        }
        public static Candidate Get(string sSQL, long nUserID)
        {
            return Candidate.Service.Get(sSQL, nUserID);
        }
        public static List<Candidate> Gets(long nUserID)
        {
            return Candidate.Service.Gets(nUserID);
        }
        public static List<Candidate> Gets(string sSQL, long nUserID)
        {
            return Candidate.Service.Gets(sSQL, nUserID);
        }
        public Candidate IUD(int nDBOperation, long nUserID)
        {
            return Candidate.Service.IUD(this, nDBOperation, nUserID);
        }
        public Candidate InsertNewCandidate(int nDBOperation, long nUserID)
        {
            return Candidate.Service.InsertNewCandidate(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICandidateService Service
        {
            get { return (ICandidateService)Services.Factory.CreateService(typeof(ICandidateService)); }
        }

        #endregion
    }
    #endregion

    #region ICandidate interface

    public interface ICandidateService
    {
        Candidate Get(int id, Int64 nUserID);
        Candidate Get(string sSQL, Int64 nUserID);
        List<Candidate> Gets(Int64 nUserID);
        List<Candidate> Gets(string sSQL, Int64 nUserID);
        Candidate IUD(Candidate oCandidate, int nDBOperation, Int64 nUserID);
        Candidate InsertNewCandidate(Candidate oCandidate, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
