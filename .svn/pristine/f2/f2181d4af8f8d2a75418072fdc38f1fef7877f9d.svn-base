using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region GRN
    public class GRN : BusinessObject
    {
        public GRN()
        {
            GRNID = 0;
            GRNNo = "";
            GRNDate = DateTime.Now;
            GLDate = DateTime.Now;
            GRNType = EnumGRNType.None;
            GRNTypeInt = 0;
            GRNStatus = EnumGRNStatus.Initialize;
            GRNStatusInt = 0;
            BUID = 0;
            ContractorID = 0;
            RefObjectID = 0;
            Remarks = "";
            CurrencyID = 0;
            ConversionRate = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            ReceivedBy = 0;
            ReceivedDateTime = DateTime.Now;
            ChallanNo = "";
            ChallanDate = DateTime.Today;
            StoreID = 0;
            StoreName = "";
            ReceivedByName = "";
            ContractorName = "";
            ContractorPhone = "";
            Address = "";
            ConShortName = "";
            Currency = "";
            Amount = 0;
            PrepareByName = "";
            ApproveByName = "";
            RefObjectNo = "";
            RefObjectDate = DateTime.Now;
            RefObjectAmount = 0;
            RefObjectRemarks = "";
            BUName = "";
            BUCode = "";
            GatePassNo = "";
            VehicleNo = "";
            StoreNameWithoutLocation = "";
            ErrorMessage = "";
            IsWillVoucherEffect = true;
            MRFNo = "";
            ImportLCNo = "";
            RefAmount = 0;
            RefDisCount = 0;
            RefServiceCharge = 0;
            TotalQty = 0;
            MUName = "";
            MRIRNo = "";
            MRIRDate = DateTime.Today;
            GRNDetails = new List<GRNDetail>();
            GRNDetailBreakDowns = new List<GRNDetailBreakDown>();
            Company = new BusinessObjects.Company();
            BusinessUnit = new BusinessUnit();
        }
        #region Properties
        public int GRNID { get; set; }
        public string GRNNo { get; set; }
        public DateTime GRNDate { get; set; }
        public DateTime GLDate { get; set; }
        public EnumGRNType GRNType { get; set; }
        public int GRNTypeInt { get; set; }
        public EnumGRNStatus GRNStatus { get; set; }
        public int GRNStatusInt { get; set; }
        public int BUID { get; set; }
        public string MRIRNo { get; set; }
        public DateTime MRIRDate { get; set; }
        public int RefObjectID { get; set; }
        public string Remarks { get; set; }
        public int CurrencyID { get; set; }
        public double ConversionRate { get; set; }
        public string StoreNameWithoutLocation { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public int ReceivedBy { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string ContractorPhone { get; set; }
        public string Address { get; set; }
        public string ConShortName { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string PrepareByName { get; set; }
        public string ApproveByName { get; set; }
        public string ReceivedByName { get; set; }
      public string GatePassNo { get; set; }
      public string VehicleNo { get; set; }
        public string RefObjectNo { get; set; }
        public DateTime RefObjectDate { get; set; }
        public double RefObjectAmount { get; set; }
        public string RefObjectRemarks { get; set; }
        public string MRFNo { get; set; }
        public string BUName { get; set; }
        public string BUCode { get; set; }
        public string ImportLCNo { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        public double RefAmount { get; set; }
        public double RefServiceCharge { get; set; }
        public double RefDisCount { get; set; }
        public double TotalQty { get; set; }
        public string MUName { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties      
        public BusinessUnit BusinessUnit { get; set; }
        public List<GRNDetail> GRNDetails { get; set; }
        public Company Company { get; set; }
        public List<GRNDetailBreakDown> GRNDetailBreakDowns { get; set; }
        public string GRNTypeSt { get { return EnumObject.jGet(this.GRNType); } }
        public string GRNStatusSt { get { return EnumObject.jGet(this.GRNStatus); } }
        public string GRNDateSt { get { return this.GRNDate.ToString("dd MMM yyyy"); } }
        public string GLDateSt { get { return this.GLDate.ToString("dd MMM yyyy"); } }
        public string MRIRDateSt { get { return this.MRIRDate.ToString("dd MMM yyyy"); } }
        
        public string ChallanDateSt { get { return this.ChallanDate.ToString("dd MMM yyyy"); } }
        public string ApproveDateSt { get { return this.ApproveBy == 0 ? "" : this.ApproveDate.ToString("dd MMM yyyy"); } }
        public string ReceivedDateTimeSt { get { return this.ReceivedBy == 0 ? "" : this.ReceivedDateTime.ToString("dd MMM yyyy"); } }
        public string RefObjectDateSt { get { return this.RefObjectDate.ToString("dd MMM yyyy"); } }
        public string TotalQtyInSt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.TotalQty) + " " + this.MUName;
            }
        }
        public string IsWillVoucherEffectSt
        {
            get
            {
                if (this.IsWillVoucherEffect)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        #endregion

        #region Functions

        public GRN Get(int id, int nUserID)
        {
            return GRN.Service.Get(id, nUserID);
        }
        public GRN Save(int nUserID)
        {
            return GRN.Service.Save(this, nUserID);
        }

        public GRN UpdateMRIRInfo(int nUserID)
        {
            return GRN.Service.UpdateMRIRInfo(this, nUserID);
        }
        public GRN Approve(int nUserID)
        {
            return GRN.Service.Approve(this, nUserID);
        }

        public GRN UndoApprove(int nUserID)
        {
            return GRN.Service.UndoApprove(this, nUserID);
        }

        
        public GRN Receive(int nUserID)
        {
            return GRN.Service.Receive(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return GRN.Service.Delete(id, nUserID);
        }
        public static List<GRN> Gets(int nUserID)
        {
            return GRN.Service.Gets(nUserID);
        }
        
        public static List<GRN> Gets(string sSQL, int nUserID)
        {
            return GRN.Service.Gets(sSQL, nUserID);
        }
        public GRN UpdateVoucherEffect(long nUserID)
        {
            return GRN.Service.UpdateVoucherEffect(this, nUserID);
        }
        
        #endregion


        #region ServiceFactory
        internal static IGRNService Service
        {
            get { return (IGRNService)Services.Factory.CreateService(typeof(IGRNService)); }
        }
        #endregion
    }
    #endregion
    
    #region IGRN interface
    public interface IGRNService
    {
        GRN Get(int id, int nUserID);
        List<GRN> Gets(int nUserID);
        string Delete(int id, int nUserID);
        GRN Save(GRN oGRN, int nUserID);
        GRN UpdateMRIRInfo(GRN oGRN, int nUserID);
        GRN Approve(GRN oGRN, int nUserID);
        GRN UndoApprove(GRN oGRN, int nUserID);
        GRN Receive(GRN oGRN, int nUserID);
        List<GRN> Gets(string sSQL, int nUserID);
        GRN UpdateVoucherEffect(GRN oGRN, Int64 nUserID);   
    }
    #endregion
}