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
    #region PreInvoice

    public class PreInvoice : BusinessObject
    {
        public PreInvoice()
        {
            PreInvoiceID = 0;
            InvoiceNo = "";
            InvoiceDate = DateTime.Now;
            ContractorID = 0;
            ContactPersonID = 0;
            SalesQuotationID = 0;
            IsNewOrder = false;
            VehicleLocation = 0;
            PRNo = "";
            KommNo = "";
            BankName = "";
            SpecialInstruction = "";
            ETAAgreement = "";
            ETAWeeks = "";
            CurrencyID = 0;
            OTRAmount = 0;
            VatAmount = 0;
            RegistrationFee = 0;
            TDSAmount = 0;
            DiscountAmount = 0;
            NetAmount = 0;
            AdvanceAmount = 0;
            AdvanceDate = DateTime.Now;
            PaymentMode = 0;
            ChequeNo = "";
            MarketingAccountName = "";
            ChequeDate = DateTime.Now;
            BankID = 0;
            ReceivedBy = 0;
            ReceivedByName = "";
            Remarks = "";
            AttachmentDoc = 0;
            ApprovedBy = 0;
            InteriorColorCode = "";
            InteriorColorName = "";
            ExteriorColorCode = "";
            ExteriorColorName = "";
            EngineNo = "";
            ChassisNo = "";
            ModelNo = "";
            SQNo = "";
            SalesQuotationImageID = 0;
            CustomerName = "";
            CustomerAddress = "";
            CustomerCity = "";
            CustomerLandline = "";
            CustomerCellPhone = "";
            CurrencyName = "";
            CurrencySymbol = "";
            ReceivedOn = "";
            ApprovedByName = "";
            HandoverDate = DateTime.Now;
            VehicleMileage = 0;
            WheelCondition = "";
            BodyWorkCondition= "";
            InteriorCondition= "";
            DealerPerson= "";
            Owner= "";
            OwnerNID= "";
            CustomerEmail = "";
            CustomerShortName = "";
            Note = "";
            YearOfManufacture = "";
            YearOfModel = "";
            ETAValue = 0;
            ETAType = EnumDisplayPart.None;
            IssueDate = DateTime.Now;
            ServiceSchedules = new List<ServiceSchedule>();
            OfferedFreeService = 0;
        }

        #region Properties
        public int PreInvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public string SQNo { get; set; }
        public string KommNo { get; set; }
        public int MarketingAccountID { get; set; }
        public string MarketingAccountName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonID { get; set; }
        public int SalesQuotationID { get; set; }
        public bool IsNewOrder { get; set; }
        public int VehicleLocation { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PRNo { get; set; }
        public string BankName { get; set; }
        public string SpecialInstruction { get; set; }
        public string ETAAgreement { get; set; }
        public string ETAWeeks { get; set; }
        public int CurrencyID { get; set; }
        public double OTRAmount { get; set; }
        public double VatAmount { get; set; }
        public double RegistrationFee { get; set; }
        public double TDSAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double NetAmount { get; set; }
        public double AdvanceAmount { get; set; }
        public DateTime AdvanceDate { get; set; }
        public int PaymentMode { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public int BankID { get; set; }
        public int ReceivedBy { get; set; }
        public string ReceivedByName { get; set; }
        public string ReceivedOn { get; set; }
        public string Remarks { get; set; }
        public int AttachmentDoc { get; set; }
        public int ApprovedBy { get; set; }
        public string InteriorColorCode { get; set; }
        public string InteriorColorName { get; set; }
        public string ExteriorColorCode { get; set; }
        public string ExteriorColorName { get; set; }
        public string EngineNo { get; set; }
        public string ChassisNo { get; set; }
        public string ModelNo { get; set; }
        public int VehicleModelID { get; set; }
        public int SalesQuotationImageID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerLandline { get; set; }
        public string CustomerCellPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerShortName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ApprovedByName { get; set; }
        public EnumPreInvoiceStatus InvoiceStatus { get; set; }
        public int BUID { get; set; }
        public double CRate { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }

        public string ModelShortName { get; set; }

        public DateTime HandoverDate { get; set; }
        public int VehicleMileage { get; set; }
        public string WheelCondition { get; set; }
        public string BodyWorkCondition { get; set; }
        public string InteriorCondition { get; set; }
        public string DealerPerson { get; set; }
        public string Owner { get; set; }
        public string OwnerNID { get; set; }
        public string Note { get; set; }
        public int OfferedFreeService { get; set; }

        public string YearOfModel { get; set; }
        public string YearOfManufacture { get; set; }
        #endregion

        #region Derived Property
        public int ETAValue { get; set; }
        public EnumDisplayPart ETAType { get; set; }
        public DateTime IssueDate { get; set; }
        public string FirstTerm { get; set; }
        public string SecondTerm { get; set; }
        public System.Drawing.Image SideImage { get; set; }
        public System.Drawing.Image TopImage { get; set; }
        public System.Drawing.Image FrontImage { get; set; }
        public System.Drawing.Image BackImage { get; set; }
        public SalesQuotation SalesQuotation { get; set; }
        public List<ServiceSchedule> ServiceSchedules { get; set; }
        public string PossibleDateInString
        {
            get
            {
                if (this.ETAType == EnumDisplayPart.Day)
                {
                    return this.IssueDate.AddDays(this.ETAValue).ToString("dd MMM yyyy");
                }
                else if (this.ETAType == EnumDisplayPart.Week)
                {
                    return this.IssueDate.AddDays(this.ETAValue * 7).ToString("dd MMM yyyy");
                }
                else if (this.ETAType == EnumDisplayPart.Month)
                {
                    return this.IssueDate.AddMonths(this.ETAValue).ToString("dd MMM yyyy");
                }
                else if (this.ETAType == EnumDisplayPart.Year)
                {
                    return this.IssueDate.AddYears(this.ETAValue).ToString("dd MMM yyyy");
                }
                else
                {
                    return "";
                }

            }
        }
        public string AdvanceDateST
        {
            get { return this.AdvanceDate.ToString("dd MMM yyyy"); }
        }
        public string ChequeDateST
        {
            get { return this.ChequeDate.ToString("dd MMM yyyy"); }
        }
        public string HandoverDateST
        {
            get 
            {
                if (HandoverDate == DateTime.MinValue) return DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
                else if (HandoverDate == DateTime.Parse("01 Jan 1900 12:00 AM")) return DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
                return this.HandoverDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string InvoiceDateST
        {
            get { return this.InvoiceDate.ToString("dd MMM yyyy"); }
        }
        public int InvoiceStatusInt
        {
            get { return (int)(this.InvoiceStatus); }
        }
        public string InvoiceStatusST
        {
            get { return EnumObject.jGet(this.InvoiceStatus); }
        }
        #endregion

        #region Functions
        public static List<PreInvoice> Gets(long nUserID)
        {
            return PreInvoice.Service.Gets(nUserID);
        }

        public PreInvoice Get(int id, long nUserID)
        {
            return PreInvoice.Service.Get(id, nUserID);
        }

        public PreInvoice Save(long nUserID)
        {
            return PreInvoice.Service.Save(this, nUserID);
        }
        public static List<PreInvoice> Gets(string sSQL, long nUserID)
        {
            return PreInvoice.Service.Gets(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PreInvoice.Service.Delete(id, nUserID);
        }

        public string UpdateStatus(PreInvoice oServiceOrder, long nUserID)
        {
            return PreInvoice.Service.UpdateStatus(oServiceOrder, nUserID);
        }

        public PreInvoice UpdateForHandoverCheckList(long nUserID)
        {
            return PreInvoice.Service.UpdateForHandoverCheckList(this, nUserID);
        }
        public PreInvoice SaveAll(List<ServiceSchedule> oServiceSchedules, int nPreInvoiceID, long nUserID)
        {
            return PreInvoice.Service.SaveAll(oServiceSchedules, nPreInvoiceID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPreInvoiceService Service
        {
            get { return (IPreInvoiceService)Services.Factory.CreateService(typeof(IPreInvoiceService)); }
        }

        #endregion


        public int nRequest { get; set; }
    }
    #endregion

    #region IPreInvoice interface

    public interface IPreInvoiceService
    {

        PreInvoice Get(int id, Int64 nUserID);

        List<PreInvoice> Gets(Int64 nUserID);

        List<PreInvoice> Gets(string sSQL, Int64 nUserID);
        PreInvoice SaveAll(List<ServiceSchedule> oServiceSchedules, int nPreInvoiceID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        PreInvoice Save(PreInvoice oPreInvoice, Int64 nUserID);
        string UpdateStatus(PreInvoice oPreInvoice, Int64 nUserID);
        PreInvoice UpdateForHandoverCheckList(PreInvoice oPreInvoice, Int64 nUserID);
    }
    #endregion
}

