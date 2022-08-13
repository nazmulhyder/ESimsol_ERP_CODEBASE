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
    #region PurchaseRequisition
    public class PurchaseRequisitionRegister : BusinessObject
    {
        public PurchaseRequisitionRegister()
        {
            PRDetailID = 0;
            PRID = 0;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            UnitName = "";
            UnitSymbol = "";
            Qty = 0;
            RequirementDate = DateTime.Today;
            PrepareByName = "";
            BuyerName= "";
		    OrderRecapNo="";
            StyleNo = "";
            OrderRecapID = 0;
            ErrorMessage = "";
            RequiredFor = "";

            PRDate = DateTime.Now;
            RequirementDate = DateTime.Now;
            Note = "";
            RequisitionBy = 0;

            LastSupplyDate = DateTime.Now;
            LastSupplyQty = 0.0;
            PresentStock = 0.0;
            PresentStockUnitName = "";
            LastSupplyUnitName = "";
            StockInQty = 0.0;
            Remarks = "";
            Specifications = "";
        }

        #region Properties
        public double LastSupplyQty { get; set; }
        public double PresentStock { get; set; }
        public double StockInQty { get; set; }
        public int PRDetailID { get; set; }
        public DateTime LastSupplyDate { get; set; }
        public int PRID { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string Remarks { get; set; }
        public string PresentStockUnitName { get; set; }
        public string LastSupplyUnitName { get; set; }
        public string RequiredFor { get; set; }
        public string ProductName { get; set; }
        public string ModelNo { get; set; }
        public string ModelShortName { get; set; }
        public DateTime RequirementDate { get; set; }
        public string PrepareByName { get; set; }
        public string ProductSpec { get; set; }
        public double Qty { get; set; } ///Actual Purchase Qty
        public string Note { get; set; }
        public string PRNo { get; set; }
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }
        public int OrderRecapID { get; set; }
        public string BuyerName { get; set; }
        public string OrderRecapNo { get; set; }
        public string StyleNo { get; set; }
        public string ErrorMessage { get; set; }

        public int Status { get; set; }
        public int BUID { get; set; }
        public int ApproveBy { get; set; }
        public string ApprovedByName { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public string SearchingData { get; set; }
        public int RequisitionBy { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public DateTime PRDate { get; set; }
        public DateTime ApproveDate { get; set; }
        #endregion

        #region Derived Property
        public string Specifications { get; set; }
        public string LastSupplyQtySt
        {
            get
            {
                return (this.LastSupplyQty <= 0 ? "0" : this.LastSupplyQty.ToString());
            }
        }
        public string RequirementDateInString
        {
            get
            {
                return this.RequirementDate.ToString("dd MMM yyyy");
            }
        }
        public string LastSupplyDateSt
        {
            get
            {
                return ((this.LastSupplyDate == DateTime.MinValue) ? " " : this.LastSupplyDate.ToString("dd MMM yyyy"));
            }
        }
        public string PRDateSt
        {
            get
            {
                return ((this.PRDate == DateTime.MinValue) ? " " : this.PRDate.ToString("dd MMM yyyy"));
            }
        }
        public string ApproveDateSt
        {
            get
            {
                return ((this.ApproveDate == DateTime.MinValue) ? " " : this.ApproveDate.ToString("dd MMM yyyy"));
            }
        }
        public string StatusSt
        {
            get
            {
                return ((EnumPurchaseRequisitionStatus)this.Status).ToString();
            }
        }
        #endregion

        #region Functions
        public static List<PurchaseRequisitionRegister> Gets(string sSQL, long nUserID)
        {
            return PurchaseRequisitionRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPurchaseRequisitionRegisterService Service
        {
            get { return (IPurchaseRequisitionRegisterService)Services.Factory.CreateService(typeof(IPurchaseRequisitionRegisterService)); }
        }
        #endregion

    }
    #endregion

    #region IPurchaseRequisition interface
    public interface IPurchaseRequisitionRegisterService
    {
        List<PurchaseRequisitionRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion

}