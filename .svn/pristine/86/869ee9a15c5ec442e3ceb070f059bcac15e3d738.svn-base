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
    #region CandidateReference
    public class CandidateReference : BusinessObject
    {
        public CandidateReference()
        {
            CRefID = 0;
            CandidateID = 0;
            Name = "";
            Organization = "";
            Department = "";
            Designation = "";
            ContactNo = "";
            Email = "";
            Address = "";
            Relation = "";
            LastUpdatedDate = DateTime.Now;
            ErrorMessage = "";



        }

        #region Properties
        public int CRefID { get; set; }
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Relation { get; set; }
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

        public static CandidateReference Get(int id, long nUserID)
        {
            return CandidateReference.Service.Get(id, nUserID);
        }
        public static CandidateReference Get(string sSQL, long nUserID)
        {
            return CandidateReference.Service.Get(sSQL, nUserID);
        }
        public static List<CandidateReference> Gets(int nCID, long nUserID)
        {
            return CandidateReference.Service.Gets(nCID, nUserID);
        }
        public static List<CandidateReference> Gets(string sSQL, long nUserID)
        {
            return CandidateReference.Service.Gets(sSQL, nUserID);
        }
        public CandidateReference IUD(int nDBOperation, long nUserID)
        {
            return CandidateReference.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICandidateReferenceService Service
        {
            get { return (ICandidateReferenceService)Services.Factory.CreateService(typeof(ICandidateReferenceService)); }
        }

        #endregion
    }
    #endregion

    #region ICandidateReference interface

    public interface ICandidateReferenceService
    {
        CandidateReference Get(int id, Int64 nUserID);
        CandidateReference Get(string sSQL, Int64 nUserID);
        List<CandidateReference> Gets(int nCID, Int64 nUserID);
        List<CandidateReference> Gets(string sSQL, Int64 nUserID);
        CandidateReference IUD(CandidateReference oCandidateReference, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
