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
    public class FabricYarnOrder : BusinessObject
    {
        public FabricYarnOrder()
        {
            FYOID = 0;
            FEOID = 0;
            ProductID = 0;
            Qty = 0;
            RequestBy = 0;
            RequestDate = DateTime.MinValue;
            UnitPrice = 0;
            ErrorMessage = string.Empty;
            OrderType = EnumOrderType.None;
            ProcessName = string.Empty;
            IsYarnDyed = false;
            Params = string.Empty;
            FYOs = new List<FabricYarnOrder>();
            //FYOAllocates = new List<FabricYarnOrderAllocate>();
        }
      
        #region Properties
        public int FYOID { get; set; }
        public int FEOID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public int RequestBy { get; set; }
        public double UnitPrice { get; set; }
        public DateTime RequestDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region dreiverd Properties
        public List<FabricYarnOrder> FYOs { get; set; }
        public string FEONo { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        public string ProcessName { get; set; }
        public bool IsYarnDyed { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string RequestByName { get; set; }
        public string ContractorName { get; set; }
        public double AllocationQty { get; set; }
        public double RemainingQty { get { return this.Qty - this.AllocationQty; } }

        public double QtyInLBS { get { return Global.GetLBS(this.Qty,2); } }

        public double UnitPriceInLBS
        {
            get
            {

                if (this.Qty > 0 && this.UnitPrice > 0)
                    return Math.Round((this.Qty * this.UnitPrice) / this.QtyInLBS,2);
                else
                    return 0;
            }
        }
        public string RequestDateStr { get { return (this.RequestDate == DateTime.MinValue) ? "" : this.RequestDate.ToString("dd MMM yyyy"); } }

        public double TotalPrice
        {
            get
            {
                return (this.UnitPrice > 0 && this.Qty > 0) ? (this.UnitPrice * this.Qty) : 0;
            }
        }

        public double TotalPriceLbs
        {
            get
            {
                return (this.UnitPriceInLBS > 0 && this.QtyInLBS > 0) ? (this.UnitPriceInLBS * this.QtyInLBS) : 0;
            }
        }
        //public List<FabricYarnOrderAllocate> FYOAllocates { get; set; }
        public string OrderNo
        {
            get
            {
                string sPrifix = "";
                if (this.FEOID > 0)
                {
                    //if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

                    //if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
                    //else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
                    //else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
                    //else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
                    //else { sPrifix = sPrifix + "-"; }

                    return sPrifix + this.FEONo;

                }
                else return "";
            }
        }
        #endregion

        #region Functions

        public static FabricYarnOrder Get(int nFYOID, long nUserID)
        {
            return FabricYarnOrder.Service.Get(nFYOID, nUserID);
        }
        public static List<FabricYarnOrder> Gets(string sSQL, long nUserID)
        {
            return FabricYarnOrder.Service.Gets(sSQL, nUserID);
        }
        public FabricYarnOrder IUD(int nDBOperation, long nUserID)
        {
            return FabricYarnOrder.Service.IUD(this, nDBOperation, nUserID);
        }

        public FabricYarnOrder RequestAndSave(long nUserID)
        {
            return FabricYarnOrder.Service.RequestAndSave(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricYarnOrderService Service
        {
            get { return (IFabricYarnOrderService)Services.Factory.CreateService(typeof(IFabricYarnOrderService)); }
        }

        #endregion
    }

    #region  IFabricYarnOrder interface
    public interface IFabricYarnOrderService
    {
        FabricYarnOrder Get(int nFYOID, Int64 nUserID);
        List<FabricYarnOrder> Gets(string sSQL, Int64 nUserID);
        FabricYarnOrder IUD(FabricYarnOrder oFabricYarnOrder, int nDBOperation, Int64 nUserID);
        FabricYarnOrder RequestAndSave(FabricYarnOrder oFabricYarnOrder, Int64 nUserID);
    }
    #endregion
}
