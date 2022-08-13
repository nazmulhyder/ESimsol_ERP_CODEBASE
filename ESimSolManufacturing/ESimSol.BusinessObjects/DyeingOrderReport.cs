using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region PIReport
    
    public class DyeingOrderReport : BusinessObject
    {
        public DyeingOrderReport()
        {
            DyeingOrderID = 0;
            OrderNo = "";
            OrderDate = DateTime.Now;
            ReviseDate = DateTime.MinValue;
            ContractorName = "";
            DeliveryToName = "";
            MKTPName = "";
            ExportLCNo = "";
            Currency = "";
            ProductName = "";
            CPName = "";
            Qty = 0;
            UnitPrice = 0;
            MUName = "";
            NoCode = "";
            DONote = "";
            ErrorMessage = "";
            OrderType = "";
            Qty_DO = 0;
            Qty_DC = 0;
            StyleNo = "";
            RefNo = "";
            OrderTypeSt = "";
            ExportPIID = 0;
            DyeingOrderDetailID = 0;
            ProductID = 0;
            ApproveLotNo = "";
            MUnitID = 0;
            BuyerCombo = 0;
            StatusDOD = EnumDOState.Initialized;
            ExportSCDetailID = 0;
            ReviseNote = "";
            StockInHand = 0;
            Qty_RC = 0;
            LabDipDetailID = 0;
        }

        #region Properties
        public int DyeingOrderID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int ProductID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public double Qty { get; set; }
        public double Qty_DO { get; set; }
        public double Qty_DC { get; set; }
        public double Qty_RC { get; set; }
        public double StockInHand { get; set; }
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }
        public string MBuyerName { get; set; }
        public string MKTPName { get; set; }
        public string Currency { get; set; }
        public string ProductName { get; set; }
        public string CPName { get; set; }
        public string Note { get; set; }
        public double UnitPrice { get; set; }
        public string MUName { get; set; }
        public string DONote { get; set; }
        public int DyeingOrderType { get; set; }
        public int PaymentType { get; set; }
        public int CurrentStatus_LC { get; set; }
        public int AmendmentNo { get; set; }
        public bool AmendmentRequired { get; set; }
        public DateTime AmendmentDate { get; set; }
        public string SampleInvoiceNo { get; set; }
        public string ExportLCNo { get; set; }
        public string PINo { get; set; }
        public string NoCode { get; set; }
        public string OrderType { get; set; }
        public int LabDipType { get; set; }
        public int Shade { get; set; }
        public string ColorName { get; set; }
        public string ColorNo { get; set; }
        public string LDNo { get; set; }
        public string PantonNo { get; set; }
        public string RGB { get; set; }
        public int HankorCone { get; set; }
        public string OrderTypeSt { get; set; }
        public string BuyerRef { get; set; }
        public int Status { get; set; }
        public EnumDOState StatusDOD { get; set; }
        public string LabdipNo { get; set; }
        public string RefNo { get; set; }
        public string StyleNo { get; set; }
        public string ApproveLotNo { get; set; }
        public int ExportPIID { get; set; }
        public int MUnitID { get; set; }
        public int ExportSCDetailID { get; set; }
        public DateTime ReviseDate { get; set; }
        public string ReviseNote { get; set; }
        //public string MBuyerorDeliveryTo 
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(this.MBuyerName))
        //        {
        //            return this.DeliveryToName;
        //        }
        //        else
        //        {
        //            return this.MBuyerName;
        //        }
        //    }
        //}
        public string ShadeSt
        {
            get
            {
                if (this.LabDipType == (int)EnumLabDipType.AVL)
                {
                    return "ANY";
                }
                else if (this.LabDipType == (int)EnumLabDipType.DTM)
                {
                    return "DTM";
                }
                else
                {
                    return ((EnumShade)this.Shade).ToString();
                }
            }
        }
     
        public string ReviseDateSt
        {
            get
            {
                if (this.ReviseDate == DateTime.MinValue) return "";
                else return ReviseDate.ToString("dd MMM yyyy");
            }
        }
        public int BuyerCombo { get; set; }
        public int LabDipDetailID { get; set; }
        #endregion

        #region Derive Property

        public string ErrorMessage { get; set; }

        public double Amount
        {
            get { return this.UnitPrice*this.Qty; }
        }
        public string AmountST
        {
            get { return this.Currency + Global.MillionFormat(this.UnitPrice * this.Qty); }
        }
        public string QtySt
        {
            get { return Global.MillionFormat(this.Qty); }
        }
        public string UPriceSt
        {
            get { return this.Currency + Global.MillionFormat(this.UnitPrice); }
        }

        public string OrderDateSt
        {
            get
            {
                return OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string StatusSt
        {
            get
            {
                return ((EnumDyeingOrderState)Status).ToString();
            }
        }
        public string StatusDODSt
        {
            get
            {
                return ((EnumDOState)StatusDOD).ToString();
            }
        }
        private string _sOrderNoFull = "";
        public string OrderNoFull
        {
            get
            {
                _sOrderNoFull = this.NoCode + this.OrderNo;

                return _sOrderNoFull;
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderPaymentType)this.PaymentType);
            }
        }
     
        public string LCStatusSt
        {
            get
            {
                if (this.AmendmentRequired)
                {
                    return "AmendmentRequired";
                }
                else
                {
                    return ((EnumExportLCStatus)this.CurrentStatus_LC).ToString();
                }
            }
        }

        #endregion


    #endregion


        #region Functions

        public static List<DyeingOrderReport> Gets(int nSampleInvoiceID, long nUserID)
        {
            return DyeingOrderReport.Service.Gets(nSampleInvoiceID, nUserID);
        }
        public static List<DyeingOrderReport> Gets(string sSQL, Int64 nUserID)
        {
            return DyeingOrderReport.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IDyeingOrderReportService Service
        {
            get { return (IDyeingOrderReportService)Services.Factory.CreateService(typeof(IDyeingOrderReportService)); }
        }
        #endregion

    }

    #region IPIReport interface
    [ServiceContract]
    public interface IDyeingOrderReportService
    {
        List<DyeingOrderReport> Gets(int nSampleInvoiceID, long nUserID);
        List<DyeingOrderReport> Gets(string sSQL, Int64 nUserID);
        
    }
    #endregion
}
