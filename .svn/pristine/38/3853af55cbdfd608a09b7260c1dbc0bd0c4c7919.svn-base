using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class ExportFollowup :BusinessObject
    {
        public ExportFollowup()
        {
            BUName = string.Empty;
            Amount_SaleBudget = 0;
            Qty_Production = 0;
            Qty_Delivery_In = 0;
            Qty_Delivery_Out = 0;
            Amount_Delivery = 0;
            Amount_Delivery_BC = 0;
            CurrencyID = 0;
            Currency = "";
            Qty_PI = 0;
            Amount_PI = 0;
            Count_PI = 0;
            Amount_PI = 0;
            BUID = 0;
            Qty_Cash = 0;
            Amount_Cash = 0;
            Amount_Bill = 0;
            LCQty = 0;
            Amount_LC = 0;
            Count_LC = 0;
            Params = string.Empty;
            Bank_Nego = string.Empty;
            ErrorMessage = string.Empty;
            nReportType = 0;
            dateType = 0;
            SName_Nego = "";
            SName_Issue = "";            
        }

        #region properties
        public int BUID { get; set; }
        public int nReportType { get; set; }
        public int dateType { get; set; }
        public String BUName { get; set; }
        public String Bank_Nego { get; set; }
        public double Amount_SaleBudget {get; set;}
        public double Qty_Production {get; set;}
        public double Qty_Delivery_In {get; set;}
        public double Qty_Delivery_Out {get; set;}
        public double Amount_Delivery {get; set;}
        public double Amount_Delivery_BC {get; set;}
        public double Qty_PI {get; set;}
        public double Amount_PI {get; set;}
        public double Count_PI {get; set;}
        public double Count_Cash { get; set; }
        public double Qty_Cash {get; set;}
        public double Amount_Cash {get; set;}
        public double Amount_Bill { get; set; }
        public double Qty_Bill { get; set; }
        public double LCQty { get; set; }
        public double Amount_LC {get; set;}
        public double Count_LC { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Currency { get; set; }
        public int CurrencyID { get; set; } // as per LC Currency
        public int BankBranchID { get; set; }
        public string BankName { get; set; }
        public double BOinHand { get; set; }
        public double BOInCusHand { get; set; }
        public double AcceptadBill { get; set; }
        public double NegoTransit { get; set; }
        public double NegotiatedBill { get; set; }
        public double Discounted { get; set; }
        public double PaymentDone { get; set; }
        public double BFDDRecd { get; set; }
        public double Amount_Due { get; set; }
        public double Amount_ODue { get; set; }
        public double Amount { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }

        #region  Properties For CHTML View
        public string Name { get; set; }
        public string Name_R{ get; set; }
        public string Name_Y { get; set; }

        public int Part { get; set; }
        public int Count { get; set; }
        public int Count_R { get; set; }
        public int Count_Y { get; set; }

        public double Qty { get; set; }
        public double Qty_R { get; set; }
        public double Qty_Y { get; set; }
        public double Amount_R { get; set; }
        public double Amount_Y { get; set; }
        #endregion

        #endregion

        #region property for Details
        public int ExportLCID { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public int BBranchID_Nego { get; set; }
        public string BankName_Nego { get; set; }
        public EnumLCBillEvent State { get; set; }        
        public string SName_Nego { get; set; }
        public string BBranchName_Nego { get; set; }
        public int BBranchID_Issue { get; set; }
        public string BankName_Issue { get; set; }
        public string SName_Issue { get; set; }
        public string BBranchName_Issue { get; set; }
        public string ExportLCNo { get; set; }
        public double DeliveryQty { get; set; }
        public double DeliveryChallanQty { get; set; }
        public double YetToInvoice { get; set; }
        public double DeliveryValue { get; set; }        
        public int LCStatus { get; set; } 
        public int ExportBillID { get; set; }
        public string ExportBillNo { get; set; }
        public string LCFileNo { get; set; }
        public DateTime SendToParty { get; set; }
        public DateTime Shipment_Date { get; set; }
        public DateTime RecdFromParty { get; set; }
        public DateTime SendToBank { get; set; }
        public DateTime RecdFromBank { get; set; }
        public double OverdueRate { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public string MKTPName { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime MaturityReceivedDate { get; set; }
        public DateTime LCRecivedDate { get; set; }
        public string LDBCNo { get; set; }
        public string LDBPNo { get; set; }
        public double LDBPAmount { get; set; }
        public DateTime LDBCDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public DateTime DiscountedDate { get; set; }
        public DateTime BankFDDRecDate { get; set; }
        public DateTime RelizationDate { get; set; }
        public DateTime EncashmentDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public int prop { get; set; }
        public int ExportSCID { get; set; }
        public int ExportPIID { get; set; }
        public int VersionNo { get; set; }
        #endregion

        #region Derived Properties
        public string Bill_Status
        {
            get
            {
                return EnumObject.jGet(this.State);
            }
        }
        public double YetToDeliveryQty { get { return (this.Qty - this.DeliveryQty); } }
        public double YetToDeliveryValue { get { return (this.Amount - this.DeliveryValue); } }        
        public string TimeLag { get {

            if (this.DeliveryDate == DateTime.MinValue)
                return "-";
            else
                return( (DateTime.Now-this.DeliveryDate).TotalDays+1).ToString("dd MMM yy"); 
        } }
        public string AmountWithCur
        {
            get
            {
                return (this.Currency + " " + String.Format("{0:0.00}", this.Amount));
            }
        }
        public string AmountCashWithCur
        {
            get
            {
                return (this.Currency + " " + String.Format("{0:0.00}", this.Amount_Cash));
            }
        }
        public string StartDateStr { 
            get {
                    return (this.StartDate == DateTime.MinValue) ? "" : this.StartDate.ToString("dd MMM yyyy"); 
            } 
        }
        public string EndDateStr
        {
            get
            {
                return (this.EndDate == DateTime.MinValue) ? "" : this.EndDate.ToString("dd MMM yyyy");
            }
        }
        public string StartDateMonthStr
        {
            get
            {
                if (this.StartDate == DateTime.MinValue)
                    return "-";
                else return (this.StartDate == DateTime.MinValue) ? "" : this.StartDate.ToString("MMMM yy");
            }
        }
        #endregion

        #region Dereived Prop For Details
        public string SendToPartyStr
        {
            get
            {
                if (this.SendToParty == DateTime.MinValue)
                    return "-";
                else return this.SendToParty.ToString("dd MMM yyyy");
            }
        }
        public string RecdFromPartyStr
        {
            get
            {
                if (this.RecdFromParty == DateTime.MinValue)
                    return "-";
                else return this.RecdFromParty.ToString("dd MMM yyyy");
            }
        }
        public string SendToBankStr
        {
            get
            {
                if (this.SendToBank == DateTime.MinValue)
                    return "-";
                else return this.SendToBank.ToString("dd MMM yyyy");
            }
        }
        public string Shipment_DateStr
        {
            get
            {
                if (this.Shipment_Date == DateTime.MinValue)
                    return "-";
                else return this.Shipment_Date.ToString("dd MMM yyyy");
            }
        }
        public string RecdFromBankStr
        {
            get
            {
                if (this.RecdFromBank == DateTime.MinValue)
                    return "-";
                else return this.RecdFromBank.ToString("dd MMM yyyy");
            }
        }
        public string LCOpeningDateStr
        {
            get
            {
                if (this.LCOpeningDate == DateTime.MinValue)
                    return "-";
                else return this.LCOpeningDate.ToString("dd MMM yyyy");
            }
        }
        public string MaturityDateStr
        {
            get
            {
                if (this.MaturityDate == DateTime.MinValue)
                    return "-";
                else return this.MaturityDate.ToString("dd MMM yyyy");
            }
        }
        public string MaturityReceivedDateStr
        {
            get
            {
                if (this.MaturityReceivedDate == DateTime.MinValue)
                    return "-";
                else return this.MaturityReceivedDate.ToString("dd MMM yyyy");
            }
        }
        public string LCRecivedDateStr
        {
            get
            {
                if (this.LCRecivedDate == DateTime.MinValue)
                    return "-";
                else return this.LCRecivedDate.ToString("dd MMM yyyy");
            }
        }
        public string LDBCDateStr
        {
            get
            {
                if (this.LDBCDate == DateTime.MinValue)
                    return "-";
                else return this.LDBCDate.ToString("dd MMM yyyy");
            }
        }
        public string AcceptanceDateStr
        {
            get
            {
                if (this.AcceptanceDate == DateTime.MinValue)
                    return "-";
                else return this.AcceptanceDate.ToString("dd MMM yyyy");
            }
        }
        public string DiscountedDateStr
        {
            get
            {
                if (this.DiscountedDate == DateTime.MinValue)
                    return "-";
                else return this.DiscountedDate.ToString("dd MMM yyyy");
            }
        }
        public string BankFDDRecDateStr
        {
            get
            {
                if (this.BankFDDRecDate == DateTime.MinValue)
                    return "-";
                else return this.BankFDDRecDate.ToString("dd MMM yyyy");
            }
        }
        public string RelizationDateStr
        {
            get
            {
                if (this.RelizationDate == DateTime.MinValue)
                    return "-";
                else return this.RelizationDate.ToString("dd MMM yyyy");
            }
        }
        public string EncashmentDateStr
        {
            get
            {
                if (this.EncashmentDate == DateTime.MinValue)
                    return "-";
                else return this.EncashmentDate.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateStr
        {
            get
            {
                if (this.DeliveryDate == DateTime.MinValue)
                    return "-";
                else return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string PIDateStr
        {
            get
            {
                if (this.PIDate == DateTime.MinValue)
                    return "-";
                else return this.PIDate.ToString("dd MMM yy");
            }
        }
        #endregion

        #region Functions
        public static List<ExportFollowup> GetsExportFollowup(int nBUID, DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return ExportFollowup.Service.GetsExportFollowup(nBUID, StartDate, EndDate, nUserID);
        }
        public static List<ExportFollowup> Gets_Summary(int nBUID, Int64 nUserID)
        {
            return ExportFollowup.Service.Gets_Summary(nBUID, nUserID);
        }
        public static List<ExportFollowup> Gets_BillRealize(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            return ExportFollowup.Service.Gets_BillRealize(nBUID, dStartDate, dEndDate, nUserID);
        }
        public static List<ExportFollowup> Gets_BillMaturity(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            return ExportFollowup.Service.Gets_BillMaturity(nBUID, dStartDate, dEndDate, nUserID);
        }
        public static List<ExportFollowup> Gets_Details(ExportFollowup oExportFollowup, Int64 nUserID)
        {
            return ExportFollowup.Service.Gets_Details(oExportFollowup, nUserID);
        }
        public static List<ExportFollowup> Gets(string sSQL, Int64 nUserID)
        {
            return ExportFollowup.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportFollowupService Service
        {
            get { return (IExportFollowupService)Services.Factory.CreateService(typeof(IExportFollowupService)); }
        }

        #endregion
    }

    #region  TipsType interface
    public interface IExportFollowupService
    {
        List<ExportFollowup> GetsExportFollowup(int nBUID, DateTime StartDate, DateTime EndDate, long nUserID);
        List<ExportFollowup> Gets_Summary(int nBUID, Int64 nUserID);
        List<ExportFollowup> Gets_Details(ExportFollowup oExportFollowup, Int64 nUserID);
        List<ExportFollowup> Gets(string sSQL, Int64 nUserID);
        List<ExportFollowup> Gets_BillRealize(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID);
        List<ExportFollowup> Gets_BillMaturity(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID);
       
    }
    #endregion
}
