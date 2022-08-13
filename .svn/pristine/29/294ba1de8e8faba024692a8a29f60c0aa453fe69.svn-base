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

    /*Class Name : RawmaterialStatus
        Purpose: For Print Report for Yarn Receive or Acessoris Receive
         Author: Md. Mahabub Alam
           Date: 28 Jan 14
    Last Modify:*/

    #region RawmaterialStatus
    
    public class RawmaterialStatus : BusinessObject
    {
        public RawmaterialStatus()
        {
            SaleOrderID = 0;            
            OrderRecapNo = "";
            SaleOrderNo = "";
            SaleOrderQty = 0;
            ProductionFactoryName = "";
            StyleNo = "";
            ProductionFactoryID = 0;
            TechnicalSheetID = 0;
            YarnCount = "";
            GG = "";
            PurchasePIID = 0;
            LabDipOrderDetailID = 0;
            ApproveShade = 0;				
            ColorTrackNo  =0;
            ColorTrackYr  ="";
            ReLabNo  =0;
            ReLabOn  = 0;
            ConSumptionWestage  ="";
            ChallanNoLotNo  = "";
            PINo = "";
            ProductID = 0;
            ProductName = "";
            SupplierID = 0;
            SupplierName = "";
            PIReceiveDate = DateTime.Now;
            LCDate = DateTime.Now;
            ColorID = 0;
            DyeingOrderNo = "";
            ColorName = "";
            OrderQty = 0;
            UnitID = 0;
            UnitName = "";
            ReceiveQty = 0;
            ReceiveDate = DateTime.Now;
            Balance = 0;
            Remarks = "";
            BuyerName = "";
            ShipmentDate = DateTime.Today;
            WorkOrderNo = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public string OrderRecapNo { get; set; }        
         
        public string SaleOrderNo { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public int LabDipOrderDetailID { get; set; }
         
        public int ProductID { get; set; }
         
        public int PurchasePIID { get; set; }
         
        public int SupplierID { get; set; }
         
        public int ColorID { get; set; }
         
        public int MerchandiserID { get; set; }
         
        public int RawmaterialStatusHistoryID { get; set; }
         
        public int SaleOrderID { get; set; }
         
        public int ProductionFactoryID { get; set; }
         
        public string RawmaterialStatusNo { get; set; }
         
        public Double OrderQty { get; set; }
         
        public double SaleOrderQty { get; set; }
         
        public string WorkOrderNo { get; set; }
         
        public int ColorTrackNo { get; set; }
         
        public int ReLabNo { get; set; }
         
        public int ReLabOn { get; set; }

         
        public string YarnCount { get; set; }
         
        public string ColorTrackYr { get; set; }
         
        public string GG { get; set; }
         
        public string ConSumptionWestage { get; set; }
         
        public string ChallanNoLotNo { get; set; }

         
        public int ApproveShade { get; set; }

         
        public int UnitID { get; set; }
         
        public string UnitName { get; set; }
         
        public Double ReceiveQty { get; set; }
         
        public Double Balance { get; set; }
         
        public string PINo { get; set; }
         
        public string ProductionFactoryName { get; set; }
         
        public string ProductName { get; set; }
         
        public string Remarks { get; set; }
         
        public string SupplierName { get; set; }
         
        public string DyeingOrderNo { get; set; }
         
        public string ColorName { get; set; }
         
        public string StyleNo { get; set; }
         
        public string ApprovedBy { get; set; }

         
        public DateTime ApprovedDate { get; set; }
         
        public string MerchandiserName { get; set; }
         
        public string SupplierContactPerson { get; set; }
         
        public string BuyerName { get; set; }
         
        public string Note { get; set; }
         
        public Double TotalAmount { get; set; }
         
        public DateTime PIReceiveDate { get; set; }
         
        public DateTime LCDate { get; set; }
         
        public DateTime ReceiveDate { get; set; }
         
        public DateTime ShipmentDate { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         
        public List<Employee> Employees { get; set; }
         
        public List<Contractor> BuyerList { get; set; }
         
        public List<ProductCategory> ProductCategorys { get; set; }
        public List<BusinessSession> BusinessSessions { get; set; }
         
        public string ApproveShadeName { get; set; }

        public string LabDipNo
        {
            get
            {
                if (ReLabOn > 0)
                {
                    return ColorTrackNo + "-" + ReLabNo + "/" + ColorTrackYr;
                }
                else
                {
                    return ColorTrackNo + "/" + ColorTrackYr;
                }


            }
        }
         
        public List<Contractor> SupplierList { get; set; }

         
        public List<Contractor> ProductionFactoryList { get; set; }


         
        public List<User> Users { get; set; }
          
        public List<RawmaterialStatus> RawmaterialStatuList { get; set; }

         
        public Company Company { get; set; }

        public string PIReceiveDateInString
        {
            get
            {
                if (PIReceiveDate == DateTime.MinValue)
                {

                    return "";
                }
                else
                {
                    return PIReceiveDate.ToString("dd MMM yyyy");
                }

            }
        }

        public string LCDateInString
        {
            get
            {
                if (LCDate == DateTime.MinValue)
                {

                    return "";
                }
                else
                {
                    return LCDate.ToString("dd MMM yyyy");
                }
             

            }
        }

        public string ReceiveDateInString
        {
            get
            {
                if (ReceiveDate == DateTime.MinValue)
                {

                    return "";
                }
                else
                {

                    return ReceiveDate.ToString("dd MMM yyyy");

                }

            }
        }

        public string OrderQtyInString
        {
            get
            {
                if (this.OrderQty == 0)
                {
                    return "";
                }
                else
                {
                    return Global.MillionFormat(this.OrderQty);
                }
            }
        }

        public string ReceiveQtyInString
        {
            get
            {
                if (this.OrderQty == 0)
                {
                    return "";
                }
                else
                {
                    return Global.MillionFormat(this.ReceiveQty);
                }
            }
        }

        public string BalanceInString
        {
            get
            {
                if (this.OrderQty == 0)
                {
                    return "";
                }
                else
                {
                    return Global.MillionFormat(this.Balance);
                }
            }
        }


        #endregion

        #region Functions
      
        public static List<RawmaterialStatus> GetYarnBySaleOrderIDs(string sSaleOrderIDs, long nUserID)
        {
            return RawmaterialStatus.Service.GetYarnBySaleOrderIDs(sSaleOrderIDs, nUserID);
        }

        public static List<RawmaterialStatus> GetAccessoriesBySaleOrderIDs(string sSaleOrderIDs, long nUserID)
        {
            return RawmaterialStatus.Service.GetAccessoriesBySaleOrderIDs(sSaleOrderIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
 
        internal static IRawmaterialStatusService Service
        {
            get { return (IRawmaterialStatusService)Services.Factory.CreateService(typeof(IRawmaterialStatusService)); }
        }


        #endregion

        
    }
    #endregion

    #region IRawmaterialStatus interface
     
    public interface IRawmaterialStatusService
    {
       
         
        List<RawmaterialStatus> GetYarnBySaleOrderIDs(string sSaleOrderIDs, Int64 nUserID);
         
        List<RawmaterialStatus> GetAccessoriesBySaleOrderIDs(string sSaleOrderIDs, Int64 nUserID);

    }
    #endregion
}
