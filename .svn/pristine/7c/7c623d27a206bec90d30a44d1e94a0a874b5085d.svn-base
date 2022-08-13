using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportLCRegister
    public class ExportLCRegister : BusinessObject
    {
        public ExportLCRegister()
        {
            ExportPIID=0;
            PINo="";
            PIDate=DateTime.MinValue;
            LCValue=0;
            BuyerID=0;
            MKTEmpID=0;
            PIStatus=0;
            VersionNo=0;
            LCOpenDate=DateTime.MinValue;
            AmendmentDate=DateTime.MinValue;
            LCReceiveDate=DateTime.MinValue;
            UDRecDate = DateTime.MinValue;
            ShipmentDate = DateTime.MinValue;
            ExpiryDate = DateTime.MinValue;
            UDRcvType = EnumUDRcvType.No_Receive;
            ExportLCID=0;
            LCNo="";
            ApplicantID=0;
            LCStatus=EnumLCStatus.None;
            NegoBankBranchID=0;
            IssueBankBranchID=0;
            NoteQuery="";
            NoteUD="";
            HaveQuery = false;
            GetOriginalCopy = false;
            Currency="";
            ApplicantName="";
            BuyerName="";
            MKTPersonName="";
            NegoBankName="";
            IssueBankName="";
            Value_DO=0;
            Value_DC=0;
            //Value_Invoice=0;
            ProductName="";
            Qty = 0.0;
            Qty_DO = 0;
            Qty_DC = 0;
            UnitPrice=0;
            Acc_Bank=0;
            Acc_Party=0;
            Qty_Invoice = 0.0;
            ReportLayout = EnumReportLayout.None;
            ExportLCType = EnumExportLCType.None;
        }
        #region Properties
        public int ExportPIID { get; set; }
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public double LCValue { get; set; }
        public int BuyerID { get; set; }
        public int MKTEmpID { get; set; }
        public EnumPIStatus PIStatus { get; set; }
        public int VersionNo { get; set; }
        public DateTime LCOpenDate { get; set; }
        public DateTime AmendmentDate { get; set; }
        public DateTime LCReceiveDate { get; set; }
        public DateTime UDRecDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public EnumUDRcvType UDRcvType { get; set; }
        public int ExportLCID { get; set; }
        public string LCNo { get; set; }
        public int ApplicantID { get; set; }
        public EnumLCStatus LCStatus { get; set; }
        public int NegoBankBranchID { get; set; }
        public int IssueBankBranchID { get; set; }
        public string NoteQuery { get; set; }
        public string NoteUD { get; set; }
        public bool HaveQuery { get; set; }
        public bool GetOriginalCopy { get; set; }
        public string Currency { get; set; }
        public string ApplicantName { get; set; }
        public string BuyerName { get; set; }
        public string MKTPersonName { get; set; }
        public string NegoBankName { get; set; }
        public string IssueBankName { get; set; }
        public double Value_DO { get; set; }
        public double Value_DC { get; set; }
        //public double Value_Invoice { get; set; }
        public string ProductName { get; set; }
        public double Qty { get; set; }
        public double Qty_DO { get; set; }
        public double Qty_DC { get; set; }
        public double Qty_Invoice { get; set; }
        public double UnitPrice { get; set; }
        public int BUID { get; set; }
        public int Acc_Bank { get; set; }
        public EnumExportLCType ExportLCType { get; set; }
        public int Acc_Party { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion
        #region Derived Property
        public string ErrorMessage { get; set; }
        public double Amount { get { return this.Qty*this.UnitPrice; } }
        public double Amount_YetToInvoice { get { return (this.Qty * this.UnitPrice) - (this.UnitPrice * this.Qty_Invoice); } }
        public string AmendmentDateSt
        {
            get 
            {
                if (this.AmendmentDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.AmendmentDate.ToString("dd MMM yyyy");
                }
            }
        }
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
        public string LCReceiveDateSt
        {
            get
            {
                if (this.LCReceiveDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.LCReceiveDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string LCOpenDateSt
        {
            get
            {
                if (this.LCOpenDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.LCOpenDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string UDRecDateSt
        {
            get
            {
                if (this.UDRecDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.UDRecDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ShipmentDateSt
        {
            get
            {
                if (this.ShipmentDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ExpiryDateSt
        {
            get
            {
                if (this.ExpiryDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ExpiryDate.ToString("dd MMM yyyy");
                }
            }
        }
        public int LCStatusInt { get { return (int)LCStatus; } }
        public string LCStatusSt { get { return EnumObject.jGet(this.LCStatus); } }
        public int UDRcvTypeInt { get { return (int)UDRcvType; } }
        public string UDRcvTypeSt { get { return EnumObject.jGet(this.UDRcvType); } }
        #endregion

        #region Functions
        public static List<ExportLCRegister> Gets(string sSQL, long nUserID)
        {
            return ExportLCRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportLCRegisterService Service
        {
            get { return (IExportLCRegisterService)Services.Factory.CreateService(typeof(IExportLCRegisterService)); }
        }
        #endregion

    }
    #endregion

    #region IExportLCRegister interface

    public interface IExportLCRegisterService
    {
        List<ExportLCRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
