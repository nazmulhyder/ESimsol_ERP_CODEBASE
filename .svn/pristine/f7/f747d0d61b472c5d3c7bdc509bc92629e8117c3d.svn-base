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
    #region SalesCommissionLC
    public class SalesCommissionLC :BusinessObject
    {
        public SalesCommissionLC()
        {
            ExportPIID = 0;
            ExportLCID = 0;
            PINo = "";
            LCNo = "";
            PIDate = DateTime.Today;
            Amount = 0;
            Currency = "";
            LCOpenDate = DateTime.Today;
            AmendmentDate = DateTime.Today;
            VersionNo = 0;
            ContractorID = 0;
            ContractorName = "";
            BuyerID = 0;
            BuyerName = "";
            InvoiceValue = 0;
            Com_PI = 0;
            Com_Dis = 0;
            Com_Payable = 0;
            Com_Paid = 0;
            ErrorMessage = "";
            Params = "";
        }
        #region Properties
        public int ExportPIID { get; set; }

        public int ExportLCID { get; set; }
        public int BUID { get; set; }
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public string LCNo { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }

        public DateTime LCOpenDate { get; set; }
        public DateTime AmendmentDate { get; set; }
        public int VersionNo { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public double InvoiceValue { get; set; }
        public double Com_PI { get; set; }
        public double Com_Dis { get; set; }
        public double Com_Payable { get; set; }
        public double Com_Paid { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Properties
        public string AmountWithCurrencySign { get { return Global.MillionFormat(this.Amount).ToString() + " " + this.Currency; } }
        public string PIDateStr { get { return (this.PIDate == DateTime.MinValue) ? "-" : this.PIDate.ToString("dd MMM yyyy"); } }
        public string LCOpenDateStr { get { return (this.LCOpenDate == DateTime.MinValue) ? "-" : this.LCOpenDate.ToString("dd MMM yyyy"); } }
        public string AmendmentDateStr { get { return (this.AmendmentDate == DateTime.MinValue) ? "-" : this.AmendmentDate.ToString("dd MMM yyyy"); } }
        public string VersionNoStr { get { return (this.VersionNo == 0) ? "-" : this.VersionNo.ToString(); } }
        public EnumLSalesCommissionStatus Status { get; set; }
        
        public string StatusStr { get { return this.Status.ToString(); } }
        #endregion

        #region Functions
        public static List<SalesCommissionLC> Gets(string sSQL, long nUserID)
        {
            return SalesCommissionLC.Service.Gets(sSQL, nUserID);
        }

        public List<SalesCommissionLC> IUD(int nDBOperation, long nUserID)
        {
            return SalesCommissionLC.Service.IUD(this, nDBOperation, nUserID);
        }
        public SalesCommissionLC GetByExportPIID(int id, Int64 nUserID)
        {
            return SalesCommissionLC.Service.GetByExportPIID(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISalesCommissionLCService Service
        {
            get { return (ISalesCommissionLCService)Services.Factory.CreateService(typeof(ISalesCommissionLCService)); }
        }
        #endregion
    }
    #endregion

    #region ISUProductionProcess interface

    public interface ISalesCommissionLCService
    {
        SalesCommissionLC GetByExportPIID(int id, Int64 nUserID);
        List<SalesCommissionLC> Gets(string sSQL, Int64 nUserID);
        List<SalesCommissionLC> IUD(SalesCommissionLC oSalesCommissionLC, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
