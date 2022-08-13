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
    #region SalesCommissionPayable
    public class SalesCommissionPayable : BusinessObject
    {
        public SalesCommissionPayable()
        {
            SalesCommissionPayableID = 0;
            ExportPIID=0;
            ExportLCID = 0;
            ContactPersonnelID = 0;
            BUID = 0;
            CurrencyID = 0;
            Percentage = 0;
            MaturityAmount = 0;
            Note = "";
            CommissionAmount = 0;
            AdjOverdueAmount = 0;
            ContractorName = "";
            ExportLCNo = "";
            Status = 0;
            Currency = "";
            ErrorMessage = "";
            AdjAdd = 0;
            AdjOverdueAmount = 0;
            RealizeAmount = 0;
            AdjDeduct = 0;
            ErrorMessage = "";
            Status_Payable = EnumLSalesCommissionStatus.Initialize;
            IsWillVoucherEffect = false;
            CRate = 0;
            CurrencyID_BC = 0;
            Params = "";
            SalesCommissionPayables = new List<SalesCommissionPayable>();
            LCOpeningDate = DateTime.MinValue;
            AmendmentDate = DateTime.MinValue;
            MaturityDate = DateTime.MinValue;
            MaturityReceivedDate = DateTime.MinValue;
            AmendmentDate = DateTime.MinValue;
            PIDate = DateTime.MinValue;
            RelizationDate = DateTime.MinValue;
        }

        #region Properties
        
        public int SalesCommissionPayableID { get; set; }
        public int SalesCommissionID { get; set; }
        public int ExportPIID { get; set; }
        public int ExportLCID { get; set; }
        public int ExportBillID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int BUID { get; set; }
        public int CurrencyID { get; set; }
        public double Percentage { get; set; }
        public double CommissionAmount { get; set; } 
        public double MaturityAmount { get; set; }
        public double RealizeAmount { get; set; }
        public double AdjOverdueAmount { get; set; }
        public double AdjAdd { get; set; }
        public double AdjDeduct { get; set; }//// During Payment entry
        public double AdjPayable { get; set; }////  Deduct  DuringPayable Approved 
        public int Status { get; set; }
        public string Note { get; set; }
        public string ContractorName { get; set; }
        public string CPName { get; set; }
        public string Phone { get; set; }
        public string ExportLCNo { get; set; }
        public string LDBCNo { get; set; }
        public string PINo { get; set; }
        public string Currency { get; set; }
        public DateTime LDBCDate { get; set; }
        public DateTime PIDate { get; set; } // PI Issue Date
        public DateTime MaturityDate { get; set; }
        public DateTime MaturityReceivedDate { get; set; } ///NegotiationDate
        public DateTime RelizationDate { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime AmendmentDate {get; set;}

        public DateTime LCOpeningDate {get; set;}
        //public string LCDateSt { get; set; }
        public int ContractorID{ get; set; }
        public double Percentage_Maturity { get; set; }
        public double CRate { get; set; }
        public int CurrencyID_BC { get; set; }    
        public EnumLSalesCommissionStatus Status_Payable {get; set;}
        public double Amount_Bill { get; set; }
        public string ExportBillNo { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        public string Params { get; set; }
        public List<SalesCommissionPayable> SalesCommissionPayables { get; set; }
        
        #endregion

        #region Derive Property

        #region YetToBDT
        public double YetToPaidBDT
        {
            get
            {
                return this.YetToPaid*this.CRate;
            }
        }
        #endregion
        #region CommissionAmountSt
        public string CommissionAmountSt
        {
            get
            {
                //return this.Currency + "" + Global.MillionFormat(Amount,4);)
                return this.Currency + "" + this.CommissionAmount.ToString("#,##0.0000");
            }
        }
        #endregion
        #region PIDateSt
        public string PIDateSt
        {
            get
            {
                if (this.PIDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.PIDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region MaturityDateSt
        public string MaturityDateSt
        {
            get
            {
                if (this.MaturityDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.MaturityDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region MaturityReceivedDateSt
        public string MaturityReceivedDateSt
        {
            get
            {
                if (this.MaturityReceivedDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.MaturityReceivedDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region RelizationDateSt
        public string RelizationDateSt
        {
            get
            {
                if (this.RelizationDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.RelizationDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
       
        public string AmendmentDateStr
        {
            get
            {
                return (this.AmendmentDate == DateTime.MinValue) ? "--" : this.AmendmentDate.ToString("dd MMM yyyy");
                
            }
        }
        public string LCOpeningDateStr
        {
            get
            {
                return (this.LCOpeningDate == DateTime.MinValue) ? "--" : this.LCOpeningDate.ToString("dd MMM yyyy");

            }
        }
        public string Status_PayableSt { get { return (this.Status_Payable ==0)? "--": this.Status_Payable.ToString(); } }

        public double PayableAmount { get { return this.MaturityAmount + this.RealizeAmount - (this.AdjDeduct + this.AdjPayable); } }
        public double ActualPayableAmount { get { return (this.MaturityAmount + this.RealizeAmount - this.AdjDeduct - this.AdjOverdueAmount); } }
        public double Amount_Paid { get; set; }
        public double YetToPaid {  get { return this.PayableAmount - this.Amount_Paid; } }
        public double Amount_W_Paid { get; set; }

        public string IsWillVoucherEffectStr
        {
            get
            {
                return (this.IsWillVoucherEffect) ? "Yes" : "No";
            }
        }

        #endregion
      
        #region Functions
        public SalesCommissionPayable Get(int id, Int64 nUserID)
        {
            return SalesCommissionPayable.Service.Get(id, nUserID);
        }
        public static List<SalesCommissionPayable> Gets(int nPIID, Int64 nUserID)
        {
            return SalesCommissionPayable.Service.Gets(nPIID,nUserID);
        }
        public static List<SalesCommissionPayable> Gets(string sSQL, Int64 nUserID)
        {
            return SalesCommissionPayable.Service.Gets(sSQL, nUserID);
        }
        public SalesCommissionPayable Save(Int64 nUserID)
        {
            return SalesCommissionPayable.Service.Save(this, nUserID);
        }
        public SalesCommissionPayable VoucherEffect(Int64 nUserID)
        {
            return SalesCommissionPayable.Service.VoucherEffect(this, nUserID);
        }
        public static List<SalesCommissionPayable> SaveAll(List<SalesCommissionPayable> oSalesCommissionPayables, long nUserID)
        {
            return SalesCommissionPayable.Service.SaveAll(oSalesCommissionPayables, nUserID);
        }
        public SalesCommissionPayable Approved(Int64 nUserID)
        {
            return SalesCommissionPayable.Service.Approved(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return SalesCommissionPayable.Service.Delete(this, nUserID);
        }
        public static List<SalesCommissionPayable> ApprovedPayable(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            return SalesCommissionPayable.Service.ApprovedPayable(oSalesCommissionPayable, nUserID);
        }
        public static SalesCommissionPayable ApprovedPayableForPI(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            return SalesCommissionPayable.Service.ApprovedPayableForPI(oSalesCommissionPayable, nUserID);
        }
        public static List<SalesCommissionPayable> ApprovedRequestedPayable(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            return SalesCommissionPayable.Service.ApprovedRequestedPayable(oSalesCommissionPayable, nUserID);
        }
        public static SalesCommissionPayable ApprovedRequestedPayableForPI(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            return SalesCommissionPayable.Service.ApprovedRequestedPayableForPI(oSalesCommissionPayable, nUserID);
        }
       
        #endregion
        #region Non DB Functions
        #endregion

        #region ServiceFactory
        internal static ISalesCommissionPayableService Service
        {
            get { return (ISalesCommissionPayableService)Services.Factory.CreateService(typeof(ISalesCommissionPayableService)); }
        }
        #endregion

    }
    #endregion

    #region ISalesCommissionPayable interface
    public interface ISalesCommissionPayableService
    {
        SalesCommissionPayable Get(int id, Int64 nUserID);
        List<SalesCommissionPayable> Gets(int nPIID, Int64 nUserID);
        List<SalesCommissionPayable> Gets(string sSQL, Int64 nUserID);
        SalesCommissionPayable Save(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID);
        SalesCommissionPayable VoucherEffect(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID);
        List<SalesCommissionPayable> SaveAll(List<SalesCommissionPayable> oSalesCommissionPayables, Int64 nUserID);
        SalesCommissionPayable Approved(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID);
        List<SalesCommissionPayable> ApprovedPayable(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID);
        SalesCommissionPayable ApprovedPayableForPI(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID);
        List<SalesCommissionPayable> ApprovedRequestedPayable(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID);
        SalesCommissionPayable ApprovedRequestedPayableForPI(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID);
        string Delete(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID);
    }
    #endregion
}
