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
    public class FabricTransferPackingListDetail
    {
        public FabricTransferPackingListDetail()
        {
            FTPLDetailID = 0;
            FTPListID = 0;
            LotID = 0;
            Qty = 0;
            LotNo = "";

            FTPL = new FabricTransferPackingList();
            FTPLDetails = new List<FabricTransferPackingListDetail>();
            ErrorMessage = "";
            IsSaveSingleLot = false;
            StoreID = 0;
            FEOID = 0;
            FNExOID = 0;
            LotIDs = "";
            FabricID = 0;
            ReceiveBy = 0;
            ReceiveDate = new DateTime(1900, 01, 01);
            ReceiveByName = "";
            WUID = 0;
            Grade = EnumFBQCGrade.None;
            StoreRcvDate = DateTime.MinValue;
            WarpLot = "";
            WeftLot = "";
        }

        #region Properties
        public string WarpLot { get; set; }
        public string WeftLot { get; set; }
        public int FTPLDetailID { get; set; }
        public int FTPListID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public string LotNo { get; set; } //Roll No
        public string ErrorMessage { get; set; }
        public bool IsSaveSingleLot { get; set; }
        public int StoreID { get; set; }
        public int FEOID { get; set; }
        public int FNExOID { get; set; }
        public string LotIDs { get; set; }
        public int FabricID { get; set; }
        public int ReceiveBy { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ReceiveByName { get; set; }
        public int WUID { get; set; }
         public EnumFBQCGrade Grade { get; set; }


        #endregion

        #region Derive Properties
         public double QtyInM { get { return Global.GetMeter(this.Qty, 2); } }
         public DateTime StoreRcvDate { get; set; }
         public string StoreRcvDateStr { get { return (this.StoreRcvDate==DateTime.MinValue)?"-":this.StoreRcvDate.ToString("dd MMM yyyy");} }
         public string GradeInString
         {
             get
             {
                 return this.Grade.ToString();
             }
         }
        public FabricTransferPackingList FTPL { get; set; }
        public List<FabricTransferPackingListDetail> FTPLDetails { get; set; }
        public string ReceiveDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.ReceiveDate == MinValue || this.ReceiveDate == MinValue1 || this.ReceiveBy == 0)
                {
                    return "-";
                }
                else {
                    return this.ReceiveDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string QtySt
        {
            get
            {
                if (this.Qty < 0) return "(" + Global.MillionFormat(this.Qty * (-1)) + ")";
                else if (this.Qty == 0) return "-";
                else return Global.MillionFormat(this.Qty);
            }
        }
        public string QtyInMeterSt
        {
            get
            {
                double nQtyInMeter = Global.GetMeter(this.Qty, 2);
                if (nQtyInMeter < 0) return "(" + Global.MillionFormat(nQtyInMeter * (-1)) + ")";
                else if (nQtyInMeter == 0) return "-";
                else return Global.MillionFormat(nQtyInMeter);
            }
        }
        #endregion

        #region Functions
        public static List<FabricTransferPackingListDetail> Gets(long nUserID)
        {
            return FabricTransferPackingListDetail.Service.Gets(nUserID);
        }
        public static List<FabricTransferPackingListDetail> Gets(int nFTPListID, long nUserID)
        {
            return FabricTransferPackingListDetail.Service.Gets(nFTPListID, nUserID);
        }
        public static List<FabricTransferPackingListDetail> Gets(string sSQL, long nUserID)
        {
            return FabricTransferPackingListDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricTransferPackingListDetail Save(long nUserID)
        {
            return FabricTransferPackingListDetail.Service.Save(this, nUserID);
        }
        public FabricTransferPackingListDetail Update(long nUserID)
        {
            return FabricTransferPackingListDetail.Service.Update(this, nUserID);
        }
        public FabricTransferPackingListDetail Get(int nEPIDID, long nUserID)
        {
            return FabricTransferPackingListDetail.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, int nFTPListID, long nUserID)
        {
            return FabricTransferPackingListDetail.Service.Delete(nId, nFTPListID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricTransferPackingListDetailService Service
        {
            get { return (IFabricTransferPackingListDetailService)Services.Factory.CreateService(typeof(IFabricTransferPackingListDetailService)); }
        }
        #endregion
    }

    #region IFabricTransferPackingListDetail interface
    public interface IFabricTransferPackingListDetailService
    {
        List<FabricTransferPackingListDetail> Gets(long nUserID);
        List<FabricTransferPackingListDetail> Gets(int nFTPListID, long nUserID);
        List<FabricTransferPackingListDetail> Gets(string sSQL, long nUserID);
        FabricTransferPackingListDetail Save(FabricTransferPackingListDetail oFabricTransferPackingListDetail, long nUserID);
        FabricTransferPackingListDetail Update(FabricTransferPackingListDetail oFabricTransferPackingListDetail, long nUserID);
        FabricTransferPackingListDetail Get(int nEPIDID, long nUserID);
        string Delete(int id, int nFTPListID, long nUserID);
    }
    #endregion
}
