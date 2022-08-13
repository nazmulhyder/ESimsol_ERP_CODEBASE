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
    #region DUDeliveryOrderDC
    
    public class DUDeliveryOrderDC : BusinessObject
    {
        public DUDeliveryOrderDC()
        {
            DUDeliveryOrderID = 0;
            DONo = "";
            ContractorID = 0;
            ContactPersonnelID = 0;
            ApproveBy = 0;
            DODate=DateTime.Now;
            Note = "";
            ApproveDate = DateTime.Now;
            Qty = 0.0;
            DeliveryDate = DateTime.Now;
            Qty_DC = 0;
            OrderType = 0;
            OrderID = 0;
            DOStatus = 0;
            ApproveDate = DateTime.Now;
            ExportPIID = 0;
            IsRaw = false;
            DeliveryDate = DateTime.Now;
            ErrorMessage = "";
            DeliveryPoint = "";
            ContactPersonnelName = "";
            ContractorName = "";
            DeliveryToName = "";
            MKTPName = "";
            ApproveByName = "";
            PreaperByName = "";
            ExportLCNo = "";
            ExportPINo = "";
            OrderNo = "";
            DUDeliveryChallan = new DUDeliveryChallan();
        }
       
        #region Properties

        public int DUDeliveryOrderID { get; set; }
        public string DONo { get; set; }
        public DateTime DODate { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public string Note { get; set; }
        public int OrderType { get; set; }
        public int OrderID { get; set; }
        public int DOStatus { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public double Qty { get; set; }
        public double Qty_DC { get; set; }
        public int ExportPIID { get; set; }
        public bool IsRaw { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ErrorMessage { get; set; }
        public string DeliveryPoint { get; set; }
        public string ContactPersonnelName { get; set; }
      
        #endregion

        #region Derived Property
        public DUDeliveryChallan DUDeliveryChallan { get; set; }
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }
        public string MKTPName { get; set; }
        public string ApproveByName { get; set; }
        public string PreaperByName { get; set; }
        public string ExportLCNo { get; set; }
        public string ExportPINo { get; set; }
        
        public string OrderNo { get; set; }
        public string DOStatusSt
        {
            get
            {
                return ((EnumDOStatus)this.DOStatus ).ToString();
            }
        }
        public string DODateSt
        {
            get
            {
                return DODate.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateSt
        {
            get
            {
                return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }

        public string OrderTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderType)this.OrderType);
            }
        }
        public string DONoFull
        {
            get
            {
                if(this.OrderType==(int)EnumOrderType.BulkOrder)
                {
                    return "BDO"+""+this.DONo;
                }
                else 
                {
                    return "SDO" + "" + this.DONo;
                }
            }
        }
  
        public string QtySt
        {
            get
            {
                { return Global.MillionFormat(this.Qty); }

            }
        }
        public double OrderValue { get; set; }
        public double OrderQty { get; set; }
        public double TotalDOQty { get; set; }
        #endregion

        #region Functions
        public  DUDeliveryOrderDC Get(int nId, long nUserID)
        {
            return DUDeliveryOrderDC.Service.Get(nId, nUserID);
        }
        public static List<DUDeliveryOrderDC> Gets(string sSQL, long nUserID)
        {
            return DUDeliveryOrderDC.Service.Gets(sSQL, nUserID);
        }
        public static List<DUDeliveryOrderDC> GetsByNo(string sOrderNo, long nUserID)
        {
            return DUDeliveryOrderDC.Service.GetsByNo(sOrderNo, nUserID);
        }
        public static List<DUDeliveryOrderDC> GetsBy(string sContractorID, long nUserID)
        {
            return DUDeliveryOrderDC.Service.GetsBy(sContractorID, nUserID);
        }
        public static List<DUDeliveryOrderDC> GetsByPI(int nExportPIID, long nUserID)
        {
            return DUDeliveryOrderDC.Service.GetsByPI(nExportPIID, nUserID);
        }
    
        #endregion

        #region ServiceFactory
        internal static IDUDeliveryOrderDCService Service
        {
            get { return (IDUDeliveryOrderDCService)Services.Factory.CreateService(typeof(IDUDeliveryOrderDCService)); }
        }
        #endregion
    }


    #region IDUDeliveryOrderDC interface
    
    public interface IDUDeliveryOrderDCService
    {
        DUDeliveryOrderDC Get(int id, long nUserID);
        List<DUDeliveryOrderDC> Gets(string sSQL, long nUserID);
        List<DUDeliveryOrderDC> GetsByNo(string sOrderNo, long nUserID);
        List<DUDeliveryOrderDC> GetsBy(string sContractorIDs, long nUserID);
        List<DUDeliveryOrderDC> GetsByPI(int nExportPIID, long nUserID);
       
        
       
    }
    #endregion

    #endregion
}
