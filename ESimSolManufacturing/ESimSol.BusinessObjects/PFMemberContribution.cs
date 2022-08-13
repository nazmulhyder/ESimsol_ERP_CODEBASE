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
    #region PFMemberContribution

    public class PFMemberContribution : BusinessObject
    {
        public PFMemberContribution()
        {
            PFMCID = 0;
            PFSchemeID = 0;
            MinAmount = 0;
            MaxAmount = 0;
            IsPercent = false;
            Value = 0;
            CalculationOn = EnumPayrollApplyOn.None;
            IsActive = false;
        }

        #region Properties

        public int PFMCID { get; set; }
        public int PFSchemeID { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public bool IsPercent { get; set; }
        public double Value { get; set; }
        public EnumPayrollApplyOn CalculationOn { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region
        public PFScheme PFS { get; set; }

        public string CalculationOnInStr { get { return this.CalculationOn.ToString(); } }
        public int CalculationOnInInt { get { return (int)this.CalculationOn; } }
        public string ActivityStatus { get { return (this.IsActive) ? "Active" : "Inactive"; } }

        public string ContributionRange
        {
            get
            {
                return Global.MillionFormat(this.MinAmount) +" - "+ Global.MillionFormat(this.MaxAmount); 
            }
        }
        public string EmployeeContribution
        {
            get
            {
                string sVal = "";
                if (this.IsPercent)
                {
                    sVal = Math.Round(this.Value, 2).ToString() + "% is the employee's " + this.CalculationOn.ToString() + " salary.";
                }
                else
                {
                    sVal = "Fixed amount " + Global.MillionFormat(this.Value) + "";
                }
                return sVal;
            }
        }


        #endregion


        #region Functions

        public static PFMemberContribution Get(int nPFMCID, long nUserID)
        {
            return PFMemberContribution.Service.Get(nPFMCID, nUserID);
        }
        public static List<PFMemberContribution> Gets(string sSQL, long nUserID)
        {
            return PFMemberContribution.Service.Gets(sSQL, nUserID);
        }
        public PFMemberContribution IUD(int nDBOperation, long nUserID)
        {
            return PFMemberContribution.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPFMemberContributionService Service
        {
            get { return (IPFMemberContributionService)Services.Factory.CreateService(typeof(IPFMemberContributionService)); }
        }
        #endregion
    }
    #endregion

    #region IPFMemberContribution interface

    public interface IPFMemberContributionService
    {
        PFMemberContribution Get(int nPFMCID, Int64 nUserID);
        List<PFMemberContribution> Gets(string sSQL, Int64 nUserID);
        PFMemberContribution IUD(PFMemberContribution oPFMemberContribution, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
