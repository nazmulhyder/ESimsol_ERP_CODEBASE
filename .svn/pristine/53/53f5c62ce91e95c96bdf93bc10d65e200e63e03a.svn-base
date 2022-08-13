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
    #region FabricBatchQCDetail
    public class FabricBatchQCDetail : BusinessObject
    {
        public FabricBatchQCDetail()
        {
            FBQCDetailID = 0;
            FBQCID = 0;
            LotNo = "";
            Qty = 0;
            DeliveryBy = 0;
            Grade = EnumFBQCGrade.None;
            DeliveryDate = DateTime.MinValue;
            StoreRcvDate = DateTime.MinValue;
            ReceiveBy = 0;
            ErrorMessage = "";
            Remark = "";
            ShiftID = 0;
            //FBPID = 0;
            FabricBatchQCDetails = new List<FabricBatchQCDetail>();
            WorkingUnitID = 0;
            FBQCDetailIDs = "";
            DBServerDateTime = DateTime.Today;
            ReedCount = 0;
            Dent = "";
            TSUID = 0;
            ProDate = DateTime.MinValue;
            FabricQCGradeID = 0;
            QCGrade = "";
            DeliveryBy = 0;
            LoomNo = "";
            DispoNo = "";
            BuyerName = "";
            Construction = "";
            ShiftName = "";
            LotBalance = 0;
            QtyTR = 0;
            LotID = 0;
            BatchNo = "";
            sSearchParams = "";
            Width = 0;
            FEOSID = 0;
            IsYD = false;
            FPFID = 0;
            FabricFaultType = EnumFabricFaultType.None;
            BUType = EnumBusinessUnitType.None;
        }

        #region Properties
        public int FBQCDetailID { get; set; }
        public int FBQCID { get; set; }
        public int FabricQCGradeID { get; set; }
        public EnumFBQCGrade Grade { get; set; }
        public int DeliveryBy { get; set; }
        public string LotNo { get; set; }
        public double Qty { get; set; }
        public double Width { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime StoreRcvDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Remark { get; set; }
        public int ShiftID { get; set; }
       // public int FBPID { get; set; }
        public string FBQCDetailIDs { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string WarpLot { get; set; }
        public string WeftLot { get; set; }
        public string ProcessType { get; set; }
        public int FEOSID { get; set; }
        public int TSUID { get; set; }
        public int LotID { get; set; }
        public string BatchNo { get; set; }
        public string sSearchParams { get; set; }
        public EnumFBQCGrade QCGradeType { get; set; }
        public EnumExcellColumn GradeSL { get; set; }
        public bool IsYD { get; set; }
        public EnumFabricRequestType OrderType { get; set; }
        #endregion

        #region Derived Properties
        public string  ShiftName { get; set; }
        public double LotBalance { get; set; }
        public DateTime ProDate { get; set; }
        public double ReedCount { get; set; }
        public string DBServerDateTimeStr { get { return this.DBServerDateTime.ToString("dd MMM yyyy"); } }
        public FabricBatchQC FabricBatchQC { get; set; }
        public List<FabricBatchQCDetail> FabricBatchQCDetails { get; set; }
        public int FPFID { get; set; }
        public EnumFabricFaultType FabricFaultType { get; set; }
        public EnumBusinessUnitType BUType { get; set; }
        public int WorkingUnitID { get; set; }
        public int GradeInInt { get; set; }
        public int ReceiveBy { get; set; }
        public string Dent { get; set; }
    
        #region  For Report
        public string LoomNo { get; set; }
        public string DispoNo { get; set; }
        public string BuyerName { get; set; }
        public string Construction { get; set; }
        public double QtyTR { get; set; }
        
        #endregion      
        public string LockInString
        {
            get
            {
                if (this.DeliveryBy!=0)
                {
                    return "Yes";
                }else
                {
                    return "--";
                }
            }
        }
        public string ReedCountWithDent
        {
            get
            {

                if (this.ReedCount > 0)
                {
                    return this.ReedCount.ToString().Split('.')[0] + "/" + this.Dent;
                }
                else
                {
                    return "";
                }

            }
        }
        public string ProDateStr { get { return (this.ProDate == DateTime.MinValue) ? "--" : this.ProDate.ToString("dd MMM yyyy"); } }
        public double QtyInMeter
        {
            get
            {
                return Global.GetMeter(this.Qty,2);
            }
        }
        public double WidthInMeter
        {
            get
            {
                return Global.GetMeter(this.Width, 2);
            }
        }

        public string QtySt
        {
            get
            {
                if (this.Qty < 0) return "(" + Global.MillionFormat(this.Qty * (-1)) + ")";
                else if (this.Qty == 0) return "-";
                else return Global.MillionFormat(this.Qty);
            }
        }
        public string QtyInMeterSt
        {
            get
            {
                double nQtyInMeter = Global.GetMeter(this.Qty, 2);
                if (nQtyInMeter < 0) return "(" + Global.MillionFormat(nQtyInMeter * (-1)) + ")";
                else if (nQtyInMeter == 0) return "-";
                else return Global.MillionFormat(nQtyInMeter);
            }
        }
        public string StoreRcvDateInString
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.StoreRcvDate == MinValue || this.StoreRcvDate == MinValue1 || this.ReceiveBy == 0)
                {
                    return "-";
                }
                else
                {
                    return StoreRcvDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string DeliveryDateSt
        {
            get
            {
                if (this.DeliveryBy!=0)
                {
                    return this.DeliveryDate.ToString("dd MMM yyyy");
                }
                else
                {
                    return "";
                }

            }
        }
        public string GradeInString
        {
            get
            {
                return this.Grade.ToString();
            }
        }
       
        public string QCGrade { get; set; }
        
        #endregion

        #region Functions

        public static List<FabricBatchQCDetail> Gets(int nFBQCID, long nUserID)
        {
            return FabricBatchQCDetail.Service.Gets(nFBQCID, nUserID);
        }
        public static List<FabricBatchQCDetail> Gets(string sSQL, long nUserID)
        {
            return FabricBatchQCDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricBatchQCDetail Save(long nUserID)
        {
            return FabricBatchQCDetail.Service.Save(this, nUserID);
        }
        public FabricBatchQCDetail Lock(long nUserID)
        {
            return FabricBatchQCDetail.Service.Lock(this, nUserID);
        }
        public FabricBatchQCDetail LockFabricBatchQCDetail(long nUserID)
        {
            return FabricBatchQCDetail.Service.LockFabricBatchQCDetail(this, nUserID);
        }
        public FabricBatchQCDetail Get( int nFBQCDID, long nUserID)
        {
            return FabricBatchQCDetail.Service.Get(nFBQCDID, nUserID);
        }
        //public FabricBatchQCDetail ReceiveInDelivery(long nUserID)
        //{
        //    return FabricBatchQCDetail.Service.ReceiveInDelivery(this, nUserID);
        //}

        public static List<FabricBatchQCDetail> ReceiveInDelivery(FabricBatchQCDetail oFabricBatchQCDetail, long nUserID)
        {
            return FabricBatchQCDetail.Service.ReceiveInDelivery(oFabricBatchQCDetail, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return FabricBatchQCDetail.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricBatchQCDetailService Service
        {
            get { return (IFabricBatchQCDetailService)Services.Factory.CreateService(typeof(IFabricBatchQCDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricBatchQCDetail interface

    public interface IFabricBatchQCDetailService
    {
        List<FabricBatchQCDetail> Gets(int nFBQCID,  long nUserID);
        List<FabricBatchQCDetail> Gets(string sSQL, long nUserID);
        FabricBatchQCDetail Save(FabricBatchQCDetail oFabricBatchQCDetail, long nUserID);
        FabricBatchQCDetail Lock(FabricBatchQCDetail oFabricBatchQCDetail, long nUserID);
        FabricBatchQCDetail LockFabricBatchQCDetail(FabricBatchQCDetail oFabricBatchQCDetail, long nUserID);
        FabricBatchQCDetail Get(int nFBQCDID, long nUserID);
        List<FabricBatchQCDetail> ReceiveInDelivery(FabricBatchQCDetail oFabricBatchQCDetail, long nUserID);
        //FabricBatchQCDetail ReceiveInDelivery(FabricBatchQCDetail oFabricBatchQCDetail, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
