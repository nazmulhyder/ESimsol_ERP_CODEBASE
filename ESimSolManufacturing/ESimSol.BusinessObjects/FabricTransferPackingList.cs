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
    public class FabricTransferPackingList
    {
        public FabricTransferPackingList()
        {
            FTPListID = 0;
            FTNID = 0;
            FEOID = 0;
            Note = "";
            StoreID = 0;
            PackingListDate = DateTime.Today;
            FEONo = "";
            ErrorMessage = "";

            CountRoll = 0;
            TotalRollQty = 0;
            FTPLDetails = new List<FabricTransferPackingListDetail>();
            LotNo = "";
            IsInHouse = true;
            OrderType = EnumOrderType.None;
            FabType = "";
            Construction = "";
            FTNNo = "";
            LotIDs = "";
            Params = "";
            GreyWidth = string.Empty;
        }

        #region Properties
        public int FTPListID { get; set; }
        public int FTNID { get; set; }
        public int FEOID { get; set; }
        public string Note { get; set; }
        public int StoreID { get; set; }
        public DateTime PackingListDate { get; set; }
        public string FEONo { get; set; }
        public string ErrorMessage { get; set; }
        public string LotNo { get; set; }
        public string Params { get; set; }
        public string Color { get; set; }
        public string FinishWidth { get; set; }
        public string WarpLot { get; set; }
        public string WeftLot { get; set; }
        public string GreyWidth { get; set; }

        #endregion

        #region Derive Properties 
        public string FTNNo { get; set; }
        public string LotIDs { get; set; }
        public string Construction { get; set; }
        public string FabType { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        public string BuyerName { get; set; }
        public int CountRoll { get; set; }
        public double TotalRollQty { get; set; }
        public DateTime NoteDate { get; set; }
        public double DetailQty { get; set; }
        public List<FabricTransferPackingListDetail> FTPLDetails { get; set; }
        public string TotalRollQtySt
        {
            get
            {
                if (this.TotalRollQty < 0) return "(" + Global.MillionFormat(this.TotalRollQty * (-1)) + ")";
                else if (this.TotalRollQty == 0) return "-";
                else return Global.MillionFormat(this.TotalRollQty);
            }
        }
        public string TotalRollQtyInMeterSt
        {
            get
            {
                double nQtyInMeter = Global.GetMeter(this.TotalRollQty, 2);
                if (nQtyInMeter < 0) return "(" + Global.MillionFormat(nQtyInMeter * (-1)) + ")";
                else if (nQtyInMeter == 0) return "-";
                else return Global.MillionFormat(nQtyInMeter);
            }
        }
        public string PackingListDateSt
        {
            get
            {
                return this.PackingListDate.ToString("dd MMM yyyy");
            }
        }
        public string NoteDateSt
        {
            get
            {
                if (NoteDate == DateTime.MinValue) return " ";
                return this.NoteDate.ToString("dd MMM yyyy");
            }
        }

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
        public static List<FabricTransferPackingList> Gets(long nUserID)
        {
            return FabricTransferPackingList.Service.Gets(nUserID);
        }
        public static List<FabricTransferPackingList> Gets(string sSQL, long nUserID)
        {
            return FabricTransferPackingList.Service.Gets(sSQL, nUserID);
        }
        public FabricTransferPackingList Save(long nUserID)
        {
            return FabricTransferPackingList.Service.Save(this, nUserID);
        }
        public FabricTransferPackingList Get(int nEPIDID, long nUserID)
        {
            return FabricTransferPackingList.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, int nFTNID, long nUserID)
        {
            return FabricTransferPackingList.Service.Delete(nId, nFTNID, nUserID);
        }
        public string Untag(int nId, long nUserID)
        {
            return FabricTransferPackingList.Service.Untag(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricTransferPackingListService Service
        {
            get { return (IFabricTransferPackingListService)Services.Factory.CreateService(typeof(IFabricTransferPackingListService)); }
        }
        #endregion
    }

    #region IFabricTransferPackingList interface
    public interface IFabricTransferPackingListService
    {
        List<FabricTransferPackingList> Gets(long nUserID);
        List<FabricTransferPackingList> Gets(string sSQL, long nUserID);
        FabricTransferPackingList Save(FabricTransferPackingList oFabricTransferPackingList, long nUserID);
        FabricTransferPackingList Get(int nEPIDID, long nUserID);
        string Delete(int id, int nFTNID, long nUserID);
        string Untag(int id, long nUserID);
    }
    #endregion
}
