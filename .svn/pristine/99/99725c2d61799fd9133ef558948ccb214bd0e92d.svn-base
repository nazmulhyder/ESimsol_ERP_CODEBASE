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
    #region FabricBatchHistory
    public class FabricBatchHistory : BusinessObject
    {
        public FabricBatchHistory()
        {
            FPBHID = 0;
            FPBID = 0;
            PreviousStatus = EnumFabricBatchState.Initialize;
            PreviousStatusInInt = (int)EnumFabricBatchState.Initialize;
            CurrentStatus = EnumFabricBatchState.Initialize;
            CurrentStatusInInt = (int)EnumFabricBatchState.Initialize;
            StartDateTime = DateTime.Now;
            EndDateTime = DateTime.Now;
            InQty = 0;
            OutQty = 0;
            BatchManID = 0;
            BatchNo = "";
            MachineNo = "";
            FEONo = "";
            Construction = "";

            ErrorMessage = "";
            StoreID = 0;
            PINo = "";
            FinishType = "";// EnumFinishType.None;
            
            BuyerName = "";
            LotNo = "";
            Width = "";
        }


        #region Properties
        public int FPBHID { get; set; }
        public int FPBID { get; set; }
        public EnumFabricBatchState PreviousStatus { get; set; }
        public int PreviousStatusInInt { get; set; }
        public EnumFabricBatchState CurrentStatus { get; set; }
        public int CurrentStatusInInt { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double InQty { get; set; }
        public double OutQty { get; set; }
        public int BatchManID { get; set; }
        public string BatchNo { get; set; }
        public string MachineNo { get; set; }
        public string FEONo { get; set; }
        public string Construction { get; set; }
        public string ErrorMessage { get; set; }
        public int StoreID { get; set; }
        public string BuyerName { get; set; }
        public string LotNo { get; set; }
        public string Width { get; set; }

        #endregion

        #region Derived Property
        public Company Company { get; set; }
        public string PINo { get; set; }
        public string FinishType { get; set; }
        public int FinishTypeInInt { get; set; }
        //public string FinishTypeSt
        //{
        //    get
        //    {
        //        return EnumFinishTypeObj.GetEnumFinishTypeObjs(this.FinishType);
        //    }
        //}

        public string StartDateTimeSt
        {
            get { return this.StartDateTime.ToString("dd MMM yyyy"); }
        }
        public string EndDateTimeSt
        {
            get { return this.EndDateTime.ToString("dd MMM yyyy"); }
        }
        public string PreviousStatusSt
        {
            get
            {
                //return FabricBatchStateObj.GetEnumFabricBatchStateObjs(this.PreviousStatus);
                return "please cheack BO";
            }
        }
        public string CurrentStatusSt
        {
            get
            {
                //return FabricBatchStateObj.GetEnumFabricBatchStateObjs(this.CurrentStatus);
                return "please cheack BO";
            }
        }
       
        #endregion

        #region Functions
        public static List<FabricBatchHistory> Gets(long nUserID)
        {
            return FabricBatchHistory.Service.Gets(nUserID);
        }
        public static List<FabricBatchHistory> Gets(string sSQL, long nUserID)
        {
            return FabricBatchHistory.Service.Gets(sSQL, nUserID);
        }
        public FabricBatchHistory Get(int nId, long nUserID)
        {
            return FabricBatchHistory.Service.Get(nId, nUserID);
        }
        public FabricBatchHistory Save(long nUserID)
        {
            return FabricBatchHistory.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchHistory.Service.Delete(nId, nUserID);
        }

        public FabricBatchHistory ReceiveInDeliveryStore(long nUserID)
        {
            return FabricBatchHistory.Service.ReceiveInDeliveryStore(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricBatchHistoryService Service
        {
            get { return (IFabricBatchHistoryService)Services.Factory.CreateService(typeof(IFabricBatchHistoryService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricBatchHistory interface
    public interface IFabricBatchHistoryService
    {
        List<FabricBatchHistory> Gets(long nUserID);
        List<FabricBatchHistory> Gets(string sSQL, long nUserID);
        FabricBatchHistory Get(int id, long nUserID);
        FabricBatchHistory Save(FabricBatchHistory oFabricBatchHistory, long nUserID);
        string Delete(int id, long nUserID);
        FabricBatchHistory ReceiveInDeliveryStore(FabricBatchHistory oFabricBatchHistory, long nUserID);
    }
    #endregion
}
