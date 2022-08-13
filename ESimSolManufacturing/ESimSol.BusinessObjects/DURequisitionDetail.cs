using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DURequisitionDetail

    public class DURequisitionDetail : BusinessObject
    {
        public DURequisitionDetail()
        {
            DURequisitionDetailID = 0;
            DURequisitionID = 0;
            ProductID = 0;
            ProductName = "";
            LotProductName = "";
            BuyerName = "";
            MUnit = "";
            Qty = 0;
            Qty_Preq = 0;
            UnitPrice = 0;
            DyeingOrderID = 0;
            LotID = 0;
            LotNo = "";
            LotQty = 0;
            DestinationLotID = 0;
            DestinationLotNo = "";
            CurrencyID = 0;
            CurrencySymbol = "";
            MUnitID = 0;
            BagNo = 0;
            Qty_Order = 0;
            Note = "";
            IsInHouse = false;
            RequisitionType = EnumInOutType.None;
            ContractorID = 0;
            WorkingUnitID = 0;
            ErrorMessage = "";
            WUName = "";
            OrderType = 0;

            RequisitionNo=""; 
            ReqDate = DateTime.Now;
            ReceiveDate = DateTime.Now;
        }

        #region Properties
        public int DURequisitionDetailID { get; set; }
        public int DURequisitionID { get; set; }
        public bool IsInHouse { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string LotProductName { get; set; }
        public string MUnit { get; set; }
        public double Qty { get; set; }
        public double LotQty { get; set; }
        public double Qty_Preq { get; set; }
        public double Qty_Order { get; set; }
        public double UnitPrice { get; set; }
        public int DyeingOrderID { get; set; }
        public string DyeingOrderNo { get; set; }
        public int LotID { get; set; }
        public int DestinationLotID { get; set; }
        public int ContractorID { get; set; }
        public int CurrencyID { get; set; }
        public int MUnitID { get; set; }
        public double  BagNo { get; set; }
        public string Note { get; set; }
        public string CurrencySymbol { get; set; }
        public string LotNo { get; set; }
        public string BuyerName { get; set; }
        public string WUName { get; set; }
        public string DestinationLotNo { get; set; }
        public int WorkingUnitID { get; set; }
        public int OrderType { get; set; }
        public EnumInOutType RequisitionType { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string UnitPriceST
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        public string ReqDateST
        {
            get
            {
                if (this.ReqDate == DateTime.MinValue)
                    return "-";
                else
                    return this.ReqDate.ToString("dd MMM yyyy");
            }
        }
        public string ReceiveDateST
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue)
                    return "-";
                else
                    return this.ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public double Amount { get { return this.Qty * this.UnitPrice; } }
        public string AmountST { get { return Global.MillionFormat(this.Qty * this.UnitPrice); } }
        public string QtyST { get { return Global.MillionFormat(this.Qty) + " " + this.MUnit; } }

        #endregion

        #region Functions
        public static List<DURequisitionDetail> Gets(long nUserID)
        {
            return DURequisitionDetail.Service.Gets(nUserID);
        }
        public static List<DURequisitionDetail> Gets(int nDURequisitionID, long nUserID)
        {
            return DURequisitionDetail.Service.Gets(nDURequisitionID, nUserID);
        }
        public static List<DURequisitionDetail> Gets(string sSQL, long nUserID)
        {
            return DURequisitionDetail.Service.Gets(sSQL, nUserID);
        }
        public DURequisitionDetail Get(int id, long nUserID)
        {
            return DURequisitionDetail.Service.Get(id, nUserID);
        }

        public DURequisitionDetail Save(long nUserID)
        {
            return DURequisitionDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DURequisitionDetail.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDURequisitionDetailService Service
        {
            get { return (IDURequisitionDetailService)Services.Factory.CreateService(typeof(IDURequisitionDetailService)); }
        }
        #endregion



    }
    #endregion


    #region IDURequisitionDetail interface

    public interface IDURequisitionDetailService
    {
        DURequisitionDetail Get(int id, Int64 nUserID);
        List<DURequisitionDetail> Gets(string sSQL, long nUserID);
        List<DURequisitionDetail> Gets(int nDURequisitionID, long nUserID);
        List<DURequisitionDetail> Gets(Int64 nUserID);
        string Delete(DURequisitionDetail oDURequisitionDetail, Int64 nUserID);
        DURequisitionDetail Save(DURequisitionDetail oDURequisitionDetail, Int64 nUserID);
    }
    #endregion
}
