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
    #region VoucherBatch
    public class VoucherBatch : BusinessObject
    {
        public VoucherBatch()
        {
            VoucherBatchID = 0;
            BatchNO = "";
            CreateBy = 0;
            CreateDate = DateTime.Now;
            BatchStatus = EnumVoucherBatchStatus.BatchOpen;
            RequestTo = 0;
            RequestDate = DateTime.Now;
            CreateByName = "";
            RequestToName = "";
            VoucherCount = 0;

            ErrorMessage = "";
        }
        #region Properties
        public int VoucherBatchID { get; set; }
        public string BatchNO { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public EnumVoucherBatchStatus BatchStatus { get; set; }
        public int RequestTo { get; set; }
        public DateTime RequestDate { get; set; }
        public string CreateByName { get; set; }
        public string RequestToName { get; set; }
        public int VoucherCount { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        
        #region Derived Property
        public string CreateDateInString { get { return this.CreateDate.ToString("dd MMM yyyy"); } }
        public string RequestDateInString { get { return this.RequestTo > 0 ? this.RequestDate.ToString("dd MMM yyyy") : ""; } }
        public int BatchStatusInInt { get { return (int)this.BatchStatus; } }
        public string BatchStatusInString { get { return EnumObject.jGet(this.BatchStatus); } } 
        public List<Voucher> Vouchers { get; set; }
        
        #endregion

        #region Functions
        
        public VoucherBatch Get(int id, int nUserID)
        {
            return VoucherBatch.Service.Get(id, nUserID);
        }
        public VoucherBatch Save(int nUserID)
        {
            return VoucherBatch.Service.Save(this, nUserID);
        }
        public VoucherBatch UpdateStatus(int nUserID)
        {
            return VoucherBatch.Service.UpdateStatus(this, nUserID);
        }
        public string VoucherBatchTransfer(int nUserID)
        {
            return VoucherBatch.Service.VoucherBatchTransfer(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return VoucherBatch.Service.Delete(id, nUserID);
        }
        public static List<VoucherBatch> Gets(int nUserID)
        {
            return VoucherBatch.Service.Gets(nUserID);
        }
        public static List<VoucherBatch> GetsByCreateBy(int nUserID)
        {
            return VoucherBatch.Service.GetsByCreateBy(nUserID);
        }
        public static List<VoucherBatch> GetsTransferTo(int nVoucherBatchID, int nUserID)
        {
            return VoucherBatch.Service.GetsTransferTo(nVoucherBatchID,nUserID);
        }
        public static List<VoucherBatch> Gets(string sSQL, int nUserID)
        {
            return VoucherBatch.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IVoucherBatchService Service
        {
            get { return (IVoucherBatchService)Services.Factory.CreateService(typeof(IVoucherBatchService)); }
        }
        #endregion
    }
    #endregion

    

    #region IVoucherBatch interface
    public interface IVoucherBatchService
    {
        VoucherBatch Get(int id, int nUserID);
        List<VoucherBatch> Gets(int nUserID);
        List<VoucherBatch> GetsByCreateBy(int nUserID);
        List<VoucherBatch> GetsTransferTo(int nVoucherBatchID, int nUserID);
        string Delete(int id, int nUserID);
        VoucherBatch Save(VoucherBatch oVoucherBatch, int nUserID);
        VoucherBatch UpdateStatus(VoucherBatch oVoucherBatch, int nUserID);
        string VoucherBatchTransfer(VoucherBatch oVoucherBatch, int nUserId);
        List<VoucherBatch> Gets(string sSQL, int nUserID);
        
        
    }
    #endregion
}