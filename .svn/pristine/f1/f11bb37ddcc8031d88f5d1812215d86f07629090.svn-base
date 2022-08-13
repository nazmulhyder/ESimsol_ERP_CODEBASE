using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region FUProcess

    public class FNBatchQCDetail : BusinessObject
    {
        public FNBatchQCDetail()
        {
            FNBatchQCDetailID = 0;
            FNBatchQCID = 0;
            Grade = EnumFBQCGrade.None;
            LotNo = string.Empty;
            Qty = 0;
            GSM = 00.0;
            Bowl_Skew = 00.0;
            PointsYard = 00.0; //Points Per 1000Yds
            IsLock = false;
            IsPassed = 0;
            LockDate = DateTime.MinValue;
            StoreRcvDate = DateTime.MinValue;
            RcvByID =0;
            DBServerDate = DateTime.MinValue;
            ShadeID = 0;
            FNBatchQC = new FNBatchQC();
            FNBatchQCDetailIDs = "";
            FNBatchQCDetails = new List<FNBatchQCDetail>();
            QtyDC = 0;
            QtyRC = 0;
            QtyExe = 0;
            LotID = 0;
            ErrorMessage = "";
            FSCDID = 0;
            ProDate = DateTime.Now;
            DeliveryDate = DateTime.Now;
            DeliveryBy = 0;
        }

        #region Properties

        public int FNBatchQCDetailID { get; set; }
        public int FNBatchQCID { get; set; }
        public EnumFBQCGrade Grade { get; set; }
        public string LotNo { get; set; }
        public double Qty { get; set; }
        public bool IsLock { get; set; }
        public DateTime LockDate { get; set; }
        public DateTime ProDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DeliveryBy { get; set; }
        public DateTime StoreRcvDate { get; set; }
        public DateTime DBServerDate { get; set; }
        public EnumFNShade ShadeID { get; set; }
        public int FSCDID { get; set; }
        public double GSM { get; set; }
        public double Bowl_Skew { get; set; }
        public double PointsYard { get; set; }
        public int RcvByID { get; set; }
        public int IsPassed { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int LotID { get; set; }
        public double QtyDC { get; set; }
        public double QtyRC { get; set; }
        public double QtyExe { get; set; }
        public int BUID { get; set; }
        public string FNBatchQCDetailIDs { get; set; }
        public List<FNBatchQCDetail> FNBatchQCDetails { get; set; }
        public FNBatchQC FNBatchQC { get; set; }
        public int WorkingUnitID { get; set; }
        public double Balance { get { return (this.Qty + this.QtyRC - this.QtyDC); } }
        public string GradeStr { get { return this.Grade.ToString(); } }
        public string LockDateStr { get { return  (this.LockDate==DateTime.MinValue)? "" : this.LockDate.ToString("dd MMM yyyy"); } }
        public string StoreRcvDateStr { get { return (this.StoreRcvDate == DateTime.MinValue) ? "" : this.StoreRcvDate.ToString("dd MMM yyyy"); } }
        public string DBServerDateStr { get { return (this.DBServerDate == DateTime.MinValue) ? "" : this.DBServerDate.ToString("dd MMM yyyy"); } }
        public string Lock { get { return (this.IsLock) ? "Yes" : "No"; } }
        public string IsPassedStr
        {
            get
            {
                if (IsPassed == 1) return "Pass"; else if (IsPassed == 2) return "Fail"; else return "";
            }
        }
        public double QtyInMtr { get { return Global.GetMeter(this.Qty, 2); } }

        public string ShadeStr { get { return (this.ShadeID == EnumFNShade.NA) ? "" : this.ShadeID.ToString(); } }

        #endregion

        #region Functions

        public static FNBatchQCDetail Get(int nFNBatchQCDetailID, long nUserID)
        {
            return FNBatchQCDetail.Service.Get(nFNBatchQCDetailID, nUserID);
        }
        public static List<FNBatchQCDetail> Gets(string sSQL, long nUserID)
        {
            return FNBatchQCDetail.Service.Gets(sSQL, nUserID);
        }
        public FNBatchQCDetail IUD(int nDBOperation, long nUserID)
        {
            return FNBatchQCDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public FNBatchQCDetail LockFNBatchQCDetail(long nUserID)
        {
            return FNBatchQCDetail.Service.LockFNBatchQCDetail(this, nUserID);
        }
        public FNBatchQCDetail ReceiveInDelivery(long nUserID)
        {
            return FNBatchQCDetail.Service.ReceiveInDelivery(this, nUserID);
        }
        public FNBatchQCDetail ReceiveInDeliveryNew(long nUserID)
        {
            return FNBatchQCDetail.Service.ReceiveInDeliveryNew(this, nUserID);
        }
        public static List<FNBatchQCDetail> ExcessQtyUpdate(List<FNBatchQCDetail> oFabrics, long nUserID)
        {
            return FNBatchQCDetail.Service.ExcessQtyUpdate(oFabrics, nUserID);
        }
       
        #endregion

        #region ServiceFactory
        internal static IFNBatchQCDetailService Service
        {
            get { return (IFNBatchQCDetailService)Services.Factory.CreateService(typeof(IFNBatchQCDetailService)); }
        }

        #endregion


    }
    #endregion

    #region IFUProcess interface

    public interface IFNBatchQCDetailService
    {
        FNBatchQCDetail Get(int nFNBatchQCDetailID, Int64 nUserID);
        List<FNBatchQCDetail> Gets(string sSQL, Int64 nUserID);
        FNBatchQCDetail IUD(FNBatchQCDetail oFNBatchQCDetail, int nDBOperation, Int64 nUserID);
        FNBatchQCDetail LockFNBatchQCDetail(FNBatchQCDetail oFNBatchQCDetail, Int64 nUserID);
        FNBatchQCDetail ReceiveInDelivery(FNBatchQCDetail oFNBatchQCDetail, Int64 nUserID);
        FNBatchQCDetail ReceiveInDeliveryNew(FNBatchQCDetail oFNBatchQCDetail, Int64 nUserID);
        List<FNBatchQCDetail> ExcessQtyUpdate(List<FNBatchQCDetail> oFNBatchQCDetails, Int64 nUserID);
    

        
    }
    #endregion
}
