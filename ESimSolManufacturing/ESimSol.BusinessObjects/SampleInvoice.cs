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
    #region SampleInvoice
 
    public class SampleInvoice : BusinessObject
    {

        #region  Constructor
        public SampleInvoice()
        {
            SampleInvoiceID = 0;
            SampleInvoiceNo = "";
            Amount = 0;
            CurrencyID = 0;
            SampleInvoiceDate = DateTime.Now;
            ContractorID = 0;
            ContractorPersopnnalID = 0;
            PaymentType = 0;
            InvoiceType = 0;
            CurrentStatus = EnumSampleInvoiceStatus.WaitingForApprove;
            ApproveBy = 0;
            ApprovalRemark = "";
            ApprovedDate = DateTime.Now;
            RequirementDate = DateTime.Now;
            IsWillVoucherEffect = true;
            PaymentDate = DateTime.Now;
            Remark = "";
            CurrencySymbol = "";
            ConversionRate = 1;
            ContractorName = "";
            BUID = 0;
            ExchangeCurrencyID = 0;
            ExchangeCurrencySymbol = "";
            ExchangeCurrencyName = "";
            CurrencyName = "";
            IsPaymentDone = false;
            IsAdvance = false;
            AlreadyPaid = 0;
            AlreadyDiscount = 0;
            AlreadyAdditionalAmount = 0;
            RateUnit = 1;
            SampleInvoiceDetails = new List<SampleInvoiceDetail>();
            DyeingOrderDetails = new List<DyeingOrderDetail>();
            SampleInvoiceCharges = new List<SampleInvoiceCharge>();
            IsRevise = false;
            ReviseNo = 0;
            Charge = 0;
            Discount = 0;
            YetToAdjust = 0;
            WaitForAdjust = 0;
            LCNo = "";
            //DyeingOrderReports = new List<DyeingOrderReport>();

        }
        #endregion

        #region Properties
        public int SampleInvoiceID { get; set; }
        public string SampleInvoiceNo { get; set; }
        public string InvoiceNo { get; set; }//// No With Revise
        public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public DateTime SampleInvoiceDate { get; set; }
        public DateTime RequirementDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public int ContractorID { get; set; }
        public int ContractorType { get; set; }
        public int ContractorPersopnnalID { get; set; }
        public int InvoiceType { get; set; }
        public int ExportPIID { get; set; }
        public int OrderType { get; set; }
        public EnumSampleInvoiceStatus CurrentStatus { get; set; }
        public Int64 ApproveBy { get; set; }
        public double ConversionRate { get; set; }
        public string ApprovalRemark { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string Remark { get; set; }
        public string ContractorName { get; set; }
        public string ContractorPersopnnalName { get; set; }
        public string ContractNo { get; set; }// Contract personnel Phone No
        public string Email { get; set; }// Contract personnel Phone No
        public string ApproveByName { get; set; }
        public string ContractorAddress { get; set; }
        public int MKTEmpID { get; set; }
        public int RateUnit { get; set; }
        public int BUID { get; set; }
        public string MKTPName { get; set; }
        public bool IsPaymentDone { get; set; }
        public string ExportPINo { get; set; }
        public string OrderNo { get; set; }
        public string MRNo { get; set; }
        public string LCNo { get; set; }
        public double Charge { get; set; }
        public double Discount { get; set; }
        public double AlreadyPaid { get; set; }
        public double AlreadyDiscount { get; set; }
        public double AlreadyAdditionalAmount { get; set; }
        public string PreparebyName { get; set; }
        public int ExchangeCurrencyID { get; set; }
        public string ExchangeCurrencySymbol { get; set; }
        public string ExchangeCurrencyName { get; set; }
        public string CurrencyName { get; set; }
        public bool IsAdvance { get; set; }
        public int ReviseNo { get; set; }
        public bool IsRevise { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        public double YetToAdjust { get; set; }
        public double WaitForAdjust { get; set; }
        public EnumSettlementStatus ProductionSettlementStatus { get; set; }
        public EnumSettlementStatus PaymentSettlementStatus { get; set; }
      
        #region AmountWithCRate
        private double _nAmountWithCRate;
        public double AmountWithCRate
        {
            get
            {
                _nAmountWithCRate = this.Amount * this.ConversionRate;
                return _nAmountWithCRate;
            }
            set { _nAmountWithCRate = value; }
        }
        public int PaymentType { get; set; }
        public int InvoiceTypeInt { get; set; }
        public int CurrentStatusInt { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        public double Amount_Paid { get; set; }
        public string CurrencySymbol { get; set; }
        
        
        
        #region Amount_PaidtWithCRate
        private double _nAmount_PaidtWithCRate;
        public double Amount_PaidtWithCRate
        {
            get
            {
                _nAmount_PaidtWithCRate = this.Amount_Paid * this.ConversionRate;
                return _nAmount_PaidtWithCRate;
            }
        }
        #endregion
        #region
        private double _nAmount_YetToPaidtWithCRate;
        public double Amount_YetToPaidtWithCRate
        {
            get
            {
                _nAmount_YetToPaidtWithCRate = (this.AmountWithCRate - this.Amount_PaidtWithCRate);
                return _nAmount_YetToPaidtWithCRate;
            }

        }
        #endregion
      
        public List<ContactPersonnel> ContactPersonnels { get; set; }
        
        #region AmountST
        private string _sAmountST = "";
        public string AmountST
        {
            get
            {
                _sAmountST = this.CurrencySymbol +Global.MillionFormat(this.Amount);

                return _sAmountST;
            }
        }
        #endregion
        #region ConversionRateST
        private string _sConversionRateST = "";
        public string ConversionRateST
        {
            get
            {
                _sConversionRateST = Global.MillionFormat(this.ConversionRate);

                return _sConversionRateST;
            }
        }
        #endregion
        #region AmountWithCRateST
        private string _sAmountWithCRateST = "";
        public string AmountWithCRateST
        {
            get
            {
                _sAmountWithCRateST =this.ExchangeCurrencySymbol+""+ Global.MillionFormat(this.AmountWithCRate);

                return _sAmountWithCRateST;
            }
        }
        #endregion
        #region Amount_PaidtWithCRateST
        private string _sAmount_PaidtWithCRateST = "";
        public string Amount_PaidtWithCRateST
        {
            get
            {
                _sAmount_PaidtWithCRateST = Global.MillionFormat(this.Amount_PaidtWithCRate);

                return _sAmount_PaidtWithCRateST;
            }
        }
        #endregion
        #region RequirementDate
        private string _sRequirementDateST = DateTime.Now.ToString("dd MMM yyyy");
        public string RequirementDateST
        {
            get
            {
                _sRequirementDateST = this.RequirementDate.ToString("dd MMM yyyy");

                return _sRequirementDateST;
            }
        }
        #endregion
        #region SampleInvoiceDateST
        private string _sSampleInvoiceDateST = DateTime.Now.ToString("dd MMM yyyy");
        public string SampleInvoiceDateST
        {
            get
            {
                _sSampleInvoiceDateST = this.SampleInvoiceDate.ToString("dd MMM yyyy");

                return _sSampleInvoiceDateST;
            }
        }
        #endregion
        #region SampleInvoiceNonDate
        private string _sSampleInvoiceNonDate = DateTime.Now.ToString("dd MMM yyyy");
        public string SampleInvoiceNonDate
        {
            get
            {
                _sSampleInvoiceNonDate = this.SampleInvoiceNo+" ["+ this.SampleInvoiceDate.ToString("dd MMM yyyy")+"]";

                return _sSampleInvoiceNonDate;
            }
        }
        #endregion
        #region IsAdvanceSt
        private string _sIsAdvance = "";
        public string IsAdvanceSt
        {
            get
            {
                if (this.IsAdvance) { _sIsAdvance = "Advance"; }
                else _sIsAdvance = "";
                return _sIsAdvance;
            }
        }
        #endregion
        #region PaymentDateST
        private string _sPaymentDateST = DateTime.Now.ToString("dd MMM yyyy");
        public string PaymentDateST
        {
            get
            {
                _sPaymentDateST = this.PaymentDate.ToString("dd MMM yyyy");

                return _sPaymentDateST;
            }
        }
        #endregion
        
        #region Derived Property
        public List<SampleInvoiceDetail> SampleInvoiceDetails { get; set; }
        public List<DyeingOrderDetail> DyeingOrderDetails { get; set; }
        public List<SampleInvoiceCharge> SampleInvoiceCharges { get; set; }
      
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderPaymentType)this.PaymentType);
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
        public string InvoiceTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumSampleInvoiceType)this.InvoiceType);
            }
        }
        public string PaymentSettlementStatusST
        {
            get
            {
                return PaymentSettlementStatus.ToString();
            }
        }
        public string ProductionSettlementStatusST
        {
            get
            {
                return ProductionSettlementStatus.ToString();
            }
        }
        public string CurrentStatusSt
        {
            get
            {
                return CurrentStatus.ToString();
            }
        }
       
        public string Amount_TakaInWord
        {
            get
            {
                return Global.TakaWords(this.AmountWithCRate);
            }
        }
        public string ImageUrl { get; set; }
        #endregion
        #endregion

        #region Functions
      
        public  SampleInvoice Get(int nId, long nUserID)
        {
            return SampleInvoice.Service.Get(nId, nUserID);
        }

        public static List<SampleInvoice> Gets(long nUserID)
        {
            return SampleInvoice.Service.Gets(nUserID);
        }

        public static List<SampleInvoice> Gets(string sSQL, long nUserID)
        {
            return SampleInvoice.Service.Gets(sSQL, nUserID);
        }

        public SampleInvoice Save(long nUserID)
        {
            return SampleInvoice.Service.Save(this, nUserID);
        }
        public SampleInvoice Save_Revise(long nUserID)
        {
            return SampleInvoice.Service.Save_Revise(this, nUserID);
        }
        public SampleInvoice Save_AddDO(long nUserID)
        {
            return SampleInvoice.Service.Save_AddDO(this, nUserID);
        }
        public SampleInvoice Save_Rate(long nUserID)
        {
            return SampleInvoice.Service.Save_Rate(this, nUserID);
        }
        public SampleInvoice UpdateSampleInvoice(long nUserID)
        {
            return SampleInvoice.Service.UpdateSampleInvoice(this, nUserID);
        }
        public SampleInvoice SaveFromOrder(long nUserID)
        {
            return SampleInvoice.Service.SaveFromOrder(this, nUserID);
        }

        public SampleInvoice  Approve( long nUserID)
        {
            return SampleInvoice.Service.Approve(this, nUserID);
        }
        public SampleInvoice Cancel(long nUserID)
        {
            return SampleInvoice.Service.Cancel(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return SampleInvoice.Service.Delete(this, nUserID);
        }
        public SampleInvoice RemoveExportPIFromBill(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            return SampleInvoice.Service.RemoveExportPIFromBill(oSampleInvoice, nUserID);
        }
        public SampleInvoice ExportPI_Attach(SampleInvoice oSampleInvoice, int nDBOperation, Int64 nUserID)
        {
            return SampleInvoice.Service.ExportPI_Attach(oSampleInvoice,nDBOperation, nUserID);
        }
        public SampleInvoice UpdateSInvoiceNo(Int64 nUserID)
        {
            return SampleInvoice.Service.UpdateSInvoiceNo(this, nUserID);
        }
        public static List<SampleInvoice> ExportPISNA(int nExportPIID, long nUserID)
        {
            return SampleInvoice.Service.ExportPISNA(nExportPIID, nUserID);
        }
        public SampleInvoice UpdateVoucherEffect(long nUserID)
        {
            return SampleInvoice.Service.UpdateVoucherEffect(this, nUserID);
        }
        
        #endregion

        #region Non DB Functions
        public static int GetIndex(List<SampleInvoice> oSampleInvoices, int nSampleInvoiceID)
        {
            int index = -1, i = 0;

            foreach (SampleInvoice oItem in oSampleInvoices)
            {
                if (oItem.SampleInvoiceID == nSampleInvoiceID)
                {
                    index = i; break;
                }
                i++;
            }
            return index;
        }
        public static string IDInString(List<SampleInvoice> oSampleInvoices)
        {
            string sReturn = "";
            foreach (SampleInvoice oItem in oSampleInvoices)
            {
                sReturn = sReturn + oItem.SampleInvoiceID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;


        }
       
        #endregion

        #region ServiceFactory
        internal static ISampleInvoiceService Service
        {
            get { return (ISampleInvoiceService)Services.Factory.CreateService(typeof(ISampleInvoiceService)); }
        }
        #endregion
    }
    #endregion

    #region ISampleInvoice interface
    [ServiceContract]
    public interface ISampleInvoiceService
    {
        SampleInvoice Get(int id, long nUserID);
        List<SampleInvoice> Gets(long nUserID);
        List<SampleInvoice> Gets(string sSQL, long nUserID);
        SampleInvoice Save(SampleInvoice oSampleInvoice, long nUserID);
        SampleInvoice Save_Revise(SampleInvoice oSampleInvoice, long nUserID);
        SampleInvoice UpdateSampleInvoice(SampleInvoice oSampleInvoice, long nUserID);
        SampleInvoice SaveFromOrder(SampleInvoice oSampleInvoice, long nUserID);
        string Delete(SampleInvoice oSampleInvoice, long nUserID);
        SampleInvoice Approve(SampleInvoice oSampleInvoice, long nUserID);
        SampleInvoice Cancel(SampleInvoice oSampleInvoice, long nUserID);
        SampleInvoice Save_AddDO(SampleInvoice oSampleInvoice, long nUserID);
        SampleInvoice Save_Rate(SampleInvoice oSampleInvoice, long nUserID);
        SampleInvoice RemoveExportPIFromBill(SampleInvoice oSampleInvoice, Int64 nUserID);
        SampleInvoice ExportPI_Attach(SampleInvoice oSampleInvoice, int nDBOperation, Int64 nUserID);
        SampleInvoice UpdateSInvoiceNo(SampleInvoice oSampleInvoice, Int64 nUserID);
        List<SampleInvoice> ExportPISNA(int nExportPIID, long nUserID);
        SampleInvoice UpdateVoucherEffect(SampleInvoice oGRN, Int64 nUserID);   
    }
    #endregion
}
