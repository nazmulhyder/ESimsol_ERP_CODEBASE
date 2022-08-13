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
    #region Export_LDBPDetail
    public class Export_LDBPDetail : BusinessObject
    {
        public Export_LDBPDetail()
        {
            Export_LDBPDetailID = 0;
            Export_LDBPID = 0;
            ExportBillID = 0;
            ExportBillNo = "";
            ExportBillDate = DateTime.Now;
            Amount = 0;
            LDBCNo = "";
            MaturityDate = DateTime.Now;
            BankName_Issue = "";
            ErrorMessage = "";
            LDBPDate = DateTime.Now;
            NegoBank = "";
            BUID = 0;
        }

        #region Properties
        public int Export_LDBPDetailID { get; set; }
        public int Export_LDBPID { get; set; }
        public int ExportBillID { get; set; }
        public string ExportBillNo { get; set; }
        public DateTime ExportBillDate { get; set; }
        public double Amount { get; set; }
        public string LDBCNo { get; set; }
        public string LDBPNo { get; set; }
        public double LDBPAmount { get; set; }
        public DateTime LDBPDate { get; set; }
        public string ExportLCNo { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public string BankName_Issue { get; set; }
        public string AccountNo { get; set; }
        public string ApplicantName { get; set; }
        public int BUID { get; set; }
        public double CCRate { get; set; }
        public int CurrencyID { get; set; }
        public string Currency { get; set; }
        public EnumLCBillEvent Status { get; set; }
        public string ErrorMessage { get; set; }
        public int BankBranchID { get; set; }
         public int BankAccountIDRecd { get; set; }
        #endregion

        #region Derived Property
         public ExportBill ExportBill { get; set; }
         public Export_LDBP Export_LDBP { get; set; }
        public string MaturityDateSt
        {
            get
            {
                return this.MaturityDate.ToString("dd MMM yyyy");
            }
        }
        public string LDBPDatest
        {
            get
            {
                if (this.LDBPDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.LDBPDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string LCOpeningDatest
        {
            get
            {
                if (this.LCOpeningDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.LCOpeningDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string AmountSt
        {
            get
            {
                return "$ " + Global.MillionFormat(this.Amount);
            }
        }
        public string StatusSt
        {
            get
            {
                return this.Status.ToString();
            }
        }
        public string NegoBank { get; set; }
        #endregion

        #region Functions
        public Export_LDBPDetail Get(int nExport_LDBPDetailID, Int64 nUserID)
        {
            return Export_LDBPDetail.Service.Get(nExport_LDBPDetailID, nUserID);
        }
        public Export_LDBPDetail GetBy(int nExportBillID, Int64 nUserID)
        {
            return Export_LDBPDetail.Service.GetBy(nExportBillID, nUserID);
        }
        public static List<Export_LDBPDetail> Gets(string sSQL, Int64 nUserID)
        {
            return Export_LDBPDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<Export_LDBPDetail> Gets(int nExport_LDBPID, Int64 nUserID)
        {
            return Export_LDBPDetail.Service.Gets(nExport_LDBPID, nUserID);
        }
        public Export_LDBPDetail Save(Int64 nUserID)
        {
            return Export_LDBPDetail.Service.Save(this, nUserID);
        }
        public Export_LDBPDetail Save_LDBP(Int64 nUserID)
        {
            return Export_LDBPDetail.Service.Save_LDBP(this, nUserID);
        }
        public Export_LDBPDetail Cancel_Request(Int64 nUserID)
        {
            return Export_LDBPDetail.Service.Cancel_Request(this, nUserID);
        }
     
        
        #endregion

        #region ServiceFactory

        internal static IExport_LDBPDetailService Service
        {
            get { return (IExport_LDBPDetailService)Services.Factory.CreateService(typeof(IExport_LDBPDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IExport_LDBPDetail interface
    public interface IExport_LDBPDetailService
    {
        Export_LDBPDetail Get(int nExport_LDBPDetailID, Int64 nUserID);
        Export_LDBPDetail GetBy(int nExportBillID, Int64 nUserID);
        List<Export_LDBPDetail> Gets(int nExport_LDBPID, Int64 nUserID);
        List<Export_LDBPDetail> Gets(string sSQL, Int64 nUserID);
        Export_LDBPDetail Save(Export_LDBPDetail oExport_LDBPDetail, Int64 nUserID);
        Export_LDBPDetail Save_LDBP(Export_LDBPDetail oExport_LDBPDetail, Int64 nUserID);
        Export_LDBPDetail Cancel_Request(Export_LDBPDetail oExport_LDBPDetail, Int64 nUserID);
    }
    
    #endregion
}