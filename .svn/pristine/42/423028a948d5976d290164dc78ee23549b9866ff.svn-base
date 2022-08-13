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
    #region PFScheme

    public class PFScheme : BusinessObject
    {
        public PFScheme()
        {
            PFSchemeID = 0;
            Name = "";
            Description = "";
            IsRecognize = false;
            RecommandedSalaryHeadID=0;
            ApproveByDate = DateTime.MinValue;
            IsActive = false;
            InactiveDate = DateTime.Now;
            PFMCs = new List<PFMemberContribution>();
            PFBs = new List<PFSchemeBenefit>();
            ErrorMessage = "";
            
        }

        #region Properties

        public int PFSchemeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRecognize { get; set; }
        public int RecommandedSalaryHeadID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region

        public List<PFMemberContribution> PFMCs { get; set; }
        public List<PFSchemeBenefit> PFBs { get; set; }

        public string ApproveByDateInStr { get { return (this.ApproveBy>0)? this.ApproveByDate.ToString("dd MMM yyyy"):"--"; } }
        public string InactiveDateInStr { get { return this.InactiveDate.ToString("dd MMM yyyy"); } }
        public string IsRecognizeStatus { get { return (this.IsRecognize) ? "Done" : "--"; } }
        public string ActivityStatus { get { return (this.IsActive) ? "Active" : "Inactive"; } }

        public string ApproveByName { get; set; }

        #endregion


        #region Functions

        public static PFScheme Get(int nPFSchemeID, long nUserID)
        {
            return PFScheme.Service.Get(nPFSchemeID, nUserID);
        }
        public static List<PFScheme> Gets(string sSQL, long nUserID)
        {
            return PFScheme.Service.Gets(sSQL, nUserID);
        }
        public PFScheme IUD(int nDBOperation, long nUserID)
        {
            return PFScheme.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPFSchemeService Service
        {
            get { return (IPFSchemeService)Services.Factory.CreateService(typeof(IPFSchemeService)); }
        }
        #endregion
    }
    #endregion

    #region IPFScheme interface

    public interface IPFSchemeService
    {
        PFScheme Get(int nPFSchemeID, Int64 nUserID);
        List<PFScheme> Gets(string sSQL, Int64 nUserID);
        PFScheme IUD(PFScheme oPFScheme, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
