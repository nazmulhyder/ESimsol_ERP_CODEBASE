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
    #region DUProGuideLineDetail

    public class DUProGuideLineDetail : BusinessObject
    {
        public DUProGuideLineDetail()
        {
            DUProGuideLineDetailID = 0;
            DUProGuideLineID = 0;
            ProductID = 0;
            ProductName = "";
            LotProductName = "";
            MUnit = "";
            Qty = 0;
            Qty_Preq = 0;
            Qty_Order = 0;
            UnitPrice = 0;
            LotID = 0;
            LotNo = "";
            LotQty = 0;
            ReceiveDate = DateTime.MinValue;
            LotParentID = 0;
            Qty_LotParent = 0;
            ChallanNo = "";
            CurrencyID = 0;
            CurrencySymbol = "";
            MUnitID = 0;
            BagNo=0;
            DyeingOrderID = 0;
            Amount = 0;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int DUProGuideLineDetailID { get; set; }
        public int DUProGuideLineID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string LotProductName { get; set; }
        public string MUnit { get; set; }
        public double Qty { get; set; }
        public double LotQty { get; set; }
        public double Qty_Preq { get; set; }
        public double Qty_Order { get; set; }
        public double Qty_LotParent { get; set; }
        public double UnitPrice { get; set; }
        public int LotID { get; set; }
        public int LotParentID { get; set; }
        public int CurrencyID { get; set; }
        public int MUnitID { get; set; }
        public double BagNo { get; set; }
        public string Note { get; set; }
        public string CurrencySymbol { get; set; }
        public string LotNo { get; set; }
        public string Brand { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int DyeingOrderID { get; set; }
        public string ChallanNo { get; set; }
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
        public string ReceiveDateSt
        {
            get
            {
                if (ReceiveDate == DateTime.MinValue)
                    return "-";
                else
                    return this.ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public double Balance { get; set; }
        public string SLNo { get; set; }
        public double Amount { get; set; }
        //public double Amount { get { return this.Qty * this.UnitPrice; } }
        public string AmountST { get { return Global.MillionFormat(this.Qty * this.UnitPrice); } }
        public string QtyST { get { return Global.MillionFormat(this.Qty) + " " + this.MUnit; } }

        #endregion

        #region Functions
        public static List<DUProGuideLineDetail> Gets(long nUserID)
        {
            return DUProGuideLineDetail.Service.Gets(nUserID);
        }
        public static List<DUProGuideLineDetail> Gets(int nImportInvoiceID, long nUserID)
        {
            return DUProGuideLineDetail.Service.Gets(nImportInvoiceID, nUserID);
        }
        public static List<DUProGuideLineDetail> Gets(string sSQL, long nUserID)
        {
            return DUProGuideLineDetail.Service.Gets(sSQL, nUserID);
        }
        public DUProGuideLineDetail Get(int id, long nUserID)
        {
            return DUProGuideLineDetail.Service.Get(id, nUserID);
        }

        public DUProGuideLineDetail Save(long nUserID)
        {
            return DUProGuideLineDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUProGuideLineDetail.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUProGuideLineDetailService Service
        {
            get { return (IDUProGuideLineDetailService)Services.Factory.CreateService(typeof(IDUProGuideLineDetailService)); }
        }
        #endregion

    }
    #endregion


    #region IDUProGuideLineDetail interface

    public interface IDUProGuideLineDetailService
    {
        DUProGuideLineDetail Get(int id, Int64 nUserID);
        List<DUProGuideLineDetail> Gets(string sSQL, long nUserID);
        List<DUProGuideLineDetail> Gets(int nImportInvoiceID, long nUserID);
        List<DUProGuideLineDetail> Gets(Int64 nUserID);
        string Delete(DUProGuideLineDetail oDUProGuideLineDetail, Int64 nUserID);
        DUProGuideLineDetail Save(DUProGuideLineDetail oDUProGuideLineDetail, Int64 nUserID);
    }
    #endregion
}
