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
    #region DUDeliveryOrderDetail
    
    public class DUDeliveryOrderDetail : BusinessObject
    {
        public DUDeliveryOrderDetail()
        {

            DUDeliveryOrderID = 0;
            DUDeliveryOrderDetailID = 0;
            Qty=0.0;
            DeliveryDate=DateTime.Now;
            Note = "";
            OrderNo = "";
            ErrorMessage = "";
            ProductID = 0;
            ExportSCDetailID = 0;
            DyeingOrderDetailID = 0;
            OrderType = 0;
            Qty_DC = 0;
            ProductName = "";
            ProductNameCode = "";
            ColorName = "";
            MUName = "";
            OrderQty =0;
            DONo = "";
            Lotbalance = 0;
            DeliveryToName = "";
            Qty_RC = 0;

        }



        #region Properties
        public int DUDeliveryOrderDetailID { get; set; }
        public int DUDeliveryOrderID { get; set; }
        public int ProductID { get; set; }
        public int ExportSCDetailID { get; set; }
        public int ContractorID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int OrderType { get; set; }
        public string ColorName { get; set; }
        public double Qty { get; set; }
        public double Qty_DC { get; set; }
        public double Qty_RC { get; set; }
        public double UnitPrice { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
        public string DONo { get; set; }
        public double Lotbalance { get; set; }
        public string DeliveryToName { get; set; }
        public string MUName { get; set; }
        public int Status { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string ProductNameCode { get; set; }
        public string ProductName { get; set; }
        public string OrderNo { get; set; }
        public string DeliveryDateSt
        {
            get
            {
                return DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public double DOQty { get; set; }
        public double OrderQty { get; set; }
        public double YetToDO
        {
            get
            {
                return Math.Round((this.OrderQty - this.DOQty), 2);
            }
        }
        public double YetToDC
        {
            get
            {
                return Math.Round((this.Qty - this.Qty_DC), 2);
            }
        }
        #endregion

    

        #region Functions
        public static DUDeliveryOrderDetail Get(int nId, long nUserID)
        {
            return DUDeliveryOrderDetail.Service.Get(nId, nUserID);
        }

        public static List<DUDeliveryOrderDetail> Gets(int nDUDeliveryOrdeID, long nUserID)
        {
            return DUDeliveryOrderDetail.Service.Gets(nDUDeliveryOrdeID, nUserID);
        }
     
    
        public static List<DUDeliveryOrderDetail> Gets(string sSQL, long nUserID)
        {
            return DUDeliveryOrderDetail.Service.Gets(sSQL, nUserID);
        }
        public DUDeliveryOrderDetail Save(long nUserID)
        {
            return DUDeliveryOrderDetail.Service.Save(this, nUserID);
        }
   
        public string Delete(long nUserID)
        {
            return DUDeliveryOrderDetail.Service.Delete(this, nUserID);
        }
      


        #endregion


        #region ServiceFactory
        internal static IDUDeliveryOrderDetailService Service
        {
            get { return (IDUDeliveryOrderDetailService)Services.Factory.CreateService(typeof(IDUDeliveryOrderDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IDUDeliveryOrderDetail interface
    
    public interface IDUDeliveryOrderDetailService
    {
        
        DUDeliveryOrderDetail Get(int id, long nUserID);
        List<DUDeliveryOrderDetail> Gets(int nDUDeliveryOrderID, long nUserID);
        List<DUDeliveryOrderDetail> Gets(string sSQL, long nUserID);
        string Delete(DUDeliveryOrderDetail oDUDeliveryOrderDetail, long nUserID);
        DUDeliveryOrderDetail Save(DUDeliveryOrderDetail oDUDeliveryOrderDetail, long nUserID);
      

      


    }
    #endregion
}
