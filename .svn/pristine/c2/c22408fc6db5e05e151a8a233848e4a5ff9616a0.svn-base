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
    #region FabricExecutionOrderDetail

    public class FabricExecutionOrderDetail : BusinessObject
    {
        #region  Constructor
        public FabricExecutionOrderDetail()
        {
            FEODID = 0;
            FEOID = 0;
            ProductID = 0;
            Qty = 0;
            ErrorMessage = "";
            FEONotes = new List<FabricExecutionOrderNote>();
            FabricExecutionOrderDetails = new List<FabricExecutionOrderDetail>();
            FabricYarnOuts = new List<FabricYarnOut>();
            FPBID = 0;
            ShortName = "";
            FEOFs = new List<FabricExecutionOrderFabric>();
            BuyerID = 0;
            FabricID = 0;
            UnitPrice = 0;
            SuggestedQty = 0;
        }
        #endregion

        #region Properties
        public int FEODID { get; set; }
        public int FEOID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public string ShortName { get; set; }
        public string ErrorMessage { get; set; }
        public int BuyerID { get; set; }
        public int FabricID { get; set; }
        public double UnitPrice { get; set; }
        public double SuggestedQty { get; set; }
        #endregion

        #region Derive Properties
        public FabricExecutionOrder FEO { get; set; }
        public List<FabricExecutionOrderNote> FEONotes { get; set; }
        public int FPBID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public List<FabricExecutionOrderDetail> FabricExecutionOrderDetails { get; set; }
        public List<FabricYarnOut> FabricYarnOuts { get; set; }
        public List<FabricExecutionOrderFabric> FEOFs { get; set; }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }

        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.Qty);
            }
        }
        public string QtyInLbsSt
        {
            get
            {
                return Global.MillionFormat(this.QtyInLbs);
            }
        }

        public double QtyInLbs { get { return Global.GetLBS(this.Qty, 2); } }
        public double SuggestedQtyInLbs { get { return Global.GetLBS(this.SuggestedQty, 2); } }
        #endregion

        #region Functions
        public static FabricExecutionOrderDetail Get(int nFEODID, long nUserID)
        {
            return FabricExecutionOrderDetail.Service.Get(nFEODID, nUserID);
        }
        public static List<FabricExecutionOrderDetail> Gets(int nFEOID, long nUserID)
        {
            return FabricExecutionOrderDetail.Service.Gets(nFEOID, nUserID);
        }
        public static List<FabricExecutionOrderDetail> Gets(string sSQL, long nUserID)
        {
            return FabricExecutionOrderDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricExecutionOrderDetail IUD(int nDBOperation, long nUserID)
        {
            return FabricExecutionOrderDetail.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricExecutionOrderDetailService Service
        {
            get { return (IFabricExecutionOrderDetailService)Services.Factory.CreateService(typeof(IFabricExecutionOrderDetailService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricExecutionOrderDetail interface
    public interface IFabricExecutionOrderDetailService
    {
        FabricExecutionOrderDetail Get(int nFEODID, long nUserID);
        List<FabricExecutionOrderDetail> Gets(int nFEOID, long nUserID);
        List<FabricExecutionOrderDetail> Gets(string sSQL, long nUserID);
        FabricExecutionOrderDetail IUD(FabricExecutionOrderDetail oFabricExecutionOrderDetail, int nDBOperation, long nUserID);
    }
    #endregion
}