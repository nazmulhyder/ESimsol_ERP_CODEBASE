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
    #region PurchaseRequisitionDetail
    public class PurchaseRequisitionDetail : BusinessObject
    {
        public PurchaseRequisitionDetail()
        {
            PRDetailID = 0;
            PRID = 0;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            MUnitID = 0;
            UnitName = "";
            UnitSymbol = "";
            Qty = 0;
            MUnitID = 0;
            VehicleModelID = 0;
            RequirementDate = DateTime.Today;
            PrepareByName = "";
            BuyerName= "";
		    OrderRecapNo="";
            StyleNo = "";
            OrderRecapID = 0;
            ErrorMessage = "";
            RequiredFor = "";


            LastSupplyDate = DateTime.Now;
            LastSupplyQty = 0.0;
            PresentStock = 0.0;
            PresentStockUnitName = "";
            LastSupplyUnitName = "";
            StockInQty = 0.0;
            Remarks = "";
            Specifications = "";
            IsSpecExist = false;
            YetToPOQty = 0;

        }

        #region Properties
        public double LastSupplyQty { get; set; }
        public double PresentStock { get; set; }
        public double StockInQty { get; set; }
        public int PRDetailID { get; set; }
        public int VehicleModelID { get; set; }
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
        public int MUnitID { get; set; }
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }
        public int OrderRecapID { get; set; }
        public string BuyerName { get; set; }
        public string OrderRecapNo { get; set; }

        public string StyleNo { get; set; }
       

        private string _sStatus = "";
       
        public string ErrorMessage { get; set; }
        public bool IsSpecExist { get; set; }
        public double YetToPOQty { get; set; }
        #endregion

        #region Derived Property
        public string Specifications { get; set; }

        public string LastSupplyQtySt
        {
            get
            {
                return (this.LastSupplyQty <= 0 ? "0" : this.LastSupplyQty.ToString());
            }
        }public string RequirementDateInString
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
        #endregion

        #region Functions
        
        public static List<PurchaseRequisitionDetail> Gets(int PurchaseRequisitionID, long nUserID)
        {
            return PurchaseRequisitionDetail.Service.Gets(PurchaseRequisitionID, nUserID);
        }
        public static List<PurchaseRequisitionDetail> GetsBy(int nPRID, int nContractorID, long nUserID)
        {
            return PurchaseRequisitionDetail.Service.GetsBy(nPRID,nContractorID, nUserID);
        }
        public static List<PurchaseRequisitionDetail> Gets(string sSQL, long nUserID)
        {
            return PurchaseRequisitionDetail.Service.Gets(sSQL, nUserID);
        }
        //public static List<PurchaseRequisitionDetail> Gets(string sSQL, long nUserID)
        //{
        //    return PurchaseRequisitionDetail.Service.Gets(sSQL, nUserID);
        //}
    
        public PurchaseRequisitionDetail Get(int PurchaseRequisitionDetailID, long nUserID)
        {
            return PurchaseRequisitionDetail.Service.Get(PurchaseRequisitionDetailID, nUserID);
        }
        public PurchaseRequisitionDetail Save(long nUserID)
        {
            return PurchaseRequisitionDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return PurchaseRequisitionDetail.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPurchaseRequisitionDetailService Service
        {
            get { return (IPurchaseRequisitionDetailService)Services.Factory.CreateService(typeof(IPurchaseRequisitionDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IPurchaseRequisitionDetail interface

    public interface IPurchaseRequisitionDetailService
    {

        PurchaseRequisitionDetail Get(int PurchaseRequisitionDetailID, Int64 nUserID);

        List<PurchaseRequisitionDetail> Gets(int nPurchaseRequisitionID, Int64 nUserID);
        List<PurchaseRequisitionDetail> GetsBy(int nPRID,int nContractorID, Int64 nUserID);
        List<PurchaseRequisitionDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(PurchaseRequisitionDetail oPurchaseRequisitionDetail, Int64 nUserID);
        PurchaseRequisitionDetail Save(PurchaseRequisitionDetail oPurchaseRequisitionDetail, Int64 nUserID);
       
    }
    #endregion

}
