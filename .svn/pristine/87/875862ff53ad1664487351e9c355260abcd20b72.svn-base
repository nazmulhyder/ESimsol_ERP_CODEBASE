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
    #region ImportInvoiceDetail
    public class ImportInvoiceDetail : BusinessObject
    {
        #region  Constructor
        public ImportInvoiceDetail()
        {
            ImportInvoiceDetailID = 0;
            ImportInvoiceID = 0;
            ProductID = 0;
            UnitPriceBC = 0;
            UnitPrice = 0;
            ReceiveQty = 0;
            MUnitID = 0;
            Qty = 0;
            GRNID  = 0;
            ProductCode ="";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            Amount = 0;
            PODQty = 0;
            YetToInvoiceQty = 0;
            PODRate = 0;
            PODAmount = 0;
            TechnicalSpec = "";
            GRNNo = "";
            GRNDate = DateTime.Now;
            CurrencyName = "";
            CurrencySymbol = "";
            YetToGRNQty = 0;
            RateUnit = 1;
            IsSerialNoApply = false;
            InvoiceLandingCost = 0;
            LCLandingCost = 0;
            ImportPIDetailID = 0;
            TechnicalSheetID = 0;
            StyleNo = "";
        }
        #endregion

        #region Properties
        public int ImportInvoiceDetailID { get; set; }
        public int ImportInvoiceID { get; set; }
        public int ImportPIDetailID { get; set; }
        public int ProductID { get; set; }
        public double UnitPrice { get; set; }
        public double UnitPriceBC { get; set; }
        public double ReceiveQty { get; set; }
        public int MUnitID { get; set; }     
        public double Qty { get; set; }
        public double Amount { get; set; }
        public bool ApplyInventory { get; set; }        
        public int GRNID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductSpec{ get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }        
        public double PODQty { get; set; }
        public double YetToInvoiceQty { get; set; }
        public double PODRate { get; set; }
        public double PODAmount { get; set; }
        public string TechnicalSpec { get; set; }
        public string GRNNo { get; set; }
        public double GRNQty { get; set; }
        public DateTime GRNDate { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public double YetToGRNQty { get; set; }
        public bool IsSerialNoApply { get; set; }        
        public double InvoiceLandingCost { get; set; }
        public double LCLandingCost { get; set; }
        public int TechnicalSheetID { get; set; }
        public string StyleNo { get; set; }
        public int RateUnit { get; set; }              
        #region DerivedProperties
        public string Currency { get; set; }
        public string ErrorMessage { get; set; }
        #region Amount
       
        #endregion

        #region AmountSt
        public string AmountSt
        {
            get { return this.Currency + "" + Global.MillionFormat(this.Amount); }
        }
        #endregion
        public string QtyST
        {
            get
            {
                return  Global.MillionFormat(this.Qty);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                if (this.RateUnit <= 1)
                {
                    return Global.MillionFormat(this.UnitPrice);
                }
                else
                {
                    return Global.MillionFormat(this.UnitPrice) + "/" + this.RateUnit.ToString();
                }
            }
        }
        public string GRNDateString
        {
            get
            {
                if (this.GRNDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.GRNDate.ToString("dd MMM yyyy");
                }
            }
        }

        #endregion

        #endregion

        #region Functions
        public ImportInvoiceDetail Get(int nImportInvoiceDetailID, long nUserID)
        {
            return ImportInvoiceDetail.Service.Get(nImportInvoiceDetailID, nUserID);
        }
        public ImportInvoiceDetail Save(long nUserID)
        {
            return ImportInvoiceDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ImportInvoiceDetail.Service.Delete(this, nUserID);
        }

        //public static List<ImportInvoiceDetail> Gets(Guid wcfSessionid)
        //{
        //    return (List<ImportInvoiceDetail>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Gets")[0];
        //}

        public static List<ImportInvoiceDetail> Gets(int nImportInvoiceID, long nUserID)
        {
            return ImportInvoiceDetail.Service.Gets(nImportInvoiceID, nUserID);
        }

        public static List<ImportInvoiceDetail> Gets(string sSQL, long nUserID)
        {
            return ImportInvoiceDetail.Service.Gets(sSQL, nUserID);
        }
     
       
        #endregion

        #region ServiceFactory

     
        internal static IImportInvoiceDetailService Service
        {
            get { return (IImportInvoiceDetailService)Services.Factory.CreateService(typeof(IImportInvoiceDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IImportInvoiceDetail interface
    public interface IImportInvoiceDetailService
    {
        ImportInvoiceDetail Get(int nID, Int64 nUserId);
        List<ImportInvoiceDetail> Gets(int nImportInvoiceID, Int64 nUserId);
        List<ImportInvoiceDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(ImportInvoiceDetail oImportInvoiceDetail, Int64 nUserId);
        ImportInvoiceDetail Save(ImportInvoiceDetail oImportInvoiceDetail, Int64 nUserID);
    }
    #endregion
}
