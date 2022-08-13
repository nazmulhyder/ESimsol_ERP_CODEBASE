using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class ExportLCReport
    {
        public ExportLCReport()
        {
            BUID = 0;
            ExportLCID = 0;
            ExportPIID = 0;
            LCNo = "";
            MasterLCNos = "";
            VersionNo = 0;
            LCOpenDate = new DateTime(1900, 1, 1);
            AmendmentDate = new DateTime(1900, 1, 1);
            LCReceiveDate = new DateTime(1900, 1, 1);
            ShipmentDate = new DateTime(1900, 1, 1);
            ExpiryDate = new DateTime(1900, 1, 1);
            LCStatus = EnumExportLCStatus.None;

            PINo = "";
            PIIssueDate = new DateTime(1900, 1, 1);
            ContractorID = 0;
            MKTEmpID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            UnitPrice = 0;
            Amount = 0;
            CurrencyID = 0;
            ProductCode = "";
            ProductName = "";
            MUSymbol = "";
            ContractorName = "";
            LAStDeliveryDate = new DateTime(1900, 1, 1);
            MKTPersonName = "";
            Currency = "";
            CurrencySymbol = "";
            BankName_Nego = "";
            BankName_Issue = "";
            BUName = "";
            DateYear = 0;
            DateMonth = 0;
            ErrorMessage = "";
            SearchingCriteria = "";
            LCReportLevelInt = 0;
            MUnitName = "";
            BankBranchID_Advice = 0;
            BankBranchID_Issue = 0;
            BankBranchID_NegoTiation = 0;
            FileNo = "";
            BuyerID = 0;
            Qty_DC = 0.0;
            Amount_DC = 0.0;
            Qty_Bill = 0.0;
            Amount_Bill = 0.0;
            BuyerAcc = 0;
            BankAcc = 0;
            NoteQuery = "";
            NoteUD = "";
            UDRcvType = 0;
            HaveQuery = false;
            GetOriginalCopy = false;
            LCTermsName = "";
        }

        #region Properties
        public int BUID { get; set; }
        public int ExportLCID { get; set; }
        public int ExportPIID { get; set; }
        public string LCNo { get; set; }
        public int VersionNo { get; set; }
        public bool HaveQuery { get; set; }
        public DateTime LCOpenDate { get; set; }
        public DateTime AmendmentDate { get; set; }
        public DateTime LCReceiveDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public EnumExportLCStatus LCStatus { get; set; }
        public string PINo { get; set; }
        public DateTime PIIssueDate { get; set; }
        public int ContractorID { get; set; }
        public int MKTEmpID { get; set; }
        public bool GetOriginalCopy { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public string MUnitName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUSymbol { get; set; }
        public string ContractorName { get; set; }
        public DateTime LAStDeliveryDate { get; set; }
        public string MKTPersonName { get; set; }
        public string MKTPName { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public string BankName_Nego { get; set; }
        public string BankName_Issue { get; set; }
        public string BUName { get; set; }
        public double DateYear { get; set; }
        public double DateMonth { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingCriteria { get; set; }
        public string MasterLCNos { get; set; }
        public int LCReportLevelInt { get; set; }
        public int LCReportTypeInt { get; set; }
        public int BankBranchID_Advice { get; set; }
        public int BankBranchID_Issue { get; set; }
        public int BankBranchID_NegoTiation { get; set; }
        public string FileNo { get; set; }
        public int BuyerID { get; set; }
        public double Qty_DC { get; set; }
        public double Amount_DC { get; set; }
        public double Qty_Bill { get; set; }
        public double Amount_Bill { get; set; }
        public int BuyerAcc { get; set; }
        public int BankAcc { get; set; }
        public string NoteQuery { get; set; }
        public string NoteUD { get; set; }
        public int UDRcvType { get; set; }
        public string BuyerName { get; set; }
        
        public string LCTermsName { get; set; }

        #endregion
        #region Derive Property
        public string LCStatusSt
        {
            get
            {
                return LCStatus.ToString();
            }
        }
        public string UDRcvTypeSt
        {
            get
            {
                if(this.UDRcvType==2)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string GetOriginalCopySt
        {
            get
            {
                if (this.GetOriginalCopy )
                {
                    return "Orginal";
                }
                else
                {
                    return "Copy";
                }
            }
        }
        public string PINoWithExportPIID
        {
            get
            {
                return this.PINo + "~" + this.ExportPIID;
            }
        }
        public string LCNoWithExportLCID
        {
            get
            {
                return this.LCNo + "~" + this.ExportLCID + "~" + this.ExportPIID + "~" + this.VersionNo;
            }
        }

        public string AmendmentNoWithExportLCID
        {
            get
            {
                if (this.VersionNo == 0 && this.ExportLCID == 0 && this.ExportPIID == 0)
                {
                    return "";
                }
                return this.VersionNo + "~" + this.ExportLCID + "~" + this.ExportPIID + "~" + this.VersionNo;
            }
        }
        public string PIDateSt
        {
            get
            {
                return this.GetDate(this.PIIssueDate);
            }
        }
        public string ShipmentDateSt
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string AmendmentDateSt
        {
            get
            {
                return this.GetDate(this.AmendmentDate);
            }
        }
        public string ExpiryDateSt
        {
            get
            {
                return this.ExpiryDate.ToString("dd MMM yyyy");
            }
        }
        public string LCReceiveDateSt
        {
            get
            {
                return this.GetDate(this.LCReceiveDate);
            }
        }
        public string LCOpenDateSt
        {
            get
            {
                return this.GetDate(this.LCOpenDate);
            }
        }


        public string UnitPriceSt
        {
            get
            {
                return this.GetAmount(this.UnitPrice);
            }
        }
        public string AmountSt
        {
            get
            {
                return this.GetAmount(this.Amount);
            }
        }
        public string Amount_BillSt
        {
            get
            {
                return this.GetAmount(this.Amount_Bill);
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.Qty) + " " + this.MUSymbol;
            }
        }
        public double YetToBill
        {
            get
            {
                return (this.Amount-this.Amount_Bill);
            }
        }


        private string GetAmount(double nVal)
        {
            if (nVal < 0)
            {
                return this.CurrencySymbol + " " + "(" + Global.MillionFormat(nVal * (-1)) + ")";
            }
            else if (nVal == 0)
            {
                return "-";
            }
            else
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(nVal);
            }
        }
        private string GetDate(DateTime dDate)
        {
            DateTime MinValue = new DateTime(1900, 01, 01);
            DateTime MinValue1 = new DateTime(0001, 01, 01);
            if (dDate == MinValue || dDate == MinValue1 || dDate == DateTime.MinValue)
            {
                return "-";
            }
            else
            {
                return dDate.ToString("dd MMM yyyy");
            }
        }
        public string BuyerAccSt
        {
            get
            {
                if (BuyerAcc == 0)
                {
                    return "";
                }
                else
                {
                    return BuyerAcc.ToString() + " part";
                }
            }
        }
        public string BankAccSt
        {
            get
            {
                if (BankAcc == 0)
                {
                    return "";
                }
                else
                {
                    return this.BankAcc.ToString() + " part";
                }
            }
        }
        #endregion

        #region Function
        public static List<ExportLCReport> Gets(Int64 nUserID)
        {
            return ExportLCReport.Service.Gets(nUserID);
        }
        public ExportLCReport Get(int nExportPIID, long nUserID)
        {
            return ExportLCReport.Service.Get(nExportPIID, nUserID);
        }
        public static List<ExportLCReport> Gets(string sSQL, EnumLCReportLevel eLCReportLevel, Int64 nUserID)
        {
            return ExportLCReport.Service.Gets(sSQL, eLCReportLevel, nUserID);
        }
        public static List<ExportLCReport> Gets(string sSQL, Int64 nUserID)
        {
            return ExportLCReport.Service.Gets(sSQL, nUserID);
        }
        public static List<ExportLCReport> GetsReportProduct(ExportLCReport oExportLCReport, Int64 nUserID)
        {
            return ExportLCReport.Service.GetsReportProduct(oExportLCReport, nUserID);
        }
        public static List<ExportLCReport> GetsReport(string sSQL, int nReportType, Int64 nUserID)
        {
            return ExportLCReport.Service.GetsReport(sSQL, nReportType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportLCReportService Service
        {
            get { return (IExportLCReportService)Services.Factory.CreateService(typeof(IExportLCReportService)); }
        }
        #endregion
    }
    #region IExportLCReport interface
    public interface IExportLCReportService
    {
        ExportLCReport Get(int nExportPIID, long nUserID);
        List<ExportLCReport> Gets(long nUserID);
        List<ExportLCReport> Gets(string sSQL, EnumLCReportLevel eLCReportLevel, long nUserID);
        List<ExportLCReport> Gets(string sSQL, long nUserID);
        List<ExportLCReport> GetsReportProduct(ExportLCReport oExportLCReport, long nUserID);
        List<ExportLCReport> GetsReport(string sSQL, int nReportType, long nUserID);
    }
    #endregion

    public class ExportLCReportDetail
    {
        public ExportLCReportDetail()
        {
            ExportPIID = 0;
            ExportPIDetailID = 0;
            PINo = "";
            ProductName = "";
            PIQty = 0;
            DOQty = 0;
            ChallanQty = 0;
            PIValue = 0;
            DOValue = 0;
            ChallanValue = 0;
            CurrencySymbol = "";
            MUName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ExportPIID { get; set; }
        public int ExportPIDetailID { get; set; }
        public string PINo { get; set; }
        public string ProductName { get; set; }
        public string CurrencySymbol { get; set; }
        public string MUName { get; set; }
        public double PIQty { get; set; }
        public double DOQty { get; set; }
        public double ChallanQty { get; set; }
        public double PIValue { get; set; }
        public double DOValue { get; set; }
        public double ChallanValue { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Properties
        public string YetToDoQtySt
        {
            get
            {
                return this.GetAmount(this.PIQty - this.DOQty);
            }
        }
        public string YetToChallanQtySt
        {
            get
            {
                return this.GetAmount(this.PIQty - this.ChallanQty);
            }
        }
        public string PIQtySt
        {
            get
            {
                return this.GetAmount(this.PIQty);
            }
        }
        public string DOQtySt
        {
            get
            {
                return this.GetAmount(this.DOQty);
            }
        }
        public string ChallanQtySt
        {
            get
            {
                return this.GetAmount(this.ChallanQty);
            }
        }
        public string PIValueSt
        {
            get
            {
                string sPIValue = this.GetAmount(this.PIValue);
                if (sPIValue == "-")
                {
                    return "-";
                }
                return this.CurrencySymbol + " " + sPIValue;
            }
        }
        public string DOValueSt
        {
            get
            {
                string sDOValue = this.GetAmount(this.DOValue);
                if (sDOValue == "-")
                {
                    return "-";
                }
                return this.CurrencySymbol + " " + sDOValue;
            }
        }
        
        public string ChallanValueSt
        {
            get
            {
                string sChallanValue = this.GetAmount(this.ChallanValue);
                if (sChallanValue == "-")
                {
                    return "-";
                }
                return this.CurrencySymbol + " " + sChallanValue;
            }
        }
        private string GetAmount(double nVal)
        {
            if (nVal < 0)
            {
                return "(" + Global.MillionFormat(nVal * (-1)) + ")";
            }
            else if (nVal == 0)
            {
                return "-";
            }
            else
            {
                return Global.MillionFormat(nVal);
            }
        }
        #endregion
    }
}
