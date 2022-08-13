using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region FNLabDipDetail

    public class FNLabDipDetail : BusinessObject
    {
        public FNLabDipDetail()
        {
            FNLabDipDetailID = 0;
            FabricID = 0;
            ReferenceLDID = 0;
            ColorSet = 3;
            ShadeCount = 3;
            KnitPlyYarn = EnumKnitPlyYarn.None;
            ColorName = string.Empty;
            RefNo = string.Empty;
            PantonNo = string.Empty;
            RGB = string.Empty;
            Combo = 1;
            LotNo = string.Empty;
            LDNo = "";
            ErrorMessage = "";
            LDNo = "";
            LabdipNo = "";
            FabricNo = "";
            Note = "";
            Construction = "";
            IssueDate = DateTime.Now;
            ContractorName = "";
            SubmitBy = 0;
            ReceiveBY = 0;
            ReceiveBYName = "";
            SubmitBYName = "";
            ReceiveBY = 0;
            SubmitBy = 0;
            ApprovedBy = 0;
            MKTPerson = "";
            ShadeNames = "";
            ShadeID_Ap = 0;
            OrderName="";
            ColorNo = "";
            ApprovalNote = "";
            SCNoFull = "";
            ShadeApproveDate =  DateTime.Now;
            LDStatus = EnumLDStatus.WaitingForIssue;
            PrepareByName = "";
            LightSource = "";
            LightSourceTwo = "";
            StyleNo = "";
            StrikeOff = "";
            ContactPersonnelID = 0;
            FSCDID = 0;
        }

        #region Properties
        public int FNLabDipDetailID { get; set; }
        public int FabricID { get; set; }
        public int ReferenceLDID { get; set; }
        public Int16 ColorSet { get; set; }
        public Int16 ShadeCount { get; set; }
        public EnumKnitPlyYarn KnitPlyYarn { get; set; }
        public string ColorName { get; set; }
        public string LDNo { get; set; }
        public string RefNo { get; set; }
        public string PantonNo { get; set; }
        public string SCNoFull { get; set; }
        public string RGB { get; set; }
        public string LabdipNo { get; set; }
        public string PrepareByName { get; set; }
        public string StyleNo { get; set; }
        public string FabricNo { get; set; }
        public string MKTPerson { get; set; }
        public Int16 Combo { get; set; }
        public string LotNo { get; set; }
        public string Note { get; set; }
        public string OrderName { get; set; }
        public int ReceiveBY { get; set; }
        public int SubmitBy { get; set; }
        public EnumShade ShadeID_Ap { get; set; }
        public EnumLDStatus LDStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ColorNo { get; set; }
        public string StrikeOff { get; set; }
        public string LightSourceDesc { get; set; }
        public int LightSourceID { get; set; }
        public string ApprovalNote { get; set; }
        
        #region Derived Property
        public int ContactPersonnelID { get; set; }
        public int FSCDID { get; set; }
        public string Params { get; set; }
        public string ShadeNames { get; set; }
        public string ErrorMessage { get; set; }
        public string KnitPlyYarnInString { get { return this.KnitPlyYarn.ToString(); } }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string LightSource { get; set; }
        public string LightSourceTwo { get; set; }
        public string ContractorName { get; set; }
        public string Construction { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ShadeApproveDate { get; set; }
        public string ProductNameCode
        {
            get { return this.ProductName+"["+this.ProductCode + "]"; }
        }
        public string IssueDateSt
        {
            get { return this.IssueDate.ToString("dd MMM yyyy"); }
        }
        public string ShadeApproveDateStr
        {
            get { return this.ShadeApproveDate.ToString("dd MMM yyyy"); }
        }
        private string _sStatus="";
        public string Status
        {
            get {
                _sStatus = LDStatus.ToString();
                return _sStatus; 
            }
        }
        public string ReceiveBYName { get; set; }
        public string SubmitBYName { get; set; }
        public string ShadeStr
        {
            get
            {
                if (this.ShadeID_Ap == EnumShade.NA) return "";
                else return this.ShadeID_Ap.ToString();
            }
        }
        #endregion

        #endregion

        #region Functions
        public FNLabDipDetail IUD(int nDBOperation, int nUserID)
        {
            return FNLabDipDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public FNLabDipDetail UpdateLot(int nUserID)
        {
            return FNLabDipDetail.Service.UpdateLot(this,  nUserID);
        }
        public static FNLabDipDetail Get(int nFNLabDipDetailID, int nUserID)
        {
            return FNLabDipDetail.Service.Get(nFNLabDipDetailID, nUserID);
        }
        public static List<FNLabDipDetail> Gets(string sSQL, int nUserID)
        {
            return FNLabDipDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<FNLabDipDetail> GetsBy(int nFabricID, int nUserID)
        {
            return FNLabDipDetail.Service.GetsBy(nFabricID, nUserID);
        }

        public static List<FNLabDipDetail> IssueLDNoMultiple(List<FNLabDipDetail> oFNLabDipDetails, int nUserID)
        {
            return FNLabDipDetail.Service.IssueLDNoMultiple(oFNLabDipDetails, nUserID);
        }

        public FNLabDipDetail Save_LDNo(int nUserID)
        {
            return FNLabDipDetail.Service.Save_LDNo(this, nUserID);
        }
        public FNLabDipDetail Submited(int nUserID)
        {
            return FNLabDipDetail.Service.Submited(this, nUserID);
        }
        public FNLabDipDetail Issued(int nUserID)
        {
            return FNLabDipDetail.Service.Issued(this, nUserID);
        }
        public FNLabDipDetail UpdateShade(int nUserID)
        {
            return FNLabDipDetail.Service.UpdateShade(this, nUserID);
        }
        public FNLabDipDetail UpdateColorSet(long nUserID)
        {
            return FNLabDipDetail.Service.UpdateColorSet(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNLabDipDetailService Service
        {
            get { return (IFNLabDipDetailService)Services.Factory.CreateService(typeof(IFNLabDipDetailService)); }
        }
        #endregion

    }
    #endregion

    #region IFNLabDipDetail interface

    public interface IFNLabDipDetailService
    {
        FNLabDipDetail IUD(FNLabDipDetail oFNLabDipDetail, int nDBOperation, int nUserID);
        FNLabDipDetail UpdateLot(FNLabDipDetail oFNLabDipDetail, int nUserID);
        FNLabDipDetail Get(int nID, int nUserID);
        List<FNLabDipDetail> GetsBy(int nFabricID, int nUserID);
        List<FNLabDipDetail> Gets(string sSQL, int nUserID);
        List<FNLabDipDetail> IssueLDNoMultiple(List<FNLabDipDetail> oFNLabDipDetails, int nUserID);
        FNLabDipDetail Save_LDNo(FNLabDipDetail oFNLabDipDetail, int nUserID);
        FNLabDipDetail Submited(FNLabDipDetail oFNLabDipDetail, int nUserID);
        FNLabDipDetail Issued(FNLabDipDetail oFNLabDipDetail, int nUserID);
        FNLabDipDetail UpdateShade(FNLabDipDetail oFNLabDipDetail, int nUserID);
        FNLabDipDetail UpdateColorSet(FNLabDipDetail oFNLabDipDetail, Int64 nUserID);
        
    }
    #endregion
}