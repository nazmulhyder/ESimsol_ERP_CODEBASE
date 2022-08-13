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
    
    public class SampleOutStanding : BusinessObject
    {
        public SampleOutStanding()
        {
            DyeingOrderID = 0;
            OrderNo = "";
            OrderDate = DateTime.Now;
            ContractorID = 0;
            ContractorName = "";
            //BuyerID = 0;
            SampleInvoiceNo = "";
            SampleInvoiceDate = DateTime.Now;
            MKTPName = "";
            LCNo = "";
            Currency = "";
            ProductName = "";
            CPName = "";
            Qty = 0;
            UnitPrice = 0;
            MUName = "";
            Note = "";
            DeliveryToName = "";
            Amount = 0;
            OrderType = 0;
            PaymentType = 0;
            InvoiceType = 0;
            Opening = 0;
            Debit = 0;
            Credit = 0;
            Closing = 0;
            OrderDateEnd = DateTime.Now;
            MarketingAccountID = 0;
            MAName = "";
            ErrorMessage = "";
            RefNo = "";
            RefDate = DateTime.Now;


        }

        #region Properties
        public int DyeingOrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string SampleInvoiceNo { get; set; }
        public DateTime SampleInvoiceDate { get; set; }
        public double Qty { get; set; }
        public string MKTPName { get; set; }
        public string Currency { get; set; }
        public string ProductName { get; set; }
        public string CPName { get; set; }
        public string Note { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string MUName { get; set; }
        public int OrderType { get; set; }
        public int PaymentType { get; set; }
        public int InvoiceType { get; set; }
        //public int AmendmentNo { get; set; }
        public string LCNo { get; set; }
        public string PINo { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }
        public double Opening { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double Closing { get; set; }
        public DateTime OrderDateEnd { get; set; }
        public string MAName { get; set; }
        public int MarketingAccountID { get; set; }
        public string RefNo { get; set; }
        public DateTime RefDate { get; set; }
        #endregion

        #region Derive Property

        [DataMember]
        public string ErrorMessage { get; set; }

     
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
        public string OrderDateEndSt
        {
            get
            {
                return this.OrderDateEnd.ToString("dd MMM yyyy");
            }
        }
     
        private string _sOrderNoFull = "";
        public string OrderNoFull
        {
            get
            {
                if (this.OrderType == (int)EnumOrderType.SampleOrder)
                {
                    _sOrderNoFull = "BSY-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.BulkOrder)
                {
                    _sOrderNoFull = "BPO-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    _sOrderNoFull = "BRD-" + this.OrderNo;
                }
                else
                {
                    _sOrderNoFull = "BSY-" + this.OrderNo;
                }

                return _sOrderNoFull;
            }
        }
        private string _sInvoiceType = "";
        public string InvoiceTypeSt
        {
            get
            {
                if (this.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Qty)
                {
                    _sInvoiceType = "LC-Adjust-Qty";
                }
                else if (this.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value)
                {
                    _sInvoiceType = "LC-Adjust-Value";
                }
                else if (this.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission)
                {
                    _sInvoiceType = "Commission-Adjust";
                }
                else if (this.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
                {
                    _sInvoiceType = "Return-Adjust";
                }
                else if (this.InvoiceType == (int)EnumSampleInvoiceType.SalesContract)
                {
                    _sInvoiceType = "Sales Contract";
                }
                else if (this.InvoiceType == (int)EnumSampleInvoiceType.SampleInvoice)
                {
                    _sInvoiceType = "Sample Invoice";
                }
                else
                {
                    _sInvoiceType = "LC-Adjust-Qty";
                }

                return _sInvoiceType;
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderPaymentType)this.PaymentType);
            }
        }
     
     

        #endregion


    #endregion


        #region Functions


        public static List<SampleOutStanding> Gets(string sSQL, bool bIsDr, Int64 nUserID)
        {
            return SampleOutStanding.Service.Gets(sSQL, bIsDr, nUserID);
        }
        public static List<SampleOutStanding> Gets(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID)
        {
            return SampleOutStanding.Service.Gets( dStartDate,  dEndDate,  sContractorIDs, nUserID);
        }
        public static List<SampleOutStanding> GetsWithMkt(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID)
        {
            return SampleOutStanding.Service.GetsWithMkt(dStartDate, dEndDate, sContractorIDs, nUserID);
        }
        public static List<SampleOutStanding> GetsMktDetail(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID)
        {
            return SampleOutStanding.Service.GetsMktDetail(dStartDate, dEndDate, sContractorIDs, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ISampleOutStandingService Service
        {
            get { return (ISampleOutStandingService)Services.Factory.CreateService(typeof(ISampleOutStandingService)); }
        }
        #endregion
    }

    #region IPIReport interface
    [ServiceContract]
    public interface ISampleOutStandingService
    {
        List<SampleOutStanding> Gets(string sSQL, bool bIsDr, Int64 nUserID);
        List<SampleOutStanding> Gets(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID);
        List<SampleOutStanding> GetsWithMkt(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID);
        List<SampleOutStanding> GetsMktDetail(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID);

    }
    #endregion
}
