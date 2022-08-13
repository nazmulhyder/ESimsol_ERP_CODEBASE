using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects.ReportingObject
{
    #region rptMPRToGRN
    public class rptMPRToGRN : BusinessObject
    {
        public rptMPRToGRN()
        {
            PRID = 0;
            PRDetailID = 0;
            MPRNo = string.Empty;
            MPRDate = DateTime.Now;
            RequiredDate = DateTime.Now;
            DepartmentName = string.Empty;
            MPRStatus = EnumPurchaseRequisitionStatus.Initialized;
            RequisitonByName = string.Empty;
            ApprovedByName = string.Empty;
            ReqQty = 0;


            ProductID = 0;
            ProductCode = string.Empty;
            ProductName = string.Empty;
            ProductCategoryName = string.Empty;
            ProductGroupName = string.Empty;
            Specification = string.Empty;


            NOAID = 0;
            NOANo = string.Empty;
            NOADate = DateTime.Now;
            QuotationID = 0;
            QuotationDetailID = 0;
            QuotationNo = string.Empty;
            NOADetailID = 0;
            NOAQty = 0;
            CSApprovedByName = string.Empty;


            PODetailID = 0;
            POID = 0;
            PORefType = EnumPOReferenceType.None;
            PONo = string.Empty;
            PODate = DateTime.Now;
            POCrateByName = string.Empty;
            POApprovedByName = string.Empty;
            PORefDetailID = 0;
            POQty = 0;


            PurchaseInvoiceDetailID = 0;
            PurchaseInvoiceID = 0;
            InvoiceNo = string.Empty;
            InvoiceDate = DateTime.Now;
            InvoiceCreateByName = string.Empty;
            InvoiceApprovedByName = string.Empty;
            InvQty = 0;


            GRNID = 0;
            GRNNo = string.Empty;
            GRNCreateByName = string.Empty;
            GRNReceiveByName = string.Empty;
            GRNReceiveDate = DateTime.Now;
            RefType = EnumGRNType.None;
            RefObjectID = 0;
            RefQty = 0;
            MUnitID = 0;
            UnitName = string.Empty;
            RejectQty = 0;
            ReceivedQty = 0;
            YetToReceiveQty = 0;
            UnitPrice = 0;
            Discount = 0;
            Expense = 0;
            TotalAmount = 0;
            PresentStock = 0;
        }
        #region Properties
        public int PRID { get; set; }
        public int PRDetailID { get; set; }
        public string MPRNo { get; set; }
        public DateTime MPRDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string DepartmentName { get; set; }
        public EnumPurchaseRequisitionStatus MPRStatus { get; set; }
        public string RequisitonByName { get; set; }
        public string ApprovedByName { get; set; }
        public double ReqQty { get; set; }


        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductGroupName { get; set; }
        public string Specification { get; set; }


        public int NOAID { get; set; }
        public string NOANo { get; set; }
        public DateTime NOADate { get; set; }
        public int QuotationID { get; set; }
        public int QuotationDetailID { get; set; }
        public string QuotationNo { get; set; }
        public int NOADetailID { get; set; }
        public double NOAQty { get; set; }
        public string CSApprovedByName { get; set; }


        public int PODetailID { get; set; }
        public int POID { get; set; }
        public EnumPOReferenceType PORefType { get; set; }
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public string POCrateByName { get; set; }
        public string POApprovedByName { get; set; }
        public int PORefDetailID { get; set; }
        public double POQty { get; set; }


        public int PurchaseInvoiceDetailID { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceCreateByName { get; set; }
        public string InvoiceApprovedByName { get; set; }
        public double InvQty { get; set; }


        public int GRNID { get; set; }
        public string GRNNo { get; set; }
        public string GRNCreateByName { get; set; }
        public string GRNReceiveByName { get; set; }
        public DateTime GRNReceiveDate { get; set; }
        public EnumGRNType RefType { get; set; }
        public int RefObjectID { get; set; }
        public double RefQty { get; set; }
        public int MUnitID { get; set; }
        public string UnitName { get; set; }
        public double RejectQty { get; set; }
        public double ReceivedQty { get; set; }
        public double YetToReceiveQty { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
        public double Expense { get; set; }
        public double TotalAmount { get; set; }
        public double PresentStock { get; set; }
        #endregion


        #region Derived Properties
        public string MPRDateSt
        {
            get
            {
                return (this.MPRDate == DateTime.MinValue) ? "-" : this.MPRDate.ToString("dd MMM yyyy");
            }
        }
        public string RequiredDateSt
        {
            get
            {
                return (this.RequiredDate == DateTime.MinValue) ? "-" : this.RequiredDate.ToString("dd MMM yyyy");
            }
        }
        public string PODateSt
        {
            get
            {
                return (this.PODate == DateTime.MinValue) ? "-" : this.PODate.ToString("dd MMM yyyy");
            }
        }

        public string InvoiceDateSt
        {
            get
            {
                return (this.InvoiceDate == DateTime.MinValue) ? "-" : this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }

        public string GRNReceiveDateSt
        {
            get
            {
                return (this.GRNReceiveDate == DateTime.MinValue) ? "-" : this.GRNReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string NOADateSt
        {
            get
            {
                return (this.NOADate == DateTime.MinValue) ? "-" : this.NOADate.ToString("dd MMM yyyy");
            }
        }
        public string PORefTypeStr { get { return this.PORefType.ToString(); } }
        public string MPRStatusStr { get { return this.MPRStatus.ToString(); } }
        #endregion

        #region Functions

        public static List<rptMPRToGRN> Gets(int nBUID, DateTime StartDate, DateTime EndDate, string sPRNo, int nUserID)
        {
            return rptMPRToGRN.Service.Gets(nBUID, StartDate, EndDate, sPRNo, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IrptMPRToGRNService Service
        {
            get { return (IrptMPRToGRNService)Services.Factory.CreateService(typeof(IrptMPRToGRNService)); }
        }
        #endregion

    }
    #endregion
    #region IrptMPRToGRNService interface
    public interface IrptMPRToGRNService
    {
        List<rptMPRToGRN> Gets(int nBUID, DateTime StartDate, DateTime EndDate, string sPRNo, Int64 nUserID);
    }
    #endregion

}
