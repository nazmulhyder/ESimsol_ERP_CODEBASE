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
    #region ProformaInvoice    
    public class ProformaInvoice : BusinessObject
    {
        public ProformaInvoice()
        {
            ProformaInvoiceID = 0;
            BUID = 0;
            PINo = "";
            IssueDate = DateTime.Now;
            PIStatus = EnumPIStatus.Initialized;
            PIStatusInInt = (int)EnumPIStatus.Initialized;
            BuyerID = 0;
            BuyerName = "";
            LCFavorOf = 0;
            LCFavorOfName = "";
            TransferBankAccountID = 0;
            UnitID = 0;
            UnitName = "";
            CurrencyID = 0;
            CurrencyName = "";
            PaymentTerm = EnumPaymentTerm.None;
            Origin = "";
            DeliveryTerm = EnumDeliveryTerm.None;
            DeliveryTermInInt = 0;
            PortOfLoadingAir = "";
            PortOfLoadingSea = "";
            Note = "";
            ApprovedBy = 0;
            ApprovedByName = "";
            LCReceived = "";
            ProformaInvoiceActionType = EnumProformaInvoiceActionType.None;
            ProformaInvoiceHistoryID = 0;
            Quantity = 0;            
            TransferingBankName = "";
            CurrencySymbol = "";
            UnitSymbol = "";
            ProformaInvoiceLogID = 0;
            BuyerAddress = "";
            BuyerPhone = "";
            BuyerFax = "";
            BankName = "";
            SwiftCode = "";
            AccountNo = "";
            BranchName = "";
            BranchAddress = "";
            ErrorMessage = "";
            SessionName = "";
            ApplicantID = 0;
            ApplicantName = "";
            GrossAmount = 0;
            DiscountAmount = 0;
            AdditionalAmount = 0;
            NetAmount = 0;
            CauseOfAddition = "";
            CauseOfDiscount = "";
            ReviseRequest = new ReviseRequest();
            ApprovalRequest = new ApprovalRequest();
            ProformaInvoiceDetails = new List<ProformaInvoiceDetail>();
            ProformaInvoiceRequiredDocs = new List<ProformaInvoiceRequiredDoc>();
            ProformaInvoiceTermsAndConditions = new List<ProformaInvoiceTermsAndCondition>();
        }

        #region Properties
        public int ProformaInvoiceID { get; set; }
        public int BUID { get; set; }
        public int ProformaInvoiceLogID { get; set; }         
        public string SessionName { get; set; }         
        public EnumPIStatus PIStatus { get; set; }         
        public string PINo { get; set; }         
        public string CurrencySymbol { get; set; }         
        public string UnitSymbol { get; set; }         
        public DateTime IssueDate { get; set; }         
        public int BuyerID { get; set; }         
        public string BuyerName { get; set; }         
        public int LCFavorOf { get; set; }         
        public string LCFavorOfName { get; set; }         
        public int TransferBankAccountID { get; set; }         
        public int UnitID { get; set; }         
        public string UnitName { get; set; }         
        public int CurrencyID { get; set; }         
        public string CurrencyName { get; set; }         
        public EnumPaymentTerm PaymentTerm { get; set; }         
        public string Origin { get; set; }         
        public EnumDeliveryTerm DeliveryTerm { get; set; }         
        public string PortOfLoadingAir { get; set; }         
        public string PortOfLoadingSea { get; set; }         
        public string Note { get; set; }         
        public int ApprovedBy { get; set; }         
        public string ApprovedByName { get; set; }         
        public string LCReceived { get; set; }         
        public int ProformaInvoiceHistoryID { get; set; }         
        public double Quantity { get; set; }
        public string TransferingBankName { get; set; }         
        public string BuyerAddress { get; set; }         
        public string BuyerPhone { get; set; }         
        public string BuyerFax { get; set; }         
        public string BankName { get; set; }         
        public string SwiftCode { get; set; }         
        public string AccountNo { get; set; }         
        public string BranchName { get; set; }         
        public string BranchAddress { get; set; }         
        public EnumProformaInvoiceActionType ProformaInvoiceActionType { get; set; }        
         public int   ApplicantID { get; set; }
         public string  ApplicantName { get; set; }
         public double   GrossAmount { get; set; }
         public double   DiscountAmount { get; set; }
         public double AdditionalAmount { get; set; }
         public double NetAmount { get; set; }
        public string CauseOfAddition { get; set; }
        public string CauseOfDiscount { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         public string DiscountInPercentageInSt
        {
            get
            {
                if (this.DiscountAmount>0 && this.GrossAmount>0)
                {
                    return Global.MillionFormat((this.DiscountAmount * 100) / this.GrossAmount);
                }
                else
                {
                    return "0.00";
                }
                
            }
        }
         public string AdditionalInPercentageInSt
         {
             get
             {
                 if (this.AdditionalAmount > 0 && this.GrossAmount > 0)
                 {
                     return Global.MillionFormat((this.AdditionalAmount * 100) / this.GrossAmount);
                 }
                 else
                 {
                     return "0.00";
                 }

             }
         }         
        public List<ProformaInvoice> ProformaInvoiceList { get; set; }
        public List<ProformaInvoiceHistory> ProformaInvoiceHistories { get; set; }
        public List<BusinessSession> BusinessSessions { get; set; }                 
        public List<Employee> Employees { get; set; }         
        public List<VOrder> SaleOrderList { get; set; }         
        public List<User> Users { get; set; }         
        public List<ProformaInvoiceDetail> ProformaInvoiceDetails { get; set; }         
        public List<ProformaInvoiceRequiredDoc> ProformaInvoiceRequiredDocs { get; set; }         
        public List<ProformaInvoiceTermsAndCondition> ProformaInvoiceTermsAndConditions { get; set; }                   
        public Company Company { get; set; }         
        public ContactPersonnel ContactPersonnel { get; set; }         
        public ApprovalRequest ApprovalRequest { get; set; }
        public ReviseRequest ReviseRequest { get; set; }         
        public Contractor Applicant { get; set; }
        public int OperationBy { get; set; }         
        public int PIStatusInInt { get; set; }         
        public int PaymentTermInInt { get; set; }
        public int DeliveryTermInInt { get; set; }
        public string ActionTypeExtra { get; set; }
        public string PinCode { get; set; }
        public string sNote { get; set; }
        public string PIStatusInString
        {
            get
            {
                return this.PIStatus.ToString();
            }
        }
        public string IssueDateInString
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");

            }
        }
        public string PaymentTermInString
        {
            get
            {
                return EnumObject.jGet(this.PaymentTerm);
            }

        }
        public string DeliveryTermInString
        {
            get
            {
                return EnumObject.jGet(this.DeliveryTerm);
            }

        }
        #endregion

        #region Functions

        public static List<ProformaInvoice> Gets(int buid, long nUserID)
        {
            return ProformaInvoice.Service.Gets(buid, nUserID);
        }
        public static List<ProformaInvoice> GetsPILog(int id, long nUserID) // id is PI ID
        {
            return ProformaInvoice.Service.GetsPILog(id, nUserID);
        }
        public static List<ProformaInvoice> Gets(string sSQL, long nUserID)
        {
            return ProformaInvoice.Service.Gets(sSQL, nUserID);
        }
        public ProformaInvoice Get(int id, long nUserID)
        {
            return ProformaInvoice.Service.Get(id, nUserID);
        }
        public ProformaInvoice GetLog(int id, long nUserID) // id is PI Log ID
        {
            return ProformaInvoice.Service.GetLog(id, nUserID);
        }
        public ProformaInvoice Save(long nUserID)
        {
            return ProformaInvoice.Service.Save(this, nUserID);
        }
        public ProformaInvoice AcceptProformaInvoiceRevise(long nUserID)
        {
            return ProformaInvoice.Service.AcceptProformaInvoiceRevise(this, nUserID);
        }
        public ProformaInvoice ChangeStatus( long nUserID)
        {
            return ProformaInvoice.Service.ChangeStatus(this, nUserID);
        }
        public string Delete(int nProformaInvoiceID, long nUserID)
        {
            return ProformaInvoice.Service.Delete(nProformaInvoiceID, nUserID);
        }
        #endregion

        #region ServiceFactory
 
        internal static IProformaInvoiceService Service
        {
            get { return (IProformaInvoiceService)Services.Factory.CreateService(typeof(IProformaInvoiceService)); }
        }

        #endregion
    }
    #endregion

    #region IProformaInvoice interface
    public interface IProformaInvoiceService
    {
        ProformaInvoice Get(int id, Int64 nUserID);
        ProformaInvoice GetLog(int id, Int64 nUserID);
        List<ProformaInvoice> Gets(int buid, Int64 nUserID);
        List<ProformaInvoice> GetsPILog(int id, Int64 nUserID);
        List<ProformaInvoice> Gets(string sSQL, Int64 nUserID);
        ProformaInvoice Save(ProformaInvoice oProformaInvoice, Int64 nUserID);
        ProformaInvoice AcceptProformaInvoiceRevise(ProformaInvoice oProformaInvoice, Int64 nUserID);
        ProformaInvoice ChangeStatus(ProformaInvoice oProformaInvoice, Int64 nUserID);
        string Delete(int nProformaInvoiceID, Int64 nUserID);
    }
    #endregion
}
