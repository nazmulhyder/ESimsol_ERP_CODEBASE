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
    #region PFSchemeBenefit

    public class PFSchemeBenefit : BusinessObject
    {
        public PFSchemeBenefit()
        {
            PFSBID = 0;
            PFSchemeID = 0;
            MaturityYear = 0;
            MaturityYrCalculateAfter = EnumRecruitmentEvent.None;
            ContributionPercentage = 0;
            IsProfitShare = false;
            IsActive = false;
            InactiveDate = DateTime.Now;
        }

        #region Properties

        public int PFSBID { get; set; }
        public int PFSchemeID { get; set; }
        public int MaturityYear { get; set; }
        public EnumRecruitmentEvent MaturityYrCalculateAfter { get; set; }
        public bool IsActive { get; set; }

        public double ContributionPercentage { get; set; }
        public bool IsProfitShare { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region
        public PFScheme PFS { get; set; }
        public int MaturityYrCalculateAfterInt { get { return (int)this.MaturityYrCalculateAfter; } }
        public string MaturityYrCalculateAfterInStr { get { return this.MaturityYrCalculateAfter.ToString(); } }
        public string InactiveDateInStr { get { return this.InactiveDate.ToString("dd MMM yyyy"); } }
        public string ActivityStatus { get { return (this.IsActive) ? "Active" : "Inactive"; } }

        public string Benefit 
        { 
            get 
            {
                string sVal= Math.Round(this.ContributionPercentage,2).ToString() +"% contribution";
                if (this.IsProfitShare)
                {
                    sVal = sVal + " with profit share.";
                }
                return sVal; 
            } 
        }

        public string Maturity
        {
            get
            {
                return this.MaturityYear.ToString() + " Years of " + this.MaturityYrCalculateAfter.ToString();
            }
        }
        #endregion


        #region Functions

        public static PFSchemeBenefit Get(int nPFSBID, long nUserID)
        {
            return PFSchemeBenefit.Service.Get(nPFSBID, nUserID);
        }
        public static List<PFSchemeBenefit> Gets(string sSQL, long nUserID)
        {
            return PFSchemeBenefit.Service.Gets(sSQL, nUserID);
        }
        public PFSchemeBenefit IUD(int nDBOperation, long nUserID)
        {
            return PFSchemeBenefit.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPFSchemeBenefitService Service
        {
            get { return (IPFSchemeBenefitService)Services.Factory.CreateService(typeof(IPFSchemeBenefitService)); }
        }
        #endregion
    }
    #endregion

    #region IPFSchemeBenefit interface

    public interface IPFSchemeBenefitService
    {
        PFSchemeBenefit Get(int nPFSBID, Int64 nUserID);
        List<PFSchemeBenefit> Gets(string sSQL, Int64 nUserID);
        PFSchemeBenefit IUD(PFSchemeBenefit oPFSchemeBenefit, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
