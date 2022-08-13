using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region CommercialInvoiceRegister
    public class CommercialInvoiceRegister : BusinessObject
    {
        public CommercialInvoiceRegister()
        {
            CommercialInvoiceDetailID = 0;
            CommercialInvoiceID = 0;
            ReferenceDetailID = 0;
            TechnicalSheetID = 0;
            OrderRecapID = 0;
            ShipmentDate = DateTime.Now;
            InvoiceQty = 0; // invoice detail qty
            FOB = 0.0;
            Discount = 0.0;
            UnitPrice = 0.0;
            Amount = 0.0;
            InvoiceNo = "";
            MasterLCID = 0;
            MasterLCNo = "";
            InvoiceDate = DateTime.Now;
            InvoiceStatus = EnumCommercialInvoiceStatus.Initialized;
            BuyerID = 0;
            InvoiceAmount = 0;
            DiscountAmount = 0;
            AdditionAmount = 0;
            ShipmentMode = EnumTransportType.None;
            DiscrepancyCharge = 0;
            NetInvoiceAmount = 0;
      
            OrderRecapNo = "";
            StyleNo = "";
            MUnit = "";
            GSP = false;
            IC = false;
            BL = false;
            BUID = 0;
            BuyerName = "";
            BUName = "";
            InvoiceStatusInInt = 0;
            AdviceBankName = "";
            AdviceBankAccount = "";
            Qty = 0.0; // invoice qty
            ApprovedBy = 0;
            CartonQty = 0;
            CurrencySymbol = "";
            ApprovedByName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int CommercialInvoiceDetailID { get; set; }
        public int CommercialInvoiceID { get; set; }
        public int ReferenceDetailID { get; set; }
        public int TechnicalSheetID { get; set; }
        public int OrderRecapID { get; set; }
        public DateTime ShipmentDate { get; set; }
        public double InvoiceQty { get; set; }
        public double FOB { get; set; }
        public EnumTransportType ShipmentMode { get; set; }         
        public double Discount { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string InvoiceNo { get; set; }
        public int MasterLCID { get; set; }
        public string MasterLCNo { get; set; }
        public string CurrencySymbol { get; set; }
        public bool GSP { get; set; }
        public bool IC { get; set; }
        public bool BL { get; set; }     
        public DateTime InvoiceDate { get; set; }
        public EnumCommercialInvoiceStatus InvoiceStatus { get; set; }
        public int BuyerID { get; set; }
        public double InvoiceAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double AdditionAmount { get; set; }
        public double DiscrepancyCharge { get; set; }
        public double NetInvoiceAmount { get; set; }
        public double CartonQty { get; set; }
        
        public string OrderRecapNo { get; set; }
        public string StyleNo { get; set; }
        public string MUnit { get; set; }
        public int BUID { get; set; }
        public string BuyerName { get; set; }
        public string AdviceBankName { get; set; }
        public string AdviceBankAccount { get; set; }
        public double Qty { get; set; }
        public string BUName { get; set; }
        public int ApprovedBy { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string SearchingData { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public string ApprovedByName { get; set; }
        public string AmountSt
        {
            get
            {
                return this.CurrencySymbol+""+Global.MillionFormat(this.Amount, 2);
            }
        }
        public int InvoiceStatusInInt { get; set; }
        public string InvoiceStatusInString
        {
            get
            {
                return EnumObject.jGet(this.InvoiceStatus);
            }
        }
        public string InvoiceDateInString
        {
            get
            {
                return this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentModeInString
        {
            get
            {
                return this.ShipmentMode.ToString();
            }
        }
        public string ShipmentDateInString
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string GSPST
        {
            get
            {
                if (this.GSP)
                {
                    return "Yes";
                }
                else
                {

                    return "No";
                }
            }
        }
        public string ICST
        {
            get
            {
                if (this.IC)
                {
                    return "Yes";
                }
                else
                {

                    return "No";
                }
            }
        }
        public string BLST
        {
            get
            {
                if (this.BL)
                {
                    return "Yes";
                }
                else
                {

                    return "No";
                }
            }
        }
        public string DiscountInPercentageInSt
        {
            get
            {
                if (this.DiscountAmount > 0 && this.InvoiceAmount > 0)
                {
                    return Global.MillionFormat((this.DiscountAmount * 100) / this.InvoiceAmount);
                }
                else
                {
                    return "0.00";
                }

            }
        }
        public string AdditionInPercentageInSt
        {
            get
            {
                if (this.AdditionAmount > 0 && this.InvoiceAmount > 0)
                {
                    return Global.MillionFormat((this.AdditionAmount * 100) / this.InvoiceAmount);
                }
                else
                {
                    return "0.00";
                }
            }
        }
     

        public string SendingMonth
        {
            get
            {
                if (this.InvoiceDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {

                    return this.InvoiceDate.ToString("MMMM");
                }
            }
        }

      
        #endregion

        #region Functions
        public static List<CommercialInvoiceRegister> Gets(string sSQL, long nUserID)
        {
            return CommercialInvoiceRegister.Service.Gets(sSQL, nUserID);
        }
        public static CommercialInvoiceRegister Get(int nCIDetailID, long nUserID)
        {
            return CommercialInvoiceRegister.Service.Get(nCIDetailID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICommercialInvoiceRegisterService Service
        {
            get { return (ICommercialInvoiceRegisterService)Services.Factory.CreateService(typeof(ICommercialInvoiceRegisterService)); }
        }
        #endregion
    }

    #region ICommercialInvoiceRegister interface
    public interface ICommercialInvoiceRegisterService
    {
        List<CommercialInvoiceRegister> Gets(string sSQL, Int64 nUserID);
        CommercialInvoiceRegister Get(int nCIDetailID, Int64 nUserID);
    }
    #endregion

    #endregion
}
