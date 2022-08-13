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
    #region PurchaseInvoice
    [DataContract]
    public class PurchaseInvoice : BusinessObject
    {
        #region  Constructor
        public PurchaseInvoice()
        {
            PurchaseInvoiceID = 0;
            PurchaseInvoiceNo = "";
            BUID = 0;
            BillNo = "";
            DateofBill = DateTime.Today;
            DateofInvoice = DateTime.Today;
            InvoiceType = EnumPInvoiceType.None;
            InvoiceTypeInt = 1;
            PaymentMethod = EnumPaymentMethod.None;
            PaymentMethodInt = 0;
            InvoicePaymentMode = EnumInvoicePaymentMode.None;
            InvoicePaymentModeInt = 0;
            InvoiceStatus = EnumPInvoiceStatus.Initialize;
            InvoiceStatusInt = 0;
            RefType = EnumInvoiceReferenceType.None;
            RefTypeInt = 0;
            //RefID = 0;
            ContractorID = 0;
            ContractorPersonalID = 0;
            Amount = 0;
            CurrencyID = 0;
            ConvertionRate = 0;
            DateofMaturity = DateTime.Today;
            Note = "";
            ApprovedBy = 0;
            ApprovedDate = DateTime.Today;
            PaymentTermID = 0;
            ContractorName = "";
            CurrencySymbol = "";
            CurrencyName = "";
            CurrencyBFDP = "";
            CurrencyBADP = "";
            RefNo = "";
            RefDate = DateTime.Today;
            RefAmount = 0;
            BUName = "";
            BUCode = "";
            ContrctorPersonalName = "";
            PrepareUserName = "";
            ApprovedUserName = "";
            PaymentTermText = "";
            AdvanceSettle = 0;
            PaymentTermID = 0;
            ShipBy = "";
            TradeTerm = "";
            DeliveryTo = 0;
            DeliveryToName = "";
            DeliveryToContactPerson = 0;
            DeliveryToContactPersonName = "";
            BillTo = 0;
            BillToName = "";
            BIllToContactPerson = 0;
            BIllToContactPersonName = "";
            PaymentTermText = "";
            ErrorMessage = "";
            PurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();
            PIT = 0;
            YetToGRNQty = 0;
            YeToBillAmount = 0;
            Discount = 0.0;
            ServiceCharge = 0.0;
            NetAmount = 0.0;
            RateOn = 1;
            LastApprovalSequence = 0;
            ApprovalSequence = 0;
            IsWillVoucherEffect = true;
            ApprovalStatus = "";
            ServiceChargeID = 0;
            ServiceChargeName = "";
        }
        #endregion

        #region Properties
        public int PurchaseInvoiceID { get; set; }
        public int ServiceChargeID { get; set; }
        public int RateOn { get; set; }
        public double Discount { get; set; }
        public double ServiceCharge { get; set; }
        public double NetAmount { get; set; }
        public string PurchaseInvoiceNo { get; set; }
        public string ServiceChargeName { get; set; }
        public int BUID { get; set; }
        public string BillNo { get; set; }
        public DateTime DateofBill { get; set; }
        public DateTime DateofInvoice { get; set; }
        public EnumPInvoiceType InvoiceType { get; set; }
        public int InvoiceTypeInt { get; set; }

        public EnumPaymentMethod PaymentMethod { get; set; }
        public int PaymentMethodInt { get; set; }
        public int BankAccountID { get; set; }
        

        public EnumInvoicePaymentMode InvoicePaymentMode { get; set; }
        public int InvoicePaymentModeInt { get; set; }
        
        public EnumPInvoiceStatus InvoiceStatus { get; set; }
        public int InvoiceStatusInt { get; set; }
        public EnumInvoiceReferenceType RefType { get; set; }
        public int RefTypeInt { get; set; }
        public int RefID { get; set; }
        public int ContractorID { get; set; }
        public int ContractorPersonalID { get; set; }
        public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public double ConvertionRate { get; set; }
        public DateTime DateofMaturity { get; set; }
        public string Note { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int PaymentTermID { get; set; }
        public string ContractorName { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyBFDP { get; set; } // Currency In Word Before Decimal Point
        public string CurrencyBADP { get; set; } // Currency In Word After Decimal Point
        public string  RefNo { get; set; }
        public DateTime  RefDate { get; set; }
        public double  RefAmount { get; set; }
        public string BUName { get; set; }
        public string BUCode { get; set; }
        public string ContrctorPersonalName { get; set; }
        public string PrepareUserName { get; set; }
        public string ApprovedUserName { get; set; }
        public string PaymentTermText { get; set; }
        public double AdvanceSettle { get; set; }
        public double YetToGRNQty { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        public string ShipBy { get; set; }
        public string TradeTerm { get; set; }
        public int DeliveryTo { get; set; }
        public string DeliveryToName { get; set; }
        public int DeliveryToContactPerson { get; set; }
        public string DeliveryToContactPersonName { get; set; }
        public int BillTo { get; set; }
        public string BillToName { get; set; }
        public int BIllToContactPerson { get; set; }
        public string BIllToContactPersonName { get; set; }
        public double YeToBillAmount { get; set; }
        public int LastApprovalSequence { get; set; }
        public int ApprovalSequence { get; set; }
        public string ApprovalStatus { get; set; }
        public string ErrorMessage { get; set; }

        #region Derive property
        public int PIT { get; set; }
        public string AccountNo { get; set; }
        public string BankShortName { get; set; }
        public List<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
        public string IsWillVoucherEffectSt
        {
            get
            {
                if(this.IsWillVoucherEffect)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string  RefAmountSt
        {
            get
            {
                return Global.MillionFormat(this. RefAmount);
            }
        }
        public string RefTypeSt
        {
            get
            {
                return this.RefType.ToString();
            }
        }
        public string InvoiceStatusSt
        {
            get
            {
                return this.InvoiceStatus.ToString();
            }
        }
        public string DateofMaturitySt
        {
            get
            {
                return DateofMaturity.ToString("dd MMM yyyy");
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string ConvertionRateSt
        {
            get
            {
                return Global.MillionFormat(this.ConvertionRate);
            }
        }
        public string InvoiceTypeInSt
        {
            get
            {
                return EnumObject.jGet(InvoiceType);
            }
        }

        public string PaymentMethodInSt
        {
            get
            {
                return EnumObject.jGet(PaymentMethod);
            }
        }

        public string DateofInvoiceSt
        {
            get
            {
                if (this.DateofInvoice == DateTime.MinValue)
                {
                    return DateTime.Now.ToString("dd MMM yyyy");
                }
                else
                {
                    return this.DateofInvoice.ToString("dd MMM yyyy");
                }
            }
        }
        public string  RefDateST
        {
            get
            {
                if (this.RefDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this. RefDate.ToString("dd MMM yyyy");
                }

            }
        }

        public string DateofBillSt
        {
            get
            {
                if (this.DateofBill == DateTime.MinValue)
                {
                    return DateTime.Now.ToString("dd MMM yyyy");
                }
                else
                {
                    return this.DateofBill.ToString("dd MMM yyyy");
                }
            }
        }
        public string ApprovedDateST
        {
            get
            {
                if (this.ApprovedDate == DateTime.MinValue)
                {
                    return DateTime.Now.ToString("dd MMM yyyy");
                }
                else
                {
                    return this.ApprovedDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #endregion

        #region Function
        public PurchaseInvoice Save(long nUserID)
        {
            return PurchaseInvoice.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return PurchaseInvoice.Service.Delete(this, nUserID);
        }       
        public PurchaseInvoice Get(int nPurchaseInvoiceID, long nUserID)
        {
            return PurchaseInvoice.Service.Get(nPurchaseInvoiceID, nUserID);
        }
        public PurchaseInvoice Get(int nRefID, int RefType, long nUserID)
        {
            return PurchaseInvoice.Service.Get(nRefID, RefType, nUserID);
        }
        public static List<PurchaseInvoice> Gets(int nPurchaseLCID, long nUserID)
        {
            return PurchaseInvoice.Service.Gets(nPurchaseLCID, nUserID);
        }          
        public static List<PurchaseInvoice> Gets(string sSQL, long nUserID)
        {
            return PurchaseInvoice.Service.Gets(sSQL, nUserID);
        }
        public static List<PurchaseInvoice> Gets(long nUserID)
        {
            return PurchaseInvoice.Service.Gets(nUserID);
        }
        public PurchaseInvoice Approved(long nUserID)
        {
            return PurchaseInvoice.Service.Approved(this, nUserID);
        }
        public PurchaseInvoice UndoApproved(long nUserID)
        {
            return PurchaseInvoice.Service.UndoApproved(this, nUserID);
        }
        
        public PurchaseInvoice UpdatePaymentMode(long nUserID)
        {
            return PurchaseInvoice.Service.UpdatePaymentMode(this, nUserID);
        }
        public PurchaseInvoice UpdateVoucherEffect(long nUserID)
        {
            return PurchaseInvoice.Service.UpdateVoucherEffect(this, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IPurchaseInvoiceService Service
        {
            get { return (IPurchaseInvoiceService)Services.Factory.CreateService(typeof(IPurchaseInvoiceService)); }
        }
        #endregion
    }
    #endregion

    #region IPurchaseInvoice interface
    public interface IPurchaseInvoiceService
    {
        List<PurchaseInvoice> Gets(Int64 nUserID);
        string Delete(PurchaseInvoice oPurchaseInvoice, Int64 nUserID);       
        PurchaseInvoice Save(PurchaseInvoice oPurchaseInvoice, Int64 nUserID);
        PurchaseInvoice Get(int nPurchaseInvoiceID, Int64 nUserID);
        PurchaseInvoice Get(int nRefID, int RefType, Int64 nUserID);
        List<PurchaseInvoice> Gets(int nPurchaseLCID, Int64 nUserID);          
        List<PurchaseInvoice> Gets(string sSQL, Int64 nUserID);
        PurchaseInvoice Approved(PurchaseInvoice oPurchaseInvoice, Int64 nUserID);
        PurchaseInvoice UndoApproved(PurchaseInvoice oPurchaseInvoice, Int64 nUserID);
        PurchaseInvoice UpdatePaymentMode(PurchaseInvoice oPurchaseInvoice, Int64 nUserID);
        PurchaseInvoice UpdateVoucherEffect(PurchaseInvoice oPurchaseInvoice, Int64 nUserID);   
    }
    #endregion
}
