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
    #region CommercialInvoice
    
    public class CommercialInvoice : BusinessObject
    {
        public CommercialInvoice()
        {
            CommercialInvoiceID = 0;
            LCTransferID = 0;
            MasterLCID = 0;
            InvoiceNo = "";
            IsSystemGeneratedInvoiceNo = false;
            InvoiceDate = DateTime.Today;
            InvoiceStatus = EnumCommercialInvoiceStatus.Initialized;
            BuyerID = 0;
            ProductionFactoryID = 0;
            InvoiceAmount = 0;
            DiscountAmount = 0;
            AdditionAmount = 0;
            NetInvoiceAmount = 0;
            Note = "";
            ReceiptNo = "";
            TransportNo = "";
            DriverName = "";
            Carrier = "";
            DeliveryDate = DateTime.Now;
            ApprovedBy = 0;
            GSP =false;
            IC = false;
            BL = false;
            MasterLCNo = "";
            LCStatus = EnumLCStatus.None;
            LCValue = 0;
            CurrencyID = 0;
            ApprovedByName = "";
            TransferNo = "";
            TransferAmount = 0;
            YetToInvoiceAmount = 0;
            LCDate = DateTime.Now;
            BuyerName = "";
            ProductionFactoryName = "";
            InvoiceStatusInInt = 0;
            CommercialInvoiceHistoryID = 0;
            Remark = "";
            VariableDate = DateTime.Now; // It use for multiple case like as SendToBuyerDate, AcceptBuyerDate, EnCashmentDate
            TotalEndrosmentCommission = 0;
            PaymentID = 0;
            PaymentApprovedBy = 0;
            ShipmentMode = EnumTransportType.None;
            ShipmentModeInInt = 0;
            BuyerOrigin = "";
            BuyerAddress = "";
            BuyerEmailAddress = "";
            ApprovedByEmailAddress = "";
            ApprovedByContactNo = "";
            IssueBankName = "";
            AdviceBankName = "";
            CurrencyName = "";
            CurrencySymbol = "";
            bIsChangeField = true;
            BUID = 0;
            ProductNature = EnumProductNature.Buying;
            CIFormat = EnumClientOperationValueFormat.Buying_Format; //only can use buying format or Garments format
            ErrorMessage = "";
            Param = "";
            CommercialInvoices = new List<CommercialInvoice>();
            
        }

        #region Properties         
        public int CommercialInvoiceID { get; set; }         
        public int LCTransferID { get; set; }         
        public int MasterLCID { get; set; }
        public int BUID { get; set; }
        public string InvoiceNo { get; set; }         
        public bool IsSystemGeneratedInvoiceNo { get; set; }         
        public DateTime InvoiceDate { get; set; }         
        public EnumCommercialInvoiceStatus InvoiceStatus { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerEmailAddress { get; set; }
        public string  ApprovedByEmailAddress {get;set;}
        public string  ApprovedByContactNo {get;set;}
        public string   IssueBankName {get;set;}
        public string AdviceBankName { get; set; }
        public int BuyerID { get; set; }
        public int ProductionFactoryID { get; set; }
        public double InvoiceAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double AdditionAmount { get; set; }

        public double NetInvoiceAmount { get; set; }
        public string BuyerOrigin { get; set; }
        public string Note { get; set; }
        public string CurrencyName {get;set;}
        public string CurrencySymbol { get; set; }
        public string ReceiptNo { get; set; }         
        public string TransportNo { get; set; }         
        public string DriverName { get; set; }         
        public string Carrier { get; set; }         
        public DateTime DeliveryDate { get; set; }         
        public int ApprovedBy { get; set; }
        public bool GSP { get; set; }         
        public bool IC { get; set; }
        public bool BL { get; set; }         
       
        public string MasterLCNo { get; set; }         
        public EnumLCStatus LCStatus { get; set; }         
        public double LCValue { get; set; }         
        public string ApprovedByName { get; set; }         
        public string TransferNo { get; set; }         
        public double TransferAmount { get; set; }         
        public string BuyerName { get; set; }         
        public string ProductionFactoryName { get; set; }         
        public double InvoiceQty { get; set; }         
        public double YetToInvoiceAmount { get; set; }         
        public double TotalEndrosmentCommission { get; set; }         
        public DateTime LCDate { get; set; }               
        public string AdviceBankAccount { get; set; }         
        public int CommercialInvoiceHistoryID { get; set; }         
        public int PaymentID { get; set; }         
        public int PaymentApprovedBy { get; set; }         
        public DateTime VariableDate { get; set; }         
        public EnumTransportType ShipmentMode { get; set; }
        public double AnnualBonus { get; set; }   
        public int ShipmentModeInInt { get; set; }         
        public string Remark { get; set; }
        public int CurrencyID { get; set; }
        public string ErrorMessage { get; set; }
        public EnumClientOperationValueFormat CIFormat { get; set; }
        public int CIFormatInInt { get; set; }
        public EnumProductNature ProductNature { get; set; }
        #endregion

        #region Derived Property 
        public string Param { get; set; }
        public int CommercialInvoiceDetailID { get; set; }//tempoaraty use
        public int InvoiceStatusInInt { get; set; }         
        public string ActionTypeInString { get; set; }         
        public List<CommercialInvoiceHistory> CommercialInvoiceHistories { get; set; }         
        public List<CommercialInvoiceDetail> CommercialInvoiceDetails { get; set; }
        public List<CommercialInvoice> CommercialInvoices { get; set; }
        public ExportDocSetup ExportDocSetup { get; set; }
        //public List<DocumentType> DocumentTypes { get; set; }
        public List<ReportLayout> ReportLayouts { get; set; }
        public MasterLC MasterLC { get; set; }  
        public Company Company { get; set; }
        public bool bIsChangeField { get; set; }
        public string ShipmentModeInString
        {
            get
            {
                return this.ShipmentMode.ToString();
            }
        }
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
        public string PaymentStatusInString
        {
            get
            {
                if(this.PaymentID>0)
                {
                    if (this.PaymentApprovedBy != 0)
                    {
                        return "Approve Done";
                    }else
                    {
                    
                    return "Execution Done";
                    }
                }else
                {
                    return "";
                }
            }
        }

        public string DeliveryDateInString
        {
            get
            {
                return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }

        public string VariableDateInString
        {
            get
            {
                return this.VariableDate.ToString("dd MMM yyyy");
            }
        }

     
        public string LCDateInString
        {
            get
            {

                    return this.LCDate.ToString("dd MMM yyyy");
                
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
          public string AnnualBonusInPercentageInSt
        {
            get
            {
                if (this.AnnualBonus > 0 && this.InvoiceAmount > 0)
                {
                    return Global.MillionFormat((this.AnnualBonus * 100) / this.InvoiceAmount);
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
        #endregion

        #region Functions

        public static List<CommercialInvoice> Gets(long nUserID)
        {
            return CommercialInvoice.Service.Gets(nUserID);
        }
        public static List<CommercialInvoice> GetsByTransfer(int id , long nUserID)
        {
            return CommercialInvoice.Service.GetsByTransfer(id, nUserID);
        }

        public static List<CommercialInvoice> GetsByLC(int id, long nUserID)
        {
            return CommercialInvoice.Service.GetsByLC(id, nUserID);
        }

        public static List<CommercialInvoice> Gets(string sSQL, long nUserID)
        {
            return CommercialInvoice.Service.Gets(sSQL, nUserID);
        }
        public CommercialInvoice Get(int id, long nUserID)
        {
            return CommercialInvoice.Service.Get(id, nUserID);
        }

        public CommercialInvoice Save(long nUserID)
        {
            return CommercialInvoice.Service.Save(this, nUserID);
        }
        public CommercialInvoice Approval(long nUserID)
        {
            return CommercialInvoice.Service.Approval(this, nUserID);
        }
        public CommercialInvoice ChangeField( long nUserID)//GSP,BL,IC  changing Function
        {
            return CommercialInvoice.Service.ChangeField(this,  nUserID);
        }
        public string Delete(int nCommercialInvoiceID, long nUserID)
        {

            return CommercialInvoice.Service.Delete(nCommercialInvoiceID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICommercialInvoiceService Service
        {
            get { return (ICommercialInvoiceService)Services.Factory.CreateService(typeof(ICommercialInvoiceService)); }
        }

        #endregion
    }
    #endregion

    #region ICommercialInvoice interface
     
    public interface ICommercialInvoiceService
    {
         
        CommercialInvoice Get(int id, Int64 nUserID);
         
        List<CommercialInvoice> Gets(Int64 nUserID);

        List<CommercialInvoice> GetsByTransfer(int id, Int64 nUserID);
        List<CommercialInvoice> GetsByLC(int id, Int64 nUserID);         
        List<CommercialInvoice> Gets(string sSQL, Int64 nUserID);
         
        
        CommercialInvoice Save(CommercialInvoice oCommercialInvoice, Int64 nUserID);
        CommercialInvoice Approval(CommercialInvoice oCommercialInvoice, Int64 nUserID);
        CommercialInvoice ChangeField(CommercialInvoice oCommercialInvoice, Int64 nUserID);
         
        string Delete(int nCommercialInvoiceID, Int64 nUserID);
    }
    #endregion
}
