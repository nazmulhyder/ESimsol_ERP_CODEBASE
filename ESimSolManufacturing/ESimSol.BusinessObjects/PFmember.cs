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
    #region PFmember

    public class PFmember : BusinessObject
    {
        public PFmember()
        {
            PFMID = 0;
            PFSchemeID = 0;
            EmployeeID = 0;
            Description = "";
            PFBalance = 0.0;
            RequestTo = 0;
            RequestDate = DateTime.Now;
            RequestByDate = DateTime.Now;
            ApproveByDate = DateTime.MinValue;
            IsActive = false;
            InactiveDate = DateTime.Now;
            PFBs = new List<PFSchemeBenefit>();
            Params = "";
            ErrorMessage = "";
            EmployeeContribution = 0;
            PFMembershipDate = DateTime.Now;
        }

        #region Properties

        public int PFMID { get; set; }
        public int PFSchemeID { get; set; }
        public int EmployeeID { get; set; }
        public string Description { get; set; }
        public double PFBalance { get; set; }
        public int RequestTo { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestBy { get; set; }
        public DateTime RequestByDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public DateTime PFMembershipDate { get; set; }
        
        #endregion

        #region Derive

        public string PFSchemeName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string RequestToName { get; set; }
        public string RequestByName { get; set; }
        public string ApproveByName { get; set; }
        public double GrossAmount { get; set; }
        public double EmployeeContribution  { get; set; }
        public DateTime DateOfJoin { get; set; }
        public DateTime DateOfConfirmation { get; set; }
        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        public string DateOfConfirmationInString
        {
            get
            {
                return DateOfConfirmation.ToString("dd MMM yyyy");
            }
        }
        public string PFMembershipDateInStr { get { if(this.PFMembershipDate < Convert.ToDateTime("01 JAN 1990")) return "-"; return this.PFMembershipDate.ToString("dd MMM yyyy"); } }
        public List<PFSchemeBenefit> PFBs { get; set; }
        public List<PFmember> PFmembers { get; set; }

        public string EmployeeNameCode { get { return this.EmployeeName + " [" + this.EmployeeCode + "]"; } }
        public string RequestDateInStr { get { return (this.RequestTo > 0 && this.RequestBy > 0) ? this.RequestDate.ToString("dd MMM yyyy") : "--"; } }
        public string RequestByDateInStr { get { return (this.RequestBy > 0) ? this.RequestByDate.ToString("dd MMM yyyy") : "--"; } }
        public string ApproveByDateInStr { get { return (this.ApproveBy>0)?this.ApproveByDate.ToString("dd MMM yyyy"):"--"; } }
        public string InactiveDateInStr { get { return this.InactiveDate.ToString("dd MMM yyyy"); } }
        public string ActivityStatus { get { return (this.IsActive) ? "Active" : "Inactive"; } }
        public string ActivityStatusInST { get { return (this.IsActive) ? "Continued" : "Discontinued"; } }
        public double TotalPF
        {
            get
            {
                return Math.Round(this.EmployeeContribution) + Math.Round(this.PFBalance);
            }
        }

        #endregion


        #region Functions

        public static PFmember Get(int nPFMID, long nUserID)
        {
            return PFmember.Service.Get(nPFMID, nUserID);
        }
        public static List<PFmember> Gets(string sSQL, long nUserID)
        {
            return PFmember.Service.Gets(sSQL, nUserID);
        }
        public PFmember IUD(int nDBOperation, long nUserID)
        {
            return PFmember.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<PFmember> UploadXL(List<PFmember> oPFmembers, long nUserID)
        {
            return PFmember.Service.UploadXL(oPFmembers, nUserID);
        }
        public static List<PFmember> GetsPFLedgerReport(string sSQL, long nUserID)
        {
            return PFmember.Service.GetsPFLedgerReport(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPFmemberService Service
        {
            get { return (IPFmemberService)Services.Factory.CreateService(typeof(IPFmemberService)); }
        }
        #endregion
    }
    #endregion

    #region IPFmember interface

    public interface IPFmemberService
    {
        PFmember Get(int nPFMID, Int64 nUserID);
        List<PFmember> Gets(string sSQL, Int64 nUserID);
        PFmember IUD(PFmember oPFmember, int nDBOperation, Int64 nUserID);
        List<PFmember> UploadXL(List<PFmember> oPFmembers, Int64 nUserID);
        List<PFmember> GetsPFLedgerReport(string sSQL, Int64 nUserID);
    }
    #endregion
}
