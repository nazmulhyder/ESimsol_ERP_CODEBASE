using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region VoucherBatchHistory
    public class VoucherBatchHistory : BusinessObject
    {
        public VoucherBatchHistory()
        {
            VoucherBatchHistoryID = 0;
            VoucherBatchID = 0;
            BatchNO = "";
            CreateBy = 0;
            CreateDate = DateTime.Now;
            PreviousBatchStatus = EnumVoucherBatchStatus.BatchOpen;
            CurrentBatchStatus = EnumVoucherBatchStatus.BatchOpen;
            RequestTo = 0;
            RequestDate = DateTime.Now;
            CreateByName = "";
            RequestToName = "";
            VoucherCount = 0;
            DBServerDateTime = DateTime.Now;

            ErrorMessage = "";
        }
        #region Properties
        public int VoucherBatchHistoryID { get; set; }
        public int VoucherBatchID { get; set; }
        public string BatchNO { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public EnumVoucherBatchStatus PreviousBatchStatus { get; set; }
        public EnumVoucherBatchStatus CurrentBatchStatus { get; set; }
        public int RequestTo { get; set; }
        public DateTime RequestDate { get; set; }
        public string CreateByName { get; set; }
        public string RequestToName { get; set; }
        public int VoucherCount { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        
        #region Derived Property
        public string CreateDateInString { get { return this.CreateDate.ToString("dd MMM yyyy"); } }
        public string RequestDateInString { get { return this.RequestTo > 0 ? this.RequestDate.ToString("dd MMM yyyy") : ""; } }
        public int PreviousBatchStatusInInt { get { return (int)this.PreviousBatchStatus; } }
        public int CurrentBatchStatusInInt { get { return (int)this.CurrentBatchStatus; } }
        public string PreviousBatchStatusInString { get { return EnumObject.jGet(this.PreviousBatchStatus); } }
        public string CurrentBatchStatusInString { get { return EnumObject.jGet(this.CurrentBatchStatus); } }
        public string OperationDateInString { get { return this.DBServerDateTime.ToString("dd MMM yyyy hh:mm tt"); } }
        public List<Voucher> Vouchers { get; set; }
        
        #endregion

        #region Functions
        
        public VoucherBatchHistory Get(int id, int nUserID)
        {
            return VoucherBatchHistory.Service.Get(id, nUserID);
        }
        
        public static List<VoucherBatchHistory> Gets(int nUserID)
        {
            return VoucherBatchHistory.Service.Gets(nUserID);
        }
        public static List<VoucherBatchHistory> GetsByBatchID(int nVoucherBatchID, int nUserID)
        {
            return VoucherBatchHistory.Service.GetsByBatchID(nVoucherBatchID,nUserID);
        }
      
        public static List<VoucherBatchHistory> Gets(string sSQL, int nUserID)
        {
            return VoucherBatchHistory.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IVoucherBatchHistoryService Service
        {
            get { return (IVoucherBatchHistoryService)Services.Factory.CreateService(typeof(IVoucherBatchHistoryService)); }
        }
        #endregion
    }
    #endregion

    

    #region IVoucherBatchHistory interface
    public interface IVoucherBatchHistoryService
    {
        VoucherBatchHistory Get(int id, int nUserID);
        List<VoucherBatchHistory> Gets(int nUserID);
        List<VoucherBatchHistory> GetsByBatchID(int nVoucherBatchID, int nUserID);
     
        List<VoucherBatchHistory> Gets(string sSQL, int nUserID);
        
        
    }
    #endregion
}