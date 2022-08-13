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
    #region FNOrderFabricReceive

    public class FNOrderFabricReceive : BusinessObject
    {
        public FNOrderFabricReceive()
        {
            FNOrderFabricReceiveID = 0;
            FSCDID = 0;
            LotID = 0;
            Qty = 0;
            QtyTrIn = 0;
            QtyTrOut = 0;
            QtyReturn = 0;
            QtyCon = 0;
            Grade = 0;
            FabricReqRollID = 0;
            ReceiveBy = 0;
            ReceiveDate = DateTime.MinValue;
            FabricProcessID = 0;
            ReceiveByName = "";
            LotNo = "";
            FEOID = 0;
            ErrorMessage = "";
            FabricSource = "";
            oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            //TEMP VERIABLE
            FNOrderFabricReceiveID_from = 0;
            FSCDID_from = 0;
            FSCDID_To = 0;
            Qty_from = 0;
            TransferQty = 0;
            tmpReturnQty = 0;
            LotBalance = 0;
            TransferQtyInMtr = 0;
        }

        public int FNOrderFabricReceiveID { get; set; }
        public int FSCDID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public double QtyTrIn { get; set; }
        public double QtyTrOut { get; set; }
        public double QtyReturn { get; set; }
        public double QtyCon{ get; set; }
        public int Grade { get; set; }
        public int FabricReqRollID { get; set; }
        public int ReceiveBy { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ReceiveByName { get; set; }
        public string LotNo { get; set; }
        public string ErrorMessage { get; set; }
        public string FabricSource { get; set; }
        public List<FNOrderFabricReceive> oFNOrderFabricReceives { get; set; }

        #region Temp Veriable
        public int FNOrderFabricReceiveID_from { get; set; }
        public int FSCDID_from { get; set; }
        public int FSCDID_To { get; set; }
        public double Qty_from { get; set; }
        public double TransferQty { get; set; }
        public double tmpReturnQty { get; set; }
        public double LotBalance { get; set; }
        public double TransferQtyInMtr { get; set; }
        #endregion

        #region Derive Value
        //public string InOutTypeSt
        //{
        //    get
        //    {
        //        if (this.InOutType == 101) return "Receive";
        //        else if (this.InOutType == 102) return "Disburse";
        //        else return "";
        //    }
        //}
        //public PurchaseInvoiceProduct PIP { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public EnumOrderType OrderType { get; set; }
        public bool IsInHouse { get; set; }
        public double UnitPrice { get; set; }
        public int WUID { get; set; }
        public int FEOID { get; set; }
        public string FNExONo { get; set; }
        public DateTime IssueDate { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string BuyerName { get; set; }
        public int BuyerID { get; set; }
        public int FabricProcessID { get; set; }
        public string ProcessTypeName { get; set; }
        public string ChallanNo { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public double QtyInMeter
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }
        public string ProductNameCode
        {
            get
            {
                if (this.ProductName == "" && this.ProductCode == "") return "";
                else return this.ProductName + " [" + this.ProductCode + "]";
            }
        }
        public string IssueDateInStr
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ReceiveDateInStr
        {
            get
            {
                return (this.ReceiveBy != 0) ? ReceiveDate.ToString("dd MMM yyyy") : "";
            }
        }

        public string OrderNo
        {
            get
            {
                string sPrifix = "";
                if (this.FEOID > 0)
                {
                    if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

                    if (this.OrderType == EnumOrderType.BulkOrder) { sPrifix = sPrifix + "-BLK-"; }
                    else if (this.OrderType == EnumOrderType.Sampling) { sPrifix = sPrifix + "-EXT-"; }
                    else if (this.OrderType == EnumOrderType.SampleOrder) { sPrifix = sPrifix + "-DEV-"; }
                    else if (this.OrderType == EnumOrderType.SampleOrder_Two) { sPrifix = sPrifix + "-SMS-"; }
                    else { sPrifix = sPrifix + "-"; }

                    return sPrifix + this.FNExONo;

                }
                else return this.FNExONo;
            }
        }

        public double YetToTransferQty
        {
            get
            {
                return ((this.Qty+this.QtyTrIn) - (this.QtyTrOut+this.QtyReturn + this.QtyCon));
            }
        }
        public double AlreadyTransferQty
        {
            get
            {
                return this.QtyTrOut;
            }
        }

        #endregion

        #region Functions

        public static FNOrderFabricReceive Get(int nFNExeFRID, Int64 nUserID)
        {
            return FNOrderFabricReceive.Service.Get(nFNExeFRID, nUserID);
        }
        public static List<FNOrderFabricReceive> Gets(string sSQL, Int64 nUserID)
        {
            return FNOrderFabricReceive.Service.Gets(sSQL, nUserID);
        }
        public FNOrderFabricReceive IUD(int nDBOperation, Int64 nUserID)
        {
            return FNOrderFabricReceive.Service.IUD(this, nDBOperation, nUserID);
        }
        public List<FNOrderFabricReceive> Receive(List<FNOrderFabricReceive> oFNOrderFabricReceives, Int64 nUserID)
        {
            return FNOrderFabricReceive.Service.Receive(oFNOrderFabricReceives, nUserID);
        }
        public List<FNOrderFabricReceive> SaveList(List<FNOrderFabricReceive> oFNOrderFabricReceives, long nUserID)
        {
            return FNOrderFabricReceive.Service.SaveList(oFNOrderFabricReceives, nUserID);
        }



        //public FNOrderFabricReceive FNExeorderReceiveByChallan(long nUserID)
        //{
        //    return FNOrderFabricReceive.Service.FNExeorderReceiveByChallan(this, nUserID);
        //}
        #endregion

        #region ServiceFactory

        internal static IFNOrderFabricReceiveService Service
        {
            get { return (IFNOrderFabricReceiveService)Services.Factory.CreateService(typeof(IFNOrderFabricReceiveService)); }
        }
        #endregion
    }
    #endregion
    #region IFNOrderFabricReceive interface

    public interface IFNOrderFabricReceiveService
    {
        FNOrderFabricReceive Get(int nFNExeFRID, Int64 nUserID);
        List<FNOrderFabricReceive> Gets(string sSQL, Int64 nUserID);
        FNOrderFabricReceive IUD(FNOrderFabricReceive oFNExeOFR, int nDBOperation, Int64 nUserID);
        List<FNOrderFabricReceive> Receive(List<FNOrderFabricReceive> oFNOrderFabricReceives, Int64 nUserID);
        List<FNOrderFabricReceive> SaveList(List<FNOrderFabricReceive> oFNOrderFabricReceives, Int64 nUserID);



        //FNOrderFabricReceive FNExeorderReceiveByChallan(FNOrderFabricReceive oFNExeOFR, Int64 nUserID);
    }
    #endregion
}
