using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region LotParent
    public class LotParent : BusinessObject
    {
        public LotParent()
        {
            LotParentID = 0;
            LotID = 0;
            Qty = 0;
            Balance = 0;
            LotNo = "";
            ParentType = 0;
            ParentID = 0;
            UnitPrice = 0.0;
            CurrencyID = 0;
            UnitPriceBC = 0.0;
            CurrencyIDBC = 0.0;
            Note = "";
            DUPGLDetailID = 0;
            DyeingOrderID = 0;
            ProductID = 0;
            Qty = 0.0;
            Balance = 0.0;
            LotParentDate = DateTime.Today;
            IsDistribute = false;
            LotParents = new List<LotParent>();
            ErrorMessage = "";
            MUName = "";
            ProductNameLot = "";
            SearchByAssingDate = "";
            SearchByOrderDate = "";
            ContractorName = "";
            OperationUnitName = "";
            ProductCode = "";
            BalanceLot = 0;
            Params = "";
            ParentLotID = 0;
            OrderType = 0;
            EntryDate = DateTime.MinValue;
        }

        #region Property
        public int LotParentID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public double Balance { get; set; }
        public string LotNo { get; set; }public int ParentType { get; set; }
        public int ParentID { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyID { get; set; }
        public double UnitPriceBC { get; set; }
        public double CurrencyIDBC { get; set; }
        public string Note { get; set; }
        public int DyeingOrderID { get; set; }
        public int ProductID { get; set; }
        public int DUPGLDetailID { get; set; }
        public int ParentLotID { get; set; }
        public int OrderType { get; set; }
        
        public bool IsDistribute { get; set; }
        public string DyeingOrderNo { get; set; }
        public int WorkingUnitID { get; set; }
        public double Qty_Order { get; set; }
        public double BalanceLot { get; set; }
        public double Qty_Soft { get; set; }
        public double Qty_Batch_Out { get; set; }
        public DateTime LotParentDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDateInString { get; set; }
        public string OperationUnitName { get; set; }
        public int DyeingOrderID_Out { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public List<LotParent> LotParents { get; set; }
        #endregion

        #region Derived Property
        public bool IsInHouse { get; set; }
        public string StoreName { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductNameLot { get; set; }
        public string MUName { get; set; }
        public string LotParentDateSt { get { return this.LotParentDate.ToString("dd MMM yyyy hh:mm tt"); } }
        public string OrderDateSt { get { return this.OrderDate.ToString("dd MMM yyyy"); } }
        public double Qty_Consume { get { return this.Qty-this.Qty_Batch_Out; } }
        public int DUProGuidelineDetailID { get; set; }
        public int DUProGuidelineDetailID_Out { get; set; }
        public string SearchByOrderDate { get; set; }
        public string SearchByAssingDate { get; set; }
        public DateTime EntryDate { get; set; }

        #endregion

        #region Functions
        public static List<LotParent> Gets(long nUserID)
        {
            return LotParent.Service.Gets(nUserID);
        }
        public static List<LotParent> GetsBy(int nLotParentID, long nUserID)
        {
            return LotParent.Service.GetsBy(nLotParentID, nUserID);
        }
        public static List<LotParent> Gets(string sSQL, long nUserID)
        {
            return LotParent.Service.Gets(sSQL, nUserID);
        }
        public LotParent Get(int id, long nUserID)
        {
            return LotParent.Service.Get(id, nUserID);
        }
        public LotParent Save(long nUserID)
        {
            return LotParent.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return LotParent.Service.Delete(id, nUserID);
        }
        public LotParent Lot_Adjustment(int nUserID)
        {
            return LotParent.Service.Lot_Adjustment(this, nUserID);
        }
        public LotParent Lot_Transfer(int nUserID)
        {
            return LotParent.Service.Lot_Transfer(this, nUserID);
        }
        public LotParent Lot_Return(int nUserID)
        {
            return LotParent.Service.Lot_Return(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILotParentService Service
        {
            get { return (ILotParentService)Services.Factory.CreateService(typeof(ILotParentService)); }
        }
        #endregion

    }
    #endregion

    #region ILotParent interface
    public interface ILotParentService
    {
        LotParent Get(int id, Int64 nUserID);
        List<LotParent> Gets(Int64 nUserID);
        List<LotParent> Gets(string sSQL, Int64 nUserID);
        List<LotParent> GetsBy(int nLotParentID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        LotParent Save(LotParent oLotParent, Int64 nUserID);
        LotParent Lot_Adjustment(LotParent oLotParent, Int64 nUserID);
        LotParent Lot_Transfer(LotParent oLotParent, Int64 nUserID);
        LotParent Lot_Return(LotParent oLotParent, Int64 nUserID);
    }
    #endregion
}
