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
    public class FabricExecutionOrderFabric
    {
        public FabricExecutionOrderFabric()
        {
            FEOFID = 0;
            FEOID = 0;
            ExportPIDetailID = 0;
            Qty = 0; //ExportPIDetail Remaming Qty (this qty use for calculation)
            UnitPrice = 0;
            Amount = 0;
            FabricID = 0;
            ProductID = 0;
            FactoryID = 0;
            ExportPIID = 0;
            MUnitID = 0;
            FEONo = "";
            PINo = "";
            PIDate = DateTime.Today;
            FabricNo = "";
            LCNo = "";
            LCDate = DateTime.Today;
            FactoryName = "";
            IsInHouse = true;
            OrderType = EnumOrderType.None;
            ColorInfo = "";
            ErrorMessage = "";

            FEO_BuyerID = 0;
            FEO_FabricID = 0;
            ExportPIIDs = "";

            Description = "";
            StyleNo = "";
            BuyerReference = "";
            ProductCode = "";
            ProductName = "";
            ProductCount = "";
            MUName = "";
            Currency = "";
            FabricWidth = "";
            Construction = "";
            ProcessType = 0;
            FabricWeave = 0;
            FinishType = 0;
            ProcessTypeName = "";
            FabricWeaveName = "";
            FinishTypeName = "";
            ExportPiTotalAmount = 0;
            BuyerName = "";
            VersionNo = 0;
            MaxQty = 0;
            IsCheckMaxQty = false;
            FabricDeliveryOrderQty = 0;
            FEOQty = 0; //(Used only for display)
            PIDetailQty = 0; //(Used only for display)
        }

        #region Properties
        public int FEOFID { get; set; }
        public int FEOID { get; set; }
        public int ExportPIDetailID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string ErrorMessage { get; set; }
        public int FEO_BuyerID { get; set; }
        public int FEO_FabricID { get; set; }
        public string ExportPIIDs { get; set; }
        #endregion

        #region Derive Properties
        public EnumOrderType OrderType { get; set; }
        public bool IsInHouse { get; set; }
        public string FEONo { get; set; }
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public string FabricNo { get; set; }
        public string LCNo { get; set; }
        public DateTime LCDate { get; set; }
        public string FactoryName { get; set; }
        public int FabricID { get; set; }
        public int ProductID { get; set; }
        public int FactoryID { get; set; }
        public int ExportPIID { get; set; }
        public int MUnitID { get; set; }
        public string Description { get; set; }
        public string StyleNo { get; set; }
        public string BuyerReference { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCount { get; set; }
        public string MUName { get; set; }
        public string Currency { get; set; }
        public string FabricWidth { get; set; }
        public string Construction { get; set; }
        public string ColorInfo { get; set; }
        public int ProcessType { get; set; }
        public int FabricWeave { get; set; }
        public int FinishType { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string FinishTypeName { get; set; }
        public double ExportPiTotalAmount { get; set; }
        public string BuyerName { get; set; }
        public int VersionNo { get; set; }
        public double MaxQty { get; set; }
        public bool IsCheckMaxQty { get; set; }
        public double FabricDeliveryOrderQty { get; set; }
        public double FEOQty { get; set; }
        public double PIDetailQty { get; set; }
        //public string OrderNo
        //{
        //    get
        //    {
        //        string sPrifix = "";
        //        if (this.FEOID > 0)
        //        {
        //            if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

        //            if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
        //            else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
        //            else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
        //            else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
        //            else { sPrifix = sPrifix + "-"; }

        //            return sPrifix + this.FEONo;

        //        }
        //        else return "";
        //    }
        //}
        public string FEOQtySt
        {
            get
            {
                return Global.GetValue(this.FEOQty);
            }
        }
        public string PIDetailQtySt
        {
            get
            {
                return Global.GetValue(this.PIDetailQty);
            }
        }
        public string QtySt
        {
            get
            {
                return Global.GetValue(this.Qty);
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public double YetToFabricDeliveryOrderQty { get { return this.Qty - this.FabricDeliveryOrderQty; } }
        #endregion

        #region Functions

        public static List<FabricExecutionOrderFabric> Gets(long nUserID)
        {
            return FabricExecutionOrderFabric.Service.Gets(nUserID);
        }
        public static List<FabricExecutionOrderFabric> Gets(string sSQL, long nUserID)
        {
            return FabricExecutionOrderFabric.Service.Gets(sSQL, nUserID);
        }
        public static List<FabricExecutionOrderFabric> Gets(int nFEOID, long nUserID)
        {
            return FabricExecutionOrderFabric.Service.Gets(nFEOID, nUserID);
        }


        public FabricExecutionOrderFabric Get(int nId, long nUserID)
        {
            return FabricExecutionOrderFabric.Service.Get(nId, nUserID);
        }

        public FabricExecutionOrderFabric Save(long nUserID)
        {
            return FabricExecutionOrderFabric.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricExecutionOrderFabric.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricExecutionOrderFabricService Service
        {
            get { return (IFabricExecutionOrderFabricService)Services.Factory.CreateService(typeof(IFabricExecutionOrderFabricService)); }
        }
        #endregion
    }

    #region IFabricExecutionOrderFabric interface

    public interface IFabricExecutionOrderFabricService
    {
        FabricExecutionOrderFabric Get(int id, long nUserID);
        List<FabricExecutionOrderFabric> Gets(long nUserID);
        List<FabricExecutionOrderFabric> Gets(string sSQL, long nUserID);
        List<FabricExecutionOrderFabric> Gets(int nFEOID, long nUserID);
        string Delete(int id, long nUserID);
        FabricExecutionOrderFabric Save(FabricExecutionOrderFabric oFabricExecutionOrderFabric, long nUserID);
    }
    #endregion
}
