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
    #region FabricDeliveryOrderDetail

    public class FabricDeliveryOrderDetail : BusinessObject
    {
        #region  Constructor
        public FabricDeliveryOrderDetail()
        {
            FDODID = 0;
            FDOID = 0;
            FabricID = 0;
            ProductID = 0;
            ExportPIDetailID = 0;
            Qty = 0;
            MUID = 0;
            UnitPrice = 0;
            DONo = "";
            ErrorMessage = "";
            FabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            FEOID = 0;
            FEONo = "";
            ProcessType = "";// EnumFabricProcessType.None;
            FabricWeave = "";//EnumFabricWeave.None;
            FinishType = "";//EnumFinishType.None;
            IsInHouse = true;
            OrderType = EnumFabricRequestType.None;
            
            //ExportLCID = 0;
            PINo = "";
            CurrencyID = 0;
            LCNo = "";
            StyleNo = "";
            BuyerRef = "";
            PIDescription = "";
            Qty_PI = 0;
            FabricProductName = "";
            FabricWidth = "";
            Qty_DC = 0;
            MKTPerson = "";
            ExeNo = "";
            BCPID = 0;
            FabricDesignName = "";
            BuyerCPName = "";
            ExportPIID = 0;
            Currency = "";
            Shrinkage = "";
            DODate = DateTime.MinValue;
            DOPriceType = EnumDOPriceType.General;
            Qty_RC = 0;
        }
        #endregion

        #region Properties

        public int FDODID { get; set; }
        public int FDOID { get; set; }
        public int FabricID { get; set; }
      
        public double Qty { get; set; }
        public int MUID { get; set; }
        public double UnitPrice { get; set; }
        public int ContractorID { get; set; }
        public int BuyerID { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string MKTPerson { get; set; }
        public string ErrorMessage { get; set; }
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public string DONo { get; set; }
        public string ExeNo { get; set; }
        public bool IsInHouse { get; set; }
        //public int ExportLCID { get; set; }
        public int ExportPIID { get; set; }
        public int ExportPIDetailID { get; set; }
        public string PINo { get; set; }
        public string LCNo { get; set; }
        public double Qty_P0 { get; set; }
        public DateTime DODate { get; set; }
        #endregion

        #region Derive Properties
        public int BCPID { get; set; }
        public string BuyerCPName { get; set; }
        public string PIDescription { get; set; }
        public string StyleNo { get; set; }
        public string BuyerRef { get; set; }
        public string HLReference { get; set; }
        public string FinishType { get; set; }
        public string Shrinkage { get; set; }
        public EnumFabricRequestType OrderType { get; set; }
        public int FinishTypeInInt { get; set; }
        public string ProcessType { get; set; }
        public int ProcessTypeInInt { get; set; }
        public string FabricWeave { get; set; }
        public string FabricDesignName { get; set; }
        public int FabricWeaveInInt { get; set; }
        public FabricDeliveryOrder FDO { get; set; }
        public string Currency { get; set; }
        public int CurrencyID { get; set; }
        public double Qty_PI { get; set; }
        public double Qty_DC { get; set; }
        public double Qty_RC { get; set; }
        public double Qty_DO { get; set; }
        public string FabricProductName { get; set; }
        public string FabricWidth { get; set; }
        public EnumDOPriceType DOPriceType { get; set; }
        public string DODateSt
        {
            get
            {
                return this.GetDate(this.DODate);
            }
        }
        public string OrderTypeSt
        {
            get
            {
                return EnumObject.jGet(this.OrderType); ;
            }
        }
        public string DOPriceTypeSt
        {
            get
            {
                return EnumObject.jGet(this.DOPriceType); 
            }
        }
        private string GetDate(DateTime dDate)
        {
            DateTime MinValue = new DateTime(1900, 01, 01);
            DateTime MinValue1 = new DateTime(0001, 01, 01);
            if (dDate == MinValue || dDate == MinValue1 || dDate == DateTime.MinValue)
            {
                return "-";
            }
            else
            {
                return dDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderNo
        {
            get
            {
                if (this.FEOID > 0)
                {
                    //if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SWC";
                    //if (this.OrderType == EnumFabricRequestType.Bulk) { sPrifix = sPrifix + "BLK-"; }
                    //else if (this.OrderType == EnumFabricRequestType.Analysis) { sPrifix = sPrifix + "EXT-"; }
                    //else if (this.OrderType == EnumFabricRequestType.Labdip) { sPrifix = sPrifix + "DEV-"; }
                    //else if (this.OrderType == EnumFabricRequestType.Sample) { sPrifix = sPrifix + "SMS-"; }
                    //else { sPrifix = sPrifix + "-"; }
                    return  this.FEONo + ", DISPO: " + this.ExeNo;
                }
                else return "";
            }
        }
      

        public List<FabricDeliveryOrderDetail> FabricDeliveryOrderDetails { get; set; }
        public string FabricNo { get; set; }
      
        public string Construction { get; set; }
        public string ColorInfo { get; set; }
        public string MUName { get; set; }
        public string ContractorAddress { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string FabricNoWithRemainingQty
        {
            get
            {
                return this.OrderNo + "[" + this.FabricNo + "] [Bal:" + Global.MillionFormat(this.Qty + this.Qty_RC- this.Qty_DC) + "(Y)]";
            }
        }
        public string RemainingQtyWithExeConstruction
        {
            get
            {
                return this.OrderNo + "[" + this.Construction + "] [DO-Bal:" + Global.MillionFormat(this.Qty + this.Qty_RC - this.Qty_DC) + "(Y)]";
            }
        }

        public double Qty_Meter { get; set; } //ForInterFace
        public double UnitPriceYard { get { return this.UnitPrice > 0 ? Math.Round(this.UnitPrice, 5) : 0; } }
        public double UnitPriceMeter { get { return this.UnitPrice > 0 ? Global.GetMeter(this.UnitPrice, 5) : 0; } }
        public double QtyYard { get { return this.Qty > 0 ? Math.Round(this.Qty, 2) : 0; } }
        public double QtyMeter { get { return this.Qty > 0 ? Global.GetMeter(this.Qty, 2) : 0; } }
        public string UnitPriceMeterInString { get { return Global.MillionFormat(this.UnitPriceMeter); } }
        public string UnitPriceYardInString { get { return Global.MillionFormat(this.UnitPriceYard); } }
        public string QtyMeterInString { get { return Global.MillionFormat(this.QtyMeter); } }
        public string QtyYardInString { get { return Global.MillionFormat(this.QtyYard); } }
        public double TotalAmount { get { return this.Qty > 0 && this.UnitPrice > 0 ? this.Qty * this.UnitPrice : 0; } }
        public string TotalAmountInString { get { return Global.MillionFormat(this.TotalAmount); } }
        public int MktAccountID { get; set; }
        public int BUID { get; set; }
        public string Weight { get; set; }
        #endregion

        #region Functions
        public static FabricDeliveryOrderDetail Get(int nFDODID, long nUserID)
        {
            return FabricDeliveryOrderDetail.Service.Get(nFDODID, nUserID);
        }
        public static List<FabricDeliveryOrderDetail> Gets(int nFDOID, long nUserID)
        {
            return FabricDeliveryOrderDetail.Service.Gets(nFDOID, nUserID);
        }
        public static List<FabricDeliveryOrderDetail> Gets(string sSQL, long nUserID)
        {
            return FabricDeliveryOrderDetail.Service.Gets(sSQL, nUserID);
        }

        public FabricDeliveryOrderDetail IUD(int nDBOperation, long nUserID)
        {
            return FabricDeliveryOrderDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricDeliveryOrderDetail Save(long nUserID)
        {
            return FabricDeliveryOrderDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricDeliveryOrderDetail.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricDeliveryOrderDetailService Service
        {
            get { return (IFabricDeliveryOrderDetailService)Services.Factory.CreateService(typeof(IFabricDeliveryOrderDetailService)); }
        }
        #endregion



    }
    #endregion


    #region IFabricExecutionOrderDetail interface
    public interface IFabricDeliveryOrderDetailService
    {
        FabricDeliveryOrderDetail Get(int nFDODID, long nUserID);
        List<FabricDeliveryOrderDetail> Gets(int nFDOID, long nUserID);
        List<FabricDeliveryOrderDetail> Gets(string sSQL, long nUserID);
        FabricDeliveryOrderDetail IUD(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail, int nDBOperation, long nUserID);
        FabricDeliveryOrderDetail Save(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
