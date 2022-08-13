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

    #region OrderRecapMgtReport

    public class OrderRecapMgtReport : BusinessObject
    {
        public OrderRecapMgtReport()
        {

            OrderRecapID = 0;
            OrderRecapNo = "";
            TechnicalSheetID = 0;
            MerchandiserID = 0;
            ShipmentDate = DateTime.Now;
            OrderDate = DateTime.Now;
            BuyerID = 0;
            FactoryID = 0;
            ProductCategoryID = 0;
            StyleNo = "";
            BuyerName = "";
            FactoryName = "";
            MerchandiserName = "";
            ProductCategoryName = "";
            FOB = 0;
            ONSQty = 0;
            ODSQty = 0;
            CMValue = 0;
            OrderQty = 0;
            FabricID = 0;
            AgentID = 0;
            AgentName = "";
            FabricName = "";
            StyleInputDate = DateTime.Now;
            SeasonName = "";
            GarmentsName = "";
            ContactPersonName = "";
            ColorRange = "";
            SizeRange = "";
            CuttingQty = 0;
            SweeingQty = 0;
            ShipmentQty = 0;
            Remarks = "";
            FactoryFOB = 0;
            MasterLCNo = "";
            LCTransferNo = "";
            Dept = 0;
            DeptName = "";
            SubGender = EnumSubGender.None;
            PIPrice = 0;
            LCShipmentDate = DateTime.MinValue;
            CurrencySymbol = "";
            FactoryShipmentDate = DateTime.MinValue;
            ErrorMessage = "";
        }

        #region Properties
        public int OrderRecapID { get; set; }
        public string OrderRecapNo { get; set; }
        public int TechnicalSheetID { get; set; }
        public int MerchandiserID { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductCategoryID { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string FactoryName { get; set; }
        public string MerchandiserName { get; set; }
        public string ProductCategoryName { get; set; }
        public double FOB { get; set; }
        public double ONSQty { get; set; }
        public double ODSQty { get; set; }
        public double CMValue { get; set; }
        public double OrderQty { get; set; }
        public int BuyerID { get; set; }
        public int FactoryID { get; set; }
        public int FabricID { get; set; }
        public string FabricName { get; set; }
        public int AgentID { get; set; }
        public string AgentName { get; set; }
        public DateTime StyleInputDate { get; set; }
        public string SeasonName { get; set; }
        public string GarmentsName { get; set; }
        public string ContactPersonName { get; set; }
        public string ColorRange { get; set; }
        public string SizeRange { get; set; }
        public double CuttingQty { get; set; }
        public double SweeingQty { get; set; }
        public double ShipmentQty { get; set; }
        public string Remarks { get; set; }
        public double FactoryFOB { get; set; }
        public string MasterLCNo { get; set; }
        public string LCTransferNo { get; set; }
        public int Dept { get; set; }
        public string DeptName { get; set; }
        public EnumSubGender SubGender { get; set; }
        public double PIPrice { get; set; }
        public DateTime LCShipmentDate { get; set; }
        public string CurrencySymbol { get; set; }
        public DateTime FactoryShipmentDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public System.Drawing.Image StyleCoverImage { get; set; }
        public List<OrderRecapMgtReport> OrderRecapMgtReports { get; set; }

        public double TotalAmount
        {
            get
            {
                return this.OrderQty * this.FOB;
            }
        }
      
        public double TotalFactoryAmount
        {
            get
            {
                return this.FactoryFOB * this.OrderQty;
            }
        }
        public double CommissionPerPc
        {
            get
            {
                return this.FOB - this.FactoryFOB;
            }
        }
        public double CommissionInPercent
        {
            get
            {
                return (this.CommissionPerPc / this.FOB) * 100;
            }
        }
        public double TotalCommission
        {
            get
            {
                return this.TotalAmount - this.TotalFactoryAmount;
            }
        }

        public string ShipmentDateInString
        {
            get
            {
                if (this.ShipmentDate == DateTime.MinValue)
                {
                    return " ";
                }
                else
                {
                    return this.ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string OrderDateInString
        {
            get
            {
                if (this.OrderDate == DateTime.MinValue)
                {
                    return " ";
                }
                else
                {
                    return this.OrderDate.ToString("dd MMM yyyy");
                }

            }
        }
        public string LCShipmentDateSt
        {
            get
            {
                if (this.LCShipmentDate == DateTime.MinValue)
                {
                    return " ";
                }
                else
                {
                    return this.LCShipmentDate.ToString("dd MMM yyyy");
                }

            }
        }
        public string FactoryShipmentDateSt
        {
            get
            {
                if (this.FactoryShipmentDate == DateTime.MinValue)
                {
                    return " ";
                }
                else
                {
                    return this.FactoryShipmentDate.ToString("dd MMM yyyy");
                }

            }
        }
        public double FactoryPrice
        {
            get
            {
                double nFactoryPrice = 0;
                if (this.CMValue > 0)
                {
                    nFactoryPrice = (this.CMValue / 12);
                }
                return nFactoryPrice;
            }
        }
        public Company Company { get; set; }
        #endregion


        #region Functions
        public static List<OrderRecapMgtReport> Gets(string sSql, int ReportFormat, long nUserID)
        {
            return OrderRecapMgtReport.Service.Gets(sSql, ReportFormat, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IOrderRecapMgtReportService Service
        {
            get { return (IOrderRecapMgtReportService)Services.Factory.CreateService(typeof(IOrderRecapMgtReportService)); }
        }

        #endregion
    }

    #endregion

    #region IOrderRecapMgtReport interface

    public interface IOrderRecapMgtReportService
    {
        List<OrderRecapMgtReport> Gets(string sSql, int ReportFormat, Int64 nUserID);

    }
    #endregion
}
