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
    #region WeavingYarnStock
    public class WeavingYarnStock : BusinessObject
    {
        public WeavingYarnStock()
        {
            FSCID = 0;
            FSCDetailID = 0;
            FEOSID = 0;
            Qty = 0;
            ExeNo = "";
            ExeDate = DateTime.Now;
            SCNoFull = "";
            Qty_Dispo = 0;
            Qty_Order = 0;
            BuyerID = 0;
            BuyerName = "";
            ContractorID = 0;
            ContractorName = "";
            WarpCount = "";
            WeftCount = "";
            RequiredWarpLength = 0;
            RequiredWarpLengthLB = 0;
            TotalWarpProduction = 0;
            ProductID = 0;
            ProductName = "";
            Construction = "";
            StyleNo = "";
            FabricID = 0;
            FabricNo = "";
            GreyYarnReqWarp = 0;
            DyedYarnReqWarp = 0;
            GreyYarnReqWeft = 0;
            DyedYarnReqWeft = 0;
            ReqDyedYarn = 0;
            ReqGreyFabrics = 0;
            TotalGreyProduction = 0;
            ReqFinishedFabrics = 0;
            ColorWarp = 0;
            ColorWeft = 0;
            SWQty = 0;
            WYReqWarp = 0;
            WYReqWeft = 0;
            BeamID = 0;
            BeamNo = "";
            LoomNo = "";
            Shade = "";
            Remarks = "";
            ErrorMessage = "";
        }

        #region Property
        public int FSCID { get; set; }
        public int FSCDetailID { get; set; }
        public int FEOSID { get; set; }
        public double Qty { get; set; }
        public string ExeNo { get; set; }
        public DateTime ExeDate { get; set; }
        public string SCNoFull { get; set; }
        public double Qty_Dispo { get; set; }
        public double Qty_Order { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string WarpCount { get; set; }
        public string WeftCount { get; set; }
        public double RequiredWarpLength { get; set; }
        public double RequiredWarpLengthLB { get; set; }
        public double TotalWarpProduction { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Construction { get; set; }
        public string StyleNo { get; set; }
        public int FabricID { get; set; }
        public string FabricNo { get; set; }
        public double GreyYarnReqWarp { get; set; }
        public double DyedYarnReqWarp { get; set; }
        public double GreyYarnReqWeft { get; set; }
        public double DyedYarnReqWeft { get; set; }
        public double ReqDyedYarn { get; set; }
        public double ReqGreyFabrics { get; set; }
        public double TotalGreyProduction { get; set; }
        public double ReqFinishedFabrics { get; set; }
        public int ColorWarp { get; set; }
        public int ColorWeft { get; set; }
        public double SWQty { get; set; }
        public double WYReqWarp { get; set; }
        public double WYReqWeft { get; set; }
        public int BeamID { get; set; }
        public string BeamNo { get; set; }
        public string LoomNo { get; set; }
        public string Shade { get; set; }
        public string ShadeWarp { get; set; }
        public string Remarks { get; set; }

        public double StoreRcvQty { get; set; }
        public double WeavingRcvQty { get; set; }
        public double DyeProductionQty { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string ExeDateInString
        {
            get
            {
                return ExeDate.ToString("dd MMM yyyy");
            }
        }
        public double StoreRcvBalance
        {
            get
            {
                return (StoreRcvQty - DyedYarnReqWeft);
            }
        }
        public double WeavingRcvBalance
        {
            get
            {
                return (StoreRcvQty - WeavingRcvQty);
            }
        }
        public double DyeBalance
        {
            get
            {
                return (SWQty - DyeProductionQty);
            }
        }
        public double GreyProdBalance
        {
            get
            {
                return (TotalGreyProduction - ReqGreyFabrics);
            }
        }
        #endregion

        #region Functions
        public static List<WeavingYarnStock> Gets(long nUserID)
        {
            return WeavingYarnStock.Service.Gets(nUserID);
        }
        public static List<WeavingYarnStock> Gets(string sSQL, int nType, long nUserID)
        {
            return WeavingYarnStock.Service.Gets(sSQL, nType, nUserID);
        }
        public WeavingYarnStock Get(int id, long nUserID)
        {
            return WeavingYarnStock.Service.Get(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IWeavingYarnStockService Service
        {
            get { return (IWeavingYarnStockService)Services.Factory.CreateService(typeof(IWeavingYarnStockService)); }
        }
        #endregion
    }
    #endregion

    #region IWeavingYarnStock interface
    public interface IWeavingYarnStockService
    {
        WeavingYarnStock Get(int id, Int64 nUserID);
        List<WeavingYarnStock> Gets(Int64 nUserID);
        List<WeavingYarnStock> Gets(string sSQL, int nType, Int64 nUserID);

    }
    #endregion
}
