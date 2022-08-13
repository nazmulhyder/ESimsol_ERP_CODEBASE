using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportDocSetup
    //[DataContract]
    public class ExportDocSetup : BusinessObject
    {
        #region  Constructor
        public ExportDocSetup()
        {
            ExportDocSetupID = 0;
            Carrier = "";
            NotifyParty = "";
            Account = "";
            Remarks = "";
            DocumentType = EnumDocumentType.None;
            DocumentTypeInt = (int)EnumDocumentType.None;
            DocName = "";
            IsPrintHeader = false;
            DocHeader = "";
            Beneficiary = "";
            BillNo = "";
            NoAndDateOfDoc = "";
            ProformaInvoiceNoAndDate = "";
            AccountOf = "";
            DocumentaryCreditNoDate = "";
            AgainstExportLC = "";
            PortofLoading = "";
            FinalDestination = "";
            IssuingBank = "";
            AdvicsBank = "";
            NegotiatingBank = "";
            CountryofOrigin = "";
            TermsofPayment = "";
            AmountInWord = "";
            Wecertifythat = "";
            Certification = "";
            IsPrintOriginal = false;
            IsPrintGrossNetWeight = false;
            IsPrintDeliveryBy = false;
            IsPrintTerm = false;
            IsPrintQty = false;
            IsPrintUnitPrice = false;
            IsPrintValue = false;
            IsPrintWeight = false;
            IsPrintFrieghtPrepaid = false;
            IsShowAmendmentNo = false;
            ClauseOne = "";
            ClauseTwo = "";
            ClauseThree = "";
            ClauseFour = "";
            Activity = false;
            AuthorisedSignature = "";
            CompanyName = "";
            Carrier = "";
            Account = "";
            NotifyParty = "";
            Remarks = "";
            IsVat = false;
            IsRegistration = false;
            IRC = "";
            GarmentsQty = "";
            HSCode = "";
            AreaCode = "";
            SpecialNote = "";
            Remark = "";
            ErrorMessage = "";
            DeliveryTo = "";
            BUID = 0;
            Sequence = 0;
            ShippingMark = "";
            ReceiverCluse = "";
            ForCaptionInDubleLine = false;
            ChallanNo = "";
            ExportDocSetups = new List<ExportDocSetup>();
            ExportBillID = 0;
            NotifyBy = EnumNotifyBy.None;
            WeightPBag = 0;
            WeightPBag =0;
            GrossWeightPTage=0;
            BagCount = 0;
            FoBTerm = "";
            DeliveryBy = "";
            TRDate = DateTime.MinValue;
            ExportBillDocID = 0;
            DriverName = "";
            ToTheOrderOf = "";
            TruckNo = "";
            TRNo = "";
            ContractorID = 0;
            ExportPartyInfoBills = null;
            GoodsDesViewType = EnumExportGoodsDesViewType.Qty;
            ProductPrintType = EnumExcellColumn.A;
            ExportLCType = EnumExportLCType.LC;
            TextWithGoodsRow = "";
            TextWithGoodsCol = "";
        }
        #endregion

        #region Properties
        public int ExportDocSetupID { get; set; }
        public int ExportBillDocID { get; set; }
        public int CompanyID { get; set; }
        public EnumDocumentType DocumentType { get; set; }
        public EnumExportLCType ExportLCType { get; set; }
        public int DocumentTypeInt { get; set; }
        public string DocName { get; set; }
        public bool IsPrintHeader { get; set; }
        public string DocHeader { get; set; }
        public string Beneficiary { get; set; }
        public string BillNo { get; set; }
        public string NoAndDateOfDoc { get; set; }
        public string ProformaInvoiceNoAndDate { get; set; }
        public string AccountOf { get; set; }
        public string DocumentaryCreditNoDate { get; set; }
        public string AgainstExportLC { get; set; }
        public string PortofLoading { get; set; }
        public string FinalDestination { get; set; }
        public string IssuingBank { get; set; }
        public string AdvicsBank { get; set; }
        public string NegotiatingBank { get; set; }
        public string CountryofOrigin { get; set; }
        public string TermsofPayment { get; set; }
        public string AmountInWord { get; set; }
        public string Wecertifythat { get; set; }
        public string Certification { get; set; }
        public bool IsPrintOriginal { get; set; }
        public bool IsShowAmendmentNo { get; set; }
        public bool IsPrintGrossNetWeight { get; set; }
        public bool IsPrintDeliveryBy { get; set; }
        public bool IsPrintTerm { get; set; }
        public bool IsPrintQty { get; set; }
        public bool IsPrintUnitPrice { get; set; }
        public bool IsPrintValue { get; set; }
        public bool IsPrintWeight { get; set; }
        public bool IsPrintFrieghtPrepaid { get; set; }
        public string ClauseOne { get; set; }
        public string ClauseTwo { get; set; }
        public string ClauseThree { get; set; }
        public string ClauseFour { get; set; }
        public bool Activity { get; set; }
        public string CompanyName { get; set; }
        public string Carrier { get; set; }
        public string Account { get; set; }
        public string NotifyParty { get; set; }
        public string Remarks { get; set; }
        public bool IsVat { get; set; }
        public bool IsRegistration { get; set; }/// TIN
        public string IRC { get; set; }
        public string GarmentsQty { get; set; }
        public string HSCode { get; set; }
        public string AreaCode { get; set; }
        public string SpecialNote { get; set; }
        public string Remark { get; set; }
        public int BUID { get; set; }
        public string DeliveryTo { get; set; }
        public bool IsPrintInvoiceDate { get; set; }
        public string AuthorisedSignature { get; set; }
        public string ReceiverSignature { get; set; }
        public string For { get; set; }
        public string MUnitName { get; set; }
        public string NetWeightName { get; set; }
        public string GrossWeightName { get; set; }
        public string CountryofOriginName { get; set; }
        public string SellingOnAbout { get; set; }
        public string PortofLoadingName { get; set; }
        public string FinalDestinationName { get; set; }
        public string Bag_Name { get; set; }
        public string TO { get; set; }
        public string Driver_Print { get; set; }
        public string TruckNo_Print { get; set; }
        public string DriverName { get; set; }
        public string TruckNo { get; set; }
        public string TRNo { get; set; }
        public DateTime TRDate { get; set; }
        public string ToTheOrderOf { get; set; }
        public EnumBankType OrderOfBankType { get; set; }
        public string TermsOfShipment { get; set; }
        public string TextWithGoodsRow { get; set; }
        public string TextWithGoodsCol { get; set; }
        public int Sequence { get; set; }
        public int ContractorID { get; set; }/// for carry  party info
        public string ShippingMark { get; set; }
        public string ReceiverCluse { get; set; }
        public bool ForCaptionInDubleLine { get; set; }
        public string CarrierName { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string MarkSAndNos { get; set; }
        public string QtyInOne { get; set; }
        public string QtyInTwo { get; set; }
        public string ValueName { get; set; }
        public string ChallanNo { get; set; }
        public EnumExcellColumn PrintOn { get; set; }
        public EnumExcellColumn ProductPrintType { get; set; }
        public string UPName { get; set; }
        public string NoOfBag { get; set; }
        public float FontSize_Normal { get; set; }
        public double FontSize_ULine { get; set; }
        public double FontSize_Bold { get; set; }
        public string CTPApplicant { get; set; }
        public string GRPNoDate { get; set; }
        public string ASPERPI { get; set; }
        public EnumExportGoodsDesViewType GoodsDesViewType { get; set; }
        public int ExportBillID { get; set; }
        public string DeliveryBy { get; set; }
        public string FoBTerm { get; set; }
        public int OrderOfBankTypeInInt { get; set; }
        public double WeightPBag { get; set; }
        public double GrossWeightPTage { get; set; }
        public double BagCount { get; set; }
        public EnumNotifyBy NotifyBy { get; set; }
        public List<ExportDocSetup> ExportDocSetups { get; set; }
        public List<ExportPartyInfoBill> ExportPartyInfoBills { get; set; }
        public string ErrorMessage { get; set; }
        public string ActivityInSt
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        public string OrderOfBankTypeSt
        {
            get
            {
                return this.OrderOfBankType.ToString();
            }
        }
        public string TRDateSt
        {
            get
            {
                if (this.TRDate == DateTime.MinValue) return "";
                else return this.TRDate.ToString("dd MMM yyyy");

            }
        }
        public string ExportLCTypeSt
        {
            get
            {
                return this.ExportLCType.ToString();

            }
        }
        #endregion

        #region Functions
        public ExportDocSetup Get(int nId, Int64 nUserID)
        {
            return ExportDocSetup.Service.Get(nId, nUserID);
        }
        public ExportDocSetup GetBy(int nID, int nExportBillID, Int64 nUserID)
        {
            return ExportDocSetup.Service.GetBy(nID, nExportBillID, nUserID);
        }
        public ExportDocSetup Save(Int64 nUserID)
        {
            return ExportDocSetup.Service.Save(this, nUserID);
        }
        public ExportDocSetup Save_Bill(Int64 nUserID)
        {
            return ExportDocSetup.Service.Save_Bill(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return ExportDocSetup.Service.Delete(this, nUserID);
        }
        public static List<ExportDocSetup> Gets(Int64 nUserID)
        {
            return ExportDocSetup.Service.Gets(nUserID);
        }
        public static List<ExportDocSetup> UpdateSequence(ExportDocSetup oExportDocSetup, Int64 nUserID)
        {
            return ExportDocSetup.Service.UpdateSequence(oExportDocSetup, nUserID);
        }
        public static List<ExportDocSetup> Gets(string sSQL, Int64 nUserID)
        {
            return ExportDocSetup.Service.Gets(sSQL, nUserID);
        }
        public static List<ExportDocSetup> Gets(bool bActivity, int nBUID, Int64 nUserID)
        {
            return ExportDocSetup.Service.Gets(bActivity, nBUID, nUserID);
        }
        public static List<ExportDocSetup> BUWiseGets(int nBUID, Int64 nUserID)
        {
            return ExportDocSetup.Service.BUWiseGets(nBUID, nUserID);
        }
        public static List<ExportDocSetup> GetsBy(int nExportBillID, Int64 nUserID)
        {
            return ExportDocSetup.Service.GetsBy(nExportBillID, nUserID);
        }
        public static List<ExportDocSetup> GetsByType(int nExportLCType, int nBUID,Int64 nUserID)
        {
            return ExportDocSetup.Service.GetsByType(nExportLCType,nBUID, nUserID);
        }
        public ExportDocSetup Activate(ExportDocSetup oExportDocSetup, Int64 nUserID)
        {
            return ExportDocSetup.Service.Activate(this, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IExportDocSetupService Service
        {
            get { return (IExportDocSetupService)Services.Factory.CreateService(typeof(IExportDocSetupService)); }
        }
        #endregion
    }
    #endregion


    #region IExportDocSetup interface
    public interface IExportDocSetupService
    {
        ExportDocSetup Get(int nID, Int64 nUserID);
        ExportDocSetup GetBy(int nID, int nExportBillID, Int64 nUserID);
        List<ExportDocSetup> Gets(Int64 nUserID);
        List<ExportDocSetup> UpdateSequence(ExportDocSetup oExportDocSetup, Int64 nUserID);
        List<ExportDocSetup> Gets(string sSQL, Int64 nUserID);
        List<ExportDocSetup> Gets(bool bActivity, int BUID, Int64 nUserID);
        List<ExportDocSetup> BUWiseGets(int BUID, Int64 nUserID);
        List<ExportDocSetup> GetsBy(int nExportBillID, Int64 nUserID);
        List<ExportDocSetup> GetsByType(int nExportLCType,int BUID, Int64 nUserID);
        ExportDocSetup Save(ExportDocSetup oExportDocSetup, Int64 nUserID);
        ExportDocSetup Save_Bill(ExportDocSetup oExportDocSetup, Int64 nUserID);
        string Delete(ExportDocSetup oExportDocSetup, Int64 nUserID);
        ExportDocSetup Activate(ExportDocSetup oExportDocSetup, Int64 nUserID);
    }
    #endregion

}
