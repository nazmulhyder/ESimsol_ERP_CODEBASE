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
    #region LabDipDetail

    public class LabDipDetail : BusinessObject
    {
        public LabDipDetail()
        {
            LabDipDetailID = 0;
            LabDipID = 0;
            LabdipChallanID = 0;
            ProductID = 0;
            ColorSet = 3;
            ShadeCount = 3;
            KnitPlyYarn = EnumKnitPlyYarn.None;
            ColorName = string.Empty;
            RefNo = string.Empty;
            PantonNo = string.Empty;
            RGB = string.Empty;
            ColorNo = "";
            Combo = 1;
            LotNo = string.Empty;
            ColorCreateBy = 0;
            ColorCreateDate = DateTime.Today;
            DBUserID = 0;
            TwistedGroup = 0;
            Gauge = 0;
            ErrorMessage = "";
            Params = "";
            ColorCode = "";
            CellRowSpans = new List<CellRowSpan>();
            LabdipColorID = 0;
            LDNo = "";
            OrderDate = DateTime.Now;
            FabricNo = "";
            SubmitBy=0;
            OrderTypeSt = "";
            OrderNo = "";
            OrderStatus = EnumLabdipOrderStatus.Initialized;
            Construction = "";
            ContractorID = 0;
            ParentID = 0;
            ContractorName = "";
            ContractorAddress = "";
            IsInHouse = false;
        }

        #region Properties
        public int LabDipDetailID { get; set; }
        public int LabDipID { get; set; }
        public int ProductID { get; set; }
        public Int16 ColorSet { get; set; }
        public Int16 ShadeCount { get; set; }
        public EnumKnitPlyYarn KnitPlyYarn { get; set; }
        public string ColorName { get; set; }
        public string RefNo { get; set; }
        public string PantonNo { get; set; }
        public string RGB { get; set; }
        public string ColorNo { get; set; }
        public string ColorCode { get; set; }
        public Int16 Combo { get; set; }
        public string LotNo { get; set; }
        public int ColorCreateBy { get; set; }
        public int SubmitBy { get; set; }
        public DateTime ColorCreateDate { get; set; }
        public int TwistedGroup { get; set; }
        public int Gauge { get; set; }
        public int DBUserID { get; set; }
        public int LabdipColorID { get; set; }
        public int LabdipChallanID { get; set; }
        public int ParentID { get; set; }
        public string ParentProduct { get; set; }
        public string PantonPageNo { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        
        #region Derived Property
        public string WarpWeftTypeSt { get { return EnumObject.jGet(this.WarpWeftType); } }
        public int WarpWeftTypeInt { get { return (int)this.WarpWeftType; } }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public string KnitPlyYarnInString { get { return this.KnitPlyYarn.ToString(); } }
        public LabDip LabDip { get; set; }
        public string LabdipNo { get; set; }
        public string LDNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime OrderDate { get; set; }
        public string FabricNo { get; set; }
        public string OrderTypeSt { get; set; }
        public string OrderNo { get; set; }
        public string Construction { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string ContractorAddress { get; set; }
        public List<CellRowSpan> CellRowSpans { get; set; }
        public string OrderDateSt
        {
            get { return this.OrderDate.ToString("dd MMM yyyy"); }
        }
        public string ProductNameCode
        {
            get { return this.ProductName + "[" + this.ProductCode + "]"; }
        }
        public string TwistedSt
        {
            get { return (this.TwistedGroup > 0 ? "Twisted" : "-"); }
        }
        public EnumLabdipOrderStatus OrderStatus { get; set; }
        public bool IsInHouse { get; set; }
        private string _sStatus = "";
        public string Status
        {
            get
            {
                //if (this.ApprovedBy == 0)
                //{
                //    _sStatus = "Wating For Approve";
                //}
                if (this.OrderStatus >= EnumLabdipOrderStatus.Approve && this.OrderStatus < EnumLabdipOrderStatus.LabdipDone)
                {
                    if (this.ColorCreateBy == 0)
                    {
                        _sStatus = "Issue";
                    }
                    else if (this.ColorCreateBy != 0 && this.SubmitBy == 0)
                    {
                        _sStatus = "Received";
                    }
                    else if (this.SubmitBy != 0)
                    {
                        _sStatus = "LabdipDone";
                    }
                }
                else
                {
                    _sStatus = EnumObject.jGet(this.OrderStatus); 
                }

                return _sStatus;
            }
        }
        #endregion

        #endregion

        #region Functions
        public LabDipDetail IUD(int nDBOperation, int nUserID)
        {
            return LabDipDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public LabDipDetail Revise(int nDBOperation, int nUserID)
        {
            return LabDipDetail.Service.Revise(this, nDBOperation, nUserID);
        }
        public LabDipDetail UpdateLot(int nUserID)
        {
            return LabDipDetail.Service.UpdateLot(this,  nUserID);
        }
        public static LabDipDetail Get(int nLabDipDetailID, int nUserID)
        {
            return LabDipDetail.Service.Get(nLabDipDetailID, nUserID);
        }
        public static List<LabDipDetail> Gets(string sSQL, int nUserID)
        {
            return LabDipDetail.Service.Gets(sSQL, nUserID);
        }
        public static LabDipDetail IssueColor(int nLabDipDetailID, int nUserID)
        {
            return LabDipDetail.Service.IssueColor(nLabDipDetailID, nUserID);
        }
        public static List<LabDipDetail> IssueColorMultiple(int[] LabDipDetailIDs, int nUserID)
        {
            return LabDipDetail.Service.IssueColorMultiple(LabDipDetailIDs, nUserID);
        }
        public static List<LabDipDetail> MakeTwistedGroup(string sLabDipDetailID, int nLabDipID, int nTwistedGroup, int nParentID, int nDBOperation, int nUserID)
        {
            return LabDipDetail.Service.MakeTwistedGroup(sLabDipDetailID, nLabDipID, nTwistedGroup, nParentID, nDBOperation, nUserID);
        }
        public LabDipDetail Save_ColorNo(int nUserID)
        {
            return LabDipDetail.Service.Save_ColorNo(this, nUserID);
        }
        public LabDipDetail Save_PantonNo(int nUserID)
        {
            return LabDipDetail.Service.Save_PantonNo(this, nUserID);
        }
        public LabDipDetail LabDip_Receive_Submit(int nDBOperation, int nUserID)
        {
            return LabDipDetail.Service.LabDip_Receive_Submit(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILabDipDetailService Service
        {
            get { return (ILabDipDetailService)Services.Factory.CreateService(typeof(ILabDipDetailService)); }
        }
        #endregion


    }
    #endregion

    #region ILabDipDetail interface

    public interface ILabDipDetailService
    {
        LabDipDetail IUD(LabDipDetail oLabDipDetail, int nDBOperation, int nUserID);
        LabDipDetail Revise(LabDipDetail oLabDipDetail, int nDBOperation, int nUserID);
        LabDipDetail UpdateLot(LabDipDetail oLabDipDetail, int nUserID);
        LabDipDetail Get(int nID, int nUserID);
        List<LabDipDetail> Gets(string sSQL, int nUserID);
        LabDipDetail IssueColor(int nLabDipDetailID, int nUserID);
        List<LabDipDetail> IssueColorMultiple(int[] LabDipDetailIDs, int nUserID);
        List<LabDipDetail> MakeTwistedGroup(string sLabDipDetailID, int nLabDipID, int nTwistedGroup, int nParentID, int nDBOperation, int nUserID);
        LabDipDetail Save_ColorNo(LabDipDetail oLabDipDetail, Int64 nUserID);
        LabDipDetail Save_PantonNo(LabDipDetail oLabDipDetail, Int64 nUserID);
        LabDipDetail LabDip_Receive_Submit(LabDipDetail oLabDipDetail, int nDBOperation, int nUserID);
    }
    #endregion
}