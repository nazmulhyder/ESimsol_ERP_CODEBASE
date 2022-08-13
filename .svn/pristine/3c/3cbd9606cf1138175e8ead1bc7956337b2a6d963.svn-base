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

    #region VPTransactionSummary
    public class VPTransactionSummary : BusinessObject
    {
        public VPTransactionSummary()
        {
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            OpeiningValue = 0;
            IsDebit = true;
            DebitAmount = 0;
            CreditAmount = 0;
            ClosingValue = 0;
            //IsDrClosing = true;
            CurrencySymbol = "";
            VPTransactionSummarys = new List<VPTransactionSummary>();
            ErrorMessage = "";
            VoucherID = 0;
            VoucherDate = DateTime.Now;
            VoucherNo = "";
            IsApproved = true;
            AccountHeadName = "";
            ParentHeadID = 0;
            ParentHeadName = "";
            ParentHeadCode = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsNullOrNot = false;
            VoucherDetailID = 0;
            ConfigTitle = "";
            Narration = "";
            VoucherNarration = "";
            Description = "";
            Level = 0;
            CCOptionID = 0;
            BusinessUnitID = 0;
            VoucherBillID = 0;
            ProductCategoryID = 0;
            ProductBaseID = 0;
            BusinessUnitIDs = "0";
            BUName = "Group Accounts";
            ConfigType = EnumConfigureType.None;
            ACConfigs = new List<ACConfig>();
            UnitPrice = 0;
        }

        #region Properties
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double OpeiningValue { get; set; }
        public bool IsDebit { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double ClosingValue { get; set; }
        public double OpeiningQty { get; set; }
        public double DebitQty { get; set; }
        public double CreditQty { get; set; }
        public double ClosingQty { get; set; }
        public string  CurrencySymbol  {get;set;}
        public string ErrorMessage { get; set; }
        public int VoucherID { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccountHeadID { get; set; }
        public bool IsApproved { get; set; }
        public int CurrencyID { get; set; }
        public string AccountHeadName { get; set; }
        public int ParentHeadID { get; set; }
        public string ParentHeadName { get; set; }
        public string ParentHeadCode { get; set; }
        public bool IsNullOrNot { get; set; }
        public int VoucherDetailID { get; set; }
        public string ConfigTitle { get; set; }
        public string Narration { get; set; }
        public string VoucherNarration { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int CCOptionID { get; set; }
        public int VoucherBillID { get; set; }
        public int BusinessUnitID { get; set; }
        public EnumConfigureType ConfigType { get; set; }
        public bool IsForCurrentDate { get; set; }
        public int ProductCategoryID { get; set; }
        public int ProductBaseID { get; set; }
        public string BusinessUnitIDs { get; set; }
        public string BUName { get; set; }
        public double UnitPrice { get; set; }
        #endregion

        #region Derived Property
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string ProductNameCode { get { return this.ProductName + " [" + this.ProductCode + "]"; } }
        public string ParentHeadNameCode { get { return this.ParentHeadName + " [" + this.ParentHeadCode + "]"; } }
        public List<ACConfig> ACConfigs { get; set; }
        public List<Contractor> Contractors { get; set; }
        public List<Currency> Currencies { get; set; }
        public  List<VPTransactionSummary> VPTransactionSummarys { get; set; }
        public Company Company { get; set; }
        public Currency Currency { get; set; }

        public double ClosingPrice
        {
            get
            {
                if (this.ClosingValue > 0 && this.ClosingQty > 0)
                {

                    return this.ClosingValue / this.ClosingQty;
                }
                else if (this.ClosingValue==0 || this.ClosingQty== 0)
                {
                    return 0;
                }
                else
                {
                    return (this.ClosingValue / this.ClosingQty) < -1 ? (this.ClosingValue / this.ClosingQty) * (-1) : (this.ClosingValue / this.ClosingQty);
                }

            }
        }
        public string ClosingPriceSt
        {
            get
            {
                if (this.ClosingValue>0  && this.ClosingQty>0)
                {
                    
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.ClosingValue / this.ClosingQty);
                }
                else if (this.ClosingValue==0 || this.ClosingQty== 0)
                {
                    return "0";
                }
                else
                {
                    return this.CurrencySymbol + " " +Global.MillionFormat( (this.ClosingValue / this.ClosingQty) < -1 ?(this.ClosingValue / this.ClosingQty) * (-1) : (this.ClosingValue / this.ClosingQty));
                }
                
            }
        }
        public string OpeiningPriceSt
        {
            get
            {
                if (this.OpeiningValue > 0 && this.OpeiningQty > 0)
                {

                    return this.CurrencySymbol + " " + Global.MillionFormat(this.OpeiningValue / this.OpeiningQty);
                }
                else if (this.OpeiningValue == 0 || this.OpeiningQty == 0)
                {
                    return "0";
                }
                else
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat((this.OpeiningValue / this.OpeiningQty) < -1 ? (this.OpeiningValue / this.OpeiningQty) * (-1) : (this.OpeiningValue / this.OpeiningQty));
                }

            }
        }

        public string DebitPriceSt
        {
            get
            {
                if (this.DebitAmount > 0 && this.DebitQty > 0)
                {

                    return this.CurrencySymbol + " " + Global.MillionFormat(this.DebitAmount / this.DebitQty);
                }
                else if (this.DebitAmount == 0 || this.DebitQty == 0)
                {
                    return "0";
                }
                else
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat((this.DebitAmount / this.DebitQty) < -1 ? (this.DebitAmount / this.DebitQty) * (-1) : (this.DebitAmount / this.DebitQty));
                }

            }
        }

        public string CreditPriceSt
        {
            get
            {
                if (this.CreditAmount > 0 && this.CreditQty > 0)
                {

                    return this.CurrencySymbol + " " + Global.MillionFormat(this.CreditAmount / this.CreditQty);
                }
                else if (this.CreditAmount == 0 || this.CreditQty == 0)
                {
                    return "0";
                }
                else
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat((this.CreditAmount / this.CreditQty) < -1 ? (this.CreditAmount / this.CreditQty) * (-1) : (this.CreditAmount / this.CreditQty));
                }

            }
        }
        public string UnitPriceSt
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.UnitPrice);

            }
        }
        public string DebitAmountInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.DebitAmount);
            }
        }
        public string CreditAmountInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.CreditAmount);
            }
        }
        public string OpeningValueInString
        {
            get
            {
               if(this.IsDebit == true)
               {
                   return "Dr " + this.CurrencySymbol + " " + Global.MillionFormat(this.OpeiningValue);
               }else{
                   return "Cr " + this.CurrencySymbol + " " + Global.MillionFormat(this.OpeiningValue);
               }
                
            }
        }
        public string IsDrOpenInString
        {
            get
            {
                if(this.IsDebit == true)
                {
                    return "Dr";
                }
                else
                {
                    return "Cr";
                }
            }
        }
        public string VoucherDateInString { get { return this.VoucherID <= 0 && this.AccountHeadName != "Opening Balance" ? this.ConfigTitle : VoucherDate.ToString("dd MMM yyyy"); } }
        public string ClosingValueString
        {
            get
            {
                if (this.ClosingValue < 0)
                {
                    return "(" + Global.MillionFormat(this.ClosingValue * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.ClosingValue);
                }

            }
        }
        //public string ClosingValueInString
        //{
        //    get
        //    {
        //        if (this.IsDrClosing == true)
        //        {
        //            return "Dr " + this.CurrencySymbol + " " + Global.MillionFormat(this.ClosingValue);
        //        }
        //        else
        //        {
        //            return "Cr " + this.CurrencySymbol + " " + Global.MillionFormat(this.ClosingValue);
        //        }

        //    }
        //}
        //public string IsDrClosingInString
        //{
        //    get
        //    {
        //        if(this.IsDrClosing == true)
        //        {
        //            return "Dr";
        //        }
        //        else
        //        {
        //            return "Cr";
        //        }
        //    }
        //}

        public string OpeiningQtyString
        {
            get
            {
                if (this.OpeiningQty < 0)
                {
                    return "(" + Global.MillionFormat(this.OpeiningQty * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.OpeiningQty);
                }

            }
        }

        public string DebitQtyString
        {
            get
            {
                if (this.DebitQty < 0)
                {
                    return "(" + Global.MillionFormat(this.DebitQty * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.DebitQty);
                }

            }
        }

        public string CreditQtyString
        {
            get
            {
                if (this.CreditQty < 0)
                {
                    return "(" + Global.MillionFormat(this.CreditQty * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.CreditQty);
                }

            }
        }

        public string ClosingQtyString
        {
            get
            {
                if (this.ClosingQty < 0)
                {
                    return "(" + Global.MillionFormat(this.ClosingQty * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.ClosingQty);
                }

            }
        }
        public string VoucherIDWithNo
        {
            get
            {
                return this.VoucherNo + "~" + this.VoucherID;
            }
        }
        public string ProductIDWithName
        {
            get
            {
                return this.ProductName + "~" + this.ProductID;
            }
        }
        #endregion

        #region Functions
        public static List<VPTransactionSummary> Gets(string BUIDs, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nUserID)
        {
            return VPTransactionSummary.Service.Gets(BUIDs, nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved, nUserID);
        }

        public static List<VPTransactionSummary> GetsForProduct(string BUIDs, int nAccountHeadID, int nProductID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nUserID)
        {
            return VPTransactionSummary.Service.GetsForProduct(BUIDs, nAccountHeadID, nProductID, nCurrencyID, StartDate, EndDate, IsApproved, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVPTransactionSummaryService Service
        {
            get { return (IVPTransactionSummaryService)Services.Factory.CreateService(typeof(IVPTransactionSummaryService)); }
        }
        #endregion
    }
    #endregion

    #region IVPTransactionSummary interface
    public interface IVPTransactionSummaryService
    {
        List<VPTransactionSummary> Gets(string BUIDs, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nUserID);
        List<VPTransactionSummary> GetsForProduct(string BUIDs, int nAccountHeadID, int nProductID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nUserID);
    }
    #endregion
    
   
}
