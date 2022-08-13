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
   public class SalesComPaymentDetail
    {
       public SalesComPaymentDetail()
       {
        SalesComPaymentDetailID =0;
        SalesComPaymentID =0;
        SalesCommissionPayableID =0;
        PayableAmountBC = 0;
        ActualPayable = 0;
        Amount = 0;
        AmountBC = 0;
        Note="";
        ErrorMessage = "";
        Params = "";
        LDBCNo = "";
        TempSalesComPayment = new SalesComPayment();
        SalesComPaymentDetails = new List<SalesComPaymentDetail>();
       }
        #region Properties
        public int SalesComPaymentDetailID {get;set;}
        public int SalesComPaymentID {get;set;}
        public int SalesCommissionPayableID { get; set; }
        public double PayableAmountBC { get; set; }
        public double ActualPayable { get; set; }
        public double Amount { get; set; }
        public double AmountBC { get; set; }
        public string Note {get;set;}
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion
        #region Derived Properties
        public SalesComPayment TempSalesComPayment { get; set; }
        public List<SalesComPaymentDetail> SalesComPaymentDetails { get; set; }
        public string CPName { get; set; }
        public string Currency { get; set; }
        public int CurrencyID { get; set; }
        public string PINo { get; set; }
        public int BUID { get; set; }
        public int ContractorID { get; set; }
        public DateTime PIDate { get; set; }
        public string ExportLCNo { get; set; }
        public int ExportLCID { get; set; }
        public double Amount_Bill { get; set; }
        public int ContactPersonnelID { get; set; }
        public double MaturityAmount { get; set; }  
        public double RealizeAmount { get; set; }
        public int ExportPIID { get; set; }
        public int ExportBillID { get; set; }
        public string ExportBillIDNo { get; set; }
        public string LDBCNo { get; set; }
        public double CommissionAmount { get; set; }

        public double Amount_Paid { get; set; }
        public double AdjAdd { get; set; }
        public double AdjDeduct { get; set; }
        public double AdjPayable { get; set; }
        public double Param_CRate { get; set; }
        public String ContractorName { get; set; }

        public double PayableAmount
        {
            get
            {
                if (this.SalesComPaymentDetailID>0)
                { return Math.Round((this.MaturityAmount + this.RealizeAmount + this.Amount + this.AdjAdd + this.AdjDeduct) - (this.AdjDeduct + this.AdjPayable + this.Amount_Paid), 2); }
                else { return Math.Round((this.MaturityAmount + this.RealizeAmount + this.AdjAdd) - (this.AdjDeduct + this.AdjPayable + this.Amount_Paid), 2); }
                
            }
        }

        #endregion

        #region Functions

        public static SalesComPaymentDetail Get(int nSalesComPaymentDetailID, long nUserID)
        {
            return SalesComPaymentDetail.Service.Get(nSalesComPaymentDetailID, nUserID);
        }
        public static List<SalesComPaymentDetail> Gets(string sSQL, long nUserID)
        {
            return SalesComPaymentDetail.Service.Gets(sSQL, nUserID);
        }
        public SalesComPaymentDetail IUD(int nDBOperation, long nUserID)
        {
            return SalesComPaymentDetail.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISalesComPaymentDetailService Service
        {
            get { return (ISalesComPaymentDetailService)Services.Factory.CreateService(typeof(ISalesComPaymentDetailService)); }
        }

        #endregion

    }

   #region ISULayDownDetail interface

   public interface ISalesComPaymentDetailService
   {

       SalesComPaymentDetail Get(int nSalesComPaymentDetailID, Int64 nUserID);
       List<SalesComPaymentDetail> Gets(string sSQL, Int64 nUserID);
       SalesComPaymentDetail IUD(SalesComPaymentDetail oSalesComPaymentDetail, int nDBOperation, Int64 nUserID);

   }
   #endregion
}
