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
    #region ExportBillPending
    public class ExportBillPending : BusinessObject
    {
        public ExportBillPending()
        {
            ExportPIDetailID = 0;
            ExportPIID = 0;
            ProductID = 0;
            MUnitID = 0;
            PIQty = 0;
            UnitPrice = 0;
            PIAmount = 0;
            DeliveryQty = 0;
            DeliveryAmount = 0;
            BillQty = 0;
            BillAmount = 0;
            BillPendingQty = 0;
            BillPendingAmount = 0;

            ExportLCID = 0;
            BUID = 0;
            BUType = EnumBusinessUnitType.None;
            ExportLCNo = "";
            ExportLCType = EnumExportLCType.None;
            ApplicantID = 0;
            OpeningDate = DateTime.MinValue;
            NegoBankBranchID = 0;
            CurrencyID = 0;
            LCAmount = 0;
            ShipmentDate = DateTime.MinValue;
            LastDeliveryDate = DateTime.MinValue;

            PINo = "";
            ApplicantName = "";
            MUnitSymbol = "";
            BUName = "";
            BankName = "";
            CurrencySymbol = "";
            ProductName = "";
            ErrorMessage = "";
            ReportLayout = EnumReportLayout.None;
            ReportLayoutInt = 0;
        }

        #region Properties
        public int ExportPIDetailID { get; set; }
        public int ExportPIID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double PIQty { get; set; }
        public double UnitPrice { get; set; }
        public double PIAmount { get; set; }
        public double DeliveryQty { get; set; }
        public double DeliveryAmount { get; set; }
        public double BillQty { get; set; }
        public double BillAmount { get; set; }
        public double BillPendingQty { get; set; }
        public double BillPendingAmount { get; set; }

        public int ExportLCID { get; set; }
        public int BUID { get; set; }
        public EnumBusinessUnitType BUType { get; set; }
        public string ExportLCNo { get; set; }
        public EnumExportLCType ExportLCType { get; set; }
        public int ApplicantID { get; set; }
        public DateTime OpeningDate { get; set; }
        public int NegoBankBranchID { get; set; }
        public int CurrencyID { get; set; }
        public double LCAmount { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime LastDeliveryDate { get; set; }

        public string PINo { get; set; }
        public string ApplicantName { get; set; }
        public string MUnitSymbol { get; set; }
        public string BUName { get; set; }
        public string BankName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ProductName { get; set; }
        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public int ReportLayoutInt { get; set; }
        #endregion

        #region Derived Property
        public string OpeningDateSt
        {
            get
            {
                if (this.OpeningDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.OpeningDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ShipmentDateSt
        {
            get
            {
                if (this.ShipmentDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string LastDeliveryDateSt
        {
            get
            {
                if (this.LastDeliveryDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.LastDeliveryDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string ExportLCTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ExportLCType);
            }
        }
        public string BUTypeSt
        {
            get
            {
                return EnumObject.jGet(this.BUType);
            }
        }
        public string PIQtySt
        {
            get
            {
                return this.PIQty.ToString("#,##0.00") + " " + this.MUnitSymbol;
            }
        }
        public string DeliveryQtySt
        {
            get
            {
                return this.DeliveryQty.ToString("#,##0.00") + " " + this.MUnitSymbol;
            }
        }
        public string BillQtySt
        {
            get
            {
                return this.BillQty.ToString("#,##0.00") + " " + this.MUnitSymbol;
            }
        }
        public string BillPendingQtySt
        {
            get
            {
                return this.BillPendingQty.ToString("#,##0.00") + " " + this.MUnitSymbol;
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return this.CurrencySymbol + " " + this.UnitPrice.ToString("#,##0.00");
            }
        }
        public string PIAmountSt
        {
            get
            {
                return this.CurrencySymbol + " " + this.PIAmount.ToString("#,##0.00");
            }
        }
        public string DeliveryAmountSt
        {
            get
            {
                return this.CurrencySymbol + " " + this.DeliveryAmount.ToString("#,##0.00");
            }
        }
        public string BillAmountSt
        {
            get
            {
                return this.CurrencySymbol + " " + this.BillAmount.ToString("#,##0.00");
            }
        }
        public string BillPendingAmountSt
        {
            get
            {
                return this.CurrencySymbol + " " + this.BillPendingAmount.ToString("#,##0.00");
            }
        }
        #endregion

        #region Functions
        public static List<ExportBillPending> Gets(string sSQL, EnumReportLayout eEnumReportLayout, long nUserID)
        {
            return ExportBillPending.Service.Gets(sSQL, eEnumReportLayout, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportBillPendingService Service
        {
            get { return (IExportBillPendingService)Services.Factory.CreateService(typeof(IExportBillPendingService)); }
        }
        #endregion
    }
    #endregion

    #region IExportBillPending interface

    public interface IExportBillPendingService
    {
        List<ExportBillPending> Gets(string sSQL, EnumReportLayout eEnumReportLayout,  Int64 nUserID);
    }
    #endregion
}