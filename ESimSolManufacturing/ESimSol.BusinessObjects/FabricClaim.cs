using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FabricClaim
    public class FabricClaim : BusinessObject
    {
        public FabricClaim()
        {
            FabricClaimID = 0;
            FSCID = 0;
            ParentFSCID = 0;
            Subject = "";
            Remarks = "";
            PrepareBy = 0;
            CheckedBy = 0;
            CheckedDate = DateTime.Now;
            Note_Checked = "";
            Note_Approve = "";
            ParentSCNo = "";
            FabricReturnChallanID = 0;
            SCNoFull = "";
            OrderName = "";
            ContractorName = "";
            BuyerName = "";
            BuyerAddress = "";
            ContractorAddress = "";
            ContractorPhone = "";
            ContractorFax = "";
            ContractorEmail = "";
            MKTPName = "";
            MKTPNickName = "";
            Currency = "";
            CPersonName = "";
            LCTermsName = "";
            ApproveByName = "";
            PreapeByName = "";
            LightSourceName = "";
            LightSourceNameTwo = "";
            Amount = 0;
            Qty = 0;
            AttCount = 0;
            PINo = "";
            ErrorMessage = "";
            FabricClaimDetails = new List<FabricClaimDetail>();
            SCDate = DateTime.Now;
        }

        #region Property
        public int FabricClaimID { get; set; }
        public int FSCID { get; set; }
        public int ParentFSCID { get; set; }
        public string Subject { get; set; }
        public string Remarks { get; set; }
        public int PrepareBy { get; set; }
        public int CheckedBy { get; set; }
        public DateTime CheckedDate { get; set; }
        public DateTime SCDate { get; set; }
        public string Note_Checked { get; set; }
        public string Note_Approve { get; set; }
        public string ParentSCNo { get; set; }
        public int FabricReturnChallanID { get; set; }
        public string SCNoFull { get; set; }
        public string OrderName { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAddress { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorFax { get; set; }
        public string ContractorEmail { get; set; }
        public string MKTPName { get; set; }
        public string MKTPNickName { get; set; }
        public string Currency { get; set; }
        public string CPersonName { get; set; }
        public string LCTermsName { get; set; }
        public string ApproveByName { get; set; }
        public string PreapeByName { get; set; }
        public string LightSourceName { get; set; }
        public string LightSourceNameTwo { get; set; }
        public double Amount { get; set; }
        public double Qty { get; set; }
        public int AttCount { get; set; }
        public string PINo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<FabricClaimDetail> FabricClaimDetails { get; set; }
        public string CheckedDateInString
        {
            get
            {
                return CheckedDate.ToString("dd MMM yyyy");
            }
        }
        public string SCDateInString
        {
            get
            {
                return SCDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FabricClaim> Gets(long nUserID)
        {
            return FabricClaim.Service.Gets(nUserID);
        }
        public static List<FabricClaim> Gets(string sSQL, long nUserID)
        {
            return FabricClaim.Service.Gets(sSQL, nUserID);
        }
        public FabricClaim Get(int id, long nUserID)
        {
            return FabricClaim.Service.Get(id, nUserID);
        }
        public FabricClaim Save(long nUserID)
        {
            return FabricClaim.Service.Save(this, nUserID);
        }
        public string Delete(FabricClaim oFabricClaim, long nUserID)
        {
            return FabricClaim.Service.Delete(oFabricClaim, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricClaimService Service
        {
            get { return (IFabricClaimService)Services.Factory.CreateService(typeof(IFabricClaimService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricClaim interface
    public interface IFabricClaimService
    {
        FabricClaim Get(int id, Int64 nUserID);
        List<FabricClaim> Gets(Int64 nUserID);
        List<FabricClaim> Gets(string sSQL, Int64 nUserID);
        string Delete(FabricClaim oFabricClaim, Int64 nUserID);
        FabricClaim Save(FabricClaim oFabricClaim, Int64 nUserID);
    }
    #endregion
}
