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

namespace ESimSol.BusinessObjects
{
    #region DeliveryOrderDetail
    public class DeliveryOrderDetail : BusinessObject
    {
        public DeliveryOrderDetail()
        {
            DeliveryOrderDetailID = 0;
            DeliveryOrderID = 0;
            DeliveryOrderLogID = 0;
            DeliveryOrderDetailLogID = 0;
            ProductID = 0;
            RefDetailID = 0;
            Qty = 0;
            Note = "";
            MUnitID = 0;
            YetToDeliveryOrderQty = 0;
            YetToDeliveryChallanQty = 0;
            ColorID = 0;
            ColorName = "";
            PTUUnit2ID = 0;
            StyleNo = "";
            Measurement = "";
            SizeName = "";
            DONo = "";
            DeliveryDate = DateTime.Today;
            ErrorMessage = "";
        }

        #region Property
        public int DeliveryOrderDetailID { get; set; }
        public int DeliveryOrderID { get; set; }
        public int DeliveryOrderLogID { get; set; }
        public int DeliveryOrderDetailLogID { get; set; }
        public int ProductID { get; set; }
        public int RefDetailID { get; set; }
        public double Qty { get; set; }
        public string Note { get; set; }
        public int MUnitID { get; set; }
        public double YetToDeliveryOrderQty { get; set; }
        public double YetToDeliveryChallanQty { get; set; }
        public int  ColorID { get; set; }
        public string ColorName { get; set; }
        public int PTUUnit2ID { get; set; }
        public string StyleNo { get; set; }
        public string Measurement { get; set; }
        public string SizeName { get; set; }
        public string DONo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MUnit { get; set; }
        public string DeliveryDateInString
        {
            get
            {
                return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string QtyInString
        {
            get
            {
                return this.Qty + " " + this.MUnit;
            }
        }
        #endregion

        #region Functions

        public DeliveryOrderDetail Get(int id, long nUserID)
        {
            return DeliveryOrderDetail.Service.Get(id, nUserID);
        }
        public static List<DeliveryOrderDetail> Gets(int nDOID, long nUserID)
        {
            return DeliveryOrderDetail.Service.Gets(nDOID, nUserID);
        }

        public static List<DeliveryOrderDetail> Gets(string sSQL, long nUserID)
        {
            return DeliveryOrderDetail.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDeliveryOrderDetailService Service
        {
            get { return (IDeliveryOrderDetailService)Services.Factory.CreateService(typeof(IDeliveryOrderDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IDeliveryOrderDetail interface
    public interface IDeliveryOrderDetailService
    {
        DeliveryOrderDetail Get(int id, Int64 nUserID);
        List<DeliveryOrderDetail> Gets(int nDOID, Int64 nUserID);
        List<DeliveryOrderDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
