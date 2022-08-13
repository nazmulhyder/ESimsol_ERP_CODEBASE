using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportCommercialDoc
    [DataContract]
    public class ExportCommercialDoc : BusinessObject
    {
        #region  Constructor
        public ExportCommercialDoc()
        {
            ExportLCID = 0;
            ApplicantID = 0;
            ErrorMessage = "";
            CarrierName = "";
            this.ExportDocSetup = new ExportDocSetup();
            this.ExportBillDetails = new List<ExportBillDetail>();
            CommercialInvoiceDetails = new List<CommercialInvoiceDetail>();
            ShippingMark = "";
            ShippingMarkName = "";
            ReceiverCluse = "";
            CommercialInvoiceNo = "";
            ForCaptionInDubleLine = false;
            NoOfPackages = ""; //Add By Faruk for Plastic & Poly Number of Cortoon Display
            PlasticNetWeight = ""; //Add By Faruk for Plastic & Poly Net Weight Display
            PlasticGrossWeight = ""; //Add By Faruk for Plastic & Poly Gross Weight Display
            PrintOn = EnumExcellColumn.A;
            CTPApplicant = "";
            GRPNoDate = "";
            ASPERPI = "";
            BankName_Endorse = "";
            BBranchName_Endorse = "";
            BankAddress_Endorse = "";
            DocDate = "";
            MUnitCon = new MeasurementUnitCon();
            ExportBillDocID = 0;
            TRNo = "";
            TRDate = "";
            GoodsDesViewType = EnumExportGoodsDesViewType.None;
            OrderOfBankType = EnumBankType.None;
            SendToBankDate = DateTime.Now;
            ExportClaimSettles = new List<ExportClaimSettle>();
            TextWithGoodsRow = "";
            TextWithGoodsCol = "";
            FontSize_Normal = 0;
            ToTheOrderOf = "";
            NegotiatingBank = "";
        }
        #endregion

        #region Properties

        #region DocSetup
     
        public int ExportDocSetupID { get; set; }
        public string BUName { get; set; }
        public string BUAddress { get; set; }
        public int BUID { get; set; }
        public int BusinessUnitType { get; set; }
        public EnumDocumentType DocumentType { get; set; }
        public string DocName { get; set; }
        public string BillNo { get; set; }
        public string DocDate { get; set; } //DocPrint Date
        public bool IsPrintHeader { get; set; }
        public string DocHeader { get; set; }
        public string SpecialNote { get; set; }
        public string Beneficiary { get; set; }
        public string NoAndDateOfDoc { get; set; }
        public string ProformaInvoiceNoAndDate { get; set; }
        public string AccountOf { get; set; } 
        public string DocumentaryCreditNoDate { get; set; }

        public string MasterLCNo { get; set; }
        public string AgainstExportLC { get; set; } 
        public string PortofLoading { get; set; }
        public string FinalDestination { get; set; } 
        public string IssuingBank { get; set; }
        public string AdvicsBank { get; set; } 
        public string NegotiatingBank { get; set; }
        public string ToTheOrderOf { get; set; }
        public string CountryofOrigin { get; set; }
        public string TermsofPayment { get; set; }
        public string AmountInWord { get; set; } 
        public string Wecertifythat { get; set; }
        public string Certification { get; set; }
        //public string Certification_Entry { get; set; }
        public string ClauseOne { get; set; }
        
        public string ClauseTwo { get; set; }
        public string ClauseThree { get; set; }
        public string ClauseFour { get; set; }
        #region Fixed Value
        public string AuthorisedSignature { get; set; }
        public string ReceiverSignature { get; set; }
        public string For { get; set; }
        public string To { get; set; }
        public string DocumentsDes { get; set; }
        public string PortofLoadingName { get; set; }
        public string PortofLoadingAddress { get; set; }
        public string FinalDestinationName { get; set; }
        public string CountryofOriginName { get; set; }
        public string CTPApplicant { get; set; }
        public string GRPNoDate { get; set; }
        public string ASPERPI { get; set; }
        public float FontSize_Normal { get; set; }
        #endregion
        public bool IsPrintInvoiceDate { get; set; }
        public string Carrier { get; set; }
        public string SellingOnAbout { get; set; }
        public string SellingOnAboutName { get; set; }
        public string Account { get; set; }
         
        public string CarrierName { get; set; }
        public double BagWeight { get; set; }
        public double WeightPerBag { get; set; }
        public string Bag_MUnit { get; set; }
        public double WeightPBag { get; set; } /// Its from Setup
        public double GrossWeightPTage { get; set; }/// Its from Setup
        public string MUName { get; set; }
        public string Bag_Name { get; set; }
        public string LCTermsName { get; set; }
        public EnumPaymentInstruction PaymentInstruction { get; set; }
        public string SL { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string Header_Construction { get; set; }
        public string Header_FabricsType { get; set; }
        public string Header_Style { get; set; }
        public string Header_Color { get; set; }
        public string NoOfBag { get; set; }
        public string WtperBag { get; set; }
        public string NetWeight { get; set; }
        public string GrossWeight { get; set; }
        public string MarkSAndNos { get; set; }
        public string QtyInKg { get; set; }
        public string QtyInLBS { get; set; }
        public string ValueDes { get; set; }
        public string UnitPriceDes { get; set; }
         public double BagCount { get; set; }
        public string Amount { get; set; }
        public string GarmentsQty { get; set; }
        public string HSCode { get; set; }
        public string AreaCode { get; set; }
        public string Remark { get; set; }
        public string Vat_ReqNo { get; set; }
        public string TIN { get; set; }
        public EnumExportGoodsDesViewType GoodsDesViewType { get; set; }
        public string AllMasterLCNo { get; set; }
        public string AllExportLCNo { get; set; }
        public string DeliveryTo { get; set; }
        public bool IsPrintUnitPrice { get; set; }
        public bool IsPrintValue { get; set; }
        public System.Drawing.Image BOEImage { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public EnumBankType OrderOfBankType { get; set; }
        public string ShippingMark { get; set; }
        public string ShippingMarkName { get; set; }
        public string ReceiverCluse { get; set; }
        public bool ForCaptionInDubleLine { get; set; }
        public string NoOfPackages { get; set; }
        public string PlasticNetWeight { get; set; }
        public string PlasticGrossWeight { get; set; }        
        public string ErrorMessage { get; set; }
        #region ExportDocSetup
        
        public List<ExportBillDetail> ExportBillDetails;
        public ExportDocSetup ExportDocSetup;
        public MeasurementUnitCon MUnitCon;
        public string IRC_Head { get; set; }
        public string GarmentsQty_Head { get; set; }
        public string HSCode_Head { get; set; }
        public string AreaCode_Head { get; set; }
        public string SpecialNote_Head { get; set; }
        public string Remark_Head { get; set; }
        public string Var_ReqNo_Head { get; set; }
        public string TIN_Head { get; set; }

        #endregion
        #endregion

        #region Export Bill
        public string ExportBillNo { get; set; }
        public string Year { get; set; }
        public double Amount_Bill { get; set; }
        public int ExportBillID { get; set; }
        public int ExportLCID { get; set; }
        public int ApplicantID { get; set; }
        public int ExportBillDocID { get; set; }    
        public int MasterLCID { get; set; }
        public string ExportBillDate { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public string DeliveryToName { get; set; }
        public string DeliveryToAddress { get; set; }
        public string FactoryAddress { get; set; }
        public string CommercialInvoiceNo { get; set; }
        public List<CommercialInvoiceDetail> CommercialInvoiceDetails { get; set; }
        public List<ExportClaimSettle> ExportClaimSettles { get; set; }
        public string BankName_Nego { get; set; }
        public string BBranchName_Nego { get; set; }
        public string BankNickName_Nego { get; set; }
        public string BankAddress_Nego { get; set; }
        public string BankName_Issue { get; set; }
        public string BBranchName_Issue { get; set; }
        public string BankAddress_Issue { get; set; }
        public string BankName_Advice { get; set; }
        public string BankAddress_Advice { get; set; }
        public string BBranchName_Advice { get; set; }
        public string BankName_Forwarding { get; set; }
        public string BBranchName_Forwarding { get; set; }
        public string BankAddress_Forwarding { get; set; }
        public string BankName_Endorse { get; set; }
        public string BBranchName_Endorse { get; set; }
        public string BankAddress_Endorse { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime SendToBankDate { get; set; }

        //Export LC Derive Property
        public string ExportLCNo { get; set; }
      
        public double OverDueRate { get; set; }
        public double Amount_LC { get; set; }
        public string PINos { get; set; }
        public string LCOpeningDate { get; set; }
        public string LCRecivedDate { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public string IRC { get; set; }
        public string ERC { get; set; }
        public string FrightPrepaid { get; set; }
        public string NotifyParty { get; set; }
        public EnumNotifyBy NotifyBy { get; set; }
        public string Remarks { get; set; }
        public string BillRemark { get; set; }
        public string Orginal { get; set; }
     //From Export TnC etup
        public bool IsPrintGrossNetWeight { get; set; }
        public bool IsPrintOriginal { get; set; }
        public string DeliveryBy { get; set; }
        public bool IsDeliveryBy { get; set; }
        public string Term { get; set; }
        public string Term_Head { get; set; }
        public bool IsTerm { get; set; }
        public string DriverName { get; set; }
        public string DriverName_Print { get; set; }
        public string TruckNo { get; set; }
        public string TruckNo_Print { get; set; }
        public string TRNo { get; set; }
        public string TRDate { get; set; }
        //
        public bool IsPrintVat { get; set; }
        public bool IsPrintRegistration { get; set; }
        public int VersionNo { get; set; }
        public string AmendmentDate { get; set; }
        public string AmendmentNonDate { get; set; }
        public string ChallanNo { get; set; }
        public string TextWithGoodsRow { get; set; }
        public string TextWithGoodsCol { get; set; }
        public EnumExcellColumn PrintOn { get; set; }
        public EnumExcellColumn ProductPrintType { get; set; }
        public string ApplicantAddress_Full
        {
            get
            {
                if (String.IsNullOrEmpty(this.FactoryAddress))
                {
                    return this.ApplicantAddress ;
                }
                else
                {
                    return this.ApplicantAddress + " \nFactory:" + this.FactoryAddress;
                }
            }
        }
        public string ExportLCNoAndDate
        {
            get
            {
                if (String.IsNullOrEmpty(this.ExportLCNo))
                {
                    return this.ExportLCNo;
                }
                else
                {
                    return this.ExportLCNo + "   DT:" + this.LCOpeningDate;
                }
            }
        }
        public string ExportBillNo_Full
        {
            get
            {
                if (String.IsNullOrEmpty(this.BillNo))
                {
                    return this.ExportBillNo;
                }
                else
                {
                    return this.BillNo + "-" + this.ExportBillNo;
                }
            }
        }
        public string ExportBill_ChallanNo
        {
            get
            {
                if (String.IsNullOrEmpty(this.ChallanNo))
                {
                    return this.ExportBillNo;
                }
                else
                {
                    return this.ChallanNo + "-" + this.ExportBillNo;
                }
            }
        }
        public string ExportBillNo_FullDate
        {
            get
            {
                if (this.IsPrintInvoiceDate)
                {
                    if (String.IsNullOrEmpty(this.BillNo))
                    {
                        return this.ExportBillNo + "  DT :" + this.ExportBillDate;
                    }
                    else
                    {
                        return this.BillNo + "-" + this.ExportBillNo + "  DT :" + this.ExportBillDate;
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(this.BillNo))
                    {
                        return this.ExportBillNo;
                    }
                    else
                    {
                        return this.BillNo + "-" + this.ExportBillNo+"   DT: ";
                    }
                }
            }
        }
    

        public string LCTermsName_Full
        {
            get
            {
                if (this.PaymentInstruction == EnumPaymentInstruction.None)
                {
                    return this.LCTermsName;
                }
                if (this.PaymentInstruction == EnumPaymentInstruction.DocumentsReceiveddate)
                {
                    return this.LCTermsName + " From the Date Of Documents Received date at LC Issuing Bank’s Counter.";
                }
                else
                {
                    return this.LCTermsName + " From the Date Of " + this.PaymentInstruction.ToString();
                }
            }
        }

        public string InvoiceDateInstring
        {
            get
            {
             return   this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }


       
        #endregion

        #endregion
        #region Functions
        public ExportCommercialDoc Get(int nId, Int64 nUserID)
        {
            return ExportCommercialDoc.Service.Get(nId, nUserID);
        }
        public ExportCommercialDoc GetForBuying(int nId, Int64 nUserID)
        {
            return ExportCommercialDoc.Service.GetForBuying(nId, nUserID);
        }
     
     
        #endregion

        #region ServiceFactory

        internal static IExportCommercialDocService Service
        {
            get { return (IExportCommercialDocService)Services.Factory.CreateService(typeof(IExportCommercialDocService)); }
        }

        #endregion


    }
    #endregion

    #region IExportBill interface
    [ServiceContract]
    public interface IExportCommercialDocService
    {
        ExportCommercialDoc Get(int id, Int64 nUserID);
        ExportCommercialDoc GetForBuying(int id, Int64 nUserID);
      
    }
    #endregion
}
