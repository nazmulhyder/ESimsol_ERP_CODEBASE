using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region VOrderRegister

    public class VOrderRegister : BusinessObject
    {
        public VOrderRegister()
        {
            VOReferenceID = 0;
            VOrderRefType = EnumVOrderRefType.None;
            VOrderRefTypeInt = 0;
            VoucherDetailID = 0;
            AccountHeadID = 0;
            VoucherID = 0;
            OrderID = 0;
            TransactionDate = DateTime.Today;
            Remarks = "";
            IsDebit = false;
            CurrencyID = 0;
            ConversionRate = 0;
            AmountInCurrency = 0;
            Amount = 0;
            CCTID = 0;
            RefNo = "";
            OrderNo = "";
            OrderDate = DateTime.Now;
            SubledgerID = 0;
            SubledgerName = "";
            RefObjIDs = "";
            ErrorMessage = "";
            VoucherNo = "";
            CurrencyName = "";
            Symbol = "";
            AccountHeadCode = "";
            AccountHeadName = "";
            ComponentID = 0;
            BUID = 0;
            IsDateApply = false;
            StartDate = "";
            EndDate = "";
            LCID = 0;
            LCNo = "";
            CreditAmountInCurrency = 0;
            CreditAmount = 0;
        }

        #region Properties
        public int VOReferenceID { get; set; }
        public EnumVOrderRefType VOrderRefType { get; set; }
        public int VOrderRefTypeInt { get; set; }
        public int VoucherDetailID { get; set; }
        public long AccountHeadID { get; set; }
        public long VoucherID { get; set; }
        public int OrderID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Remarks { get; set; }
        public bool IsDebit { get; set; }
        public int CurrencyID { get; set; }
        public double ConversionRate { get; set; }
        public double AmountInCurrency { get; set; }
        public double Amount { get; set; }
        public int CCTID { get; set; }
        public string RefNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int SubledgerID { get; set; }
        public string SubledgerName { get; set; }
        public string RefObjIDs { get; set; }
        public string VoucherNo { get; set; }
        public string CurrencyName { get; set; }
        public string Symbol { get; set; }
        public string AccountHeadCode { get; set; }
        public string AccountHeadName { get; set; }
        public int ComponentID { get; set; }
        public string ErrorMessage { get; set; }
        public int BUID { get; set; }
        public bool IsDateApply { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public int ReportLayoutInt { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int  LCID { get; set; }
        public string LCNo { get; set; }
        #endregion

        #region Derived Property
        public string TransactionDateInString
        {
            get
            {
                return this.TransactionDate.ToString("dd MMM yyyy");
            }
        }
        public double CreditAmountInCurrency { get; set; }
        public double CreditAmount{ get; set; }
        public string AmountInString
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string AmountInCurrencySt
        {
            get
            {
                return Global.MillionFormat(this.AmountInCurrency);
            }
        }

        public string DebitAmountInCurrencySt
        {
            get
            {
                if (this.IsDebit && this.AmountInCurrency > 0)
                {
                    return Global.MillionFormat(this.AmountInCurrency);
                }
                else
                {
                    return "-";
                }
                
            }
        }
        public string CreditAmountInCurrencySt
        {
            get
            {
                if (this.IsDebit == false && this.AmountInCurrency > 0)
                {
                    return Global.MillionFormat(this.AmountInCurrency);
                }
                else
                {
                    return "-";
                }

            }
        }

        public string DebitAmountSt
        {
            get
            {
                if (this.IsDebit && this.Amount > 0)
                {
                    return Global.MillionFormat(this.Amount);
                }
                else
                {
                    return "-";
                }

            }
        }
        public string CreditAmountSt
        {
            get
            {
                if (this.IsDebit == false && this.Amount > 0)
                {
                    return Global.MillionFormat(this.Amount);
                }
                else
                {
                    return "-";
                }

            }
        }

        public string OrderDateSt { get { return this.OrderDate.ToString("dd MMM yyyy"); } }
        #endregion

        #region Functions
        public static List<VOrderRegister> Gets(VOrderRegister oVOrderRegister, string sSQL, int nUserID)
        {
            return VOrderRegister.Service.Gets(oVOrderRegister, sSQL,  nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IVOrderRegisterService Service
        {
            get { return (IVOrderRegisterService)Services.Factory.CreateService(typeof(IVOrderRegisterService)); }
        }

        #endregion
    }
    #endregion

    #region IVOrderRegister interface

    public interface IVOrderRegisterService
    {
        List<VOrderRegister> Gets(VOrderRegister oVOrderRegister, string sSQL, int nUserID);
    }
    #endregion
}
