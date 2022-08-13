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
    #region FabricAvailableStock
    public class FabricAvailableStock : BusinessObject
    {
        public FabricAvailableStock()
        {
            FSCDID = 0;
            FSCID = 0;
            PONo = "";
            PODate = DateTime.Now;
            DispoNo = "";
            DispoQty = 0;
            ProductID = 0;
            ProductName = "";
            BuyerID = 0;
            BuyerName = "";
            IsInHouse = true;
            ColorName = "";
            MUnitID = 0;
            LotID = 0;
            LotNo = "";
            LotBalance = 0;
            WUName = "";
            WUID = 0;
            FNBatchQCDetailID = 0;
            FNBatchQCID = 0;
            QCQty = 0;
            Construction = "";
            Composition = "";
            FabricID = 0;
            FabricNo = "";
            FinishType = 0;
            FinishTypeName = "";
            ErrorMessage = "";
            RollQty = 0;
        }

        #region Property
        public int FSCDID { get; set; }
        public int FSCID { get; set; }
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public string DispoNo { get; set; }
        public double DispoQty { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public bool IsInHouse { get; set; }
        public int RollQty { get; set; }
        public string ColorName { get; set; }
        public int MUnitID { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public double LotBalance { get; set; }
        public string WUName { get; set; }
        public int WUID { get; set; }
        public int FNBatchQCDetailID { get; set; }
        public int FNBatchQCID { get; set; }
        public double QCQty { get; set; }
        public string Construction { get; set; }
        public string Composition { get; set; }
        public int FabricID { get; set; }
        public string FabricNo { get; set; }
        public int FinishType { get; set; }
        public string FinishTypeName { get; set; }
        public int WeaveType { get; set; }
        public string WeaveTypeName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string PODateInString
        {
            get
            {
                return PODate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FabricAvailableStock> Gets(long nUserID)
        {
            return FabricAvailableStock.Service.Gets(nUserID);
        }
        public static List<FabricAvailableStock> Gets(string sSQL, long nUserID)
        {
            return FabricAvailableStock.Service.Gets(sSQL, nUserID);
        }
        public FabricAvailableStock Get(int id, long nUserID)
        {
            return FabricAvailableStock.Service.Get(id, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IFabricAvailableStockService Service
        {
            get { return (IFabricAvailableStockService)Services.Factory.CreateService(typeof(IFabricAvailableStockService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricAvailableStock interface
    public interface IFabricAvailableStockService
    {
        FabricAvailableStock Get(int id, Int64 nUserID);
        List<FabricAvailableStock> Gets(Int64 nUserID);
        List<FabricAvailableStock> Gets(string sSQL, Int64 nUserID);
        
    }
    #endregion
}
