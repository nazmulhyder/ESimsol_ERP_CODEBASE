using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
   public class FabricYarnDeliveryOrderDetail :BusinessObject
    {
        #region FabricYarnDeliveryOrderDetail
        public FabricYarnDeliveryOrderDetail()
        {
            FYDODetailID =0;
            FYDOID =0;
            ProductID =0;
            Qty =0;
            UnitPrice=0;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            FYDO = null;

        }
        #endregion

        #region Properties
        public int  FYDODetailID {get;set;}
        public int  FYDOID {get;set;}
        public int  ProductID {get;set;}
        public double Qty {get;set;}
        public double UnitPrice { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public FabricYarnDeliveryOrder FYDO { get; set; }
        #endregion

        #region Deriverd Properties 
		public string  ProductCode { get; set; }
        public string ProductName { get; set; }
        public double ChallanQty { get; set; }

        public int FEOID { get; set; }
        public string FEONO { get; set; }

        public double RemainingQty
        {
            get
            {
                return this.Qty - this.ChallanQty;
            }
        }
        public double  QtyInLBS { get { return Global.GetLBS(this.Qty,2); }}
        public double UnitPriceInLBS
        {
            get
            {

                if (this.Qty > 0 && this.UnitPrice > 0)
                    return Math.Round((this.Qty * this.UnitPrice) / this.QtyInLBS, 2);
                else
                    return 0;
            }
        }

        public double TotalPrice
        {
            get
            {
                return this.Qty * this.UnitPrice;
            }
        }

        public double TotalPriceInLBS
        {
            get
            {
                return this.QtyInLBS * this.UnitPriceInLBS;
            }
        }
        #endregion

        #region Functions

        public static FabricYarnDeliveryOrderDetail Get(int nFYDODetailID, long nUserID)
        {
            return FabricYarnDeliveryOrderDetail.Service.Get(nFYDODetailID, nUserID);
        }
        public static List<FabricYarnDeliveryOrderDetail> Gets(string sSQL, long nUserID)
        {
            return FabricYarnDeliveryOrderDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricYarnDeliveryOrderDetail IUD(int nDBOperation, long nUserID)
        {
            return FabricYarnDeliveryOrderDetail.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricYarnDeliveryOrderDetailService Service
        {
            get { return (IFabricYarnDeliveryOrderDetailService)Services.Factory.CreateService(typeof(IFabricYarnDeliveryOrderDetailService)); }
        }

        #endregion
    }
    #region IFabricYarnDeliveryOrderDetail

   public interface IFabricYarnDeliveryOrderDetailService
    {

        FabricYarnDeliveryOrderDetail Get(int nFYDODetailID, Int64 nUserID);
        List<FabricYarnDeliveryOrderDetail> Gets(string sSQL, Int64 nUserID);
        FabricYarnDeliveryOrderDetail IUD(FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
