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
    #region WorkOrderDetail
    public class WorkOrderDetail : BusinessObject
    {
        public WorkOrderDetail()
        {
            WorkOrderDetailID = 0;
            WorkOrderID = 0;
            ProductID = 0;
            StyleID = 0;
            ColorID = 0;
            SizeID = 0;
            Measurement = "";
            ProductDescription = "";
            UnitID = 0;
            Qty = 0;
            UnitPrice = 0;
            ProductCode = "";
            ProductName = "";
            IsApplyColor = false;
            IsApplySize = false;
            ColorName = "";
            SizeName = "";
            StyleNo = "";
            BuyerName = "";
            UnitName = "";
            UnitSymbol = "";
            Amount = 0;
            YetToGRNQty = 0;
            GRNQty = 0;
            WorkOrderNo = "";
            WorkOrderDetailLogID = 0;
            WorkOrderLogID = 0;
            OrderRecapID = 0;
            RateUnit = 1;
            OrderRecapNo = "";
            WorkOrderStatus = EnumWorkOrderStatus.Intialize;
            YetToPurchaseReturnQty = 0;
            LotBalance = 0;
            LotID = 0;
            LotNo = "";
            MCDia = "";
            FinishDia = "";
            GSM = "";
            Remarks = "";
            Stretch_Length = "";
            ErrorMessage = "";
        }

        #region Property
        public int WorkOrderDetailID { get; set; }
        public int WorkOrderID { get; set; }
        public int OrderRecapID { get; set; }
        public int ProductID { get; set; }
        public int StyleID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public string Measurement { get; set; }
        public string ProductDescription { get; set; }
        public int UnitID { get; set; }
        public string WorkOrderNo { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public bool IsApplyColor { get; set; }
        public bool IsApplySize { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }
        public double Amount { get; set; }
        public double YetToGRNQty { get; set; }
        public double GRNQty { get; set; }
        public int WorkOrderDetailLogID { get; set; }
        public int WorkOrderLogID { get; set; }
        public string Reference { get; set; }
        public double RateUnit { get; set; }
        
        public string  OrderRecapNo  { get; set; }
        public EnumWorkOrderStatus WorkOrderStatus { get; set; }
        public int WorkOrderStatusInInt { get; set; }
        public double YetToPurchaseReturnQty { get; set; }
        public double LotBalance { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public string MCDia  { get; set; } 
        public string FinishDia  { get; set; }
        public string GSM { get; set; }
        public string Stretch_Length { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
       
        #endregion

        #region Functions
        public static List<WorkOrderDetail> Gets(int nORSID, long nUserID)
        {
            return WorkOrderDetail.Service.Gets(nORSID, nUserID);
        }

        public static List<WorkOrderDetail> GetsByLog(int nORSLogID, long nUserID)
        {
            return WorkOrderDetail.Service.GetsByLog(nORSLogID, nUserID);
        }
        public static List<WorkOrderDetail> Gets(string sSQL, long nUserID)
        {
            return WorkOrderDetail.Service.Gets(sSQL, nUserID);
        }
        public WorkOrderDetail Get(int id, long nUserID)
        {
            return WorkOrderDetail.Service.Get(id, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IWorkOrderDetailService Service
        {
            get { return (IWorkOrderDetailService)Services.Factory.CreateService(typeof(IWorkOrderDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IWorkOrderDetail interface
    public interface IWorkOrderDetailService
    {
        WorkOrderDetail Get(int id, Int64 nUserID);
        List<WorkOrderDetail> Gets(int nORSID, Int64 nUserID);
        List<WorkOrderDetail> GetsByLog(int nORSID, Int64 nUserID);
        List<WorkOrderDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
