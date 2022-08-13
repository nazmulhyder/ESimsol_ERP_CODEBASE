using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUDeliveryStock
    public class DUDeliveryStock : BusinessObject
    {
        #region  Constructor
        public DUDeliveryStock()
        {
            LotNo = "";
            Buyer = "";
            Qty = 0;
            Product = "";
            FactN = "";
            PINo = "";
            Qty_Tr = 0;
        }
        #endregion

        #region Properties
        public string LotNo { get; set; }
        public string OrderNo { get; set; }
        public string PINo { get; set; }
        public string RSDate { get; set; }
        public string Buyer { get; set; }
        public string FactN { get; set; }
        public string Product { get; set; }
        [DataMember]
        public double Qty { get; set; }
        public double Qty_Tr { get; set; }
        public string IsManage { get; set; }
        public string WorkingUnit { get; set; }
        public int WorkingUnitID { get; set; }

        public int LotID { get; set; }
        public int OrderType { get; set; }
        private string _sOrderNoFull = "";
        public string OrderNoFull
        {
            get
            {
                if (this.OrderType == (int)EnumOrderType.SampleOrder)
                {
                    _sOrderNoFull = "BSY-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.BulkOrder)
                {
                    _sOrderNoFull = "BPO-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    _sOrderNoFull = "BRD-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.ClaimOrder)
                {
                    _sOrderNoFull = "BCO-" + this.OrderNo;
                }
                else if (this.OrderType == (int)EnumOrderType.Sampling)
                {
                    _sOrderNoFull = "SP-" + this.OrderNo;
                }
                return _sOrderNoFull;
            }
        }
        public string ErrorMessage { get; set; }

        #endregion

        #region Functions
        public static List<DUDeliveryStock> Gets(int nOrderType, int nWorkingUnitID, string sSQL, long nUserID)
        {
            return DUDeliveryStock.Service.Gets(nOrderType, nWorkingUnitID, sSQL, nUserID);
        }
        public string SendToRequsitionToDelivery(DUDeliveryStock oDUDeliveryStock, long nUserID)
        {
            return DUDeliveryStock.Service.SendToRequsitionToDelivery(oDUDeliveryStock, nUserID);
        }
        public static List<DUDeliveryStock> GetsAvalnDelivery(int nOrderType, int nWorkingUnitID, string sSQL, long nUserID)
        {
            return DUDeliveryStock.Service.GetsAvalnDelivery(nOrderType, nWorkingUnitID, sSQL, nUserID);
        }

        #endregion

        #region Non DB Function

        #endregion
        #region ServiceFactory
        internal static IDUDeliveryStockService Service
        {
            get { return (IDUDeliveryStockService)Services.Factory.CreateService(typeof(IDUDeliveryStockService)); }
        }

        #endregion
    }
    #endregion


    #region IDUDeliveryStock interface
    public interface IDUDeliveryStockService
    {
        List<DUDeliveryStock> Gets(int nOrderType, int nWorkingUnitID, string sSQL, Int64 nUserID);
        string SendToRequsitionToDelivery(DUDeliveryStock oDUDeliveryStock, long nUserID);
        List<DUDeliveryStock> GetsAvalnDelivery(int nOrderType, int nWorkingUnitID, string sSQL, Int64 nUserID);
        

    }

    #endregion

}