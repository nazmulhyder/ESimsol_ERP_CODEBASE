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

    #region PurchaseOrder
    public class PurchaseOrder : BusinessObject
    {
        public PurchaseOrder()
        {

            POID = 0;
            BUID = 0;
            PONo = "";
            PODate = DateTime.Today;
            RefType = EnumPOReferenceType.Open;
            RefTypeInt = 0;
            RefID = 0;
            Status = EnumPOStatus.Initialize;
            StatusInt = 0;
            ContractorID = 0;
            ContactPersonnelID = 0;
            Note = "";
            ConcernPersonID = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.Today;
            CurrencyID = 0;
            ContractorName = "";
            ContractorShortName = "";
            ApprovedByName = "";
            PrepareByName = "";
            PrepareBy = 0;
            ConcernPersonName = "";
            ContactPersonName = "";
            CurrencySymbol = "";
            CurrencyBFDP = "";
            CurrencyBADP = "";            
            BUCode = "";
            BUName = "";
            RefNo = "";
            RefDate = DateTime.Today;
            RefBy = "";
            Amount = 0;
            YetToGRNQty = 0;
            YetToInvocieQty = 0;
            PaymentTermID = 0;
            ShipBy = "";
            TradeTerm = "";
            DeliveryTo = 0;
            DeliveryToName = "";
            DeliveryToContactPerson = 0;
            DeliveryToContactPersonName = "";
            BillTo = 0;
            BillToName = "";
            BIllToContactPerson = 0;
            LotBalance = 0;
            YetToPurchaseReturnQty = 0;
            BIllToContactPersonName = "";
            PaymentTermText = "";
            CRate = 1;
            LastApprovalSequence = 0;
            ApprovalSequence = 0;
            ApprovalStatus = "";
            ErrorMessage = "";
            SCPersons = new List<ContactPersonnel>();
            PurchaseOrderDetails = new List<PurchaseOrderDetail>();
            POTandCClauses = new List<POTandCClause>();
            PurchaseCosts = new List<PurchaseCost>();
            PurchaseInvoices = new List<PurchaseInvoice>();
            DiscountInPercent=0;
            DiscountInAmount = 0;
            POSpecs = new List<POSpec>();
            SubjectName = "";
            DiscountAmountOfPO = 0;
        }

        #region Properties
        public int POID { get; set; }
        public int BUID { get; set; }
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public EnumPOReferenceType RefType { get; set; }
        public int RefTypeInt { get; set; }
        public int RefID { get; set; }
        public EnumPOStatus Status { get; set; }
        public int StatusInt { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public string Note { get; set; }
        public int ConcernPersonID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public int CurrencyID { get; set; }
        public string ContractorName { get; set; }
        public string ContractorShortName { get; set; }
        public string ApprovedByName { get; set; }
        public string PrepareByName { get; set; }
        public int PrepareBy { get; set; }
        public string ConcernPersonName { get; set; }
        public string ContactPersonName { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyBFDP { get; set; } // Currency In Word Before Decimal Point
        public string CurrencyBADP { get; set; } // Currency In Word After Decimal Point
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string RefNo { get; set; }
        public DateTime RefDate { get; set; }
        public string RefBy { get; set; }
        public double Amount { get; set; }
        public double YetToGRNQty { get; set; }
        public double YetToInvocieQty { get; set; }
        public int  PaymentTermID{ get; set; }
        public string  ShipBy{ get; set; }
        public string TradeTerm { get; set; }
        public int  DeliveryTo{ get; set; }
        public string DeliveryToName { get; set; }
        public int  DeliveryToContactPerson{ get; set; }
        public string DeliveryToContactPersonName { get; set; }
        public int  BillTo{ get; set; }
        public string BillToName { get; set; }
        public int  BIllToContactPerson{ get; set; }
        public string BIllToContactPersonName { get; set; }
        public string PaymentTermText { get; set; }
        public double CRate { get; set; }
        public double LotBalance { get; set; }
        public double YetToPurchaseReturnQty { get; set; }
        public string ErrorMessage { get; set; }
        public int LastApprovalSequence { get; set; }
        public int ApprovalSequence { get; set; }
        public string ApprovalStatus { get; set; }
        public string SubjectName { get; set; }
        public double DiscountInPercent{get; set;}
        public double DiscountInAmount { get; set; }
        public EnumInvoicePaymentMode PaymentMode { get; set; }
        public int PaymentModeInt { get; set; }
        public double YetToPI_Amount { get; set; }
        #endregion

        #region Derived Property
        public double DiscountAmountOfPO { get; set; }
        public List<POSpec> POSpecs { get; set; }
        public List<ContactPersonnel> SCPersons { get; set; }
        public List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public List<POTandCClause> POTandCClauses { get; set; }
        public List<PurchaseCost> PurchaseCosts { get; set; }
        public List<PurchaseInvoice> PurchaseInvoices { get; set; }
        public string PaymentModeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentMode);
            }
        }
        public string FullPONo
        {
            get
            {
                return "PO-" + this.PONo;
            }
        }
        public string PODateSt
        {
            get
            {
                return PODate.ToString("dd MMM yyyy");
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string RefDateSt
        {
            get
            {
                return RefDate.ToString("dd MMM yyyy");
            }
        }
        public string POStatusSt
        {
            get
            {
                return this.Status.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<PurchaseOrder> Gets(long nUserID)
        {
            return PurchaseOrder.Service.Gets(nUserID);
        }

        public static List<PurchaseOrder> Gets(string sSQL, long nUserID)
        {
            return PurchaseOrder.Service.Gets(sSQL, nUserID);
        }

        public PurchaseOrder Get(int id, long nUserID)
        {
            return PurchaseOrder.Service.Get(id, nUserID);
        }

        public PurchaseOrder Save(long nUserID)
        {
            return PurchaseOrder.Service.Save(this, nUserID);
        }
        public PurchaseOrder Approved(long nUserID)
        {
            return PurchaseOrder.Service.Approved(this, nUserID);
        }
        public PurchaseOrder UndoApproved(long nUserID)
        {
            return PurchaseOrder.Service.UndoApproved(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PurchaseOrder.Service.Delete(id, nUserID);
        }
        public PurchaseOrder UpdateReportSubject(long nUserID)
        {
            return PurchaseOrder.Service.UpdateReportSubject(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPurchaseOrderService Service
        {
            get { return (IPurchaseOrderService)Services.Factory.CreateService(typeof(IPurchaseOrderService)); }
        }

        #endregion


    }
    #endregion

    #region IPurchaseOrder interface

    public interface IPurchaseOrderService
    {

        PurchaseOrder Get(int id, Int64 nUserID);
        List<PurchaseOrder> Gets(Int64 nUserID);
        List<PurchaseOrder> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        PurchaseOrder Save(PurchaseOrder oPurchaseOrder, Int64 nUserID);
        PurchaseOrder Approved(PurchaseOrder oPurchaseOrder, Int64 nUserID);
        PurchaseOrder UndoApproved(PurchaseOrder oPurchaseOrder, Int64 nUserID);
        PurchaseOrder UpdateReportSubject(PurchaseOrder oPurchaseOrder, Int64 nUserID);

    }
    #endregion

}